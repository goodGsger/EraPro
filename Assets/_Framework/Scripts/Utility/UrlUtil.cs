using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public static class UrlUtil
    {
        private const string directorySeparatorChar = "\\";

        /// <summary>
        /// 根据url获取文件名
        /// 
        /// Test:
        /// Debug.Log(UrlUtil.GetFileName("Test.png"));
        /// Debug.Log(UrlUtil.GetFileName("Test"));
        /// Debug.Log(UrlUtil.GetFileName("/Test"));
        /// Debug.Log(UrlUtil.GetFileName("/Test.png"));
        /// Debug.Log(UrlUtil.GetFileName("Folder1/Folder2/Test.png"));
        /// Debug.Log(UrlUtil.GetFileName("Folder1/Folder2/Test"));
        /// Debug.Log(UrlUtil.GetFileName("Folder./Test"));
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetFileName(string url)
        {
            int indexS = url.LastIndexOf('/');
            if (indexS == -1)
                indexS = 0;
            else
                indexS += 1;

            int indexD = url.LastIndexOf('.');
            if (indexD == -1)
                indexD = url.Length;

            if (indexS > indexD)
                indexD = url.Length;

            return url.Substring(indexS, indexD - indexS);
        }

        public static string GetFileName2(string url)
        {
            int indexS = url.LastIndexOf('\\');
            if (indexS == -1)
                indexS = 0;
            else
                indexS += 1;

            int indexD = url.LastIndexOf('.');
            if (indexD == -1)
                indexD = url.Length;

            if (indexS > indexD)
                indexD = url.Length;

            return url.Substring(indexS, indexD - indexS);
        }

        /// <summary>
        /// 获取相对路径
        /// </summary>
        /// <param name="filespec"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static string GetRelativePath(string filespec, string folder)
        {
            Uri pathUri = new Uri(filespec);

            if (!folder.EndsWith(directorySeparatorChar))
            {
                folder += directorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);

            if (pathUri.AbsolutePath.ToCharArray()[0] != folderUri.AbsolutePath.ToCharArray()[0])
                return filespec;
            else
                return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace("/", directorySeparatorChar));
        }

        public static string GetRandomVersion(string url)
        {
            if (url.LastIndexOf("?") == -1)
            {
                return url + "?v=" + UnityEngine.Random.Range(0f, 1f);
            }
            else
            {
                return url + "&v=" + UnityEngine.Random.Range(0f, 1f);
            }
        }
    }
}
