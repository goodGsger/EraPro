using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Framework
{
    public class ThreadFileManager : Manager
    {
        private struct FileData
        {
            public FileAction action;
            public bool isDirectory;
            public string path;
            public string destPath;
            public byte[] bytes;
        }

        private enum FileAction
        {
            save = 1,
            delete = 2,
            move = 3,
            copy = 4
        }

        private Dictionary<string, Object> _resourceDict;
        private Queue<FileData> _fileDataQueue;
        private Thread _thread;

        protected override void Init()
        {
            _resourceDict = new Dictionary<string, Object>();
            _fileDataQueue = new Queue<FileData>();
            _thread = new Thread(HandleFile);
            _thread.IsBackground = true;
            _thread.Start();
        }

        private void HandleFile()
        {
            while (true)
            {
                while (_fileDataQueue.Count > 0)
                {
                    lock (_fileDataQueue)
                    {
                        FileData data = _fileDataQueue.Dequeue();
                        string path = data.path;

                        if (path != null)
                        {
                            switch (data.action)
                            {
                                case FileAction.save:
                                    if (data.isDirectory == false)
                                        FileHelper.WriteFile(path, data.bytes, true);
                                    break;
                                case FileAction.delete:
                                    if (data.isDirectory)
                                        FileHelper.DeleteDirectory(path);
                                    else
                                        FileHelper.DeleteFile(path);
                                    break;
                                case FileAction.move:
                                    if (data.destPath != null)
                                    {
                                        if (data.isDirectory)
                                            FileHelper.MoveDirectory(path, data.destPath);
                                        else
                                            FileHelper.MoveFile(path, data.destPath);
                                    }
                                    break;
                                case FileAction.copy:
                                    if (data.destPath != null)
                                    {
                                        if (data.isDirectory)
                                            FileHelper.CopyDirectory(path, data.destPath);
                                        else
                                            FileHelper.CopyFile(path, data.destPath, true);
                                    }
                                    break;
                            }
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 加载Resource资源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public Object LoadResource(string id, string path)
        {
            Object res;
            if (_resourceDict.TryGetValue(id, out res))
                return res;

            res = _resourceDict[id] = Resources.Load(path);
            return res;
        }

        /// <summary>
        /// 获取Resource资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public T LoadResourece<T>(string id, string path) where T : Object
        {
            Object resource = LoadResource(id, path);
            if (resource == null)
                return default(T);

            return (T)LoadResource(id, path);
        }

        /// <summary>
        /// 卸载Resource资源
        /// </summary>
        /// <param name="id"></param>
        public void UnloadResource(string id)
        {
            Object res;
            if (_resourceDict.TryGetValue(id, out res))
                Resources.UnloadAsset(res);
        }

        /// <summary>
        /// 判断Streaming文件夹是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool DirectoryExistsStreaming(string path)
        {
            return FileHelper.DirectoryExists(App.pathManager.streamingAssetPathFile + path);
        }

        /// <summary>
        /// 判断Streaming文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool FileExistsStreaming(string path)
        {
            return FileHelper.FileExists(App.pathManager.streamingAssetPathFile + path);
        }

        /// <summary>
        /// 读取Streaming文件字节数组
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public byte[] ReadFileStreaming(string path)
        {
            return FileHelper.ReadFile(App.pathManager.streamingAssetPathFile + path);
        }

        /// <summary>
        /// 读取Streaming文件文本
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ReadTextStreaming(string path)
        {
            return FileHelper.ReadText(App.pathManager.streamingAssetPathFile + path);
        }

        /// <summary>
        /// 读取Streaming文本行
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] ReadLinesStreaming(string path)
        {
            return FileHelper.ReadLines(App.pathManager.streamingAssetPathFile + path);
        }

        /// <summary>
        /// 从Steaming目录向Persistent目录复制文件
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public bool CopyFileStreaming(string sourcePath, string destPath, bool overwrite)
        {
            FileData fileData = new FileData();
            fileData.action = FileAction.copy;
            fileData.isDirectory = false;
            fileData.path = App.pathManager.streamingAssetPathFile + sourcePath;
            fileData.destPath = App.pathManager.persistentDataPathFile + destPath;
            _fileDataQueue.Enqueue(fileData);
            return true;
        }

        /// <summary>
        /// 判断Persistent文件夹是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool DirectoryExistsPersistent(string path)
        {
            return FileHelper.DirectoryExists(App.pathManager.persistentDataPathFile + path);
        }

        /// <summary>
        /// 判断Persistent文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool FileExistsPersistent(string path)
        {
            return FileHelper.FileExists(App.pathManager.persistentDataPathFile + path);
        }

        /// <summary>
        /// 读取Persistent文件字节数组
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public byte[] ReadFilePersistent(string path)
        {
            return FileHelper.ReadFile(App.pathManager.persistentDataPathFile + path);
        }

        /// <summary>
        /// 读取Persistent文件文本
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ReadTextPersistent(string path)
        {
            return FileHelper.ReadText(App.pathManager.persistentDataPathFile + path);
        }

        /// <summary>
        /// 读取Persistent文本行
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] ReadLinesPersistent(string path)
        {
            return FileHelper.ReadLines(App.pathManager.persistentDataPathFile + path);
        }

        /// <summary>
        /// 写入Persistent文件字节数组
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bytes"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public bool WriteFilePersistent(string path, byte[] bytes, bool overwrite)
        {
            FileData fileData = new FileData();
            fileData.action = FileAction.save;
            fileData.isDirectory = false;
            fileData.path = App.pathManager.persistentDataPathFile + path;
            fileData.bytes = bytes;
            _fileDataQueue.Enqueue(fileData);
            return true;
        }

        /// <summary>
        /// 写入Persistent文件文本
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public bool WriteTextPersistent(string path, string text, bool overwrite)
        {
            FileData fileData = new FileData();
            fileData.action = FileAction.save;
            fileData.isDirectory = false;
            fileData.path = App.pathManager.persistentDataPathFile + path;
            fileData.bytes = Encoding.Default.GetBytes(text);
            _fileDataQueue.Enqueue(fileData);
            return true;
        }

        /// <summary>
        /// Persistent删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool DeleteFilePersistent(string path)
        {
            FileData fileData = new FileData();
            fileData.action = FileAction.delete;
            fileData.isDirectory = false;
            fileData.path = App.pathManager.persistentDataPathFile + path;
            _fileDataQueue.Enqueue(fileData);
            return true;
        }

        /// <summary>
        /// Persistent目录移动文件
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        /// <returns></returns>
        public bool MoveFilePersistent(string sourcePath, string destPath)
        {
            FileData fileData = new FileData();
            fileData.action = FileAction.move;
            fileData.isDirectory = false;
            fileData.path = App.pathManager.persistentDataPathFile + sourcePath;
            fileData.destPath = App.pathManager.persistentDataPathFile + destPath;
            _fileDataQueue.Enqueue(fileData);
            return true;
        }

        /// <summary>
        /// Persistent目录复制文件
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public bool CopyFilePersistent(string sourcePath, string destPath, bool overwrite)
        {
            FileData fileData = new FileData();
            fileData.action = FileAction.copy;
            fileData.isDirectory = false;
            fileData.path = App.pathManager.persistentDataPathFile + sourcePath;
            fileData.destPath = App.pathManager.persistentDataPathFile + destPath;
            _fileDataQueue.Enqueue(fileData);
            return true;
        }

        /// <summary>
        /// Persistent删除目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool DeleteDirectoryPersistent(string path)
        {
            FileData fileData = new FileData();
            fileData.action = FileAction.delete;
            fileData.isDirectory = true;
            fileData.path = App.pathManager.persistentDataPathFile + path;
            _fileDataQueue.Enqueue(fileData);
            return true;
        }
    }
}
