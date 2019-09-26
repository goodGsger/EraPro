using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Framework
{
    public abstract class AbstractLoader : ILoader
    {
        public static float loadTimeOut = 5f;

        protected string _urlRelative;
        protected string _urlAbsolute;
        protected LoadType _type;
        protected Dictionary<string, object> _loadParams;
        protected LoadStats _stats;
        protected LoadState _state;
        protected object _data;
        protected IAsset _asset;
        protected string _error;
        protected int _retryCount;
        protected float _lastLoadTime;
        protected CoroutineTask _task;
        protected UnityWebRequest _request;

        protected Action<ILoader> _startCallback;
        protected Action<ILoader> _stopCallback;
        protected Action<ILoader> _completeCallback;
        protected Action<ILoader> _errorCallback;
        protected Action<ILoader> _progressCallback;

        public AbstractLoader()
        {
            _loadParams = new Dictionary<string, object>();
            _stats = new LoadStats();
            _state = LoadState.STOPED;

            InitParams();
        }

        /// <summary>
        /// 相对路径
        /// </summary>
        public virtual string urlRelative
        {
            get { return _urlRelative; }
            set { _urlRelative = value; }
        }

        /// <summary>
        /// 绝对路径
        /// </summary>
        public virtual string urlAbsolute
        {
            get { return _urlAbsolute; }
        }

        /// <summary>
        /// 下载类型
        /// </summary>
        public virtual LoadType type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// 参数
        /// </summary>
        public virtual Dictionary<string, object> loadParams
        {
            get { return _loadParams; }
            set { _loadParams = value; }
        }

        /// <summary>
        /// 统计
        /// </summary>
        public virtual LoadStats stats
        {
            get { return _stats; }
            set { _stats = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual LoadState state
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 数据
        /// </summary>
        public virtual object data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public virtual IAsset asset
        {
            get { return _asset; }
            set { _asset = value; }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public virtual string error
        {
            get { return _error; }
            set { _error = value; }
        }

        protected virtual void InitParams()
        {
            SetParam(LoadParam.RETRY_COUNT, 2);
        }

        /// <summary>
        /// 开始加载
        /// </summary>
        public virtual void Start()
        {
            if (_state != LoadState.STOPED)
                Stop();

            _state = LoadState.STARTED;
            _stats.Start();

            if (_task == null)
                _task = App.objectPoolManager.GetObject<CoroutineTask>();

            _task.routine = InvokeLoading();
            _task.Start();

            if (_startCallback != null)
                _startCallback.Invoke(this);
        }

        /// <summary>
        /// 开始加载
        /// </summary>
        public virtual void Start(string url)
        {
            _urlRelative = url;
            Start();
        }

        /// <summary>
        /// 重加载
        /// </summary>
        protected virtual void RetryStart()
        {
            _state = LoadState.STARTED;
            _stats.Start();
            _task.Start();
        }

        protected virtual IEnumerator InvokeLoading()
        {
            byte[] bytes = null;
            if (!App.fileManager.FileExistsPersistent(_urlRelative))
            {
                _urlAbsolute = new StringBuilder(App.pathManager.externalPath).Append(_urlRelative).ToString();
                _request = UnityWebRequest.Get(_urlAbsolute);
                _request.SendWebRequest();

                bool timeOut = false;
                _lastLoadTime = Time.realtimeSinceStartup;
                while (_request.isDone == false)
                {
                    if (_stats.progress == _request.downloadProgress)
                    {
                        if (Time.realtimeSinceStartup - _lastLoadTime >= loadTimeOut)
                        {
                            timeOut = true;
                            break;
                        }
                    }
                    else
                        _lastLoadTime = Time.realtimeSinceStartup;
                    UpdateProgress(_request.downloadProgress);
                    yield return null;
                }

                if (_request.error != null || (_request.responseCode != 0 && _request.responseCode != 200) || timeOut)
                {
                    if (Retry())
                        yield break;
                    _error = _request.error;
                    if (timeOut)
                        _error = "timeOut";
                    if (_error == null)
                        _error = "responseCode:" + _request.responseCode;
                    if (_errorCallback != null)
                        _errorCallback.Invoke(this);
                    yield return null;
                }
                else
                {
                    bytes = _request.downloadHandler.data;
                    InvokeSaveFile(bytes);
                    yield return InvokeLoadComplete(bytes);
                }
            }
            else
            {
                yield return InvokeLoadComplete(bytes);
            }
        }

        protected abstract void InvokeSaveFile(byte[] bytes);

        protected abstract IEnumerator InvokeLoadComplete(byte[] bytes);

        /// <summary>
        /// 尝试重新加载
        /// </summary>
        /// <returns></returns>
        protected virtual bool Retry()
        {
            if (_loadParams.ContainsKey(LoadParam.RETRY_COUNT) == false)
                return false;

            int maxRetryCount = (int)_loadParams[LoadParam.RETRY_COUNT];
            if (_retryCount < maxRetryCount)
            {
                _retryCount++;

                if (_request != null)
                {
                    _request.Dispose();
                    _request = null;
                }
                _task.Stop();
                _stats.Reset();
                _state = LoadState.STARTED;
                _stats.Start();
                _task.routine = InvokeLoading();
                _task.Start();
                return true;
            }
            else
                return false;
        }

        protected virtual void InvokeStopLoading()
        {
            if (_stopCallback != null)
                _stopCallback.Invoke(this);
        }

        protected virtual void UpdateProgress(float progress)
        {
            if (_stats.progress == progress)
                return;

            _stats.Update(progress);
            if (_progressCallback != null)
                _progressCallback.Invoke(this);
        }

        /// <summary>
        /// 停止加载
        /// </summary>
        public virtual void Stop()
        {
            _stats.Reset();
            _state = LoadState.STOPED;

            if (_task != null)
                _task.Stop();

            if (_request != null)
            {
                _request.Abort();
                _request.Dispose();
                _request = null;
            }

            if (_stopCallback != null)
                _stopCallback.Invoke(this);
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Dispose()
        {
            Stop();

            onDispose();

            if (_task != null)
            {
                App.objectPoolManager.ReleaseObject(_task);
                _task = null;
            }

            _loadParams = null;
            _stats = null;
            _state = LoadState.STOPED;
            _data = null;
            _asset = null;

            _startCallback = null;
            _stopCallback = null;
            _completeCallback = null;
            _errorCallback = null;
            _progressCallback = null;
        }

        protected virtual void onDispose()
        {

        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="param"></param>
        public virtual void SetParam(string id, object param)
        {
            _loadParams[id] = param;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual object GetParam(string id)
        {
            if (_loadParams.ContainsKey(id))
                return _loadParams[id];

            return null;
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="param"></param>
        public virtual void AddParam(LoadParam param)
        {
            SetParam(param.id, param.value);
        }

        /// <summary>
        /// 从对象池中获取
        /// </summary>
        public virtual void OnPoolGet()
        {

        }

        /// <summary>
        /// 重置对象池对象
        /// </summary>
        public virtual void OnPoolReset()
        {
            Stop();
            _loadParams.Clear();
            _state = LoadState.STOPED;
            _data = null;
            _asset = null;
            _error = null;
            _retryCount = 0;
            _lastLoadTime = 0f;

            _startCallback = null;
            _stopCallback = null;
            _completeCallback = null;
            _errorCallback = null;
            _progressCallback = null;
        }

        /// <summary>
        /// 销毁对象池对象
        /// </summary>
        public virtual void OnPoolDispose()
        {
            Dispose();
        }

        /// <summary>
        /// 开始回调
        /// </summary>
        public virtual Action<ILoader> startCallback
        {
            get { return _startCallback; }
            set { _startCallback = value; }
        }

        /// <summary>
        /// 停止回调
        /// </summary>
        public virtual Action<ILoader> stopCallback
        {
            get { return _stopCallback; }
            set { _stopCallback = value; }
        }

        /// <summary>
        /// 成功回调
        /// </summary>
        public virtual Action<ILoader> completeCallback
        {
            get { return _completeCallback; }
            set { _completeCallback = value; }
        }

        /// <summary>
        /// 失败回调
        /// </summary>
        public virtual Action<ILoader> errorCallback
        {
            get { return _errorCallback; }
            set { _errorCallback = value; }
        }

        /// <summary>
        /// 更新进度回调
        /// </summary>
        public virtual Action<ILoader> progressCallback
        {
            get { return _progressCallback; }
            set { _progressCallback = value; }
        }
    }
}
