using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class TimerManagerEventArgs : EventArguments, IPooledObject
    {
        public const string TIME_OUT = "TIME_OUT";

        public TimerManagerEventArgs()
        {

        }

        public TimerManagerEventArgs(string type)
        {
            _type = type;
        }

        public void OnPoolGet()
        {

        }

        public void OnPoolReset()
        {
            _type = null;
            _data = null;
        }

        public void OnPoolDispose()
        {
            _type = null;
            _data = null;
        }
    }
}
