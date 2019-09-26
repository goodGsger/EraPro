using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class Action3DManager
    {
        private static Action3DManager _inst;

        private bool _enabled;
        private Dictionary<string, Dictionary<string, ActionDataPackage3D>> _actionCaches;
        private float _autoClearTime;

        public Action3DManager()
        {
            _actionCaches = new Dictionary<string, Dictionary<string, ActionDataPackage3D>>();
            _autoClearTime = 10;
            enabled = true;
        }

        public static Action3DManager inst
        {
            get
            {
                if (_inst == null)
                    _inst = new Action3DManager();

                return _inst;
            }
        }

        public bool enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (_enabled)
                {
                    App.timerManager.RegisterLoop(5f, ClearGameObjects);
                }
                else
                {
                    App.timerManager.Unregister(ClearGameObjects);
                }
            }
        }

        /// <summary>
        /// 自动清理时间
        /// </summary>
        public float autoClearTime
        {
            get { return _autoClearTime; }
            set
            {
                _autoClearTime = value;
                ClearGameObjects();
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        private ActionDataPackage3D GetCache(string packageName, string objectName)
        {
            Dictionary<string, ActionDataPackage3D> pkg;
            if (_actionCaches.TryGetValue(packageName, out pkg) == false)
                return null;

            ActionDataPackage3D cache;
            if (pkg.TryGetValue(objectName, out cache) == false)
                return null;

            return cache;
        }

        /// <summary>
        /// 获取游戏对象
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public ActionData3D GetActionData(string packageName, string objectName)
        {
            ActionDataPackage3D cache = GetCache(packageName, objectName);
            if (cache == null)
            {
                IAssetPackage assetPackage = App.assetManager.GetAssetPackage(packageName);
                if (assetPackage == null)
                    return null;

                GameObject prefab = assetPackage.GetAsset(objectName) as GameObject;
                if (prefab == null)
                    return null;

                cache = App.objectPoolManager.GetObject<ActionDataPackage3D>();
                cache.packageName = packageName;
                cache.name = objectName;
                cache.assetPackage = assetPackage;
                cache.prefab = prefab;

                Dictionary<string, ActionDataPackage3D> pkg;
                if (_actionCaches.TryGetValue(packageName, out pkg) == false)
                    pkg = _actionCaches[packageName] = new Dictionary<string, ActionDataPackage3D>();

                pkg[objectName] = cache;
            }

            cache.Use();
            return cache.Get();
        }

        /// <summary>
        /// 释放游戏对象
        /// </summary>
        /// <param name="actionData"></param>
        public void ReleaseActionData(ActionData3D actionData)
        {
            ActionDataPackage3D cache = GetCache(actionData.package.packageName, actionData.package.name);
            if (cache == null)
                return;

            cache.Release(actionData);
            cache.Unuse();
        }

        /// <summary>
        /// 清理游戏对象
        /// </summary>
        public void ClearGameObjects()
        {
            float time = Time.time;
            Dictionary<ActionDataPackage3D, Dictionary<string, ActionDataPackage3D>> removedDict = new Dictionary<ActionDataPackage3D, Dictionary<string, ActionDataPackage3D>>();
            foreach (var v in _actionCaches)
            {
                foreach (var v2 in v.Value)
                {
                    if (v2.Value.useCount == 0)
                    {
                        if (time - v2.Value.lastUseTime >= _autoClearTime)
                        {
                            removedDict[v2.Value] = v.Value;
                        }
                    }
                }
            }

            foreach (var v in removedDict)
            {
                v.Value.Remove(v.Key.name);
                App.objectPoolManager.ReleaseObject(v.Key);
            }
        }
    }
}
