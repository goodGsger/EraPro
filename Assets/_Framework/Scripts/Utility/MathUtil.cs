using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public static class MathUtil
    {
        /// <summary>
        /// 整数随机
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int RandomRangeInt(float min, float max)
        {
            return (int)(Mathf.Floor(UnityEngine.Random.Range(0f, 1f) * (max - min + 1)) + min);
        }

        /// <summary>
        /// 浮点数随机
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float RandomRangeFloat(float min, float max)
        {
            return Mathf.Floor(UnityEngine.Random.Range(0f, 1f) * (max - min + 1)) + min;
        }

        /// <summary>
        /// 范围限制
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static float RangeLimit(float value, float minValue, float maxValue)
        {
            if (value < minValue)
                value = minValue;
            if (value > maxValue)
                value = maxValue;

            return value;
        }

        /// <summary>
        /// 四舍五入保留小数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dot"></param>
        /// <returns></returns>
        public static float RoundFixed(float value, int dot)
        {
            dot = (int)RangeLimit(dot, 0, 16);
            float num = Mathf.Pow(10, dot);
            return Mathf.Round(value * num) / num;
        }

        /// <summary>
        /// 向下取整保留小数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dot"></param>
        /// <returns></returns>
        public static float FloorFixed(float value, int dot)
        {
            dot = (int)RangeLimit(dot, 0, 16);
            float num = Mathf.Pow(10, dot);
            return (int)(value * num) / num;
        }

        /// <summary>
        /// 角度转弧度
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public static float DegToRad(float deg)
        {
            return deg * Mathf.Deg2Rad;
        }

        /// <summary>
        /// 弧度转角度
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public static float RadToDeg(float rad)
        {
            return rad * Mathf.Rad2Deg;
        }

        /// <summary>
        /// 规范化角度
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public static float NormalizeDeg(float deg)
        {
            if (deg >= 0 && deg < 360)
                return deg;

            deg %= 360;
            if (deg < 0)
                deg += 360;

            return deg;
        }

        /// <summary>
        /// 获得弧度
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static float GetRad(float x, float y)
        {
            return Mathf.Atan2(y, x);
        }

        /// <summary>
        /// 获得弧度
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static float GetRad(float x1, float y1, float x2, float y2)
        {
            return GetRad(x2 - x1, y2 - y1);
        }

        /// <summary>
        /// 获得弧度
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float GetRad(Vector2 v1, Vector2 v2)
        {
            return GetRad(v2.x - v1.x, v2.y - v1.y);
        }

        /// <summary>
        /// 获得角度
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static float GetDeg(float x, float y)
        {
            return RadToDeg(GetRad(x, y));
        }

        /// <summary>
        /// 获得角度
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static float GetDeg(float x1, float y1, float x2, float y2)
        {
            return GetDeg(x2 - x1, y2 - y1);
        }

        /// <summary>
        /// 获得角度
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float GetDeg(Vector2 v1, Vector2 v2)
        {
            return GetDeg(v2.x - v1.x, v2.y - v1.y);
        }

        /// <summary>
        /// 获得规范化角度
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static float GetNormalizeDeg(float x, float y)
        {
            return NormalizeDeg(GetDeg(x, y));
        }

        /// <summary>
        /// 获得规范化角度
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static float GetNormalizeDeg(float x1, float y1, float x2, float y2)
        {
            return GetNormalizeDeg(x2 - x1, y2 - y1);
        }

        /// <summary>
        /// 获得规范化角度
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float GetNormalizeDeg(Vector2 v1, Vector2 v2)
        {
            return GetNormalizeDeg(v2.x - v1.x, v2.y - v1.y);
        }

        /// <summary>
        /// 获得距离
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static float GetDistance(float x, float y)
        {
            return Mathf.Sqrt(x * x + y * y);
        }

        /// <summary>
        /// 获得距离
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static float GetDistance(float x1, float y1, float x2, float y2)
        {
            return GetDistance(x2 - x1, y2 - y1);
        }

        /// <summary>
        /// 获得距离
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float GetDistance(Vector2 v1, Vector2 v2)
        {
            return GetDistance(v2.x - v1.x, v2.y - v1.y);
        }

        /// <summary>
        /// 获得平方
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static float GetSquare(float x, float y)
        {
            return x * x + y * y;
        }

        /// <summary>
        /// 获得平方
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static float GetSquare(float x1, float y1, float x2, float y2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;
            return dx * dx + dy * dy;
        }

        /// <summary>
        /// 获得平方
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float GetSquare(Vector2 v1, Vector2 v2)
        {
            return GetSquare(v2.x - v1.x, v2.y - v1.y);
        }

        /// <summary>
        /// 根据长度获取直角边长
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Vector2 GetRightSide(float x1, float y1, float x2, float y2, float length)
        {
            float rad = GetRad(x1, y1, x2, y2);
            float xx = length * Mathf.Cos(rad);
            float yy = length * Mathf.Sin(rad);
            return new Vector2(xx, yy);
        }

        /// <summary>
        /// 根据长度获取直角边长
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Vector2 GetRightSide(Vector2 v1, Vector2 v2, float length)
        {
            return GetRightSide(v1.x, v1.y, v2.x, v2.y, length);
        }

        /// <summary>
        /// 线性插值获取线段(x1, y1) (x2, y2)上距离点(x1, y1)位移为length的点
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Vector2 GetLinePoint(float x1, float y1, float x2, float y2, float length)
        {
            float dist = GetDistance(x1, y1, x2, y2);
            float rate = length / dist;
            return new Vector2(x1 + rate * (x2 - x1), y1 + rate * (y2 - y1));
        }

        /// <summary>
        /// 线性插值获取线段(x1, y1) (x2, y2)上距离点(x1, y1)位移为length的点
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Vector2 GetLinePoint(Vector2 v1, Vector2 v2, float length)
        {
            return GetLinePoint(v1.x, v1.y, v2.x, v2.y, length);
        }

        /// <summary>
        /// 根据弧度获取距离点(x, y)length长度的点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="length"></param>
        /// <param name="rad"></param>
        /// <returns></returns>
        public static Vector2 GetLinePoint2(float x, float y, float length, float rad)
        {
            return new Vector2(x + length * Mathf.Cos(rad), y + length * Mathf.Sin(rad));
        }

        /// <summary>
        /// 根据弧度获取距离点(x, y)length长度的点
        /// </summary>
        /// <param name="v"></param>
        /// <param name="length"></param>
        /// <param name="rad"></param>
        /// <returns></returns>
        public static Vector2 GetLinePoint2(Vector2 v, float length, float rad)
        {
            return GetLinePoint2(v.x, v.y, length, rad);
        }

        /// <summary>
        /// 单位换算
        /// </summary>
        /// <param name="num"></param>
        /// <param name="unit"></param>
        /// <param name="unitChar"></param>
        /// <returns></returns>
        public static string ConvertUnits(float num, float unit, string unitChar)
        {
            return (int)(num / unit) + unitChar;
        }

        /// <summary>
        /// 货币格式转换
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string CurrencyFormat(double num)
        {
            int numInt = (int)num;
            double dec = num - numInt;

            // 整数部分
            string numStr = numInt.ToString();
            string str = "";
            int length = numStr.Length;
            while (length > 0)
            {
                int c = length >= 3 ? 3 : length;
                str = numStr.Substring(length - c, c) + str;
                length -= c;
                if (length != 0)
                {
                    str = "," + str;
                }
            }

            // 小数部分
            if (dec > 0)
            {
                string decStr = dec.ToString();
                int index = decStr.IndexOf(".");
                str += decStr.Substring(index, decStr.Length - index);
            }
            return str;
        }

        /// <summary>
        /// 货币格式转换（使用系统函数）
        /// </summary>
        /// <param name="num"></param>
        /// <param name="dec"></param>
        /// <returns></returns>
        public static string CurrencyFormat2(double num, int dec = 0)
        {
            return string.Format("{0:N" + dec + "}", num);
        }
    }
}
