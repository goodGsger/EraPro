using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class PointParticleEmitter : IParticleEmitter
    {
        protected Vector3 _position;
        protected Vector3 _velocity;
        protected IParticleGenerator _generator;

        /// <summary>
        /// 绑定粒子生成器
        /// </summary>
        /// <param name="generator"></param>
        public virtual void AttachGenerator(IParticleGenerator generator)
        {
            _generator = generator;
        }

        /// <summary>
        /// 设置发射器位置
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetPosition(Vector3 value)
        {
            _position = value;
        }

        /// <summary>
        /// 发射粒子
        /// </summary>
        /// <returns></returns>
        public virtual Particle EmitParticle()
        {
            if (_generator == null)
                return null;

            Particle particle = _generator.GenerateParticle();
            particle.originPosition = _position;
            particle.position = _position;
            particle.velocity = _velocity;
            return particle;
        }

        /// <summary>
        /// 设置发射器速度
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetVelocity(Vector3 value)
        {
            _velocity = value;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset()
        {

        }
    }
}
