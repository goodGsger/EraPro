using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface IObjectPoolManager : IManager
    {
        /// <summary>
        /// 注册对象池
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IObjectPool RegisterObjectPool(Type type);

        /// <summary>
        /// 注册对象池
        /// </summary>
        /// <param name="type"></param>
        /// <param name="minimumPoolSize"></param>
        /// <param name="maximumPoolSize"></param>
        /// <returns></returns>
        IObjectPool RegisterObjectPool(Type type, int minimumPoolSize, int maximumPoolSize);

        /// <summary>
        /// 注册对象池
        /// </summary>
        /// <param name="type"></param>
        /// <param name="minimumPoolSize"></param>
        /// <param name="maximumPoolSize"></param>
        /// <param name="factoryMethod"></param>
        /// <returns></returns>
        IObjectPool RegisterObjectPool(Type type, int minimumPoolSize, int maximumPoolSize, ObjectPoolFactoryMethod factoryMethod);

        /// <summary>
        /// 注销对象池
        /// </summary>
        /// <param name="type"></param>
        void UnregisterObjectPool(Type type);


        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IObjectPool GetObjectPool(Type type);

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IPooledObject GetObject(Type type);

        /// <summary>
        /// 注册对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IObjectPool RegisterObjectPool<T>() where T : IPooledObject;

        /// <summary>
        /// 注册对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="minimumPoolSize"></param>
        /// <param name="maximumPoolSize"></param>
        /// <returns></returns>
        IObjectPool RegisterObjectPool<T>(int minimumPoolSize, int maximumPoolSize) where T : IPooledObject;

        /// <summary>
        /// 注册对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="minimumPoolSize"></param>
        /// <param name="maximumPoolSize"></param>
        /// <param name="factoryMethod"></param>
        /// <returns></returns>
        IObjectPool RegisterObjectPool<T>(int minimumPoolSize, int maximumPoolSize, ObjectPoolFactoryMethod factoryMethod) where T : IPooledObject;

        /// <summary>
        /// 注销对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void UnregisterObjectPool<T>() where T : IPooledObject;

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IObjectPool GetObjectPool<T>() where T : IPooledObject;

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetObject<T>() where T : IPooledObject;

        /// <summary>
        /// 释放对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pooledObject"></param>
        //void ReleaseObject<T>(T pooledObject) where T : IPooledObject;
        void ReleaseObject(IPooledObject pooledObject);
    }
}
