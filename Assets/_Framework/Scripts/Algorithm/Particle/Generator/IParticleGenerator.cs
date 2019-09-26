using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface IParticleGenerator
    {
        /// <summary>
        /// 生成粒子
        /// </summary>
        /// <returns></returns>
        Particle GenerateParticle();

        /// <summary>
        /// 重置
        /// </summary>
        void Reset();
    }
}
