using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface IStageManager : IManager
    {
        /// <summary>
        /// 帧频
        /// </summary>
        int fps { get; set; }

        /// <summary>
        /// 基础帧频
        /// </summary>
        int baseFps { get; set; }

        /// <summary>
        /// 帧频缩放
        /// </summary>
        float fpsFactor { get; }

        /// <summary>
        /// 帧间隔（秒）
        /// </summary>
        float interval { get; }

        /// <summary>
        /// 帧间隔（毫秒）
        /// </summary>
        float intervalMilliseconds { get; }

        /// <summary>
        /// 基础帧间隔（秒）
        /// </summary>
        float baseInterval { get; }

        /// <summary>
        /// 基础帧间隔（毫秒）
        /// </summary>
        float baseIntervalMilliseconds { get; }

        /// <summary>
        /// 设计宽度
        /// </summary>
        int designWidth { get; }

        /// <summary>
        /// 设计高度
        /// </summary>
        int designHeight { get; }

        /// <summary>
        /// 场景宽度
        /// </summary>
        int screenWidth { get; }

        /// <summary>
        /// 场景高度
        /// </summary>
        int screenHeight { get; }

        /// <summary>
        /// 设计比例
        /// </summary>
        float designRatio { get; }

        /// <summary>
        /// 屏幕比例
        /// </summary>
        float screenRatio { get; }

        /// <summary>
        /// 最小缩放
        /// </summary>
        float minScaleFactor { get; }

        /// <summary>
        /// 最大缩放
        /// </summary>
        float maxScaleFactor { get; }

        /// <summary>
        /// 设置设计尺寸
        /// </summary>
        /// <param name="designWidth"></param>
        /// <param name="designHeight"></param>
        void SetDesignSize(int designWidth, int designHeight);

        /// <summary>
        /// 设置屏幕尺寸
        /// </summary>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        void SetScreenSize(int screenWidth, int screenHeight);

        /// <summary>
        /// 基础帧转换为运行帧
        /// </summary>
        /// <param name="frames"></param>
        /// <returns></returns>
        int BaseFramesToCurrentFrames(int frames);

        /// <summary>
        /// 基础时间换为运行时间
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        float BaseIntervalToCurrentInterval(float interval);

        /// <summary>
        /// 运行帧转换为基础帧
        /// </summary>
        /// <param name="frames"></param>
        /// <returns></returns>
        int CurrentFramesToBaseFrames(int frames);

        /// <summary>
        /// 运行时间换为基础时间
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        float CurrentIntervalToBaseInterval(float interval);

        /// <summary>
        /// 基础时间换为运行时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        int TimeToFrames(float time);

        /// <summary>
        /// 帧转换为时间
        /// </summary>
        /// <param name="frames"></param>
        /// <returns></returns>
        float FramesToTime(int frames);
    }
}
