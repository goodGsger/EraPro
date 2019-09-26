using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Live2D.Cubism.Framework.Motion;
using Live2D.Cubism.Framework.MotionFade;

namespace Framework
{
    public class Live2DRenderer : IPooledObject
    {
        public Live2DData live2DData;
        public GameObject gameObject;
        public Transform transform;

        public Animator animator;
        public CubismFadeController fadeController;
        public CubismMotionController motionController;

        /// <summary>
        /// 设置Live2D对象
        /// </summary>
        /// <param name="live2DData"></param>
        public void SetLive2DData(Live2DData live2DData)
        {
            if (this.live2DData != null)
                Clear();

            this.live2DData = live2DData;
            gameObject = live2DData.GetGameObject();
            transform = gameObject.transform;

            animator = gameObject.GetComponent<Animator>();

            fadeController = gameObject.GetComponent<CubismFadeController>();
            if (fadeController == null)
                fadeController = gameObject.AddComponent<CubismFadeController>();
            fadeController.CubismFadeMotionList = live2DData.fadeMotionList;

            motionController = gameObject.GetComponent<CubismMotionController>();
            if (motionController == null)
                motionController = gameObject.AddComponent<CubismMotionController>();
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            transform = null;
            animator = null;
            fadeController.CubismFadeMotionList = null;
            fadeController = null;
            motionController = null;
            live2DData.ReleaseGameObject(gameObject);
            live2DData = null;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset()
        {
            Clear();
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
