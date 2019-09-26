using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class FrictionParticleUpdater : IParticleUpdater
    {
        protected Vector3 _friction;

        public virtual void UpdateParticle(Particle particle, float deltaTime)
        {
            if (particle.pause)
                return;
            if (_friction.Equals(Vector3.zero))
                return;
            Vector3 v = _friction * deltaTime;
            Vector3 d = 0.5f * v * deltaTime;
        }

        public virtual void SetFriction(Vector3 value)
        {
            _friction = value;
        }

        public virtual void Reset()
        {

        }
    }
}
