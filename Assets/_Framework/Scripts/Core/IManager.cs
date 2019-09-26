using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public interface IManager : IEventDispatcher
    {
        /// <summary>
        /// GameObject
        /// </summary>
        GameObject gameObject { get; }

        /// <summary>
        /// AppBehaviour
        /// </summary>
        AppBehaviour behaviour { get; }

        /// <summary>
        /// 更新管理器
        /// </summary>
        /// <param name="deltaTime"></param>
        void Update(float deltaTime);

        /// <summary>
        /// 销毁
        /// </summary>
        void Destroy();

        /// <summary>
        /// 应用程序获得焦点
        /// </summary>
        void OnApplicationGetFocus();

        /// <summary>
        /// 应用程序失去焦点
        /// </summary>
        void OnApplicationLoseFocus();

        /// <summary>
        /// 应用程序退出
        /// </summary>
        void OnApplicationQuit();

        /// <summary>
        /// OnGUI
        /// </summary>
        void OnGUI();
    }
}
