using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class TouchContext
    {
        public int touchID;
        public Vector2 position;
        public Vector2 downPosition;
        public bool touchDown;
        public int clickCount;
        public float lastClickTime;

        public void Reset()
        {
            position = Vector2.zero;
            downPosition = Vector2.zero;
            touchDown = false;
            clickCount = 0;
            lastClickTime = 0f;
        }
    }
}
