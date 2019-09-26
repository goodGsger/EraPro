using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class EventArguments
    {
        protected string _type;
        protected object _data;

        public EventArguments()
        {
        }

        public EventArguments(string type)
        {
            _type = type;
        }

        public EventArguments(string type, object data)
        {
            _type = type;
            _data = data;
        }

        public string type
        {
            get { return _type; }
            set { _type = value; }
        }

        public object data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
}
