/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

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

            public _TicksWithCount(long ticks, long count)
            {
                if (ticks < DateTime.MinValue.Ticks || ticks > DateTime.MaxValue.Ticks)
                {
                    throw new ArgumentOutOfRangeException(nameof(ticks));
                }

                if (count <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }

                //

                _Ticks = ticks;
                _Count = count;
            }

            public _TicksWithCount(long ticks) : this(ticks, 1) { }

            // 获取或设置此 _TicksWithCount 对象的计时周期数。
            public long Ticks
            {
                get => _Ticks;
                set => _Ticks = value;
            }

            // 获取或设置此 _TicksWithCount 对象的计数。
            public long Count
            {
                get => _Count;
                set => _Count = value;
            }
        }

        //

        private long _SampleTicks; // 采样周期的计时周期数。
        private long _ExtendSampleTicks; // 扩展的采样周期的计时周期数。
        private IndexableQueue<_TicksWithCount> _TicksHistory; // 历史计时计数队列。

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用指定的采样周期初始化 FrequencyCounter 的新实例。
        /// <param name="samplePeriod">采样周期。</param>
        /// <param name="extendSamplePeriod">扩展的采样周期。</param>
        /// </summary>
        public FrequencyCounter(TimeSpan samplePeriod, TimeSpan extendSamplePeriod)
        {
            if (samplePeriod.Ticks <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(samplePeriod));
            }

            if (extendSamplePeriod.Ticks < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(extendSamplePeriod));
            }

            //

            _SampleTicks = samplePeriod.Ticks;
            _ExtendSampleTicks = extendSamplePeriod.Ticks;
            _TicksHistory = new IndexableQueue<_TicksWithCount>(32, false);
        }

        /// <summary>
        /// 使用指定的采样周期初始化 FrequencyCounter 的新实例。
        /// <param name="samplePeriod">采样周期。</param>
        /// </summary>
        public FrequencyCounter(TimeSpan samplePeriod) : this(samplePeriod, TimeSpan.Zero) { }

        /// <summary>
        /// 使用指定的采样周期初始化 FrequencyCounter 的新实例。
        /// </summary>
        /// <param name="sampleSeconds">采样周期（秒）。</param>
        /// <param name="extendSampleSeconds">扩展的采样周期（秒）。</param>
        public FrequencyCounter(double sampleSeconds, double extendSampleSeconds)
        {
            if (InternalMethod.IsNaNOrInfinity(sampleSeconds) || sampleSeconds < 1.0 / TimeSpan.TicksPerSecond || sampleSeconds > TimeSpan.MaxValue.Seconds)
            {
                throw new ArgumentOutOfRangeException(nameof(sampleSeconds));
            }

            if (InternalMethod.IsNaNOrInfinity(extendSampleSeconds) || extendSampleSeconds < 0 || extendSampleSeconds > TimeSpan.MaxValue.Seconds)
            {
                throw new ArgumentOutOfRangeException(nameof(extendSampleSeconds));
            }

            //

            _SampleTicks = Math.Max(1, (long)Math.Round(sampleSeconds * TimeSpan.TicksPerSecond));
            _ExtendSampleTicks = (long)Math.Round(extendSampleSeconds * TimeSpan.TicksPerSecond);
            _TicksHistory = new IndexableQueue<_TicksWithCount>(32, false);
        }

        /// <summary>
        /// 使用指定的采样周期初始化 FrequencyCounter 的新实例。
        /// </summary>
        /// <param name="sampleSeconds">采样周期（秒）。</param>
        public FrequencyCounter(double sampleSeconds) : this(sampleSeconds, 0) { }

        /// <summary>
        /// 使用默认的采样周期初始化 FrequencyCounter 的新实例。
        /// </summary>
        public FrequencyCounter() : this(1, 0) { }

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
                    long currentTicks = DateTime.UtcNow.Ticks;
                    double deltaTicks = 0;
                    double count = 0;

                    for (int i = _TicksHistory.Count - 1; i >= 1; i--)
                    {
                        deltaTicks = currentTicks - _TicksHistory[i - 1].Ticks;

                        if (deltaTicks <= _SampleTicks)
                        {
                            count += _TicksHistory[i].Count;
                        }
                        else if (count <= 0 && deltaTicks <= _ExtendSampleTicks)
                        {
                            count += _TicksHistory[i].Count;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (deltaTicks > 0)
                    {
                        return count / deltaTicks * TimeSpan.TicksPerSecond;
                    }
                    else
                    {
                        return 0;
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
        public double Period => 1 / Frequency;

        /// <summary>
        /// 获取此 FrequencyCounter 对象的采样周期。
        /// </summary>
        public TimeSpan SamplePeriod => TimeSpan.FromTicks(_SampleTicks);

        /// <summary>
        /// 获取此 FrequencyCounter 对象的扩展的采样周期。
        /// </summary>
        public TimeSpan ExtendSamplePeriod => TimeSpan.FromTicks(_ExtendSampleTicks);

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
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            //

            long currentTicks = DateTime.UtcNow.Ticks;

            if (_TicksHistory.IsEmpty)
            {
                _TicksHistory.Enqueue(new _TicksWithCount(currentTicks, count));
            }
            else
            {
                _TicksWithCount tail = _TicksHistory.Tail;

                if (currentTicks < tail.Ticks)
                {
                    Reset();

                    _TicksHistory.Enqueue(new _TicksWithCount(currentTicks, count));
                }
                else if (currentTicks == tail.Ticks)
                {
                    tail.Count += count;
                }
                else
                {
                    while (_TicksHistory.Count > 2 && currentTicks - _TicksHistory.Head.Ticks > _SampleTicks)
                    {
                        _TicksHistory.Dequeue();
                    }

                    if (_TicksHistory.IsFull && currentTicks - _TicksHistory.Head.Ticks <= _SampleTicks)
                    {
                        _TicksHistory.Resize(_TicksHistory.Capacity * 2);
                    }

                    _TicksHistory.Enqueue(new _TicksWithCount(currentTicks, count));
                }
            }
        }

        /// <summary>
        /// 更新此 FrequencyCounter 对象一次计数。
        /// </summary>
        public void Update() => Update(1);

        /// <summary>
        /// 重置此 FrequencyCounter 对象。
        /// </summary>
        public void Reset() => _TicksHistory.Clear();

        #endregion
    }
}