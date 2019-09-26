using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class TextureAsset : AbstractAsset
    {
        protected Texture2D _texture;

        public override object asset
        {
            get
            {
                return _asset;
            }

            set
            {
                _asset = value;
                _texture = _asset as Texture2D;
            }
        }

        public Texture2D texture
        {
            get { return _texture; }
        }

        protected override byte[] CreateBytes()
        {
            if (_texture != null)
                return _texture.GetRawTextureData();

            return null;
        }

        override public void Dispose()
        {
            if (_texture != null)
            {
#if !UNITY_EDITOR && !UNITY_STANDALONE_WIN
                Resources.UnloadAsset(_texture);
#else
                UnityEngine.Object.Destroy(_texture);
#endif
                _texture = null;
            }
            base.Dispose();
        }
    }
}
