using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface IScene
    {
        /// <summary>
        /// 场景名称
        /// </summary>
        string sceneName { get; set; }

        /// <summary>
        /// 场景状态
        /// </summary>
        SceneState state { get; set; }

        /// <summary>
        /// 场景内模块
        /// </summary>
        List<IModule> modules { get; }

        /// <summary>
        /// 场景进入回调
        /// </summary>
        Action<IScene> enterCallback { get; set; }

        /// <summary>
        /// 场景退出回调
        /// </summary>
        Action<IScene> exitCallback { get; set; }

        /// <summary>
        /// 进入场景
        /// </summary>
        void EnterScene();

        /// <summary>
        /// 退出场景
        /// </summary>
        void ExitScene();

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="module"></param>
        void AddModule(IModule module);

        /// <summary>
        /// 移除模块
        /// </summary>
        /// <param name="module"></param>
        void RemoveModule(IModule module);

        /// <summary>
        /// 是否含有模块
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        bool HasModule(IModule module);
    }
}
