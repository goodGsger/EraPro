using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public delegate IPooledObject ObjectPoolFactoryMethod();

    public interface IObjectPool
    {
        /// <summary>
        /// 对象池数量上限
        /// </summary>
        int maximumPoolSize { get; set; }

        /// <summary>
        /// 对象池数量下限
        /// </summary>
        int minimumPoolSize { get; set; }

        /// <summary>
        /// 当前对象数量
        /// </summary>
        int poolSize { get; }

        /// <summary>
        /// 工厂方法
        /// </summary>
        ObjectPoolFactoryMethod factoryMethod { get; }

        /// <summary>
        /// 从对象池中获取对象
        /// </summary>
        /// <returns></returns>
        IPooledObject Get();

        /// <summary>
        /// 将对象放入对象池
        /// </summary>
        /// <param name="pooledObject"></param>
        void Release(IPooledObject pooledObject);

        /// <summary>
        /// 清理对象池
        /// </summary>
        void Clear();
    }

    public interface IObjectPool<T> where T : IPooledObject
    {
        /// <summary>
        /// 对象池数量上限
        /// </summary>
        int maximumPoolSize { get; set; }

        /// <summary>
        /// 对象池数量下限
        /// </summary>
        int minimumPoolSize { get; set; }

        /// <summary>
        /// 当前对象数量
        /// </summary>
        int poolSize { get; }

        /// <summary>
        /// 工厂方法
        /// </summary>
        Func<T> factoryMethod { get; }

        /// <summary>
        /// 从对象池中获取对象
        /// </summary>
        /// <returns></returns>
        T Get();

        /// <summary>
        /// 将对象放入对象池
        /// </summary>
        /// <param name="pooledObject"></param>
        void Release(T pooledObject);

        /// <summary>
        /// 清理对象池
        /// </summary>
        void Clear();
    }
}
