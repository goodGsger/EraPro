using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 动作缓存
    /// </summary>
    public class ActionDataPackage3D : IPooledObject
    {
        public string packageName;
        public string name;
        public IAssetPackage assetPackage;
        public GameObject prefab;
        public Queue<ActionData3D> actionDatas;
        public int useCount;
        public float lastUseTime;

        public ActionDataPackage3D()
        {
            actionDatas = new Queue<ActionData3D>();
        }

        public ActionData3D Get()
        {
            if (actionDatas.Count > 0)
            {
                ActionData3D actionData = actionDatas.Dequeue();
                actionData.gameObject.SetActive(true);

                return actionData;
            }
            else if (prefab != null)
            {
                ActionData3D actionData = App.objectPoolManager.GetObject<ActionData3D>();
                GameObject gameObject = UnityEngine.Object.Instantiate(prefab) as GameObject;
                actionData.package = this;
                actionData.gameObject = gameObject;

                return actionData;
            }
            return null;
        }

        public void Release(ActionData3D actionData)
        {
            actionData.gameObject.SetActive(false);
            Transform transform = actionData.gameObject.transform;
            transform.SetParent(null, false);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            actionDatas.Enqueue(actionData);
        }

        public void Use(int count = 1)
        {
            useCount += count;
            lastUseTime = Time.time;
        }

        public void Unuse(int count = 1)
        {
            useCount -= count;

            if (useCount < 0)
                useCount = 0;

            if (useCount == 0)
                lastUseTime = Time.time;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset()
        {
            foreach (ActionData3D actionData in actionDatas)
            {
                App.objectPoolManager.ReleaseObject(actionData);
            }
            actionDatas.Clear();
            prefab = null;
            assetPackage = null;
            App.assetManager.RemoveAsset(packageName);
            packageName = null;
            name = null;
            useCount = 0;
            lastUseTime = 0f;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Dispose()
        {
            Reset();
            actionDatas = null;
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
