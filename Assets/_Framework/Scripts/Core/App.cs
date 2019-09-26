using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class App
    {
        private static App _inst;

        private GameObject _gameObject;
        private AppBehaviour _behaviour;
        private List<IManager> _managerList;
        private Dictionary<Type, IManager> _managerDict;

        // 管理器
        private static ITimerManager _timerManager;
        private static IStageManager _stageManager;
        private static IPathManager _pathManager;
        private static ILogManager _logManager;
        private static ILangManager _langManager;
        private static ISocketManager _socketManager;
        private static ITableManager _tableManager;
        private static IObjectPoolManager _objectPoolManager;
        private static IFileManager _fileManager;
        private static IResourceManager _resourceManager;
        private static IAssetManager _assetManager;
        private static IAudioManager _audioManager;
        private static IScenesManager _sceneManager;
        private static ITouchManager _touchManager;
        private static IPlatformManager _platformManager;

        public App()
        {
            _managerList = new List<IManager>();
            _managerDict = new Dictionary<Type, IManager>();
        }

        public static App inst
        {
            get
            {
                if (_inst == null)
                    _inst = new App();

                return _inst;
            }
        }

        /// <summary>
        /// 主应用程序GameObject
        /// </summary>
        public GameObject gameObject
        {
            get { return _gameObject; }
        }

        /// <summary>
        /// 主应用程序Behaviour
        /// </summary>
        public AppBehaviour behaviour
        {
            get { return _behaviour; }
        }

        /// <summary>
        /// 计时器管理器
        /// </summary>
        public static ITimerManager timerManager
        {
            get { return _timerManager; }
            set { _timerManager = value; }
        }

        /// <summary>
        /// 舞台管理器
        /// </summary>
        public static IStageManager stageManager
        {
            get { return _stageManager; }
            set { _stageManager = value; }
        }

        /// <summary>
        /// 路径管理器
        /// </summary>
        public static IPathManager pathManager
        {
            get { return _pathManager; }
            set { _pathManager = value; }
        }

        /// <summary>
        /// 日志管理器
        /// </summary>
        public static ILogManager logManager
        {
            get { return _logManager; }
            set { _logManager = value; }
        }

        /// <summary>
        /// 语言管理器
        /// </summary>
        public static ILangManager langManager
        {
            get { return _langManager; }
            set { _langManager = value; }
        }

        /// <summary>
        /// Socket管理器
        /// </summary>
        public static ISocketManager socketManager
        {
            get { return _socketManager; }
            set { _socketManager = value; }
        }

        /// <summary>
        /// 数据表管理器
        /// </summary>
        public static ITableManager tableManager
        {
            get { return _tableManager; }
            set { _tableManager = value; }
        }

        /// <summary>
        /// 对象池管理器
        /// </summary>
        public static IObjectPoolManager objectPoolManager
        {
            get { return _objectPoolManager; }
            set { _objectPoolManager = value; }
        }

        /// <summary>
        /// 协程管理器
        /// </summary>
        public static IFileManager fileManager
        {
            get { return _fileManager; }
            set { _fileManager = value; }
        }

        /// <summary>
        /// 资源加载器
        /// </summary>
        public static IResourceManager resourceManager
        {
            get { return _resourceManager; }
            set { _resourceManager = value; }
        }

        /// <summary>
        /// 资源管理器
        /// </summary>
        public static IAssetManager assetManager
        {
            get { return _assetManager; }
            set { _assetManager = value; }
        }

        /// <summary>
        /// 音频管理器
        /// </summary>
        public static IAudioManager audioManager
        {
            get { return _audioManager; }
            set { _audioManager = value; }
        }

        /// <summary>
        /// 场景管理器
        /// </summary>
        public static IScenesManager sceneManager
        {
            get { return _sceneManager; }
            set { _sceneManager = value; }
        }

        /// <summary>
        /// 触摸管理器
        /// </summary>
        public static ITouchManager touchManager
        {
            get { return _touchManager; }
            set { _touchManager = value; }
        }

        /// <summary>
        /// 平台管理器
        /// </summary>
        public static IPlatformManager platformManager
        {
            get { return _platformManager; }
            set { _platformManager = value; }
        }

        /// <summary>
        /// 初始化默认管理器
        /// </summary>
        public void InitDefaultManagers()
        {
            _timerManager = new TimerManager();
            AddManager(_timerManager);
            _stageManager = new StageManager();
            AddManager(_stageManager);
            _pathManager = new PathManager();
            AddManager(_pathManager);
            _logManager = new LogManager();
            AddManager(_logManager);
            _langManager = new LangManager();
            AddManager(_langManager);
            _socketManager = new SocketManager();
            AddManager(_socketManager);
            _tableManager = new TableManager();
            AddManager(_tableManager);
            _objectPoolManager = new ObjectPoolManager();
            AddManager(_objectPoolManager);
            _fileManager = new FileManager();
            AddManager(_fileManager);
            _resourceManager = new ResourceManager();
            AddManager(_resourceManager);
            _assetManager = new AssetManager();
            AddManager(_assetManager);
            _audioManager = new AudioManager();
            AddManager(_audioManager);
            _sceneManager = new ScenesManager();
            AddManager(_sceneManager);
            _touchManager = new TouchManager();
            AddManager(_touchManager);
            _platformManager = new PlatformManager();
            AddManager(_platformManager);
        }

        /// <summary>
        /// 初始化App
        /// </summary>
        public void Init()
        {
            _gameObject = new GameObject("App");
            _behaviour = _gameObject.AddComponent<AppBehaviour>();
            GameObject.DontDestroyOnLoad(_behaviour);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            foreach (IManager manager in _managerList)
                manager.Update(deltaTime);
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Destroy()
        {
            foreach (IManager manager in _managerList)
                manager.Destroy();
        }

        /// <summary>
        /// 应用程序获得焦点
        /// </summary>
        public void OnApplicationGetFocus()
        {
            foreach (IManager manager in _managerList)
                manager.OnApplicationGetFocus();
        }

        /// <summary>
        /// 应用程序失去焦点
        /// </summary>
        public void OnApplicationLoseFocus()
        {
            foreach (IManager manager in _managerList)
                manager.OnApplicationLoseFocus();
        }

        /// <summary>
        /// 应用程序退出
        /// </summary>
        public void OnApplicationQuit()
        {
            foreach (IManager manager in _managerList)
                manager.OnApplicationQuit();
        }

        /// <summary>
        /// OnGUI
        /// </summary>
        public void OnGUI()
        {
            foreach (IManager manager in _managerList)
                manager.OnGUI();
        }

        /// <summary>
        /// 添加管理器
        /// </summary>
        /// <param name="manager"></param>
        public void AddManager(IManager manager)
        {
            _managerDict[manager.GetType()] = manager;
            _managerList.Add(manager);
        }

        /// <summary>
        /// 移除管理器
        /// </summary>
        /// <param name="manager"></param>
        public void RemoveManager(IManager manager)
        {
            Type type = manager.GetType();
            if (_managerDict.ContainsKey(type))
            {
                _managerDict.Remove(type);
                _managerList.Remove(manager);
            }
        }

        /// <summary>
        /// 获取管理器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IManager GetManager(Type type)
        {
            if (_managerDict.TryGetValue(type, out IManager mananger))
                return mananger;

            return null;
        }
    }
}
