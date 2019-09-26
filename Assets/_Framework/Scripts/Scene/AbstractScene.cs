using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class AbstractScene : EventDispatcher, IScene
    {
        protected string _sceneName;
        protected SceneState _state;
        protected List<IModule> _modules;

        protected Action<IScene> _enterCallback;
        protected Action<IScene> _exitCallback;

        public AbstractScene(string sceneName)
        {
            _modules = new List<IModule>();
            _sceneName = sceneName;
            Init();
        }

        /// <summary>
        /// 场景名称
        /// </summary>
        public string sceneName
        {
            get { return _sceneName; }
            set { _sceneName = value; }
        }

        /// <summary>
        /// 场景状态
        /// </summary>
        public SceneState state
        {
            get { return _state; }
            set { _state = value; }
        }

        public List<IModule> modules
        {
            get { return _modules; }
        }

        /// <summary>
        /// 场景进入回调
        /// </summary>
        public Action<IScene> enterCallback
        {
            get { return _enterCallback; }
            set { _enterCallback = value; }
        }

        /// <summary>
        /// 场景退出回调
        /// </summary>
        public Action<IScene> exitCallback
        {
            get { return _exitCallback; }
            set { _exitCallback = value; }
        }

        protected virtual void Init()
        {

        }

        /// <summary>
        /// 进入场景
        /// </summary>
        public virtual void EnterScene()
        {

        }

        /// <summary>
        /// 退出场景
        /// </summary>
        public virtual void ExitScene()
        {

        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="module"></param>
        public virtual void AddModule(IModule module)
        {
            _modules.Add(module);
        }

        /// <summary>
        /// 移除模块
        /// </summary>
        /// <param name="module"></param>
        public virtual void RemoveModule(IModule module)
        {
            _modules.Remove(module);
        }

        /// <summary>
        /// 是否含有模块
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public virtual bool HasModule(IModule module)
        {
            return _modules.Contains(module);
        }

        /// <summary>
        /// 场景进入完成回调，需手动调用
        /// </summary>
        protected virtual void OnSceneEntered()
        {
            if (_enterCallback != null)
                _enterCallback.Invoke(this);
        }

        /// <summary>
        /// 场景退出完成回调，需手动调用
        /// </summary>
        protected virtual void OnSceneExited()
        {
            if (_exitCallback != null)
                _exitCallback.Invoke(this);
        }
    }
}
