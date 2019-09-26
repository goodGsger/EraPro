using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class TextureAssetBundleLoader : AssetBundleLoader
    {
        protected Texture2D _texture;

        public TextureAssetBundleLoader() : base()
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
            string fileName = UrlUtil.GetFileName(_urlRelative);
            _data = _texture = _assetBundle.LoadAsset<Texture2D>(fileName);

            //_assetBundle.Unload(false);
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
