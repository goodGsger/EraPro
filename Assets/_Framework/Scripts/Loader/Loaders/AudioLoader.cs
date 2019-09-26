using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class AudioLoader : WWWLoader
    {
        protected AudioClip _audioClip;

        public AudioLoader() : base()
        {
            _type = LoadType.AUDIO;
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
            _data = _audioClip = _www.GetAudioClip();
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
