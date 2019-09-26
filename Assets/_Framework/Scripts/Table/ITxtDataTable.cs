using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface ITxtDataTable : ITable
    {
        /// <summary>
        /// 从Text中填充数据，返回唯一ID
        /// </summary>
        /// <param name="txtStr"></param>
        /// <returns></returns>
        int FillData(string[] txtStr);

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        void InitData();

        /// <summary>
        /// 拷贝原型
        /// </summary>
        /// <returns></returns>
        ITxtDataTable ClonePrototype();
    }
}
