using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class ActionData : IPooledObject
    {
        private int _actionID;
        private TextureExtSprite _textureExtSprite;

        public void Init(string fileName, AssetBundle assetBundle)
        {
            int index = fileName.IndexOf('_');
            if (index > 0)
            {
                int.TryParse(fileName.Substring(0, index), out _actionID);
            }
            _textureExtSprite = App.objectPoolManager.GetObject<TextureExtSprite>();
            _textureExtSprite.Init(fileName, assetBundle);
        }

        /// <summary>
        /// 
        /// </summary>
        public int actionID
        {
            get { return _actionID; }
            set { _actionID = value; }
        }

        /// <summary>
        /// 纹理数据
        /// </summary>
        public TextureExtSprite textureExtSprite
        {
            get { return _textureExtSprite; }
        }

        /// <summary>
        /// 获取Sprite
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Sprite GetSprite(int index)
        {
            if (_textureExtSprite != null)
                return _textureExtSprite.GetSprite(index);

            return null;
        }

        /// <summary>
        /// 获取AlphaSprite
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Sprite GetAlphaSprite(int index)
        {
            if (_textureExtSprite != null)
                return _textureExtSprite.GetAlphaSprite(index);

            return null;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            if (_textureExtSprite != null)
            {
                App.objectPoolManager.ReleaseObject(_textureExtSprite);
                _textureExtSprite = null;
            }
            _actionID = 0;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            if (_textureExtSprite != null)
            {
                App.objectPoolManager.ReleaseObject(_textureExtSprite);
                _textureExtSprite = null;
            }
            _actionID = 0;
        }

        /// <summary>
        /// 从对象池中获取
        /// </summary>
        public virtual void OnPoolGet()
        {

        }

        /// <summary>
        /// 重置对象池对象
        /// </summary>
        public virtual void OnPoolReset()
        {
            Reset();
        }

        /// <summary>
        /// 销毁对象池对象
        /// </summary>
        public virtual void OnPoolDispose()
        {
            Dispose();
        }
    }
}
