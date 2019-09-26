using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class SkillHelper
    {

        /// <summary>
        /// 根据脚本创建技能
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public static SkillEntity CreateSkillFromScript(string script)
        {
            SkillEntity entity = new SkillEntity();
            return entity;
        }

        /// <summary>
        /// 创建技能动作实体
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static SkillActionEntity CreateSkillActionEntity(SkillAction action)
        {
            SkillActionEntity entity = new SkillActionEntity();
            entity.action = action;
            return entity;
        }
    }
}
