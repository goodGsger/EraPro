using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public interface ISkillProxy
    {
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="attrID"></param>
        /// <returns></returns>
        int GetAttribute(int attrID);

        /// <summary>
        /// 是否拥有指定物品
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        bool HasItem(int itemID, int count);
    }
}
