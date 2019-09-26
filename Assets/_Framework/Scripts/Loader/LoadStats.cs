using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class LoadStats
    {
        protected float _totalTime;
        protected float _progress;

        protected float _startTime;

        /// <summary>
        /// 总时间
        /// </summary>
        public float totalTime
        {
            get { return _totalTime; }
            set { _totalTime = value; }
        }

        /// <summary>
        /// 下载进度
        /// </summary>
        public float progress
        {
            get { return _progress; }
            set { _progress = value; }
        }

        /// <summary>
        /// 开始下载
        /// </summary>
        public void Start()
        {
            _totalTime = 0f;
            _progress = 0f;

            _startTime = Time.time;
        }

        /// <summary>
        /// 更新下载进度
        /// </summary>
        /// <param name="bytesLoaded"></param>
        /// <param name="bytesTotal"></param>
        public void Update(float progress)
        {
            _progress = progress;
        }

        /// <summary>
        /// 完成下载
        /// </summary>
        public void Done()
        {
            Update(1f);
            _totalTime = Time.time - _startTime;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            _totalTime = 0f;
            _progress = 0f;
            _startTime = 0f;
        }
    }
}
