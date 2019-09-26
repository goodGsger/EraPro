using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class TextureExtSpriteAsset : AbstractAssetPackage
    {
        protected TextureExtSprite _textureExtSprite;

        public override object asset
        {
            get
            {
                return _asset;
            }

            set
            {
                _asset = value;
                _textureExtSprite = _asset as TextureExtSprite;
            }
        }

        public TextureExtSprite textureExtSprite
        {
            get { return _textureExtSprite; }
        }

        protected override byte[] CreateBytes()
        {
            return null;
        }

        override public void Dispose()
        {
            if (_textureExtSprite != null)
            {
                App.objectPoolManager.ReleaseObject(_textureExtSprite);
                _textureExtSprite = null;
            }
            base.Dispose();
        }

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override object GetAsset(string name)
        {
            return _textureExtSprite.GetSprite(name);
        }

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override object GetAsset(string name, Type type)
        {
            return _textureExtSprite.GetSprite(name);
        }

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public override T GetAsset<T>(string name)
        {
            return _textureExtSprite.GetSprite(name) as T;
        }

        /// <summary>
        /// 资源包中是否包含资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override bool HasAsset(string name)
        {
            return _textureExtSprite.HasSprite(name);
        }

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override object[] GetAssets(Type type)
        {
            return null;
        }

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override T[] GetAssets<T>()
        {
            return null;
        }
    }
}
