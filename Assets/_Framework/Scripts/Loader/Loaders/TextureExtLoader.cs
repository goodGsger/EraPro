using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class TextureExtLoader : AssetBundleLoader
    {
        protected TextureExt _textureExt;

        public TextureExtLoader() : base()
        {
            _type = LoadType.TEXTURE_EXT;
        }

        /// <summary>
        /// 纹理数据
        /// </summary>
        public TextureExt textureExt
        {
            get { return _textureExt; }
            set { _textureExt = value; }
        }

        protected override void OnLoadComplete()
        {
            string fileName = UrlUtil.GetFileName(_urlRelative);

            _textureExt = App.objectPoolManager.GetObject<TextureExt>();
            _textureExt.Init(fileName, _assetBundle);
            _data = _textureExt;

            //_assetBundle.Unload(false);
        }

        protected override void onDispose()
        {
            base.onDispose();
            _textureExt = null;
        }

        public override void OnPoolReset()
        {
            base.OnPoolGet();
            _textureExt = null;
        }
    }
}
