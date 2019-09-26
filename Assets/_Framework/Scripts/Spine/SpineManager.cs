using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Spine;
using Spine.Unity;

namespace Framework
{
    public class SpineManager
    {
        private static SpineManager _inst;

        public bool bytesMode;
        public Shader defaultShader;
        public float defaultScale = 0.01f;
        private Dictionary<string, SpineData> _spineDataDict;
        private Dictionary<string, AssetBundle> _assetBundleDict;

        public SpineManager()
        {
            _spineDataDict = new Dictionary<string, SpineData>();
            _assetBundleDict = new Dictionary<string, AssetBundle>();
            defaultShader = ShaderManager.inst.GetShader("Spine/Skeleton");
        }

        public static SpineManager inst
        {
            get
            {
                if (_inst == null)
                    _inst = new SpineManager();

                return _inst;
            }
        }

        /// <summary>
        /// 获取骨骼数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SpineData GetSpineData(string name)
        {
            if (_spineDataDict.TryGetValue(name, out SpineData spineData))
                return spineData;
            return null;
        }

        /// <summary>
        /// 创建骨骼数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="atlas"></param>
        /// <param name="texture"></param>
        /// <param name="json"></param>
        /// <param name="shader"></param>
        /// <returns></returns>
        public SpineData CreateSpineData(string name, TextAsset atlas, Texture texture, TextAsset json, Shader shader = null)
        {
            if (shader == null)
                shader = defaultShader;

            SpineData spineData = App.objectPoolManager.GetObject<SpineData>();
            spineData.name = name;
            spineData.atlas = atlas;
            spineData.texture = texture;
            spineData.json = json;
            spineData.shader = shader;
            spineData.scale = defaultScale;

            return spineData;
        }

        /// <summary>
        /// 通过AssetBundle创建骨骼数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="assetBundle"></param>
        /// <param name="shader"></param>
        /// <returns></returns>
        public SpineData CreateSpineDataFromAssetBundle(string name, AssetBundle assetBundle, Shader shader = null)
        {
            SpineData spineData;
            if (_spineDataDict.TryGetValue(name, out spineData))
                return spineData;

            string atlasName = name + ".atlas";
            TextAsset atlasText = assetBundle.LoadAsset<TextAsset>(atlasName);
            if (atlasText == null)
            {
                App.logManager.Error("SpineManager.CreateSpineDataFromAssetBundle Error. name:" + name + " atlasName:" + atlasName);
                return null;
            }

            string textureName = name;
            Texture texture = assetBundle.LoadAsset<Texture>(textureName);
            if (texture == null)
            {
                App.logManager.Error("SpineManager.CreateSpineDataFromAssetBundle Error. name:" + name + " textureName:" + textureName);
                return null;
            }

            string jsonName = bytesMode ? name + ".skel" : name;
            TextAsset jsonText = assetBundle.LoadAsset<TextAsset>(jsonName);
            if (jsonText == null)
            {
                App.logManager.Error("SpineManager.CreateSpineDataFromAssetBundle Error. name:" + name + " jsonName:" + jsonName);
                return null;
            }

            spineData = CreateSpineData(name, atlasText, texture, jsonText, shader);
            _spineDataDict[name] = spineData;
            _assetBundleDict[name] = assetBundle;

            return spineData;
        }

        /// <summary>
        /// 通过Path创建骨骼数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="shader"></param>
        /// <returns></returns>
        public SpineData CreateSpineDataFromPath(string name, string path, Shader shader = null)
        {
            SpineData spineData;
            if (_spineDataDict.TryGetValue(name, out spineData))
                return spineData;

            path = App.fileManager.GetFilePath(path);
            if (path == null)
                return null;

            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
            if (assetBundle == null)
            {
                App.logManager.Error("SpineManager.CreateSpineDataFromPath Error. name:" + name + " path:" + path);
                return null;
            }

            spineData = CreateSpineDataFromAssetBundle(name, assetBundle, shader);
            if (spineData == null)
            {
                assetBundle.Unload(true);
                return null;
            }

            return spineData;
        }

        /// <summary>
        /// 通过Resources创建骨骼数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="shader"></param>
        /// <returns></returns>
        public SpineData CreateSpineDataFromResources(string name, string path, Shader shader = null)
        {
            SpineData spineData;
            if (_spineDataDict.TryGetValue(name, out spineData))
                return spineData;

            string atlasName = path + ".atlas";
            TextAsset atlasText = Resources.Load<TextAsset>(atlasName);
            if (atlasText == null)
            {
                App.logManager.Error("SpineManager.CreateSpineDataFromResources Error. name:" + name + " path:" + path + " atlasName:" + atlasName);
                return null;
            }

            string textureName = path;
            Texture texture = Resources.Load<Texture>(textureName);
            if (texture == null)
            {
                App.logManager.Error("SpineManager.CreateSpineDataFromResources Error. name:" + name + " path:" + path + " textureName:" + textureName);
                return null;
            }

            string jsonName = bytesMode ? path + ".skel" : path;
            TextAsset jsonText = Resources.Load<TextAsset>(jsonName);
            if (jsonText == null)
            {
                App.logManager.Error("SpineManager.CreateSpineDataFromResources Error. name:" + name + " path:" + path + " jsonName:" + jsonName);
                return null;
            }

            spineData = CreateSpineData(name, atlasText, texture, jsonText, shader);
            _spineDataDict[name] = spineData;

            return spineData;
        }

        /// <summary>
        /// 销毁SpineData
        /// </summary>
        /// <param name="name"></param>
        public void DestorySpineData(string name)
        {
            SpineData spineData;
            if (!_spineDataDict.TryGetValue(name, out spineData))
                return;
            _spineDataDict.Remove(name);
            App.objectPoolManager.ReleaseObject(spineData);

            if (_assetBundleDict.TryGetValue(name, out AssetBundle assetBundle))
            {
                _assetBundleDict.Remove(name);
                assetBundle.Unload(true);
            }
        }

        /// <summary>
        /// 创建骨骼动画
        /// </summary>
        /// <param name="spineData"></param>
        /// <param name="shared"></param>
        /// <returns></returns>
        public SpineRenderer CreateSpineRenderer(SpineData spineData, bool shared = false)
        {
            SpineRenderer renderer = App.objectPoolManager.GetObject<SpineRenderer>();
            renderer.spineData = spineData;
            renderer.skeletonAnimation.skeletonDataAsset = spineData.GetSkeletonDataAsset(shared);
            renderer.skeletonAnimation.Initialize(false);

            return renderer;
        }

        /// <summary>
        /// 创建骨骼动画
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SpineRenderer CreateSpineRenderer(string name, bool shared = false)
        {
            SpineData spineData = GetSpineData(name);
            if (spineData == null)
                return null;

            return CreateSpineRenderer(spineData, shared);
        }

        /// <summary>
        /// 释放骨骼动画
        /// </summary>
        /// <param name="renderer"></param>
        public void ReleaseSpineRenderer(SpineRenderer renderer)
        {
            renderer.spineData.ReleaseSkeletonDataAsset(renderer.skeletonAnimation.SkeletonDataAsset);
            App.objectPoolManager.ReleaseObject(renderer);
        }
    }
}
