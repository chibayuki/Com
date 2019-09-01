/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.PointD5D
Version 19.9.1.0000

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
    /// 以一组有序的双精度浮点数表示的五维直角坐标系坐标。
    /// </summary>
    public struct PointD5D : IEquatable<PointD5D>, IComparable, IComparable<PointD5D>, IEuclideanVector<PointD5D>, IAffine<PointD5D>
    {
        #region 私有成员与内部成员

        private const int _Dimension = 5; // PointD5D 结构的维度。

        private static readonly Size _AffineMatrixSize = new Size(_Dimension + 1, _Dimension + 1); // PointD5D 结构的仿射矩阵大小。

        //

        private double _X; // X 坐标。
        private double _Y; // Y 坐标。
        private double _Z; // Z 坐标。
        private double _U; // U 坐标。
        private double _V; // V 坐标。

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用双精度浮点数表示的 X 坐标、Y 坐标、Z 坐标、U 坐标与 V 坐标初始化 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="x">双精度浮点数表示的 X 坐标。</param>
        /// <param name="y">双精度浮点数表示的 Y 坐标。</param>
        /// <param name="z">双精度浮点数表示的 Z 坐标。</param>
        /// <param name="u">双精度浮点数表示的 U 坐标。</param>
        /// <param name="v">双精度浮点数表示的 V 坐标。</param>
        public PointD5D(double x, double y, double z, double u, double v)
        {
            _X = x;
            _Y = y;
            _Z = z;
            _U = u;
            _V = v;
        }

        #endregion

        #region 字段

        /// <summary>
        /// 表示零向量的 PointD5D 结构的实例。
        /// </summary>
        public static readonly PointD5D Zero = new PointD5D(0, 0, 0, 0, 0);

        //

        /// <summary>
        /// 表示所有分量为非数字的 PointD5D 结构的实例。
        /// </summary>
        public static readonly PointD5D NaN = new PointD5D(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);

        //

        /// <summary>
        /// 表示 X 基向量的 PointD5D 结构的实例。
        /// </summary>
        public static readonly PointD5D Ex = new PointD5D(1, 0, 0, 0, 0);

        /// <summary>
        /// 表示 Y 基向量的 PointD5D 结构的实例。
        /// </summary>
        public static readonly PointD5D Ey = new PointD5D(0, 1, 0, 0, 0);

        /// <summary>
        /// 表示 Z 基向量的 PointD5D 结构的实例。
        /// </summary>
        public static readonly PointD5D Ez = new PointD5D(0, 0, 1, 0, 0);

        /// <summary>
        /// 表示 U 基向量的 PointD5D 结构的实例。
        /// </summary>
        public static readonly PointD5D Eu = new PointD5D(0, 0, 0, 1, 0);

        /// <summary>
        /// 表示 V 基向量的 PointD5D 结构的实例。
        /// </summary>
        public static readonly PointD5D Ev = new PointD5D(0, 0, 0, 0, 1);

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置此 PointD5D 结构在指定的基向量方向的分量。
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
                    case 3: return _U;
                    case 4: return _V;
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
                    case 3: _U = value; break;
                    case 4: _V = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置此 PointD5D 结构在 X 轴的分量。
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
        /// 获取或设置此 PointD5D 结构在 Y 轴的分量。
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
        /// 获取或设置此 PointD5D 结构在 Z 轴的分量。
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

        /// <summary>
        /// 获取或设置此 PointD5D 结构在 U 轴的分量。
        /// </summary>
        public double U
        {
            get
            {
                return _U;
            }

            set
            {
                _U = value;
            }
        }

        /// <summary>
        /// 获取或设置此 PointD5D 结构在 V 轴的分量。
        /// </summary>
        public double V
        {
            get
            {
                return _V;
            }

            set
            {
                _V = value;
            }
        }

        //

        /// <summary>
        /// 获取此 PointD5D 结构的维度。
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
        /// 获取表示此 PointD5D 结构是否为空向量的布尔值。
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获取表示此 PointD5D 结构是否为零向量的布尔值。
        /// </summary>
        public bool IsZero
        {
            get
            {
                return (_X == 0 && _Y == 0 && _Z == 0 && _U == 0 && _V == 0);
            }
        }

        /// <summary>
        /// 获取表示此 PointD5D 结构是否只读的布尔值。
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获取表示此 PointD5D 结构是否具有固定的维度的布尔值。
        /// </summary>
        public bool IsFixedSize
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 获取表示此 PointD5D 结构是否包含非数字分量的布尔值。
        /// </summary>
        public bool IsNaN
        {
            get
            {
                return (double.IsNaN(_X) || double.IsNaN(_Y) || double.IsNaN(_Z) || double.IsNaN(_U) || double.IsNaN(_V));
            }
        }

        /// <summary>
        /// 获取表示此 PointD5D 结构是否包含无穷大分量的布尔值。
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return (!IsNaN && (double.IsInfinity(_X) || double.IsInfinity(_Y) || double.IsInfinity(_Z) || double.IsInfinity(_U) || double.IsInfinity(_V)));
            }
        }

        /// <summary>
        /// 获取表示此 PointD5D 结构是否包含非数字或无穷大分量的布尔值。
        /// </summary>
        public bool IsNaNOrInfinity
        {
            get
            {
                return (InternalMethod.IsNaNOrInfinity(_X) || InternalMethod.IsNaNOrInfinity(_Y) || InternalMethod.IsNaNOrInfinity(_Z) || InternalMethod.IsNaNOrInfinity(_U) || InternalMethod.IsNaNOrInfinity(_V));
            }
        }

        //

        /// <summary>
        /// 获取此 PointD5D 结构的模。
        /// </summary>
        public double Module
        {
            get
            {
                double AbsX = Math.Abs(_X);
                double AbsY = Math.Abs(_Y);
                double AbsZ = Math.Abs(_Z);
                double AbsU = Math.Abs(_U);
                double AbsV = Math.Abs(_V);

                double AbsMax = Math.Max(Math.Max(Math.Max(Math.Max(AbsX, AbsY), AbsZ), AbsU), AbsV);

                if (AbsMax == 0)
                {
                    return 0;
                }
                else
                {
                    AbsX /= AbsMax;
                    AbsY /= AbsMax;
                    AbsZ /= AbsMax;
                    AbsU /= AbsMax;
                    AbsV /= AbsMax;

                    double SqrSum = AbsX * AbsX + AbsY * AbsY + AbsZ * AbsZ + AbsU * AbsU + AbsV * AbsV;

                    return (AbsMax * Math.Sqrt(SqrSum));
                }
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构的模的平方。
        /// </summary>
        public double ModuleSquared
        {
            get
            {
                return (_X * _X + _Y * _Y + _Z * _Z + _U * _U + _V * _V);
            }
        }

        //

        /// <summary>
        /// 获取此 PointD5D 结构的相反向量。
        /// </summary>
        public PointD5D Opposite
        {
            get
            {
                return new PointD5D(-_X, -_Y, -_Z, -_U, -_V);
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构的规范化向量。
        /// </summary>
        public PointD5D Normalize
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
                    return new PointD5D(_X / Mod, _Y / Mod, _Z / Mod, _U / Mod, _V / Mod);
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置此 PointD5D 结构在 XYZU 空间的分量。
        /// </summary>
        public PointD4D XYZU
        {
            get
            {
                return new PointD4D(_X, _Y, _Z, _U);
            }

            set
            {
                _X = value.X;
                _Y = value.Y;
                _Z = value.Z;
                _U = value.U;
            }
        }

        /// <summary>
        /// 获取或设置此 PointD5D 结构在 YZUV 空间的分量。
        /// </summary>
        public PointD4D YZUV
        {
            get
            {
                return new PointD4D(_Y, _Z, _U, _V);
            }

            set
            {
                _Y = value.X;
                _Z = value.Y;
                _U = value.Z;
                _V = value.U;
            }
        }

        /// <summary>
        /// 获取或设置此 PointD5D 结构在 ZUVX 空间的分量。
        /// </summary>
        public PointD4D ZUVX
        {
            get
            {
                return new PointD4D(_Z, _U, _V, _X);
            }

            set
            {
                _Z = value.X;
                _U = value.Y;
                _V = value.Z;
                _X = value.U;
            }
        }

        /// <summary>
        /// 获取或设置此 PointD5D 结构在 UVXY 空间的分量。
        /// </summary>
        public PointD4D UVXY
        {
            get
            {
                return new PointD4D(_U, _V, _X, _Y);
            }

            set
            {
                _U = value.X;
                _V = value.Y;
                _X = value.Z;
                _Y = value.U;
            }
        }

        /// <summary>
        /// 获取或设置此 PointD5D 结构在 VXYZ 空间的分量。
        /// </summary>
        public PointD4D VXYZ
        {
            get
            {
                return new PointD4D(_V, _X, _Y, _Z);
            }

            set
            {
                _V = value.X;
                _X = value.Y;
                _Y = value.Z;
                _Z = value.U;
            }
        }

        //

        /// <summary>
        /// 获取此 PointD5D 结构与 X 轴之间的夹角（弧度）。
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
        /// 获取此 PointD5D 结构与 Y 轴之间的夹角（弧度）。
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
        /// 获取此 PointD5D 结构与 Z 轴之间的夹角（弧度）。
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
        /// 获取此 PointD5D 结构与 U 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleFromU
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }
                else
                {
                    return AngleFrom(_U >= 0 ? Eu : -Eu);
                }
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构与 V 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleFromV
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }
                else
                {
                    return AngleFrom(_V >= 0 ? Ev : -Ev);
                }
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构与 XYZU 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleFromXYZU
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }
                else
                {
                    return (Constant.HalfPi - AngleFromV);
                }
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构与 YZUV 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleFromYZUV
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
        /// 获取此 PointD5D 结构与 ZUVX 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleFromZUVX
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

        /// <summary>
        /// 获取此 PointD5D 结构与 UVXY 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleFromUVXY
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
        /// 获取此 PointD5D 结构与 VXYZ 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleFromVXYZ
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }
                else
                {
                    return (Constant.HalfPi - AngleFromU);
                }
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 PointD5D 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>布尔值，表示此 PointD5D 结构是否与指定的对象相等。</returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj == null || !(obj is PointD5D))
            {
                return false;
            }
            else
            {
                return Equals((PointD5D)obj);
            }
        }

        /// <summary>
        /// 返回此 PointD5D 结构的哈希代码。
        /// </summary>
        /// <returns>32 位整数，表示此 PointD5D 结构的哈希代码。</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 PointD5D 结构转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 PointD5D 结构的字符串形式。</returns>
        public override string ToString()
        {
            return string.Concat("{X=", _X, ", Y=", _Y, ", Z=", _Z, ", U=", _U, ", V=", _V, "}");
        }

        //

        /// <summary>
        /// 判断此 PointD5D 结构是否与指定的 PointD5D 结构相等。
        /// </summary>
        /// <param name="pt">用于比较的 PointD5D 结构。</param>
        /// <returns>布尔值，表示此 PointD5D 结构是否与指定的 PointD5D 结构相等。</returns>
        public bool Equals(PointD5D pt)
        {
            return (_X.Equals(pt._X) && _Y.Equals(pt._Y) && _Z.Equals(pt._Z) && _U.Equals(pt._U) && _V.Equals(pt._V));
        }

        //

        /// <summary>
        /// 将此 PointD5D 结构与指定的对象进行次序比较。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>32 位整数，表示将此 PointD5D 结构与指定的对象进行次序比较得到的结果。</returns>
        public int CompareTo(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return 0;
            }
            else if (obj == null || !(obj is PointD5D))
            {
                return 1;
            }
            else
            {
                return CompareTo((PointD5D)obj);
            }
        }

        /// <summary>
        /// 将此 PointD5D 结构与指定的 PointD5D 结构进行次序比较。
        /// </summary>
        /// <param name="pt">用于比较的 PointD5D 结构。</param>
        /// <returns>32 位整数，表示将此 PointD5D 结构与指定的 PointD5D 结构进行次序比较得到的结果。</returns>
        public int CompareTo(PointD5D pt)
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
        /// 遍历此 PointD5D 结构的所有分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int IndexOf(double item)
        {
            return Array.IndexOf(ToArray(), item, 0, _Dimension);
        }

        /// <summary>
        /// 从指定的索引开始遍历此 PointD5D 结构的所有分量并返回第一个与指定值相等的分量的索引。
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
        /// 从指定的索引开始遍历此 PointD5D 结构指定数量的分量并返回第一个与指定值相等的分量的索引。
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
        /// 逆序遍历此 PointD5D 结构的所有分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int LastIndexOf(double item)
        {
            return Array.LastIndexOf(ToArray(), item, _Dimension - 1, _Dimension);
        }

        /// <summary>
        /// 从指定的索引开始逆序遍历此 PointD5D 结构的所有分量并返回第一个与指定值相等的分量的索引。
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
        /// 从指定的索引开始逆序遍历此 PointD5D 结构指定数量的分量并返回第一个与指定值相等的分量的索引。
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
        /// 遍历此 PointD5D 结构的所有分量并返回表示是否存在与指定值相等的分量的布尔值。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>布尔值，表示是否存在与指定值相等的分量。</returns>
        public bool Contains(double item)
        {
            if (_X.Equals(item) || _Y.Equals(item) || _Z.Equals(item) || _U.Equals(item) || _V.Equals(item))
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
        /// 将此 PointD5D 结构转换为双精度浮点数数组。
        /// </summary>
        /// <returns>双精度浮点数数组，数组元素表示此 PointD5D 结构的分量。</returns>
        public double[] ToArray()
        {
            return new double[_Dimension] { _X, _Y, _Z, _U, _V };
        }

        /// <summary>
        /// 将此 PointD5D 结构转换为双精度浮点数列表。
        /// </summary>
        /// <returns>双精度浮点数列表，列表元素表示此 PointD5D 结构的分量。</returns>
        public List<double> ToList()
        {
            return new List<double>(_Dimension) { _X, _Y, _Z, _U, _V };
        }

        //

        /// <summary>
        /// 返回将此 PointD5D 结构表示的直角坐标系坐标转换为超球坐标系坐标的 PointD5D 结构的新实例。
        /// </summary>
        /// <returns>PointD5D 结构，表示将此 PointD5D 结构表示的直角坐标系坐标转换为超球坐标系坐标得到的结果。</returns>
        public PointD5D ToSpherical()
        {
            Vector result = ToColumnVector().ToSpherical();

            if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }
            else
            {
                return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
            }
        }

        /// <summary>
        /// 返回将此 PointD5D 结构表示的超球坐标系坐标转换为直角坐标系坐标的 PointD5D 结构的新实例。
        /// </summary>
        /// <returns>PointD5D 结构，表示将此 PointD5D 结构表示的超球坐标系坐标转换为直角坐标系坐标得到的结果。</returns>
        public PointD5D ToCartesian()
        {
            Vector result = ToColumnVector().ToCartesian();

            if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }
            else
            {
                return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
            }
        }

        //

        /// <summary>
        /// 返回此 PointD5D 结构与指定的 PointD5D 结构之间的距离。
        /// </summary>
        /// <param name="pt">PointD5D 结构，表示另一个向量。</param>
        /// <returns>双精度浮点数，表示此 PointD5D 结构与指定的 PointD5D 结构之间的距离。</returns>
        public double DistanceFrom(PointD5D pt)
        {
            double AbsDx = Math.Abs(_X - pt._X);
            double AbsDy = Math.Abs(_Y - pt._Y);
            double AbsDz = Math.Abs(_Z - pt._Z);
            double AbsDu = Math.Abs(_U - pt._U);
            double AbsDv = Math.Abs(_V - pt._V);

            double AbsMax = Math.Max(Math.Max(Math.Max(Math.Max(AbsDx, AbsDy), AbsDz), AbsDu), AbsDv);

            if (AbsMax == 0)
            {
                return 0;
            }
            else
            {
                AbsDx /= AbsMax;
                AbsDy /= AbsMax;
                AbsDz /= AbsMax;
                AbsDu /= AbsMax;
                AbsDv /= AbsMax;

                double SqrSum = AbsDx * AbsDx + AbsDy * AbsDy + AbsDz * AbsDz + AbsDu * AbsDu + AbsDv * AbsDv;

                return (AbsMax * Math.Sqrt(SqrSum));
            }
        }

        /// <summary>
        /// 返回此 PointD5D 结构与指定的 PointD5D 结构之间的夹角（弧度）。
        /// </summary>
        /// <param name="pt">PointD5D 结构，表示另一个向量。</param>
        /// <returns>双精度浮点数，表示此 PointD5D 结构与指定的 PointD5D 结构之间的夹角（弧度）。</returns>
        public double AngleFrom(PointD5D pt)
        {
            if (IsZero || pt.IsZero)
            {
                return 0;
            }
            else
            {
                double ModProduct = Module * pt.Module;
                double CosA = _X * pt._X / ModProduct + _Y * pt._Y / ModProduct + _Z * pt._Z / ModProduct + _U * pt._U / ModProduct + _V * pt._V / ModProduct;

                return Math.Acos(CosA);
            }
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的位移将此 PointD5D 结构的所有分量平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public void Offset(double d)
        {
            _X += d;
            _Y += d;
            _Z += d;
            _U += d;
            _V += d;
        }

        /// <summary>
        /// 按双精度浮点数表示的 X 坐标位移、Y 坐标位移、Z 坐标位移、U 坐标位移与 V 坐标位移将此 PointD5D 结构平移指定的量。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标位移。</param>
        /// <param name="du">双精度浮点数表示的 U 坐标位移。</param>
        /// <param name="dv">双精度浮点数表示的 V 坐标位移。</param>
        public void Offset(double dx, double dy, double dz, double du, double dv)
        {
            _X += dx;
            _Y += dy;
            _Z += dz;
            _U += du;
            _V += dv;
        }

        /// <summary>
        /// 按 PointD5D 结构表示的位移将此 PointD5D 结构平移指定的量。
        /// </summary>
        /// <param name="pt">PointD5D 结构表示的位移。</param>
        public void Offset(PointD5D pt)
        {
            _X += pt._X;
            _Y += pt._Y;
            _Z += pt._Z;
            _U += pt._U;
            _V += pt._V;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的位移将此 PointD5D 结构的所有分量平移指定的量的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>PointD5D 结构，表示按双精度浮点数表示的位移将此 PointD5D 结构的所有分量平移指定的量得到的结果。</returns>
        public PointD5D OffsetCopy(double d)
        {
            return new PointD5D(_X + d, _Y + d, _Z + d, _U + d, _V + d);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标位移、Y 坐标位移、Z 坐标位移、U 坐标位移与 V 坐标位移将此 PointD5D 结构平移指定的量的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标位移。</param>
        /// <param name="du">双精度浮点数表示的 U 坐标位移。</param>
        /// <param name="dv">双精度浮点数表示的 V 坐标位移。</param>
        /// <returns>PointD5D 结构，表示按双精度浮点数表示的 X 坐标位移、Y 坐标位移、Z 坐标位移、U 坐标位移与 V 坐标位移将此 PointD5D 结构平移指定的量得到的结果。</returns>
        public PointD5D OffsetCopy(double dx, double dy, double dz, double du, double dv)
        {
            return new PointD5D(_X + dx, _Y + dy, _Z + dz, _U + du, _V + dv);
        }

        /// <summary>
        /// 返回按 PointD5D 结构表示的位移将此 PointD5D 结构平移指定的量的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构表示的位移。</param>
        /// <returns>PointD5D 结构，表示按 PointD5D 结构表示的位移将此 PointD5D 结构平移指定的量得到的结果。</returns>
        public PointD5D OffsetCopy(PointD5D pt)
        {
            return new PointD5D(_X + pt._X, _Y + pt._Y, _Z + pt._Z, _U + pt._U, _V + pt._V);
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的缩放因数将此 PointD5D 结构的所有分量缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public void Scale(double s)
        {
            _X *= s;
            _Y *= s;
            _Z *= s;
            _U *= s;
            _V *= s;
        }

        /// <summary>
        /// 按双精度浮点数表示的 X 坐标缩放因数、Y 坐标缩放因数、Z 坐标缩放因数、U 坐标缩放因数与 V 坐标缩放因数将此 PointD5D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因数。</param>
        /// <param name="su">双精度浮点数表示的 U 坐标缩放因数。</param>
        /// <param name="sv">双精度浮点数表示的 V 坐标缩放因数。</param>
        public void Scale(double sx, double sy, double sz, double su, double sv)
        {
            _X *= sx;
            _Y *= sy;
            _Z *= sz;
            _U *= su;
            _V *= sv;
        }

        /// <summary>
        /// 按 PointD5D 结构表示的缩放因数将此 PointD5D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="pt">PointD5D 结构表示的缩放因数。</param>
        public void Scale(PointD5D pt)
        {
            _X *= pt._X;
            _Y *= pt._Y;
            _Z *= pt._Z;
            _U *= pt._U;
            _V *= pt._V;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数将此 PointD5D 结构的所有分量缩放指定的倍数的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>PointD5D 结构，表示按双精度浮点数表示的缩放因数将此 PointD5D 结构的所有分量缩放指定的倍数得到的结果。</returns>
        public PointD5D ScaleCopy(double s)
        {
            return new PointD5D(_X * s, _Y * s, _Z * s, _U * s, _V * s);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标缩放因数、Y 坐标缩放因数、Z 坐标缩放因数、U 坐标缩放因数与 V 坐标缩放因数将此 PointD5D 结构缩放指定的倍数的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因数。</param>
        /// <param name="su">双精度浮点数表示的 U 坐标缩放因数。</param>
        /// <param name="sv">双精度浮点数表示的 V 坐标缩放因数。</param>
        /// <returns>PointD5D 结构，表示按双精度浮点数表示的 X 坐标缩放因数、Y 坐标缩放因数、Z 坐标缩放因数、U 坐标缩放因数与 V 坐标缩放因数将此 PointD5D 结构缩放指定的倍数得到的结果。</returns>
        public PointD5D ScaleCopy(double sx, double sy, double sz, double su, double sv)
        {
            return new PointD5D(_X * sx, _Y * sy, _Z * sz, _U * su, _V * sv);
        }

        /// <summary>
        /// 返回按 PointD5D 结构表示的缩放因数将此 PointD5D 结构缩放指定的倍数的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构表示的缩放因数。</param>
        /// <returns>PointD5D 结构，表示按 PointD5D 结构表示的缩放因数将此 PointD5D 结构缩放指定的倍数得到的结果。</returns>
        public PointD5D ScaleCopy(PointD5D pt)
        {
            return new PointD5D(_X * pt._X, _Y * pt._Y, _Z * pt._Z, _U * pt._U, _V * pt._V);
        }

        //

        /// <summary>
        /// 将此 PointD5D 结构在指定的基向量方向的分量翻转。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        public void Reflect(int index)
        {
            switch (index)
            {
                case 0: _X = -_X; break;
                case 1: _Y = -_Y; break;
                case 2: _Z = -_Z; break;
                case 3: _U = -_U; break;
                case 4: _V = -_V; break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 返回将此 PointD5D 结构在指定的基向量方向的分量翻转的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        /// <returns>PointD5D 结构，表示将此 PointD5D 结构在指定的基向量方向的分量翻转得到的结果。</returns>
        public PointD5D ReflectCopy(int index)
        {
            switch (index)
            {
                case 0: return new PointD5D(-_X, _Y, _Z, _U, _V);
                case 1: return new PointD5D(_X, -_Y, _Z, _U, _V);
                case 2: return new PointD5D(_X, _Y, -_Z, _U, _V);
                case 3: return new PointD5D(_X, _Y, _Z, -_U, _V);
                case 4: return new PointD5D(_X, _Y, _Z, _U, -_V);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD5D 结构剪切指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向剪切的角度（弧度）。</param>
        public void Shear(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().ShearCopy(index1, index2, angle);

            if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }
            else
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
                _U = result[3];
                _V = result[4];
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD5D 结构剪切指定的角度的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向剪切的角度（弧度）。</param>
        /// <returns>PointD5D 结构，表示按双精度浮点数表示的弧度将此 PointD5D 结构剪切指定的角度得到的结果。</returns>
        public PointD5D ShearCopy(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().ShearCopy(index1, index2, angle);

            if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }
            else
            {
                return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
            }
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD5D 结构旋转指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        public void Rotate(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().RotateCopy(index1, index2, angle);

            if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }
            else
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
                _U = result[3];
                _V = result[4];
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD5D 结构旋转指定的角度的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        /// <returns>PointD5D 结构，表示按双精度浮点数表示的弧度将此 PointD5D 结构旋转指定的角度得到的结果。</returns>
        public PointD5D RotateCopy(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().RotateCopy(index1, index2, angle);

            if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }
            else
            {
                return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
            }
        }

        //

        /// <summary>
        /// 按 PointD5D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量、V 基向量与偏移向量将此 PointD5D 结构进行仿射变换。
        /// </summary>
        /// <param name="ex">PointD5D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD5D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD5D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD5D 结构表示的 U 基向量。</param>
        /// <param name="ev">PointD5D 结构表示的 V 基向量。</param>
        /// <param name="offset">PointD5D 结构表示的偏移向量。</param>
        public void AffineTransform(PointD5D ex, PointD5D ey, PointD5D ez, PointD5D eu, PointD5D ev, PointD5D offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[_Dimension + 1, _Dimension + 1]
            {
                { ex._X, ex._Y, ex._Z, ex._U, ex._V, 0 },
                { ey._X, ey._Y, ey._Z, ey._U, ey._V, 0 },
                { ez._X, ez._Y, ez._Z, ez._U, ez._V, 0 },
                { eu._X, eu._Y, eu._Z, eu._U, eu._V, 0 },
                { ev._X, ev._Y, ev._Z, ev._U, ev._V, 0 },
                { offset._X, offset._Y, offset._Z, offset._U, offset._V, 1 }
            });

            Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

            if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }
            else
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
                _U = result[3];
                _V = result[4];
            }
        }

        /// <summary>
        /// 按 Matrix 对象表示的 6x6 仿射矩阵（左矩阵）将此 PointD5D 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 6x6 仿射矩阵（左矩阵）。</param>
        public void AffineTransform(Matrix matrixLeft)
        {
            if (Matrix.IsNullOrEmpty(matrixLeft) || matrixLeft.Size != _AffineMatrixSize)
            {
                throw new ArithmeticException();
            }
            else
            {
                Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

                if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    _X = result[0];
                    _Y = result[1];
                    _Z = result[2];
                    _U = result[3];
                    _V = result[4];
                }
            }
        }

        /// <summary>
        /// 按 Matrix 对象数组表示的 6x6 仿射矩阵（左矩阵）数组将此 PointD5D 结构进行仿射变换。
        /// </summary>
        /// <param name="matricesLeft">Matrix 对象数组，表示 6x6 仿射矩阵（左矩阵）数组。</param>
        public void AffineTransform(params Matrix[] matricesLeft)
        {
            if (!InternalMethod.IsNullOrEmpty(matricesLeft))
            {
                Vector result = ToColumnVector().AffineTransformCopy(matricesLeft);

                if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    _X = result[0];
                    _Y = result[1];
                    _Z = result[2];
                    _U = result[3];
                    _V = result[4];
                }
            }
        }

        /// <summary>
        /// 按 Matrix 对象列表表示的 6x6 仿射矩阵（左矩阵）列表将此 PointD5D 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 6x6 仿射矩阵（左矩阵）列表。</param>
        public void AffineTransform(List<Matrix> matrixLeftList)
        {
            if (!InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                Vector result = ToColumnVector().AffineTransformCopy(matrixLeftList);

                if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    _X = result[0];
                    _Y = result[1];
                    _Z = result[2];
                    _U = result[3];
                    _V = result[4];
                }
            }
        }

        /// <summary>
        /// 返回按 PointD5D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量、V 基向量与偏移向量将此 PointD5D 结构进行仿射变换的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="ex">PointD5D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD5D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD5D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD5D 结构表示的 U 基向量。</param>
        /// <param name="ev">PointD5D 结构表示的 V 基向量。</param>
        /// <param name="offset">PointD5D 结构表示的偏移向量。</param>
        /// <returns>PointD5D 结构，表示按 PointD5D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量、V 基向量与偏移向量将此 PointD5D 结构进行仿射变换得到的结果。</returns>
        public PointD5D AffineTransformCopy(PointD5D ex, PointD5D ey, PointD5D ez, PointD5D eu, PointD5D ev, PointD5D offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[_Dimension + 1, _Dimension + 1]
            {
                { ex._X, ex._Y, ex._Z, ex._U, ex._V, 0 },
                { ey._X, ey._Y, ey._Z, ey._U, ey._V, 0 },
                { ez._X, ez._Y, ez._Z, ez._U, ez._V, 0 },
                { eu._X, eu._Y, eu._Z, eu._U, eu._V, 0 },
                { ev._X, ev._Y, ev._Z, ev._U, ev._V, 0 },
                { offset._X, offset._Y, offset._Z, offset._U, offset._V, 1 }
            });

            Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

            if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }
            else
            {
                return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的 6x6 仿射矩阵（左矩阵）将此 PointD5D 结构进行仿射变换的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 6x6 仿射矩阵（左矩阵）。</param>
        /// <returns>PointD5D 结构，表示按 Matrix 对象表示的 6x6 仿射矩阵（左矩阵）将此 PointD5D 结构进行仿射变换得到的结果。</returns>
        public PointD5D AffineTransformCopy(Matrix matrixLeft)
        {
            if (Matrix.IsNullOrEmpty(matrixLeft) || matrixLeft.Size != _AffineMatrixSize)
            {
                throw new ArithmeticException();
            }
            else
            {
                Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

                if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
                }
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象数组表示的 6x6 仿射矩阵（左矩阵）数组将此 PointD5D 结构进行仿射变换的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="matricesLeft">Matrix 对象数组，表示 6x6 仿射矩阵（左矩阵）数组。</param>
        /// <returns>PointD5D 结构，表示按 Matrix 对象数组表示的 6x6 仿射矩阵（左矩阵）数组将此 PointD5D 结构进行仿射变换得到的结果。</returns>
        public PointD5D AffineTransformCopy(params Matrix[] matricesLeft)
        {
            if (InternalMethod.IsNullOrEmpty(matricesLeft))
            {
                return this;
            }
            else
            {
                Vector result = ToColumnVector().AffineTransformCopy(matricesLeft);

                if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
                }
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 6x6 仿射矩阵（左矩阵）列表将此 PointD5D 结构进行仿射变换的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 6x6 仿射矩阵（左矩阵）列表。</param>
        /// <returns>PointD5D 结构，表示按 Matrix 对象列表表示的 6x6 仿射矩阵（左矩阵）列表将此 PointD5D 结构进行仿射变换得到的结果。</returns>
        public PointD5D AffineTransformCopy(List<Matrix> matrixLeftList)
        {
            if (InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                return this;
            }
            else
            {
                Vector result = ToColumnVector().AffineTransformCopy(matrixLeftList);

                if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
                }
            }
        }

        /// <summary>
        /// 按 PointD5D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量、V 基向量与偏移向量将此 PointD5D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="ex">PointD5D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD5D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD5D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD5D 结构表示的 U 基向量。</param>
        /// <param name="ev">PointD5D 结构表示的 V 基向量。</param>
        /// <param name="offset">PointD5D 结构表示的偏移向量。</param>
        public void InverseAffineTransform(PointD5D ex, PointD5D ey, PointD5D ez, PointD5D eu, PointD5D ev, PointD5D offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[_Dimension + 1, _Dimension + 1]
            {
                { ex._X, ex._Y, ex._Z, ex._U, ex._V, 0 },
                { ey._X, ey._Y, ey._Z, ey._U, ey._V, 0 },
                { ez._X, ez._Y, ez._Z, ez._U, ez._V, 0 },
                { eu._X, eu._Y, eu._Z, eu._U, eu._V, 0 },
                { ev._X, ev._Y, ev._Z, ev._U, ev._V, 0 },
                { offset._X, offset._Y, offset._Z, offset._U, offset._V, 1 }
            });

            Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

            if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }
            else
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
                _U = result[3];
                _V = result[4];
            }
        }

        /// <summary>
        /// 按 Matrix 对象表示的 6x6 仿射矩阵（左矩阵）将此 PointD5D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 6x6 仿射矩阵（左矩阵）。</param>
        public void InverseAffineTransform(Matrix matrixLeft)
        {
            if (Matrix.IsNullOrEmpty(matrixLeft) || matrixLeft.Size != _AffineMatrixSize)
            {
                throw new ArithmeticException();
            }
            else
            {
                Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

                if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    _X = result[0];
                    _Y = result[1];
                    _Z = result[2];
                    _U = result[3];
                    _V = result[4];
                }
            }
        }

        /// <summary>
        /// 按 Matrix 对象数组表示的 6x6 仿射矩阵（左矩阵）数组将此 PointD5D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matricesLeft">Matrix 对象数组，表示 6x6 仿射矩阵（左矩阵）数组。</param>
        public void InverseAffineTransform(params Matrix[] matricesLeft)
        {
            if (!InternalMethod.IsNullOrEmpty(matricesLeft))
            {
                Vector result = ToColumnVector().InverseAffineTransformCopy(matricesLeft);

                if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    _X = result[0];
                    _Y = result[1];
                    _Z = result[2];
                    _U = result[3];
                    _V = result[4];
                }
            }
        }

        /// <summary>
        /// 按 Matrix 对象列表表示的 6x6 仿射矩阵（左矩阵）列表将此 PointD5D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 6x6 仿射矩阵（左矩阵）列表。</param>
        public void InverseAffineTransform(List<Matrix> matrixLeftList)
        {
            if (!InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeftList);

                if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    _X = result[0];
                    _Y = result[1];
                    _Z = result[2];
                    _U = result[3];
                    _V = result[4];
                }
            }
        }

        /// <summary>
        /// 返回按 PointD5D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量、V 基向量与偏移向量将此 PointD5D 结构进行逆仿射变换的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="ex">PointD5D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD5D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD5D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD5D 结构表示的 U 基向量。</param>
        /// <param name="ev">PointD5D 结构表示的 V 基向量。</param>
        /// <param name="offset">PointD5D 结构表示的偏移向量。</param>
        /// <returns>PointD5D 结构，表示按 PointD5D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量、V 基向量与偏移向量将此 PointD5D 结构进行逆仿射变换得到的结果。</returns>
        public PointD5D InverseAffineTransformCopy(PointD5D ex, PointD5D ey, PointD5D ez, PointD5D eu, PointD5D ev, PointD5D offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[_Dimension + 1, _Dimension + 1]
            {
                { ex._X, ex._Y, ex._Z, ex._U, ex._V, 0 },
                { ey._X, ey._Y, ey._Z, ey._U, ey._V, 0 },
                { ez._X, ez._Y, ez._Z, ez._U, ez._V, 0 },
                { eu._X, eu._Y, eu._Z, eu._U, eu._V, 0 },
                { ev._X, ev._Y, ev._Z, ev._U, ev._V, 0 },
                { offset._X, offset._Y, offset._Z, offset._U, offset._V, 1 }
            });

            Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

            if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }
            else
            {
                return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的 6x6 仿射矩阵（左矩阵）将此 PointD5D 结构进行逆仿射变换的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 6x6 仿射矩阵（左矩阵）。</param>
        /// <returns>PointD5D 结构，表示按 Matrix 对象表示的 6x6 仿射矩阵（左矩阵）将此 PointD5D 结构进行逆仿射变换得到的结果。</returns>
        public PointD5D InverseAffineTransformCopy(Matrix matrixLeft)
        {
            if (Matrix.IsNullOrEmpty(matrixLeft) || matrixLeft.Size != _AffineMatrixSize)
            {
                throw new ArithmeticException();
            }
            else
            {
                Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

                if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
                }
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象数组表示的 6x6 仿射矩阵（左矩阵）数组将此 PointD5D 结构进行逆仿射变换的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="matricesLeft">Matrix 对象数组，表示 6x6 仿射矩阵（左矩阵）数组。</param>
        /// <returns>PointD5D 结构，表示按 Matrix 对象数组表示的 6x6 仿射矩阵（左矩阵）数组将此 PointD5D 结构进行逆仿射变换得到的结果。</returns>
        public PointD5D InverseAffineTransformCopy(params Matrix[] matricesLeft)
        {
            if (InternalMethod.IsNullOrEmpty(matricesLeft))
            {
                return this;
            }
            else
            {
                Vector result = ToColumnVector().InverseAffineTransformCopy(matricesLeft);

                if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
                }
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 6x6 仿射矩阵（左矩阵）列表将此 PointD5D 结构进行逆仿射变换的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 6x6 仿射矩阵（左矩阵）列表。</param>
        /// <returns>PointD5D 结构，表示按 Matrix 对象列表表示的 6x6 仿射矩阵（左矩阵）列表将此 PointD5D 结构进行逆仿射变换得到的结果。</returns>
        public PointD5D InverseAffineTransformCopy(List<Matrix> matrixLeftList)
        {
            if (InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                return this;
            }
            else
            {
                Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeftList);

                if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
                }
            }
        }

        //

        /// <summary>
        /// 返回将此 PointD5D 结构投影至平行于 XYZU 空间的投影空间的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD5D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维测度其真实大小与投影大小的比值等于其到投影空间的距离与此距离的比值。</param>
        /// <returns>PointD4D 结构，表示将此 PointD5D 结构投影至平行于 XYZU 空间的投影空间得到的结果。</returns>
        public PointD4D ProjectToXYZU(PointD5D prjCenter, double trueLenDist)
        {
            if (trueLenDist == 0)
            {
                return XYZU;
            }
            else
            {
                if (_V == prjCenter._V)
                {
                    return PointD4D.NaN;
                }
                else
                {
                    double Scale = trueLenDist / (_V - prjCenter._V);

                    if (InternalMethod.IsNaNOrInfinity(Scale) || Scale <= 0)
                    {
                        return PointD4D.NaN;
                    }
                    else
                    {
                        return (Scale * XYZU + (1 - Scale) * prjCenter.XYZU);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 PointD5D 结构投影至平行于 YZUV 空间的投影空间的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD5D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维测度其真实大小与投影大小的比值等于其到投影空间的距离与此距离的比值。</param>
        /// <returns>PointD4D 结构，表示将此 PointD5D 结构投影至平行于 YZUV 空间的投影空间得到的结果。</returns>
        public PointD4D ProjectToYZUV(PointD5D prjCenter, double trueLenDist)
        {
            if (trueLenDist == 0)
            {
                return YZUV;
            }
            else
            {
                if (_X == prjCenter._X)
                {
                    return PointD4D.NaN;
                }
                else
                {
                    double Scale = trueLenDist / (_X - prjCenter._X);

                    if (InternalMethod.IsNaNOrInfinity(Scale) || Scale <= 0)
                    {
                        return PointD4D.NaN;
                    }
                    else
                    {
                        return (Scale * YZUV + (1 - Scale) * prjCenter.YZUV);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 PointD5D 结构投影至平行于 ZUVX 空间的投影空间的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD5D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维测度其真实大小与投影大小的比值等于其到投影空间的距离与此距离的比值。</param>
        /// <returns>PointD4D 结构，表示将此 PointD5D 结构投影至平行于 ZUVX 空间的投影空间得到的结果。</returns>
        public PointD4D ProjectToZUVX(PointD5D prjCenter, double trueLenDist)
        {
            if (trueLenDist == 0)
            {
                return ZUVX;
            }
            else
            {
                if (_Y == prjCenter._Y)
                {
                    return PointD4D.NaN;
                }
                else
                {
                    double Scale = trueLenDist / (_Y - prjCenter._Y);

                    if (InternalMethod.IsNaNOrInfinity(Scale) || Scale <= 0)
                    {
                        return PointD4D.NaN;
                    }
                    else
                    {
                        return (Scale * ZUVX + (1 - Scale) * prjCenter.ZUVX);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 PointD5D 结构投影至平行于 UVXY 空间的投影空间的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD5D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维测度其真实大小与投影大小的比值等于其到投影空间的距离与此距离的比值。</param>
        /// <returns>PointD4D 结构，表示将此 PointD5D 结构投影至平行于 UVXY 空间的投影空间得到的结果。</returns>
        public PointD4D ProjectToUVXY(PointD5D prjCenter, double trueLenDist)
        {
            if (trueLenDist == 0)
            {
                return UVXY;
            }
            else
            {
                if (_Z == prjCenter._Z)
                {
                    return PointD4D.NaN;
                }
                else
                {
                    double Scale = trueLenDist / (_Z - prjCenter._Z);

                    if (InternalMethod.IsNaNOrInfinity(Scale) || Scale <= 0)
                    {
                        return PointD4D.NaN;
                    }
                    else
                    {
                        return (Scale * UVXY + (1 - Scale) * prjCenter.UVXY);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 PointD5D 结构投影至平行于 VXYZ 空间的投影空间的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD5D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维测度其真实大小与投影大小的比值等于其到投影空间的距离与此距离的比值。</param>
        /// <returns>PointD4D 结构，表示将此 PointD5D 结构投影至平行于 VXYZ 空间的投影空间得到的结果。</returns>
        public PointD4D ProjectToVXYZ(PointD5D prjCenter, double trueLenDist)
        {
            if (trueLenDist == 0)
            {
                return VXYZ;
            }
            else
            {
                if (_U == prjCenter._U)
                {
                    return PointD4D.NaN;
                }
                else
                {
                    double Scale = trueLenDist / (_U - prjCenter._U);

                    if (InternalMethod.IsNaNOrInfinity(Scale) || Scale <= 0)
                    {
                        return PointD4D.NaN;
                    }
                    else
                    {
                        return (Scale * VXYZ + (1 - Scale) * prjCenter.VXYZ);
                    }
                }
            }
        }

        //

        /// <summary>
        /// 返回将此 PointD5D 结构转换为列向量的 Vector 的新实例。
        /// </summary>
        /// <returns>Vector 对象，表示将此 PointD5D 结构转换为列向量得到的结果。</returns>
        public Vector ToColumnVector()
        {
            return Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, _X, _Y, _Z, _U, _V);
        }

        /// <summary>
        /// 返回将此 PointD5D 结构转换为行向量的 Vector 的新实例。
        /// </summary>
        /// <returns>Vector 对象，表示将此 PointD5D 结构转换为行向量得到的结果。</returns>
        public Vector ToRowVector()
        {
            return Vector.UnsafeCreateInstance(Vector.Type.RowVector, _X, _Y, _Z, _U, _V);
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断两个 PointD5D 结构是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 PointD5D 结构。</param>
        /// <param name="right">用于比较的第二个 PointD5D 结构。</param>
        /// <returns>布尔值，表示两个 PointD5D 结构是否相等。</returns>
        public static bool Equals(PointD5D left, PointD5D right)
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
        /// 比较两个 PointD5D 结构的次序。
        /// </summary>
        /// <param name="left">用于比较的第一个 PointD5D 结构。</param>
        /// <param name="right">用于比较的第二个 PointD5D 结构。</param>
        /// <returns>32 位整数，表示将两个 PointD5D 结构进行次序比较得到的结果。</returns>
        public static int Compare(PointD5D left, PointD5D right)
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
        /// 返回单位矩阵，表示不对 PointD5D 结构进行仿射变换的 6x6 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <returns>Matrix 对象，表示不对 PointD5D 结构进行仿射变换的 6x6 仿射矩阵（左矩阵）。</returns>
        public static Matrix IdentityMatrix()
        {
            return Matrix.Identity(_Dimension + 1);
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的位移将 PointD5D 结构的所有分量平移指定的量的 6x6 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的位移将 PointD5D 结构的所有分量平移指定的量的 6x6 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(double d)
        {
            return Vector.OffsetMatrix(Vector.Type.ColumnVector, _Dimension, d);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的 X 坐标位移、Y 坐标位移、Z 坐标位移、U 坐标位移与 V 坐标位移将 PointD5D 结构平移指定的量的 6x6 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标位移。</param>
        /// <param name="du">双精度浮点数表示的 U 坐标位移。</param>
        /// <param name="dv">双精度浮点数表示的 V 坐标位移。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的 X 坐标位移、Y 坐标位移、Z 坐标位移、U 坐标位移与 V 坐标位移将 PointD5D 结构平移指定的量的 6x6 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(double dx, double dy, double dz, double du, double dv)
        {
            return Vector.OffsetMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, dx, dy, dz, du, dv));
        }

        /// <summary>
        /// 返回表示按 PointD5D 结构表示的位移将 PointD5D 结构平移指定的量的 6x6 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构表示的位移。</param>
        /// <returns>Matrix 对象，表示按 PointD5D 结构表示的位移将 PointD5D 结构平移指定的量的 6x6 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(PointD5D pt)
        {
            return Vector.OffsetMatrix(pt.ToColumnVector());
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的缩放因数将 PointD5D 结构的所有分量缩放指定的倍数的 6x6 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的缩放因数将 PointD5D 结构的所有分量缩放指定的倍数的 6x6 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(double s)
        {
            return Vector.ScaleMatrix(Vector.Type.ColumnVector, _Dimension, s);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的 X 坐标缩放因数、Y 坐标缩放因数、Z 坐标缩放因数、U 坐标缩放因数与 V 坐标缩放因数将此 PointD5D 结构缩放指定的倍数的 6x6 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因数。</param>
        /// <param name="su">双精度浮点数表示的 U 坐标缩放因数。</param>
        /// <param name="sv">双精度浮点数表示的 V 坐标缩放因数。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的 X 坐标缩放因数、Y 坐标缩放因数、Z 坐标缩放因数、U 坐标缩放因数与 V 坐标缩放因数将此 PointD5D 结构缩放指定的倍数的 6x6 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(double sx, double sy, double sz, double su, double sv)
        {
            return Vector.ScaleMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, sx, sy, sz, su, sv));
        }

        /// <summary>
        /// 返回表示按 PointD5D 结构表示的缩放因数将 PointD5D 结构缩放指定的倍数的 6x6 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按 PointD5D 结构表示的缩放因数将 PointD5D 结构缩放指定的倍数的 6x6 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(PointD5D pt)
        {
            return Vector.ScaleMatrix(pt.ToColumnVector());
        }

        //

        /// <summary>
        /// 返回表示用于翻转 PointD5D 结构的 6x6 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        /// <returns>Matrix 对象，表示用于翻转 PointD5D 结构的 6x6 仿射矩阵（左矩阵）。</returns>
        public static Matrix ReflectMatrix(int index)
        {
            return Vector.ReflectMatrix(Vector.Type.ColumnVector, _Dimension, index);
        }

        //

        /// <summary>
        /// 返回表示用于剪切 PointD5D 结构的 6x6 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向剪切的角度（弧度）。</param>
        /// <returns>Matrix 对象，表示用于剪切 PointD5D 结构的 6x6 仿射矩阵（左矩阵）。</returns>
        public static Matrix ShearMatrix(int index1, int index2, double angle)
        {
            return Vector.ShearMatrix(Vector.Type.ColumnVector, _Dimension, index1, index2, angle);
        }

        //

        /// <summary>
        /// 返回表示用于旋转 PointD5D 结构的 6x6 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示 PointD5D 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        /// <returns>Matrix 对象，表示用于旋转 PointD5D 结构的 6x6 仿射矩阵（左矩阵）。</returns>
        public static Matrix RotateMatrix(int index1, int index2, double angle)
        {
            return Vector.RotateMatrix(Vector.Type.ColumnVector, _Dimension, index1, index2, angle);
        }

        //

        /// <summary>
        /// 返回两个 PointD5D 结构之间的距离。
        /// </summary>
        /// <param name="left">第一个 PointD5D 结构。</param>
        /// <param name="right">第二个 PointD5D 结构。</param>
        /// <returns>双精度浮点数，表示两个 PointD5D 结构之间的距离。</returns>
        public static double DistanceBetween(PointD5D left, PointD5D right)
        {
            double AbsDx = Math.Abs(left._X - right._X);
            double AbsDy = Math.Abs(left._Y - right._Y);
            double AbsDz = Math.Abs(left._Z - right._Z);
            double AbsDu = Math.Abs(left._U - right._U);
            double AbsDv = Math.Abs(left._V - right._V);

            double AbsMax = Math.Max(Math.Max(Math.Max(Math.Max(AbsDx, AbsDy), AbsDz), AbsDu), AbsDv);

            if (AbsMax == 0)
            {
                return 0;
            }
            else
            {
                AbsDx /= AbsMax;
                AbsDy /= AbsMax;
                AbsDz /= AbsMax;
                AbsDu /= AbsMax;
                AbsDv /= AbsMax;

                double SqrSum = AbsDx * AbsDx + AbsDy * AbsDy + AbsDz * AbsDz + AbsDu * AbsDu + AbsDv * AbsDv;

                return (AbsMax * Math.Sqrt(SqrSum));
            }
        }

        /// <summary>
        /// 返回两个 PointD5D 结构之间的夹角（弧度）。
        /// </summary>
        /// <param name="left">第一个 PointD5D 结构。</param>
        /// <param name="right">第二个 PointD5D 结构。</param>
        /// <returns>双精度浮点数，表示两个 PointD5D 结构之间的夹角（弧度）。</returns>
        public static double AngleBetween(PointD5D left, PointD5D right)
        {
            if (left.IsZero || right.IsZero)
            {
                return 0;
            }
            else
            {
                double ModProduct = left.Module * right.Module;
                double CosA = left._X * right._X / ModProduct + left._Y * right._Y / ModProduct + left._Z * right._Z / ModProduct + left._U * right._U / ModProduct + left._V * right._V / ModProduct;

                return Math.Acos(CosA);
            }
        }

        //

        /// <summary>
        /// 返回两个 PointD5D 结构的数量积。
        /// </summary>
        /// <param name="left">第一个 PointD5D 结构。</param>
        /// <param name="right">第二个 PointD5D 结构。</param>
        /// <returns>双精度浮点数，表示两个 PointD5D 结构的数量积。</returns>
        public static double DotProduct(PointD5D left, PointD5D right)
        {
            return Vector.DotProduct(left.ToColumnVector(), right.ToColumnVector());
        }

        /// <summary>
        /// 返回两个 PointD5D 结构的向量积。该向量积为一个十维向量，其所有分量的数值依次为 X∧Y 基向量、X∧Z 基向量、X∧U 基向量、X∧V 基向量、Y∧Z 基向量、Y∧U 基向量、Y∧V 基向量、Z∧U 基向量、Z∧V 基向量与 U∧V 基向量的系数。
        /// </summary>
        /// <param name="left">第一个 PointD5D 结构。</param>
        /// <param name="right">第二个 PointD5D 结构。</param>
        /// <returns>Vector 对象，表示两个 PointD5D 结构的向量积。</returns>
        public static Vector CrossProduct(PointD5D left, PointD5D right)
        {
            return Vector.CrossProduct(left.ToColumnVector(), right.ToColumnVector());
        }

        //

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量取符号数得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于转换的结构。</param>
        /// <returns>PointD5D 结构，表示将 PointD5D 结构的所有分量取符号数得到的结果</returns>
        public static PointD5D Sign(PointD5D pt)
        {
            return new PointD5D((double.IsNaN(pt._X) ? 0 : Math.Sign(pt._X)), (double.IsNaN(pt._Y) ? 0 : Math.Sign(pt._Y)), (double.IsNaN(pt._Z) ? 0 : Math.Sign(pt._Z)), (double.IsNaN(pt._U) ? 0 : Math.Sign(pt._U)), (double.IsNaN(pt._V) ? 0 : Math.Sign(pt._V)));
        }

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量取绝对值得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于转换的结构。</param>
        /// <returns>PointD5D 结构，表示将 PointD5D 结构的所有分量取绝对值得到的结果</returns>
        public static PointD5D Abs(PointD5D pt)
        {
            return new PointD5D(Math.Abs(pt._X), Math.Abs(pt._Y), Math.Abs(pt._Z), Math.Abs(pt._U), Math.Abs(pt._V));
        }

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量舍入到较大的整数值得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于转换的结构。</param>
        /// <returns>PointD5D 结构，表示将 PointD5D 结构的所有分量舍入到较大的整数值得到的结果</returns>
        public static PointD5D Ceiling(PointD5D pt)
        {
            return new PointD5D(Math.Ceiling(pt._X), Math.Ceiling(pt._Y), Math.Ceiling(pt._Z), Math.Ceiling(pt._U), Math.Ceiling(pt._V));
        }

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量舍入到较小的整数值得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于转换的结构。</param>
        /// <returns>PointD5D 结构，表示将 PointD5D 结构的所有分量舍入到较小的整数值得到的结果</returns>
        public static PointD5D Floor(PointD5D pt)
        {
            return new PointD5D(Math.Floor(pt._X), Math.Floor(pt._Y), Math.Floor(pt._Z), Math.Floor(pt._U), Math.Floor(pt._V));
        }

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量舍入到最接近的整数值得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于转换的结构。</param>
        /// <returns>PointD5D 结构，表示将 PointD5D 结构的所有分量舍入到最接近的整数值得到的结果</returns>
        public static PointD5D Round(PointD5D pt)
        {
            return new PointD5D(Math.Round(pt._X), Math.Round(pt._Y), Math.Round(pt._Z), Math.Round(pt._U), Math.Round(pt._V));
        }

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量截断小数部分取整得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于转换的结构。</param>
        /// <returns>PointD5D 结构，表示将 PointD5D 结构的所有分量截断小数部分取整得到的结果</returns>
        public static PointD5D Truncate(PointD5D pt)
        {
            return new PointD5D(Math.Truncate(pt._X), Math.Truncate(pt._Y), Math.Truncate(pt._Z), Math.Truncate(pt._U), Math.Truncate(pt._V));
        }

        /// <summary>
        /// 返回将两个 PointD5D 结构的所有分量分别取最大值得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD5D 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD5D 结构，用于比较的第二个结构。</param>
        /// <returns>PointD5D 结构，表示将两个 PointD5D 结构的所有分量分别取最大值得到的结果</returns>
        public static PointD5D Max(PointD5D left, PointD5D right)
        {
            return new PointD5D(Math.Max(left._X, right._X), Math.Max(left._Y, right._Y), Math.Max(left._Z, right._Z), Math.Max(left._U, right._U), Math.Max(left._V, right._V));
        }

        /// <summary>
        /// 返回将两个 PointD5D 结构的所有分量分别取最小值得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD5D 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD5D 结构，用于比较的第二个结构。</param>
        /// <returns>PointD5D 结构，表示将两个 PointD5D 结构的所有分量分别取最小值得到的结果</returns>
        public static PointD5D Min(PointD5D left, PointD5D right)
        {
            return new PointD5D(Math.Min(left._X, right._X), Math.Min(left._Y, right._Y), Math.Min(left._Z, right._Z), Math.Min(left._U, right._U), Math.Min(left._V, right._V));
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 PointD5D 结构是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD5D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD5D 结构。</param>
        /// <returns>布尔值，表示两个 PointD5D 结构是否相等。</returns>
        public static bool operator ==(PointD5D left, PointD5D right)
        {
            return (left._X == right._X && left._Y == right._Y && left._Z == right._Z && left._U == right._U && left._V == right._V);
        }

        /// <summary>
        /// 判断两个 PointD5D 结构是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD5D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD5D 结构。</param>
        /// <returns>布尔值，表示两个 PointD5D 结构是否不相等。</returns>
        public static bool operator !=(PointD5D left, PointD5D right)
        {
            return (left._X != right._X || left._Y != right._Y || left._Z != right._Z || left._U != right._U || left._V != right._V);
        }

        /// <summary>
        /// 判断两个 PointD5D 结构的字典序是否前者小于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD5D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD5D 结构。</param>
        /// <returns>布尔值，表示两个 PointD5D 结构的字典序是否前者小于后者。</returns>
        public static bool operator <(PointD5D left, PointD5D right)
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
        /// 判断两个 PointD5D 结构的字典序是否前者大于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD5D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD5D 结构。</param>
        /// <returns>布尔值，表示两个 PointD5D 结构的字典序是否前者大于后者。</returns>
        public static bool operator >(PointD5D left, PointD5D right)
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
        /// 判断两个 PointD5D 结构的字典序是否前者小于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD5D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD5D 结构。</param>
        /// <returns>布尔值，表示两个 PointD5D 结构的字典序是否前者小于或等于后者。</returns>
        public static bool operator <=(PointD5D left, PointD5D right)
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
        /// 判断两个 PointD5D 结构的字典序是否前者大于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD5D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD5D 结构。</param>
        /// <returns>布尔值，表示两个 PointD5D 结构的字典序是否前者大于或等于后者。</returns>
        public static bool operator >=(PointD5D left, PointD5D right)
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
        /// 返回在 PointD5D 结构的所有分量前添加正号得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">运算符右侧的 PointD5D 结构。</param>
        /// <returns>PointD5D 结构，表示在 PointD5D 结构的所有分量前添加正号得到的结果。</returns>
        public static PointD5D operator +(PointD5D pt)
        {
            return pt;
        }

        /// <summary>
        /// 返回在 PointD5D 结构的所有分量前添加负号得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">运算符右侧的 PointD5D 结构。</param>
        /// <returns>PointD5D 结构，表示在 PointD5D 结构的所有分量前添加负号得到的结果。</returns>
        public static PointD5D operator -(PointD5D pt)
        {
            return new PointD5D(-pt._X, -pt._Y, -pt._Z, -pt._U, -pt._V);
        }

        //

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量与双精度浮点数相加得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，表示被加数。</param>
        /// <param name="n">双精度浮点数，表示加数。</param>
        /// <returns>PointD5D 结构，表示将 PointD5D 结构的所有分量与双精度浮点数相加得到的结果。</returns>
        public static PointD5D operator +(PointD5D pt, double n)
        {
            return new PointD5D(pt._X + n, pt._Y + n, pt._Z + n, pt._U + n, pt._V + n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD5D 结构的所有分量相加得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被加数。</param>
        /// <param name="pt">PointD5D 结构，表示加数。</param>
        /// <returns>PointD5D 结构，表示将双精度浮点数与 PointD5D 结构的所有分量相加得到的结果。</returns>
        public static PointD5D operator +(double n, PointD5D pt)
        {
            return new PointD5D(n + pt._X, n + pt._Y, n + pt._Z, n + pt._U, n + pt._V);
        }

        /// <summary>
        /// 返回将 PointD5D 结构与 PointD5D 结构的所有分量对应相加得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD5D 结构，表示被加数。</param>
        /// <param name="right">PointD5D 结构，表示加数。</param>
        /// <returns>PointD5D 结构，表示将 PointD5D 结构与 PointD5D 结构的所有分量对应相加得到的结果。</returns>
        public static PointD5D operator +(PointD5D left, PointD5D right)
        {
            return new PointD5D(left._X + right._X, left._Y + right._Y, left._Z + right._Z, left._U + right._U, left._V + right._V);
        }

        //

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量与双精度浮点数相减得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，表示被减数。</param>
        /// <param name="n">双精度浮点数，表示减数。</param>
        /// <returns>PointD5D 结构，表示将 PointD5D 结构的所有分量与双精度浮点数相减得到的结果。</returns>
        public static PointD5D operator -(PointD5D pt, double n)
        {
            return new PointD5D(pt._X - n, pt._Y - n, pt._Z - n, pt._U - n, pt._V - n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD5D 结构的所有分量相减得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被减数。</param>
        /// <param name="pt">PointD5D 结构，表示减数。</param>
        /// <returns>PointD5D 结构，表示将双精度浮点数与 PointD5D 结构的所有分量相减得到的结果。</returns>
        public static PointD5D operator -(double n, PointD5D pt)
        {
            return new PointD5D(n - pt._X, n - pt._Y, n - pt._Z, n - pt._U, n - pt._V);
        }

        /// <summary>
        /// 返回将 PointD5D 结构与 PointD5D 结构的所有分量对应相减得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD5D 结构，表示被减数。</param>
        /// <param name="right">PointD5D 结构，表示减数。</param>
        /// <returns>PointD5D 结构，表示将 PointD5D 结构与 PointD5D 结构的所有分量对应相减得到的结果。</returns>
        public static PointD5D operator -(PointD5D left, PointD5D right)
        {
            return new PointD5D(left._X - right._X, left._Y - right._Y, left._Z - right._Z, left._U - right._U, left._V - right._V);
        }

        //

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量与双精度浮点数相乘得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，表示被乘数。</param>
        /// <param name="n">双精度浮点数，表示乘数。</param>
        /// <returns>PointD5D 结构，表示将 PointD5D 结构的所有分量与双精度浮点数相乘得到的结果。</returns>
        public static PointD5D operator *(PointD5D pt, double n)
        {
            return new PointD5D(pt._X * n, pt._Y * n, pt._Z * n, pt._U * n, pt._V * n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD5D 结构的所有分量相乘得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被乘数。</param>
        /// <param name="pt">PointD5D 结构，表示乘数。</param>
        /// <returns>PointD5D 结构，表示将双精度浮点数与 PointD5D 结构的所有分量相乘得到的结果。</returns>
        public static PointD5D operator *(double n, PointD5D pt)
        {
            return new PointD5D(n * pt._X, n * pt._Y, n * pt._Z, n * pt._U, n * pt._V);
        }

        /// <summary>
        /// 返回将 PointD5D 结构与 PointD5D 结构的所有分量对应相乘得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD5D 结构，表示被乘数。</param>
        /// <param name="right">PointD5D 结构，表示乘数。</param>
        /// <returns>PointD5D 结构，表示将 PointD5D 结构与 PointD5D 结构的所有分量对应相乘得到的结果。</returns>
        public static PointD5D operator *(PointD5D left, PointD5D right)
        {
            return new PointD5D(left._X * right._X, left._Y * right._Y, left._Z * right._Z, left._U * right._U, left._V * right._V);
        }

        //

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量与双精度浮点数相除得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，表示被除数。</param>
        /// <param name="n">双精度浮点数，表示除数。</param>
        /// <returns>PointD5D 结构，表示将 PointD5D 结构的所有分量与双精度浮点数相除得到的结果。</returns>
        public static PointD5D operator /(PointD5D pt, double n)
        {
            return new PointD5D(pt._X / n, pt._Y / n, pt._Z / n, pt._U / n, pt._V / n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD5D 结构的所有分量相除得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被除数。</param>
        /// <param name="pt">PointD5D 结构，表示除数。</param>
        /// <returns>PointD5D 结构，表示将双精度浮点数与 PointD5D 结构的所有分量相除得到的结果。</returns>
        public static PointD5D operator /(double n, PointD5D pt)
        {
            return new PointD5D(n / pt._X, n / pt._Y, n / pt._Z, n / pt._U, n / pt._V);
        }

        /// <summary>
        /// 返回将 PointD5D 结构与 PointD5D 结构的所有分量对应相除得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD5D 结构，表示被除数。</param>
        /// <param name="right">PointD5D 结构，表示除数。</param>
        /// <returns>PointD5D 结构，表示将 PointD5D 结构与 PointD5D 结构的所有分量对应相除得到的结果。</returns>
        public static PointD5D operator /(PointD5D left, PointD5D right)
        {
            return new PointD5D(left._X / right._X, left._Y / right._Y, left._Z / right._Z, left._U / right._U, left._V / right._V);
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
            this = new PointD5D();
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
            private PointD5D _Pt;
            private int _Index;

            internal Enumerator(PointD5D pt)
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
            this = new PointD5D();
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
            private PointD5D _Pt;
            private int _Index;

            internal GenericEnumerator(PointD5D pt)
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