/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

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
            _X = pt.X;
            _Y = pt.Y;
        }

        /// <summary>
        /// 使用 PointF 结构初始化 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointF 结构。</param>
        public PointD(PointF pt)
        {
            _X = pt.X;
            _Y = pt.Y;
        }

        /// <summary>
        /// 使用 Size 结构初始化 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构。</param>
        public PointD(Size sz)
        {
            _X = sz.Width;
            _Y = sz.Height;
        }

        /// <summary>
        /// 使用 SizeF 结构初始化 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构。</param>
        public PointD(SizeF sz)
        {
            _X = sz.Width;
            _Y = sz.Height;
        }

        /// <summary>
        /// 使用 Complex 结构初始化 PointD 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构。</param>
        public PointD(Complex comp)
        {
            _X = comp.Real;
            _Y = comp.Imaginary;
        }

        #endregion

        #region 字段

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
                    default: throw new IndexOutOfRangeException();
                }
            }

            set
            {
                switch (index)
                {
                    case 0: _X = value; break;
                    case 1: _Y = value; break;
                    default: throw new IndexOutOfRangeException();
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
                return (_X == 0 && _Y == 0);
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
                return (!IsNaN && (double.IsInfinity(_X) || double.IsInfinity(_Y)));
            }
        }

        /// <summary>
        /// 获取表示此 PointD 结构是否包含非数字或无穷大分量的布尔值。
        /// </summary>
        public bool IsNaNOrInfinity
        {
            get
            {
                return (InternalMethod.IsNaNOrInfinity(_X) || InternalMethod.IsNaNOrInfinity(_Y));
            }
        }

        //

        /// <summary>
        /// 获取此 PointD 结构的模。
        /// </summary>
        public double Module
        {
            get
            {
                double AbsX = Math.Abs(_X);
                double AbsY = Math.Abs(_Y);

                double AbsMax = Math.Max(AbsX, AbsY);

                if (AbsMax == 0)
                {
                    return 0;
                }
                else
                {
                    AbsX /= AbsMax;
                    AbsY /= AbsMax;

                    double SqrSum = AbsX * AbsX + AbsY * AbsY;

                    return (AbsMax * Math.Sqrt(SqrSum));
                }
            }
        }

        /// <summary>
        /// 获取此 PointD 结构的模的平方。
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
        /// 获取此 PointD 结构的相反向量。
        /// </summary>
        public PointD Negate
        {
            get
            {
                return new PointD(-_X, -_Y);
            }
        }

        /// <summary>
        /// 获取此 PointD 结构的规范化向量。
        /// </summary>
        public PointD Normalize
        {
            get
            {
                double Mod = Module;

                if (Mod <= 0)
                {
                    return Ex;
                }
                else
                {
                    return new PointD(_X / Mod, _Y / Mod);
                }
            }
        }

        //

        /// <summary>
        /// 获取此 PointD 结构与 X 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleFromX
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }
                else
                {
                    return AngleFrom(_X >= 0 ? Ex : -Ex);
                }
            }
        }

        /// <summary>
        /// 获取此 PointD 结构与 Y 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleFromY
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }
                else
                {
                    return AngleFrom(_Y >= 0 ? Ey : -Ey);
                }
            }
        }

        //

        /// <summary>
        /// 获取此 PointD 结构的方位角。方位角是 PointD 结构与 +X 轴之间的夹角（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。
        /// </summary>
        public double Azimuth
        {
            get
            {
                return Math.Atan2(_Y, _X);
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 PointD 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>布尔值，表示此 PointD 结构是否与指定的对象相等。</returns>
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
            else
            {
                return Equals((PointD)obj);
            }
        }

        /// <summary>
        /// 返回此 PointD 结构的哈希代码。
        /// </summary>
        /// <returns>32 位整数，表示此 PointD 结构的哈希代码。</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 PointD 结构转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 PointD 结构的字符串形式。</returns>
        public override string ToString()
        {
            return string.Concat("{X=", _X, ", Y=", _Y, "}");
        }

        //

        /// <summary>
        /// 判断此 PointD 结构是否与指定的 PointD 结构相等。
        /// </summary>
        /// <param name="pt">用于比较的 PointD 结构。</param>
        /// <returns>布尔值，表示此 PointD 结构是否与指定的 PointD 结构相等。</returns>
        public bool Equals(PointD pt)
        {
            return (_X.Equals(pt._X) && _Y.Equals(pt._Y));
        }

        //

        /// <summary>
        /// 将此 PointD 结构与指定的对象进行次序比较。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>32 位整数，表示将此 PointD 结构与指定的对象进行次序比较得到的结果。</returns>
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
            else
            {
                return CompareTo((PointD)obj);
            }
        }

        /// <summary>
        /// 将此 PointD 结构与指定的 PointD 结构进行次序比较。
        /// </summary>
        /// <param name="pt">用于比较的 PointD 结构。</param>
        /// <returns>32 位整数，表示将此 PointD 结构与指定的 PointD 结构进行次序比较得到的结果。</returns>
        public int CompareTo(PointD pt)
        {
            for (int i = 0; i < Dimension; i++)
            {
                int result = this[i].CompareTo(pt[i]);

                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        //

        /// <summary>
        /// 遍历此 PointD 结构的所有分量并返回第一个与指定值相等的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的索引。</returns>
        public int IndexOf(double item)
        {
            return Array.IndexOf(ToArray(), item, 0, Dimension);
        }

        /// <summary>
        /// 从指定的索引开始遍历此 PointD 结构的所有分量并返回第一个与指定值相等的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的索引。</returns>
        public int IndexOf(double item, int startIndex)
        {
            if (startIndex < 0 || startIndex >= Dimension)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return Array.IndexOf(ToArray(), item, startIndex, Dimension - startIndex);
        }

        /// <summary>
        /// 从指定的索引开始遍历此 PointD 结构指定数量的分量并返回第一个与指定值相等的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <param name="count">遍历的分量数量。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的索引。</returns>
        public int IndexOf(double item, int startIndex, int count)
        {
            if ((startIndex < 0 || startIndex >= Dimension) || count <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            count = Math.Min(Dimension - startIndex, count);

            return Array.IndexOf(ToArray(), item, startIndex, count);
        }

        /// <summary>
        /// 逆序遍历此 PointD 结构的所有分量并返回第一个与指定值相等的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的索引。</returns>
        public int LastIndexOf(double item)
        {
            return Array.LastIndexOf(ToArray(), item, Dimension - 1, Dimension);
        }

        /// <summary>
        /// 从指定的索引开始逆序遍历此 PointD 结构的所有分量并返回第一个与指定值相等的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的索引。</returns>
        public int LastIndexOf(double item, int startIndex)
        {
            if (startIndex < 0 || startIndex >= Dimension)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return Array.LastIndexOf(ToArray(), item, startIndex, startIndex + 1);
        }

        /// <summary>
        /// 从指定的索引开始逆序遍历此 PointD 结构指定数量的分量并返回第一个与指定值相等的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <param name="count">遍历的分量数量。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的索引。</returns>
        public int LastIndexOf(double item, int startIndex, int count)
        {
            if ((startIndex < 0 || startIndex >= Dimension) || count <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            count = Math.Min(startIndex + 1, count);

            return Array.LastIndexOf(ToArray(), item, startIndex, count);
        }

        /// <summary>
        /// 遍历此 PointD 结构的所有分量并返回表示是否存在与指定值相等的分量的布尔值。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>布尔值，表示是否存在与指定值相等的分量。</returns>
        public bool Contains(double item)
        {
            if (_X.Equals(item) || _Y.Equals(item))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //

        /// <summary>
        /// 将此 PointD 结构转换为双精度浮点数数组。
        /// </summary>
        /// <returns>双精度浮点数数组，数组元素表示此 PointD 结构的分量。</returns>
        public double[] ToArray()
        {
            return new double[2] { _X, _Y };
        }

        /// <summary>
        /// 将此 PointD 结构转换为双精度浮点数列表。
        /// </summary>
        /// <returns>双精度浮点数列表，列表元素表示此 PointD 结构的分量。</returns>
        public List<double> ToList()
        {
            return new List<double>(2) { _X, _Y };
        }

        //

        /// <summary>
        /// 返回将此 PointD 结构表示的直角坐标系坐标转换为极坐标系坐标的 PointD 结构的新实例。
        /// </summary>
        /// <returns>PointD 结构，表示将此 PointD 结构表示的直角坐标系坐标转换为极坐标系坐标得到的结果。</returns>
        public PointD ToSpherical()
        {
            return new PointD(Module, Azimuth);
        }

        /// <summary>
        /// 返回将此 PointD 结构表示的极坐标系坐标转换为直角坐标系坐标的 PointD 结构的新实例。
        /// </summary>
        /// <returns>PointD 结构，表示将此 PointD 结构表示的极坐标系坐标转换为直角坐标系坐标得到的结果。</returns>
        public PointD ToCartesian()
        {
            return new PointD(_X * Math.Cos(_Y), _X * Math.Sin(_Y));
        }

        //

        /// <summary>
        /// 返回此 PointD 结构与指定的 PointD 结构之间的距离。
        /// </summary>
        /// <param name="pt">PointD 结构，表示另一个点。</param>
        /// <returns>双精度浮点数，表示此 PointD 结构与指定的 PointD 结构之间的距离。</returns>
        public double DistanceFrom(PointD pt)
        {
            double AbsDx = Math.Abs(_X - pt._X);
            double AbsDy = Math.Abs(_Y - pt._Y);

            double AbsMax = Math.Max(AbsDx, AbsDy);

            if (AbsMax == 0)
            {
                return 0;
            }
            else
            {
                AbsDx /= AbsMax;
                AbsDy /= AbsMax;

                double SqrSum = AbsDx * AbsDx + AbsDy * AbsDy;

                return (AbsMax * Math.Sqrt(SqrSum));
            }
        }

        /// <summary>
        /// 返回此 PointD 结构与指定的 PointD 结构之间的夹角（弧度）。
        /// </summary>
        /// <param name="pt">PointD 结构，表示另一个向量。</param>
        /// <returns>双精度浮点数，表示此 PointD 结构与指定的 PointD 结构之间的夹角（弧度）。</returns>
        public double AngleFrom(PointD pt)
        {
            if (IsZero || pt.IsZero)
            {
                return 0;
            }
            else
            {
                double ModProduct = Module * pt.Module;
                double CosA = _X * pt._X / ModProduct + _Y * pt._Y / ModProduct;

                return Math.Acos(CosA);
            }
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
            _X += pt._X;
            _Y += pt._Y;
        }

        /// <summary>
        /// 按 Point 结构将此 PointD 结构平移指定的量。
        /// </summary>
        /// <param name="pt">Point 结构，用于平移此 PointD 结构。</param>
        public void Offset(Point pt)
        {
            _X += pt.X;
            _Y += pt.Y;
        }

        /// <summary>
        /// 按 PointF 结构将此 PointD 结构平移指定的量。
        /// </summary>
        /// <param name="pt">PointF 结构，用于平移此 PointD 结构。</param>
        public void Offset(PointF pt)
        {
            _X += pt.X;
            _Y += pt.Y;
        }

        /// <summary>
        /// 按 Size 结构将此 PointD 结构平移指定的量。
        /// </summary>
        /// <param name="sz">Size 结构，用于平移此 PointD 结构。</param>
        public void Offset(Size sz)
        {
            _X += sz.Width;
            _Y += sz.Height;
        }

        /// <summary>
        /// 按 SizeF 结构将此 PointD 结构平移指定的量。
        /// </summary>
        /// <param name="sz">SizeF 结构，用于平移此 PointD 结构。</param>
        public void Offset(SizeF sz)
        {
            _X += sz.Width;
            _Y += sz.Height;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的位移将此 PointD 结构的所有分量平移指定的量的 PointD 结构的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的位移将此 PointD 结构的所有分量平移指定的量得到的结果。</returns>
        public PointD OffsetCopy(double d)
        {
            return new PointD(_X + d, _Y + d);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标位移与 Y 坐标位移将此 PointD 结构平移指定的量的 PointD 结构的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的 X 坐标位移与 Y 坐标位移将此 PointD 结构平移指定的量得到的结果。</returns>
        public PointD OffsetCopy(double dx, double dy)
        {
            return new PointD(_X + dx, _Y + dy);
        }

        /// <summary>
        /// 返回按 PointD 结构将此 PointD 结构平移指定的量的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于平移此 PointD 结构。</param>
        /// <returns>PointD 结构，表示按 PointD 结构将此 PointD 结构平移指定的量得到的结果。</returns>
        public PointD OffsetCopy(PointD pt)
        {
            return new PointD(_X + pt._X, _Y + pt._Y);
        }

        /// <summary>
        /// 返回按 Point 结构将此 PointD 结构平移指定的量的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">Point 结构，用于平移此 PointD 结构。</param>
        /// <returns>PointD 结构，表示按 Point 结构将此 PointD 结构平移指定的量得到的结果。</returns>
        public PointD OffsetCopy(Point pt)
        {
            return new PointD(_X + pt.X, _Y + pt.Y);
        }

        /// <summary>
        /// 返回按 PointF 结构将此 PointD 结构平移指定的量的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointF 结构，用于平移此 PointD 结构。</param>
        /// <returns>PointD 结构，表示按 PointF 结构将此 PointD 结构平移指定的量得到的结果。</returns>
        public PointD OffsetCopy(PointF pt)
        {
            return new PointD(_X + pt.X, _Y + pt.Y);
        }

        /// <summary>
        /// 返回按 Size 结构将此 PointD 结构平移指定的量的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，用于平移此 PointD 结构。</param>
        /// <returns>PointD 结构，表示按 Size 结构将此 PointD 结构平移指定的量得到的结果。</returns>
        public PointD OffsetCopy(Size sz)
        {
            return new PointD(_X + sz.Width, _Y + sz.Height);
        }

        /// <summary>
        /// 返回按 SizeF 结构将此 PointD 结构平移指定的量的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，用于平移此 PointD 结构。</param>
        /// <returns>PointD 结构，表示按 SizeF 结构将此 PointD 结构平移指定的量得到的结果。</returns>
        public PointD OffsetCopy(SizeF sz)
        {
            return new PointD(_X + sz.Width, _Y + sz.Height);
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
            _X *= pt._X;
            _Y *= pt._Y;
        }

        /// <summary>
        /// 按 Point 结构将此 PointD 结构缩放指定的倍数。
        /// </summary>
        /// <param name="pt">Point 结构，用于缩放此 PointD 结构。</param>
        public void Scale(Point pt)
        {
            _X *= pt.X;
            _Y *= pt.Y;
        }

        /// <summary>
        /// 按 PointF 结构将此 PointD 结构缩放指定的倍数。
        /// </summary>
        /// <param name="pt">PointF 结构，用于缩放此 PointD 结构。</param>
        public void Scale(PointF pt)
        {
            _X *= pt.X;
            _Y *= pt.Y;
        }

        /// <summary>
        /// 按 Size 结构将此 PointD 结构缩放指定的倍数。
        /// </summary>
        /// <param name="sz">Size 结构，用于缩放此 PointD 结构。</param>
        public void Scale(Size sz)
        {
            _X *= sz.Width;
            _Y *= sz.Height;
        }

        /// <summary>
        /// 按 SizeF 结构将此 PointD 结构缩放指定的倍数。
        /// </summary>
        /// <param name="sz">SizeF 结构，用于缩放此 PointD 结构。</param>
        public void Scale(SizeF sz)
        {
            _X *= sz.Width;
            _Y *= sz.Height;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数将此 PointD 结构的所有分量缩放指定的倍数的 PointD 结构的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的缩放因数将此 PointD 结构的所有分量缩放指定的倍数得到的结果。</returns>
        public PointD ScaleCopy(double s)
        {
            return new PointD(_X * s, _Y * s);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标缩放因数与 Y 坐标缩放因数将此 PointD 结构缩放指定的倍数的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的 X 坐标缩放因数与 Y 坐标缩放因数将此 PointD 结构缩放指定的倍数得到的结果。</returns>
        public PointD ScaleCopy(double sx, double sy)
        {
            return new PointD(_X * sx, _Y * sy);
        }

        /// <summary>
        /// 返回按 PointD 结构将此 PointD 结构缩放指定的倍数的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于缩放此 PointD 结构。</param>
        /// <returns>PointD 结构，表示按 PointD 结构将此 PointD 结构缩放指定的倍数得到的结果。</returns>
        public PointD ScaleCopy(PointD pt)
        {
            return new PointD(_X * pt._X, _Y * pt._Y);
        }

        /// <summary>
        /// 返回按 Point 结构将此 PointD 结构缩放指定的倍数的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">Point 结构，用于缩放此 PointD 结构。</param>
        /// <returns>PointD 结构，表示按 Point 结构将此 PointD 结构缩放指定的倍数得到的结果。</returns>
        public PointD ScaleCopy(Point pt)
        {
            return new PointD(_X * pt.X, _Y * pt.Y);
        }

        /// <summary>
        /// 返回按 PointF 结构将此 PointD 结构缩放指定的倍数的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointF 结构，用于缩放此 PointD 结构。</param>
        /// <returns>PointD 结构，表示按 PointF 结构将此 PointD 结构缩放指定的倍数得到的结果。</returns>
        public PointD ScaleCopy(PointF pt)
        {
            return new PointD(_X * pt.X, _Y * pt.Y);
        }

        /// <summary>
        /// 返回按 Size 结构将此 PointD 结构缩放指定的倍数的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，用于缩放此 PointD 结构。</param>
        /// <returns>PointD 结构，表示按 Size 结构将此 PointD 结构缩放指定的倍数得到的结果。</returns>
        public PointD ScaleCopy(Size sz)
        {
            return new PointD(_X * sz.Width, _Y * sz.Height);
        }

        /// <summary>
        /// 返回按 SizeF 结构将此 PointD 结构缩放指定的倍数的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，用于缩放此 PointD 结构。</param>
        /// <returns>PointD 结构，表示按 SizeF 结构将此 PointD 结构缩放指定的倍数得到的结果。</returns>
        public PointD ScaleCopy(SizeF sz)
        {
            return new PointD(_X * sz.Width, _Y * sz.Height);
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
                default: throw new ArgumentOutOfRangeException();
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
        /// 返回将此 PointD 结构的由指定的基向量方向的分量翻转的 PointD 结构的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        /// <returns>PointD 结构，表示将此 PointD 结构的由指定的基向量方向的分量翻转得到的结果。</returns>
        public PointD ReflectCopy(int index)
        {
            switch (index)
            {
                case 0: return new PointD(-_X, _Y);
                case 1: return new PointD(_X, -_Y);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 返回将此 PointD 结构在 X 轴的分量翻转的 PointD 结构的新实例。
        /// </summary>
        /// <returns>PointD 结构，表示将此 PointD 结构在 X 轴的分量翻转得到的结果。</returns>
        public PointD ReflectXCopy()
        {
            return new PointD(-_X, _Y);
        }

        /// <summary>
        /// 返回将此 PointD 结构在 Y 轴的分量翻转的 PointD 结构的新实例。
        /// </summary>
        /// <returns>PointD 结构，表示将此 PointD 结构在 Y 轴的分量翻转得到的结果。</returns>
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
        /// <param name="angle">双精度浮点数，表示此 PointD 结构沿平行于索引 index1 指定的基向量且与之同方向以及垂直于 index2 指定的基向量且与之共平面的方向剪切的角度（弧度）。</param>
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
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构剪切指定的角度的 PointD 结构的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向平行且同方向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向垂直且共平面的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构沿平行于索引 index1 指定的基向量且与之同方向以及垂直于 index2 指定的基向量且与之共平面的方向剪切的角度（弧度）。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的弧度将此 PointD 结构剪切指定的角度得到的结果。</returns>
        public PointD ShearCopy(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().ShearCopy(index1, index2, angle);

            if (Vector.IsNullOrEmpty(result) || result.Dimension != 2)
            {
                return NaN;
            }
            else
            {
                return new PointD(result[0], result[1]);
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构向 +X 轴剪切指定的角度的 PointD 结构的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构向 +X 轴剪切的角度（弧度）。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的弧度将此 PointD 结构向 +X 轴剪切指定的角度得到的结果。</returns>
        public PointD ShearXCopy(double angle)
        {
            return new PointD(_X + _Y * Math.Tan(angle), _Y);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构向 +Y 轴剪切指定的角度的 PointD 结构的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构向 +Y 轴剪切的角度（弧度）。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的弧度将此 PointD 结构向 +Y 轴剪切指定的角度得到的结果。</returns>
        public PointD ShearYCopy(double angle)
        {
            return new PointD(_X, _Y + _X * Math.Tan(angle));
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
            double __X = _X, __Y = _Y;

            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            _X = (__X - pt._X) * CosA - (__Y - pt._Y) * SinA + pt._X;
            _Y = (__X - pt._X) * SinA + (__Y - pt._Y) * CosA + pt._Y;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构旋转指定的角度的 PointD 结构的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的弧度将此 PointD 结构旋转指定的角度得到的结果。</returns>
        public PointD RotateCopy(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().RotateCopy(index1, index2, angle);

            if (Vector.IsNullOrEmpty(result) || result.Dimension != 2)
            {
                return NaN;
            }
            else
            {
                return new PointD(result[0], result[1]);
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构绕原点旋转指定的角度的 PointD 结构的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构绕原点旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的弧度将此 PointD 结构绕原点旋转指定的角度得到的结果。</returns>
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
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构绕指定的 PointD 结构旋转指定的角度的 PointD 结构的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构绕指定的 PointD 结构旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <param name="pt">PointD 结构，表示旋转中心。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的弧度将此 PointD 结构绕指定的 PointD 结构旋转指定的角度得到的结果。</returns>
        public PointD RotateCopy(double angle, PointD pt)
        {
            PointD result = new PointD();

            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            result._X = (_X - pt._X) * CosA - (_Y - pt._Y) * SinA + pt._X;
            result._Y = (_X - pt._X) * SinA + (_Y - pt._Y) * CosA + pt._Y;

            return result;
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
        /// 返回按 PointD 结构表示的 X 基向量、Y 基向量与偏移向量将此 PointD 结构进行仿射变换的 PointD 结构的新实例。
        /// </summary>
        /// <param name="ex">PointD 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD 结构表示的 Y 基向量。</param>
        /// <param name="offset">PointD 结构表示的偏移向量。</param>
        /// <returns>PointD 结构，表示按 PointD 结构表示的 X 基向量、Y 基向量与偏移向量将此 PointD 结构进行仿射变换得到的结果。</returns>
        public PointD AffineTransformCopy(PointD ex, PointD ey, PointD offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[3, 3]
            {
                { ex._X, ex._Y, 0 },
                { ey._X, ey._Y, 0 },
                { offset._X, offset._Y, 1 }
            });

            Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

            if (Vector.IsNullOrEmpty(result) || result.Dimension != 2)
            {
                return NaN;
            }
            else
            {
                return new PointD(result[0], result[1]);
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行仿射变换的 PointD 结构的新实例。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）。</param>
        /// <returns>PointD 结构，表示按 Matrix 对象表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行仿射变换得到的结果。</returns>
        public PointD AffineTransformCopy(Matrix matrixLeft)
        {
            if (Matrix.IsNullOrEmpty(matrixLeft) || matrixLeft.Size != new Size(3, 3))
            {
                return NaN;
            }
            else
            {
                Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

                if (Vector.IsNullOrEmpty(result) || result.Dimension != 2)
                {
                    return NaN;
                }
                else
                {
                    return new PointD(result[0], result[1]);
                }
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构进行仿射变换的 PointD 结构的新实例。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）列表。</param>
        /// <returns>PointD 结构，表示按 Matrix 对象列表表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构进行仿射变换得到的结果。</returns>
        public PointD AffineTransformCopy(List<Matrix> matrixLeftList)
        {
            if (InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                return NaN;
            }
            else
            {
                Vector result = ToColumnVector().AffineTransformCopy(matrixLeftList);

                if (Vector.IsNullOrEmpty(result) || result.Dimension != 2)
                {
                    return NaN;
                }
                else
                {
                    return new PointD(result[0], result[1]);
                }
            }
        }

        /// <summary>
        /// 按 PointD 结构表示的 X 基向量、Y 基向量与偏移向量将此 PointD 结构进行逆仿射变换。
        /// </summary>
        /// <param name="ex">PointD 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD 结构表示的 Y 基向量。</param>
        /// <param name="offset">PointD 结构表示的偏移向量。</param>
        public void InverseAffineTransform(PointD ex, PointD ey, PointD offset)
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

        /// <summary>
        /// 按 Matrix 对象表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行逆仿射变换。
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
        /// 返回按 PointD 结构表示的 X 基向量、Y 基向量与偏移向量将此 PointD 结构进行逆仿射变换的 PointD 结构的新实例。
        /// </summary>
        /// <param name="ex">PointD 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD 结构表示的 Y 基向量。</param>
        /// <param name="offset">PointD 结构表示的偏移向量。</param>
        /// <returns>PointD 结构，表示按 PointD 结构表示的 X 基向量、Y 基向量与偏移向量将此 PointD 结构进行逆仿射变换得到的结果。</returns>
        public PointD InverseAffineTransformCopy(PointD ex, PointD ey, PointD offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[3, 3]
            {
                { ex._X, ex._Y, 0 },
                { ey._X, ey._Y, 0 },
                { offset._X, offset._Y, 1 }
            });

            Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

            if (Vector.IsNullOrEmpty(result) || result.Dimension != 2)
            {
                return NaN;
            }
            else
            {
                return new PointD(result[0], result[1]);
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行逆仿射变换的 PointD 结构的新实例。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）。</param>
        /// <returns>PointD 结构，表示按 Matrix 对象表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行逆仿射变换得到的结果。</returns>
        public PointD InverseAffineTransformCopy(Matrix matrixLeft)
        {
            if (Matrix.IsNullOrEmpty(matrixLeft) || matrixLeft.Size != new Size(3, 3))
            {
                return NaN;
            }
            else
            {
                Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

                if (Vector.IsNullOrEmpty(result) || result.Dimension != 2)
                {
                    return NaN;
                }
                else
                {
                    return new PointD(result[0], result[1]);
                }
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构进行逆仿射变换的 PointD 结构的新实例。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）列表。</param>
        /// <returns>PointD 结构，表示按 Matrix 对象列表表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构进行逆仿射变换得到的结果。</returns>
        public PointD InverseAffineTransformCopy(List<Matrix> matrixLeftList)
        {
            if (InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                return NaN;
            }
            else
            {
                Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeftList);

                if (Vector.IsNullOrEmpty(result) || result.Dimension != 2)
                {
                    return NaN;
                }
                else
                {
                    return new PointD(result[0], result[1]);
                }
            }
        }

        //

        /// <summary>
        /// 返回将此 PointD 结构转换为列向量的 Vector 的新实例。
        /// </summary>
        /// <returns>Vector 对象，表示将此 PointD 结构转换为列向量得到的结果。</returns>
        public Vector ToColumnVector()
        {
            return Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, _X, _Y);
        }

        /// <summary>
        /// 返回将此 PointD 结构转换为行向量的 Vector 的新实例。
        /// </summary>
        /// <returns>Vector 对象，表示将此 PointD 结构转换为行向量得到的结果。</returns>
        public Vector ToRowVector()
        {
            return Vector.UnsafeCreateInstance(Vector.Type.RowVector, _X, _Y);
        }

        //

        /// <summary>
        /// 返回将此 PointD 结构转换为 Point 结构的新实例。
        /// </summary>
        /// <returns>Point 结构，表示转换的结果。</returns>
        public Point ToPoint()
        {
            if ((_X < int.MinValue || _X > int.MaxValue) || (_Y < int.MinValue || _Y > int.MaxValue))
            {
                throw new OverflowException();
            }

            //

            return new Point((int)_X, (int)_Y);
        }

        /// <summary>
        /// 返回将此 PointD 结构转换为 PointF 结构的新实例。
        /// </summary>
        /// <returns>PointF 结构，表示转换的结果。</returns>
        public PointF ToPointF()
        {
            return new PointF((float)_X, (float)_Y);
        }

        /// <summary>
        /// 返回将此 PointD 结构转换为 Size 结构的新实例。
        /// </summary>
        /// <returns>Size 结构，表示转换的结果。</returns>
        public Size ToSize()
        {
            if ((_X < int.MinValue || _X > int.MaxValue) || (_Y < int.MinValue || _Y > int.MaxValue))
            {
                throw new OverflowException();
            }

            //

            return new Size((int)_X, (int)_Y);
        }

        /// <summary>
        /// 返回将此 PointD 结构转换为 SizeF 结构的新实例。
        /// </summary>
        /// <returns>SizeF 结构，表示转换的结果。</returns>
        public SizeF ToSizeF()
        {
            return new SizeF((float)_X, (float)_Y);
        }

        /// <summary>
        /// 返回将此 PointD 结构转换为 Complex 结构的新实例。
        /// </summary>
        /// <returns>Complex 结构，表示转换的结果。</returns>
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
        /// <returns>布尔值，表示两个 PointD 结构是否相等。</returns>
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
            else
            {
                return left.Equals(right);
            }
        }

        //

        /// <summary>
        /// 比较两个 PointD 结构的次序。
        /// </summary>
        /// <param name="left">用于比较的第一个 PointD 结构。</param>
        /// <param name="right">用于比较的第二个 PointD 结构。</param>
        /// <returns>32 位整数，表示将两个 PointD 结构进行次序比较得到的结果。</returns>
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
            else
            {
                return left.CompareTo(right);
            }
        }

        //

        /// <summary>
        /// 返回将 Point 结构转换为 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">Point 结构。</param>
        /// <returns>PointD 结构，表示转换的结果。</returns>
        public static PointD FromPoint(Point pt)
        {
            return new PointD(pt);
        }

        /// <summary>
        /// 返回将 PointF 结构转换为 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointF 结构。</param>
        /// <returns>PointD 结构，表示转换的结果。</returns>
        public static PointD FromPointF(PointF pt)
        {
            return new PointD(pt);
        }

        /// <summary>
        /// 返回将 Size 结构转换为 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构。</param>
        /// <returns>PointD 结构，表示转换的结果。</returns>
        public static PointD FromSize(Size sz)
        {
            return new PointD(sz);
        }

        /// <summary>
        /// 返回将 SizeF 结构转换为 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构。</param>
        /// <returns>PointD 结构，表示转换的结果。</returns>
        public static PointD FromSizeF(SizeF sz)
        {
            return new PointD(sz);
        }

        /// <summary>
        /// 返回将 Complex 结构转换为 PointD 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构。</param>
        /// <returns>PointD 结构，表示转换的结果。</returns>
        public static PointD FromComplex(Complex comp)
        {
            return new PointD(comp);
        }

        //

        /// <summary>
        /// 返回单位矩阵，表示不对 PointD 结构进行仿射变换的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <returns>Matrix 对象，表示不对 PointD 结构进行仿射变换的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix IdentityMatrix()
        {
            return Matrix.Identity(3);
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的位移将 PointD 结构的所有分量平移指定的量的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的位移将 PointD 结构的所有分量平移指定的量的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(double d)
        {
            return Vector.OffsetMatrix(Vector.Type.ColumnVector, 2, d);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的 X 坐标位移与 Y 坐标位移将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的 X 坐标位移与 Y 坐标位移将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(double dx, double dy)
        {
            return Vector.OffsetMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, dx, dy));
        }

        /// <summary>
        /// 返回表示按 PointD 结构将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于平移 PointD 结构。</param>
        /// <returns>Matrix 对象，表示按 PointD 结构将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(PointD pt)
        {
            return Vector.OffsetMatrix(pt.ToColumnVector());
        }

        /// <summary>
        /// 返回表示按 Point 结构将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">Point 结构，用于平移 PointD 结构。</param>
        /// <returns>Matrix 对象，表示按 Point 结构将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(Point pt)
        {
            return Vector.OffsetMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, pt.X, pt.Y));
        }

        /// <summary>
        /// 返回表示按 PointF 结构将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointF 结构，用于平移 PointD 结构。</param>
        /// <returns>Matrix 对象，表示按 PointF 结构将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(PointF pt)
        {
            return Vector.OffsetMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, pt.X, pt.Y));
        }

        /// <summary>
        /// 返回表示按 Size 结构将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，用于平移 PointD 结构。</param>
        /// <returns>Matrix 对象，表示按 Size 结构将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(Size sz)
        {
            return Vector.OffsetMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, sz.Width, sz.Height));
        }

        /// <summary>
        /// 返回表示按 SizeF 结构将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，用于平移 PointD 结构。</param>
        /// <returns>Matrix 对象，表示按 SizeF 结构将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(SizeF sz)
        {
            return Vector.OffsetMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, sz.Width, sz.Height));
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的缩放因数将 PointD 结构的所有分量缩放指定的倍数的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的缩放因数将 PointD 结构的所有分量缩放指定的倍数的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(double s)
        {
            return Vector.ScaleMatrix(Vector.Type.ColumnVector, 2, s);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的 X 坐标缩放因数与 Y 坐标缩放因数将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的 X 坐标缩放因数与 Y 坐标缩放因数将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(double sx, double sy)
        {
            return Vector.ScaleMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, sx, sy));
        }

        /// <summary>
        /// 返回表示按 PointD 结构将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于缩放 PointD 结构。</param>
        /// <returns>Matrix 对象，表示按 PointD 结构将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(PointD pt)
        {
            return Vector.ScaleMatrix(pt.ToColumnVector());
        }

        /// <summary>
        /// 返回表示按 Point 结构将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">Point 结构，用于缩放 PointD 结构。</param>
        /// <returns>Matrix 对象，表示按 Point 结构将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(Point pt)
        {
            return Vector.ScaleMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, pt.X, pt.Y));
        }

        /// <summary>
        /// 返回表示按 PointF 结构将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointF 结构，用于缩放 PointD 结构。</param>
        /// <returns>Matrix 对象，表示按 PointF 结构将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(PointF pt)
        {
            return Vector.ScaleMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, pt.X, pt.Y));
        }

        /// <summary>
        /// 返回表示按 Size 结构将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，用于缩放 PointD 结构。</param>
        /// <returns>Matrix 对象，表示按 Size 结构将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(Size sz)
        {
            return Vector.ScaleMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, sz.Width, sz.Height));
        }

        /// <summary>
        /// 返回表示按 SizeF 结构将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，用于缩放 PointD 结构。</param>
        /// <returns>Matrix 对象，表示按 SizeF 结构将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(SizeF sz)
        {
            return Vector.ScaleMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, sz.Width, sz.Height));
        }

        //

        /// <summary>
        /// 返回表示用于翻转 PointD 结构的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        /// <returns>Matrix 对象，表示用于翻转 PointD 结构的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ReflectMatrix(int index)
        {
            return Vector.ReflectMatrix(Vector.Type.ColumnVector, 2, index);
        }

        /// <summary>
        /// 返回表示将 PointD 结构在 X 轴的分量翻转的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <returns>Matrix 对象，表示将 PointD 结构在 X 轴的分量翻转的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ReflectXMatrix()
        {
            return ReflectMatrix(0);
        }

        /// <summary>
        /// 返回表示将 PointD 结构在 Y 轴的分量翻转的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <returns>Matrix 对象，表示将 PointD 结构在 Y 轴的分量翻转的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ReflectYMatrix()
        {
            return ReflectMatrix(1);
        }

        //

        /// <summary>
        /// 返回表示用于剪切 PointD 结构的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示 PointD 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        /// <returns>Matrix 对象，表示用于剪切 PointD 结构的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ShearMatrix(int index1, int index2, double angle)
        {
            return Vector.ShearMatrix(Vector.Type.ColumnVector, 2, index1, index2, angle);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD 结构向 +X 轴剪切指定的角度的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构向 +X 轴剪切的角度（弧度）。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的弧度将 PointD 结构向 +X 轴剪切指定的角度的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ShearXMatrix(double angle)
        {
            return ShearMatrix(0, 1, angle);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD 结构向 +Y 轴剪切指定的角度的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构向 +Y 轴剪切的角度（弧度）。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的弧度将 PointD 结构向 +Y 轴剪切指定的角度的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ShearYMatrix(double angle)
        {
            return ShearMatrix(1, 0, angle);
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD 结构绕原点旋转指定的角度的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD 结构绕原点旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的弧度将 PointD 结构绕原点旋转指定的角度的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix RotateMatrix(double angle)
        {
            return Vector.RotateMatrix(Vector.Type.ColumnVector, 2, 0, 1, angle);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD 结构绕指定的 PointD 结构旋转指定的角度的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD 结构绕指定的 PointD 结构旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <param name="pt">PointD 结构，表示旋转中心。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的弧度将 PointD 结构绕指定的 PointD 结构旋转指定的角度的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix RotateMatrix(double angle, PointD pt)
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

        //

        /// <summary>
        /// 返回两个 PointD 结构之间的距离。
        /// </summary>
        /// <param name="left">第一个 PointD 结构。</param>
        /// <param name="right">第二个 PointD 结构。</param>
        /// <returns>双精度浮点数，表示两个 PointD 结构之间的距离。</returns>
        public static double DistanceBetween(PointD left, PointD right)
        {
            double AbsDx = Math.Abs(left._X - right._X);
            double AbsDy = Math.Abs(left._Y - right._Y);

            double AbsMax = Math.Max(AbsDx, AbsDy);

            if (AbsMax == 0)
            {
                return 0;
            }
            else
            {
                AbsDx /= AbsMax;
                AbsDy /= AbsMax;

                double SqrSum = AbsDx * AbsDx + AbsDy * AbsDy;

                return (AbsMax * Math.Sqrt(SqrSum));
            }
        }

        /// <summary>
        /// 返回两个 PointD 结构之间的夹角（弧度）。
        /// </summary>
        /// <param name="left">第一个 PointD 结构。</param>
        /// <param name="right">第二个 PointD 结构。</param>
        /// <returns>双精度浮点数，表示两个 PointD 结构之间的夹角（弧度）。</returns>
        public static double AngleBetween(PointD left, PointD right)
        {
            if (left.IsZero || right.IsZero)
            {
                return 0;
            }
            else
            {
                double ModProduct = left.Module * right.Module;
                double CosA = left._X * right._X / ModProduct + left._Y * right._Y / ModProduct;

                return Math.Acos(CosA);
            }
        }

        //

        /// <summary>
        /// 返回两个 PointD 结构的数量积。
        /// </summary>
        /// <param name="left">第一个 PointD 结构。</param>
        /// <param name="right">第二个 PointD 结构。</param>
        /// <returns>双精度浮点数，表示两个 PointD 结构的数量积。</returns>
        public static double DotProduct(PointD left, PointD right)
        {
            return (left._X * right._X + left._Y * right._Y);
        }

        /// <summary>
        /// 返回两个 PointD 结构的向量积。该向量积为一个一维向量，其数值为 X∧Y 基向量的系数。
        /// </summary>
        /// <param name="left">第一个 PointD 结构。</param>
        /// <param name="right">第二个 PointD 结构。</param>
        /// <returns>Vector 对象，表示两个 PointD 结构的向量积。</returns>
        public static Vector CrossProduct(PointD left, PointD right)
        {
            return Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, left._X * right._Y - left._Y * right._X);
        }

        //

        /// <summary>
        /// 返回将 PointD 结构的所有分量取符号数得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        /// <returns>PointD 结构，表示将 PointD 结构的所有分量取符号数得到的结果</returns>
        public static PointD Sign(PointD pt)
        {
            return new PointD((double.IsNaN(pt._X) ? 0 : Math.Sign(pt._X)), (double.IsNaN(pt._Y) ? 0 : Math.Sign(pt._Y)));
        }

        /// <summary>
        /// 返回将 PointD 结构的所有分量取绝对值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        /// <returns>PointD 结构，表示将 PointD 结构的所有分量取绝对值得到的结果</returns>
        public static PointD Abs(PointD pt)
        {
            return new PointD(Math.Abs(pt._X), Math.Abs(pt._Y));
        }

        /// <summary>
        /// 返回将 PointD 结构的所有分量舍入到较大的整数值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        /// <returns>PointD 结构，表示将 PointD 结构的所有分量舍入到较大的整数值得到的结果</returns>
        public static PointD Ceiling(PointD pt)
        {
            return new PointD(Math.Ceiling(pt._X), Math.Ceiling(pt._Y));
        }

        /// <summary>
        /// 返回将 PointD 结构的所有分量舍入到较小的整数值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        /// <returns>PointD 结构，表示将 PointD 结构的所有分量舍入到较小的整数值得到的结果</returns>
        public static PointD Floor(PointD pt)
        {
            return new PointD(Math.Floor(pt._X), Math.Floor(pt._Y));
        }

        /// <summary>
        /// 返回将 PointD 结构的所有分量舍入到最接近的整数值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        /// <returns>PointD 结构，表示将 PointD 结构的所有分量舍入到最接近的整数值得到的结果</returns>
        public static PointD Round(PointD pt)
        {
            return new PointD(Math.Round(pt._X), Math.Round(pt._Y));
        }

        /// <summary>
        /// 返回将 PointD 结构的所有分量截断小数部分取整得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        /// <returns>PointD 结构，表示将 PointD 结构的所有分量截断小数部分取整得到的结果</returns>
        public static PointD Truncate(PointD pt)
        {
            return new PointD(Math.Truncate(pt._X), Math.Truncate(pt._Y));
        }

        /// <summary>
        /// 返回将两个 PointD 结构的所有分量分别取最大值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD 结构，用于比较的第二个结构。</param>
        /// <returns>PointD 结构，表示将两个 PointD 结构的所有分量分别取最大值得到的结果</returns>
        public static PointD Max(PointD left, PointD right)
        {
            return new PointD(Math.Max(left._X, right._X), Math.Max(left._Y, right._Y));
        }

        /// <summary>
        /// 返回将两个 PointD 结构的所有分量分别取最小值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD 结构，用于比较的第二个结构。</param>
        /// <returns>PointD 结构，表示将两个 PointD 结构的所有分量分别取最小值得到的结果</returns>
        public static PointD Min(PointD left, PointD right)
        {
            return new PointD(Math.Min(left._X, right._X), Math.Min(left._Y, right._Y));
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 PointD 结构是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD 结构。</param>
        /// <returns>布尔值，表示两个 PointD 结构是否相等。</returns>
        public static bool operator ==(PointD left, PointD right)
        {
            return (left._X == right._X && left._Y == right._Y);
        }

        /// <summary>
        /// 判断两个 PointD 结构是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD 结构。</param>
        /// <returns>布尔值，表示两个 PointD 结构是否不相等。</returns>
        public static bool operator !=(PointD left, PointD right)
        {
            return (left._X != right._X || left._Y != right._Y);
        }

        /// <summary>
        /// 判断两个 PointD 结构的字典序是否前者小于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD 结构。</param>
        /// <returns>布尔值，表示两个 PointD 结构的字典序是否前者小于后者。</returns>
        public static bool operator <(PointD left, PointD right)
        {
            for (int i = 0; i < left.Dimension; i++)
            {
                if (left[i] != right[i])
                {
                    return (left[i] < right[i]);
                }
            }

            return false;
        }

        /// <summary>
        /// 判断两个 PointD 结构的字典序是否前者大于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD 结构。</param>
        /// <returns>布尔值，表示两个 PointD 结构的字典序是否前者大于后者。</returns>
        public static bool operator >(PointD left, PointD right)
        {
            for (int i = 0; i < left.Dimension; i++)
            {
                if (left[i] != right[i])
                {
                    return (left[i] > right[i]);
                }
            }

            return false;
        }

        /// <summary>
        /// 判断两个 PointD 结构的字典序是否前者小于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD 结构。</param>
        /// <returns>布尔值，表示两个 PointD 结构的字典序是否前者小于或等于后者。</returns>
        public static bool operator <=(PointD left, PointD right)
        {
            for (int i = 0; i < left.Dimension; i++)
            {
                if (left[i] != right[i])
                {
                    return (left[i] < right[i]);
                }
            }

            return true;
        }

        /// <summary>
        /// 判断两个 PointD 结构的字典序是否前者大于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD 结构。</param>
        /// <returns>布尔值，表示两个 PointD 结构的字典序是否前者大于或等于后者。</returns>
        public static bool operator >=(PointD left, PointD right)
        {
            for (int i = 0; i < left.Dimension; i++)
            {
                if (left[i] != right[i])
                {
                    return (left[i] > right[i]);
                }
            }

            return true;
        }

        //

        /// <summary>
        /// 返回在 PointD 结构的所有分量前添加正号得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">运算符右侧的 PointD 结构。</param>
        /// <returns>PointD 结构，表示在 PointD 结构的所有分量前添加正号得到的结果。</returns>
        public static PointD operator +(PointD pt)
        {
            return pt;
        }

        /// <summary>
        /// 返回在 PointD 结构的所有分量前添加负号得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">运算符右侧的 PointD 结构。</param>
        /// <returns>PointD 结构，表示在 PointD 结构的所有分量前添加负号得到的结果。</returns>
        public static PointD operator -(PointD pt)
        {
            return new PointD(-pt._X, -pt._Y);
        }

        //

        /// <summary>
        /// 返回将 PointD 结构的所有分量与双精度浮点数相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被加数。</param>
        /// <param name="n">双精度浮点数，表示加数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与双精度浮点数的相加得到的结果。</returns>
        public static PointD operator +(PointD pt, double n)
        {
            return new PointD(pt._X + n, pt._Y + n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD 结构的所有分量相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被加数。</param>
        /// <param name="pt">PointD 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将双精度浮点数与 PointD 结构的相加得到的结果。</returns>
        public static PointD operator +(double n, PointD pt)
        {
            return new PointD(n + pt._X, n + pt._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 PointD 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被加数。</param>
        /// <param name="right">PointD 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 PointD 结构的相加得到的结果。</returns>
        public static PointD operator +(PointD left, PointD right)
        {
            return new PointD(left._X + right._X, left._Y + right._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 Point 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被加数。</param>
        /// <param name="right">Point 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 Point 结构的相加得到的结果。</returns>
        public static PointD operator +(PointD left, Point right)
        {
            return new PointD(left._X + right.X, left._Y + right.Y);
        }

        /// <summary>
        /// 返回将 Point 结构与 PointD 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">Point 结构，表示被加数。</param>
        /// <param name="right">PointD 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 Point 结构与 PointD 结构的相加得到的结果。</returns>
        public static PointD operator +(Point left, PointD right)
        {
            return new PointD(left.X + right._X, left.Y + right._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 PointF 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被加数。</param>
        /// <param name="right">PointF 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 PointF 结构的相加得到的结果。</returns>
        public static PointD operator +(PointD left, PointF right)
        {
            return new PointD(left._X + right.X, left._Y + right.Y);
        }

        /// <summary>
        /// 返回将 PointF 结构与 PointD 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointF 结构，表示被加数。</param>
        /// <param name="right">PointD 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 PointF 结构与 PointD 结构的相加得到的结果。</returns>
        public static PointD operator +(PointF left, PointD right)
        {
            return new PointD(left.X + right._X, left.Y + right._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 Size 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被加数。</param>
        /// <param name="sz">Size 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 Size 结构的相加得到的结果。</returns>
        public static PointD operator +(PointD pt, Size sz)
        {
            return new PointD(pt._X + sz.Width, pt._Y + sz.Height);
        }

        /// <summary>
        /// 返回将 Size 结构与 PointD 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，表示被加数。</param>
        /// <param name="pt">PointD 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 Size 结构与 PointD 结构的相加得到的结果。</returns>
        public static PointD operator +(Size sz, PointD pt)
        {
            return new PointD(sz.Width + pt._X, sz.Height + pt._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 SizeF 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被加数。</param>
        /// <param name="sz">SizeF 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 SizeF 结构的相加得到的结果。</returns>
        public static PointD operator +(PointD pt, SizeF sz)
        {
            return new PointD(pt._X + sz.Width, pt._Y + sz.Height);
        }

        /// <summary>
        /// 返回将 SizeF 结构与 PointD 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，表示被加数。</param>
        /// <param name="pt">PointD 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 SizeF 结构与 PointD 结构的相加得到的结果。</returns>
        public static PointD operator +(SizeF sz, PointD pt)
        {
            return new PointD(sz.Width + pt._X, sz.Height + pt._Y);
        }

        //

        /// <summary>
        /// 返回将 PointD 结构的所有分量与双精度浮点数相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被减数。</param>
        /// <param name="n">双精度浮点数，表示减数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与双精度浮点数的相减得到的结果。</returns>
        public static PointD operator -(PointD pt, double n)
        {
            return new PointD(pt._X - n, pt._Y - n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD 结构的所有分量相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被减数。</param>
        /// <param name="pt">PointD 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将双精度浮点数与 PointD 结构的相减得到的结果。</returns>
        public static PointD operator -(double n, PointD pt)
        {
            return new PointD(n - pt._X, n - pt._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 PointD 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被减数。</param>
        /// <param name="right">PointD 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 PointD 结构的相减得到的结果。</returns>
        public static PointD operator -(PointD left, PointD right)
        {
            return new PointD(left._X - right._X, left._Y - right._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 Point 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被减数。</param>
        /// <param name="right">Point 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 Point 结构的相减得到的结果。</returns>
        public static PointD operator -(PointD left, Point right)
        {
            return new PointD(left._X - right.X, left._Y - right.Y);
        }

        /// <summary>
        /// 返回将 Point 结构与 PointD 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">Point 结构，表示被减数。</param>
        /// <param name="right">PointD 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 Point 结构与 PointD 结构的相减得到的结果。</returns>
        public static PointD operator -(Point left, PointD right)
        {
            return new PointD(left.X - right._X, left.Y - right._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 PointF 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被减数。</param>
        /// <param name="right">PointF 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 PointF 结构的相减得到的结果。</returns>
        public static PointD operator -(PointD left, PointF right)
        {
            return new PointD(left._X - right.X, left._Y - right.Y);
        }

        /// <summary>
        /// 返回将 PointF 结构与 PointD 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointF 结构，表示被减数。</param>
        /// <param name="right">PointD 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 PointF 结构与 PointD 结构的相减得到的结果。</returns>
        public static PointD operator -(PointF left, PointD right)
        {
            return new PointD(left.X - right._X, left.Y - right._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 Size 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被减数。</param>
        /// <param name="sz">Size 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 Size 结构的相减得到的结果。</returns>
        public static PointD operator -(PointD pt, Size sz)
        {
            return new PointD(pt._X - sz.Width, pt._Y - sz.Height);
        }

        /// <summary>
        /// 返回将 Size 结构与 PointD 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，表示被减数。</param>
        /// <param name="pt">PointD 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 Size 结构与 PointD 结构的相减得到的结果。</returns>
        public static PointD operator -(Size sz, PointD pt)
        {
            return new PointD(sz.Width - pt._X, sz.Height - pt._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 SizeF 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被减数。</param>
        /// <param name="sz">SizeF 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 SizeF 结构的相减得到的结果。</returns>
        public static PointD operator -(PointD pt, SizeF sz)
        {
            return new PointD(pt._X - sz.Width, pt._Y - sz.Height);
        }

        /// <summary>
        /// 返回将 SizeF 结构与 PointD 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，表示被减数。</param>
        /// <param name="pt">PointD 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 SizeF 结构与 PointD 结构的相减得到的结果。</returns>
        public static PointD operator -(SizeF sz, PointD pt)
        {
            return new PointD(sz.Width - pt._X, sz.Height - pt._Y);
        }

        //

        /// <summary>
        /// 返回将 PointD 结构的所有分量与双精度浮点数相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被乘数。</param>
        /// <param name="n">双精度浮点数，表示乘数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与双精度浮点数的相乘得到的结果。</returns>
        public static PointD operator *(PointD pt, double n)
        {
            return new PointD(pt._X * n, pt._Y * n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD 结构的所有分量相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被乘数。</param>
        /// <param name="pt">PointD 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将双精度浮点数与 PointD 结构的相乘得到的结果。</returns>
        public static PointD operator *(double n, PointD pt)
        {
            return new PointD(n * pt._X, n * pt._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 PointD 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被乘数。</param>
        /// <param name="right">PointD 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 PointD 结构的相乘得到的结果。</returns>
        public static PointD operator *(PointD left, PointD right)
        {
            return new PointD(left._X * right._X, left._Y * right._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 Point 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被乘数。</param>
        /// <param name="right">Point 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 Point 结构的相乘得到的结果。</returns>
        public static PointD operator *(PointD left, Point right)
        {
            return new PointD(left._X * right.X, left._Y * right.Y);
        }

        /// <summary>
        /// 返回将 Point 结构与 PointD 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">Point 结构，表示被乘数。</param>
        /// <param name="right">PointD 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 Point 结构与 PointD 结构的相乘得到的结果。</returns>
        public static PointD operator *(Point left, PointD right)
        {
            return new PointD(left.X * right._X, left.Y * right._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 PointF 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被乘数。</param>
        /// <param name="right">PointF 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 PointF 结构的相乘得到的结果。</returns>
        public static PointD operator *(PointD left, PointF right)
        {
            return new PointD(left._X * right.X, left._Y * right.Y);
        }

        /// <summary>
        /// 返回将 PointF 结构与 PointD 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointF 结构，表示被乘数。</param>
        /// <param name="right">PointD 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 PointF 结构与 PointD 结构的相乘得到的结果。</returns>
        public static PointD operator *(PointF left, PointD right)
        {
            return new PointD(left.X * right._X, left.Y * right._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 Size 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被乘数。</param>
        /// <param name="sz">Size 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 Size 结构的相乘得到的结果。</returns>
        public static PointD operator *(PointD pt, Size sz)
        {
            return new PointD(pt._X * sz.Width, pt._Y * sz.Height);
        }

        /// <summary>
        /// 返回将 Size 结构与 PointD 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，表示被乘数。</param>
        /// <param name="pt">PointD 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 Size 结构与 PointD 结构的相乘得到的结果。</returns>
        public static PointD operator *(Size sz, PointD pt)
        {
            return new PointD(sz.Width * pt._X, sz.Height * pt._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 SizeF 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被乘数。</param>
        /// <param name="sz">SizeF 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 SizeF 结构的相乘得到的结果。</returns>
        public static PointD operator *(PointD pt, SizeF sz)
        {
            return new PointD(pt._X * sz.Width, pt._Y * sz.Height);
        }

        /// <summary>
        /// 返回将 SizeF 结构与 PointD 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，表示被乘数。</param>
        /// <param name="pt">PointD 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 SizeF 结构与 PointD 结构的相乘得到的结果。</returns>
        public static PointD operator *(SizeF sz, PointD pt)
        {
            return new PointD(sz.Width * pt._X, sz.Height * pt._Y);
        }

        //

        /// <summary>
        /// 返回将 PointD 结构的所有分量与双精度浮点数相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被除数。</param>
        /// <param name="n">双精度浮点数，表示除数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与双精度浮点数的相除得到的结果。</returns>
        public static PointD operator /(PointD pt, double n)
        {
            return new PointD(pt._X / n, pt._Y / n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD 结构的所有分量相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被除数。</param>
        /// <param name="pt">PointD 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将双精度浮点数与 PointD 结构的相除得到的结果。</returns>
        public static PointD operator /(double n, PointD pt)
        {
            return new PointD(n / pt._X, n / pt._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 PointD 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被除数。</param>
        /// <param name="right">PointD 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 PointD 结构的相除得到的结果。</returns>
        public static PointD operator /(PointD left, PointD right)
        {
            return new PointD(left._X / right._X, left._Y / right._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 Point 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被除数。</param>
        /// <param name="right">Point 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 Point 结构的相除得到的结果。</returns>
        public static PointD operator /(PointD left, Point right)
        {
            return new PointD(left._X / right.X, left._Y / right.Y);
        }

        /// <summary>
        /// 返回将 Point 结构与 PointD 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">Point 结构，表示被除数。</param>
        /// <param name="right">PointD 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 Point 结构与 PointD 结构的相除得到的结果。</returns>
        public static PointD operator /(Point left, PointD right)
        {
            return new PointD(left.X / right._X, left.Y / right._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 PointF 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被除数。</param>
        /// <param name="right">PointF 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 PointF 结构的相除得到的结果。</returns>
        public static PointD operator /(PointD left, PointF right)
        {
            return new PointD(left._X / right.X, left._Y / right.Y);
        }

        /// <summary>
        /// 返回将 PointF 结构与 PointD 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointF 结构，表示被除数。</param>
        /// <param name="right">PointD 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 PointF 结构与 PointD 结构的相除得到的结果。</returns>
        public static PointD operator /(PointF left, PointD right)
        {
            return new PointD(left.X / right._X, left.Y / right._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 Size 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被除数。</param>
        /// <param name="sz">Size 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 Size 结构的相除得到的结果。</returns>
        public static PointD operator /(PointD pt, Size sz)
        {
            return new PointD(pt._X / sz.Width, pt._Y / sz.Height);
        }

        /// <summary>
        /// 返回将 Size 结构与 PointD 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，表示被除数。</param>
        /// <param name="pt">PointD 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 Size 结构与 PointD 结构的相除得到的结果。</returns>
        public static PointD operator /(Size sz, PointD pt)
        {
            return new PointD(sz.Width / pt._X, sz.Height / pt._Y);
        }

        /// <summary>
        /// 返回将 PointD 结构与 SizeF 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被除数。</param>
        /// <param name="sz">SizeF 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 SizeF 结构的相除得到的结果。</returns>
        public static PointD operator /(PointD pt, SizeF sz)
        {
            return new PointD(pt._X / sz.Width, pt._Y / sz.Height);
        }

        /// <summary>
        /// 返回将 SizeF 结构与 PointD 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，表示被除数。</param>
        /// <param name="pt">PointD 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 SizeF 结构与 PointD 结构的相除得到的结果。</returns>
        public static PointD operator /(SizeF sz, PointD pt)
        {
            return new PointD(sz.Width / pt._X, sz.Height / pt._Y);
        }

        //

        /// <summary>
        /// 将指定的 PointD 结构显式转换为 Point 结构。
        /// </summary>
        /// <param name="pt">用于转换的 PointD 结构。</param>
        /// <returns>Point 结构，表示显式转换的结果。</returns>
        public static explicit operator Point(PointD pt)
        {
            return pt.ToPoint();
        }

        /// <summary>
        /// 将指定的 PointD 结构显式转换为 PointF 结构。
        /// </summary>
        /// <param name="pt">用于转换的 PointD 结构。</param>
        /// <returns>PointF 结构，表示显式转换的结果。</returns>
        public static explicit operator PointF(PointD pt)
        {
            return pt.ToPointF();
        }

        /// <summary>
        /// 将指定的 PointD 结构显式转换为 Size 结构。
        /// </summary>
        /// <param name="pt">用于转换的 PointD 结构。</param>
        /// <returns>Size 结构，表示显式转换的结果。</returns>
        public static explicit operator Size(PointD pt)
        {
            return pt.ToSize();
        }

        /// <summary>
        /// 将指定的 PointD 结构显式转换为 SizeF 结构。
        /// </summary>
        /// <param name="pt">用于转换的 PointD 结构。</param>
        /// <returns>SizeF 结构，表示显式转换的结果。</returns>
        public static explicit operator SizeF(PointD pt)
        {
            return pt.ToSizeF();
        }

        /// <summary>
        /// 将指定的 PointD 结构显式转换为 Complex 结构。
        /// </summary>
        /// <param name="pt">用于转换的 PointD 结构。</param>
        /// <returns>Complex 结构，表示显式转换的结果。</returns>
        public static explicit operator Complex(PointD pt)
        {
            return pt.ToComplex();
        }

        /// <summary>
        /// 将指定的 Point 结构隐式转换为 PointD 结构。
        /// </summary>
        /// <param name="pt">用于转换的 Point 结构。</param>
        /// <returns>PointD 结构，表示隐式转换的结果。</returns>
        public static implicit operator PointD(Point pt)
        {
            return new PointD(pt);
        }

        /// <summary>
        /// 将指定的 PointF 结构隐式转换为 PointD 结构。
        /// </summary>
        /// <param name="pt">用于转换的 PointF 结构。</param>
        /// <returns>PointD 结构，表示隐式转换的结果。</returns>
        public static implicit operator PointD(PointF pt)
        {
            return new PointD(pt);
        }

        /// <summary>
        /// 将指定的 Size 结构显式转换为 PointD 结构。
        /// </summary>
        /// <param name="sz">用于转换的 Size 结构。</param>
        /// <returns>PointD 结构，表示显式转换的结果。</returns>
        public static explicit operator PointD(Size sz)
        {
            return new PointD(sz);
        }

        /// <summary>
        /// 将指定的 SizeF 结构显式转换为 PointD 结构。
        /// </summary>
        /// <param name="sz">用于转换的 SizeF 结构。</param>
        /// <returns>PointD 结构，表示显式转换的结果。</returns>
        public static explicit operator PointD(SizeF sz)
        {
            return new PointD(sz);
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
                if (value == null || !(value is double))
                {
                    throw new ArgumentNullException();
                }

                //

                this[index] = (double)value;
            }
        }

        int IList.Add(object item)
        {
            throw new NotSupportedException();
        }

        void IList.Clear()
        {
            this = new PointD();
        }

        bool IList.Contains(object item)
        {
            if (item == null || !(item is double))
            {
                return false;
            }
            else
            {
                return Contains((double)item);
            }
        }

        int IList.IndexOf(object item)
        {
            if (item == null || !(item is double))
            {
                return -1;
            }
            else
            {
                return IndexOf((double)item);
            }
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
            if (array == null)
            {
                throw new ArgumentNullException();
            }

            if (array.Rank != 1)
            {
                throw new RankException();
            }

            if (array.Length < Dimension)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            ToArray().CopyTo(array, index);
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
                    if (_Index < 0 || _Index >= _Pt.Dimension)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    //

                    return _Pt[_Index];
                }
            }

            bool IEnumerator.MoveNext()
            {
                if (_Index >= _Pt.Dimension - 1)
                {
                    return false;
                }
                else
                {
                    _Index++;

                    return true;
                }
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
            this = new PointD();
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
                    if (_Index < 0 || _Index >= _Pt.Dimension)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    //

                    return _Pt[_Index];
                }
            }

            bool IEnumerator.MoveNext()
            {
                if (_Index >= _Pt.Dimension - 1)
                {
                    return false;
                }
                else
                {
                    _Index++;

                    return true;
                }
            }

            void IEnumerator.Reset()
            {
                _Index = -1;
            }

            double IEnumerator<double>.Current
            {
                get
                {
                    if (_Index < 0 || _Index >= _Pt.Dimension)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    //

                    return _Pt[_Index];
                }
            }
        }

        #endregion

        #endregion
    }
}