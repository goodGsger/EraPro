using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface ILoader : IPooledObject
    {
        /// <summary>
        /// 相对路径
        /// </summary>
        string urlRelative { get; set; }

        /// <summary>
        /// 绝对路径（实际加载路径）
        /// </summary>
        string urlAbsolute { get; }

        /// <summary>
        /// 下载类型
        /// </summary>
        LoadType type { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        Dictionary<string, object> loadParams { get; set; }

        /// <summary>
        /// 统计
        /// </summary>
        LoadStats stats { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        LoadState state { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        object data { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        IAsset asset { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        string error { get; set; }

        /// <summary>
        /// 开始加载
        /// </summary>
        void Start();

        /// <summary>
        /// 开始加载
        /// </summary>
        /// <param name="url"></param>
        void Start(string url);

        /// <summary>
        /// 停止加载
        /// </summary>
        void Stop();

        /// <summary>
        /// 销毁
        /// </summary>
        void Dispose();

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="param"></param>
        void SetParam(string id, object param);

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        object GetParam(string id);

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="param"></param>
        void AddParam(LoadParam param);

        /// <summary>
        /// 开始回调
        /// </summary>
        Action<ILoader> startCallback { get; set; }

        /// <summary>
        /// 停止回调
        /// </summary>
        Action<ILoader> stopCallback { get; set; }

        /// <summary>
        /// 成功回调
        /// </summary>
        Action<ILoader> completeCallback { get; set; }

        /// <summary>
        /// 失败回调
        /// </summary>
        Action<ILoader> errorCallback { get; set; }

        /// <summary>
        /// 更新进度回调
        /// </summary>
        Action<ILoader> progressCallback { get; set; }
    }
}
