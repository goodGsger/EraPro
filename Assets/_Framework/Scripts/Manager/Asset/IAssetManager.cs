using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public delegate bool CheckAutoClearDelegate(IAsset asset);

    public interface IAssetManager : IManager
    {
        /// <summary>
        /// 是否禁用
        /// </summary>
        bool enabled { get; set; }

        /// <summary>
        /// 自动清理时间
        /// </summary>
        float autoClearTime { get; set; }

        /// <summary>
        /// 检测自动清理
        /// </summary>
        CheckAutoClearDelegate checkAutoClearDelegate { get; set; }

        /// <summary>
        /// 向内存中添加资源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="asset"></param>
        void AddAsset(string id, IAsset asset);

        /// <summary>
        /// 获取内存中是否包含资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool HasAsset(string id);

        /// <summary>
        /// 从内存中移除资源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="destroy"></param>
        void RemoveAsset(string id, bool destroy = true);

        /// <summary>
        /// 根据ID获取内存中的资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IAsset GetAsset(string id);

        /// <summary>
        /// 根据ID获取内存中的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetAsset<T>(string id) where T : IAsset;

        /// <summary>
        /// 根据ID获取内存中的Package资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IAssetPackage GetAssetPackage(string id);

        /// <summary>
        /// 根据ID获取内存中的Package资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetAssetPackage<T>(string id) where T : IAssetPackage;

        /// <summary>
        /// 根据ID，Name获取Package中的资源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        object GetAssetInPackage(string id, string name);

        /// <summary>
        /// 根据ID，Name获取Package中的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        T GetAssetInPackage<T>(string id, string name) where T : UnityEngine.Object;

        /// <summary>
        /// 使用资源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        void UseAsset(string id, int count = 1);

        /// <summary>
        /// 不使用资源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        void UnuseAsset(string id, int count = 1);

        /// <summary>
        /// 向本地保存资源
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="path"></param>
        /// <param name="overwrite"></param>
        void SaveAsset(IAsset asset, string path, bool overwrite);

        /// <summary>
        /// 从本地删除资源
        /// </summary>
        /// <param name="path"></param>
        void DeleteAsset(string path);

        /// <summary>
        /// 从本地删除资源
        /// </summary>
        /// <param name="asset"></param>
        void DeleteAsset(IAsset asset);
    }
}
