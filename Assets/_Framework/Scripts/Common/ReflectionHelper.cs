using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    /// <summary>
    /// 反射帮助类
    /// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// 根据Type创建实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// 根据Type创建实例（带参数）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object CreateInstance(Type type, object[] args)
        {
            return Activator.CreateInstance(type, args);
        }

        /// <summary>
        /// 根据TypeName创建实例
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static object CreateInstance(string typeName)
        {
            Type type = Type.GetType(typeName);
            if (type != null)
                return Activator.CreateInstance(type);

            return null;
        }

        /// <summary>
        /// 根据TypeName创建实例（带参数）
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object CreateInstance(string typeName, object[] args)
        {
            Type type = Type.GetType(typeName);
            if (type != null)
                return Activator.CreateInstance(type, args);

            return null;
        }
    }
}
