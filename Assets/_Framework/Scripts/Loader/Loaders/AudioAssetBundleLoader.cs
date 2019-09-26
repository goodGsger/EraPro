using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Framework
{
    public class AudioAssetBundleLoader : AssetBundleLoader
    {
        protected AudioClip _audioClip;

        public AudioAssetBundleLoader() : base()
        {
            _type = LoadType.AUDIO_ASSET_BUNDLE;
        }

        /// <summary>
        /// 音频
        /// </summary>
        public AudioClip audioClip
        {
            get { return _audioClip; }
            set { _audioClip = value; }
        }

        protected override void OnLoadComplete()
        {
            // 解析音频
            string fileName = UrlUtil.GetFileName(_urlRelative);
            _data = _audioClip = _assetBundle.LoadAsset<AudioClip>(fileName);

            //_assetBundle.Unload(false);
        }

        protected override void onDispose()
        {
            base.onDispose();
            _audioClip = null;
        }

        public override void OnPoolReset()
        {
            base.OnPoolGet();
            _audioClip = null;
        }
    }
}
