using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class XMLTable : IXMLTable
    {
        /// <summary>
        /// 从XML字符串中填充数据，返回唯一ID
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public virtual int FillData(string xmlStr)
        {
            return 0;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public virtual void InitData()
        {

        }

        /// <summary>
        /// 拷贝原型
        /// </summary>
        /// <returns></returns>
        public virtual IXMLTable ClonePrototype()
        {
            return null;
        }
    }
}
