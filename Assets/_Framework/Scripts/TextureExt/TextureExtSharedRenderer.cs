using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class TextureExtSharedRenderer : TextureExtRenderer
    {
        protected override void GetMaterial()
        {
            _material = _renderer.sharedMaterial;
        }

        /// <summary>
        /// 透明度
        /// </summary>
        public override float alpha
        {
            get
            {
                return base.alpha;
            }
            set
            {
                _alpha = value;
                MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                _renderer.GetPropertyBlock(mpb);
                mpb.SetFloat("_Alpha", value);
                _renderer.SetPropertyBlock(mpb);
            }
        }

        /// <summary>
        /// 设置纹理资源
        /// </summary>
        /// <param name="textureExtSpriteAsset"></param>
        /// <param name="name"></param>
        public override void SetTextureExtAsset(TextureExtSpriteAsset textureExtSpriteAsset, string name = null)
        {
            // 清除原资源
            if (_textureExtSpriteAsset != null)
                _textureExtSpriteAsset.Unuse();

            _textureExtSpriteAsset = textureExtSpriteAsset;

            if (_textureExtSpriteAsset != null)
            {
                // 重设当前帧
                _gameObject.SetActive(true);
                if (name != null)
                {
                    SetTextureName(name);
                    MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                    _renderer.GetPropertyBlock(mpb);
                    mpb.SetTexture("_AlphaTex", _textureExtSpriteAsset.textureExtSprite.texture.alphaTexture);
                    _renderer.SetPropertyBlock(mpb);
                }
                else
                {
                    MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                    _renderer.GetPropertyBlock(mpb);
                    mpb.SetTexture("_AlphaTex", null);
                    _renderer.SetPropertyBlock(mpb);
                }
                _textureExtSpriteAsset.Use();
            }
            else
            {
                // 隐藏当前帧
                _renderer.sprite = null;
                _gameObject.SetActive(false);
            }
        }
    }
}
