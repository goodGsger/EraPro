using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public struct MapPos : IEqualityComparer<MapPos>
    {
        public int x;
        public int y;

        public MapPos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <returns></returns>
        public bool isNone
        {
            get { return x == 0 && y == 0; }
        }

        /// <summary>
        /// 设置为空
        /// </summary>
        /// <returns></returns>
        public void SetNone()
        {
            x = 0;
            y = 0;
        }

        /// <summary>
        /// 地图实际坐标X
        /// </summary>
        /// <returns></returns>
        public int realX
        {
            get { return x * MapSetting.gridWidth + MapSetting.gridWidthHalf; }
            set { x = (value - MapSetting.gridWidthHalf) / MapSetting.gridWidth; }
        }

        /// <summary>
        /// 地图实际坐标Y
        /// </summary>
        /// <returns></returns>
        public int realY
        {
            get { return y * MapSetting.gridHeight + MapSetting.gridHeightHalf; }
            set { y = (value - MapSetting.gridHeightHalf) / MapSetting.gridHeight; }
        }

        /// <summary>
        /// 地图实际坐标X四舍五入
        /// </summary>
        public int realXRound
        {
            set { x = (int)Mathf.Round(value / MapSetting.gridWidth); }
        }

        /// <summary>
        /// 地图实际坐标Y四舍五入
        /// </summary>
        public int realYRound
        {
            set { y = (int)Mathf.Round(value / MapSetting.gridHeight); }
        }

        /// <summary>
        /// 位置索引
        /// </summary>
        public int posKey
        {
            get { return x * 1000 + y; }
        }

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 设置地图坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isRound"></param>
        public void SetRealPosition(int x, int y, bool isRound = false)
        {
            if (isRound)
            {
                realXRound = x;
                realYRound = y;
            }
            else
            {
                realX = x;
                realY = y;
            }
        }

        /// <summary>
        /// 获取地图实际坐标点XY
        /// </summary>
        /// <returns></returns>
        public Vector2 GetRealPosition2D()
        {
            return new Vector2(realX, realY);
        }

        /// <summary>
        /// 获取地图实际坐标点XYZ
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRealPosition3D()
        {
            return new Vector3(realX, realY, 0);
        }

        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="scaleX"></param>
        /// <param name="scaleY"></param>
        public void Scale(float scaleX, float scaleY)
        {
            x = (int)(x * scaleX);
            y = (int)(y * scaleY);
        }

        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(float scale)
        {
            Scale(scale, scale);
        }

        /// <summary>
        /// 根据坐标获取相对于该坐标的方向
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public ActionDirection GetDirectionByMapPos(int x, int y)
        {
            return ActionDirectionUtil.GetDirection8(this.x, this.y, x, y);
        }

        /// <summary>
        /// 根据坐标获取相对于该坐标的方向
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public ActionDirection GetDirectionByMapPos(MapPos pos)
        {
            return GetDirectionByMapPos(pos.x, pos.y);
        }

        /// <summary>
        /// 根据实际坐标获取相对于该坐标的方向
        /// </summary>
        /// <param name="realX"></param>
        /// <param name="realY"></param>
        /// <returns></returns>
        public ActionDirection GetDirectionByRealPos(int realX, int realY)
        {
            return ActionDirectionUtil.GetDirection8(this.realX, this.realY, realX, realY);
        }

        /// <summary>
        /// 根据实际坐标获取相对于该坐标的方向
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public ActionDirection GetDirectionByRealPos(MapPos pos)
        {
            return GetDirectionByRealPos(pos.realX, pos.realY);
        }

        /// <summary>
        /// 根据方向获取指定距离的坐标点
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="dist"></param>
        /// <returns></returns>
        public MapPos GetPosByDirection(ActionDirection direction, int dist = 1)
        {
            Vector3 vector = ActionDirectionUtil.GetVectorByDirection(direction);
            return new MapPos(x + (int)vector.x * dist, y + (int)vector.y * dist);
        }

        /// <summary>
        /// 根据目标点获取指定距离的坐标点
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dist"></param>
        /// <returns></returns>
        public MapPos GetPosByMapPos(MapPos pos, int dist = 1)
        {
            ActionDirection direction = GetDirectionByMapPos(pos);
            return GetPosByDirection(direction, dist);
        }

        /// <summary>
        /// 根据目标点获取偏移
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public MapPos GetOffset(int x, int y)
        {
            return new MapPos(x - this.x, y - this.y);
        }

        /// <summary>
        /// 根据目标点获取偏移
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public MapPos GetOffset(MapPos pos)
        {
            return GetOffset(pos.x, pos.y);
        }

        /// <summary>
        /// 根据目标点获取地图距离
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int GetMapDistByMapPos(int x, int y)
        {
            MapPos offset = GetOffset(x, y);
            if (offset.x < 0)
                offset.x = -offset.x;
            if (offset.y < 0)
                offset.y = -offset.y;
            return offset.x > offset.y ? offset.x : offset.y;
        }

        /// <summary>
        /// 根据目标点获取地图距离
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public int GetMapDistByMapPos(MapPos pos)
        {
            return GetMapDistByMapPos(pos.x, pos.y);
        }

        /// <summary>
        /// 根据实际目标点获取实际距离
        /// </summary>
        /// <param name="realX"></param>
        /// <param name="realY"></param>
        /// <returns></returns>
        public float GetRealDistByRealPos(int realX, int realY)
        {
            return MathUtil.GetDistance(this.realX, this.realY, realX, realY);
        }

        /// <summary>
        /// 根据实际目标点获取实际距离
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public float GetRealDistByRealPos(MapPos pos)
        {
            return GetMapDistByMapPos(pos.realX, pos.realY);
        }

        /// <summary>
        /// 是否在指定点范围内
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool EqualsRange(int x, int y, int range)
        {
            int dist = GetMapDistByMapPos(x, y);
            return dist <= range;
        }

        /// <summary>
        /// 是否在指定点范围内
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public bool EqualsRange(MapPos pos, int range)
        {
            return EqualsRange(pos.x, pos.y, range);
        }

        /// <summary>
        /// 添加随机偏移
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <returns></returns>
        public MapPos AddRandomOffset(int offsetX, int offsetY)
        {
            x += Random.Range(-offsetX, offsetX);
            y += Random.Range(-offsetY, offsetY);
            return this;
        }

        /// <summary>
        /// 添加随机偏移
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public MapPos AddRandomOffset(int offset)
        {
            return AddRandomOffset(offset, offset);
        }

        /// <summary>
        /// 判断是否和指定点在一条直线
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool CheckAtOneLine(int x, int y)
        {
            return this.x == x || this.y == y || Mathf.Abs(this.x - x) == Mathf.Abs(this.y - y);
        }

        /// <summary>
        /// 判断是否和指定点在一条直线
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool CheckAtOneLine(MapPos pos)
        {
            return CheckAtOneLine(pos.x, pos.y);
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public MapPos Clone()
        {
            return new MapPos(x, y);
        }

        public bool Equals(MapPos a, MapPos b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public int GetHashCode(MapPos pos)
        {
            return pos.x * 1000 + pos.y;
        }

        public static MapPos operator -(MapPos a)
        {
            return new MapPos(-a.x, -a.y);
        }

        public static MapPos operator +(MapPos a, MapPos b)
        {
            return new MapPos(a.x + b.x, a.y + b.y);
        }

        public static MapPos operator -(MapPos a, MapPos b)
        {
            return new MapPos(a.x - b.x, a.y - b.y);
        }

        public static MapPos operator *(int d, MapPos a)
        {
            return new MapPos(d * a.x, d * a.y);
        }

        public static MapPos operator *(MapPos a, int d)
        {
            return new MapPos(a.x * d, a.y * d);
        }

        public static MapPos operator /(MapPos a, int d)
        {
            return new MapPos(a.x / d, a.y / d);
        }

        public static bool operator ==(MapPos lhs, MapPos rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public static bool operator !=(MapPos lhs, MapPos rhs)
        {
            return lhs.x != rhs.x || lhs.y != rhs.y;
        }

        public static implicit operator MapPos(Vector2 v)
        {
            return new MapPos((int)v.x, (int)v.y);
        }

        public static implicit operator Vector2(MapPos v)
        {
            return new Vector2(v.x, v.y);
        }

        public static implicit operator MapPos(Vector3 v)
        {
            return new MapPos((int)v.x, (int)v.y);
        }

        public static implicit operator Vector3(MapPos v)
        {
            return new Vector3(v.x, v.y);
        }
    }
}
