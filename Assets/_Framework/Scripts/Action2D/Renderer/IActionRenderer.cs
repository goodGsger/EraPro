using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public interface IActionRenderer : IPooledObject
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
        /// 帧索引
        /// </summary>
        int[] frames { get; set; }

        /// <summary>
        /// 排序图层
        /// </summary>
        string sortingLayer { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        int sortingIndex { get; set; }

        /// <summary>
        /// 强制深度
        /// </summary>
        int sortingOrder { get; set; }

        /// <summary>
        /// 深度
        /// </summary>
        int depth { get; set; }

        /// <summary>
        /// 是否循环
        /// </summary>
        bool loop { get; set; }

        /// <summary>
        /// 透明度
        /// </summary>
        float alpha { get; set; }

        /// <summary>
        /// 颜色滤镜
        /// </summary>
        ColorFilter colorFilter { get; set; }

        /// <summary>
        /// 是否锁定动作和方向
        /// </summary>
        bool lockActionAndDirection { get; set; }

        /// <summary>
        /// 锁定深度（不进行排序）
        /// </summary>
        bool lockDepth { get; set; }

        /// <summary>
        /// 锁定排序（不进行排序）
        /// </summary>
        bool lockSorting { get; set; }

        /// <summary>
        /// gameObject
        /// </summary>
        GameObject gameObject { get; }

        /// <summary>
        /// transform
        /// </summary>
        Transform transform { get; }

        /// <summary>
        /// 渲染器
        /// </summary>
        Renderer renderer { get; }

        /// <summary>
        /// Sprite
        /// </summary>
        Sprite sprite { get; }

        /// <summary>
        /// 动作资源
        /// </summary>
        ActionAsset actionAsset { get; }

        /// <summary>
        /// 设置动作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="direction"></param>
        /// <param name="callback"></param>
        void SetAction(int action, ActionDirection direction, Action callback = null);

        /// <summary>
        /// 刷新深度
        /// </summary>
        void RefreshDepth();

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
        /// 设置动作资源
        /// </summary>
        /// <param name="actionAsset"></param>
        void SetActionAsset(ActionAsset actionAsset);

        /// <summary>
        /// 动态加载动作数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="defaultUrl"></param>
        /// <param name="priority"></param>
        /// <param name="loadCallback"></param>
        void LoadAction(string url, string defaultUrl = null, LoadPriority priority = LoadPriority.LV_2, Action loadCallback = null);

        /// <summary>
        /// 停止加载动作数据
        /// </summary>
        void StopLoadAction();

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
