using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Framework
{
    public static class CryptographyUtil
    {
        /// <summary>
        /// 获取字符串MD5码
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetMD5(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            byte[] hash = md5.ComputeHash(bytes, 0, bytes.Length);
            string destString = "";
            for (int i = 0; i < bytes.Length; i++)
                destString += System.Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            destString = destString.PadLeft(32, '0');
            return destString;
        }

        /// <summary>
        /// 获取文件MD5码
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetMD5File(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                builder.Append(hash[i].ToString("x2"));
            return builder.ToString();
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string EncodeBase64(string text)
        {
            string base64 = null;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                base64 = Convert.ToBase64String(bytes);
            }
            catch (Exception)
            {

            }
            return base64;
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string DecodeBase64(string base64)
        {
            string text = null;
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                text = Encoding.UTF8.GetString(bytes);
            }
            catch (Exception)
            {

            }
            return text;
        }
    }
}
