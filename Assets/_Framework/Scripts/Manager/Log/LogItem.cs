using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class LogItem
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string user { get; set; }
        /// <summary>
        /// 平台标识
        /// </summary>
        public string plat { get; set; }
        /// <summary>
        /// 区服编号
        /// </summary>
        public string sid { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public string role_id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string role_name { get; set; }
        /// <summary>
        /// 类型 1 报错 2 崩溃
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 包名
        /// </summary>
        public string pack { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>
        public string devi { get; set; }
        /// <summary>
        /// 机型 -1 未知 1 苹果 2 安卓 3 Windows
        /// </summary>
        public int devi_type { get; set; }
        /// <summary>
        /// 错误日志信息
        /// </summary>
        public string info { get; set; }
        /// <summary>
        /// 备份字段
        /// </summary>
        public string bak { get; set; }
        /// <summary>
        /// 计入时间戳
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 应用版本
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 大版本
        /// </summary>
        public string packageVersion { get; set; }
        /// <summary>
        /// 热更版本
        /// </summary>
        public string patchVersion { get; set; }
        /// <summary>
        /// 设备模型
        /// </summary>
        public string deviceModel { get; set; }
        /// <summary>
        /// 设备当前IP
        /// </summary>
        public string ip { get; set; }
    }
}
