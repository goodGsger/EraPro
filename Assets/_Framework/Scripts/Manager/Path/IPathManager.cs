using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface IPathManager : IManager
    {
        /// <summary>
        /// 本地Streaming路径
        /// </summary>
        string streamingAssetPathFile { get; set; }

        /// <summary>
        /// 本地Persistent路径
        /// </summary>
        string persistentDataPathFile { get; set; }

        /// <summary>
        ///  WWWStreaming路径
        /// </summary>
        string streamingAssetPathWWW { get; set; }

        /// <summary>
        /// WWWPersistent路径
        /// </summary>
        string persistentDataPathWWW { get; set; }

        /// <summary>
        /// 外部资源路径
        /// </summary>
        string externalPath { get; set; }

        /// <summary>
        /// 资源根目录
        /// </summary>
        string root { get; set; }

        /// <summary>
        /// lua目录
        /// </summary>
        string lua { get; set; }

        /// <summary>
        /// ui目录
        /// </summary>
        string ui { get; set; }

        /// <summary>
        /// config目录
        /// </summary>
        string config { get; set; }

        /// <summary>
        /// 地图目录
        /// </summary>
        string map { get; set; }

        /// <summary>
        /// 地图asssetBundle目录
        /// </summary>
        string map_ab { get; set; }

        /// <summary>
        /// 动作目录
        /// </summary>
        string action { get; set; }

        /// <summary>
        /// 特效目录
        /// </summary>
        string effect { get; set; }

        /// <summary>
        /// 音频目录
        /// </summary>
        string audio { get; set; }

        /// <summary>
        /// 纹理目录
        /// </summary>
        string texture { get; set; }

        /// <summary>
        /// 录音文件目录
        /// </summary>
        string record { get; set; }
    }
}
