using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public enum NodeType
    {
        WALKABLE = 0,
        UNWALKABLE = 1,
        TRANSPARENT = 2
    }

    public class Node : IPooledObject
    {
        public int x;
        public int y;
        public float f;
        public float g;
        public float h;
        public bool moveable;
        public bool moveableOriginal;
        public Node parent;
        //public float costMultiplier = 1.0f;
        public int version;

        public Node()
        {

        }

        public Node(int x, int y, bool moveable)
        {
            this.x = x;
            this.y = y;
            this.moveable = moveableOriginal = moveable;
        }

        /// <summary>
        /// 初始化是否可行走
        /// </summary>
        /// <param name="moveable"></param>
        public void InitMoveable(bool moveable)
        {
            this.moveable = this.moveableOriginal = moveable;
        }

        /// <summary>
        /// 重设是否可行走
        /// </summary>
        public void ResetMoveable()
        {
            moveable = moveableOriginal;
        }

        /// <summary>
        /// 从对象池中获取
        /// </summary>
        public void OnPoolGet()
        {

        }

        /// <summary>
        /// 重置对象池对象
        /// </summary>
        public void OnPoolReset()
        {
            parent = null;
        }

        /// <summary>
        /// 销毁对象池对象
        /// </summary>
        public void OnPoolDispose()
        {
            parent = null;
        }
    }
}
