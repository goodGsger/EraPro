using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface IParticleCleaner
    {
        /// <summary>
        /// 清除粒子
        /// </summary>
        /// <param name="particle"></param>
        /// <returns></returns>
        bool CleanParticle(Particle particle);

        /// <summary>
        /// 重置
        /// </summary>
        void Reset();
    }
}
