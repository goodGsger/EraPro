using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class AudioAsset : AbstractAsset
    {
        protected AudioClip _audioClip;

        public override object asset
        {
            get
            {
                return _asset;
            }

            set
            {
                _asset = value;
                _audioClip = _asset as AudioClip;
            }
        }

        public AudioClip audioClip
        {
            get { return _audioClip; }
        }

        public override void OnAdd()
        {
            _lastUseTime = Time.time;
        }

        protected override byte[] CreateBytes()
        {
            if (_audioClip == null)
                return null;

            float[] data = new float[_audioClip.samples * _audioClip.channels];
            _audioClip.GetData(data, 0);
            byte[] bytes = new byte[data.Length * 4];
            Buffer.BlockCopy(data, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        override public void Dispose()
        {
            _audioClip.UnloadAudioData();
            UnityEngine.Object.DestroyImmediate(_audioClip, true);
            _audioClip = null;
            base.Dispose();
        }
    }
}
