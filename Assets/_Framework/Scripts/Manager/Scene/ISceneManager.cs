using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface IScenesManager : IManager
    {
        /// <summary>
        /// 注册场景
        /// </summary>
        /// <param name="scene"></param>
        void RegisterScene(IScene scene);

        /// <summary>
        /// 注销场景
        /// </summary>
        /// <param name="scene"></param>
        void UnregisterScene(IScene scene);

        /// <summary>
        /// 进入场景
        /// </summary>
        /// <param name="scene"></param>
        void EnterScene(IScene scene);
    }
}
