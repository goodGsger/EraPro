using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class SC_Attribute : SkillCondition
    {
        /// <summary>
        /// 属性ID
        /// </summary>
        public int attrID;

        /// <summary>
        /// 属性值
        /// </summary>
        public int value;

        public override bool Check()
        {
            return SkillManager.inst.skillProxy.GetAttribute(attrID) >= value;
        }
    }
}
