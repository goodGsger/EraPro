using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class ParticleRenderer3D : IPooledObject
    {
        protected ActionDirection _direction;
        protected float _time;
        protected float _maxTime;
        protected string _sortingLayer;
        protected int _sortingIndex;
        protected int _sortingOrder = int.MinValue;
        protected int _depth;
        protected bool _loop;
        protected bool _lockSorting;

        protected Action _callback;

        protected ActionData3D _actionData;
        protected GameObject _gameObject;
        protected Transform _transform;
        protected ParticleSystem _particleSystem;
        protected ParticleSystem[] _particleSystemList;
        protected List<Renderer> _particleRendererList;
        protected Animation[] _animationList;
        protected List<AnimationState> _animationStateList;

        public ParticleRenderer3D()
        {
            _particleRendererList = new List<Renderer>(10);
        }

        /// <summary>
        /// 方向
        /// </summary>
        public virtual ActionDirection direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        /// <summary>
        /// 当前时间
        /// </summary>
        public virtual float time
        {
            get { return _time; }
            set { _time = value; }
        }

        /// <summary>
        /// 总时间
        /// </summary>
        public virtual float maxTime
        {
            get { return _maxTime; }
            set { _maxTime = value; }
        }

        /// <summary>
        /// actionData
        /// </summary>
        public virtual ActionData3D actionData
        {
            get { return _actionData; }
            set
            {
                if (_actionData != value)
                {
                    if (_actionData != null)
                    {
                        ResetActionData();
                    }

                    _actionData = value;

                    if (_actionData != null)
                    {
                        _gameObject = _actionData.gameObject;
                        _transform = _actionData.gameObject.transform;
                        _particleSystem = _actionData.gameObject.GetComponent<ParticleSystem>();
                        _particleSystemList = _actionData.gameObject.GetComponentsInChildren<ParticleSystem>();
                        _particleRendererList.Clear();
                        foreach (ParticleSystem system in _particleSystemList)
                        {
                            Renderer renderer = system.GetComponent<Renderer>();
                            if (renderer != null)
                                _particleRendererList.Add(renderer);
                        }
                        sortingLayer = _sortingLayer;
                        RefreshSortingOrder();

                        _animationList = _actionData.gameObject.GetComponentsInChildren<Animation>();
                        //_animationStateList = new List<AnimationState>();
                        //foreach (Animation animation in _animationList)
                        //{
                        //    if (animation.clip != null)
                        //        _animationStateList.Add(animation[animation.clip.name]);
                        //}
                        Stop();
                    }
                    else
                    {
                        _gameObject = null;
                        _transform = null;
                        _particleSystem = null;
                        _particleSystemList = null;
                        _particleRendererList.Clear();
                        _animationList = null;
                        _animationStateList = null;
                    }
                }
            }
        }

        /// <summary>
        /// gameObject
        /// </summary>
        public virtual GameObject gameObject
        {
            get { return _gameObject; }
        }

        /// <summary>
        /// transform
        /// </summary>
        public virtual Transform transform
        {
            get { return _transform; }
        }

        /// <summary>
        /// particleSystem
        /// </summary>
        public virtual ParticleSystem particleSystem
        {
            get { return _particleSystem; }
        }

        /// <summary>
        /// particleSystemList
        /// </summary>
        public virtual ParticleSystem[] particleSystemList
        {
            get { return _particleSystemList; }
        }

        /// <summary>
        /// animationList
        /// </summary>
        public virtual Animation[] animationList
        {
            get { return _animationList; }
        }

        /// <summary>
        /// 播放
        /// </summary>
        public virtual void Play()
        {
            if (_particleSystemList != null)
            {
                foreach (ParticleSystem system in _particleSystemList)
                    system.Play();
            }

            if (_animationList != null)
            {
                foreach (Animation animation in _animationList)
                    animation.Play();
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public virtual void Stop()
        {
            if (_particleSystemList != null)
            {
                foreach (ParticleSystem system in _particleSystemList)
                    system.Play();
            }

            if (_animationList != null)
            {
                foreach (Animation animation in _animationList)
                    animation.Stop();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="time"></param>
        public virtual void Update(float deltaTime, float time)
        {
            _time = time;
            //if (_particleSystemList != null)
            //{
            //    foreach (ParticleSystem system in _particleSystemList)
            //    {
            //        system.Simulate(deltaTime, false, false);
            //    }
            //}

            //if (_animationStateList != null)
            //{
            //    foreach (AnimationState state in _animationStateList)
            //    {
            //        state.time = _time;
            //    }
            //}
        }

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="position"></param>
        public virtual void SetPosition(Vector3 position)
        {
            SetPosition(position.x, position.y, position.z);
        }

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public virtual void SetPosition(float x = 0, float y = 0, float z = 0)
        {
            if (_transform != null)
                _transform.localPosition = new Vector3(x, -y, z);
        }

        /// <summary>
        /// 重置游戏对象
        /// </summary>
        public virtual void ResetActionData()
        {
            _gameObject = null;
            _transform = null;
            if (_particleSystem != null)
            {
                _particleSystem.Stop(true);
                _particleSystem = null;
            }
            if (_animationList != null)
            {
                foreach (Animation animation in _animationList)
                    animation.Stop();
                _animationList = null;
            }
            _particleSystemList = null;
            _particleRendererList.Clear();
            _animationStateList = null;
            if (_actionData != null)
            {
                Action3DManager.inst.ReleaseActionData(_actionData);
                _actionData = null;
            }
        }

        /// <summary>
        /// 刷新排序
        /// </summary>
        protected virtual void RefreshSortingOrder()
        {
            if (_lockSorting)
                return;

            if (_particleSystemList != null)
            {
                int index = 0;
                if (_sortingOrder == int.MinValue)
                    index = _sortingIndex * 10 + _depth;
                else
                    index = _sortingOrder;

                foreach (Renderer renderer in _particleRendererList)
                {
                    renderer.sortingOrder = index;
                }
            }
        }

        /// <summary>
        /// 排序层级
        /// </summary>
        public virtual string sortingLayer
        {
            get { return _sortingLayer; }
            set
            {
                _sortingLayer = value;
                if (_particleSystemList != null)
                {
                    foreach (Renderer renderer in _particleRendererList)
                    {
                        renderer.sortingLayerName = value;
                    }
                }
            }
        }

        /// <summary>
        /// 深度（排序用）
        /// </summary>
        public virtual int sortingIndex
        {
            get { return _sortingIndex; }
            set
            {
                _sortingIndex = value;
                RefreshSortingOrder();
            }
        }

        /// <summary>
        /// 强制深度
        /// </summary>
        public virtual int sortingOrder
        {
            get { return _sortingOrder; }
            set
            {
                _sortingOrder = value;
                RefreshSortingOrder();
            }
        }

        /// <summary>
        /// 深度
        /// </summary>
        public virtual int depth
        {
            get { return _depth; }
            set
            {
                _depth = value;
                RefreshSortingOrder();
            }
        }

        /// <summary>
        /// 是否循环
        /// </summary>
        public virtual bool loop
        {
            get { return _loop; }
            set
            {
                _loop = value;
                if (_particleSystemList != null)
                {
                    foreach (ParticleSystem system in _particleSystemList)
                    {
                        ParticleSystem.MainModule mainModule = system.main;
                        mainModule.loop = value;
                    }
                }
                if (_animationList != null)
                {
                    foreach (Animation animation in _animationList)
                    {
                        animation.wrapMode = _loop ? WrapMode.Loop : WrapMode.Once;
                    }
                }
            }
        }

        /// <summary>
        /// 锁定排序（不进行排序）
        /// </summary>
        public virtual bool lockSorting
        {
            get { return _lockSorting; }
            set { _lockSorting = value; }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset()
        {
            _direction = ActionDirection.NONE;
            _time = 0f;
            _maxTime = 0f;
            _sortingLayer = null;
            _sortingIndex = 0;
            _sortingOrder = int.MinValue;
            _depth = 0;
            _loop = false;
            _lockSorting = false;
            _callback = null;
            ResetActionData();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Dispose()
        {
            Reset();
            _particleSystem = null;
            _particleSystemList = null;
            _particleRendererList = null;
            _animationList = null;
            _animationStateList = null;
        }

        /// <summary>
        /// 从对象池中获取
        /// </summary>
        public virtual void OnPoolGet()
        {

        }

        /// <summary>
        /// 重置对象池对象
        /// </summary>
        public virtual void OnPoolReset()
        {
            Reset();
        }

        /// <summary>
        /// 销毁对象池对象
        /// </summary>
        public virtual void OnPoolDispose()
        {
            Dispose();
        }
    }
}
