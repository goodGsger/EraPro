using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Live2D.Cubism.Framework.MotionFade;

namespace Framework
{
    public class Live2DManager
    {
        private static Live2DManager _inst;

        public float defaultScale = 1f;
        private Dictionary<string, Live2DData> _live2DDataDict;
        private Dictionary<string, AssetBundle> _assetBundleDict;

        public Live2DManager()
        {
            _live2DDataDict = new Dictionary<string, Live2DData>();
            _assetBundleDict = new Dictionary<string, AssetBundle>();
        }

        public static Live2DManager inst
        {
            get
            {
                if (_inst == null)
                    _inst = new Live2DManager();

                return _inst;
            }
        }

        /// <summary>
        /// 获取Live2D数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Live2DData GetLive2DData(string name)
        {
            if (_live2DDataDict.TryGetValue(name, out Live2DData live2DData))
                return live2DData;
            return null;
        }

        /// <summary>
        /// 创建Live2D数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="prefab"></param>
        /// <param name="animatorController"></param>
        /// <param name="fadeMotionList"></param>
        /// <returns></returns>
        public Live2DData CreateLive2DData(string name, GameObject prefab, RuntimeAnimatorController animatorController, CubismFadeMotionList fadeMotionList)
        {
            Live2DData live2DData = App.objectPoolManager.GetObject<Live2DData>();
            live2DData.name = name;
            live2DData.prefab = prefab;
            live2DData.animatorController = animatorController;
            live2DData.fadeMotionList = fadeMotionList;
            live2DData.scale = defaultScale;

            return live2DData;
        }

        /// <summary>
        /// 通过AssetBundle创建Live2D数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="assetBundle"></param>
        /// <returns></returns>
        public Live2DData CreateLive2DDataFromAssetBundle(string name, AssetBundle assetBundle)
        {
            Live2DData live2DData;
            if (_live2DDataDict.TryGetValue(name, out live2DData))
                return live2DData;

            GameObject prefab = assetBundle.LoadAsset<GameObject>(name);
            if (prefab == null)
            {
                App.logManager.Error("Live2DManager.CreateLive2DDataFromAssetBundle Error. name:" + name);
                return null;
            }

            string animatorControllerName = name;
            RuntimeAnimatorController animatorController = assetBundle.LoadAsset<RuntimeAnimatorController>(animatorControllerName);
            if (animatorController == null)
            {
                App.logManager.Error("Live2DManager.CreateLive2DDataFromAssetBundle Error. animatorControllerName:" + animatorControllerName);
                return null;
            }

            string fadeMotionName = name + ".fadeMotionList";
            CubismFadeMotionList fadeMotionList = assetBundle.LoadAsset<CubismFadeMotionList>(fadeMotionName);
            if (fadeMotionList == null)
            {
                App.logManager.Error("Live2DManager.CreateLive2DDataFromAssetBundle Error. fadeMotionName:" + fadeMotionName);
                return null;
            }

            live2DData = CreateLive2DData(name, prefab, animatorController, fadeMotionList);
            _live2DDataDict[name] = live2DData;
            _assetBundleDict[name] = assetBundle;

            return live2DData;
        }

        /// <summary>
        /// 通过Path创建Live2D数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public Live2DData CreateLive2DDataFromPath(string name, string path)
        {
            Live2DData live2DData;
            if (_live2DDataDict.TryGetValue(name, out live2DData))
                return live2DData;

            path = App.fileManager.GetFilePath(path);
            if (path == null)
                return null;

            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
            if (assetBundle == null)
            {
                App.logManager.Error("Live2DManager.CreateLive2DDataFromPath Error. name:" + name + " path:" + path);
                return null;
            }

            live2DData = CreateLive2DDataFromAssetBundle(name, assetBundle);
            if (live2DData == null)
            {
                assetBundle.Unload(true);
                return null;
            }

            return live2DData;
        }

        /// <summary>
        /// 通过Resources创建Live2D数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public Live2DData CreateLive2DDataFromResources(string name, string path)
        {
            Live2DData live2DData;
            if (_live2DDataDict.TryGetValue(name, out live2DData))
                return live2DData;

            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab == null)
            {
                App.logManager.Error("Live2DManager.CreateLive2DDataFromResources Error. name:" + name + " path:" + path);
                return null;
            }

            string animatorControllerName = "Animation/" + name;
            RuntimeAnimatorController animatorController = Resources.Load<RuntimeAnimatorController>(animatorControllerName);
            if (animatorController == null)
            {
                App.logManager.Error("Live2DManager.CreateLive2DDataFromAssetBundle Error. animatorControllerName:" + animatorControllerName);
                return null;
            }

            string fadeMotionName = path + ".fadeMotionList";
            CubismFadeMotionList fadeMotionList = Resources.Load<CubismFadeMotionList>(fadeMotionName);
            if (fadeMotionList == null)
            {
                App.logManager.Error("Live2DManager.CreateLive2DDataFromResources Error. fadeMotionName:" + fadeMotionName);
                return null;
            }

            live2DData = CreateLive2DData(name, prefab, animatorController, fadeMotionList);
            _live2DDataDict[name] = live2DData;

            return live2DData;
        }

        /// <summary>
        /// 销毁Live2DData
        /// </summary>
        /// <param name="name"></param>
        public void DestoryLive2DData(string name)
        {
            Live2DData live2DData;
            if (!_live2DDataDict.TryGetValue(name, out live2DData))
                return;
            _live2DDataDict.Remove(name);
            App.objectPoolManager.ReleaseObject(live2DData);

            if (_assetBundleDict.TryGetValue(name, out AssetBundle assetBundle))
            {
                _assetBundleDict.Remove(name);
                assetBundle.Unload(true);
            }
        }

        /// <summary>
        /// 创建Live2D动画
        /// </summary>
        /// <param name="live2DData"></param>
        /// <returns></returns>
        public Live2DRenderer CreateLive2DRenderer(Live2DData live2DData)
        {
            Live2DRenderer renderer = App.objectPoolManager.GetObject<Live2DRenderer>();
            renderer.SetLive2DData(live2DData);

            return renderer;
        }

        /// <summary>
        /// 创建Live2D动画
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Live2DRenderer CreateLive2DRenderer(string name)
        {
            Live2DData live2DData = GetLive2DData(name);
            if (live2DData == null)
                return null;

            return CreateLive2DRenderer(live2DData);
        }

        /// <summary>
        /// 释放Live2D动画
        /// </summary>
        /// <param name="renderer"></param>
        public void ReleaseLive2DRenderer(Live2DRenderer renderer)
        {
            App.objectPoolManager.ReleaseObject(renderer);
        }
    }
}
