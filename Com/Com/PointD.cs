/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.PointD
Version 18.9.28.2200

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Drawing;

namespace Com
{
    /// <summary>
    /// 以一组有序的双精度浮点数表示的二维直角坐标系坐标。
    /// </summary>
    public struct PointD : IEquatable<PointD>, IComparable, IComparable<PointD>, IEuclideanVector<PointD>, IAffine<PointD>
    {
        #region 私有成员与内部成员

        private double _X; // X 坐标。
        private double _Y; // Y 坐标。

        #endregion

        #region 常量与只读字段

        /// <summary>
        /// 表示零向量的 PointD 结构的实例。
        /// </summary>
        public static readonly PointD Zero = new PointD(0, 0);

        //

        /// <summary>
        /// 表示所有分量为非数字的 PointD 结构的实例。
        /// </summary>
        public static readonly PointD NaN = new PointD(double.NaN, double.NaN);

        //

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
        /// 获取此 PointD 结构的维度。
        /// </summary>
        public int Dimension
        {
            get
            {
                return 2;
            }
        }

        //

        /// <summary>
        /// 获取表示此 PointD 结构是否为空向量的布尔值。
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获取表示此 PointD 结构是否为零向量的布尔值。
        /// </summary>
        public bool IsZero
        {
            get
            {
                return (_X == Zero._X && _Y == Zero._Y);
            }
        }

        /// <summary>
        /// 获取表示此 PointD 结构是否只读的布尔值。
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获取表示此 PointD 结构是否具有固定的维度的布尔值。
        /// </summary>
        public bool IsFixedSize
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 获取表示此 PointD 结构是否包含非数字分量的布尔值。
        /// </summary>
        public bool IsNaN
        {
            get
            {
                return (double.IsNaN(_X) || double.IsNaN(_Y));
            }
        }

        /// <summary>
        /// 获取表示此 PointD 结构是否包含无穷大分量的布尔值。
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return ((!double.IsNaN(_X) && !double.IsNaN(_Y)) && (double.IsInfinity(_X) || double.IsInfinity(_Y)));
            }
        }

        /// <summary>
        /// 获取表示此 PointD 结构是否包含非数字或无穷大分量的布尔值。
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
        /// 获取此 PointD 结构表示的向量的模。
        /// </summary>
        public double Module
        {
            get
            {
                return Math.Sqrt(ModuleSquared);
            }
        }

        /// <summary>
        /// 获取此 PointD 结构表示的向量的模平方。
        /// </summary>
        public double ModuleSquared
        {
            get
            {
                return (_X * _X + _Y * _Y);
            }
        }

        //

        /// <summary>
        /// 获取此 PointD 结构表示的向量的相反向量。
        /// </summary>
        public PointD Negate
        {
            get
            {
                return new PointD(-_X, -_Y);
            }
        }

        /// <summary>
        /// 获取此 PointD 结构表示的向量的规范化向量。
        /// </summary>
        public PointD Normalize
        {
            get
            {
                double Mod = Module;

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

        //

        /// <summary>
        /// 获取此 PointD 结构表示的向量与 X 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleFromX
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }

                return AngleFrom(_X >= 0 ? Ex : -Ex);
            }
        }

        /// <summary>
        /// 获取此 PointD 结构表示的向量与 Y 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleFromY
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }

                return AngleFrom(_Y >= 0 ? Ey : -Ey);
            }
        }

        //

        /// <summary>
        /// 获取此 PointD 结构表示的向量的方位角。方位角是向量与 +X 轴之间的夹角（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。
        /// </summary>
        public double Azimuth
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

        #endregion

        #region 方法

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
            else if (object.ReferenceEquals(this, obj))
            {
                return true;
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

        //

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
            else if (object.ReferenceEquals(this, pt))
            {
                return true;
            }

            return (_X.Equals(pt._X) && _Y.Equals(pt._Y));
        }

        //

        /// <summary>
        /// 将此 PointD 结构与指定的对象进行次序比较。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is PointD))
            {
                return 1;
            }
            else if (object.ReferenceEquals(this, obj))
            {
                return 0;
            }

            return CompareTo((PointD)obj);
        }

        /// <summary>
        /// 将此 PointD 结构与指定的 PointD 结构进行次序比较。
        /// </summary>
        /// <param name="pt">用于比较的 PointD 结构。</param>
        public int CompareTo(PointD pt)
        {
            if ((object)pt == null)
            {
                return 1;
            }
            else if (object.ReferenceEquals(this, pt))
            {
                return 0;
            }

            return ModuleSquared.CompareTo(pt.ModuleSquared);
        }

        //

        /// <summary>
        /// 遍历此 PointD 结构的所有分量并返回第一个与指定值相等的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        public int IndexOf(double item)
        {
            if (_X.Equals(item))
            {
                return 0;
            }
            else if (_Y.Equals(item))
            {
                return 1;
            }

            return -1;
        }

        /// <summary>
        /// 遍历此 PointD 结构的所有分量并返回表示是否存在与指定值相等的分量的布尔值。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        public bool Contains(double item)
        {
            if (_X.Equals(item) || _Y.Equals(item))
            {
                return true;
            }

            return false;
        }

        //

        /// <summary>
        /// 将此 PointD 结构转换为双精度浮点数数组。
        /// </summary>
        public double[] ToArray()
        {
            return new double[2] { _X, _Y };
        }

        /// <summary>
        /// 将此 PointD 结构转换为双精度浮点数列表。
        /// </summary>
        public List<double> ToList()
        {
            return new List<double>(2) { _X, _Y };
        }

        //

        /// <summary>
        /// 返回将此 PointD 结构表示的直角坐标系坐标转换为极坐标系坐标的新实例。
        /// </summary>
        public PointD ToSpherical()
        {
            return new PointD(Module, Azimuth);
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
        /// 返回此 PointD 结构与指定的 PointD 结构之间的距离。
        /// </summary>
        /// <param name="pt">PointD 结构，表示另一个点。</param>
        public double DistanceFrom(PointD pt)
        {
            if ((object)pt != null)
            {
                double dx = _X - pt._X, dy = _Y - pt._Y;

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
                if (IsZero || pt.IsZero)
                {
                    return 0;
                }

                double DotProduct = _X * pt._X + _Y * pt._Y;

                return Math.Acos(DotProduct / Module / pt.Module);
            }

            return double.NaN;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的位移将此 PointD 结构的所有分量平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public void Offset(double d)
        {
            _X += d;
            _Y += d;
        }

        /// <summary>
        /// 按双精度浮点数表示的 X 坐标位移与 Y 坐标位移将此 PointD 结构平移指定的量。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
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
                _X += pt._X;
                _Y += pt._Y;
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
        /// 返回按双精度浮点数表示的位移将此 PointD 结构的副本的所有分量平移指定的量的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public PointD OffsetCopy(double d)
        {
            return new PointD(_X + d, _Y + d);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标位移与 Y 坐标位移将此 PointD 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
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
                return new PointD(_X + pt._X, _Y + pt._Y);
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
        /// 按双精度浮点数表示的缩放因数将此 PointD 结构的所有分量缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public void Scale(double s)
        {
            _X *= s;
            _Y *= s;
        }

        /// <summary>
        /// 按双精度浮点数表示的 X 坐标缩放因数与 Y 坐标缩放因数将此 PointD 结构缩放指定的倍数。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
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
                _X *= pt._X;
                _Y *= pt._Y;
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
        /// 返回按双精度浮点数表示的缩放因数将此 PointD 结构的副本的所有分量缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public PointD ScaleCopy(double s)
        {
            return new PointD(_X * s, _Y * s);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标缩放因数与 Y 坐标缩放因数将此 PointD 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
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
                return new PointD(_X * pt._X, _Y * pt._Y);
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
        /// 将此 PointD 结构的指定的基向量方向的分量翻转。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        public void Reflect(int index)
        {
            switch (index)
            {
                case 0: _X = -_X; break;
                case 1: _Y = -_Y; break;
            }
        }

        /// <summary>
        /// 将此 PointD 结构在 X 轴的分量翻转。
        /// </summary>
        public void ReflectX()
        {
            _X = -_X;
        }

        /// <summary>
        /// 将此 PointD 结构在 Y 轴的分量翻转。
        /// </summary>
        public void ReflectY()
        {
            _Y = -_Y;
        }

        /// <summary>
        /// 返回将此 PointD 结构的副本的由指定的基向量方向的分量翻转的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        public PointD ReflectCopy(int index)
        {
            switch (index)
            {
                case 0: return new PointD(-_X, _Y);
                case 1: return new PointD(_X, -_Y);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将此 PointD 结构的副本在 X 轴的分量翻转的新实例。
        /// </summary>
        public PointD ReflectXCopy()
        {
            return new PointD(-_X, _Y);
        }

        /// <summary>
        /// 返回将此 PointD 结构的副本在 Y 轴的分量翻转的新实例。
        /// </summary>
        public PointD ReflectYCopy()
        {
            return new PointD(_X, -_Y);
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD 结构剪切指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向平行且同方向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向垂直且共平面的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构的副本沿平行于索引 index1 指定的基向量且与之同方向以及垂直于 index2 指定的基向量且与之共平面的方向剪切的角度（弧度）。</param>
        public void Shear(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().ShearCopy(index1, index2, angle);

            if (!Vector.IsNullOrEmpty(result) && result.Dimension == 2)
            {
                _X = result[0];
                _Y = result[1];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD 结构向 +X 轴剪切指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构向 +X 轴剪切的角度（弧度）。</param>
        public void ShearX(double angle)
        {
            _X += _Y * Math.Tan(angle);
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD 结构向 +Y 轴剪切指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构向 +Y 轴剪切的角度（弧度）。</param>
        public void ShearY(double angle)
        {
            _Y += _X * Math.Tan(angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构的副本剪切指定的角度的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向平行且同方向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向垂直且共平面的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构的副本沿平行于索引 index1 指定的基向量且与之同方向以及垂直于 index2 指定的基向量且与之共平面的方向剪切的角度（弧度）。</param>
        public PointD ShearCopy(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().ShearCopy(index1, index2, angle);

            if (!Vector.IsNullOrEmpty(result) && result.Dimension == 2)
            {
                return new PointD(result[0], result[1]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构的副本向 +X 轴剪切指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构的副本向 +X 轴剪切的角度（弧度）。</param>
        public PointD ShearXCopy(double angle)
        {
            return new PointD(_X + _Y * Math.Tan(angle), _Y);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构的副本向 +Y 轴剪切指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构的副本向 +Y 轴剪切的角度（弧度）。</param>
        public PointD ShearYCopy(double angle)
        {
            return new PointD(_Y + _X * Math.Tan(angle), _X);
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD 结构旋转指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        public void Rotate(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().RotateCopy(index1, index2, angle);

            if (!Vector.IsNullOrEmpty(result) && result.Dimension == 2)
            {
                _X = result[0];
                _Y = result[1];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD 结构绕原点旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构绕原点旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public void Rotate(double angle)
        {
            double __X = _X, __Y = _Y;

            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            _X = __X * CosA - __Y * SinA;
            _Y = __X * SinA + __Y * CosA;
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD 结构绕指定的 PointD 结构旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构绕指定的 PointD 结构旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <param name="pt">PointD 结构，表示旋转中心。</param>
        public void Rotate(double angle, PointD pt)
        {
            if ((object)pt != null)
            {
                double __X = _X, __Y = _Y;

                double CosA = Math.Cos(angle);
                double SinA = Math.Sin(angle);

                _X = (__X - pt._X) * CosA - (__Y - pt._Y) * SinA + pt._X;
                _Y = (__X - pt._X) * SinA + (__Y - pt._Y) * CosA + pt._Y;
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构的副本旋转指定的角度的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构的副本绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        public PointD RotateCopy(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().RotateCopy(index1, index2, angle);

            if (!Vector.IsNullOrEmpty(result) && result.Dimension == 2)
            {
                return new PointD(result[0], result[1]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构的副本绕原点旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构的副本绕原点旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public PointD RotateCopy(double angle)
        {
            PointD result = new PointD();

            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            result._X = _X * CosA - _Y * SinA;
            result._Y = _X * SinA + _Y * CosA;

            return result;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构的副本绕指定的 PointD 结构旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构的副本绕指定的 PointD 结构旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <param name="pt">PointD 结构，表示旋转中心。</param>
        public PointD RotateCopy(double angle, PointD pt)
        {
            if ((object)pt != null)
            {
                PointD result = new PointD();

                double CosA = Math.Cos(angle);
                double SinA = Math.Sin(angle);

                result._X = (_X - pt._X) * CosA - (_Y - pt._Y) * SinA + pt._X;
                result._Y = (_X - pt._X) * SinA + (_Y - pt._Y) * CosA + pt._Y;

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
                Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[3, 3]
                {
                    { ex._X, ex._Y, 0 },
                    { ey._X, ey._Y, 0 },
                    { offset._X, offset._Y, 1 }
                });

                Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 2)
                {
                    _X = result[0];
                    _Y = result[1];
                }
            }
        }

        /// <summary>
        /// 按 Matrix 对象表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 3x3 仿射矩阵（左矩阵）。</param>
        public void AffineTransform(Matrix matrixLeft)
        {
            if (!Matrix.IsNullOrEmpty(matrixLeft) && matrixLeft.Size == new Size(3, 3))
            {
                Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 2)
                {
                    _X = result[0];
                    _Y = result[1];
                }
            }
        }

        /// <summary>
        /// 按 Matrix 对象列表表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）列表。</param>
        public void AffineTransform(List<Matrix> matrixLeftList)
        {
            if (!InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                Vector result = ToColumnVector().AffineTransformCopy(matrixLeftList);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 2)
                {
                    _X = result[0];
                    _Y = result[1];
                }
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
                Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[3, 3]
                {
                    { ex._X, ex._Y, 0 },
                    { ey._X, ey._Y, 0 },
                    { offset._X, offset._Y, 1 }
                });

                Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 2)
                {
                    return new PointD(result[0], result[1]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）。</param>
        public PointD AffineTransformCopy(Matrix matrixLeft)
        {
            if (!Matrix.IsNullOrEmpty(matrixLeft) && matrixLeft.Size == new Size(3, 3))
            {
                Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 2)
                {
                    return new PointD(result[0], result[1]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）列表。</param>
        public PointD AffineTransformCopy(List<Matrix> matrixLeftList)
        {
            if (!InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                Vector result = ToColumnVector().AffineTransformCopy(matrixLeftList);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 2)
                {
                    return new PointD(result[0], result[1]);
                }
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
                Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[3, 3]
                {
                    { ex._X, ex._Y, 0 },
                    { ey._X, ey._Y, 0 },
                    { offset._X, offset._Y, 1 }
                });

                Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 2)
                {
                    _X = result[0];
                    _Y = result[1];
                }
            }
        }

        /// <summary>
        /// 按 Matrix 对象列表表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）。</param>
        public void InverseAffineTransform(Matrix matrixLeft)
        {
            if (!Matrix.IsNullOrEmpty(matrixLeft) && matrixLeft.Size == new Size(3, 3))
            {
                Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 2)
                {
                    _X = result[0];
                    _Y = result[1];
                }
            }
        }

        /// <summary>
        /// 按 Matrix 对象列表表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）列表。</param>
        public void InverseAffineTransform(List<Matrix> matrixLeftList)
        {
            if (!InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeftList);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 2)
                {
                    _X = result[0];
                    _Y = result[1];
                }
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
                Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[3, 3]
                {
                    { ex._X, ex._Y, 0 },
                    { ey._X, ey._Y, 0 },
                    { offset._X, offset._Y, 1 }
                });

                Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 2)
                {
                    return new PointD(result[0], result[1]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）。</param>
        public PointD InverseAffineTransformCopy(Matrix matrixLeft)
        {
            if (!Matrix.IsNullOrEmpty(matrixLeft) && matrixLeft.Size == new Size(3, 3))
            {
                Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 2)
                {
                    return new PointD(result[0], result[1]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）列表。</param>
        public PointD InverseAffineTransformCopy(List<Matrix> matrixLeftList)
        {
            if (!InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeftList);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 2)
                {
                    return new PointD(result[0], result[1]);
                }
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将此 PointD 结构转换为列向量的 Vector 的新实例。
        /// </summary>
        public Vector ToColumnVector()
        {
            return Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, _X, _Y);
        }

        /// <summary>
        /// 返回将此 PointD 结构转换为行向量的 Vector 的新实例。
        /// </summary>
        public Vector ToRowVector()
        {
            return Vector.UnsafeCreateInstance(Vector.Type.RowVector, _X, _Y);
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

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断两个 PointD 结构是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 PointD 结构。</param>
        /// <param name="right">用于比较的第二个 PointD 结构。</param>
        public static bool Equals(PointD left, PointD right)
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
        /// 比较两个 PointD 结构的次序。
        /// </summary>
        /// <param name="left">用于比较的第一个 PointD 结构。</param>
        /// <param name="right">用于比较的第二个 PointD 结构。</param>
        public static int Compare(PointD left, PointD right)
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
        /// 返回单位矩阵，表示不对 PointD 结构进行仿射变换的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        public static Matrix IdentityMatrix()
        {
            return Matrix.Identity(3);
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的位移将 PointD 结构的所有分量平移指定的量的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public static Matrix OffsetMatrix(double d)
        {
            return Vector.OffsetMatrix(Vector.Type.ColumnVector, 2, d);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的 X 坐标位移与 Y 坐标位移将 PointD 结构平移指定的量的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
        public static Matrix OffsetMatrix(double dx, double dy)
        {
            return Vector.OffsetMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, dx, dy));
        }

        /// <summary>
        /// 返回表示按 PointD 结构将 PointD 结构平移指定的量的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于平移 PointD 结构。</param>
        public static Matrix OffsetMatrix(PointD pt)
        {
            if ((object)pt != null)
            {
                return Vector.OffsetMatrix(pt.ToColumnVector());
            }

            return Matrix.Empty;
        }

        /// <summary>
        /// 返回表示按 Point 结构将 PointD 结构平移指定的量的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">Point 结构，用于平移 PointD 结构。</param>
        public static Matrix OffsetMatrix(Point pt)
        {
            if ((object)pt != null)
            {
                return Vector.OffsetMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, pt.X, pt.Y));
            }

            return Matrix.Empty;
        }

        /// <summary>
        /// 返回表示按 PointF 结构将 PointD 结构平移指定的量的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointF 结构，用于平移 PointD 结构。</param>
        public static Matrix OffsetMatrix(PointF pt)
        {
            if ((object)pt != null)
            {
                return Vector.OffsetMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, pt.X, pt.Y));
            }

            return Matrix.Empty;
        }

        /// <summary>
        /// 返回表示按 Size 结构将 PointD 结构平移指定的量的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，用于平移 PointD 结构。</param>
        public static Matrix OffsetMatrix(Size sz)
        {
            if ((object)sz != null)
            {
                return Vector.OffsetMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, sz.Width, sz.Height));
            }

            return Matrix.Empty;
        }

        /// <summary>
        /// 返回表示按 SizeF 结构将 PointD 结构平移指定的量的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，用于平移 PointD 结构。</param>
        public static Matrix OffsetMatrix(SizeF sz)
        {
            if ((object)sz != null)
            {
                return Vector.OffsetMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, sz.Width, sz.Height));
            }

            return Matrix.Empty;
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的缩放因数将 PointD 结构的所有分量缩放指定的倍数的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public static Matrix ScaleMatrix(double s)
        {
            return Vector.ScaleMatrix(Vector.Type.ColumnVector, 2, s);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的 X 坐标缩放因数与 Y 坐标缩放因数将 PointD 结构缩放指定的倍数的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
        public static Matrix ScaleMatrix(double sx, double sy)
        {
            return Vector.ScaleMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, sx, sy));
        }

        /// <summary>
        /// 返回表示按 PointD 结构将 PointD 结构缩放指定的倍数的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于缩放 PointD 结构。</param>
        public static Matrix ScaleMatrix(PointD pt)
        {
            if ((object)pt != null)
            {
                return Vector.ScaleMatrix(pt.ToColumnVector());
            }

            return Matrix.Empty;
        }

        /// <summary>
        /// 返回表示按 Point 结构将 PointD 结构缩放指定的倍数的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">Point 结构，用于缩放 PointD 结构。</param>
        public static Matrix ScaleMatrix(Point pt)
        {
            if ((object)pt != null)
            {
                return Vector.ScaleMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, pt.X, pt.Y));
            }

            return Matrix.Empty;
        }

        /// <summary>
        /// 返回表示按 PointF 结构将 PointD 结构缩放指定的倍数的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointF 结构，用于缩放 PointD 结构。</param>
        public static Matrix ScaleMatrix(PointF pt)
        {
            if ((object)pt != null)
            {
                return Vector.ScaleMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, pt.X, pt.Y));
            }

            return Matrix.Empty;
        }

        /// <summary>
        /// 返回表示按 Size 结构将 PointD 结构缩放指定的倍数的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，用于缩放 PointD 结构。</param>
        public static Matrix ScaleMatrix(Size sz)
        {
            if ((object)sz != null)
            {
                return Vector.ScaleMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, sz.Width, sz.Height));
            }

            return Matrix.Empty;
        }

        /// <summary>
        /// 返回表示按 SizeF 结构将 PointD 结构缩放指定的倍数的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，用于缩放 PointD 结构。</param>
        public static Matrix ScaleMatrix(SizeF sz)
        {
            if ((object)sz != null)
            {
                return Vector.ScaleMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, sz.Width, sz.Height));
            }

            return Matrix.Empty;
        }

        //

        /// <summary>
        /// 返回表示用于翻转 PointD 结构的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        public static Matrix ReflectMatrix(int index)
        {
            return Vector.ReflectMatrix(Vector.Type.ColumnVector, 2, index);
        }

        /// <summary>
        /// 返回将 PointD 结构在 X 轴的分量翻转的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        public static Matrix ReflectXMatrix()
        {
            return ReflectMatrix(0);
        }

        /// <summary>
        /// 返回将 PointD 结构在 Y 轴的分量翻转的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        public static Matrix ReflectYMatrix()
        {
            return ReflectMatrix(1);
        }

        //

        /// <summary>
        /// 返回表示用于剪切 PointD 结构的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示 PointD 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        public static Matrix ShearMatrix(int index1, int index2, double angle)
        {
            return Vector.ShearMatrix(Vector.Type.ColumnVector, 2, index1, index2, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD 结构向 +X 轴剪切指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构向 +X 轴剪切的角度（弧度）。</param>
        public static Matrix ShearXMatrix(double angle)
        {
            return ShearMatrix(0, 1, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD 结构向 +Y 轴剪切指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构向 +Y 轴剪切的角度（弧度）。</param>
        public static Matrix ShearYMatrix(double angle)
        {
            return ShearMatrix(1, 0, angle);
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD 结构绕原点旋转指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD 结构绕原点旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public static Matrix RotateMatrix(double angle)
        {
            return Vector.RotateMatrix(Vector.Type.ColumnVector, 2, 0, 1, angle);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD 结构绕指定的 PointD 结构旋转指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD 结构绕指定的 PointD 结构旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <param name="pt">PointD 结构，表示旋转中心。</param>
        public static Matrix RotateMatrix(double angle, PointD pt)
        {
            if ((object)pt != null)
            {
                double CosA = Math.Cos(angle);
                double SinA = Math.Sin(angle);

                return Matrix.UnsafeCreateInstance(new double[3, 3]
                {
                    { CosA, SinA, 0 },
                    { -SinA, CosA, 0 },
                    { -pt._X * CosA + pt._Y * SinA + pt._X, -pt._X * SinA - pt._Y * CosA + pt._Y, 1 }
                });
            }

            return Matrix.Empty;
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
                double dx = left._X - right._X, dy = left._Y - right._Y;

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
                if (left.IsZero || right.IsZero)
                {
                    return 0;
                }

                double DotProduct = left._X * right._X + left._Y * right._Y;

                return Math.Acos(DotProduct / left.Module / right.Module);
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
                return (left._X * right._X + left._Y * right._Y);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回 PointD 结构表示的两个向量的向量积。该向量积为一个一维向量，其数值为 X∧Y 基向量的系数。
        /// </summary>
        /// <param name="left">PointD 结构，表示左向量。</param>
        /// <param name="right">PointD 结构，表示右向量。</param>
        public static Vector CrossProduct(PointD left, PointD right)
        {
            if ((object)left != null && (object)right != null)
            {
                return Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, left._X * right._Y - left._Y * right._X);
            }

            return Vector.Empty;
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
                return new PointD(Math.Abs(pt._X), Math.Abs(pt._Y));
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
                return new PointD(Math.Sign(pt._X), Math.Sign(pt._Y));
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
                return new PointD(Math.Ceiling(pt._X), Math.Ceiling(pt._Y));
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
                return new PointD(Math.Floor(pt._X), Math.Floor(pt._Y));
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
                return new PointD(Math.Round(pt._X), Math.Round(pt._Y));
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
                return new PointD(Math.Truncate(pt._X), Math.Truncate(pt._Y));
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
                return new PointD(Math.Max(left._X, right._X), Math.Max(left._Y, right._Y));
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
                return new PointD(Math.Min(left._X, right._X), Math.Min(left._Y, right._Y));
            }

            return NaN;
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 PointD 结构是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD 结构。</param>
        public static bool operator ==(PointD left, PointD right)
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

            return (left._X == right._X && left._Y == right._Y);
        }

        /// <summary>
        /// 判断两个 PointD 结构是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD 结构。</param>
        public static bool operator !=(PointD left, PointD right)
        {
            if ((object)left == null && (object)right == null)
            {
                return false;
            }
            else if ((object)left == null || (object)right == null)
            {
                return true;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return false;
            }

            return (left._X != right._X || left._Y != right._Y);
        }

        /// <summary>
        /// 判断两个 PointD 结构表示的向量的模平方是否前者小于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD 结构。</param>
        public static bool operator <(PointD left, PointD right)
        {
            if ((object)left == null || (object)right == null || object.ReferenceEquals(left, right))
            {
                return false;
            }

            return (left.ModuleSquared < right.ModuleSquared);
        }

        /// <summary>
        /// 判断两个 PointD 结构表示的向量的模平方是否前者大于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD 结构。</param>
        public static bool operator >(PointD left, PointD right)
        {
            if ((object)left == null || (object)right == null || object.ReferenceEquals(left, right))
            {
                return false;
            }

            return (left.ModuleSquared > right.ModuleSquared);
        }

        /// <summary>
        /// 判断两个 PointD 结构表示的向量的模平方是否前者小于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD 结构。</param>
        public static bool operator <=(PointD left, PointD right)
        {
            if ((object)left == null || (object)right == null || object.ReferenceEquals(left, right))
            {
                return false;
            }

            return (left.ModuleSquared <= right.ModuleSquared);
        }

        /// <summary>
        /// 判断两个 PointD 结构表示的向量的模平方是否前者大于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD 结构。</param>
        public static bool operator >=(PointD left, PointD right)
        {
            if ((object)left == null || (object)right == null || object.ReferenceEquals(left, right))
            {
                return false;
            }

            return (left.ModuleSquared >= right.ModuleSquared);
        }

        //

        /// <summary>
        /// 返回在 PointD 结构的所有分量前添加正号得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        public static PointD operator +(PointD pt)
        {
            if ((object)pt != null)
            {
                return new PointD(+pt._X, +pt._Y);
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
                return new PointD(-pt._X, -pt._Y);
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
                return new PointD(pt._X + n, pt._Y + n);
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
                return new PointD(n + pt._X, n + pt._Y);
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
                return new PointD(left._X + right._X, left._Y + right._Y);
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
                return new PointD(left._X + right.X, left._Y + right.Y);
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
                return new PointD(left.X + right._X, left.Y + right._Y);
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
                return new PointD(left._X + right.X, left._Y + right.Y);
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
                return new PointD(left.X + right._X, left.Y + right._Y);
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
                return new PointD(pt._X + sz.Width, pt._Y + sz.Height);
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
                return new PointD(sz.Width + pt._X, sz.Height + pt._Y);
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
                return new PointD(pt._X + sz.Width, pt._Y + sz.Height);
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
                return new PointD(sz.Width + pt._X, sz.Height + pt._Y);
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
                return new PointD(pt._X - n, pt._Y - n);
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
                return new PointD(n - pt._X, n - pt._Y);
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
                return new PointD(left._X - right._X, left._Y - right._Y);
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
                return new PointD(left._X - right.X, left._Y - right.Y);
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
                return new PointD(left.X - right._X, left.Y - right._Y);
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
                return new PointD(left._X - right.X, left._Y - right.Y);
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
                return new PointD(left.X - right._X, left.Y - right._Y);
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
                return new PointD(pt._X - sz.Width, pt._Y - sz.Height);
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
                return new PointD(sz.Width - pt._X, sz.Height - pt._Y);
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
                return new PointD(pt._X - sz.Width, pt._Y - sz.Height);
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
                return new PointD(sz.Width - pt._X, sz.Height - pt._Y);
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
                return new PointD(pt._X * n, pt._Y * n);
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
                return new PointD(n * pt._X, n * pt._Y);
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
                return new PointD(left._X * right._X, left._Y * right._Y);
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
                return new PointD(left._X * right.X, left._Y * right.Y);
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
                return new PointD(left.X * right._X, left.Y * right._Y);
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
                return new PointD(left._X * right.X, left._Y * right.Y);
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
                return new PointD(left.X * right._X, left.Y * right._Y);
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
                return new PointD(pt._X * sz.Width, pt._Y * sz.Height);
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
                return new PointD(sz.Width * pt._X, sz.Height * pt._Y);
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
                return new PointD(pt._X * sz.Width, pt._Y * sz.Height);
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
                return new PointD(sz.Width * pt._X, sz.Height * pt._Y);
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
                return new PointD(pt._X / n, pt._Y / n);
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
                return new PointD(n / pt._X, n / pt._Y);
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
                return new PointD(left._X / right._X, left._Y / right._Y);
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
                return new PointD(left._X / right.X, left._Y / right.Y);
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
                return new PointD(left.X / right._X, left.Y / right._Y);
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
                return new PointD(left._X / right.X, left._Y / right.Y);
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
                return new PointD(left.X / right._X, left.Y / right._Y);
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
                return new PointD(pt._X / sz.Width, pt._Y / sz.Height);
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
                return new PointD(sz.Width / pt._X, sz.Height / pt._Y);
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
                return new PointD(pt._X / sz.Width, pt._Y / sz.Height);
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
                return new PointD(sz.Width / pt._X, sz.Height / pt._Y);
            }

            return NaN;
        }

        #endregion

        #region 显式接口成员实现

        #region Com.IVector<T>

        int IVector<double>.Size
        {
            get
            {
                return Dimension;
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        int IVector<double>.Capacity
        {
            get
            {
                return Dimension;
            }
        }

        void IVector<double>.Trim()
        {
            throw new NotSupportedException();
        }

        #endregion

        #region System.Collections.IList

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                this[index] = (double)value;
            }
        }

        int IList.Add(object item)
        {
            throw new NotSupportedException();
        }

        void IList.Clear()
        {
            this = default(PointD);
        }

        bool IList.Contains(object item)
        {
            if (item != null && item is double)
            {
                return Contains((double)item);
            }

            return false;
        }

        int IList.IndexOf(object item)
        {
            if (item != null && item is double)
            {
                return IndexOf((double)item);
            }

            return -1;
        }

        void IList.Insert(int index, object item)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(object item)
        {
            throw new NotSupportedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region System.Collections.ICollection

        int ICollection.Count
        {
            get
            {
                return Dimension;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return this;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array != null && array.Rank == 1 && array.Length >= Dimension)
            {
                ToArray().CopyTo(array, index);
            }
        }

        #endregion

        #region System.Collections.IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        private sealed class Enumerator : IEnumerator // 实现 System.Collections.IEnumerator 的迭代器。
        {
            private PointD _Pt;
            private int _Index;

            internal Enumerator(PointD pt)
            {
                _Pt = pt;
                _Index = -1;
            }

            object IEnumerator.Current
            {
                get
                {
                    if (_Index >= 0 && _Index < _Pt.Dimension)
                    {
                        return _Pt[_Index];
                    }

                    return null;
                }
            }

            bool IEnumerator.MoveNext()
            {
                if (_Index < _Pt.Dimension - 1)
                {
                    _Index++;

                    return true;
                }

                return false;
            }

            void IEnumerator.Reset()
            {
                _Index = -1;
            }
        }

        #endregion

        #region System.Collections.Generic.IList<T>

        void IList<double>.Insert(int index, double item)
        {
            throw new NotSupportedException();
        }

        void IList<double>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region System.Collections.Generic.ICollection<T>

        int ICollection<double>.Count
        {
            get
            {
                return Dimension;
            }
        }

        void ICollection<double>.Add(double item)
        {
            throw new NotSupportedException();
        }

        void ICollection<double>.Clear()
        {
            this = default(PointD);
        }

        void ICollection<double>.CopyTo(double[] array, int index)
        {
            if (array != null && array.Length >= Dimension)
            {
                ToArray().CopyTo(array, index);
            }
        }

        bool ICollection<double>.Remove(double item)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region System.Collections.Generic.IEnumerable<out T>

        IEnumerator<double> IEnumerable<double>.GetEnumerator()
        {
            return new GenericEnumerator(this);
        }

        private sealed class GenericEnumerator : IEnumerator<double> // 实现 System.Collections.Generic.IEnumerator<out T> 的迭代器。
        {
            private PointD _Pt;
            private int _Index;

            internal GenericEnumerator(PointD pt)
            {
                _Pt = pt;
                _Index = -1;
            }

            void IDisposable.Dispose()
            {

            }

            object IEnumerator.Current
            {
                get
                {
                    if (_Index >= 0 && _Index < _Pt.Dimension)
                    {
                        return _Pt[_Index];
                    }

                    return null;
                }
            }

            bool IEnumerator.MoveNext()
            {
                if (_Index < _Pt.Dimension - 1)
                {
                    _Index++;

                    return true;
                }

                return false;
            }

            void IEnumerator.Reset()
            {
                _Index = -1;
            }

            double IEnumerator<double>.Current
            {
                get
                {
                    if (_Index >= 0 && _Index < _Pt.Dimension)
                    {
                        return _Pt[_Index];
                    }

                    return double.NaN;
                }
            }
        }

        #endregion

        #endregion
    }
}