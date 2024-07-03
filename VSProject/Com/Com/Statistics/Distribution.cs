/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.Statistics
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
    /// 为统计学的基本计算供静态方法。
    /// </summary>
    public static partial class Statistics
    {
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
                            return p * Math.Pow(1 - p, value - 1);
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
                    return (double)(CombinationReal(M, value) / CombinationReal(N, n) * CombinationReal(N - M, n - value));
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
                            return (double)(CombinationReal(n, value) * Real.Pow(p, value) * Real.Pow(1 - p, n - value));
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
                        return (double)(Real.Pow(lambda, value) * Real.Exp(-lambda) / Factorial(value));
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
                            return -Math.Exp(-lambda * val);
                        }
                    };

                    return Prim(right) - Prim(left);
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
                return Math.Exp(-0.5 * value * value) / Constant.SqrtDoublePi;
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

                        return (double)(Real.Pow(value, (Real)HalfK - 1) * Real.Exp((Real)(-value) / 2) / Real.Pow(2, HalfK) / Gamma(HalfK));
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
    }
}