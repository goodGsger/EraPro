using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public interface IAudioManager : IManager
    {
        /// <summary>
        /// 音效是否启用
        /// </summary>
        bool effectEnabled { get; set; }

        /// <summary>
        /// 音乐是否启用
        /// </summary>
        bool musicEnabled { get; set; }

        /// <summary>
        /// 特效音量
        /// </summary>
        float effectVolume { get; set; }

        /// <summary>
        /// 音乐音量
        /// </summary>
        float musicVolume { get; set; }

        /// <summary>
        /// 停止播放
        /// </summary>
        /// <param name="item"></param>
        void Stop(AudioItem item);

        /// <summary>
        /// 停止播放
        /// </summary>
        /// <param name="id"></param>
        void Stop(string id);

        /// <summary>
        /// 判断拥有者是否正在播放指定音频
        /// </summary>
        /// <param name="url"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        bool HasAudiosByOwner(string url, string owner);

        /// <summary>
        /// 判断拥有者是否正在播放音频
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        bool HasAudiosByGroup(string group);

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
        AudioItem PlayEffect(string url, string id, string owner, float volume, bool playWhenLoaded, Action<AudioItem> callback);

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        AudioItem PlayEffect(string url);

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="url"></param>
        /// <param name="owner"></param>
        /// <param name="volume"></param>
        /// <param name="playWhenLoaded"></param>
        /// <returns></returns>
        AudioItem PlayEffect(string url, string owner, float volume, bool playWhenLoaded);

        /// <summary>
        /// 停止播放音频
        /// </summary>
        /// <param name="id"></param>
        void StopEffect(string id);

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
        AudioItem PlayMusic(string url, string id, bool loop, float volume, bool playWhenLoaded, bool forcePlay);

        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        AudioItem PlayMusic(string url);

        /// <summary>
        /// 停止播放音乐
        /// </summary>
        /// <param name="id"></param>
        void StopMusic(string id);

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="id"></param>
        /// <param name="volume"></param>
        void SetVolume(string id, float volume);

        /// <summary>
        /// 设置音效音量
        /// </summary>
        /// <param name="volume"></param>
        void SetEffectVolume(float volume);

        /// <summary>
        /// 设置音乐音量
        /// </summary>
        /// <param name="volume"></param>
        void SetMusicVolume(float volume);
    }
}
