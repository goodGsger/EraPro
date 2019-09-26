using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface IModule
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        string moduleName { get; set; }

        /// <summary>
        /// 当前所属场景
        /// </summary>
        IScene scene { get; }

        /// <summary>
        /// 上一所属场景
        /// </summary>
        IScene lastScene { get; }

        /// <summary>
        /// 进入模块
        /// </summary>
        void EnterModule();

        /// <summary>
        /// 退出模块
        /// </summary>
        void ExitModule();

        /// <summary>
        /// 改变场景
        /// </summary>
        /// <param name="scene"></param>
        void ChangeScene(IScene scene);
    }
}
