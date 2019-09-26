using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class GlobalSetting
    {
        /// <summary>
        /// 全局设置
        /// </summary>
        public static Dictionary<string, string> settings = new Dictionary<string, string>();

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetSetting(string name)
        {
            if (settings.ContainsKey(name))
            {
                return settings[name];
            }
            return null;
        }

        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetSetting(string name, string value)
        {
            settings[name] = value;
        }

        /// <summary>
        /// 版本号
        /// </summary>
        public static string version;

        /// <summary>
        /// 版本号
        /// </summary>
        public static string version_old;

        /// <summary>
        /// 原始包版本号
        /// </summary>
        public static string packageVersion;

        /// <summary>
        /// 原始包版本号
        /// </summary>
        public static string packageVersion_old;

        /// <summary>
        /// 补丁包版本号
        /// </summary>
        public static string patchVersion;

        /// <summary>
        /// 补丁包版本号
        /// </summary>
        public static string patchVersion_old;

        /// <summary>
        /// 原始包路径
        /// </summary>
        public static string packagePath;

        /// <summary>
        /// 补丁包路径
        /// </summary>
        public static string patchPath;

        /// <summary>
        /// 原始包全路径
        /// </summary>
        public static string packageFullPath;

        /// <summary>
        /// 补丁包全路径
        /// </summary>
        public static string patchFullPath;

        /// <summary>
        /// 是否热更
        /// </summary>
        public static bool isHot;

        /// <summary>
        /// 自动热更大小
        /// </summary>
        public static ulong hotFixSize;

        /// <summary>
        /// 设备ID
        /// </summary>
        public static string deviceID;

        /// <summary>
        /// 唯一ID
        /// </summary>
        public static string uniqueIdentifier;

        /// <summary>
        /// 设备名
        /// </summary>
        public static string deviceModel;

        /// <summary>
        /// 操作系统
        /// </summary>
        public static string operatingSystem;
    }
}