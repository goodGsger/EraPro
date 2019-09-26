using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class ColorUtil
    {
        public static Color FromHexRGB(uint color)
        {
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8 & 0xff);
            byte b = (byte)(color & 0xff);
            return new Color32(r, g, b, 255);
        }

        public static Color FromHexARGB(uint color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16 & 0xffff);
            byte g = (byte)(color >> 8 & 0xff);
            byte b = (byte)(color & 0xff);
            return new Color32(r, g, b, a);
        }
    }
}
