using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class GravityParticleUpdater : IParticleUpdater
    {
        protected float _gravity;

        public virtual void UpdateParticle(Particle particle, float deltaTime)
        {
            if (particle.pause)
                return;
            if (_gravity == 0)
                return;
            if (particle.position.z > 0)
            {
                Vector3 position = particle.position;
                float v = _gravity * deltaTime;
                float z = position.z - 0.5f * v * deltaTime;
                if (z < 0)
                    z = 0;
                particle.position.z = z;
                particle.velocity.z -= v;
            }
        }

        public virtual void SetGravity(float value)
        {
            _gravity = value;
        }

        public virtual void Reset()
        {

        }
    }
}
