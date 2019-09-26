using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class SkillManager
    {
        private static SkillManager _inst;

        private Dictionary<int, SkillEntity> _skillDict;
        private List<SkillEntity> _skillEntities;
        private List<SkillEntity> _endSkillEntities;
        private ISkillProxy _skillProxy;

        public SkillManager()
        {
            SkillDefine.Init();

            _skillDict = new Dictionary<int, SkillEntity>();
            _skillEntities = new List<SkillEntity>();
            _endSkillEntities = new List<SkillEntity>();
        }

        public static SkillManager inst
        {
            get
            {
                if (_inst == null)
                    _inst = new SkillManager();

                return _inst;
            }
        }

        /// <summary>
        /// 技能代理
        /// </summary>
        public ISkillProxy skillProxy
        {
            get { return _skillProxy; }
            set { _skillProxy = value; }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            foreach (SkillEntity entity in _skillEntities)
            {
                if (entity.isEnd)
                {
                    _endSkillEntities.Add(entity);
                }
                else
                {
                    entity.Update(deltaTime);
                }
            }

            foreach (SkillEntity entity in _endSkillEntities)
            {
                RemoveSkill(entity);
                entity.End();
            }
            _endSkillEntities.Clear();
        }

        /// <summary>
        /// 添加技能
        /// </summary>
        /// <param name="entity"></param>
        public void AddSkill(SkillEntity entity)
        {
            _skillDict.Add(entity.id, entity);
            _skillEntities.Add(entity);
        }

        /// <summary>
        /// 移除技能
        /// </summary>
        /// <param name="entity"></param>
        public void RemoveSkill(SkillEntity entity)
        {
            _skillDict.Remove(entity.id);
            _skillEntities.Remove(entity);
        }

        /// <summary>
        /// 是否存在技能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasSkill(int id)
        {
            return _skillDict.ContainsKey(id);
        }

        /// <summary>
        /// 获取技能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SkillEntity GetSkill(int id)
        {
            SkillEntity entity;
            if (_skillDict.TryGetValue(id, out entity))
            {
                return entity;
            }
            return null;
        }

        /// <summary>
        /// 对指定技能添加动作实体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        public void AddSkillAction(int id, SkillAction action)
        {
            SkillEntity entity = GetSkill(id);
            if (entity != null)
            {
                entity.AddAction(action);
            }
        }
    }
}
