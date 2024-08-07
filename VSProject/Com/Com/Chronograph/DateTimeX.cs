﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.DateTimeX
Version 24.7.21.1040

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com
{
    /// <summary>
    /// 以日期与当天的时间表示一个时刻。
    /// </summary>
    public struct DateTimeX : IEquatable<DateTimeX>, IComparable, IComparable<DateTimeX>
    {
        #region 非公开成员

        private const double _MinUtcOffset = -24, _MaxUtcOffset = 24, _Utc = 0; // 所在时区的标准时间与协调世界时（UTC）之间的时差的小时数的最小值、最大值与表示与协调世界时（UTC）之间为零时差的值。

        private static double _LocalUtcOffset => TimeZoneInfo.Local.BaseUtcOffset.TotalHours; // 本地时区的标准时间与协调世界时（UTC）之间的时差的小时数。

        private const decimal _MinTotalMilliseconds = -796899343984252546694400000M, _MaxTotalMilliseconds = 796899343984252578143999999M, _ChristianTotalMilliseconds = 0M; // 自公元时刻以来的总毫秒数的最小值、最大值与公元时刻的值，最小值等于由 System.Int64.MinValue 表示的自公元时刻以来的总天数所在年份加 2 所得年份的元旦午夜对应的毫秒数，最大值等于由 System.Int64.MaxValue 表示的自公元时刻以来的总天数所在年份减 1 所得年份的元旦午夜对应的毫秒数减 1。

        private const long _MinYear = -25252756133808173, _MaxYear = 25252756133808174, _ChristianYear = 1; // 年的最小值、最大值与公元时刻的值，最小值等于由 System.Int64.MinValue 表示的自公元时刻以来的总天数所在年份加 2，最大值等于由 System.Int64.MaxValue 表示的自公元时刻以来的总天数所在年份减 2。
        private const int _MinMonth = 1, _MaxMonth = 12; // 月的最小值与最大值。
        private const int _MinDay = 1, _MaxDay = 31; // 日的最小值与最大值。
        private const int _MinHour = 0, _MaxHour = 23; // 时的最小值与最大值。
        private const int _MinMinute = 0, _MaxMinute = 59; // 分的最小值与最大值。
        private const int _MinSecond = 0, _MaxSecond = 59; // 秒的最小值与最大值。
        private const int _MinMillisecond = 0, _MaxMillisecond = 999; // 毫秒的最小值与最大值。

        private const int _DaysPerYear = 365; // 每个平年的天数。
        private const int _DaysPer4Years = 1461; // 每 4 年的天数。
        private const int _DaysPer100Years = 36524; // 每 100 年的天数。
        private const int _DaysPer400Years = 146097; // 每 400 年的天数。
        private const int _DaysPer3200Years = 1168775; // 每 3200 年的天数。
        private const int _DaysPer172800Years = 63113851; // 每 172800 年的天数。

        private const int _MillisecondsPerSecond = 1000; // 每秒的毫秒数。
        private const int _MillisecondsPerMinute = 60000; // 每分钟的毫秒数。
        private const int _MillisecondsPerHour = 3600000; // 每小时的毫秒数。
        private const int _MillisecondsPerDay = 86400000; // 每天的毫秒数。
        private const int _MillisecondsPerWeek = 604800000; // 每周的毫秒数。
        private const int _SecondsPerMinute = 60; // 每分钟的秒数。
        private const int _SecondsPerHour = 3600; // 每小时的秒数。
        private const int _SecondsPerDay = 86400; // 每天的秒数。
        private const int _MinutesPerHour = 60; // 每小时的分钟数。
        private const int _MinutesPerDay = 1440; // 每天的分钟数。
        private const int _HoursPerDay = 24; // 每天的小时数。
        private const int _MonthsPerYear = 12; // 每年的月数。

        private static readonly int[] _DaysToMonth365 = new int[] { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 }; // 平年中前若干个月的总天数。
        private static readonly int[] _DaysToMonth366 = new int[] { 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 }; // 闰年中前若干个月的总天数。

        //

        // 对所在时区的标准时间与协调世界时（UTC）之间的时差的小时数的值进行合法性检查，返回合法的值。
        private static double _CheckUtcOffset(double utcOffset)
        {
            if (InternalMethod.IsNaNOrInfinity(utcOffset))
            {
                throw new ArgumentOutOfRangeException(nameof(utcOffset));
            }

            //

            if (utcOffset < _MinUtcOffset)
            {
                if (utcOffset <= _MinUtcOffset - 5E-12)
                {
                    throw new ArgumentOutOfRangeException(nameof(utcOffset));
                }

                return _MinUtcOffset;
            }
            else if (utcOffset > _MaxUtcOffset)
            {
                if (utcOffset >= _MaxUtcOffset + 5E-12)
                {
                    throw new ArgumentOutOfRangeException(nameof(utcOffset));
                }

                return _MaxUtcOffset;
            }
            else
            {
                return utcOffset;
            }
        }

        // 对自公元时刻以来的总毫秒数的值进行合法性检查，返回合法的值。
        private static decimal _CheckTotalMilliseconds(decimal totalMilliseconds)
        {
            decimal _totalMilliseconds = Math.Round(totalMilliseconds);

            if (_totalMilliseconds < _MinTotalMilliseconds || _totalMilliseconds > _MaxTotalMilliseconds)
            {
                throw new ArgumentOutOfRangeException(nameof(totalMilliseconds));
            }

            //

            return _totalMilliseconds;
        }

        // 对年的值进行合法性检查，返回合法的值。
        private static long _CheckYear(long year, double utcOffset)
        {
            if (year == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(year));
            }
            else
            {
                if (year < (utcOffset < 0 ? _MinYear - 1 : _MinYear))
                {
                    throw new ArgumentOutOfRangeException(nameof(year));
                }
                else if (year > (utcOffset > 0 ? _MaxYear + 1 : _MaxYear))
                {
                    throw new ArgumentOutOfRangeException(nameof(year));
                }
                else
                {
                    return year;
                }
            }
        }

        // 对月的值进行合法性检查，返回合法的值。
        private static int _CheckMonth(int month)
        {
            if (month < _MinMonth || month > _MaxMonth)
            {
                throw new ArgumentOutOfRangeException(nameof(month));
            }

            //

            return month;
        }

        // 对日的值进行合法性检查，返回合法的值。
        private static int _CheckDay(long year, int month, int day)
        {
            if (day < _MinDay)
            {
                throw new ArgumentOutOfRangeException(nameof(day));
            }
            else if (day > DaysInMonth(year, month))
            {
                throw new ArgumentOutOfRangeException(nameof(day));
            }
            else
            {
                return day;
            }
        }

        // 对时的值进行合法性检查，返回合法的值。
        private static int _CheckHour(int hour)
        {
            if (hour < _MinHour || hour > _MaxHour)
            {
                throw new ArgumentOutOfRangeException(nameof(hour));
            }

            //

            return hour;
        }

        // 对分的值进行合法性检查，返回合法的值。
        private static int _CheckMinute(int minute)
        {
            if (minute < _MinMinute || minute > _MaxMinute)
            {
                throw new ArgumentOutOfRangeException(nameof(minute));
            }

            //

            return minute;
        }

        // 对秒的值进行合法性检查，返回合法的值。
        private static int _CheckSecond(int second)
        {
            if (second < _MinSecond || second > _MaxSecond)
            {
                throw new ArgumentOutOfRangeException(nameof(second));
            }

            //

            return second;
        }

        // 对毫秒的值进行合法性检查，返回合法的值。
        private static int _CheckMillisecond(int millisecond)
        {
            if (millisecond < _MinMillisecond || millisecond > _MaxMillisecond)
            {
                throw new ArgumentOutOfRangeException(nameof(millisecond));
            }

            //

            return millisecond;
        }

        //

        // 将自公元时刻以来的总毫秒数转换为年、月、日、时、分、秒与毫秒。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        private static void _TotalMillisecondsToDateTime(decimal totalMilliseconds, out long year, out int month, out int day, out int hour, out int minute, out int second, out int millisecond)
        {
            long TotalDays = (long)Math.Floor(totalMilliseconds / _MillisecondsPerDay);

            long Year = 1;
            int DayOfYear = 1;

            bool Leap = false;

            if (totalMilliseconds >= 0)
            {
                long TD = TotalDays;

                Year = 1;

                Year += TD / _DaysPer172800Years * 172800;
                TD %= _DaysPer172800Years;

                if (TD == _DaysPer172800Years - 1)
                {
                    Year += 172799;
                    TD = _DaysPerYear;
                }
                else
                {
                    Year += TD / _DaysPer3200Years * 3200;
                    TD %= _DaysPer3200Years;

                    Year += TD / _DaysPer400Years * 400;
                    TD %= _DaysPer400Years;

                    if (TD == _DaysPer400Years - 1)
                    {
                        Year += 399;
                        TD = _DaysPerYear;
                    }
                    else
                    {
                        Year += TD / _DaysPer100Years * 100;
                        TD %= _DaysPer100Years;

                        Year += TD / _DaysPer4Years * 4;
                        TD %= _DaysPer4Years;

                        if (TD == _DaysPer4Years - 1)
                        {
                            Year += 3;
                            TD = _DaysPerYear;
                        }
                        else
                        {
                            Year += TD / _DaysPerYear;
                            TD %= _DaysPerYear;
                        }
                    }
                }

                Leap = IsLeapYear(Year);

                DayOfYear = 1 + (int)TD;
            }
            else
            {
                long TD = -TotalDays;

                Year = -1;

                if (TD > 366)
                {
                    Year -= 1;
                    TD -= 367;

                    Year -= TD / _DaysPer172800Years * 172800;
                    TD %= _DaysPer172800Years;

                    if (TD == _DaysPer172800Years - 1)
                    {
                        Year -= 172799;
                        TD = _DaysPerYear;
                    }
                    else
                    {
                        Year -= TD / _DaysPer3200Years * 3200;
                        TD %= _DaysPer3200Years;

                        Year -= TD / _DaysPer400Years * 400;
                        TD %= _DaysPer400Years;

                        if (TD == _DaysPer400Years - 1)
                        {
                            Year -= 399;
                            TD = _DaysPerYear;
                        }
                        else
                        {
                            Year -= TD / _DaysPer100Years * 100;
                            TD %= _DaysPer100Years;

                            Year -= TD / _DaysPer4Years * 4;
                            TD %= _DaysPer4Years;

                            if (TD == _DaysPer4Years - 1)
                            {
                                Year -= 3;
                                TD = _DaysPerYear;
                            }
                            else
                            {
                                Year -= TD / _DaysPerYear;
                                TD %= _DaysPerYear;
                            }
                        }
                    }

                    TD += 1;
                }

                Leap = IsLeapYear(Year);

                DayOfYear = 1 + ((Leap ? 366 : 365) - (int)TD);
            }

            year = Year;

            int[] DaysToMonth = Leap ? _DaysToMonth366 : _DaysToMonth365;

            int Month = 1;

            while (DayOfYear > DaysToMonth[Month])
            {
                Month++;
            }

            month = Month;
            day = DayOfYear - DaysToMonth[Month - 1];

            int MS = (int)(totalMilliseconds - (decimal)TotalDays * _MillisecondsPerDay);
            hour = MS / _MillisecondsPerHour % _HoursPerDay;
            minute = MS / _MillisecondsPerMinute % _MinutesPerHour;
            second = MS / _MillisecondsPerSecond % _SecondsPerMinute;
            millisecond = MS % _MillisecondsPerSecond;
        }

        // 将年、月、日、时、分、秒与毫秒转换为自公元时刻以来的总毫秒数。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        private static void _DateTimeToTotalMilliseconds(long year, int month, int day, int hour, int minute, int second, int millisecond, out decimal totalMilliseconds)
        {
            long TotalDays = 0;

            int[] DaysToMonth = IsLeapYear(year) ? _DaysToMonth366 : _DaysToMonth365;

            if (year > 0)
            {
                long Yr = year - 1;

                TotalDays = Yr * _DaysPerYear + Yr / 4 - Yr / 100 + Yr / 400 - Yr / 3200 + Yr / 172800 + DaysToMonth[month - 1] + day - 1;
            }
            else
            {
                long Yr = -year - 1;

                TotalDays = -(366 + Yr * _DaysPerYear + Yr / 4 - Yr / 100 + Yr / 400 - Yr / 3200 + Yr / 172800) + DaysToMonth[month - 1] + day - 1;
            }

            totalMilliseconds = (decimal)TotalDays * _MillisecondsPerDay + (decimal)(hour * _MillisecondsPerHour + minute * _MillisecondsPerMinute + second * _MillisecondsPerSecond + millisecond);
        }

        //

        private bool _Initialized; // 表示此 DateTimeX 结构是否已初始化。

        private double _UtcOffset; // 所在时区的标准时间与协调世界时（UTC）之间的时差的小时数。

        private decimal _TotalMilliseconds; // 自公元时刻以来的总毫秒数。

        private long _Year; // 年。
        private int _Month; // 月。
        private int _Day; // 日。
        private int _Hour; // 时。
        private int _Minute; // 分。
        private int _Second; // 秒。
        private int _Millisecond; // 毫秒。

        //

        // 表示此 DateTimeX 结构所在时区时刻的最小可能值的 DateTimeX 结构的实例。
        private DateTimeX _LocalMinValue => new DateTimeX(_MinTotalMilliseconds, _UtcOffset);

        // 表示此 DateTimeX 结构所在时区时刻的最大可能值的 DateTimeX 结构的实例。
        private DateTimeX _LocalMaxValue => new DateTimeX(_MaxTotalMilliseconds, _UtcOffset);

        //

        // 获取此 DateTimeX 结构表示的日期是所在周的第几天（以周一为第一天）。
        private int _DayOfWeek
        {
            get
            {
                long TotalDays = (long)Math.Floor((_TotalMilliseconds + (decimal)_UtcOffset * _MillisecondsPerHour) / _MillisecondsPerDay);

                if (TotalDays >= 0)
                {
                    return 1 + (int)(TotalDays % 7);
                }
                else
                {
                    return 7 - (int)((-TotalDays - 1) % 7);
                }
            }
        }

        //

        // 为以自公元时刻以来的总毫秒数为参数的构造函数提供实现。
        private void _CtorTotalMilliseconds(decimal totalMilliseconds, double utcOffset)
        {
            _UtcOffset = _CheckUtcOffset(utcOffset);

            decimal _UtcOffsetMS = (decimal)_UtcOffset * _MillisecondsPerHour;

            _TotalMilliseconds = _CheckTotalMilliseconds(totalMilliseconds);

            _TotalMillisecondsToDateTime(_TotalMilliseconds + _UtcOffsetMS, out _Year, out _Month, out _Day, out _Hour, out _Minute, out _Second, out _Millisecond);

            //

            _Initialized = true;
        }

        // 为以年、月、日、时、分、秒与毫秒为参数的构造函数提供实现。
        private void _CtorDateTime(long year, int month, int day, int hour, int minute, int second, int millisecond, double utcOffset)
        {
            _UtcOffset = _CheckUtcOffset(utcOffset);

            decimal _UtcOffsetMS = (decimal)_UtcOffset * _MillisecondsPerHour;

            _Year = _CheckYear(year, _UtcOffset);
            _Month = _CheckMonth(month);
            _Day = _CheckDay(_Year, _Month, day);
            _Hour = _CheckHour(hour);
            _Minute = _CheckMinute(minute);
            _Second = _CheckSecond(second);
            _Millisecond = _CheckMillisecond(millisecond);

            _DateTimeToTotalMilliseconds(_Year, _Month, _Day, _Hour, _Minute, _Second, _Millisecond, out _TotalMilliseconds);

            _TotalMilliseconds = _CheckTotalMilliseconds(_TotalMilliseconds - _UtcOffsetMS);

            if (_UtcOffset != _Utc && (year <= _MinYear || year >= _MaxYear))
            {
                _TotalMillisecondsToDateTime(_TotalMilliseconds + _UtcOffsetMS, out _Year, out _Month, out _Day, out _Hour, out _Minute, out _Second, out _Millisecond);
            }

            //

            _Initialized = true;
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用十进制浮点数表示的自公元时刻以来的总毫秒与所在时区的标准时间与协调世界时（UTC）之间的时差的小时数初始化 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="totalMilliseconds">十进制浮点数表示的自公元时刻以来的总毫秒数。</param>
        /// <param name="utcOffset">双精度浮点数表示的所在时区的标准时间与协调世界时（UTC）之间的时差的小时数。</param>
        public DateTimeX(decimal totalMilliseconds, double utcOffset) : this() => _CtorTotalMilliseconds(totalMilliseconds, utcOffset);

        /// <summary>
        /// 使用十进制浮点数表示的自公元时刻以来的总毫秒数初始化以本地时区表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="totalMilliseconds">十进制浮点数表示的自公元时刻以来的总毫秒数。</param>
        public DateTimeX(decimal totalMilliseconds) : this() => _CtorTotalMilliseconds(totalMilliseconds, _LocalUtcOffset);

        /// <summary>
        /// 使用指定的年、月、日、时、分、秒、毫秒与所在时区的标准时间与协调世界时（UTC）之间的时差的小时数初始化 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="month">32 位整数表示的月。</param>
        /// <param name="day">32 位整数表示的日。</param>
        /// <param name="hour">32 位整数表示的时。</param>
        /// <param name="minute">32 位整数表示的分。</param>
        /// <param name="second">32 位整数表示的秒。</param>
        /// <param name="millisecond">32 位整数表示的毫秒。</param>
        /// <param name="utcOffset">双精度浮点数表示的所在时区的标准时间与协调世界时（UTC）之间的时差的小时数。</param>
        public DateTimeX(long year, int month, int day, int hour, int minute, int second, int millisecond, double utcOffset) : this() => _CtorDateTime(year, month, day, hour, minute, second, millisecond, utcOffset);

        /// <summary>
        /// 使用指定的年、月、日、时、分、秒与毫秒初始化以本地时区表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="month">32 位整数表示的月。</param>
        /// <param name="day">32 位整数表示的日。</param>
        /// <param name="hour">32 位整数表示的时。</param>
        /// <param name="minute">32 位整数表示的分。</param>
        /// <param name="second">32 位整数表示的秒。</param>
        /// <param name="millisecond">32 位整数表示的毫秒。</param>
        public DateTimeX(long year, int month, int day, int hour, int minute, int second, int millisecond) : this() => _CtorDateTime(year, month, day, hour, minute, second, millisecond, _LocalUtcOffset);

        /// <summary>
        /// 使用指定的年、月、日、时、分、秒与所在时区的标准时间与协调世界时（UTC）之间的时差的小时数初始化 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="month">32 位整数表示的月。</param>
        /// <param name="day">32 位整数表示的日。</param>
        /// <param name="hour">32 位整数表示的时。</param>
        /// <param name="minute">32 位整数表示的分。</param>
        /// <param name="second">32 位整数表示的秒。</param>
        /// <param name="utcOffset">双精度浮点数表示的所在时区的标准时间与协调世界时（UTC）之间的时差的小时数。</param>
        public DateTimeX(long year, int month, int day, int hour, int minute, int second, double utcOffset) : this() => _CtorDateTime(year, month, day, hour, minute, second, _MinMillisecond, utcOffset);

        /// <summary>
        /// 使用指定的年、月、日、时、分与秒初始化以本地时区表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="month">32 位整数表示的月。</param>
        /// <param name="day">32 位整数表示的日。</param>
        /// <param name="hour">32 位整数表示的时。</param>
        /// <param name="minute">32 位整数表示的分。</param>
        /// <param name="second">32 位整数表示的秒。</param>
        public DateTimeX(long year, int month, int day, int hour, int minute, int second) : this() => _CtorDateTime(year, month, day, hour, minute, second, _MinMillisecond, _LocalUtcOffset);

        /// <summary>
        /// 使用指定的年、月、日与所在时区的标准时间与协调世界时（UTC）之间的时差的小时数初始化 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="month">32 位整数表示的月。</param>
        /// <param name="day">32 位整数表示的日。</param>
        /// <param name="utcOffset">双精度浮点数表示的所在时区的标准时间与协调世界时（UTC）之间的时差的小时数。</param>
        public DateTimeX(long year, int month, int day, double utcOffset) : this() => _CtorDateTime(year, month, day, _MinHour, _MinMinute, _MinSecond, _MinMillisecond, utcOffset);

        /// <summary>
        /// 使用指定的年、月与日初始化以本地时区表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="month">32 位整数表示的月。</param>
        /// <param name="day">32 位整数表示的日。</param>
        public DateTimeX(long year, int month, int day) : this() => _CtorDateTime(year, month, day, _MinHour, _MinMinute, _MinSecond, _MinMillisecond, _LocalUtcOffset);

        /// <summary>
        /// 使用 DateTimeX 结构与所在时区的标准时间与协调世界时（UTC）之间的时差的小时数初始化 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="dateTime">DateTimeX 结构。</param>
        /// <param name="utcOffset">双精度浮点数表示的所在时区的标准时间与协调世界时（UTC）之间的时差的小时数。</param>
        public DateTimeX(DateTimeX dateTime, double utcOffset) : this() => _CtorDateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime._Hour, dateTime._Minute, dateTime._Second, dateTime._Millisecond, utcOffset);

        /// <summary>
        /// 使用 DateTimeX 结构初始化以本地时区表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="dateTime">DateTimeX 结构。</param>
        public DateTimeX(DateTimeX dateTime) : this() => _CtorDateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime._Hour, dateTime._Minute, dateTime._Second, dateTime._Millisecond, _LocalUtcOffset);

        /// <summary>
        /// 使用 DateTime 结构与所在时区的标准时间与协调世界时（UTC）之间的时差的小时数初始化 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="dateTime">DateTime 结构。</param>
        /// <param name="utcOffset">双精度浮点数表示的所在时区的标准时间与协调世界时（UTC）之间的时差的小时数。</param>
        public DateTimeX(DateTime dateTime, double utcOffset) : this() => _CtorDateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, utcOffset);

        /// <summary>
        /// 使用 DateTime 结构初始化以本地时区表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="dateTime">DateTime 结构。</param>
        public DateTimeX(DateTime dateTime) : this() => _CtorDateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, _LocalUtcOffset);

        #endregion

        #region 字段

        /// <summary>
        /// 表示所有属性为其数据类型的默认值的 DateTimeX 结构的实例。
        /// </summary>
        public static readonly DateTimeX Empty = new DateTimeX();

        //

        /// <summary>
        /// 表示公元时刻的 DateTimeX 结构的实例。
        /// </summary>
        public static readonly DateTimeX ChristianEra = new DateTimeX(_ChristianTotalMilliseconds, _Utc);

        /// <summary>
        /// 表示时刻的最小可能值的 DateTimeX 结构的实例。
        /// </summary>
        public static readonly DateTimeX MinValue = new DateTimeX(_MinTotalMilliseconds, _Utc);

        /// <summary>
        /// 表示时刻的最大可能值的 DateTimeX 结构的实例。
        /// </summary>
        public static readonly DateTimeX MaxValue = new DateTimeX(_MaxTotalMilliseconds, _Utc);

        #endregion

        #region 属性

        /// <summary>
        /// 获取表示此 DateTimeX 结构是否未初始化的布尔值。
        /// </summary>
        public bool IsEmpty => !_Initialized;

        //

        /// <summary>
        /// 获取表示此 DateTimeX 结构是否为公元时刻的布尔值。
        /// </summary>
        public bool IsChristianEra => _TotalMilliseconds == _ChristianTotalMilliseconds;

        /// <summary>
        /// 获取表示此 DateTimeX 结构是否为时刻的最小可能值的布尔值。
        /// </summary>
        public bool IsMinValue => _TotalMilliseconds == _MinTotalMilliseconds;

        /// <summary>
        /// 获取表示此 DateTimeX 结构是否为时刻的最大可能值的布尔值。
        /// </summary>
        public bool IsMaxValue => _TotalMilliseconds == _MaxTotalMilliseconds;

        /// <summary>
        /// 获取表示此 DateTimeX 结构是否为一个公元后的时刻（含公元时刻）的布尔值。
        /// </summary>
        public bool IsAnnoDomini => _TotalMilliseconds >= _ChristianTotalMilliseconds;

        /// <summary>
        /// 获取表示此 DateTimeX 结构是否为一个公元前的时刻（不含公元时刻）的布尔值。
        /// </summary>
        public bool IsBeforeChrist => _TotalMilliseconds < _ChristianTotalMilliseconds;

        /// <summary>
        /// 获取表示此 DateTimeX 结构的年是否为闰年的布尔值。
        /// </summary>
        public bool IsLeap => IsLeapYear(Year);

        //

        /// <summary>
        /// 获取或设置此 DateTimeX 结构表示的时刻所在时区的标准时间与协调世界时（UTC）之间的时差的小时数。
        /// </summary>
        public double UtcOffset
        {
            get => _UtcOffset;
            set => _CtorTotalMilliseconds(_TotalMilliseconds, value);
        }

        /// <summary>
        /// 获取或设置此 DateTimeX 结构表示的时刻自公元时刻以来的总毫秒数。
        /// </summary>
        public decimal TotalMilliseconds
        {
            get => _TotalMilliseconds;
            set => _CtorTotalMilliseconds(value, _UtcOffset);
        }

        /// <summary>
        /// 获取或设置此 DateTimeX 结构的年。
        /// </summary>
        public long Year
        {
            get
            {
                if (!_Initialized)
                {
                    return _ChristianYear;
                }
                else
                {
                    return _Year;
                }
            }

            set => _CtorDateTime(value, Month, Day, _Hour, _Minute, _Second, _Millisecond, _UtcOffset);
        }

        /// <summary>
        /// 获取或设置此 DateTimeX 结构的月。
        /// </summary>
        public int Month
        {
            get
            {
                if (!_Initialized)
                {
                    return _MinMonth;
                }
                else
                {
                    return _Month;
                }
            }

            set => _CtorDateTime(Year, value, Day, _Hour, _Minute, _Second, _Millisecond, _UtcOffset);
        }

        /// <summary>
        /// 获取或设置此 DateTimeX 结构的日。
        /// </summary>
        public int Day
        {
            get
            {
                if (!_Initialized)
                {
                    return _MinDay;
                }
                else
                {
                    return _Day;
                }
            }

            set => _CtorDateTime(Year, Month, value, _Hour, _Minute, _Second, _Millisecond, _UtcOffset);
        }

        /// <summary>
        /// 获取或设置此 DateTimeX 结构的时。
        /// </summary>
        public int Hour
        {
            get => _Hour;
            set => _CtorDateTime(Year, Month, Day, value, _Minute, _Second, _Millisecond, _UtcOffset);
        }

        /// <summary>
        /// 获取或设置此 DateTimeX 结构的分。
        /// </summary>
        public int Minute
        {
            get => _Minute;
            set => _CtorDateTime(Year, Month, Day, _Hour, value, _Second, _Millisecond, _UtcOffset);
        }

        /// <summary>
        /// 获取或设置此 DateTimeX 结构的秒。
        /// </summary>
        public int Second
        {
            get => _Second;
            set => _CtorDateTime(Year, Month, Day, _Hour, _Minute, value, _Millisecond, _UtcOffset);
        }

        /// <summary>
        /// 获取或设置此 DateTimeX 结构的毫秒。
        /// </summary>
        public int Millisecond
        {
            get => _Millisecond;
            set => _CtorDateTime(Year, Month, Day, _Hour, _Minute, _Second, value, _UtcOffset);
        }

        //

        /// <summary>
        /// 获取表示此 DateTimeX 结构的日期部分的 DateTimeX 结构的实例，时间值为午夜。
        /// </summary>
        public DateTimeX Date => new DateTimeX(Year, Month, Day, _UtcOffset);

        /// <summary>
        /// 获取表示此 DateTimeX 结构的当天的时间的 TimeSpan 结构的实例，表示自午夜以来的时间间隔。
        /// </summary>
        public TimeSpan TimeOfDay => new TimeSpan(0, _Hour, _Minute, _Second, _Millisecond);

        //

        /// <summary>
        /// 获取此 DateTimeX 结构表示的日期是所在年的第几周（以 1 月 1 日所在周为第一周）。
        /// </summary>
        public int WeekOfYear => (DayOfYear - (2 - new DateTimeX(Year, _UtcOffset)._DayOfWeek)) / 7 + 1;

        /// <summary>
        /// 获取此 DateTimeX 结构表示的日期是所在年的第几天（以 1 月 1 日为第一天）。
        /// </summary>
        public int DayOfYear
        {
            get
            {
                int[] DaysToMonth = IsLeapYear(Year) ? _DaysToMonth366 : _DaysToMonth365;

                return DaysToMonth[Month - 1] + Day;
            }
        }

        /// <summary>
        /// 获取此 DateTimeX 结构表示的时刻是所在年的第几小时（以 1 月 1 日 0 时为第一小时）。
        /// </summary>
        public int HourOfYear => (DayOfYear - 1) * _HoursPerDay + _Hour + 1;

        /// <summary>
        /// 获取此 DateTimeX 结构表示的时刻是所在年的第几分钟（以 1 月 1 日 0 时 0 分为第一分钟）。
        /// </summary>
        public int MinuteOfYear => (DayOfYear - 1) * _MinutesPerDay + _Hour * _MinutesPerHour + _Minute + 1;

        /// <summary>
        /// 获取此 DateTimeX 结构表示的时刻是所在年的第几秒（以 1 月 1 日 0 时 0 分 0 秒为第一秒）。
        /// </summary>
        public int SecondOfYear => (DayOfYear - 1) * _SecondsPerDay + _Hour * _SecondsPerHour + _Minute * _SecondsPerMinute + _Second + 1;

        /// <summary>
        /// 获取此 DateTimeX 结构表示的时刻是所在年的第几毫秒（以 1 月 1 日 0 时 0 分 0 秒 0 毫秒为第一毫秒）。
        /// </summary>
        public long MillisecondOfYear => (long)(DayOfYear - 1) * _MillisecondsPerDay + _Hour * _MillisecondsPerHour + _Minute * _MillisecondsPerMinute + _Second * _MillisecondsPerSecond + _Millisecond + 1;

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的日期是一周中的某天的枚举。
        /// </summary>
        public DayOfWeek DayOfWeek => (DayOfWeek)(_DayOfWeek % 7);

        /// <summary>
        /// 获取此 DateTimeX 结构表示的时刻是所在天的第几小时（以 0 时为第一小时）。
        /// </summary>
        public int HourOfDay => _Hour + 1;

        /// <summary>
        /// 获取此 DateTimeX 结构表示的时刻是所在天的第几分钟（以 0 时 0 分为第一分钟）。
        /// </summary>
        public int MinuteOfDay => _Hour * _MinutesPerHour + _Minute + 1;

        /// <summary>
        /// 获取此 DateTimeX 结构表示的时刻是所在天的第几秒（以 0 时 0 分 0 秒为第一秒）。
        /// </summary>
        public int SecondOfDay => _Hour * _SecondsPerHour + _Minute * _SecondsPerMinute + _Second + 1;

        /// <summary>
        /// 获取此 DateTimeX 结构表示的时刻是所在天的第几毫秒（以 0 时 0 分 0 秒 0 毫秒为第一毫秒）。
        /// </summary>
        public int MillisecondOfDay => _Hour * _MillisecondsPerHour + _Minute * _MillisecondsPerMinute + _Second * _MillisecondsPerSecond + _Millisecond + 1;

        //

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的月的中文字符串。
        /// </summary>
        public string MonthStringInChinese
        {
            get
            {
                switch (Month)
                {
                    case 1: return "一月";
                    case 2: return "二月";
                    case 3: return "三月";
                    case 4: return "四月";
                    case 5: return "五月";
                    case 6: return "六月";
                    case 7: return "七月";
                    case 8: return "八月";
                    case 9: return "九月";
                    case 10: return "十月";
                    case 11: return "十一月";
                    case 12: return "十二月";
                    default: throw new ArgumentOutOfRangeException(nameof(Month));
                }
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的月的英文长字符串。
        /// </summary>
        public string MonthLongStringInEnglish
        {
            get
            {
                switch (Month)
                {
                    case 1: return "January";
                    case 2: return "February";
                    case 3: return "March";
                    case 4: return "April";
                    case 5: return "May";
                    case 6: return "June";
                    case 7: return "July";
                    case 8: return "August";
                    case 9: return "September";
                    case 10: return "October";
                    case 11: return "November";
                    case 12: return "December";
                    default: throw new ArgumentOutOfRangeException(nameof(Month));
                }
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的月的英文短字符串。
        /// </summary>
        public string MonthShortStringInEnglish
        {
            get
            {
                switch (Month)
                {
                    case 1: return "Jan";
                    case 2: return "Feb";
                    case 3: return "Mar";
                    case 4: return "Apr";
                    case 5: return "May";
                    case 6: return "Jun";
                    case 7: return "Jul";
                    case 8: return "Aug";
                    case 9: return "Sept";
                    case 10: return "Oct";
                    case 11: return "Nov";
                    case 12: return "Dec";
                    default: throw new ArgumentOutOfRangeException(nameof(Month));
                }
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的月的日文汉字字符串。
        /// </summary>
        public string MonthStringInJapaneseKanji
        {
            get
            {
                switch (Month)
                {
                    case 1: return "睦月";
                    case 2: return "如月";
                    case 3: return "弥生";
                    case 4: return "卯月";
                    case 5: return "皐月";
                    case 6: return "水無月";
                    case 7: return "文月";
                    case 8: return "葉月";
                    case 9: return "長月";
                    case 10: return "神無月";
                    case 11: return "霜月";
                    case 12: return "師走";
                    default: throw new ArgumentOutOfRangeException(nameof(Month));
                }
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的月的日文平假名字符串。
        /// </summary>
        public string MonthStringInJapaneseHiragana
        {
            get
            {
                switch (Month)
                {
                    case 1: return "むつき";
                    case 2: return "きさらぎ";
                    case 3: return "やよい";
                    case 4: return "うづき";
                    case 5: return "さつき";
                    case 6: return "みなづき";
                    case 7: return "ふみづき";
                    case 8: return "はづき";
                    case 9: return "ながつき";
                    case 10: return "かんなづき";
                    case 11: return "しもつき";
                    case 12: return "しわす";
                    default: throw new ArgumentOutOfRangeException(nameof(Month));
                }
            }
        }

        //

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的日期是一周中的某天的中文长字符串。
        /// </summary>
        public string WeekdayLongStringInChinese
        {
            get
            {
                switch (_DayOfWeek)
                {
                    case 1: return "星期一";
                    case 2: return "星期二";
                    case 3: return "星期三";
                    case 4: return "星期四";
                    case 5: return "星期五";
                    case 6: return "星期六";
                    case 7: return "星期日";
                    default: throw new ArgumentOutOfRangeException(nameof(_DayOfWeek));
                }
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的日期是一周中的某天的中文短字符串。
        /// </summary>
        public string WeekdayShortStringInChinese
        {
            get
            {
                switch (_DayOfWeek)
                {
                    case 1: return "周一";
                    case 2: return "周二";
                    case 3: return "周三";
                    case 4: return "周四";
                    case 5: return "周五";
                    case 6: return "周六";
                    case 7: return "周日";
                    default: throw new ArgumentOutOfRangeException(nameof(_DayOfWeek));
                }
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的日期是一周中的某天的英文长字符串。
        /// </summary>
        public string WeekdayLongStringInEnglish
        {
            get
            {
                switch (_DayOfWeek)
                {
                    case 1: return "Monday";
                    case 2: return "Tuesday";
                    case 3: return "Wednesday";
                    case 4: return "Thursday";
                    case 5: return "Friday";
                    case 6: return "Saturday";
                    case 7: return "Sunday";
                    default: throw new ArgumentOutOfRangeException(nameof(_DayOfWeek));
                }
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的日期是一周中的某天的英文短字符串。
        /// </summary>
        public string WeekdayShortStringInEnglish
        {
            get
            {
                switch (_DayOfWeek)
                {
                    case 1: return "Mon";
                    case 2: return "Tue";
                    case 3: return "Wed";
                    case 4: return "Thur";
                    case 5: return "Fri";
                    case 6: return "Sat";
                    case 7: return "Sun";
                    default: throw new ArgumentOutOfRangeException(nameof(_DayOfWeek));
                }
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的日期是一周中的某天的日文长字符串。
        /// </summary>
        public string WeekdayLongStringInJapanese
        {
            get
            {
                switch (_DayOfWeek)
                {
                    case 1: return "月曜日";
                    case 2: return "火曜日";
                    case 3: return "水曜日";
                    case 4: return "木曜日";
                    case 5: return "金曜日";
                    case 6: return "土曜日";
                    case 7: return "日曜日";
                    default: throw new ArgumentOutOfRangeException(nameof(_DayOfWeek));
                }
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的日期是一周中的某天的日文短字符串。
        /// </summary>
        public string WeekdayShortStringInJapanese
        {
            get
            {
                switch (_DayOfWeek)
                {
                    case 1: return "月";
                    case 2: return "火";
                    case 3: return "水";
                    case 4: return "木";
                    case 5: return "金";
                    case 6: return "土";
                    case 7: return "日";
                    default: throw new ArgumentOutOfRangeException(nameof(_DayOfWeek));
                }
            }
        }

        //

        /// <summary>
        /// 获取表示此 DateTimeX 结构日期部分的长字符串。
        /// </summary>
        public string DateLongString => $"{(Year < 0 ? $"公元前{-Year}" : Year.ToString())}年{Month}月{Day}日";

        /// <summary>
        /// 获取表示此 DateTimeX 结构日期部分的短字符串。
        /// </summary>
        public string DateShortString => $"{(Year < 0 ? $"BC{-Year}" : Year.ToString())}/{Month}/{Day}";

        /// <summary>
        /// 获取表示此 DateTimeX 结构时间部分的长字符串。
        /// </summary>
        public string TimeLongString => $"{_Hour}:{_Minute:D2}:{_Second:D2}.{_Millisecond:D3}";

        /// <summary>
        /// 获取表示此 DateTimeX 结构时间部分的短字符串。
        /// </summary>
        public string TimeShortString => $"{_Hour}:{_Minute:D2}";

        #endregion

        #region 静态属性

        /// <summary>
        /// 获取以本地时区表示的此计算机上的当前日期与时间的 DateTimeX 结构的实例。
        /// </summary>
        public static DateTimeX Now => new DateTimeX(DateTime.Now, _LocalUtcOffset);

        /// <summary>
        /// 获取以协调世界时（UTC）表示的此计算机上的当前日期与时间的 DateTimeX 结构的实例。
        /// </summary>
        public static DateTimeX UtcNow => new DateTimeX(DateTime.Now, _Utc);

        /// <summary>
        /// 获取以本地时区表示的此计算机上的当前日期的 DateTimeX 结构的实例。
        /// </summary>
        public static DateTimeX Today => new DateTimeX(DateTime.Now.Date, _LocalUtcOffset);

        /// <summary>
        /// 获取以协调世界时（UTC）表示的此计算机上的当前日期的 DateTimeX 结构的实例。
        /// </summary>
        public static DateTimeX UtcToday => new DateTimeX(DateTime.Now.Date, _Utc);

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 DateTimeX 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>布尔值，表示此 DateTimeX 结构是否与指定的对象相等。</returns>
        public override bool Equals(object obj)
        {
            if (obj is null || !(obj is DateTimeX))
            {
                return false;
            }
            else
            {
                return Equals((DateTimeX)obj);
            }
        }

        /// <summary>
        /// 返回此 DateTimeX 结构的哈希代码。
        /// </summary>
        /// <returns>32 位整数，表示此 DateTimeX 结构的哈希代码。</returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// 将此 DateTimeX 结构转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 DateTimeX 结构的字符串形式。</returns>
        public override string ToString()
        {
            if (!_Initialized)
            {
                return $"{base.GetType().Name} [Empty]";
            }
            else
            {
                return $"{(Year < 0 ? $"BC{-Year}" : Year.ToString())}/{Month}/{Day} {_Hour}:{_Minute:D2}:{_Second:D2}";
            }
        }

        //

        /// <summary>
        /// 判断此 DateTimeX 结构是否与指定的 DateTimeX 结构相等。
        /// </summary>
        /// <param name="dateTime">用于比较的 DateTimeX 结构。</param>
        /// <returns>布尔值，表示此 DateTimeX 结构是否与指定的 DateTimeX 结构相等。</returns>
        public bool Equals(DateTimeX dateTime) => _Initialized == dateTime._Initialized && _UtcOffset.Equals(dateTime._UtcOffset) && _TotalMilliseconds.Equals(dateTime._TotalMilliseconds) && _Year == dateTime._Year && _Month == dateTime._Month && _Day == dateTime._Day && _Hour == dateTime._Hour && _Minute == dateTime._Minute && _Second == dateTime._Second && _Millisecond == dateTime._Millisecond;

        //

        /// <summary>
        /// 将此 DateTimeX 结构与指定的对象进行次序比较。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>32 位整数，表示将此 DateTimeX 结构与指定的对象进行次序比较得到的结果。</returns>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (!(obj is DateTimeX))
            {
                throw new ArgumentException();
            }
            else
            {
                return CompareTo((DateTimeX)obj);
            }
        }

        /// <summary>
        /// 将此 DateTimeX 结构与指定的 DateTimeX 结构进行次序比较。
        /// </summary>
        /// <param name="dateTime">用于比较的 DateTimeX 结构。</param>
        /// <returns>32 位整数，表示将此 DateTimeX 结构与指定的 DateTimeX 结构进行次序比较得到的结果。</returns>
        public int CompareTo(DateTimeX dateTime) => _TotalMilliseconds.CompareTo(dateTime._TotalMilliseconds);

        //

        /// <summary>
        /// 返回将此 DateTimeX 结构与 TimeSpanX 结构相加得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="timeSpan">TimeSpanX 结构，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构与 TimeSpanX 结构相加得到的结果。</returns>
        public DateTimeX Add(TimeSpanX timeSpan) => AddMilliseconds(timeSpan.TotalMilliseconds);

        /// <summary>
        /// 返回将此 DateTimeX 结构与 TimeSpan 结构相加得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="timeSpan">TimeSpan 结构，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构与 TimeSpan 结构相加得到的结果。</returns>
        public DateTimeX Add(TimeSpan timeSpan) => AddMilliseconds(timeSpan.TotalMilliseconds);

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干年得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="years">64 位整数表示的年数，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干年得到的结果。</returns>
        public DateTimeX AddYears(long years)
        {
            if (years == 0)
            {
                return this;
            }
            else
            {
                if ((double)Year + years - 1 < long.MinValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(years));
                }
                else if ((double)Year + years + 1 > long.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(years));
                }
                else
                {
                    long newYear = Year + years;

                    if (Year < 0 && newYear >= 0)
                    {
                        newYear += 1;
                    }
                    else if (Year > 0 && newYear <= 0)
                    {
                        newYear -= 1;
                    }

                    if (newYear < _LocalMinValue.Year)
                    {
                        throw new ArgumentOutOfRangeException(nameof(years));
                    }
                    else if (newYear > _LocalMaxValue.Year)
                    {
                        throw new ArgumentOutOfRangeException(nameof(years));
                    }
                    else
                    {
                        int newMonth = Month;
                        int newDay = Math.Min(Day, DaysInMonth(newYear, newMonth));

                        return new DateTimeX(newYear, newMonth, newDay, _Hour, _Minute, _Second, _Millisecond, _UtcOffset);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干个月得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="months">64 位整数表示的月数，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干个月得到的结果。</returns>
        public DateTimeX AddMonths(long months)
        {
            if (months == 0)
            {
                return this;
            }
            else
            {
                if ((double)Year + (months / _MonthsPerYear) - 2 < long.MinValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(months));
                }
                else if ((double)Year + (months / _MonthsPerYear) + 2 > long.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(months));
                }
                else
                {
                    long newYear = Year + (months / _MonthsPerYear);
                    int newMonth = Month + (int)(months % _MonthsPerYear);

                    if (newMonth < _MinMonth)
                    {
                        newYear -= 1;
                        newMonth += _MonthsPerYear;
                    }
                    else if (newMonth > _MaxMonth)
                    {
                        newYear += 1;
                        newMonth -= _MonthsPerYear;
                    }

                    if (Year < 0 && newYear >= 0)
                    {
                        newYear += 1;
                    }
                    else if (Year > 0 && newYear <= 0)
                    {
                        newYear -= 1;
                    }

                    DateTimeX localMinValue = _LocalMinValue;

                    if (newYear < localMinValue._Year || (newYear == localMinValue._Year && newMonth < localMinValue._Month))
                    {
                        throw new ArgumentOutOfRangeException(nameof(months));
                    }
                    else
                    {
                        DateTimeX localMaxValue = _LocalMaxValue;

                        if (newYear > localMaxValue._Year || (newYear == localMaxValue._Year && newMonth > localMaxValue._Month))
                        {
                            throw new ArgumentOutOfRangeException(nameof(months));
                        }
                        else
                        {
                            int newDay = Math.Min(Day, DaysInMonth(newYear, newMonth));

                            return new DateTimeX(newYear, newMonth, newDay, _Hour, _Minute, _Second, _Millisecond, _UtcOffset);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干周得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="weeks">双精度浮点数表示的周数，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干周得到的结果。</returns>
        public DateTimeX AddWeeks(double weeks)
        {
            if (InternalMethod.IsNaNOrInfinity(weeks))
            {
                throw new ArgumentOutOfRangeException(nameof(weeks));
            }

            //

            if (weeks == 0)
            {
                return this;
            }
            else
            {
                if ((double)_TotalMilliseconds + weeks * _MillisecondsPerWeek < (double)decimal.MinValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(weeks));
                }
                else if ((double)_TotalMilliseconds + weeks * _MillisecondsPerWeek > (double)decimal.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(weeks));
                }
                else
                {
                    decimal newTotalMS = _TotalMilliseconds + (decimal)weeks * _MillisecondsPerWeek;

                    if (newTotalMS < _MinTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(weeks));
                    }
                    else if (newTotalMS > _MaxTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(weeks));
                    }
                    else
                    {
                        return new DateTimeX(newTotalMS, _UtcOffset);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干天得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="days">十进制浮点数表示的天数，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干天得到的结果。</returns>
        public DateTimeX AddDays(decimal days)
        {
            if (days == 0)
            {
                return this;
            }
            else
            {
                if ((double)_TotalMilliseconds + (double)days * _MillisecondsPerDay < (double)decimal.MinValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(days));
                }
                else if ((double)_TotalMilliseconds + (double)days * _MillisecondsPerDay > (double)decimal.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(days));
                }
                else
                {
                    decimal newTotalMS = _TotalMilliseconds + days * _MillisecondsPerDay;

                    if (newTotalMS < _MinTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(days));
                    }
                    else if (newTotalMS > _MaxTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(days));
                    }
                    else
                    {
                        return new DateTimeX(newTotalMS, _UtcOffset);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干天得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="days">双精度浮点数表示的天数，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干天得到的结果。</returns>
        public DateTimeX AddDays(double days)
        {
            if (InternalMethod.IsNaNOrInfinity(days))
            {
                throw new ArgumentOutOfRangeException(nameof(days));
            }

            //

            if (days == 0)
            {
                return this;
            }
            else
            {
                if ((double)_TotalMilliseconds + days * _MillisecondsPerDay < (double)decimal.MinValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(days));
                }
                else if ((double)_TotalMilliseconds + days * _MillisecondsPerDay > (double)decimal.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(days));
                }
                else
                {
                    decimal newTotalMS = _TotalMilliseconds + (decimal)days * _MillisecondsPerDay;

                    if (newTotalMS < _MinTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(days));
                    }
                    else if (newTotalMS > _MaxTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(days));
                    }
                    else
                    {
                        return new DateTimeX(newTotalMS, _UtcOffset);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干小时得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="hours">十进制浮点数表示的小时数，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干小时得到的结果。</returns>
        public DateTimeX AddHours(decimal hours)
        {
            if (hours == 0)
            {
                return this;
            }
            else
            {
                if ((double)_TotalMilliseconds + (double)hours * _MillisecondsPerHour < (double)decimal.MinValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(hours));
                }
                else if ((double)_TotalMilliseconds + (double)hours * _MillisecondsPerHour > (double)decimal.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(hours));
                }
                else
                {
                    decimal newTotalMS = _TotalMilliseconds + hours * _MillisecondsPerHour;

                    if (newTotalMS < _MinTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(hours));
                    }
                    else if (newTotalMS > _MaxTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(hours));
                    }
                    else
                    {
                        return new DateTimeX(newTotalMS, _UtcOffset);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干小时得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="hours">双精度浮点数表示的小时数，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干小时得到的结果。</returns>
        public DateTimeX AddHours(double hours)
        {
            if (InternalMethod.IsNaNOrInfinity(hours))
            {
                throw new ArgumentOutOfRangeException(nameof(hours));
            }

            //

            if (hours == 0)
            {
                return this;
            }
            else
            {
                if ((double)_TotalMilliseconds + hours * _MillisecondsPerHour < (double)decimal.MinValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(hours));
                }
                else if ((double)_TotalMilliseconds + hours * _MillisecondsPerHour > (double)decimal.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(hours));
                }
                else
                {
                    decimal newTotalMS = _TotalMilliseconds + (decimal)hours * _MillisecondsPerHour;

                    if (newTotalMS < _MinTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(hours));
                    }
                    else if (newTotalMS > _MaxTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(hours));
                    }
                    else
                    {
                        return new DateTimeX(newTotalMS, _UtcOffset);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干分钟得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="minutes">十进制浮点数表示的分钟数，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干分钟得到的结果。</returns>
        public DateTimeX AddMinutes(decimal minutes)
        {
            if (minutes == 0)
            {
                return this;
            }
            else
            {
                if ((double)_TotalMilliseconds + (double)minutes * _MillisecondsPerMinute < (double)decimal.MinValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(minutes));
                }
                else if ((double)_TotalMilliseconds + (double)minutes * _MillisecondsPerMinute > (double)decimal.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(minutes));
                }
                else
                {
                    decimal newTotalMS = _TotalMilliseconds + minutes * _MillisecondsPerMinute;

                    if (newTotalMS < _MinTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(minutes));
                    }
                    else if (newTotalMS > _MaxTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(minutes));
                    }
                    else
                    {
                        return new DateTimeX(newTotalMS, _UtcOffset);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干分钟得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="minutes">双精度浮点数表示的分钟数，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干分钟得到的结果。</returns>
        public DateTimeX AddMinutes(double minutes)
        {
            if (InternalMethod.IsNaNOrInfinity(minutes))
            {
                throw new ArgumentOutOfRangeException(nameof(minutes));
            }

            //

            if (minutes == 0)
            {
                return this;
            }
            else
            {
                if ((double)_TotalMilliseconds + minutes * _MillisecondsPerMinute < (double)decimal.MinValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(minutes));
                }
                else if ((double)_TotalMilliseconds + minutes * _MillisecondsPerMinute > (double)decimal.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(minutes));
                }
                else
                {
                    decimal newTotalMS = _TotalMilliseconds + (decimal)minutes * _MillisecondsPerMinute;

                    if (newTotalMS < _MinTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(minutes));
                    }
                    else if (newTotalMS > _MaxTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(minutes));
                    }
                    else
                    {
                        return new DateTimeX(newTotalMS, _UtcOffset);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干秒得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="seconds">十进制浮点数表示的秒数，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干秒得到的结果。</returns>
        public DateTimeX AddSeconds(decimal seconds)
        {
            if (seconds == 0)
            {
                return this;
            }
            else
            {
                if ((double)_TotalMilliseconds + (double)seconds * _MillisecondsPerSecond < (double)decimal.MinValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(seconds));
                }
                else if ((double)_TotalMilliseconds + (double)seconds * _MillisecondsPerSecond > (double)decimal.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(seconds));
                }
                else
                {
                    decimal newTotalMS = _TotalMilliseconds + seconds * _MillisecondsPerSecond;

                    if (newTotalMS < _MinTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(seconds));
                    }
                    else if (newTotalMS > _MaxTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(seconds));
                    }
                    else
                    {
                        return new DateTimeX(newTotalMS, _UtcOffset);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干秒得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="seconds">双精度浮点数表示的秒数，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干秒得到的结果。</returns>
        public DateTimeX AddSeconds(double seconds)
        {
            if (InternalMethod.IsNaNOrInfinity(seconds))
            {
                throw new ArgumentOutOfRangeException(nameof(seconds));
            }

            //

            if (seconds == 0)
            {
                return this;
            }
            else
            {
                if ((double)_TotalMilliseconds + seconds * _MillisecondsPerSecond < (double)decimal.MinValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(seconds));
                }
                else if ((double)_TotalMilliseconds + seconds * _MillisecondsPerSecond > (double)decimal.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(seconds));
                }
                else
                {
                    decimal newTotalMS = _TotalMilliseconds + (decimal)seconds * _MillisecondsPerSecond;

                    if (newTotalMS < _MinTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(seconds));
                    }
                    else if (newTotalMS > _MaxTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(seconds));
                    }
                    else
                    {
                        return new DateTimeX(newTotalMS, _UtcOffset);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干毫秒得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="milliseconds">十进制浮点数表示的毫秒数，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干毫秒得到的结果。</returns>
        public DateTimeX AddMilliseconds(decimal milliseconds)
        {
            if (milliseconds == 0)
            {
                return this;
            }
            else
            {
                if ((double)_TotalMilliseconds + (double)milliseconds < (double)decimal.MinValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(milliseconds));
                }
                else if ((double)_TotalMilliseconds + (double)milliseconds > (double)decimal.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(milliseconds));
                }
                else
                {
                    decimal newTotalMS = _TotalMilliseconds + milliseconds;

                    if (newTotalMS < _MinTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(milliseconds));
                    }
                    else if (newTotalMS > _MaxTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(milliseconds));
                    }
                    else
                    {
                        return new DateTimeX(newTotalMS, _UtcOffset);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干毫秒得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="milliseconds">双精度浮点数表示的毫秒数，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干毫秒得到的结果。</returns>
        public DateTimeX AddMilliseconds(double milliseconds)
        {
            if (InternalMethod.IsNaNOrInfinity(milliseconds))
            {
                throw new ArgumentOutOfRangeException(nameof(milliseconds));
            }

            //

            if (milliseconds == 0)
            {
                return this;
            }
            else
            {
                if ((double)_TotalMilliseconds + milliseconds < (double)decimal.MinValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(milliseconds));
                }
                else if ((double)_TotalMilliseconds + milliseconds > (double)decimal.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(milliseconds));
                }
                else
                {
                    decimal newTotalMS = _TotalMilliseconds + (decimal)milliseconds;

                    if (newTotalMS < _MinTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(milliseconds));
                    }
                    else if (newTotalMS > _MaxTotalMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(nameof(milliseconds));
                    }
                    else
                    {
                        return new DateTimeX(newTotalMS, _UtcOffset);
                    }
                }
            }
        }

        //

        /// <summary>
        /// 返回将此 DateTimeX 结构转换为以本地时区表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构转换为以本地时区表示的 DateTimeX 结构得到的结果。</returns>
        public DateTimeX ToLocalTime() => new DateTimeX(_TotalMilliseconds, _LocalUtcOffset);

        /// <summary>
        /// 返回将此 DateTimeX 结构转换为以协调世界时（UTC）表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构转换为以协调世界时（UTC）表示的 DateTimeX 结构得到的结果。</returns>
        public DateTimeX ToUniversalTime() => new DateTimeX(_TotalMilliseconds, _Utc);

        //

        /// <summary>
        /// 返回表示此 DateTimeX 结构的长日期字符串。
        /// </summary>
        /// <returns>字符串，表示此 DateTimeX 结构的长日期字符串形式。</returns>
        public string ToLongDateString() => DateLongString;

        /// <summary>
        /// 返回表示此 DateTimeX 结构的短日期字符串。
        /// </summary>
        /// <returns>字符串，表示此 DateTimeX 结构的短日期字符串形式。</returns>
        public string ToShortDateString() => DateShortString;

        /// <summary>
        /// 返回表示此 DateTimeX 结构的长时间字符串。
        /// </summary>
        /// <returns>字符串，表示此 DateTimeX 结构的长时间字符串形式。</returns>
        public string ToLongTimeString() => TimeLongString;

        /// <summary>
        /// 返回表示此 DateTimeX 结构的短时间字符串。
        /// </summary>
        /// <returns>字符串，表示此 DateTimeX 结构的短时间字符串形式。</returns>
        public string ToShortTimeString() => TimeShortString;

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断两个 DateTimeX 结构是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 DateTimeX 结构。</param>
        /// <param name="right">用于比较的第二个 DateTimeX 结构。</param>
        /// <returns>布尔值，表示两个 DateTimeX 结构是否相等。</returns>
        public static bool Equals(DateTimeX left, DateTimeX right) => left.Equals(right);

        //

        /// <summary>
        /// 比较两个 DateTimeX 结构的次序。
        /// </summary>
        /// <param name="left">用于比较的第一个 DateTimeX 结构。</param>
        /// <param name="right">用于比较的第二个 DateTimeX 结构。</param>
        /// <returns>32 位整数，表示将两个 DateTimeX 结构进行次序比较得到的结果。</returns>
        public static int Compare(DateTimeX left, DateTimeX right) => left.CompareTo(right);

        //

        /// <summary>
        /// 返回表示指定年是否为闰年的布尔值。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <returns>布尔值，表示指定年是否为闰年。</returns>
        public static bool IsLeapYear(long year)
        {
            if (year == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(year));
            }

            //

            if (year < 0)
            {
                year = -year - 1;
            }

            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0) && (year % 3200 != 0 || year % 172800 == 0);
        }

        //

        /// <summary>
        /// 返回指定年的天数。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <returns>32 位整数，表示指定年的天数。</returns>
        public static int DaysInYear(long year)
        {
            if (year == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(year));
            }

            //

            return IsLeapYear(year) ? 366 : 365;
        }

        /// <summary>
        /// 返回指定年的指定月的天数。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="month">32 位整数表示的月。</param>
        /// <returns>32 位整数，表示指定年的指定月的天数。</returns>
        public static int DaysInMonth(long year, int month)
        {
            if (year == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(year));
            }

            if (month < _MinMonth || month > _MaxMonth)
            {
                throw new ArgumentOutOfRangeException(nameof(month));
            }

            //

            int[] DaysToMonth = IsLeapYear(year) ? _DaysToMonth366 : _DaysToMonth365;

            return DaysToMonth[month] - DaysToMonth[month - 1];
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 DateTimeX 结构是否表示相同的时刻。
        /// </summary>
        /// <param name="left">运算符左侧比较的 DateTimeX 结构。</param>
        /// <param name="right">运算符右侧比较的 DateTimeX 结构。</param>
        /// <returns>布尔值，表示两个 DateTimeX 结构是否表示相同的时刻。</returns>
        public static bool operator ==(DateTimeX left, DateTimeX right)
        {
            if (!left._Initialized || !right._Initialized)
            {
                return false;
            }
            else
            {
                return left._TotalMilliseconds == right._TotalMilliseconds;
            }
        }

        /// <summary>
        /// 判断两个 DateTimeX 结构是否表示不同的时刻。
        /// </summary>
        /// <param name="left">运算符左侧比较的 DateTimeX 结构。</param>
        /// <param name="right">运算符右侧比较的 DateTimeX 结构。</param>
        /// <returns>布尔值，表示两个 DateTimeX 结构是否表示不同的时刻。</returns>
        public static bool operator !=(DateTimeX left, DateTimeX right)
        {
            if (!left._Initialized || !right._Initialized)
            {
                return true;
            }
            else
            {
                return left._TotalMilliseconds != right._TotalMilliseconds;
            }
        }

        /// <summary>
        /// 判断两个 DateTimeX 结构表示的时刻是否前者早于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 DateTimeX 结构。</param>
        /// <param name="right">运算符右侧比较的 DateTimeX 结构。</param>
        /// <returns>布尔值，表示两个 DateTimeX 结构表示的时刻是否前者早于后者。</returns>
        public static bool operator <(DateTimeX left, DateTimeX right)
        {
            if (!left._Initialized || !right._Initialized)
            {
                return false;
            }
            else
            {
                return left._TotalMilliseconds < right._TotalMilliseconds;
            }
        }

        /// <summary>
        /// 判断两个 DateTimeX 结构表示的时刻是否前者晚于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 DateTimeX 结构。</param>
        /// <param name="right">运算符右侧比较的 DateTimeX 结构。</param>
        /// <returns>布尔值，表示两个 DateTimeX 结构表示的时刻是否前者晚于后者。</returns>
        public static bool operator >(DateTimeX left, DateTimeX right)
        {
            if (!left._Initialized || !right._Initialized)
            {
                return false;
            }
            else
            {
                return left._TotalMilliseconds > right._TotalMilliseconds;
            }
        }

        /// <summary>
        /// 判断两个 DateTimeX 结构表示的时刻是否前者早于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 DateTimeX 结构。</param>
        /// <param name="right">运算符右侧比较的 DateTimeX 结构。</param>
        /// <returns>布尔值，表示两个 DateTimeX 结构表示的时刻是否前者早于或等于后者。</returns>
        public static bool operator <=(DateTimeX left, DateTimeX right)
        {
            if (!left._Initialized || !right._Initialized)
            {
                return false;
            }
            else
            {
                return left._TotalMilliseconds <= right._TotalMilliseconds;
            }
        }

        /// <summary>
        /// 判断两个 DateTimeX 结构表示的时刻是否前者晚于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 DateTimeX 结构。</param>
        /// <param name="right">运算符右侧比较的 DateTimeX 结构。</param>
        /// <returns>布尔值，表示两个 DateTimeX 结构表示的时刻是否前者晚于或等于后者。</returns>
        public static bool operator >=(DateTimeX left, DateTimeX right)
        {
            if (!left._Initialized || !right._Initialized)
            {
                return false;
            }
            else
            {
                return left._TotalMilliseconds >= right._TotalMilliseconds;
            }
        }

        //

        /// <summary>
        /// 返回将 DateTimeX 结构与 TimeSpanX 结构相加得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="dateTime">DateTimeX 结构，表示被加数。</param>
        /// <param name="timeSpan">TimeSpanX 结构，表示加数。</param>
        /// <returns>DateTimeX 结构，表示将 DateTimeX 结构与 TimeSpanX 结构相加得到的结果。</returns>
        public static DateTimeX operator +(DateTimeX dateTime, TimeSpanX timeSpan) => dateTime.AddMilliseconds(timeSpan.TotalMilliseconds);

        /// <summary>
        /// 返回将 DateTimeX 结构与 TimeSpan 结构相加得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="dateTime">DateTimeX 结构，表示被加数。</param>
        /// <param name="timeSpan">TimeSpan 结构，表示加数。</param>
        /// <returns>DateTimeX 结构，表示将 DateTimeX 结构与 TimeSpan 结构相加得到的结果。</returns>
        public static DateTimeX operator +(DateTimeX dateTime, TimeSpan timeSpan) => dateTime.AddMilliseconds(timeSpan.TotalMilliseconds);

        /// <summary>
        /// 返回将 DateTimeX 结构与 TimeSpanX 结构相减得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="dateTime">DateTimeX 结构，表示被减数。</param>
        /// <param name="timeSpan">TimeSpanX 结构，表示减数。</param>
        /// <returns>DateTimeX 结构，表示将 DateTimeX 结构与 TimeSpanX 结构相减得到的结果。</returns>
        public static DateTimeX operator -(DateTimeX dateTime, TimeSpanX timeSpan) => dateTime.AddMilliseconds(-timeSpan.TotalMilliseconds);

        /// <summary>
        /// 返回将 DateTimeX 结构与 TimeSpan 结构相减得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="dateTime">DateTimeX 结构，表示被减数。</param>
        /// <param name="timeSpan">TimeSpan 结构，表示减数。</param>
        /// <returns>DateTimeX 结构，表示将 DateTimeX 结构与 TimeSpan 结构相减得到的结果。</returns>
        public static DateTimeX operator -(DateTimeX dateTime, TimeSpan timeSpan) => dateTime.AddMilliseconds(-timeSpan.TotalMilliseconds);

        //

        /// <summary>
        /// 将指定的 DateTime 结构隐式转换为 DateTimeX 结构。
        /// </summary>
        /// <param name="dateTime">用于转换的 DateTime 结构。</param>
        /// <returns>DateTimeX 结构，表示隐式转换的结果。</returns>
        public static implicit operator DateTimeX(DateTime dateTime) => new DateTimeX(dateTime);

        #endregion
    }
}