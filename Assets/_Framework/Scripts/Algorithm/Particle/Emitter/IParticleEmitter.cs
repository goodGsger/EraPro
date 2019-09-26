using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public interface IParticleEmitter
    {
        /// <summary>
        /// 绑定粒子生成器
        /// </summary>
        /// <param name="generator"></param>
        void AttachGenerator(IParticleGenerator generator);

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="position"></param>
        void SetPosition(Vector3 position);

        /// <summary>
        /// 发射粒子
        /// </summary>
        /// <returns></returns>
        Particle EmitParticle();

        /// <summary>
        /// 重置
        /// </summary>
        void Reset();
    }
}
