using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Spine.Unity;

namespace Framework
{
    public class SpineRenderer : IPooledObject
    {
        public SpineData spineData;
        public GameObject gameObject;
        public Transform transform;
        public MeshRenderer meshRenderer;
        public MaterialPropertyBlock mpb;
        public SkeletonAnimation skeletonAnimation;

        public SpineRenderer()
        {
            gameObject = new GameObject("SpineRenderer");
            transform = gameObject.transform;
            skeletonAnimation = gameObject.AddComponent<SkeletonAnimation>();
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
            mpb = new MaterialPropertyBlock();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset()
        {
            spineData = null;
            mpb.Clear();
            meshRenderer.SetPropertyBlock(null);
            skeletonAnimation.skeletonDataAsset = null;
            skeletonAnimation.Initialize(true);
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Dispose()
        {
            Reset();
            UnityEngine.Object.Destroy(gameObject);
            gameObject = null;
            transform = null;
            meshRenderer = null;
            skeletonAnimation = null;
            mpb = null;
        }

        /// <summary>
        /// 从对象池中获取
        /// </summary>
        public virtual void OnPoolGet()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 重置对象池对象
        /// </summary>
        public virtual void OnPoolReset()
        {
            Reset();
            transform.SetParent(null, false);
            gameObject.SetActive(false);
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
