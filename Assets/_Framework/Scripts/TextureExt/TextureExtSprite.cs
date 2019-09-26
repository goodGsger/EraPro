using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class TextureExtSprite : IPooledObject
    {
        private TextureExt _texture;
        private Sprite[] _spriteList;
        private Dictionary<string, Sprite> _spriteDict;
        private Sprite[] _alphaSpriteList;
        private Dictionary<string, Sprite> _alphaSpriteDict;

        public TextureExtSprite()
        {
            _spriteDict = new Dictionary<string, Sprite>();
            _alphaSpriteDict = new Dictionary<string, Sprite>();
        }

        public void Init(string fileName, AssetBundle assetBundle)
        {
            _texture = App.objectPoolManager.GetObject<TextureExt>();
            _texture.Init(fileName, assetBundle);

            int length = _texture.paramList.Length;
            _spriteList = new Sprite[length];
            _alphaSpriteList = new Sprite[length];

            _spriteDict.Clear();
            _alphaSpriteDict.Clear();
        }

        /// <summary>
        /// 精灵数量
        /// </summary>
        public int spriteCount
        {
            get { return _texture.paramList.Length; }
        }

        /// <summary>
        /// 纹理数据
        /// </summary>
        public TextureExt texture
        {
            get { return _texture; }
        }

        /// <summary>
        /// Sprite数组
        /// </summary>
        public Sprite[] spriteList
        {
            get { return _spriteList; }
        }

        /// <summary>
        /// Sprite字典
        /// </summary>
        public Dictionary<string, Sprite> spriteDict
        {
            get { return _spriteDict; }
        }

        /// <summary>
        /// 创建Sprite
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Sprite CreateSprite(int index)
        {
            TextureExtParam param = _texture.GetParam(index);
            Sprite sprite = Sprite.Create(_texture.texture, param.rect, param.uv, 1);
            _spriteList[index] = sprite;
            _spriteDict[param.name] = sprite;
            return sprite;
        }

        /// <summary>
        /// 创建Sprite
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Sprite CreateSprite(string name)
        {
            TextureExtParam param = _texture.GetParam(name);
            Sprite sprite = Sprite.Create(_texture.texture, param.rect, param.uv, 1);
            _spriteList[param.index] = sprite;
            _spriteDict[name] = sprite;
            return sprite;
        }

        /// <summary>
        /// 获取Sprite
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Sprite GetSprite(int index)
        {
            if (HasSprite(index))
            {
                Sprite sprite = _spriteList[index];
                if (sprite == null)
                    sprite = _spriteList[index] = CreateSprite(index);

                return sprite;
            }

            return null;
        }

        /// <summary>
        /// 获取Sprite
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Sprite GetSprite(string name)
        {
            if (HasSprite(name))
            {
                Sprite sprite;
                if (_spriteDict.TryGetValue(name, out sprite) == false)
                    sprite = _spriteDict[name] = CreateSprite(name);
                return sprite;
            }

            return null;
        }

        /// <summary>
        /// 是否包含Sprite
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool HasSprite(int index)
        {
            return _texture.HasParam(index);
        }

        /// <summary>
        /// 是否包含Sprite
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasSprite(string name)
        {
            return _texture.HasParam(name);
        }

        /// <summary>
        /// AlphaSprite数组
        /// </summary>
        public Sprite[] alphaSpriteList
        {
            get { return _alphaSpriteList; }
        }

        /// <summary>
        /// AlphaSprite字典
        /// </summary>
        public Dictionary<string, Sprite> alphaSpriteDict
        {
            get { return _alphaSpriteDict; }
        }

        /// <summary>
        /// 创建AlphaSprite
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Sprite CreateAlphaSprite(int index)
        {
            TextureExtParam param = _texture.GetParam(index);
            Sprite sprite = Sprite.Create(_texture.alphaTexture, param.rect, param.uv, 1);
            _alphaSpriteList[index] = sprite;
            _alphaSpriteDict[param.name] = sprite;
            return sprite;
        }

        /// <summary>
        /// 创建AlphaSprite
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Sprite CreateAlphaSprite(string name)
        {
            TextureExtParam param = _texture.GetParam(name);
            Sprite sprite = Sprite.Create(_texture.alphaTexture, param.rect, param.uv, 1);
            _alphaSpriteList[param.index] = sprite;
            _alphaSpriteDict[name] = sprite;
            return sprite;
        }

        /// <summary>
        /// 获取AlphaSprite
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Sprite GetAlphaSprite(int index)
        {
            if (HasAlphaSprite(index))
            {
                Sprite sprite = _alphaSpriteList[index];
                if (sprite == null)
                    sprite = _alphaSpriteList[index] = CreateAlphaSprite(index);

                return sprite;
            }

            return null;
        }

        /// <summary>
        /// 获取AlphaSprite
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Sprite GetAlphaSprite(string name)
        {
            if (HasAlphaSprite(name))
            {
                Sprite sprite;
                if (_alphaSpriteDict.TryGetValue(name, out sprite) == false)
                    sprite = _alphaSpriteDict[name] = CreateAlphaSprite(name);
                return sprite;
            }

            return null;
        }

        /// <summary>
        /// 是否包含AlphaSprite
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool HasAlphaSprite(int index)
        {
            return _texture.HasParam(index);
        }

        /// <summary>
        /// 是否包含AlphaSprite
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasAlphaSprite(string name)
        {
            return _texture.HasParam(name);
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            if (_texture != null)
            {
                App.objectPoolManager.ReleaseObject(_texture);
                _texture = null;
            }
            foreach (Sprite sprite in _spriteList)
            {
                UnityEngine.Object.Destroy(sprite);
            }
            foreach (Sprite sprite in _alphaSpriteList)
            {
                UnityEngine.Object.Destroy(sprite);
            }
            _spriteList = null;
            _spriteDict.Clear();
            _alphaSpriteList = null;
            _alphaSpriteDict.Clear();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            Reset();
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
