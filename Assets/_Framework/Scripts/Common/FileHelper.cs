using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class FileHelper
    {
        /// <summary>
        /// 文件夹是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CreateDirectory(string path)
        {
            if (Directory.Exists(path))
                return true;

            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception exception)
            {
                App.logManager.Error("FileHelper.CreateDirectory Error:" + exception.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 根据文件创建目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CreateFileDirectory(string path)
        {
            int index = path.LastIndexOf("/");
            string folder = path.Substring(0, index);

            return CreateDirectory(folder);
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DeleteDirectory(string path)
        {
            if (!Directory.Exists(path))
                return true;

            try
            {
                Directory.Delete(path, true);
            }
            catch (Exception exception)
            {
                App.logManager.Error("FileHelper.DeleteDirectory Error:" + exception.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// 读取字节数组
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] ReadFile(string path)
        {
            if (FileExists(path))
            {
                byte[] bytes = null;
                try
                {
                    bytes = File.ReadAllBytes(path);
                }
                catch (Exception exception)
                {
                    App.logManager.Error("FileHelper.ReadFile Error:" + exception.ToString());
                }
                return bytes;
            }
            return null;
        }

        /// <summary>
        /// 读取文本
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadText(string path)
        {
            if (FileExists(path))
            {
                string text = null;
                try
                {
                    text = File.ReadAllText(path);
                }
                catch (Exception exception)
                {
                    App.logManager.Error("FileHelper.ReadText Error:" + exception.ToString());
                }
                return text;
            }
            return null;
        }

        /// <summary>
        /// 读取文本行
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] ReadLines(string path)
        {
            if (FileExists(path))
            {
                string[] lines = null;
                try
                {
                    lines = File.ReadAllLines(path);
                }
                catch (Exception exception)
                {
                    App.logManager.Error("FileHelper.ReadLines Error:" + exception.ToString());
                }
                return lines;
            }

            return null;
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="folder"></param>
        /// <param name="name"></param>
        /// <param name="bytes"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static bool WriteFile(string path, string folder, string name, byte[] bytes, bool overwrite)
        {
            if (bytes == null)
                return false;

            if (overwrite == false && File.Exists(path))
                return false;

            if (!CreateDirectory(folder))
                return false;

            try
            {
                File.WriteAllBytes(path, bytes);
            }
            catch (Exception exception)
            {
                App.logManager.Error("FileHelper.WriteFile Error:" + exception.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bytes"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static bool WriteFile(string path, byte[] bytes, bool overwrite)
        {
            if (bytes == null)
                return false;

            int index = path.LastIndexOf("/");
            string name = path.Substring(index + 1, path.Length - index - 1);
            string folder = path.Substring(0, index);

            return WriteFile(path, folder, name, bytes, overwrite);
        }

        /// <summary>
        /// 写入文本
        /// </summary>
        /// <param name="path"></param>
        /// <param name="folder"></param>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static bool WriteText(string path, string folder, string name, string text, bool overwrite)
        {
            if (text == null)
                return false;

            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return WriteFile(path, folder, name, bytes, overwrite);
        }

        /// <summary>
        /// 写入文本
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static bool WriteText(string path, string text, bool overwrite)
        {
            if (text == null)
                return false;

            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return WriteFile(path, bytes, overwrite);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DeleteFile(string path)
        {
            if (!File.Exists(path))
                return false;

            try
            {
                File.Delete(path);
            }
            catch (Exception exception)
            {
                App.logManager.Error("FileHelper.DeleteFile Error:" + exception.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        /// <param name="destFolder"></param>
        /// <returns></returns>
        public static bool MoveFile(string sourcePath, string destPath, string destFolder)
        {
            if (!File.Exists(sourcePath))
                return false;

            if (File.Exists(destPath))
                return false;

            if (!CreateDirectory(destFolder))
                return false;

            try
            {
                File.Move(sourcePath, destPath);
            }
            catch (Exception exception)
            {
                App.logManager.Error("FileHelper.MoveFile Error:" + exception.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        /// <returns></returns>
        public static bool MoveFile(string sourcePath, string destPath)
        {
            int index = destPath.LastIndexOf("/");
            string folder = destPath.Substring(0, index);
            return MoveFile(sourcePath, destPath, folder);
        }

        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        /// <param name="destFolder"></param>
        /// <param name="overwirte"></param>
        /// <returns></returns>
        public static bool CopyFile(string sourcePath, string destPath, string destFolder, bool overwirte)
        {
            if (!File.Exists(sourcePath))
                return false;

            if (!overwirte && File.Exists(destPath))
                return false;

            if (!CreateDirectory(destFolder))
                return false;

            try
            {
                File.Copy(sourcePath, destPath, overwirte);
            }
            catch (Exception exception)
            {
                App.logManager.Error("FileHelper.CopyFile Error:" + exception.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        /// <param name="overwirte"></param>
        /// <returns></returns>
        public static bool CopyFile(string sourcePath, string destPath, bool overwirte)
        {
            int index = destPath.LastIndexOf("/");
            string folder = destPath.Substring(0, index);
            return CopyFile(sourcePath, destPath, folder, overwirte);
        }

        /// <summary>
        ///  拷贝目录
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        public static void CopyDirectory(string sourcePath, string destPath)
        {
            CreateDirectory(destPath);

            string[] files = Directory.GetFiles(sourcePath);
            foreach (string filePath in files)
            {
                CopyFile(filePath, new StringBuilder(destPath).Append("/").Append(Path.GetFileName(filePath)).ToString(), true);
            }

            string[] folders = Directory.GetDirectories(sourcePath);
            foreach (string folderPath in folders)
            {
                CopyDirectory(folderPath, new StringBuilder(destPath).Append("/").Append(Path.GetFileName(folderPath)).ToString());
            }
        }

        /// <summary>
        /// 移动文件夹
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        public static void MoveDirectory(string sourcePath, string destPath)
        {
            if (Directory.Exists(sourcePath))
            {
                Directory.Move(sourcePath, destPath);
            }
        }

        /// <summary>
        /// 获取所有目录
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <returns></returns>
        public static List<string> GetAllDirectories(string sourcePath)
        {
            List<string> folders = new List<string>();
            GetAllDirectoriesInner(folders, sourcePath);
            return folders;
        }

        private static void GetAllDirectoriesInner(List<string> list, string sourcePath)
        {
            if (Directory.Exists(sourcePath))
            {
                string[] folders = Directory.GetDirectories(sourcePath);
                list.AddRange(folders);
                foreach (string folder in folders)
                {
                    GetAllDirectoriesInner(list, folder);
                }
            }
        }

        /// <summary>
        /// 获取所有文件
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="searchPattern"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static List<string> GetAllFiles(string sourcePath, string searchPattern = "*", SearchOption option = SearchOption.AllDirectories)
        {
            List<string> files = new List<string>();
            List<string> folders = new List<string>() { sourcePath };
            folders.AddRange(GetAllDirectories(sourcePath));
            foreach (string folder in folders)
            {
                if (Directory.Exists(folder))
                {
                    string[] folderFiles = Directory.GetFiles(folder, searchPattern, option);
                    files.AddRange(folderFiles);
                }
            }
            return files;
        }
    }
}
