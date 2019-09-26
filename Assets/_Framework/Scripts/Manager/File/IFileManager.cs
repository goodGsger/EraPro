using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public interface IFileManager : IManager
    {
        /// <summary>
        /// 加载Resource资源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        Object LoadResource(string id, string path);

        /// <summary>
        /// 获取Resource资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        T LoadResourece<T>(string id, string path) where T : Object;

        /// <summary>
        /// 卸载Resource资源
        /// </summary>
        /// <param name="id"></param>
        void UnloadResource(string id);

        /// <summary>
        /// 判断Streaming文件夹是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool DirectoryExistsStreaming(string path);

        /// <summary>
        /// 判断Streaming文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool FileExistsStreaming(string path);

        /// <summary>
        /// 读取Streaming文件字节数组
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        byte[] ReadFileStreaming(string path);

        /// <summary>
        /// 读取Streaming文件文本
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string ReadTextStreaming(string path);

        /// <summary>
        /// 读取Streaming文本行
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string[] ReadLinesStreaming(string path);

        /// <summary>
        /// 从Steaming目录向Persistent目录复制文件
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        bool CopyFileStreaming(string sourcePath, string destPath, bool overwrite);

        /// <summary>
        /// 从Steaming目录向Persistent目录复制文件（异步）
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        bool CopyFileStreamingAsync(string sourcePath, string destPath, bool overwrite);

        /// <summary>
        /// 判断Persistent文件夹是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool DirectoryExistsPersistent(string path);

        /// <summary>
        /// 判断Persistent文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool FileExistsPersistent(string path);

        /// <summary>
        /// 读取Persistent文件字节数组
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        byte[] ReadFilePersistent(string path);

        /// <summary>
        /// 读取Persistent文件文本
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string ReadTextPersistent(string path);

        /// <summary>
        /// 读取Persistent文本行
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string[] ReadLinesPersistent(string path);

        /// <summary>
        /// 写入Persistent文件字节数组
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bytes"></param>
        /// <param name="overwite"></param>
        /// <returns></returns>
        bool WriteFilePersistent(string path, byte[] bytes, bool overwite);

        /// <summary>
        /// 写入Persistent文件字节数组（异步）
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bytes"></param>
        /// <param name="overwite"></param>
        /// <returns></returns>
        bool WriteFilePersistentAsync(string path, byte[] bytes, bool overwite);

        /// <summary>
        /// 写入Persistent文件文本
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        /// <param name="overwite"></param>
        /// <returns></returns>
        bool WriteTextPersistent(string path, string text, bool overwite);

        /// <summary>
        /// 写入Persistent文件文本（异步）
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        /// <param name="overwite"></param>
        /// <returns></returns>
        bool WriteTextPersistentAsync(string path, string text, bool overwite);

        /// <summary>
        /// Persistent删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool DeleteFilePersistent(string path);

        /// <summary>
        /// Persistent删除文件（异步）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool DeleteFilePersistentAsync(string path);

        /// <summary>
        /// Persistent目录移动文件
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        /// <returns></returns>
        bool MoveFilePersistent(string sourcePath, string destPath);

        /// <summary>
        /// Persistent目录移动文件（异步）
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        /// <returns></returns>
        bool MoveFilePersistentAsync(string sourcePath, string destPath);

        /// <summary>
        /// Persistent目录复制文件
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        bool CopyFilePersistent(string sourcePath, string destPath, bool overwrite);

        /// <summary>
        /// Persistent目录复制文件（异步）
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        bool CopyFilePersistentAsync(string sourcePath, string destPath, bool overwrite);

        /// <summary>
        /// Persistent删除目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool DeleteDirectoryPersistent(string path);

        /// <summary>
        /// Persistent删除目录（异步）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool DeleteDirectoryPersistentAsync(string path);

        /// <summary>
        /// 是否存在文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool HasFile(string path);

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string GetFilePath(string path);
    }
}
