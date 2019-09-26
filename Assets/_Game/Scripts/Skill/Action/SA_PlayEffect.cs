using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class SA_PlayEffect : SkillAction
    {
        public override void Start(SkillActionEntity entity)
        {
            UnityEngine.Debug.Log("SA_PlayEffect Start");
        }

        public override void End(SkillActionEntity entity)
        {
            UnityEngine.Debug.Log("SA_PlayEffect End");
        }
    }
}
