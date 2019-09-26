using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public interface IActionView : IPooledObject
    {
        /// <summary>
        /// 类型
        /// </summary>
        int type { get; set; }

        /// <summary>
        /// 动作
        /// </summary>
        int action { get; set; }

        /// <summary>
        /// 方向
        /// </summary>
        ActionDirection direction { get; set; }

        /// <summary>
        /// 当前索引
        /// </summary>
        int frameIndex { get; set; }

        /// <summary>
        /// 最大索引
        /// </summary>
        int maxFrameIndex { get; set; }

        /// <summary>
        /// 排序层级
        /// </summary>
        string sortingLayer { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        int sortingIndex { get; set; }

        /// <summary>
        /// 强制排序
        /// </summary>
        int sortingOrder { get; set; }

        /// <summary>
        /// 帧频
        /// </summary>
        int[] frames { get; set; }

        /// <summary>
        /// 深度（排序用）
        /// </summary>
        int depth { get; set; }

        /// <summary>
        /// 是否循环
        /// </summary>
        bool loop { get; set; }

        /// <summary>
        /// 设置透明度
        /// </summary>
        float alpha { get; set; }

        /// <summary>
        /// gameObject
        /// </summary>
        GameObject gameObject { get; }

        /// <summary>
        /// transform
        /// </summary>
        Transform transform { get; }

        /// <summary>
        /// 添加动作渲染
        /// </summary>
        /// <param name="renderer"></param>
        void AddRenderer(IActionRenderer renderer);

        /// <summary>
        /// 移除动作渲染
        /// </summary>
        /// <param name="type"></param>
        IActionRenderer RemoveRenderer(int type);

        /// <summary>
        /// 移除动作渲染
        /// </summary>
        /// <param name="renderer"></param>
        void RemoveRenderer(IActionRenderer renderer);

        /// <summary>
        /// 获取动作渲染
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IActionRenderer GetRenderer(int type);

        /// <summary>
        /// 刷新动作渲染
        /// </summary>
        void RefreshRenderers();

        /// <summary>
        /// 设置动作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="direction"></param>
        /// <param name="callback"></param>
        void SetAction(int action, ActionDirection direction, Action callback = null);

        /// <summary>
        /// 跳转到下一帧
        /// </summary>
        void NextFrame();

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="position"></param>
        void SetPosition(Vector3 position);

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        void SetPosition(float x = 0, float y = 0, float z = 0);

        /// <summary>
        /// 重置
        /// </summary>
        void Reset();

        /// <summary>
        /// 销毁
        /// </summary>
        void Dispose();
    }
}
