using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class SimpleParticleGenerator : IParticleGenerator
    {
        public virtual Particle GenerateParticle()
        {
            return new Particle();
        }

        public virtual void Reset()
        {

        }
    }
}
