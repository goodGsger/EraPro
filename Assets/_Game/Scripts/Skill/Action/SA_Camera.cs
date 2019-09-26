using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class SA_Camera : SkillAction
    {
        public override void Start(SkillActionEntity entity)
        {
            UnityEngine.Debug.Log("SA_Camera Start");
        }

        public override void Update(SkillActionEntity entity, float delatTime)
        {
            UnityEngine.Debug.Log("SA_Camera Update:" + entity.currentTime);
        }

        public override void End(SkillActionEntity entity)
        {
            UnityEngine.Debug.Log("SA_Camera End");
        }

        public override void Reset(SkillActionEntity entity)
        {
            UnityEngine.Debug.Log("SA_Camera Reset");
        }
    }
}
