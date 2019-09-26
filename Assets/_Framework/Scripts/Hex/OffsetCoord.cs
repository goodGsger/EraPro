using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public struct OffsetCoord
    {
        // 平
        public const int FLAT = 1;
        // 尖
        public const int POINTY = 2;
        // 奇数偏移
        public const int ODD = -1;
        // 偶数偏移
        public const int EVEN = 1;
        // Math.Sqrt(3)
        public const float SQRT_3 = 1.732051f;

        public int x;
        public int y;

        public OffsetCoord(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 尖六边形排列按指定偏移pixel转offset
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="point"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static OffsetCoord GetROffsetCoordFromPixel(int offset, Vector2 point, float size)
        {
            float gridWidth = size * SQRT_3 / 2;
            float gridHeight = size * 1.5f;
            float offsetOdd = (gridHeight * gridHeight - gridWidth * gridWidth) / 2;
            float offsetEven = (gridHeight * gridHeight + gridWidth * gridWidth) / 2;

            int gridX = (int)(point.x / gridWidth);
            int gridY = (int)(point.y / gridHeight);
            float x = point.x - gridX * gridWidth;
            float y = point.y - gridY * gridHeight;

            if ((gridY & 1) != 0)
            {
                if (y * gridHeight - x * gridWidth > offsetOdd)
                    gridY++;
            }
            else
            {
                if (y * gridHeight + x * gridWidth > offsetEven)
                    gridY++;
            }

            gridX = (gridX + (1 + offset * (gridY & 1))) / 2;

            return new OffsetCoord(gridX, gridY);
        }

        /// <summary>
        /// 平六边形排列按指定偏移pixel转offset
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="point"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static OffsetCoord GetQOffsetCoordFromPixel(int offset, Vector2 point, float size)
        {
            float gridWidth = size * 1.5f;
            float gridHeight = size * SQRT_3 / 2;
            float offsetOdd = (gridWidth * gridWidth - gridHeight * gridHeight) / 2;
            float offsetEven = (gridWidth * gridWidth + gridHeight * gridHeight) / 2;

            int gridX = (int)(point.x / gridWidth);
            int gridY = (int)(point.y / gridHeight);
            float x = point.x - gridX * gridWidth;
            float y = point.y - gridY * gridHeight;

            if ((gridX & 1) != 0)
            {
                if (x * gridWidth - y * gridHeight > offsetOdd)
                    gridX++;
            }
            else
            {
                if (x * gridWidth + y * gridHeight > offsetEven)
                    gridX++;
            }

            gridY = (gridY + (1 + offset * (gridX & 1))) / 2;

            return new OffsetCoord(gridX, gridY);
        }

        /// <summary>
        /// 尖六边形排列指定偏移cube转offset
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static OffsetCoord GetROffsetCoordFromCube(int offset, Hex hex)
        {
            int x = hex.x + (hex.z + offset * (hex.z & 1)) / 2;
            int y = hex.z;
            return new OffsetCoord(x, y);
        }

        /// <summary>
        /// 平六边形排列按指定偏移cube转offset
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static OffsetCoord GetQOffsetCoordFromCube(int offset, Hex hex)
        {
            int x = hex.x;
            int y = hex.z + (hex.x + offset * (hex.x & 1)) / 2;
            return new OffsetCoord(x, y);
        }

        /// <summary>
        /// 尖六边形排列按指定偏移offset转pixel
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="offsetCoord"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Vector2 GetRPixelFromOffsetCoord(int offset, OffsetCoord offsetCoord, float size)
        {
            float px = size * SQRT_3 * (offsetCoord.x - offset * (offsetCoord.y & 1) / 2f);
            float py = size * 1.5f * offsetCoord.y;
            return new Vector2(px, py);
        }

        /// <summary>
        /// 平六边形排列按指定偏移cube转pixel
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="offsetCoord"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Vector2 GetQPixelFromOffsetCoord(int offset, OffsetCoord offsetCoord, float size)
        {
            float px = size * 1.5f * offsetCoord.x;
            float py = size * SQRT_3 * (offsetCoord.y - offset * (offsetCoord.x & 1) / 2f);
            return new Vector2(px, py);
        }

        /// <summary>
        /// 尖六边形排列按指定偏移cube转pixel
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="hex"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Vector2 GetRPixelFromCube(int offset, Hex hex, float size)
        {
            OffsetCoord offsetCoord = GetROffsetCoordFromCube(offset, hex);
            return GetRPixelFromOffsetCoord(offset, offsetCoord, size);
        }

        /// <summary>
        /// 平六边形排列按指定偏移cube转pixel
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="hex"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Vector2 GetQPixelFromCube(int offset, Hex hex, float size)
        {
            OffsetCoord offsetCoord = GetQOffsetCoordFromCube(offset, hex);
            return GetQPixelFromOffsetCoord(offset, offsetCoord, size);
        }

        /// <summary>
        /// 尖六边形排列按指定偏移offset转cube
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static Hex GetCubeFromROffsetCoord(int offset, OffsetCoord offsetCoord)
        {
            int x = offsetCoord.x - (offsetCoord.y + offset * (offsetCoord.y & 1)) / 2;
            int z = offsetCoord.y;
            int y = -x - z;
            return new Hex(x, y, z);
        }

        /// <summary>
        /// 平六边形排列按指定偏移offset转cube
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static Hex GetCubeFromQOffsetCoord(int offset, OffsetCoord offsetCoord)
        {
            int x = offsetCoord.x;
            int z = offsetCoord.y - (offsetCoord.x + offset * (offsetCoord.x & 1)) / 2;
            int y = -x - z;
            return new Hex(x, y, z);
        }

        /// <summary>
        /// 尖六边形排列按指定偏移Pixel转cube
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="point"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Hex GetCubeFromRPixel(int offset, Vector2 point, float size)
        {
            OffsetCoord offsetCoord = GetROffsetCoordFromPixel(offset, point, size);
            return GetCubeFromROffsetCoord(offset, offsetCoord);
        }

        /// <summary>
        /// 平六边形排列按指定偏移Pixel转cube
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="point"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Hex GetCubeFromQPixel(int offset, Vector2 point, float size)
        {
            OffsetCoord offsetCoord = GetQOffsetCoordFromPixel(offset, point, size);
            return GetCubeFromQOffsetCoord(offset, offsetCoord);
        }
    }
}
