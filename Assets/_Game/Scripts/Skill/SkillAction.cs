using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public abstract class SkillAction
    {
        /// <summary>
        /// 是否需要更新
        /// </summary>
        public bool needUpdate;

        /// <summary>
        /// 总时间
        /// </summary>
        public float totalTime;

        /// <summary>
        /// 起始时间
        /// </summary>
        public float startTime;

        public virtual void Start(SkillActionEntity entity)
        {

        }

        public virtual void Update(SkillActionEntity entity, float delatTime)
        {

        }

        public virtual void End(SkillActionEntity entity)
        {

        }

        public virtual void Reset(SkillActionEntity entity)
        {

        }
    }
}
