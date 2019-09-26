using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public delegate void ProtocolCallback(IProtocol protocol);

    public interface ISocketManager : IManager
    {
        /// <summary>
        /// 注册Socket处理器
        /// </summary>
        /// <param name="id"></param>
        /// <param name="socketHandler"></param>
        /// <returns></returns>
        SocketClient RegisterSocket(string id, ISocketHandler socketHandler);

        /// <summary>
        /// 注销Socket处理器
        /// </summary>
        /// <param name="socketClient"></param>
        /// <returns></returns>
        SocketClient UnregisterSocket(SocketClient socketClient);

        /// <summary>
        /// 注销Socket处理器
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SocketClient UnregisterSocket(string id);

        /// <summary>
        /// 获取Socket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SocketClient GetSocket(string id);

        /// <summary>
        /// 连接Socket
        /// </summary>
        /// <param name="socketClient"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        bool Connect(SocketClient socketClient, string host, int port);

        /// <summary>
        /// 连接Socket
        /// </summary>
        /// <param name="id"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        bool Connect(string id, string host, int port);

        /// <summary>
        /// 连接成功
        /// </summary>
        /// <param name="socketClient"></param>
        void OnConnect(SocketClient socketClient);

        /// <summary>
        /// 关闭Socket
        /// </summary>
        /// <param name="socketClient"></param>
        /// <returns></returns>
        bool Close(SocketClient socketClient);

        /// <summary>
        /// 关闭Socket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Close(string id);

        /// <summary>
        /// 销毁Socket
        /// </summary>
        /// <param name="socketClient"></param>
        /// <returns></returns>
        bool Destroy(SocketClient socketClient);

        /// <summary>
        /// 销毁Socket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Destroy(string id);

        /// <summary>
        /// 发送协议
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Send(IProtocol protocol, string id);

        /// <summary>
        /// 接收协议
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="id"></param>
        void Receive(IProtocol protocol, string id);

        /// <summary>
        /// 注册协议回调
        /// </summary>
        /// <param name="protocolID"></param>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        void RegisterProtocolCallback(int protocolID, string id, ProtocolCallback callback);

        /// <summary>
        /// 注销协议回调
        /// </summary>
        /// <param name="protocolID"></param>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        void UnregisterProtocolCallback(int protocolID, string id, ProtocolCallback callback);
    }
}
