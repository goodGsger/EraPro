using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class DistanceLimitParticleCleaner : IParticleCleaner
    {
        protected Vector3 _distance;

        public virtual bool CleanParticle(Particle particle)
        {
            if (_distance.x > 0.1f && Math.Abs(particle.position.x - particle.originPosition.x) >= _distance.x)
                particle.isAlive = false;
            else if (_distance.y > 0.1f && Math.Abs(particle.position.y - particle.originPosition.y) >= _distance.y)
                particle.isAlive = false;
            else if (_distance.z > 0.1f && Math.Abs(particle.position.z - particle.originPosition.z) >= _distance.z)
                particle.isAlive = false;
            return !particle.isAlive;
        }

        public virtual void SetDistance(Vector3 value)
        {
            _distance = value;
        }

        public virtual void Reset()
        {

        }
    }
}
