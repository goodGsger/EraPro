using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class AssetManager : Manager, IAssetManager
    {
        private Dictionary<string, IAsset> _assetDict;

        private bool _enabled;
        private float _autoClearTime;
        private CheckAutoClearDelegate _checkAutoClearDelegate;

        protected override void Init()
        {
            _assetDict = new Dictionary<string, IAsset>();
            _autoClearTime = 10;
            enabled = true;
        }

        public bool enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (_enabled)
                {
                    App.timerManager.RegisterLoop(5f, ClearGameObjects);
                }
                else
                {
                    App.timerManager.Unregister(ClearGameObjects);
                }
            }
        }

        /// <summary>
        /// 自动清理时间
        /// </summary>
        public float autoClearTime
        {
            get { return _autoClearTime; }
            set
            {
                _autoClearTime = value;
                ClearGameObjects();
            }
        }

        /// <summary>
        /// 检测自动清理
        /// </summary>
        public CheckAutoClearDelegate checkAutoClearDelegate
        {
            get { return _checkAutoClearDelegate; }
            set { _checkAutoClearDelegate = value; }
        }

        /// <summary>
        /// 清理游戏对象
        /// </summary>
        public void ClearGameObjects()
        {
            float time = Time.time;
            List<IAsset> removedList = new List<IAsset>(10);
            foreach (var v in _assetDict)
            {
                IAsset asset = v.Value;

                if (asset.autoClear == false)
                    continue;

                if (asset.lastUseTime > 0 && asset.useCount == 0)
                {
                    if (_checkAutoClearDelegate != null)
                    {
                        if (_checkAutoClearDelegate.Invoke(asset))
                            continue;
                    }

                    if (time - asset.lastUseTime >= _autoClearTime)
                    {
                        removedList.Add(asset);
                    }
                }
            }

            foreach (IAsset asset in removedList)
            {
                asset.Dispose();
                _assetDict.Remove(asset.id);
            }
        }

        /// <summary>
        /// 向内存中添加资源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="asset"></param>
        public void AddAsset(string id, IAsset asset)
        {
            _assetDict[id] = asset;
            asset.OnAdd();
        }

        /// <summary>
        /// 获取内存中是否包含资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasAsset(string id)
        {
            return _assetDict.ContainsKey(id);
        }

        /// <summary>
        /// 从内存中移除资源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="destroy"></param>
        public void RemoveAsset(string id, bool destroy = true)
        {
            if (_assetDict.TryGetValue(id, out IAsset asset))
            {
                _assetDict.Remove(id);
                if (destroy)
                    asset.Dispose();
            }
        }

        /// <summary>
        /// 根据ID获取内存中的资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IAsset GetAsset(string id)
        {
            if (_assetDict.TryGetValue(id, out IAsset asset))
                return asset;

            return null;
        }

        /// <summary>
        /// 根据ID获取内存中的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetAsset<T>(string id) where T : IAsset
        {
            IAsset asset = GetAsset(id);
            if (asset != null)
                return (T)asset;

            return default;
        }

        /// <summary>
        /// 根据ID获取内存中的Package资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IAssetPackage GetAssetPackage(string id)
        {
            if (_assetDict.TryGetValue(id, out IAsset assetPackage))
                return assetPackage as IAssetPackage;

            return null;
        }

        /// <summary>
        /// 根据ID获取内存中的Package资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetAssetPackage<T>(string id) where T : IAssetPackage
        {
            IAssetPackage assetPackage = GetAssetPackage(id);
            if (assetPackage != null)
                return (T)assetPackage;

            return default;
        }

        /// <summary>
        /// 根据ID，Name获取Package中的资源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetAssetInPackage(string id, string name)
        {
            IAssetPackage assetPackage = GetAssetPackage(id);
            if (assetPackage != null)
                return assetPackage.GetAsset(name);

            return null;
        }

        /// <summary>
        /// 根据ID，Name获取Package中的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetAssetInPackage<T>(string id, string name) where T : UnityEngine.Object
        {
            IAssetPackage assetPackage = GetAssetPackage(id);
            if (assetPackage != null)
                return assetPackage.GetAsset<T>(name);

            return null;
        }

        /// <summary>
        /// 使用资源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        public void UseAsset(string id, int count = 1)
        {
            IAsset asset = GetAsset(id);
            if (asset != null)
                asset.Use(count);
        }

        /// <summary>
        /// 不使用资源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        public void UnuseAsset(string id, int count = 1)
        {
            IAsset asset = GetAsset(id);
            if (asset != null)
                asset.Unuse(count);
        }

        /// <summary>
        /// 向本地保存资源
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="path"></param>
        /// <param name="overwrite"></param>
        public void SaveAsset(IAsset asset, string path, bool overwrite)
        {
            if (asset == null)
                return;

            byte[] bytes = asset.GetBytes();
            if (bytes == null)
                return;

            App.fileManager.WriteFilePersistentAsync(path, bytes, overwrite);
        }

        /// <summary>
        /// 从本地删除资源
        /// </summary>
        /// <param name="path"></param>
        public void DeleteAsset(string path)
        {
            App.fileManager.DeleteFilePersistentAsync(path);
        }

        /// <summary>
        /// 从本地删除资源
        /// </summary>
        /// <param name="asset"></param>
        public void DeleteAsset(IAsset asset)
        {
            DeleteAsset(asset.url);
        }
    }
}
