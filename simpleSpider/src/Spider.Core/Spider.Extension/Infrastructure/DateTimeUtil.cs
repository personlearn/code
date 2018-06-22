﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Extension.Infrastructure
{
    /// <summary>
	/// 时间的帮助类
	/// </summary>
	public static class DateTimeUtil
    {
        private static readonly DateTimeOffset Epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
        private const long TicksPerMicrosecond = 10;

        /// <summary>
        /// 当天的RunId: 2017-12-20
        /// </summary>
        public static string RunIdOfToday { get; }

        /// <summary>
        /// 当月的RunId: 2017-12-01
        /// </summary>
        public static string RunIdOfMonthly { get; }

        /// <summary>
        /// 当周的RunId: 2018-01-01 (it's monday)
        /// </summary>
        public static string RunIdOfMonday { get; }

        /// <summary>
        /// 当前月份的第一天
        /// </summary>
        public static DateTime FirstDayOfTheMonth { get; }
        /// <summary>
        /// 当前月份的第一天
        /// </summary>
        public static DateTime LastDayOfTheMonth { get; }
        /// <summary>
        /// 当前月份的最后一天
        /// </summary>
        public static DateTime FirstDayOfLastMonth { get; }
        /// <summary>
        /// 前一月份的最后一天
        /// </summary>
        public static DateTime LastDayOfLastMonth { get; }
        /// <summary>
        /// 星期一
        /// </summary>
        public static DateTime Today { get; }
        /// <summary>
        /// 星期一
        /// </summary>
        public static DateTime Monday { get; }
        /// <summary>
        /// 星期二
        /// </summary>
        public static DateTime Tuesday { get; }
        /// <summary>
        /// 星期三
        /// </summary>
        public static DateTime Wednesday { get; }
        /// <summary>
        /// 星期四
        /// </summary>
        public static DateTime Thursday { get; }
        /// <summary>
        /// 星期五
        /// </summary>
        public static DateTime Friday { get; }
        /// <summary>
        /// 星期六
        /// </summary>
        public static DateTime Saturday { get; }
        /// <summary>
        /// 星期天
        /// </summary>
        public static DateTime Sunday { get; }
        /// <summary>
        /// 上周的星期一
        /// </summary>
        public static DateTime LastMonday { get; }
        /// <summary>
        /// 上周的星期二
        /// </summary>
        public static DateTime LastTuesday { get; }
        /// <summary>
        /// 上周的星期三
        /// </summary>
        public static DateTime LastWednesday { get; }
        /// <summary>
        /// 上周的星期四
        /// </summary>
        public static DateTime LastThursday { get; }
        /// <summary>
        /// 上周的星期五
        /// </summary>
        public static DateTime LastFriday { get; }
        /// <summary>
        /// 上周的星期六
        /// </summary>
        public static DateTime LastSaturday { get; }
        /// <summary>
        /// 上周的星期天
        /// </summary>
        public static DateTime LastSunday { get; }
        /// <summary>
        /// 下周的星期一
        /// </summary>
        public static DateTime NextMonday { get; }
        /// <summary>
        /// 下周的星期二
        /// </summary>
        public static DateTime NextTuesday { get; }
        /// <summary>
        /// 下周的星期三
        /// </summary>
        public static DateTime NextWednesday { get; }
        /// <summary>
        /// 下周的星期四
        /// </summary>
        public static DateTime NextThursday { get; }
        /// <summary>
        /// 下周的星期五
        /// </summary>
        public static DateTime NextFriday { get; }
        /// <summary>
        /// 下周的星期六
        /// </summary>
        public static DateTime NextSaturday { get; }
        /// <summary>
        /// 下周的星期天
        /// </summary>
        public static DateTime NextSunday { get; }

        /// <summary>
        /// 把时间转换成Unix时间: 1515133023012
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>Unix时间</returns>
        public static string ConvertDateTimeToUnix(DateTime time)
        {
            return time.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds.ToString("f0");
        }

        /// <summary>
        /// 把Unix时间转换成DateTime
        /// </summary>
        /// <param name="unixTime">Unix时间</param>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTimeOffset(long unixTime)
        {
            return Epoch.AddTicks(unixTime * TicksPerMicrosecond).DateTime;
        }

        /// <summary>
        /// 获取当前Unix时间
        /// </summary>
        /// <returns>Unix时间</returns>
        public static string GetCurrentUnixTimeString()
        {
            return ConvertDateTimeToUnix(DateTime.UtcNow);
        }

        /// <summary>
        /// 获取当前Unix时间
        /// </summary>
        /// <returns>Unix时间</returns>
        public static double GetCurrentUnixTimeNumber()
        {
            return DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        static DateTimeUtil()
        {
            var now = DateTime.Now.Date;
            Today = now;

            FirstDayOfTheMonth = now.AddDays(now.Day * -1 + 1);
            LastDayOfTheMonth = FirstDayOfTheMonth.AddMonths(1).AddDays(-1);
            FirstDayOfLastMonth = FirstDayOfTheMonth.AddMonths(-1);
            LastDayOfLastMonth = FirstDayOfTheMonth.AddDays(-1);

            int i = now.DayOfWeek - DayOfWeek.Monday == -1 ? 6 : -1;
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);

            Monday = now.Subtract(ts).Date;
            Tuesday = Monday.AddDays(1);
            Wednesday = Monday.AddDays(2);
            Thursday = Monday.AddDays(3);
            Friday = Monday.AddDays(4);
            Saturday = Monday.AddDays(5);
            Sunday = Monday.AddDays(6);

            LastMonday = Monday.AddDays(-7);
            LastTuesday = LastMonday.AddDays(1);
            LastWednesday = LastMonday.AddDays(2);
            LastThursday = LastMonday.AddDays(3);
            LastFriday = LastMonday.AddDays(4);
            LastSaturday = LastMonday.AddDays(5);
            LastSunday = LastMonday.AddDays(6);

            NextMonday = Sunday.AddDays(1);
            NextTuesday = Monday.AddDays(1);
            NextWednesday = Monday.AddDays(2);
            NextThursday = Monday.AddDays(3);
            NextFriday = Monday.AddDays(4);
            NextSaturday = Monday.AddDays(5);
            NextSunday = Monday.AddDays(6);

            RunIdOfToday = now.ToString("yyyy-MM-dd");
            RunIdOfMonthly = FirstDayOfTheMonth.ToString("yyyy-MM-dd");
            RunIdOfMonday = Monday.ToString("yyyy-MM-dd");
        }
    }
}
