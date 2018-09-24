/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.Complex
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
    /// 以一组有序的双精度浮点数表示的直角坐标形式的二元复数。
    /// </summary>
    public struct Complex
    {
        #region 私有与内部成员

        private double _Real; // 实部。
        private double _Image; // 虚部。

        #endregion

        #region 常量与只读成员

        /// <summary>
        /// 表示所有属性为非数字的 Complex 结构的实例。
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
        public static readonly Complex I = new Complex(0, 1);

        /// <summary>
        /// 表示圆周率 π 的 Complex 结构的实例。
        /// </summary>
        public static readonly Complex PI = new Complex(Math.PI, 0);

        /// <summary>
        /// 表示自然常数 e 的 Complex 结构的实例。
        /// </summary>
        public static readonly Complex E = new Complex(Math.E, 0);

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用双精度浮点数表示的实部与虚部初始化 Complex 结构的新实例。
        /// </summary>
        /// <param name="real">双精度浮点数表示的实部。</param>
        /// <param name="image">双精度浮点数表示的虚部。</param>
        public Complex(double real, double image)
        {
            _Real = real;
            _Image = image;
        }

        /// <summary>
        /// 使用双精度浮点数表示的实部初始化 Complex 结构的新实例。
        /// </summary>
        /// <param name="real">双精度浮点数表示的实部。</param>
        public Complex(double real)
        {
            _Real = real;
            _Image = 0;
        }

        /// <summary>
        /// 使用 PointD 结构初始化 Complex 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构。</param>
        public Complex(PointD pt)
        {
            if ((object)pt != null)
            {
                _Real = pt.X;
                _Image = pt.Y;
            }
            else
            {
                _Real = 0;
                _Image = 0;
            }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取表示此 Complex 结构是否为 NaN 的布尔值。
        /// </summary>
        public bool IsNaN
        {
            get
            {
                return (double.IsNaN(_Real) || double.IsNaN(_Image));
            }
        }

        /// <summary>
        /// 获取表示此 Complex 结构是否为 Infinity 的布尔值。
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return ((!double.IsNaN(_Real) && !double.IsNaN(_Image)) && (double.IsInfinity(_Real) || double.IsInfinity(_Image)));
            }
        }

        /// <summary>
        /// 获取表示此 Complex 结构是否为 NaN 或 Infinity 的布尔值。
        /// </summary>
        public bool IsNaNOrInfinity
        {
            get
            {
                return ((double.IsNaN(_Real) || double.IsNaN(_Image)) || (double.IsInfinity(_Real) || double.IsInfinity(_Image)));
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
                return (_Real == Zero._Real && _Image == Zero._Image);
            }
        }

        /// <summary>
        /// 获取表示此 Complex 结构是否为实数单位 1 的布尔值。
        /// </summary>
        public bool IsOne
        {
            get
            {
                return (_Real == One._Real && _Image == One._Image);
            }
        }

        /// <summary>
        /// 获取表示此 Complex 结构是否为虚数单位 i 的布尔值。
        /// </summary>
        public bool IsI
        {
            get
            {
                return (_Real == I._Real && _Image == I._Image);
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
        public double Image
        {
            get
            {
                return _Image;
            }

            set
            {
                _Image = value;
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
                return Math.Sqrt(_Real * _Real + _Image * _Image);
            }
        }

        /// <summary>
        /// 获取此 Complex 结构的模平方。
        /// </summary>
        public double ModuleSquared
        {
            get
            {
                return (_Real * _Real + _Image * _Image);
            }
        }

        /// <summary>
        /// 获取此 Complex 结构的辐角（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。
        /// </summary>
        public double Argument
        {
            get
            {
                if (_Real == 0 && _Image == 0)
                {
                    return 0;
                }
                else
                {
                    double Angle = Math.Atan(_Image / _Real);

                    if (_Real < 0)
                    {
                        return (Angle + Math.PI);
                    }
                    else if (_Image < 0)
                    {
                        return (Angle + 2 * Math.PI);
                    }
                    else
                    {
                        return Angle;
                    }
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
                return new Complex(_Real, -_Image);
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 Complex 结构是否与指定的 Complex 结构相等。
        /// </summary>
        /// <param name="comp">用于比较的 Complex 结构。</param>
        public bool Equals(Complex comp)
        {
            if ((object)comp == null)
            {
                return false;
            }

            return (_Real.Equals(comp._Real) && _Image.Equals(comp._Image));
        }

        //

        /// <summary>
        /// 返回将此 Complex 结构转换为 PointD 结构的新实例。
        /// </summary>
        public PointD ToPointD()
        {
            return new PointD(_Real, _Image);
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断两个 Complex 结构是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 Complex 结构。</param>
        /// <param name="right">用于比较的第二个 Complex 结构。</param>
        public static bool Equals(Complex left, Complex right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return left.Equals(right);
        }

        //

        /// <summary>
        /// 返回将 PointD 结构转换为 Complex 结构的新实例。
        /// </summary>
        /// <param name="pt">Point 结构。</param>
        public static Complex FromPointD(PointD pt)
        {
            if ((object)pt != null)
            {
                return new Complex(pt.X, pt.Y);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回对 Complex 结构的计算平方得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示底数。</param>
        public static Complex Sqr(Complex comp)
        {
            if ((object)comp != null)
            {
                return new Complex(comp._Real * comp._Real - comp._Image * comp._Image, 2 * comp._Real * comp._Image);
            }

            return NaN;
        }

        /// <summary>
        /// 返回对 Complex 结构的计算平方根得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示被开方数。</param>
        public static Complex Sqrt(Complex comp)
        {
            if ((object)comp != null)
            {
                double Mod = Math.Sqrt(comp.Module);
                double Arg = comp.Argument / 2;

                return new Complex(Mod * Math.Cos(Arg), Mod * Math.Sin(Arg));
            }

            return NaN;
        }

        /// <summary>
        /// 返回对 Complex 结构的计算自然幂得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示底数。</param>
        public static Complex Exp(Complex comp)
        {
            if ((object)comp != null)
            {
                double Mod = Math.Exp(comp._Real);
                double Arg = comp._Image;

                return new Complex(Mod * Math.Cos(Arg), Mod * Math.Sin(Arg));
            }

            return NaN;
        }

        /// <summary>
        /// 返回对 Complex 结构的计算幂得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示底数。</param>
        /// <param name="right">Complex 结构，表示指数。</param>
        public static Complex Pow(Complex left, Complex right)
        {
            if ((object)left != null && (object)right != null)
            {
                double ModL = left.Module;

                if (ModL == 0)
                {
                    if (right.Module == 0)
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

                    double Mod = Math.Exp(LnModL * right._Real - ArgL * right._Image);
                    double Arg = ArgL * right._Real + LnModL * right._Image;

                    return new Complex(Mod * Math.Cos(Arg), Mod * Math.Sin(Arg));
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回对 Complex 结构的计算自然对数得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示幂。</param>
        public static Complex Log(Complex comp)
        {
            if ((object)comp != null)
            {
                double Real = Math.Log(comp.Module);
                double Image = comp.Argument;

                return new Complex(Real, Image);
            }

            return NaN;
        }

        /// <summary>
        /// 返回对 Complex 结构的计算对数得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示基数。</param>
        /// <param name="right">Complex 结构，表示幂。</param>
        public static Complex Log(Complex left, Complex right)
        {
            if ((object)left != null && (object)right != null)
            {
                if (left.Module == 0 && right.IsOne)
                {
                    return Zero;
                }
                else
                {
                    return (Log(right) / Log(left));
                }
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回对 Complex 结构的计算正弦得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示以弧度计量的角度。</param>
        public static Complex Sin(Complex comp)
        {
            if ((object)comp != null)
            {
                double ExpPIm = Math.Exp(comp._Image);
                double ExpNIm = Math.Exp(-comp._Image);

                return new Complex((ExpPIm + ExpNIm) * Math.Sin(comp._Real) / 2, (ExpPIm - ExpNIm) * Math.Cos(comp._Real) / 2);
            }

            return NaN;
        }

        /// <summary>
        /// 返回对 Complex 结构的计算余弦得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示以弧度计量的角度。</param>
        public static Complex Cos(Complex comp)
        {
            if ((object)comp != null)
            {
                double ExpPIm = Math.Exp(comp._Image);
                double ExpNIm = Math.Exp(-comp._Image);

                return new Complex((ExpPIm + ExpNIm) * Math.Cos(comp._Real) / 2, -(ExpPIm - ExpNIm) * Math.Sin(comp._Real) / 2);
            }

            return NaN;
        }

        /// <summary>
        /// 返回对 Complex 结构的计算正切得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示以弧度计量的角度。</param>
        public static Complex Tan(Complex comp)
        {
            if ((object)comp != null)
            {
                return (Sin(comp) / Cos(comp));
            }

            return NaN;
        }

        /// <summary>
        /// 返回对 Complex 结构的计算反正弦得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示正弦值的复数。</param>
        public static Complex Asin(Complex comp)
        {
            if ((object)comp != null)
            {
                return (-I * Log(I * comp + Sqrt(One - comp * comp)));
            }

            return NaN;
        }

        /// <summary>
        /// 返回对 Complex 结构的计算反余弦得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示余弦值的复数。</param>
        public static Complex Acos(Complex comp)
        {
            if ((object)comp != null)
            {
                return (-I * Log(comp + Sqrt(comp * comp - One)));
            }

            return NaN;
        }

        /// <summary>
        /// 返回对 Complex 结构的计算反正切得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示正切值的复数。</param>
        public static Complex Atan(Complex comp)
        {
            if ((object)comp != null)
            {
                return (I * Log((One - I * comp) / (One + I * comp)) / new Complex(2));
            }

            return NaN;
        }

        /// <summary>
        /// 返回对 Complex 结构的计算双曲正弦得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示以弧度计量的角度。</param>
        public static Complex Sinh(Complex comp)
        {
            if ((object)comp != null)
            {
                return (-I * Sin(I * comp));
            }

            return NaN;
        }

        /// <summary>
        /// 返回对 Complex 结构的计算双曲余弦得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示以弧度计量的角度。</param>
        public static Complex Cosh(Complex comp)
        {
            if ((object)comp != null)
            {
                return Cos(I * comp);
            }

            return NaN;
        }

        /// <summary>
        /// 返回对 Complex 结构的计算双曲正切得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示以弧度计量的角度。</param>
        public static Complex Tanh(Complex comp)
        {
            if ((object)comp != null)
            {
                return (-I * Tan(I * comp));
            }

            return NaN;
        }

        /// <summary>
        /// 返回对 Complex 结构的计算反双曲正弦得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示双曲正弦值的复数。</param>
        public static Complex Asinh(Complex comp)
        {
            if ((object)comp != null)
            {
                return Log(comp + Sqrt(comp * comp + One));
            }

            return NaN;
        }

        /// <summary>
        /// 返回对 Complex 结构的计算反双曲余弦得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示双曲余弦值的复数。</param>
        public static Complex Acosh(Complex comp)
        {
            if ((object)comp != null)
            {
                return Log(comp + Sqrt(comp * comp - One));
            }

            return NaN;
        }

        /// <summary>
        /// 返回对 Complex 结构的计算反双曲正切得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，表示双曲正切值的复数。</param>
        public static Complex Atanh(Complex comp)
        {
            if ((object)comp != null)
            {
                return (Log((One + comp) / (One - comp)) / new Complex(2));
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 Complex 结构的所有分量取绝对值得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，用于转换的结构。</param>
        public static Complex Abs(Complex comp)
        {
            if ((object)comp != null)
            {
                return new Complex(Math.Abs(comp._Real), Math.Abs(comp._Image));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Complex 结构的所有分量取符号数得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，用于转换的结构。</param>
        public static Complex Sign(Complex comp)
        {
            if ((object)comp != null)
            {
                return new Complex(Math.Sign(comp._Real), Math.Sign(comp._Image));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Complex 结构的所有分量舍入到较大的整数值得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，用于转换的结构。</param>
        public static Complex Ceiling(Complex comp)
        {
            if ((object)comp != null)
            {
                return new Complex(Math.Ceiling(comp._Real), Math.Ceiling(comp._Image));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Complex 结构的所有分量舍入到较小的整数值得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，用于转换的结构。</param>
        public static Complex Floor(Complex comp)
        {
            if ((object)comp != null)
            {
                return new Complex(Math.Floor(comp._Real), Math.Floor(comp._Image));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Complex 结构的所有分量舍入到最接近的整数值得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，用于转换的结构。</param>
        public static Complex Round(Complex comp)
        {
            if ((object)comp != null)
            {
                return new Complex(Math.Round(comp._Real), Math.Round(comp._Image));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Complex 结构的所有分量截断小数部分取整得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，用于转换的结构。</param>
        public static Complex Truncate(Complex comp)
        {
            if ((object)comp != null)
            {
                return new Complex(Math.Truncate(comp._Real), Math.Truncate(comp._Image));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将两个 Complex 结构的所有分量分别取最大值得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，用于比较的第一个结构。</param>
        /// <param name="right">Complex 结构，用于比较的第二个结构。</param>
        public static Complex Max(Complex left, Complex right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new Complex(Math.Max(left._Real, right._Real), Math.Max(left._Image, right._Image));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将两个 Complex 结构的所有分量分别取最小值得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，用于比较的第一个结构。</param>
        /// <param name="right">Complex 结构，用于比较的第二个结构。</param>
        public static Complex Min(Complex left, Complex right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new Complex(Math.Min(left._Real, right._Real), Math.Min(left._Image, right._Image));
            }

            return NaN;
        }

        #endregion

        #region 基类方法

        /// <summary>
        /// 判断此 Complex 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Complex))
            {
                return false;
            }

            return Equals((Complex)obj);
        }

        /// <summary>
        /// 返回此 Complex 结构的哈希代码。
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 Complex 结构转换为字符串。
        /// </summary>
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
                if (_Image == 0)
                {
                    return _Real.ToString();
                }
                else
                {
                    if (_Real == 0)
                    {
                        if (_Image == 1)
                        {
                            return "i";
                        }
                        else if (_Image == -1)
                        {
                            return "-i";
                        }
                        else
                        {
                            return string.Concat(_Image, " i");
                        }
                    }
                    else
                    {
                        if (_Image == 1)
                        {
                            return string.Concat(_Real, " + i");
                        }
                        else if (_Image == -1)
                        {
                            return string.Concat(_Real, " - i");
                        }
                        else if (_Image > 0)
                        {
                            return string.Concat(_Real, " + ", _Image, " i");
                        }
                        else
                        {
                            return string.Concat(_Real, " - ", (-_Image), " i");
                        }
                    }
                }
            }
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 Complex 结构是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Complex 结构。</param>
        /// <param name="right">运算符右侧比较的 Complex 结构。</param>
        public static bool operator ==(Complex left, Complex right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left._Real == right._Real && left._Image == right._Image);
        }

        /// <summary>
        /// 判断两个 Complex 结构是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Complex 结构。</param>
        /// <param name="right">运算符右侧比较的 Complex 结构。</param>
        public static bool operator !=(Complex left, Complex right)
        {
            if ((object)left == null && (object)right == null)
            {
                return false;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return false;
            }
            else if ((object)left == null || (object)right == null)
            {
                return true;
            }

            return (left._Real != right._Real || left._Image != right._Image);
        }

        /// <summary>
        /// 判断两个 Complex 结构的模平方是否前者小于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Complex 结构。</param>
        /// <param name="right">运算符右侧比较的 Complex 结构。</param>
        public static bool operator <(Complex left, Complex right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left.ModuleSquared < right.ModuleSquared);
        }

        /// <summary>
        /// 判断两个 Complex 结构的模平方是否前者大于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Complex 结构。</param>
        /// <param name="right">运算符右侧比较的 Complex 结构。</param>
        public static bool operator >(Complex left, Complex right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left.ModuleSquared > right.ModuleSquared);
        }

        /// <summary>
        /// 判断两个 Complex 结构的模平方是否前者小于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Complex 结构。</param>
        /// <param name="right">运算符右侧比较的 Complex 结构。</param>
        public static bool operator <=(Complex left, Complex right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left.ModuleSquared <= right.ModuleSquared);
        }

        /// <summary>
        /// 判断两个 Complex 结构的模平方是否前者大于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Complex 结构。</param>
        /// <param name="right">运算符右侧比较的 Complex 结构。</param>
        public static bool operator >=(Complex left, Complex right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left.ModuleSquared >= right.ModuleSquared);
        }

        //

        /// <summary>
        /// 返回在 Complex 结构的所有分量前添加正号得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，用于转换的结构。</param>
        public static Complex operator +(Complex comp)
        {
            if ((object)comp != null)
            {
                return new Complex(+comp._Real, +comp._Image);
            }

            return NaN;
        }

        /// <summary>
        /// 返回在 Complex 结构的所有分量前添加负号得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构，用于转换的结构。</param>
        public static Complex operator -(Complex comp)
        {
            if ((object)comp != null)
            {
                return new Complex(-comp._Real, -comp._Image);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 Complex 结构与 Complex 结构的相加得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示被加数。</param>
        /// <param name="right">Complex 结构，表示加数。</param>
        public static Complex operator +(Complex left, Complex right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new Complex(left._Real + right._Real, left._Image + right._Image);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Complex 结构与 Complex 结构的相减得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示被减数。</param>
        /// <param name="right">Complex 结构，表示减数。</param>
        public static Complex operator -(Complex left, Complex right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new Complex(left._Real - right._Real, left._Image - right._Image);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Complex 结构与 Complex 结构的相乘得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示被乘数。</param>
        /// <param name="right">Complex 结构，表示乘数。</param>
        public static Complex operator *(Complex left, Complex right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new Complex(left._Real * right._Real - left._Image * right._Image, left._Image * right._Real + left._Real * right._Image);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Complex 结构与 Complex 结构的相除得到的 Complex 结构的新实例。
        /// </summary>
        /// <param name="left">Complex 结构，表示被除数。</param>
        /// <param name="right">Complex 结构，表示除数。</param>
        public static Complex operator /(Complex left, Complex right)
        {
            if ((object)left != null && (object)right != null)
            {
                double ModSqrR = right.ModuleSquared;

                return new Complex((left._Real * right._Real + left._Image * right._Image) / ModSqrR, (left._Image * right._Real - left._Real * right._Image) / ModSqrR);
            }

            return NaN;
        }

        #endregion
    }
}