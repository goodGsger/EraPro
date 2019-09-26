using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public abstract class AbstractAssetPackage : AbstractAsset, IAssetPackage
    {
        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract object GetAsset(string name);

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract object GetAsset(string name, Type type);

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract T GetAsset<T>(string name) where T : UnityEngine.Object;

        /// <summary>
        /// 资源包中是否包含资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract bool HasAsset(string name);

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract object[] GetAssets(Type type);

        /// <summary>
        /// 获取资源包中的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract T[] GetAssets<T>() where T : UnityEngine.Object;
    }
}
