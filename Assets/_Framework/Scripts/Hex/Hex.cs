using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public struct Hex : IEqualityComparer<Hex>
    {
        /**
		 * 邻边六个方向
		 */
        public static readonly Hex[] directions = new Hex[] {
            new Hex(1, -1, 0),
            new Hex(1, 0, -1),
            new Hex(0, 1, -1),
            new Hex(-1, 1, 0),
            new Hex(-1, 0, 1),
            new Hex(0, -1, 1)
        };

        /**
		 * 邻边六个对角线方向
		 */
        public static readonly Hex[] diagonals = new Hex[] {
            new Hex(2, -1, -1),
            new Hex(1, 1, -2),
            new Hex(-1, 2, -1),
            new Hex(-2, 1, 1),
            new Hex(-1, -1, 2),
            new Hex(1, -2, 1)
        };

        public int x;
        public int y;
        public int z;

        public Hex(int x = 0, int y = 0, int z = 0)
        {
            if (x + y + z != 0)
                throw new Exception("Hex Constructor: x + y + z must be 0!");
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void SetXYZ(int x = 0, int y = 0, int z = 0)
        {
            if (x + y + z != 0)
                throw new Exception("Hex SetXYZ: must be 0!");
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// 相加
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Hex Add(int x = 0, int y = 0, int z = 0)
        {
            return new Hex(this.x + x, this.y + y, this.z + z);
        }

        /// <summary>
        /// 相加
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public Hex Add(Hex hex)
        {
            return new Hex(x + hex.x, y + hex.y, z + hex.z);
        }

        /// <summary>
        /// 相减
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Hex Subtract(int x = 0, int y = 0, int z = 0)
        {
            return new Hex(this.x - x, this.y - y, this.z - z);
        }

        /// <summary>
        /// 相减
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public Hex Subtract(Hex hex)
        {
            return new Hex(x - hex.x, y - hex.y, z - hex.z);
        }

        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public Hex Scale(float scale)
        {
            return new Hex((int)(x * scale), (int)(y * scale), (int)(z * scale));
        }

        /// <summary>
        /// 左旋
        /// </summary>
        /// <returns></returns>
        public Hex RotateLeft()
        {
            return new Hex(-y, -z, -x);
        }

        /// <summary>
        /// 右旋
        /// </summary>
        /// <returns></returns>
        public Hex RotateRight()
        {
            return new Hex(-z, -x, -y);
        }

        /// <summary>
        /// 获取相邻Hex
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Hex GetNeighbor(int direction)
        {
            if (direction > 6)
                throw new Exception("Hex GetNeighbor: direction must less than 6!");
            return Add(directions[direction]);
        }

        /// <summary>
        /// 获取相邻对角线Hex
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Hex GetDiagonalNeighbor(int direction)
        {
            if (direction > 6)
                throw new Exception("Hex GetDiagonalNeighbor: direction must less than 6!");
            return Add(diagonals[direction]);
        }

        /// <summary>
        /// 获取距离
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public int GetDistance(Hex hex)
        {
            return Subtract(hex).Length;
        }

        /// <summary>
        /// 长度
        /// </summary>
        public int Length
        {
            get { return (Math.Abs(x) + Math.Abs(y) + Math.Abs(z)) / 2; }
        }

        /// <summary>
        /// 转换字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return x + "," + y + "," + z;
        }

        public bool Equals(Hex a, Hex b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public int GetHashCode(Hex hex)
        {
            return hex.x * 1000000 + hex.y * 1000 + z;
        }
    }
}
