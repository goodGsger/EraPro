using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface IProtocol
    {
        /// <summary>
        /// 协议ID
        /// </summary>
        int protocolID { get; }

        /// <summary>
        /// 协议编码
        /// </summary>
        /// <param name="bytes"></param>
        void Encode(ByteArray bytes);

        /// <summary>
        /// 协议解码
        /// </summary>
        /// <param name="bytes"></param>
        void Decode(ByteArray bytes);

        /// <summary>
        /// 协议重置
        /// </summary>
        void Reset();
    }
}
