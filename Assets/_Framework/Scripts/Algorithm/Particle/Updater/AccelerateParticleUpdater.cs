using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class AccelerateParticleUpdater : IParticleUpdater
    {
        protected Vector3 _acceleration;

        public virtual void UpdateParticle(Particle particle, float deltaTime)
        {
            if (particle.pause)
                return;
            if (_acceleration.Equals(Vector3.zero))
                return;
            Vector3 v = _acceleration * deltaTime;
            Vector3 d = 0.5f * v * deltaTime;
            particle.position += d;
            particle.velocity += v;
        }

        public virtual void SetAcceleration(Vector3 value)
        {
            _acceleration = value;
        }

        public virtual void Reset()
        {

        }
    }
}
