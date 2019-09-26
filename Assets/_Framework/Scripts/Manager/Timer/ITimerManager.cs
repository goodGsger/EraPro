using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public delegate void TimerCallback();

    public interface ITimerManager : IManager
    {
        /// <summary>
        /// 当前时间（双精秒）
        /// </summary>
        double time { get; set; }

        /// <summary>
        /// 当前时间（浮点秒）
        /// </summary>
        float timeFloat { get;}

        /// <summary>
        /// 当前时间（秒）
        /// </summary>
        int timeSecond { get; }

        /// <summary>
        /// 当前时间（毫秒）
        /// </summary>
        long timeMilliseconds { get; }

        /// <summary>
        /// 计时器运行时间
        /// </summary>
        double currentTime { get; }

        /// <summary>
        /// 注册计时器回调函数
        /// </summary>
        /// <param name="isFrame"></param>
        /// <param name="delayTime"></param>
        /// <param name="callback"></param>
        /// <param name="count"></param>
        void Register(bool isFrame, float delayTime, TimerCallback callback, int count);

        /// <summary>
        /// 注销计时器回调函数
        /// </summary>
        /// <param name="callback"></param>
        void Unregister(TimerCallback callback);

        /// <summary>
        /// 注册计时器回调函数（执行一次）
        /// </summary>  
        /// <param name="delayTime"></param>
        /// <param name="callback"></param>
        void RegisterOnce(float delayTime, TimerCallback callback);

        /// <summary>
        /// 注册计时器回调函数（执行循环）
        /// </summary>
        /// <param name="delayTime"></param>
        /// <param name="callback"></param>
        void RegisterLoop(float delayTime, TimerCallback callback);

        /// <summary>
        /// 注册帧回调函数（执行一次）
        /// </summary>  
        /// <param name="delayTime"></param>
        /// <param name="callback"></param>
        void RegisterFrameOnce(int delayTime, TimerCallback callback);

        /// <summary>
        /// 注册帧回调函数（执行循环）
        /// </summary>
        /// <param name="delayTime"></param>
        /// <param name="callback"></param>
        void RegisterFrameLoop(int delayTime, TimerCallback callback);
    }
}
