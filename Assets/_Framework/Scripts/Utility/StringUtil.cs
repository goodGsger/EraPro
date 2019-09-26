using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public static class StringUtil
    {
        public static readonly string[] WARP = new string[] { "\r\n" };
        public static string regValidChars = "^[a-zA-Z0-9_\u2e80-\u9fff]+$";

        /// <summary>
        /// 验证非法字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckInvalidChars(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, regValidChars);
        }
    }
}
