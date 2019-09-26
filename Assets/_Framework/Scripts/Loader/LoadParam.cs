using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class LoadParam
    {
        /// <summary>
        /// 重试次数
        /// </summary>
        public const string RETRY_COUNT = "RETRY_COUNT";

        /// <summary>
        /// 版本号
        /// </summary>
        public const string VERSION = "VERSION";

        /// <summary>
        /// 文件大小
        /// </summary>
        public const string WEIGHT = "WEIGHT";

        protected string _id;
        protected object _value;

        public LoadParam(string id, object value)
        {
            _id = id;
            _value = value;
        }

        public string id
        {
            get { return _id; }
            set { _id = value; }
        }

        public object value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
