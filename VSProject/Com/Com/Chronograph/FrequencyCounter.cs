/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.FrequencyCounter
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
    /// 频率计数器，用于实时计算在过去一小段时间间隔内某一事件发生的频率。
    /// </summary>
    public sealed class FrequencyCounter
    {
        #region 非公开成员

        // 采样点。
        private sealed class _SamplePoint
        {
            private long _Ticks; // 计时周期数。
            private long _Count; // 计数。

            public _SamplePoint(long ticks, long count)
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

            public _SamplePoint(long ticks) : this(ticks, 1) { }

            public long Ticks
            {
                get => _Ticks;
                set => _Ticks = value;
            }

            public long Count
            {
                get => _Count;
                set => _Count = value;
            }
        }

        //

        private long _SamplePeriodTicks; // 采样周期的计时周期数。
        private IndexableQueue<_SamplePoint> _SamplePoints; // 采样点队列。

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用指定的采样周期初始化 FrequencyCounter 的新实例。
        /// <param name="samplePeriod">采样周期。</param>
        /// </summary>
        public FrequencyCounter(TimeSpan samplePeriod)
        {
            if (samplePeriod.Ticks <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(samplePeriod));
            }

            //

            _SamplePeriodTicks = samplePeriod.Ticks;
            _SamplePoints = new IndexableQueue<_SamplePoint>(64, true);
        }

        /// <summary>
        /// 使用指定的采样周期初始化 FrequencyCounter 的新实例。
        /// </summary>
        /// <param name="samplePeriodSeconds">采样周期（秒）。</param>
        public FrequencyCounter(double samplePeriodSeconds)
        {
            if (InternalMethod.IsNaNOrInfinity(samplePeriodSeconds) || samplePeriodSeconds < 1.0 / TimeSpan.TicksPerSecond || samplePeriodSeconds > TimeSpan.MaxValue.Seconds)
            {
                throw new ArgumentOutOfRangeException(nameof(samplePeriodSeconds));
            }

            //

            _SamplePeriodTicks = Math.Max(1, (long)Math.Round(samplePeriodSeconds * TimeSpan.TicksPerSecond));
            _SamplePoints = new IndexableQueue<_SamplePoint>(64, true);
        }

        /// <summary>
        /// 使用默认的采样周期（1秒）初始化 FrequencyCounter 的新实例。
        /// </summary>
        public FrequencyCounter() : this(1) { }

        #endregion

        #region 属性

        /// <summary>
        /// 获取此 FrequencyCounter 对象的采样周期。
        /// </summary>
        public TimeSpan SamplePeriod => TimeSpan.FromTicks(_SamplePeriodTicks);

        /// <summary>
        /// 获取此 FrequencyCounter 对象的频率（赫兹）。
        /// </summary>
        public double Frequency
        {
            get
            {
                if (_SamplePoints.Count >= 2)
                {
                    long tailTicks = _SamplePoints.Tail.Ticks;
                    long currentTicks = DateTime.UtcNow.Ticks;

                    int headIndex = 1;
                    _SamplePoint preHead = _SamplePoints[headIndex - 1], head = _SamplePoints[headIndex];
                    long preHeadTicks = preHead.Ticks, headTicks = head.Ticks;
                    while (true)
                    {
                        // 确保第一个采样点的前一个采样点与最后一个采样点的时间间隔在一个采样周期内，且所有采样点都在距今一个采样周期内
                        if (tailTicks - preHeadTicks <= _SamplePeriodTicks && currentTicks - headTicks <= _SamplePeriodTicks)
                        {
                            break;
                        }

                        headIndex++;
                        // 有可能所有采样点都不在距今一个采样周期内
                        if (headIndex >= _SamplePoints.Count)
                        {
                            break;
                        }

                        preHead = head;
                        preHeadTicks = headTicks;
                        head = _SamplePoints[headIndex];
                        headTicks = head.Ticks;
                    }

                    double totalCount = 0;
                    for (int i = headIndex; i < _SamplePoints.Count; i++)
                    {
                        totalCount += _SamplePoints[i].Count;
                    }

                    long deltaTicks = tailTicks - preHeadTicks;
                    if (totalCount > 0 && deltaTicks > 0)
                    {
                        return totalCount / deltaTicks * TimeSpan.TicksPerSecond;
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

            if (_SamplePoints.IsEmpty)
            {
                _SamplePoints.Enqueue(new _SamplePoint(currentTicks, count));
            }
            else
            {
                _SamplePoint tail = _SamplePoints.Tail;

                // 确保计时的单调性
                if (currentTicks < tail.Ticks)
                {
                    Reset();

                    _SamplePoints.Enqueue(new _SamplePoint(currentTicks, count));
                }
                else if (currentTicks == tail.Ticks)
                {
                    tail.Count += count;
                }
                else
                {
                    // 只需保留距今一个采样周期内的采样点
                    while (!_SamplePoints.IsEmpty && currentTicks - _SamplePoints.Head.Ticks > _SamplePeriodTicks)
                    {
                        _SamplePoints.Dequeue();
                    }

                    _SamplePoints.Enqueue(new _SamplePoint(currentTicks, count));
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
        public void Reset() => _SamplePoints.Clear();

        #endregion
    }
}