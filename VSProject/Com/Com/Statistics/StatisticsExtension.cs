/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.Statistics
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
    /// 为统计学的基本计算供静态方法。
    /// </summary>
    public static partial class Statistics
    {
        #region 极值，极差，求和，平均

        /// <summary>
        /// 计算一组 8 位整数的最小值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>8 位整数，表示一组 8 位整数的最小值。</returns>
        public static sbyte Min(params sbyte[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                sbyte result = values[0];

                sbyte val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                byte result = values[0];

                byte val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                short result = values[0];

                short val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                ushort result = values[0];

                ushort val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                int result = values[0];

                int val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                uint result = values[0];

                uint val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                long result = values[0];

                long val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                ulong result = values[0];

                ulong val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                decimal result = values[0];

                decimal val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                float result = values[0];

                float val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                double result = values[0];

                double val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                T result = values[0];

                T val;

                if (default(T) == null)
                {
                    for (int i = 1; i < len; i++)
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
                    for (int i = 1; i < len; i++)
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
        [Obsolete("请改为使用 Min<T>(params T[]) 方法")]
        public static IComparable Min(params IComparable[] values) => Min<IComparable>(values);

        //

        /// <summary>
        /// 计算一组 8 位整数的最大值。
        /// </summary>
        /// <param name="values">用于计算的值。</param>
        /// <returns>8 位整数，表示一组 8 位整数的最大值。</returns>
        public static sbyte Max(params sbyte[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                sbyte result = values[0];

                sbyte val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                byte result = values[0];

                byte val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                short result = values[0];

                short val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                ushort result = values[0];

                ushort val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                int result = values[0];

                int val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                uint result = values[0];

                uint val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                long result = values[0];

                long val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                ulong result = values[0];

                ulong val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                decimal result = values[0];

                decimal val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                float result = values[0];

                float val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                double result = values[0];

                double val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                T result = values[0];

                T val;

                if (default(T) == null)
                {
                    for (int i = 1; i < len; i++)
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
                    for (int i = 1; i < len; i++)
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
        [Obsolete("请改为使用 Max<T>(params T[]) 方法")]
        public static IComparable Max(params IComparable[] values) => Max<IComparable>(values);

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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                sbyte min = values[0];
                sbyte max = values[0];

                sbyte val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                byte min = values[0];
                byte max = values[0];

                byte val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                short min = values[0];
                short max = values[0];

                short val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                ushort min = values[0];
                ushort max = values[0];

                ushort val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                int min = values[0];
                int max = values[0];

                int val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                uint min = values[0];
                uint max = values[0];

                uint val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                long min = values[0];
                long max = values[0];

                long val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                ulong min = values[0];
                ulong max = values[0];

                ulong val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                decimal min = values[0];
                decimal max = values[0];

                decimal val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                float min = values[0];
                float max = values[0];

                float val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                double min = values[0];
                double max = values[0];

                double val;

                for (int i = 1; i < len; i++)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                T min = values[0];
                T max = values[0];

                T val;

                if (default(T) == null)
                {
                    for (int i = 1; i < len; i++)
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
                    for (int i = 1; i < len; i++)
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
        [Obsolete("请改为使用 MinMax<T>(params T[]) 方法")]
        public static (IComparable Min, IComparable Max) MinMax(params IComparable[] values) => MinMax<IComparable>(values);

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
                throw new ArgumentNullException(nameof(values));
            }

            //

            (int Min, int Max) = MinMax(values);

            return Max - Min;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            (int Min, int Max) = MinMax(values);

            return Max - Min;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            (int Min, int Max) = MinMax(values);

            return Max - Min;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            (int Min, int Max) = MinMax(values);

            return Max - Min;
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
                throw new ArgumentNullException(nameof(values));
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            (long Min, long Max) = MinMax(values);

            return Max - Min;
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
                throw new ArgumentNullException(nameof(values));
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            (double Min, double Max) = MinMax(values);

            return Max - Min;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            (decimal Min, decimal Max) = MinMax(values);

            return Max - Min;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            (float Min, float Max) = MinMax(values);

            return Max - Min;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            (double Min, double Max) = MinMax(values);

            return Max - Min;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                int result = 0;

                checked
                {
                    int i = 0;

                    while (i < len - 16)
                    {
                        result += (int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                        i += 16;
                    }

                    while (i < len)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                int result = 0;

                checked
                {
                    int i = 0;

                    while (i < len - 16)
                    {
                        result += (int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                        i += 16;
                    }

                    while (i < len)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                int result = 0;

                checked
                {
                    int i = 0;

                    while (i < len - 16)
                    {
                        result += (int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                        i += 16;
                    }

                    while (i < len)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                int result = 0;

                checked
                {
                    int i = 0;

                    while (i < len - 16)
                    {
                        result += (int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                        i += 16;
                    }

                    while (i < len)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                int result = 0;

                checked
                {
                    int i = 0;

                    while (i < len - 16)
                    {
                        result += values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                        i += 16;
                    }

                    while (i < len)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                long result = 0;

                checked
                {
                    int i = 0;

                    while (i < len - 16)
                    {
                        result += (long)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                        i += 16;
                    }

                    while (i < len)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                long result = 0;

                checked
                {
                    int i = 0;

                    while (i < len - 16)
                    {
                        result += values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                        i += 16;
                    }

                    while (i < len)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                double result = 0;

                int i = 0;

                while (i < len - 16)
                {
                    result += (double)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                decimal result = 0;

                int i = 0;

                while (i < len - 16)
                {
                    result += values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                float result = 0;

                int i = 0;

                while (i < len - 16)
                {
                    result += values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                double result = 0;

                int i = 0;

                while (i < len - 16)
                {
                    result += values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < len - 16)
                {
                    sum += (int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
                {
                    sum += values[i];

                    i++;
                }

                return sum / len;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < len - 16)
                {
                    sum += (int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
                {
                    sum += values[i];

                    i++;
                }

                return sum / len;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < len - 16)
                {
                    sum += (int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
                {
                    sum += values[i];

                    i++;
                }

                return sum / len;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < len - 16)
                {
                    sum += (int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
                {
                    sum += values[i];

                    i++;
                }

                return sum / len;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < len - 16)
                {
                    sum += (long)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
                {
                    sum += values[i];

                    i++;
                }

                return sum / len;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < len - 16)
                {
                    sum += (long)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
                {
                    sum += values[i];

                    i++;
                }

                return sum / len;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < len - 16)
                {
                    sum += (double)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
                {
                    sum += values[i];

                    i++;
                }

                return sum / len;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < len - 16)
                {
                    sum += (double)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
                {
                    sum += values[i];

                    i++;
                }

                return sum / len;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                decimal sum = 0;

                int i = 0;

                while (i < len - 16)
                {
                    sum += values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
                {
                    sum += values[i];

                    i++;
                }

                return sum / len;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < len - 16)
                {
                    sum += (double)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
                {
                    sum += values[i];

                    i++;
                }

                return (float)(sum / len);
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                double sum = 0;

                int i = 0;

                while (i < len - 16)
                {
                    sum += values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
                {
                    sum += values[i];

                    i++;
                }

                return sum / len;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                sbyte min = values[0];
                sbyte max = values[0];
                double sum = 0;

                int i = 0;

                sbyte val;

                while (i < len - 16)
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

                    sum += (int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
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

                return (min, max, sum / len);
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                byte min = values[0];
                byte max = values[0];
                double sum = 0;

                int i = 0;

                byte val;

                while (i < len - 16)
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

                    sum += (int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
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

                return (min, max, sum / len);
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                short min = values[0];
                short max = values[0];
                double sum = 0;

                int i = 0;

                short val;

                while (i < len - 16)
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

                    sum += (int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
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

                return (min, max, sum / len);
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                ushort min = values[0];
                ushort max = values[0];
                double sum = 0;

                int i = 0;

                ushort val;

                while (i < len - 16)
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

                    sum += (int)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
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

                return (min, max, sum / len);
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                int min = values[0];
                int max = values[0];
                double sum = 0;

                int i = 0;

                int val;

                while (i < len - 16)
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

                    sum += (long)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
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

                return (min, max, sum / len);
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                uint min = values[0];
                uint max = values[0];
                double sum = 0;

                int i = 0;

                uint val;

                while (i < len - 16)
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

                    sum += (long)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
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

                return (min, max, sum / len);
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                long min = values[0];
                long max = values[0];
                double sum = 0;

                int i = 0;

                long val;

                while (i < len - 16)
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

                    sum += (double)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
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

                return (min, max, sum / len);
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                ulong min = values[0];
                ulong max = values[0];
                double sum = 0;

                int i = 0;

                ulong val;

                while (i < len - 16)
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

                    sum += (double)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
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

                return (min, max, sum / len);
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                decimal min = values[0];
                decimal max = values[0];
                decimal sum = 0;

                int i = 0;

                decimal val;

                while (i < len - 16)
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

                    sum += values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
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

                return (min, max, sum / len);
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                float min = values[0];
                float max = values[0];
                double sum = 0;

                int i = 0;

                float val;

                while (i < len - 16)
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

                    sum += (double)values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
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

                return (min, max, (float)(sum / len));
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            if (len > 1)
            {
                double min = values[0];
                double max = values[0];
                double sum = 0;

                int i = 0;

                double val;

                while (i < len - 16)
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

                    sum += values[i] + values[i + 1] + values[i + 2] + values[i + 3] + values[i + 4] + values[i + 5] + values[i + 6] + values[i + 7] + values[i + 8] + values[i + 9] + values[i + 10] + values[i + 11] + values[i + 12] + values[i + 13] + values[i + 14] + values[i + 15];

                    i += 16;
                }

                while (i < len)
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

                return (min, max, sum / len);
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            double Avg = Average(values);
            double SqrSum = 0;

            for (int i = 0; i < len; i++)
            {
                double Delta = values[i] - Avg;

                SqrSum += Delta * Delta;
            }

            return SqrSum / len;
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            double Avg = Average(values);
            double SqrSum = 0;

            for (int i = 0; i < len; i++)
            {
                double Delta = values[i] - Avg;

                SqrSum += Delta * Delta;
            }

            return SqrSum / (len - 1);
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            double Sum = 0;
            double AbsMax = 0;

            for (int i = 0; i < len; i++)
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
                double Avg = Sum / len;
                double SqrSum = 0;

                for (int i = 0; i < len; i++)
                {
                    double Delta = (values[i] - Avg) / AbsMax;

                    SqrSum += Delta * Delta;
                }

                return AbsMax * Math.Sqrt(SqrSum / len);
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
                throw new ArgumentNullException(nameof(values));
            }

            //

            int len = values.Length;

            double Sum = 0;
            double AbsMax = 0;

            for (int i = 0; i < len; i++)
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
                double Avg = Sum / len;
                double SqrSum = 0;

                for (int i = 0; i < len; i++)
                {
                    double Delta = (values[i] - Avg) / AbsMax;

                    SqrSum += Delta * Delta;
                }

                return AbsMax * Math.Sqrt(SqrSum / (len - 1));
            }
        }

        #endregion
    }
}