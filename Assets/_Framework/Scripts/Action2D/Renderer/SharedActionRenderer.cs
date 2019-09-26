using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class SharedActionRenderer : ActionRenderer
    {
        public SharedActionRenderer() : base()
        {
            
        }

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
        /// 设置动作资源
        /// </summary>
        /// <param name="actionAsset"></param>
        public override void SetActionAsset(ActionAsset actionAsset)
        {
            // 清除原资源
            if (_actionAsset != null)
                _actionAsset.Unuse();

            _actionAsset = actionAsset;

            if (_actionAsset != null)
            {
                // 重设当前帧
                _gameObject.SetActive(true);
                frameIndex = _frameIndex;
                _actionAsset.Use();
                MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                _renderer.GetPropertyBlock(mpb);
                mpb.SetTexture("_AlphaTex", _actionAsset.actionData.textureExtSprite.texture.alphaTexture);
                _renderer.SetPropertyBlock(mpb);
            }
            else
            {
                // 隐藏当前帧
                _renderer.sprite = null;
                _gameObject.SetActive(false);
            }
        }

        public override void Reset()
        {
            base.Reset();
            _lockDepth = true;
            _lockSorting = true;
        }
    }
}
