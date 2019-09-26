using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Framework
{
    public class ObjectPoolManager : Manager, IObjectPoolManager
    {
        private Dictionary<Type, IObjectPool> _objectPools;

        protected override void Init()
        {
            _objectPools = new Dictionary<Type, IObjectPool>();
        }

        /// <summary>
        /// 注册对象池
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IObjectPool RegisterObjectPool(Type type)
        {
            return RegisterObjectPool(type, 0, int.MaxValue, null);
        }

        /// <summary>
        /// 注册对象池
        /// </summary>
        /// <param name="type"></param>
        /// <param name="minimumPoolSize"></param>
        /// <param name="maximumPoolSize"></param>
        /// <returns></returns>
        public IObjectPool RegisterObjectPool(Type type, int minimumPoolSize, int maximumPoolSize)
        {
            return RegisterObjectPool(type, minimumPoolSize, maximumPoolSize, null);
        }

        /// <summary>
        /// 注册对象池
        /// </summary>
        /// <param name="type"></param>
        /// <param name="minimumPoolSize"></param>
        /// <param name="maximumPoolSize"></param>
        /// <param name="factoryMethod"></param>
        /// <returns></returns>
        public IObjectPool RegisterObjectPool(Type type, int minimumPoolSize, int maximumPoolSize, ObjectPoolFactoryMethod factoryMethod)
        {
            IObjectPool objectPool;
            if (_objectPools.TryGetValue(type, out objectPool))
            {
                App.logManager.Warn("ObjectPoolManager.RegisterObjectPool Warn:" + type.Name + " has already register!");
                return objectPool;
            }

            objectPool = _objectPools[type] = new ObjectPool(type, minimumPoolSize, maximumPoolSize, factoryMethod);

            return objectPool;
        }

        /// <summary>
        /// 注销对象池
        /// </summary>
        /// <param name="type"></param>
        public void UnregisterObjectPool(Type type)
        {
            if (_objectPools.ContainsKey(type))
                _objectPools.Remove(type);
        }


        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IObjectPool GetObjectPool(Type type)
        {
            IObjectPool objectPool;
            if (_objectPools.TryGetValue(type, out objectPool))
                return objectPool;

            return null;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IPooledObject GetObject(Type type)
        {
            IObjectPool objectPool = GetObjectPool(type);
            if (objectPool == null)
                objectPool = RegisterObjectPool(type);

            return objectPool.Get();
        }

        /// <summary>
        /// 注册对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IObjectPool RegisterObjectPool<T>() where T : IPooledObject
        {
            return RegisterObjectPool<T>(0, int.MaxValue, null);
        }

        /// <summary>
        /// 注册对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="minimumPoolSize"></param>
        /// <param name="maximumPoolSize"></param>
        /// <returns></returns>
        public IObjectPool RegisterObjectPool<T>(int minimumPoolSize, int maximumPoolSize) where T : IPooledObject
        {
            return RegisterObjectPool<T>(minimumPoolSize, maximumPoolSize, null);
        }

        /// <summary>
        /// 注册对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="minimumPoolSize"></param>
        /// <param name="maximumPoolSize"></param>
        /// <param name="factoryMethod"></param>
        /// <returns></returns>
        public IObjectPool RegisterObjectPool<T>(int minimumPoolSize, int maximumPoolSize, ObjectPoolFactoryMethod factoryMethod) where T : IPooledObject
        {
            Type type = typeof(T);
            IObjectPool objectPool;
            if (_objectPools.TryGetValue(type, out objectPool))
            {
                App.logManager.Warn("ObjectPoolManager.RegisterObjectPool Warn:" + type.Name + " has already register!");
                return objectPool;
            }

            objectPool = _objectPools[type] = new ObjectPool(type, minimumPoolSize, maximumPoolSize, factoryMethod);

            return objectPool;
        }

        /// <summary>
        /// 注销对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void UnregisterObjectPool<T>() where T : IPooledObject
        {
            Type type = typeof(T);
            if (_objectPools.ContainsKey(type))
                _objectPools.Remove(type);
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IObjectPool GetObjectPool<T>() where T : IPooledObject
        {
            Type type = typeof(T);
            if (_objectPools.TryGetValue(type, out IObjectPool objectPool))
                return objectPool;

            return null;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetObject<T>() where T : IPooledObject
        {
            IObjectPool objectPool = GetObjectPool<T>();
            if (objectPool == null)
                objectPool = RegisterObjectPool<T>();

            return (T)objectPool.Get();
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pooledObject"></param>
        public void ReleaseObject(IPooledObject pooledObject)
        {
            Type type = pooledObject.GetType();
            IObjectPool objectPool = GetObjectPool(type);
            if (objectPool == null)
                objectPool = RegisterObjectPool(type);

            objectPool.Release(pooledObject);
        }
    }
}
