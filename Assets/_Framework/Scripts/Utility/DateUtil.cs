using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public static class DateUtil
    {
        public static readonly DateTime BASE_TIME = new DateTime(1970, 1, 1);

        public static string DATE_YEAR = "年";
        public static string DATE_MONTH = "月";
        public static string DATE_DAY = "日";
        public static string DATE_HOUR = "时";
        public static string DATE_MINUTE = "分";
        public static string DATE_SECOND = "秒";
        public static string DATE_AM = "上午";
        public static string DATE_PM = "下午";

        public static string TIME_YEAR = "年";
        public static string TIME_MONTH = "月";
        public static string TIME_DAY = "天";
        public static string TIME_HOUR = "小时";
        public static string TIME_MINUTE = "分钟";
        public static string TIME_SECOND = "秒";

        /// <summary>
        /// 获取DateTime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(double time)
        {
            return BASE_TIME.AddSeconds(time);
        }

        /// <summary>
        /// 获取当前时区
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentTimeZone()
        {
            return (int)(DateTime.Now - DateTime.UtcNow).TotalHours;
        }

        /// <summary>
        /// 格式化日期输出
        /// </summary>
        /// <param name="time"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string FormatDate(double time, string format = "yyyy-MM-dd HH:mm:ss")
        {
            DateTime dt = BASE_TIME.AddSeconds(time);
            return dt.ToString(format);
        }

        /// <summary>
        /// 格式化中文日期输出
        /// </summary>
        /// <param name="time"></param>
        /// <param name="hourSystem">true为24小时制，false为12小时制</param>
        /// <returns></returns>
        public static string FormatDateInChinese(double time, bool hourSystem = true)
        {
            if (hourSystem)
            {
                return FormatDate(time, "yyyy年MM月dd日 HH:mm:ss");
            }
            else
            {
                DateTime dt = BASE_TIME.AddSeconds(time);
                string dateStr = dt.Year.ToString() + DATE_YEAR + dt.Month.ToString() + DATE_MONTH + dt.Day.ToString() + " ";
                if (dt.Hour <= 12)
                {
                    dateStr += DATE_AM + AddZero(dt.Hour).ToString() + ":" + AddZero(dt.Minute).ToString();
                }
                else
                {
                    dateStr += DATE_PM + AddZero(dt.Hour - 12).ToString() + ":" + AddZero(dt.Minute).ToString();
                }
                return dateStr;
            }
        }

        /// <summary>
        /// 时间加0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string AddZero(int value)
        {
            return value < 10 ? "0" + value : value.ToString();
        }

        /// <summary>
        /// 格式化时间输出
        /// </summary>
        /// <param name="time"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string FormatTime(double time, string separator = ":")
        {
            string hh = AddZero((int)time / 3600);
            time %= 3600;
            string mm = AddZero((int)time / 60);
            time %= 60;
            string ss = AddZero((int)time);
            return hh + separator + mm + separator + ss;
        }

        /// <summary>
        /// 格式化时间输出（中文）
        /// </summary>
        /// <param name="time"></param>
        /// <param name="showDays"></param>
        /// <param name="showHours"></param>
        /// <param name="showMinutes"></param>
        /// <param name="showSeconds"></param>
        /// <returns></returns>
        public static string FormatTimeInChinese(double time, bool showDay = true, bool showHour = true, bool showMinute = true, bool showSecond = true)
        {
            string str = "";
            if (showDay)
            {
                int dd = (int)time / 86400;
                if (dd > 0)
                {
                    str += dd + TIME_DAY;
                    time %= 86400;
                }
            }

            if (showHour)
            {
                int hh = (int)time / 3600;
                if (hh > 0)
                {
                    str += hh + TIME_HOUR;
                    time %= 3600;
                }
            }

            if (showMinute)
            {
                int mm = (int)time / 60;
                if (mm > 0)
                {
                    str += mm + TIME_MINUTE;
                    time %= 60;
                }
            }

            if (showSecond)
            {
                int ss = (int)time;
                if (ss > 0)
                {
                    str += ss + TIME_SECOND;
                }
            }
            return str;
        }

        /// <summary>
        /// 获取指定时间0点时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static double GetZeroTime(double time)
        {
            DateTime dtNow = BASE_TIME.AddSeconds(time);
            DateTime dtZero = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day);
            return (dtZero - BASE_TIME).TotalSeconds;
        }

        /// <summary>
        /// 获取指定时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static double GetDayTime(double time, int hour = 0, int minute = 0, int second = 0)
        {
            DateTime dtNow = BASE_TIME.AddSeconds(time);
            DateTime dtDay = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, hour, minute, second);
            return (dtDay - BASE_TIME).TotalSeconds;
        }

        /// <summary>
        /// 获得当前时间戳（秒）
        /// </summary>
        /// <returns></returns>
        public static long GetNowEpoch()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        /// <summary>
        /// 获得当前时间戳(毫秒)
        /// </summary>
        /// <returns></returns>
        public static long GetNowEpochMillisecond()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        }

        /// <summary>
        /// 获取当前星期数
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetDayWeek(double time)
        {
            DateTime dt = BASE_TIME.AddMilliseconds(time);
            int week = (int)dt.DayOfWeek;
            return week;
        }
    }
}
