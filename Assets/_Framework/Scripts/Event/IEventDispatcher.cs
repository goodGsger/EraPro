using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public delegate void EventDelegate(EventArguments eventArgs);

    public interface IEventDispatcher
    {
        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="type"></param>
        /// <param name="eventDelegate"></param>
        void AddEventListener(string type, EventDelegate eventDelegate);

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="type"></param>
        /// <param name="eventDelegate"></param>
        void RemoveEventListener(string type, EventDelegate eventDelegate);

        /// <summary>
        /// 删除所有事件监听
        /// </summary>
        void RemoveEventListeners();

        /// <summary>
        /// 移除所有事件监听
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        void RemoveEventListeners(string type);

        /// <summary>
        /// 是否已添加事件监听
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool HasEventListener(string type);

        /// <summary>
        /// 是否已添加事件监听
        /// </summary>
        /// <param name="type"></param>
        /// <param name="eventDelegate"></param>
        /// <returns></returns>
        bool HasEventListener(string type, EventDelegate eventDelegate);

        /// <summary>
        /// 派发事件
        /// </summary>
        /// <param name="eventArg"></param>
        /// <returns></returns>
        bool DispatchEvent(EventArguments eventArg);
    }
}
