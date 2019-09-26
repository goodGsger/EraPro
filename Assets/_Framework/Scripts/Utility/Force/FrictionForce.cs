using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class FrictionForce : AForce
    {
        private Vector3 _sign = new Vector3();

        public FrictionForce()
        {

        }

        public FrictionForce(Vector3 speed, Vector3 frictionForce)
        {
            this.speed = speed;
            acceleration = frictionForce;
        }

        public override Vector3 speed
        {
            get { return base.speed; }
            set
            {
                base.speed = value;
                RefreshSign();
            }
        }

        private int GetSign(float speed)
        {
            if (speed == 0)
                return 0;

            return speed > 0 ? -1 : 1;
        }

        private void RefreshSign()
        {
            _sign.x = GetSign(_speed.x);
            _sign.y = GetSign(_speed.y);
            _sign.z = GetSign(_speed.z);
        }

        /// <summary>
        /// 摩擦力
        /// </summary>
        public Vector3 frictionForce
        {
            get { return _acceleration; }
            set { _acceleration = value; }
        }

        /// <summary>
        /// 是否已停止
        /// </summary>
        public bool stoped
        {
            get { return _speed.Equals(Vector3.zero); }
        }

        /// <summary>
        /// 更新速度
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public override Vector3 UpdateSpeed(float time)
        {
            if (stoped)
                return Vector3.zero;

            Vector3 dist = base.UpdateSpeed(time);
            dist.Scale(_sign);

            if (_sign.x > 0 && dist.x <= 0 || _sign.x < 0 && dist.x >= 0)
            {
                _speed.x = 0;
                dist.x = 0;
            }
            if (_sign.y > 0 && dist.y <= 0 || _sign.y < 0 && dist.y >= 0)
            {
                _speed.y = 0;
                dist.y = 0;
            }
            if (_sign.z > 0 && dist.z <= 0 || _sign.z < 0 && dist.z >= 0)
            {
                _speed.z = 0;
                dist.z = 0;
            }

            RefreshSign();
            return dist;
        }
    }
}
