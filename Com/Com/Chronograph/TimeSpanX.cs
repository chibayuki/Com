/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.TimespanX
Version 20.10.27.1900

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
    /// 表示一个时间间隔。
    /// </summary>
    public struct TimeSpanX : IEquatable<TimeSpanX>, IComparable, IComparable<TimeSpanX>
    {
        #region 非公开成员

        private const decimal _MinTotalMilliseconds = -796899343984252629811200000M, _MaxTotalMilliseconds = 796899343984252629811199999M, _ZeroTotalMilliseconds = 0M; // 时间间隔的总毫秒数的最小值、最大值与零时间间隔的值，最小值等于由 System.Int64.MinValue 表示的总天数对应的毫秒数，最大值等于由 (System.Int64.MaxValue + 1) 表示的总天数对应的毫秒数减 1。

        private const int _MillisecondsPerSecond = 1000; // 每秒的毫秒数。
        private const int _MillisecondsPerMinute = 60000; // 每分钟的毫秒数。
        private const int _MillisecondsPerHour = 3600000; // 每小时的毫秒数。
        private const int _MillisecondsPerDay = 86400000; // 每天的毫秒数。
        private const int _SecondsPerMinute = 60; // 每分钟的秒数。
        private const int _MinutesPerHour = 60; // 每小时的分钟数。
        private const int _HoursPerDay = 24; // 每天的小时数。

        //

        // 对时间间隔的总毫秒数的值进行合法性检查，返回合法的值。
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

        //

        private decimal _TotalMilliseconds; // 时间间隔的总毫秒数。

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用十进制浮点数表示的时间间隔的总毫秒数初始化 TimespanX 结构的新实例。
        /// </summary>
        /// <param name="totalMilliseconds">十进制浮点数表示的时间间隔的总毫秒数。</param>
        public TimeSpanX(decimal totalMilliseconds) => _TotalMilliseconds = _CheckTotalMilliseconds(totalMilliseconds);

        /// <summary>
        /// 使用指定的小时数、分钟数与秒数初始化 TimespanX 结构的新实例。
        /// </summary>
        /// <param name="hours">32 位整数表示的小时数。</param>
        /// <param name="minutes">32 位整数表示的分钟数。</param>
        /// <param name="seconds">32 位整数表示的秒数。</param>
        public TimeSpanX(int hours, int minutes, int seconds) => _TotalMilliseconds = _CheckTotalMilliseconds(hours * _MillisecondsPerHour + minutes * _MillisecondsPerMinute + seconds * _MillisecondsPerSecond);

        /// <summary>
        /// 使用指定的天数、小时数、分钟数与秒数初始化 TimespanX 结构的新实例。
        /// </summary>
        /// <param name="days">32 位整数表示的天数。</param>
        /// <param name="hours">32 位整数表示的小时数。</param>
        /// <param name="minutes">32 位整数表示的分钟数。</param>
        /// <param name="seconds">32 位整数表示的秒数。</param>
        public TimeSpanX(long days, int hours, int minutes, int seconds) => _TotalMilliseconds = _CheckTotalMilliseconds((decimal)days * _MillisecondsPerDay + hours * _MillisecondsPerHour + minutes * _MillisecondsPerMinute + seconds * _MillisecondsPerSecond);

        /// <summary>
        /// 使用指定的天数、小时数、分钟数、秒数与毫秒数初始化 TimespanX 结构的新实例。
        /// </summary>
        /// <param name="days">32 位整数表示的天数。</param>
        /// <param name="hours">32 位整数表示的小时数。</param>
        /// <param name="minutes">32 位整数表示的分钟数。</param>
        /// <param name="seconds">32 位整数表示的秒数。</param>
        /// <param name="milliseconds">32 位整数表示的毫秒数。</param>
        public TimeSpanX(long days, int hours, int minutes, int seconds, int milliseconds) => _TotalMilliseconds = _CheckTotalMilliseconds((decimal)days * _MillisecondsPerDay + hours * _MillisecondsPerHour + minutes * _MillisecondsPerMinute + seconds * _MillisecondsPerSecond + milliseconds);

        /// <summary>
        /// 使用 TimeSpan 结构初始化 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="timeSpan">TimeSpan 结构。</param>
        public TimeSpanX(TimeSpan timeSpan) => _TotalMilliseconds = _CheckTotalMilliseconds((decimal)timeSpan.TotalMilliseconds);

        #endregion

        #region 字段

        /// <summary>
        /// 表示零时间间隔的 TimespanX 结构的实例。
        /// </summary>
        public static readonly TimeSpanX Zero = new TimeSpanX(_ZeroTotalMilliseconds);

        /// <summary>
        /// 表示时间间隔的最小可能值的 TimespanX 结构的实例。
        /// </summary>
        public static readonly TimeSpanX MinValue = new TimeSpanX(_MinTotalMilliseconds);

        /// <summary>
        /// 表示时间间隔的最大可能值的 TimespanX 结构的实例。
        /// </summary>
        public static readonly TimeSpanX MaxValue = new TimeSpanX(_MaxTotalMilliseconds);

        #endregion

        #region 属性

        /// <summary>
        /// 获取表示此 TimespanX 结构是否为零时间间隔的布尔值。
        /// </summary>
        public bool IsZero => _TotalMilliseconds == _ZeroTotalMilliseconds;

        /// <summary>
        /// 获取表示此 TimespanX 结构是否为时间间隔的最小可能值的布尔值。
        /// </summary>
        public bool IsMinValue => _TotalMilliseconds == _MinTotalMilliseconds;

        /// <summary>
        /// 获取表示此 TimespanX 结构是否为时间间隔的最大可能值的布尔值。
        /// </summary>
        public bool IsMaxValue => _TotalMilliseconds == _MaxTotalMilliseconds;

        //

        /// <summary>
        /// 获取此 TimespanX 结构的天数。
        /// </summary>
        public long Days => (long)(_TotalMilliseconds / _MillisecondsPerDay);

        /// <summary>
        /// 获取此 TimespanX 结构的小时数。
        /// </summary>
        public int Hours => (int)(_TotalMilliseconds / _MillisecondsPerHour % _HoursPerDay);

        /// <summary>
        /// 获取此 TimespanX 结构的分钟数。
        /// </summary>
        public int Minutes => (int)(_TotalMilliseconds / _MillisecondsPerMinute % _MinutesPerHour);

        /// <summary>
        /// 获取此 TimespanX 结构的秒数。
        /// </summary>
        public int Seconds => (int)(_TotalMilliseconds / _MillisecondsPerSecond % _SecondsPerMinute);

        /// <summary>
        /// 获取此 TimespanX 结构的毫秒数。
        /// </summary>
        public int Milliseconds => (int)(_TotalMilliseconds % _MillisecondsPerSecond);

        //

        /// <summary>
        /// 获取或设置此 TimespanX 结构的总天数。
        /// </summary>
        public decimal TotalDays
        {
            get => _TotalMilliseconds / _MillisecondsPerDay;
            set => _TotalMilliseconds = _CheckTotalMilliseconds(value * _MillisecondsPerDay);
        }

        /// <summary>
        /// 获取或设置此 TimespanX 结构的总小时数。
        /// </summary>
        public decimal TotalHours
        {
            get => _TotalMilliseconds / _MillisecondsPerHour;
            set => _TotalMilliseconds = _CheckTotalMilliseconds(value * _MillisecondsPerHour);
        }

        /// <summary>
        /// 获取或设置此 TimespanX 结构的总分钟数。
        /// </summary>
        public decimal TotalMinutes
        {
            get => _TotalMilliseconds / _MillisecondsPerMinute;
            set => _TotalMilliseconds = _CheckTotalMilliseconds(value * _MillisecondsPerMinute);
        }

        /// <summary>
        /// 获取或设置此 TimespanX 结构的总秒数。
        /// </summary>
        public decimal TotalSeconds
        {
            get => _TotalMilliseconds / _MillisecondsPerSecond;
            set => _TotalMilliseconds = _CheckTotalMilliseconds(value * _MillisecondsPerSecond);
        }

        /// <summary>
        /// 获取或设置此 TimespanX 结构的总毫秒数。
        /// </summary>
        public decimal TotalMilliseconds
        {
            get => _TotalMilliseconds;
            set => _TotalMilliseconds = _CheckTotalMilliseconds(value);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 TimeSpanX 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>布尔值，表示此 TimeSpanX 结构是否与指定的对象相等。</returns>
        public override bool Equals(object obj)
        {
            if (obj is null || !(obj is TimeSpanX))
            {
                return false;
            }
            else
            {
                return Equals((TimeSpanX)obj);
            }
        }

        /// <summary>
        /// 返回此 TimeSpanX 结构的哈希代码。
        /// </summary>
        /// <returns>32 位整数，表示此 TimeSpanX 结构的哈希代码。</returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// 将此 TimeSpanX 结构转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 TimeSpanX 结构的字符串形式。</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (_TotalMilliseconds < 0)
            {
                sb.Append('-');
            }

            long days = Math.Abs(Days);

            if (days != 0)
            {
                sb.Append(days);
                sb.Append(':');
            }

            int hours = Math.Abs(Hours);

            if (days == 0)
            {
                sb.Append(hours);
            }
            else
            {
                sb.Append(hours.ToString("D2"));
            }

            sb.Append(':');

            sb.Append(Math.Abs(Minutes).ToString("D2"));
            sb.Append(':');

            sb.Append(Math.Abs(Seconds).ToString("D2"));

            int milliseconds = Math.Abs(Milliseconds);

            if (milliseconds != 0)
            {
                sb.Append('.');
                sb.Append(milliseconds.ToString("D3"));
            }

            return sb.ToString();
        }

        //

        /// <summary>
        /// 判断此 TimeSpanX 结构是否与指定的 TimeSpanX 结构相等。
        /// </summary>
        /// <param name="timeSpan">用于比较的 TimeSpanX 结构。</param>
        /// <returns>布尔值，表示此 TimeSpanX
        /// 结构是否与指定的 TimeSpanX 结构相等。</returns>
        public bool Equals(TimeSpanX timeSpan) => _TotalMilliseconds.Equals(timeSpan._TotalMilliseconds);

        //

        /// <summary>
        /// 将此 TimeSpanX 结构与指定的对象进行次序比较。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>32 位整数，表示将此 TimeSpanX 结构与指定的对象进行次序比较得到的结果。</returns>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (!(obj is TimeSpanX))
            {
                throw new ArgumentException();
            }
            else
            {
                return CompareTo((TimeSpanX)obj);
            }
        }

        /// <summary>
        /// 将此 TimeSpanX 结构与指定的 TimeSpanX 结构进行次序比较。
        /// </summary>
        /// <param name="timeSpan">用于比较的 TimeSpanX 结构。</param>
        /// <returns>32 位整数，表示将此 TimeSpanX 结构与指定的 TimeSpanX 结构进行次序比较得到的结果。</returns>
        public int CompareTo(TimeSpanX timeSpan) => _TotalMilliseconds.CompareTo(timeSpan._TotalMilliseconds);

        //

        /// <summary>
        /// 返回将此 TimeSpanX 结构与 TimeSpanX 结构相加得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="timeSpan">TimeSpanX 结构，用于相加到此 TimeSpanX 结构。</param>
        /// <returns>TimeSpanX 结构，表示将此 TimeSpanX 结构与 TimeSpanX 结构相加得到的结果。</returns>
        public TimeSpanX Add(TimeSpanX timeSpan) => AddMilliseconds(timeSpan._TotalMilliseconds);

        /// <summary>
        /// 返回将此 TimeSpanX 结构与 TimeSpan 结构相加得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="timeSpan">TimeSpan 结构，用于相加到此 TimeSpanX 结构。</param>
        /// <returns>TimeSpanX 结构，表示将此 TimeSpanX 结构与 TimeSpan 结构相加得到的结果。</returns>
        public TimeSpanX Add(TimeSpan timeSpan) => AddMilliseconds(timeSpan.TotalMilliseconds);

        /// <summary>
        /// 返回将此 TimeSpanX 结构加上若干天得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="days">十进制浮点数表示的天数，用于相加到此 TimeSpanX 结构。</param>
        /// <returns>TimeSpanX 结构，表示将此 TimeSpanX 结构加上若干天得到的结果。</returns>
        public TimeSpanX AddDays(decimal days)
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
                        return new TimeSpanX(newTotalMS);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 TimeSpanX 结构加上若干天得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="days">双精度浮点数表示的天数，用于相加到此 TimeSpanX 结构。</param>
        /// <returns>TimeSpanX 结构，表示将此 TimeSpanX 结构加上若干天得到的结果。</returns>
        public TimeSpanX AddDays(double days)
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
                        return new TimeSpanX(newTotalMS);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 TimeSpanX 结构加上若干小时得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="hours">十进制浮点数表示的小时数，用于相加到此 TimeSpanX 结构。</param>
        /// <returns>TimeSpanX 结构，表示将此 TimeSpanX 结构加上若干小时得到的结果。</returns>
        public TimeSpanX AddHours(decimal hours)
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
                        return new TimeSpanX(newTotalMS);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 TimeSpanX 结构加上若干小时得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="hours">双精度浮点数表示的小时数，用于相加到此 TimeSpanX 结构。</param>
        /// <returns>TimeSpanX 结构，表示将此 TimeSpanX 结构加上若干小时得到的结果。</returns>
        public TimeSpanX AddHours(double hours)
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
                        return new TimeSpanX(newTotalMS);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 TimeSpanX 结构加上若干分钟得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="minutes">十进制浮点数表示的分钟数，用于相加到此 TimeSpanX 结构。</param>
        /// <returns>TimeSpanX 结构，表示将此 TimeSpanX 结构加上若干分钟得到的结果。</returns>
        public TimeSpanX AddMinutes(decimal minutes)
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
                        return new TimeSpanX(newTotalMS);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 TimeSpanX 结构加上若干分钟得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="minutes">双精度浮点数表示的分钟数，用于相加到此 TimeSpanX 结构。</param>
        /// <returns>TimeSpanX 结构，表示将此 TimeSpanX 结构加上若干分钟得到的结果。</returns>
        public TimeSpanX AddMinutes(double minutes)
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
                        return new TimeSpanX(newTotalMS);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 TimeSpanX 结构加上若干秒得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="seconds">十进制浮点数表示的秒数，用于相加到此 TimeSpanX 结构。</param>
        /// <returns>TimeSpanX 结构，表示将此 TimeSpanX 结构加上若干秒得到的结果。</returns>
        public TimeSpanX AddSeconds(decimal seconds)
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
                        return new TimeSpanX(newTotalMS);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 TimeSpanX 结构加上若干秒得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="seconds">双精度浮点数表示的秒数，用于相加到此 TimeSpanX 结构。</param>
        /// <returns>TimeSpanX 结构，表示将此 TimeSpanX 结构加上若干秒得到的结果。</returns>
        public TimeSpanX AddSeconds(double seconds)
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
                        return new TimeSpanX(newTotalMS);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 TimeSpanX 结构加上若干毫秒得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="milliseconds">十进制浮点数表示的毫秒数，用于相加到此 TimeSpanX 结构。</param>
        /// <returns>TimeSpanX 结构，表示将此 TimeSpanX 结构加上若干毫秒得到的结果。</returns>
        public TimeSpanX AddMilliseconds(decimal milliseconds)
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
                        return new TimeSpanX(newTotalMS);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 TimeSpanX 结构加上若干毫秒得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="milliseconds">双精度浮点数表示的毫秒数，用于相加到此 TimeSpanX 结构。</param>
        /// <returns>TimeSpanX 结构，表示将此 TimeSpanX 结构加上若干毫秒得到的结果。</returns>
        public TimeSpanX AddMilliseconds(double milliseconds)
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
                        return new TimeSpanX(newTotalMS);
                    }
                }
            }
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断两个 TimeSpanX 结构是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 TimeSpanX 结构。</param>
        /// <param name="right">用于比较的第二个 TimeSpanX 结构。</param>
        /// <returns>布尔值，表示两个 TimeSpanX 结构是否相等。</returns>
        public static bool Equals(TimeSpanX left, TimeSpanX right) => left.Equals(right);

        //

        /// <summary>
        /// 比较两个 TimeSpanX 结构的次序。
        /// </summary>
        /// <param name="left">用于比较的第一个 TimeSpanX 结构。</param>
        /// <param name="right">用于比较的第二个 TimeSpanX 结构。</param>
        /// <returns>32 位整数，表示将两个 TimeSpanX 结构进行次序比较得到的结果。</returns>
        public static int Compare(TimeSpanX left, TimeSpanX right) => left.CompareTo(right);

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 TimeSpanX 结构是否表示相同的时刻。
        /// </summary>
        /// <param name="left">运算符左侧比较的 TimeSpanX 结构。</param>
        /// <param name="right">运算符右侧比较的 TimeSpanX 结构。</param>
        /// <returns>布尔值，表示两个 TimeSpanX 结构是否表示相同的时刻。</returns>
        public static bool operator ==(TimeSpanX left, TimeSpanX right) => left._TotalMilliseconds == right._TotalMilliseconds;

        /// <summary>
        /// 判断两个 TimeSpanX 结构是否表示不同的时刻。
        /// </summary>
        /// <param name="left">运算符左侧比较的 TimeSpanX 结构。</param>
        /// <param name="right">运算符右侧比较的 TimeSpanX 结构。</param>
        /// <returns>布尔值，表示两个 TimeSpanX 结构是否表示不同的时刻。</returns>
        public static bool operator !=(TimeSpanX left, TimeSpanX right) => left._TotalMilliseconds != right._TotalMilliseconds;

        /// <summary>
        /// 判断两个 TimeSpanX 结构表示的时刻是否前者早于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 TimeSpanX 结构。</param>
        /// <param name="right">运算符右侧比较的 TimeSpanX 结构。</param>
        /// <returns>布尔值，表示两个 TimeSpanX 结构表示的时刻是否前者早于后者。</returns>
        public static bool operator <(TimeSpanX left, TimeSpanX right) => left._TotalMilliseconds < right._TotalMilliseconds;

        /// <summary>
        /// 判断两个 TimeSpanX 结构表示的时刻是否前者晚于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 TimeSpanX 结构。</param>
        /// <param name="right">运算符右侧比较的 TimeSpanX 结构。</param>
        /// <returns>布尔值，表示两个 TimeSpanX 结构表示的时刻是否前者晚于后者。</returns>
        public static bool operator >(TimeSpanX left, TimeSpanX right) => left._TotalMilliseconds > right._TotalMilliseconds;

        /// <summary>
        /// 判断两个 TimeSpanX 结构表示的时刻是否前者早于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 TimeSpanX 结构。</param>
        /// <param name="right">运算符右侧比较的 TimeSpanX 结构。</param>
        /// <returns>布尔值，表示两个 TimeSpanX 结构表示的时刻是否前者早于或等于后者。</returns>
        public static bool operator <=(TimeSpanX left, TimeSpanX right) => left._TotalMilliseconds <= right._TotalMilliseconds;

        /// <summary>
        /// 判断两个 TimeSpanX 结构表示的时刻是否前者晚于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 TimeSpanX 结构。</param>
        /// <param name="right">运算符右侧比较的 TimeSpanX 结构。</param>
        /// <returns>布尔值，表示两个 TimeSpanX 结构表示的时刻是否前者晚于或等于后者。</returns>
        public static bool operator >=(TimeSpanX left, TimeSpanX right) => left._TotalMilliseconds >= right._TotalMilliseconds;

        //

        /// <summary>
        /// 返回在 TimeSpanX 结构前添加正号得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="timeSpan">运算符右侧的TimeSpanX 结构。</param>
        /// <returns>TimeSpanX 结构，表示在 TimeSpanX 结构前添加正号得到的结果。</returns>
        public static TimeSpanX operator +(TimeSpanX timeSpan) => timeSpan;

        /// <summary>
        /// 返回在 TimeSpanX 结构前添加负号得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="timeSpan">运算符右侧的TimeSpanX 结构。</param>
        /// <returns>TimeSpanX 结构，表示在 TimeSpanX 结构前添加负号得到的结果。</returns>
        public static TimeSpanX operator -(TimeSpanX timeSpan) => new TimeSpanX(-timeSpan._TotalMilliseconds);

        //

        /// <summary>
        /// 返回将 TimeSpanX 结构与 TimeSpanX 结构相加得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="left">TimeSpanX 结构，表示被加数。</param>
        /// <param name="right">TimeSpanX 结构，表示加数。</param>
        /// <returns>TimeSpanX 结构，表示将 TimeSpanX 结构与 TimeSpanX 结构相加得到的结果。</returns>
        public static TimeSpanX operator +(TimeSpanX left, TimeSpanX right) => left.AddMilliseconds(right._TotalMilliseconds);

        /// <summary>
        /// 返回将 TimeSpanX 结构与 TimeSpan 结构相加得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="left">TimeSpanX 结构，表示被加数。</param>
        /// <param name="right">TimeSpan 结构，表示加数。</param>
        /// <returns>TimeSpanX 结构，表示将 TimeSpanX 结构与 TimeSpan 结构相加得到的结果。</returns>
        public static TimeSpanX operator +(TimeSpanX left, TimeSpan right) => left.AddMilliseconds(right.TotalMilliseconds);

        /// <summary>
        /// 返回将 TimeSpan 结构与 TimeSpanX 结构相加得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="left">TimeSpan 结构，表示被加数。</param>
        /// <param name="right">TimeSpanX 结构，表示加数。</param>
        /// <returns>TimeSpanX 结构，表示将 TimeSpan 结构与 TimeSpanX 结构相加得到的结果。</returns>
        public static TimeSpanX operator +(TimeSpan left, TimeSpanX right) => right.AddMilliseconds(left.TotalMilliseconds);

        /// <summary>
        /// 返回将 TimeSpanX 结构与 TimeSpanX 结构相减得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="left">TimeSpanX 结构，表示被减数。</param>
        /// <param name="right">TimeSpanX 结构，表示减数。</param>
        /// <returns>TimeSpanX 结构，表示将 TimeSpanX 结构与 TimeSpanX 结构相减得到的结果。</returns>
        public static TimeSpanX operator -(TimeSpanX left, TimeSpanX right) => left.AddMilliseconds(-right._TotalMilliseconds);

        /// <summary>
        /// 返回将 TimeSpanX 结构与 TimeSpan 结构相减得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="left">TimeSpanX 结构，表示被减数。</param>
        /// <param name="right">TimeSpan 结构，表示减数。</param>
        /// <returns>TimeSpanX 结构，表示将 TimeSpanX 结构与 TimeSpan 结构相减得到的结果。</returns>
        public static TimeSpanX operator -(TimeSpanX left, TimeSpan right) => left.AddMilliseconds(-right.TotalMilliseconds);

        /// <summary>
        /// 返回将 TimeSpan 结构与 TimeSpanX 结构相减得到的 TimeSpanX 结构的新实例。
        /// </summary>
        /// <param name="left">TimeSpan 结构，表示被减数。</param>
        /// <param name="right">TimeSpanX 结构，表示减数。</param>
        /// <returns>TimeSpanX 结构，表示将 TimeSpan 结构与 TimeSpanX 结构相减得到的结果。</returns>
        public static TimeSpanX operator -(TimeSpan left, TimeSpanX right) => right.AddMilliseconds(-left.TotalMilliseconds);

        //

        /// <summary>
        /// 将指定的 TimeSpan 结构隐式转换为 TimeSpanX 结构。
        /// </summary>
        /// <param name="timeSpan">用于转换的 TimeSpan 结构。</param>
        /// <returns>TimeSpanX 结构，表示隐式转换的结果。</returns>
        public static implicit operator TimeSpanX(TimeSpan timeSpan) => new TimeSpanX(timeSpan);

        #endregion
    }
}