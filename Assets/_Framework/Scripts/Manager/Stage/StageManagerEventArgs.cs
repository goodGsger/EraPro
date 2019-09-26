using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class StageManagerEventArgs : EventArguments, IPooledObject
    {
        public const string RESIZE = "RESIZE";

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
