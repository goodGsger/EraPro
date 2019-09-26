using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class AssetBundleAsset : AbstractAssetPackage
    {
        protected Dictionary<string, object> _assetCache;

        public AssetBundleAsset()
        {
            _assetCache = new Dictionary<string, object>();
        }

        public override object asset
        {
            get
            {
                return _asset;
            }

            set
            {
                _asset = value;
                _assetBundle = _asset as AssetBundle;
            }
        }

        protected override byte[] CreateBytes()
        {
            return null;
        }

        override public void Dispose()
        {
            _assetCache = null;
            base.Dispose();
        }

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override object GetAsset(string name)
        {
            object asset;
            if (_assetCache.TryGetValue(name, out asset) == true)
                return asset;

            asset = _assetBundle.LoadAsset(name);
            if (asset != null)
                _assetCache[name] = asset;
            return asset;
        }

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override object GetAsset(string name, Type type)
        {
            object asset;
            if (_assetCache.TryGetValue(name, out asset) == true)
                return asset;

            asset = _assetBundle.LoadAsset(name, type);
            if (asset != null)
                _assetCache[name] = asset;
            return asset;
        }

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public override T GetAsset<T>(string name)
        {
            object asset;
            if (_assetCache.TryGetValue(name, out asset) == true)
                return asset as T;

            asset = _assetBundle.LoadAsset<T>(name);
            if (asset != null)
                _assetCache[name] = asset;
            return asset as T;
        }

        /// <summary>
        /// 资源包中是否包含资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override bool HasAsset(string name)
        {
            return _assetBundle.Contains(name);
        }

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override object[] GetAssets(Type type)
        {
            return _assetBundle.LoadAllAssets(type);
        }

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override T[] GetAssets<T>()
        {
            return _assetBundle.LoadAllAssets<T>();
        }
    }
}
