using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class RotateParticleUpdater : IParticleUpdater
    {
        public virtual void UpdateParticle(Particle particle, float deltaTime)
        {
            if (particle.pause)
                return;
        }

        public virtual void Reset()
        {

        }
    }
}
