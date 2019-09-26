using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class SkillActionEntity
    {
        /// <summary>
        /// 技能动作
        /// </summary>
        public SkillAction action;

        /// <summary>
        /// 是否已执行过
        /// </summary>
        public bool started;

        /// <summary>
        /// 当前时间
        /// </summary>
        public float currentTime;

        /// <summary>
        /// 开始回调
        /// </summary>
        public Action<SkillActionEntity> startCallback;

        /// <summary>
        /// 结束回调
        /// </summary>
        public Action<SkillActionEntity> endCallback;

        /// <summary>
        /// 是否已经结束
        /// </summary>
        public bool isEnd
        {
            get { return currentTime >= action.totalTime; }
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            started = true;
            action.Start(this);
            startCallback(this);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            currentTime += deltaTime;
            action.Update(this, deltaTime);
        }

        /// <summary>
        /// 结束
        /// </summary>
        public void End()
        {
            action.End(this);
            endCallback(this);
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            started = false;
            currentTime = 0;
            startCallback = null;
            endCallback = null;
            action.Reset(this);
        }
    }
}
