/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

Com.FrequencyCounter
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
    /// 频率计数器，用于实时计算在过去一小段时间间隔内某一事件发生的频率。
    /// </summary>
    public sealed class FrequencyCounter
    {
        #region 非公开成员

        // 表示携带计数的计时周期数。
        private sealed class _TicksWithCount
        {
            private long _Ticks; // 计时周期数。
            private long _Count; // 计数。

            public _TicksWithCount(long ticks, int count)
            {
                if ((ticks < DateTime.MinValue.Ticks || ticks > DateTime.MaxValue.Ticks) || count <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                //

                Ticks = ticks;
                Count = count;
            }

            public _TicksWithCount(long ticks) : this(ticks, 1)
            {
            }

            // 获取或设置此 _TicksWithCount 对象的计时周期数。
            public long Ticks
            {
                get
                {
                    return _Ticks;
                }

                set
                {
                    _Ticks = value;
                }
            }

            // 获取或设置此 _TicksWithCount 对象的计数。
            public long Count
            {
                get
                {
                    return _Count;
                }

                set
                {
                    _Count = value;
                }
            }
        }

        //

        private const double _TicksPerSecond = 1E7; // 每秒的计时周期数。

        //

        private long _DeltaTTicks; // 采样时间间隔的计时周期数。
        private IndexableQueue<_TicksWithCount> _TicksHistory; // 历史计时计数队列。

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用指定的采样时间间隔初始化 FrequencyCounter 的新实例。
        /// </summary>
        /// <param name="deltaTSeconds">采样时间间隔（秒）。</param>
        public FrequencyCounter(double deltaTSeconds)
        {
            if (double.IsNaN(deltaTSeconds) || double.IsInfinity(deltaTSeconds) || deltaTSeconds <= 0)
            {
                throw new ArgumentException();
            }

            //

            _DeltaTTicks = Math.Max(1, (long)Math.Round(deltaTSeconds * _TicksPerSecond));
            _TicksHistory = new IndexableQueue<_TicksWithCount>(32, false);
        }

        /// <summary>
        /// 使用默认的采样时间间隔初始化 FrequencyCounter 的新实例。
        /// </summary>
        public FrequencyCounter() : this(1)
        {
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取此 FrequencyCounter 对象的频率（赫兹）。
        /// </summary>
        public double Frequency
        {
            get
            {
                if (_TicksHistory.Count >= 2)
                {
                    long ticks = DateTime.UtcNow.Ticks;

                    if (_TicksHistory.Count == 2)
                    {
                        return (_TicksHistory.Tail.Count * _TicksPerSecond / (ticks - _TicksHistory.Head.Ticks));
                    }
                    else
                    {
                        long count = 0;

                        _TicksWithCount head = _TicksHistory.Head;

                        for (int i = _TicksHistory.Count - 1; i >= 1; i--)
                        {
                            head = _TicksHistory[i - 1];

                            if (ticks - head.Ticks <= _DeltaTTicks)
                            {
                                count += _TicksHistory[i].Count;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (count <= 0)
                        {
                            count = _TicksHistory.Tail.Count;
                            head = _TicksHistory[_TicksHistory.Count - 2];
                        }

                        return (count * _TicksPerSecond / (ticks - head.Ticks));
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 获取此 FrequencyCounter 对象的周期（秒）。
        /// </summary>
        public double Period
        {
            get
            {
                return (1 / Frequency);
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 更新此 FrequencyCounter 对象指定的计数次数。
        /// </summary>
        /// <param name="count">计数次数。</param>
        public void Update(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            long ticks = DateTime.UtcNow.Ticks;

            if (_TicksHistory.IsEmpty)
            {
                _TicksHistory.Enqueue(new _TicksWithCount(ticks, count));
            }
            else
            {
                _TicksWithCount tail = _TicksHistory.Tail;

                if (ticks < tail.Ticks)
                {
                    Reset();
                }
                else if (ticks == tail.Ticks)
                {
                    tail.Count += count;
                }
                else
                {
                    if (_TicksHistory.IsFull && ticks - _TicksHistory.Head.Ticks <= _DeltaTTicks)
                    {
                        _TicksHistory.Resize(_TicksHistory.Capacity * 2);
                    }

                    _TicksHistory.Enqueue(new _TicksWithCount(ticks, count));
                }
            }

            while (_TicksHistory.Count > 2 && ticks - _TicksHistory.Head.Ticks > _DeltaTTicks)
            {
                _TicksHistory.Dequeue();
            }
        }

        /// <summary>
        /// 更新此 FrequencyCounter 对象一次计数。
        /// </summary>
        public void Update()
        {
            Update(1);
        }

        /// <summary>
        /// 重置此 FrequencyCounter 对象。
        /// </summary>
        public void Reset()
        {
            _TicksHistory.Clear();
        }

        #endregion
    }
}