using FairyGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 未处理手势
    /// </summary>
    public class TouchManager : Manager, ITouchManager
    {
        private bool _touchMode;
        private TouchContext[] _contexts;

        private float _gestureDistance;

        private TouchHandle _OnTouchDown;
        private TouchHandle _OnTouchUp;
        private TouchHandle _OnTouchClick;
        private TouchHandle _OnTouchMove;
        private TouchHandle _OnTouchGesture;

        private bool _enableCheckIdleState;
        private float _maxIdleTime;
        private float _lastTouchTime;
        private int _lastTouchCount;
        private Action _OnIdle;

        public float gestureDistance
        {
            set { _gestureDistance = value; }
            get { return _gestureDistance; }
        }

        public TouchHandle OnTouchDown
        {
            get { return _OnTouchDown; }
            set { _OnTouchDown = value; }
        }

        public TouchHandle OnTouchUp
        {
            get { return _OnTouchUp; }
            set { _OnTouchUp = value; }
        }

        public TouchHandle OnTouchClick
        {
            get { return _OnTouchClick; }
            set { _OnTouchClick = value; }
        }

        public TouchHandle OnTouchMove
        {
            get { return _OnTouchMove; }
            set { _OnTouchMove = value; }
        }

        public TouchHandle OnTouchGesture
        {
            get { return _OnTouchGesture; }
            set { _OnTouchGesture = value; }
        }

        public Action OnIdle
        {
            get { return _OnIdle; }
            set { _OnIdle = value; }
        }

        public float maxIdleTime
        {
            get { return _maxIdleTime; }
            set { _maxIdleTime = value; }
        }

        public float lastTouchTime
        {
            get { return _lastTouchTime; }
            set { _lastTouchTime = value; }
        }

        protected override void Init()
        {
            base.Init();

            if (Application.platform == RuntimePlatform.WindowsPlayer
                || Application.platform == RuntimePlatform.WindowsEditor
                || Application.platform == RuntimePlatform.OSXPlayer
                || Application.platform == RuntimePlatform.OSXEditor)
                _touchMode = false;
            else
                _touchMode = Input.touchSupported && SystemInfo.deviceType != DeviceType.Desktop;

            _contexts = new TouchContext[5];
            for (int i = 0; i < _contexts.Length; i++)
            {
                _contexts[i] = new TouchContext();
                _contexts[i].touchID = i;
            }
        }

        public override void Update(float detalTime)
        {
            base.Update(detalTime);
            if (_touchMode)
            {
                HandleTouchEvents();
            }
            else
            {
                HandleMouseEvents();
            }
        }

        private void HandleTouchEvents()
        {
            CheckTouchIdleState(Input.touchCount);
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                for (int j = 0; j < 5; j++)
                {
                    TouchContext context = _contexts[j];
                    if (context.touchID == touch.fingerId)
                    {
                        if (context.touchDown)
                        {
                            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                            {
                                HandleTouchUp(context, touch);
                            }
                            else if (touch.phase == TouchPhase.Moved)
                            {
                                HandleTouchMove(context, touch);
                            }
                        }
                        else
                        {
                            if (touch.phase == TouchPhase.Began)
                            {
                                HandleTouchDown(context, touch);
                            }
                        }
                    }
                }
            }
        }

        private void HandleTouchDown(TouchContext context, Touch touch)
        {
            if (Stage.isTouchOnUI)
                return;

            context.touchDown = true;
            context.position = context.downPosition = touch.position;

            if (_OnTouchDown != null)
                _OnTouchDown.Invoke(context);
        }

        private void HandleTouchUp(TouchContext context, Touch touch)
        {
            context.touchDown = false;

            if (_OnTouchUp != null)
                _OnTouchUp.Invoke(context);

            if (_OnTouchClick != null)
                _OnTouchClick.Invoke(context);

            context.Reset();
        }

        private void HandleTouchMove(TouchContext context, Touch touch)
        {
            if (!touch.position.Equals(context.position))
            {
                context.position = touch.position;

                if (_OnTouchMove != null)
                    _OnTouchMove.Invoke(context);
            }
        }

        private void HandleMouseEvents()
        {
            CheckTouchIdleState(Input.GetMouseButton(0) ? 1 : 0);
            TouchContext context = _contexts[0];
            if (Input.GetMouseButtonDown(0))
            {
                HandleMouseDown(context);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (context.touchDown)
                {
                    HandleMouseUp(context);
                }
            }
            else
            {
                if (context.touchDown)
                {
                    HandleMouseMove(context);
                }
            }
        }

        private void HandleMouseDown(TouchContext context)
        {
            if (Stage.isTouchOnUI)
                return;

            context.touchDown = true;
            context.position = context.downPosition = Input.mousePosition;

            if (_OnTouchDown != null)
                _OnTouchDown.Invoke(context);
        }

        private void HandleMouseUp(TouchContext context)
        {
            context.touchDown = false;

            if (_OnTouchUp != null)
                _OnTouchUp.Invoke(context);

            if (_OnTouchClick != null)
                _OnTouchClick.Invoke(context);

            context.Reset();
        }

        private void HandleMouseMove(TouchContext context)
        {
            Vector3 mousePosition = Input.mousePosition;

            if (!mousePosition.Equals(context.position))
            {
                context.position = mousePosition;

                if (_OnTouchMove != null)
                    _OnTouchMove.Invoke(context);
            }
        }

        private void CheckTouchIdleState(int touchCount)
        {
            if (_enableCheckIdleState == false)
                return;
            float time = Time.realtimeSinceStartup;
            if (_lastTouchCount != touchCount)
            {
                _lastTouchCount = touchCount;
                _lastTouchTime = time;
            }
            else
            {
                if (_lastTouchCount == 0)
                {
                    if (time - _lastTouchTime >= _maxIdleTime)
                    {
                        //_lastTouchTime = time;
                        if (_OnIdle != null)
                            _OnIdle.Invoke();
                    }
                }
            }
        }

        /// <summary>
        /// 开始检查空闲状态
        /// </summary>
        public void StartCheckIdleState()
        {
            _enableCheckIdleState = true;
            _lastTouchTime = Time.realtimeSinceStartup;
        }

        /// <summary>
        /// 停止检查空闲状态
        /// </summary>
        public void StopCheckIdleState()
        {
            _enableCheckIdleState = false;
            _lastTouchTime = 0;
        }

        /// <summary>
        /// 重设空闲触摸状态
        /// </summary>
        public void ResetTouchIdleState()
        {
            _lastTouchTime = Time.realtimeSinceStartup;
        }
    }
}
