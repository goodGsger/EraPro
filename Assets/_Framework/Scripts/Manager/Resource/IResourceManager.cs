using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public delegate void LoadCallback(LoadItem loadItem);
    public delegate void LoadQueueCallback(LoadQueue loadQueue);

    public interface IResourceManager : IManager
    {
        /// <summary>
        /// 最大加载数量
        /// </summary>
        int maxLoaders { get; set; }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="url"></param>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="priority"></param>
        /// <param name="completeCallback"></param>
        /// <param name="progressCallback"></param>
        /// <param name="errorCallback"></param>
        /// <param name="save"></param>
        /// <param name="cache"></param>
        /// <param name="loadImmediately"></param>
        /// <returns></returns>
        LoadItem Load(string url, string id, LoadType type = LoadType.AUTO, LoadPriority priority = LoadPriority.LV_2,
            LoadCallback completeCallback = null, LoadCallback progressCallback = null, LoadCallback errorCallback = null, bool save = true, bool cache = true, bool loadImmediately = false);

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type"></param>
        /// <param name="priority"></param>
        /// <param name="completeCallback"></param>
        /// <param name="save"></param>
        /// <param name="cache"></param>
        /// <param name="loadImmediately"></param>
        /// <returns></returns>
        LoadItem Load(string url, LoadType type = LoadType.AUTO, LoadPriority priority = LoadPriority.LV_2,
            LoadCallback completeCallback = null, bool save = true, bool cache = true, bool loadImmediately = false);

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        LoadItem Load(LoadItem item);

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="items"></param>
        /// <param name="completeCallback"></param>
        /// <param name="progressCallback"></param>
        /// <param name="errorCallback"></param>
        /// <param name="dispatchEvent"></param>
        /// <returns></returns>
        LoadQueue Load(List<LoadItem> items, LoadQueueCallback completeCallback = null, LoadQueueCallback progressCallback = null,
            LoadQueueCallback errorCallback = null, bool dispatchEvent = true);

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        LoadQueue Load(LoadQueue queue);

        /// <summary>
        /// 停止加载
        /// </summary>
        /// <param name="url"></param>
        void StopLoad(string url);

        /// <summary>
        /// 停止加载
        /// </summary>
        /// <param name="item"></param>
        void StopLoad(LoadItem item);

        /// <summary>
        /// 移除加载回调
        /// </summary>
        /// <param name="url"></param>
        /// <param name="completeCallback"></param>
        /// <param name="progressCallback"></param>
        /// <param name="errorCallback"></param>
        void RemoveLoadCallback(string url, LoadCallback completeCallback = null, LoadCallback progressCallback = null, LoadCallback errorCallback = null);

        /// <summary>
        /// 移除加载回调
        /// </summary>
        /// <param name="item"></param>
        void RemoveLoadCallback(LoadItem item);

        /// <summary>
        /// 立即加载资源
        /// </summary>
        /// <param name="url"></param>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="completeCallback"></param>
        /// <param name="progressCallback"></param>
        /// <param name="errorCallback"></param>
        /// <param name="save"></param>
        /// <param name="cache"></param>
        /// <param name="loadImmediately"></param>
        /// <returns></returns>
        LoadItem LoadImmediately(string url, string id, LoadType type = LoadType.AUTO, LoadCallback completeCallback = null,
            LoadCallback progressCallback = null, LoadCallback errorCallback = null, bool save = true, bool cache = true, bool loadImmediately = false);

        /// <summary>
        /// 立即加载资源
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type"></param>
        /// <param name="completeCallback"></param>
        /// <param name="save"></param>
        /// <param name="cache"></param>
        /// <param name="loadImmediately"></param>
        /// <returns></returns>
        LoadItem LoadImmediately(string url, LoadType type = LoadType.AUTO, LoadCallback completeCallback = null, bool save = true, bool cache = true, bool loadImmediately = false);

        /// <summary>
        /// 立即加载资源
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        LoadItem LoadImmediately(LoadItem item);

        /// <summary>
        /// 立即加载资源
        /// </summary>
        /// <param name="items"></param>
        /// <param name="completeCallback"></param>
        /// <param name="progressCallback"></param>
        /// <param name="errorCallback"></param>
        /// <param name="dispatchEvent"></param>
        /// <returns></returns>
        LoadQueue LoadImmediately(List<LoadItem> items, LoadQueueCallback completeCallback = null, LoadQueueCallback progressCallback = null,
            LoadQueueCallback errorCallback = null, bool dispatchEvent = true);

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        LoadQueue LoadImmediately(LoadQueue queue);

        /// <summary>
        /// 立即停止资源
        /// </summary>
        /// <param name="item"></param>
        void StopLoadImmediately(string url);

        /// <summary>
        /// 立即停止资源
        /// </summary>
        /// <param name="item"></param>
        void StopLoadImmediately(LoadItem item);

        /// <summary>
        /// 移除加载回调
        /// </summary>
        /// <param name="url"></param>
        /// <param name="completeCallback"></param>
        /// <param name="progressCallback"></param>
        /// <param name="errorCallback"></param>
        void RemoveLoadCallbackImmediately(string url, LoadCallback completeCallback = null, LoadCallback progressCallback = null, LoadCallback errorCallback = null);

        /// <summary>
        /// 移除加载回调
        /// </summary>
        /// <param name="item"></param>
        void RemoveLoadCallbackImmediately(LoadItem item);

        /// <summary>
        /// 是否正在加载
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        bool IsLoading(string url);

        /// <summary>
        /// 是否正在加载
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool IsLoading(LoadItem item);

        /// <summary>
        /// 从原始数据重创建资源
        /// </summary>
        /// <param name="type"></param>
        /// <param name="url"></param>
        /// <param name="assetBundle"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        IAsset CreateAssetFromAssetBundle(LoadType type, string url, AssetBundle assetBundle, byte[] bytes = null);

        /// <summary>
        /// 从本地加载资源
        /// </summary>
        /// <param name="url"></param>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="cache"></param>
        /// <param name="autoClear"></param>
        /// <returns></returns>
        IAsset LoadAssetFromAssetBundle(string url, string id, LoadType type, bool cache = true, bool autoClear = true);
    }
}
