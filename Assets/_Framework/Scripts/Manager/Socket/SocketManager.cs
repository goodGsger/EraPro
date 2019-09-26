using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class SocketManager : Manager, ISocketManager
    {
        private Dictionary<string, SocketClient> _socketClientDict;
        private Dictionary<string, Dictionary<int, ProtocolCallback>> _callbackDict;

        protected override void Init()
        {
            _socketClientDict = new Dictionary<string, SocketClient>();
            _callbackDict = new Dictionary<string, Dictionary<int, ProtocolCallback>>();
        }

        /// <summary>
        /// 注册Socket处理器
        /// </summary>
        /// <param name="id"></param>
        /// <param name="socketHandler"></param>
        /// <returns></returns>
        public SocketClient RegisterSocket(string id, ISocketHandler socketHandler)
        {
            SocketClient socketClient;
            if (_socketClientDict.TryGetValue(id, out socketClient))
            {
                App.logManager.Warn("SocketManager.RegisterSocket Warn:Socket \"" + id + "\" has already registered!");
                socketClient.SocketHandler = socketHandler;
                return socketClient;
            }
            socketClient = _socketClientDict[id] = new SocketClient(socketHandler);
            return socketClient;
        }

        /// <summary>
        /// 注销Socket处理器
        /// </summary>
        /// <param name="socketClient"></param>
        /// <returns></returns>
        public SocketClient UnregisterSocket(SocketClient socketClient)
        {
            return UnregisterSocket(socketClient.id);
        }

        /// <summary>
        /// 注销Socket处理器
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SocketClient UnregisterSocket(string id)
        {
            SocketClient socketClient;
            if (_socketClientDict.TryGetValue(id, out socketClient))
            {
                _socketClientDict.Remove(id);
                return socketClient;
            }
            App.logManager.Warn("SocketManager.RegisterSocket Warn:Socket \"" + id + "\" is not exist!");
            return null;
        }

        /// <summary>
        /// 获取Socket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SocketClient GetSocket(string id)
        {
            SocketClient socketClient;
            if (_socketClientDict.TryGetValue(id, out socketClient))
                return socketClient;

            return null;
        }

        /// <summary>
        /// 连接Socket
        /// </summary>
        /// <param name="socketClient"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool Connect(SocketClient socketClient, string host, int port)
        {
            return Connect(socketClient.id, host, port);
        }

        /// <summary>
        /// 连接Socket
        /// </summary>
        /// <param name="id"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool Connect(string id, string host, int port)
        {
            SocketClient socketClient = GetSocket(id);
            if (socketClient == null)
            {
                App.logManager.Error("SocketManager.Connect Error:Socket \"" + id + "\" is not exist!");
                return false;
            }

            if (socketClient.connected)
            {
                App.logManager.Error("SocketManager.Connect Warn:Socket \"" + id + "\" has already connected!");
                return false;
            }

            socketClient.Connect(host, port);
            DispatchEvent(SocketManagerEventArgs.START_CONNECT, socketClient);
            return true;
        }

        /// <summary>
        /// 连接成功
        /// </summary>
        /// <param name="socketClient"></param>
        public void OnConnect(SocketClient socketClient)
        {
            DispatchEvent(SocketManagerEventArgs.CONNECT, socketClient);
        }

        /// <summary>
        /// 关闭Socket
        /// </summary>
        /// <param name="socketClient"></param>
        /// <returns></returns>
        public bool Close(SocketClient socketClient)
        {
            return Close(socketClient.id);
        }

        /// <summary>
        /// 关闭Socket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Close(string id)
        {
            SocketClient socketClient = GetSocket(id);
            if (socketClient == null)
            {
                App.logManager.Error("SocketManager.Connect Error:Socket \"" + id + "\" is not exist!");
                return false;
            }

            socketClient.Close();
            DispatchEvent(SocketManagerEventArgs.CLOSE, socketClient);
            return true;
        }

        /// <summary>
        /// 销毁Socket
        /// </summary>
        /// <param name="socketClient"></param>
        /// <returns></returns>
        public bool Destroy(SocketClient socketClient)
        {
            return Destroy(socketClient.id);
        }

        /// <summary>
        /// 销毁Socket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Destroy(string id)
        {
            SocketClient socketClient = GetSocket(id);
            if (socketClient == null)
            {
                App.logManager.Error("SocketManager.Connect Error:Socket \"" + id + "\" is not exist!");
                return false;
            }

            socketClient.Dispose();
            return true;
        }

        /// <summary>
        /// 发送协议
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Send(IProtocol protocol, string id)
        {
            SocketClient socketClient = GetSocket(id);
            if (socketClient == null)
            {
                App.logManager.Error("SocketManager.Connect Error:Socket \"" + id + "\" is not exist!");
                return false;
            }

            socketClient.Send(protocol);
            DispatchEvent(SocketManagerEventArgs.SEND, socketClient);
            return true;
        }

        /// <summary>
        /// 接收协议
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="id"></param>
        public void Receive(IProtocol protocol, string id)
        {
            Dictionary<int, ProtocolCallback> callbacks;
            if (_callbackDict.TryGetValue(id, out callbacks))
            {
                ProtocolCallback callbackList;
                if (callbacks.TryGetValue(protocol.protocolID, out callbackList))
                    callbackList.Invoke(protocol);
            }
            DispatchEvent(SocketManagerEventArgs.RECEIVED, GetSocket(id));
        }

        /// <summary>
        /// 注册协议回调
        /// </summary>
        /// <param name="protocolID"></param>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void RegisterProtocolCallback(int protocolID, string id, ProtocolCallback callback)
        {
            Dictionary<int, ProtocolCallback> callbacks;
            if (_callbackDict.TryGetValue(id, out callbacks) == false)
                _callbackDict[id] = callbacks = new Dictionary<int, ProtocolCallback>();

            ProtocolCallback callbackList;
            if (callbacks.TryGetValue(protocolID, out callbackList) == false)
                callbacks[protocolID] = callbackList = callback;
            else
                callbackList += callback;
        }

        /// <summary>
        /// 注销协议回调
        /// </summary>
        /// <param name="protocolID"></param>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void UnregisterProtocolCallback(int protocolID, string id, ProtocolCallback callback)
        {
            Dictionary<int, ProtocolCallback> callbacks;
            if (_callbackDict.TryGetValue(id, out callbacks))
            {
                ProtocolCallback callbackList;
                if (callbacks.TryGetValue(protocolID, out callbackList))
                {
                    callbackList -= callback;
                    if (callbackList == null)
                        callbacks.Remove(protocolID);
                }
            }
        }

        private void DispatchEvent(string type, SocketClient socketClient)
        {
            SocketManagerEventArgs eventArgs = App.objectPoolManager.GetObject<SocketManagerEventArgs>();
            eventArgs.type = type;
            eventArgs.socketClient = socketClient;
            DispatchEvent(eventArgs);
            App.objectPoolManager.ReleaseObject(eventArgs);
        }
    }
}
