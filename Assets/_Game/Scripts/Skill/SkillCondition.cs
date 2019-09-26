using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public abstract class SkillCondition
    {
        /// <summary>
        /// 检查
        /// </summary>
        /// <returns></returns>
        public virtual bool Check()
        {
            return false;
        }
    }
}
