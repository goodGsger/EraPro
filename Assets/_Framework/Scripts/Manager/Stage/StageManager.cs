using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class StageManager : Manager, IStageManager
    {
        private int _fps;
        private int _baseFps;
        private float _fpsFactor;
        private float _interval;
        private float _intervalMilliseconds;
        private float _baseInterval;
        private float _baseIntervalMilliseconds;
        private int _designWidth;
        private int _designHeight;
        private int _screenWidth;
        private int _screenHeight;
        private float _designRatio;
        private float _screenRatio;
        private float _minScaleFactor;
        private float _maxScaleFactor;

        /// <summary>
        /// 帧频
        /// </summary>
        public int fps
        {
            get { return _fps; }
            set
            {
                if (_fps != value)
                {
                    _fps = value;
                    _interval = 1f / _fps;
                    _intervalMilliseconds = _interval * 1000;
                    _fpsFactor = (float)_fps / _baseFps;
                }
            }
        }

        /// <summary>
        /// 基础帧频
        /// </summary>
        public int baseFps
        {
            get { return _baseFps; }
            set
            {
                _baseFps = value;
                _baseInterval = 1f / _baseFps;
                _baseIntervalMilliseconds = _baseInterval * 1000;
            }
        }

        /// <summary>
        /// 实际帧倍数
        /// </summary>
        public float fpsFactor
        {
            get { return _fpsFactor; }
        }

        /// <summary>
        /// 帧间隔（秒）
        /// </summary>
        public float interval
        {
            get { return _interval; }
        }

        /// <summary>
        /// 帧间隔（毫秒）
        /// </summary>
        public float intervalMilliseconds
        {
            get { return _intervalMilliseconds; }
        }

        /// <summary>
        /// 基础帧间隔（秒）
        /// </summary>
        public float baseInterval
        {
            get { return _baseInterval; }
        }

        /// <summary>
        /// 基础帧间隔（毫秒）
        /// </summary>
        public float baseIntervalMilliseconds
        {
            get { return _baseIntervalMilliseconds; }
        }

        /// <summary>
        /// 设计宽度
        /// </summary>
        public int designWidth
        {
            get { return _designWidth; }
        }

        /// <summary>
        /// 设计高度
        /// </summary>
        public int designHeight
        {
            get { return _designHeight; }
        }

        /// <summary>
        /// 场景宽度
        /// </summary>
        public int screenWidth
        {
            get { return _screenWidth; }
        }

        /// <summary>
        /// 场景高度
        /// </summary>
        public int screenHeight
        {
            get { return _screenHeight; }
        }

        /// <summary>
        /// 设计比例
        /// </summary>
        public float designRatio
        {
            get { return _designRatio; }
        }

        /// <summary>
        /// 屏幕比例
        /// </summary>
        public float screenRatio
        {
            get { return _screenRatio; }
        }

        /// <summary>
        /// 最小缩放
        /// </summary>
        public float minScaleFactor
        {
            get { return _minScaleFactor; }
        }

        /// <summary>
        /// 最大缩放
        /// </summary>
        public float maxScaleFactor
        {
            get { return _maxScaleFactor; }
        }

        /// <summary>
        /// 设置设计尺寸
        /// </summary>
        /// <param name="designWidth"></param>
        /// <param name="designHeight"></param>
        public void SetDesignSize(int designWidth, int designHeight)
        {
            _designWidth = designWidth;
            _designHeight = designHeight;
            _designRatio = (float)_designWidth / _designHeight;
        }

        /// <summary>
        /// 设置屏幕尺寸
        /// </summary>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        public void SetScreenSize(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _screenRatio = (float)_screenWidth / _screenHeight;
            if (_designWidth == 0 || _designHeight == 0)
            {
                throw new Exception("must set design size first!");
            }
            float sx = (float)_screenWidth / _designWidth;
            float sy = (float)_screenHeight / designHeight;
            _minScaleFactor = Mathf.Min(sx, sy);
            _maxScaleFactor = Mathf.Max(sx, sy);
            DispatchEvent(StageManagerEventArgs.RESIZE);
        }

        /// <summary>
        /// 基础帧转换为运行帧
        /// </summary>
        /// <param name="frames"></param>
        /// <returns></returns>
        public int BaseFramesToCurrentFrames(int frames)
        {
            return (int)(frames * _fpsFactor);
        }

        /// <summary>
        /// 基础时间换为运行时间
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public float BaseIntervalToCurrentInterval(float interval)
        {
            return interval * _fpsFactor;
        }

        /// <summary>
        /// 运行帧转换为基础帧
        /// </summary>
        /// <param name="frames"></param>
        /// <returns></returns>
        public int CurrentFramesToBaseFrames(int frames)
        {
            return (int)(frames / _fpsFactor);
        }

        /// <summary>
        /// 运行时间换为基础时间
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public float CurrentIntervalToBaseInterval(float interval)
        {
            return interval / _fpsFactor;
        }

        /// <summary>
        /// 基础时间换为运行时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public int TimeToFrames(float time)
        {
            return BaseFramesToCurrentFrames((int)(time / _baseInterval));
        }

        /// <summary>
        /// 帧转换为时间
        /// </summary>
        /// <param name="frames"></param>
        /// <returns></returns>
        public float FramesToTime(int frames)
        {
            return frames / _baseFps;
        }

        private void DispatchEvent(string type)
        {
            StageManagerEventArgs eventArgs = App.objectPoolManager.GetObject<StageManagerEventArgs>();
            eventArgs.type = type;
            App.objectPoolManager.ReleaseObject(eventArgs);
        }
    }
}
