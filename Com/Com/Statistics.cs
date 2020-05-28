/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

Com.Statistics
Version 20.5.28.2000

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

        private static Real _Gamma(double n) // 计算指定双精度浮点数的伽马函数。
        {
            if (InternalMethod.IsNaNOrInfinity(n))
            {
                return Real.NaN;
            }
            else
            {
                Func<int, double> GammaTruncI32 = (trunc) =>
                {
                    double result = 1;

                    int i = 2;

                    while (i < trunc - 8)
                    {
                        result *= ((long)(i * (i + 1) * (i + 2) * (i + 3)) * ((i + 4) * (i + 5) * (i + 6) * (i + 7)));

                        i += 8;
                    }

                    while (i < trunc)
                    {
                        result *= i;

                        i++;
                    }

                    return result;
                };

                Func<long, Real> GammaTruncI64 = (trunc) =>
                {
                    Real result = Real.One;

                    long i = 2;

                    while (i < trunc - 16)
                    {
                        result *= ((double)i * (i + 1) * (i + 2) * (i + 3) * (i + 4) * (i + 5) * (i + 6) * (i + 7) * (i + 8) * (i + 9) * (i + 10) * (i + 11) * (i + 12) * (i + 13) * (i + 14) * (i + 15));

                        i += 16;
                    }

                    while (i < trunc)
                    {
                        result *= i;

                        i++;
                    }

                    return result;
                };

                Func<long, double, Real> GammaTruncI64Mod = (trunc, mod) =>
                {
                    Real result = mod;

                    long i = 1;

                    while (i < trunc - 16)
                    {
                        result *= ((i + mod) * (i + 1 + mod) * (i + 2 + mod) * (i + 3 + mod) * (i + 4 + mod) * (i + 5 + mod) * (i + 6 + mod) * (i + 7 + mod) * (i + 8 + mod) * (i + 9 + mod) * (i + 10 + mod) * (i + 11 + mod) * (i + 12 + mod) * (i + 13 + mod) * (i + 14 + mod) * (i + 15 + mod));

                        i += 16;
                    }

                    while (i < trunc)
                    {
                        result *= (i + mod);

                        i++;
                    }

                    return result;
                };

                Func<double, double> Gamma0To1 = (val) =>
                {
                    double[] Coeff = new double[]
                    {
                        0.99999999999980993,
                        676.5203681218851,
                        -1259.1392167224028,
                        771.32342877765313,
                        -176.61502916214059,
                        12.507343278686905,
                        -0.13857109526572012,
                        9.9843695780195716E-6,
                        1.5056327351493116E-7
                    };

                    double Sum = Coeff[0];

                    for (int i = 1; i < Coeff.Length; i++)
                    {
                        Sum += Coeff[i] / (val + i - 1);
                    }

                    double Base = val + Coeff.Length - 2.5;

                    return (Constant.SqrtDoublePi * Math.Pow(Base, val - 0.5) * Math.Exp(-Base) * Sum);
                };

                Func<double, Real> GammaPositive = (val) =>
                {
                    long Trunc = (long)Math.Truncate(val);
                    double Mod = val - Trunc;

                    return (GammaTruncI64Mod(Trunc, Mod) * Gamma0To1(Mod));
                };

                Func<double, Real> GammaNegative = (val) =>
                {
                    return (Constant.Pi / Math.Sin(Constant.Pi * val) / GammaPositive(1 - val));
                };

                if (n == Math.Truncate(n))
                {
                    if (n == 1 || n == 2)
                    {
                        return Real.One;
                    }
                    else if (n > 2 && n <= 171)
                    {
                        return GammaTruncI32((int)n);
                    }
                    else if (n > 171 && n <= 1E15)
                    {
                        return GammaTruncI64((long)n);
                    }
                    else if (n > 1E15)
                    {
                        return Real.PositiveInfinity;
                    }
                    else
                    {
                        return Real.NaN;
                    }
                }
                else
                {
                    if (n > 0 && n <= 1)
                    {
                        return Gamma0To1(n);
                    }
                    else if (n > 1 && n <= 1E15)
                    {
                        return GammaPositive(n);
                    }
                    else if (n >= 1 - 1E15 && n <= 0)
                    {
                        return GammaNegative(n);
                    }
                    else if (n < 1 - 1E15)
                    {
                        return Real.Zero;
                    }
                    else
                    {
                        return Real.PositiveInfinity;
                    }
                }
            }
        }

        private static Real _Factorial(double n) // 计算指定双精度浮点数的阶乘。
        {
            if (InternalMethod.IsNaNOrInfinity(n))
            {
                return Real.NaN;
            }
            else
            {
                return _Gamma(n + 1);
            }
        }

        private static Real _Arrangement(double total, double selection) // 计算从有限个元素中任取若干个元素的排列数。
        {
            if (InternalMethod.IsNaNOrInfinity(total) || InternalMethod.IsNaNOrInfinity(selection))
            {
                return Real.NaN;
            }
            else
            {
                double TruncTotal = Math.Truncate(total);
                double TruncSelection = Math.Truncate(selection);

                if ((total == TruncTotal && TruncTotal >= 0 && TruncTotal < 1E15) && (selection == TruncSelection && TruncSelection > -1E15 && TruncSelection < 1E15))
                {
                    long TotalI64 = (long)TruncTotal;
                    long SelectionI64 = (long)TruncSelection;
                    long Delta = TotalI64 - SelectionI64;

                    if (Delta < 0)
                    {
                        return Real.Zero;
                    }
                    else
                    {
                        Real result = Real.One;

                        if (TotalI64 > Delta)
                        {
                            long i = TotalI64;

                            while (i > Delta + 16)
                            {
                                result *= ((double)i * (i - 1) * (i - 2) * (i - 3) * (i - 4) * (i - 5) * (i - 6) * (i - 7) * (i - 8) * (i - 9) * (i - 10) * (i - 11) * (i - 12) * (i - 13) * (i - 14) * (i - 15));

                                i -= 16;
                            }

                            while (i > Delta)
                            {
                                result *= i;

                                i--;
                            }
                        }
                        else if (TotalI64 < Delta)
                        {
                            long i = Delta;

                            while (i > TotalI64 + 16)
                            {
                                result /= ((double)i * (i - 1) * (i - 2) * (i - 3) * (i - 4) * (i - 5) * (i - 6) * (i - 7) * (i - 8) * (i - 9) * (i - 10) * (i - 11) * (i - 12) * (i - 13) * (i - 14) * (i - 15));

                                i -= 16;
                            }

                            while (i > TotalI64)
                            {
                                result /= i;

                                i--;
                            }
                        }

                        return result;
                    }
                }
                else if (total >= 0 || total != TruncTotal)
                {
                    double Delta = total - selection;
                    double TruncDelta = Math.Truncate(Delta);

                    if (Delta < 0 && Delta == TruncDelta)
                    {
                        return Real.Zero;
                    }
                    else
                    {
                        return (_Factorial(total) / _Factorial(Delta));
                    }
                }
                else
                {
                    return Real.NaN;
                }
            }
        }

        private static Real _Combination(double total, double selection) // 计算从有限个元素中任取若干个元素的组合数。
        {
            if (InternalMethod.IsNaNOrInfinity(total) || InternalMethod.IsNaNOrInfinity(selection))
            {
                return Real.NaN;
            }
            else
            {
                double TruncTotal = Math.Truncate(total);
                double TruncSelection = Math.Truncate(selection);

                if ((total == TruncTotal && TruncTotal >= 0 && TruncTotal < 1E15) && (selection == TruncSelection && TruncSelection > -1E15 && TruncSelection < 1E15))
                {
                    long TotalI64 = (long)TruncTotal;
                    long SelectionI64 = (long)TruncSelection;

                    if (SelectionI64 < 0)
                    {
                        return Real.Zero;
                    }
                    else
                    {
                        long Delta = TotalI64 - SelectionI64;

                        if (Delta < 0)
                        {
                            return Real.Zero;
                        }
                        else
                        {
                            Real result = Real.One;

                            if (TotalI64 > Delta)
                            {
                                long i = TotalI64;

                                while (i > Delta + 16)
                                {
                                    result *= ((double)i * (i - 1) * (i - 2) * (i - 3) * (i - 4) * (i - 5) * (i - 6) * (i - 7) * (i - 8) * (i - 9) * (i - 10) * (i - 11) * (i - 12) * (i - 13) * (i - 14) * (i - 15));

                                    i -= 16;
                                }

                                while (i > Delta)
                                {
                                    result *= i;

                                    i--;
                                }
                            }
                            else if (TotalI64 < Delta)
                            {
                                long i = Delta;

                                while (i > TotalI64 + 16)
                                {
                                    result /= ((double)i * (i - 1) * (i - 2) * (i - 3) * (i - 4) * (i - 5) * (i - 6) * (i - 7) * (i - 8) * (i - 9) * (i - 10) * (i - 11) * (i - 12) * (i - 13) * (i - 14) * (i - 15));

                                    i -= 16;
                                }

                                while (i > TotalI64)
                                {
                                    result /= i;

                                    i--;
                                }
                            }

                            if (!result.IsInfinity && result != Real.Zero)
                            {
                                long i = SelectionI64;

                                while (i > 1 + 16)
                                {
                                    result /= ((double)i * (i - 1) * (i - 2) * (i - 3) * (i - 4) * (i - 5) * (i - 6) * (i - 7) * (i - 8) * (i - 9) * (i - 10) * (i - 11) * (i - 12) * (i - 13) * (i - 14) * (i - 15));

                                    i -= 16;
                                }

                                while (i > 1)
                                {
                                    result /= i;

                                    i--;
                                }
                            }

                            return result;
                        }
                    }
                }
                else if (total >= 0 || total != TruncTotal)
                {
                    if (selection < 0 && selection == TruncSelection)
                    {
                        return Real.Zero;
                    }
                    else
                    {
                        double Delta = total - selection;
                        double TruncDelta = Math.Truncate(Delta);

                        if (Delta < 0 && Delta == TruncDelta)
                        {
                            return Real.Zero;
                        }
                        else
                        {
                            return (_Factorial(total) / _Factorial(selection) / _Factorial(Delta));
                        }
                    }
                }
                else
                {
                    return Real.NaN;
                }
            }
        }

        #endregion

        #region 随机数

        /// <summary>
        /// 返回一个概率密度平均分布的非负随机 32 位整数。
        /// </summary>
        /// <returns>32 位整数，表示概率密度平均分布的非负随机数。</returns>
        public static int RandomInteger()
        {
            return _Rand.Next();
        }

        /// <summary>
        /// 返回一个在 0 与右端点指定的区间内概率密度平均分布的非负随机 32 位整数。
        /// </summary>
        /// <param name="right">区间右端点（不含）。</param>
        /// <returns>32 位整数，表示概率密度平均分布的非负随机数。</returns>
        public static int RandomInteger(int right)
        {
            if (right < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (right == 0)
            {
                return 0;
            }
            else
            {
                return _Rand.Next(right);
            }
        }

        /// <summary>
        /// 返回一个在指定区间内概率密度平均分布的随机 32 位整数。
        /// </summary>
        /// <param name="left">区间左端点（含）。</param>
        /// <param name="right">区间右端点（不含）。</param>
        /// <returns>32 位整数，表示概率密度平均分布的随机数。</returns>
        public static int RandomInteger(int left, int right)
        {
            if (left > right)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (left == right)
            {
                return left;
            }
            else
            {
                return _Rand.Next(left, right);
            }
        }

        //

        /// <summary>
        /// 返回一个小于 1 的概率密度平均分布的非负随机双精度浮点数。
        /// </summary>
        /// <returns>双精度浮点数，表示概率密度平均分布的非负随机数。</returns>
        public static double RandomDouble()
        {
            return _Rand.NextDouble();
        }

        /// <summary>
        /// 返回一个在 0 与右端点指定的区间内概率密度平均分布的非负随机双精度浮点数。
        /// </summary>
        /// <param name="right">区间右端点（不含）。</param>
        /// <returns>双精度浮点数，表示概率密度平均分布的非负随机数。</returns>
        public static double RandomDouble(double right)
        {
            if (InternalMethod.IsNaNOrInfinity(right) || right < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (right == 0)
            {
                return 0;
            }
            else
            {
                return (right * _Rand.NextDouble());
            }
        }

        /// <summary>
        /// 返回一个在指定区间内概率密度平均分布的随机双精度浮点数。
        /// </summary>
        /// <param name="left">区间左端点（含）。</param>
        /// <param name="right">区间右端点（不含）。</param>
        /// <returns>双精度浮点数，表示概率密度平均分布的随机数。</returns>
        public static double RandomDouble(double left, double right)
        {
            if (InternalMethod.IsNaNOrInfinity(left) || InternalMethod.IsNaNOrInfinity(right) || left > right)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (left == right)
            {
                return left;
            }
            else
            {
                return (left + (right - left) * _Rand.NextDouble());
            }
        }

        //

        /// <summary>
        /// 返回一个概率密度服从标准正态分布的随机 32 位整数。
        /// </summary>
        /// <returns>32 位整数，表示概率密度服从标准正态分布的随机数。</returns>
        public static int NormalDistributionRandomInteger()
        {
            return (int)Math.Round(_StdNormalRandom()[0]);
        }

        /// <summary>
        /// 返回一个概率密度服从正态分布的随机 32 位整数。
        /// </summary>
        /// <param name="ev">数学期望。</param>
        /// <param name="sd">标准差。</param>
        /// <returns>32 位整数，表示概率密度服从正态分布的随机数。</returns>
        public static int NormalDistributionRandomInteger(double ev, double sd)
        {
            if (InternalMethod.IsNaNOrInfinity(ev) || InternalMethod.IsNaNOrInfinity(sd))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return (int)Math.Round(_StdNormalRandom()[0] * sd * sd + ev);
        }

        /// <summary>
        /// 返回一个概率密度服从标准正态分布的随机双精度浮点数。
        /// </summary>
        /// <returns>双精度浮点数，表示概率密度服从标准正态分布的随机数。</returns>
        public static double NormalDistributionRandomDouble()
        {
            return _StdNormalRandom()[0];
        }

        /// <summary>
        /// 返回一个概率密度服从正态分布的随机双精度浮点数。
        /// </summary>
        /// <param name="ev">数学期望。</param>
        /// <param name="sd">标准差。</param>
        /// <returns>双精度浮点数，表示概率密度服从正态分布的随机数。</returns>
        public static double NormalDistributionRandomDouble(double ev, double sd)
        {
            if (InternalMethod.IsNaNOrInfinity(ev) || InternalMethod.IsNaNOrInfinity(sd))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return (_StdNormalRandom()[0] * sd * sd + ev);
        }

        #endregion

        #region 排列组合

        /// <summary>
        /// 计算从有限个元素中任取若干个元素的排列数。
        /// </summary>
        /// <param name="total">元素总数。</param>
        /// <param name="selection">抽取的元素数量。</param>
        /// <returns>双精度浮点数，表示从有限个元素中任取若干个元素的排列数。</returns>
        public static double Arrangement(double total, double selection)
        {
            if (InternalMethod.IsNaNOrInfinity(total) || InternalMethod.IsNaNOrInfinity(selection))
            {
                return double.NaN;
            }
            else
            {
                return (double)_Arrangement(total, selection);
            }
        }

        /// <summary>
        /// 计算从有限个元素中任取若干个元素的组合数。
        /// </summary>
        /// <param name="total">元素总数。</param>
        /// <param name="selection">抽取的元素数量。</param>
        /// <returns>双精度浮点数，表示从有限个元素中任取若干个元素的组合数。</returns>
        public static double Combination(double total, double selection)
        {
            if (InternalMethod.IsNaNOrInfinity(total) || InternalMethod.IsNaNOrInfinity(selection))
            {
                return double.NaN;
            }
            else
            {
                return (double)_Combination(total, selection);
            }
        }

        #endregion

        #region 分布

        /// <summary>
        /// 计算服从几何分布的随机变量在指定分布参数的概率。
        /// </summary>
        /// <param name="value">测试的样本数量。</param>
        /// <param name="p">单个样本满足某个条件的概率，等于数学期望的倒数。</param>
        /// <returns>双精度浮点数，表示服从几何分布的随机变量在指定分布参数的概率。</returns>
        public static double GeometricDistributionProbability(int value, double p)
        {
            if (InternalMethod.IsNaNOrInfinity(p))
            {
                return double.NaN;
            }
            else
            {
                if (p >= 0 && p <= 1)
                {
                    if (p == 1)
                    {
                        if (value > 0)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else if (p > 0)
                    {
                        if (value > 0)
                        {
                            return (p * Math.Pow(1 - p, value - 1));
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
                else
                {
                    return double.NaN;
                }
            }
        }

        /// <summary>
        /// 计算服从超几何分布的随机变量在指定分布参数的概率。
        /// </summary>
        /// <param name="value">测试的样本中满足某个条件的样本数量。</param>
        /// <param name="N">样本总量。</param>
        /// <param name="M">满足某个条件的样本数量。</param>
        /// <param name="n">测试的样本数量。</param>
        /// <returns>双精度浮点数，表示服从超几何分布的随机变量在指定分布参数的概率。</returns>
        public static double HypergeometricDistributionProbability(int value, int N, int M, int n)
        {
            if (N <= 0 || M >= N)
            {
                return double.NaN;
            }
            else
            {
                if (M > 0 && n > 0 && (value >= 0 && value <= n))
                {
                    return (double)(_Combination(M, value) / _Combination(N, n) * _Combination(N - M, n - value));
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 计算服从二项分布的随机变量在指定分布参数的概率。
        /// </summary>
        /// <param name="value">测试的样本数量。</param>
        /// <param name="n">样本总量。</param>
        /// <param name="p">单个样本满足某个条件的概率，等于样本总量为 1 时的数学期望。</param>
        /// <returns>双精度浮点数，表示服从二项分布的随机变量在指定分布参数的概率。</returns>
        public static double BinomialDistributionProbability(int value, int n, double p)
        {
            if (InternalMethod.IsNaNOrInfinity(p))
            {
                return double.NaN;
            }
            else
            {
                if (n > 0 && (p >= 0 && p <= 1))
                {
                    if (value >= 0 && value <= n)
                    {
                        if (p == 1)
                        {
                            return 1;
                        }
                        else if (p > 0)
                        {
                            return (double)(_Combination(n, value) * Real.Pow(p, value) * Real.Pow(1 - p, n - value));
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
                else
                {
                    return double.NaN;
                }
            }
        }

        /// <summary>
        /// 计算服从泊松分布的随机变量在指定分布参数的概率。
        /// </summary>
        /// <param name="value">单位测度内测试的样本数量。</param>
        /// <param name="lambda">单位测度内满足某个条件的平均样本数量，等于数学期望或方差。</param>
        /// <returns>双精度浮点数，表示服从泊松分布的随机变量在指定分布参数的概率。</returns>
        public static double PoissonDistributionProbability(int value, double lambda)
        {
            if (InternalMethod.IsNaNOrInfinity(lambda))
            {
                return double.NaN;
            }
            else
            {
                if (lambda > 0)
                {
                    if (value >= 0)
                    {
                        return (double)(Real.Pow(lambda, value) * Real.Exp(-lambda) / _Factorial(value));
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return double.NaN;
                }
            }
        }

        //

        /// <summary>
        /// 计算服从指数分布的随机变量在指定分布参数的概率密度。
        /// </summary>
        /// <param name="value">样本值。</param>
        /// <param name="lambda">率参数，等于数学期望的倒数或标准差的倒数。</param>
        /// <returns>双精度浮点数，表示服从指数分布的随机变量在指定分布参数的概率密度。</returns>
        public static double ExponentialDistributionProbabilityDensity(double value, double lambda)
        {
            if (InternalMethod.IsNaNOrInfinity(value) || InternalMethod.IsNaNOrInfinity(lambda))
            {
                return double.NaN;
            }
            else
            {
                if (lambda > 0)
                {
                    if (value >= 0)
                    {
                        return (double)(lambda * Real.Exp((Real)(-lambda) * value));
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return double.NaN;
                }
            }
        }

        /// <summary>
        /// 计算服从指数分布的随机变量在指定分布参数的在指定区间的概率。
        /// </summary>
        /// <param name="lambda">率参数，等于数学期望的倒数或标准差的倒数。</param>
        /// <param name="left">区间左端点。</param>
        /// <param name="right">区间右端点。</param>
        /// <returns>双精度浮点数，表示服从指数分布的随机变量在指定分布参数的在指定区间的概率。</returns>
        public static double ExponentialDistributionProbability(double lambda, double left, double right)
        {
            if (InternalMethod.IsNaNOrInfinity(lambda) || InternalMethod.IsNaNOrInfinity(left) || InternalMethod.IsNaNOrInfinity(right) || left > right)
            {
                return double.NaN;
            }
            else
            {
                if (lambda > 0)
                {
                    Func<double, double> Prim = (val) =>
                    {
                        if (val <= 0)
                        {
                            return 0;
                        }
                        else
                        {
                            return (-Math.Exp(-lambda * val));
                        }
                    };

                    return (Prim(right) - Prim(left));
                }
                else
                {
                    return double.NaN;
                }
            }
        }

        /// <summary>
        /// 计算服从标准正态分布的随机变量在指定分布参数的概率密度。
        /// </summary>
        /// <param name="value">样本值。</param>
        /// <returns>双精度浮点数，表示服从标准正态分布的随机变量在指定分布参数的概率密度。</returns>
        public static double NormalDistributionProbabilityDensity(double value)
        {
            if (InternalMethod.IsNaNOrInfinity(value))
            {
                return double.NaN;
            }
            else
            {
                return (Math.Exp(-0.5 * value * value) / Constant.SqrtDoublePi);
            }
        }

        /// <summary>
        /// 计算服从正态分布的随机变量在指定分布参数的概率密度。
        /// </summary>
        /// <param name="value">样本值。</param>
        /// <param name="ev">数学期望。</param>
        /// <param name="sd">标准差。</param>
        /// <returns>双精度浮点数，表示服从正态分布的随机变量在指定分布参数的概率密度。</returns>
        public static double NormalDistributionProbabilityDensity(double value, double ev, double sd)
        {
            if (InternalMethod.IsNaNOrInfinity(value) || InternalMethod.IsNaNOrInfinity(ev) || InternalMethod.IsNaNOrInfinity(sd))
            {
                return double.NaN;
            }
            else
            {
                Real N = ((Real)value - ev) / sd;

                return (double)(Real.Exp(-0.5 * N * N) / Constant.SqrtDoublePi / sd);
            }
        }

        /// <summary>
        /// 计算服从卡方分布的随机变量在指定分布参数的概率密度。
        /// </summary>
        /// <param name="value">样本值。</param>
        /// <param name="k">自由度。</param>
        /// <returns>双精度浮点数，表示服从卡方分布的随机变量在指定分布参数的概率密度。</returns>
        public static double ChiSquaredDistributionProbabilityDensity(double value, int k)
        {
            if (InternalMethod.IsNaNOrInfinity(value))
            {
                return double.NaN;
            }
            else
            {
                if (k > 0)
                {
                    if (value > 0)
                    {
                        double HalfK = k * 0.5;

                        return (double)(Real.Pow(value, (Real)HalfK - 1) * Real.Exp((Real)(-value) / 2) / Real.Pow(2, HalfK) / _Gamma(HalfK));
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return double.NaN;
                }
            }
        }

        #endregion

        #region 极值，极差，求和，平均

        /// <summary>
        /// 计算一组 8 位整数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>8 位整数，表示一组 8 位整数的最大值。</returns>
        public static sbyte Max(params sbyte[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                sbyte result = values[0];

                sbyte val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result < val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 8 位无符号整数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>8 位无符号整数，表示一组 8 位无符号整数的最大值。</returns>
        public static byte Max(params byte[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                byte result = values[0];

                byte val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result < val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 16 位整数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>16 位整数，表示一组 16 位整数的最大值。</returns>
        public static short Max(params short[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                short result = values[0];

                short val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result < val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 16 位无符号整数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>16 位无符号整数，表示一组 16 位无符号整数的最大值。</returns>
        public static ushort Max(params ushort[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                ushort result = values[0];

                ushort val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result < val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 32 位整数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>32 位整数，表示一组 32 位整数的最大值。</returns>
        public static int Max(params int[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                int result = values[0];

                int val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result < val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 32 位无符号整数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>32 位无符号整数，表示一组 32 位无符号整数的最大值。</returns>
        public static uint Max(params uint[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                uint result = values[0];

                uint val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result < val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 64 位整数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>64 位整数，表示一组 64 位整数的最大值。</returns>
        public static long Max(params long[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                long result = values[0];

                long val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result < val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 64 位无符号整数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>64 位无符号整数，表示一组 64 位无符号整数的最大值。</returns>
        public static ulong Max(params ulong[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                ulong result = values[0];

                ulong val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result < val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组十进制浮点数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>十进制浮点数，表示一组十进制浮点数的最大值。</returns>
        public static decimal Max(params decimal[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                decimal result = values[0];

                decimal val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result < val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组单精度浮点数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>单精度浮点数，表示一组单精度浮点数的最大值。</returns>
        public static float Max(params float[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                float result = values[0];

                float val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result < val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组双精度浮点数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组双精度浮点数的最大值。</returns>
        public static double Max(params double[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                double result = values[0];

                double val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result < val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组可排序对象的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>IComparable 对象，表示一组可排序对象的最大值。</returns>
        public static T Max<T>(params T[] values) where T : IComparable
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                T result = values[0];

                T val;

                if (default(T) == null)
                {
                    for (int i = 1; i < Len; i++)
                    {
                        val = values[i];

                        if (val != null && (result == null || val.CompareTo(result) > 0))
                        {
                            result = val;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < Len; i++)
                    {
                        val = values[i];

                        if (val.CompareTo(result) > 0)
                        {
                            result = val;
                        }
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组可排序对象的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>IComparable 对象，表示一组可排序对象的最大值。</returns>
        [Obsolete]
        public static IComparable Max(params IComparable[] values)
        {
            return Max<IComparable>(values);
        }

        //

        /// <summary>
        /// 计算一组 8 位整数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>8 位整数，表示一组 8 位整数的最小值。</returns>
        public static sbyte Min(params sbyte[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                sbyte result = values[0];

                sbyte val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result > val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 8 位无符号整数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>8 位无符号整数，表示一组 8 位无符号整数的最小值。</returns>
        public static byte Min(params byte[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                byte result = values[0];

                byte val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result > val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 16 位整数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>16 位整数，表示一组 16 位整数的最小值。</returns>
        public static short Min(params short[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                short result = values[0];

                short val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result > val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 16 位无符号整数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>16 位无符号整数，表示一组 16 位无符号整数的最小值。</returns>
        public static ushort Min(params ushort[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                ushort result = values[0];

                ushort val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result > val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 32 位整数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>32 位整数，表示一组 32 位整数的最小值。</returns>
        public static int Min(params int[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                int result = values[0];

                int val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result > val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 32 位无符号整数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>32 位无符号整数，表示一组 32 位无符号整数的最小值。</returns>
        public static uint Min(params uint[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                uint result = values[0];

                uint val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result > val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 64 位整数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>64 位整数，表示一组 64 位整数的最小值。</returns>
        public static long Min(params long[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                long result = values[0];

                long val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result > val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 64 位无符号整数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>64 位无符号整数，表示一组 64 位无符号整数的最小值。</returns>
        public static ulong Min(params ulong[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                ulong result = values[0];

                ulong val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result > val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组十进制浮点数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>十进制浮点数，表示一组十进制浮点数的最小值。</returns>
        public static decimal Min(params decimal[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                decimal result = values[0];

                decimal val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result > val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组单精度浮点数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>单精度浮点数，表示一组单精度浮点数的最小值。</returns>
        public static float Min(params float[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                float result = values[0];

                float val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result > val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组双精度浮点数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组双精度浮点数的最小值。</returns>
        public static double Min(params double[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                double result = values[0];

                double val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (result > val)
                    {
                        result = val;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组可排序对象的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>IComparable 对象，表示一组可排序对象的最小值。</returns>
        public static T Min<T>(params T[] values) where T : IComparable
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                T result = values[0];

                T val;

                if (default(T) == null)
                {
                    for (int i = 1; i < Len; i++)
                    {
                        val = values[i];

                        if (val != null && (result == null || val.CompareTo(result) < 0))
                        {
                            result = val;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < Len; i++)
                    {
                        val = values[i];

                        if (val.CompareTo(result) < 0)
                        {
                            result = val;
                        }
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组可排序对象的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>IComparable 对象，表示一组可排序对象的最小值。</returns>
        [Obsolete]
        public static IComparable Min(params IComparable[] values)
        {
            return Min<IComparable>(values);
        }

        //

        /// <summary>
        /// 计算一组 8 位整数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(sbyte, sbyte) 元组，表示一组 8 位整数的最小值与最大值。</returns>
        public static (sbyte Min, sbyte Max) MinMax(params sbyte[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                sbyte min = values[0];
                sbyte max = values[0];

                sbyte val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }
                }

                return (min, max);
            }
            else
            {
                sbyte val = values[0];

                return (val, val);
            }
        }

        /// <summary>
        /// 计算一组 8 位无符号整数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(byte, byte) 元组，表示一组 8 位无符号整数的最小值与最大值。</returns>
        public static (byte Min, byte Max) MinMax(params byte[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                byte min = values[0];
                byte max = values[0];

                byte val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }
                }

                return (min, max);
            }
            else
            {
                byte val = values[0];

                return (val, val);
            }
        }

        /// <summary>
        /// 计算一组 16 位整数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(short, short) 元组，表示一组 16 位整数的最小值与最大值。</returns>
        public static (short Min, short Max) MinMax(params short[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                short min = values[0];
                short max = values[0];

                short val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }
                }

                return (min, max);
            }
            else
            {
                short val = values[0];

                return (val, val);
            }
        }

        /// <summary>
        /// 计算一组 16 位无符号整数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(ushort, ushort) 元组，表示一组 16 位无符号整数的最小值与最大值。</returns>
        public static (ushort Min, ushort Max) MinMax(params ushort[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                ushort min = values[0];
                ushort max = values[0];

                ushort val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }
                }

                return (min, max);
            }
            else
            {
                ushort val = values[0];

                return (val, val);
            }
        }

        /// <summary>
        /// 计算一组 32 位整数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(int, int) 元组，表示一组 32 位整数的最小值与最大值。</returns>
        public static (int Min, int Max) MinMax(params int[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                int min = values[0];
                int max = values[0];

                int val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }
                }

                return (min, max);
            }
            else
            {
                int val = values[0];

                return (val, val);
            }
        }

        /// <summary>
        /// 计算一组 32 位无符号整数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(uint, uint) 元组，表示一组 32 位无符号整数的最小值与最大值。</returns>
        public static (uint Min, uint Max) MinMax(params uint[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                uint min = values[0];
                uint max = values[0];

                uint val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }
                }

                return (min, max);
            }
            else
            {
                uint val = values[0];

                return (val, val);
            }
        }

        /// <summary>
        /// 计算一组 64 位整数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(long, long) 元组，表示一组 64 位整数的最小值与最大值。</returns>
        public static (long Min, long Max) MinMax(params long[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                long min = values[0];
                long max = values[0];

                long val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }
                }

                return (min, max);
            }
            else
            {
                long val = values[0];

                return (val, val);
            }
        }

        /// <summary>
        /// 计算一组 64 位无符号整数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(ulong, ulong) 元组，表示一组 64 位无符号整数的最小值与最大值。</returns>
        public static (ulong Min, ulong Max) MinMax(params ulong[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                ulong min = values[0];
                ulong max = values[0];

                ulong val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }
                }

                return (min, max);
            }
            else
            {
                ulong val = values[0];

                return (val, val);
            }
        }

        /// <summary>
        /// 计算一组十进制浮点数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(decimal, decimal) 元组，表示一组十进制浮点数的最小值与最大值。</returns>
        public static (decimal Min, decimal Max) MinMax(params decimal[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                decimal min = values[0];
                decimal max = values[0];

                decimal val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }
                }

                return (min, max);
            }
            else
            {
                decimal val = values[0];

                return (val, val);
            }
        }

        /// <summary>
        /// 计算一组单精度浮点数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(float, float) 元组，表示一组单精度浮点数的最小值与最大值。</returns>
        public static (float Min, float Max) MinMax(params float[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                float min = values[0];
                float max = values[0];

                float val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }
                }

                return (min, max);
            }
            else
            {
                float val = values[0];

                return (val, val);
            }
        }

        /// <summary>
        /// 计算一组双精度浮点数的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(double, double) 元组，表示一组双精度浮点数的最小值与最大值。</returns>
        public static (double Min, double Max) MinMax(params double[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                double min = values[0];
                double max = values[0];

                double val;

                for (int i = 1; i < Len; i++)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }
                }

                return (min, max);
            }
            else
            {
                double val = values[0];

                return (val, val);
            }
        }

        /// <summary>
        /// 计算一组可排序对象的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(IComparable, IComparable) 元组，表示一组可排序对象的最小值与最大值。</returns>
        public static (T Min, T Max) MinMax<T>(params T[] values) where T : IComparable
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                T min = values[0];
                T max = values[0];

                T val;

                if (default(T) == null)
                {
                    for (int i = 1; i < Len; i++)
                    {
                        val = values[i];

                        if (val != null)
                        {
                            if (min == null || val.CompareTo(min) < 0)
                            {
                                min = val;
                            }

                            if (max == null || val.CompareTo(max) > 0)
                            {
                                max = val;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < Len; i++)
                    {
                        val = values[i];

                        if (val.CompareTo(min) < 0)
                        {
                            min = val;
                        }

                        if (val.CompareTo(max) > 0)
                        {
                            max = val;
                        }
                    }
                }

                return (min, max);
            }
            else
            {
                T val = values[0];

                return (val, val);
            }
        }

        /// <summary>
        /// 计算一组可排序对象的最小值与最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(IComparable, IComparable) 元组，表示一组可排序对象的最小值与最大值。</returns>
        [Obsolete]
        public static (IComparable Min, IComparable Max) MinMax(params IComparable[] values)
        {
            return MinMax<IComparable>(values);
        }

        //

        /// <summary>
        /// 计算一组 8 位整数的极差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>32 位整数，表示一组 8 位整数的极差。</returns>
        public static int Range(params sbyte[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            (int Min, int Max) = MinMax(values);

            return (Max - Min);
        }

        /// <summary>
        /// 计算一组 8 位无符号整数的极差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>32 位整数，表示一组 8 位无符号整数的极差。</returns>
        public static int Range(params byte[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            (int Min, int Max) = MinMax(values);

            return (Max - Min);
        }

        /// <summary>
        /// 计算一组 16 位整数的极差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>32 位整数，表示一组 16 位整数的极差。</returns>
        public static int Range(params short[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            (int Min, int Max) = MinMax(values);

            return (Max - Min);
        }

        /// <summary>
        /// 计算一组 16 位无符号整数的极差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>32 位整数，表示一组 16 位无符号整数的极差。</returns>
        public static int Range(params ushort[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            (int Min, int Max) = MinMax(values);

            return (Max - Min);
        }

        /// <summary>
        /// 计算一组 32 位整数的极差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>32 位整数，表示一组 32 位整数的极差。</returns>
        public static int Range(params int[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            (long Min, long Max) = MinMax(values);

            long result = Max - Min;

            if (result < int.MinValue || result > int.MaxValue)
            {
                throw new OverflowException();
            }

            return (int)result;
        }

        /// <summary>
        /// 计算一组 32 位无符号整数的极差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>64 位整数，表示一组 32 位无符号整数的极差。</returns>
        public static long Range(params uint[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            (long Min, long Max) = MinMax(values);

            return (Max - Min);
        }

        /// <summary>
        /// 计算一组 64 位整数的极差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>64 位整数，表示一组 64 位整数的极差。</returns>
        public static long Range(params long[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            (decimal Min, decimal Max) = MinMax(values);

            decimal result = Max - Min;

            if (result < long.MinValue || result > long.MaxValue)
            {
                throw new OverflowException();
            }

            return (long)result;
        }

        /// <summary>
        /// 计算一组 64 位无符号整数的极差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组 64 位无符号整数的极差。</returns>
        public static double Range(params ulong[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            (double Min, double Max) = MinMax(values);

            return (Max - Min);
        }

        /// <summary>
        /// 计算一组十进制浮点数的极差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>十进制浮点数，表示一组十进制浮点数的极差。</returns>
        public static decimal Range(params decimal[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            (decimal Min, decimal Max) = MinMax(values);

            return (Max - Min);
        }

        /// <summary>
        /// 计算一组单精度浮点数的极差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>单精度浮点数，表示一组单精度浮点数的极差。</returns>
        public static float Range(params float[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            (float Min, float Max) = MinMax(values);

            return (Max - Min);
        }

        /// <summary>
        /// 计算一组双精度浮点数的极差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组双精度浮点数的极差。</returns>
        public static double Range(params double[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            (double Min, double Max) = MinMax(values);

            return (Max - Min);
        }

        //

        /// <summary>
        /// 计算一组 8 位整数的求和。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>32 位整数，表示一组 8 位整数的求和。</returns>
        public static int Sum(params sbyte[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                int result = 0;

                checked
                {
                    int i = 0;

                    while (i < Len - 16)
                    {
                        result += ((int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                        i += 16;
                    }

                    while (i < Len)
                    {
                        result += values[i];

                        i++;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 8 位无符号整数的求和。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>32 位整数，表示一组 8 位无符号整数的求和。</returns>
        public static int Sum(params byte[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                int result = 0;

                checked
                {
                    int i = 0;

                    while (i < Len - 16)
                    {
                        result += ((int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                        i += 16;
                    }

                    while (i < Len)
                    {
                        result += values[i];

                        i++;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 16 位整数的求和。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>32 位整数，表示一组 16 位整数的求和。</returns>
        public static int Sum(params short[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                int result = 0;

                checked
                {
                    int i = 0;

                    while (i < Len - 16)
                    {
                        result += ((int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                        i += 16;
                    }

                    while (i < Len)
                    {
                        result += values[i];

                        i++;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 16 位无符号整数的求和。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>32 位整数，表示一组 16 位无符号整数的求和。</returns>
        public static int Sum(params ushort[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                int result = 0;

                checked
                {
                    int i = 0;

                    while (i < Len - 16)
                    {
                        result += ((int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                        i += 16;
                    }

                    while (i < Len)
                    {
                        result += values[i];

                        i++;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 32 位整数的求和。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>32 位整数，表示一组 32 位整数的求和。</returns>
        public static int Sum(params int[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                int result = 0;

                checked
                {
                    int i = 0;

                    while (i < Len - 16)
                    {
                        result += (values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                        i += 16;
                    }

                    while (i < Len)
                    {
                        result += values[i];

                        i++;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 32 位无符号整数的求和。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>64 位整数，表示一组 32 位无符号整数的求和。</returns>
        public static long Sum(params uint[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                long result = 0;

                checked
                {
                    int i = 0;

                    while (i < Len - 16)
                    {
                        result += ((long)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                        i += 16;
                    }

                    while (i < Len)
                    {
                        result += values[i];

                        i++;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 64 位整数的求和。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>64 位整数，表示一组 64 位整数的求和。</returns>
        public static long Sum(params long[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                long result = 0;

                checked
                {
                    int i = 0;

                    while (i < Len - 16)
                    {
                        result += (values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                        i += 16;
                    }

                    while (i < Len)
                    {
                        result += values[i];

                        i++;
                    }
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 64 位无符号整数的求和。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组 64 位无符号整数的求和。</returns>
        public static double Sum(params ulong[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                double result = 0;

                int i = 0;

                while (i < Len - 16)
                {
                    result += ((double)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    result += values[i];

                    i++;
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组十进制浮点数的求和。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>十进制浮点数，表示一组十进制浮点数的求和。</returns>
        public static decimal Sum(params decimal[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                decimal result = 0;

                int i = 0;

                while (i < Len - 16)
                {
                    result += (values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    result += values[i];

                    i++;
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组单精度浮点数的求和。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>单精度浮点数，表示一组单精度浮点数的求和。</returns>
        public static float Sum(params float[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                float result = 0;

                int i = 0;

                while (i < Len - 16)
                {
                    result += (values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    result += values[i];

                    i++;
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组双精度浮点数的求和。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组双精度浮点数的求和。</returns>
        public static double Sum(params double[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                double result = 0;

                int i = 0;

                while (i < Len - 16)
                {
                    result += (values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    result += values[i];

                    i++;
                }

                return result;
            }
            else
            {
                return values[0];
            }
        }

        //

        /// <summary>
        /// 计算一组 8 位整数的平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组 8 位整数的平均值。</returns>
        public static double Average(params sbyte[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < Len - 16)
                {
                    sum += ((int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    sum += values[i];

                    i++;
                }

                return (sum / Len);
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 8 位无符号整数的平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组 8 位无符号整数的平均值。</returns>
        public static double Average(params byte[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < Len - 16)
                {
                    sum += ((int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    sum += values[i];

                    i++;
                }

                return (sum / Len);
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 16 位整数的平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组 16 位整数的平均值。</returns>
        public static double Average(params short[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < Len - 16)
                {
                    sum += ((int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    sum += values[i];

                    i++;
                }

                return (sum / Len);
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 16 位无符号整数的平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组 16 位无符号整数的平均值。</returns>
        public static double Average(params ushort[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < Len - 16)
                {
                    sum += ((int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    sum += values[i];

                    i++;
                }

                return (sum / Len);
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 32 位整数的平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组 32 位整数的平均值。</returns>
        public static double Average(params int[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < Len - 16)
                {
                    sum += ((long)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    sum += values[i];

                    i++;
                }

                return (sum / Len);
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 32 位无符号整数的平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组 32 位无符号整数的平均值。</returns>
        public static double Average(params uint[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < Len - 16)
                {
                    sum += ((long)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    sum += values[i];

                    i++;
                }

                return (sum / Len);
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 64 位整数的平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组 64 位整数的平均值。</returns>
        public static double Average(params long[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < Len - 16)
                {
                    sum += ((double)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    sum += values[i];

                    i++;
                }

                return (sum / Len);
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组 64 位无符号整数的平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组 64 位无符号整数的平均值。</returns>
        public static double Average(params ulong[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < Len - 16)
                {
                    sum += ((double)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    sum += values[i];

                    i++;
                }

                return (sum / Len);
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组十进制浮点数的平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>十进制浮点数，表示一组十进制浮点数的平均值。</returns>
        public static decimal Average(params decimal[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                decimal sum = 0;

                int i = 0;

                while (i < Len - 16)
                {
                    sum += (values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    sum += values[i];

                    i++;
                }

                return (sum / Len);
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组单精度浮点数的平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>单精度浮点数，表示一组单精度浮点数的平均值。</returns>
        public static float Average(params float[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < Len - 16)
                {
                    sum += ((double)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    sum += values[i];

                    i++;
                }

                return (float)(sum / Len);
            }
            else
            {
                return values[0];
            }
        }

        /// <summary>
        /// 计算一组双精度浮点数的平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组双精度浮点数的平均值。</returns>
        public static double Average(params double[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < Len - 16)
                {
                    sum += (values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    sum += values[i];

                    i++;
                }

                return (sum / Len);
            }
            else
            {
                return values[0];
            }
        }

        //

        /// <summary>
        /// 计算一组 8 位整数的最小值、最大值与平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(sbyte, sbyte, double) 元组，表示一组 8 位整数的最小值、最大值与平均值。</returns>
        public static (sbyte Min, sbyte Max, double Average) MinMaxAverage(params sbyte[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                sbyte min = values[0];
                sbyte max = values[0];
                double sum = 0;

                int i = 0;

                sbyte val;

                while (i < Len - 16)
                {
                    for (int j = i; j < i + 16; j++)
                    {
                        val = values[j];

                        if (min > val)
                        {
                            min = val;
                        }

                        if (max < val)
                        {
                            max = val;
                        }
                    }

                    sum += ((int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }

                    sum += values[i];

                    i++;
                }

                return (min, max, sum / Len);
            }
            else
            {
                sbyte val = values[0];

                return (val, val, val);
            }
        }

        /// <summary>
        /// 计算一组 8 位无符号整数的最小值、最大值与平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(byte, byte, double) 元组，表示一组 8 位无符号整数的最小值、最大值与平均值。</returns>
        public static (byte Min, byte Max, double Average) MinMaxAverage(params byte[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                byte min = values[0];
                byte max = values[0];
                double sum = 0;

                int i = 0;

                byte val;

                while (i < Len - 16)
                {
                    for (int j = i; j < i + 16; j++)
                    {
                        val = values[j];

                        if (min > val)
                        {
                            min = val;
                        }

                        if (max < val)
                        {
                            max = val;
                        }
                    }

                    sum += ((int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }

                    sum += values[i];

                    i++;
                }

                return (min, max, sum / Len);
            }
            else
            {
                byte val = values[0];

                return (val, val, val);
            }
        }

        /// <summary>
        /// 计算一组 16 位整数的最小值、最大值与平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(short, short, double) 元组，表示一组 16 位整数的最小值、最大值与平均值。</returns>
        public static (short Min, short Max, double Average) MinMaxAverage(params short[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                short min = values[0];
                short max = values[0];
                double sum = 0;

                int i = 0;

                short val;

                while (i < Len - 16)
                {
                    for (int j = i; j < i + 16; j++)
                    {
                        val = values[j];

                        if (min > val)
                        {
                            min = val;
                        }

                        if (max < val)
                        {
                            max = val;
                        }
                    }

                    sum += ((int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }

                    sum += values[i];

                    i++;
                }

                return (min, max, sum / Len);
            }
            else
            {
                short val = values[0];

                return (val, val, val);
            }
        }

        /// <summary>
        /// 计算一组 16 位无符号整数的最小值、最大值与平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(ushort, ushort, double) 元组，表示一组 16 位无符号整数的最小值、最大值与平均值。</returns>
        public static (ushort Min, ushort Max, double Average) MinMaxAverage(params ushort[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                ushort min = values[0];
                ushort max = values[0];
                double sum = 0;

                int i = 0;

                ushort val;

                while (i < Len - 16)
                {
                    for (int j = i; j < i + 16; j++)
                    {
                        val = values[j];

                        if (min > val)
                        {
                            min = val;
                        }

                        if (max < val)
                        {
                            max = val;
                        }
                    }

                    sum += ((int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }

                    sum += values[i];

                    i++;
                }

                return (min, max, sum / Len);
            }
            else
            {
                ushort val = values[0];

                return (val, val, val);
            }
        }

        /// <summary>
        /// 计算一组 32 位整数的最小值、最大值与平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(int, int, double) 元组，表示一组 32 位整数的最小值、最大值与平均值。</returns>
        public static (int Min, int Max, double Average) MinMaxAverage(params int[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                int min = values[0];
                int max = values[0];
                double sum = 0;

                int i = 0;

                int val;

                while (i < Len - 16)
                {
                    for (int j = i; j < i + 16; j++)
                    {
                        val = values[j];

                        if (min > val)
                        {
                            min = val;
                        }

                        if (max < val)
                        {
                            max = val;
                        }
                    }

                    sum += ((long)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }

                    sum += values[i];

                    i++;
                }

                return (min, max, sum / Len);
            }
            else
            {
                int val = values[0];

                return (val, val, val);
            }
        }

        /// <summary>
        /// 计算一组 32 位无符号整数的最小值、最大值与平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(uint, uint, double) 元组，表示一组 32 位无符号整数的最小值、最大值与平均值。</returns>
        public static (uint Min, uint Max, double Average) MinMaxAverage(params uint[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                uint min = values[0];
                uint max = values[0];
                double sum = 0;

                int i = 0;

                uint val;

                while (i < Len - 16)
                {
                    for (int j = i; j < i + 16; j++)
                    {
                        val = values[j];

                        if (min > val)
                        {
                            min = val;
                        }

                        if (max < val)
                        {
                            max = val;
                        }
                    }

                    sum += ((long)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }

                    sum += values[i];

                    i++;
                }

                return (min, max, sum / Len);
            }
            else
            {
                uint val = values[0];

                return (val, val, val);
            }
        }

        /// <summary>
        /// 计算一组 64 位整数的最小值、最大值与平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(long, long, double) 元组，表示一组 64 位整数的最小值、最大值与平均值。</returns>
        public static (long Min, long Max, double Average) MinMaxAverage(params long[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                long min = values[0];
                long max = values[0];
                double sum = 0;

                int i = 0;

                long val;

                while (i < Len - 16)
                {
                    for (int j = i; j < i + 16; j++)
                    {
                        val = values[j];

                        if (min > val)
                        {
                            min = val;
                        }

                        if (max < val)
                        {
                            max = val;
                        }
                    }

                    sum += ((double)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }

                    sum += values[i];

                    i++;
                }

                return (min, max, sum / Len);
            }
            else
            {
                long val = values[0];

                return (val, val, val);
            }
        }

        /// <summary>
        /// 计算一组 64 位无符号整数的最小值、最大值与平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(ulong, ulong, double) 元组，表示一组 64 位无符号整数的最小值、最大值与平均值。</returns>
        public static (ulong Min, ulong Max, double Average) MinMaxAverage(params ulong[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                ulong min = values[0];
                ulong max = values[0];
                double sum = 0;

                int i = 0;

                ulong val;

                while (i < Len - 16)
                {
                    for (int j = i; j < i + 16; j++)
                    {
                        val = values[j];

                        if (min > val)
                        {
                            min = val;
                        }

                        if (max < val)
                        {
                            max = val;
                        }
                    }

                    sum += ((double)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }

                    sum += values[i];

                    i++;
                }

                return (min, max, sum / Len);
            }
            else
            {
                ulong val = values[0];

                return (val, val, val);
            }
        }

        /// <summary>
        /// 计算一组十进制浮点数的最小值、最大值与平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(decimal, decimal, double) 元组，表示一组十进制浮点数的最小值、最大值与平均值。</returns>
        public static (decimal Min, decimal Max, decimal Average) MinMaxAverage(params decimal[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                decimal min = values[0];
                decimal max = values[0];
                decimal sum = 0;

                int i = 0;

                decimal val;

                while (i < Len - 16)
                {
                    for (int j = i; j < i + 16; j++)
                    {
                        val = values[j];

                        if (min > val)
                        {
                            min = val;
                        }

                        if (max < val)
                        {
                            max = val;
                        }
                    }

                    sum += (values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }

                    sum += values[i];

                    i++;
                }

                return (min, max, sum / Len);
            }
            else
            {
                decimal val = values[0];

                return (val, val, val);
            }
        }

        /// <summary>
        /// 计算一组单精度浮点数的最小值、最大值与平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(float, float, double) 元组，表示一组单精度浮点数的最小值、最大值与平均值。</returns>
        public static (float Min, float Max, float Average) MinMaxAverage(params float[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                float min = values[0];
                float max = values[0];
                double sum = 0;

                int i = 0;

                float val;

                while (i < Len - 16)
                {
                    for (int j = i; j < i + 16; j++)
                    {
                        val = values[j];

                        if (min > val)
                        {
                            min = val;
                        }

                        if (max < val)
                        {
                            max = val;
                        }
                    }

                    sum += ((double)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }

                    sum += values[i];

                    i++;
                }

                return (min, max, (float)(sum / Len));
            }
            else
            {
                float val = values[0];

                return (val, val, val);
            }
        }

        /// <summary>
        /// 计算一组双精度浮点数的最小值、最大值与平均值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>(double, double, double) 元组，表示一组双精度浮点数的最小值、最大值与平均值。</returns>
        public static (double Min, double Max, double Average) MinMaxAverage(params double[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            if (Len > 1)
            {
                double min = values[0];
                double max = values[0];
                double sum = 0;

                int i = 0;

                double val;

                while (i < Len - 16)
                {
                    for (int j = i; j < i + 16; j++)
                    {
                        val = values[j];

                        if (min > val)
                        {
                            min = val;
                        }

                        if (max < val)
                        {
                            max = val;
                        }
                    }

                    sum += (values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15]);

                    i += 16;
                }

                while (i < Len)
                {
                    val = values[i];

                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }

                    sum += values[i];

                    i++;
                }

                return (min, max, sum / Len);
            }
            else
            {
                double val = values[0];

                return (val, val, val);
            }
        }

        #endregion

        #region 方差与标准差

        /// <summary>
        /// 计算一组双精度浮点数的方差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组双精度浮点数的方差。</returns>
        public static double Deviation(params double[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            double Avg = Average(values);
            double SqrSum = 0;

            for (int i = 0; i < Len; i++)
            {
                double Delta = values[i] - Avg;

                SqrSum += Delta * Delta;
            }

            return (SqrSum / Len);
        }

        /// <summary>
        /// 计算一组双精度浮点数的样本方差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组双精度浮点数的样本方差。</returns>
        public static double SampleDeviation(params double[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            double Avg = Average(values);
            double SqrSum = 0;

            for (int i = 0; i < Len; i++)
            {
                double Delta = values[i] - Avg;

                SqrSum += Delta * Delta;
            }

            return (SqrSum / (Len - 1));
        }

        /// <summary>
        /// 计算一组双精度浮点数的标准差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组双精度浮点数的标准差。</returns>
        public static double StandardDeviation(params double[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            double Sum = 0;
            double AbsMax = 0;

            for (int i = 0; i < Len; i++)
            {
                Sum += values[i];
                AbsMax = Math.Max(AbsMax, Math.Abs(values[i]));
            }

            if (AbsMax == 0)
            {
                return 0;
            }
            else
            {
                double Avg = Sum / Len;
                double SqrSum = 0;

                for (int i = 0; i < Len; i++)
                {
                    double Delta = (values[i] - Avg) / AbsMax;

                    SqrSum += Delta * Delta;
                }

                return (AbsMax * Math.Sqrt(SqrSum / Len));
            }
        }

        /// <summary>
        /// 计算一组双精度浮点数的样本标准差。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>双精度浮点数，表示一组双精度浮点数的样本标准差。</returns>
        public static double SampleStandardDeviation(params double[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException();
            }

            //

            int Len = values.Length;

            double Sum = 0;
            double AbsMax = 0;

            for (int i = 0; i < Len; i++)
            {
                Sum += values[i];
                AbsMax = Math.Max(AbsMax, Math.Abs(values[i]));
            }

            if (AbsMax == 0)
            {
                return 0;
            }
            else
            {
                double Avg = Sum / Len;
                double SqrSum = 0;

                for (int i = 0; i < Len; i++)
                {
                    double Delta = (values[i] - Avg) / AbsMax;

                    SqrSum += Delta * Delta;
                }

                return (AbsMax * Math.Sqrt(SqrSum / (Len - 1)));
            }
        }

        #endregion
    }
}