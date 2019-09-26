using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class SocketManagerEventArgs : EventArguments, IPooledObject
    {
        public const string CONNECT = "CONNECT";
        public const string CLOSE = "CLOSE";
        public const string START_CONNECT = "START_CONNECT";
        public const string SEND = "SEND";
        public const string RECEIVED = "RECEIVED";

        private SocketClient _socketClient;

        public SocketManagerEventArgs()
        {

        }

        public SocketManagerEventArgs(string type, SocketClient socketClient)
        {
            _type = type;
            _socketClient = socketClient;
        }

        /// <summary>
        /// SocketClient
        /// </summary>
        public SocketClient socketClient
        {
            get { return _socketClient; }
            set { _socketClient = value; }
        }

        public void OnPoolGet()
        {

        }

        public void OnPoolReset()
        {
            _type = null;
            _data = null;
            socketClient = null;
        }

        public void OnPoolDispose()
        {
            _type = null;
            _data = null;
            socketClient = null;
        }
    }
}
