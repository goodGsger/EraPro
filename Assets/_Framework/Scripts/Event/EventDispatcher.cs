using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class EventDispatcher : IEventDispatcher
    {
        private Dictionary<string, HashSet<EventDelegate>> _eventListeners;

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="type"></param>
        /// <param name="eventDelegate"></param>
        public void AddEventListener(string type, EventDelegate eventDelegate)
        {
            if (_eventListeners == null)
                _eventListeners = new Dictionary<string, HashSet<EventDelegate>>();

            HashSet<EventDelegate> listeners;
            if (_eventListeners.TryGetValue(type, out listeners) == false)
                _eventListeners[type] = listeners = new HashSet<EventDelegate>();

            listeners.Add(eventDelegate);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="type"></param>
        /// <param name="eventDelegate"></param>
        public void RemoveEventListener(string type, EventDelegate eventDelegate)
        {
            if (_eventListeners == null)
                return;

            HashSet<EventDelegate> listeners;
            if (_eventListeners.TryGetValue(type, out listeners))
                listeners.Remove(eventDelegate);
        }

        /// <summary>
        /// 移除所有事件监听
        /// </summary>
        public void RemoveEventListeners()
        {
            if (_eventListeners == null)
                return;

            _eventListeners.Clear();
        }

        /// <summary>
        /// 移除所有事件监听
        /// </summary>
        /// <param name="type"></param>
        public void RemoveEventListeners(string type)
        {
            if (_eventListeners == null)
                return;

            if (_eventListeners.ContainsKey(type))
                _eventListeners.Remove(type);
        }

        /// <summary>
        /// 是否已添加事件监听
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool HasEventListener(string type)
        {
            if (_eventListeners == null)
                return false;

            return _eventListeners.ContainsKey(type);
        }

        /// <summary>
        /// 是否已添加事件监听
        /// </summary>
        /// <param name="type"></param>
        /// <param name="eventDelegate"></param>
        /// <returns></returns>
        public bool HasEventListener(string type, EventDelegate eventDelegate)
        {
            if (_eventListeners == null)
                return false;

            HashSet<EventDelegate> listeners;
            if (_eventListeners.TryGetValue(type, out listeners))
                return listeners.Contains(eventDelegate);

            return false;
        }

        /// <summary>
        /// 派发事件
        /// </summary>
        /// <param name="eventArg"></param>
        /// <returns></returns>
        public bool DispatchEvent(EventArguments eventArg)
        {
            if (_eventListeners == null)
                return false;

            HashSet<EventDelegate> listeners;
            if (_eventListeners.TryGetValue(eventArg.type, out listeners))
            {
                foreach (EventDelegate eventDelegate in listeners)
                    eventDelegate.Invoke(eventArg);

                return true;
            }

            return false;
        }
    }
}
