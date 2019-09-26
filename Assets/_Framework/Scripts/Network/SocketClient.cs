using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using System.Net.Sockets;
using System.Threading;

namespace Framework
{
    public class SocketClient
    {
        private const int BUFFER_SIZE = 65535;

        protected string _id;
        protected string _host;
        protected int _port;
        protected bool _connected;
        protected ISocketHandler _socketHandler;

        protected Socket _socket;
        protected Thread _threadReceive;
        protected Thread _threadSend;
        protected byte[] _bufferReceive;

        public SocketClient()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.SendTimeout = 1000;
            _socket.ReceiveTimeout = 1000;
            _socket.NoDelay = true;
            _bufferReceive = new byte[BUFFER_SIZE];
        }

        public SocketClient(ISocketHandler socketHandler) : this()
        {
            _socketHandler = socketHandler;
        }

        /// <summary>
        /// 唯一ID
        /// </summary>
        public string id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Host
        /// </summary>
        public string host
        {
            get { return _host; }
            set { _host = value; }
        }

        /// <summary>
        /// Port
        /// </summary>
        public int port
        {
            get { return _port; }
            set { _port = value; }
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool connected
        {
            get { return _connected; }
        }

        /// <summary>
        /// 协议处理器
        /// </summary>
        public ISocketHandler SocketHandler
        {
            get { return _socketHandler; }
            set { _socketHandler = value; }
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void Connect(string host, int port)
        {
            this.host = host;
            this.port = port;
            Connect();
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        public void Connect()
        {
            try
            {
                _socket.BeginConnect(host, port, ConnectCallback, _socket);
            }
            catch (SocketException exception)
            {
                App.logManager.Error("SocketClient.Connect Error:" + exception.Message);
            }
        }

        private void ConnectCallback(IAsyncResult asyncResult)
        {
            try
            {
                _socket.EndConnect(asyncResult);
                if (_socket.Connected)
                {
                    _threadReceive = new Thread(ReceiveData);
                    _threadReceive.IsBackground = true;
                    _threadReceive.Start();
                    _threadSend = new Thread(SendData);
                    _threadSend.IsBackground = true;
                    _threadSend.Start();
                    App.socketManager.OnConnect(this);
                }
            }
            catch (SocketException exception)
            {
                App.logManager.Error("SocketClient.ConnectCallback Error:" + exception.Message);
            }
        }

        /// <summary>
        /// 发送协议
        /// </summary>
        /// <param name="protocol"></param>
        public void Send(IProtocol protocol)
        {
            if (_socket.Connected == false)
            {
                App.socketManager.Close(this);
                return;
            }
            SocketHandler.Send(protocol);
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            try
            {
                if (_socket.Connected)
                    _socket.Close();
            }
            catch (SocketException exception)
            {
                App.logManager.Error("SocketClient.Close Error:" + exception.Message);
            }


            if (_threadReceive != null)
            {
                try
                {
                    if (_threadReceive.IsAlive)
                        _threadReceive.Abort();
                }
                catch (ThreadAbortException exception)
                {
                    App.logManager.Error("SocketClient.Close Error:" + exception.Message);
                }
                _threadReceive = null;
            }

            if (_threadSend != null)
            {
                try
                {
                    if (_threadSend.IsAlive)
                        _threadSend.Abort();
                }
                catch (ThreadAbortException exception)
                {
                    App.logManager.Error("SocketClient.Close Error:" + exception.Message);
                }
                _threadSend = null;
            }
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            Close();
            _socket = null;
            SocketHandler = null;
        }

        /// <summary>
        /// 接收数据线程
        /// </summary>
        private void ReceiveData()
        {
            while (true)
            {
                if (_socket.Connected == false)
                {
                    App.socketManager.Close(this);
                    break;
                }
                try
                {
                    int length = _socket.Receive(_bufferReceive);
                    if (length <= 0)
                    {
                        App.socketManager.Close(this);
                        break;
                    }
                    SocketHandler.Receive(_bufferReceive, length);
                }
                catch (SocketException exception)
                {
                    App.logManager.Error("SocketClient.ReceiveData Error:" + exception.Message);
                    App.socketManager.Close(this);
                    break;
                }

                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 发送数据线程
        /// </summary>
        private void SendData()
        {
            while (true)
            {
                if (_socket.Connected == false)
                {
                    App.socketManager.Close(this);
                    break;
                }
                try
                {
                    while (SocketHandler.queueSend.Count > 0)
                        _socket.Send(SocketHandler.queueSend.Dequeue());

                }
                catch (SocketException exception)
                {
                    App.logManager.Error("SocketClient.SendData Error:" + exception.Message);
                    App.socketManager.Close(this);
                    break;
                }

                Thread.Sleep(1);
            }
        }
    }
}
