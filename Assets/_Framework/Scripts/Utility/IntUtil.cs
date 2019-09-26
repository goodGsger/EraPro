using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public static class IntUtil
    {
        private static int counter = int.MinValue;

        /// <summary>
        /// 创建唯一数字
        /// </summary>
        /// <returns></returns>
        public static int CreateUniqueInt()
        {
            return ++counter;
        }
    }
}
