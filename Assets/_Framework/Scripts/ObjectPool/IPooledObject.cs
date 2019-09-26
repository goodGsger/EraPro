using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
   public interface IPooledObject
    {
        /// <summary>
        /// 从对象池中获取
        /// </summary>
        void OnPoolGet();

        /// <summary>
        /// 重置对象池对象
        /// </summary>
        void OnPoolReset();

        /// <summary>
        /// 销毁对象池对象
        /// </summary>
        void OnPoolDispose();
    }
}
