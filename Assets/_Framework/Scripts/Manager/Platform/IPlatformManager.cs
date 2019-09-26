using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface IPlatformManager : IManager
    {
        /// <summary>
        /// 网络状态
        /// </summary>
        int networkReachability { get; }

        /// <summary>
        /// 网络切换
        /// </summary>
        Action networkChanged { get; }
    }
}
