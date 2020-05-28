/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

Com.Complex
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
    /// 以一组有序的双精度浮点数表示的直角坐标形式的二元复数。
    /// </summary>
    public struct Complex : IEquatable<Complex>, IComparable, IComparable<Complex>
    {
        #region 私有成员与内部成员

        private static Complex _MultiplyByImaginaryOne(Complex comp) // 返回将虚数单位 i 与 Complex 结构的相乘得到的 Complex 结构的新实例。
        {
            return new Complex(-comp._Imaginary, comp.Real);
        }

        //

        private double _Real; // 实部。
        private double _Imaginary; // 虚部。

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用双精度浮点数表示的实部与虚部初始化 Complex 结构的新实例。
        /// </summary>
        /// <param name="real">双精度浮点数表示的实部。</param>
        /// <param name="imaginary">双精度浮点数表示的虚部。</param>
        public Complex(double real, double imaginary)
        {
            _Real = real;
            _Imaginary = imaginary;
        }

        /// <summary>
        /// 使用双精度浮点数表示的实部初始化 Complex 结构的新实例。
        /// </summary>
        /// <param name="real">双精度浮点数表示的实部。</param>
        public Complex(double real)
        {
            _Real = real;
            _Imaginary = 0;
        }

        /// <summary>
        /// 使用 PointD 结构初始化 Complex 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构。</param>
        public Complex(PointD pt)
        {
            _Real = pt.X;
            _Imaginary = pt.Y;
        }

        #endregion

        #region 字段

        /// <summary>
        /// 表示非数字的 Complex 结构的实例。
        /// </summary>
        public static readonly Complex NaN = new Complex(double.NaN, double.NaN);

        //

        /// <summary>
        /// 表示 0 的 Complex 结构的实例。
        /// </summary>
        public static readonly Complex Zero = new Complex(0, 0);

        /// <summary>
        /// 表示实数单位 1 的 Complex 结构的实例。
        /// </summary>
        public static readonly Complex One = new Complex(1, 0);

        /// <summary>
        /// 表示虚数单位 i 的 Complex 结构的实例。
        /// </summary>
        public static readonly Complex ImaginaryOne = new Complex(0, 1);

        #endregion

        #region 属性

        /// <summary>
        /// 获取表示此 Complex 结构是否为非数字的布尔值。
        /// </summary>
        public bool IsNaN
        {
            get
            {
                return (double.IsNaN(_Real) || double.IsNaN(_Imaginary));
            }
        }

        /// <summary>
        /// 获取表示此 Complex 结构是否为无穷大的布尔值。
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return (!IsNaN && (double.IsInfinity(_Real) || double.IsInfinity(_Imaginary)));
            }
        }

        /// <summary>
        /// 获取表示此 Complex 结构是否为非数字或无穷大的布尔值。
        /// </summary>
        public bool IsNaNOrInfinity
        {
            get
            {
                return (InternalMethod.IsNaNOrInfinity(_Real) || InternalMethod.IsNaNOrInfinity(_Imaginary));
            }
        }

        //

        /// <summary>
        /// 获取表示此 Complex 结构是否为 0 的布尔值。
        /// </summary>
        public bool IsZero
        {
            get
            {
                return (_Real == 0 && _Imaginary == 0);
            }
        }

        /// <summary>
        /// 获取表示此 Complex 结构是否为实数单位 1 的布尔值。
        /// </summary>
        public bool IsOne
        {
            get
            {
                return (_Real == 1 && _Imaginary == 0);
            }
        }

        /// <summary>
        /// 获取表示此 Complex 结构是否为虚数单位 i 的布尔值。
        /// </summary>
        public bool IsImaginaryOne
        {
            get
            {
                return (_Real == 0 && _Imaginary == 1);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 Complex 结构的实部。
        /// </summary>
        public double Real
        {
            get
            {
                return _Real;
            }

            set
            {
                _Real = value;
            }
        }

        /// <summary>
        /// 获取或设置此 Complex 结构的虚部。
        /// </summary>
        public double Imaginary
        {
            get
            {
                return _Imaginary;
            }

            set
            {
                _Imaginary = value;
            }
        }

        //

        /// <summary>
        /// 获取此 Complex 结构的模。
        /// </summary>
        public double Module
        {
            get
            {
                double AbsRe = Math.Abs(_Real);
                double AbsIm = Math.Abs(_Imaginary);

                double AbsMax = Math.Max(AbsRe, AbsIm);

                if (AbsMax == 0)
                {
                    return 0;
                }
                else
                {
                    AbsRe /= AbsMax;
                    AbsIm /= AbsMax;

                    double SqrSum = AbsRe * AbsRe + AbsIm * AbsIm;

                    return (AbsMax * Math.Sqrt(SqrSum));
                }
            }
        }

        /// <summary>
        /// 获取此 Complex 结构的模的平方。
        /// </summary>
        public double ModuleSquared
        {
            get
            {
                return (_Real * _Real + _Imaginary * _Imaginary);
            }
        }

        /// <summary>
        /// 获取此 Complex 结构的辐角（弧度）（以实轴正方向为 0 弧度，从实轴正方向指向虚轴正方向的方向为正方向）。
        /// </summary>
        public double Argument
        {
            get
            {
                return Math.Atan2(_Imaginary, _Real);
            }
        }

        //

        /// <summary>
        /// 获取此 Complex 结构的相反数。
        /// </summary>
        public Complex Opposite
        {
            get
            {
                return new Complex(-_Real, -_Imaginary);
            }
        }

        /// <summary>
        /// 获取此 Complex 结构的倒数。
        /// </summary>
        public Complex Reciprocal
        {
            get
            {
                if (IsZero)
                {
                    return NaN;
                }
                else
                {
                    double ModSqr = ModuleSquared;

                    return new Complex(_Real / ModSqr, -_Imaginary / ModSqr);
                }
            }
        }

        /// <summary>
        /// 获取此 Complex 结构的共轭复数。
        /// </summary>
        public Complex Conjugate
        {
            get
            {
                return new Complex(_Real, -_Imaginary);
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 Complex 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>布尔值，表示此 Complex 结构是否与指定的对象相等。</returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj is null || !(obj is Complex))
            {
                return false;
            }
            else
            {
                return Equals((Complex)obj);
            }
        }

        /// <summary>
        /// 返回此 Complex 结构的哈希代码。
        /// </summary>
        /// <returns>32 位整数，表示此 Complex 结构的哈希代码。</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 Complex 结构转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 Complex 结构的字符串形式。</returns>
        public override string ToString()
        {
            if (IsNaN)
            {
                return "NaN";
            }
            else if (IsInfinity)
            {
                return "Infinity";
            }
            else
            {
                if (_Imaginary == 0)
                {
                    return _Real.ToString();
                }
                else
                {
                    if (_Real == 0)
                    {
                        if (_Imaginary == 1)
                        {
                            return "i";
                        }
                        else if (_Imaginary == -1)
                        {
                            return "-i";
                        }
                        else
                        {
                            return (_Imaginary + " i");
                        }
                    }
                    else
                    {
                        if (_Imaginary == 1)
                        {
                            return (_Real + " + i");
                        }
                        else if (_Imaginary == -1)
                        {
                            return (_Real + " - i");
                        }
                        else if (_Imaginary > 0)
                        {
                            return string.Concat(_Real, " + ", _Imaginary, " i");
                        }
                        else
                        {
                            return string.Concat(_Real, " - ", (-_Imaginary), " i");
                        }
                    }
                }
            }
        }

        //

        /// <summary>
        /// 判断此 Complex 结构是否与指定的 Complex 结构相等。
        /// </summary>
        /// <param name="comp">用于比较的 Complex 结构。</param>
        /// <returns>布尔值，表示此 Complex 结构是否与指定的 Complex 结构相等。</returns>
        public bool Equals(Complex comp)
        {
            return (_Real.Equals(comp._Real) && _Imaginary.Equals(comp._Imaginary));
        }

        //

        /// <summary>
        /// 将此 Complex 结构与指定的对象进行次序比较。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>32 位整数，表示将此 Complex 结构与指定的对象进行次序比较得到的结果。</returns>
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
            else if (!(obj is Complex))
            {
                throw new ArgumentException();
            }
            else
            {
                return CompareTo((Complex)obj);
            }
        }

        /// <summary>
        /// 将此 Complex 结构与指定的 Complex 结构进行次序比较。
        /// </summary>
        /// <param name="comp">用于比较的 Complex 结构。</param>
        /// <returns>32 位整数，表示将此 Complex 结构与指定的 Complex 结构进行次序比较得到的结果。</returns>
        public int CompareTo(Complex comp)
        {
            return Module.CompareTo(comp.Module);
        }

        //

        /// <summary>
        /// 返回将此 Complex 结构转换为 PointD 结构的新实例。
        /// </summary>
        /// <returns>PointD 结构，表示将此 Complex 结构转换为 PointD 结构得到的结果。</returns>
        public PointD ToPointD()
        {
            return new PointD(_Real, _Imaginary);
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断两个 Complex 结构是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 Complex 结构。</param>
        /// <param name="right">用于比较的第二个 Complex 结构。</param>
        /// <returns>布尔值，表示两个 Complex 结构是否相等。</returns>
        public static bool Equals(Complex left, Complex right)
        {
            return left.Equals(right);
        }

        //

        /// <summary>
        /// 比较两个 Complex 结构的次序。
        /// </summary>
        /// <param name="left">用于比较的第一个 Complex 结构。</param>
        /// <param name="right">用于比较的第二个 Complex 结构。</param>
        /// <returns>32 位整数，表示将两个 Complex 结构进行次序比较得到的结果。</returns>
        public static int Compare(Complex left, Complex right)
        {
            return left.CompareTo(right);
        }

        //

        /// <summary>
        /// 返回将 PointD 结构转换为 Complex 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构。</param>
        /// <returns>Complex 结构，表示将 PointD 结构转换为 Complex 结构得到的结果。</returns>
        public static Complex FromPointD(PointD pt)
        {
            return new Complex(pt);
        }

        //

        /// <summary>
        /// 返回将 Complex 结构的极坐标分量模与辐角（弧度）转换为 Complex 结构的新实例。
        /// </summary>
        /// <param name="module">表示 Complex 结构的模。</param>
        /// <param name="argument">表示 Complex 结构的辐角（弧度）。</param>
        /// <returns>Complex 结构，表示将 Complex 结构的极坐标分量模与辐角（弧度）转换为 Complex 结构得到的结果。</returns>
        public static Complex FromPolarCoordinates(double module, double argument)
        {
            return new Complex(module * Math.Cos(argument), module * Math.Sin(argument));
        }

        //

        /// <summary>
        /// 返回对 Complex 结构计算平方得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示底数。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算平方得到的结果。</returns>
        public static Complex Sqr(Complex comp)
        {
            return new Complex(comp._Real * comp._Real - comp._Imaginary * comp._Imaginary, 2 * comp._Real * comp._Imaginary);
        }

        /// <summary>
        /// 返回对 Complex 结构计算平方根得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示被开方数。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算平方根得到的结果。</returns>
        public static Complex Sqrt(Complex comp)
        {
            double Mod = Math.Sqrt(comp.Module);
            double Arg = comp.Argument / 2;

            return FromPolarCoordinates(Mod, Arg);
        }

        /// <summary>
        /// 返回对 Complex 结构计算自然幂得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示底数。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算自然幂得到的结果。</returns>
        public static Complex Exp(Complex comp)
        {
            double Mod = Math.Exp(comp._Real);
            double Arg = comp._Imaginary;

            return FromPolarCoordinates(Mod, Arg);
        }

        /// <summary>
        /// 返回对 Complex 结构计算复幂得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示底数。</param>
        /// <param name="right">Complex 结构，表示指数。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算复幂得到的结果。</returns>
        public static Complex Pow(Complex left, Complex right)
        {
            double ModL = left.Module;

            if (ModL == 0)
            {
                if (right.IsZero)
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
                double LnModL = Math.Log(ModL);
                double ArgL = left.Argument;

                double Mod = Math.Exp(LnModL * right._Real - ArgL * right._Imaginary);
                double Arg = ArgL * right._Real + LnModL * right._Imaginary;

                return FromPolarCoordinates(Mod, Arg);
            }
        }

        /// <summary>
        /// 返回对 Complex 结构计算实幂得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示底数。</param>
        /// <param name="right">双精度浮点数，表示指数。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算实幂得到的结果。</returns>
        public static Complex Pow(Complex left, double right)
        {
            double ModL = left.Module;

            if (ModL == 0)
            {
                if (right == 0)
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
                double LnModL = Math.Log(ModL);
                double ArgL = left.Argument;

                double Mod = Math.Exp(LnModL * right);
                double Arg = ArgL * right;

                return FromPolarCoordinates(Mod, Arg);
            }
        }

        /// <summary>
        /// 返回对双精度浮点数计算复幂得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">双精度浮点数，表示底数。</param>
        /// <param name="right">Complex 结构，表示指数。</param>
        /// <returns>Complex 结构，表示对双精度浮点数计算复幂得到的结果。</returns>
        public static Complex Pow(double left, Complex right)
        {
            double ModL = left;

            if (ModL == 0)
            {
                if (right.IsZero)
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
                double LnModL = Math.Log(ModL);
                double ArgL = 0;

                double Mod = Math.Exp(LnModL * right._Real - ArgL * right._Imaginary);
                double Arg = ArgL * right._Real + LnModL * right._Imaginary;

                return FromPolarCoordinates(Mod, Arg);
            }
        }

        /// <summary>
        /// 返回对 Complex 结构计算自然对数得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示幂。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算自然对数得到的结果。</returns>
        public static Complex Log(Complex comp)
        {
            return new Complex(Math.Log(comp.Module), comp.Argument);
        }

        /// <summary>
        /// 返回对 Complex 结构计算复对数得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示幂。</param>
        /// <param name="right">Complex 结构，表示底数。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算复对数得到的结果。</returns>
        public static Complex Log(Complex left, Complex right)
        {
            if (left.IsOne && right.IsZero)
            {
                return Zero;
            }
            else
            {
                return (Log(left) / Log(right));
            }
        }

        /// <summary>
        /// 返回对双精度浮点数计算复对数得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示幂。</param>
        /// <param name="right">双精度浮点数，表示底数。</param>
        /// <returns>Complex 结构，表示对双精度浮点数计算复对数得到的结果。</returns>
        public static Complex Log(Complex left, double right)
        {
            if (left.IsOne && right == 0)
            {
                return Zero;
            }
            else
            {
                return (Log(left) / Math.Log(right));
            }
        }

        /// <summary>
        /// 返回对 Complex 结构计算实对数得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">双精度浮点数，表示幂。</param>
        /// <param name="right">Complex 结构，表示底数。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算实对数得到的结果。</returns>
        public static Complex Log(double left, Complex right)
        {
            if (left == 1 && right.IsZero)
            {
                return Zero;
            }
            else
            {
                return (Math.Log(left) / Log(right));
            }
        }

        //

        /// <summary>
        /// 返回对 Complex 结构计算正弦得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示以弧度计量的角度。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算正弦得到的结果。</returns>
        public static Complex Sin(Complex comp)
        {
            double ExpPIm = Math.Exp(comp._Imaginary);
            double ExpNIm = Math.Exp(-comp._Imaginary);

            return new Complex((ExpPIm + ExpNIm) * Math.Sin(comp._Real) / 2, (ExpPIm - ExpNIm) * Math.Cos(comp._Real) / 2);
        }

        /// <summary>
        /// 返回对 Complex 结构计算余弦得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示以弧度计量的角度。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算余弦得到的结果。</returns>
        public static Complex Cos(Complex comp)
        {
            double ExpPIm = Math.Exp(comp._Imaginary);
            double ExpNIm = Math.Exp(-comp._Imaginary);

            return new Complex((ExpPIm + ExpNIm) * Math.Cos(comp._Real) / 2, -(ExpPIm - ExpNIm) * Math.Sin(comp._Real) / 2);
        }

        /// <summary>
        /// 返回对 Complex 结构计算正切得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示以弧度计量的角度。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算正切得到的结果。</returns>
        public static Complex Tan(Complex comp)
        {
            return (Sin(comp) / Cos(comp));
        }

        /// <summary>
        /// 返回对 Complex 结构计算反正弦得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示正弦值。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算反正弦得到的结果。</returns>
        public static Complex Asin(Complex comp)
        {
            return (-_MultiplyByImaginaryOne(Log(_MultiplyByImaginaryOne(comp) + Sqrt(1 - Sqr(comp)))));
        }

        /// <summary>
        /// 返回对 Complex 结构计算反余弦得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示余弦值。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算反余弦得到的结果。</returns>
        public static Complex Acos(Complex comp)
        {
            return (-_MultiplyByImaginaryOne(Log(comp + Sqrt(Sqr(comp) - 1))));
        }

        /// <summary>
        /// 返回对 Complex 结构计算反正切得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示正切值。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算反正切得到的结果。</returns>
        public static Complex Atan(Complex comp)
        {
            return (_MultiplyByImaginaryOne(Log((1 - _MultiplyByImaginaryOne(comp)) / (1 + _MultiplyByImaginaryOne(comp)))) / 2);
        }

        /// <summary>
        /// 返回对 Complex 结构计算双曲正弦得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示以弧度计量的角度。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算双曲正弦得到的结果。</returns>
        public static Complex Sinh(Complex comp)
        {
            return (-_MultiplyByImaginaryOne(Sin(_MultiplyByImaginaryOne(comp))));
        }

        /// <summary>
        /// 返回对 Complex 结构计算双曲余弦得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示以弧度计量的角度。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算双曲余弦得到的结果。</returns>
        public static Complex Cosh(Complex comp)
        {
            return Cos(_MultiplyByImaginaryOne(comp));
        }

        /// <summary>
        /// 返回对 Complex 结构计算双曲正切得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示以弧度计量的角度。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算双曲正切得到的结果。</returns>
        public static Complex Tanh(Complex comp)
        {
            return (-_MultiplyByImaginaryOne(Tan(_MultiplyByImaginaryOne(comp))));
        }

        /// <summary>
        /// 返回对 Complex 结构计算反双曲正弦得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示双曲正弦值。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算反双曲正弦得到的结果。</returns>
        public static Complex Asinh(Complex comp)
        {
            return Log(comp + Sqrt(Sqr(comp) + 1));
        }

        /// <summary>
        /// 返回对 Complex 结构计算反双曲余弦得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示双曲余弦值。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算反双曲余弦得到的结果。</returns>
        public static Complex Acosh(Complex comp)
        {
            return Log(comp + Sqrt(Sqr(comp) - 1));
        }

        /// <summary>
        /// 返回对 Complex 结构计算反双曲正切得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示双曲正切值。</param>
        /// <returns>Complex 结构，表示对 Complex 结构计算反双曲正切得到的结果。</returns>
        public static Complex Atanh(Complex comp)
        {
            return (Log((1 + comp) / (1 - comp)) / 2);
        }

        //

        /// <summary>
        /// 返回将 Complex 结构的所有分量取符号数得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，用于转换的结构。</param>
        /// <returns>Complex 结构，表示将 Complex 结构的所有分量取符号数得到的结果。</returns>
        public static Complex Sign(Complex comp)
        {
            return new Complex((double.IsNaN(comp._Real) ? 0 : Math.Sign(comp._Real)), (double.IsNaN(comp._Imaginary) ? 0 : Math.Sign(comp._Imaginary)));
        }

        /// <summary>
        /// 返回将 Complex 结构的所有分量取绝对值得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，用于转换的结构。</param>
        /// <returns>Complex 结构，表示将 Complex 结构的所有分量取绝对值得到的结果。</returns>
        public static Complex Abs(Complex comp)
        {
            return new Complex(Math.Abs(comp._Real), Math.Abs(comp._Imaginary));
        }

        /// <summary>
        /// 返回将 Complex 结构的所有分量舍入到较大的整数值得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，用于转换的结构。</param>
        /// <returns>Complex 结构，表示将 Complex 结构的所有分量舍入到较大的整数值得到的结果。</returns>
        public static Complex Ceiling(Complex comp)
        {
            return new Complex(Math.Ceiling(comp._Real), Math.Ceiling(comp._Imaginary));
        }

        /// <summary>
        /// 返回将 Complex 结构的所有分量舍入到较小的整数值得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，用于转换的结构。</param>
        /// <returns>Complex 结构，表示将 Complex 结构的所有分量舍入到较小的整数值得到的结果。</returns>
        public static Complex Floor(Complex comp)
        {
            return new Complex(Math.Floor(comp._Real), Math.Floor(comp._Imaginary));
        }

        /// <summary>
        /// 返回将 Complex 结构的所有分量舍入到最接近的整数值得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，用于转换的结构。</param>
        /// <returns>Complex 结构，表示将 Complex 结构的所有分量舍入到最接近的整数值得到的结果。</returns>
        public static Complex Round(Complex comp)
        {
            return new Complex(Math.Round(comp._Real), Math.Round(comp._Imaginary));
        }

        /// <summary>
        /// 返回将 Complex 结构的所有分量截断小数部分取整得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，用于转换的结构。</param>
        /// <returns>Complex 结构，表示将 Complex 结构的所有分量截断小数部分取整得到的结果。</returns>
        public static Complex Truncate(Complex comp)
        {
            return new Complex(Math.Truncate(comp._Real), Math.Truncate(comp._Imaginary));
        }

        /// <summary>
        /// 返回将两个 Complex 结构的所有分量分别取最大值得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，用于比较的第一个结构。</param>
        /// <param name="right">Complex 结构，用于比较的第二个结构。</param>
        /// <returns>Complex 结构，表示将两个 Complex 结构的所有分量分别取最大值得到的结果。</returns>
        public static Complex Max(Complex left, Complex right)
        {
            return new Complex(Math.Max(left._Real, right._Real), Math.Max(left._Imaginary, right._Imaginary));
        }

        /// <summary>
        /// 返回将两个 Complex 结构的所有分量分别取最小值得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，用于比较的第一个结构。</param>
        /// <param name="right">Complex 结构，用于比较的第二个结构。</param>
        /// <returns>Complex 结构，表示将两个 Complex 结构的所有分量分别取最小值得到的结果。</returns>
        public static Complex Min(Complex left, Complex right)
        {
            return new Complex(Math.Min(left._Real, right._Real), Math.Min(left._Imaginary, right._Imaginary));
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 Complex 结构是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Complex 结构。</param>
        /// <param name="right">运算符右侧比较的 Complex 结构。</param>
        /// <returns>布尔值，表示两个 Complex 结构是否相等。</returns>
        public static bool operator ==(Complex left, Complex right)
        {
            return (left._Real == right._Real && left._Imaginary == right._Imaginary);
        }

        /// <summary>
        /// 判断两个 Complex 结构是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Complex 结构。</param>
        /// <param name="right">运算符右侧比较的 Complex 结构。</param>
        /// <returns>布尔值，表示两个 Complex 结构是否不相等。</returns>
        public static bool operator !=(Complex left, Complex right)
        {
            return (left._Real != right._Real || left._Imaginary != right._Imaginary);
        }

        /// <summary>
        /// 判断两个 Complex 结构的模是否前者小于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Complex 结构。</param>
        /// <param name="right">运算符右侧比较的 Complex 结构。</param>
        /// <returns>布尔值，表示两个 Complex 结构的模是否前者小于后者。</returns>
        public static bool operator <(Complex left, Complex right)
        {
            return (left.Module < right.Module);
        }

        /// <summary>
        /// 判断两个 Complex 结构的模是否前者大于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Complex 结构。</param>
        /// <param name="right">运算符右侧比较的 Complex 结构。</param>
        /// <returns>布尔值，表示两个 Complex 结构的模是否前者大于后者。</returns>
        public static bool operator >(Complex left, Complex right)
        {
            return (left.Module > right.Module);
        }

        /// <summary>
        /// 判断两个 Complex 结构的模是否前者小于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Complex 结构。</param>
        /// <param name="right">运算符右侧比较的 Complex 结构。</param>
        /// <returns>布尔值，表示两个 Complex 结构的模是否前者小于或等于后者。</returns>
        public static bool operator <=(Complex left, Complex right)
        {
            return (left.Module <= right.Module);
        }

        /// <summary>
        /// 判断两个 Complex 结构的模是否前者大于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Complex 结构。</param>
        /// <param name="right">运算符右侧比较的 Complex 结构。</param>
        /// <returns>布尔值，表示两个 Complex 结构的模是否前者大于或等于后者。</returns>
        public static bool operator >=(Complex left, Complex right)
        {
            return (left.Module >= right.Module);
        }

        //

        /// <summary>
        /// 返回在 Complex 结构的所有分量前添加正号得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，用于转换的结构。</param>
        /// <returns>Complex 结构，表示在 Complex 结构的所有分量前添加正号得到的结果。</returns>
        public static Complex operator +(Complex comp)
        {
            return comp;
        }

        /// <summary>
        /// 返回在 Complex 结构的所有分量前添加负号得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，用于转换的结构。</param>
        /// <returns>Complex 结构，表示在 Complex 结构的所有分量前添加负号得到的结果。</returns>
        public static Complex operator -(Complex comp)
        {
            return new Complex(-comp._Real, -comp._Imaginary);
        }

        //

        /// <summary>
        /// 返回将 Complex 结构与 Complex 结构的相加得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示被加数。</param>
        /// <param name="right">Complex 结构，表示加数。</param>
        /// <returns>Complex 结构，表示将 Complex 结构与 Complex 结构的相加得到的结果。</returns>
        public static Complex operator +(Complex left, Complex right)
        {
            return new Complex(left._Real + right._Real, left._Imaginary + right._Imaginary);
        }

        /// <summary>
        /// 返回将 Complex 结构与双精度浮点数的相加得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示被加数。</param>
        /// <param name="right">双精度浮点数，表示加数。</param>
        /// <returns>Complex 结构，表示将 Complex 结构与双精度浮点数的相加得到的结果。</returns>
        public static Complex operator +(Complex left, double right)
        {
            return new Complex(left._Real + right, left._Imaginary);
        }

        /// <summary>
        /// 返回将双精度浮点数与 Complex 结构的相加得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">双精度浮点数，表示被加数。</param>
        /// <param name="right">Complex 结构，表示加数。</param>
        /// <returns>Complex 结构，表示将双精度浮点数与 Complex 结构的相加得到的结果。</returns>
        public static Complex operator +(double left, Complex right)
        {
            return new Complex(left + right._Real, right._Imaginary);
        }

        //

        /// <summary>
        /// 返回将 Complex 结构与 Complex 结构的相减得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示被减数。</param>
        /// <param name="right">Complex 结构，表示减数。</param>
        /// <returns>Complex 结构，表示将 Complex 结构与 Complex 结构的相减得到的结果。</returns>
        public static Complex operator -(Complex left, Complex right)
        {
            return new Complex(left._Real - right._Real, left._Imaginary - right._Imaginary);
        }

        /// <summary>
        /// 返回将 Complex 结构与双精度浮点数的相减得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示被减数。</param>
        /// <param name="right">双精度浮点数，表示减数。</param>
        /// <returns>Complex 结构，表示将 Complex 结构与双精度浮点数的相减得到的结果。</returns>
        public static Complex operator -(Complex left, double right)
        {
            return new Complex(left._Real - right, left._Imaginary);
        }

        /// <summary>
        /// 返回将双精度浮点数与 Complex 结构的相减得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">双精度浮点数，表示被减数。</param>
        /// <param name="right">Complex 结构，表示减数。</param>
        /// <returns>Complex 结构，表示将双精度浮点数与 Complex 结构的相减得到的结果。</returns>
        public static Complex operator -(double left, Complex right)
        {
            return new Complex(left - right._Real, -right._Imaginary);
        }

        //

        /// <summary>
        /// 返回将 Complex 结构与 Complex 结构的相乘得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示被乘数。</param>
        /// <param name="right">Complex 结构，表示乘数。</param>
        /// <returns>Complex 结构，表示将 Complex 结构与 Complex 结构的相乘得到的结果。</returns>
        public static Complex operator *(Complex left, Complex right)
        {
            return new Complex(left._Real * right._Real - left._Imaginary * right._Imaginary, left._Imaginary * right._Real + left._Real * right._Imaginary);
        }

        /// <summary>
        /// 返回将 Complex 结构与双精度浮点数的相乘得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示被乘数。</param>
        /// <param name="right">双精度浮点数，表示乘数。</param>
        /// <returns>Complex 结构，表示将 Complex 结构与双精度浮点数的相乘得到的结果。</returns>
        public static Complex operator *(Complex left, double right)
        {
            return new Complex(left._Real * right, left._Imaginary * right);
        }

        /// <summary>
        /// 返回将双精度浮点数与 Complex 结构的相乘得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">双精度浮点数，表示被乘数。</param>
        /// <param name="right">Complex 结构，表示乘数。</param>
        /// <returns>Complex 结构，表示将双精度浮点数与 Complex 结构的相乘得到的结果。</returns>
        public static Complex operator *(double left, Complex right)
        {
            return new Complex(left * right._Real, left * right._Imaginary);
        }

        //

        /// <summary>
        /// 返回将 Complex 结构与 Complex 结构的相除得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示被除数。</param>
        /// <param name="right">Complex 结构，表示除数。</param>
        /// <returns>Complex 结构，表示将 Complex 结构与 Complex 结构的相除得到的结果。</returns>
        public static Complex operator /(Complex left, Complex right)
        {
            double AbsReR = Math.Abs(right._Real);
            double AbsImR = Math.Abs(right._Imaginary);

            double AbsMaxR = Math.Max(AbsReR, AbsImR);

            if (AbsMaxR == 0)
            {
                return NaN;
            }
            else
            {
                if (AbsReR > AbsImR)
                {
                    double ImRDivReR = right._Imaginary / right._Real;
                    double Down = right._Real + right._Imaginary * ImRDivReR;

                    return new Complex((left._Real + left._Imaginary * ImRDivReR) / Down, (-left._Real * ImRDivReR + left._Imaginary) / Down);
                }
                else
                {
                    double ReRDivImR = right._Real / right._Imaginary;
                    double Down = right._Imaginary + right._Real * ReRDivImR;

                    return new Complex((left._Real * ReRDivImR + left._Imaginary) / Down, (-left._Real + left._Imaginary * ReRDivImR) / Down);
                }
            }
        }

        /// <summary>
        /// 返回将 Complex 结构与双精度浮点数的相除得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示被除数。</param>
        /// <param name="right">双精度浮点数，表示除数。</param>
        /// <returns>Complex 结构，表示将 Complex 结构与双精度浮点数的相除得到的结果。</returns>
        public static Complex operator /(Complex left, double right)
        {
            return new Complex(left._Real / right, left._Imaginary / right);
        }

        /// <summary>
        /// 返回将双精度浮点数与 Complex 结构的相除得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示被除数。</param>
        /// <param name="right">Complex 结构，表示除数。</param>
        /// <returns>Complex 结构，表示将双精度浮点数与 Complex 结构的相除得到的结果。</returns>
        public static Complex operator /(double left, Complex right)
        {
            double AbsReR = Math.Abs(right._Real);
            double AbsImR = Math.Abs(right._Imaginary);

            double AbsMaxR = Math.Max(AbsReR, AbsImR);

            if (AbsMaxR == 0)
            {
                return NaN;
            }
            else
            {
                if (AbsReR > AbsImR)
                {
                    double ImRDivReR = right._Imaginary / right._Real;
                    double Down = right._Real + right._Imaginary * ImRDivReR;

                    return new Complex(left / Down, -left * ImRDivReR / Down);
                }
                else
                {
                    double ReRDivImR = right._Real / right._Imaginary;
                    double Down = right._Imaginary + right._Real * ReRDivImR;

                    return new Complex(left * ReRDivImR / Down, -left / Down);
                }
            }
        }

        //

        /// <summary>
        /// 将指定的 Complex 结构显式转换为 PointD 结构。
        /// </summary>
        /// <param name="comp">用于转换的 Complex 结构。</param>
        /// <returns>PointD 结构，表示显式转换的结果。</returns>
        public static explicit operator PointD(Complex comp)
        {
            return comp.ToPointD();
        }

        /// <summary>
        /// 将指定的双精度浮点数显式转换为 Complex 结构。
        /// </summary>
        /// <param name="real">用于转换的双精度浮点数。</param>
        /// <returns>Complex 结构，表示显式转换的结果。</returns>
        public static explicit operator Complex(double real)
        {
            return new Complex(real);
        }

        #endregion
    }
}