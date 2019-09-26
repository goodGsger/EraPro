using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class Manager : EventDispatcher, IManager
    {
        public Manager()
        {
            Init();
        }

        /// <summary>
        /// GameObject
        /// </summary>
        public GameObject gameObject
        {
            get { return App.inst.gameObject; }
        }

        /// <summary>
        /// AppBehaviour
        /// </summary>
        public AppBehaviour behaviour
        {
            get { return App.inst.behaviour; }
        }

        /// <summary>
        /// 初始化管理器
        /// </summary>
        protected virtual void Init()
        {

        }

        /// <summary>
        /// 更新管理器
        /// </summary>
        /// <param name="detalTime"></param>
        public virtual void Update(float detalTime)
        {

        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Destroy()
        {

        }

        /// <summary>
        /// 应用程序获得焦点
        /// </summary>
        public virtual void OnApplicationGetFocus()
        {

        }

        /// <summary>
        /// 应用程序失去焦点
        /// </summary>
        public virtual void OnApplicationLoseFocus()
        {

        }

        /// <summary>
        /// 应用程序退出
        /// </summary>
        public virtual void OnApplicationQuit()
        {

        }

        /// <summary>
        /// OnGUI
        /// </summary>
        public virtual void OnGUI()
        {

        }
    }
}
