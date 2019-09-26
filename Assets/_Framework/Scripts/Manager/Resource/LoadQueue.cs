using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class LoadQueue
    {
        private List<LoadItem> _items;
        private List<LoadItem> _needLoadItems;
        private bool _dispatchEvent;

        private float _progress;
        private string _error;
        private LoadItem _currentItem;

        private LoadQueueCallback _completeCallback;
        private LoadQueueCallback _progressCallback;
        private LoadQueueCallback _errorCallback;

        public LoadQueue(List<LoadItem> items, LoadQueueCallback completeCallback = null, LoadQueueCallback progressCallback = null, LoadQueueCallback errorCallback = null, bool dispatchEvent = true)
        {
            _items = items;
            _completeCallback = completeCallback;
            _progressCallback = progressCallback;
            _errorCallback = errorCallback;
            _dispatchEvent = dispatchEvent;
        }

        /// <summary>
        /// 加载项
        /// </summary>
        public List<LoadItem> items
        {
            get { return _items; }
            set { _items = value; }
        }

        /// <summary>
        /// 加载进度
        /// </summary>
        public float progress
        {
            get { return _progress; }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string error
        {
            get { return _error; }
        }

        /// <summary>
        /// 当前加载项
        /// </summary>
        public LoadItem currentItem
        {
            get { return _currentItem; }
        }

        /// <summary>
        /// 加载完成回调
        /// </summary>
        public LoadQueueCallback completeCallback
        {
            get { return _completeCallback; }
            set { _completeCallback = value; }
        }

        /// <summary>
        /// 加载进度回调
        /// </summary>
        public LoadQueueCallback progressCallback
        {
            get { return _progressCallback; }
            set { _progressCallback = value; }
        }

        /// <summary>
        /// 加载失败回调
        /// </summary>
        public LoadQueueCallback errorCallback
        {
            get { return _errorCallback; }
            set { _errorCallback = value; }
        }

        public void Load()
        {
            if (_items == null || _items.Count == 0)
            {
                if (_completeCallback != null)
                    _completeCallback.Invoke(this);

                return;
            }

            _needLoadItems = _items.GetRange(0, _items.Count);

            // 是否派发事件
            if (_dispatchEvent)
                DispatchEvent(ResourceManagerEventArgs.QUEUE_START, this);

            // 依次加载子项
            foreach (LoadItem item in _items)
            {
                item.completeCallback += OnItemComplete;
                item.progressCallback += OnItemProgress;
                item.errorCallback += OnItemError;
                App.resourceManager.Load(item);
            }
        }

        public void LoadImmediately()
        {
            if (_items == null || _items.Count == 0)
            {
                if (_completeCallback != null)
                    _completeCallback.Invoke(this);

                return;
            }

            _needLoadItems = _items.GetRange(0, _items.Count);

            // 是否派发事件
            if (_dispatchEvent)
                DispatchEvent(ResourceManagerEventArgs.QUEUE_START, this);

            // 依次加载子项
            foreach (LoadItem item in _items)
            {
                item.completeCallback += OnItemComplete;
                item.progressCallback += OnItemProgress;
                item.errorCallback += OnItemError;
                App.resourceManager.LoadImmediately(item);
            }
        }

        private void OnItemComplete(LoadItem item)
        {
            RemoveLoadedItem(item);
            if (_needLoadItems.Count == 0)
            {
                if (_completeCallback != null)
                    _completeCallback.Invoke(this);

                if (_dispatchEvent)
                    DispatchEvent(ResourceManagerEventArgs.QUEUE_COMPLETE, this);
            }
        }

        private void OnItemProgress(LoadItem item)
        {
            _currentItem = item;

            SetLoadItemProgress(item);

            float totalProgress = 0f;
            foreach (LoadItem loadItem in _items)
                totalProgress += loadItem.progress;
            _progress = totalProgress / _items.Count;

            if (_progressCallback != null)
                _progressCallback.Invoke(this);

            if (_dispatchEvent)
                DispatchEvent(ResourceManagerEventArgs.QUEUE_PROGRESS, this);
        }

        private void OnItemError(LoadItem item)
        {
            _error = item.error;

            if (_errorCallback != null)
                _errorCallback.Invoke(this);

            if (_dispatchEvent)
                DispatchEvent(ResourceManagerEventArgs.QUEUE_ERROR, this);
        }

        private void SetLoadItemProgress(LoadItem item)
        {
            LoadItem loadItem;
            for (int i = 0; i < _needLoadItems.Count; i++)
            {
                loadItem = _needLoadItems[i];
                if (loadItem.url == item.url)
                    loadItem.progress = item.progress;
            }
        }

        private void RemoveLoadedItem(LoadItem item)
        {
            for (int i = _needLoadItems.Count - 1; i >= 0; i--)
                if (_needLoadItems[i].url == item.url)
                    _needLoadItems.RemoveAt(i);
        }

        private void DispatchEvent(string type, LoadQueue queue)
        {
            ResourceManagerEventArgs eventArgs = App.objectPoolManager.GetObject<ResourceManagerEventArgs>();
            eventArgs.type = type;
            eventArgs.loadQueue = queue;
            App.resourceManager.DispatchEvent(eventArgs);
            App.objectPoolManager.ReleaseObject(eventArgs);
        }
    }
}
