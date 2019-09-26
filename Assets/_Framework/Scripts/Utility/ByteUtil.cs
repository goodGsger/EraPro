using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public static class ByteUtil
    {
        /// <summary>
        /// 与
        /// </summary>
        /// <param name="expression1"></param>
        /// <param name="expression2"></param>
        /// <returns></returns>
        public static int AND(int expression1, int expression2)
        {
            return expression1 & expression2;
        }

        /// <summary>
        /// 或
        /// </summary>
        /// <param name="expression1"></param>
        /// <param name="expression2"></param>
        /// <returns></returns>
        public static int OR(int expression1, int expression2)
        {
            return expression1 | expression2;
        }

        /// <summary>
        /// 异或
        /// </summary>
        /// <param name="expression1"></param>
        /// <param name="expression2"></param>
        /// <returns></returns>
        public static int XOR(int expression1, int expression2)
        {
            return expression1 ^ expression2;
        }

        /// <summary>
        /// 非
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static int NOT(int expression)
        {
            return ~expression;
        }
    }
}
