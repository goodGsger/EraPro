using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

namespace Framework
{
    public class LogManager : Manager, ILogManager
    {
        private Dictionary<string, Logger> _loggers;

        private bool _enabled;
        private bool _isPrint;
        private bool _isPrintToScreen;
        private bool _isSendLog;
        private bool _isSaveLog;
        private bool _isCatchLog;
        private string _logUrl;
        private LogItem _logItem;

        private bool _isShowScreenPrint;
        private Queue<string> _screenLogs;
        private string _screenString;
        private string _lastLog;
        private List<string> _filterLogs;

        private LogCallbackHandler _logCallback;

        override protected void Init()
        {
            _loggers = new Dictionary<string, Logger>();
            _screenLogs = new Queue<string>();
            _logItem = new LogItem();
            _filterLogs = new List<string>();
        }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (_enabled)
                    Application.logMessageReceivedThreaded += OnLogMessageReceivedThreaded;
                else
                    Application.logMessageReceivedThreaded -= OnLogMessageReceivedThreaded;
            }
        }

        /// <summary>
        /// 是否打印
        /// </summary>
        public bool isPrint
        {
            get { return _isPrint; }
            set { _isPrint = value; }
        }

        /// <summary>
        /// 是否打印到屏幕
        /// </summary>
        public bool isPrintToScreen
        {
            get { return _isPrintToScreen; }
            set { _isPrintToScreen = value; }
        }

        /// <summary>
        /// 是否保存日志到本地
        /// </summary>
        public bool isSaveLog
        {
            get { return _isSaveLog; }
            set { _isSaveLog = value; }
        }

        /// <summary>
        /// 是否发送日志
        /// </summary>
        public bool isSendLog
        {
            get { return _isSendLog; }
            set { _isSendLog = value; }
        }

        /// <summary>
        /// 是否发送自定义log类型的错误
        /// </summary>
        public bool isCatchLog
        {
            get { return _isCatchLog; }
            set { _isCatchLog = value; }
        }

        /// <summary>
        /// 日志回调
        /// </summary>
        public LogCallbackHandler logCallback
        {
            get { return _logCallback; }
            set { _logCallback = value; }
        }

        /// <summary>
        /// 日志服务器地址
        /// </summary>
        public string logUrl
        {
            get { return _logUrl; }
            set { _logUrl = value; }
        }

        /// <summary>
        /// 日志数据结构
        /// </summary>
        public LogItem logItem
        {
            get { return _logItem; }
            set { _logItem = value; }
        }

        /// <summary>
        /// 添加过滤日志
        /// </summary>
        /// <param name="filterLog"></param>
        public void AddFilterLog(string filterLog)
        {
            _filterLogs.Add(filterLog);
        }

        /// <summary>
        ///  注册日志记录器
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public Logger RegisterLogger(string owner, LogLevel level = LogLevel.Info)
        {
            Logger logger = _loggers[owner] = new Logger(owner, level);
            return logger;
        }

        /// <summary>
        /// 注销日志记录器
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public Logger UnregisterLogger(string owner)
        {
            Logger logger;
            if (_loggers.TryGetValue(owner, out logger))
                _loggers.Remove(owner);

            return logger;
        }

        /// <summary>
        /// 记录日志（信息）
        /// </summary>
        /// <param name="log"></param>
        public void Info(string log)
        {
            Log(Logger.SYS_OWNER, LogLevel.Info, log);
        }

        /// <summary>
        /// 记录日志（信息）
        /// </summary>
        /// <param name="log"></param>
        /// <param name="owner"></param>
        public void Info(string log, string owner)
        {
            Log(owner, LogLevel.Info, log);
        }

        /// <summary>
        /// 记录日志（警告）
        /// </summary>
        /// <param name="log"></param>
        public void Warn(string log)
        {
            Log(Logger.SYS_OWNER, LogLevel.Warn, log);
        }

        /// <summary>
        /// 记录日志（警告）
        /// </summary>
        /// <param name="log"></param>
        /// <param name="owner"></param>
        public void Warn(string log, string owner)
        {
            Log(owner, LogLevel.Warn, log);
        }

        /// <summary>
        /// 记录日志（错误）
        /// </summary>
        /// <param name="log"></param>
        public void Error(string log)
        {
            Log(Logger.SYS_OWNER, LogLevel.Error, log);
        }

        /// <summary>
        /// 记录日志（错误）
        /// </summary>
        /// <param name="log"></param>
        /// <param name="owner"></param>
        public void Error(string log, string owner)
        {
            Log(owner, LogLevel.Error, log);
        }

        /// <summary>
        /// 记录日志（灾难）
        /// </summary>
        /// <param name="log"></param>
        public void Fatal(string log)
        {
            Log(Logger.SYS_OWNER, LogLevel.Fatal, log);
        }

        /// <summary>
        /// 记录日志（灾难）
        /// </summary>
        /// <param name="log"></param>
        /// <param name="owner"></param>
        public void Fatal(string log, string owner)
        {
            Log(owner, LogLevel.Fatal, log);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="level"></param>
        /// <param name="log"></param>
        public void Log(string owner, LogLevel level, string log)
        {
            if (_enabled == false)
                return;

            Logger logger;
            if (_loggers.TryGetValue(owner, out logger) == false)
                logger = RegisterLogger(owner, LogLevel.Info);

            if (level >= logger.level)
            {
                string logFormat = "[LOG] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " [Owner:\"" + owner + "\" Level:\"" + level.ToString() + "\"] - " + log;
                DispatchEvent(LogManagerEventArgs.LOG, log, logFormat, owner, level);

                if (isPrint)
                {
                    switch (level)
                    {
                        case LogLevel.Info:
                            Debug.Log(logFormat);
                            break;
                        case LogLevel.Warn:
                            Debug.LogWarning(logFormat);
                            break;
                        case LogLevel.Error:
                        case LogLevel.Fatal:
                            Debug.LogError(logFormat);
                            break;
                        default:
                            Debug.Log(logFormat);
                            break;
                    }
                }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (_isPrintToScreen)
                {
                    _screenLogs.Enqueue(logFormat);
                    if (_screenLogs.Count > 50)
                        _screenLogs.Dequeue();

                    StringBuilder sb = new StringBuilder();
                    foreach (string scrrenLog in _screenLogs)
                        sb.Append(scrrenLog).Append("\n");

                    _screenString = sb.ToString();
                }
#endif
            }
        }

        /// <summary>
        /// 输出到屏幕
        /// </summary>
        /// <param name="log"></param>
        public void LogScreen(string log)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            if (_isPrintToScreen)
            {
                _screenLogs.Enqueue(log);
                if (_screenLogs.Count > 50)
                    _screenLogs.Dequeue();

                StringBuilder sb = new StringBuilder();
                foreach (string scrrenLog in _screenLogs)
                    sb.Append(scrrenLog).Append("\n");

                _screenString = sb.ToString();
            }
#endif
        }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        public override void Update(float detalTime)
        {
            if (_isPrintToScreen)
            {
                if (Input.GetKeyUp(KeyCode.F12))
                    _isShowScreenPrint = !_isShowScreenPrint;
            }
        }

        public override void OnGUI()
        {
            if (_isPrintToScreen)
            {
                if (_isShowScreenPrint)
                {
                    GUI.Label(new Rect(50, 50, Screen.width - 100, Screen.height - 100), _screenString);
                }
            }
        }
#endif

        private void OnLogMessageReceivedThreaded(string condition, string stackTrace, LogType type)
        {
            //if (_logCallback != null)
            //    _logCallback.Invoke(condition, stackTrace, type);

            if (type == LogType.Assert || type == LogType.Exception || (_isCatchLog && type == LogType.Error))
            {
                if (_lastLog != stackTrace)
                {
                    if (condition != null && _filterLogs.Count > 0)
                    {
                        foreach (string filterLog in _filterLogs)
                        {
                            if (condition.Contains(filterLog))
                                return;
                        }
                    }
                    string log = string.Format("logCondition:{0} logStack:{1} ", condition, stackTrace);
                    if (_isSaveLog)
                        File.AppendAllText(Application.persistentDataPath + "/log.txt", log);

                    if (_isSendLog)
                    {
                        _logItem.type = (int)type;
                        _logItem.devi_type = GetPlatform();
                        _logItem.devi = GlobalSetting.deviceID;
                        _logItem.pack = Application.identifier;
                        _logItem.info = log;
                        _logItem.time = DateUtil.FormatDate(App.timerManager.timeSecond);
                        _logItem.version = GlobalSetting.version;
                        _logItem.packageVersion = GlobalSetting.packageVersion;
                        _logItem.patchVersion = GlobalSetting.patchVersion;
                        _logItem.deviceModel = GlobalSetting.deviceModel;
                        _logItem.ip = SystemUtil.GetIP();
                        JsonSerializerSettings jsetting = new JsonSerializerSettings();
                        jsetting.NullValueHandling = NullValueHandling.Ignore;
                        string jsonStr = JsonConvert.SerializeObject(_logItem, Formatting.Indented, jsetting);
                        HttpHelper.SendRequest1("data=" + jsonStr, logUrl);
                    }
                    _lastLog = stackTrace;
                }
            }
        }

        private void DispatchEvent(string type, string log, string logFormat, string owner, LogLevel level)
        {
            LogManagerEventArgs eventArgs = App.objectPoolManager.GetObject<LogManagerEventArgs>();
            eventArgs.type = type;
            eventArgs.log = log;
            eventArgs.logFormat = logFormat;
            eventArgs.owner = owner;
            eventArgs.level = level;
            DispatchEvent(eventArgs);
            App.objectPoolManager.ReleaseObject(eventArgs);
        }

        /// <summary>
        /// 获取平台
        /// </summary>
        /// <returns></returns>
        private static int GetPlatform()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                return 1;
            if (Application.platform == RuntimePlatform.Android)
                return 2;
            return 3;
        }
    }
}
