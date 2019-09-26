using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public delegate void TouchHandle(TouchContext context);

    public interface ITouchManager : IManager
    {
        float gestureDistance { get; set; }

        TouchHandle OnTouchDown { get; set; }

        TouchHandle OnTouchUp { get; set; }

        TouchHandle OnTouchClick { get; set; }

        TouchHandle OnTouchMove { get; set; }

        TouchHandle OnTouchGesture { get; set; }

        Action OnIdle { get; set; }

        float maxIdleTime { get; set; }

        /// <summary>
        /// 开始检查空闲状态
        /// </summary>
        void StartCheckIdleState();

        /// <summary>
        /// 停止检查空闲状态
        /// </summary>
        void StopCheckIdleState();

        /// <summary>
        /// 重设空闲触摸状态
        /// </summary>
        void ResetTouchIdleState();
    }
}
