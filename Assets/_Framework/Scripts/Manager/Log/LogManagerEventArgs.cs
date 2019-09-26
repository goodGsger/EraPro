using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class LogManagerEventArgs : EventArguments, IPooledObject
    {
        public const string LOG = "LOG";

        private string _log;
        private string _logFormat;
        private string _owner;
        private LogLevel _level;

        public LogManagerEventArgs()
        {

        }

        public LogManagerEventArgs(string type, string log, string logFormat, string owner, LogLevel level)
        {
            _type = type;
            _log = log;
            _logFormat = logFormat;
            _owner = owner;
            _level = level;
        }

        /// <summary>
        /// 日志
        /// </summary>
        public string log
        {
            get { return _log; }
            set { _log = value; }
        }

        public string logFormat
        {
            get { return _logFormat; }
            set { _logFormat = value; }
        }

        public string owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        public LogLevel level
        {
            get { return _level; }
            set { _level = value; }
        }

        public void OnPoolGet()
        {

        }

        public void OnPoolReset()
        {
            _type = null;
            _data = null;
            _log = null;
            _logFormat = null;
            _owner = null;
            _level = 0;
        }

        public void OnPoolDispose()
        {
            _type = null;
            _data = null;
            _log = null;
            _logFormat = null;
            _owner = null;
            _level = 0;
        }
    }
}
