using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    public class SkillDefine
    {
        public static Dictionary<int, SkillCondition> skillConditions;
        public static Dictionary<int, SkillAction> skillActions;

        // Condition
        public static int SC_Attribute = 1;
        public static int SC_Distance = 2;
        public static int SC_Item = 3;

        // Action
        public static int SA_PlayAnimation = 1;
        public static int SA_PlayEffect = 2;
        public static int SA_PlaySound = 3;
        public static int SA_AddBuff = 4;
        public static int SA_RemoveBuff = 5;
        public static int SA_Camera = 6;
        public static int SA_Move = 7;
        public static int SA_ShowRange = 8;
        public static int SA_ShowDamage = 9;


        public static void Init()
        {
            skillConditions = new Dictionary<int, SkillCondition>();
            skillActions = new Dictionary<int, SkillAction>();
            InitConditions();
            InitActions();
        }

        private static void InitConditions()
        {
            skillConditions[SC_Attribute] = new SC_Attribute();
            skillConditions[SC_Distance] = new SC_Distance();
            skillConditions[SC_Item] = new SC_Item();
        }

        private static void InitActions()
        {
            skillActions[SA_PlayAnimation] = new SA_PlayAnimation();
            skillActions[SA_PlayEffect] = new SA_PlayEffect();
            skillActions[SA_PlaySound] = new SA_PlaySound();
            skillActions[SA_AddBuff] = new SA_AddBuff();
            skillActions[SA_RemoveBuff] = new SA_RemoveBuff();
            skillActions[SA_Camera] = new SA_Camera();
            skillActions[SA_Move] = new SA_Move();
            skillActions[SA_ShowRange] = new SA_ShowRange();
            skillActions[SA_ShowDamage] = new SA_ShowDamage();
        }

        /// <summary>
        /// 获取条件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SkillCondition GetCondition(int id)
        {
            SkillCondition condition;
            if (skillConditions.TryGetValue(id, out condition))
            {
                return condition;
            }
            return null;
        }

        /// <summary>
        /// 获取动作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SkillAction GetAction(int id)
        {
            SkillAction action;
            if (skillActions.TryGetValue(id, out action))
            {
                return action;
            }
            return null;
        }
    }
}
