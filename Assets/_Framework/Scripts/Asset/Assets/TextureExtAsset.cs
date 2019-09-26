using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class TextureExtAsset : AbstractAssetPackage
    {
        protected TextureExt _textureExt;

        public override object asset
        {
            get
            {
                return _asset;
            }

            set
            {
                _asset = value;
                _textureExt = _asset as TextureExt;
            }
        }

        public TextureExt textureExt
        {
            get { return _textureExt; }
        }

        protected override byte[] CreateBytes()
        {
            return null;
        }

        override public void Dispose()
        {
            if (_textureExt != null)
            {
                App.objectPoolManager.ReleaseObject(_textureExt);
                _textureExt = null;
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
            return _textureExt.GetParam(name);
        }

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override object GetAsset(string name, Type type)
        {
            return _textureExt.GetParam(name);
        }

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public override T GetAsset<T>(string name)
        {
            return _textureExt.GetParam(name) as T;
        }

        /// <summary>
        /// 资源包中是否包含资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override bool HasAsset(string name)
        {
            return _textureExt.HasParam(name);
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
