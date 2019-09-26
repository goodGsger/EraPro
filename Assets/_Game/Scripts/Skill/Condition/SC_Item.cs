using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class SC_Item : SkillCondition
    {
        /// <summary>
        /// 物品ID
        /// </summary>
        public int itemID;

        /// <summary>
        /// 物品数量
        /// </summary>
        public int count;

        public override bool Check()
        {
            return SkillManager.inst.skillProxy.HasItem(itemID, count);
        }
    }
}
