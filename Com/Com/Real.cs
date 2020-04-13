/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.Real
Version 19.12.1.0000

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

        private const long _MinMagnitude = -999999999999999; // 数量级的最小值，等于 double 能够精确表示的最大负整数。
        private const long _MaxMagnitude = 999999999999999; // 数量级的最大值，等于 double 能够精确表示的最大正整数。

        //

        private static readonly double[] _PositiveMagnitudeGeometricValues = new double[] // 用于在一定范围内进行类型转换的正数量级等比数列。
        {
            1E+000, 1E+001, 1E+002, 1E+003, 1E+004, 1E+005, 1E+006, 1E+007, 1E+008, 1E+009,
            1E+010, 1E+011, 1E+012, 1E+013, 1E+014, 1E+015, 1E+016, 1E+017, 1E+018, 1E+019,
            1E+020, 1E+021, 1E+022, 1E+023, 1E+024, 1E+025, 1E+026, 1E+027, 1E+028, 1E+029,
            1E+030, 1E+031, 1E+032, 1E+033, 1E+034, 1E+035, 1E+036, 1E+037, 1E+038, 1E+039,
            1E+040, 1E+041, 1E+042, 1E+043, 1E+044, 1E+045, 1E+046, 1E+047, 1E+048, 1E+049,
            1E+050, 1E+051, 1E+052, 1E+053, 1E+054, 1E+055, 1E+056, 1E+057, 1E+058, 1E+059,
            1E+060, 1E+061, 1E+062, 1E+063, 1E+064, 1E+065, 1E+066, 1E+067, 1E+068, 1E+069,
            1E+070, 1E+071, 1E+072, 1E+073, 1E+074, 1E+075, 1E+076, 1E+077, 1E+078, 1E+079,
            1E+080, 1E+081, 1E+082, 1E+083, 1E+084, 1E+085, 1E+086, 1E+087, 1E+088, 1E+089,
            1E+090, 1E+091, 1E+092, 1E+093, 1E+094, 1E+095, 1E+096, 1E+097, 1E+098, 1E+099,
            1E+100, 1E+101, 1E+102, 1E+103, 1E+104, 1E+105, 1E+106, 1E+107, 1E+108, 1E+109,
            1E+110, 1E+111, 1E+112, 1E+113, 1E+114, 1E+115, 1E+116, 1E+117, 1E+118, 1E+119,
            1E+120, 1E+121, 1E+122, 1E+123, 1E+124, 1E+125, 1E+126, 1E+127, 1E+128, 1E+129,
            1E+130, 1E+131, 1E+132, 1E+133, 1E+134, 1E+135, 1E+136, 1E+137, 1E+138, 1E+139,
            1E+140, 1E+141, 1E+142, 1E+143, 1E+144, 1E+145, 1E+146, 1E+147, 1E+148, 1E+149,
            1E+150, 1E+151, 1E+152, 1E+153, 1E+154, 1E+155, 1E+156, 1E+157, 1E+158, 1E+159,
            1E+160, 1E+161, 1E+162, 1E+163, 1E+164, 1E+165, 1E+166, 1E+167, 1E+168, 1E+169,
            1E+170, 1E+171, 1E+172, 1E+173, 1E+174, 1E+175, 1E+176, 1E+177, 1E+178, 1E+179,
            1E+180, 1E+181, 1E+182, 1E+183, 1E+184, 1E+185, 1E+186, 1E+187, 1E+188, 1E+189,
            1E+190, 1E+191, 1E+192, 1E+193, 1E+194, 1E+195, 1E+196, 1E+197, 1E+198, 1E+199,
            1E+200, 1E+201, 1E+202, 1E+203, 1E+204, 1E+205, 1E+206, 1E+207, 1E+208, 1E+209,
            1E+210, 1E+211, 1E+212, 1E+213, 1E+214, 1E+215, 1E+216, 1E+217, 1E+218, 1E+219,
            1E+220, 1E+221, 1E+222, 1E+223, 1E+224, 1E+225, 1E+226, 1E+227, 1E+228, 1E+229,
            1E+230, 1E+231, 1E+232, 1E+233, 1E+234, 1E+235, 1E+236, 1E+237, 1E+238, 1E+239,
            1E+240, 1E+241, 1E+242, 1E+243, 1E+244, 1E+245, 1E+246, 1E+247, 1E+248, 1E+249,
            1E+250, 1E+251, 1E+252, 1E+253, 1E+254, 1E+255, 1E+256, 1E+257, 1E+258, 1E+259,
            1E+260, 1E+261, 1E+262, 1E+263, 1E+264, 1E+265, 1E+266, 1E+267, 1E+268, 1E+269,
            1E+270, 1E+271, 1E+272, 1E+273, 1E+274, 1E+275, 1E+276, 1E+277, 1E+278, 1E+279,
            1E+280, 1E+281, 1E+282, 1E+283, 1E+284, 1E+285, 1E+286, 1E+287, 1E+288, 1E+289,
            1E+290, 1E+291, 1E+292, 1E+293, 1E+294, 1E+295, 1E+296, 1E+297, 1E+298, 1E+299,
            1E+300, 1E+301, 1E+302, 1E+303, 1E+304, 1E+305, 1E+306, 1E+307, 1E+308, double.MaxValue
        };

        private static readonly double[] _NegativeMagnitudeGeometricValues = new double[] // 用于在一定范围内进行类型转换的负数量级等比数列。
        {
            1E-000, 1E-001, 1E-002, 1E-003, 1E-004, 1E-005, 1E-006, 1E-007, 1E-008, 1E-009,
            1E-010, 1E-011, 1E-012, 1E-013, 1E-014, 1E-015, 1E-016, 1E-017, 1E-018, 1E-019,
            1E-020, 1E-021, 1E-022, 1E-023, 1E-024, 1E-025, 1E-026, 1E-027, 1E-028, 1E-029,
            1E-030, 1E-031, 1E-032, 1E-033, 1E-034, 1E-035, 1E-036, 1E-037, 1E-038, 1E-039,
            1E-040, 1E-041, 1E-042, 1E-043, 1E-044, 1E-045, 1E-046, 1E-047, 1E-048, 1E-049,
            1E-050, 1E-051, 1E-052, 1E-053, 1E-054, 1E-055, 1E-056, 1E-057, 1E-058, 1E-059,
            1E-060, 1E-061, 1E-062, 1E-063, 1E-064, 1E-065, 1E-066, 1E-067, 1E-068, 1E-069,
            1E-070, 1E-071, 1E-072, 1E-073, 1E-074, 1E-075, 1E-076, 1E-077, 1E-078, 1E-079,
            1E-080, 1E-081, 1E-082, 1E-083, 1E-084, 1E-085, 1E-086, 1E-087, 1E-088, 1E-089,
            1E-090, 1E-091, 1E-092, 1E-093, 1E-094, 1E-095, 1E-096, 1E-097, 1E-098, 1E-099,
            1E-100, 1E-101, 1E-102, 1E-103, 1E-104, 1E-105, 1E-106, 1E-107, 1E-108, 1E-109,
            1E-110, 1E-111, 1E-112, 1E-113, 1E-114, 1E-115, 1E-116, 1E-117, 1E-118, 1E-119,
            1E-120, 1E-121, 1E-122, 1E-123, 1E-124, 1E-125, 1E-126, 1E-127, 1E-128, 1E-129,
            1E-130, 1E-131, 1E-132, 1E-133, 1E-134, 1E-135, 1E-136, 1E-137, 1E-138, 1E-139,
            1E-140, 1E-141, 1E-142, 1E-143, 1E-144, 1E-145, 1E-146, 1E-147, 1E-148, 1E-149,
            1E-150, 1E-151, 1E-152, 1E-153, 1E-154, 1E-155, 1E-156, 1E-157, 1E-158, 1E-159,
            1E-160, 1E-161, 1E-162, 1E-163, 1E-164, 1E-165, 1E-166, 1E-167, 1E-168, 1E-169,
            1E-170, 1E-171, 1E-172, 1E-173, 1E-174, 1E-175, 1E-176, 1E-177, 1E-178, 1E-179,
            1E-180, 1E-181, 1E-182, 1E-183, 1E-184, 1E-185, 1E-186, 1E-187, 1E-188, 1E-189,
            1E-190, 1E-191, 1E-192, 1E-193, 1E-194, 1E-195, 1E-196, 1E-197, 1E-198, 1E-199,
            1E-200, 1E-201, 1E-202, 1E-203, 1E-204, 1E-205, 1E-206, 1E-207, 1E-208, 1E-209,
            1E-210, 1E-211, 1E-212, 1E-213, 1E-214, 1E-215, 1E-216, 1E-217, 1E-218, 1E-219,
            1E-220, 1E-221, 1E-222, 1E-223, 1E-224, 1E-225, 1E-226, 1E-227, 1E-228, 1E-229,
            1E-230, 1E-231, 1E-232, 1E-233, 1E-234, 1E-235, 1E-236, 1E-237, 1E-238, 1E-239,
            1E-240, 1E-241, 1E-242, 1E-243, 1E-244, 1E-245, 1E-246, 1E-247, 1E-248, 1E-249,
            1E-250, 1E-251, 1E-252, 1E-253, 1E-254, 1E-255, 1E-256, 1E-257, 1E-258, 1E-259,
            1E-260, 1E-261, 1E-262, 1E-263, 1E-264, 1E-265, 1E-266, 1E-267, 1E-268, 1E-269,
            1E-270, 1E-271, 1E-272, 1E-273, 1E-274, 1E-275, 1E-276, 1E-277, 1E-278, 1E-279,
            1E-280, 1E-281, 1E-282, 1E-283, 1E-284, 1E-285, 1E-286, 1E-287, 1E-288, 1E-289,
            1E-290, 1E-291, 1E-292, 1E-293, 1E-294, 1E-295, 1E-296, 1E-297, 1E-298, 1E-299,
            1E-300, 1E-301, 1E-302, 1E-303, 1E-304, 1E-305, 1E-306, 1E-307, 1E-308, 1E-309,
            1E-310, 1E-311, 1E-312, 1E-313, 1E-314, 1E-315, 1E-316, 1E-317, 1E-318, 1E-319,
            1E-320, 1E-321, 1E-322, 1E-323, double.Epsilon
        };

        //

        private static readonly Real _Pi = new Real(Constant.Pi); // 表示圆周率 π 的 Real 结构的实例。
        private static readonly Real _DoublePi = new Real(Constant.DoublePi); // 表示圆周率 π 的 2 倍的 Real 结构的实例。
        private static readonly Real _HalfPi = new Real(Constant.HalfPi); // 表示圆周率 π 的 1/2 的 Real 结构的实例。
        private static readonly Real _MinusHalfPi = new Real(-Constant.HalfPi); // 表示圆周率 π 的 -1/2 的 Real 结构的实例。

        private static readonly Real _E = new Real(Constant.E); // 表示自然常数 E 的 Real 结构的实例。
        private static readonly Real _LgE = new Real(Constant.LgE); // 表示自然常数 E 的常用对数的 Real 结构的实例。

        private static readonly Real _Ten = new Real(1, 1); // 表示数字 10 的 Real 结构的实例。

        //

        private double _Value; // 值。
        private long _Magnitude; // 数量级。

        //

        private void _Rectify() // 校正此 Real 结构的值与数量级。
        {
            if (InternalMethod.IsNaNOrInfinity(_Value) || _Value == 0)
            {
                _Magnitude = 0;
            }
            else
            {
                if (_Value <= -10 || _Value >= 10)
                {
                    long MagShift = (long)Math.Floor(Math.Log10(Math.Abs(_Value)));

                    if (_Magnitude > _MaxMagnitude - MagShift)
                    {
                        _Value = (_Value > 0 ? double.PositiveInfinity : double.NegativeInfinity);
                        _Magnitude = 0;

                        return;
                    }
                    else
                    {
                        if (MagShift > 0)
                        {
                            _Value /= _PositiveMagnitudeGeometricValues[MagShift];
                            _Magnitude += MagShift;
                        }

                        if (_Value > -1 && _Value < 1)
                        {
                            _Value *= 10;
                            _Magnitude--;
                        }
                    }
                }
                else if (_Value > -1 && _Value < 1)
                {
                    long MagShift = -(long)Math.Floor(Math.Log10(Math.Abs(_Value)));

                    if (_Magnitude < _MinMagnitude + MagShift)
                    {
                        _Value = (_Value > 0 ? double.PositiveInfinity : double.NegativeInfinity);
                        _Magnitude = 0;

                        return;
                    }
                    else
                    {
                        if (MagShift > 0)
                        {
                            _Value *= 10;

                            if (MagShift > 1)
                            {
                                _Value /= _NegativeMagnitudeGeometricValues[MagShift - 1];
                            }

                            _Magnitude -= MagShift;
                        }

                        if (_Value > -1 && _Value < 1)
                        {
                            _Value *= 10;
                            _Magnitude--;
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
        }

        //

        private enum _Parity // 奇偶性。
        {
            NonParity = -1, // 非奇非偶，表示既约分数的分母能够被 2 整除的有理数以及其他不满足下述偶或奇的定义的数。
            Even, // 偶，表示既约分数的分母不能被 2 整除且分子能够被 2 整除的有理数。
            Odd // 奇，表示既约分数的分母与分子均不能被 2 整除的有理数。
        }

        private _Parity _GetParity() // 获取此 Real 结构的奇偶性。
        {
            if (InternalMethod.IsNaNOrInfinity(_Value))
            {
                return _Parity.NonParity;
            }
            else if (_Value == 0)
            {
                return _Parity.Even;
            }
            else
            {
                Func<double, _Parity> GetParityOfRational = (val) =>
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
                        return _Parity.Even;
                    }
                    else if (RemN != 0 && RemD != 0)
                    {
                        return _Parity.Odd;
                    }
                    else
                    {
                        return _Parity.NonParity;
                    }
                };

                if (_Magnitude > 15)
                {
                    return _Parity.Even;
                }
                else if (_Magnitude < -15)
                {
                    return _Parity.NonParity;
                }
                else if (_Magnitude < 0)
                {
                    return GetParityOfRational(_Value * _NegativeMagnitudeGeometricValues[-_Magnitude]);
                }
                else
                {
                    double Val = _Value * _PositiveMagnitudeGeometricValues[_Magnitude];
                    double Trunc = Math.Truncate(Val);

                    if (Val == Trunc)
                    {
                        if (((long)Trunc) % 2 == 0)
                        {
                            return _Parity.Even;
                        }
                        else
                        {
                            return _Parity.Odd;
                        }
                    }
                    else
                    {
                        return GetParityOfRational(Val);
                    }
                }
            }
        }

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
        /// 使用十进制浮点数表示的值初始化 Real 结构的新实例。
        /// </summary>
        /// <param name="value">十进制浮点数表示的值。</param>
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

        #region 字段

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
        /// 获取表示此 Real 结构是否为 0 的布尔值。
        /// </summary>
        public bool IsZero
        {
            get
            {
                return (_Value == 0);
            }
        }

        /// <summary>
        /// 获取表示此 Real 结构是否为 1 的布尔值。
        /// </summary>
        public bool IsOne
        {
            get
            {
                return (_Value == 1 && _Magnitude == 0);
            }
        }

        /// <summary>
        /// 获取表示此 Real 结构是否为 -1 的布尔值。
        /// </summary>
        public bool IsMinusOne
        {
            get
            {
                return (_Value == -1 && _Magnitude == 0);
            }
        }

        //

        /// <summary>
        /// 获取表示此 Real 结构是否为正数的布尔值。
        /// </summary>
        public bool IsPositive
        {
            get
            {
                return (_Value > 0);
            }
        }

        /// <summary>
        /// 获取表示此 Real 结构是否为负数的布尔值。
        /// </summary>
        public bool IsNegative
        {
            get
            {
                return (_Value < 0);
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
                        double Val = _Value * _PositiveMagnitudeGeometricValues[_Magnitude];
                        double Trunc = Math.Truncate(Val);

                        return (Val == Trunc);
                    }
                }
            }
        }

        /// <summary>
        /// 获取表示此 Real 结构是否为小数的布尔值。
        /// </summary>
        public bool IsDecimal
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
                    return false;
                }
                else
                {
                    if (_Magnitude < 0)
                    {
                        return true;
                    }
                    else if (_Magnitude > 15)
                    {
                        return false;
                    }
                    else
                    {
                        double Val = _Value * _PositiveMagnitudeGeometricValues[_Magnitude];
                        double Trunc = Math.Truncate(Val);

                        return (Val != Trunc);
                    }
                }
            }
        }

        //

        /// <summary>
        /// 获取表示此 Real 结构是否为偶数的布尔值。
        /// </summary>
        public bool IsEven
        {
            get
            {
                return (_GetParity() == _Parity.Even);
            }
        }

        /// <summary>
        /// 获取表示此 Real 结构是否为奇数的布尔值。
        /// </summary>
        public bool IsOdd
        {
            get
            {
                return (_GetParity() == _Parity.Odd);
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

        //

        /// <summary>
        /// 获取此 Real 结构的相反数。
        /// </summary>
        public Real Opposite
        {
            get
            {
                if (double.IsNaN(_Value))
                {
                    return NaN;
                }
                else if (double.IsInfinity(_Value))
                {
                    if (double.IsPositiveInfinity(_Value))
                    {
                        return NegativeInfinity;
                    }
                    else
                    {
                        return PositiveInfinity;
                    }
                }
                else if (_Value == 0)
                {
                    return Zero;
                }
                else
                {
                    return new Real(-_Value, _Magnitude);
                }
            }
        }

        /// <summary>
        /// 获取此 Real 结构的倒数。
        /// </summary>
        public Real Reciprocal
        {
            get
            {
                if (double.IsNaN(_Value))
                {
                    return NaN;
                }
                else if (double.IsInfinity(_Value))
                {
                    return Zero;
                }
                else if (_Value == 0)
                {
                    return PositiveInfinity;
                }
                else
                {
                    if (this == One)
                    {
                        return One;
                    }
                    else if (this == MinusOne)
                    {
                        return MinusOne;
                    }
                    else
                    {
                        return new Real(1 / _Value, -_Magnitude);
                    }
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
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj is null || !(obj is Real))
            {
                return false;
            }
            else
            {
                return Equals((Real)obj);
            }
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
                    return (_Value * _NegativeMagnitudeGeometricValues[-_Magnitude]).ToString();
                }
                else if (_Magnitude > 0 && _Magnitude < 15)
                {
                    return (_Value * _PositiveMagnitudeGeometricValues[_Magnitude]).ToString();
                }
                else
                {
                    string Part1 = _Value.ToString();
                    string Part2 = "E";

                    long Mag = _Magnitude;

                    if (Part1 == "10")
                    {
                        Part1 = "1";
                        Mag++;
                    }
                    else if (Part1 == "-10")
                    {
                        Part1 = "-1";
                        Mag++;
                    }

                    string Part3 = string.Empty;
                    string Part4 = string.Empty;

                    if (Mag > 0)
                    {
                        Part3 = "+";
                        Part4 = Mag.ToString();
                    }
                    else
                    {
                        Part4 = (-Mag).ToString();

                        if (Mag > -10)
                        {
                            Part3 = "-0";
                        }
                        else
                        {
                            Part3 = "-";
                        }
                    }

                    return string.Concat(Part1, Part2, Part3, Part4);
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
            return (_Magnitude == real._Magnitude && _Value.Equals(real._Value));
        }

        //

        /// <summary>
        /// 将此 Real 结构与指定的对象进行次序比较。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>32 位整数，表示此 Real 与指定的对象进行次序比较的结果。</returns>
        public int CompareTo(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return 0;
            }
            else if (obj is null)
            {
                return 1;
            }
            else if (!(obj is Real))
            {
                throw new ArgumentException();
            }
            else
            {
                return CompareTo((Real)obj);
            }
        }

        /// <summary>
        /// 将此 Real 结构与指定的 Real 结构进行次序比较。
        /// </summary>
        /// <param name="real">用于比较的 Real 结构。</param>
        /// <returns>32 位整数，表示此 Real 与指定的 Real 对象进行次序比较的结果。</returns>
        public int CompareTo(Real real)
        {
            bool LIsNaN = double.IsNaN(_Value);
            bool RIsNaN = double.IsNaN(real._Value);

            if (LIsNaN || RIsNaN)
            {
                if (LIsNaN && RIsNaN)
                {
                    return 0;
                }
                else
                {
                    if (LIsNaN)
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
            return left.Equals(right);
        }

        //

        /// <summary>
        /// 比较两个 Real 结构的次序。
        /// </summary>
        /// <param name="left">用于比较的第一个 Real 结构。</param>
        /// <param name="right">用于比较的第二个 Real 结构。</param>
        /// <returns>32 位整数，表示对两个 Real 对象进行次序比较的结果。</returns>
        public static int Compare(Real left, Real right)
        {
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
            if (double.IsNaN(real._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(real._Value))
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
        /// 返回对 Real 结构计算以 10 为底的幂得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示指数。</param>
        /// <returns>Real 结构，表示对 Real 结构计算以 10 为底的幂得到的结果。</returns>
        public static Real Exp10(Real real)
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
                    return Zero;
                }
            }
            else if (real._Value == 0)
            {
                return One;
            }
            else
            {
                return (_Ten ^ real);
            }
        }

        /// <summary>
        /// 返回对 Real 结构计算自然幂得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示指数。</param>
        /// <returns>Real 结构，表示对 Real 结构计算自然幂得到的结果。</returns>
        public static Real Exp(Real real)
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
        /// 返回对 Real 结构计算幂得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="left">Real 结构，表示底数。</param>
        /// <param name="right">Real 结构，表示指数。</param>
        /// <returns>Real 结构，表示对 Real 结构计算幂得到的结果。</returns>
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
        /// 返回对 Real 结构计算对数得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="left">Real 结构，表示幂。</param>
        /// <param name="right">Real 结构，表示底数。</param>
        /// <returns>Real 结构，表示对 Real 结构计算对数得到的结果。</returns>
        public static Real Log(Real left, Real right)
        {
            return (Log10(left) / Log10(right));
        }

        //

        /// <summary>
        /// 返回对 Real 结构计算正弦得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示以弧度计量的角度。</param>
        /// <returns>Real 结构，表示对 Real 结构计算正弦得到的结果。</returns>
        public static Real Sin(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(real._Value))
            {
                return NaN;
            }
            else if (real._Value == 0)
            {
                return Zero;
            }
            else
            {
                return new Real(Math.Sin((double)(real % _DoublePi)));
            }
        }

        /// <summary>
        /// 返回对 Real 结构计算余弦得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示以弧度计量的角度。</param>
        /// <returns>Real 结构，表示对 Real 结构计算余弦得到的结果。</returns>
        public static Real Cos(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(real._Value))
            {
                return NaN;
            }
            else if (real._Value == 0)
            {
                return One;
            }
            else
            {
                return new Real(Math.Cos((double)(real % _DoublePi)));
            }
        }

        /// <summary>
        /// 返回对 Real 结构计算正切得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示以弧度计量的角度。</param>
        /// <returns>Real 结构，表示对 Real 结构计算正切得到的结果。</returns>
        public static Real Tan(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(real._Value))
            {
                return NaN;
            }
            else if (real._Value == 0)
            {
                return Zero;
            }
            else
            {
                return new Real(Math.Tan((double)(real % _Pi)));
            }
        }

        /// <summary>
        /// 返回对 Real 结构计算反正弦得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示正弦值。</param>
        /// <returns>Real 结构，表示对 Real 结构计算反正弦得到的结果。</returns>
        public static Real Asin(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(real._Value))
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
                    return new Real(Math.Asin((double)real));
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
        /// <param name="real">Real 结构，表示余弦值。</param>
        /// <returns>Real 结构，表示对 Real 结构计算反余弦得到的结果。</returns>
        public static Real Acos(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(real._Value))
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
                    return new Real(Math.Acos((double)real));
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
        /// <param name="real">Real 结构，表示正切值。</param>
        /// <returns>Real 结构，表示对 Real 结构计算反正切得到的结果。</returns>
        public static Real Atan(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(real._Value))
            {
                if (double.IsPositiveInfinity(real._Value))
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
            if (double.IsNaN(real._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(real._Value))
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
            if (double.IsNaN(real._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(real._Value))
            {
                if (double.IsPositiveInfinity(real._Value))
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
        /// <param name="real">Real 结构，表示双曲正弦值。</param>
        /// <returns>Real 结构，表示对 Real 结构计算反双曲正弦得到的结果。</returns>
        public static Real Asinh(Real real)
        {
            return Log(real + Sqrt(Sqr(real) + One));
        }

        /// <summary>
        /// 返回对 Real 结构计算反双曲余弦得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示双曲余弦值。</param>
        /// <returns>Real 结构，表示对 Real 结构计算反双曲余弦得到的结果。</returns>
        public static Real Acosh(Real real)
        {
            return Log(real + Sqrt(Sqr(real) + MinusOne));
        }

        /// <summary>
        /// 返回对 Real 结构计算反双曲正切得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，表示双曲正切值。</param>
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
                return PositiveInfinity;
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
                    double Val = real._Value * _PositiveMagnitudeGeometricValues[real._Magnitude];

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
                    double Val = real._Value * _PositiveMagnitudeGeometricValues[real._Magnitude];

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
                    double Val = real._Value * _PositiveMagnitudeGeometricValues[real._Magnitude];

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
                    double Val = real._Value * _PositiveMagnitudeGeometricValues[real._Magnitude];

                    return new Real(Math.Truncate(Val));
                }
            }
        }

        /// <summary>
        /// 返回将两个 Real 结构取最大值得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="left">Real 结构，用于比较的第一个结构。</param>
        /// <param name="right">Real 结构，用于比较的第二个结构。</param>
        /// <returns>Real 结构，表示将两个 Real 结构取最大值得到的结果。</returns>
        public static Real Max(Real left, Real right)
        {
            if (double.IsNaN(left._Value) || double.IsNaN(right._Value))
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
        /// 返回将两个 Real 结构取最小值得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="left">Real 结构，用于比较的第一个结构。</param>
        /// <param name="right">Real 结构，用于比较的第二个结构。</param>
        /// <returns>Real 结构，表示将两个 Real 结构取最小值得到的结果。</returns>
        public static Real Min(Real left, Real right)
        {
            if (double.IsNaN(left._Value) || double.IsNaN(right._Value))
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
            if (double.IsNaN(left._Value) || double.IsNaN(right._Value))
            {
                return false;
            }
            else if (double.IsInfinity(left._Value) || double.IsInfinity(right._Value))
            {
                bool LIsPosInf = double.IsPositiveInfinity(left._Value);
                bool LIsNegInf = double.IsNegativeInfinity(left._Value);
                bool LIsInf = (LIsPosInf || LIsNegInf);
                bool RIsPosInf = double.IsPositiveInfinity(right._Value);
                bool RIsNegInf = double.IsNegativeInfinity(right._Value);
                bool RIsInf = (RIsPosInf || RIsNegInf);

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
            if (double.IsNaN(left._Value) || double.IsNaN(right._Value))
            {
                return false;
            }
            else if (double.IsInfinity(left._Value) || double.IsInfinity(right._Value))
            {
                bool LIsPosInf = double.IsPositiveInfinity(left._Value);
                bool LIsNegInf = double.IsNegativeInfinity(left._Value);
                bool LIsInf = (LIsPosInf || LIsNegInf);
                bool RIsPosInf = double.IsPositiveInfinity(right._Value);
                bool RIsNegInf = double.IsNegativeInfinity(right._Value);
                bool RIsInf = (RIsPosInf || RIsNegInf);

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
            if (double.IsNaN(left._Value) || double.IsNaN(right._Value))
            {
                return false;
            }
            else if (double.IsInfinity(left._Value) || double.IsInfinity(right._Value))
            {
                bool LIsPosInf = double.IsPositiveInfinity(left._Value);
                bool LIsNegInf = double.IsNegativeInfinity(left._Value);
                bool LIsInf = (LIsPosInf || LIsNegInf);
                bool RIsPosInf = double.IsPositiveInfinity(right._Value);
                bool RIsNegInf = double.IsNegativeInfinity(right._Value);
                bool RIsInf = (RIsPosInf || RIsNegInf);

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
            if (double.IsNaN(left._Value) || double.IsNaN(right._Value))
            {
                return false;
            }
            else if (double.IsInfinity(left._Value) || double.IsInfinity(right._Value))
            {
                bool LIsPosInf = double.IsPositiveInfinity(left._Value);
                bool LIsNegInf = double.IsNegativeInfinity(left._Value);
                bool LIsInf = (LIsPosInf || LIsNegInf);
                bool RIsPosInf = double.IsPositiveInfinity(right._Value);
                bool RIsNegInf = double.IsNegativeInfinity(right._Value);
                bool RIsInf = (RIsPosInf || RIsNegInf);

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
            return real;
        }

        /// <summary>
        /// 返回在 Real 结构前添加负号得到的 Real 结构的新实例。
        /// </summary>
        /// <param name="real">Real 结构，用于转换的结构。</param>
        /// <returns>Real 结构，表示在 Real 结构前添加负号得到的结果。</returns>
        public static Real operator -(Real real)
        {
            if (double.IsNaN(real._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(real._Value))
            {
                if (double.IsPositiveInfinity(real._Value))
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
            if (double.IsNaN(real._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(real._Value))
            {
                if (double.IsPositiveInfinity(real._Value))
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
            if (double.IsNaN(left._Value) || double.IsNaN(right._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(left._Value) || double.IsInfinity(right._Value))
            {
                bool LIsPosInf = double.IsPositiveInfinity(left._Value);
                bool LIsNegInf = double.IsNegativeInfinity(left._Value);
                bool LIsInf = (LIsPosInf || LIsNegInf);
                bool RIsPosInf = double.IsPositiveInfinity(right._Value);
                bool RIsNegInf = double.IsNegativeInfinity(right._Value);
                bool RIsInf = (RIsPosInf || RIsNegInf);

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
                long DeltaMag = left._Magnitude - right._Magnitude;

                if (DeltaMag > 0)
                {
                    if (DeltaMag <= 16)
                    {
                        return new Real(left._Value + right._Value / _PositiveMagnitudeGeometricValues[DeltaMag], left._Magnitude);
                    }
                    else
                    {
                        return left;
                    }
                }
                else if (DeltaMag < 0)
                {
                    if (DeltaMag >= -16)
                    {
                        return new Real(left._Value / _PositiveMagnitudeGeometricValues[-DeltaMag] + right._Value, right._Magnitude);
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
            if (double.IsNaN(left._Value) || double.IsNaN(right._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(left._Value) || double.IsInfinity(right._Value))
            {
                bool LIsPosInf = double.IsPositiveInfinity(left._Value);
                bool LIsNegInf = double.IsNegativeInfinity(left._Value);
                bool LIsInf = (LIsPosInf || LIsNegInf);
                bool RIsPosInf = double.IsPositiveInfinity(right._Value);
                bool RIsNegInf = double.IsNegativeInfinity(right._Value);
                bool RIsInf = (RIsPosInf || RIsNegInf);

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
                long DeltaMag = left._Magnitude - right._Magnitude;

                if (DeltaMag > 0)
                {
                    if (DeltaMag <= 16)
                    {
                        return new Real(left._Value - right._Value / _PositiveMagnitudeGeometricValues[DeltaMag], left._Magnitude);
                    }
                    else
                    {
                        return left;
                    }
                }
                else if (DeltaMag < 0)
                {
                    if (DeltaMag >= -16)
                    {
                        return new Real(left._Value / _PositiveMagnitudeGeometricValues[-DeltaMag] - right._Value, right._Magnitude);
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
            if (double.IsNaN(left._Value) || double.IsNaN(right._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(left._Value) || double.IsInfinity(right._Value))
            {
                bool LIsPosInf = double.IsPositiveInfinity(left._Value);
                bool LIsNegInf = double.IsNegativeInfinity(left._Value);
                bool LIsInf = (LIsPosInf || LIsNegInf);
                bool RIsPosInf = double.IsPositiveInfinity(right._Value);
                bool RIsNegInf = double.IsNegativeInfinity(right._Value);
                bool RIsInf = (RIsPosInf || RIsNegInf);

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
            if (double.IsNaN(left._Value) || double.IsNaN(right._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(left._Value) || double.IsInfinity(right._Value))
            {
                bool LIsPosInf = double.IsPositiveInfinity(left._Value);
                bool LIsNegInf = double.IsNegativeInfinity(left._Value);
                bool LIsInf = (LIsPosInf || LIsNegInf);
                bool RIsPosInf = double.IsPositiveInfinity(right._Value);
                bool RIsNegInf = double.IsNegativeInfinity(right._Value);
                bool RIsInf = (RIsPosInf || RIsNegInf);

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
            if (double.IsNaN(left._Value) || double.IsNaN(right._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(left._Value) || double.IsInfinity(right._Value))
            {
                bool LIsInf = double.IsInfinity(left._Value);
                bool RIsInf = double.IsInfinity(right._Value);

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

                Real result = Abs(left);

                while (result._Value > 0 && (result._Magnitude > right._Magnitude || (result._Magnitude == right._Magnitude && result._Value > ValAbsR)))
                {
                    if (result._Value > ValAbsR)
                    {
                        result._Value %= ValAbsR;
                    }
                    else
                    {
                        result._Value = (result._Value * 10) % ValAbsR;
                        result._Magnitude--;
                    }

                    if (result._Value < 1)
                    {
                        for (int i = 16; i > 0; i--)
                        {
                            if (result._Value < _NegativeMagnitudeGeometricValues[i - 1])
                            {
                                result._Value *= _PositiveMagnitudeGeometricValues[i];
                                result._Magnitude -= i;

                                break;
                            }
                        }
                    }
                }

                result._Value *= ValSignL;
                result._Rectify();

                return result;
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
            if (double.IsNaN(left._Value) || double.IsNaN(right._Value))
            {
                return NaN;
            }
            else if (double.IsInfinity(left._Value) || double.IsInfinity(right._Value))
            {
                bool LIsPosInf = double.IsPositiveInfinity(left._Value);
                bool LIsNegInf = double.IsNegativeInfinity(left._Value);
                bool LIsInf = (LIsPosInf || LIsNegInf);
                bool RIsPosInf = double.IsPositiveInfinity(right._Value);
                bool RIsNegInf = double.IsNegativeInfinity(right._Value);
                bool RIsInf = (RIsPosInf || RIsNegInf);

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
                                case _Parity.Even: return PositiveInfinity;
                                case _Parity.Odd: return NegativeInfinity;
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
                            case _Parity.Even: return One;
                            case _Parity.Odd: return MinusOne;
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
                    Func<Real, Real, Real> FastPower = (baseValue, expValue) =>
                    {
                        double Lg0;

                        if (baseValue._Value == 1)
                        {
                            Lg0 = baseValue._Magnitude * expValue._Value;
                        }
                        else
                        {
                            Lg0 = Math.Log10(Math.Pow(baseValue._Value, expValue._Value)) + baseValue._Magnitude * expValue._Value;
                        }

                        long M0 = (long)Math.Truncate(Lg0);
                        double V0 = Math.Pow(10, Lg0 - M0);

                        Real result = new Real(V0, M0);

                        if (!double.IsNaN(result._Value) && !double.IsInfinity(result._Value) && result._Value != 0)
                        {
                            if (expValue._Magnitude != 0)
                            {
                                double Lgi;

                                if (expValue._Magnitude > 0)
                                {
                                    for (long i = 0; i < expValue._Magnitude; i++)
                                    {
                                        Lgi = Math.Log10(Math.Pow(result._Value, 10)) + 10 * result._Magnitude;

                                        result._Magnitude = (long)Math.Truncate(Lgi);
                                        result._Value = Math.Pow(10, Lgi - result._Magnitude);
                                        result._Rectify();

                                        if (double.IsNaN(result._Value) || double.IsInfinity(result._Value) || result._Value == 0)
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    for (long i = 0; i < -expValue._Magnitude; i++)
                                    {
                                        Lgi = Math.Log10(Math.Pow(result._Value, 0.1)) + 0.1 * result._Magnitude;

                                        result._Magnitude = (long)Math.Truncate(Lgi);
                                        result._Value = Math.Pow(10, Lgi - result._Magnitude);
                                        result._Rectify();

                                        if (double.IsNaN(result._Value) || double.IsInfinity(result._Value) || result._Value == 0)
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
                                case _Parity.Even: return (One / FastPower(-left, -right));
                                case _Parity.Odd: return (MinusOne / FastPower(-left, -right));
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
                                case _Parity.Even: return FastPower(-left, right);
                                case _Parity.Odd: return -FastPower(-left, right);
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
                        return ((real._Magnitude > 0 ? real._Value * _PositiveMagnitudeGeometricValues[real._Magnitude] : (real._Magnitude == -1 ? real._Value * 0.1 : (real._Value * 0.1) * _NegativeMagnitudeGeometricValues[-real._Magnitude - 1])));
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
                    double Val = real._Value * (real._Magnitude > 0 ? _PositiveMagnitudeGeometricValues[real._Magnitude] : _NegativeMagnitudeGeometricValues[-real._Magnitude]);

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
        /// 将指定的 Real 结构显式转换为十进制浮点数。
        /// </summary>
        /// <param name="real">用于转换的 Real 结构。</param>
        /// <returns>十进制浮点数，表示显式转换的结果。</returns>
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
                    double Val = real._Value * (real._Magnitude > 0 ? _PositiveMagnitudeGeometricValues[real._Magnitude] : _NegativeMagnitudeGeometricValues[-real._Magnitude]);

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
                    double Val = real._Value * _PositiveMagnitudeGeometricValues[real._Magnitude];

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
                    double Val = real._Value * _PositiveMagnitudeGeometricValues[real._Magnitude];

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
                    double Val = real._Value * _PositiveMagnitudeGeometricValues[real._Magnitude];

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
                    double Val = real._Value * _PositiveMagnitudeGeometricValues[real._Magnitude];

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
                    double Val = real._Value * _PositiveMagnitudeGeometricValues[real._Magnitude];

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
                    double Val = real._Value * _PositiveMagnitudeGeometricValues[real._Magnitude];

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
                    double Val = real._Value * _PositiveMagnitudeGeometricValues[real._Magnitude];

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
                    double Val = real._Value * _PositiveMagnitudeGeometricValues[real._Magnitude];

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
        /// 将指定的十进制浮点数显式转换为 Real 结构。
        /// </summary>
        /// <param name="value">用于转换的十进制浮点数。</param>
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