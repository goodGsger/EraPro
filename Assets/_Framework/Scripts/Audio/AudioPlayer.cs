using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class AudioPlayer : IPooledObject
    {
        private AudioAsset _audioAsset;
        private AudioSource _audioSource;
        private AudioState _state;

        private string _url;
        private float _volume = 1f;
        private float _spatialBlend;
        private float _time;
        private bool _loop;
        private bool _playWhenLoaded;
        private Action<AudioPlayer> _completeCallback;

        private CoroutineTask _task;

        public AudioPlayer()
        {

        }

        /// <summary>
        /// 音频资源
        /// </summary>
        public AudioAsset audioAsset
        {
            get { return _audioAsset; }
            set
            {
                // 清除原资源
                if (_audioAsset != null)
                    _audioAsset.Unuse();

                _audioAsset = value;

                if (_audioAsset != null)
                    _audioAsset.Use();

                if (_audioSource != null)
                {
                    if (_audioAsset != null)
                        _audioSource.clip = _audioAsset.audioClip;
                    else
                        _audioSource.clip = null;
                }
            }
        }

        /// <summary>
        /// 音频源
        /// </summary>
        public AudioSource audioSource
        {
            get { return _audioSource; }
            set
            {
                _audioSource = value;
                InitAudioSource();
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public AudioState state
        {
            get { return _state; }
        }

        /// <summary>
        /// 音频地址
        /// </summary>
        public string url
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// 音量
        /// </summary>
        public float volume
        {
            get { return _volume; }
            set
            {
                _volume = value;

                if (_audioSource != null)
                    _audioSource.volume = value;
            }
        }

        /// <summary>
        /// spatialBlend
        /// </summary>
        public float spatialBlend
        {
            get { return _spatialBlend; }
            set
            {
                _spatialBlend = value;

                if (_audioSource != null)
                    _audioSource.spatialBlend = value;
            }
        }

        /// <summary>
        /// 起始播放时间
        /// </summary>
        public float time
        {
            get { return _time; }
            set
            {
                _time = value;

                if (_audioSource != null)
                    _audioSource.time = value;
            }
        }

        /// <summary>
        /// 是否循环
        /// </summary>
        public bool loop
        {
            get { return _loop; }
            set
            {
                _loop = value;

                if (_audioSource != null)
                    _audioSource.loop = value;
            }
        }

        /// <summary>
        /// 是否加载完成后立即播放
        /// </summary>
        public bool playWhenLoaded
        {
            get { return _playWhenLoaded; }
            set { _playWhenLoaded = value; }
        }

        /// <summary>
        /// 播放完成回调
        /// </summary>
        public Action<AudioPlayer> completeCallback
        {
            get { return _completeCallback; }
            set { _completeCallback = value; }
        }

        private void InitAudioSource()
        {
            if (_audioAsset != null)
                _audioSource.clip = _audioAsset.audioClip;
            _audioSource.volume = _volume;
            _audioSource.spatialBlend = _spatialBlend;
            _audioSource.time = _time;
            _audioSource.loop = _loop;
        }

        private void Load()
        {
            App.resourceManager.Load(_url, _url, LoadType.AUDIO_ASSET_BUNDLE, LoadPriority.LV_4, LoadCompleteCallback, null, LoadErrorCallback);
            //App.resourceManager.LoadImmediately(_url, _url, LoadType.AUDIO_ASSET_BUNDLE, LoadCompleteCallback, null, LoadErrorCallback);
        }

        private void StopLoad()
        {
            App.resourceManager.RemoveLoadCallback(_url, LoadCompleteCallback, null, LoadErrorCallback);
            //App.resourceManager.RemoveLoadCallbackImmediately(_url, LoadCompleteCallback, null, LoadErrorCallback);
        }

        private void LoadCompleteCallback(LoadItem item)
        {
            audioAsset = item.asset as AudioAsset;
            _state = AudioState.STOP;

            // 加载完毕判断是否需要播放
            if (_playWhenLoaded)
                Play();
            else if (_loop == false && _completeCallback != null)
                _completeCallback.Invoke(this);
        }

        private void LoadErrorCallback(LoadItem item)
        {
            _state = AudioState.UNLOAD;
        }

        /// <summary>
        /// 播放
        /// </summary>
        public void Play()
        {
            // 音频资源不存在则加载资源
            if (_audioAsset == null)
            {
                AudioAsset asset = App.assetManager.GetAsset<AudioAsset>(_url);
                if (asset != null)
                    audioAsset = asset;
                if (_audioAsset == null)
                {
                    if (_state == AudioState.LOADING)
                        return;

                    // 音频资源未正在加载则开始加载资源
                    _state = AudioState.LOADING;
                    Load();
                    return;
                }
            }

            // 音频资源存在，开启协程播放音频
            _state = AudioState.PLAY;
            if (_task == null)
                _task = App.objectPoolManager.GetObject<CoroutineTask>();

            _task.routine = StartPlay();
            _task.Start();
        }

        private IEnumerator StartPlay()
        {
            // 开始播放音频
            _audioSource.Play();
            yield return new WaitForSeconds(_audioSource.clip.length);
            // 音频播放完毕执行回调
            if (_loop == false && _completeCallback != null)
                _completeCallback.Invoke(this);
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            // 正在加载中则停止加载
            if (_state == AudioState.LOADING)
                StopLoad();

            // 正在播放中则停止协程
            if (_task != null)
                _task.Stop();

            // 正在播放中则停止播放
            _state = AudioState.STOP;
            _audioSource.Stop();
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            _state = AudioState.PAUSE;
            _audioSource.Pause();
        }

        /// <summary>
        /// 继续
        /// </summary>
        public void UnPanuse()
        {
            _state = AudioState.PLAY;
            _audioSource.UnPause();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            Stop();

            // 销毁协程
            if (_task != null)
            {
                App.objectPoolManager.ReleaseObject(_task);
                _task = null;
            }

            audioAsset = null;

            // 销毁音频源
            if (_audioSource != null)
            {
                _audioSource.clip = null;
                UnityEngine.Object.DestroyImmediate(_audioSource, true);
                _audioSource = null;
            }

            _completeCallback = null;

            _state = AudioState.UNLOAD;
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
            Dispose();
        }

        /// <summary>
        /// 销毁对象池对象
        /// </summary>
        public void OnPoolDispose()
        {
            Dispose();
        }
    }
}
