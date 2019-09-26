using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class AgeLimitParticleCleaner : IParticleCleaner
    {
        protected float _ageLimit;

        public virtual bool CleanParticle(Particle particle)
        {
            if (particle.maxAge != -1)
            {
                if (particle.age >= particle.maxAge)
                    particle.isAlive = false;
            }

            if (_ageLimit > 0)
                if (particle.age >= _ageLimit)
                    particle.isAlive = false;

            return !particle.isAlive;
        }

        public void SetAgeLimit(float value)
        {
            _ageLimit = value;
        }

        public virtual void Reset()
        {

        }
    }
}
