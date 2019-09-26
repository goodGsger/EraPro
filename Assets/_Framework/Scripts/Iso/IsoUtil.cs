using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class IsoUtil
    {
        public const float Y_CORRECT = 1.224745f; // Mathf.Cos(-Mathf.PI / 6) * Mathf.Sqrt(2)

        /// <summary>
        /// 空间3D坐标转换为屏幕坐标
        /// </summary>
        /// <param name="iso"></param>
        /// <returns></returns>
        public static Vector2 IsoToScreen(Vector3 iso)
        {
            float screenX = iso.x - iso.z;
            float screenY = iso.y * Y_CORRECT + (iso.x + iso.z) * 0.5f;
            return new Vector2(screenX, screenY);
        }

        /// <summary>
        /// 屏幕坐标转换为空间3D坐标转换
        /// </summary>
        /// <param name="screen"></param>
        /// <returns></returns>
        public static Vector3 ScreenToIso(Vector2 screen)
        {
            float x = screen.y + screen.x * 0.5f;
            float y = 0;
            float z = screen.y - screen.x * 0.5f;
            return new Vector3(x, y, z);
        }
    }
}