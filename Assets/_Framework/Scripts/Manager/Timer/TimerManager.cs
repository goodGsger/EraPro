using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class TimerManager : Manager, ITimerManager
    {
        private ObjectPool<TimerHandler> _timerHandlerPool;
        private ObjectPool<DelayHanlder> _delayHandlerPool;

        private Dictionary<TimerCallback, TimerHandler> _timerHandlers;
        private Queue<DelayHanlder> _delayHanlders;

        private float _realTime;
        private double _time;
        private int _currentFrame;
        private double _currentTime;
        private double _deltaTime;
        private double _timeOut;
        private bool _inUpdate;

        /// <summary>
        /// 当前时间（双精秒）
        /// </summary>
        public double time
        {
            get { return _time; }
            set
            {
                _time = value;
                _realTime = Time.realtimeSinceStartup;
            }
        }

        /// <summary>
        /// 当前时间（浮点秒）
        /// </summary>
        public float timeFloat
        {
            get { return (float)_time; }
        }

        /// <summary>
        /// 当前时间（秒）
        /// </summary>
        public int timeSecond
        {
            get { return (int)_time; }
        }

        /// <summary>
        /// 当前时间（毫秒）
        /// </summary>
        public long timeMilliseconds
        {
            get { return (long)(_time * 1000); }
        }

        /// <summary>
        /// 计时器运行时间（双精秒）
        /// </summary>
        public double currentTime
        {
            get { return _currentTime; }
        }

        /// <summary>
        /// 帧间隔时间（双精秒）
        /// </summary>
        public double deltaTime
        {
            get { return _deltaTime; }
        }

        /// <summary>
        /// 超时时间（双精秒）
        /// </summary>
        public double timeOut
        {
            get { return _timeOut; }
            set { _timeOut = value; }
        }

        override protected void Init()
        {
            _timerHandlerPool = new ObjectPool<TimerHandler>();
            _delayHandlerPool = new ObjectPool<DelayHanlder>();

            _timerHandlers = new Dictionary<TimerCallback, TimerHandler>();
            _delayHanlders = new Queue<DelayHanlder>();

            _realTime = 0;
            _time = 0;
            _currentFrame = 0;
            _currentTime = 0;
            _deltaTime = 0;
            _timeOut = 0;
        }

        public override void Update(float deltaTime)
        {
            _inUpdate = true;

            _deltaTime = deltaTime;
            _currentFrame++;
            _currentTime += deltaTime;

            if (_time != 0)
            {
                float tempTime = Time.realtimeSinceStartup;
                _time += tempTime - _realTime;
                _realTime = tempTime;
            }

            // 判断超时
            if (_timeOut > 0)
            {
                if (deltaTime > _timeOut)
                {
                    foreach (var v in _timerHandlers)
                    {
                        TimerHandler timerHandler = v.Value;
                        double time = timerHandler.isFrame ? _currentFrame : _currentTime;
                        timerHandler.currentTime = time;
                    }
                    DispatchEvent(TimerManagerEventArgs.TIME_OUT);
                }
            }

            TimerCallback callback;
            foreach (var v in _timerHandlers)
            {
                TimerHandler timerHandler = v.Value;
                if (timerHandler.removeFlag)
                    continue;

                double time = timerHandler.isFrame ? _currentFrame : _currentTime;
                if (time >= timerHandler.currentTime)
                {
                    callback = timerHandler.callback;
                    if (callback != null)
                    {
                        while (time >= timerHandler.currentTime && timerHandler.count != 0)
                        {
                            timerHandler.currentTime += timerHandler.delayTime;
                            if (timerHandler.count > 0)
                            {
                                timerHandler.count--;
                                if (timerHandler.count == 0)
                                    Unregister(callback);
                            }

                            callback.Invoke();
                        }
                    }
                }
            }

            _inUpdate = false;

            // 更新注册/注销计时器回调
            while (_delayHanlders.Count > 0)
            {
                DelayHanlder delayHanlder = _delayHanlders.Dequeue();
                callback = delayHanlder.callback;
                bool isRemove = delayHanlder.isRemove;
                bool isFrame = delayHanlder.isFrame;
                float delayTime = delayHanlder.delayTime;
                int count = delayHanlder.count;

                _delayHandlerPool.Release(delayHanlder);

                if (isRemove)
                    Unregister(callback);
                else
                    Register(isFrame, delayTime, callback, count);
            }
        }

        /// <summary>
        /// 注册计时器回调函数
        /// </summary>
        /// <param name="isFrame"></param>
        /// <param name="delayTime"></param>
        /// <param name="callback"></param>
        /// <param name="count"></param>
        public void Register(bool isFrame, float delayTime, TimerCallback callback, int count)
        {
            if (delayTime <= 0)
            {
                App.logManager.Error("TimerManager.Register Error: DelayTime must be greater than 0!");
                return;
            }

            if (callback == null || count == 0)
                return;

            // 更新中注册计时器回调
            if (_inUpdate)
            {
                DelayHanlder delayHanlder = _delayHandlerPool.Get();
                delayHanlder.isFrame = isFrame;
                delayHanlder.callback = callback;
                delayHanlder.delayTime = delayTime;
                delayHanlder.count = count;
                _delayHanlders.Enqueue(delayHanlder);
                return;
            }

            // 判断是否已经注册计时器回调
            TimerHandler timerHandler;
            _timerHandlers.TryGetValue(callback, out timerHandler);
            if (timerHandler != null)
            {
                timerHandler.isFrame = isFrame;
                timerHandler.delayTime = delayTime;
                timerHandler.count = count;
                timerHandler.currentTime = (isFrame ? _currentFrame : _currentTime) + delayTime;
                return;
            }

            // 注册新计时器回调
            timerHandler = _timerHandlerPool.Get();
            timerHandler.isFrame = isFrame;
            timerHandler.delayTime = delayTime;
            timerHandler.callback = callback;
            timerHandler.count = count;
            timerHandler.currentTime = (isFrame ? _currentFrame : _currentTime) + delayTime;
            _timerHandlers[callback] = timerHandler;
        }

        /// <summary>
        /// 注销计时器回调
        /// </summary>
        /// <param name="callback"></param>
        public void Unregister(TimerCallback callback)
        {
            if (callback == null)
                return;

            // 更新中注销计时器回调
            if (_inUpdate)
            {
                foreach (var v in _timerHandlers)
                {
                    if (v.Value.callback == callback)
                        v.Value.removeFlag = true;
                }

                DelayHanlder delayHanlder = _delayHandlerPool.Get();
                delayHanlder.callback = callback;
                delayHanlder.isRemove = true;
                _delayHanlders.Enqueue(delayHanlder);
                return;
            }

            // 注销计时器回调
            TimerHandler timerHandler;
            _timerHandlers.TryGetValue(callback, out timerHandler);
            if (timerHandler != null)
            {
                _timerHandlers.Remove(callback);
                _timerHandlerPool.Release(timerHandler);
            }
        }

        /// <summary>
        /// 注册计时器回调函数（执行一次）
        /// </summary>  
        /// <param name="delayTime"></param>
        /// <param name="callback"></param>
        public void RegisterOnce(float delayTime, TimerCallback callback)
        {
            Register(false, delayTime, callback, 1);
        }

        /// <summary>
        /// 注册计时器回调函数（执行循环）
        /// </summary>
        /// <param name="delayTime"></param>
        /// <param name="callback"></param>
        public void RegisterLoop(float delayTime, TimerCallback callback)
        {
            Register(false, delayTime, callback, -1);
        }

        /// <summary>
        /// 注册帧回调函数（执行一次）
        /// </summary>  
        /// <param name="delayTime"></param>
        /// <param name="callback"></param>
        public void RegisterFrameOnce(int delayTime, TimerCallback callback)
        {
            Register(true, delayTime, callback, 1);
        }

        /// <summary>
        /// 注册帧回调函数（执行循环）
        /// </summary>
        /// <param name="delayTime"></param>
        /// <param name="callback"></param>
        public void RegisterFrameLoop(int delayTime, TimerCallback callback)
        {
            Register(true, delayTime, callback, -1);
        }

        private void DispatchEvent(string type)
        {
            TimerManagerEventArgs eventArgs = App.objectPoolManager.GetObject<TimerManagerEventArgs>();
            eventArgs.type = type;
            DispatchEvent(eventArgs);
            App.objectPoolManager.ReleaseObject(eventArgs);
        }

        /// <summary>
        /// 计时处理器
        /// </summary>
        private class TimerHandler : IPooledObject
        {
            public bool isFrame;
            public TimerCallback callback;
            public float delayTime;
            public int count;
            public double currentTime;
            public bool removeFlag;

            public void OnPoolGet()
            {

            }

            public void OnPoolReset()
            {
                callback = null;
                removeFlag = false;
            }

            public void OnPoolDispose()
            {
                isFrame = false;
                callback = null;
                delayTime = 0f;
                count = 0;
                currentTime = 0f;
                removeFlag = false;
            }
        }

        /// <summary>
        /// 延迟处理器
        /// </summary>
        private class DelayHanlder : IPooledObject
        {
            public bool isFrame;
            public TimerCallback callback;
            public float delayTime;
            public int count;
            public bool isRemove;

            public void OnPoolGet()
            {

            }

            public void OnPoolReset()
            {
                isFrame = false;
                callback = null;
                delayTime = 0f;
                count = 0;
                isRemove = false;
            }

            public void OnPoolDispose()
            {
                isFrame = false;
                callback = null;
                delayTime = 0f;
                count = 0;
                isRemove = false;
            }
        }
    }
}
