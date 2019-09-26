using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class AbstractModule : EventDispatcher, IModule
    {
        protected string _moduleName;
        protected bool _inited;
        protected IScene _scene;
        protected IScene _lastScene;

        public AbstractModule(string moduleName)
        {
            _moduleName = moduleName;
        }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string moduleName
        {
            get { return _moduleName; }
            set { _moduleName = value; }
        }

        /// <summary>
        /// 是否已完成初始化
        /// </summary>
        public bool inited
        {
            get { return _inited; }
            set { _inited = value; }
        }

        /// <summary>
        /// 当前所属场景
        /// </summary>
        public IScene scene
        {
            get { return _scene; }
        }

        /// <summary>
        /// 上一所属场景
        /// </summary>
        public IScene lastScene
        {
            get { return _lastScene; }
        }

        /// <summary>
        /// 进入模块
        /// </summary>
        public void EnterModule()
        {
            if (_inited == false)
            {
                Init();
                _inited = true;
            }
            Show();
        }

        /// <summary>
        /// 退出模块
        /// </summary>
        public void ExitModule()
        {
            Remove();
        }

        /// <summary>
        /// 改变场景
        /// </summary>
        /// <param name="scene"></param>
        public void ChangeScene(IScene scene)
        {
            _lastScene = _scene;
            _scene = scene;
            OnSceneChange();
        }

        protected virtual void Init()
        {

        }

        protected virtual void Show()
        {

        }

        protected virtual void Remove()
        {

        }

        protected virtual void OnSceneChange()
        {

        }

        public virtual void Reset()
        {

        }
    }
}
