/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.Statistics
Version 20.10.27.1900

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com
{
    /// <summary>
    /// 为统计学的基本计算供静态方法。
    /// </summary>
    public static partial class Statistics
    {
        #region 随机数

        private static readonly Random _Rand = new Random(); // 用于生成随机数的 Random 类的实例。

        // 返回两个概率密度服从标准正态分布的随机双精度浮点数。
        private static double[] _StdNormalRandom()
        {
            double r0 = 0, r1 = 0, v0 = 0, v1 = 0, sum = 0;

            while (sum > 1 || sum == 0)
            {
                r0 = _Rand.NextDouble();
                r1 = _Rand.NextDouble();

                v0 = 2 * r0 - 1;
                v1 = 2 * r1 - 1;

                sum = v0 * v0 + v1 * v1;
            }

            double gr0 = Math.Sqrt(-2 * Math.Log(sum) / sum) * v0;
            double gr1 = Math.Sqrt(-2 * Math.Log(sum) / sum) * v1;

            return new double[2] { gr0, gr1 };
        }

        //

        /// <summary>
        /// 返回一个概率密度平均分布的非负随机 32 位整数。
        /// </summary>
        /// <returns>32 位整数，表示概率密度平均分布的非负随机数。</returns>
        public static int RandomInteger() => _Rand.Next();

        /// <summary>
        /// 返回一个在 0 与右端点指定的区间内概率密度平均分布的非负随机 32 位整数。
        /// </summary>
        /// <param name="maxValue">最大值（不含）。</param>
        /// <returns>32 位整数，表示概率密度平均分布的非负随机数。</returns>
        public static int RandomInteger(int maxValue)
        {
            if (maxValue == 0)
            {
                return 0;
            }
            else if (maxValue > 0)
            {
                return _Rand.Next(maxValue);
            }
            else
            {
                return -_Rand.Next(-(maxValue + 1)) - 1;
            }
        }

        /// <summary>
        /// 返回一个在指定区间内概率密度平均分布的随机 32 位整数。
        /// </summary>
        /// <param name="minValue">最小值（含）。</param>
        /// <param name="maxValue">最大值（不含）。</param>
        /// <returns>32 位整数，表示概率密度平均分布的随机数。</returns>
        public static int RandomInteger(int minValue, int maxValue)
        {
            if (minValue == maxValue)
            {
                return minValue;
            }
            else if (minValue < maxValue)
            {
                return _Rand.Next(minValue, maxValue);
            }
            else
            {
                return unchecked(-_Rand.Next(-minValue - 1, -(maxValue + 1)) - 1);
            }
        }

        //

        /// <summary>
        /// 返回一个小于 1 的概率密度平均分布的非负随机双精度浮点数。
        /// </summary>
        /// <returns>双精度浮点数，表示概率密度平均分布的非负随机数。</returns>
        public static double RandomDouble() => _Rand.NextDouble();

        /// <summary>
        /// 返回一个在 0 与右端点指定的区间内概率密度平均分布的非负随机双精度浮点数。
        /// </summary>
        /// <param name="maxValue">最大值（不含）。</param>
        /// <returns>双精度浮点数，表示概率密度平均分布的非负随机数。</returns>
        public static double RandomDouble(double maxValue)
        {
            if (InternalMethod.IsNaNOrInfinity(maxValue))
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue));
            }

            //

            if (maxValue == 0)
            {
                return 0;
            }
            else
            {
                return maxValue * _Rand.NextDouble();
            }
        }

        /// <summary>
        /// 返回一个在指定区间内概率密度平均分布的随机双精度浮点数。
        /// </summary>
        /// <param name="minValue">最小值（含）。</param>
        /// <param name="maxValue">最大值（不含）。</param>
        /// <returns>双精度浮点数，表示概率密度平均分布的随机数。</returns>
        public static double RandomDouble(double minValue, double maxValue)
        {
            if (InternalMethod.IsNaNOrInfinity(minValue))
            {
                throw new ArgumentOutOfRangeException(nameof(minValue));
            }

            if (InternalMethod.IsNaNOrInfinity(maxValue))
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue));
            }

            //

            if (minValue == maxValue)
            {
                return minValue;
            }
            else
            {
                return minValue + (maxValue - minValue) * _Rand.NextDouble();
            }
        }

        //

        /// <summary>
        /// 返回一个概率密度服从标准正态分布的随机 32 位整数。
        /// </summary>
        /// <returns>32 位整数，表示概率密度服从标准正态分布的随机数。</returns>
        public static int NormalDistributionRandomInteger() => (int)Math.Round(_StdNormalRandom()[0]);

        /// <summary>
        /// 返回一个概率密度服从正态分布的随机 32 位整数。
        /// </summary>
        /// <param name="ev">数学期望。</param>
        /// <param name="sd">标准差。</param>
        /// <returns>32 位整数，表示概率密度服从正态分布的随机数。</returns>
        public static int NormalDistributionRandomInteger(double ev, double sd)
        {
            if (InternalMethod.IsNaNOrInfinity(ev))
            {
                throw new ArgumentOutOfRangeException(nameof(ev));
            }

            if (InternalMethod.IsNaNOrInfinity(sd))
            {
                throw new ArgumentOutOfRangeException(nameof(sd));
            }

            //

            return (int)Math.Round(_StdNormalRandom()[0] * sd * sd + ev);
        }

        /// <summary>
        /// 返回一个概率密度服从标准正态分布的随机双精度浮点数。
        /// </summary>
        /// <returns>双精度浮点数，表示概率密度服从标准正态分布的随机数。</returns>
        public static double NormalDistributionRandomDouble() => _StdNormalRandom()[0];

        /// <summary>
        /// 返回一个概率密度服从正态分布的随机双精度浮点数。
        /// </summary>
        /// <param name="ev">数学期望。</param>
        /// <param name="sd">标准差。</param>
        /// <returns>双精度浮点数，表示概率密度服从正态分布的随机数。</returns>
        public static double NormalDistributionRandomDouble(double ev, double sd)
        {
            if (InternalMethod.IsNaNOrInfinity(ev))
            {
                throw new ArgumentOutOfRangeException(nameof(ev));
            }

            if (InternalMethod.IsNaNOrInfinity(sd))
            {
                throw new ArgumentOutOfRangeException(nameof(sd));
            }

            //

            return _StdNormalRandom()[0] * sd * sd + ev;
        }

        #endregion
    }
}