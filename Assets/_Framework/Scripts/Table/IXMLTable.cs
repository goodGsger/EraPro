using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface IXMLTable : ITable
    {
        /// <summary>
        /// 从XML字符串中填充数据，返回唯一ID
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        int FillData(string xmlStr);

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        void InitData();

        /// <summary>
        /// 拷贝原型
        /// </summary>
        /// <returns></returns>
        IXMLTable ClonePrototype();
    }
}
