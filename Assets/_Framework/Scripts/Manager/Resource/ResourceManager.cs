using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class ResourceManager : Manager, IResourceManager
    {
        // 因为使用枚举或结构体作key时从字典中取值会产生gc，所以改为用int作key
        private Dictionary<int, List<LoadItem>> _loadListDict;
        private Dictionary<string, LoadItem> _loadDict;
        private Dictionary<string, ILoader> _currentLoaders;
        private Dictionary<string, LoadItem> _immediateDict;

        private int _maxLoaders;

        protected override void Init()
        {
            _loadListDict = new Dictionary<int, List<LoadItem>>();
            _loadDict = new Dictionary<string, LoadItem>();
            _currentLoaders = new Dictionary<string, ILoader>();
            _immediateDict = new Dictionary<string, LoadItem>();

            _maxLoaders = 2;

            for (int i = (int)LoadPriority.LV_FIRST; i <= (int)LoadPriority.LV_4; i++)
                _loadListDict.Add(i, new List<LoadItem>());
        }

        /// <summary>
        /// 最大加载数量
        /// </summary>
        public int maxLoaders
        {
            get { return _maxLoaders; }
            set { _maxLoaders = value; }
        }

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
        public LoadItem Load(string url, string id, LoadType type = LoadType.AUTO, LoadPriority priority = LoadPriority.LV_2,
            LoadCallback completeCallback = null, LoadCallback progressCallback = null, LoadCallback errorCallback = null, bool save = true, bool cache = true, bool loadImmediately = false)
        {
            // 创建加载项
            LoadItem item = new LoadItem(url, id, type, priority, save, cache);
            item.completeCallback = completeCallback;
            item.progressCallback = progressCallback;
            item.errorCallback = errorCallback;

            return Load(item);
        }

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
        public LoadItem Load(string url, LoadType type = LoadType.AUTO, LoadPriority priority = LoadPriority.LV_2,
            LoadCallback completeCallback = null, bool save = true, bool cache = true, bool loadImmediately = false)
        {
            return Load(url, url, type, priority, completeCallback, null, null, save, cache);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public LoadItem Load(LoadItem item)
        {
            if (item == null)
                return null;

            // 判断内存中是否存在该资源
            IAsset asset = App.assetManager.GetAsset(item.id);
            if (asset != null)
            {
                item.asset = asset;
                if (item.completeCallback != null)
                    item.completeCallback.Invoke(item);
                return item;
            }

            // 判断是否为ab包
            if (item.loadImmediately && item.isAssetBundle && item.type != LoadType.AUDIO_ASSET_BUNDLE)
            {
                asset = LoadAssetFromAssetBundle(item.url, item.id, item.type, item.cache);
                if (asset != null)
                {
                    item.asset = asset;
                    // 执行加载完成回调
                    if (item.completeCallback != null)
                        item.completeCallback.Invoke(item);
                    return item;
                }
            }

            // 判断内存中是否存在
            if (App.fileManager.FileExistsPersistent(item.url))
            {
                item.priority = LoadPriority.LV_FIRST;
            }

            // 内存中不存在则开始加载资源
            LoadItem loadItem;
            if (_loadDict.TryGetValue(item.url, out loadItem) == false)
            {
                // 加载队列中未存在该加载项
                _loadDict[item.url] = item;
                _loadListDict[(int)item.priority].Add(item);
            }
            else
            {
                // 加载队列中已存在该加载项
                if (item.priority != loadItem.priority)
                {
                    // 重新设置加载优先级
                    _loadListDict[(int)loadItem.priority].Remove(loadItem);
                    _loadListDict[(int)item.priority].Add(loadItem);
                    loadItem.priority = item.priority;
                }
                // 添加加载回调
                loadItem.completeCallback += item.completeCallback;
                loadItem.errorCallback += item.errorCallback;
                loadItem.progressCallback += item.progressCallback;
            }

            // 继续执行加载
            LoadNext();
            return loadItem;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="items"></param>
        /// <param name="completeCallback"></param>
        /// <param name="progressCallback"></param>
        /// <param name="errorCallback"></param>
        /// <param name="dispatchEvent"></param>
        /// <returns></returns>
        public LoadQueue Load(List<LoadItem> items, LoadQueueCallback completeCallback = null, LoadQueueCallback progressCallback = null,
            LoadQueueCallback errorCallback = null, bool dispatchEvent = true)
        {
            return Load(new LoadQueue(items, completeCallback, progressCallback, errorCallback, dispatchEvent));
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public LoadQueue Load(LoadQueue queue)
        {
            queue.Load();
            return queue;
        }

        /// <summary>
        /// 停止加载
        /// </summary>
        /// <param name="url"></param>
        public void StopLoad(string url)
        {
            LoadItem item;
            if (_loadDict.TryGetValue(url, out item))
                StopLoad(item);
        }

        /// <summary>
        /// 停止加载
        /// </summary>
        /// <param name="item"></param>
        public void StopLoad(LoadItem item)
        {
            // 移除加载字典
            if (_loadDict.ContainsKey(item.url))
                _loadDict.Remove(item.url);

            // 移除加载优先级字典
            _loadListDict[(int)item.priority].Remove(item);

            // 移除当前加载器字典
            ILoader loader;
            if (_currentLoaders.TryGetValue(item.url, out loader))
            {
                _currentLoaders.Remove(item.url);
                loader.Dispose();
            }
        }

        /// <summary>
        /// 移除加载回调
        /// </summary>
        /// <param name="url"></param>
        /// <param name="completeCallback"></param>
        /// <param name="progressCallback"></param>
        /// <param name="errorCallback"></param>
        public void RemoveLoadCallback(string url, LoadCallback completeCallback = null, LoadCallback progressCallback = null, LoadCallback errorCallback = null)
        {
            LoadItem item;
            if (_loadDict.TryGetValue(url, out item) == false)
                return;

            // 移除加载完成回调
            if (completeCallback != null)
                item.completeCallback -= completeCallback;

            // 移除加载进度回调
            if (progressCallback != null)
                item.progressCallback -= progressCallback;

            // 移除加载失败回调
            if (errorCallback != null)
                item.errorCallback -= errorCallback;

            // 如果加载项已经不存在加载回调，则关闭加载
            if (item.isLoading == false && item.completeCallback == null)
                StopLoad(item);
        }

        /// <summary>
        /// 移除加载回调
        /// </summary>
        /// <param name="item"></param>
        public void RemoveLoadCallback(LoadItem item)
        {
            RemoveLoadCallback(item.url, item.completeCallback, item.progressCallback, item.errorCallback);
        }

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
        public LoadItem LoadImmediately(string url, string id, LoadType type = LoadType.AUTO, LoadCallback completeCallback = null,
            LoadCallback progressCallback = null, LoadCallback errorCallback = null, bool save = true, bool cache = true, bool loadImmediately = false)
        {
            // 创建加载项
            LoadItem item = new LoadItem(url, id, type, LoadPriority.LV_0, save, cache);
            item.completeCallback = completeCallback;
            item.progressCallback = progressCallback;
            item.errorCallback = errorCallback;

            return LoadImmediately(item);
        }

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
        public LoadItem LoadImmediately(string url, LoadType type = LoadType.AUTO, LoadCallback completeCallback = null, bool save = true, bool cache = true, bool loadImmediately = false)
        {
            return LoadImmediately(url, url, type, completeCallback, null, null, save, cache);
        }

        /// <summary>
        /// 立即加载资源
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public LoadItem LoadImmediately(LoadItem item)
        {
            if (item == null)
                return null;

            // 判断内存中是否存在该资源
            IAsset asset = App.assetManager.GetAsset(item.id);
            if (asset != null)
            {
                item.asset = asset;
                if (item.completeCallback != null)
                    item.completeCallback.Invoke(item);
                return item;
            }

            // 判断是否为ab包
            if (item.loadImmediately && item.isAssetBundle && item.type != LoadType.AUDIO_ASSET_BUNDLE)
            {
                asset = LoadAssetFromAssetBundle(item.url, item.id, item.type, item.cache);
                if (asset != null)
                {
                    item.asset = asset;
                    // 执行加载完成回调
                    if (item.completeCallback != null)
                        item.completeCallback.Invoke(item);
                    return item;
                }
            }

            // 内存中不存在则开始加载资源
            LoadItem loadItem;
            if (_immediateDict.TryGetValue(item.url, out loadItem) == false)
            {
                // 加载队列中未存在该加载项
                _immediateDict[item.url] = item;
                // 设置加载项开始加载
                item.isLoading = true;
                // 创建加载器
                //string urlAbsolute;
                //// 判断是否从Persistent目录加载
                //if (App.fileManager.FileExistsPersistent(item.url))
                //    urlAbsolute = new StringBuilder(App.pathManager.persistentDataPathWWW).Append(item.url).ToString();
                //else
                //    urlAbsolute = new StringBuilder(App.pathManager.externalPath).Append(item.url).ToString();

                ILoader loader = LoaderFactory.CreateLoader(item.url, null, item.type);
                item.loader = loader;

                // 设置加载回调
                loader.completeCallback = LoadCompleteImmediately;
                loader.progressCallback = LoadProgressImmediately;
                loader.errorCallback = LoadErrorImmediately;

                // 开始加载
                loader.Start();
            }
            else
            {
                // 加载队列中已存在该加载项
                loadItem.completeCallback += item.completeCallback;
                loadItem.errorCallback += item.errorCallback;
                loadItem.progressCallback += item.progressCallback;
            }

            return loadItem;
        }

        /// <summary>
        /// 立即加载资源
        /// </summary>
        /// <param name="items"></param>
        /// <param name="completeCallback"></param>
        /// <param name="progressCallback"></param>
        /// <param name="errorCallback"></param>
        /// <param name="dispatchEvent"></param>
        /// <returns></returns>
        public LoadQueue LoadImmediately(List<LoadItem> items, LoadQueueCallback completeCallback = null, LoadQueueCallback progressCallback = null,
            LoadQueueCallback errorCallback = null, bool dispatchEvent = true)
        {
            return LoadImmediately(new LoadQueue(items, completeCallback, progressCallback, errorCallback, dispatchEvent));
        }

        /// <summary>
        /// 立即加载资源
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public LoadQueue LoadImmediately(LoadQueue queue)
        {
            queue.LoadImmediately();
            return queue;
        }

        /// <summary>
        /// 立即停止资源
        /// </summary>
        /// <param name="item"></param>
        public void StopLoadImmediately(string url)
        {
            LoadItem item;
            if (_immediateDict.TryGetValue(url, out item))
                StopLoadImmediately(item);
        }

        /// <summary>
        /// 立即停止资源
        /// </summary>
        /// <param name="item"></param>
        public void StopLoadImmediately(LoadItem item)
        {
            // 移除加载字典
            if (_immediateDict.ContainsKey(item.url))
                _immediateDict.Remove(item.url);

            if (item.loader != null)
                item.loader.Dispose();
        }

        /// <summary>
        /// 移除加载回调
        /// </summary>
        /// <param name="url"></param>
        /// <param name="completeCallback"></param>
        /// <param name="progressCallback"></param>
        /// <param name="errorCallback"></param>
        public void RemoveLoadCallbackImmediately(string url, LoadCallback completeCallback = null, LoadCallback progressCallback = null, LoadCallback errorCallback = null)
        {
            LoadItem item;
            if (_immediateDict.TryGetValue(url, out item) == false)
                return;

            // 移除加载完成回调
            if (completeCallback != null)
                item.completeCallback -= completeCallback;

            // 移除加载进度回调
            if (progressCallback != null)
                item.progressCallback -= progressCallback;

            // 移除加载失败回调
            if (errorCallback != null)
                item.errorCallback -= errorCallback;

            // 如果加载项已经不存在加载回调，则关闭加载
            if (item.isLoading == false && item.completeCallback == null)
                StopLoad(item);
        }

        /// <summary>
        /// 移除加载回调
        /// </summary>
        /// <param name="item"></param>
        public void RemoveLoadCallbackImmediately(LoadItem item)
        {
            RemoveLoadCallbackImmediately(item.url, item.completeCallback, item.progressCallback, item.errorCallback);
        }

        private void LoadNext()
        {
            // 判断是否已达到最大加载限制
            if (_currentLoaders.Count >= _maxLoaders)
                return;

            // 遍历加载列表
            for (int i = (int)LoadPriority.LV_FIRST; i <= (int)LoadPriority.LV_4; i++)
            {
                List<LoadItem> items = _loadListDict[i];
                while (items.Count > 0)
                {
                    // 取出加载队列中最早的加载项，执行加载
                    LoadItem item = items[0];
                    items.RemoveAt(0);

                    // 判断内存中是否已存在资源
                    IAsset asset = App.assetManager.GetAsset(item.id);

                    // 已存在资源则结束加载
                    if (asset != null)
                        EndLoad(item, asset);
                    else
                    {
                        // 开始加载
                        StartLoad(item);

                        // 判断是否已达到最大加载限制
                        if (_currentLoaders.Count >= _maxLoaders)
                            return;
                    }
                }
            }
        }

        private void StartLoad(LoadItem item)
        {
            // 设置加载项开始加载
            item.isLoading = true;

            // 创建加载器
            //string urlAbsolute;
            //// 判断是否从Persistent目录加载
            //if (App.fileManager.FileExistsPersistent(item.url))
            //    urlAbsolute = new StringBuilder(App.pathManager.persistentDataPathWWW).Append(item.url).ToString();
            //else
            //    urlAbsolute = new StringBuilder(App.pathManager.externalPath).Append(item.url).ToString();

            ILoader loader = LoaderFactory.CreateLoader(item.url, null, item.type);
            item.loader = loader;

            // 加入当前加载器字典
            _currentLoaders[item.url] = loader;

            // 设置加载回调
            loader.completeCallback = LoadComplete;
            loader.progressCallback = LoadProgress;
            loader.errorCallback = LoadError;

            // 开始加载
            loader.Start();

            // 派发事件
            DispatchEvent(ResourceManagerEventArgs.ITEM_START, item);
        }

        private void EndLoad(LoadItem item, IAsset asset)
        {
            if (item == null)
                return;

            // 移除加载字典
            if (_loadDict.ContainsKey(item.url))
                _loadDict.Remove(item.url);

            // 填充LoadItem
            item.asset = asset;

            // 执行加载完成回调
            if (item.completeCallback != null)
                item.completeCallback.Invoke(item);
            else if (item.cache == false)
                asset.Dispose();
        }

        private void LoadComplete(ILoader loader)
        {
            // 获取LoadItem
            LoadItem item;
            if (_loadDict.TryGetValue(loader.urlRelative, out item) == false)
                return;

            // 缓存Asset
            IAsset asset = loader.asset;
            if (asset != null)
            {
                // 设置资源唯一ID
                asset.id = item.id;
                // 判断是否要将Asset放入内存
                if (item.cache)
                    App.assetManager.AddAsset(item.id, asset);

                // 判断是否需要写入Persistent目录
                //if (item.save)
                //    App.fileManager.WriteFilePersistentAsync(loader.urlRelative, loader.bytes, false);
            }

            // 移除当前加载器字典
            if (_currentLoaders.ContainsKey(item.url))
                _currentLoaders.Remove(item.url);

            // 加载完毕
            EndLoad(item, asset);

            // 派发事件
            DispatchEvent(ResourceManagerEventArgs.ITEM_COMPLETE, item);

            // 销毁加载器
            loader.Dispose();

            // 继续执行加载
            LoadNext();
        }

        private void LoadProgress(ILoader loader)
        {
            // 获取LoadItem
            LoadItem item;
            if (_loadDict.TryGetValue(loader.urlRelative, out item) == false)
                return;

            // 设置加载进度
            item.progress = loader.stats.progress;

            // 执行加载进度回调
            if (item.progressCallback != null)
                item.progressCallback.Invoke(item);

            // 派发事件
            DispatchEvent(ResourceManagerEventArgs.ITEM_PROGRESS, item);
        }

        private void LoadError(ILoader loader)
        {
            if (loader.error != "timeOut")
            {
                App.logManager.Warn("ResourceManager.Load Error: urlAbsolute:\"" + loader.urlAbsolute + "\" urlRelative:\"" + loader.urlRelative + "\" Message:" + loader.error);
                if (loader.error == "loadContentIsNull")
                {
                    // 文件加载失败时删除文件
                    if (App.fileManager.FileExistsPersistent(loader.urlRelative))
                    {
                        App.fileManager.DeleteFilePersistent(loader.urlRelative);
                    }
                }
            }

            // 获取LoadItem
            LoadItem item;
            if (_loadDict.TryGetValue(loader.urlRelative, out item) == false)
            {
                App.logManager.Warn("ResourceManager.Load Error: LoadItem is null: urlAbsolute:\"" + loader.urlAbsolute + "\" urlRelative:\"" + loader.urlRelative);
                return;
            }

            // 设置失败信息
            item.error = loader.error;

            // 移除加载字典
            if (_loadDict.ContainsKey(item.url))
                _loadDict.Remove(item.url);

            // 移除当前加载器字典
            if (_currentLoaders.ContainsKey(item.url))
                _currentLoaders.Remove(item.url);

            // 执行加载失败回调
            if (item.errorCallback != null)
                item.errorCallback.Invoke(item);

            // 派发事件
            DispatchEvent(ResourceManagerEventArgs.ITEM_ERROR, item);

            // 销毁加载器
            loader.Dispose();

            // 继续执行加载
            LoadNext();
        }

        private void LoadCompleteImmediately(ILoader loader)
        {
            // 获取LoadItem
            LoadItem item;
            if (_immediateDict.TryGetValue(loader.urlRelative, out item) == false)
                return;

            // 缓存Asset
            IAsset asset = loader.asset;
            if (asset != null)
            {
                // 设置资源唯一ID
                asset.id = item.id;
                // 判断是否要将Asset放入内存
                if (item.cache)
                    App.assetManager.AddAsset(item.id, asset);

                // 判断是否需要写入Persistent目录
                //if (item.save)
                //    App.fileManager.WriteFilePersistentAsync(loader.urlRelative, loader.bytes, false);
            }

            // 填充LoadItem
            item.asset = asset;

            // 执行加载完成回调
            if (item.completeCallback != null)
                item.completeCallback.Invoke(item);

            // 移除当前加载列表
            _immediateDict.Remove(item.url);

            // 销毁加载器
            loader.Dispose();
        }

        private void LoadProgressImmediately(ILoader loader)
        {
            // 获取LoadItem
            LoadItem item;
            if (_immediateDict.TryGetValue(loader.urlRelative, out item) == false)
                return;

            // 设置加载进度
            item.progress = loader.stats.progress;

            // 执行加载进度回调
            if (item.progressCallback != null)
                item.progressCallback.Invoke(item);
        }

        private void LoadErrorImmediately(ILoader loader)
        {
            if (loader.error != "timeOut")
            {
                App.logManager.Warn("ResourceManager.LoadErrorImmediately: urlAbsolute:\"" + loader.urlAbsolute + "\" urlRelative:\"" + loader.urlRelative + "\" Message:" + loader.error);
            }

            // 获取LoadItem
            LoadItem item;
            if (_immediateDict.TryGetValue(loader.urlRelative, out item) == false)
            {
                App.logManager.Warn("ResourceManager.LoadErrorImmediately: LoadItem is null: urlAbsolute:\"" + loader.urlAbsolute + "\" urlRelative:\"" + loader.urlRelative);
                return;
            }

            // 设置失败信息
            item.error = loader.error;

            // 移除加载字典
            _immediateDict.Remove(item.url);

            // 执行加载失败回调
            if (item.errorCallback != null)
                item.errorCallback.Invoke(item);

            // 销毁加载器
            loader.Dispose();
        }

        /// <summary>
        /// 是否正在加载
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool IsLoading(string url)
        {
            return _loadDict.ContainsKey(url);
        }

        /// <summary>
        /// 是否正在加载
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsLoading(LoadItem item)
        {
            return _loadDict.ContainsKey(item.url);
        }

        private void DispatchEvent(string type, LoadItem item)
        {
            ResourceManagerEventArgs eventArgs = App.objectPoolManager.GetObject<ResourceManagerEventArgs>();
            eventArgs.type = type;
            eventArgs.loadItem = item;
            DispatchEvent(eventArgs);
            App.objectPoolManager.ReleaseObject(eventArgs);
        }

        /// <summary>
        /// 从原始数据重创建资源
        /// </summary>
        /// <param name="type"></param>
        /// <param name="url"></param>
        /// <param name="assetBundle"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public IAsset CreateAssetFromAssetBundle(LoadType type, string url, AssetBundle assetBundle, byte[] bytes = null)
        {
            IAsset asset;
            string fileName;

            switch (type)
            {
                case LoadType.ASSETBUNDLE:
                    asset = new AssetBundleAsset();
                    asset.asset = assetBundle;
                    break;
                case LoadType.TEXTURE_ASSET_BUNDLE:
                    asset = new TextureAsset();
                    fileName = UrlUtil.GetFileName(url);
                    asset.asset = assetBundle.LoadAsset<Texture2D>(fileName);
                    break;
                case LoadType.AUDIO_ASSET_BUNDLE:
                    asset = new AudioAsset();
                    fileName = UrlUtil.GetFileName(url);
                    asset.asset = assetBundle.LoadAsset<AudioClip>(fileName);
                    break;
                case LoadType.ACTION:
                    asset = new ActionAsset();
                    fileName = UrlUtil.GetFileName(url);
                    ActionData actionData = App.objectPoolManager.GetObject<ActionData>();
                    actionData.Init(fileName, assetBundle);
                    asset.asset = actionData;
                    break;
                case LoadType.TEXTURE_EXT:
                    asset = new TextureExtAsset();
                    fileName = UrlUtil.GetFileName(url);
                    TextureExt textureExt = App.objectPoolManager.GetObject<TextureExt>();
                    textureExt.Init(fileName, assetBundle);
                    asset.asset = textureExt;
                    break;
                case LoadType.TEXTURE_EXT_SPRITE:
                    asset = new TextureExtSpriteAsset();
                    fileName = UrlUtil.GetFileName(url);
                    TextureExtSprite textureExtSprite = App.objectPoolManager.GetObject<TextureExtSprite>();
                    textureExtSprite.Init(fileName, assetBundle);
                    asset.asset = textureExtSprite;
                    break;
                default:
                    asset = null;
                    break;
            }

            if (asset == null)
                return null;

            asset.assetBundle = assetBundle;
            //asset.bytes = bytes;
            return asset;
        }

        /// <summary>
        /// 从本地加载资源
        /// </summary>
        /// <param name="url"></param>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="cache"></param>
        /// <param name="autoClear"></param>
        /// <returns></returns>
        public IAsset LoadAssetFromAssetBundle(string url, string id, LoadType type, bool cache = true, bool autoClear = true)
        {
            if (!App.fileManager.FileExistsPersistent(url))
                return null;

            string path = new StringBuilder(App.pathManager.persistentDataPathFile).Append(url).ToString();
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
            if (assetBundle == null)
                return null;

            IAsset asset = CreateAssetFromAssetBundle(type, url, assetBundle);
            if (asset == null)
                return null;

            asset.id = id;
            asset.autoClear = autoClear;
            if (cache)
                App.assetManager.AddAsset(id, asset);
            //assetBundle.Unload(false);
            return asset;
        }
    }
}
