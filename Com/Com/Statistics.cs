/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.Statistics
Version 18.9.24.1600

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
    /// 为统计学的基本计算供静态方法。
    /// </summary>
    public static class Statistics
    {
        private static readonly Random _Rand = new Random(); // 用于生成随机数的 Random 类的实例。

        //

        /// <summary>
        /// 返回一个概率密度平均分布的非负随机整数。
        /// </summary>
        public static int RandomInteger()
        {
            try
            {
                return _Rand.Next();
            }
            catch
            {
                return int.MinValue;
            }
        }

        /// <summary>
        /// 返回一个在 0 与右端点指定的区间内概率密度平均分布的非负随机整数。
        /// </summary>
        /// <param name="right">区间右端点（不含）。</param>
        public static int RandomInteger(int right)
        {
            try
            {
                if (right <= 0)
                {
                    return int.MinValue;
                }

                return _Rand.Next(right);
            }
            catch
            {
                return int.MinValue;
            }
        }

        /// <summary>
        /// 返回一个在指定区间内概率密度平均分布的随机整数。
        /// </summary>
        /// <param name="left">区间左端点（含）。</param>
        /// <param name="right">区间右端点（不含）。</param>
        public static int RandomInteger(int left, int right)
        {
            try
            {
                if (left >= right)
                {
                    return int.MinValue;
                }

                return _Rand.Next(left, right);
            }
            catch
            {
                return int.MinValue;
            }
        }

        //

        /// <summary>
        /// 返回一个小于 1 的概率密度平均分布的非负随机双精度浮点数。
        /// </summary>
        public static double RandomDouble()
        {
            try
            {
                return _Rand.NextDouble();
            }
            catch
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// 返回一个在 0 与右端点指定的区间内概率密度平均分布的非负随机双精度浮点数。
        /// </summary>
        /// <param name="right">区间右端点（不含）。</param>
        public static double RandomDouble(double right)
        {
            try
            {
                if (InternalMethod.IsNaNOrInfinity(right) || right <= 0)
                {
                    return double.NaN;
                }

                return right * _Rand.NextDouble();
            }
            catch
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// 返回一个在指定区间内概率密度平均分布的随机双精度浮点数。
        /// </summary>
        /// <param name="left">区间左端点（含）。</param>
        /// <param name="right">区间右端点（不含）。</param>
        public static double RandomDouble(double left, double right)
        {
            try
            {
                if (InternalMethod.IsNaNOrInfinity(left) || InternalMethod.IsNaNOrInfinity(right) || left >= right)
                {
                    return double.NaN;
                }

                return left + (right - left) * _Rand.NextDouble();
            }
            catch
            {
                return double.NaN;
            }
        }

        //

        private static double[] _StdGaussRandom() // 返回两个概率密度服从一维标准高斯分布的随机双精度浮点数（数学期望 = 0，标准差 = 1）。
        {
            try
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
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 一维高斯分布的概率密度函数。
        /// </summary>
        /// <param name="ev">数学期望。</param>
        /// <param name="sd">标准差。</param>
        /// <param name="x">样本的值。</param>
        public static double GaussDistributionProbabilityDensity(double ev, double sd, double x)
        {
            try
            {
                if (InternalMethod.IsNaNOrInfinity(ev) || InternalMethod.IsNaNOrInfinity(sd) || InternalMethod.IsNaNOrInfinity(x))
                {
                    return double.NaN;
                }

                double N = (x - ev) / sd;

                return Math.Exp(-0.5 * N * N) / Math.Sqrt(2 * Math.PI) / sd;
            }
            catch
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// 返回一个概率密度服从一维标准高斯分布的随机双精度浮点数。
        /// </summary>
        public static double GaussRandom()
        {
            try
            {
                return _StdGaussRandom()[0];
            }
            catch
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// 返回一个概率密度在指定区间内服从一维高斯分布的随机双精度浮点数。
        /// </summary>
        /// <param name="ev">数学期望。</param>
        /// <param name="sd">标准差。</param>
        /// <param name="left">区间左端点（含）。</param>
        /// <param name="right">区间右端点（不含）。</param>
        public static double GaussRandom(double ev, double sd, double left, double right)
        {
            try
            {
                if (InternalMethod.IsNaNOrInfinity(ev) || InternalMethod.IsNaNOrInfinity(sd) || InternalMethod.IsNaNOrInfinity(left) || InternalMethod.IsNaNOrInfinity(right))
                {
                    return double.NaN;
                }

                double result = 0;

                do
                {
                    result = _StdGaussRandom()[0] * sd * sd + ev;
                }
                while (result < left || result >= right);

                return result;
            }
            catch
            {
                return double.NaN;
            }
        }
    }
}