using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public abstract class AbstractAsset : IAsset
    {
        protected string _id;
        protected string _url;
        protected object _asset;
        protected AssetBundle _assetBundle;
        protected int _useCount;
        protected double _lastUseTime;
        protected bool _autoClear = true;

        /// <summary>
        /// 唯一ID
        /// </summary>
        public string id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string url
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public virtual object asset
        {
            get { return _asset; }
            set { _asset = value; }
        }

        /// <summary>
        /// assetBundle
        /// </summary>
        public virtual AssetBundle assetBundle
        {
            get { return _assetBundle; }
            set { _assetBundle = value; }
        }

        /// <summary>
        /// 使用计数
        /// </summary>
        public int useCount
        {
            get { return _useCount; }
            set { _useCount = value; }
        }

        /// <summary>
        /// 最后使用时间
        /// </summary>
        public double lastUseTime
        {
            get { return _lastUseTime; }
            set { _lastUseTime = value; }
        }

        /// <summary>
        /// 是否自动清理
        /// </summary>
        public bool autoClear
        {
            get { return _autoClear; }
            set { _autoClear = value; }
        }

        /// <summary>
        /// 获取字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            return CreateBytes();
        }

        /// <summary>
        /// 使用计数
        /// </summary>
        /// <param name="count"></param>
        public virtual void Use(int count = 1)
        {
            _useCount += count;
            lastUseTime = Time.time;
        }

        /// <summary>
        /// 不使用计数
        /// </summary>
        /// <param name="count"></param>
        public virtual void Unuse(int count = 1)
        {
            _useCount -= count;

            if (_useCount < 0)
                _useCount = 0;

            if (_useCount == 0)
                _lastUseTime = Time.time;
        }

        /// <summary>
        /// 添加到资源管理器中
        /// </summary>
        public virtual void OnAdd()
        {
            
        }

        /// <summary>
        /// 生成字节数组
        /// </summary>
        /// <returns></returns>
        protected abstract byte[] CreateBytes();

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Dispose()
        {
            _asset = null;
            if (_assetBundle != null)
            {
                _assetBundle.Unload(true);
                _assetBundle = null;
            }
        }
    }
}
