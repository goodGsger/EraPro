using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class AudioManager : Manager, IAudioManager
    {
        private Dictionary<string, AudioItem> _idDict;
        private Dictionary<string, Dictionary<string, AudioItem>> _ownerDict;

        private bool _effectEnabled = true;
        private bool _musicEnabled = true;

        private float _effectVolume = 1f;
        private float _musicVolume = 1f;

        /// <summary>
        /// 音效是否启用
        /// </summary>
        public bool effectEnabled
        {
            get { return _effectEnabled; }
            set { _effectEnabled = value; }
        }

        /// <summary>
        /// 音乐是否启用
        /// </summary>
        public bool musicEnabled
        {
            get { return _musicEnabled; }
            set { _musicEnabled = value; }
        }

        /// <summary>
        /// 特效音量
        /// </summary>
        public float effectVolume
        {
            get { return _effectVolume; }
            set { _effectVolume = value; }
        }

        /// <summary>
        /// 音乐音量
        /// </summary>
        public float musicVolume
        {
            get { return _musicVolume; }
            set { _musicVolume = value; }
        }

        protected override void Init()
        {
            _idDict = new Dictionary<string, AudioItem>();
            _ownerDict = new Dictionary<string, Dictionary<string, AudioItem>>();
        }

        /// <summary>
        /// 创建音频
        /// </summary>
        /// <param name="type"></param>
        /// <param name="url"></param>
        /// <param name="id"></param>
        /// <param name="owner"></param>
        /// <param name="volume"></param>
        /// <param name="loop"></param>
        /// <param name="playWhenLoaded"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private AudioItem CreateAudioItem(AudioItemType type, string url, string id = null, string owner = null,
            float volume = 1f, bool loop = false, bool playWhenLoaded = false, Action<AudioItem> callback = null)
        {
            AudioItem item = new AudioItem();
            item.type = type;
            item.id = id;
            item.owner = owner;
            item.callback = callback;
            AudioPlayer player = App.objectPoolManager.GetObject<AudioPlayer>();
            player.url = url;
            player.volume = volume;
            player.loop = loop;
            player.playWhenLoaded = playWhenLoaded;
            player.audioSource = behaviour.gameObject.AddComponent<AudioSource>();
            item.player = player;
            return item;
        }

        /// <summary>
        /// 释放音频
        /// </summary>
        /// <param name="item"></param>
        private void ReleaseAudioItem(AudioItem item)
        {
            // 移除id字典
            if (item.id != null)
                if (_idDict.TryGetValue(item.id, out item))
                    _idDict.Remove(item.id);

            // 移除拥有者字典
            if (item.owner != null)
            {
                Dictionary<string, AudioItem> dict;
                if (_ownerDict.TryGetValue(item.owner, out dict))
                    if (dict.ContainsKey(item.player.url))
                        dict.Remove(item.player.url);
            }

            App.objectPoolManager.ReleaseObject(item.player);
            item.player = null;
            item.callback = null;
        }

        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private AudioItem Play(AudioItem item)
        {
            // 存入id字典
            if (item.id != null)
            {
                if (_idDict.ContainsKey(item.id))
                    Stop(item);
                _idDict[item.id] = item;
            }

            // 存入拥有者字典
            if (item.owner != null)
            {
                Dictionary<string, AudioItem> dict;
                if (_ownerDict.TryGetValue(item.owner, out dict) == false)
                    dict = _ownerDict[item.owner] = new Dictionary<string, AudioItem>();
                dict[item.player.url] = item;
            }

            item.player.completeCallback = (player) =>
            {
                if (item.callback != null)
                    item.callback.Invoke(item);
                ReleaseAudioItem(item);
            };
            item.player.Play();

            return item;
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        /// <param name="item"></param>
        public void Stop(AudioItem item)
        {
            ReleaseAudioItem(item);
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        /// <param name="id"></param>
        public void Stop(string id)
        {
            AudioItem item;
            if (_idDict.TryGetValue(id, out item))
                ReleaseAudioItem(item);
        }

        /// <summary>
        /// 判断拥有者是否正在播放指定音频
        /// </summary>
        /// <param name="url"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public bool HasAudiosByOwner(string url, string owner)
        {
            Dictionary<string, AudioItem> dict;
            if (_ownerDict.TryGetValue(owner, out dict) == false)
                return false;

            return dict.ContainsKey(url);
        }

        /// <summary>
        /// 判断拥有者是否正在播放音频
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool HasAudiosByGroup(string group)
        {
            Dictionary<string, AudioItem> dict;
            if (_ownerDict.TryGetValue(group, out dict) == false)
                return false;

            return dict.Count > 0;
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="url"></param>
        /// <param name="id"></param>
        /// <param name="owner"></param>
        /// <param name="volume"></param>
        /// <param name="playWhenLoaded"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public AudioItem PlayEffect(string url, string id, string owner, float volume, bool playWhenLoaded, Action<AudioItem> callback)
        {
            if (_effectEnabled == false)
                return null;

            // 判断是否已经正在播放
            if (owner != null && HasAudiosByOwner(url, owner))
                return null;

            AudioItem item = CreateAudioItem(AudioItemType.EFFECT, url, id, owner, _effectVolume * volume, false, playWhenLoaded, callback);
            return Play(item);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public AudioItem PlayEffect(string url)
        {
            if (_effectEnabled == false)
                return null;

            AudioItem item = CreateAudioItem(AudioItemType.EFFECT, url, null, null, _effectVolume);
            return Play(item);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="url"></param>
        /// <param name="owner"></param>
        /// <param name="volume"></param>
        /// <param name="playWhenLoaded"></param>
        /// <returns></returns>
        public AudioItem PlayEffect(string url, string owner, float volume, bool playWhenLoaded)
        {
            if (_effectEnabled == false)
                return null;

            AudioItem item = CreateAudioItem(AudioItemType.EFFECT, url, null, owner, _effectVolume * volume, false, playWhenLoaded);
            return Play(item);
        }

        /// <summary>
        /// 停止播放音频
        /// </summary>
        /// <param name="id"></param>
        public void StopEffect(string id)
        {
            Stop(id);
        }

        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="url"></param>
        /// <param name="id"></param>
        /// <param name="loop"></param>
        /// <param name="volume"></param>
        /// <param name="playWhenLoaded"></param>
        /// <param name="forcePlay"></param>
        /// <returns></returns>
        public AudioItem PlayMusic(string url, string id, bool loop, float volume, bool playWhenLoaded, bool forcePlay = false)
        {
            if (forcePlay == false && _idDict.ContainsKey(id))
                return _idDict[id];

            if (_musicEnabled == false)
                return null;

            AudioItem item = CreateAudioItem(AudioItemType.MUSIC, url, id, null, _musicVolume * volume, loop, playWhenLoaded);
            return Play(item);
        }

        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public AudioItem PlayMusic(string url)
        {
            if (_musicEnabled == false)
                return null;

            AudioItem item = CreateAudioItem(AudioItemType.MUSIC, url, url, null, _musicVolume, true, true);
            return Play(item);
        }

        /// <summary>
        /// 停止播放音乐
        /// </summary>
        /// <param name="id"></param>
        public void StopMusic(string id)
        {
            Stop(id);
        }

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="id"></param>
        /// <param name="volume"></param>
        public void SetVolume(string id, float volume)
        {
            AudioItem item;
            if (_idDict.TryGetValue(id, out item))
                item.player.volume = volume;
        }

        /// <summary>
        /// 设置音效音量
        /// </summary>
        /// <param name="volume"></param>
        public void SetEffectVolume(float volume)
        {
            foreach (var v in _idDict)
            {
                AudioItem item = v.Value;
                if (item.type == AudioItemType.EFFECT)
                    item.player.volume = volume;
            }
        }

        /// <summary>
        /// 设置音乐音量
        /// </summary>
        /// <param name="volume"></param>
        public void SetMusicVolume(float volume)
        {
            foreach (var v in _idDict)
            {
                AudioItem item = v.Value;
                if (item.type == AudioItemType.MUSIC)
                    item.player.volume = volume;
            }
        }
    }
}
