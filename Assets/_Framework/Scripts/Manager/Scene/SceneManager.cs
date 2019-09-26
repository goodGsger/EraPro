using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class ScenesManager : Manager, IScenesManager
    {
        private Dictionary<string, IScene> _scenes;
        private IScene _currentScene;
        private IScene _nextScene;

        public ScenesManager()
        {

        }

        protected override void Init()
        {
            _scenes = new Dictionary<string, IScene>();
        }

        /// <summary>
        /// 注册场景
        /// </summary>
        /// <param name="scene"></param>
        public void RegisterScene(IScene scene)
        {
            if (_scenes.ContainsKey(scene.sceneName))
            {
                App.logManager.Warn("SceneManager.RegisterScene:\"" + scene.sceneName + "\" has already registed!");
                return;
            }
            scene.enterCallback = OnSceneEntered;
            scene.exitCallback = OnSceneExited;
            _scenes[scene.sceneName] = scene;
        }

        /// <summary>
        /// 注销场景
        /// </summary>
        /// <param name="scene"></param>
        public void UnregisterScene(IScene scene)
        {
            if (_scenes.ContainsKey(scene.sceneName))
                _scenes.Remove(scene.sceneName);
        }

        /// <summary>
        /// 进入场景
        /// </summary>
        /// <param name="scene"></param>
        public void EnterScene(IScene scene)
        {
            // 不能重复进入同一场景
            if (_currentScene != null && _currentScene.sceneName == scene.sceneName)
            {
                App.logManager.Warn("SceneManager.EnterScene: scene \"" + scene.sceneName + "\" + is entered!");
                return;
            }
            // 下一场景进入中不能进入场景
            if (_nextScene != null && _nextScene.state == SceneState.ENTERING)
            {
                App.logManager.Warn("SceneManager.EnterScene: nextScene \"" + _nextScene.sceneName + "\" + is enterning!");
                return;
            }
            _nextScene = scene;
            // 退出当前场景
            if (_currentScene == null)
                EnterNextScene();
            else
                ExitCurrentScene();
        }

        /// <summary>
        /// 场景进入回调
        /// </summary>
        /// <param name="scene"></param>
        private void OnSceneEntered(IScene scene)
        {
            if (_nextScene == scene)
            {
                _nextScene.state = SceneState.RUNNING;
                DispatchEvent(SceneManagerEventArgs.ENTER_SCENE, _nextScene);
                _currentScene = _nextScene;
                _nextScene = null;
            }
        }

        /// <summary>
        /// 场景退出回调
        /// </summary>
        /// <param name="scene"></param>
        private void OnSceneExited(IScene scene)
        {
            if (_currentScene == scene)
            {
                _currentScene.state = SceneState.IDLE;
                DispatchEvent(SceneManagerEventArgs.EXIT_SCENE, _currentScene);
                EnterNextScene();
            }
        }

        /// <summary>
        /// 退出当前场景
        /// </summary>
        private void ExitCurrentScene()
        {
            if (_currentScene.state == SceneState.RUNNING)
            {
                _currentScene.state = SceneState.EXITING;
                _currentScene.ExitScene();
                DispatchEvent(SceneManagerEventArgs.EXITING_SCENE, _currentScene);
            }
        }

        /// <summary>
        /// 进入下一场景
        /// </summary>
        private void EnterNextScene()
        {
            if (_nextScene == null)
                return;

            // 退出当前场景
            if (_currentScene != null)
            {
                // 退出当前场景模块
                foreach (IModule module in _currentScene.modules)
                    if (_nextScene.HasModule(module) == false)
                        module.ExitModule();
            }

            // 进入新场景
            foreach (IModule module in _nextScene.modules)
            {
                if (module.scene != _nextScene)
                    module.ChangeScene(_nextScene);

                if (_currentScene == null || _currentScene.HasModule(module) == false)
                    module.EnterModule();
            }

            _nextScene.state = SceneState.ENTERING;
            _nextScene.EnterScene();
            DispatchEvent(SceneManagerEventArgs.ENTERING_SCENE, _nextScene);
        }

        private void DispatchEvent(string type, IScene scene)
        {
            SceneManagerEventArgs eventArgs = App.objectPoolManager.GetObject<SceneManagerEventArgs>();
            eventArgs.type = type;
            eventArgs.scene = scene;
            DispatchEvent(eventArgs);
            App.objectPoolManager.ReleaseObject(eventArgs);
        }
    }
}
