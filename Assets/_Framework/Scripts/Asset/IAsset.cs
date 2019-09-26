using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public interface IAsset
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        string id { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        string url { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        object asset { get; set; }

        /// <summary>
        /// assetBundle
        /// </summary>
        AssetBundle assetBundle { get; set; }

        /// <summary>
        /// 使用计数
        /// </summary>
        int useCount { get; set; }

        /// <summary>
        /// 最后使用时间
        /// </summary>
        double lastUseTime { get; set; }

        /// <summary>
        /// 是否自动清理
        /// </summary>
        bool autoClear { get; set; }

        /// <summary>
        /// 获取字节数组
        /// </summary>
        /// <returns></returns>
        byte[] GetBytes();

        /// <summary>
        /// 使用计数
        /// </summary>
        /// <param name="count"></param>
        void Use(int count = 1);

        /// <summary>
        /// 不使用计数
        /// </summary>
        /// <param name="count"></param>
        void Unuse(int count = 1);

        /// <summary>
        /// 添加到资源管理器中
        /// </summary>
        void OnAdd();

        /// <summary>
        /// 销毁
        /// </summary>
        void Dispose();
    }
}
