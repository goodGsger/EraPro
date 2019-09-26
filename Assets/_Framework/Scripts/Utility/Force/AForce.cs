using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class AForce
    {
        protected Vector3 _speed = new Vector3();
        protected Vector3 _acceleration = new Vector3();

        public AForce()
        {

        }

        public AForce(Vector3 speed, Vector3 acceleration)
        {
            this.speed = speed;
            this.acceleration = acceleration;
        }

        /// <summary>
        /// 速度
        /// </summary>
        public virtual Vector3 speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        /// <summary>
        /// 加速度
        /// </summary>
        public virtual Vector3 acceleration
        {
            get { return _acceleration; }
            set { _acceleration = value; }
        }

        /// <summary>
        /// 更新速度
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public virtual Vector3 UpdateSpeed(float time)
        {
            Vector3 dist = _speed * time + 0.5f * _acceleration * time * time;
            _speed += _acceleration * time;

            return dist;
        }
    }
}
