/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.PointD
Version 18.7.6.2250

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Com
{
    /// <summary>
    /// 以一组有序的双精度浮点数表示的二维直角坐标系坐标。
    /// </summary>
    public struct PointD
    {
        #region 私有与内部成员

        private double _X; // X 坐标。
        private double _Y; // Y 坐标。

        #endregion

        #region 常量与只读成员

        /// <summary>
        /// 表示所有属性为其数据类型的默认值的 PointD 结构的实例。
        /// </summary>
        public static readonly PointD Empty = default(PointD);

        /// <summary>
        /// 表示所有属性为非数字的 PointD 结构的实例。
        /// </summary>
        public static readonly PointD NaN = new PointD(double.NaN, double.NaN);

        //

        /// <summary>
        /// 表示零向量的 PointD 结构的实例。
        /// </summary>
        public static readonly PointD Zero = new PointD(0, 0);

        /// <summary>
        /// 表示 X 基向量的 PointD 结构的实例。
        /// </summary>
        public static readonly PointD Ex = new PointD(1, 0);

        /// <summary>
        /// 表示 Y 基向量的 PointD 结构的实例。
        /// </summary>
        public static readonly PointD Ey = new PointD(0, 1);

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用双精度浮点数表示的 X 坐标与 Y 坐标初始化 PointD 结构的新实例。
        /// </summary>
        /// <param name="x">双精度浮点数表示的 X 坐标。</param>
        /// <param name="y">双精度浮点数表示的 Y 坐标。</param>
        public PointD(double x, double y)
        {
            _X = x;
            _Y = y;
        }

        /// <summary>
        /// 使用 Point 结构初始化 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">Point 结构。</param>
        public PointD(Point pt)
        {
            if ((object)pt != null)
            {
                _X = pt.X;
                _Y = pt.Y;
            }
            else
            {
                _X = 0;
                _Y = 0;
            }
        }

        /// <summary>
        /// 使用 PointF 结构初始化 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointF 结构。</param>
        public PointD(PointF pt)
        {
            if ((object)pt != null)
            {
                _X = pt.X;
                _Y = pt.Y;
            }
            else
            {
                _X = 0;
                _Y = 0;
            }
        }

        /// <summary>
        /// 使用 Size 结构初始化 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构。</param>
        public PointD(Size sz)
        {
            if ((object)sz != null)
            {
                _X = sz.Width;
                _Y = sz.Height;
            }
            else
            {
                _X = 0;
                _Y = 0;
            }
        }

        /// <summary>
        /// 使用 SizeF 结构初始化 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构。</param>
        public PointD(SizeF sz)
        {
            if ((object)sz != null)
            {
                _X = sz.Width;
                _Y = sz.Height;
            }
            else
            {
                _X = 0;
                _Y = 0;
            }
        }

        /// <summary>
        /// 使用 Complex 结构初始化 PointD 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构。</param>
        public PointD(Complex comp)
        {
            if ((object)comp != null)
            {
                _X = comp.Real;
                _Y = comp.Image;
            }
            else
            {
                _X = 0;
                _Y = 0;
            }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置此 PointD 结构在指定索引的坐标轴的分量。
        /// </summary>
        /// <param name="index">索引。</param>
        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return _X;
                    case 1: return _Y;
                    default: return double.NaN;
                }
            }

            set
            {
                switch (index)
                {
                    case 0: _X = value; break;
                    case 1: _Y = value; break;
                }
            }
        }

        //

        /// <summary>
        /// 获取表示此 PointD 结构是否为 Empty 的布尔值。
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (_X == Empty._X && _Y == Empty._Y);
            }
        }

        /// <summary>
        /// 获取表示此 PointD 结构是否为 NaN 的布尔值。
        /// </summary>
        public bool IsNaN
        {
            get
            {
                return (double.IsNaN(_X) || double.IsNaN(_Y));
            }
        }

        /// <summary>
        /// 获取表示此 PointD 结构是否为 Infinity 的布尔值。
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return ((!double.IsNaN(_X) && !double.IsNaN(_Y)) && (double.IsInfinity(_X) || double.IsInfinity(_Y)));
            }
        }

        /// <summary>
        /// 获取表示此 PointD 结构是否为 NaN 或 Infinity 的布尔值。
        /// </summary>
        public bool IsNaNOrInfinity
        {
            get
            {
                return ((double.IsNaN(_X) || double.IsNaN(_Y)) || (double.IsInfinity(_X) || double.IsInfinity(_Y)));
            }
        }

        //

        /// <summary>
        /// 获取或设置此 PointD 结构在 X 轴的分量。
        /// </summary>
        public double X
        {
            get
            {
                return _X;
            }

            set
            {
                _X = value;
            }
        }

        /// <summary>
        /// 获取或设置此 PointD 结构在 Y 轴的分量。
        /// </summary>
        public double Y
        {
            get
            {
                return _Y;
            }

            set
            {
                _Y = value;
            }
        }

        //

        /// <summary>
        /// 获取此 PointD 结构表示的向量与 X 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleX
        {
            get
            {
                return AngleFrom(_X >= 0 ? new PointD(1, 0) : new PointD(-1, 0));
            }
        }

        /// <summary>
        /// 获取此 PointD 结构表示的向量与 Y 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleY
        {
            get
            {
                return AngleFrom(_Y >= 0 ? new PointD(0, 1) : new PointD(0, -1));
            }
        }

        //

        /// <summary>
        /// 获取此 PointD 结构表示的向量的模。
        /// </summary>
        public double VectorModule
        {
            get
            {
                return Math.Sqrt(_X * _X + _Y * _Y);
            }
        }

        /// <summary>
        /// 获取此 PointD 结构表示的向量的模平方。
        /// </summary>
        public double VectorModuleSquared
        {
            get
            {
                return (_X * _X + _Y * _Y);
            }
        }

        /// <summary>
        /// 获取此 PointD 结构表示的向量与 +X 轴之间的夹角（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。
        /// </summary>
        public double VectorAngle
        {
            get
            {
                if (_X == 0 && _Y == 0)
                {
                    return 0;
                }
                else
                {
                    double Angle = Math.Atan(_Y / _X);

                    if (_X < 0)
                    {
                        return (Angle + Math.PI);
                    }
                    else if (_Y < 0)
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
        /// 获取此 PointD 结构表示的向量的相反向量。
        /// </summary>
        public PointD VectorNegate
        {
            get
            {
                return new PointD(-_X, -_Y);
            }
        }

        /// <summary>
        /// 获取此 PointD 结构表示的向量的规范化向量。
        /// </summary>
        public PointD VectorNormalize
        {
            get
            {
                double Mod = VectorModule;

                if (Mod > 0)
                {
                    return new PointD(_X / Mod, _Y / Mod);
                }
                else
                {
                    return Ex;
                }
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 PointD 结构是否与指定的 PointD 结构相等。
        /// </summary>
        /// <param name="pt">用于比较的 PointD 结构。</param>
        public bool Equals(PointD pt)
        {
            if ((object)pt == null)
            {
                return false;
            }

            return (_X == pt._X && _Y == pt._Y);
        }

        //

        /// <summary>
        /// 返回将此 PointD 结构转换为 Point 结构的新实例。
        /// </summary>
        public Point ToPoint()
        {
            int x = (_X.Equals(double.NaN) ? 0 : (_X < int.MinValue ? int.MinValue : (_X > int.MaxValue ? int.MaxValue : (int)_X)));
            int y = (_Y.Equals(double.NaN) ? 0 : (_Y < int.MinValue ? int.MinValue : (_Y > int.MaxValue ? int.MaxValue : (int)_Y)));

            return new Point(x, y);
        }

        /// <summary>
        /// 返回将此 PointD 结构转换为 PointF 结构的新实例。
        /// </summary>
        public PointF ToPointF()
        {
            float x = (_X.Equals(double.NaN) ? float.NaN : (_X < float.MinValue ? float.NegativeInfinity : (_X > float.MaxValue ? float.PositiveInfinity : (float)_X)));
            float y = (_Y.Equals(double.NaN) ? float.NaN : (_Y < float.MinValue ? float.NegativeInfinity : (_Y > float.MaxValue ? float.PositiveInfinity : (float)_Y)));

            return new PointF(x, y);
        }

        /// <summary>
        /// 返回将此 PointD 结构转换为 Size 结构的新实例。
        /// </summary>
        public Size ToSize()
        {
            int width = (_X.Equals(double.NaN) ? 0 : (_X < int.MinValue ? int.MinValue : (_X > int.MaxValue ? int.MaxValue : (int)_X)));
            int height = (_Y.Equals(double.NaN) ? 0 : (_Y < int.MinValue ? int.MinValue : (_Y > int.MaxValue ? int.MaxValue : (int)_Y)));

            return new Size(width, height);
        }

        /// <summary>
        /// 返回将此 PointD 结构转换为 SizeF 结构的新实例。
        /// </summary>
        public SizeF ToSizeF()
        {
            float width = (_X.Equals(double.NaN) ? float.NaN : (_X < float.MinValue ? float.NegativeInfinity : (_X > float.MaxValue ? float.PositiveInfinity : (float)_X)));
            float height = (_Y.Equals(double.NaN) ? float.NaN : (_Y < float.MinValue ? float.NegativeInfinity : (_Y > float.MaxValue ? float.PositiveInfinity : (float)_Y)));

            return new SizeF(width, height);
        }

        /// <summary>
        /// 返回将此 PointD 结构转换为 Complex 结构的新实例。
        /// </summary>
        public Complex ToComplex()
        {
            return new Complex(_X, _Y);
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的所有坐标偏移量将此 PointD 结构平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的所有坐标偏移量。</param>
        public void Offset(double d)
        {
            _X += d;
            _Y += d;
        }

        /// <summary>
        /// 按双精度浮点数表示的 X 坐标偏移量与 Y 坐标偏移量将此 PointD 结构平移指定的量。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标偏移量。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标偏移量。</param>
        public void Offset(double dx, double dy)
        {
            _X += dx;
            _Y += dy;
        }

        /// <summary>
        /// 按 PointD 结构将此 PointD 结构平移指定的量。
        /// </summary>
        /// <param name="pt">PointD 结构，用于平移此 PointD 结构。</param>
        public void Offset(PointD pt)
        {
            if ((object)pt != null)
            {
                _X += pt.X;
                _Y += pt.Y;
            }
        }

        /// <summary>
        /// 按 Point 结构将此 PointD 结构平移指定的量。
        /// </summary>
        /// <param name="pt">Point 结构，用于平移此 PointD 结构。</param>
        public void Offset(Point pt)
        {
            if ((object)pt != null)
            {
                _X += pt.X;
                _Y += pt.Y;
            }
        }

        /// <summary>
        /// 按 PointF 结构将此 PointD 结构平移指定的量。
        /// </summary>
        /// <param name="pt">PointF 结构，用于平移此 PointD 结构。</param>
        public void Offset(PointF pt)
        {
            if ((object)pt != null)
            {
                _X += pt.X;
                _Y += pt.Y;
            }
        }

        /// <summary>
        /// 按 Size 结构将此 PointD 结构平移指定的量。
        /// </summary>
        /// <param name="sz">Size 结构，用于平移此 PointD 结构。</param>
        public void Offset(Size sz)
        {
            if ((object)sz != null)
            {
                _X += sz.Width;
                _Y += sz.Height;
            }
        }

        /// <summary>
        /// 按 SizeF 结构将此 PointD 结构平移指定的量。
        /// </summary>
        /// <param name="sz">SizeF 结构，用于平移此 PointD 结构。</param>
        public void Offset(SizeF sz)
        {
            if ((object)sz != null)
            {
                _X += sz.Width;
                _Y += sz.Height;
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标偏移量将此 PointD 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的所有坐标偏移量。</param>
        public PointD OffsetCopy(double d)
        {
            return new PointD(_X + d, _Y + d);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标偏移量与 Y 坐标偏移量将此 PointD 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标偏移量。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标偏移量。</param>
        public PointD OffsetCopy(double dx, double dy)
        {
            return new PointD(_X + dx, _Y + dy);
        }

        /// <summary>
        /// 返回按 PointD 结构将此 PointD 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于平移此 PointD 结构。</param>
        public PointD OffsetCopy(PointD pt)
        {
            if ((object)pt != null)
            {
                return new PointD(_X + pt.X, _Y + pt.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 Point 结构将此 PointD 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="pt">Point 结构，用于平移此 PointD 结构。</param>
        public PointD OffsetCopy(Point pt)
        {
            if ((object)pt != null)
            {
                return new PointD(_X + pt.X, _Y + pt.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 PointF 结构将此 PointD 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="pt">PointF 结构，用于平移此 PointD 结构。</param>
        public PointD OffsetCopy(PointF pt)
        {
            if ((object)pt != null)
            {
                return new PointD(_X + pt.X, _Y + pt.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 Size 结构将此 PointD 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，用于平移此 PointD 结构。</param>
        public PointD OffsetCopy(Size sz)
        {
            if ((object)sz != null)
            {
                return new PointD(_X + sz.Width, _Y + sz.Height);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 SizeF 结构将此 PointD 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，用于平移此 PointD 结构。</param>
        public PointD OffsetCopy(SizeF sz)
        {
            if ((object)sz != null)
            {
                return new PointD(_X + sz.Width, _Y + sz.Height);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的所有坐标缩放因子将此 PointD 结构缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的所有坐标缩放因子。</param>
        public void Scale(double s)
        {
            _X *= s;
            _Y *= s;
        }

        /// <summary>
        /// 按双精度浮点数表示的 X 坐标缩放因子与 Y 坐标缩放因子将此 PointD 结构缩放指定的倍数。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因子。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因子。</param>
        public void Scale(double sx, double sy)
        {
            _X *= sx;
            _Y *= sy;
        }

        /// <summary>
        /// 按 PointD 结构将此 PointD 结构缩放指定的倍数。
        /// </summary>
        /// <param name="pt">PointD 结构，用于缩放此 PointD 结构。</param>
        public void Scale(PointD pt)
        {
            if ((object)pt != null)
            {
                _X *= pt.X;
                _Y *= pt.Y;
            }
        }

        /// <summary>
        /// 按 Point 结构将此 PointD 结构缩放指定的倍数。
        /// </summary>
        /// <param name="pt">Point 结构，用于缩放此 PointD 结构。</param>
        public void Scale(Point pt)
        {
            if ((object)pt != null)
            {
                _X *= pt.X;
                _Y *= pt.Y;
            }
        }

        /// <summary>
        /// 按 PointF 结构将此 PointD 结构缩放指定的倍数。
        /// </summary>
        /// <param name="pt">PointF 结构，用于缩放此 PointD 结构。</param>
        public void Scale(PointF pt)
        {
            if ((object)pt != null)
            {
                _X *= pt.X;
                _Y *= pt.Y;
            }
        }

        /// <summary>
        /// 按 Size 结构将此 PointD 结构缩放指定的倍数。
        /// </summary>
        /// <param name="sz">Size 结构，用于缩放此 PointD 结构。</param>
        public void Scale(Size sz)
        {
            if ((object)sz != null)
            {
                _X *= sz.Width;
                _Y *= sz.Height;
            }
        }

        /// <summary>
        /// 按 SizeF 结构将此 PointD 结构缩放指定的倍数。
        /// </summary>
        /// <param name="sz">SizeF 结构，用于缩放此 PointD 结构。</param>
        public void Scale(SizeF sz)
        {
            if ((object)sz != null)
            {
                _X *= sz.Width;
                _Y *= sz.Height;
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标缩放因子将此 PointD 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的所有坐标缩放因子。</param>
        public PointD ScaleCopy(double s)
        {
            return new PointD(_X * s, _Y * s);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标缩放因子与 Y 坐标缩放因子将此 PointD 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因子。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因子。</param>
        public PointD ScaleCopy(double sx, double sy)
        {
            return new PointD(_X * sx, _Y * sy);
        }

        /// <summary>
        /// 返回按 PointD 结构将此 PointD 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于缩放此 PointD 结构。</param>
        public PointD ScaleCopy(PointD pt)
        {
            if ((object)pt != null)
            {
                return new PointD(_X * pt.X, _Y * pt.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 Point 结构将此 PointD 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="pt">Point 结构，用于缩放此 PointD 结构。</param>
        public PointD ScaleCopy(Point pt)
        {
            if ((object)pt != null)
            {
                return new PointD(_X * pt.X, _Y * pt.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 PointF 结构将此 PointD 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="pt">PointF 结构，用于缩放此 PointD 结构。</param>
        public PointD ScaleCopy(PointF pt)
        {
            if ((object)pt != null)
            {
                return new PointD(_X * pt.X, _Y * pt.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 Size 结构将此 PointD 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，用于缩放此 PointD 结构。</param>
        public PointD ScaleCopy(Size sz)
        {
            if ((object)sz != null)
            {
                return new PointD(_X * sz.Width, _Y * sz.Height);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 SizeF 结构将此 PointD 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，用于缩放此 PointD 结构。</param>
        public PointD ScaleCopy(SizeF sz)
        {
            if ((object)sz != null)
            {
                return new PointD(_X * sz.Width, _Y * sz.Height);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD 结构绕原点旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕原点旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public void Rotate(double angle)
        {
            double __X = _X, __Y = _Y;

            _X = __X * Math.Cos(angle) - __Y * Math.Sin(angle);
            _Y = __X * Math.Sin(angle) + __Y * Math.Cos(angle);
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD 结构绕指定的 PointD 结构旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕指定的 PointD 结构旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <param name="pt">PointD 结构，表示旋转中心。</param>
        public void Rotate(double angle, PointD pt)
        {
            if ((object)pt != null)
            {
                double __X = _X, __Y = _Y;

                _X = (__X - pt.X) * Math.Cos(angle) - (__Y - pt.Y) * Math.Sin(angle) + pt.X;
                _Y = (__X - pt.X) * Math.Sin(angle) + (__Y - pt.Y) * Math.Cos(angle) + pt.Y;
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构的副本绕原点旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕原点旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public PointD RotateCopy(double angle)
        {
            PointD result = new PointD();

            result.X = _X * Math.Cos(angle) - _Y * Math.Sin(angle);
            result.Y = _X * Math.Sin(angle) + _Y * Math.Cos(angle);

            return result;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构的副本绕指定的 PointD 结构旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕指定的 PointD 结构旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <param name="pt">PointD 结构，表示旋转中心。</param>
        public PointD RotateCopy(double angle, PointD pt)
        {
            if ((object)pt != null)
            {
                PointD result = new PointD();

                result.X = (_X - pt.X) * Math.Cos(angle) - (_Y - pt.Y) * Math.Sin(angle) + pt.X;
                result.Y = (_X - pt.X) * Math.Sin(angle) + (_Y - pt.Y) * Math.Cos(angle) + pt.Y;

                return result;
            }

            return NaN;
        }

        //

        /// <summary>
        /// 按 PointD 结构表示的 X 基向量、Y 基向量与偏移向量将此 PointD 结构进行仿射变换。
        /// </summary>
        /// <param name="ex">PointD 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD 结构表示的 Y 基向量。</param>
        /// <param name="offset">PointD 结构表示的偏移向量。</param>
        public void AffineTransform(PointD ex, PointD ey, PointD offset)
        {
            if ((object)ex != null && (object)ey != null && (object)offset != null)
            {
                Matrix2D matrixLeft = new Matrix2D(new double[3, 3]
                {
                    { ex.X, ex.Y, 0 },
                    { ey.X, ey.Y, 0 },
                    { offset.X, offset.Y, 1 }
                });

                Matrix2D matrixRight = new Matrix2D(new double[1, 3]
                {
                    { _X, _Y, 1 }
                });

                Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

                if (!Matrix2D.IsNullOrNonMatrix(result))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                }
            }
        }

        /// <summary>
        /// 按双精度浮点数数组表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeft">双精度浮点数数组表示的 3x3 仿射矩阵（左矩阵）。</param>
        public void AffineTransform(Matrix2D matrixLeft)
        {
            if (!Matrix2D.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(3, 3))
            {
                Matrix2D matrixRight = new Matrix2D(new double[1, 3]
                {
                    { _X, _Y, 1 }
                });

                Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

                if (!Matrix2D.IsNullOrNonMatrix(result))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                }
            }
        }

        /// <summary>
        /// 按双精度浮点数数组表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">双精度浮点数数组表示的 3x3 仿射矩阵（左矩阵）列表。</param>
        public void AffineTransform(List<Matrix2D> matrixLeftList)
        {
            if (matrixLeftList.Count > 0)
            {
                Matrix2D result = new Matrix2D(new double[1, 3]
                {
                    { _X, _Y, 1 }
                });

                for (int i = 0; i < matrixLeftList.Count; i++)
                {
                    Matrix2D matrixLeft = matrixLeftList[i];

                    bool flag = (!Matrix2D.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(3, 3));

                    if (flag)
                    {
                        result = Matrix2D.Multiply(matrixLeft, result);

                        flag = !Matrix2D.IsNullOrNonMatrix(result);
                    }

                    if (!flag)
                    {
                        return;
                    }
                }

                _X = result[0, 0];
                _Y = result[0, 1];
            }
        }

        /// <summary>
        /// 返回按 PointD 结构表示的 X 基向量、Y 基向量与偏移向量将此 PointD 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="ex">PointD 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD 结构表示的 Y 基向量。</param>
        /// <param name="offset">PointD 结构表示的偏移向量。</param>
        public PointD AffineTransformCopy(PointD ex, PointD ey, PointD offset)
        {
            if ((object)ex != null && (object)ey != null && (object)offset != null)
            {
                Matrix2D matrixLeft = new Matrix2D(new double[3, 3]
                {
                    { ex.X, ex.Y, 0 },
                    { ey.X, ey.Y, 0 },
                    { offset.X, offset.Y, 1 }
                });

                Matrix2D matrixRight = new Matrix2D(new double[1, 3]
                {
                    { _X, _Y, 1 }
                });

                Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

                if (!Matrix2D.IsNullOrNonMatrix(result))
                {
                    return new PointD(result[0, 0], result[0, 1]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数数组表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeft">双精度浮点数数组表示的 3x3 仿射矩阵（左矩阵）。</param>
        public PointD AffineTransformCopy(Matrix2D matrixLeft)
        {
            if (!Matrix2D.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(3, 3))
            {
                Matrix2D matrixRight = new Matrix2D(new double[1, 3]
                {
                    { _X, _Y, 1 }
                });

                Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

                if (!Matrix2D.IsNullOrNonMatrix(result))
                {
                    return new PointD(result[0, 0], result[0, 1]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数数组表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeftList">双精度浮点数数组表示的 3x3 仿射矩阵（左矩阵）列表。</param>
        public PointD AffineTransformCopy(List<Matrix2D> matrixLeftList)
        {
            if (matrixLeftList.Count > 0)
            {
                Matrix2D result = new Matrix2D(new double[1, 3]
                {
                    { _X, _Y, 1 }
                });

                for (int i = 0; i < matrixLeftList.Count; i++)
                {
                    Matrix2D matrixLeft = matrixLeftList[i];

                    bool flag = (!Matrix2D.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(3, 3));

                    if (flag)
                    {
                        result = Matrix2D.Multiply(matrixLeft, result);

                        flag = !Matrix2D.IsNullOrNonMatrix(result);
                    }

                    if (!flag)
                    {
                        return NaN;
                    }
                }

                return new PointD(result[0, 0], result[0, 1]);
            }

            return NaN;
        }

        /// <summary>
        /// 按 PointD 结构表示的 X 基向量、Y 基向量与偏移向量将此 PointD 结构进行逆仿射变换。
        /// </summary>
        /// <param name="ex">PointD 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD 结构表示的 Y 基向量。</param>
        /// <param name="offset">PointD 结构表示的偏移向量。</param>
        public void InverseAffineTransform(PointD ex, PointD ey, PointD offset)
        {
            if ((object)ex != null && (object)ey != null && (object)offset != null)
            {
                Matrix2D matrixLeft = new Matrix2D(new double[3, 3]
                {
                    { ex.X, ex.Y, 0 },
                    { ey.X, ey.Y, 0 },
                    { offset.X, offset.Y, 1 }
                });

                Matrix2D matrixRight = new Matrix2D(new double[1, 3]
                {
                    { _X, _Y, 1 }
                });

                Matrix2D result = Matrix2D.DivideLeft(matrixLeft, matrixRight);

                if (!Matrix2D.IsNullOrNonMatrix(result))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                }
            }
        }

        /// <summary>
        /// 按双精度浮点数数组表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeft">双精度浮点数数组表示的 3x3 仿射矩阵（左矩阵）。</param>
        public void InverseAffineTransform(Matrix2D matrixLeft)
        {
            if (!Matrix2D.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(3, 3))
            {
                Matrix2D matrixRight = new Matrix2D(new double[1, 3]
                {
                    { _X, _Y, 1 }
                });

                Matrix2D result = Matrix2D.DivideLeft(matrixLeft, matrixRight);

                if (!Matrix2D.IsNullOrNonMatrix(result))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                }
            }
        }

        /// <summary>
        /// 按双精度浮点数数组表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">双精度浮点数数组表示的 3x3 仿射矩阵（左矩阵）列表。</param>
        public void InverseAffineTransform(List<Matrix2D> matrixLeftList)
        {
            if (matrixLeftList.Count > 0)
            {
                Matrix2D result = new Matrix2D(new double[1, 3]
                {
                    { _X, _Y, 1 }
                });

                for (int i = matrixLeftList.Count - 1; i >= 0; i--)
                {
                    Matrix2D matrixLeft = matrixLeftList[i];

                    bool flag = (!Matrix2D.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(3, 3));

                    if (flag)
                    {
                        result = Matrix2D.DivideLeft(matrixLeft, result);

                        flag = !Matrix2D.IsNullOrNonMatrix(result);
                    }

                    if (!flag)
                    {
                        return;
                    }
                }

                _X = result[0, 0];
                _Y = result[0, 1];
            }
        }

        /// <summary>
        /// 返回按 PointD 结构表示的 X 基向量、Y 基向量与偏移向量将此 PointD 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="ex">PointD 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD 结构表示的 Y 基向量。</param>
        /// <param name="offset">PointD 结构表示的偏移向量。</param>
        public PointD InverseAffineTransformCopy(PointD ex, PointD ey, PointD offset)
        {
            if ((object)ex != null && (object)ey != null && (object)offset != null)
            {
                Matrix2D matrixLeft = new Matrix2D(new double[3, 3]
                {
                    { ex.X, ex.Y, 0 },
                    { ey.X, ey.Y, 0 },
                    { offset.X, offset.Y, 1 }
                });

                Matrix2D matrixRight = new Matrix2D(new double[1, 3]
                {
                    { _X, _Y, 1 }
                });

                Matrix2D result = Matrix2D.DivideLeft(matrixLeft, matrixRight);

                if (!Matrix2D.IsNullOrNonMatrix(result))
                {
                    return new PointD(result[0, 0], result[0, 1]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数数组表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeft">双精度浮点数数组表示的 3x3 仿射矩阵（左矩阵）。</param>
        public PointD InverseAffineTransformCopy(Matrix2D matrixLeft)
        {
            if (!Matrix2D.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(3, 3))
            {
                Matrix2D matrixRight = new Matrix2D(new double[1, 3]
                {
                    { _X, _Y, 1 }
                });

                Matrix2D result = Matrix2D.DivideLeft(matrixLeft, matrixRight);

                if (!Matrix2D.IsNullOrNonMatrix(result))
                {
                    return new PointD(result[0, 0], result[0, 1]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数数组表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeftList">双精度浮点数数组表示的 3x3 仿射矩阵（左矩阵）列表。</param>
        public PointD InverseAffineTransformCopy(List<Matrix2D> matrixLeftList)
        {
            if (matrixLeftList.Count > 0)
            {
                Matrix2D result = new Matrix2D(new double[1, 3]
                {
                    { _X, _Y, 1 }
                });

                for (int i = matrixLeftList.Count - 1; i >= 0; i--)
                {
                    Matrix2D matrixLeft = matrixLeftList[i];

                    bool flag = (!Matrix2D.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(3, 3));

                    if (flag)
                    {
                        result = Matrix2D.DivideLeft(matrixLeft, result);

                        flag = !Matrix2D.IsNullOrNonMatrix(result);
                    }

                    if (!flag)
                    {
                        return NaN;
                    }
                }

                return new PointD(result[0, 0], result[0, 1]);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回此 PointD 结构与指定的 PointD 结构之间的距离。
        /// </summary>
        /// <param name="pt">PointD 结构，表示另一个点。</param>
        public double DistanceFrom(PointD pt)
        {
            if ((object)pt != null)
            {
                double dx = _X - pt.X, dy = _Y - pt.Y;

                return Math.Sqrt(dx * dx + dy * dy);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回此 PointD 结构表示的向量与指定的 PointD 结构表示的向量之间的夹角（弧度）。
        /// </summary>
        /// <param name="pt">PointD 结构，表示另一个向量。</param>
        public double AngleFrom(PointD pt)
        {
            if ((object)pt != null)
            {
                if (_X == 0 && _Y == 0)
                {
                    _X = 1;
                }

                if (pt.X == 0 && pt.Y == 0)
                {
                    pt.X = 1;
                }

                double DotProduct = _X * pt.X + _Y * pt.Y;
                double ModProduct = VectorModule * pt.VectorModule;

                return Math.Acos(DotProduct / ModProduct);
            }

            return double.NaN;
        }

        //

        /// <summary>
        /// 返回将此 PointD 结构表示的直角坐标系坐标转换为极坐标系坐标的新实例。
        /// </summary>
        public PointD ToPolar()
        {
            return new PointD(VectorModule, VectorAngle);
        }

        /// <summary>
        /// 返回将此 PointD 结构表示的极坐标系坐标转换为直角坐标系坐标的新实例。
        /// </summary>
        public PointD ToCartesian()
        {
            return new PointD(_X * Math.Cos(_Y), _X * Math.Sin(_Y));
        }

        //

        /// <summary>
        /// 返回将此 PointD 结构转换为表示列向量的 Vector 的新实例。
        /// </summary>
        public Vector ToVectorColumn()
        {
            return new Vector(Vector.Type.ColumnVector, _X, _Y);
        }

        /// <summary>
        /// 返回将此 PointD 结构转换为表示行向量的 Vector 的新实例。
        /// </summary>
        public Vector ToVectorRow()
        {
            return new Vector(Vector.Type.RowVector, _X, _Y);
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 返回将 Point 结构转换为 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">Point 结构。</param>
        public static PointD FromPoint(Point pt)
        {
            if ((object)pt != null)
            {
                return new PointD(pt.X, pt.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointF 结构转换为 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointF 结构。</param>
        public static PointD FromPointF(PointF pt)
        {
            if ((object)pt != null)
            {
                return new PointD(pt.X, pt.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Size 结构转换为 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构。</param>
        public static PointD FromSize(Size sz)
        {
            if ((object)sz != null)
            {
                return new PointD(sz.Width, sz.Height);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 SizeF 结构转换为 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构。</param>
        public static PointD FromSizeF(SizeF sz)
        {
            if ((object)sz != null)
            {
                return new PointD(sz.Width, sz.Height);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Complex 结构转换为 PointD 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构。</param>
        public static PointD FromComplex(Complex comp)
        {
            if ((object)comp != null)
            {
                return new PointD(comp.Real, comp.Image);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回单位矩阵，表示不对 PointD 结构进行仿射变换的仿射矩阵（左矩阵）。
        /// </summary>
        public static Matrix2D IdentityMatrix()
        {
            return new Matrix2D(new double[3, 3]
            {
                { 1, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 1 }
            });
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的所有坐标偏移量将 PointD 结构平移指定的量的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="d">双精度浮点数表示的所有坐标偏移量。</param>
        public static Matrix2D OffsetMatrix(double d)
        {
            return new Matrix2D(new double[3, 3]
            {
                { 1, 0, 0 },
                { 0, 1, 0 },
                { d, d, 1 }
            });
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的 X 坐标偏移量与 Y 坐标偏移量将 PointD 结构平移指定的量的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标偏移量。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标偏移量。</param>
        public static Matrix2D OffsetMatrix(double dx, double dy)
        {
            return new Matrix2D(new double[3, 3]
            {
                { 1, 0, 0 },
                { 0, 1, 0 },
                { dx, dy, 1 }
            });
        }

        /// <summary>
        /// 返回表示按 PointD 结构将 PointD 结构平移指定的量的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="pt">PointD 结构，用于平移 PointD 结构。</param>
        public static Matrix2D OffsetMatrix(PointD pt)
        {
            if ((object)pt != null)
            {
                return new Matrix2D(new double[3, 3]
                {
                    { 1, 0, 0 },
                    { 0, 1, 0 },
                    { pt.X, pt.Y, 1 }
                });
            }

            return Matrix2D.NonMatrix;
        }

        /// <summary>
        /// 返回表示按 Point 结构将 PointD 结构平移指定的量的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="pt">Point 结构，用于平移 PointD 结构。</param>
        public static Matrix2D OffsetMatrix(Point pt)
        {
            if ((object)pt != null)
            {
                return new Matrix2D(new double[3, 3]
                {
                    { 1, 0, 0 },
                    { 0, 1, 0 },
                    { pt.X, pt.Y, 1 }
                });
            }

            return Matrix2D.NonMatrix;
        }

        /// <summary>
        /// 返回表示按 PointF 结构将 PointD 结构平移指定的量的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="pt">PointF 结构，用于平移 PointD 结构。</param>
        public static Matrix2D OffsetMatrix(PointF pt)
        {
            if ((object)pt != null)
            {
                return new Matrix2D(new double[3, 3]
                {
                    { 1, 0, 0 },
                    { 0, 1, 0 },
                    { pt.X, pt.Y, 1 }
                });
            }

            return Matrix2D.NonMatrix;
        }

        /// <summary>
        /// 返回表示按 Size 结构将 PointD 结构平移指定的量的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="sz">Size 结构，用于平移 PointD 结构。</param>
        public static Matrix2D OffsetMatrix(Size sz)
        {
            if ((object)sz != null)
            {
                return new Matrix2D(new double[3, 3]
                {
                    { 1, 0, 0 },
                    { 0, 1, 0 },
                    { sz.Width, sz.Height, 1 }
                });
            }

            return Matrix2D.NonMatrix;
        }

        /// <summary>
        /// 返回表示按 SizeF 结构将 PointD 结构平移指定的量的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="sz">SizeF 结构，用于平移 PointD 结构。</param>
        public static Matrix2D OffsetMatrix(SizeF sz)
        {
            if ((object)sz != null)
            {
                return new Matrix2D(new double[3, 3]
                {
                    { 1, 0, 0 },
                    { 0, 1, 0 },
                    { sz.Width, sz.Height, 1 }
                });
            }

            return Matrix2D.NonMatrix;
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的所有坐标缩放因子将 PointD 结构缩放指定的倍数的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="s">双精度浮点数表示的所有坐标缩放因子。</param>
        public static Matrix2D ScaleMatrix(double s)
        {
            return new Matrix2D(new double[3, 3]
            {
                { s, 0, 0 },
                { 0, s, 0 },
                { 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的 X 坐标缩放因子与 Y 坐标缩放因子将 PointD 结构缩放指定的倍数的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因子。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因子。</param>
        public static Matrix2D ScaleMatrix(double sx, double sy)
        {
            return new Matrix2D(new double[3, 3]
            {
                { sx, 0, 0 },
                { 0, sy, 0 },
                { 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回表示按 PointD 结构将 PointD 结构缩放指定的倍数的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="pt">PointD 结构，用于缩放 PointD 结构。</param>
        public static Matrix2D ScaleMatrix(PointD pt)
        {
            if ((object)pt != null)
            {
                return new Matrix2D(new double[3, 3]
                {
                    { pt.X, 0, 0 },
                    { 0, pt.Y, 0 },
                    { 0, 0, 1 }
                });
            }

            return Matrix2D.NonMatrix;
        }

        /// <summary>
        /// 返回表示按 Point 结构将 PointD 结构缩放指定的倍数的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="pt">Point 结构，用于缩放 PointD 结构。</param>
        public static Matrix2D ScaleMatrix(Point pt)
        {
            if ((object)pt != null)
            {
                return new Matrix2D(new double[3, 3]
                {
                    { pt.X, 0, 0 },
                    { 0, pt.Y, 0 },
                    { 0, 0, 1 }
                });
            }

            return Matrix2D.NonMatrix;
        }

        /// <summary>
        /// 返回表示按 PointF 结构将 PointD 结构缩放指定的倍数的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="pt">PointF 结构，用于缩放 PointD 结构。</param>
        public static Matrix2D ScaleMatrix(PointF pt)
        {
            if ((object)pt != null)
            {
                return new Matrix2D(new double[3, 3]
                {
                    { pt.X, 0, 0 },
                    { 0, pt.Y, 0 },
                    { 0, 0, 1 }
                });
            }

            return Matrix2D.NonMatrix;
        }

        /// <summary>
        /// 返回表示按 Size 结构将 PointD 结构缩放指定的倍数的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="sz">Size 结构，用于缩放 PointD 结构。</param>
        public static Matrix2D ScaleMatrix(Size sz)
        {
            if ((object)sz != null)
            {
                return new Matrix2D(new double[3, 3]
                {
                    { sz.Width, 0, 0 },
                    { 0, sz.Height, 0 },
                    { 0, 0, 1 }
                });
            }

            return Matrix2D.NonMatrix;
        }

        /// <summary>
        /// 返回表示按 SizeF 结构将 PointD 结构缩放指定的倍数的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="sz">SizeF 结构，用于缩放 PointD 结构。</param>
        public static Matrix2D ScaleMatrix(SizeF sz)
        {
            if ((object)sz != null)
            {
                return new Matrix2D(new double[3, 3]
                {
                    { sz.Width, 0, 0 },
                    { 0, sz.Height, 0 },
                    { 0, 0, 1 }
                });
            }

            return Matrix2D.NonMatrix;
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD 结构绕原点旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示 PointD 结构绕原点旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public static Matrix2D RotateMatrix(double angle)
        {
            return new Matrix2D(new double[3, 3]
            {
                { Math.Cos(angle), Math.Sin(angle), 0 },
                { -Math.Sin(angle), Math.Cos(angle), 0 },
                { 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD 结构绕指定的 PointD 结构旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示 PointD 结构绕指定的 PointD 结构旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <param name="pt">PointD 结构，表示旋转中心。</param>
        public static Matrix2D RotateMatrix(double angle, PointD pt)
        {
            if ((object)pt != null)
            {
                return new Matrix2D(new double[3, 3]
                {
                    { Math.Cos(angle), Math.Sin(angle), 0 },
                    { -Math.Sin(angle), Math.Cos(angle), 0 },
                    { -pt.X * Math.Cos(angle) + pt.Y * Math.Sin(angle) + pt.X, -pt.X * Math.Sin(angle) - pt.Y * Math.Cos(angle) + pt.Y, 1 }
                });
            }

            return Matrix2D.NonMatrix;
        }

        //

        /// <summary>
        /// 返回两个 PointD 结构之间的距离。
        /// </summary>
        /// <param name="left">PointD 结构，表示第一个点。</param>
        /// <param name="right">PointD 结构，表示第二个点。</param>
        public static double DistanceBetween(PointD left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                double dx = left.X - right.X, dy = left.Y - right.Y;

                return Math.Sqrt(dx * dx + dy * dy);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回 PointD 结构表示的两个向量之间的夹角（弧度）。
        /// </summary>
        /// <param name="left">PointD 结构，表示第一个向量。</param>
        /// <param name="right">PointD 结构，表示第二个向量。</param>
        public static double AngleBetween(PointD left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                if (left.X == 0 && left.Y == 0)
                {
                    left.X = 1;
                }

                if (right.X == 0 && right.Y == 0)
                {
                    right.X = 1;
                }

                double DotProduct = left.X * right.X + left.Y * right.Y;
                double ModProduct = left.VectorModule * right.VectorModule;

                return Math.Acos(DotProduct / ModProduct);
            }

            return double.NaN;
        }

        //

        /// <summary>
        /// 返回 PointD 结构表示的两个向量的数量积。
        /// </summary>
        /// <param name="left">PointD 结构，表示第一个向量。</param>
        /// <param name="right">PointD 结构，表示第二个向量。</param>
        public static double DotProduct(PointD left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                return (left.X * right.X + left.Y * right.Y);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回 PointD 结构表示的两个向量的向量积，该向量积为一个一维向量，其数值为 X∧Y 基向量的系数。
        /// </summary>
        /// <param name="left">PointD 结构，表示左向量。</param>
        /// <param name="right">PointD 结构，表示右向量。</param>
        public static Vector CrossProduct(PointD left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new Vector(Vector.Type.ColumnVector, left.X * right.Y - left.Y * right.X);
            }

            return Vector.NonVector;
        }

        //

        /// <summary>
        /// 返回将 PointD 结构的所有分量取绝对值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        public static PointD Abs(PointD pt)
        {
            if ((object)pt != null)
            {
                return new PointD(Math.Abs(pt.X), Math.Abs(pt.Y));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构的所有分量取符号数得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        public static PointD Sign(PointD pt)
        {
            if ((object)pt != null)
            {
                return new PointD(Math.Sign(pt.X), Math.Sign(pt.Y));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构的所有分量舍入到较大的整数值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        public static PointD Ceiling(PointD pt)
        {
            if ((object)pt != null)
            {
                return new PointD(Math.Ceiling(pt.X), Math.Ceiling(pt.Y));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构的所有分量舍入到较小的整数值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        public static PointD Floor(PointD pt)
        {
            if ((object)pt != null)
            {
                return new PointD(Math.Floor(pt.X), Math.Floor(pt.Y));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构的所有分量舍入到最接近的整数值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        public static PointD Round(PointD pt)
        {
            if ((object)pt != null)
            {
                return new PointD(Math.Round(pt.X), Math.Round(pt.Y));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构的所有分量截断小数部分取整得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        public static PointD Truncate(PointD pt)
        {
            if ((object)pt != null)
            {
                return new PointD(Math.Truncate(pt.X), Math.Truncate(pt.Y));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将两个 PointD 结构的所有分量分别取最大值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD 结构，用于比较的第二个结构。</param>
        public static PointD Max(PointD left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(Math.Max(left.X, right.X), Math.Max(left.Y, right.Y));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将两个 PointD 结构的所有分量分别取最小值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD 结构，用于比较的第二个结构。</param>
        public static PointD Min(PointD left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(Math.Min(left.X, right.X), Math.Min(left.Y, right.Y));
            }

            return NaN;
        }

        #endregion

        #region 基类方法

        /// <summary>
        /// 判断此 PointD 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is PointD))
            {
                return false;
            }

            return Equals((PointD)obj);
        }

        /// <summary>
        /// 返回此 PointD 结构的哈希代码。
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 PointD 结构转换为字符串。
        /// </summary>
        public override string ToString()
        {
            return string.Concat("{X=", _X, ", Y=", _Y, "}");
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 返回在 PointD 结构的所有分量前添加正号得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        public static PointD operator +(PointD pt)
        {
            if ((object)pt != null)
            {
                return new PointD(+pt.X, +pt.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回在 PointD 结构的所有分量前添加负号得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        public static PointD operator -(PointD pt)
        {
            if ((object)pt != null)
            {
                return new PointD(-pt.X, -pt.Y);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD 结构的所有分量与双精度浮点数相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被加数。</param>
        /// <param name="n">双精度浮点数，表示加数。</param>
        public static PointD operator +(PointD pt, double n)
        {
            if ((object)pt != null)
            {
                return new PointD(pt.X + n, pt.Y + n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD 结构的所有分量相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被加数。</param>
        /// <param name="pt">PointD 结构，表示加数。</param>
        public static PointD operator +(double n, PointD pt)
        {
            if ((object)pt != null)
            {
                return new PointD(n + pt.X, n + pt.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 PointD 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被加数。</param>
        /// <param name="right">PointD 结构，表示加数。</param>
        public static PointD operator +(PointD left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X + right.X, left.Y + right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 Point 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被加数。</param>
        /// <param name="right">Point 结构，表示加数。</param>
        public static PointD operator +(PointD left, Point right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X + right.X, left.Y + right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Point 结构与 PointD 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">Point 结构，表示被加数。</param>
        /// <param name="right">PointD 结构，表示加数。</param>
        public static PointD operator +(Point left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X + right.X, left.Y + right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 PointF 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被加数。</param>
        /// <param name="right">PointF 结构，表示加数。</param>
        public static PointD operator +(PointD left, PointF right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X + right.X, left.Y + right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointF 结构与 PointD 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointF 结构，表示被加数。</param>
        /// <param name="right">PointD 结构，表示加数。</param>
        public static PointD operator +(PointF left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X + right.X, left.Y + right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 Size 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被加数。</param>
        /// <param name="sz">Size 结构，表示加数。</param>
        public static PointD operator +(PointD pt, Size sz)
        {
            if ((object)pt != null && (object)sz != null)
            {
                return new PointD(pt.X + sz.Width, pt.Y + sz.Height);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Size 结构与 PointD 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，表示被加数。</param>
        /// <param name="pt">PointD 结构，表示加数。</param>
        public static PointD operator +(Size sz, PointD pt)
        {
            if ((object)sz != null && (object)pt != null)
            {
                return new PointD(sz.Width + pt.X, sz.Height + pt.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 SizeF 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被加数。</param>
        /// <param name="sz">SizeF 结构，表示加数。</param>
        public static PointD operator +(PointD pt, SizeF sz)
        {
            if ((object)pt != null && (object)sz != null)
            {
                return new PointD(pt.X + sz.Width, pt.Y + sz.Height);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 SizeF 结构与 PointD 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，表示被加数。</param>
        /// <param name="pt">PointD 结构，表示加数。</param>
        public static PointD operator +(SizeF sz, PointD pt)
        {
            if ((object)sz != null && (object)pt != null)
            {
                return new PointD(sz.Width + pt.X, sz.Height + pt.Y);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD 结构的所有分量与双精度浮点数相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被减数。</param>
        /// <param name="n">双精度浮点数，表示减数。</param>
        public static PointD operator -(PointD pt, double n)
        {
            if ((object)pt != null)
            {
                return new PointD(pt.X - n, pt.Y - n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD 结构的所有分量相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被减数。</param>
        /// <param name="pt">PointD 结构，表示减数。</param>
        public static PointD operator -(double n, PointD pt)
        {
            if ((object)pt != null)
            {
                return new PointD(n - pt.X, n - pt.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 PointD 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被减数。</param>
        /// <param name="right">PointD 结构，表示减数。</param>
        public static PointD operator -(PointD left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X - right.X, left.Y - right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 Point 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被减数。</param>
        /// <param name="right">Point 结构，表示减数。</param>
        public static PointD operator -(PointD left, Point right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X - right.X, left.Y - right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Point 结构与 PointD 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">Point 结构，表示被减数。</param>
        /// <param name="right">PointD 结构，表示减数。</param>
        public static PointD operator -(Point left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X - right.X, left.Y - right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 PointF 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被减数。</param>
        /// <param name="right">PointF 结构，表示减数。</param>
        public static PointD operator -(PointD left, PointF right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X - right.X, left.Y - right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointF 结构与 PointD 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointF 结构，表示被减数。</param>
        /// <param name="right">PointD 结构，表示减数。</param>
        public static PointD operator -(PointF left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X - right.X, left.Y - right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 Size 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被减数。</param>
        /// <param name="sz">Size 结构，表示减数。</param>
        public static PointD operator -(PointD pt, Size sz)
        {
            if ((object)pt != null && (object)sz != null)
            {
                return new PointD(pt.X - sz.Width, pt.Y - sz.Height);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Size 结构与 PointD 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，表示被减数。</param>
        /// <param name="pt">PointD 结构，表示减数。</param>
        public static PointD operator -(Size sz, PointD pt)
        {
            if ((object)sz != null && (object)pt != null)
            {
                return new PointD(sz.Width - pt.X, sz.Height - pt.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 SizeF 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被减数。</param>
        /// <param name="sz">SizeF 结构，表示减数。</param>
        public static PointD operator -(PointD pt, SizeF sz)
        {
            if ((object)pt != null && (object)sz != null)
            {
                return new PointD(pt.X - sz.Width, pt.Y - sz.Height);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 SizeF 结构与 PointD 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，表示被减数。</param>
        /// <param name="pt">PointD 结构，表示减数。</param>
        public static PointD operator -(SizeF sz, PointD pt)
        {
            if ((object)sz != null && (object)pt != null)
            {
                return new PointD(sz.Width - pt.X, sz.Height - pt.Y);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD 结构的所有分量与双精度浮点数相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被乘数。</param>
        /// <param name="n">双精度浮点数，表示乘数。</param>
        public static PointD operator *(PointD pt, double n)
        {
            if ((object)pt != null)
            {
                return new PointD(pt.X * n, pt.Y * n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD 结构的所有分量相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被乘数。</param>
        /// <param name="pt">PointD 结构，表示乘数。</param>
        public static PointD operator *(double n, PointD pt)
        {
            if ((object)pt != null)
            {
                return new PointD(n * pt.X, n * pt.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 PointD 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被乘数。</param>
        /// <param name="right">PointD 结构，表示乘数。</param>
        public static PointD operator *(PointD left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X * right.X, left.Y * right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 Point 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被乘数。</param>
        /// <param name="right">Point 结构，表示乘数。</param>
        public static PointD operator *(PointD left, Point right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X * right.X, left.Y * right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Point 结构与 PointD 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">Point 结构，表示被乘数。</param>
        /// <param name="right">PointD 结构，表示乘数。</param>
        public static PointD operator *(Point left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X * right.X, left.Y * right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 PointF 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被乘数。</param>
        /// <param name="right">PointF 结构，表示乘数。</param>
        public static PointD operator *(PointD left, PointF right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X * right.X, left.Y * right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointF 结构与 PointD 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointF 结构，表示被乘数。</param>
        /// <param name="right">PointD 结构，表示乘数。</param>
        public static PointD operator *(PointF left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X * right.X, left.Y * right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 Size 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被乘数。</param>
        /// <param name="sz">Size 结构，表示乘数。</param>
        public static PointD operator *(PointD pt, Size sz)
        {
            if ((object)pt != null && (object)sz != null)
            {
                return new PointD(pt.X * sz.Width, pt.Y * sz.Height);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Size 结构与 PointD 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，表示被乘数。</param>
        /// <param name="pt">PointD 结构，表示乘数。</param>
        public static PointD operator *(Size sz, PointD pt)
        {
            if ((object)sz != null && (object)pt != null)
            {
                return new PointD(sz.Width * pt.X, sz.Height * pt.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 SizeF 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被乘数。</param>
        /// <param name="sz">SizeF 结构，表示乘数。</param>
        public static PointD operator *(PointD pt, SizeF sz)
        {
            if ((object)pt != null && (object)sz != null)
            {
                return new PointD(pt.X * sz.Width, pt.Y * sz.Height);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 SizeF 结构与 PointD 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，表示被乘数。</param>
        /// <param name="pt">PointD 结构，表示乘数。</param>
        public static PointD operator *(SizeF sz, PointD pt)
        {
            if ((object)sz != null && (object)pt != null)
            {
                return new PointD(sz.Width * pt.X, sz.Height * pt.Y);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD 结构的所有分量与双精度浮点数相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被除数。</param>
        /// <param name="n">双精度浮点数，表示除数。</param>
        public static PointD operator /(PointD pt, double n)
        {
            if ((object)pt != null)
            {
                return new PointD(pt.X / n, pt.Y / n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD 结构的所有分量相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被除数。</param>
        /// <param name="pt">PointD 结构，表示除数。</param>
        public static PointD operator /(double n, PointD pt)
        {
            if ((object)pt != null)
            {
                return new PointD(n / pt.X, n / pt.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 PointD 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被除数。</param>
        /// <param name="right">PointD 结构，表示除数。</param>
        public static PointD operator /(PointD left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X / right.X, left.Y / right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 Point 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被除数。</param>
        /// <param name="right">Point 结构，表示除数。</param>
        public static PointD operator /(PointD left, Point right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X / right.X, left.Y / right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Point 结构与 PointD 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">Point 结构，表示被除数。</param>
        /// <param name="right">PointD 结构，表示除数。</param>
        public static PointD operator /(Point left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X / right.X, left.Y / right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 PointF 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被除数。</param>
        /// <param name="right">PointF 结构，表示除数。</param>
        public static PointD operator /(PointD left, PointF right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X / right.X, left.Y / right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointF 结构与 PointD 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointF 结构，表示被除数。</param>
        /// <param name="right">PointD 结构，表示除数。</param>
        public static PointD operator /(PointF left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD(left.X / right.X, left.Y / right.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 Size 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被除数。</param>
        /// <param name="sz">Size 结构，表示除数。</param>
        public static PointD operator /(PointD pt, Size sz)
        {
            if ((object)pt != null && (object)sz != null)
            {
                return new PointD(pt.X / sz.Width, pt.Y / sz.Height);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 Size 结构与 PointD 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，表示被除数。</param>
        /// <param name="pt">PointD 结构，表示除数。</param>
        public static PointD operator /(Size sz, PointD pt)
        {
            if ((object)sz != null && (object)pt != null)
            {
                return new PointD(sz.Width / pt.X, sz.Height / pt.Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD 结构与 SizeF 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被除数。</param>
        /// <param name="sz">SizeF 结构，表示除数。</param>
        public static PointD operator /(PointD pt, SizeF sz)
        {
            if ((object)pt != null && (object)sz != null)
            {
                return new PointD(pt.X / sz.Width, pt.Y / sz.Height);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 SizeF 结构与 PointD 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，表示被除数。</param>
        /// <param name="pt">PointD 结构，表示除数。</param>
        public static PointD operator /(SizeF sz, PointD pt)
        {
            if ((object)sz != null && (object)pt != null)
            {
                return new PointD(sz.Width / pt.X, sz.Height / pt.Y);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 判断 PointD 结构的两个实例是否相等。
        /// </summary>
        /// <param name="left">PointD 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD 结构，运算符右侧比较的结构。</param>
        public static bool operator ==(PointD left, PointD right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left.X == right.X && left.Y == right.Y);
        }

        /// <summary>
        /// 判断 PointD 结构的两个实例是否不相等。
        /// </summary>
        /// <param name="left">PointD 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD 结构，运算符右侧比较的结构。</param>
        public static bool operator !=(PointD left, PointD right)
        {
            if ((object)left == null || (object)right == null)
            {
                return true;
            }

            return (left.X != right.X || left.Y != right.Y);
        }

        /// <summary>
        /// 判断 PointD 结构的两个实例表示的向量的模平方是否前者小于后者。
        /// </summary>
        /// <param name="left">PointD 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD 结构，运算符右侧比较的结构。</param>
        public static bool operator <(PointD left, PointD right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared < right.VectorModuleSquared);
        }

        /// <summary>
        /// 判断 PointD 结构的两个实例表示的向量的模平方是否前者大于后者。
        /// </summary>
        /// <param name="left">PointD 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD 结构，运算符右侧比较的结构。</param>
        public static bool operator >(PointD left, PointD right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared > right.VectorModuleSquared);
        }

        /// <summary>
        /// 判断 PointD 结构的两个实例表示的向量的模平方是否前者小于或等于后者。
        /// </summary>
        /// <param name="left">PointD 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD 结构，运算符右侧比较的结构。</param>
        public static bool operator <=(PointD left, PointD right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared <= right.VectorModuleSquared);
        }

        /// <summary>
        /// 判断 PointD 结构的两个实例表示的向量的模平方是否前者大于或等于后者。
        /// </summary>
        /// <param name="left">PointD 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD 结构，运算符右侧比较的结构。</param>
        public static bool operator >=(PointD left, PointD right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared >= right.VectorModuleSquared);
        }

        #endregion
    }
}