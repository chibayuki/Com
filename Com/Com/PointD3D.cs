/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.PointD3D
Version 19.11.7.0000

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
    /// 以一组有序的双精度浮点数表示的三维直角坐标系坐标。
    /// </summary>
    public struct PointD3D : IEquatable<PointD3D>, IComparable, IComparable<PointD3D>, IEuclideanVector<PointD3D>, IAffine<PointD3D>
    {
        #region 私有成员与内部成员

        private const int _Dimension = 3; // PointD3D 结构的维度。

        private static readonly Size _AffineMatrixSize = new Size(_Dimension + 1, _Dimension + 1); // PointD3D 结构的仿射矩阵大小。

        //

        private double _X; // X 坐标。
        private double _Y; // Y 坐标。
        private double _Z; // Z 坐标。

        //

        private void _UpdateByVector(Vector vector) // 按 Vector 对象更新此 PointD3D 结构。
        {
            if (Vector.IsNullOrEmpty(vector) || vector.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }

            //

            _X = vector[0];
            _Y = vector[1];
            _Z = vector[2];
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用双精度浮点数表示的 X 坐标、Y 坐标与 Z 坐标初始化 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="x">双精度浮点数表示的 X 坐标。</param>
        /// <param name="y">双精度浮点数表示的 Y 坐标。</param>
        /// <param name="z">双精度浮点数表示的 Z 坐标。</param>
        public PointD3D(double x, double y, double z)
        {
            _X = x;
            _Y = y;
            _Z = z;
        }

        #endregion

        #region 字段

        /// <summary>
        /// 表示零向量的 PointD3D 结构的实例。
        /// </summary>
        public static readonly PointD3D Zero = new PointD3D(0, 0, 0);

        //

        /// <summary>
        /// 表示所有分量为非数字的 PointD3D 结构的实例。
        /// </summary>
        public static readonly PointD3D NaN = new PointD3D(double.NaN, double.NaN, double.NaN);

        //

        /// <summary>
        /// 表示 X 基向量的 PointD3D 结构的实例。
        /// </summary>
        public static readonly PointD3D Ex = new PointD3D(1, 0, 0);

        /// <summary>
        /// 表示 Y 基向量的 PointD3D 结构的实例。
        /// </summary>
        public static readonly PointD3D Ey = new PointD3D(0, 1, 0);

        /// <summary>
        /// 表示 Z 基向量的 PointD3D 结构的实例。
        /// </summary>
        public static readonly PointD3D Ez = new PointD3D(0, 0, 1);

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置此 PointD3D 结构在指定的基向量方向的分量。
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
                    case 2: return _Z;
                    default: throw new IndexOutOfRangeException();
                }
            }

            set
            {
                switch (index)
                {
                    case 0: _X = value; break;
                    case 1: _Y = value; break;
                    case 2: _Z = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置此 PointD3D 结构在 X 轴的分量。
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
        /// 获取或设置此 PointD3D 结构在 Y 轴的分量。
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

        /// <summary>
        /// 获取或设置此 PointD3D 结构在 Z 轴的分量。
        /// </summary>
        public double Z
        {
            get
            {
                return _Z;
            }

            set
            {
                _Z = value;
            }
        }

        //

        /// <summary>
        /// 获取此 PointD3D 结构的维度。
        /// </summary>
        public int Dimension
        {
            get
            {
                return _Dimension;
            }
        }

        //

        /// <summary>
        /// 获取表示此 PointD3D 结构是否为空向量的布尔值。
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获取表示此 PointD3D 结构是否为零向量的布尔值。
        /// </summary>
        public bool IsZero
        {
            get
            {
                return (_X == 0 && _Y == 0 && _Z == 0);
            }
        }

        /// <summary>
        /// 获取表示此 PointD3D 结构是否只读的布尔值。
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获取表示此 PointD3D 结构是否具有固定的维度的布尔值。
        /// </summary>
        public bool IsFixedSize
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 获取表示此 PointD3D 结构是否包含非数字分量的布尔值。
        /// </summary>
        public bool IsNaN
        {
            get
            {
                return (double.IsNaN(_X) || double.IsNaN(_Y) || double.IsNaN(_Z));
            }
        }

        /// <summary>
        /// 获取表示此 PointD3D 结构是否包含无穷大分量的布尔值。
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return (!IsNaN && (double.IsInfinity(_X) || double.IsInfinity(_Y) || double.IsInfinity(_Z)));
            }
        }

        /// <summary>
        /// 获取表示此 PointD3D 结构是否包含非数字或无穷大分量的布尔值。
        /// </summary>
        public bool IsNaNOrInfinity
        {
            get
            {
                return (InternalMethod.IsNaNOrInfinity(_X) || InternalMethod.IsNaNOrInfinity(_Y) || InternalMethod.IsNaNOrInfinity(_Z));
            }
        }

        //

        /// <summary>
        /// 获取此 PointD3D 结构的模。
        /// </summary>
        public double Module
        {
            get
            {
                double AbsX = Math.Abs(_X);
                double AbsY = Math.Abs(_Y);
                double AbsZ = Math.Abs(_Z);

                double AbsMax = Math.Max(Math.Max(AbsX, AbsY), AbsZ);

                if (AbsMax == 0)
                {
                    return 0;
                }
                else
                {
                    AbsX /= AbsMax;
                    AbsY /= AbsMax;
                    AbsZ /= AbsMax;

                    double SqrSum = AbsX * AbsX + AbsY * AbsY + AbsZ * AbsZ;

                    return (AbsMax * Math.Sqrt(SqrSum));
                }
            }
        }

        /// <summary>
        /// 获取此 PointD3D 结构的模的平方。
        /// </summary>
        public double ModuleSquared
        {
            get
            {
                return (_X * _X + _Y * _Y + _Z * _Z);
            }
        }

        //

        /// <summary>
        /// 获取此 PointD3D 结构的相反向量。
        /// </summary>
        public PointD3D Opposite
        {
            get
            {
                return new PointD3D(-_X, -_Y, -_Z);
            }
        }

        /// <summary>
        /// 获取此 PointD3D 结构的规范化向量。
        /// </summary>
        public PointD3D Normalize
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
                    return new PointD3D(_X / Mod, _Y / Mod, _Z / Mod);
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置此 PointD3D 结构在 XY 平面的分量。
        /// </summary>
        public PointD XY
        {
            get
            {
                return new PointD(_X, _Y);
            }

            set
            {
                _X = value.X;
                _Y = value.Y;
            }
        }

        /// <summary>
        /// 获取或设置此 PointD3D 结构在 YZ 平面的分量。
        /// </summary>
        public PointD YZ
        {
            get
            {
                return new PointD(_Y, _Z);
            }

            set
            {
                _Y = value.X;
                _Z = value.Y;
            }
        }

        /// <summary>
        /// 获取或设置此 PointD3D 结构在 ZX 平面的分量。
        /// </summary>
        public PointD ZX
        {
            get
            {
                return new PointD(_Z, _X);
            }

            set
            {
                _Z = value.X;
                _X = value.Y;
            }
        }

        //

        /// <summary>
        /// 获取此 PointD3D 结构与 X 轴之间的夹角（弧度）。
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
        /// 获取此 PointD3D 结构与 Y 轴之间的夹角（弧度）。
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

        /// <summary>
        /// 获取此 PointD3D 结构与 Z 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleFromZ
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }
                else
                {
                    return AngleFrom(_Z >= 0 ? Ez : -Ez);
                }
            }
        }

        /// <summary>
        /// 获取此 PointD3D 结构与 XY 平面之间的夹角（弧度）。
        /// </summary>
        public double AngleFromXY
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }
                else
                {
                    return (Constant.HalfPi - AngleFromZ);
                }
            }
        }

        /// <summary>
        /// 获取此 PointD3D 结构与 YZ 平面之间的夹角（弧度）。
        /// </summary>
        public double AngleFromYZ
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }
                else
                {
                    return (Constant.HalfPi - AngleFromX);
                }
            }
        }

        /// <summary>
        /// 获取此 PointD3D 结构与 ZX 平面之间的夹角（弧度）。
        /// </summary>
        public double AngleFromZX
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }
                else
                {
                    return (Constant.HalfPi - AngleFromY);
                }
            }
        }

        //

        /// <summary>
        /// 获取此 PointD3D 结构的仰角。仰角是 PointD3D 结构与 +Z 轴之间的夹角（弧度）（以 +Z 轴为 0 弧度，远离 +Z 轴的方向为正方向）。
        /// </summary>
        public double Zenith
        {
            get
            {
                if (_X == 0 && _Y == 0 && _Z == 0)
                {
                    return 0;
                }
                else
                {
                    return Math.Acos(_Z / Module);
                }
            }
        }

        /// <summary>
        /// 获取此 PointD3D 结构的方位角。方位角是 PointD3D 结构在 XY 平面内的投影与 +X 轴之间的夹角（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。
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
        /// 判断此 PointD3D 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>布尔值，表示此 PointD3D 结构是否与指定的对象相等。</returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj == null || !(obj is PointD3D))
            {
                return false;
            }
            else
            {
                return Equals((PointD3D)obj);
            }
        }

        /// <summary>
        /// 返回此 PointD3D 结构的哈希代码。
        /// </summary>
        /// <returns>32 位整数，表示此 PointD3D 结构的哈希代码。</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 PointD3D 结构转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 PointD3D 结构的字符串形式。</returns>
        public override string ToString()
        {
            return string.Concat("{X=", _X, ", Y=", _Y, ", Z=", _Z, "}");
        }

        //

        /// <summary>
        /// 判断此 PointD3D 结构是否与指定的 PointD3D 结构相等。
        /// </summary>
        /// <param name="pt">用于比较的 PointD3D 结构。</param>
        /// <returns>布尔值，表示此 PointD3D 结构是否与指定的 PointD3D 结构相等。</returns>
        public bool Equals(PointD3D pt)
        {
            return (_X.Equals(pt._X) && _Y.Equals(pt._Y) && _Z.Equals(pt._Z));
        }

        //

        /// <summary>
        /// 将此 PointD3D 结构与指定的对象进行次序比较。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>32 位整数，表示将此 PointD3D 结构与指定的对象进行次序比较得到的结果。</returns>
        public int CompareTo(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return 0;
            }
            else if (obj == null || !(obj is PointD3D))
            {
                return 1;
            }
            else
            {
                return CompareTo((PointD3D)obj);
            }
        }

        /// <summary>
        /// 将此 PointD3D 结构与指定的 PointD3D 结构进行次序比较。
        /// </summary>
        /// <param name="pt">用于比较的 PointD3D 结构。</param>
        /// <returns>32 位整数，表示将此 PointD3D 结构与指定的 PointD3D 结构进行次序比较得到的结果。</returns>
        public int CompareTo(PointD3D pt)
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
        /// 遍历此 PointD3D 结构的所有分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int IndexOf(double item)
        {
            return Array.IndexOf(ToArray(), item, 0, _Dimension);
        }

        /// <summary>
        /// 从指定的索引开始遍历此 PointD3D 结构的所有分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int IndexOf(double item, int startIndex)
        {
            if (startIndex < 0 || startIndex >= _Dimension)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return Array.IndexOf(ToArray(), item, startIndex, _Dimension - startIndex);
        }

        /// <summary>
        /// 从指定的索引开始遍历此 PointD3D 结构指定数量的分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <param name="count">遍历的分量数量。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int IndexOf(double item, int startIndex, int count)
        {
            if ((startIndex < 0 || startIndex >= _Dimension) || count <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return Array.IndexOf(ToArray(), item, startIndex, Math.Min(_Dimension - startIndex, count));
        }

        /// <summary>
        /// 逆序遍历此 PointD3D 结构的所有分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int LastIndexOf(double item)
        {
            return Array.LastIndexOf(ToArray(), item, _Dimension - 1, _Dimension);
        }

        /// <summary>
        /// 从指定的索引开始逆序遍历此 PointD3D 结构的所有分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int LastIndexOf(double item, int startIndex)
        {
            if (startIndex < 0 || startIndex >= _Dimension)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return Array.LastIndexOf(ToArray(), item, startIndex, startIndex + 1);
        }

        /// <summary>
        /// 从指定的索引开始逆序遍历此 PointD3D 结构指定数量的分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <param name="count">遍历的分量数量。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int LastIndexOf(double item, int startIndex, int count)
        {
            if ((startIndex < 0 || startIndex >= _Dimension) || count <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return Array.LastIndexOf(ToArray(), item, startIndex, Math.Min(startIndex + 1, count));
        }

        /// <summary>
        /// 遍历此 PointD3D 结构的所有分量并返回表示是否存在与指定值相等的分量的布尔值。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>布尔值，表示是否存在与指定值相等的分量。</returns>
        public bool Contains(double item)
        {
            if (_X.Equals(item) || _Y.Equals(item) || _Z.Equals(item))
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
        /// 将此 PointD3D 结构转换为双精度浮点数数组。
        /// </summary>
        /// <returns>双精度浮点数数组，数组元素表示此 PointD3D 结构的分量。</returns>
        public double[] ToArray()
        {
            return new double[_Dimension] { _X, _Y, _Z };
        }

        /// <summary>
        /// 将此 PointD3D 结构转换为双精度浮点数列表。
        /// </summary>
        /// <returns>双精度浮点数列表，列表元素表示此 PointD3D 结构的分量。</returns>
        public List<double> ToList()
        {
            return new List<double>(_Dimension) { _X, _Y, _Z };
        }

        //

        /// <summary>
        /// 返回将此 PointD3D 结构表示的直角坐标系坐标转换为球坐标系坐标的 PointD3D 结构的新实例。
        /// </summary>
        /// <returns>PointD3D 结构，表示将此 PointD3D 结构表示的直角坐标系坐标转换为球坐标系坐标得到的结果。</returns>
        public PointD3D ToSpherical()
        {
            return new PointD3D(Module, Zenith, Azimuth);
        }

        /// <summary>
        /// 返回将此 PointD3D 结构表示的球坐标系坐标转换为直角坐标系坐标的 PointD3D 结构的新实例。
        /// </summary>
        /// <returns>PointD3D 结构，表示将此 PointD3D 结构表示的球坐标系坐标转换为直角坐标系坐标得到的结果。</returns>
        public PointD3D ToCartesian()
        {
            return new PointD3D(_X * Math.Sin(_Y) * Math.Cos(_Z), _X * Math.Sin(_Y) * Math.Sin(_Z), _X * Math.Cos(_Y));
        }

        //

        /// <summary>
        /// 返回此 PointD3D 结构与指定的 PointD3D 结构之间的距离。
        /// </summary>
        /// <param name="pt">PointD3D 结构，表示另一个向量。</param>
        /// <returns>双精度浮点数，表示此 PointD3D 结构与指定的 PointD3D 结构之间的距离。</returns>
        public double DistanceFrom(PointD3D pt)
        {
            double AbsDx = Math.Abs(_X - pt._X);
            double AbsDy = Math.Abs(_Y - pt._Y);
            double AbsDz = Math.Abs(_Z - pt._Z);

            double AbsMax = Math.Max(Math.Max(AbsDx, AbsDy), AbsDz);

            if (AbsMax == 0)
            {
                return 0;
            }
            else
            {
                AbsDx /= AbsMax;
                AbsDy /= AbsMax;
                AbsDz /= AbsMax;

                double SqrSum = AbsDx * AbsDx + AbsDy * AbsDy + AbsDz * AbsDz;

                return (AbsMax * Math.Sqrt(SqrSum));
            }
        }

        /// <summary>
        /// 返回此 PointD3D 结构与指定的 PointD3D 结构之间的夹角（弧度）。
        /// </summary>
        /// <param name="pt">PointD3D 结构，表示另一个向量。</param>
        /// <returns>双精度浮点数，表示此 PointD3D 结构与指定的 PointD3D 结构之间的夹角（弧度）。</returns>
        public double AngleFrom(PointD3D pt)
        {
            if (IsZero || pt.IsZero)
            {
                return 0;
            }
            else
            {
                double ModProduct = Module * pt.Module;
                double CosA = _X * pt._X / ModProduct + _Y * pt._Y / ModProduct + _Z * pt._Z / ModProduct;

                return Math.Acos(CosA);
            }
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的位移将此 PointD3D 结构在指定的基向量方向的分量平移指定的量。
        /// </summary>
        /// <param name="index">索引，用于指定平移的分量所在方向的基向量。</param>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public void Offset(int index, double d)
        {
            switch (index)
            {
                case 0: _X += d; break;
                case 1: _Y += d; break;
                case 2: _Z += d; break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的位移将此 PointD3D 结构的所有分量平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public void Offset(double d)
        {
            _X += d;
            _Y += d;
            _Z += d;
        }

        /// <summary>
        /// 按 PointD3D 结构表示的位移将此 PointD3D 结构平移指定的量。
        /// </summary>
        /// <param name="pt">PointD3D 结构表示的位移。</param>
        public void Offset(PointD3D pt)
        {
            _X += pt._X;
            _Y += pt._Y;
            _Z += pt._Z;
        }

        /// <summary>
        /// 按双精度浮点数表示的 X 坐标位移、Y 坐标位移与 Z 坐标位移将此 PointD3D 结构平移指定的量。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标位移。</param>
        public void Offset(double dx, double dy, double dz)
        {
            _X += dx;
            _Y += dy;
            _Z += dz;
        }

        /// <summary>
        /// 按双精度浮点数表示的位移将此 PointD3D 结构在 X 轴的分量平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public void OffsetX(double d)
        {
            _X += d;
        }

        /// <summary>
        /// 按双精度浮点数表示的位移将此 PointD3D 结构在 Y 轴的分量平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public void OffsetY(double d)
        {
            _Y += d;
        }

        /// <summary>
        /// 按双精度浮点数表示的位移将此 PointD3D 结构在 Z 轴的分量平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public void OffsetZ(double d)
        {
            _Z += d;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的位移将此 PointD3D 结构在指定的基向量方向的分量平移指定的量的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定平移的分量所在方向的基向量。</param>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的位移将此 PointD3D 结构在指定的基向量方向的分量平移指定的量得到的结果。</returns>
        public PointD3D OffsetCopy(int index, double d)
        {
            switch (index)
            {
                case 0: return new PointD3D(_X + d, _Y, _Z);
                case 1: return new PointD3D(_X, _Y + d, _Z);
                case 2: return new PointD3D(_X, _Y, _Z + d);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的位移将此 PointD3D 结构的所有分量平移指定的量的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的位移将此 PointD3D 结构的所有分量平移指定的量得到的结果。</returns>
        public PointD3D OffsetCopy(double d)
        {
            return new PointD3D(_X + d, _Y + d, _Z + d);
        }

        /// <summary>
        /// 返回按 PointD3D 结构表示的位移将此 PointD3D 结构平移指定的量的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构表示的位移。</param>
        /// <returns>PointD3D 结构，表示按 PointD3D 结构表示的位移将此 PointD3D 结构平移指定的量得到的结果。</returns>
        public PointD3D OffsetCopy(PointD3D pt)
        {
            return new PointD3D(_X + pt._X, _Y + pt._Y, _Z + pt._Z);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标位移、Y 坐标位移与 Z 坐标位移将此 PointD3D 结构平移指定的量的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标位移。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的 X 坐标位移、Y 坐标位移与 Z 坐标位移将此 PointD3D 结构平移指定的量得到的结果。</returns>
        public PointD3D OffsetCopy(double dx, double dy, double dz)
        {
            return new PointD3D(_X + dx, _Y + dy, _Z + dz);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的位移将此 PointD3D 结构在 X 轴的分量平移指定的量的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的位移将此 PointD3D 结构在 X 轴的分量平移指定的量得到的结果。</returns>
        public PointD3D OffsetXCopy(double d)
        {
            return new PointD3D(_X + d, _Y, _Z);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的位移将此 PointD3D 结构在 Y 轴的分量平移指定的量的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的位移将此 PointD3D 结构在 Y 轴的分量平移指定的量得到的结果。</returns>
        public PointD3D OffsetYCopy(double d)
        {
            return new PointD3D(_X, _Y + d, _Z);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的位移将此 PointD3D 结构在 Z 轴的分量平移指定的量的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的位移将此 PointD3D 结构在 Z 轴的分量平移指定的量得到的结果。</returns>
        public PointD3D OffsetZCopy(double d)
        {
            return new PointD3D(_X, _Y, _Z + d);
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的缩放因数将此 PointD3D 结构在指定的基向量方向的分量缩放指定的倍数。
        /// </summary>
        /// <param name="index">索引，用于指定缩放的分量所在方向的基向量。</param>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public void Scale(int index, double s)
        {
            switch (index)
            {
                case 0: _X *= s; break;
                case 1: _Y *= s; break;
                case 2: _Z *= s; break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的缩放因数将此 PointD3D 结构的所有分量缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public void Scale(double s)
        {
            _X *= s;
            _Y *= s;
            _Z *= s;
        }

        /// <summary>
        /// 按 PointD3D 结构表示的缩放因数将此 PointD3D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="pt">PointD3D 结构表示的缩放因数。</param>
        public void Scale(PointD3D pt)
        {
            _X *= pt._X;
            _Y *= pt._Y;
            _Z *= pt._Z;
        }

        /// <summary>
        /// 按双精度浮点数表示的 X 坐标缩放因数、Y 坐标缩放因数与 Z 坐标缩放因数将此 PointD3D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因数。</param>
        public void Scale(double sx, double sy, double sz)
        {
            _X *= sx;
            _Y *= sy;
            _Z *= sz;
        }

        /// <summary>
        /// 按双精度浮点数表示的缩放因数将此 PointD3D 结构在 X 轴的分量缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public void ScaleX(double s)
        {
            _X *= s;
        }

        /// <summary>
        /// 按双精度浮点数表示的缩放因数将此 PointD3D 结构在 Y 轴的分量缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public void ScaleY(double s)
        {
            _Y *= s;
        }

        /// <summary>
        /// 按双精度浮点数表示的缩放因数将此 PointD3D 结构在 Z 轴的分量缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public void ScaleZ(double s)
        {
            _Z *= s;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数将此 PointD3D 结构在指定的基向量方向的分量缩放指定的倍数的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定缩放的分量所在方向的基向量。</param>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的缩放因数将此 PointD3D 结构在指定的基向量方向的分量缩放指定的倍数得到的结果。</returns>
        public PointD3D ScaleCopy(int index, double s)
        {
            switch (index)
            {
                case 0: return new PointD3D(_X * s, _Y, _Z);
                case 1: return new PointD3D(_X, _Y * s, _Z);
                case 2: return new PointD3D(_X, _Y, _Z * s);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数将此 PointD3D 结构的所有分量缩放指定的倍数的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的缩放因数将此 PointD3D 结构的所有分量缩放指定的倍数得到的结果。</returns>
        public PointD3D ScaleCopy(double s)
        {
            return new PointD3D(_X * s, _Y * s, _Z * s);
        }

        /// <summary>
        /// 返回按 PointD3D 结构表示的缩放因数将此 PointD3D 结构缩放指定的倍数的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构表示的缩放因数。</param>
        /// <returns>PointD3D 结构，表示按 PointD3D 结构表示的缩放因数将此 PointD3D 结构缩放指定的倍数得到的结果。</returns>
        public PointD3D ScaleCopy(PointD3D pt)
        {
            return new PointD3D(_X * pt._X, _Y * pt._Y, _Z * pt._Z);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标缩放因数、Y 坐标缩放因数与 Z 坐标缩放因数将此 PointD3D 结构缩放指定的倍数的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因数。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的 X 坐标缩放因数、Y 坐标缩放因数与 Z 坐标缩放因数将此 PointD3D 结构缩放指定的倍数得到的结果。</returns>
        public PointD3D ScaleCopy(double sx, double sy, double sz)
        {
            return new PointD3D(_X * sx, _Y * sy, _Z * sz);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数将此 PointD3D 结构在 X 轴的分量缩放指定的倍数的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的缩放因数将此 PointD3D 结构在 X 轴的分量缩放指定的倍数得到的结果。</returns>
        public PointD3D ScaleXCopy(double s)
        {
            return new PointD3D(_X * s, _Y, _Z);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数将此 PointD3D 结构在 Y 轴的分量缩放指定的倍数的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的缩放因数将此 PointD3D 结构在 Y 轴的分量缩放指定的倍数得到的结果。</returns>
        public PointD3D ScaleYCopy(double s)
        {
            return new PointD3D(_X, _Y * s, _Z);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数将此 PointD3D 结构在 Z 轴的分量缩放指定的倍数的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的缩放因数将此 PointD3D 结构在 Z 轴的分量缩放指定的倍数得到的结果。</returns>
        public PointD3D ScaleZCopy(double s)
        {
            return new PointD3D(_X, _Y, _Z * s);
        }

        //

        /// <summary>
        /// 将此 PointD3D 结构在指定的基向量方向的分量翻转。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        public void Reflect(int index)
        {
            switch (index)
            {
                case 0: _X = -_X; break;
                case 1: _Y = -_Y; break;
                case 2: _Z = -_Z; break;
            }
        }

        /// <summary>
        /// 将此 PointD3D 结构在 X 轴的分量翻转。
        /// </summary>
        public void ReflectX()
        {
            _X = -_X;
        }

        /// <summary>
        /// 将此 PointD3D 结构在 Y 轴的分量翻转。
        /// </summary>
        public void ReflectY()
        {
            _Y = -_Y;
        }

        /// <summary>
        /// 将此 PointD3D 结构在 Z 轴的分量翻转。
        /// </summary>
        public void ReflectZ()
        {
            _Z = -_Z;
        }

        /// <summary>
        /// 返回将此 PointD3D 结构在指定的基向量方向的分量翻转的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        /// <returns>PointD3D 结构，表示将此 PointD3D 结构在指定的基向量方向的分量翻转得到的结果。</returns>
        public PointD3D ReflectCopy(int index)
        {
            switch (index)
            {
                case 0: return new PointD3D(-_X, _Y, _Z);
                case 1: return new PointD3D(_X, -_Y, _Z);
                case 2: return new PointD3D(_X, _Y, -_Z);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 返回将此 PointD3D 结构在 X 轴的分量翻转的 PointD3D 结构的新实例。
        /// </summary>
        /// <returns>PointD3D 结构，表示将此 PointD3D 结构在 X 轴的分量翻转得到的结果。</returns>
        public PointD3D ReflectXCopy()
        {
            return new PointD3D(-_X, _Y, _Z);
        }

        /// <summary>
        /// 返回将此 PointD3D 结构在 Y 轴的分量翻转的 PointD3D 结构的新实例。
        /// </summary>
        /// <returns>PointD3D 结构，表示将此 PointD3D 结构在 Y 轴的分量翻转得到的结果。</returns>
        public PointD3D ReflectYCopy()
        {
            return new PointD3D(_X, -_Y, _Z);
        }

        /// <summary>
        /// 返回将此 PointD3D 结构在 Z 轴的分量翻转的 PointD3D 结构的新实例。
        /// </summary>
        /// <returns>PointD3D 结构，表示将此 PointD3D 结构在 Z 轴的分量翻转得到的结果。</returns>
        public PointD3D ReflectZCopy()
        {
            return new PointD3D(_X, _Y, -_Z);
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构剪切指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向剪切的角度（弧度）。</param>
        public void Shear(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().ShearCopy(index1, index2, angle);

            _UpdateByVector(result);
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构在 XY 平面向 +X 轴剪切指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 XY 平面向 +X 轴剪切的角度（弧度）。</param>
        public void ShearXY(double angle)
        {
            _X += _Y * Math.Tan(angle);
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构在 XY 平面向 +Y 轴剪切指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 XY 平面向 +Y 轴剪切的角度（弧度）。</param>
        public void ShearYX(double angle)
        {
            _Y += _X * Math.Tan(angle);
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构在 YZ 平面向 +Y 轴剪切指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 YZ 平面向 +Y 轴剪切的角度（弧度）。</param>
        public void ShearYZ(double angle)
        {
            _Y += _Z * Math.Tan(angle);
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构在 YZ 平面向 +Z 轴剪切指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 YZ 平面向 +Z 轴剪切的角度（弧度）。</param>
        public void ShearZY(double angle)
        {
            _Z += _Y * Math.Tan(angle);
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构在 ZX 平面向 +Z 轴剪切指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 ZX 平面向 +Z 轴剪切的角度（弧度）。</param>
        public void ShearZX(double angle)
        {
            _Z += _X * Math.Tan(angle);
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构在 ZX 平面向 +X 轴剪切指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 ZX 平面向 +X 轴剪切的角度（弧度）。</param>
        public void ShearXZ(double angle)
        {
            _X += _Z * Math.Tan(angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构剪切指定的角度的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向剪切的角度（弧度）。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的弧度将此 PointD3D 结构剪切指定的角度得到的结果。</returns>
        public PointD3D ShearCopy(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().ShearCopy(index1, index2, angle);

            return FromVector(result);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构在 XY 平面向 +X 轴剪切指定的角度的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 XY 平面向 +X 轴剪切的角度（弧度）。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的弧度将此 PointD3D 结构在 XY 平面向 +X 轴剪切指定的角度得到的结果。</returns>
        public PointD3D ShearXYCopy(double angle)
        {
            return new PointD3D(_X + _Y * Math.Tan(angle), _Y, _Z);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构在 XY 平面向 +Y 轴剪切指定的角度的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 XY 平面向 +Y 轴剪切的角度（弧度）。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的弧度将此 PointD3D 结构在 XY 平面向 +Y 轴剪切指定的角度得到的结果。</returns>
        public PointD3D ShearYXCopy(double angle)
        {
            return new PointD3D(_X, _Y + _X * Math.Tan(angle), _Z);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构在 YZ 平面向 +Y 轴剪切指定的角度的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 YZ 平面向 +Y 轴剪切的角度（弧度）。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的弧度将此 PointD3D 结构在 YZ 平面向 +Y 轴剪切指定的角度得到的结果。</returns>
        public PointD3D ShearYZCopy(double angle)
        {
            return new PointD3D(_X, _Y + _Z * Math.Tan(angle), _Z);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构在 YZ 平面向 +Z 轴剪切指定的角度的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 YZ 平面向 +Z 轴剪切的角度（弧度）。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的弧度将此 PointD3D 结构在 YZ 平面向 +Z 轴剪切指定的角度得到的结果。</returns>
        public PointD3D ShearZYCopy(double angle)
        {
            return new PointD3D(_X, _Y, _Z + _Y * Math.Tan(angle));
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构在 ZX 平面向 +Z 轴剪切指定的角度的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 ZX 平面向 +Z 轴剪切的角度（弧度）。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的弧度将此 PointD3D 结构在 ZX 平面向 +Z 轴剪切指定的角度得到的结果。</returns>
        public PointD3D ShearZXCopy(double angle)
        {
            return new PointD3D(_X, _Y, _Z + _X * Math.Tan(angle));
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构在 ZX 平面向 +X 轴剪切指定的角度的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 ZX 平面向 +X 轴剪切的角度（弧度）。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的弧度将此 PointD3D 结构在 ZX 平面向 +X 轴剪切指定的角度得到的结果。</returns>
        public PointD3D ShearXZCopy(double angle)
        {
            return new PointD3D(_X + _Z * Math.Tan(angle), _Y, _Z);
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构旋转指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        public void Rotate(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().RotateCopy(index1, index2, angle);

            _UpdateByVector(result);
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构绕 X 轴旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构绕 X 轴旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        public void RotateX(double angle)
        {
            double __Y = _Y, __Z = _Z;

            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            _Y = __Y * CosA - __Z * SinA;
            _Z = __Z * CosA + __Y * SinA;
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构绕 Y 轴旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构绕 Y 轴旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +X 轴的方向为正方向）。</param>
        public void RotateY(double angle)
        {
            double __Z = _Z, __X = _X;

            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            _Z = __Z * CosA - __X * SinA;
            _X = __X * CosA + __Z * SinA;
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构绕 Z 轴旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构绕 Z 轴旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public void RotateZ(double angle)
        {
            double __X = _X, __Y = _Y;

            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            _X = __X * CosA - __Y * SinA;
            _Y = __Y * CosA + __X * SinA;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构旋转指定的角度的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的弧度将此 PointD3D 结构旋转指定的角度得到的结果。</returns>
        public PointD3D RotateCopy(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().RotateCopy(index1, index2, angle);

            return FromVector(result);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构绕 X 轴旋转指定的角度的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构绕 X 轴旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的弧度将此 PointD3D 结构绕 X 轴旋转指定的角度得到的结果。</returns>
        public PointD3D RotateXCopy(double angle)
        {
            PointD3D result = new PointD3D();

            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            result._X = _X;
            result._Y = _Y * CosA - _Z * SinA;
            result._Z = _Z * CosA + _Y * SinA;

            return result;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构绕 Y 轴旋转指定的角度的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构绕 Y 轴旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +X 轴的方向为正方向）。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的弧度将此 PointD3D 结构绕 Y 轴旋转指定的角度得到的结果。</returns>
        public PointD3D RotateYCopy(double angle)
        {
            PointD3D result = new PointD3D();

            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            result._X = _X * CosA + _Z * SinA;
            result._Y = _Y;
            result._Z = _Z * CosA - _X * SinA;

            return result;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构绕 Z 轴旋转指定的角度的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构绕 Z 轴旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <returns>PointD3D 结构，表示按双精度浮点数表示的弧度将此 PointD3D 结构绕 Z 轴旋转指定的角度得到的结果。</returns>
        public PointD3D RotateZCopy(double angle)
        {
            PointD3D result = new PointD3D();

            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            result._X = _X * CosA - _Y * SinA;
            result._Y = _Y * CosA + _X * SinA;
            result._Z = _Z;

            return result;
        }

        //

        /// <summary>
        /// 按 PointD3D 结构表示的 X 基向量、Y 基向量、Z 基向量与偏移向量将此 PointD3D 结构进行仿射变换。
        /// </summary>
        /// <param name="ex">PointD3D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD3D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD3D 结构表示的 Z 基向量。</param>
        /// <param name="offset">PointD3D 结构表示的偏移向量。</param>
        public void AffineTransform(PointD3D ex, PointD3D ey, PointD3D ez, PointD3D offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[_Dimension + 1, _Dimension + 1]
            {
                { ex._X, ex._Y, ex._Z, 0 },
                { ey._X, ey._Y, ey._Z, 0 },
                { ez._X, ez._Y, ez._Z, 0 },
                { offset._X, offset._Y, offset._Z, 1 }
            });

            Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

            _UpdateByVector(result);
        }

        /// <summary>
        /// 按 Matrix 对象表示的 4x4 仿射矩阵（左矩阵）将此 PointD3D 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 4x4 仿射矩阵（左矩阵）。</param>
        public void AffineTransform(Matrix matrixLeft)
        {
            Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

            _UpdateByVector(result);
        }

        /// <summary>
        /// 按 Matrix 对象数组表示的 4x4 仿射矩阵（左矩阵）数组将此 PointD3D 结构进行仿射变换。
        /// </summary>
        /// <param name="matricesLeft">Matrix 对象数组，表示 4x4 仿射矩阵（左矩阵）数组。</param>
        public void AffineTransform(params Matrix[] matricesLeft)
        {
            Vector result = ToColumnVector().AffineTransformCopy(matricesLeft);

            _UpdateByVector(result);
        }

        /// <summary>
        /// 按 Matrix 对象列表表示的 4x4 仿射矩阵（左矩阵）列表将此 PointD3D 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 4x4 仿射矩阵（左矩阵）列表。</param>
        public void AffineTransform(List<Matrix> matrixLeftList)
        {
            Vector result = ToColumnVector().AffineTransformCopy(matrixLeftList);

            _UpdateByVector(result);
        }

        /// <summary>
        /// 返回按 PointD3D 结构表示的 X 基向量、Y 基向量、Z 基向量与偏移向量将此 PointD3D 结构进行仿射变换的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="ex">PointD3D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD3D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD3D 结构表示的 Z 基向量。</param>
        /// <param name="offset">PointD3D 结构表示的偏移向量。</param>
        /// <returns>PointD3D 结构，表示按 PointD3D 结构表示的 X 基向量、Y 基向量、Z 基向量与偏移向量将此 PointD3D 结构进行仿射变换得到的结果。</returns>
        public PointD3D AffineTransformCopy(PointD3D ex, PointD3D ey, PointD3D ez, PointD3D offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[_Dimension + 1, _Dimension + 1]
            {
                { ex._X, ex._Y, ex._Z, 0 },
                { ey._X, ey._Y, ey._Z, 0 },
                { ez._X, ez._Y, ez._Z, 0 },
                { offset._X, offset._Y, offset._Z, 1 }
            });

            Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

            return FromVector(result);
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的 4x4 仿射矩阵（左矩阵）将此 PointD3D 结构进行仿射变换的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 4x4 仿射矩阵（左矩阵）。</param>
        /// <returns>PointD3D 结构，表示按 Matrix 对象表示的 4x4 仿射矩阵（左矩阵）将此 PointD3D 结构进行仿射变换得到的结果。</returns>
        public PointD3D AffineTransformCopy(Matrix matrixLeft)
        {
            Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

            return FromVector(result);
        }

        /// <summary>
        /// 返回按 Matrix 对象数组表示的 4x4 仿射矩阵（左矩阵）数组将此 PointD3D 结构进行仿射变换的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="matricesLeft">Matrix 对象数组，表示 4x4 仿射矩阵（左矩阵）数组。</param>
        /// <returns>PointD3D 结构，表示按 Matrix 对象数组表示的 4x4 仿射矩阵（左矩阵）数组将此 PointD3D 结构进行仿射变换得到的结果。</returns>
        public PointD3D AffineTransformCopy(params Matrix[] matricesLeft)
        {
            Vector result = ToColumnVector().AffineTransformCopy(matricesLeft);

            return FromVector(result);
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 4x4 仿射矩阵（左矩阵）列表将此 PointD3D 结构进行仿射变换的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 4x4 仿射矩阵（左矩阵）列表。</param>
        /// <returns>PointD3D 结构，表示按 Matrix 对象列表表示的 4x4 仿射矩阵（左矩阵）列表将此 PointD3D 结构进行仿射变换得到的结果。</returns>
        public PointD3D AffineTransformCopy(List<Matrix> matrixLeftList)
        {
            Vector result = ToColumnVector().AffineTransformCopy(matrixLeftList);

            return FromVector(result);
        }

        /// <summary>
        /// 按 PointD3D 结构表示的 X 基向量、Y 基向量、Z 基向量与偏移向量将此 PointD3D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="ex">PointD3D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD3D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD3D 结构表示的 Z 基向量。</param>
        /// <param name="offset">PointD3D 结构表示的偏移向量。</param>
        public void InverseAffineTransform(PointD3D ex, PointD3D ey, PointD3D ez, PointD3D offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[_Dimension + 1, _Dimension + 1]
            {
                { ex._X, ex._Y, ex._Z, 0 },
                { ey._X, ey._Y, ey._Z, 0 },
                { ez._X, ez._Y, ez._Z, 0 },
                { offset._X, offset._Y, offset._Z, 1 }
            });

            Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

            _UpdateByVector(result);
        }

        /// <summary>
        /// 按 Matrix 对象表示的 4x4 仿射矩阵（左矩阵）将此 PointD3D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 4x4 仿射矩阵（左矩阵）。</param>
        public void InverseAffineTransform(Matrix matrixLeft)
        {
            Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

            _UpdateByVector(result);
        }

        /// <summary>
        /// 按 Matrix 对象数组表示的 4x4 仿射矩阵（左矩阵）数组将此 PointD3D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matricesLeft">Matrix 对象数组，表示 4x4 仿射矩阵（左矩阵）数组。</param>
        public void InverseAffineTransform(params Matrix[] matricesLeft)
        {
            Vector result = ToColumnVector().InverseAffineTransformCopy(matricesLeft);

            _UpdateByVector(result);
        }

        /// <summary>
        /// 按 Matrix 对象列表表示的 4x4 仿射矩阵（左矩阵）列表将此 PointD3D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 4x4 仿射矩阵（左矩阵）列表。</param>
        public void InverseAffineTransform(List<Matrix> matrixLeftList)
        {
            Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeftList);

            _UpdateByVector(result);
        }

        /// <summary>
        /// 返回按 PointD3D 结构表示的 X 基向量、Y 基向量、Z 基向量与偏移向量将此 PointD3D 结构进行逆仿射变换的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="ex">PointD3D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD3D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD3D 结构表示的 Z 基向量。</param>
        /// <param name="offset">PointD3D 结构表示的偏移向量。</param>
        /// <returns>PointD3D 结构，表示按 PointD3D 结构表示的 X 基向量、Y 基向量、Z 基向量与偏移向量将此 PointD3D 结构进行逆仿射变换得到的结果。</returns>
        public PointD3D InverseAffineTransformCopy(PointD3D ex, PointD3D ey, PointD3D ez, PointD3D offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[_Dimension + 1, _Dimension + 1]
            {
                { ex._X, ex._Y, ex._Z, 0 },
                { ey._X, ey._Y, ey._Z, 0 },
                { ez._X, ez._Y, ez._Z, 0 },
                { offset._X, offset._Y, offset._Z, 1 }
            });

            Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

            return FromVector(result);
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的 4x4 仿射矩阵（左矩阵）将此 PointD3D 结构进行逆仿射变换的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 4x4 仿射矩阵（左矩阵）。</param>
        /// <returns>PointD3D 结构，表示按 Matrix 对象表示的 4x4 仿射矩阵（左矩阵）将此 PointD3D 结构进行逆仿射变换得到的结果。</returns>
        public PointD3D InverseAffineTransformCopy(Matrix matrixLeft)
        {
            Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

            return FromVector(result);
        }

        /// <summary>
        /// 返回按 Matrix 对象数组表示的 4x4 仿射矩阵（左矩阵）数组将此 PointD3D 结构进行逆仿射变换的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="matricesLeft">Matrix 对象数组，表示 4x4 仿射矩阵（左矩阵）数组。</param>
        /// <returns>PointD3D 结构，表示按 Matrix 对象数组表示的 4x4 仿射矩阵（左矩阵）数组将此 PointD3D 结构进行逆仿射变换得到的结果。</returns>
        public PointD3D InverseAffineTransformCopy(params Matrix[] matricesLeft)
        {
            Vector result = ToColumnVector().InverseAffineTransformCopy(matricesLeft);

            return FromVector(result);
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 4x4 仿射矩阵（左矩阵）列表将此 PointD3D 结构进行逆仿射变换的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 4x4 仿射矩阵（左矩阵）列表。</param>
        /// <returns>PointD3D 结构，表示按 Matrix 对象列表表示的 4x4 仿射矩阵（左矩阵）列表将此 PointD3D 结构进行逆仿射变换得到的结果。</returns>
        public PointD3D InverseAffineTransformCopy(List<Matrix> matrixLeftList)
        {
            Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeftList);

            return FromVector(result);
        }

        //

        /// <summary>
        /// 返回将此 PointD3D 结构投影至平行于 XY 平面的投影面的 PointD 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD3D 结构，表示投影中心在投影面的正投影在原坐标系的坐标。</param>
        /// <param name="focalLength">双精度浮点数表示的焦距，平行于投影面的一维测度其真实大小与投影大小的比值等于其到投影面的距离与焦距的比值。</param>
        /// <returns>PointD 结构，表示将此 PointD3D 结构投影至平行于 XY 平面的投影面得到的结果。</returns>
        public PointD ProjectToXY(PointD3D prjCenter, double focalLength)
        {
            if (focalLength == 0)
            {
                return XY;
            }
            else
            {
                if (_Z == prjCenter._Z)
                {
                    return PointD.NaN;
                }
                else
                {
                    double Scale = focalLength / (_Z - prjCenter._Z);

                    if (InternalMethod.IsNaNOrInfinity(Scale) || Scale <= 0)
                    {
                        return PointD.NaN;
                    }
                    else
                    {
                        return (Scale * XY + (1 - Scale) * prjCenter.XY);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 PointD3D 结构投影至平行于 YZ 平面的投影面的 PointD 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD3D 结构，表示投影中心在投影面的正投影在原坐标系的坐标。</param>
        /// <param name="focalLength">双精度浮点数表示的焦距，平行于投影面的一维测度其真实大小与投影大小的比值等于其到投影面的距离与焦距的比值。</param>
        /// <returns>PointD 结构，表示将此 PointD3D 结构投影至平行于 YZ 平面的投影面得到的结果。</returns>
        public PointD ProjectToYZ(PointD3D prjCenter, double focalLength)
        {
            if (focalLength == 0)
            {
                return YZ;
            }
            else
            {
                if (_X == prjCenter._X)
                {
                    return PointD.NaN;
                }
                else
                {
                    double Scale = focalLength / (_X - prjCenter._X);

                    if (InternalMethod.IsNaNOrInfinity(Scale) || Scale <= 0)
                    {
                        return PointD.NaN;
                    }
                    else
                    {
                        return (Scale * YZ + (1 - Scale) * prjCenter.YZ);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 PointD3D 结构投影至平行于 ZX 平面的投影面的 PointD 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD3D 结构，表示投影中心在投影面的正投影在原坐标系的坐标。</param>
        /// <param name="focalLength">双精度浮点数表示的焦距，平行于投影面的一维测度其真实大小与投影大小的比值等于其到投影面的距离与焦距的比值。</param>
        /// <returns>PointD 结构，表示将此 PointD3D 结构投影至平行于 ZX 平面的投影面得到的结果。</returns>
        public PointD ProjectToZX(PointD3D prjCenter, double focalLength)
        {
            if (focalLength == 0)
            {
                return ZX;
            }
            else
            {
                if (_Y == prjCenter._Y)
                {
                    return PointD.NaN;
                }
                else
                {
                    double Scale = focalLength / (_Y - prjCenter._Y);

                    if (InternalMethod.IsNaNOrInfinity(Scale) || Scale <= 0)
                    {
                        return PointD.NaN;
                    }
                    else
                    {
                        return (Scale * ZX + (1 - Scale) * prjCenter.ZX);
                    }
                }
            }
        }

        //

        /// <summary>
        /// 返回将此 PointD3D 结构转换为列向量的 Vector 的新实例。
        /// </summary>
        /// <returns>Vector 对象，表示将此 PointD3D 结构转换为列向量得到的结果。</returns>
        public Vector ToColumnVector()
        {
            return Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, _X, _Y, _Z);
        }

        /// <summary>
        /// 返回将此 PointD3D 结构转换为行向量的 Vector 的新实例。
        /// </summary>
        /// <returns>Vector 对象，表示将此 PointD3D 结构转换为行向量得到的结果。</returns>
        public Vector ToRowVector()
        {
            return Vector.UnsafeCreateInstance(Vector.Type.RowVector, _X, _Y, _Z);
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断两个 PointD3D 结构是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 PointD3D 结构。</param>
        /// <param name="right">用于比较的第二个 PointD3D 结构。</param>
        /// <returns>布尔值，表示两个 PointD3D 结构是否相等。</returns>
        public static bool Equals(PointD3D left, PointD3D right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if ((object)left == null || (object)right == null)
            {
                return false;
            }
            else
            {
                return left.Equals(right);
            }
        }

        //

        /// <summary>
        /// 比较两个 PointD3D 结构的次序。
        /// </summary>
        /// <param name="left">用于比较的第一个 PointD3D 结构。</param>
        /// <param name="right">用于比较的第二个 PointD3D 结构。</param>
        /// <returns>32 位整数，表示将两个 PointD3D 结构进行次序比较得到的结果。</returns>
        public static int Compare(PointD3D left, PointD3D right)
        {
            if (object.ReferenceEquals(left, right))
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
            else
            {
                return left.CompareTo(right);
            }
        }

        //

        /// <summary>
        /// 返回将 Vector 对象转换为 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象。</param>
        /// <returns>PointD3D 结构，表示转换的结果。</returns>
        public static PointD3D FromVector(Vector vector)
        {
            if (Vector.IsNullOrEmpty(vector) || vector.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }

            //

            return new PointD3D(vector[0], vector[1], vector[2]);
        }

        //

        /// <summary>
        /// 返回单位矩阵，表示不对 PointD3D 结构进行仿射变换的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <returns>Matrix 对象，表示不对 PointD3D 结构进行仿射变换的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix IdentityMatrix()
        {
            return Matrix.Identity(_Dimension + 1);
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的位移将 PointD3D 结构在指定的基向量方向的分量平移指定的量的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定平移的分量所在方向的基向量。</param>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的位移将 PointD3D 结构在指定的基向量方向的分量平移指定的量的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(int index, double d)
        {
            return Vector.OffsetMatrix(Vector.Type.ColumnVector, _Dimension, index, d);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的位移将 PointD3D 结构的所有分量平移指定的量的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的位移将 PointD3D 结构的所有分量平移指定的量的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(double d)
        {
            return Vector.OffsetMatrix(Vector.Type.ColumnVector, _Dimension, d);
        }

        /// <summary>
        /// 返回表示按 PointD3D 结构表示的位移将此 PointD3D 结构平移指定的量的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构表示的位移。</param>
        /// <returns>Matrix 对象，表示按 PointD3D 结构表示的位移将此 PointD3D 结构平移指定的量的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(PointD3D pt)
        {
            return Vector.OffsetMatrix(pt.ToColumnVector());
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的 X 坐标位移、Y 坐标位移与 Z 坐标位移将 PointD3D 结构平移指定的量的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标位移。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的 X 坐标位移、Y 坐标位移与 Z 坐标位移将 PointD3D 结构平移指定的量的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(double dx, double dy, double dz)
        {
            return Vector.OffsetMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, dx, dy, dz));
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的位移将 PointD3D 结构在 X 轴的分量平移指定的量的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的位移将 PointD3D 结构在 X 轴的分量平移指定的量的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetXMatrix(double d)
        {
            return Vector.OffsetMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, d, 0, 0));
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的位移将 PointD3D 结构在 Y 轴的分量平移指定的量的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的位移将 PointD3D 结构在 Y 轴的分量平移指定的量的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetYMatrix(double d)
        {
            return Vector.OffsetMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, 0, d, 0));
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的位移将 PointD3D 结构在 Z 轴的分量平移指定的量的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的位移将 PointD3D 结构在 Z 轴的分量平移指定的量的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetZMatrix(double d)
        {
            return Vector.OffsetMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, 0, 0, d));
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的缩放因数将 PointD3D 结构在指定的基向量方向的分量缩放指定的倍数的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定平移的分量所在方向的基向量。</param>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的缩放因数将 PointD3D 结构在指定的基向量方向的分量缩放指定的倍数的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(int index, double s)
        {
            return Vector.ScaleMatrix(Vector.Type.ColumnVector, _Dimension, index, s);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的缩放因数将 PointD3D 结构的所有分量缩放指定的倍数的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的缩放因数将 PointD3D 结构的所有分量缩放指定的倍数的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(double s)
        {
            return Vector.ScaleMatrix(Vector.Type.ColumnVector, _Dimension, s);
        }

        /// <summary>
        /// 返回表示按 PointD3D 结构表示的缩放因数将 PointD3D 结构缩放指定的倍数的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按 PointD3D 结构表示的缩放因数将 PointD3D 结构缩放指定的倍数的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(PointD3D pt)
        {
            return Vector.ScaleMatrix(pt.ToColumnVector());
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的 X 坐标缩放因数、Y 坐标缩放因数与 Z 坐标缩放因数将 PointD3D 结构缩放指定的倍数的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因数。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的 X 坐标缩放因数、Y 坐标缩放因数与 Z 坐标缩放因数将 PointD3D 结构缩放指定的倍数的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(double sx, double sy, double sz)
        {
            return Vector.ScaleMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, sx, sy, sz));
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的缩放因数将 PointD3D 结构在 X 轴的分量缩放指定的倍数的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的缩放因数将 PointD3D 结构在 X 轴的分量缩放指定的倍数的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleXMatrix(double s)
        {
            return Vector.ScaleMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, s, 0, 0));
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的缩放因数将 PointD3D 结构在 Y 轴的分量缩放指定的倍数的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的缩放因数将 PointD3D 结构在 Y 轴的分量缩放指定的倍数的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleYMatrix(double s)
        {
            return Vector.ScaleMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, 0, s, 0));
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的缩放因数将 PointD3D 结构在 Z 轴的分量缩放指定的倍数的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的缩放因数将 PointD3D 结构在 Z 轴的分量缩放指定的倍数的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleZMatrix(double s)
        {
            return Vector.ScaleMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, 0, 0, s));
        }

        //

        /// <summary>
        /// 返回表示将 PointD3D 结构在指定的基向量方向的分量翻转的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        /// <returns>Matrix 对象，表示将 PointD3D 结构在指定的基向量方向的分量翻转的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ReflectMatrix(int index)
        {
            return Vector.ReflectMatrix(Vector.Type.ColumnVector, _Dimension, index);
        }

        /// <summary>
        /// 返回表示将 PointD3D 结构在 X 轴的分量翻转的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <returns>Matrix 对象，表示将 PointD3D 结构在 X 轴的分量翻转的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ReflectXMatrix()
        {
            return ReflectMatrix(0);
        }

        /// <summary>
        /// 返回表示将 PointD3D 结构在 Y 轴的分量翻转的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <returns>Matrix 对象，表示将 PointD3D 结构在 Y 轴的分量翻转的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ReflectYMatrix()
        {
            return ReflectMatrix(1);
        }

        /// <summary>
        /// 返回表示将 PointD3D 结构在 Z 轴的分量翻转的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <returns>Matrix 对象，表示将 PointD3D 结构在 Z 轴的分量翻转的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ReflectZMatrix()
        {
            return ReflectMatrix(2);
        }

        //

        /// <summary>
        /// 返回表示用于剪切 PointD3D 结构的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向剪切的角度（弧度）。</param>
        /// <returns>Matrix 对象，表示用于剪切 PointD3D 结构的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ShearMatrix(int index1, int index2, double angle)
        {
            return Vector.ShearMatrix(Vector.Type.ColumnVector, _Dimension, index1, index2, angle);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD3D 结构在 XY 平面向 +X 轴剪切指定的角度的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 XY 平面向 +X 轴剪切的角度（弧度）。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的弧度将 PointD3D 结构在 XY 平面向 +X 轴剪切指定的角度的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ShearXYMatrix(double angle)
        {
            return ShearMatrix(0, 1, angle);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD3D 结构在 XY 平面向 +Y 轴剪切指定的角度的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 XY 平面向 +Y 轴剪切的角度（弧度）。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的弧度将 PointD3D 结构在 XY 平面向 +Y 轴剪切指定的角度的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ShearYXMatrix(double angle)
        {
            return ShearMatrix(1, 0, angle);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD3D 结构在 YZ 平面向 +Y 轴剪切指定的角度的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 YZ 平面向 +Y 轴剪切的角度（弧度）。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的弧度将 PointD3D 结构在 YZ 平面向 +Y 轴剪切指定的角度的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ShearYZMatrix(double angle)
        {
            return ShearMatrix(1, 2, angle);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD3D 结构在 YZ 平面向 +Z 轴剪切指定的角度的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 YZ 平面向 +Z 轴剪切的角度（弧度）。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的弧度将 PointD3D 结构在 YZ 平面向 +Z 轴剪切指定的角度的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ShearZYMatrix(double angle)
        {
            return ShearMatrix(2, 1, angle);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD3D 结构在 ZX 平面向 +Z 轴剪切指定的角度的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 ZX 平面向 +Z 轴剪切的角度（弧度）。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的弧度将 PointD3D 结构在 ZX 平面向 +Z 轴剪切指定的角度的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ShearZXMatrix(double angle)
        {
            return ShearMatrix(2, 0, angle);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD3D 结构在 ZX 平面向 +X 轴剪切指定的角度的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 ZX 平面向 +X 轴剪切的角度（弧度）。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的弧度将 PointD3D 结构在 ZX 平面向 +X 轴剪切指定的角度的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix ShearXZMatrix(double angle)
        {
            return ShearMatrix(0, 2, angle);
        }

        //

        /// <summary>
        /// 返回表示用于旋转 PointD3D 结构的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示 PointD3D 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        /// <returns>Matrix 对象，表示用于旋转 PointD3D 结构的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix RotateMatrix(int index1, int index2, double angle)
        {
            return Vector.RotateMatrix(Vector.Type.ColumnVector, _Dimension, index1, index2, angle);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD3D 结构绕 X 轴旋转指定的角度的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD3D 结构绕 X 轴旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的弧度将 PointD3D 结构绕 X 轴旋转指定的角度的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix RotateXMatrix(double angle)
        {
            return RotateMatrix(1, 2, angle);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD3D 结构绕 Y 轴旋转指定的角度的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD3D 结构绕 Y 轴旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +X 轴的方向为正方向）。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的弧度将 PointD3D 结构绕 Y 轴旋转指定的角度的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix RotateYMatrix(double angle)
        {
            return RotateMatrix(2, 0, angle);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度将 PointD3D 结构绕 Z 轴旋转指定的角度的 4x4 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD3D 结构绕 Z 轴旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的弧度将 PointD3D 结构绕 Z 轴旋转指定的角度的 4x4 仿射矩阵（左矩阵）。</returns>
        public static Matrix RotateZMatrix(double angle)
        {
            return RotateMatrix(0, 1, angle);
        }

        //

        /// <summary>
        /// 返回两个 PointD3D 结构之间的距离。
        /// </summary>
        /// <param name="left">第一个 PointD3D 结构。</param>
        /// <param name="right">第二个 PointD3D 结构。</param>
        /// <returns>双精度浮点数，表示两个 PointD3D 结构之间的距离。</returns>
        public static double DistanceBetween(PointD3D left, PointD3D right)
        {
            double AbsDx = Math.Abs(left._X - right._X);
            double AbsDy = Math.Abs(left._Y - right._Y);
            double AbsDz = Math.Abs(left._Z - right._Z);

            double AbsMax = Math.Max(Math.Max(AbsDx, AbsDy), AbsDz);

            if (AbsMax == 0)
            {
                return 0;
            }
            else
            {
                AbsDx /= AbsMax;
                AbsDy /= AbsMax;
                AbsDz /= AbsMax;

                double SqrSum = AbsDx * AbsDx + AbsDy * AbsDy + AbsDz * AbsDz;

                return (AbsMax * Math.Sqrt(SqrSum));
            }
        }

        /// <summary>
        /// 返回两个 PointD3D 结构之间的夹角（弧度）。
        /// </summary>
        /// <param name="left">第一个 PointD3D 结构。</param>
        /// <param name="right">第二个 PointD3D 结构。</param>
        /// <returns>双精度浮点数，表示两个 PointD3D 结构之间的夹角（弧度）。</returns>
        public static double AngleBetween(PointD3D left, PointD3D right)
        {
            if (left.IsZero || right.IsZero)
            {
                return 0;
            }
            else
            {
                double ModProduct = left.Module * right.Module;
                double CosA = left._X * right._X / ModProduct + left._Y * right._Y / ModProduct + left._Z * right._Z / ModProduct;

                return Math.Acos(CosA);
            }
        }

        //

        /// <summary>
        /// 返回两个 PointD3D 结构的数量积。
        /// </summary>
        /// <param name="left">第一个 PointD3D 结构。</param>
        /// <param name="right">第二个 PointD3D 结构。</param>
        /// <returns>双精度浮点数，表示两个 PointD3D 结构的数量积。</returns>
        public static double DotProduct(PointD3D left, PointD3D right)
        {
            return (left._X * right._X + left._Y * right._Y + left._Z * right._Z);
        }

        /// <summary>
        /// 返回两个 PointD3D 结构的向量积。该向量积为一个三维向量，其所有分量的数值依次为 X 基向量、Y 基向量与 Z 基向量的系数。
        /// </summary>
        /// <param name="left">第一个 PointD3D 结构。</param>
        /// <param name="right">第二个 PointD3D 结构。</param>
        /// <returns>PointD3D 结构，表示两个 PointD3D 结构的向量积。</returns>
        public static PointD3D CrossProduct(PointD3D left, PointD3D right)
        {
            return new PointD3D(left._Y * right._Z - left._Z * right._Y, left._Z * right._X - left._X * right._Z, left._X * right._Y - left._Y * right._X);
        }

        //

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量取符号数得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        /// <returns>PointD3D 结构，表示将 PointD3D 结构的所有分量取符号数得到的结果</returns>
        public static PointD3D Sign(PointD3D pt)
        {
            return new PointD3D((double.IsNaN(pt._X) ? 0 : Math.Sign(pt._X)), (double.IsNaN(pt._Y) ? 0 : Math.Sign(pt._Y)), (double.IsNaN(pt._Z) ? 0 : Math.Sign(pt._Z)));
        }

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量取绝对值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        /// <returns>PointD3D 结构，表示将 PointD3D 结构的所有分量取绝对值得到的结果</returns>
        public static PointD3D Abs(PointD3D pt)
        {
            return new PointD3D(Math.Abs(pt._X), Math.Abs(pt._Y), Math.Abs(pt._Z));
        }

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量舍入到较大的整数值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        /// <returns>PointD3D 结构，表示将 PointD3D 结构的所有分量舍入到较大的整数值得到的结果</returns>
        public static PointD3D Ceiling(PointD3D pt)
        {
            return new PointD3D(Math.Ceiling(pt._X), Math.Ceiling(pt._Y), Math.Ceiling(pt._Z));
        }

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量舍入到较小的整数值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        /// <returns>PointD3D 结构，表示将 PointD3D 结构的所有分量舍入到较小的整数值得到的结果</returns>
        public static PointD3D Floor(PointD3D pt)
        {
            return new PointD3D(Math.Floor(pt._X), Math.Floor(pt._Y), Math.Floor(pt._Z));
        }

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量舍入到最接近的整数值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        /// <returns>PointD3D 结构，表示将 PointD3D 结构的所有分量舍入到最接近的整数值得到的结果</returns>
        public static PointD3D Round(PointD3D pt)
        {
            return new PointD3D(Math.Round(pt._X), Math.Round(pt._Y), Math.Round(pt._Z));
        }

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量截断小数部分取整得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        /// <returns>PointD3D 结构，表示将 PointD3D 结构的所有分量截断小数部分取整得到的结果</returns>
        public static PointD3D Truncate(PointD3D pt)
        {
            return new PointD3D(Math.Truncate(pt._X), Math.Truncate(pt._Y), Math.Truncate(pt._Z));
        }

        /// <summary>
        /// 返回将两个 PointD3D 结构的所有分量分别取最大值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD3D 结构，用于比较的第二个结构。</param>
        /// <returns>PointD3D 结构，表示将两个 PointD3D 结构的所有分量分别取最大值得到的结果</returns>
        public static PointD3D Max(PointD3D left, PointD3D right)
        {
            return new PointD3D(Math.Max(left._X, right._X), Math.Max(left._Y, right._Y), Math.Max(left._Z, right._Z));
        }

        /// <summary>
        /// 返回将两个 PointD3D 结构的所有分量分别取最小值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD3D 结构，用于比较的第二个结构。</param>
        /// <returns>PointD3D 结构，表示将两个 PointD3D 结构的所有分量分别取最小值得到的结果</returns>
        public static PointD3D Min(PointD3D left, PointD3D right)
        {
            return new PointD3D(Math.Min(left._X, right._X), Math.Min(left._Y, right._Y), Math.Min(left._Z, right._Z));
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 PointD3D 结构是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD3D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD3D 结构。</param>
        /// <returns>布尔值，表示两个 PointD3D 结构是否相等。</returns>
        public static bool operator ==(PointD3D left, PointD3D right)
        {
            return (left._X == right._X && left._Y == right._Y && left._Z == right._Z);
        }

        /// <summary>
        /// 判断两个 PointD3D 结构是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD3D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD3D 结构。</param>
        /// <returns>布尔值，表示两个 PointD3D 结构是否不相等。</returns>
        public static bool operator !=(PointD3D left, PointD3D right)
        {
            return (left._X != right._X || left._Y != right._Y || left._Z != right._Z);
        }

        /// <summary>
        /// 判断两个 PointD3D 结构的字典序是否前者小于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD3D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD3D 结构。</param>
        /// <returns>布尔值，表示两个 PointD3D 结构的字典序是否前者小于后者。</returns>
        public static bool operator <(PointD3D left, PointD3D right)
        {
            for (int i = 0; i < _Dimension; i++)
            {
                if (left[i] != right[i])
                {
                    return (left[i] < right[i]);
                }
            }

            return false;
        }

        /// <summary>
        /// 判断两个 PointD3D 结构的字典序是否前者大于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD3D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD3D 结构。</param>
        /// <returns>布尔值，表示两个 PointD3D 结构的字典序是否前者大于后者。</returns>
        public static bool operator >(PointD3D left, PointD3D right)
        {
            for (int i = 0; i < _Dimension; i++)
            {
                if (left[i] != right[i])
                {
                    return (left[i] > right[i]);
                }
            }

            return false;
        }

        /// <summary>
        /// 判断两个 PointD3D 结构的字典序是否前者小于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD3D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD3D 结构。</param>
        /// <returns>布尔值，表示两个 PointD3D 结构的字典序是否前者小于或等于后者。</returns>
        public static bool operator <=(PointD3D left, PointD3D right)
        {
            for (int i = 0; i < _Dimension; i++)
            {
                if (left[i] != right[i])
                {
                    return (left[i] < right[i]);
                }
            }

            return true;
        }

        /// <summary>
        /// 判断两个 PointD3D 结构的字典序是否前者大于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD3D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD3D 结构。</param>
        /// <returns>布尔值，表示两个 PointD3D 结构的字典序是否前者大于或等于后者。</returns>
        public static bool operator >=(PointD3D left, PointD3D right)
        {
            for (int i = 0; i < _Dimension; i++)
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
        /// 返回在 PointD3D 结构的所有分量前添加正号得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">运算符右侧的 PointD3D 结构。</param>
        /// <returns>PointD3D 结构，表示在 PointD3D 结构的所有分量前添加正号得到的结果。</returns>
        public static PointD3D operator +(PointD3D pt)
        {
            return pt;
        }

        /// <summary>
        /// 返回在 PointD3D 结构的所有分量前添加负号得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">运算符右侧的 PointD3D 结构。</param>
        /// <returns>PointD3D 结构，表示在 PointD3D 结构的所有分量前添加负号得到的结果。</returns>
        public static PointD3D operator -(PointD3D pt)
        {
            return new PointD3D(-pt._X, -pt._Y, -pt._Z);
        }

        //

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量与双精度浮点数相加得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，表示被加数。</param>
        /// <param name="n">双精度浮点数，表示加数。</param>
        /// <returns>PointD3D 结构，表示将 PointD3D 结构的所有分量与双精度浮点数相加得到的结果。</returns>
        public static PointD3D operator +(PointD3D pt, double n)
        {
            return new PointD3D(pt._X + n, pt._Y + n, pt._Z + n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD3D 结构的所有分量相加得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被加数。</param>
        /// <param name="pt">PointD3D 结构，表示加数。</param>
        /// <returns>PointD3D 结构，表示将双精度浮点数与 PointD3D 结构的所有分量相加得到的结果。</returns>
        public static PointD3D operator +(double n, PointD3D pt)
        {
            return new PointD3D(n + pt._X, n + pt._Y, n + pt._Z);
        }

        /// <summary>
        /// 返回将 PointD3D 结构与 PointD3D 结构的所有分量对应相加得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示被加数。</param>
        /// <param name="right">PointD3D 结构，表示加数。</param>
        /// <returns>PointD3D 结构，表示将 PointD3D 结构与 PointD3D 结构的所有分量对应相加得到的结果。</returns>
        public static PointD3D operator +(PointD3D left, PointD3D right)
        {
            return new PointD3D(left._X + right._X, left._Y + right._Y, left._Z + right._Z);
        }

        //

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量与双精度浮点数相减得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，表示被减数。</param>
        /// <param name="n">双精度浮点数，表示减数。</param>
        /// <returns>PointD3D 结构，表示将 PointD3D 结构的所有分量与双精度浮点数相减得到的结果。</returns>
        public static PointD3D operator -(PointD3D pt, double n)
        {
            return new PointD3D(pt._X - n, pt._Y - n, pt._Z - n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD3D 结构的所有分量相减得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被减数。</param>
        /// <param name="pt">PointD3D 结构，表示减数。</param>
        /// <returns>PointD3D 结构，表示将双精度浮点数与 PointD3D 结构的所有分量相减得到的结果。</returns>
        public static PointD3D operator -(double n, PointD3D pt)
        {
            return new PointD3D(n - pt._X, n - pt._Y, n - pt._Z);
        }

        /// <summary>
        /// 返回将 PointD3D 结构与 PointD3D 结构的所有分量对应相减得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示被减数。</param>
        /// <param name="right">PointD3D 结构，表示减数。</param>
        /// <returns>PointD3D 结构，表示将 PointD3D 结构与 PointD3D 结构的所有分量对应相减得到的结果。</returns>
        public static PointD3D operator -(PointD3D left, PointD3D right)
        {
            return new PointD3D(left._X - right._X, left._Y - right._Y, left._Z - right._Z);
        }

        //

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量与双精度浮点数相乘得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，表示被乘数。</param>
        /// <param name="n">双精度浮点数，表示乘数。</param>
        /// <returns>PointD3D 结构，表示将 PointD3D 结构的所有分量与双精度浮点数相乘得到的结果。</returns>
        public static PointD3D operator *(PointD3D pt, double n)
        {
            return new PointD3D(pt._X * n, pt._Y * n, pt._Z * n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD3D 结构的所有分量相乘得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被乘数。</param>
        /// <param name="pt">PointD3D 结构，表示乘数。</param>
        /// <returns>PointD3D 结构，表示将双精度浮点数与 PointD3D 结构的所有分量相乘得到的结果。</returns>
        public static PointD3D operator *(double n, PointD3D pt)
        {
            return new PointD3D(n * pt._X, n * pt._Y, n * pt._Z);
        }

        /// <summary>
        /// 返回将 PointD3D 结构与 PointD3D 结构的所有分量对应相乘得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示被乘数。</param>
        /// <param name="right">PointD3D 结构，表示乘数。</param>
        /// <returns>PointD3D 结构，表示将 PointD3D 结构与 PointD3D 结构的所有分量对应相乘得到的结果。</returns>
        public static PointD3D operator *(PointD3D left, PointD3D right)
        {
            return new PointD3D(left._X * right._X, left._Y * right._Y, left._Z * right._Z);
        }

        //

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量与双精度浮点数相除得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，表示被除数。</param>
        /// <param name="n">双精度浮点数，表示除数。</param>
        /// <returns>PointD3D 结构，表示将 PointD3D 结构的所有分量与双精度浮点数相除得到的结果。</returns>
        public static PointD3D operator /(PointD3D pt, double n)
        {
            return new PointD3D(pt._X / n, pt._Y / n, pt._Z / n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD3D 结构的所有分量相除得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被除数。</param>
        /// <param name="pt">PointD3D 结构，表示除数。</param>
        /// <returns>PointD3D 结构，表示将双精度浮点数与 PointD3D 结构的所有分量相除得到的结果。</returns>
        public static PointD3D operator /(double n, PointD3D pt)
        {
            return new PointD3D(n / pt._X, n / pt._Y, n / pt._Z);
        }

        /// <summary>
        /// 返回将 PointD3D 结构与 PointD3D 结构的所有分量对应相除得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示被除数。</param>
        /// <param name="right">PointD3D 结构，表示除数。</param>
        /// <returns>PointD3D 结构，表示将 PointD3D 结构与 PointD3D 结构的所有分量对应相除得到的结果。</returns>
        public static PointD3D operator /(PointD3D left, PointD3D right)
        {
            return new PointD3D(left._X / right._X, left._Y / right._Y, left._Z / right._Z);
        }

        #endregion

        #region 显式接口成员实现

        #region Com.IVector<T>

        int IVector<double>.Size
        {
            get
            {
                return _Dimension;
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
                return _Dimension;
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
            this = new PointD3D();
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
                return _Dimension;
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

            if (array.Length < _Dimension)
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
            private PointD3D _Pt;
            private int _Index;

            internal Enumerator(PointD3D pt)
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
                        throw new IndexOutOfRangeException();
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
                return _Dimension;
            }
        }

        void ICollection<double>.Add(double item)
        {
            throw new NotSupportedException();
        }

        void ICollection<double>.Clear()
        {
            this = new PointD3D();
        }

        void ICollection<double>.CopyTo(double[] array, int index)
        {
            if (array != null && array.Length >= _Dimension)
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
            private PointD3D _Pt;
            private int _Index;

            internal GenericEnumerator(PointD3D pt)
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
                    if (_Index < 0 || _Index >= _Dimension)
                    {
                        throw new IndexOutOfRangeException();
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

            void IEnumerator.Reset()
            {
                _Index = -1;
            }

            double IEnumerator<double>.Current
            {
                get
                {
                    if (_Index < 0 || _Index >= _Dimension)
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