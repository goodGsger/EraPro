using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface IParticleUpdater
    {

        /// <summary>
        /// 更新粒子
        /// </summary>
        /// <param name="particle"></param>
        /// <param name="deltaTime"></param>
        void UpdateParticle(Particle particle, float deltaTime);

        /// <summary>
        /// 重置
        /// </summary>
        void Reset();
    }
}
