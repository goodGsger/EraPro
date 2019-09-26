using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public delegate void CoroutineCallback(CoroutineTask task);

    public class CoroutineTask : IPooledObject
    {
        protected bool _running;
        protected bool _paused;
        protected IEnumerator _routine;
        protected CoroutineCallback _callback;

        protected MonoBehaviour _behaviour;
        protected Coroutine _coroutine;

        /// <summary>
        /// 是否运行
        /// </summary>
        public bool running
        {
            get { return _running; }
        }

        /// <summary>
        /// 是否暂停
        /// </summary>
        public bool paused
        {
            get { return _paused; }
        }

        /// <summary>
        /// 协程函数
        /// </summary>
        public IEnumerator routine
        {
            get { return _routine; }
            set { _routine = value; }
        }

        /// <summary>
        /// 回调函数
        /// </summary>
        public CoroutineCallback callback
        {
            get { return _callback; }
            set { _callback = value; }
        }

        public CoroutineTask()
        {

        }

        public CoroutineTask(IEnumerator routine)
        {
            _routine = routine;
        }

        public CoroutineTask(IEnumerator routine, CoroutineCallback callback)
        {
            _routine = routine;
            _callback = callback;
        }

        /// <summary>
        /// 启动协程
        /// </summary>
        public void Start()
        {
            Start(App.inst.behaviour);
        }

        /// <summary>
        /// 启动协程
        /// </summary>
        /// <param name="behaviour"></param>
        public void Start(MonoBehaviour behaviour)
        {
            Stop();

            if (_routine == null)
                return;

            _behaviour = behaviour;
            _coroutine = behaviour.StartCoroutine(CoroutineWrapper());
            _running = true;
        }

        /// <summary>
        /// 协程包装函数
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoroutineWrapper()
        {
            yield return null;
            while (_running)
            {
                if (_paused)
                    yield return null;
                else
                {
                    IEnumerator r = _routine;
                    if (_routine != null && _routine.MoveNext())
                    {
                        if (_routine != null)
                            yield return _routine.Current;
                        else
                            _running = false;
                    }
                    else
                    {
                        if (_routine == r)
                            _running = false;
                    }

                }
            }

            if (_callback != null)
                _callback.Invoke(this);
        }

        /// <summary>
        /// 停止协程
        /// </summary>
        public void Stop()
        {
            if (_behaviour != null && _coroutine != null)
                _behaviour.StopCoroutine(_coroutine);

            _behaviour = null;
            _coroutine = null;
            _running = false;
            _paused = false;
        }

        /// <summary>
        /// 暂停协程
        /// </summary>
        public void Pause()
        {
            _paused = true;
        }

        /// <summary>
        /// 继续协程
        /// </summary>
        public void Unpause()
        {
            _paused = false;
        }

        /// <summary>
        /// 从对象池中获取
        /// </summary>
        public void OnPoolGet()
        {

        }

        /// <summary>
        /// 重置对象池对象
        /// </summary>
        public void OnPoolReset()
        {
            if (_behaviour != null && _coroutine != null)
                _behaviour.StopCoroutine(_coroutine);

            _running = false;
            _paused = false;
            _routine = null;
            _callback = null;
            _behaviour = null;
            _coroutine = null;
        }

        /// <summary>
        /// 销毁对象池对象
        /// </summary>
        public void OnPoolDispose()
        {
            OnPoolReset();
        }
    }
}
