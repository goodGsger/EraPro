using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class Logger
    {
        public const string SYS_OWNER = "Sys";

        private string _owner;
        private LogLevel _level;

        public Logger(string owner, LogLevel level)
        {
            _owner = owner;
            _level = level;
        }

        /// <summary>
        /// 所有者
        /// </summary>
        public string owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevel level
        {
            get { return _level; }
            set { _level = value; }
        }
    }
}
