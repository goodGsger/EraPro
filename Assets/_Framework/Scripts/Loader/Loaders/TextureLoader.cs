using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class TextureLoader : WWWLoader
    {
        protected Texture2D _texture;

        public TextureLoader() : base()
        {
            _type = LoadType.TEXTURE;
        }

        public Texture2D texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        protected override void OnLoadComplete()
        {
            _data = _texture = _www.texture;
        }

        protected override void onDispose()
        {
            base.onDispose();
            _texture = null;
        }

        public override void OnPoolReset()
        {
            base.OnPoolGet();
            _texture = null;
        }
    }
}
