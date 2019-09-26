using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Spine;
using Spine.Unity;

namespace Framework
{
    public class SpineData : IPooledObject
    {
        public string name;
        public TextAsset atlas;
        public Texture texture;
        public TextAsset json;
        public Shader shader;
        public float scale = 1f;

        private SkeletonDataAsset _sharedSkeletonDataAsset;
        private Queue<SkeletonDataAsset> _skeletonDataAssets;

        public SpineData()
        {
            _skeletonDataAssets = new Queue<SkeletonDataAsset>();
        }

        /// <summary>
        /// 创建共享骨骼资源
        /// </summary>
        /// <param name="shared"></param>
        /// <returns></returns>
        public SkeletonDataAsset GetSkeletonDataAsset(bool shared = false)
        {
            if (shared)
            {
                if (_sharedSkeletonDataAsset == null)
                    _sharedSkeletonDataAsset = CreateSkeletonDataAsset();
                return _sharedSkeletonDataAsset;
            }

            SkeletonDataAsset skeletonDataAsset;
            if (_skeletonDataAssets.Count > 0)
                skeletonDataAsset = _skeletonDataAssets.Dequeue();
            else
                skeletonDataAsset = CreateSkeletonDataAsset();

            return skeletonDataAsset;
        }

        /// <summary>
        /// 释放骨骼资源
        /// </summary>
        /// <param name="atlasAssets"></param>
        public void ReleaseSkeletonDataAsset(SkeletonDataAsset skeletonDataAsset)
        {
            if (_sharedSkeletonDataAsset == skeletonDataAsset)
                return;

            _skeletonDataAssets.Enqueue(skeletonDataAsset);
        }

        /// <summary>
        /// 创建骨骼资源
        /// </summary>
        /// <returns></returns>
        private SkeletonDataAsset CreateSkeletonDataAsset()
        {
            Material material = new Material(shader);
            material.mainTexture = texture;
            Material[] materials = new Material[] { material };

            SpineAtlasAsset atlasAsset = ScriptableObject.CreateInstance<SpineAtlasAsset>();
            atlasAsset.atlasFile = atlas;
            atlasAsset.materials = materials;
            SpineAtlasAsset[] atlasAssets = new SpineAtlasAsset[] { atlasAsset };

            SkeletonDataAsset skeletonDataAsset = ScriptableObject.CreateInstance<SkeletonDataAsset>();
            skeletonDataAsset.skeletonJSON = json;
            skeletonDataAsset.atlasAssets = atlasAssets;
            skeletonDataAsset.scale = scale;

            return skeletonDataAsset;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset()
        {
            name = null;
            UnityEngine.Object.Destroy(atlas);
            UnityEngine.Object.Destroy(texture);
            UnityEngine.Object.Destroy(json);
            atlas = null;
            texture = null;
            json = null;
            shader = null;
            scale = 1f;

            if (_sharedSkeletonDataAsset != null)
            {
                UnityEngine.Object.Destroy(_sharedSkeletonDataAsset);
                _sharedSkeletonDataAsset = null;
            }
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Dispose()
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
