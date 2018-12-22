/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.Statistics
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
    /// 为统计学的基本计算供静态方法。
    /// </summary>
    public static class Statistics
    {
        #region 私有成员与内部成员

        private static readonly Random _Rand = new Random(); // 用于生成随机数的 Random 类的实例。

        //

        private static double[] _StdNormalRandom() // 返回两个概率密度服从标准正态分布的随机双精度浮点数。
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

        #endregion

        #region 随机数

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

        /// <summary>
        /// 返回一个概率密度服从一维标准高斯分布的随机双精度浮点数。
        /// </summary>
        public static double GaussRandom()
        {
            try
            {
                return _StdNormalRandom()[0];
            }
            catch
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// 返回一个概率密度服从一维高斯分布的随机双精度浮点数。
        /// </summary>
        /// <param name="ev">数学期望。</param>
        /// <param name="sd">标准差。</param>
        public static double GaussRandom(double ev, double sd)
        {
            try
            {
                if (InternalMethod.IsNaNOrInfinity(ev) || InternalMethod.IsNaNOrInfinity(sd))
                {
                    return double.NaN;
                }

                return _StdNormalRandom()[0] * sd * sd + ev;
            }
            catch
            {
                return double.NaN;
            }
        }

        #endregion

        #region 概率分布

        /// <summary>
        /// 正态分布的概率密度函数。
        /// </summary>
        /// <param name="ev">数学期望。</param>
        /// <param name="sd">标准差。</param>
        /// <param name="x">样本的值。</param>
        public static double NormalDistributionProbabilityDensity(double ev, double sd, double x)
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
        /// 标准正态分布的概率密度函数。
        /// </summary>
        /// <param name="x">样本的值。</param>
        public static double StandardNormalDistributionProbabilityDensity(double x)
        {
            try
            {
                if (InternalMethod.IsNaNOrInfinity(x))
                {
                    return double.NaN;
                }

                return Math.Exp(-0.5 * x * x) / Math.Sqrt(2 * Math.PI);
            }
            catch
            {
                return double.NaN;
            }
        }

        #endregion

        #region 极值

        /// <summary>
        /// 计算一组可排序对象的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static IComparable Max(params IComparable[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    IComparable result = values[0];

                    for (int i = 1; i < values.Length; i++)
                    {
                        if (values[i] != null)
                        {
                            if (values[i].CompareTo(result) > 0)
                            {
                                result = values[i];
                            }
                        }
                        else if (result != null)
                        {
                            if (result.CompareTo(values[i]) < 0)
                            {
                                result = values[i];
                            }
                        }
                    }

                    return result;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 计算一组 8 位整数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static sbyte Max(params sbyte[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    sbyte result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Max(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return sbyte.MinValue;
            }
        }

        /// <summary>
        /// 计算一组 8 位无符号整数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static byte Max(params byte[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    byte result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Max(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return byte.MinValue;
            }
        }

        /// <summary>
        /// 计算一组 16 位整数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static short Max(params short[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    short result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Max(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return short.MinValue;
            }
        }

        /// <summary>
        /// 计算一组 16 位无符号整数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static ushort Max(params ushort[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    ushort result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Max(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return ushort.MinValue;
            }
        }

        /// <summary>
        /// 计算一组 32 位整数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static int Max(params int[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    int result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Max(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return int.MinValue;
            }
        }

        /// <summary>
        /// 计算一组 32 位无符号整数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static uint Max(params uint[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    uint result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Max(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return uint.MinValue;
            }
        }

        /// <summary>
        /// 计算一组 64 位整数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static long Max(params long[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    long result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Max(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return long.MinValue;
            }
        }

        /// <summary>
        /// 计算一组 64 位无符号整数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static ulong Max(params ulong[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    ulong result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Max(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return ulong.MinValue;
            }
        }

        /// <summary>
        /// 计算一组单精度浮点数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static float Max(params float[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    float result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Max(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return float.NaN;
            }
        }

        /// <summary>
        /// 计算一组双精度浮点数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static double Max(params double[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    double result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Max(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// 计算一组十进制数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static decimal Max(params decimal[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    decimal result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Max(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return decimal.MinValue;
            }
        }

        //

        /// <summary>
        /// 计算一组可排序对象的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static IComparable Min(params IComparable[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    IComparable result = values[0];

                    for (int i = 1; i < values.Length; i++)
                    {
                        if (values[i] != null)
                        {
                            if (values[i].CompareTo(result) < 0)
                            {
                                result = values[i];
                            }
                        }
                        else if (result != null)
                        {
                            if (result.CompareTo(values[i]) > 0)
                            {
                                result = values[i];
                            }
                        }
                    }

                    return result;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 计算一组 8 位整数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static sbyte Min(params sbyte[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    sbyte result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Min(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return sbyte.MinValue;
            }
        }

        /// <summary>
        /// 计算一组 8 位无符号整数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static byte Min(params byte[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    byte result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Min(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return byte.MinValue;
            }
        }

        /// <summary>
        /// 计算一组 16 位整数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static short Min(params short[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    short result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Min(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return short.MinValue;
            }
        }

        /// <summary>
        /// 计算一组 16 位无符号整数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static ushort Min(params ushort[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    ushort result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Min(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return ushort.MinValue;
            }
        }

        /// <summary>
        /// 计算一组 32 位整数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static int Min(params int[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    int result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Min(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return int.MinValue;
            }
        }

        /// <summary>
        /// 计算一组 32 位无符号整数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static uint Min(params uint[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    uint result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Min(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return uint.MinValue;
            }
        }

        /// <summary>
        /// 计算一组 64 位整数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static long Min(params long[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    long result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Min(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return long.MinValue;
            }
        }

        /// <summary>
        /// 计算一组 64 位无符号整数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static ulong Min(params ulong[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    ulong result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Min(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return ulong.MinValue;
            }
        }

        /// <summary>
        /// 计算一组单精度浮点数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static float Min(params float[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    float result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Min(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return float.NaN;
            }
        }

        /// <summary>
        /// 计算一组双精度浮点数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static double Min(params double[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    double result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Min(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// 计算一组十进制数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static decimal Min(params decimal[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    decimal result = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result = Math.Min(result, values[i]);
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return decimal.MinValue;
            }
        }

        //

        /// <summary>
        /// 计算一组可排序对象的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static (IComparable, IComparable) MinMax(params IComparable[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    IComparable Min = values[0];
                    IComparable Max = values[0];

                    for (int i = 1; i < values.Length; i++)
                    {
                        if (values[i] != null)
                        {
                            if (values[i].CompareTo(Min) < 0)
                            {
                                Min = values[i];
                            }

                            if (values[i].CompareTo(Max) > 0)
                            {
                                Max = values[i];
                            }
                        }
                        else
                        {
                            if (Min != null)
                            {
                                if (Min.CompareTo(values[i]) > 0)
                                {
                                    Min = values[i];
                                }
                            }

                            if (Max != null)
                            {
                                if (Max.CompareTo(values[i]) < 0)
                                {
                                    Max = values[i];
                                }
                            }
                        }
                    }

                    return (Min, Max);
                }

                return (null, null);
            }
            catch
            {
                return (null, null);
            }
        }

        /// <summary>
        /// 计算一组 8 位整数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static (sbyte, sbyte) MinMax(params sbyte[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    sbyte Min = values[0];
                    sbyte Max = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        Min = Math.Min(Min, values[i]);
                        Max = Math.Max(Max, values[i]);
                    }

                    return (Min, Max);
                }

                return (0, 0);
            }
            catch
            {
                return (sbyte.MinValue, sbyte.MinValue);
            }
        }

        /// <summary>
        /// 计算一组 8 位无符号整数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static (byte, byte) MinMax(params byte[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    byte Min = values[0];
                    byte Max = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        Min = Math.Min(Min, values[i]);
                        Max = Math.Max(Max, values[i]);
                    }

                    return (Min, Max);
                }

                return (0, 0);
            }
            catch
            {
                return (byte.MinValue, byte.MinValue);
            }
        }

        /// <summary>
        /// 计算一组 16 位整数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static (short, short) MinMax(params short[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    short Min = values[0];
                    short Max = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        Min = Math.Min(Min, values[i]);
                        Max = Math.Max(Max, values[i]);
                    }

                    return (Min, Max);
                }

                return (0, 0);
            }
            catch
            {
                return (short.MinValue, short.MinValue);
            }
        }

        /// <summary>
        /// 计算一组 16 位无符号整数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static (ushort, ushort) MinMax(params ushort[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    ushort Min = values[0];
                    ushort Max = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        Min = Math.Min(Min, values[i]);
                        Max = Math.Max(Max, values[i]);
                    }

                    return (Min, Max);
                }

                return (0, 0);
            }
            catch
            {
                return (ushort.MinValue, ushort.MinValue);
            }
        }

        /// <summary>
        /// 计算一组 32 位整数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static (int, int) MinMax(params int[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    int Min = values[0];
                    int Max = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        Min = Math.Min(Min, values[i]);
                        Max = Math.Max(Max, values[i]);
                    }

                    return (Min, Max);
                }

                return (0, 0);
            }
            catch
            {
                return (int.MinValue, int.MinValue);
            }
        }

        /// <summary>
        /// 计算一组 32 位无符号整数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static (uint, uint) MinMax(params uint[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    uint Min = values[0];
                    uint Max = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        Min = Math.Min(Min, values[i]);
                        Max = Math.Max(Max, values[i]);
                    }

                    return (Min, Max);
                }

                return (0, 0);
            }
            catch
            {
                return (uint.MinValue, uint.MinValue);
            }
        }

        /// <summary>
        /// 计算一组 64 位整数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static (long, long) MinMax(params long[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    long Min = values[0];
                    long Max = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        Min = Math.Min(Min, values[i]);
                        Max = Math.Max(Max, values[i]);
                    }

                    return (Min, Max);
                }

                return (0, 0);
            }
            catch
            {
                return (long.MinValue, long.MinValue);
            }
        }

        /// <summary>
        /// 计算一组 64 位无符号整数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static (ulong, ulong) MinMax(params ulong[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    ulong Min = values[0];
                    ulong Max = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        Min = Math.Min(Min, values[i]);
                        Max = Math.Max(Max, values[i]);
                    }

                    return (Min, Max);
                }

                return (0, 0);
            }
            catch
            {
                return (ulong.MinValue, ulong.MinValue);
            }
        }

        /// <summary>
        /// 计算一组单精度浮点数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static (float, float) MinMax(params float[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    float Min = values[0];
                    float Max = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        Min = Math.Min(Min, values[i]);
                        Max = Math.Max(Max, values[i]);
                    }

                    return (Min, Max);
                }

                return (0, 0);
            }
            catch
            {
                return (float.NaN, float.NaN);
            }
        }

        /// <summary>
        /// 计算一组双精度浮点数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static (double, double) MinMax(params double[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    double Min = values[0];
                    double Max = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        Min = Math.Min(Min, values[i]);
                        Max = Math.Max(Max, values[i]);
                    }

                    return (Min, Max);
                }

                return (0, 0);
            }
            catch
            {
                return (double.NaN, double.NaN);
            }
        }

        /// <summary>
        /// 计算一组十进制数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static (decimal, decimal) MinMax(params decimal[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    decimal Min = values[0];
                    decimal Max = values[0];

                    for (int i = 0; i < values.Length; i++)
                    {
                        Min = Math.Min(Min, values[i]);
                        Max = Math.Max(Max, values[i]);
                    }

                    return (Min, Max);
                }

                return (0, 0);
            }
            catch
            {
                return (decimal.MinValue, decimal.MinValue);
            }
        }

        #endregion

        #region 统计

        /// <summary>
        /// 计算一组双精度浮点数的求和。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static double Sum(params double[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    double result = 0;

                    for (int i = 0; i < values.Length; i++)
                    {
                        result += values[i];
                    }

                    return result;
                }

                return 0;
            }
            catch
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// 计算一组双精度浮点数的平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static double Average(params double[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    return (Sum(values) / values.Length);
                }

                return 0;
            }
            catch
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// 计算一组双精度浮点数的最小值、最大值与平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static (double, double, double) MinMaxAverage(params double[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    double Min = values[0];
                    double Max = values[0];
                    double Sum = 0;

                    for (int i = 0; i < values.Length; i++)
                    {
                        Min = Math.Min(Min, values[i]);
                        Max = Math.Max(Max, values[i]);
                        Sum += values[i];
                    }

                    return (Min, Max, Sum / values.Length);
                }

                return (0, 0, 0);
            }
            catch
            {
                return (double.NaN, double.NaN, double.NaN);
            }
        }

        //

        /// <summary>
        /// 计算一组双精度浮点数的方差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static double Deviation(params double[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    double Avg = Average(values);
                    double SqrSum = 0;

                    for (int i = 0; i < values.Length; i++)
                    {
                        double Delta = values[i] - Avg;

                        SqrSum += Delta * Delta;
                    }

                    return (SqrSum / values.Length);
                }

                return 0;
            }
            catch
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// 计算一组双精度浮点数的样本方差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static double SampleDeviation(params double[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    double Avg = Average(values);
                    double SqrSum = 0;

                    for (int i = 0; i < values.Length; i++)
                    {
                        double Delta = values[i] - Avg;

                        SqrSum += Delta * Delta;
                    }

                    return (SqrSum / (values.Length - 1));
                }

                return 0;
            }
            catch
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// 计算一组双精度浮点数的标准差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static double StandardDeviation(params double[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    int Len = values.Length;

                    double Sum = 0;
                    double AbsMax = 0;

                    for (int i = 0; i < Len; i++)
                    {
                        Sum += values[i];
                        AbsMax = Math.Max(AbsMax, Math.Abs(values[i]));
                    }

                    double Avg = Sum / Len;
                    double SqrSum = 0;

                    for (int i = 0; i < Len; i++)
                    {
                        double Delta = (values[i] - Avg) / AbsMax;

                        SqrSum += Delta * Delta;
                    }

                    return (Math.Sqrt(AbsMax) * Math.Sqrt(SqrSum / Len));
                }

                return 0;
            }
            catch
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// 计算一组双精度浮点数的样本标准差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        public static double SampleStandardDeviation(params double[] values)
        {
            try
            {
                if (!InternalMethod.IsNullOrEmpty(values))
                {
                    int Len = values.Length;

                    double Sum = 0;
                    double AbsMax = 0;

                    for (int i = 0; i < Len; i++)
                    {
                        Sum += values[i];
                        AbsMax = Math.Max(AbsMax, Math.Abs(values[i]));
                    }

                    double Avg = Sum / Len;
                    double SqrSum = 0;

                    for (int i = 0; i < Len; i++)
                    {
                        double Delta = (values[i] - Avg) / AbsMax;

                        SqrSum += Delta * Delta;
                    }

                    return (Math.Sqrt(AbsMax) * Math.Sqrt(SqrSum / (Len - 1)));
                }

                return 0;
            }
            catch
            {
                return double.NaN;
            }
        }

        #endregion
    }
}