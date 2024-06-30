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
        #region 排列组合

        // 计算指定双精度浮点数的伽马函数。
        private static Real Gamma(double n)
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
                        result *= (long)(i * (i + 1) * (i + 2) * (i + 3)) * ((i + 4) * (i + 5) * (i + 6) * (i + 7));

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
                        result *= (double)i * (i + 1) * (i + 2) * (i + 3) * (i + 4) * (i + 5) * (i + 6) * (i + 7) * (i + 8) * (i + 9) * (i + 10) * (i + 11) * (i + 12) * (i + 13) * (i + 14) * (i + 15);

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
                        result *= (i + mod) * (i + 1 + mod) * (i + 2 + mod) * (i + 3 + mod) * (i + 4 + mod) * (i + 5 + mod) * (i + 6 + mod) * (i + 7 + mod) * (i + 8 + mod) * (i + 9 + mod) * (i + 10 + mod) * (i + 11 + mod) * (i + 12 + mod) * (i + 13 + mod) * (i + 14 + mod) * (i + 15 + mod);

                        i += 16;
                    }

                    while (i < trunc)
                    {
                        result *= i + mod;

                        i++;
                    }

                    return result;
                };

                Func<double, double> Gamma0To1 = (val) =>
                {
                    double[] coeff = new double[]
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

                    double sum = coeff[0];

                    for (int i = 1; i < coeff.Length; i++)
                    {
                        sum += coeff[i] / (val + i - 1);
                    }

                    double baseVal = val + coeff.Length - 2.5;

                    return Constant.SqrtDoublePi * Math.Pow(baseVal, val - 0.5) * Math.Exp(-baseVal) * sum;
                };

                Func<double, Real> GammaPositive = (val) =>
                {
                    long trunc = (long)Math.Truncate(val);
                    double mod = val - trunc;

                    return GammaTruncI64Mod(trunc, mod) * Gamma0To1(mod);
                };

                Func<double, Real> GammaNegative = (val) => Constant.Pi / Math.Sin(Constant.Pi * val) / GammaPositive(1 - val);

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

        // 计算指定双精度浮点数的阶乘。
        private static Real Factorial(double n)
        {
            if (InternalMethod.IsNaNOrInfinity(n))
            {
                return Real.NaN;
            }
            else
            {
                return Gamma(n + 1);
            }
        }

        // 计算从有限个元素中任取若干个元素的排列数。
        private static Real ArrangementReal(double total, double selection)
        {
            if (InternalMethod.IsNaNOrInfinity(total) || InternalMethod.IsNaNOrInfinity(selection))
            {
                return Real.NaN;
            }
            else
            {
                double truncTotal = Math.Truncate(total);
                double truncSelection = Math.Truncate(selection);

                if ((total == truncTotal && truncTotal >= 0 && truncTotal < 1E15) && (selection == truncSelection && truncSelection > -1E15 && truncSelection < 1E15))
                {
                    long totalI64 = (long)truncTotal;
                    long selectionI64 = (long)truncSelection;
                    long delta = totalI64 - selectionI64;

                    if (delta < 0)
                    {
                        return Real.Zero;
                    }
                    else
                    {
                        Real result = Real.One;

                        if (totalI64 > delta)
                        {
                            long i = totalI64;

                            while (i > delta + 16)
                            {
                                result *= (double)i * (i - 1) * (i - 2) * (i - 3) * (i - 4) * (i - 5) * (i - 6) * (i - 7) * (i - 8) * (i - 9) * (i - 10) * (i - 11) * (i - 12) * (i - 13) * (i - 14) * (i - 15);

                                i -= 16;
                            }

                            while (i > delta)
                            {
                                result *= i;

                                i--;
                            }
                        }
                        else if (totalI64 < delta)
                        {
                            long i = delta;

                            while (i > totalI64 + 16)
                            {
                                result /= (double)i * (i - 1) * (i - 2) * (i - 3) * (i - 4) * (i - 5) * (i - 6) * (i - 7) * (i - 8) * (i - 9) * (i - 10) * (i - 11) * (i - 12) * (i - 13) * (i - 14) * (i - 15);

                                i -= 16;
                            }

                            while (i > totalI64)
                            {
                                result /= i;

                                i--;
                            }
                        }

                        return result;
                    }
                }
                else if (total >= 0 || total != truncTotal)
                {
                    double delta = total - selection;
                    double truncDelta = Math.Truncate(delta);

                    if (delta < 0 && delta == truncDelta)
                    {
                        return Real.Zero;
                    }
                    else
                    {
                        return Factorial(total) / Factorial(delta);
                    }
                }
                else
                {
                    return Real.NaN;
                }
            }
        }

        // 计算从有限个元素中任取若干个元素的组合数。
        private static Real CombinationReal(double total, double selection)
        {
            if (InternalMethod.IsNaNOrInfinity(total) || InternalMethod.IsNaNOrInfinity(selection))
            {
                return Real.NaN;
            }
            else
            {
                double truncTotal = Math.Truncate(total);
                double truncSelection = Math.Truncate(selection);

                if ((total == truncTotal && truncTotal >= 0 && truncTotal < 1E15) && (selection == truncSelection && truncSelection > -1E15 && truncSelection < 1E15))
                {
                    long totalI64 = (long)truncTotal;
                    long selectionI64 = (long)truncSelection;

                    if (selectionI64 < 0)
                    {
                        return Real.Zero;
                    }
                    else
                    {
                        long delta = totalI64 - selectionI64;

                        if (delta < 0)
                        {
                            return Real.Zero;
                        }
                        else
                        {
                            Real result = Real.One;

                            if (totalI64 > delta)
                            {
                                long i = totalI64;

                                while (i > delta + 16)
                                {
                                    result *= (double)i * (i - 1) * (i - 2) * (i - 3) * (i - 4) * (i - 5) * (i - 6) * (i - 7) * (i - 8) * (i - 9) * (i - 10) * (i - 11) * (i - 12) * (i - 13) * (i - 14) * (i - 15);

                                    i -= 16;
                                }

                                while (i > delta)
                                {
                                    result *= i;

                                    i--;
                                }
                            }
                            else if (totalI64 < delta)
                            {
                                long i = delta;

                                while (i > totalI64 + 16)
                                {
                                    result /= (double)i * (i - 1) * (i - 2) * (i - 3) * (i - 4) * (i - 5) * (i - 6) * (i - 7) * (i - 8) * (i - 9) * (i - 10) * (i - 11) * (i - 12) * (i - 13) * (i - 14) * (i - 15);

                                    i -= 16;
                                }

                                while (i > totalI64)
                                {
                                    result /= i;

                                    i--;
                                }
                            }

                            if (!result.IsInfinity && result != Real.Zero)
                            {
                                long i = selectionI64;

                                while (i > 1 + 16)
                                {
                                    result /= (double)i * (i - 1) * (i - 2) * (i - 3) * (i - 4) * (i - 5) * (i - 6) * (i - 7) * (i - 8) * (i - 9) * (i - 10) * (i - 11) * (i - 12) * (i - 13) * (i - 14) * (i - 15);

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
                else if (total >= 0 || total != truncTotal)
                {
                    if (selection < 0 && selection == truncSelection)
                    {
                        return Real.Zero;
                    }
                    else
                    {
                        double delta = total - selection;
                        double truncDelta = Math.Truncate(delta);

                        if (delta < 0 && delta == truncDelta)
                        {
                            return Real.Zero;
                        }
                        else
                        {
                            return Factorial(total) / Factorial(selection) / Factorial(delta);
                        }
                    }
                }
                else
                {
                    return Real.NaN;
                }
            }
        }

        //

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
                return (double)ArrangementReal(total, selection);
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
                return (double)CombinationReal(total, selection);
            }
        }

        #endregion
    }
}