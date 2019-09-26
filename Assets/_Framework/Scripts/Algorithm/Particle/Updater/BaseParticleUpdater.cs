using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class BaseParticleUpdater : IParticleUpdater
    {
        public virtual void UpdateParticle(Particle particle, float deltaTime)
        {
            if (particle.pause)
                return;
            particle.age += deltaTime;
            particle.BakLastPosition();
            particle.position += particle.velocity * deltaTime;
        }

        public virtual void Reset()
        {

        }
    }
}
