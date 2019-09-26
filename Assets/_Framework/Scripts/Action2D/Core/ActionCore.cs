using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    /// <summary>
    /// 动作渲染深度计算器
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public delegate int ActionRendererDepthCalculator(ActionDirection direction, int type);

    /// <summary>
    /// 动作视图深度改变检查器
    /// </summary>
    /// <param name="oldDirection"></param>
    /// <param name="newDirection"></param>
    /// <returns></returns>
    public delegate bool ActionViewDepthChangeChecker(ActionDirection oldDirection, ActionDirection newDirection);

    public class ActionCore
    {
        /// <summary>
        /// 动作视图深度改变检查器
        /// </summary>
        public static ActionViewDepthChangeChecker actionViewDepthChangeChecker;

        /// <summary>
        /// 动作渲染深度计算器
        /// </summary>
        public static ActionRendererDepthCalculator actionRendererDepthCalculator;
    }
}
