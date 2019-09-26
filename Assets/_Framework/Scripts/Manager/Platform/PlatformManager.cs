using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class PlatformManager : Manager, IPlatformManager
    {
        private NetworkReachability _networkReachability;
        private bool _escKeyDown;

        private Action _networkChanged;
        private Action _applicationGetFocus;
        private Action _applicationLoseFocus;
        private Action _applicationQuit;
        private Action _onEscKeyClicked;

        /// <summary>
        /// 网络状态
        /// </summary>
        public int networkReachability
        {
            get { return (int)_networkReachability; }
        }

        /// <summary>
        /// 网络切换
        /// </summary>
        public Action networkChanged
        {
            get { return _networkChanged; }
            set { _networkChanged = value; }
        }

        /// <summary>
        /// 应用程序获得焦点
        /// </summary>
        public Action applicationGetFocus
        {
            get { return _applicationGetFocus; }
            set { _applicationGetFocus = value; }
        }

        /// <summary>
        /// 应用程序失去焦点
        /// </summary>
        public Action applicationLoseFocus
        {
            get { return _applicationLoseFocus; }
            set { _applicationLoseFocus = value; }
        }

        /// <summary>
        /// 应用程序退出
        /// </summary>
        public Action applicationQuit
        {
            get { return _applicationQuit; }
            set { _applicationQuit = value; }
        }

        /// <summary>
        /// 返回键
        /// </summary>
        public Action onEscKeyClicked
        {
            get { return _onEscKeyClicked; }
            set { _onEscKeyClicked = value; }
        }

        /// <summary>
        /// 是否为IphoneX
        /// </summary>
        public bool isIphoneX
        {
            get
            {
                string deviceModel = SystemInfo.deviceModel;
                return deviceModel.IndexOf("iPhone10,3") != -1 || deviceModel.IndexOf("iPhone10,6") != -1;
            }
        }

        /// <summary>
        /// 初始化管理器
        /// </summary>
        protected override void Init()
        {
            _networkReachability = Application.internetReachability;
        }

        /// <summary>
        /// 更新管理器
        /// </summary>
        /// <param name="detalTime"></param>
        public override void Update(float detalTime)
        {
            // 判断网络状态
            NetworkReachability current = Application.internetReachability;
            if (current != _networkReachability)
            {
                _networkReachability = current;
                if (_networkChanged != null)
                    _networkChanged.Invoke();
            }

            // 返回键
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_escKeyDown == false)
                {
                    _escKeyDown = true;
                    if (_onEscKeyClicked != null)
                        _onEscKeyClicked.Invoke();
                }
            }
            else
            {
                _escKeyDown = false;
            }
        }

        /// <summary>
        /// 应用程序获得焦点
        /// </summary>
        public override void OnApplicationGetFocus()
        {
            if (_applicationGetFocus != null)
                _applicationGetFocus.Invoke();
        }

        /// <summary>
        /// 应用程序失去焦点
        /// </summary>
        public override void OnApplicationLoseFocus()
        {
            if (_applicationLoseFocus != null)
                _applicationLoseFocus.Invoke();
        }

        /// <summary>
        /// 应用程序退出
        /// </summary>
        public override void OnApplicationQuit()
        {
            if (_applicationQuit != null)
                _applicationQuit.Invoke();
        }

        /// <summary>
        /// 手机振动
        /// </summary>
        public void Vibrate()
        {
#if !UNITY_STANDALONE_WIN
            Handheld.Vibrate();
#endif
        }
    }
}
