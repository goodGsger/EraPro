using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface ISocketHandler
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        string id { get; set; }

        /// <summary>
        /// 协议数据缓冲区
        /// </summary>
        Queue<byte[]> queueSend { get; }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="length"></param>
        void Receive(byte[] bytes, int length);

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="protocol"></param>
        void Send(IProtocol protocol);
    }
}
