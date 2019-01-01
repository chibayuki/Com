/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.DateTimeX
Version 18.9.28.2200

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
        #region 私有成员与内部成员

        private const double _MinUtcOffset = -24D, _MaxUtcOffset = 24D, _Utc = 0D; // 所在时区的标准时间与协调世界时（UTC）之间的时差的小时数的最小值、最大值与表示与协调世界时（UTC）之间为零时差的值。

        private static double _LocalUtcOffset => TimeZoneInfo.Local.BaseUtcOffset.TotalHours; // 本地时区的标准时间与协调世界时（UTC）之间的时差的小时数。

        private const decimal _MinTotalMilliseconds = -796899343984252546694400000M, _MaxTotalMilliseconds = 796899343984252578143999999M, _ChristianTotalMilliseconds = 0M; // 自公元时刻以来的总毫秒数的最小值、最大值与公元时刻的值。

        private const long _MinYear = -25252756133808173L, _MaxYear = 25252756133808174L, _ChristianYear = 1L; // 年的最小值、最大值与公元时刻的值。
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

        private static double _CheckUtcOffset(double utcOffset) // 对所在时区的标准时间与协调世界时（UTC）之间的时差的小时数的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(utcOffset))
            {
                return _LocalUtcOffset;
            }

            if (utcOffset < _MinUtcOffset)
            {
                return _MinUtcOffset;
            }
            else if (utcOffset > _MaxUtcOffset)
            {
                return _MaxUtcOffset;
            }

            return utcOffset;
        }

        private static decimal _CheckTotalMilliseconds(decimal totalMilliseconds) // 对自公元时刻以来的总毫秒数的值进行合法性检查，返回合法的值。
        {
            decimal _totalMilliseconds = Math.Round(totalMilliseconds);

            if (_totalMilliseconds < _MinTotalMilliseconds)
            {
                return _MinTotalMilliseconds;
            }
            else if (_totalMilliseconds > _MaxTotalMilliseconds)
            {
                return _MaxTotalMilliseconds;
            }

            return _totalMilliseconds;
        }

        private static long _CheckYear(long year, double utcOffset) // 对年的值进行合法性检查，返回合法的值。
        {
            if (year == 0)
            {
                return _ChristianYear;
            }

            long MinYear = _MinYear - (utcOffset < 0 ? 1 : 0);

            if (year < MinYear)
            {
                return MinYear;
            }
            else
            {
                long MaxYear = _MaxYear + (utcOffset > 0 ? 1 : 0);

                if (year > MaxYear)
                {
                    return MaxYear;
                }
            }

            return year;
        }

        private static int _CheckMonth(int month) // 对月的值进行合法性检查，返回合法的值。
        {
            if (month < _MinMonth)
            {
                return _MinMonth;
            }
            else if (month > _MaxMonth)
            {
                return _MaxMonth;
            }

            return month;
        }

        private static int _CheckDay(long year, int month, int day) // 对日的值进行合法性检查，返回合法的值。
        {
            if (day < _MinDay)
            {
                return _MinDay;
            }
            else
            {
                int _DaysInMonth = DaysInMonth(year, month);

                if (day > _DaysInMonth)
                {
                    return _DaysInMonth;
                }
            }

            return day;
        }

        private static int _CheckHour(int hour) // 对时的值进行合法性检查，返回合法的值。
        {
            if (hour < _MinHour)
            {
                return _MinHour;
            }
            else if (hour > _MaxHour)
            {
                return _MaxHour;
            }

            return hour;
        }

        private static int _CheckMinute(int minute) // 对分的值进行合法性检查，返回合法的值。
        {
            if (minute < _MinMinute)
            {
                return _MinMinute;
            }
            else if (minute > _MaxMinute)
            {
                return _MaxMinute;
            }

            return minute;
        }

        private static int _CheckSecond(int second) // 对秒的值进行合法性检查，返回合法的值。
        {
            if (second < _MinSecond)
            {
                return _MinSecond;
            }
            else if (second > _MaxSecond)
            {
                return _MaxSecond;
            }

            return second;
        }

        private static int _CheckMillisecond(int millisecond) // 对毫秒的值进行合法性检查，返回合法的值。
        {
            if (millisecond < _MinMillisecond)
            {
                return _MinMillisecond;
            }
            else if (millisecond > _MaxMillisecond)
            {
                return _MaxMillisecond;
            }

            return millisecond;
        }

        //

        private static void _TotalMillisecondsToDateTime(decimal totalMilliseconds, out long year, out int month, out int day, out int hour, out int minute, out int second, out int millisecond) // 将自公元时刻以来的总毫秒数转换为年、月、日、时、分、秒与毫秒。此函数不对参数进行合法性检查。
        {
            long TotalDays = (long)(Math.Floor(totalMilliseconds / _MillisecondsPerDay));

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

            int[] DaysToMonth = (Leap ? _DaysToMonth366 : _DaysToMonth365);

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

        private static void _DateTimeToTotalMilliseconds(long year, int month, int day, int hour, int minute, int second, int millisecond, out decimal totalMilliseconds) // 将年、月、日、时、分、秒与毫秒转换为自公元时刻以来的总毫秒数。此函数不对参数进行合法性检查。
        {
            long TotalDays = 0;

            int[] DaysToMonth = (IsLeapYear(year) ? _DaysToMonth366 : _DaysToMonth365);

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

        private DateTimeX _ThisMinValue => new DateTimeX(_MinTotalMilliseconds, UtcOffset); // 表示所在时区时刻的最小可能值的 DateTimeX 结构的实例。

        private DateTimeX _ThisMaxValue => new DateTimeX(_MaxTotalMilliseconds, UtcOffset); // 表示所在时区时刻的最大可能值的 DateTimeX 结构的实例。

        //

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

        private void _CtorTotalMilliseconds(decimal totalMilliseconds, double utcOffset) // 为以自公元时刻以来的总毫秒数为参数的构造函数提供实现。
        {
            _UtcOffset = _CheckUtcOffset(utcOffset);

            decimal _UtcOffsetMS = (decimal)_UtcOffset * _MillisecondsPerHour;

            _TotalMilliseconds = _CheckTotalMilliseconds(totalMilliseconds);

            _TotalMillisecondsToDateTime(_TotalMilliseconds + _UtcOffsetMS, out _Year, out _Month, out _Day, out _Hour, out _Minute, out _Second, out _Millisecond);

            _Year = _CheckYear(_Year, _UtcOffset);
            _Month = _CheckMonth(_Month);
            _Day = _CheckDay(_Year, _Month, _Day);
            _Hour = _CheckHour(_Hour);
            _Minute = _CheckMinute(_Minute);
            _Second = _CheckSecond(_Second);
            _Millisecond = _CheckMillisecond(_Millisecond);
        }

        private void _CtorDateTime(long year, int month, int day, int hour, int minute, int second, int millisecond, double utcOffset) // 为以年、月、日、时、分、秒与毫秒为参数的构造函数提供实现。
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

                _Year = _CheckYear(_Year, _UtcOffset);
                _Month = _CheckMonth(_Month);
                _Day = _CheckDay(_Year, _Month, _Day);
                _Hour = _CheckHour(_Hour);
                _Minute = _CheckMinute(_Minute);
                _Second = _CheckSecond(_Second);
                _Millisecond = _CheckMillisecond(_Millisecond);
            }
        }

        #endregion

        #region 常量与只读字段

        /// <summary>
        /// 表示所有属性为其数据类型的默认值的 DateTimeX 结构的实例。
        /// </summary>
        public static readonly DateTimeX Empty = default(DateTimeX);

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

        #region 构造函数

        /// <summary>
        /// 使用十进制数表示的自公元时刻以来的总毫秒与所在时区的标准时间与协调世界时（UTC）之间的时差的小时数数初始化 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="totalMilliseconds">十进制数表示的自公元时刻以来的总毫秒数。</param>
        /// <param name="utcOffset">双精度浮点数表示的所在时区的标准时间与协调世界时（UTC）之间的时差的小时数。</param>
        public DateTimeX(decimal totalMilliseconds, double utcOffset)
        {
            this = default(DateTimeX);

            _CtorTotalMilliseconds(totalMilliseconds, utcOffset);
        }

        /// <summary>
        /// 使用十进制数表示的自公元时刻以来的总毫秒数初始化以本地时区表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="totalMilliseconds">十进制数表示的自公元时刻以来的总毫秒数。</param>
        public DateTimeX(decimal totalMilliseconds)
        {
            this = default(DateTimeX);

            _CtorTotalMilliseconds(totalMilliseconds, _LocalUtcOffset);
        }

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
        public DateTimeX(long year, int month, int day, int hour, int minute, int second, int millisecond, double utcOffset)
        {
            this = default(DateTimeX);

            _CtorDateTime(year, month, day, hour, minute, second, millisecond, utcOffset);
        }

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
        public DateTimeX(long year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            this = default(DateTimeX);

            _CtorDateTime(year, month, day, hour, minute, second, millisecond, _LocalUtcOffset);
        }

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
        public DateTimeX(long year, int month, int day, int hour, int minute, int second, double utcOffset)
        {
            this = default(DateTimeX);

            _CtorDateTime(year, month, day, hour, minute, second, _MinMillisecond, utcOffset);
        }

        /// <summary>
        /// 使用指定的年、月、日、时、分与秒初始化以本地时区表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="month">32 位整数表示的月。</param>
        /// <param name="day">32 位整数表示的日。</param>
        /// <param name="hour">32 位整数表示的时。</param>
        /// <param name="minute">32 位整数表示的分。</param>
        /// <param name="second">32 位整数表示的秒。</param>
        public DateTimeX(long year, int month, int day, int hour, int minute, int second)
        {
            this = default(DateTimeX);

            _CtorDateTime(year, month, day, hour, minute, second, _MinMillisecond, _LocalUtcOffset);
        }

        /// <summary>
        /// 使用指定的年、月、日、时、分与所在时区的标准时间与协调世界时（UTC）之间的时差的小时数初始化 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="month">32 位整数表示的月。</param>
        /// <param name="day">32 位整数表示的日。</param>
        /// <param name="hour">32 位整数表示的时。</param>
        /// <param name="minute">32 位整数表示的分。</param>
        /// <param name="utcOffset">双精度浮点数表示的所在时区的标准时间与协调世界时（UTC）之间的时差的小时数。</param>
        public DateTimeX(long year, int month, int day, int hour, int minute, double utcOffset)
        {
            this = default(DateTimeX);

            _CtorDateTime(year, month, day, hour, minute, _MinSecond, _MinMillisecond, utcOffset);
        }

        /// <summary>
        /// 使用指定的年、月、日、时与分初始化以本地时区表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="month">32 位整数表示的月。</param>
        /// <param name="day">32 位整数表示的日。</param>
        /// <param name="hour">32 位整数表示的时。</param>
        /// <param name="minute">32 位整数表示的分。</param>
        public DateTimeX(long year, int month, int day, int hour, int minute)
        {
            this = default(DateTimeX);

            _CtorDateTime(year, month, day, hour, minute, _MinSecond, _MinMillisecond, _LocalUtcOffset);
        }

        /// <summary>
        /// 使用指定的年、月、日、时与所在时区的标准时间与协调世界时（UTC）之间的时差的小时数初始化 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="month">32 位整数表示的月。</param>
        /// <param name="day">32 位整数表示的日。</param>
        /// <param name="hour">32 位整数表示的时。</param>
        /// <param name="utcOffset">双精度浮点数表示的所在时区的标准时间与协调世界时（UTC）之间的时差的小时数。</param>
        public DateTimeX(long year, int month, int day, int hour, double utcOffset)
        {
            this = default(DateTimeX);

            _CtorDateTime(year, month, day, hour, _MinMinute, _MinSecond, _MinMillisecond, utcOffset);
        }

        /// <summary>
        /// 使用指定的年、月、日与时初始化以本地时区表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="month">32 位整数表示的月。</param>
        /// <param name="day">32 位整数表示的日。</param>
        /// <param name="hour">32 位整数表示的时。</param>
        public DateTimeX(long year, int month, int day, int hour)
        {
            this = default(DateTimeX);

            _CtorDateTime(year, month, day, hour, _MinMinute, _MinSecond, _MinMillisecond, _LocalUtcOffset);
        }

        /// <summary>
        /// 使用指定的年、月、日与所在时区的标准时间与协调世界时（UTC）之间的时差的小时数初始化 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="month">32 位整数表示的月。</param>
        /// <param name="day">32 位整数表示的日。</param>
        /// <param name="utcOffset">双精度浮点数表示的所在时区的标准时间与协调世界时（UTC）之间的时差的小时数。</param>
        public DateTimeX(long year, int month, int day, double utcOffset)
        {
            this = default(DateTimeX);

            _CtorDateTime(year, month, day, _MinHour, _MinMinute, _MinSecond, _MinMillisecond, utcOffset);
        }

        /// <summary>
        /// 使用指定的年、月与日初始化以本地时区表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="month">32 位整数表示的月。</param>
        /// <param name="day">32 位整数表示的日。</param>
        public DateTimeX(long year, int month, int day)
        {
            this = default(DateTimeX);

            _CtorDateTime(year, month, day, _MinHour, _MinMinute, _MinSecond, _MinMillisecond, _LocalUtcOffset);
        }

        /// <summary>
        /// 使用指定的年、月与所在时区的标准时间与协调世界时（UTC）之间的时差的小时数初始化 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="month">32 位整数表示的月。</param>
        /// <param name="utcOffset">双精度浮点数表示的所在时区的标准时间与协调世界时（UTC）之间的时差的小时数。</param>
        public DateTimeX(long year, int month, double utcOffset)
        {
            this = default(DateTimeX);

            _CtorDateTime(year, month, _MinDay, _MinHour, _MinMinute, _MinSecond, _MinMillisecond, utcOffset);
        }

        /// <summary>
        /// 使用指定的年与月初始化以本地时区表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="month">32 位整数表示的月。</param>
        public DateTimeX(long year, int month)
        {
            this = default(DateTimeX);

            _CtorDateTime(year, month, _MinDay, _MinHour, _MinMinute, _MinSecond, _MinMillisecond, _LocalUtcOffset);
        }

        /// <summary>
        /// 使用指定的年与所在时区的标准时间与协调世界时（UTC）之间的时差的小时数初始化 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="utcOffset">双精度浮点数表示的所在时区的标准时间与协调世界时（UTC）之间的时差的小时数。</param>
        public DateTimeX(long year, double utcOffset)
        {
            this = default(DateTimeX);

            _CtorDateTime(year, _MinMonth, _MinDay, _MinHour, _MinMinute, _MinSecond, _MinMillisecond, utcOffset);
        }

        /// <summary>
        /// 使用指定的年初始化以本地时区表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        public DateTimeX(long year)
        {
            this = default(DateTimeX);

            _CtorDateTime(year, _MinMonth, _MinDay, _MinHour, _MinMinute, _MinSecond, _MinMillisecond, _LocalUtcOffset);
        }

        /// <summary>
        /// 使用 DateTimeX 结构与所在时区的标准时间与协调世界时（UTC）之间的时差的小时数初始化 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="dateTime">DateTimeX 结构。</param>
        /// <param name="utcOffset">双精度浮点数表示的所在时区的标准时间与协调世界时（UTC）之间的时差的小时数。</param>
        public DateTimeX(DateTimeX dateTime, double utcOffset)
        {
            this = default(DateTimeX);

            _CtorDateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, utcOffset);
        }

        /// <summary>
        /// 使用 DateTimeX 结构初始化以本地时区表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="dateTime">DateTimeX 结构。</param>
        public DateTimeX(DateTimeX dateTime)
        {
            this = default(DateTimeX);

            _CtorDateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, _LocalUtcOffset);
        }

        /// <summary>
        /// 使用 DateTime 结构与所在时区的标准时间与协调世界时（UTC）之间的时差的小时数初始化 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="dateTime">DateTime 结构。</param>
        /// <param name="utcOffset">双精度浮点数表示的所在时区的标准时间与协调世界时（UTC）之间的时差的小时数。</param>
        public DateTimeX(DateTime dateTime, double utcOffset)
        {
            this = default(DateTimeX);

            _CtorDateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, utcOffset);
        }

        /// <summary>
        /// 使用 DateTime 结构初始化以本地时区表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="dateTime">DateTime 结构。</param>
        public DateTimeX(DateTime dateTime)
        {
            this = default(DateTimeX);

            _CtorDateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, _LocalUtcOffset);
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取表示此 DateTimeX 结构是否为 Empty 的布尔值。
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (_UtcOffset == Empty._UtcOffset && _TotalMilliseconds == Empty._TotalMilliseconds && _Year == Empty._Year && _Month == Empty._Month && _Day == Empty._Day && _Hour == Empty._Hour && _Minute == Empty._Minute && _Second == Empty._Second && _Millisecond == Empty._Millisecond);
            }
        }

        //

        /// <summary>
        /// 获取表示此 DateTimeX 结构是否为公元时刻的布尔值。
        /// </summary>
        public bool IsChristianEra
        {
            get
            {
                return (!IsEmpty && _TotalMilliseconds == _ChristianTotalMilliseconds);
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构是否为时刻的最小可能值的布尔值。
        /// </summary>
        public bool IsMinValue
        {
            get
            {
                return (!IsEmpty && _TotalMilliseconds == _MinTotalMilliseconds);
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构是否为时刻的最大可能值的布尔值。
        /// </summary>
        public bool IsMaxValue
        {
            get
            {
                return (!IsEmpty && _TotalMilliseconds == _MaxTotalMilliseconds);
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构是否为一个公元后的时刻（含公元时刻）的布尔值。
        /// </summary>
        public bool IsAnnoDomini
        {
            get
            {
                return (!IsEmpty && _TotalMilliseconds >= _ChristianTotalMilliseconds);
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构是否为一个公元前的时刻（不含公元时刻）的布尔值。
        /// </summary>
        public bool IsBeforeChrist
        {
            get
            {
                return (!IsEmpty && _TotalMilliseconds < _ChristianTotalMilliseconds);
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构的年是否为闰年的布尔值。
        /// </summary>
        public bool IsLeap
        {
            get
            {
                return (!IsEmpty && IsLeapYear(_CheckYear(_Year, _CheckUtcOffset(_UtcOffset))));
            }
        }

        //

        /// <summary>
        /// 获取或设置此 DateTimeX 结构表示的时刻所在时区的标准时间与协调世界时（UTC）之间的时差的小时数。
        /// </summary>
        public double UtcOffset
        {
            get
            {
                return _CheckUtcOffset(_UtcOffset);
            }

            set
            {
                _CtorTotalMilliseconds(_TotalMilliseconds, value);
            }
        }

        /// <summary>
        /// 获取或设置此 DateTimeX 结构表示的时刻自公元时刻以来的总毫秒数。
        /// </summary>
        public decimal TotalMilliseconds
        {
            get
            {
                return _CheckTotalMilliseconds(_TotalMilliseconds);
            }

            set
            {
                _CtorTotalMilliseconds(value, _UtcOffset);
            }
        }

        /// <summary>
        /// 获取或设置此 DateTimeX 结构的年。
        /// </summary>
        public long Year
        {
            get
            {
                return _CheckYear(_Year, _CheckUtcOffset(_UtcOffset));
            }

            set
            {
                _CtorDateTime(value, _Month, _Day, _Hour, _Minute, _Second, _Millisecond, _UtcOffset);
            }
        }

        /// <summary>
        /// 获取或设置此 DateTimeX 结构的月。
        /// </summary>
        public int Month
        {
            get
            {
                return _CheckMonth(_Month);
            }

            set
            {
                _CtorDateTime(_Year, value, _Day, _Hour, _Minute, _Second, _Millisecond, _UtcOffset);
            }
        }

        /// <summary>
        /// 获取或设置此 DateTimeX 结构的日。
        /// </summary>
        public int Day
        {
            get
            {
                return _CheckDay(_CheckYear(_Year, _CheckUtcOffset(_UtcOffset)), _CheckMonth(_Month), _Day);
            }

            set
            {
                _CtorDateTime(_Year, _Month, value, _Hour, _Minute, _Second, _Millisecond, _UtcOffset);
            }
        }

        /// <summary>
        /// 获取或设置此 DateTimeX 结构的时。
        /// </summary>
        public int Hour
        {
            get
            {
                return _CheckHour(_Hour);
            }

            set
            {
                _CtorDateTime(_Year, _Month, _Day, value, _Minute, _Second, _Millisecond, _UtcOffset);
            }
        }

        /// <summary>
        /// 获取或设置此 DateTimeX 结构的分。
        /// </summary>
        public int Minute
        {
            get
            {
                return _CheckMinute(_Minute);
            }

            set
            {
                _CtorDateTime(_Year, _Month, _Day, _Hour, value, _Second, _Millisecond, _UtcOffset);
            }
        }

        /// <summary>
        /// 获取或设置此 DateTimeX 结构的秒。
        /// </summary>
        public int Second
        {
            get
            {
                return _CheckSecond(_Second);
            }

            set
            {
                _CtorDateTime(_Year, _Month, _Day, _Hour, _Minute, value, _Millisecond, _UtcOffset);
            }
        }

        /// <summary>
        /// 获取或设置此 DateTimeX 结构的毫秒。
        /// </summary>
        public int Millisecond
        {
            get
            {
                return _CheckMillisecond(_Millisecond);
            }

            set
            {
                _CtorDateTime(_Year, _Month, _Day, _Hour, _Minute, _Second, value, _UtcOffset);
            }
        }

        //

        /// <summary>
        /// 获取表示此 DateTimeX 结构的日期部分的 DateTimeX 结构的实例，时间值为午夜。
        /// </summary>
        public DateTimeX Date
        {
            get
            {
                return new DateTimeX(Year, Month, Day, UtcOffset);
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构的当天的时间的 TimeSpan 结构的实例，表示自午夜以来的时间间隔。
        /// </summary>
        public TimeSpan TimeOfDay
        {
            get
            {
                return new TimeSpan(0, Hour, Minute, Second, Millisecond);
            }
        }

        //

        /// <summary>
        /// 获取此 DateTimeX 结构表示的日期是该年的第几周（以 1 月 1 日所在周为第一周）。
        /// </summary>
        public int WeekOfYear
        {
            get
            {
                return ((DayOfYear - (2 - new DateTimeX(Year, UtcOffset).DayOfThisWeek)) / 7 + 1);
            }
        }

        /// <summary>
        /// 获取此 DateTimeX 结构表示的日期是该年的第几天（以 1 月 1 日为第一天）。
        /// </summary>
        public int DayOfYear
        {
            get
            {
                int[] DaysToMonth = (IsLeapYear(Year) ? _DaysToMonth366 : _DaysToMonth365);

                return (DaysToMonth[Month - 1] + Day);
            }
        }

        /// <summary>
        /// 获取此 DateTimeX 结构表示的时刻是该年的第几小时（以 1 月 1 日 0 时为第一小时）。
        /// </summary>
        public int HourOfYear
        {
            get
            {
                return ((DayOfYear - 1) * _HoursPerDay + Hour + 1);
            }
        }

        /// <summary>
        /// 获取此 DateTimeX 结构表示的时刻是该年的第几分钟（以 1 月 1 日 0 时 0 分为第一分钟）。
        /// </summary>
        public int MinuteOfYear
        {
            get
            {
                return ((DayOfYear - 1) * _MinutesPerDay + Hour * _MinutesPerHour + Minute + 1);
            }
        }

        /// <summary>
        /// 获取此 DateTimeX 结构表示的时刻是该年的第几秒（以 1 月 1 日 0 时 0 分 0 秒为第一秒）。
        /// </summary>
        public int SecondOfYear
        {
            get
            {
                return ((DayOfYear - 1) * _SecondsPerDay + Hour * _SecondsPerHour + Minute * _SecondsPerMinute + Second + 1);
            }
        }

        /// <summary>
        /// 获取此 DateTimeX 结构表示的时刻是该年的第几毫秒（以 1 月 1 日 0 时 0 分 0 秒 0 毫秒为第一毫秒）。
        /// </summary>
        public long MillisecondOfYear
        {
            get
            {
                return ((long)(DayOfYear - 1) * _MillisecondsPerDay + Hour * _MillisecondsPerHour + Minute * _MillisecondsPerMinute + Second * _MillisecondsPerSecond + Millisecond + 1);
            }
        }

        /// <summary>
        /// 获取此 DateTimeX 结构表示的日期是所在周的第几天（以周一为第一天）。
        /// </summary>
        public int DayOfThisWeek
        {
            get
            {
                long TotalDays = (long)(Math.Floor((TotalMilliseconds + (decimal)UtcOffset * _MillisecondsPerHour) / _MillisecondsPerDay));

                if (TotalDays >= 0)
                {
                    return (1 + (int)(TotalDays % 7));
                }
                else
                {
                    return (7 - (int)((-TotalDays - 1) % 7));
                }
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的日期是一周的某天的枚举。
        /// </summary>
        public DayOfWeek DayOfWeek
        {
            get
            {
                return (DayOfWeek)(DayOfThisWeek % 7);
            }
        }

        /// <summary>
        /// 获取此 DateTimeX 结构表示的时刻是该天的第几小时（以 0 时为第一小时）。
        /// </summary>
        public int HourOfDay
        {
            get
            {
                return (Hour + 1);
            }
        }

        /// <summary>
        /// 获取此 DateTimeX 结构表示的时刻是该天的第几分钟（以 0 时 0 分为第一分钟）。
        /// </summary>
        public int MinuteOfDay
        {
            get
            {
                return (Hour * _MinutesPerHour + Minute + 1);
            }
        }

        /// <summary>
        /// 获取此 DateTimeX 结构表示的时刻是该天的第几秒（以 0 时 0 分 0 秒为第一秒）。
        /// </summary>
        public int SecondOfDay
        {
            get
            {
                return (Hour * _SecondsPerHour + Minute * _SecondsPerMinute + Second + 1);
            }
        }

        /// <summary>
        /// 获取此 DateTimeX 结构表示的时刻是该天的第几毫秒（以 0 时 0 分 0 秒 0 毫秒为第一毫秒）。
        /// </summary>
        public int MillisecondOfDay
        {
            get
            {
                return (Hour * _MillisecondsPerHour + Minute * _MillisecondsPerMinute + Second * _MillisecondsPerSecond + Millisecond + 1);
            }
        }

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
                    default: return string.Empty;
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
                    default: return string.Empty;
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
                    default: return string.Empty;
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
                    default: return string.Empty;
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
                    default: return string.Empty;
                }
            }
        }

        //

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的日期是一周的某天的中文长字符串。
        /// </summary>
        public string WeekdayLongStringInChinese
        {
            get
            {
                switch (DayOfThisWeek)
                {
                    case 1: return "星期一";
                    case 2: return "星期二";
                    case 3: return "星期三";
                    case 4: return "星期四";
                    case 5: return "星期五";
                    case 6: return "星期六";
                    case 7: return "星期日";
                    default: return string.Empty;
                }
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的日期是一周的某天的中文短字符串。
        /// </summary>
        public string WeekdayShortStringInChinese
        {
            get
            {
                switch (DayOfThisWeek)
                {
                    case 1: return "周一";
                    case 2: return "周二";
                    case 3: return "周三";
                    case 4: return "周四";
                    case 5: return "周五";
                    case 6: return "周六";
                    case 7: return "周日";
                    default: return string.Empty;
                }
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的日期是一周的某天的英文长字符串。
        /// </summary>
        public string WeekdayLongStringInEnglish
        {
            get
            {
                switch (DayOfThisWeek)
                {
                    case 1: return "Monday";
                    case 2: return "Tuesday";
                    case 3: return "Wednesday";
                    case 4: return "Thursday";
                    case 5: return "Friday";
                    case 6: return "Saturday";
                    case 7: return "Sunday";
                    default: return string.Empty;
                }
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的日期是一周的某天的英文短字符串。
        /// </summary>
        public string WeekdayShortStringInEnglish
        {
            get
            {
                switch (DayOfThisWeek)
                {
                    case 1: return "Mon";
                    case 2: return "Tue";
                    case 3: return "Wed";
                    case 4: return "Thur";
                    case 5: return "Fri";
                    case 6: return "Sat";
                    case 7: return "Sun";
                    default: return string.Empty;
                }
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的日期是一周的某天的日文长字符串。
        /// </summary>
        public string WeekdayLongStringInJapanese
        {
            get
            {
                switch (DayOfThisWeek)
                {
                    case 1: return "月曜日";
                    case 2: return "火曜日";
                    case 3: return "水曜日";
                    case 4: return "木曜日";
                    case 5: return "金曜日";
                    case 6: return "土曜日";
                    case 7: return "日曜日";
                    default: return string.Empty;
                }
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构表示的日期是一周的某天的日文短字符串。
        /// </summary>
        public string WeekdayShortStringInJapanese
        {
            get
            {
                switch (DayOfThisWeek)
                {
                    case 1: return "月";
                    case 2: return "火";
                    case 3: return "水";
                    case 4: return "木";
                    case 5: return "金";
                    case 6: return "土";
                    case 7: return "日";
                    default: return string.Empty;
                }
            }
        }

        //

        /// <summary>
        /// 获取表示此 DateTimeX 结构日期部分的长字符串。
        /// </summary>
        public string DateLongString
        {
            get
            {
                return string.Concat((Year < 0 ? string.Concat("公元前", (-Year)) : Year.ToString()), "年", Month, "月", Day, "日");
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构日期部分的短字符串。
        /// </summary>
        public string DateShortString
        {
            get
            {
                return string.Concat((Year < 0 ? string.Concat("BC", (-Year)) : Year.ToString()), "/" + Month, "/" + Day);
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构时间部分的长字符串。
        /// </summary>
        public string TimeLongString
        {
            get
            {
                return string.Concat(Hour, ":", Minute.ToString("D2"), ":", Second.ToString("D2"), ".", Millisecond.ToString("D3"));
            }
        }

        /// <summary>
        /// 获取表示此 DateTimeX 结构时间部分的短字符串。
        /// </summary>
        public string TimeShortString
        {
            get
            {
                return string.Concat(Hour, ":", Minute.ToString("D2"));
            }
        }

        #endregion

        #region 静态属性

        /// <summary>
        /// 获取以本地时区表示的此计算机上的当前日期与时间的 DateTimeX 结构的实例。
        /// </summary>
        public static DateTimeX Now
        {
            get
            {
                return new DateTimeX(DateTime.Now, _LocalUtcOffset);
            }
        }

        /// <summary>
        /// 获取以协调世界时（UTC）表示的此计算机上的当前日期与时间的 DateTimeX 结构的实例。
        /// </summary>
        public static DateTimeX UtcNow
        {
            get
            {
                return new DateTimeX(DateTime.Now, _Utc);
            }
        }

        /// <summary>
        /// 获取以本地时区表示的此计算机上的当前日期的 DateTimeX 结构的实例。
        /// </summary>
        public static DateTimeX Today
        {
            get
            {
                return new DateTimeX(DateTime.Now.Date, _LocalUtcOffset);
            }
        }

        /// <summary>
        /// 获取以协调世界时（UTC）表示的此计算机上的当前日期的 DateTimeX 结构的实例。
        /// </summary>
        public static DateTimeX UtcToday
        {
            get
            {
                return new DateTimeX(DateTime.Now.Date, _Utc);
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 DateTimeX 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>布尔值，表示此 DateTimeX 结构是否与指定的对象相等。</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is DateTimeX))
            {
                return false;
            }
            else if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals((DateTimeX)obj);
        }

        /// <summary>
        /// 返回此 DateTimeX 结构的哈希代码。
        /// </summary>
        /// <returns>32 位整数，表示此 DateTimeX 结构的哈希代码。</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 DateTimeX 结构转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 DateTimeX 结构的字符串形式。</returns>
        public override string ToString()
        {
            return string.Concat((Year < 0 ? string.Concat("BC", (-Year)) : Year.ToString()), "/", Month, "/", Day, " ", Hour, ":", Minute.ToString("D2"), ":", Second.ToString("D2"));
        }

        //

        /// <summary>
        /// 判断此 DateTimeX 结构是否与指定的 DateTimeX 结构相等。
        /// </summary>
        /// <param name="dateTime">用于比较的 DateTimeX 结构。</param>
        /// <returns>布尔值，表示此 DateTimeX 结构是否与指定的 DateTimeX 结构相等。</returns>
        public bool Equals(DateTimeX dateTime)
        {
            return (_UtcOffset.Equals(dateTime._UtcOffset) && _TotalMilliseconds == dateTime._TotalMilliseconds && _Year == dateTime._Year && _Month == dateTime._Month && _Day == dateTime._Day && _Hour == dateTime._Hour && _Minute == dateTime._Minute && _Second == dateTime._Second && _Millisecond == dateTime._Millisecond);
        }

        //

        /// <summary>
        /// 将此 DateTimeX 结构与指定的对象进行次序比较。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>32 位整数，表示将此 DateTimeX 结构与指定的对象进行次序比较得到的结果。</returns>
        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is DateTimeX))
            {
                return 1;
            }
            else if (object.ReferenceEquals(this, obj))
            {
                return 0;
            }

            return CompareTo((DateTimeX)obj);
        }

        /// <summary>
        /// 将此 DateTimeX 结构与指定的 DateTimeX 结构进行次序比较。
        /// </summary>
        /// <param name="dateTime">用于比较的 DateTimeX 结构。</param>
        /// <returns>32 位整数，表示将此 DateTimeX 结构与指定的 DateTimeX 结构进行次序比较得到的结果。</returns>
        public int CompareTo(DateTimeX dateTime)
        {
            return TotalMilliseconds.CompareTo(dateTime.TotalMilliseconds);
        }

        //

        /// <summary>
        /// 返回将此 DateTimeX 结构与 TimeSpan 结构相加得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="timeSpan">TimeSpan 结构，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构与 TimeSpan 结构相加得到的结果。</returns>
        public DateTimeX Add(TimeSpan timeSpan)
        {
            return AddMilliseconds(timeSpan.TotalMilliseconds);
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干年得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="years">64 位整数表示的年，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干年得到的结果。</returns>
        public DateTimeX AddYears(long years)
        {
            if ((double)Year + years - 1 < long.MinValue)
            {
                return _ThisMinValue;
            }
            else if ((double)Year + years + 1 > long.MaxValue)
            {
                return _ThisMaxValue;
            }
            else
            {
                long NewYear = Year + years;

                if (Year < 0 && NewYear >= 0)
                {
                    NewYear += 1;
                }
                else if (Year > 0 && NewYear <= 0)
                {
                    NewYear -= 1;
                }

                DateTimeX ThisMinValue = _ThisMinValue;

                if (NewYear < ThisMinValue.Year)
                {
                    return ThisMinValue;
                }
                else
                {
                    DateTimeX ThisMaxValue = _ThisMaxValue;

                    if (NewYear > ThisMaxValue.Year)
                    {
                        return ThisMaxValue;
                    }
                }

                return new DateTimeX(NewYear, Month, Day, Hour, Minute, Second, Millisecond, UtcOffset);
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干个月得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="months">64 位整数表示的月，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干个月得到的结果。</returns>
        public DateTimeX AddMonths(long months)
        {
            if ((double)Year + (months / _MonthsPerYear) - 2 < long.MinValue)
            {
                return _ThisMinValue;
            }
            else if ((double)Year + (months / _MonthsPerYear) + 2 > long.MaxValue)
            {
                return _ThisMaxValue;
            }
            else
            {
                long NewYear = Year + (months / _MonthsPerYear);
                int NewMonth = Month + (int)(months % _MonthsPerYear);

                if (NewMonth < _MinMonth)
                {
                    NewYear -= 1;
                    NewMonth += _MonthsPerYear;
                }
                else if (NewMonth > _MaxMonth)
                {
                    NewYear += 1;
                    NewMonth -= _MonthsPerYear;
                }

                if (Year < 0 && NewYear >= 0)
                {
                    NewYear += 1;
                }
                else if (Year > 0 && NewYear <= 0)
                {
                    NewYear -= 1;
                }

                DateTimeX ThisMinValue = _ThisMinValue;

                if (NewYear < ThisMinValue.Year || (NewYear == ThisMinValue.Year && NewMonth < ThisMinValue.Month))
                {
                    return ThisMinValue;
                }
                else
                {
                    DateTimeX ThisMaxValue = _ThisMaxValue;

                    if (NewYear > ThisMaxValue.Year || (NewYear == ThisMaxValue.Year && NewMonth > ThisMaxValue.Month))
                    {
                        return ThisMaxValue;
                    }
                }

                return new DateTimeX(NewYear, NewMonth, Day, Hour, Minute, Second, Millisecond, UtcOffset);
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干周得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="weeks">双精度浮点数表示的周，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干周得到的结果。</returns>
        public DateTimeX AddWeeks(double weeks)
        {
            if (InternalMethod.IsNaNOrInfinity(weeks))
            {
                return this;
            }

            if ((double)TotalMilliseconds + weeks * _MillisecondsPerWeek < (double)decimal.MinValue)
            {
                return _ThisMinValue;
            }
            else if ((double)TotalMilliseconds + weeks * _MillisecondsPerWeek > (double)decimal.MaxValue)
            {
                return _ThisMaxValue;
            }
            else
            {
                decimal NewTotalMS = TotalMilliseconds + (decimal)weeks * _MillisecondsPerWeek;

                if (NewTotalMS < _MinTotalMilliseconds)
                {
                    return _ThisMinValue;
                }
                else if (NewTotalMS > _MaxTotalMilliseconds)
                {
                    return _ThisMaxValue;
                }

                return new DateTimeX(NewTotalMS, UtcOffset);
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干天得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="days">双精度浮点数表示的天，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干天得到的结果。</returns>
        public DateTimeX AddDays(double days)
        {
            if (InternalMethod.IsNaNOrInfinity(days))
            {
                return this;
            }

            if ((double)TotalMilliseconds + days * _MillisecondsPerDay < (double)decimal.MinValue)
            {
                return _ThisMinValue;
            }
            else if ((double)TotalMilliseconds + days * _MillisecondsPerDay > (double)decimal.MaxValue)
            {
                return _ThisMaxValue;
            }
            else
            {
                decimal NewTotalMS = TotalMilliseconds + (decimal)days * _MillisecondsPerDay;

                if (NewTotalMS < _MinTotalMilliseconds)
                {
                    return _ThisMinValue;
                }
                else if (NewTotalMS > _MaxTotalMilliseconds)
                {
                    return _ThisMaxValue;
                }

                return new DateTimeX(NewTotalMS, UtcOffset);
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干小时得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="hours">双精度浮点数表示的小时，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干小时得到的结果。</returns>
        public DateTimeX AddHours(double hours)
        {
            if (InternalMethod.IsNaNOrInfinity(hours))
            {
                return this;
            }

            if ((double)TotalMilliseconds + hours * _MillisecondsPerHour < (double)decimal.MinValue)
            {
                return _ThisMinValue;
            }
            else if ((double)TotalMilliseconds + hours * _MillisecondsPerHour > (double)decimal.MaxValue)
            {
                return _ThisMaxValue;
            }
            else
            {
                decimal NewTotalMS = TotalMilliseconds + (decimal)hours * _MillisecondsPerHour;

                if (NewTotalMS < _MinTotalMilliseconds)
                {
                    return _ThisMinValue;
                }
                else if (NewTotalMS > _MaxTotalMilliseconds)
                {
                    return _ThisMaxValue;
                }

                return new DateTimeX(NewTotalMS, UtcOffset);
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干分钟得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="minutes">双精度浮点数表示的分钟，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干分钟得到的结果。</returns>
        public DateTimeX AddMinutes(double minutes)
        {
            if (InternalMethod.IsNaNOrInfinity(minutes))
            {
                return this;
            }

            if ((double)TotalMilliseconds + minutes * _MillisecondsPerMinute < (double)decimal.MinValue)
            {
                return _ThisMinValue;
            }
            else if ((double)TotalMilliseconds + minutes * _MillisecondsPerMinute > (double)decimal.MaxValue)
            {
                return _ThisMaxValue;
            }
            else
            {
                decimal NewTotalMS = TotalMilliseconds + (decimal)minutes * _MillisecondsPerMinute;

                if (NewTotalMS < _MinTotalMilliseconds)
                {
                    return _ThisMinValue;
                }
                else if (NewTotalMS > _MaxTotalMilliseconds)
                {
                    return _ThisMaxValue;
                }

                return new DateTimeX(NewTotalMS, UtcOffset);
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干秒得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="seconds">双精度浮点数表示的秒，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干秒得到的结果。</returns>
        public DateTimeX AddSeconds(double seconds)
        {
            if (InternalMethod.IsNaNOrInfinity(seconds))
            {
                return this;
            }

            if ((double)TotalMilliseconds + seconds * _MillisecondsPerSecond < (double)decimal.MinValue)
            {
                return _ThisMinValue;
            }
            else if ((double)TotalMilliseconds + seconds * _MillisecondsPerSecond > (double)decimal.MaxValue)
            {
                return _ThisMaxValue;
            }
            else
            {
                decimal NewTotalMS = TotalMilliseconds + (decimal)seconds * _MillisecondsPerSecond;

                if (NewTotalMS < _MinTotalMilliseconds)
                {
                    return _ThisMinValue;
                }
                else if (NewTotalMS > _MaxTotalMilliseconds)
                {
                    return _ThisMaxValue;
                }

                return new DateTimeX(NewTotalMS, UtcOffset);
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干毫秒得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="milliseconds">十进制数表示的毫秒，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干毫秒得到的结果。</returns>
        public DateTimeX AddMilliseconds(decimal milliseconds)
        {
            if ((double)TotalMilliseconds + (double)milliseconds < (double)decimal.MinValue)
            {
                return _ThisMinValue;
            }
            else if ((double)TotalMilliseconds + (double)milliseconds > (double)decimal.MaxValue)
            {
                return _ThisMaxValue;
            }
            else
            {
                decimal NewTotalMS = TotalMilliseconds + milliseconds;

                if (NewTotalMS < _MinTotalMilliseconds)
                {
                    return _ThisMinValue;
                }
                else if (NewTotalMS > _MaxTotalMilliseconds)
                {
                    return _ThisMaxValue;
                }

                return new DateTimeX(NewTotalMS, UtcOffset);
            }
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构加上若干毫秒得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="milliseconds">双精度浮点数表示的毫秒，用于相加到此 DateTimeX 结构。</param>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构加上若干毫秒得到的结果。</returns>
        public DateTimeX AddMilliseconds(double milliseconds)
        {
            if (InternalMethod.IsNaNOrInfinity(milliseconds))
            {
                return this;
            }

            if ((double)TotalMilliseconds + milliseconds < (double)decimal.MinValue)
            {
                return _ThisMinValue;
            }
            else if ((double)TotalMilliseconds + milliseconds > (double)decimal.MaxValue)
            {
                return _ThisMaxValue;
            }
            else
            {
                decimal NewTotalMS = TotalMilliseconds + (decimal)milliseconds;

                if (NewTotalMS < _MinTotalMilliseconds)
                {
                    return _ThisMinValue;
                }
                else if (NewTotalMS > _MaxTotalMilliseconds)
                {
                    return _ThisMaxValue;
                }

                return new DateTimeX(NewTotalMS, UtcOffset);
            }
        }

        //

        /// <summary>
        /// 返回将此 DateTimeX 结构转换为以本地时区表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构转换为以本地时区表示的 DateTimeX 结构的新实例。</returns>
        public DateTimeX ToLocalTime()
        {
            return new DateTimeX(TotalMilliseconds, _LocalUtcOffset);
        }

        /// <summary>
        /// 返回将此 DateTimeX 结构转换为以协调世界时（UTC）表示的 DateTimeX 结构的新实例。
        /// </summary>
        /// <returns>DateTimeX 结构，表示将此 DateTimeX 结构转换为以协调世界时（UTC）表示的 DateTimeX 结构的新实例。</returns>
        public DateTimeX ToUniversalTime()
        {
            return new DateTimeX(TotalMilliseconds, _Utc);
        }

        //

        /// <summary>
        /// 返回表示此 DateTimeX 结构的长日期字符串。
        /// </summary>
        /// <returns>字符串，表示此 DateTimeX 结构的长日期字符串形式。</returns>
        public string ToLongDateString()
        {
            return DateLongString;
        }

        /// <summary>
        /// 返回表示此 DateTimeX 结构的短日期字符串。
        /// </summary>
        /// <returns>字符串，表示此 DateTimeX 结构的短日期字符串形式。</returns>
        public string ToShortDateString()
        {
            return DateShortString;
        }

        /// <summary>
        /// 返回表示此 DateTimeX 结构的长时间字符串。
        /// </summary>
        /// <returns>字符串，表示此 DateTimeX 结构的长时间字符串形式。</returns>
        public string ToLongTimeString()
        {
            return TimeLongString;
        }

        /// <summary>
        /// 返回表示此 DateTimeX 结构的短时间字符串。
        /// </summary>
        /// <returns>字符串，表示此 DateTimeX 结构的短时间字符串形式。</returns>
        public string ToShortTimeString()
        {
            return TimeShortString;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断两个 DateTimeX 结构是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 DateTimeX 结构。</param>
        /// <param name="right">用于比较的第二个 DateTimeX 结构。</param>
        /// <returns>布尔值，表示两个 DateTimeX 结构是否相等。</returns>
        public static bool Equals(DateTimeX left, DateTimeX right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            else if ((object)left == null || (object)right == null)
            {
                return false;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return true;
            }

            return left.Equals(right);
        }

        //

        /// <summary>
        /// 比较两个 DateTimeX 结构的次序。
        /// </summary>
        /// <param name="left">用于比较的第一个 DateTimeX 结构。</param>
        /// <param name="right">用于比较的第二个 DateTimeX 结构。</param>
        /// <returns>32 位整数，表示将两个 DateTimeX 结构进行次序比较得到的结果。</returns>
        public static int Compare(DateTimeX left, DateTimeX right)
        {
            if ((object)left == null && (object)right == null)
            {
                return 0;
            }
            else if ((object)left == null)
            {
                return -1;
            }
            else if ((object)right == null)
            {
                return 1;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return 0;
            }

            return left.CompareTo(right);
        }

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
                return false;
            }

            if (year < 0)
            {
                year = -year - 1;
            }

            return (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0) && (year % 3200 != 0 || year % 172800 == 0));
        }

        //

        /// <summary>
        /// 返回指定年的天数。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <returns>32 位整数，表示指定年的天数。</returns>
        public static int DaysInYear(long year)
        {
            if (year != 0)
            {
                return (IsLeapYear(year) ? 366 : 365);
            }

            return -1;
        }

        /// <summary>
        /// 返回指定年的指定月的天数。
        /// </summary>
        /// <param name="year">64 位整数表示的年。</param>
        /// <param name="month">32 位整数表示的月。</param>
        /// <returns>32 位整数，表示指定年的指定月的天数。</returns>
        public static int DaysInMonth(long year, int month)
        {
            if (year != 0 && (month >= _MinMonth && month <= _MaxMonth))
            {
                int[] DaysToMonth = (IsLeapYear(year) ? _DaysToMonth366 : _DaysToMonth365);

                return (DaysToMonth[month] - DaysToMonth[month - 1]);
            }

            return -1;
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 DateTimeX 结构是否表示相同的时刻。
        /// </summary>
        /// <param name="left">运算符左侧比较的 DateTimeX 结构。</param>
        /// <param name="right">运算符右侧比较的 DateTimeX 结构。</param>
        /// <returns>布尔值，表示两个 Complex。</returns>
        public static bool operator ==(DateTimeX left, DateTimeX right)
        {
            if (left.IsEmpty || right.IsEmpty)
            {
                return false;
            }

            return (left.TotalMilliseconds == right.TotalMilliseconds);
        }

        /// <summary>
        /// 判断两个 DateTimeX 结构是否表示不同的时刻。
        /// </summary>
        /// <param name="left">运算符左侧比较的 DateTimeX 结构。</param>
        /// <param name="right">运算符右侧比较的 DateTimeX 结构。</param>
        /// <returns>布尔值，表示两个 DateTimeX 结构是否表示不同的时刻。</returns>
        public static bool operator !=(DateTimeX left, DateTimeX right)
        {
            if (left.IsEmpty || right.IsEmpty)
            {
                return true;
            }

            return (left.TotalMilliseconds != right.TotalMilliseconds);
        }

        /// <summary>
        /// 判断两个 DateTimeX 结构是否前者早于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 DateTimeX 结构。</param>
        /// <param name="right">运算符右侧比较的 DateTimeX 结构。</param>
        /// <returns>布尔值，表示两个 DateTimeX 结构是否前者早于后者。</returns>
        public static bool operator <(DateTimeX left, DateTimeX right)
        {
            if (left.IsEmpty || right.IsEmpty)
            {
                return false;
            }

            return (left.TotalMilliseconds < right.TotalMilliseconds);
        }

        /// <summary>
        /// 判断两个 DateTimeX 结构是否前者晚于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 DateTimeX 结构。</param>
        /// <param name="right">运算符右侧比较的 DateTimeX 结构。</param>
        /// <returns>布尔值，表示两个 DateTimeX 结构是否前者晚于后者。</returns>
        public static bool operator >(DateTimeX left, DateTimeX right)
        {
            if (left.IsEmpty || right.IsEmpty)
            {
                return false;
            }

            return (left.TotalMilliseconds > right.TotalMilliseconds);
        }

        /// <summary>
        /// 判断两个 DateTimeX 结构是否前者早于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 DateTimeX 结构。</param>
        /// <param name="right">运算符右侧比较的 DateTimeX 结构。</param>
        /// <returns>布尔值，表示两个 DateTimeX 结构是否前者早于或等于后者。</returns>
        public static bool operator <=(DateTimeX left, DateTimeX right)
        {
            if (left.IsEmpty || right.IsEmpty)
            {
                return false;
            }

            return (left.TotalMilliseconds <= right.TotalMilliseconds);
        }

        /// <summary>
        /// 判断两个 DateTimeX 结构是否前者晚于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 DateTimeX 结构。</param>
        /// <param name="right">运算符右侧比较的 DateTimeX 结构。</param>
        /// <returns>布尔值，表示两个 DateTimeX 结构是否前者晚于或等于后者。</returns>
        public static bool operator >=(DateTimeX left, DateTimeX right)
        {
            if (left.IsEmpty || right.IsEmpty)
            {
                return false;
            }

            return (left.TotalMilliseconds >= right.TotalMilliseconds);
        }

        //

        /// <summary>
        /// 返回将 DateTimeX 结构与 TimeSpan 结构相加得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="dateTime">DateTimeX 结构，表示被加数。</param>
        /// <param name="timeSpan">TimeSpan 结构，表示加数。</param>
        /// <returns>DateTimeX 结构，表示将 DateTimeX 结构与 TimeSpan 结构相加得到的结果。</returns>
        public static DateTimeX operator +(DateTimeX dateTime, TimeSpan timeSpan)
        {
            return dateTime.AddMilliseconds(timeSpan.TotalMilliseconds);
        }

        /// <summary>
        /// 返回将 DateTimeX 结构与 TimeSpan 结构相减得到的 DateTimeX 结构的新实例。
        /// </summary>
        /// <param name="dateTime">DateTimeX 结构，表示被减数。</param>
        /// <param name="timeSpan">TimeSpan 结构，表示减数。</param>
        /// <returns>DateTimeX 结构，表示将 DateTimeX 结构与 TimeSpan 结构相减得到的结果。</returns>
        public static DateTimeX operator -(DateTimeX dateTime, TimeSpan timeSpan)
        {
            return dateTime.AddMilliseconds(-(timeSpan.TotalMilliseconds));
        }

        //

        /// <summary>
        /// 将指定的 DateTime 结构隐式转换为 DateTimeX 结构。
        /// </summary>
        /// <param name="dateTime">用于转换的 DateTime 结构。</param>
        /// <returns>DateTimeX 结构，表示隐式转换的结果。</returns>
        public static implicit operator DateTimeX(DateTime dateTime)
        {
            return new DateTimeX(dateTime);
        }

        #endregion
    }
}