using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    public class SkillEntity
    {
        private static List<SkillAction> startActions = new List<SkillAction>();
        private static List<SkillActionEntity> endActionEntities = new List<SkillActionEntity>();

        /// <summary>
        /// id
        /// </summary>
        public int id;

        /// <summary>
        /// 技能配置
        /// </summary>
        public SkillConfig config;

        /// <summary>
        /// 角色
        /// </summary>
        public Role role;

        /// <summary>
        /// 目标
        /// </summary>
        public Role target;

        /// <summary>
        /// 坐标
        /// </summary>
        public OffsetCoord position;

        /// <summary>
        /// 当前时间
        /// </summary>
        public float currentTime;

        /// <summary>
        /// 开始回调
        /// </summary>
        public Action<SkillEntity> startCallback;

        /// <summary>
        /// 结束回调
        /// </summary>
        public Action<SkillEntity> endCallback;

        /// <summary>
        /// 动作列表
        /// </summary>
        public List<SkillAction> actions;

        /// <summary>
        /// 当前正在执行的动作列表
        /// </summary>
        public List<SkillActionEntity> currentActionEntities;

        public SkillEntity()
        {
            actions = new List<SkillAction>();
            currentActionEntities = new List<SkillActionEntity>();
        }

        /// <summary>
        /// 是否已经结束
        /// </summary>
        public bool isEnd
        {
            get { return currentTime >= config.totalTime; }
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
        /// 添加动作实体
        /// </summary>
        /// <param name="actionEntity"></param>
        public void AddActionEntity(SkillActionEntity actionEntity)
        {
            actionEntity.startCallback = OnActionStart;
            actionEntity.endCallback = OnActionEnd;
            currentActionEntities.Add(actionEntity);
            actionEntity.Start();
        }

        /// <summary>
        /// 移除动作实体
        /// </summary>
        /// <param name="actionEntity"></param>
        public void RemoveActionEntity(SkillActionEntity actionEntity)
        {
            currentActionEntities.Remove(actionEntity);
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            actions.AddRange(config.actions);
            CheckActionsStart();
            if (endCallback != null)
            {
                startCallback(this);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            currentTime += deltaTime;

            foreach (SkillActionEntity actionEntity in currentActionEntities)
            {
                if (actionEntity.action.needUpdate)
                {
                    actionEntity.Update(deltaTime);
                }
            }

            CheckActionsEnd();
            CheckActionsStart();
        }

        /// <summary>
        /// 检查动作结束
        /// </summary>
        protected void CheckActionsEnd()
        {
            foreach (SkillActionEntity actionEntity in currentActionEntities)
            {
                if (actionEntity.isEnd)
                {
                    endActionEntities.Add(actionEntity);
                }
            }

            if (endActionEntities.Count > 0)
            {
                foreach (SkillActionEntity actionEntity in endActionEntities)
                {
                    currentActionEntities.Remove(actionEntity);
                    actionEntity.End();
                }
                endActionEntities.Clear();
            }
        }

        /// <summary>
        /// 检查动作开始
        /// </summary>
        protected void CheckActionsStart()
        {
            foreach (SkillAction action in actions)
            {
                if (currentTime >= action.startTime)
                {
                    startActions.Add(action);
                }
            }

            if (startActions.Count > 0)
            {
                foreach (SkillAction action in startActions)
                {
                    actions.Remove(action);
                    SkillActionEntity actionEntity = SkillHelper.CreateSkillActionEntity(action);
                    if (actionEntity != null)
                    {
                        AddActionEntity(actionEntity);
                    }
                }
                startActions.Clear();
            }
        }

        /// <summary>
        /// 结束
        /// </summary>
        public void End()
        {
            UnityEngine.Debug.Log("entity end");
            currentTime = 0;
            EndCurrentActions();
            if (endCallback != null)
            {
                endCallback(this);
            }
        }

        protected void EndCurrentActions()
        {
            foreach (SkillActionEntity actionEntity in currentActionEntities)
            {
                actionEntity.End();
            }
            currentActionEntities.Clear();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            EndCurrentActions();
            currentTime = 0;
            startCallback = null;
            endCallback = null;
        }

        /// <summary>
        /// 动作开始回调
        /// </summary>
        /// <param name="action"></param>
        protected void OnActionStart(SkillActionEntity actionEntity)
        {
            
        }

        /// <summary>
        /// 动作结束回调
        /// </summary>
        /// <param name="action"></param>
        protected void OnActionEnd(SkillActionEntity actionEntity)
        {
            actionEntity.startCallback = null;
            actionEntity.endCallback = null;
        }
    }
}
