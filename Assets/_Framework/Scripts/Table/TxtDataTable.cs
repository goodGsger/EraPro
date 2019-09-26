using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class TxtDataTable : ITxtDataTable
    {
        /// <summary>
        /// 从Text中填充数据，返回唯一ID
        /// </summary>
        /// <param name="txtStr"></param>
        /// <returns></returns>
        public virtual int FillData(string[] txtStr)
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
        public virtual ITxtDataTable ClonePrototype()
        {
            return null;
        }
    }
}