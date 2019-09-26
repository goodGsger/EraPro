using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class SkillConfig
    {
        /// <summary>
        /// id
        /// </summary>
        public int id;

        /// <summary>
        /// 总时间
        /// </summary>
        public float totalTime;

        /// <summary>
        /// 条件列表
        /// </summary>
        public List<SkillCondition> conditions;

        /// <summary>
        /// 动作列表
        /// </summary>
        public List<SkillAction> actions;

        public SkillConfig()
        {
            conditions = new List<SkillCondition>();
            actions = new List<SkillAction>();
        }

        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="condition"></param>
        public void AddCondition(SkillCondition condition)
        {
            conditions.Add(condition);
        }

        /// <summary>
        /// 添加动作
        /// </summary>
        /// <param name="action"></param>
        public void AddAction(SkillAction action)
        {
            actions.Add(action);
        }

        /// <summary>
        /// 检查条件
        /// </summary>
        public bool CheckConditions()
        {
            foreach (SkillCondition entity in conditions)
            {
                if (!entity.Check())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
