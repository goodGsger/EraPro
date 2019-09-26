using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public abstract class AbstractModel : EventDispatcher, IModel
    {
        public AbstractModel()
        {
            Init();
            RegisterInterestedProtocols();
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        public virtual void Reset()
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Init()
        {

        }

        /// <summary>
        /// 注册感兴趣的协议
        /// </summary>
        protected virtual void RegisterInterestedProtocols()
        {

        }

        /// <summary>
        /// 注册协议
        /// </summary>
        /// <param name="protocolId"></param>
        /// <param name="callback"></param>
        protected virtual void Reg(int protocolId, ProtocolCallback callback)
        {

        }

        /// <summary>
        /// 发送协议
        /// </summary>
        /// <param name="protocol"></param>
        protected virtual void Send(IProtocol protocol)
        {

        }
    }
}
