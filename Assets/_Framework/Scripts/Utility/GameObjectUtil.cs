using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class GameObjectUtil
    {
        /// <summary>
        /// 设置layer
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="layer"></param>
        /// <param name="withChildren"></param>
        public static void SetLayer(GameObject gameObject, int layer, bool withChildren = false)
        {
            gameObject.layer = layer;
            if (withChildren)
            {
                Transform transform = gameObject.transform;
                foreach (Transform childTransform in transform)
                {
                    childTransform.gameObject.layer = layer;
                }
            }
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        public static void UnloadUnusedAssets()
        {
            Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 执行GC
        /// </summary>
        public static void DoGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// 卸载并自动执行GC
        /// </summary>
        public static void UnloadUnusedAssetsAndDoGC()
        {
            Resources.UnloadUnusedAssets();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// 判断对象是否为prefab
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsPrefab(UnityEngine.Object obj)
        {
            GameObject gameObject = obj as GameObject;
            if (gameObject != null)
                return IsPrefab(gameObject);
            Component component = obj as Component;
            if (component != null)
                return IsPrefab(component);
            return false;
        }

        /// <summary>
        /// 判断对象是否为prefab
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static bool IsPrefab(GameObject gameObject)
        {
            return gameObject.transform.hideFlags == HideFlags.HideInHierarchy;
        }

        /// <summary>
        /// 判断对象是否为prefab
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public static bool IsPrefab(Component component)
        {
            return component.gameObject.transform.hideFlags == HideFlags.HideInHierarchy;
        }
    }
}
