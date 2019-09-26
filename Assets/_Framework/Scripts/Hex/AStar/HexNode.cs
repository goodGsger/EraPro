using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class HexNode : IPooledObject
    {
        public Hex hex;
        public float f;
        public float g;
        public float h;
        public bool moveable;
        public HexNode parent;
        //public float costMultiplier = 1.0f;
        public int version;

        public HexNode()
        {
        }

        public HexNode(Hex hex, bool moveable)
        {
            this.hex = hex;
            this.moveable = moveable;
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
