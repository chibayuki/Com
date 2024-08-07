﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.PointD
Version 24.7.21.1040

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
    public struct PointD : IEquatable<PointD>, IComparable, IComparable<PointD>, IEuclideanVector<PointD>, IAffineTransformable<PointD>, IAffine<PointD>
    {
        #region 非公开成员

        private const int _Dimension = 2; // PointD 结构的维度。

        //

        private double _X; // X 坐标。
        private double _Y; // Y 坐标。

        //

        // 按 Vector 对象更新此 PointD 结构。
        private void _UpdateByVector(Vector vector)
        {
            if (Vector.IsNullOrEmpty(vector) || vector.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }

            //

            _X = vector.UnsafeGetData()[0];
            _Y = vector.UnsafeGetData()[1];
        }

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
        /// 获取或设置此 PointD 结构在指定的基向量方向的分量。
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
                    default: throw new IndexOutOfRangeException(nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0: _X = value; break;
                    case 1: _Y = value; break;
                    default: throw new IndexOutOfRangeException(nameof(index));
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置此 PointD 结构在 X 轴的分量。
        /// </summary>
        public double X
        {
            get => _X;
            set => _X = value;
        }

        /// <summary>
        /// 获取或设置此 PointD 结构在 Y 轴的分量。
        /// </summary>
        public double Y
        {
            get => _Y;
            set => _Y = value;
        }

        //

        /// <summary>
        /// 获取此 PointD 结构的维度。
        /// </summary>
        public int Dimension => _Dimension;

        //

        /// <summary>
        /// 获取表示此 PointD 结构是否为空向量的布尔值。
        /// </summary>
        public bool IsEmpty => false;

        /// <summary>
        /// 获取表示此 PointD 结构是否为零向量的布尔值。
        /// </summary>
        public bool IsZero => _X == 0 && _Y == 0;

        /// <summary>
        /// 获取表示此 PointD 结构是否只读的布尔值。
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// 获取表示此 PointD 结构是否具有固定的维度的布尔值。
        /// </summary>
        public bool IsFixedSize => true;

        /// <summary>
        /// 获取表示此 PointD 结构是否包含非数字分量的布尔值。
        /// </summary>
        public bool IsNaN => double.IsNaN(_X) || double.IsNaN(_Y);

        /// <summary>
        /// 获取表示此 PointD 结构是否包含无穷大分量的布尔值。
        /// </summary>
        public bool IsInfinity => !IsNaN && (double.IsInfinity(_X) || double.IsInfinity(_Y));

        /// <summary>
        /// 获取表示此 PointD 结构是否包含非数字或无穷大分量的布尔值。
        /// </summary>
        public bool IsNaNOrInfinity => InternalMethod.IsNaNOrInfinity(_X) || InternalMethod.IsNaNOrInfinity(_Y);

        //

        /// <summary>
        /// 获取此 PointD 结构的模。
        /// </summary>
        public double Module
        {
            get
            {
                double absX = Math.Abs(_X);
                double absY = Math.Abs(_Y);

                double absMax = Math.Max(absX, absY);

                if (absMax == 0)
                {
                    return 0;
                }
                else
                {
                    absX /= absMax;
                    absY /= absMax;

                    double sqrSum = absX * absX + absY * absY;

                    return absMax * Math.Sqrt(sqrSum);
                }
            }
        }

        /// <summary>
        /// 获取此 PointD 结构的模的平方。
        /// </summary>
        public double ModuleSquared => _X * _X + _Y * _Y;

        //

        /// <summary>
        /// 获取此 PointD 结构的相反向量。
        /// </summary>
        public PointD Opposite => new PointD(-_X, -_Y);

        /// <summary>
        /// 获取此 PointD 结构的规范化向量。
        /// </summary>
        public PointD Normalize
        {
            get
            {
                double mod = Module;

                if (mod <= 0)
                {
                    return Ex;
                }
                else
                {
                    return new PointD(_X / mod, _Y / mod);
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
        public double Azimuth => Math.Atan2(_Y, _X);

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 PointD 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>布尔值，表示此 PointD 结构是否与指定的对象相等。</returns>
        public override bool Equals(object obj)
        {
            if (obj is null || !(obj is PointD))
            {
                return false;
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
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// 将此 PointD 结构转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 PointD 结构的字符串形式。</returns>
        public override string ToString() => $"{{X={_X}, Y={_Y}}}";

        //

        /// <summary>
        /// 判断此 PointD 结构是否与指定的 PointD 结构相等。
        /// </summary>
        /// <param name="pt">用于比较的 PointD 结构。</param>
        /// <returns>布尔值，表示此 PointD 结构是否与指定的 PointD 结构相等。</returns>
        public bool Equals(PointD pt) => _X.Equals(pt._X) && _Y.Equals(pt._Y);

        //

        /// <summary>
        /// 将此 PointD 结构与指定的对象进行次序比较。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>32 位整数，表示将此 PointD 结构与指定的对象进行次序比较得到的结果。</returns>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (!(obj is PointD))
            {
                throw new ArgumentException();
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
            for (int i = 0; i < _Dimension; i++)
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
        /// 遍历此 PointD 结构的所有分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int IndexOf(double item) => Array.IndexOf(ToArray(), item, 0, _Dimension);

        /// <summary>
        /// 从指定的索引开始遍历此 PointD 结构的所有分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int IndexOf(double item, int startIndex)
        {
            if (startIndex < 0 || startIndex >= _Dimension)
            {
                throw new IndexOutOfRangeException(nameof(startIndex));
            }

            //

            return Array.IndexOf(ToArray(), item, startIndex, _Dimension - startIndex);
        }

        /// <summary>
        /// 从指定的索引开始遍历此 PointD 结构指定数量的分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <param name="count">遍历的分量数量。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int IndexOf(double item, int startIndex, int count)
        {
            if (startIndex < 0 || startIndex >= _Dimension)
            {
                throw new IndexOutOfRangeException(nameof(startIndex));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            //

            return Array.IndexOf(ToArray(), item, startIndex, Math.Min(_Dimension - startIndex, count));
        }

        /// <summary>
        /// 逆序遍历此 PointD 结构的所有分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int LastIndexOf(double item) => Array.LastIndexOf(ToArray(), item, _Dimension - 1, _Dimension);

        /// <summary>
        /// 从指定的索引开始逆序遍历此 PointD 结构的所有分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int LastIndexOf(double item, int startIndex)
        {
            if (startIndex < 0 || startIndex >= _Dimension)
            {
                throw new IndexOutOfRangeException(nameof(startIndex));
            }

            //

            return Array.LastIndexOf(ToArray(), item, startIndex, startIndex + 1);
        }

        /// <summary>
        /// 从指定的索引开始逆序遍历此 PointD 结构指定数量的分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <param name="count">遍历的分量数量。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int LastIndexOf(double item, int startIndex, int count)
        {
            if (startIndex < 0 || startIndex >= _Dimension)
            {
                throw new IndexOutOfRangeException(nameof(startIndex));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            //

            return Array.LastIndexOf(ToArray(), item, startIndex, Math.Min(startIndex + 1, count));
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
        public double[] ToArray() => new double[_Dimension] { _X, _Y };

        /// <summary>
        /// 将此 PointD 结构转换为双精度浮点数列表。
        /// </summary>
        /// <returns>双精度浮点数列表，列表元素表示此 PointD 结构的分量。</returns>
        public List<double> ToList() => new List<double>(_Dimension) { _X, _Y };

        //

        /// <summary>
        /// 将此 PointD 结构的所有分量复制到双精度浮点数数组中。
        /// </summary>
        /// <param name="array">双精度浮点数数组。</param>
        /// <param name="index">双精度浮点数数组的起始索引。</param>
        public void CopyTo(double[] array, int index)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Length - index < _Dimension)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }

            //

            ToArray().CopyTo(array, index);
        }

        //

        /// <summary>
        /// 返回将此 PointD 结构表示的直角坐标系坐标转换为极坐标系坐标的 PointD 结构的新实例。
        /// </summary>
        /// <returns>PointD 结构，表示将此 PointD 结构表示的直角坐标系坐标转换为极坐标系坐标得到的结果。</returns>
        public PointD ToSpherical() => new PointD(Module, Azimuth);

        /// <summary>
        /// 返回将此 PointD 结构表示的极坐标系坐标转换为直角坐标系坐标的 PointD 结构的新实例。
        /// </summary>
        /// <returns>PointD 结构，表示将此 PointD 结构表示的极坐标系坐标转换为直角坐标系坐标得到的结果。</returns>
        public PointD ToCartesian() => new PointD(_X * Math.Cos(_Y), _X * Math.Sin(_Y));

        //

        /// <summary>
        /// 返回此 PointD 结构与指定的 PointD 结构之间的距离。
        /// </summary>
        /// <param name="pt">PointD 结构，表示另一个向量。</param>
        /// <returns>双精度浮点数，表示此 PointD 结构与指定的 PointD 结构之间的距离。</returns>
        public double DistanceFrom(PointD pt)
        {
            double absDx = Math.Abs(_X - pt._X);
            double absDy = Math.Abs(_Y - pt._Y);

            double absMax = Math.Max(absDx, absDy);

            if (absMax == 0)
            {
                return 0;
            }
            else
            {
                absDx /= absMax;
                absDy /= absMax;

                double sqrSum = absDx * absDx + absDy * absDy;

                return absMax * Math.Sqrt(sqrSum);
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
                double modProduct = Module * pt.Module;
                double cosA = _X * pt._X / modProduct + _Y * pt._Y / modProduct;

                return Math.Acos(cosA);
            }
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的位移将此 PointD 结构的指定的基向量方向的分量平移指定的量。
        /// </summary>
        /// <param name="index">索引，用于指定平移的分量所在方向的基向量。</param>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public void Offset(int index, double d)
        {
            switch (index)
            {
                case 0: _X += d; break;
                case 1: _Y += d; break;
                default: throw new IndexOutOfRangeException(nameof(index));
            }
        }

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
        /// 按 PointD 结构表示的位移将此 PointD 结构平移指定的量。
        /// </summary>
        /// <param name="pt">PointD 结构表示的位移。</param>
        public void Offset(PointD pt)
        {
            _X += pt._X;
            _Y += pt._Y;
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
        /// 按双精度浮点数表示的位移将此 PointD 结构在 X 轴的分量平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public void OffsetX(double d) => _X += d;

        /// <summary>
        /// 按双精度浮点数表示的位移将此 PointD 结构在 Y 轴的分量平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public void OffsetY(double d) => _Y += d;

        /// <summary>
        /// 按 Point 结构表示的位移将此 PointD 结构平移指定的量。
        /// </summary>
        /// <param name="pt">Point 结构表示的位移。</param>
        public void Offset(Point pt)
        {
            _X += pt.X;
            _Y += pt.Y;
        }

        /// <summary>
        /// 按 PointF 结构表示的位移将此 PointD 结构平移指定的量。
        /// </summary>
        /// <param name="pt">PointF 结构表示的位移。</param>
        public void Offset(PointF pt)
        {
            _X += pt.X;
            _Y += pt.Y;
        }

        /// <summary>
        /// 按 Size 结构表示的位移将此 PointD 结构平移指定的量。
        /// </summary>
        /// <param name="sz">Size 结构表示的位移。</param>
        public void Offset(Size sz)
        {
            _X += sz.Width;
            _Y += sz.Height;
        }

        /// <summary>
        /// 按 SizeF 结构表示的位移将此 PointD 结构平移指定的量。
        /// </summary>
        /// <param name="sz">SizeF 结构表示的位移。</param>
        public void Offset(SizeF sz)
        {
            _X += sz.Width;
            _Y += sz.Height;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的位移将此 PointD 结构的指定的基向量方向的分量平移指定的量的 PointD 结构的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定平移的分量所在方向的基向量。</param>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的位移将此 PointD 结构的指定的基向量方向的分量平移指定的量得到的结果。</returns>
        public PointD OffsetCopy(int index, double d)
        {
            switch (index)
            {
                case 0: return new PointD(_X + d, _Y);
                case 1: return new PointD(_X, _Y + d);
                default: throw new IndexOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的位移将此 PointD 结构的所有分量平移指定的量的 PointD 结构的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的位移将此 PointD 结构的所有分量平移指定的量得到的结果。</returns>
        public PointD OffsetCopy(double d) => new PointD(_X + d, _Y + d);

        /// <summary>
        /// 返回按 PointD 结构表示的位移将此 PointD 结构平移指定的量的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构表示的位移。</param>
        /// <returns>PointD 结构，表示按 PointD 结构表示的位移将此 PointD 结构平移指定的量得到的结果。</returns>
        public PointD OffsetCopy(PointD pt) => new PointD(_X + pt._X, _Y + pt._Y);

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标位移与 Y 坐标位移将此 PointD 结构平移指定的量的 PointD 结构的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的 X 坐标位移与 Y 坐标位移将此 PointD 结构平移指定的量得到的结果。</returns>
        public PointD OffsetCopy(double dx, double dy) => new PointD(_X + dx, _Y + dy);

        /// <summary>
        /// 返回按双精度浮点数表示的位移将此 PointD 结构在 X 轴的分量平移指定的量的 PointD 结构的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的位移将此 PointD 结构在 X 轴的分量平移指定的量得到的结果。</returns>
        public PointD OffsetXCopy(double d) => new PointD(_X + d, _Y);

        /// <summary>
        /// 返回按双精度浮点数表示的位移将此 PointD 结构在 Y 轴的分量平移指定的量的 PointD 结构的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的位移将此 PointD 结构在 Y 轴的分量平移指定的量得到的结果。</returns>
        public PointD OffsetYCopy(double d) => new PointD(_X, _Y + d);

        /// <summary>
        /// 返回按 Point 结构表示的位移将此 PointD 结构平移指定的量的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">Point 结构表示的位移。</param>
        /// <returns>PointD 结构，表示按 Point 结构表示的位移将此 PointD 结构平移指定的量得到的结果。</returns>
        public PointD OffsetCopy(Point pt) => new PointD(_X + pt.X, _Y + pt.Y);

        /// <summary>
        /// 返回按 PointF 结构表示的位移将此 PointD 结构平移指定的量的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointF 结构表示的位移。</param>
        /// <returns>PointD 结构，表示按 PointF 结构表示的位移将此 PointD 结构平移指定的量得到的结果。</returns>
        public PointD OffsetCopy(PointF pt) => new PointD(_X + pt.X, _Y + pt.Y);

        /// <summary>
        /// 返回按 Size 结构表示的位移将此 PointD 结构平移指定的量的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构表示的位移。</param>
        /// <returns>PointD 结构，表示按 Size 结构表示的位移将此 PointD 结构平移指定的量得到的结果。</returns>
        public PointD OffsetCopy(Size sz) => new PointD(_X + sz.Width, _Y + sz.Height);

        /// <summary>
        /// 返回按 SizeF 结构表示的位移将此 PointD 结构平移指定的量的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构表示的位移。</param>
        /// <returns>PointD 结构，表示按 SizeF 结构表示的位移将此 PointD 结构平移指定的量得到的结果。</returns>
        public PointD OffsetCopy(SizeF sz) => new PointD(_X + sz.Width, _Y + sz.Height);

        //

        /// <summary>
        /// 按双精度浮点数表示的缩放因数将此 PointD 结构的指定的基向量方向的分量缩放指定的倍数。
        /// </summary>
        /// <param name="index">索引，用于指定缩放的分量所在方向的基向量。</param>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public void Scale(int index, double s)
        {
            switch (index)
            {
                case 0: _X *= s; break;
                case 1: _Y *= s; break;
                default: throw new IndexOutOfRangeException(nameof(index));
            }
        }

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
        /// 按 PointD 结构表示的缩放因数将此 PointD 结构缩放指定的倍数。
        /// </summary>
        /// <param name="pt">PointD 结构表示的缩放因数。</param>
        public void Scale(PointD pt)
        {
            _X *= pt._X;
            _Y *= pt._Y;
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
        /// 按双精度浮点数表示的缩放因数将此 PointD 结构在 X 轴的分量缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public void ScaleX(double s) => _X *= s;

        /// <summary>
        /// 按双精度浮点数表示的缩放因数将此 PointD 结构在 Y 轴的分量缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public void ScaleY(double s) => _Y *= s;

        /// <summary>
        /// 按 Point 结构表示的缩放因数将此 PointD 结构缩放指定的倍数。
        /// </summary>
        /// <param name="pt">Point 结构表示的缩放因数。</param>
        public void Scale(Point pt)
        {
            _X *= pt.X;
            _Y *= pt.Y;
        }

        /// <summary>
        /// 按 PointF 结构表示的缩放因数将此 PointD 结构缩放指定的倍数。
        /// </summary>
        /// <param name="pt">PointF 结构表示的缩放因数。</param>
        public void Scale(PointF pt)
        {
            _X *= pt.X;
            _Y *= pt.Y;
        }

        /// <summary>
        /// 按 Size 结构表示的缩放因数将此 PointD 结构缩放指定的倍数。
        /// </summary>
        /// <param name="sz">Size 结构表示的缩放因数。</param>
        public void Scale(Size sz)
        {
            _X *= sz.Width;
            _Y *= sz.Height;
        }

        /// <summary>
        /// 按 SizeF 结构表示的缩放因数将此 PointD 结构缩放指定的倍数。
        /// </summary>
        /// <param name="sz">SizeF 结构表示的缩放因数。</param>
        public void Scale(SizeF sz)
        {
            _X *= sz.Width;
            _Y *= sz.Height;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数将此 PointD 结构的指定的基向量方向的分量缩放指定的倍数的 PointD 结构的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定缩放的分量所在方向的基向量。</param>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的缩放因数将此 PointD 结构的指定的基向量方向的分量缩放指定的倍数得到的结果。</returns>
        public PointD ScaleCopy(int index, double s)
        {
            switch (index)
            {
                case 0: return new PointD(_X * s, _Y);
                case 1: return new PointD(_X, _Y * s);
                default: throw new IndexOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数将此 PointD 结构的所有分量缩放指定的倍数的 PointD 结构的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的缩放因数将此 PointD 结构的所有分量缩放指定的倍数得到的结果。</returns>
        public PointD ScaleCopy(double s) => new PointD(_X * s, _Y * s);

        /// <summary>
        /// 返回按 PointD 结构表示的缩放因数将此 PointD 结构缩放指定的倍数的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构表示的缩放因数。</param>
        /// <returns>PointD 结构，表示按 PointD 结构表示的缩放因数将此 PointD 结构缩放指定的倍数得到的结果。</returns>
        public PointD ScaleCopy(PointD pt) => new PointD(_X * pt._X, _Y * pt._Y);

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标缩放因数与 Y 坐标缩放因数将此 PointD 结构缩放指定的倍数的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的 X 坐标缩放因数与 Y 坐标缩放因数将此 PointD 结构缩放指定的倍数得到的结果。</returns>
        public PointD ScaleCopy(double sx, double sy) => new PointD(_X * sx, _Y * sy);

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数将此 PointD 结构在 X 轴的分量缩放指定的倍数的 PointD 结构的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的缩放因数将此 PointD 结构在 X 轴的分量缩放指定的倍数得到的结果。</returns>
        public PointD ScaleXCopy(double s) => new PointD(_X * s, _Y);

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数将此 PointD 结构在 Y 轴的分量缩放指定的倍数的 PointD 结构的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的缩放因数将此 PointD 结构在 Y 轴的分量缩放指定的倍数得到的结果。</returns>
        public PointD ScaleYCopy(double s) => new PointD(_X, _Y * s);

        /// <summary>
        /// 返回按 Point 结构表示的缩放因数将此 PointD 结构缩放指定的倍数的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">Point 结构表示的缩放因数。</param>
        /// <returns>PointD 结构，表示按 Point 结构表示的缩放因数将此 PointD 结构缩放指定的倍数得到的结果。</returns>
        public PointD ScaleCopy(Point pt) => new PointD(_X * pt.X, _Y * pt.Y);

        /// <summary>
        /// 返回按 PointF 结构表示的缩放因数将此 PointD 结构缩放指定的倍数的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointF 结构表示的缩放因数。</param>
        /// <returns>PointD 结构，表示按 PointF 结构表示的缩放因数将此 PointD 结构缩放指定的倍数得到的结果。</returns>
        public PointD ScaleCopy(PointF pt) => new PointD(_X * pt.X, _Y * pt.Y);

        /// <summary>
        /// 返回按 Size 结构表示的缩放因数将此 PointD 结构缩放指定的倍数的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构表示的缩放因数。</param>
        /// <returns>PointD 结构，表示按 Size 结构表示的缩放因数将此 PointD 结构缩放指定的倍数得到的结果。</returns>
        public PointD ScaleCopy(Size sz) => new PointD(_X * sz.Width, _Y * sz.Height);

        /// <summary>
        /// 返回按 SizeF 结构表示的缩放因数将此 PointD 结构缩放指定的倍数的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构表示的缩放因数。</param>
        /// <returns>PointD 结构，表示按 SizeF 结构表示的缩放因数将此 PointD 结构缩放指定的倍数得到的结果。</returns>
        public PointD ScaleCopy(SizeF sz) => new PointD(_X * sz.Width, _Y * sz.Height);

        //

        /// <summary>
        /// 将此 PointD 结构在指定的基向量方向的分量翻转。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        public void Reflect(int index)
        {
            switch (index)
            {
                case 0: _X = -_X; break;
                case 1: _Y = -_Y; break;
                default: throw new IndexOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// 将此 PointD 结构在 X 轴的分量翻转。
        /// </summary>
        public void ReflectX() => _X = -_X;

        /// <summary>
        /// 将此 PointD 结构在 Y 轴的分量翻转。
        /// </summary>
        public void ReflectY() => _Y = -_Y;

        /// <summary>
        /// 返回将此 PointD 结构在指定的基向量方向的分量翻转的 PointD 结构的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        /// <returns>PointD 结构，表示将此 PointD 结构在指定的基向量方向的分量翻转得到的结果。</returns>
        public PointD ReflectCopy(int index)
        {
            switch (index)
            {
                case 0: return new PointD(-_X, _Y);
                case 1: return new PointD(_X, -_Y);
                default: throw new IndexOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// 返回将此 PointD 结构在 X 轴的分量翻转的 PointD 结构的新实例。
        /// </summary>
        /// <returns>PointD 结构，表示将此 PointD 结构在 X 轴的分量翻转得到的结果。</returns>
        public PointD ReflectXCopy() => new PointD(-_X, _Y);

        /// <summary>
        /// 返回将此 PointD 结构在 Y 轴的分量翻转的 PointD 结构的新实例。
        /// </summary>
        /// <returns>PointD 结构，表示将此 PointD 结构在 Y 轴的分量翻转得到的结果。</returns>
        public PointD ReflectYCopy() => new PointD(_X, -_Y);

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD 结构错切指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定与错切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与错切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向错切的角度（弧度）。</param>
        public void Shear(int index1, int index2, double angle) => _UpdateByVector(ToColumnVector().ShearCopy(index1, index2, angle));

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD 结构向 +X 轴错切指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构向 +X 轴错切的角度（弧度）。</param>
        public void ShearX(double angle) => _X += _Y * Math.Tan(angle);

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD 结构向 +Y 轴错切指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构向 +Y 轴错切的角度（弧度）。</param>
        public void ShearY(double angle) => _Y += _X * Math.Tan(angle);

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构错切指定的角度的 PointD 结构的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定与错切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与错切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向错切的角度（弧度）。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的弧度将此 PointD 结构错切指定的角度得到的结果。</returns>
        public PointD ShearCopy(int index1, int index2, double angle) => FromVector(ToColumnVector().ShearCopy(index1, index2, angle));

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构向 +X 轴错切指定的角度的 PointD 结构的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构向 +X 轴错切的角度（弧度）。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的弧度将此 PointD 结构向 +X 轴错切指定的角度得到的结果。</returns>
        public PointD ShearXCopy(double angle) => new PointD(_X + _Y * Math.Tan(angle), _Y);

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构向 +Y 轴错切指定的角度的 PointD 结构的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构向 +Y 轴错切的角度（弧度）。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的弧度将此 PointD 结构向 +Y 轴错切指定的角度得到的结果。</returns>
        public PointD ShearYCopy(double angle) => new PointD(_X, _Y + _X * Math.Tan(angle));

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD 结构旋转指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        public void Rotate(int index1, int index2, double angle) => _UpdateByVector(ToColumnVector().RotateCopy(index1, index2, angle));

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD 结构绕原点旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构绕原点旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public void Rotate(double angle)
        {
            double __X = _X, __Y = _Y;

            double cosA = Math.Cos(angle);
            double sinA = Math.Sin(angle);

            _X = __X * cosA - __Y * sinA;
            _Y = __Y * cosA + __X * sinA;
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD 结构绕指定的 PointD 结构旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构绕指定的 PointD 结构旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <param name="pt">PointD 结构，表示旋转中心。</param>
        public void Rotate(double angle, PointD pt)
        {
            double __X = _X, __Y = _Y;

            double cosA = Math.Cos(angle);
            double sinA = Math.Sin(angle);

            _X = (__X - pt._X) * cosA - (__Y - pt._Y) * sinA + pt._X;
            _Y = (__Y - pt._Y) * cosA + (__X - pt._X) * sinA + pt._Y;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构旋转指定的角度的 PointD 结构的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的弧度将此 PointD 结构旋转指定的角度得到的结果。</returns>
        public PointD RotateCopy(int index1, int index2, double angle) => FromVector(ToColumnVector().RotateCopy(index1, index2, angle));

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD 结构绕原点旋转指定的角度的 PointD 结构的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构绕原点旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <returns>PointD 结构，表示按双精度浮点数表示的弧度将此 PointD 结构绕原点旋转指定的角度得到的结果。</returns>
        public PointD RotateCopy(double angle)
        {
            PointD result = new PointD();

            double cosA = Math.Cos(angle);
            double sinA = Math.Sin(angle);

            result._X = _X * cosA - _Y * sinA;
            result._Y = _Y * cosA + _X * sinA;

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

            double cosA = Math.Cos(angle);
            double sinA = Math.Sin(angle);

            result._X = (_X - pt._X) * cosA - (_Y - pt._Y) * sinA + pt._X;
            result._Y = (_Y - pt._Y) * cosA + (_X - pt._X) * sinA + pt._Y;

            return result;
        }

        //

        /// <summary>
        /// 按 Matrix 对象表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 3x3 仿射矩阵（左矩阵）。</param>
        public void MatrixTransform(Matrix matrixLeft) => _UpdateByVector(ToColumnVector().MatrixTransformCopy(matrixLeft));

        /// <summary>
        /// 返回按 Matrix 对象表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行仿射变换的 PointD 结构的新实例。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）。</param>
        /// <returns>PointD 结构，表示按 Matrix 对象表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行仿射变换得到的结果。</returns>
        public PointD MatrixTransformCopy(Matrix matrixLeft) => FromVector(ToColumnVector().MatrixTransformCopy(matrixLeft));

        //

        /// <summary>
        /// 按 AffineTransformation 对象将此 PointD 结构进行仿射变换。
        /// </summary>
        /// <param name="affineTransformation">AffineTransformation 对象。</param>
        public void AffineTransform(AffineTransformation affineTransformation) => _UpdateByVector(ToColumnVector().AffineTransformCopy(affineTransformation));

        /// <summary>
        /// 返回按 AffineTransformation 对象将此 PointD 结构进行仿射变换得到的结果。
        /// </summary>
        /// <param name="affineTransformation">AffineTransformation 对象。</param>
        /// <returns>PointD 结构，表示按 AffineTransformation 对象将此 PointD 结构进行仿射变换得到的结果。</returns>
        public PointD AffineTransformCopy(AffineTransformation affineTransformation) => FromVector(ToColumnVector().AffineTransformCopy(affineTransformation));

        //

        /// <summary>
        /// 按 PointD 结构表示的 X 基向量、Y 基向量与偏移向量将此 PointD 结构进行仿射变换。
        /// </summary>
        /// <param name="ex">PointD 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD 结构表示的 Y 基向量。</param>
        /// <param name="offset">PointD 结构表示的偏移向量。</param>
        [Obsolete("请改为使用 MatrixTransform(Matrix) 方法")]
        public void AffineTransform(PointD ex, PointD ey, PointD offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[_Dimension + 1, _Dimension + 1]
            {
                { ex._X, ex._Y, 0 },
                { ey._X, ey._Y, 0 },
                { offset._X, offset._Y, 1 }
            });

            _UpdateByVector(ToColumnVector().AffineTransformCopy(matrixLeft));
        }

        /// <summary>
        /// 按 Matrix 对象表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 3x3 仿射矩阵（左矩阵）。</param>
        [Obsolete("请改为使用 MatrixTransform(Matrix) 方法")]
        public void AffineTransform(Matrix matrixLeft) => _UpdateByVector(ToColumnVector().AffineTransformCopy(matrixLeft));

        /// <summary>
        /// 按 Matrix 对象数组表示的 3x3 仿射矩阵（左矩阵）数组将此 PointD 结构进行仿射变换。
        /// </summary>
        /// <param name="matricesLeft">Matrix 对象数组，表示 3x3 仿射矩阵（左矩阵）数组。</param>
        [Obsolete("请改为使用 AffineTransform(AffineTransformation) 方法")]
        public void AffineTransform(params Matrix[] matricesLeft) => _UpdateByVector(ToColumnVector().AffineTransformCopy(matricesLeft));

        /// <summary>
        /// 按 Matrix 对象列表表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构进行仿射变换。
        /// </summary>
        /// <param name="matricesLeft">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）列表。</param>
        [Obsolete("请改为使用 AffineTransform(AffineTransformation) 方法")]
        public void AffineTransform(List<Matrix> matricesLeft) => _UpdateByVector(ToColumnVector().AffineTransformCopy(matricesLeft));

        /// <summary>
        /// 返回按 PointD 结构表示的 X 基向量、Y 基向量与偏移向量将此 PointD 结构进行仿射变换的 PointD 结构的新实例。
        /// </summary>
        /// <param name="ex">PointD 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD 结构表示的 Y 基向量。</param>
        /// <param name="offset">PointD 结构表示的偏移向量。</param>
        /// <returns>PointD 结构，表示按 PointD 结构表示的 X 基向量、Y 基向量与偏移向量将此 PointD 结构进行仿射变换得到的结果。</returns>
        [Obsolete("请改为使用 MatrixTransformCopy(Matrix) 方法")]
        public PointD AffineTransformCopy(PointD ex, PointD ey, PointD offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[_Dimension + 1, _Dimension + 1]
            {
                { ex._X, ex._Y, 0 },
                { ey._X, ey._Y, 0 },
                { offset._X, offset._Y, 1 }
            });

            return FromVector(ToColumnVector().AffineTransformCopy(matrixLeft));
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行仿射变换的 PointD 结构的新实例。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）。</param>
        /// <returns>PointD 结构，表示按 Matrix 对象表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行仿射变换得到的结果。</returns>
        [Obsolete("请改为使用 MatrixTransformCopy(Matrix) 方法")]
        public PointD AffineTransformCopy(Matrix matrixLeft) => FromVector(ToColumnVector().AffineTransformCopy(matrixLeft));

        /// <summary>
        /// 返回按 Matrix 对象数组表示的 3x3 仿射矩阵（左矩阵）数组将此 PointD 结构进行仿射变换的 PointD 结构的新实例。
        /// </summary>
        /// <param name="matricesLeft">Matrix 对象数组，表示 3x3 仿射矩阵（左矩阵）数组。</param>
        /// <returns>PointD 结构，表示按 Matrix 对象数组表示的 3x3 仿射矩阵（左矩阵）数组将此 PointD 结构进行仿射变换得到的结果。</returns>
        [Obsolete("请改为使用 AffineTransformCopy(AffineTransformation) 方法")]
        public PointD AffineTransformCopy(params Matrix[] matricesLeft) => FromVector(ToColumnVector().AffineTransformCopy(matricesLeft));

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构进行仿射变换的 PointD 结构的新实例。
        /// </summary>
        /// <param name="matricesLeft">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）列表。</param>
        /// <returns>PointD 结构，表示按 Matrix 对象列表表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构进行仿射变换得到的结果。</returns>
        [Obsolete("请改为使用 AffineTransformCopy(AffineTransformation) 方法")]
        public PointD AffineTransformCopy(List<Matrix> matricesLeft) => FromVector(ToColumnVector().AffineTransformCopy(matricesLeft));

        /// <summary>
        /// 按 PointD 结构表示的 X 基向量、Y 基向量与偏移向量将此 PointD 结构进行逆仿射变换。
        /// </summary>
        /// <param name="ex">PointD 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD 结构表示的 Y 基向量。</param>
        /// <param name="offset">PointD 结构表示的偏移向量。</param>
        [Obsolete("请改为使用 AffineTransform(AffineTransformation) 方法")]
        public void InverseAffineTransform(PointD ex, PointD ey, PointD offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[_Dimension + 1, _Dimension + 1]
            {
                { ex._X, ex._Y, 0 },
                { ey._X, ey._Y, 0 },
                { offset._X, offset._Y, 1 }
            });

            _UpdateByVector(ToColumnVector().InverseAffineTransformCopy(matrixLeft));
        }

        /// <summary>
        /// 按 Matrix 对象表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）。</param>
        [Obsolete("请改为使用 AffineTransform(AffineTransformation) 方法")]
        public void InverseAffineTransform(Matrix matrixLeft) => _UpdateByVector(ToColumnVector().InverseAffineTransformCopy(matrixLeft));

        /// <summary>
        /// 按 Matrix 对象数组表示的 3x3 仿射矩阵（左矩阵）数组将此 PointD 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matricesLeft">Matrix 对象数组，表示 3x3 仿射矩阵（左矩阵）数组。</param>
        [Obsolete("请改为使用 AffineTransform(AffineTransformation) 方法")]
        public void InverseAffineTransform(params Matrix[] matricesLeft) => _UpdateByVector(ToColumnVector().InverseAffineTransformCopy(matricesLeft));

        /// <summary>
        /// 按 Matrix 对象列表表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matricesLeft">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）列表。</param>
        [Obsolete("请改为使用 AffineTransform(AffineTransformation) 方法")]
        public void InverseAffineTransform(List<Matrix> matricesLeft) => _UpdateByVector(ToColumnVector().InverseAffineTransformCopy(matricesLeft));

        /// <summary>
        /// 返回按 PointD 结构表示的 X 基向量、Y 基向量与偏移向量将此 PointD 结构进行逆仿射变换的 PointD 结构的新实例。
        /// </summary>
        /// <param name="ex">PointD 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD 结构表示的 Y 基向量。</param>
        /// <param name="offset">PointD 结构表示的偏移向量。</param>
        /// <returns>PointD 结构，表示按 PointD 结构表示的 X 基向量、Y 基向量与偏移向量将此 PointD 结构进行逆仿射变换得到的结果。</returns>
        [Obsolete("请改为使用 AffineTransformCopy(AffineTransformation) 方法")]
        public PointD InverseAffineTransformCopy(PointD ex, PointD ey, PointD offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[_Dimension + 1, _Dimension + 1]
            {
                { ex._X, ex._Y, 0 },
                { ey._X, ey._Y, 0 },
                { offset._X, offset._Y, 1 }
            });

            return FromVector(ToColumnVector().InverseAffineTransformCopy(matrixLeft));
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行逆仿射变换的 PointD 结构的新实例。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）。</param>
        /// <returns>PointD 结构，表示按 Matrix 对象表示的 3x3 仿射矩阵（左矩阵）将此 PointD 结构进行逆仿射变换得到的结果。</returns>
        [Obsolete("请改为使用 AffineTransformCopy(AffineTransformation) 方法")]
        public PointD InverseAffineTransformCopy(Matrix matrixLeft) => FromVector(ToColumnVector().InverseAffineTransformCopy(matrixLeft));

        /// <summary>
        /// 返回按 Matrix 对象数组表示的 3x3 仿射矩阵（左矩阵）数组将此 PointD 结构进行逆仿射变换的 PointD 结构的新实例。
        /// </summary>
        /// <param name="matricesLeft">Matrix 对象数组，表示 3x3 仿射矩阵（左矩阵）数组。</param>
        /// <returns>PointD 结构，表示按 Matrix 对象数组表示的 3x3 仿射矩阵（左矩阵）数组将此 PointD 结构进行逆仿射变换得到的结果。</returns>
        [Obsolete("请改为使用 AffineTransformCopy(AffineTransformation) 方法")]
        public PointD InverseAffineTransformCopy(params Matrix[] matricesLeft) => FromVector(ToColumnVector().InverseAffineTransformCopy(matricesLeft));

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构进行逆仿射变换的 PointD 结构的新实例。
        /// </summary>
        /// <param name="matricesLeft">Matrix 对象列表，表示 3x3 仿射矩阵（左矩阵）列表。</param>
        /// <returns>PointD 结构，表示按 Matrix 对象列表表示的 3x3 仿射矩阵（左矩阵）列表将此 PointD 结构进行逆仿射变换得到的结果。</returns>
        [Obsolete("请改为使用 AffineTransformCopy(AffineTransformation) 方法")]
        public PointD InverseAffineTransformCopy(List<Matrix> matricesLeft) => FromVector(ToColumnVector().InverseAffineTransformCopy(matricesLeft));

        //

        /// <summary>
        /// 返回将此 PointD 结构转换为列向量的 Vector 的新实例。
        /// </summary>
        /// <returns>Vector 对象，表示将此 PointD 结构转换为列向量得到的结果。</returns>
        public Vector ToColumnVector() => Vector.UnsafeCreateInstance(VectorType.ColumnVector, _X, _Y);

        /// <summary>
        /// 返回将此 PointD 结构转换为行向量的 Vector 的新实例。
        /// </summary>
        /// <returns>Vector 对象，表示将此 PointD 结构转换为行向量得到的结果。</returns>
        public Vector ToRowVector() => Vector.UnsafeCreateInstance(VectorType.RowVector, _X, _Y);

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
        public PointF ToPointF() => new PointF((float)_X, (float)_Y);

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
        public SizeF ToSizeF() => new SizeF((float)_X, (float)_Y);

        /// <summary>
        /// 返回将此 PointD 结构转换为 Complex 结构的新实例。
        /// </summary>
        /// <returns>Complex 结构，表示转换的结果。</returns>
        public Complex ToComplex() => new Complex(_X, _Y);

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断两个 PointD 结构是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 PointD 结构。</param>
        /// <param name="right">用于比较的第二个 PointD 结构。</param>
        /// <returns>布尔值，表示两个 PointD 结构是否相等。</returns>
        public static bool Equals(PointD left, PointD right) => left.Equals(right);

        //

        /// <summary>
        /// 比较两个 PointD 结构的次序。
        /// </summary>
        /// <param name="left">用于比较的第一个 PointD 结构。</param>
        /// <param name="right">用于比较的第二个 PointD 结构。</param>
        /// <returns>32 位整数，表示将两个 PointD 结构进行次序比较得到的结果。</returns>
        public static int Compare(PointD left, PointD right) => left.CompareTo(right);

        //

        /// <summary>
        /// 返回将 Vector 对象转换为 PointD 结构的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象。</param>
        /// <returns>PointD 结构，表示转换的结果。</returns>
        public static PointD FromVector(Vector vector)
        {
            if (Vector.IsNullOrEmpty(vector) || vector.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }

            //

            return new PointD(vector.UnsafeGetData()[0], vector.UnsafeGetData()[1]);
        }

        //

        /// <summary>
        /// 返回将 Point 结构转换为 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">Point 结构。</param>
        /// <returns>PointD 结构，表示转换的结果。</returns>
        public static PointD FromPoint(Point pt) => new PointD(pt);

        /// <summary>
        /// 返回将 PointF 结构转换为 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointF 结构。</param>
        /// <returns>PointD 结构，表示转换的结果。</returns>
        public static PointD FromPointF(PointF pt) => new PointD(pt);

        /// <summary>
        /// 返回将 Size 结构转换为 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构。</param>
        /// <returns>PointD 结构，表示转换的结果。</returns>
        public static PointD FromSize(Size sz) => new PointD(sz);

        /// <summary>
        /// 返回将 SizeF 结构转换为 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构。</param>
        /// <returns>PointD 结构，表示转换的结果。</returns>
        public static PointD FromSizeF(SizeF sz) => new PointD(sz);

        /// <summary>
        /// 返回将 Complex 结构转换为 PointD 结构的新实例。
        /// </summary>
        /// <param name="comp">Complex 结构。</param>
        /// <returns>PointD 结构，表示转换的结果。</returns>
        public static PointD FromComplex(Complex comp) => new PointD(comp);

        //

        /// <summary>
        /// 返回单位矩阵，表示不对 PointD 结构进行仿射变换的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <returns>Matrix 对象，表示不对 PointD 结构进行仿射变换的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix IdentityMatrix() => Matrix.Identity(_Dimension + 1);

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的位移将 PointD 结构在指定的基向量方向的分量平移指定的量的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定平移的分量所在方向的基向量。</param>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的位移将 PointD 结构在指定的基向量方向的分量平移指定的量的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(int index, double d) => Vector.OffsetMatrix(VectorType.ColumnVector, _Dimension, index, d);

        /// <summary>
        /// 返回表示按双精度浮点数表示的位移将 PointD 结构的所有分量平移指定的量的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的位移将 PointD 结构的所有分量平移指定的量的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(double d) => Vector.OffsetMatrix(VectorType.ColumnVector, _Dimension, d);

        /// <summary>
        /// 返回表示按 PointD 结构表示的位移将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构表示的位移。</param>
        /// <returns>Matrix 对象，表示按 PointD 结构表示的位移将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(PointD pt) => Vector.OffsetMatrix(pt.ToColumnVector());

        /// <summary>
        /// 返回表示按双精度浮点数表示的 X 坐标位移与 Y 坐标位移将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的 X 坐标位移与 Y 坐标位移将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(double dx, double dy) => Vector.OffsetMatrix(Vector.UnsafeCreateInstance(VectorType.ColumnVector, dx, dy));

        /// <summary>
        /// 返回表示按双精度浮点数表示的位移将 PointD 结构在 X 轴的分量平移指定的量的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的位移将 PointD 结构在 X 轴的分量平移指定的量的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetXMatrix(double d) => Vector.OffsetMatrix(Vector.UnsafeCreateInstance(VectorType.ColumnVector, d, 0));

        /// <summary>
        /// 返回表示按双精度浮点数表示的位移将 PointD 结构在 Y 轴的分量平移指定的量的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的位移将 PointD 结构在 Y 轴的分量平移指定的量的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetYMatrix(double d) => Vector.OffsetMatrix(Vector.UnsafeCreateInstance(VectorType.ColumnVector, 0, d));

        /// <summary>
        /// 返回表示按 Point 结构表示的位移将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">Point 结构表示的位移。</param>
        /// <returns>Matrix 对象，表示按 Point 结构表示的位移将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(Point pt) => Vector.OffsetMatrix(Vector.UnsafeCreateInstance(VectorType.ColumnVector, pt.X, pt.Y));

        /// <summary>
        /// 返回表示按 PointF 结构表示的位移将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointF 结构表示的位移。</param>
        /// <returns>Matrix 对象，表示按 PointF 结构表示的位移将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(PointF pt) => Vector.OffsetMatrix(Vector.UnsafeCreateInstance(VectorType.ColumnVector, pt.X, pt.Y));

        /// <summary>
        /// 返回表示按 Size 结构表示的位移将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sz">Size 结构表示的位移。</param>
        /// <returns>Matrix 对象，表示按 Size 结构表示的位移将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(Size sz) => Vector.OffsetMatrix(Vector.UnsafeCreateInstance(VectorType.ColumnVector, sz.Width, sz.Height));

        /// <summary>
        /// 返回表示按 SizeF 结构表示的位移将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构表示的位移。</param>
        /// <returns>Matrix 对象，表示按 SizeF 结构表示的位移将 PointD 结构平移指定的量的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(SizeF sz) => Vector.OffsetMatrix(Vector.UnsafeCreateInstance(VectorType.ColumnVector, sz.Width, sz.Height));

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的缩放因数将 PointD 结构在指定的基向量方向的分量缩放指定的倍数的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定平移的分量所在方向的基向量。</param>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的缩放因数将 PointD 结构在指定的基向量方向的分量缩放指定的倍数的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(int index, double s) => Vector.ScaleMatrix(_Dimension, index, s);

        /// <summary>
        /// 返回表示按双精度浮点数表示的缩放因数将 PointD 结构的所有分量缩放指定的倍数的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的缩放因数将 PointD 结构的所有分量缩放指定的倍数的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(double s) => Vector.ScaleMatrix(_Dimension, s);

        /// <summary>
        /// 返回表示按 PointD 结构表示的缩放因数将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按 PointD 结构表示的缩放因数将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(PointD pt) => Vector.ScaleMatrix(pt.ToColumnVector());

        /// <summary>
        /// 返回表示按双精度浮点数表示的 X 坐标缩放因数与 Y 坐标缩放因数将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的 X 坐标缩放因数与 Y 坐标缩放因数将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(double sx, double sy) => Vector.ScaleMatrix(Vector.UnsafeCreateInstance(VectorType.ColumnVector, sx, sy));

        /// <summary>
        /// 返回表示按双精度浮点数表示的缩放因数将 PointD 结构在 X 轴的分量缩放指定的倍数的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的缩放因数将 PointD 结构在 X 轴的分量缩放指定的倍数的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleXMatrix(double s) => Vector.ScaleMatrix(Vector.UnsafeCreateInstance(VectorType.ColumnVector, s, 0));

        /// <summary>
        /// 返回表示按双精度浮点数表示的缩放因数将 PointD 结构在 Y 轴的分量缩放指定的倍数的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的缩放因数将 PointD 结构在 Y 轴的分量缩放指定的倍数的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleYMatrix(double s) => Vector.ScaleMatrix(Vector.UnsafeCreateInstance(VectorType.ColumnVector, 0, s));

        /// <summary>
        /// 返回表示按 Point 结构表示的缩放因数将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">Point 结构表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按 Point 结构表示的缩放因数将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(Point pt) => Vector.ScaleMatrix(Vector.UnsafeCreateInstance(VectorType.ColumnVector, pt.X, pt.Y));

        /// <summary>
        /// 返回表示按 PointF 结构表示的缩放因数将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointF 结构表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按 PointF 结构表示的缩放因数将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(PointF pt) => Vector.ScaleMatrix(Vector.UnsafeCreateInstance(VectorType.ColumnVector, pt.X, pt.Y));

        /// <summary>
        /// 返回表示按 Size 结构表示的缩放因数将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sz">Size 结构表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按 Size 结构表示的缩放因数将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(Size sz) => Vector.ScaleMatrix(Vector.UnsafeCreateInstance(VectorType.ColumnVector, sz.Width, sz.Height));

        /// <summary>
        /// 返回表示按 SizeF 结构表示的缩放因数将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按 SizeF 结构表示的缩放因数将 PointD 结构缩放指定的倍数的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(SizeF sz) => Vector.ScaleMatrix(Vector.UnsafeCreateInstance(VectorType.ColumnVector, sz.Width, sz.Height));

        //

        /// <summary>
        /// 返回表示将 PointD 结构在指定的基向量方向的分量翻转的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        /// <returns>Matrix 对象，表示将 PointD 结构在指定的基向量方向的分量翻转的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ReflectMatrix(int index) => Vector.ReflectMatrix(_Dimension, index);

        /// <summary>
        /// 返回表示将 PointD 结构在 X 轴的分量翻转的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <returns>Matrix 对象，表示将 PointD 结构在 X 轴的分量翻转的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ReflectXMatrix() => ReflectMatrix(0);

        /// <summary>
        /// 返回表示将 PointD 结构在 Y 轴的分量翻转的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <returns>Matrix 对象，表示将 PointD 结构在 Y 轴的分量翻转的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ReflectYMatrix() => ReflectMatrix(1);

        //

        /// <summary>
        /// 返回表示用于错切 PointD 结构的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定与错切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与错切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向错切的角度（弧度）。</param>
        /// <returns>Matrix 对象，表示用于错切 PointD 结构的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ShearMatrix(int index1, int index2, double angle) => Vector.ShearMatrix(VectorType.ColumnVector, _Dimension, index1, index2, angle);

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD 结构向 +X 轴错切指定的角度的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构向 +X 轴错切的角度（弧度）。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的弧度将 PointD 结构向 +X 轴错切指定的角度的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ShearXMatrix(double angle) => ShearMatrix(0, 1, angle);

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD 结构向 +Y 轴错切指定的角度的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD 结构向 +Y 轴错切的角度（弧度）。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的弧度将 PointD 结构向 +Y 轴错切指定的角度的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix ShearYMatrix(double angle) => ShearMatrix(1, 0, angle);

        //

        /// <summary>
        /// 返回表示用于旋转 PointD 结构的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示 PointD 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        /// <returns>Matrix 对象，表示用于旋转 PointD 结构的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix RotateMatrix(int index1, int index2, double angle) => Vector.RotateMatrix(VectorType.ColumnVector, _Dimension, index1, index2, angle);

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD 结构绕原点旋转指定的角度的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD 结构绕原点旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的弧度将 PointD 结构绕原点旋转指定的角度的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix RotateMatrix(double angle) => RotateMatrix(0, 1, angle);

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD 结构绕指定的 PointD 结构旋转指定的角度的 3x3 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD 结构绕指定的 PointD 结构旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <param name="pt">PointD 结构，表示旋转中心。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的弧度将 PointD 结构绕指定的 PointD 结构旋转指定的角度的 3x3 仿射矩阵（左矩阵）。</returns>
        public static Matrix RotateMatrix(double angle, PointD pt)
        {
            double cosA = Math.Cos(angle);
            double sinA = Math.Sin(angle);

            return Matrix.UnsafeCreateInstance(new double[_Dimension + 1, _Dimension + 1]
            {
                { cosA, sinA, 0 },
                { -sinA, cosA, 0 },
                { -pt._X * cosA + pt._Y * sinA + pt._X, -pt._X * sinA - pt._Y * cosA + pt._Y, 1 }
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
            double absDx = Math.Abs(left._X - right._X);
            double absDy = Math.Abs(left._Y - right._Y);

            double absMax = Math.Max(absDx, absDy);

            if (absMax == 0)
            {
                return 0;
            }
            else
            {
                absDx /= absMax;
                absDy /= absMax;

                double sqrSum = absDx * absDx + absDy * absDy;

                return absMax * Math.Sqrt(sqrSum);
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
                double modProduct = left.Module * right.Module;
                double cosA = left._X * right._X / modProduct + left._Y * right._Y / modProduct;

                return Math.Acos(cosA);
            }
        }

        //

        /// <summary>
        /// 返回两个 PointD 结构的内积。
        /// </summary>
        /// <param name="left">第一个 PointD 结构。</param>
        /// <param name="right">第二个 PointD 结构。</param>
        /// <returns>双精度浮点数，表示两个 PointD 结构的内积。</returns>
        public static double DotProduct(PointD left, PointD right) => left._X * right._X + left._Y * right._Y;

        /// <summary>
        /// 返回两个 PointD 结构的外积。
        /// </summary>
        /// <param name="left">第一个 PointD 结构。</param>
        /// <param name="right">第二个 PointD 结构。</param>
        /// <returns>双精度浮点数，表示两个 PointD 结构的外积。</returns>
        public static double CrossProduct(PointD left, PointD right) => left._X * right._Y - left._Y * right._X;

        //

        /// <summary>
        /// 返回将 PointD 结构的所有分量取符号数得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        /// <returns>PointD 结构，表示将 PointD 结构的所有分量取符号数得到的结果</returns>
        public static PointD Sign(PointD pt) => new PointD(double.IsNaN(pt._X) ? 0 : Math.Sign(pt._X), double.IsNaN(pt._Y) ? 0 : Math.Sign(pt._Y));

        /// <summary>
        /// 返回将 PointD 结构的所有分量取绝对值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        /// <returns>PointD 结构，表示将 PointD 结构的所有分量取绝对值得到的结果</returns>
        public static PointD Abs(PointD pt) => new PointD(Math.Abs(pt._X), Math.Abs(pt._Y));

        /// <summary>
        /// 返回将 PointD 结构的所有分量舍入到较大的整数值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        /// <returns>PointD 结构，表示将 PointD 结构的所有分量舍入到较大的整数值得到的结果</returns>
        public static PointD Ceiling(PointD pt) => new PointD(Math.Ceiling(pt._X), Math.Ceiling(pt._Y));

        /// <summary>
        /// 返回将 PointD 结构的所有分量舍入到较小的整数值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        /// <returns>PointD 结构，表示将 PointD 结构的所有分量舍入到较小的整数值得到的结果</returns>
        public static PointD Floor(PointD pt) => new PointD(Math.Floor(pt._X), Math.Floor(pt._Y));

        /// <summary>
        /// 返回将 PointD 结构的所有分量舍入到最接近的整数值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        /// <returns>PointD 结构，表示将 PointD 结构的所有分量舍入到最接近的整数值得到的结果</returns>
        public static PointD Round(PointD pt) => new PointD(Math.Round(pt._X), Math.Round(pt._Y));

        /// <summary>
        /// 返回将 PointD 结构的所有分量截断小数部分取整得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，用于转换的结构。</param>
        /// <returns>PointD 结构，表示将 PointD 结构的所有分量截断小数部分取整得到的结果</returns>
        public static PointD Truncate(PointD pt) => new PointD(Math.Truncate(pt._X), Math.Truncate(pt._Y));

        /// <summary>
        /// 返回将两个 PointD 结构的所有分量分别取最大值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD 结构，用于比较的第二个结构。</param>
        /// <returns>PointD 结构，表示将两个 PointD 结构的所有分量分别取最大值得到的结果</returns>
        public static PointD Max(PointD left, PointD right) => new PointD(Math.Max(left._X, right._X), Math.Max(left._Y, right._Y));

        /// <summary>
        /// 返回将两个 PointD 结构的所有分量分别取最小值得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD 结构，用于比较的第二个结构。</param>
        /// <returns>PointD 结构，表示将两个 PointD 结构的所有分量分别取最小值得到的结果</returns>
        public static PointD Min(PointD left, PointD right) => new PointD(Math.Min(left._X, right._X), Math.Min(left._Y, right._Y));

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 PointD 结构是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD 结构。</param>
        /// <returns>布尔值，表示两个 PointD 结构是否相等。</returns>
        public static bool operator ==(PointD left, PointD right) => left._X == right._X && left._Y == right._Y;

        /// <summary>
        /// 判断两个 PointD 结构是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD 结构。</param>
        /// <returns>布尔值，表示两个 PointD 结构是否不相等。</returns>
        public static bool operator !=(PointD left, PointD right) => left._X != right._X || left._Y != right._Y;

        /// <summary>
        /// 判断两个 PointD 结构的字典序是否前者小于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD 结构。</param>
        /// <returns>布尔值，表示两个 PointD 结构的字典序是否前者小于后者。</returns>
        public static bool operator <(PointD left, PointD right)
        {
            for (int i = 0; i < _Dimension; i++)
            {
                if (left[i] != right[i])
                {
                    return left[i] < right[i];
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
            for (int i = 0; i < _Dimension; i++)
            {
                if (left[i] != right[i])
                {
                    return left[i] > right[i];
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
            for (int i = 0; i < _Dimension; i++)
            {
                if (left[i] != right[i])
                {
                    return left[i] < right[i];
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
            for (int i = 0; i < _Dimension; i++)
            {
                if (left[i] != right[i])
                {
                    return left[i] > right[i];
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
        public static PointD operator +(PointD pt) => pt;

        /// <summary>
        /// 返回在 PointD 结构的所有分量前添加负号得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">运算符右侧的 PointD 结构。</param>
        /// <returns>PointD 结构，表示在 PointD 结构的所有分量前添加负号得到的结果。</returns>
        public static PointD operator -(PointD pt) => new PointD(-pt._X, -pt._Y);

        //

        /// <summary>
        /// 返回将 PointD 结构的所有分量与双精度浮点数相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被加数。</param>
        /// <param name="n">双精度浮点数，表示加数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构的所有分量与双精度浮点数相加得到的结果。</returns>
        public static PointD operator +(PointD pt, double n) => new PointD(pt._X + n, pt._Y + n);

        /// <summary>
        /// 返回将双精度浮点数与 PointD 结构的所有分量相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被加数。</param>
        /// <param name="pt">PointD 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将双精度浮点数与 PointD 结构的所有分量相加得到的结果。</returns>
        public static PointD operator +(double n, PointD pt) => new PointD(n + pt._X, n + pt._Y);

        /// <summary>
        /// 返回将 PointD 结构与 PointD 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被加数。</param>
        /// <param name="right">PointD 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 PointD 结构的所有分量对应相加得到的结果。</returns>
        public static PointD operator +(PointD left, PointD right) => new PointD(left._X + right._X, left._Y + right._Y);

        /// <summary>
        /// 返回将 PointD 结构与 Point 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被加数。</param>
        /// <param name="right">Point 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 Point 结构的所有分量对应相加得到的结果。</returns>
        public static PointD operator +(PointD left, Point right) => new PointD(left._X + right.X, left._Y + right.Y);

        /// <summary>
        /// 返回将 Point 结构与 PointD 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">Point 结构，表示被加数。</param>
        /// <param name="right">PointD 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 Point 结构与 PointD 结构的所有分量对应相加得到的结果。</returns>
        public static PointD operator +(Point left, PointD right) => new PointD(left.X + right._X, left.Y + right._Y);

        /// <summary>
        /// 返回将 PointD 结构与 PointF 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被加数。</param>
        /// <param name="right">PointF 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 PointF 结构的所有分量对应相加得到的结果。</returns>
        public static PointD operator +(PointD left, PointF right) => new PointD(left._X + right.X, left._Y + right.Y);

        /// <summary>
        /// 返回将 PointF 结构与 PointD 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointF 结构，表示被加数。</param>
        /// <param name="right">PointD 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 PointF 结构与 PointD 结构的所有分量对应相加得到的结果。</returns>
        public static PointD operator +(PointF left, PointD right) => new PointD(left.X + right._X, left.Y + right._Y);

        /// <summary>
        /// 返回将 PointD 结构与 Size 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被加数。</param>
        /// <param name="sz">Size 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 Size 结构的所有分量对应相加得到的结果。</returns>
        public static PointD operator +(PointD pt, Size sz) => new PointD(pt._X + sz.Width, pt._Y + sz.Height);

        /// <summary>
        /// 返回将 Size 结构与 PointD 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，表示被加数。</param>
        /// <param name="pt">PointD 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 Size 结构与 PointD 结构的所有分量对应相加得到的结果。</returns>
        public static PointD operator +(Size sz, PointD pt) => new PointD(sz.Width + pt._X, sz.Height + pt._Y);

        /// <summary>
        /// 返回将 PointD 结构与 SizeF 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被加数。</param>
        /// <param name="sz">SizeF 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 SizeF 结构的所有分量对应相加得到的结果。</returns>
        public static PointD operator +(PointD pt, SizeF sz) => new PointD(pt._X + sz.Width, pt._Y + sz.Height);

        /// <summary>
        /// 返回将 SizeF 结构与 PointD 结构的所有分量对应相加得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，表示被加数。</param>
        /// <param name="pt">PointD 结构，表示加数。</param>
        /// <returns>PointD 结构，表示将 SizeF 结构与 PointD 结构的所有分量对应相加得到的结果。</returns>
        public static PointD operator +(SizeF sz, PointD pt) => new PointD(sz.Width + pt._X, sz.Height + pt._Y);

        //

        /// <summary>
        /// 返回将 PointD 结构的所有分量与双精度浮点数相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被减数。</param>
        /// <param name="n">双精度浮点数，表示减数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构的所有分量与双精度浮点数相减得到的结果。</returns>
        public static PointD operator -(PointD pt, double n) => new PointD(pt._X - n, pt._Y - n);

        /// <summary>
        /// 返回将双精度浮点数与 PointD 结构的所有分量相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被减数。</param>
        /// <param name="pt">PointD 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将双精度浮点数与 PointD 结构的所有分量相减得到的结果。</returns>
        public static PointD operator -(double n, PointD pt) => new PointD(n - pt._X, n - pt._Y);

        /// <summary>
        /// 返回将 PointD 结构与 PointD 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被减数。</param>
        /// <param name="right">PointD 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 PointD 结构的所有分量对应相减得到的结果。</returns>
        public static PointD operator -(PointD left, PointD right) => new PointD(left._X - right._X, left._Y - right._Y);

        /// <summary>
        /// 返回将 PointD 结构与 Point 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被减数。</param>
        /// <param name="right">Point 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 Point 结构的所有分量对应相减得到的结果。</returns>
        public static PointD operator -(PointD left, Point right) => new PointD(left._X - right.X, left._Y - right.Y);

        /// <summary>
        /// 返回将 Point 结构与 PointD 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">Point 结构，表示被减数。</param>
        /// <param name="right">PointD 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 Point 结构与 PointD 结构的所有分量对应相减得到的结果。</returns>
        public static PointD operator -(Point left, PointD right) => new PointD(left.X - right._X, left.Y - right._Y);

        /// <summary>
        /// 返回将 PointD 结构与 PointF 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被减数。</param>
        /// <param name="right">PointF 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 PointF 结构的所有分量对应相减得到的结果。</returns>
        public static PointD operator -(PointD left, PointF right) => new PointD(left._X - right.X, left._Y - right.Y);

        /// <summary>
        /// 返回将 PointF 结构与 PointD 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointF 结构，表示被减数。</param>
        /// <param name="right">PointD 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 PointF 结构与 PointD 结构的所有分量对应相减得到的结果。</returns>
        public static PointD operator -(PointF left, PointD right) => new PointD(left.X - right._X, left.Y - right._Y);

        /// <summary>
        /// 返回将 PointD 结构与 Size 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被减数。</param>
        /// <param name="sz">Size 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 Size 结构的所有分量对应相减得到的结果。</returns>
        public static PointD operator -(PointD pt, Size sz) => new PointD(pt._X - sz.Width, pt._Y - sz.Height);

        /// <summary>
        /// 返回将 Size 结构与 PointD 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，表示被减数。</param>
        /// <param name="pt">PointD 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 Size 结构与 PointD 结构的所有分量对应相减得到的结果。</returns>
        public static PointD operator -(Size sz, PointD pt) => new PointD(sz.Width - pt._X, sz.Height - pt._Y);

        /// <summary>
        /// 返回将 PointD 结构与 SizeF 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被减数。</param>
        /// <param name="sz">SizeF 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 SizeF 结构的所有分量对应相减得到的结果。</returns>
        public static PointD operator -(PointD pt, SizeF sz) => new PointD(pt._X - sz.Width, pt._Y - sz.Height);

        /// <summary>
        /// 返回将 SizeF 结构与 PointD 结构的所有分量对应相减得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，表示被减数。</param>
        /// <param name="pt">PointD 结构，表示减数。</param>
        /// <returns>PointD 结构，表示将 SizeF 结构与 PointD 结构的所有分量对应相减得到的结果。</returns>
        public static PointD operator -(SizeF sz, PointD pt) => new PointD(sz.Width - pt._X, sz.Height - pt._Y);

        //

        /// <summary>
        /// 返回将 PointD 结构的所有分量与双精度浮点数相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被乘数。</param>
        /// <param name="n">双精度浮点数，表示乘数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构的所有分量与双精度浮点数相乘得到的结果。</returns>
        public static PointD operator *(PointD pt, double n) => new PointD(pt._X * n, pt._Y * n);

        /// <summary>
        /// 返回将双精度浮点数与 PointD 结构的所有分量相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被乘数。</param>
        /// <param name="pt">PointD 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将双精度浮点数与 PointD 结构的所有分量相乘得到的结果。</returns>
        public static PointD operator *(double n, PointD pt) => new PointD(n * pt._X, n * pt._Y);

        /// <summary>
        /// 返回将 PointD 结构与 PointD 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被乘数。</param>
        /// <param name="right">PointD 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 PointD 结构的所有分量对应相乘得到的结果。</returns>
        public static PointD operator *(PointD left, PointD right) => new PointD(left._X * right._X, left._Y * right._Y);

        /// <summary>
        /// 返回将 PointD 结构与 Point 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被乘数。</param>
        /// <param name="right">Point 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 Point 结构的所有分量对应相乘得到的结果。</returns>
        public static PointD operator *(PointD left, Point right) => new PointD(left._X * right.X, left._Y * right.Y);

        /// <summary>
        /// 返回将 Point 结构与 PointD 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">Point 结构，表示被乘数。</param>
        /// <param name="right">PointD 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 Point 结构与 PointD 结构的所有分量对应相乘得到的结果。</returns>
        public static PointD operator *(Point left, PointD right) => new PointD(left.X * right._X, left.Y * right._Y);

        /// <summary>
        /// 返回将 PointD 结构与 PointF 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被乘数。</param>
        /// <param name="right">PointF 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 PointF 结构的所有分量对应相乘得到的结果。</returns>
        public static PointD operator *(PointD left, PointF right) => new PointD(left._X * right.X, left._Y * right.Y);

        /// <summary>
        /// 返回将 PointF 结构与 PointD 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointF 结构，表示被乘数。</param>
        /// <param name="right">PointD 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 PointF 结构与 PointD 结构的所有分量对应相乘得到的结果。</returns>
        public static PointD operator *(PointF left, PointD right) => new PointD(left.X * right._X, left.Y * right._Y);

        /// <summary>
        /// 返回将 PointD 结构与 Size 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被乘数。</param>
        /// <param name="sz">Size 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 Size 结构的所有分量对应相乘得到的结果。</returns>
        public static PointD operator *(PointD pt, Size sz) => new PointD(pt._X * sz.Width, pt._Y * sz.Height);

        /// <summary>
        /// 返回将 Size 结构与 PointD 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，表示被乘数。</param>
        /// <param name="pt">PointD 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 Size 结构与 PointD 结构的所有分量对应相乘得到的结果。</returns>
        public static PointD operator *(Size sz, PointD pt) => new PointD(sz.Width * pt._X, sz.Height * pt._Y);

        /// <summary>
        /// 返回将 PointD 结构与 SizeF 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被乘数。</param>
        /// <param name="sz">SizeF 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 SizeF 结构的所有分量对应相乘得到的结果。</returns>
        public static PointD operator *(PointD pt, SizeF sz) => new PointD(pt._X * sz.Width, pt._Y * sz.Height);

        /// <summary>
        /// 返回将 SizeF 结构与 PointD 结构的所有分量对应相乘得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，表示被乘数。</param>
        /// <param name="pt">PointD 结构，表示乘数。</param>
        /// <returns>PointD 结构，表示将 SizeF 结构与 PointD 结构的所有分量对应相乘得到的结果。</returns>
        public static PointD operator *(SizeF sz, PointD pt) => new PointD(sz.Width * pt._X, sz.Height * pt._Y);

        //

        /// <summary>
        /// 返回将 PointD 结构的所有分量与双精度浮点数相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被除数。</param>
        /// <param name="n">双精度浮点数，表示除数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构的所有分量与双精度浮点数相除得到的结果。</returns>
        public static PointD operator /(PointD pt, double n) => new PointD(pt._X / n, pt._Y / n);

        /// <summary>
        /// 返回将双精度浮点数与 PointD 结构的所有分量相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被除数。</param>
        /// <param name="pt">PointD 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将双精度浮点数与 PointD 结构的所有分量相除得到的结果。</returns>
        public static PointD operator /(double n, PointD pt) => new PointD(n / pt._X, n / pt._Y);

        /// <summary>
        /// 返回将 PointD 结构与 PointD 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被除数。</param>
        /// <param name="right">PointD 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 PointD 结构的所有分量对应相除得到的结果。</returns>
        public static PointD operator /(PointD left, PointD right) => new PointD(left._X / right._X, left._Y / right._Y);

        /// <summary>
        /// 返回将 PointD 结构与 Point 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被除数。</param>
        /// <param name="right">Point 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 Point 结构的所有分量对应相除得到的结果。</returns>
        public static PointD operator /(PointD left, Point right) => new PointD(left._X / right.X, left._Y / right.Y);

        /// <summary>
        /// 返回将 Point 结构与 PointD 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">Point 结构，表示被除数。</param>
        /// <param name="right">PointD 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 Point 结构与 PointD 结构的所有分量对应相除得到的结果。</returns>
        public static PointD operator /(Point left, PointD right) => new PointD(left.X / right._X, left.Y / right._Y);

        /// <summary>
        /// 返回将 PointD 结构与 PointF 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointD 结构，表示被除数。</param>
        /// <param name="right">PointF 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 PointF 结构的所有分量对应相除得到的结果。</returns>
        public static PointD operator /(PointD left, PointF right) => new PointD(left._X / right.X, left._Y / right.Y);

        /// <summary>
        /// 返回将 PointF 结构与 PointD 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="left">PointF 结构，表示被除数。</param>
        /// <param name="right">PointD 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 PointF 结构与 PointD 结构的所有分量对应相除得到的结果。</returns>
        public static PointD operator /(PointF left, PointD right) => new PointD(left.X / right._X, left.Y / right._Y);

        /// <summary>
        /// 返回将 PointD 结构与 Size 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被除数。</param>
        /// <param name="sz">Size 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 Size 结构的所有分量对应相除得到的结果。</returns>
        public static PointD operator /(PointD pt, Size sz) => new PointD(pt._X / sz.Width, pt._Y / sz.Height);

        /// <summary>
        /// 返回将 Size 结构与 PointD 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">Size 结构，表示被除数。</param>
        /// <param name="pt">PointD 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 Size 结构与 PointD 结构的所有分量对应相除得到的结果。</returns>
        public static PointD operator /(Size sz, PointD pt) => new PointD(sz.Width / pt._X, sz.Height / pt._Y);

        /// <summary>
        /// 返回将 PointD 结构与 SizeF 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD 结构，表示被除数。</param>
        /// <param name="sz">SizeF 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 PointD 结构与 SizeF 结构的所有分量对应相除得到的结果。</returns>
        public static PointD operator /(PointD pt, SizeF sz) => new PointD(pt._X / sz.Width, pt._Y / sz.Height);

        /// <summary>
        /// 返回将 SizeF 结构与 PointD 结构的所有分量对应相除得到的 PointD 结构的新实例。
        /// </summary>
        /// <param name="sz">SizeF 结构，表示被除数。</param>
        /// <param name="pt">PointD 结构，表示除数。</param>
        /// <returns>PointD 结构，表示将 SizeF 结构与 PointD 结构的所有分量对应相除得到的结果。</returns>
        public static PointD operator /(SizeF sz, PointD pt) => new PointD(sz.Width / pt._X, sz.Height / pt._Y);

        //

        /// <summary>
        /// 将指定的 PointD 结构显式转换为 Point 结构。
        /// </summary>
        /// <param name="pt">用于转换的 PointD 结构。</param>
        /// <returns>Point 结构，表示显式转换的结果。</returns>
        public static explicit operator Point(PointD pt) => pt.ToPoint();

        /// <summary>
        /// 将指定的 PointD 结构显式转换为 PointF 结构。
        /// </summary>
        /// <param name="pt">用于转换的 PointD 结构。</param>
        /// <returns>PointF 结构，表示显式转换的结果。</returns>
        public static explicit operator PointF(PointD pt) => pt.ToPointF();

        /// <summary>
        /// 将指定的 PointD 结构显式转换为 Size 结构。
        /// </summary>
        /// <param name="pt">用于转换的 PointD 结构。</param>
        /// <returns>Size 结构，表示显式转换的结果。</returns>
        public static explicit operator Size(PointD pt) => pt.ToSize();

        /// <summary>
        /// 将指定的 PointD 结构显式转换为 SizeF 结构。
        /// </summary>
        /// <param name="pt">用于转换的 PointD 结构。</param>
        /// <returns>SizeF 结构，表示显式转换的结果。</returns>
        public static explicit operator SizeF(PointD pt) => pt.ToSizeF();

        /// <summary>
        /// 将指定的 PointD 结构显式转换为 Complex 结构。
        /// </summary>
        /// <param name="pt">用于转换的 PointD 结构。</param>
        /// <returns>Complex 结构，表示显式转换的结果。</returns>
        public static explicit operator Complex(PointD pt) => pt.ToComplex();

        /// <summary>
        /// 将指定的 Point 结构隐式转换为 PointD 结构。
        /// </summary>
        /// <param name="pt">用于转换的 Point 结构。</param>
        /// <returns>PointD 结构，表示隐式转换的结果。</returns>
        public static implicit operator PointD(Point pt) => new PointD(pt);

        /// <summary>
        /// 将指定的 PointF 结构隐式转换为 PointD 结构。
        /// </summary>
        /// <param name="pt">用于转换的 PointF 结构。</param>
        /// <returns>PointD 结构，表示隐式转换的结果。</returns>
        public static implicit operator PointD(PointF pt) => new PointD(pt);

        /// <summary>
        /// 将指定的 Size 结构显式转换为 PointD 结构。
        /// </summary>
        /// <param name="sz">用于转换的 Size 结构。</param>
        /// <returns>PointD 结构，表示显式转换的结果。</returns>
        public static explicit operator PointD(Size sz) => new PointD(sz);

        /// <summary>
        /// 将指定的 SizeF 结构显式转换为 PointD 结构。
        /// </summary>
        /// <param name="sz">用于转换的 SizeF 结构。</param>
        /// <returns>PointD 结构，表示显式转换的结果。</returns>
        public static explicit operator PointD(SizeF sz) => new PointD(sz);

        //

        /// <summary>
        /// 将指定的值元组隐式转换为 PointD 结构。
        /// </summary>
        /// <param name="tuple">用于转换的值元组。</param>
        /// <returns>PointD 结构，表示隐式转换的结果。</returns>
        public static implicit operator PointD((double X, double Y) tuple) => new PointD(tuple.X, tuple.Y);

        /// <summary>
        /// 将指定的 PointD 结构隐式转换为值元组。
        /// </summary>
        /// <param name="pt">用于转换的 PointD 结构。</param>
        /// <returns>值元组，表示隐式转换的结果。</returns>
        public static implicit operator (double X, double Y)(PointD pt) => (pt._X, pt._Y);

        #endregion

        #region 显式接口成员实现

        #region Com.IVector<T>

        int IVector<double>.Size
        {
            get => _Dimension;
            set => throw new NotSupportedException();
        }

        int IVector<double>.Capacity => _Dimension;

        void IVector<double>.Trim() => throw new NotSupportedException();

        #endregion

        #region System.Collections.IList

        object IList.this[int index]
        {
            get => this[index];

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!(value is double))
                {
                    throw new ArgumentException();
                }

                //

                this[index] = (double)value;
            }
        }

        int IList.Add(object item) => throw new NotSupportedException();

        void IList.Clear() => this = new PointD();

        bool IList.Contains(object item)
        {
            if (item is null || !(item is double))
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
            if (item is null || !(item is double))
            {
                return -1;
            }
            else
            {
                return IndexOf((double)item);
            }
        }

        void IList.Insert(int index, object item) => throw new NotSupportedException();

        void IList.Remove(object item) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();

        #endregion

        #region System.Collections.ICollection

        int ICollection.Count => _Dimension;

        object ICollection.SyncRoot => this;

        bool ICollection.IsSynchronized => false;

        void ICollection.CopyTo(Array array, int index)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Rank != 1)
            {
                throw new RankException();
            }

            if (array.Length - index < _Dimension)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }

            //

            ToArray().CopyTo(array, index);
        }

        #endregion

        #region System.Collections.IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

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
                    if (_Index < 0 || _Index >= _Dimension)
                    {
                        throw new IndexOutOfRangeException(nameof(_Index));
                    }

                    //

                    return _Pt[_Index];
                }
            }

            bool IEnumerator.MoveNext()
            {
                if (_Index >= _Dimension - 1)
                {
                    return false;
                }
                else
                {
                    _Index++;

                    return true;
                }
            }

            void IEnumerator.Reset() => _Index = -1;
        }

        #endregion

        #region System.Collections.Generic.IList<T>

        void IList<double>.Insert(int index, double item) => throw new NotSupportedException();

        void IList<double>.RemoveAt(int index) => throw new NotSupportedException();

        #endregion

        #region System.Collections.Generic.ICollection<T>

        int ICollection<double>.Count => _Dimension;

        void ICollection<double>.Add(double item) => throw new NotSupportedException();

        void ICollection<double>.Clear() => this = new PointD();

        bool ICollection<double>.Remove(double item) => throw new NotSupportedException();

        #endregion

        #region System.Collections.Generic.IEnumerable<out T>

        IEnumerator<double> IEnumerable<double>.GetEnumerator() => new GenericEnumerator(this);

        private sealed class GenericEnumerator : IEnumerator<double> // 实现 System.Collections.Generic.IEnumerator<out T> 的迭代器。
        {
            private PointD _Pt;
            private int _Index;

            internal GenericEnumerator(PointD pt)
            {
                _Pt = pt;
                _Index = -1;
            }

            void IDisposable.Dispose() { }

            object IEnumerator.Current
            {
                get
                {
                    if (_Index < 0 || _Index >= _Dimension)
                    {
                        throw new IndexOutOfRangeException(nameof(_Index));
                    }

                    //

                    return _Pt[_Index];
                }
            }

            bool IEnumerator.MoveNext()
            {
                if (_Index >= _Dimension - 1)
                {
                    return false;
                }
                else
                {
                    _Index++;

                    return true;
                }
            }

            void IEnumerator.Reset() => _Index = -1;

            double IEnumerator<double>.Current
            {
                get
                {
                    if (_Index < 0 || _Index >= _Dimension)
                    {
                        throw new IndexOutOfRangeException(nameof(_Index));
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