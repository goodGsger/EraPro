using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class Particle : IPooledObject
    {
        private float _age;
        private float _maxAge = -1;
        private bool _isAlive;
        private bool _pause;

        public Vector3 rotation;
        public Vector3 velocity;
        public Vector3 originPosition;
        public Vector3 position;
        public Vector3 lastPosition;

        /// <summary>
        /// 粒子年龄
        /// </summary>
        public virtual float age
        {
            get { return _age; }
            set { _age = value; }
        }

        /// <summary>
        /// 最大粒子年龄
        /// </summary>
        public virtual float maxAge
        {
            get { return _maxAge; }
            set { _maxAge = value; }
        }

        /// <summary>
        /// 是否存活
        /// </summary>
        public virtual bool isAlive
        {
            get { return _isAlive; }
            set { _isAlive = value; }
        }

        /// <summary>
        /// 是否暂停
        /// </summary>
        public virtual bool pause
        {
            get { return _pause; }
            set { _pause = value; }
        }

        /// <summary>
        /// 缓存前一位置
        /// </summary>
        public virtual void BakLastPosition()
        {
            lastPosition = position;
        }

        public virtual void OnPoolGet()
        {
            _isAlive = true;
        }

        public virtual void OnPoolReset()
        {
            _age = 0;
            _maxAge = -1;
            _isAlive = false;
            _pause = false;

            rotation = Vector3.zero;
            velocity = Vector3.zero;
        }

        public virtual void OnPoolDispose()
        {

        }
    }
}
