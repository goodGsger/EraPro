using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class TextureExtSpriteLoader : AssetBundleLoader
    {
        protected TextureExtSprite _textureExtSprite;

        public TextureExtSpriteLoader() : base()
        {
            _type = LoadType.TEXTURE_EXT_SPRITE;
        }

        /// <summary>
        /// 纹理数据
        /// </summary>
        public TextureExtSprite textureExtSprite
        {
            get { return _textureExtSprite; }
            set { _textureExtSprite = value; }
        }

        protected override void OnLoadComplete()
        {
            string fileName = UrlUtil.GetFileName(_urlRelative);

            _textureExtSprite = App.objectPoolManager.GetObject<TextureExtSprite>();
            _textureExtSprite.Init(fileName, _assetBundle);
            _data = _textureExtSprite;

            //_assetBundle.Unload(false);
        }

        protected override void onDispose()
        {
            base.onDispose();
            _textureExtSprite = null;
        }

        public override void OnPoolReset()
        {
            base.OnPoolGet();
            _textureExtSprite = null;
        }
    }
}
