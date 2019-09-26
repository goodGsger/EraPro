using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class GravityForce : AForce
    {
        public GravityForce()
        {

        }

        public GravityForce(Vector3 speed, Vector3 gravityForce)
        {
            this.speed = speed;
            acceleration = gravityForce;
        }

        /// <summary>
        /// 重力
        /// </summary>
        public Vector3 gravityForce
        {
            get { return _acceleration; }
            set { _acceleration = value; }
        }
    }
}
