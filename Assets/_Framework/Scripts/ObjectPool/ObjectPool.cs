using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class ObjectPool : IObjectPool
    {
        /// <summary>
        /// 类型
        /// </summary>
        private Type _type;

        /// <summary>
        /// 对象池数量上限
        /// </summary>
        private int _maximumPoolSize;

        /// <summary>
        /// 对象池数量下限
        /// </summary>
        private int _minimumPoolSize;

        /// <summary>
        /// 当前对象数量
        /// </summary>
        private int _poolSize;

        /// <summary>
        /// 池对象队列
        /// </summary>
        private Queue<IPooledObject> _pooledObjects;

        /// <summary>
        /// 工厂方法
        /// </summary>
        private ObjectPoolFactoryMethod _factoryMethod;

        public ObjectPool(Type type) : this(type, 0, int.MaxValue, null)
        {

        }

        public ObjectPool(Type type, int minimumPoolSize, int maximumPoolSize, ObjectPoolFactoryMethod factoryMethod)
        {
            _type = type;
            _minimumPoolSize = minimumPoolSize;
            _maximumPoolSize = maximumPoolSize;
            _factoryMethod = factoryMethod;

            _pooledObjects = new Queue<IPooledObject>();
            AdjustMinimumPoolSize();
            AdjustMaximumPoolSize();
        }

        public int maximumPoolSize
        {
            get { return _maximumPoolSize; }
            set
            {
                _maximumPoolSize = value;
                AdjustMaximumPoolSize();
            }
        }

        public int minimumPoolSize
        {
            get { return _minimumPoolSize; }
            set
            {
                _minimumPoolSize = value;
                AdjustMinimumPoolSize();
            }
        }

        public int poolSize
        {
            get { return _poolSize; }
        }

        public ObjectPoolFactoryMethod factoryMethod
        {
            get { return _factoryMethod; }
        }

        /// <summary>
        /// 从对象池中获取对象
        /// </summary>
        /// <returns></returns>
        public IPooledObject Get()
        {
            IPooledObject pooledObject;
            Dequeue(out pooledObject);
            pooledObject.OnPoolGet();
            return pooledObject;
        }

        /// <summary>
        /// 将对象放入对象池
        /// </summary>
        /// <param name="pooledObject"></param>
        public void Release(IPooledObject pooledObject)
        {
            if (Enqueue(pooledObject))
                pooledObject.OnPoolReset();
            else
                DestroyPooledObject(pooledObject);
        }

        /// <summary>
        /// 清理对象池
        /// </summary>
        public void Clear()
        {
            foreach (var pooledObject in _pooledObjects)
                DestroyPooledObject(pooledObject);

            _pooledObjects.Clear();
            _poolSize = 0;

            AdjustMinimumPoolSize();
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        /// <param name="pooledObject"></param>
        private void DestroyPooledObject(IPooledObject pooledObject)
        {
            pooledObject.OnPoolDispose();
        }

        /// <summary>
        /// 调整对象池数量下限
        /// </summary>
        private void AdjustMinimumPoolSize()
        {
            while (_poolSize < _minimumPoolSize && _pooledObjects.Count < _maximumPoolSize)
                Enqueue(CreateObject());
        }

        /// <summary>
        /// 调整对象池数量上限
        /// </summary>
        private void AdjustMaximumPoolSize()
        {
            IPooledObject pooledObject;
            while (_poolSize > _maximumPoolSize && _pooledObjects.Count > _maximumPoolSize && Dequeue(out pooledObject))
                DestroyPooledObject(pooledObject);
        }

        /// <summary>
        /// 对象入队
        /// </summary>
        /// <param name="pooledObject"></param>
        /// <returns></returns>
        private bool Enqueue(IPooledObject pooledObject)
        {
            if (_pooledObjects.Count == _maximumPoolSize)
                return false;

            _pooledObjects.Enqueue(pooledObject);
            _poolSize++;
            return true;
        }

        /// <summary>
        /// 对象出队
        /// </summary>
        /// <param name="pooledObject"></param>
        /// <returns></returns>
        private bool Dequeue(out IPooledObject pooledObject)
        {
            if (_pooledObjects.Count == 0)
            {
                pooledObject = CreateObject();
                return false;
            }

            pooledObject = _pooledObjects.Dequeue();
            _poolSize--;

            if (_poolSize < _minimumPoolSize)
                Enqueue(CreateObject());

            return true;
        }

        /// <summary>
        /// 创建新对象
        /// </summary>
        /// <returns></returns>
        private IPooledObject CreateObject()
        {
            IPooledObject newObject;
            try
            {
                newObject = factoryMethod != null ? factoryMethod.Invoke() : Activator.CreateInstance(_type) as IPooledObject;
            }
            catch (Exception e)
            {

                throw;
            }

            return newObject;
        }
    }

    public class ObjectPool<T> : IObjectPool<T> where T : IPooledObject
    {
        /// <summary>
        /// 对象池数量上限
        /// </summary>
        private int _maximumPoolSize;

        /// <summary>
        /// 对象池数量下限
        /// </summary>
        private int _minimumPoolSize;

        /// <summary>
        /// 当前对象数量
        /// </summary>
        private int _poolSize;

        /// <summary>
        /// 池对象队列
        /// </summary>
        private Queue<T> _pooledObjects;

        /// <summary>
        /// 工厂方法
        /// </summary>
        private Func<T> _factoryMethod;

        public ObjectPool() : this(0, int.MaxValue, null)
        {

        }

        public ObjectPool(int minimumPoolSize, int maximumPoolSize, Func<T> factoryMethod)
        {
            _minimumPoolSize = minimumPoolSize;
            _maximumPoolSize = maximumPoolSize;
            _factoryMethod = factoryMethod;

            _pooledObjects = new Queue<T>();
            AdjustMinimumPoolSize();
            AdjustMaximumPoolSize();
        }

        public int maximumPoolSize
        {
            get { return _maximumPoolSize; }
            set
            {
                _maximumPoolSize = value;
                AdjustMaximumPoolSize();
            }
        }

        public int minimumPoolSize
        {
            get { return _minimumPoolSize; }
            set
            {
                _minimumPoolSize = value;
                AdjustMinimumPoolSize();
            }
        }

        public int poolSize
        {
            get { return _poolSize; }
        }

        public Func<T> factoryMethod
        {
            get { return _factoryMethod; }
        }

        /// <summary>
        /// 从对象池中获取对象
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            T pooledObject;
            Dequeue(out pooledObject);
            pooledObject.OnPoolGet();
            return pooledObject;
        }

        /// <summary>
        /// 将对象放入对象池
        /// </summary>
        /// <param name="pooledObject"></param>
        public void Release(T pooledObject)
        {
            if (Enqueue(pooledObject))
                pooledObject.OnPoolReset();
            else
                DestroyPooledObject(pooledObject);
        }

        /// <summary>
        /// 清理对象池
        /// </summary>
        public void Clear()
        {
            foreach (var pooledObject in _pooledObjects)
                DestroyPooledObject(pooledObject);

            _pooledObjects.Clear();
            _poolSize = 0;

            AdjustMinimumPoolSize();
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        /// <param name="pooledObject"></param>
        private void DestroyPooledObject(T pooledObject)
        {
            pooledObject.OnPoolDispose();
        }

        /// <summary>
        /// 调整对象池数量下限
        /// </summary>
        private void AdjustMinimumPoolSize()
        {
            while (_poolSize < _minimumPoolSize && _pooledObjects.Count < _maximumPoolSize)
                Enqueue(CreateObject());
        }

        /// <summary>
        /// 调整对象池数量上限
        /// </summary>
        private void AdjustMaximumPoolSize()
        {
            T pooledObject;
            while (_poolSize > _maximumPoolSize && _pooledObjects.Count > _maximumPoolSize && Dequeue(out pooledObject))
                DestroyPooledObject(pooledObject);
        }

        /// <summary>
        /// 对象入队
        /// </summary>
        /// <param name="pooledObject"></param>
        /// <returns></returns>
        private bool Enqueue(T pooledObject)
        {
            if (_pooledObjects.Count == _maximumPoolSize)
                return false;

            _pooledObjects.Enqueue(pooledObject);
            _poolSize++;
            return true;
        }

        /// <summary>
        /// 对象出队
        /// </summary>
        /// <param name="pooledObject"></param>
        /// <returns></returns>
        private bool Dequeue(out T pooledObject)
        {
            if (_pooledObjects.Count == 0)
            {
                pooledObject = CreateObject();
                return false;
            }

            pooledObject = _pooledObjects.Dequeue();
            _poolSize--;

            if (_poolSize < _minimumPoolSize)
                Enqueue(CreateObject());

            return true;
        }

        /// <summary>
        /// 创建新对象
        /// </summary>
        /// <returns></returns>
        private T CreateObject()
        {
            T newObject;
            try
            {
                newObject = factoryMethod != null ? factoryMethod.Invoke() : Activator.CreateInstance<T>();
            }
            catch (Exception e)
            {

                throw;
            }

            return newObject;
        }
    }
}
