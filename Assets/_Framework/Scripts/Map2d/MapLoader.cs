using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    internal class MapLoader : IPooledObject
    {
        private MapLoaderItem _loaderItem;
        private Action<MapLoader> _completeCallback;
        private Action<MapLoader> _errorCallback;

        private ILoader _loader;

        public MapLoader()
        {
            if (MapSetting.assetBundleMode)
                _loader = new TextureAssetBundleLoader();
            else
                _loader = new TextureLoader();
            _loader.completeCallback = OnCompleteCallback;
            _loader.errorCallback = OnErrorCallback;
        }

        /// <summary>
        /// 加载item
        /// </summary>
        public MapLoaderItem loaderItem
        {
            get { return _loaderItem; }
            set { _loaderItem = value; }
        }

        /// <summary>
        /// 加载完成回调
        /// </summary>
        public Action<MapLoader> completeCallback
        {
            get { return _completeCallback; }
            set { _completeCallback = value; }
        }

        /// <summary>
        /// 加载失败回调
        /// </summary>
        public Action<MapLoader> errorCallback
        {
            get { return _errorCallback; }
            set { _errorCallback = value; }
        }

        /// <summary>
        /// 资源加载器
        /// </summary>
        public ILoader loader
        {
            get { return _loader; }
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="urlRelative"></param>
        public void StartLoad(string urlRelative)
        {
            //string urlAbsolute;
            //// 判断是否从Persistent目录加载
            //if (App.fileManager.FileExistsPersistent(urlRelative))
            //    urlAbsolute = new StringBuilder(App.pathManager.persistentDataPathWWW).Append(urlRelative).ToString();
            //else
            //    urlAbsolute = new StringBuilder(App.pathManager.externalPath).Append(urlRelative).ToString();

            _loader.urlRelative = urlRelative;
            //_loader.urlAbsolute = urlAbsolute;
            _loader.Start();
        }

        /// <summary>
        /// 停止加载
        /// </summary>
        public void StopLoad()
        {
            _loader.Stop();
        }

        private void OnCompleteCallback(ILoader loader)
        {
            if (_completeCallback != null)
                _completeCallback.Invoke(this);
        }

        private void OnErrorCallback(ILoader loader)
        {
            if (_errorCallback != null)
                _errorCallback.Invoke(this);
        }

        /// <summary>
        /// 从对象池中获取
        /// </summary>
        public void OnPoolGet()
        {

        }

        /// <summary>
        /// 重置对象池对象
        /// </summary>
        public void OnPoolReset()
        {
            _completeCallback = null;
            _errorCallback = null;
            //_loader.Stop();
        }

        /// <summary>
        /// 销毁对象池对象
        /// </summary>
        public void OnPoolDispose()
        {
            _completeCallback = null;
            _errorCallback = null;
            _loader.Dispose();
            _loader = null;
        }
    }
}
