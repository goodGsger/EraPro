using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class RangeLimitParticleCleaner : IParticleCleaner
    {
        protected Vector2 _rangeX;
        protected Vector2 _rangeY;
        protected Vector2 _rangeZ;

        public virtual bool CleanParticle(Particle particle)
        {
            if (_rangeX.x != 0 && _rangeX.y != 0 && particle.position.x < _rangeX.x || particle.position.y > _rangeX.y)
                particle.isAlive = false;
            else if (_rangeY.x != 0 && _rangeY.y != 0 && particle.position.x < _rangeY.x || particle.position.y > _rangeY.y)
                particle.isAlive = false;
            else if (_rangeZ.x != 0 && _rangeZ.y != 0 && particle.position.x < _rangeZ.x || particle.position.y > _rangeZ.y)
                particle.isAlive = false;
            return !particle.isAlive;
        }

        public virtual void SetRangeX(Vector2 range)
        {
            _rangeX = range;
        }

        public virtual void SetRangeY(Vector2 range)
        {
            _rangeY = range;
        }

        public virtual void SetRangeZ(Vector2 range)
        {
            _rangeZ = range;
        }

        public virtual void Reset()
        {

        }
    }
}
