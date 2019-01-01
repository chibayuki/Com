/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.Real
Version 18.12.25.0000

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
    /// 表示实数。
    /// </summary>
    public struct Real : IEquatable<Real>, IComparable, IComparable<Real>
    {
        #region 私有成员与内部成员

        private const int _DichotomyDepth = 9; // 二分法计算数量级的最大深度。

        private static readonly long[] _DichotomyMagnitudes = new long[] // 用于二分法计算数量级的数量级数列。
        {
            256, 128, 64, 32,
            16, 8, 4, 2,
            1
        };

        private static readonly double[] _LargePositiveDichotomyValues = new double[] // 用于二分法计算数量级的值数列，数量级与值均为正。
        {
            1E+256, 1E+128, 1E+64, 1E+32,
            1E+16, 1E+8, 1E+4, 1E+2,
            1E+1
        };

        private static readonly double[] _LargeNegativeDichotomyValues = new double[] // 用于二分法计算数量级的值数列，数量级为正，值为负。
        {
            -1E+256, -1E+128, -1E+64, -1E+32,
            -1E+16, -1E+8, -1E+4, -1E+2,
            -1E+1
        };

        private static readonly double[] _SmallPositiveDichotomyValues = new double[] // 用于二分法计算数量级的值数列，数量级为负，值为正。
        {
            1E-255, 1E-127, 1E-63, 1E-31,
            1E-15, 1E-7, 1E-3, 1E-1,
            1E-0
        };

        private static readonly double[] _SmallNegativeDichotomyValues = new double[] // 用于二分法计算数量级的值数列，数量级与值均为负。
        {
            -1E-255, -1E-127, -1E-63, -1E-31,
            -1E-15, -1E-7, -1E-3, -1E-1,
            -1E-0
        };

        //

        private static readonly double[] _LargePositiveGeometricValues = new double[] // 用于在一定范围内进行类型转换的值数列，数量级为正。
        {
            1E+0, 1E+1, 1E+2, 1E+3,
            1E+4, 1E+5, 1E+6, 1E+7,
            1E+8, 1E+9, 1E+10, 1E+11,
            1E+12, 1E+13, 1E+14, 1E+15,
            1E+16, 1E+17, 1E+18, 1E+19,
            1E+20, 1E+21, 1E+22, 1E+23,
            1E+24, 1E+25, 1E+26, 1E+27,
            1E+28
        };

        private static readonly double[] _SmallPositiveGeometricValues = new double[] // 用于在一定范围内进行类型转换的值数列，数量级为负。
        {
            1E-0, 1E-1, 1E-2, 1E-3,
            1E-4, 1E-5, 1E-6, 1E-7,
            1E-8, 1E-9, 1E-10, 1E-11,
            1E-12, 1E-13, 1E-14, 1E-15,
            1E-16, 1E-17, 1E-18, 1E-19,
            1E-20, 1E-21, 1E-22, 1E-23,
            1E-24, 1E-25, 1E-26, 1E-27,
            1E-28
        };

        //

        private static readonly Real _Pi = new Real(3.1415926535897932); // 表示圆周率 π 的 Real 结构的实例。
        private static readonly Real _2Pi = new Real(6.2831853071795865); // 表示圆周率 π 的 2 倍的 Real 结构的实例。
        private static readonly Real _HalfPi = new Real(1.5707963267948966); // 表示圆周率 π 的 1/2 的 Real 结构的实例。
        private static readonly Real _MinusHalfPi = new Real(-1.5707963267948966); // 表示圆周率 π 的 -1/2 的 Real 结构的实例。

        private static readonly Real _E = new Real(2.7182818284590451); // 表示自然常数 E 的 Real 结构的实例。
        private static readonly Real _LgE = new Real(0.43429448190325183); // 表示自然常数 E 的常用对数的 Real 结构的实例。

        //

        private const long _MinMagnitude = -999999999999999; // 数量级的最小值。
        private const long _MaxMagnitude = 999999999999999; // 数量级的最大值。

        //

        private double _Value; // 值。
        private long _Magnitude; // 数量级。

        //

        private void _Rectify() // 校正此 Real 结构的值与数量级。
        {
            if (!InternalMethod.IsNaNOrInfinity(_Value) && _Value != 0)
            {
                if (_Value <= -10 || _Value >= 10)
                {
                    for (int i = 0; i < _DichotomyDepth; i++)
                    {
                        if (_Value <= _LargeNegativeDichotomyValues[i] || _Value >= _LargePositiveDichotomyValues[i])
                        {
                            if (_Magnitude <= _MaxMagnitude - _DichotomyMagnitudes[i])
                            {
                                _Value /= _LargePositiveDichotomyValues[i];
                                _Magnitude += _DichotomyMagnitudes[i];
                            }
                            else
                            {
                                _Value = (_Value > 0 ? double.PositiveInfinity : double.NegativeInfinity);
                                _Magnitude = 0;

                                break;
                            }
                        }
                    }
                }
                else if (_Value > -1 && _Value < 1)
                {
                    for (int i = 0; i < _DichotomyDepth; i++)
                    {
                        if (_Value > _SmallNegativeDichotomyValues[i] && _Value < _SmallPositiveDichotomyValues[i])
                        {
                            if (_Magnitude >= _MinMagnitude + _DichotomyMagnitudes[i])
                            {
                                _Value *= _LargePositiveDichotomyValues[i];
                                _Magnitude -= _DichotomyMagnitudes[i];
                            }
                            else
                            {
                                _Value = 0;
                                _Magnitude = 0;

                                break;
                            }
                        }
                    }
                }

                if (_Magnitude != 0)
                {
                    if (_Magnitude < _MinMagnitude)
                    {
                        _Value = 0;
                        _Magnitude = 0;
                    }
                    else if (_Magnitude > _MaxMagnitude)
                    {
                        _Value = (_Value > 0 ? double.PositiveInfinity : double.NegativeInfinity);
                        _Magnitude = 0;
                    }
                }
            }
            else
            {
                _Magnitude = 0;
            }
        }

        //

        private enum Parity // 奇偶性。
        {
            NonParity = -1, // 非奇非偶。
            Even, // 偶。
            Odd // 奇。
        }

        private Parity _GetParity() // 获取此 Real 结构的奇偶性。
        {
            if (InternalMethod.IsNaNOrInfinity(_Value))
            {
                return Parity.NonParity;
            }
            else if (_Value == 0)
            {
                return Parity.Even;
            }
            else
            {
                Func<double, Parity> GetParityOfDecimal = (val) =>
                {
                    long Numerator = 0;
                    long Denominator = 1;

                    double Trunc = Math.Truncate(val);

                    if (val == Trunc)
                    {
                        Denominator = 10;
                    }
                    else
                    {
                        Denominator = 1;

                        do
                        {
                            Denominator *= 10;

                            val *= 10;

                            Trunc = Math.Truncate(val);
                        }
                        while (val != Trunc);
                    }

                    Numerator = (long)Trunc;

                    while (Numerator % 2 == 0 && Denominator % 2 == 0)
                    {
                        Numerator /= 2;
                        Denominator /= 2;
                    }

                    while (Numerator % 5 == 0 && Denominator % 5 == 0)
                    {
                        Numerator /= 5;
                        Denominator /= 5;
                    }

                    long RemN = Numerator % 2;
                    long RemD = Denominator % 2;

                    if (RemN == 0 && RemD != 0)
                    {
                        return Parity.Even;
                    }
                    else if (RemN != 0 && RemD != 0)
                    {
                        return Parity.Odd;
                    }
                    else
                    {
                        return Parity.NonParity;
                    }
                };

                if (_Magnitude < 0)
                {
                    return GetParityOfDecimal(_Value);
                }
                else if (_Magnitude > 15)
                {
                    return Parity.Even;
                }
                else
                {
                    double Val = _Value * _LargePositiveGeometricValues[_Magnitude];
                    double Trunc = Math.Truncate(Val);

                    if (Val == Trunc)
                    {
                        if (((long)Trunc) % 2 == 0)
                        {
                            return Parity.Even;
                        }
                        else
                        {
                            return Parity.Odd;
                        }
                    }
                    else
                    {
                        return GetParityOfDecimal(_Value);
                    }
                }
            }
        }

        #endregion

        #region 常量与只读字段

        /// <summary>
        /// 表示数字 0 的 Real 结构的实例。
        /// </summary>
        public static readonly Real Zero = new Real(0, 0);

        /// <summary>
        /// 表示数字 1 的 Real 结构的实例。
        /// </summary>
        public static readonly Real One = new Real(1, 0);

        /// <summary>
        /// 表示数字 -1 的 Real 结构的实例。
        /// </summary>
        public static readonly Real MinusOne = new Real(-1, 0);

        //

        /// <summary>
        /// 表示 Real 结构的最小可能值。
        /// </summary>
        public static readonly Real MinValue = new Real(-9.999999999999999, _MaxMagnitude);

        /// <summary>
        /// 表示 Real 结构的最大可能值。
        /// </summary>
        public static readonly Real MaxValue = new Real(9.999999999999999, _MaxMagnitude);

        /// <summary>
        /// 表示 Real 结构的大于零的最小可能值。
        /// </summary>
        public static readonly Real Epsilon = new Real(1, _MinMagnitude);

        //

        /// <summary>
        /// 表示正无穷大的 Real 结构的实例。
        /// </summary>
        public static readonly Real PositiveInfinity = new Real(double.PositiveInfinity, 0);

        /// <summary>
        /// 表示负无穷大的 Real 结构的实例。
        /// </summary>
        public static readonly Real NegativeInfinity = new Real(double.NegativeInfinity, 0);

        /// <summary>
        /// 表示非数字的 Real 结构的实例。
        /// </summary>
        public static readonly Real NaN = new Real(double.NaN, 0);

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用双精度浮点数表示的值与 64 位整数表示的数量级初始化 Real 结构的新实例。
        /// </summary>
        /// <param name="value">双精度浮点数表示的值。</param>
        /// <param name="magnitude">64 位整数表示的数量级。</param>
        public Real(double value, long magnitude)
        {
            _Value = value;
            _Magnitude = magnitude;

            _Rectify();
        }

        /// <summary>
        /// 使用双精度浮点数表示的值初始化 Real 结构的新实例。
        /// </summary>
        /// <param name="value">双精度浮点数表示的值。</param>
        public Real(double value)
        {
            _Value = value;
            _Magnitude = 0;

            _Rectify();
        }

        /// <summary>
        /// 使用单精度浮点数表示的值初始化 Real 结构的新实例。
        /// </summary>
        /// <param name="value">单精度浮点数表示的值。</param>
        public Real(float value)
        {
            _Value = value;
            _Magnitude = 0;

            _Rectify();
        }

        /// <summary>
        /// 使用十进制数表示的值初始化 Real 结构的新实例。
        /// </summary>
        /// <param name="value">十进制数表示的值。</param>
        public Real(decimal value)
        {
            _Value = (double)value;
            _Magnitude = 0;

            _Rectify();
        }

        /// <summary>
        /// 使用 64 位无符号整数表示的值初始化 Real 结构的新实例。
        /// </summary>
        /// <param name="value">64 位无符号整数表示的值。</param>
        public Real(ulong value)
        {
            _Value = value;
            _Magnitude = 0;

            _Rectify();
        }

        /// <summary>
        /// 使用 64 位整数表示的值初始化 Real 结构的新实例。
        /// </summary>
        /// <param name="value">64 位整数表示的值。</param>
        public Real(long value)
        {
            _Value = value;
            _Magnitude = 0;

            _Rectify();
        }

        /// <summary>
        /// 使用 32 位无符号整数表示的值初始化 Real 结构的新实例。
        /// </summary>
        /// <param name="value">32 位无符号整数表示的值。</param>
        public Real(uint value)
        {
            _Value = value;
            _Magnitude = 0;

            _Rectify();
        }

        /// <summary>
        /// 使用 32 位整数表示的值初始化 Real 结构的新实例。
        /// </summary>
        /// <param name="value">32 位整数表示的值。</param>
        public Real(int value)
        {
            _Value = value;
            _Magnitude = 0;

            _Rectify();
        }

        /// <summary>
        /// 使用 16 位无符号整数表示的值初始化 Real 结构的新实例。
        /// </summary>
        /// <param name="value">16 位无符号整数表示的值。</param>
        public Real(ushort value)
        {
            _Value = value;
            _Magnitude = 0;

            _Rectify();
        }

        /// <summary>
        /// 使用 16 位整数表示的值初始化 Real 结构的新实例。
        /// </summary>
        /// <param name="value">16 位整数表示的值。</param>
        public Real(short value)
        {
            _Value = value;
            _Magnitude = 0;

            _Rectify();
        }

        /// <summary>
        /// 使用 8 位无符号整数表示的值初始化 Real 结构的新实例。
        /// </summary>
        /// <param name="value">8 位无符号整数表示的值。</param>
        public Real(byte value)
        {
            _Value = value;
            _Magnitude = 0;

            _Rectify();
        }

        /// <summary>
        /// 使用 8 位整数表示的值初始化 Real 结构的新实例。
        /// </summary>
        /// <param name="value">8 位整数表示的值。</param>
        public Real(sbyte value)
        {
            _Value = value;
            _Magnitude = 0;

            _Rectify();
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取表示此 Real 结构是否为非数字的布尔值。
        /// </summary>
        public bool IsNaN
        {
            get
            {
                return double.IsNaN(_Value);
            }
        }

        /// <summary>
        /// 获取表示此 Real 结构是否为正无穷大的布尔值。
        /// </summary>
        public bool IsPositiveInfinity
        {
            get
            {
                return double.IsPositiveInfinity(_Value);
            }
        }

        /// <summary>
        /// 获取表示此 Real 结构是否为负无穷大的布尔值。
        /// </summary>
        public bool IsNegativeInfinity
        {
            get
            {
                return double.IsNegativeInfinity(_Value);
            }
        }

        /// <summary>
        /// 获取表示此 Real 结构是否为无穷大的布尔值。
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return double.IsInfinity(_Value);
            }
        }

        /// <summary>
        /// 获取表示此 Real 结构是否为非数字或无穷大的布尔值。
        /// </summary>
        public bool IsNaNOrInfinity
        {
            get
            {
                return InternalMethod.IsNaNOrInfinity(_Value);
            }
        }

        //

        /// <summary>
        /// 获取表示此 Real 结构是否为整数的布尔值。
        /// </summary>
        public bool IsInteger
        {
            get
            {
                if (double.IsNaN(_Value))
                {
                    return false;
                }
                else if (double.IsInfinity(_Value))
                {
                    return false;
                }
                else if (_Value == 0)
                {
                    return true;
                }
                else
                {
                    if (_Magnitude < 0)
                    {
                        return false;
                    }
                    else if (_Magnitude > 15)
                    {
                        return true;
                    }
                    else
                    {
                        double Val = _Value * _LargePositiveGeometricValues[_Magnitude];
                        double Trunc = Math.Truncate(Val);

                        return (Val == Trunc);
                    }
                }
            }
        }

        /// <summary>
        /// 获取表示此 Real 结构是否为奇数的布尔值。
        /// </summary>
        public bool IsOdd
        {
            get
            {
                return (_GetParity() == Parity.Odd);
            }
        }

        /// <summary>
        /// 获取表示此 Real 结构是否为偶数的布尔值。
        /// </summary>
        public bool IsEven
        {
            get
            {
                return (_GetParity() == Parity.Even);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 Real 结构的值。
        /// </summary>
        public double Value
        {
            get
            {
                return _Value;
            }

            set
            {
                _Value = value;

                _Rectify();
            }
        }

        /// <summary>
        /// 获取或设置此 Real 结构的数量级。
        /// </summary>
        public long Magnitude
        {
            get
            {
                return _Magnitude;
            }

            set
            {
                if (!InternalMethod.IsNaNOrInfinity(_Value) && _Value != 0)
                {
                    _Magnitude = value;

                    _Rectify();
                }
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 Real 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>布尔值，表示此 Real 结构是否与指定的对象相等。</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Real))
            {
                return false;
            }
            else if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals((Real)obj);
        }

        /// <summary>
        /// 返回此 Real 结构的哈希代码。
        /// </summary>
        /// <returns>32 位整数，表示此 Real 结构的哈希代码。</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 Real 结构转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 Real 结构的字符串形式。</returns>
        public override string ToString()
        {
            if (double.IsNaN(_Value))
            {
                return "NaN";
            }
            else if (double.IsInfinity(_Value))
            {
                if (double.IsPositiveInfinity(_Value))
                {
                    return "+Infinity";
                }
                else
                {
                    return "-Infinity";
                }
            }
            else if (_Value == 0)
            {
                return "0";
            }
            else
            {
                if (_Magnitude == 0)
                {
                    return _Value.ToString();
                }
                else if (_Magnitude > -5 && _Magnitude < 0)
                {
                    return (_Value * _SmallPositiveGeometricValues[-_Magnitude]).ToString();
                }
                else if (_Magnitude > 0 && _Magnitude < 15)
                {
                    return (_Value * _LargePositiveGeometricValues[_Magnitude]).ToString();
                }
                else
                {
                    return string.Concat(_Value, "×10^", _Magnitude);
                }
            }
        }

        //

        /// <summary>
        /// 判断此 Real 结构是否与指定的 Real 结构相等。
        /// </summary>
        /// <param name="real">用于比较的 Real 结构。</param>
        /// <returns>布尔值，表示此 Real 结构是否与指定的 Real 结构相等。</returns>
        public bool Equals(Real real)
        {
            return (_Magnitude.Equals(real._Magnitude) && _Value.Equals(real._Value));
        }

        //

        /// <summary>
        /// 将此 Real 结构与指定的对象进行次序比较。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is Real))
            {
                return 1;
            }
            else if (object.ReferenceEquals(this, obj))
            {
                return 0;
            }

            return CompareTo((Real)obj);
        }

        /// <summary>
        /// 将此 Real 结构与指定的 Real 结构进行次序比较。
        /// </summary>
        /// <param name="real">用于比较的 Real 结构。</param>
        public int CompareTo(Real real)
        {
            if (IsNaN || real.IsNaN)
            {
                if (IsNaN && real.IsNaN)
                {
                    return 0;
                }
                else
                {
                    if (IsNaN)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            else
            {
                if (this == real)
                {
                    return 0;
                }
                else if (this < real)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断两个 Real 结构是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 Real 结构。</param>
        /// <param name="right">用于比较的第二个 Real 结构。</param>
        /// <returns>布尔值，表示两个 Real 结构是否相等。</returns>
        public static bool Equals(Real left, Real right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            else if ((object)left == null || (object)right == null)
            {
                return false;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return true;
            }

            return left.Equals(right);
        }

        //

        /// <summary>
        /// 比较两个 Real 结构的次序。
        /// </summary>
        /// <param name="left">用于比较的第一个 Real 结构。</param>
        /// <param name="right">用于比较的第二个 Real 结构。</param>
        public static int Compare(Real left, Real right)
        {
            if ((object)left == null && (object)right == null)
            {
                return 0;
            }
            else if ((object)left == null)
            {
                return -1;
            }
            else if ((object)right == null)
            {
                return 1;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return 0;
            }

            return left.CompareTo(right);
        }

        //

        /// <summary>
        /// 返回对 Real 结构计算平方得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示底数。</param>
        /// <returns>Real 结构，表示对 Real 结构计算平方得到的结果。</returns>
        public static Real Sqr(Real real)
        {
            if (real.IsNaN)
            {
                return NaN;
            }
            else if (real.IsInfinity)
            {
                return PositiveInfinity;
            }
            else if (real._Value == 0)
            {
                return Zero;
            }
            else
            {
                long NewMag = real._Magnitude * 2;

                if (NewMag < _MinMagnitude)
                {
                    double NewVal = real._Value * real._Value;

                    if (NewMag + 1 == _MinMagnitude && (NewVal <= -10 || NewVal >= 10))
                    {
                        return new Real(NewVal / 10, _MinMagnitude);
                    }
                    else
                    {
                        return Zero;
                    }
                }
                else if (NewMag > _MaxMagnitude)
                {
                    return PositiveInfinity;
                }
                else
                {
                    return new Real(real._Value * real._Value, NewMag);
                }
            }
        }

        /// <summary>
        /// 返回对 Real 结构计算平方根得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示被开方数。</param>
        /// <returns>Real 结构，表示对 Real 结构计算平方根得到的结果。</returns>
        public static Real Sqrt(Real real)
        {
            if (real.IsNaN)
            {
                return NaN;
            }
            else if (real.IsInfinity)
            {
                if (real.IsPositiveInfinity)
                {
                    return PositiveInfinity;
                }
                else
                {
                    return NaN;
                }
            }
            else if (real._Value == 0)
            {
                return Zero;
            }
            else
            {
                if (real._Value < 0)
                {
                    return NaN;
                }
                else
                {
                    if (real._Magnitude % 2 == 0)
                    {
                        return new Real(Math.Sqrt(real._Value), real._Magnitude / 2);
                    }
                    else
                    {
                        return new Real(Math.Sqrt(real._Value * 10), real._Magnitude / 2);
                    }
                }
            }
        }

        /// <summary>
        /// 返回对 Real 结构计算自然幂得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示底数。</param>
        /// <returns>Real 结构，表示对 Real 结构计算自然幂得到的结果。</returns>
        public static Real Exp(Real real)
        {
            if (real.IsNaN)
            {
                return NaN;
            }
            else if (real.IsInfinity)
            {
                if (real.IsPositiveInfinity)
                {
                    return PositiveInfinity;
                }
                else
                {
                    return Zero;
                }
            }
            else if (real._Value == 0)
            {
                return One;
            }
            else
            {
                return (_E ^ real);
            }
        }

        /// <summary>
        /// 返回对 Real 结构计算复幂得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="left">Real 结构，表示底数。</param>
        /// <param name="right">Real 结构，表示指数。</param>
        /// <returns>Real 结构，表示对 Real 结构计算复幂得到的结果。</returns>
        public static Real Pow(Real left, Real right)
        {
            return (left ^ right);
        }

        /// <summary>
        /// 返回对 Real 结构计算常用对数得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示幂。</param>
        /// <returns>Real 结构，表示对 Real 结构计算常用对数得到的结果。</returns>
        public static Real Log10(Real real)
        {
            if (real.IsNaN)
            {
                return NaN;
            }
            else if (real.IsInfinity)
            {
                if (real.IsPositiveInfinity)
                {
                    return PositiveInfinity;
                }
                else
                {
                    return NaN;
                }
            }
            else if (real._Value == 0)
            {
                return NegativeInfinity;
            }
            else
            {
                if (real._Value > 0)
                {
                    return new Real(Math.Log10(real._Value) + real._Magnitude);
                }
                else
                {
                    return NaN;
                }
            }
        }

        /// <summary>
        /// 返回对 Real 结构计算自然对数得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示幂。</param>
        /// <returns>Real 结构，表示对 Real 结构计算自然对数得到的结果。</returns>
        public static Real Log(Real real)
        {
            return (Log10(real) / _LgE);
        }

        /// <summary>
        /// 返回对 Real 结构计算复对数得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="left">Real 结构，表示基数。</param>
        /// <param name="right">Real 结构，表示幂。</param>
        /// <returns>Real 结构，表示对 Real 结构计算复对数得到的结果。</returns>
        public static Real Log(Real left, Real right)
        {
            return (Log10(right) / Log10(left));
        }

        //

        /// <summary>
        /// 返回对 Real 结构计算正弦得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示以弧度计量的角度。</param>
        /// <returns>Real 结构，表示对 Real 结构计算正弦得到的结果。</returns>
        public static Real Sin(Real real)
        {
            if (real.IsNaN)
            {
                return NaN;
            }
            else if (real.IsInfinity)
            {
                return NaN;
            }
            else if (real._Value == 0)
            {
                return Zero;
            }
            else
            {
                return new Real(Math.Sin((real % _2Pi)._Value));
            }
        }

        /// <summary>
        /// 返回对 Real 结构计算余弦得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示以弧度计量的角度。</param>
        /// <returns>Real 结构，表示对 Real 结构计算余弦得到的结果。</returns>
        public static Real Cos(Real real)
        {
            if (real.IsNaN)
            {
                return NaN;
            }
            else if (real.IsInfinity)
            {
                return NaN;
            }
            else if (real._Value == 0)
            {
                return One;
            }
            else
            {
                return new Real(Math.Cos((real % _2Pi)._Value));
            }
        }

        /// <summary>
        /// 返回对 Real 结构计算正切得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示以弧度计量的角度。</param>
        /// <returns>Real 结构，表示对 Real 结构计算正切得到的结果。</returns>
        public static Real Tan(Real real)
        {
            if (real.IsNaN)
            {
                return NaN;
            }
            else if (real.IsInfinity)
            {
                return NaN;
            }
            else if (real._Value == 0)
            {
                return Zero;
            }
            else
            {
                return new Real(Math.Tan((real % _Pi)._Value));
            }
        }

        /// <summary>
        /// 返回对 Real 结构计算反正弦得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示正弦值的复数。</param>
        /// <returns>Real 结构，表示对 Real 结构计算反正弦得到的结果。</returns>
        public static Real Asin(Real real)
        {
            if (real.IsNaN)
            {
                return NaN;
            }
            else if (real.IsInfinity)
            {
                return NaN;
            }
            else if (real._Value == 0)
            {
                return Zero;
            }
            else
            {
                if (real >= MinusOne && real <= One)
                {
                    return new Real(Math.Asin(real._Value));
                }
                else
                {
                    return NaN;
                }
            }
        }

        /// <summary>
        /// 返回对 Real 结构计算反余弦得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示余弦值的复数。</param>
        /// <returns>Real 结构，表示对 Real 结构计算反余弦得到的结果。</returns>
        public static Real Acos(Real real)
        {
            if (real.IsNaN)
            {
                return NaN;
            }
            else if (real.IsInfinity)
            {
                return NaN;
            }
            else if (real._Value == 0)
            {
                return _HalfPi;
            }
            else
            {
                if (real >= MinusOne && real <= One)
                {
                    return new Real(Math.Acos(real._Value));
                }
                else
                {
                    return NaN;
                }
            }
        }

        /// <summary>
        /// 返回对 Real 结构计算反正切得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示正切值的复数。</param>
        /// <returns>Real 结构，表示对 Real 结构计算反正切得到的结果。</returns>
        public static Real Atan(Real real)
        {
            if (real.IsNaN)
            {
                return NaN;
            }
            else if (real.IsInfinity)
            {
                if (real.IsPositiveInfinity)
                {
                    return _HalfPi;
                }
                else
                {
                    return _MinusHalfPi;
                }
            }
            else if (real._Value == 0)
            {
                return Zero;
            }
            else
            {
                if (real > double.MaxValue)
                {
                    return _HalfPi;
                }
                else if (real < double.MinValue)
                {
                    return _MinusHalfPi;
                }
                else
                {
                    return new Real(Math.Atan((double)real));
                }
            }
        }

        /// <summary>
        /// 返回对 Real 结构计算双曲正弦得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示以弧度计量的角度。</param>
        /// <returns>Real 结构，表示对 Real 结构计算双曲正弦得到的结果。</returns>
        public static Real Sinh(Real real)
        {
            if (real.IsNaN)
            {
                return NaN;
            }
            else if (real.IsInfinity)
            {
                if (real.IsPositiveInfinity)
                {
                    return PositiveInfinity;
                }
                else
                {
                    return NegativeInfinity;
                }
            }
            else if (real._Value == 0)
            {
                return Zero;
            }
            else
            {
                if (real > double.MaxValue && real < double.MinValue)
                {
                    return new Real(Math.Sinh((double)real));
                }
                else
                {
                    return ((Exp(real) - Exp(-real)) / 2);
                }
            }
        }

        /// <summary>
        /// 返回对 Real 结构计算双曲余弦得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示以弧度计量的角度。</param>
        /// <returns>Real 结构，表示对 Real 结构计算双曲余弦得到的结果。</returns>
        public static Real Cosh(Real real)
        {
            if (real.IsNaN)
            {
                return NaN;
            }
            else if (real.IsInfinity)
            {
                return PositiveInfinity;
            }
            else if (real._Value == 0)
            {
                return One;
            }
            else
            {
                if (real > double.MaxValue && real < double.MinValue)
                {
                    return new Real(Math.Cosh((double)real));
                }
                else
                {
                    return ((Exp(real) + Exp(-real)) / 2);
                }
            }
        }

        /// <summary>
        /// 返回对 Real 结构计算双曲正切得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示以弧度计量的角度。</param>
        /// <returns>Real 结构，表示对 Real 结构计算双曲正切得到的结果。</returns>
        public static Real Tanh(Real real)
        {
            if (real.IsNaN)
            {
                return NaN;
            }
            else if (real.IsInfinity)
            {
                if (real.IsPositiveInfinity)
                {
                    return One;
                }
                else
                {
                    return MinusOne;
                }
            }
            else if (real._Value == 0)
            {
                return Zero;
            }
            else
            {
                if (real > double.MaxValue)
                {
                    return One;
                }
                else if (real < double.MinValue)
                {
                    return MinusOne;
                }
                else
                {
                    return new Real(Math.Tanh((double)real));
                }
            }
        }

        /// <summary>
        /// 返回对 Real 结构计算反双曲正弦得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示双曲正弦值的复数。</param>
        /// <returns>Real 结构，表示对 Real 结构计算反双曲正弦得到的结果。</returns>
        public static Real Asinh(Real real)
        {
            return Log(real + Sqrt(Sqr(real) + One));
        }

        /// <summary>
        /// 返回对 Real 结构计算反双曲余弦得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示双曲余弦值的复数。</param>
        /// <returns>Real 结构，表示对 Real 结构计算反双曲余弦得到的结果。</returns>
        public static Real Acosh(Real real)
        {
            return Log(real + Sqrt(Sqr(real) + MinusOne));
        }

        /// <summary>
        /// 返回对 Real 结构计算反双曲正切得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示双曲正切值的复数。</param>
        /// <returns>Real 结构，表示对 Real 结构计算反双曲正切得到的结果。</returns>
        public static Real Atanh(Real real)
        {
            return (Log((One + real) / (One - real)) / 2);
        }

        //

        /// <summary>
        /// 返回 Real 结构的符号数。
        /// </summary>
        /// <param name="real">Real 结构，用于转换的结构。</param>
        /// <returns>32 位整数，表示 Real 结构的符号数。</returns>
        public static int Sign(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                return 0;
            }
            else if (double.IsInfinity(real._Value))
            {
                if (double.IsPositiveInfinity(real._Value))
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            else if (real._Value == 0)
            {
                return 0;
            }
            else
            {
                return Math.Sign(real._Value);
            }
        }

        /// <summary>
        /// 返回将 Real 结构取绝对值得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，用于转换的结构。</param>
        /// <returns>Real 结构，表示将 Real 结构取绝对值得到的结果。</returns>
        public static Real Abs(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(real._Value))
            {
                if (double.IsPositiveInfinity(real._Value))
                {
                    return PositiveInfinity;
                }
                else
                {
                    return NegativeInfinity;
                }
            }
            else if (real._Value == 0)
            {
                return Zero;
            }
            else
            {
                return new Real(Math.Abs(real._Value), real._Magnitude);
            }
        }

        /// <summary>
        /// 返回将 Real 结构舍入到较大的整数值得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，用于转换的结构。</param>
        /// <returns>Real 结构，表示将 Real 结构舍入到较大的整数值得到的结果。</returns>
        public static Real Ceiling(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(real._Value))
            {
                if (double.IsPositiveInfinity(real._Value))
                {
                    return PositiveInfinity;
                }
                else
                {
                    return NegativeInfinity;
                }
            }
            else if (real._Value == 0)
            {
                return Zero;
            }
            else
            {
                if (real._Magnitude < 0)
                {
                    if (real._Value > 0)
                    {
                        return One;
                    }
                    else
                    {
                        return Zero;
                    }
                }
                else if (real._Magnitude > 15)
                {
                    return real;
                }
                else
                {
                    double Val = real._Value * _LargePositiveGeometricValues[real._Magnitude];

                    return new Real(Math.Ceiling(Val));
                }
            }
        }

        /// <summary>
        /// 返回将 Real 结构舍入到较小的整数值得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，用于转换的结构。</param>
        /// <returns>Real 结构，表示将 Real 结构舍入到较小的整数值得到的结果。</returns>
        public static Real Floor(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(real._Value))
            {
                if (double.IsPositiveInfinity(real._Value))
                {
                    return PositiveInfinity;
                }
                else
                {
                    return NegativeInfinity;
                }
            }
            else if (real._Value == 0)
            {
                return Zero;
            }
            else
            {
                if (real._Magnitude < 0)
                {
                    if (real._Value > 0)
                    {
                        return Zero;
                    }
                    else
                    {
                        return MinusOne;
                    }
                }
                else if (real._Magnitude > 15)
                {
                    return real;
                }
                else
                {
                    double Val = real._Value * _LargePositiveGeometricValues[real._Magnitude];

                    return new Real(Math.Floor(Val));
                }
            }
        }

        /// <summary>
        /// 返回将 Real 结构舍入到最接近的整数值得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，用于转换的结构。</param>
        /// <returns>Real 结构，表示将 Real 结构舍入到最接近的整数值得到的结果。</returns>
        public static Real Round(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(real._Value))
            {
                if (double.IsPositiveInfinity(real._Value))
                {
                    return PositiveInfinity;
                }
                else
                {
                    return NegativeInfinity;
                }
            }
            else if (real._Value == 0)
            {
                return Zero;
            }
            else
            {
                if (real._Magnitude < 0)
                {
                    if (real._Value > 0)
                    {
                        if (real._Value > 5)
                        {
                            return One;
                        }
                        else
                        {
                            return Zero;
                        }
                    }
                    else
                    {
                        if (real._Value < -5)
                        {
                            return MinusOne;
                        }
                        else
                        {
                            return Zero;
                        }
                    }
                }
                else if (real._Magnitude > 15)
                {
                    return real;
                }
                else
                {
                    double Val = real._Value * _LargePositiveGeometricValues[real._Magnitude];

                    return new Real(Math.Round(Val));
                }
            }
        }

        /// <summary>
        /// 返回将 Real 结构截断小数部分取整得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，用于转换的结构。</param>
        /// <returns>Real 结构，表示将 Real 结构截断小数部分取整得到的结果。</returns>
        public static Real Truncate(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(real._Value))
            {
                if (double.IsPositiveInfinity(real._Value))
                {
                    return PositiveInfinity;
                }
                else
                {
                    return NegativeInfinity;
                }
            }
            else if (real._Value == 0)
            {
                return Zero;
            }
            else
            {
                if (real._Magnitude < 0)
                {
                    return Zero;
                }
                else if (real._Magnitude > 15)
                {
                    return real;
                }
                else
                {
                    double Val = real._Value * _LargePositiveGeometricValues[real._Magnitude];

                    return new Real(Val);
                }
            }
        }

        /// <summary>
        /// 返回将两个 Real 结构分别取最大值得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="left">Real 结构，用于比较的第一个结构。</param>
        /// <param name="right">Real 结构，用于比较的第二个结构。</param>
        /// <returns>Real 结构，表示将两个 Real 结构分别取最大值得到的结果。</returns>
        public static Real Max(Real left, Real right)
        {
            if (left.IsNaN || right.IsNaN)
            {
                return NaN;
            }
            else
            {
                if (left > right)
                {
                    return left;
                }
                else
                {
                    return right;
                }
            }
        }

        /// <summary>
        /// 返回将两个 Real 结构分别取最小值得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="left">Real 结构，用于比较的第一个结构。</param>
        /// <param name="right">Real 结构，用于比较的第二个结构。</param>
        /// <returns>Real 结构，表示将两个 Real 结构分别取最小值得到的结果。</returns>
        public static Real Min(Real left, Real right)
        {
            if (left.IsNaN || right.IsNaN)
            {
                return NaN;
            }
            else
            {
                if (left < right)
                {
                    return left;
                }
                else
                {
                    return right;
                }
            }
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 Real 结构是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Real 结构。</param>
        /// <param name="right">运算符右侧比较的 Real 结构。</param>
        /// <returns>布尔值，表示两个 Real 结构是否相等。</returns>
        public static bool operator ==(Real left, Real right)
        {
            return (left._Magnitude == right._Magnitude && left._Value == right._Value);
        }

        /// <summary>
        /// 判断两个 Real 结构是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Real 结构。</param>
        /// <param name="right">运算符右侧比较的 Real 结构。</param>
        /// <returns>布尔值，表示两个 Real 结构是否不相等。</returns>
        public static bool operator !=(Real left, Real right)
        {
            return (left._Magnitude != right._Magnitude || left._Value != right._Value);
        }

        /// <summary>
        /// 判断两个 Real 结构是否前者小于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Real 结构。</param>
        /// <param name="right">运算符右侧比较的 Real 结构。</param>
        /// <returns>布尔值，表示两个 Real 结构是否前者小于后者。</returns>
        public static bool operator <(Real left, Real right)
        {
            if (left.IsNaN || right.IsNaN)
            {
                return false;
            }
            else if (left.IsInfinity || right.IsInfinity)
            {
                bool LIsInf = left.IsInfinity;
                bool LIsPosInf = left.IsPositiveInfinity;
                bool LIsNegInf = left.IsNegativeInfinity;
                bool RIsInf = right.IsInfinity;
                bool RIsPosInf = right.IsPositiveInfinity;
                bool RIsNegInf = right.IsNegativeInfinity;

                if (LIsInf && RIsInf)
                {
                    if (LIsPosInf && RIsPosInf)
                    {
                        return false;
                    }
                    else if (LIsPosInf && RIsNegInf)
                    {
                        return false;
                    }
                    else if (LIsNegInf && RIsPosInf)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (LIsPosInf)
                    {
                        return false;
                    }
                    else if (LIsNegInf)
                    {
                        return true;
                    }
                    else if (RIsPosInf)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (left._Value == 0 || right._Value == 0)
            {
                if (left._Value == 0 && right._Value == 0)
                {
                    return false;
                }
                else if (left._Value == 0)
                {
                    return (right._Value > 0);
                }
                else
                {
                    return (left._Value < 0);
                }
            }
            else
            {
                if (left._Magnitude == right._Magnitude)
                {
                    return (left._Value < right._Value);
                }
                else if (Math.Sign(left._Value) == Math.Sign(right._Value))
                {
                    if (left._Value > 0)
                    {
                        return (left._Magnitude < right._Magnitude);
                    }
                    else
                    {
                        return (left._Magnitude > right._Magnitude);
                    }
                }
                else
                {
                    return (left._Value < right._Value);
                }
            }
        }

        /// <summary>
        /// 判断两个 Real 结构是否前者大于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Real 结构。</param>
        /// <param name="right">运算符右侧比较的 Real 结构。</param>
        /// <returns>布尔值，表示两个 Real 结构是否前者大于后者。</returns>
        public static bool operator >(Real left, Real right)
        {
            if (left.IsNaN || right.IsNaN)
            {
                return false;
            }
            else if (left.IsInfinity || right.IsInfinity)
            {
                bool LIsInf = left.IsInfinity;
                bool LIsPosInf = left.IsPositiveInfinity;
                bool LIsNegInf = left.IsNegativeInfinity;
                bool RIsInf = right.IsInfinity;
                bool RIsPosInf = right.IsPositiveInfinity;
                bool RIsNegInf = right.IsNegativeInfinity;

                if (LIsInf && RIsInf)
                {
                    if (LIsPosInf && RIsPosInf)
                    {
                        return false;
                    }
                    else if (LIsPosInf && RIsNegInf)
                    {
                        return true;
                    }
                    else if (LIsNegInf && RIsPosInf)
                    {
                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (LIsPosInf)
                    {
                        return true;
                    }
                    else if (LIsNegInf)
                    {
                        return false;
                    }
                    else if (RIsPosInf)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else if (left._Value == 0 || right._Value == 0)
            {
                if (left._Value == 0 && right._Value == 0)
                {
                    return false;
                }
                else if (left._Value == 0)
                {
                    return (right._Value < 0);
                }
                else
                {
                    return (left._Value > 0);
                }
            }
            else
            {
                if (left._Magnitude == right._Magnitude)
                {
                    return (left._Value > right._Value);
                }
                else if (Math.Sign(left._Value) == Math.Sign(right._Value))
                {
                    if (left._Value > 0)
                    {
                        return (left._Magnitude > right._Magnitude);
                    }
                    else
                    {
                        return (left._Magnitude < right._Magnitude);
                    }
                }
                else
                {
                    return (left._Value > right._Value);
                }
            }
        }

        /// <summary>
        /// 判断两个 Real 结构是否前者小于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Real 结构。</param>
        /// <param name="right">运算符右侧比较的 Real 结构。</param>
        /// <returns>布尔值，表示两个 Real 结构是否前者小于或等于后者。</returns>
        public static bool operator <=(Real left, Real right)
        {
            if (left.IsNaN || right.IsNaN)
            {
                return false;
            }
            else if (left.IsInfinity || right.IsInfinity)
            {
                bool LIsInf = left.IsInfinity;
                bool LIsPosInf = left.IsPositiveInfinity;
                bool LIsNegInf = left.IsNegativeInfinity;
                bool RIsInf = right.IsInfinity;
                bool RIsPosInf = right.IsPositiveInfinity;
                bool RIsNegInf = right.IsNegativeInfinity;

                if (LIsInf && RIsInf)
                {
                    if (LIsPosInf && RIsPosInf)
                    {
                        return true;
                    }
                    else if (LIsPosInf && RIsNegInf)
                    {
                        return false;
                    }
                    else if (LIsNegInf && RIsPosInf)
                    {
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (LIsPosInf)
                    {
                        return false;
                    }
                    else if (LIsNegInf)
                    {
                        return true;
                    }
                    else if (RIsPosInf)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (left._Value == 0 || right._Value == 0)
            {
                if (left._Value == 0 && right._Value == 0)
                {
                    return true;
                }
                else if (left._Value == 0)
                {
                    return (right._Value >= 0);
                }
                else
                {
                    return (left._Value <= 0);
                }
            }
            else
            {
                if (left._Magnitude == right._Magnitude)
                {
                    return (left._Value <= right._Value);
                }
                else if (Math.Sign(left._Value) == Math.Sign(right._Value))
                {
                    if (left._Value > 0)
                    {
                        return (left._Magnitude < right._Magnitude);
                    }
                    else
                    {
                        return (left._Magnitude > right._Magnitude);
                    }
                }
                else
                {
                    return (left._Value < right._Value);
                }
            }
        }

        /// <summary>
        /// 判断两个 Real 结构是否前者大于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Real 结构。</param>
        /// <param name="right">运算符右侧比较的 Real 结构。</param>
        /// <returns>布尔值，表示两个 Real 结构是否前者大于或等于后者。</returns>
        public static bool operator >=(Real left, Real right)
        {
            if (left.IsNaN || right.IsNaN)
            {
                return false;
            }
            else if (left.IsInfinity || right.IsInfinity)
            {
                bool LIsInf = left.IsInfinity;
                bool LIsPosInf = left.IsPositiveInfinity;
                bool LIsNegInf = left.IsNegativeInfinity;
                bool RIsInf = right.IsInfinity;
                bool RIsPosInf = right.IsPositiveInfinity;
                bool RIsNegInf = right.IsNegativeInfinity;

                if (LIsInf && RIsInf)
                {
                    if (LIsPosInf && RIsPosInf)
                    {
                        return true;
                    }
                    else if (LIsPosInf && RIsNegInf)
                    {
                        return true;
                    }
                    else if (LIsNegInf && RIsPosInf)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (LIsPosInf)
                    {
                        return true;
                    }
                    else if (LIsNegInf)
                    {
                        return false;
                    }
                    else if (RIsPosInf)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else if (left._Value == 0 || right._Value == 0)
            {
                if (left._Value == 0 && right._Value == 0)
                {
                    return true;
                }
                else if (left._Value == 0)
                {
                    return (right._Value <= 0);
                }
                else
                {
                    return (left._Value >= 0);
                }
            }
            else
            {
                if (left._Magnitude == right._Magnitude)
                {
                    return (left._Value >= right._Value);
                }
                else if (Math.Sign(left._Value) == Math.Sign(right._Value))
                {
                    if (left._Value > 0)
                    {
                        return (left._Magnitude > right._Magnitude);
                    }
                    else
                    {
                        return (left._Magnitude < right._Magnitude);
                    }
                }
                else
                {
                    return (left._Value > right._Value);
                }
            }
        }

        //

        /// <summary>
        /// 返回在 Real 结构前添加正号得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，用于转换的结构。</param>
        /// <returns>Real 结构，表示在 Real 结构前添加正号得到的结果。</returns>
        public static Real operator +(Real real)
        {
            if (real.IsNaN)
            {
                return NaN;
            }
            else if (real.IsInfinity)
            {
                if (real.IsPositiveInfinity)
                {
                    return PositiveInfinity;
                }
                else
                {
                    return NegativeInfinity;
                }
            }
            else if (real._Value == 0)
            {
                return Zero;
            }
            else
            {
                return new Real(+real._Value, real._Magnitude);
            }
        }

        /// <summary>
        /// 返回在 Real 结构前添加负号得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，用于转换的结构。</param>
        /// <returns>Real 结构，表示在 Real 结构前添加负号得到的结果。</returns>
        public static Real operator -(Real real)
        {
            if (real.IsNaN)
            {
                return NaN;
            }
            else if (real.IsInfinity)
            {
                if (real.IsPositiveInfinity)
                {
                    return NegativeInfinity;
                }
                else
                {
                    return PositiveInfinity;
                }
            }
            else if (real._Value == 0)
            {
                return Zero;
            }
            else
            {
                return new Real(-real._Value, real._Magnitude);
            }
        }

        //

        /// <summary>
        /// 返回将 Real 结构加 1 得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，用于转换的结构。</param>
        /// <returns>Real 结构，表示将 Real 结构加 1 得到的结果。</returns>
        public static Real operator ++(Real real)
        {
            if (real.IsNaN)
            {
                return NaN;
            }
            else if (real.IsInfinity)
            {
                if (real.IsPositiveInfinity)
                {
                    return PositiveInfinity;
                }
                else
                {
                    return NegativeInfinity;
                }
            }
            else if (real._Value == 0)
            {
                return One;
            }
            else
            {
                return (real + One);
            }
        }

        /// <summary>
        /// 返回将 Real 结构减 1 得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，用于转换的结构。</param>
        /// <returns>Real 结构，表示将 Real 结构减 1 得到的结果。</returns>
        public static Real operator --(Real real)
        {
            if (real.IsNaN)
            {
                return NaN;
            }
            else if (real.IsInfinity)
            {
                if (real.IsPositiveInfinity)
                {
                    return NegativeInfinity;
                }
                else
                {
                    return PositiveInfinity;
                }
            }
            else if (real._Value == 0)
            {
                return MinusOne;
            }
            else
            {
                return (real + MinusOne);
            }
        }

        //

        /// <summary>
        /// 返回将 Real 结构与 Real 结构的相加得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="left">Real 结构，表示被加数。</param>
        /// <param name="right">Real 结构，表示加数。</param>
        /// <returns>Real 结构，表示将 Real 结构与 Real 结构的相加得到的结果。</returns>
        public static Real operator +(Real left, Real right)
        {
            if (left.IsNaN || right.IsNaN)
            {
                return NaN;
            }
            else if (left.IsInfinity || right.IsInfinity)
            {
                bool LIsInf = left.IsInfinity;
                bool LIsPosInf = left.IsPositiveInfinity;
                bool LIsNegInf = left.IsNegativeInfinity;
                bool RIsInf = right.IsInfinity;
                bool RIsPosInf = right.IsPositiveInfinity;
                bool RIsNegInf = right.IsNegativeInfinity;

                if (LIsInf && RIsInf)
                {
                    if (LIsPosInf && RIsPosInf)
                    {
                        return PositiveInfinity;
                    }
                    else if (LIsPosInf && RIsNegInf)
                    {
                        return NaN;
                    }
                    else if (LIsNegInf && RIsPosInf)
                    {
                        return NaN;
                    }
                    else
                    {
                        return NegativeInfinity;
                    }
                }
                else
                {
                    if (LIsPosInf)
                    {
                        return PositiveInfinity;
                    }
                    else if (LIsNegInf)
                    {
                        return NegativeInfinity;
                    }
                    else if (RIsPosInf)
                    {
                        return PositiveInfinity;
                    }
                    else
                    {
                        return NegativeInfinity;
                    }
                }
            }
            else if (left._Value == 0 || right._Value == 0)
            {
                if (left._Value == 0 && right._Value == 0)
                {
                    return Zero;
                }
                else if (left._Value == 0)
                {
                    return right;
                }
                else
                {
                    return left;
                }
            }
            else
            {
                if (left._Magnitude > right._Magnitude)
                {
                    if (left._Magnitude - right._Magnitude < 16)
                    {
                        return new Real(left._Value + right._Value / _LargePositiveGeometricValues[left._Magnitude - right._Magnitude], left._Magnitude);
                    }
                    else
                    {
                        return left;
                    }
                }
                else if (left._Magnitude < right._Magnitude)
                {
                    if (right._Magnitude - left._Magnitude < 16)
                    {
                        return new Real(left._Value / _LargePositiveGeometricValues[right._Magnitude - left._Magnitude] + right._Value, right._Magnitude);
                    }
                    else
                    {
                        return right;
                    }
                }
                else
                {
                    return new Real(left._Value + right._Value, left._Magnitude);
                }
            }
        }

        /// <summary>
        /// 返回将 Real 结构与 Real 结构的相减得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="left">Real 结构，表示被减数。</param>
        /// <param name="right">Real 结构，表示减数。</param>
        /// <returns>Real 结构，表示将 Real 结构与 Real 结构的相减得到的结果。</returns>
        public static Real operator -(Real left, Real right)
        {
            if (left.IsNaN || right.IsNaN)
            {
                return NaN;
            }
            else if (left.IsInfinity || right.IsInfinity)
            {
                bool LIsInf = left.IsInfinity;
                bool LIsPosInf = left.IsPositiveInfinity;
                bool LIsNegInf = left.IsNegativeInfinity;
                bool RIsInf = right.IsInfinity;
                bool RIsPosInf = right.IsPositiveInfinity;
                bool RIsNegInf = right.IsNegativeInfinity;

                if (LIsInf && RIsInf)
                {
                    if (LIsPosInf && RIsPosInf)
                    {
                        return NaN;
                    }
                    else if (LIsPosInf && RIsNegInf)
                    {
                        return PositiveInfinity;
                    }
                    else if (LIsNegInf && RIsPosInf)
                    {
                        return NegativeInfinity;
                    }
                    else
                    {
                        return NaN;
                    }
                }
                else
                {
                    if (LIsPosInf)
                    {
                        return PositiveInfinity;
                    }
                    else if (LIsNegInf)
                    {
                        return NegativeInfinity;
                    }
                    else if (RIsPosInf)
                    {
                        return NegativeInfinity;
                    }
                    else
                    {
                        return PositiveInfinity;
                    }
                }
            }
            else if (left._Value == 0 || right._Value == 0)
            {
                if (left._Value == 0 && right._Value == 0)
                {
                    return Zero;
                }
                else if (left._Value == 0)
                {
                    return -right;
                }
                else
                {
                    return left;
                }
            }
            else
            {
                if (left._Magnitude > right._Magnitude)
                {
                    if (left._Magnitude - right._Magnitude < 16)
                    {
                        return new Real(left._Value - right._Value / _LargePositiveGeometricValues[left._Magnitude - right._Magnitude], left._Magnitude);
                    }
                    else
                    {
                        return left;
                    }
                }
                else if (left._Magnitude < right._Magnitude)
                {
                    if (right._Magnitude - left._Magnitude < 16)
                    {
                        return new Real(left._Value / _LargePositiveGeometricValues[right._Magnitude - left._Magnitude] - right._Value, right._Magnitude);
                    }
                    else
                    {
                        return right;
                    }
                }
                else
                {
                    return new Real(left._Value - right._Value, left._Magnitude);
                }
            }
        }

        /// <summary>
        /// 返回将 Real 结构与 Real 结构的相乘得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="left">Real 结构，表示被乘数。</param>
        /// <param name="right">Real 结构，表示乘数。</param>
        /// <returns>Real 结构，表示将 Real 结构与 Real 结构的相乘得到的结果。</returns>
        public static Real operator *(Real left, Real right)
        {
            if (left.IsNaN || right.IsNaN)
            {
                return NaN;
            }
            else if (left.IsInfinity || right.IsInfinity)
            {
                bool LIsInf = left.IsInfinity;
                bool LIsPosInf = left.IsPositiveInfinity;
                bool LIsNegInf = left.IsNegativeInfinity;
                bool RIsInf = right.IsInfinity;
                bool RIsPosInf = right.IsPositiveInfinity;
                bool RIsNegInf = right.IsNegativeInfinity;

                if (LIsInf && RIsInf)
                {
                    if (LIsPosInf && RIsPosInf)
                    {
                        return PositiveInfinity;
                    }
                    else if (LIsPosInf && RIsNegInf)
                    {
                        return NegativeInfinity;
                    }
                    else if (LIsNegInf && RIsPosInf)
                    {
                        return NegativeInfinity;
                    }
                    else
                    {
                        return PositiveInfinity;
                    }
                }
                else
                {
                    if (LIsPosInf)
                    {
                        if (right._Magnitude == 0)
                        {
                            return NaN;
                        }
                        else if (right._Magnitude > 0)
                        {
                            return PositiveInfinity;
                        }
                        else
                        {
                            return NegativeInfinity;
                        }
                    }
                    else if (LIsNegInf)
                    {
                        if (right._Magnitude == 0)
                        {
                            return NaN;
                        }
                        else if (right._Magnitude > 0)
                        {
                            return NegativeInfinity;
                        }
                        else
                        {
                            return PositiveInfinity;
                        }
                    }
                    else if (RIsPosInf)
                    {
                        if (left._Magnitude == 0)
                        {
                            return NaN;
                        }
                        else if (left._Magnitude > 0)
                        {
                            return PositiveInfinity;
                        }
                        else
                        {
                            return NegativeInfinity;
                        }
                    }
                    else
                    {
                        if (left._Magnitude == 0)
                        {
                            return NaN;
                        }
                        else if (left._Magnitude > 0)
                        {
                            return NegativeInfinity;
                        }
                        else
                        {
                            return PositiveInfinity;
                        }
                    }
                }
            }
            else if (left._Value == 0 || right._Value == 0)
            {
                return Zero;
            }
            else
            {
                bool LIsOne = (left == One);
                bool LIsMinusOne = (left == MinusOne);
                bool RIsOne = (right == One);
                bool RIsMinusOne = (right == MinusOne);

                if (LIsOne || RIsOne)
                {
                    if (LIsOne && RIsOne)
                    {
                        return One;
                    }
                    else if (LIsOne)
                    {
                        return right;
                    }
                    else
                    {
                        return left;
                    }
                }
                else if (LIsMinusOne || RIsMinusOne)
                {
                    if (LIsMinusOne && RIsMinusOne)
                    {
                        return One;
                    }
                    else if (LIsMinusOne)
                    {
                        return new Real(-right._Value, right._Magnitude);
                    }
                    else
                    {
                        return new Real(-left._Value, left._Magnitude);
                    }
                }
                else
                {
                    long NewMag = left._Magnitude + right._Magnitude;

                    if (NewMag < _MinMagnitude)
                    {
                        double NewVal = left._Value * right._Value;

                        if (NewMag + 1 == _MinMagnitude && (NewVal <= -10 || NewVal >= 10))
                        {
                            return new Real(NewVal / 10, _MinMagnitude);
                        }
                        else
                        {
                            return Zero;
                        }
                    }
                    else if (NewMag > _MaxMagnitude)
                    {
                        if (Math.Sign(left._Value) == Math.Sign(right._Value))
                        {
                            return PositiveInfinity;
                        }
                        else
                        {
                            return NegativeInfinity;
                        }
                    }
                    else
                    {
                        return new Real(left._Value * right._Value, NewMag);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将 Real 结构与 Real 结构的相除得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="left">Real 结构，表示被除数。</param>
        /// <param name="right">Real 结构，表示除数。</param>
        /// <returns>Real 结构，表示将 Real 结构与 Real 结构的相除得到的结果。</returns>
        public static Real operator /(Real left, Real right)
        {
            if (left.IsNaN || right.IsNaN)
            {
                return NaN;
            }
            else if (left.IsInfinity || right.IsInfinity)
            {
                bool LIsInf = left.IsInfinity;
                bool LIsPosInf = left.IsPositiveInfinity;
                bool LIsNegInf = left.IsNegativeInfinity;
                bool RIsInf = right.IsInfinity;
                bool RIsPosInf = right.IsPositiveInfinity;
                bool RIsNegInf = right.IsNegativeInfinity;

                if (LIsInf && RIsInf)
                {
                    return NaN;
                }
                else
                {
                    if (LIsPosInf)
                    {
                        if (right._Magnitude >= 0)
                        {
                            return PositiveInfinity;
                        }
                        else
                        {
                            return NegativeInfinity;
                        }
                    }
                    else if (LIsNegInf)
                    {
                        if (right._Magnitude >= 0)
                        {
                            return NegativeInfinity;
                        }
                        else
                        {
                            return PositiveInfinity;
                        }
                    }
                    else
                    {
                        return Zero;
                    }
                }
            }
            else if (left._Value == 0 || right._Value == 0)
            {
                if (left._Value == 0 && right._Value == 0)
                {
                    return NaN;
                }
                else if (left._Value == 0)
                {
                    return Zero;
                }
                else
                {
                    if (left._Value > 0)
                    {
                        return PositiveInfinity;
                    }
                    else
                    {
                        return NegativeInfinity;
                    }
                }
            }
            else
            {
                bool LIsOne = (left == One);
                bool LIsMinusOne = (left == MinusOne);
                bool RIsOne = (right == One);
                bool RIsMinusOne = (right == MinusOne);

                if (LIsOne || RIsOne)
                {
                    if (LIsOne && RIsOne)
                    {
                        return One;
                    }
                    else if (LIsOne)
                    {
                        return new Real(1 / right._Value, -right._Magnitude);
                    }
                    else
                    {
                        return left;
                    }
                }
                else if (LIsMinusOne || RIsMinusOne)
                {
                    if (LIsMinusOne && RIsMinusOne)
                    {
                        return One;
                    }
                    else if (LIsMinusOne)
                    {
                        return new Real(-1 / right._Value, -right._Magnitude);
                    }
                    else
                    {
                        return new Real(-left._Value, left._Magnitude);
                    }
                }
                else
                {
                    long NewMag = left._Magnitude - right._Magnitude;

                    if (NewMag < _MinMagnitude)
                    {
                        return Zero;
                    }
                    else if (NewMag > _MaxMagnitude)
                    {
                        double NewVal = left._Value / right._Value;

                        if (NewMag - 1 == _MaxMagnitude && (NewVal > -1 && NewVal < 1))
                        {
                            return new Real(NewVal * 10, _MaxMagnitude);
                        }
                        else
                        {
                            if (Math.Sign(left._Value) == Math.Sign(right._Value))
                            {
                                return PositiveInfinity;
                            }
                            else
                            {
                                return NegativeInfinity;
                            }
                        }
                    }
                    else
                    {
                        return new Real(left._Value / right._Value, NewMag);
                    }
                }
            }
        }

        //

        /// <summary>
        /// 返回表示将 Real 结构与 Real 结构的相除得到的余数的 Real 结构的新实例。
        /// </summary>
        /// <param name="left">Real 结构，表示被除数。</param>
        /// <param name="right">Real 结构，表示除数。</param>
        /// <returns>Real 结构，表示将 Real 结构与 Real 结构的相除得到的余数。</returns>
        public static Real operator %(Real left, Real right)
        {
            if (left.IsNaN || right.IsNaN)
            {
                return NaN;
            }
            else if (left.IsInfinity || right.IsInfinity)
            {
                bool LIsInf = left.IsInfinity;
                bool RIsInf = right.IsInfinity;

                if (LIsInf && RIsInf)
                {
                    return NaN;
                }
                else
                {
                    if (LIsInf)
                    {
                        return NaN;
                    }
                    else
                    {
                        return left;
                    }
                }
            }
            else if (left._Value == 0 || right._Value == 0)
            {
                if (left._Value == 0 && right._Value == 0)
                {
                    return NaN;
                }
                else if (left._Value == 0)
                {
                    return Zero;
                }
                else
                {
                    return NaN;
                }
            }
            else
            {
                int ValSignL = Math.Sign(left._Value);
                double ValAbsR = Math.Abs(right._Value);

                Real Rem = Abs(left);

                while (Rem._Value > 0 && (Rem._Magnitude > right._Magnitude || (Rem._Magnitude == right._Magnitude && Rem._Value > ValAbsR)))
                {
                    if (Rem._Value > ValAbsR)
                    {
                        Rem._Value -= Math.Truncate(Rem._Value / ValAbsR) * ValAbsR;
                    }
                    else
                    {
                        Rem._Value = Rem._Value * 10 - Math.Truncate(Rem._Value * 10 / ValAbsR) * ValAbsR;
                        Rem._Magnitude--;
                    }

                    if (Rem._Value < 1)
                    {
                        Rem._Value *= 10;
                        Rem._Magnitude--;
                    }
                }

                Rem._Value *= ValSignL;
                Rem._Rectify();

                return Rem;
            }
        }

        //

        /// <summary>
        /// 返回对 Real 结构计算幂得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="left">Real 结构，表示底数。</param>
        /// <param name="right">Real 结构，表示指数。</param>
        /// <returns>Real 结构，表示对 Real 结构计算幂得到的结果。</returns>
        public static Real operator ^(Real left, Real right)
        {
            if (left.IsNaN || right.IsNaN)
            {
                return NaN;
            }
            else if (left.IsInfinity || right.IsInfinity)
            {
                bool LIsInf = left.IsInfinity;
                bool LIsPosInf = left.IsPositiveInfinity;
                bool LIsNegInf = left.IsNegativeInfinity;
                bool RIsInf = right.IsInfinity;
                bool RIsPosInf = right.IsPositiveInfinity;
                bool RIsNegInf = right.IsNegativeInfinity;

                if (LIsInf && RIsInf)
                {
                    if (LIsPosInf && RIsPosInf)
                    {
                        return PositiveInfinity;
                    }
                    else if (LIsPosInf && RIsNegInf)
                    {
                        return Zero;
                    }
                    else if (LIsNegInf && RIsPosInf)
                    {
                        return PositiveInfinity;
                    }
                    else
                    {
                        return Zero;
                    }
                }
                else
                {
                    if (LIsPosInf)
                    {
                        if (right._Value == 0)
                        {
                            return One;
                        }
                        else if (right._Value < 0)
                        {
                            return Zero;
                        }
                        else
                        {
                            return PositiveInfinity;
                        }
                    }
                    else if (LIsNegInf)
                    {
                        if (right._Value == 0)
                        {
                            return One;
                        }
                        else if (right._Value < 0)
                        {
                            return Zero;
                        }
                        else
                        {
                            switch (right._GetParity())
                            {
                                case Parity.Even: return PositiveInfinity;
                                case Parity.Odd: return NegativeInfinity;
                                default: return NaN;
                            }
                        }
                    }
                    else if (RIsPosInf)
                    {
                        if (left._Value == 0)
                        {
                            return Zero;
                        }
                        else if (left._Value < 0)
                        {
                            if (left == MinusOne)
                            {
                                return NaN;
                            }
                            else if (left < MinusOne)
                            {
                                return PositiveInfinity;
                            }
                            else
                            {
                                return Zero;
                            }
                        }
                        else
                        {
                            if (left == One)
                            {
                                return One;
                            }
                            else if (left > One)
                            {
                                return PositiveInfinity;
                            }
                            else
                            {
                                return Zero;
                            }
                        }
                    }
                    else
                    {
                        if (left._Value == 0)
                        {
                            return PositiveInfinity;
                        }
                        else if (left._Value < 0)
                        {
                            if (left < MinusOne)
                            {
                                return Zero;
                            }
                            else if (left > MinusOne)
                            {
                                return PositiveInfinity;
                            }
                            else
                            {
                                return NaN;
                            }
                        }
                        else
                        {
                            if (left > One)
                            {
                                return Zero;
                            }
                            else if (left < One)
                            {
                                return PositiveInfinity;
                            }
                            else
                            {
                                return One;
                            }
                        }
                    }
                }
            }
            else if (left._Value == 0 || right._Value == 0)
            {
                if (left._Value == 0 && right._Value == 0)
                {
                    return One;
                }
                else if (left._Value == 0)
                {
                    if (right._Value > 0)
                    {
                        return Zero;
                    }
                    else
                    {
                        return PositiveInfinity;
                    }
                }
                else
                {
                    return One;
                }
            }
            else
            {
                bool LIsOne = (left == One);
                bool LIsMinusOne = (left == MinusOne);
                bool RIsOne = (right == One);
                bool RIsMinusOne = (right == MinusOne);

                if (LIsOne || RIsOne)
                {
                    if (LIsOne && RIsOne)
                    {
                        return One;
                    }
                    else if (LIsOne)
                    {
                        return One;
                    }
                    else
                    {
                        return left;
                    }
                }
                else if (LIsMinusOne || RIsMinusOne)
                {
                    if (LIsMinusOne && RIsMinusOne)
                    {
                        return MinusOne;
                    }
                    else if (LIsMinusOne)
                    {
                        switch (right._GetParity())
                        {
                            case Parity.Even: return One;
                            case Parity.Odd: return MinusOne;
                            default: return NaN;
                        }
                    }
                    else
                    {
                        return new Real(1 / right._Value, -right._Magnitude);
                    }
                }
                else
                {
                    Func<Real, Real, Real> FastPower = (baseNum, expNum) =>
                    {
                        double Lg0;

                        if (baseNum._Value == 1)
                        {
                            Lg0 = baseNum._Magnitude * expNum._Value;
                        }
                        else
                        {
                            Lg0 = Math.Log10(Math.Pow(baseNum._Value, expNum._Value)) + baseNum._Magnitude * expNum._Value;
                        }

                        long M0 = (long)Math.Truncate(Lg0);
                        double V0 = Math.Pow(10, Lg0 - M0);

                        Real result = new Real(V0, M0);

                        if (!result.IsNaN && !result.IsInfinity && result._Value != 0)
                        {
                            if (expNum._Magnitude != 0)
                            {
                                double Lgi;

                                if (expNum._Magnitude > 0)
                                {
                                    for (long i = 0; i < expNum._Magnitude; i++)
                                    {
                                        Lgi = Math.Log10(Math.Pow(result._Value, 10)) + 10 * result._Magnitude;

                                        result._Magnitude = (long)Math.Truncate(Lgi);
                                        result._Value = Math.Pow(10, Lgi - result._Magnitude);
                                        result._Rectify();

                                        if (result.IsNaN || result.IsInfinity || result._Value == 0)
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    for (long i = 0; i < -expNum._Magnitude; i++)
                                    {
                                        Lgi = Math.Log10(Math.Pow(result._Value, 0.1)) + 0.1 * result._Magnitude;

                                        result._Magnitude = (long)Math.Truncate(Lgi);
                                        result._Value = Math.Pow(10, Lgi - result._Magnitude);
                                        result._Rectify();

                                        if (result.IsNaN || result.IsInfinity || result._Value == 0)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        return result;
                    };

                    if (right._Value < 0)
                    {
                        if (left._Value < 0)
                        {
                            switch (right._GetParity())
                            {
                                case Parity.Even: return (One / FastPower(-left, -right));
                                case Parity.Odd: return (MinusOne / FastPower(-left, -right));
                                default: return NaN;
                            }
                        }
                        else
                        {
                            return (One / FastPower(left, -right));
                        }
                    }
                    else
                    {
                        if (left._Value < 0)
                        {
                            switch (right._GetParity())
                            {
                                case Parity.Even: return FastPower(-left, right);
                                case Parity.Odd: return -FastPower(-left, right);
                                default: return NaN;
                            }
                        }
                        else
                        {
                            return FastPower(left, right);
                        }
                    }
                }
            }
        }

        //

        /// <summary>
        /// 将指定的 Real 结构显式转换为双精度浮点数。
        /// </summary>
        /// <param name="real">用于转换的 Real 结构。</param>
        /// <returns>双精度浮点数，表示显式转换的结果。</returns>
        public static explicit operator double(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                return double.NaN;
            }
            else if (double.IsInfinity(real._Value))
            {
                if (double.IsPositiveInfinity(real._Value))
                {
                    return double.PositiveInfinity;
                }
                else
                {
                    return double.NegativeInfinity;
                }
            }
            else if (real._Value == 0)
            {
                return 0;
            }
            else
            {
                if (real._Magnitude == 0)
                {
                    return real._Value;
                }
                else if (real._Magnitude > 308)
                {
                    return (real._Value > 0 ? double.PositiveInfinity : double.NegativeInfinity);
                }
                else if (real._Magnitude < -324)
                {
                    return 0;
                }
                else
                {
                    if (real < double.MinValue || real > double.MaxValue)
                    {
                        return (real._Value > 0 ? double.PositiveInfinity : double.NegativeInfinity);
                    }
                    else
                    {
                        return real._Value * Math.Pow(10, real._Magnitude);
                    }
                }
            }
        }

        /// <summary>
        /// 将指定的 Real 结构显式转换为单精度浮点数。
        /// </summary>
        /// <param name="real">用于转换的 Real 结构。</param>
        /// <returns>单精度浮点数，表示显式转换的结果。</returns>
        public static explicit operator float(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                return float.NaN;
            }
            else if (double.IsInfinity(real._Value))
            {
                if (double.IsPositiveInfinity(real._Value))
                {
                    return float.PositiveInfinity;
                }
                else
                {
                    return float.NegativeInfinity;
                }
            }
            else if (real._Value == 0)
            {
                return 0;
            }
            else
            {
                if (real._Magnitude == 0)
                {
                    return (float)real._Value;
                }
                else if (real._Magnitude > 38)
                {
                    return (real._Value > 0 ? float.PositiveInfinity : float.NegativeInfinity);
                }
                else if (real._Magnitude < -45)
                {
                    return 0;
                }
                else
                {
                    double Val = real._Value * Math.Pow(10, real._Magnitude);

                    if (Val < float.MinValue || Val > float.MaxValue)
                    {
                        return (real._Value > 0 ? float.PositiveInfinity : float.NegativeInfinity);
                    }
                    else
                    {
                        return (float)Val;
                    }
                }
            }
        }

        /// <summary>
        /// 将指定的 Real 结构显式转换为十进制数。
        /// </summary>
        /// <param name="real">用于转换的 Real 结构。</param>
        /// <returns>十进制数，表示显式转换的结果。</returns>
        public static explicit operator decimal(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                throw new OverflowException();
            }
            else if (double.IsInfinity(real._Value))
            {
                throw new OverflowException();
            }
            else if (real._Value == 0)
            {
                return 0;
            }
            else
            {
                if (real._Magnitude == 0)
                {
                    return (decimal)real._Value;
                }
                else if (real._Magnitude > 28)
                {
                    throw new OverflowException();
                }
                else if (real._Magnitude < -28)
                {
                    return 0;
                }
                else
                {
                    double Val = real._Value * (real._Magnitude > 0 ? _LargePositiveGeometricValues[real._Magnitude] : _SmallPositiveGeometricValues[-real._Magnitude]);

                    if (Val < (double)decimal.MinValue || Val > (double)decimal.MaxValue)
                    {
                        throw new OverflowException();
                    }
                    else
                    {
                        return (decimal)Val;
                    }
                }
            }
        }

        /// <summary>
        /// 将指定的 Real 结构显式转换为 64 位无符号整数。
        /// </summary>
        /// <param name="real">用于转换的 Real 结构。</param>
        /// <returns>64 位无符号整数，表示显式转换的结果。</returns>
        public static explicit operator ulong(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                throw new OverflowException();
            }
            else if (double.IsInfinity(real._Value))
            {
                throw new OverflowException();
            }
            else if (real._Value == 0)
            {
                return 0;
            }
            else
            {
                if (real._Magnitude == 0)
                {
                    return (ulong)real._Value;
                }
                else if (real._Magnitude > 19)
                {
                    throw new OverflowException();
                }
                else if (real._Magnitude < 0)
                {
                    return 0;
                }
                else
                {
                    double Val = real._Value * _LargePositiveGeometricValues[real._Magnitude];

                    if (Val < ulong.MinValue || Val > ulong.MaxValue)
                    {
                        throw new OverflowException();
                    }
                    else
                    {
                        return (ulong)Val;
                    }
                }
            }
        }

        /// <summary>
        /// 将指定的 Real 结构显式转换为 64 位整数。
        /// </summary>
        /// <param name="real">用于转换的 Real 结构。</param>
        /// <returns>64 位整数，表示显式转换的结果。</returns>
        public static explicit operator long(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                throw new OverflowException();
            }
            else if (double.IsInfinity(real._Value))
            {
                throw new OverflowException();
            }
            else if (real._Value == 0)
            {
                return 0;
            }
            else
            {
                if (real._Magnitude == 0)
                {
                    return (long)real._Value;
                }
                else if (real._Magnitude > 18)
                {
                    throw new OverflowException();
                }
                else if (real._Magnitude < 0)
                {
                    return 0;
                }
                else
                {
                    double Val = real._Value * _LargePositiveGeometricValues[real._Magnitude];

                    if (Val < long.MinValue || Val > long.MaxValue)
                    {
                        throw new OverflowException();
                    }
                    else
                    {
                        return (long)Val;
                    }
                }
            }
        }

        /// <summary>
        /// 将指定的 Real 结构显式转换为 32 位无符号整数。
        /// </summary>
        /// <param name="real">用于转换的 Real 结构。</param>
        /// <returns>32 位无符号整数，表示显式转换的结果。</returns>
        public static explicit operator uint(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                throw new OverflowException();
            }
            else if (double.IsInfinity(real._Value))
            {
                throw new OverflowException();
            }
            else if (real._Value == 0)
            {
                return 0;
            }
            else
            {
                if (real._Magnitude == 0)
                {
                    return (uint)real._Value;
                }
                else if (real._Magnitude > 9)
                {
                    throw new OverflowException();
                }
                else if (real._Magnitude < 0)
                {
                    return 0;
                }
                else
                {
                    double Val = real._Value * _LargePositiveGeometricValues[real._Magnitude];

                    if (Val < uint.MinValue || Val > uint.MaxValue)
                    {
                        throw new OverflowException();
                    }
                    else
                    {
                        return (uint)Val;
                    }
                }
            }
        }

        /// <summary>
        /// 将指定的 Real 结构显式转换为 32 位整数。
        /// </summary>
        /// <param name="real">用于转换的 Real 结构。</param>
        /// <returns>32 位整数，表示显式转换的结果。</returns>
        public static explicit operator int(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                throw new OverflowException();
            }
            else if (double.IsInfinity(real._Value))
            {
                throw new OverflowException();
            }
            else if (real._Value == 0)
            {
                return 0;
            }
            else
            {
                if (real._Magnitude == 0)
                {
                    return (int)real._Value;
                }
                else if (real._Magnitude > 9)
                {
                    throw new OverflowException();
                }
                else if (real._Magnitude < 0)
                {
                    return 0;
                }
                else
                {
                    double Val = real._Value * _LargePositiveGeometricValues[real._Magnitude];

                    if (Val < int.MinValue || Val > int.MaxValue)
                    {
                        throw new OverflowException();
                    }
                    else
                    {
                        return (int)Val;
                    }
                }
            }
        }

        /// <summary>
        /// 将指定的 Real 结构显式转换为 16 位无符号整数。
        /// </summary>
        /// <param name="real">用于转换的 Real 结构。</param>
        /// <returns>16 位无符号整数，表示显式转换的结果。</returns>
        public static explicit operator ushort(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                throw new OverflowException();
            }
            else if (double.IsInfinity(real._Value))
            {
                throw new OverflowException();
            }
            else if (real._Value == 0)
            {
                return 0;
            }
            else
            {
                if (real._Magnitude == 0)
                {
                    return (ushort)real._Value;
                }
                else if (real._Magnitude > 4)
                {
                    throw new OverflowException();
                }
                else if (real._Magnitude < 0)
                {
                    return 0;
                }
                else
                {
                    double Val = real._Value * _LargePositiveGeometricValues[real._Magnitude];

                    if (Val < ushort.MinValue || Val > ushort.MaxValue)
                    {
                        throw new OverflowException();
                    }
                    else
                    {
                        return (ushort)Val;
                    }
                }
            }
        }

        /// <summary>
        /// 将指定的 Real 结构显式转换为 16 位整数。
        /// </summary>
        /// <param name="real">用于转换的 Real 结构。</param>
        /// <returns>16 位整数，表示显式转换的结果。</returns>
        public static explicit operator short(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                throw new OverflowException();
            }
            else if (double.IsInfinity(real._Value))
            {
                throw new OverflowException();
            }
            else if (real._Value == 0)
            {
                return 0;
            }
            else
            {
                if (real._Magnitude == 0)
                {
                    return (short)real._Value;
                }
                else if (real._Magnitude > 4)
                {
                    throw new OverflowException();
                }
                else if (real._Magnitude < 0)
                {
                    return 0;
                }
                else
                {
                    double Val = real._Value * _LargePositiveGeometricValues[real._Magnitude];

                    if (Val < short.MinValue || Val > short.MaxValue)
                    {
                        throw new OverflowException();
                    }
                    else
                    {
                        return (short)Val;
                    }
                }
            }
        }

        /// <summary>
        /// 将指定的 Real 结构显式转换为 8 位无符号整数。
        /// </summary>
        /// <param name="real">用于转换的 Real 结构。</param>
        /// <returns>8 位无符号整数，表示显式转换的结果。</returns>
        public static explicit operator byte(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                throw new OverflowException();
            }
            else if (double.IsInfinity(real._Value))
            {
                throw new OverflowException();
            }
            else if (real._Value == 0)
            {
                return 0;
            }
            else
            {
                if (real._Magnitude == 0)
                {
                    return (byte)real._Value;
                }
                else if (real._Magnitude > 2)
                {
                    throw new OverflowException();
                }
                else if (real._Magnitude < 0)
                {
                    return 0;
                }
                else
                {
                    double Val = real._Value * _LargePositiveGeometricValues[real._Magnitude];

                    if (Val < byte.MinValue || Val > byte.MaxValue)
                    {
                        throw new OverflowException();
                    }
                    else
                    {
                        return (byte)Val;
                    }
                }
            }
        }

        /// <summary>
        /// 将指定的 Real 结构显式转换为 8 位整数。
        /// </summary>
        /// <param name="real">用于转换的 Real 结构。</param>
        /// <returns>8 位整数，表示显式转换的结果。</returns>
        public static explicit operator sbyte(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                throw new OverflowException();
            }
            else if (double.IsInfinity(real._Value))
            {
                throw new OverflowException();
            }
            else if (real._Value == 0)
            {
                return 0;
            }
            else
            {
                if (real._Magnitude == 0)
                {
                    return (sbyte)real._Value;
                }
                else if (real._Magnitude > 2)
                {
                    throw new OverflowException();
                }
                else if (real._Magnitude < 0)
                {
                    return 0;
                }
                else
                {
                    double Val = real._Value * _LargePositiveGeometricValues[real._Magnitude];

                    if (Val < sbyte.MinValue || Val > sbyte.MaxValue)
                    {
                        throw new OverflowException();
                    }
                    else
                    {
                        return (sbyte)Val;
                    }
                }
            }
        }

        //

        /// <summary>
        /// 将指定的双精度浮点数隐式转换为 Real 结构。
        /// </summary>
        /// <param name="value">用于转换的双精度浮点数。</param>
        /// <returns>Real 结构，表示隐式转换的结果。</returns>
        public static implicit operator Real(double value)
        {
            return new Real(value);
        }

        /// <summary>
        /// 将指定的单精度浮点数隐式转换为 Real 结构。
        /// </summary>
        /// <param name="value">用于转换的单精度浮点数。</param>
        /// <returns>Real 结构，表示隐式转换的结果。</returns>
        public static implicit operator Real(float value)
        {
            return new Real(value);
        }

        /// <summary>
        /// 将指定的十进制数显式转换为 Real 结构。
        /// </summary>
        /// <param name="value">用于转换的十进制数。</param>
        /// <returns>Real 结构，表示显式转换的结果。</returns>
        public static explicit operator Real(decimal value)
        {
            return new Real(value);
        }

        /// <summary>
        /// 将指定的 64 位无符号整数隐式转换为 Real 结构。
        /// </summary>
        /// <param name="value">用于转换的 64 位无符号整数。</param>
        /// <returns>Real 结构，表示隐式转换的结果。</returns>
        public static implicit operator Real(ulong value)
        {
            return new Real(value);
        }

        /// <summary>
        /// 将指定的 64 位整数隐式转换为 Real 结构。
        /// </summary>
        /// <param name="value">用于转换的 64 位整数。</param>
        /// <returns>Real 结构，表示隐式转换的结果。</returns>
        public static implicit operator Real(long value)
        {
            return new Real(value);
        }

        /// <summary>
        /// 将指定的 32 位无符号整数隐式转换为 Real 结构。
        /// </summary>
        /// <param name="value">用于转换的 32 位无符号整数。</param>
        /// <returns>Real 结构，表示隐式转换的结果。</returns>
        public static implicit operator Real(uint value)
        {
            return new Real(value);
        }

        /// <summary>
        /// 将指定的 32 位整数隐式转换为 Real 结构。
        /// </summary>
        /// <param name="value">用于转换的 32 位整数。</param>
        /// <returns>Real 结构，表示隐式转换的结果。</returns>
        public static implicit operator Real(int value)
        {
            return new Real(value);
        }

        /// <summary>
        /// 将指定的 16 位无符号整数隐式转换为 Real 结构。
        /// </summary>
        /// <param name="value">用于转换的 16 位无符号整数。</param>
        /// <returns>Real 结构，表示隐式转换的结果。</returns>
        public static implicit operator Real(ushort value)
        {
            return new Real(value);
        }

        /// <summary>
        /// 将指定的 16 位整数隐式转换为 Real 结构。
        /// </summary>
        /// <param name="value">用于转换的 16 位整数。</param>
        /// <returns>Real 结构，表示隐式转换的结果。</returns>
        public static implicit operator Real(short value)
        {
            return new Real(value);
        }

        /// <summary>
        /// 将指定的 8 位无符号整数隐式转换为 Real 结构。
        /// </summary>
        /// <param name="value">用于转换的 8 位无符号整数。</param>
        /// <returns>Real 结构，表示隐式转换的结果。</returns>
        public static implicit operator Real(byte value)
        {
            return new Real(value);
        }

        /// <summary>
        /// 将指定的 8 位整数隐式转换为 Real 结构。
        /// </summary>
        /// <param name="value">用于转换的 8 位整数。</param>
        /// <returns>Real 结构，表示隐式转换的结果。</returns>
        public static implicit operator Real(sbyte value)
        {
            return new Real(value);
        }

        #endregion
    }
}