using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class ActionDirectionUtil
    {
        public static readonly ActionDirection[] ALL_DIRECTIONS = new ActionDirection[]
        {
            ActionDirection.DOWN,
            ActionDirection.LEFT_DOWN,
            ActionDirection.LEFT,
            ActionDirection.LEFT_UP,
            ActionDirection.UP,
            ActionDirection.RIGHT_UP,
            ActionDirection.RIGHT,
            ActionDirection.RIGHT_DOWN
        };

        public static float SEGMENT_45 = 45f;
        public static float SEGMENT_135 = 135f;
        public static float SEGMENT_225 = 225f;
        public static float SEGMENT_315 = 315f;

        public static float SEGMENT_22_5 = 22.5f;
        public static float SEGMENT_67_5 = 67.5f;
        public static float SEGMENT_112_5 = 112.5f;
        public static float SEGMENT_157_5 = 157.5f;
        public static float SEGMENT_202_5 = 202.5f;
        public static float SEGMENT_247_5 = 247.5f;
        public static float SEGMENT_292_5 = 292.5f;
        public static float SEGMENT_337_5 = 337.5f;

        public static float SEGMENT_28 = 28f;
        public static float SEGMENT_73 = 73f;
        public static float SEGMENT_107 = 107f;
        public static float SEGMENT_152 = 152f;
        public static float SEGMENT_208 = 208f;
        public static float SEGMENT_253 = 253f;
        public static float SEGMENT_287 = 287f;
        public static float SEGMENT_332 = 332f;

        /// <summary>
        /// 获取相反方向
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static ActionDirection GetOppositeDirection(ActionDirection direction)
        {
            ActionDirection oppositeDirection = direction + 4;
            return oppositeDirection;
        }

        /// <summary>
        /// 获取镜像方向
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static ActionDirection GetCopyDirection(ActionDirection direction)
        {
            if (direction <= ActionDirection.DOWN)
                return direction;

            switch (direction)
            {
                case ActionDirection.LEFT_UP:
                    return ActionDirection.RIGHT_UP;
                case ActionDirection.LEFT:
                    return ActionDirection.RIGHT;
                case ActionDirection.LEFT_DOWN:
                    return ActionDirection.RIGHT_DOWN;
            }
            return direction;
        }

        /// <summary>
        /// 获取前一方向
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static ActionDirection GetPreviousDirection(ActionDirection direction)
        {
            ActionDirection previousDirection = direction - 1;
            if (previousDirection < ActionDirection.UP)
                previousDirection = ActionDirection.LEFT_UP;
            return previousDirection;
        }

        /// <summary>
        /// 获取后一方向
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static ActionDirection GetNextDirection(ActionDirection direction)
        {
            ActionDirection nextDirection = direction + 1;
            if (nextDirection > ActionDirection.LEFT_UP)
                nextDirection = ActionDirection.UP;
            return nextDirection;
        }

        /// <summary>
        /// 获取2方向朝向
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        public static ActionDirection GetDirection2(float x1, float x2)
        {
            return x2 >= x1 ? ActionDirection.RIGHT : ActionDirection.LEFT;
        }

        /// <summary>
        /// 获取2方向朝向
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static ActionDirection GetDirection2(Vector3 v1, Vector3 v2)
        {
            return GetDirection2(v1.x, v2.x);
        }

        /// <summary>
        /// 获取4方向朝向
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public static ActionDirection GetDirection4(float deg)
        {
            deg = MathUtil.NormalizeDeg(deg);
            if (deg <= SEGMENT_45 || deg >= SEGMENT_315)
                return ActionDirection.RIGHT;
            else if (deg >= SEGMENT_45 && deg <= SEGMENT_135)
                return ActionDirection.DOWN;
            else if (deg >= SEGMENT_135 && deg <= SEGMENT_225)
                return ActionDirection.LEFT;
            else if (deg >= SEGMENT_247_5 && deg <= SEGMENT_292_5)
                return ActionDirection.UP;
            else if (deg >= SEGMENT_225 && deg <= SEGMENT_315)
                return ActionDirection.RIGHT_UP;
            return ActionDirection.DOWN;
        }

        /// <summary>
        /// 获取4方向朝向
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static ActionDirection GetDirection4(float x1, float y1, float x2, float y2)
        {
            if (x1 == x2)
            {
                if (y2 >= y1)
                    return ActionDirection.DOWN;
                return ActionDirection.UP;
            }
            else if (y1 == y2)
            {
                if (x2 >= x1)
                    return ActionDirection.RIGHT;
                return ActionDirection.LEFT;
            }
            else
            {
                float deg = MathUtil.GetDeg(x2 - x1, y2 - y1);
                return GetDirection4(deg);
            }
        }

        /// <summary>
        /// 获取4方向朝向
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static ActionDirection GetDirection4(Vector3 v1, Vector3 v2)
        {
            return GetDirection4(v1.x, v1.y, v2.x, v2.y);
        }

        /// <summary>
        /// 获取8方向朝向
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public static ActionDirection GetDirection8(float deg)
        {
            deg = MathUtil.NormalizeDeg(deg);
            if (deg <= SEGMENT_22_5 || deg >= SEGMENT_337_5)
                return ActionDirection.RIGHT;
            else if (deg >= SEGMENT_22_5 && deg <= SEGMENT_67_5)
                return ActionDirection.RIGHT_DOWN;
            else if (deg >= SEGMENT_67_5 && deg <= SEGMENT_112_5)
                return ActionDirection.DOWN;
            else if (deg >= SEGMENT_112_5 && deg <= SEGMENT_157_5)
                return ActionDirection.LEFT_DOWN;
            else if (deg >= SEGMENT_157_5 && deg <= SEGMENT_202_5)
                return ActionDirection.LEFT;
            else if (deg >= SEGMENT_202_5 && deg <= SEGMENT_247_5)
                return ActionDirection.LEFT_UP;
            else if (deg >= SEGMENT_247_5 && deg <= SEGMENT_292_5)
                return ActionDirection.UP;
            else if (deg >= SEGMENT_292_5 && deg <= SEGMENT_337_5)
                return ActionDirection.RIGHT_UP;
            return ActionDirection.DOWN;
        }

        /**
		 * 获取8方向朝向
		 * @param x1
		 * @param y1
		 * @param x2
		 * @param y2
		 * @return 
		 * 
		 */
        public static ActionDirection GetDirection8(float x1, float y1, float x2, float y2)
        {
            if (x1 == x2)
            {
                if (y2 >= y1)
                    return ActionDirection.DOWN;
                return ActionDirection.UP;
            }
            else if (y1 == y2)
            {
                if (x2 >= x1)
                    return ActionDirection.RIGHT;
                return ActionDirection.LEFT;
            }
            else
            {
                float deg = MathUtil.GetDeg(x2 - x1, y2 - y1);
                return GetDirection8(deg);
            }
        }

        /// <summary>
        /// 获取8方向朝向
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static ActionDirection GetDirection8(Vector2 v1, Vector2 v2)
        {
            return GetDirection8(v1.x, v1.y, v2.x, v2.y);
        }

        /// <summary>
        /// 获取8方向朝向
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static ActionDirection GetDirection8(Vector3 v1, Vector3 v2)
        {
            return GetDirection8(v1.x, v1.y, v2.x, v2.y);
        }

        /**
		 * 获取8方向朝向
		 * @param x1
		 * @param y1
		 * @param x2
		 * @param y2
		 * @return 
		 * 
		 */
        public static ActionDirection GetDirection8RealPos(float x1, float y1, float x2, float y2)
        {
            if (x1 == x2)
            {
                if (y2 >= y1)
                    return ActionDirection.DOWN;
                return ActionDirection.UP;
            }
            else if (y1 == y2)
            {
                if (x2 >= x1)
                    return ActionDirection.RIGHT;
                return ActionDirection.LEFT;
            }
            else
            {
                float deg = MathUtil.GetDeg(x2 - x1, y2 - y1);
                if (deg <= SEGMENT_28 || deg >= SEGMENT_332)
                    return ActionDirection.RIGHT;
                else if (deg >= SEGMENT_28 && deg <= SEGMENT_73)
                    return ActionDirection.RIGHT_DOWN;
                else if (deg >= SEGMENT_73 && deg <= SEGMENT_107)
                    return ActionDirection.DOWN;
                else if (deg >= SEGMENT_107 && deg <= SEGMENT_152)
                    return ActionDirection.LEFT_DOWN;
                else if (deg >= SEGMENT_152 && deg <= SEGMENT_208)
                    return ActionDirection.LEFT;
                else if (deg >= SEGMENT_208 && deg <= SEGMENT_253)
                    return ActionDirection.LEFT_UP;
                else if (deg >= SEGMENT_253 && deg <= SEGMENT_287)
                    return ActionDirection.UP;
                else if (deg >= SEGMENT_287 && deg <= SEGMENT_332)
                    return ActionDirection.RIGHT_UP;
                return ActionDirection.DOWN;
            }
        }

        /// <summary>
        /// 获取8方向朝向
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static ActionDirection GetDirection8RealPos(Vector2 v1, Vector2 v2)
        {
            return GetDirection8RealPos(v1.x, v1.y, v2.x, v2.y);
        }

        /// <summary>
        /// 获取8方向朝向
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static ActionDirection GetDirection8RealPos(Vector3 v1, Vector3 v2)
        {
            return GetDirection8RealPos(v1.x, v1.y, v2.x, v2.y);
        }

        /// <summary>
        /// 根据朝向获取角度
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static float GetDegByDirection(ActionDirection direction)
        {
            switch (direction)
            {
                case ActionDirection.RIGHT:
                    return 0f;
                case ActionDirection.RIGHT_DOWN:
                    return 45f;
                case ActionDirection.DOWN:
                    return 90f;
                case ActionDirection.LEFT_DOWN:
                    return 135f;
                case ActionDirection.LEFT:
                    return 180f;
                case ActionDirection.LEFT_UP:
                    return 225f;
                case ActionDirection.UP:
                    return 270f;
                case ActionDirection.RIGHT_UP:
                    return 315f;
            }
            return 0f;
        }

        /// <summary>
        /// 根据坐标获取偏移系数(-1, 0, 1)
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="z1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="z2"></param>
        /// <returns></returns>
        public static Vector3 GetVectorByPosition(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            Vector3 vector = new Vector3();
            float dx = x2 - x1;
            float dy = y2 - y1;
            float dz = z2 - z1;

            if (dx > 0)
                vector.x = 1;
            else if (dx < 0)
                vector.x = -1;
            else
                vector.x = 0;

            if (dy > 0)
                vector.y = 1;
            else if (dy < 0)
                vector.y = -1;
            else
                vector.y = 0;

            if (dz > 0)
                vector.z = 1;
            else if (dz < 0)
                vector.z = -1;
            else
                vector.z = 0;

            return vector;
        }

        /// <summary>
        /// 根据坐标获取偏移系数(-1, 0, 1)
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3 GetVectorByPosition(Vector3 v1, Vector3 v2)
        {
            return GetVectorByPosition(v1.x, v1.y, v1.z, v2.x, v2.y, v2.z);
        }

        /// <summary>
        /// 根据方向获取偏移系数(-1, 0, 1)
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector3 GetVectorByDirection(ActionDirection direction)
        {
            Vector3 vector = new Vector3();
            switch (direction)
            {
                case ActionDirection.RIGHT:
                    vector.x = 1;
                    break;
                case ActionDirection.RIGHT_DOWN:
                    vector.x = 1;
                    vector.y = 1;
                    break;
                case ActionDirection.DOWN:
                    vector.y = 1;
                    break;
                case ActionDirection.LEFT_DOWN:
                    vector.x = -1;
                    vector.y = 1;
                    break;
                case ActionDirection.LEFT:
                    vector.x = -1;
                    break;
                case ActionDirection.LEFT_UP:
                    vector.x = -1;
                    vector.y = -1;
                    break;
                case ActionDirection.UP:
                    vector.y = -1;
                    break;
                case ActionDirection.RIGHT_UP:
                    vector.x = 1;
                    vector.y = -1;
                    break;
            }
            return vector;
        }
    }
}
