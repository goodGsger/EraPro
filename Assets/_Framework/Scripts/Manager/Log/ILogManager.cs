using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public delegate void LogCallbackHandler(string condition, string stackTrace, LogType type);

    public interface ILogManager : IManager
    {
        /// <summary>
        /// 是否禁用
        /// </summary>
        bool enabled { get; set; }

        /// <summary>
        /// 是否打印
        /// </summary>
        bool isPrint { get; set; }

        /// <summary>
        /// 是否打印到屏幕
        /// </summary>
        bool isPrintToScreen { get; set; }

        /// <summary>
        /// 是否保存日志
        /// </summary>
        bool isSaveLog { get; set; }

        /// <summary>
        /// 是否发送日志
        /// </summary>
        bool isSendLog { get; set; }

        /// <summary>
        /// 是否发送自定义log类型的错误
        /// </summary>
        bool isCatchLog { get; set; }

        /// <summary>
        /// 日志监听回调
        /// </summary>
        /// <returns></returns>
        LogCallbackHandler logCallback { get; set; }

        /// <summary>
        /// 日志服务器地址
        /// </summary>
        string logUrl { get; set; }

        /// <summary>
        /// 日志数据结构
        /// </summary>
        LogItem logItem { get; set; }

        /// <summary>
        ///  注册日志记录器
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        Logger RegisterLogger(string owner, LogLevel level = LogLevel.Info);

        /// <summary>
        /// 注销日志记录器
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        Logger UnregisterLogger(string owner);

        /// <summary>
        /// 打印日志
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="level"></param>
        /// <param name="log"></param>
        void Log(string owner, LogLevel level, string log);

        /// <summary>
        /// 打印到屏幕
        /// </summary>
        /// <param name="log"></param>
        void LogScreen(string log);

        /// <summary>
        /// 记录日志（信息）
        /// </summary>
        /// <param name="log"></param>
        void Info(string log);

        /// <summary>
        /// 记录日志（信息）
        /// </summary>
        /// <param name="log"></param>
        /// <param name="owner"></param>
        void Info(string log, string owner);

        /// <summary>
        /// 记录日志（警告）
        /// </summary>
        /// <param name="log"></param>
        void Warn(string log);

        /// <summary>
        /// 记录日志（警告）
        /// </summary>
        /// <param name="log"></param>
        /// <param name="owner"></param>
        void Warn(string log, string owner);

        /// <summary>
        /// 记录日志（错误）
        /// </summary>
        /// <param name="log"></param>
        void Error(string log);

        /// <summary>
        /// 记录日志（错误）
        /// </summary>
        /// <param name="log"></param>
        /// <param name="owner"></param>
        void Error(string log, string owner);

        /// <summary>
        /// 记录日志（灾难）
        /// </summary>
        /// <param name="log"></param>
        void Fatal(string log);

        /// <summary>
        /// 记录日志（灾难）
        /// </summary>
        /// <param name="log"></param>
        /// <param name="owner"></param>
        void Fatal(string log, string owner);
    }
}
