using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class TextureExtParam
    {
        public int index;
        public string name;
        public Rect rect;
        public Vector2 uv;

        /// <summary>
        /// x坐标
        /// </summary>
        public float x
        {
            get { return rect.x; }
        }

        /// <summary>
        /// y坐标
        /// </summary>
        public float y
        {
            get { return rect.y; }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        public float width
        {
            get { return rect.width; }
        }

        /// <summary>
        /// 高度
        /// </summary>
        public float height
        {
            get { return rect.height; }
        }
    }
}
