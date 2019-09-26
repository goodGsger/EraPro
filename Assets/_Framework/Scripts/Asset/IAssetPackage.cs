using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface IAssetPackage : IAsset
    {
        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object GetAsset(string name);

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        object GetAsset(string name, Type type);

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        T GetAsset<T>(string name) where T : UnityEngine.Object;

        /// <summary>
        /// 资源包中是否包含资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool HasAsset(string name);

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object[] GetAssets(Type type);

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T[] GetAssets<T>() where T : UnityEngine.Object;
    }
}
