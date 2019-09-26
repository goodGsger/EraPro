using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Live2D.Cubism.Framework.MotionFade;

namespace Framework
{
    public class Live2DData : IPooledObject
    {
        public string name;
        public GameObject prefab;
        public RuntimeAnimatorController animatorController;
        public CubismFadeMotionList fadeMotionList;
        public float scale = 1f;

        private Queue<GameObject> gameObjects;

        public Live2DData()
        {
            gameObjects = new Queue<GameObject>();
        }

        /// <summary>
        /// 获取游戏对象
        /// </summary>
        /// <returns></returns>
        public GameObject GetGameObject()
        {
            GameObject gameObject = null;
            if (gameObjects.Count > 0)
                gameObject = gameObjects.Dequeue();
            else if (prefab != null)
                gameObject = UnityEngine.Object.Instantiate(prefab);

            if (gameObject != null)
            {
                gameObject.SetActive(true);
                gameObject.transform.localScale = new Vector3(scale, scale, scale);
            }

            return gameObject;
        }

        /// <summary>
        /// 释放游戏对象
        /// </summary>
        /// <param name="gameObject"></param>
        public void ReleaseGameObject(GameObject gameObject)
        {
            gameObject.SetActive(false);
            gameObject.transform.SetParent(null, false);
            gameObjects.Enqueue(gameObject);
        }

        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset()
        {
            foreach (GameObject gameObject in gameObjects)
                UnityEngine.Object.Destroy(gameObject);

            gameObjects.Clear();
            name = null;
            if (prefab != null)
            {
                UnityEngine.Object.Destroy(prefab);
                prefab = null;
            }
            if (animatorController != null)
            {
                UnityEngine.Object.Destroy(animatorController);
                animatorController = null;
            }
            if (fadeMotionList != null)
            {
                UnityEngine.Object.Destroy(fadeMotionList);
                fadeMotionList = null;
            }
            scale = 1f;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Dispose()
        {
            Reset();
            gameObjects = null;
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
