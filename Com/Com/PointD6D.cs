﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.PointD6D
Version 19.8.5.2000

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
    /// 以一组有序的双精度浮点数表示的六维直角坐标系坐标。
    /// </summary>
    public struct PointD6D : IEquatable<PointD6D>, IComparable, IComparable<PointD6D>, IEuclideanVector<PointD6D>, IAffine<PointD6D>
    {
        #region 私有成员与内部成员

        private const int _Dimension = 6; // PointD6D 结构的维度。

        private static readonly Size _AffineMatrixSize = new Size(_Dimension + 1, _Dimension + 1); // PointD6D 结构的仿射矩阵大小。

        //

        private double _X; // X 坐标。
        private double _Y; // Y 坐标。
        private double _Z; // Z 坐标。
        private double _U; // U 坐标。
        private double _V; // V 坐标。
        private double _W; // W 坐标。

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用双精度浮点数表示的 X 坐标、Y 坐标、Z 坐标、U 坐标、V 坐标与 W 坐标初始化 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="x">双精度浮点数表示的 X 坐标。</param>
        /// <param name="y">双精度浮点数表示的 Y 坐标。</param>
        /// <param name="z">双精度浮点数表示的 Z 坐标。</param>
        /// <param name="u">双精度浮点数表示的 U 坐标。</param>
        /// <param name="v">双精度浮点数表示的 V 坐标。</param>
        /// <param name="w">双精度浮点数表示的 W 坐标。</param>
        public PointD6D(double x, double y, double z, double u, double v, double w)
        {
            _X = x;
            _Y = y;
            _Z = z;
            _U = u;
            _V = v;
            _W = w;
        }

        #endregion

        #region 字段

        /// <summary>
        /// 表示零向量的 PointD6D 结构的实例。
        /// </summary>
        public static readonly PointD6D Zero = new PointD6D(0, 0, 0, 0, 0, 0);

        //

        /// <summary>
        /// 表示所有分量为非数字的 PointD6D 结构的实例。
        /// </summary>
        public static readonly PointD6D NaN = new PointD6D(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);

        //

        /// <summary>
        /// 表示 X 基向量的 PointD6D 结构的实例。
        /// </summary>
        public static readonly PointD6D Ex = new PointD6D(1, 0, 0, 0, 0, 0);

        /// <summary>
        /// 表示 Y 基向量的 PointD6D 结构的实例。
        /// </summary>
        public static readonly PointD6D Ey = new PointD6D(0, 1, 0, 0, 0, 0);

        /// <summary>
        /// 表示 Z 基向量的 PointD6D 结构的实例。
        /// </summary>
        public static readonly PointD6D Ez = new PointD6D(0, 0, 1, 0, 0, 0);

        /// <summary>
        /// 表示 U 基向量的 PointD6D 结构的实例。
        /// </summary>
        public static readonly PointD6D Eu = new PointD6D(0, 0, 0, 1, 0, 0);

        /// <summary>
        /// 表示 V 基向量的 PointD6D 结构的实例。
        /// </summary>
        public static readonly PointD6D Ev = new PointD6D(0, 0, 0, 0, 1, 0);

        /// <summary>
        /// 表示 W 基向量的 PointD6D 结构的实例。
        /// </summary>
        public static readonly PointD6D Ew = new PointD6D(0, 0, 0, 0, 0, 1);

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置此 PointD6D 结构在指定的基向量方向的分量。
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
                    case 5: return _W;
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
                    case 5: _W = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置此 PointD6D 结构在 X 轴的分量。
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
        /// 获取或设置此 PointD6D 结构在 Y 轴的分量。
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
        /// 获取或设置此 PointD6D 结构在 Z 轴的分量。
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
        /// 获取或设置此 PointD6D 结构在 U 轴的分量。
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
        /// 获取或设置此 PointD6D 结构在 V 轴的分量。
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

        /// <summary>
        /// 获取或设置此 PointD6D 结构在 W 轴的分量。
        /// </summary>
        public double W
        {
            get
            {
                return _W;
            }

            set
            {
                _W = value;
            }
        }

        //

        /// <summary>
        /// 获取此 PointD6D 结构的维度。
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
        /// 获取表示此 PointD6D 结构是否为空向量的布尔值。
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获取表示此 PointD6D 结构是否为零向量的布尔值。
        /// </summary>
        public bool IsZero
        {
            get
            {
                return (_X == 0 && _Y == 0 && _Z == 0 && _U == 0 && _V == 0 && _W == 0);
            }
        }

        /// <summary>
        /// 获取表示此 PointD6D 结构是否只读的布尔值。
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获取表示此 PointD6D 结构是否具有固定的维度的布尔值。
        /// </summary>
        public bool IsFixedSize
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 获取表示此 PointD6D 结构是否包含非数字分量的布尔值。
        /// </summary>
        public bool IsNaN
        {
            get
            {
                return (double.IsNaN(_X) || double.IsNaN(_Y) || double.IsNaN(_Z) || double.IsNaN(_U) || double.IsNaN(_V) || double.IsNaN(_W));
            }
        }

        /// <summary>
        /// 获取表示此 PointD6D 结构是否包含无穷大分量的布尔值。
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return (!IsNaN && (double.IsInfinity(_X) || double.IsInfinity(_Y) || double.IsInfinity(_Z) || double.IsInfinity(_U) || double.IsInfinity(_V) || double.IsInfinity(_W)));
            }
        }

        /// <summary>
        /// 获取表示此 PointD6D 结构是否包含非数字或无穷大分量的布尔值。
        /// </summary>
        public bool IsNaNOrInfinity
        {
            get
            {
                return (InternalMethod.IsNaNOrInfinity(_X) || InternalMethod.IsNaNOrInfinity(_Y) || InternalMethod.IsNaNOrInfinity(_Z) || InternalMethod.IsNaNOrInfinity(_U) || InternalMethod.IsNaNOrInfinity(_V) || InternalMethod.IsNaNOrInfinity(_W));
            }
        }

        //

        /// <summary>
        /// 获取此 PointD6D 结构的模。
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
                double AbsW = Math.Abs(_W);

                double AbsMax = Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(AbsX, AbsY), AbsZ), AbsU), AbsV), AbsW);

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
                    AbsW /= AbsMax;

                    double SqrSum = AbsX * AbsX + AbsY * AbsY + AbsZ * AbsZ + AbsU * AbsU + AbsV * AbsV + AbsW * AbsW;

                    return (AbsMax * Math.Sqrt(SqrSum));
                }
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构的模的平方。
        /// </summary>
        public double ModuleSquared
        {
            get
            {
                return (_X * _X + _Y * _Y + _Z * _Z + _U * _U + _V * _V + _W * _W);
            }
        }

        //

        /// <summary>
        /// 获取此 PointD6D 结构的相反向量。
        /// </summary>
        public PointD6D Opposite
        {
            get
            {
                return new PointD6D(-_X, -_Y, -_Z, -_U, -_V, -_W);
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构的规范化向量。
        /// </summary>
        public PointD6D Normalize
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
                    return new PointD6D(_X / Mod, _Y / Mod, _Z / Mod, _U / Mod, _V / Mod, _W / Mod);
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置此 PointD6D 结构在 XYZUV 空间的分量。
        /// </summary>
        public PointD5D XYZUV
        {
            get
            {
                return new PointD5D(_X, _Y, _Z, _U, _V);
            }

            set
            {
                _X = value.X;
                _Y = value.Y;
                _Z = value.Z;
                _U = value.U;
                _V = value.V;
            }
        }

        /// <summary>
        /// 获取或设置此 PointD6D 结构在 YZUVW 空间的分量。
        /// </summary>
        public PointD5D YZUVW
        {
            get
            {
                return new PointD5D(_Y, _Z, _U, _V, _W);
            }

            set
            {
                _Y = value.X;
                _Z = value.Y;
                _U = value.Z;
                _V = value.U;
                _W = value.V;
            }
        }

        /// <summary>
        /// 获取或设置此 PointD6D 结构在 ZUVWX 空间的分量。
        /// </summary>
        public PointD5D ZUVWX
        {
            get
            {
                return new PointD5D(_Z, _U, _V, _W, _X);
            }

            set
            {
                _Z = value.X;
                _U = value.Y;
                _V = value.Z;
                _W = value.U;
                _X = value.V;
            }
        }

        /// <summary>
        /// 获取或设置此 PointD6D 结构在 UVWXY 空间的分量。
        /// </summary>
        public PointD5D UVWXY
        {
            get
            {
                return new PointD5D(_U, _V, _W, _X, _Y);
            }

            set
            {
                _U = value.X;
                _V = value.Y;
                _W = value.Z;
                _X = value.U;
                _Y = value.V;
            }
        }

        /// <summary>
        /// 获取或设置此 PointD6D 结构在 VWXYZ 空间的分量。
        /// </summary>
        public PointD5D VWXYZ
        {
            get
            {
                return new PointD5D(_V, _W, _X, _Y, _Z);
            }

            set
            {
                _V = value.X;
                _W = value.Y;
                _X = value.Z;
                _Y = value.U;
                _Z = value.V;
            }
        }

        /// <summary>
        /// 获取或设置此 PointD6D 结构在 WXYZU 空间的分量。
        /// </summary>
        public PointD5D WXYZU
        {
            get
            {
                return new PointD5D(_W, _X, _Y, _Z, _U);
            }

            set
            {
                _W = value.X;
                _X = value.Y;
                _Y = value.Z;
                _Z = value.U;
                _U = value.V;
            }
        }

        //

        /// <summary>
        /// 获取此 PointD6D 结构与 X 轴之间的夹角（弧度）。
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
        /// 获取此 PointD6D 结构与 Y 轴之间的夹角（弧度）。
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
        /// 获取此 PointD6D 结构与 Z 轴之间的夹角（弧度）。
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
        /// 获取此 PointD6D 结构与 U 轴之间的夹角（弧度）。
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
        /// 获取此 PointD6D 结构与 V 轴之间的夹角（弧度）。
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
        /// 获取此 PointD6D 结构与 W 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleFromW
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }
                else
                {
                    return AngleFrom(_W >= 0 ? Ew : -Ew);
                }
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构与 XYZUV 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleFromXYZUV
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }
                else
                {
                    return (Constant.HalfPi - AngleFromW);
                }
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构与 YZUVW 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleFromYZUVW
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
        /// 获取此 PointD6D 结构与 ZUVWX 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleFromZUVWX
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
        /// 获取此 PointD6D 结构与 UVWXY 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleFromUVWXY
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
        /// 获取此 PointD6D 结构与 VWXYZ 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleFromVWXYZ
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

        /// <summary>
        /// 获取此 PointD6D 结构与 WXYZU 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleFromWXYZU
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

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 PointD6D 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>布尔值，表示此 PointD6D 结构是否与指定的对象相等。</returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj == null || !(obj is PointD6D))
            {
                return false;
            }
            else
            {
                return Equals((PointD6D)obj);
            }
        }

        /// <summary>
        /// 返回此 PointD6D 结构的哈希代码。
        /// </summary>
        /// <returns>32 位整数，表示此 PointD6D 结构的哈希代码。</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 PointD6D 结构转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 PointD6D 结构的字符串形式。</returns>
        public override string ToString()
        {
            return string.Concat("{X=", _X, ", Y=", _Y, ", Z=", _Z, ", U=", _U, ", V=", _V, ", W=", _W, "}");
        }

        //

        /// <summary>
        /// 判断此 PointD6D 结构是否与指定的 PointD6D 结构相等。
        /// </summary>
        /// <param name="pt">用于比较的 PointD6D 结构。</param>
        /// <returns>布尔值，表示此 PointD6D 结构是否与指定的 PointD6D 结构相等。</returns>
        public bool Equals(PointD6D pt)
        {
            return (_X.Equals(pt._X) && _Y.Equals(pt._Y) && _Z.Equals(pt._Z) && _U.Equals(pt._U) && _V.Equals(pt._V) && _W.Equals(pt._W));
        }

        //

        /// <summary>
        /// 将此 PointD6D 结构与指定的对象进行次序比较。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>32 位整数，表示将此 PointD6D 结构与指定的对象进行次序比较得到的结果。</returns>
        public int CompareTo(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return 0;
            }
            else if (obj == null || !(obj is PointD6D))
            {
                return 1;
            }
            else
            {
                return CompareTo((PointD6D)obj);
            }
        }

        /// <summary>
        /// 将此 PointD6D 结构与指定的 PointD6D 结构进行次序比较。
        /// </summary>
        /// <param name="pt">用于比较的 PointD6D 结构。</param>
        /// <returns>32 位整数，表示将此 PointD6D 结构与指定的 PointD6D 结构进行次序比较得到的结果。</returns>
        public int CompareTo(PointD6D pt)
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
        /// 遍历此 PointD6D 结构的所有分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int IndexOf(double item)
        {
            return Array.IndexOf(ToArray(), item, 0, _Dimension);
        }

        /// <summary>
        /// 从指定的索引开始遍历此 PointD6D 结构的所有分量并返回第一个与指定值相等的分量的索引。
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
        /// 从指定的索引开始遍历此 PointD6D 结构指定数量的分量并返回第一个与指定值相等的分量的索引。
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
        /// 逆序遍历此 PointD6D 结构的所有分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int LastIndexOf(double item)
        {
            return Array.LastIndexOf(ToArray(), item, _Dimension - 1, _Dimension);
        }

        /// <summary>
        /// 从指定的索引开始逆序遍历此 PointD6D 结构的所有分量并返回第一个与指定值相等的分量的索引。
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
        /// 从指定的索引开始逆序遍历此 PointD6D 结构指定数量的分量并返回第一个与指定值相等的分量的索引。
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
        /// 遍历此 PointD6D 结构的所有分量并返回表示是否存在与指定值相等的分量的布尔值。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>布尔值，表示是否存在与指定值相等的分量。</returns>
        public bool Contains(double item)
        {
            if (_X.Equals(item) || _Y.Equals(item) || _Z.Equals(item) || _U.Equals(item) || _V.Equals(item) || _W.Equals(item))
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
        /// 将此 PointD6D 结构转换为双精度浮点数数组。
        /// </summary>
        /// <returns>双精度浮点数数组，数组元素表示此 PointD6D 结构的分量。</returns>
        public double[] ToArray()
        {
            return new double[_Dimension] { _X, _Y, _Z, _U, _V, _W };
        }

        /// <summary>
        /// 将此 PointD6D 结构转换为双精度浮点数列表。
        /// </summary>
        /// <returns>双精度浮点数列表，列表元素表示此 PointD6D 结构的分量。</returns>
        public List<double> ToList()
        {
            return new List<double>(_Dimension) { _X, _Y, _Z, _U, _V, _W };
        }

        //

        /// <summary>
        /// 返回将此 PointD6D 结构表示的直角坐标系坐标转换为超球坐标系坐标的 PointD6D 结构的新实例。
        /// </summary>
        /// <returns>PointD6D 结构，表示将此 PointD6D 结构表示的直角坐标系坐标转换为超球坐标系坐标得到的结果。</returns>
        public PointD6D ToSpherical()
        {
            Vector result = ToColumnVector().ToSpherical();

            if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }
            else
            {
                return new PointD6D(result[0], result[1], result[2], result[3], result[4], result[5]);
            }
        }

        /// <summary>
        /// 返回将此 PointD6D 结构表示的超球坐标系坐标转换为直角坐标系坐标的 PointD6D 结构的新实例。
        /// </summary>
        /// <returns>PointD6D 结构，表示将此 PointD6D 结构表示的超球坐标系坐标转换为直角坐标系坐标得到的结果。</returns>
        public PointD6D ToCartesian()
        {
            Vector result = ToColumnVector().ToCartesian();

            if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }
            else
            {
                return new PointD6D(result[0], result[1], result[2], result[3], result[4], result[5]);
            }
        }

        //

        /// <summary>
        /// 返回此 PointD6D 结构与指定的 PointD6D 结构之间的距离。
        /// </summary>
        /// <param name="pt">PointD6D 结构，表示另一个向量。</param>
        /// <returns>双精度浮点数，表示此 PointD6D 结构与指定的 PointD6D 结构之间的距离。</returns>
        public double DistanceFrom(PointD6D pt)
        {
            double AbsDx = Math.Abs(_X - pt._X);
            double AbsDy = Math.Abs(_Y - pt._Y);
            double AbsDz = Math.Abs(_Z - pt._Z);
            double AbsDu = Math.Abs(_U - pt._U);
            double AbsDv = Math.Abs(_V - pt._V);
            double AbsDw = Math.Abs(_W - pt._W);

            double AbsMax = Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(AbsDx, AbsDy), AbsDz), AbsDu), AbsDv), AbsDw);

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
                AbsDw /= AbsMax;

                double SqrSum = AbsDx * AbsDx + AbsDy * AbsDy + AbsDz * AbsDz + AbsDu * AbsDu + AbsDv * AbsDv + AbsDw * AbsDw;

                return (AbsMax * Math.Sqrt(SqrSum));
            }
        }

        /// <summary>
        /// 返回此 PointD6D 结构与指定的 PointD6D 结构之间的夹角（弧度）。
        /// </summary>
        /// <param name="pt">PointD6D 结构，表示另一个向量。</param>
        /// <returns>双精度浮点数，表示此 PointD6D 结构与指定的 PointD6D 结构之间的夹角（弧度）。</returns>
        public double AngleFrom(PointD6D pt)
        {
            if (IsZero || pt.IsZero)
            {
                return 0;
            }
            else
            {
                double ModProduct = Module * pt.Module;
                double CosA = _X * pt._X / ModProduct + _Y * pt._Y / ModProduct + _Z * pt._Z / ModProduct + _U * pt._U / ModProduct + _V * pt._V / ModProduct + _W * pt._W / ModProduct;

                return Math.Acos(CosA);
            }
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的位移将此 PointD6D 结构的所有分量平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public void Offset(double d)
        {
            _X += d;
            _Y += d;
            _Z += d;
            _U += d;
            _V += d;
            _W += d;
        }

        /// <summary>
        /// 按双精度浮点数表示的 X 坐标位移、Y 坐标位移、Z 坐标位移、U 坐标位移、V 坐标位移与 W 坐标位移将此 PointD6D 结构平移指定的量。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标位移。</param>
        /// <param name="du">双精度浮点数表示的 U 坐标位移。</param>
        /// <param name="dv">双精度浮点数表示的 V 坐标位移。</param>
        /// <param name="dw">双精度浮点数表示的 W 坐标位移。</param>
        public void Offset(double dx, double dy, double dz, double du, double dv, double dw)
        {
            _X += dx;
            _Y += dy;
            _Z += dz;
            _U += du;
            _V += dv;
            _W += dw;
        }

        /// <summary>
        /// 按 PointD6D 结构表示的位移将此 PointD6D 结构平移指定的量。
        /// </summary>
        /// <param name="pt">PointD6D 结构表示的位移。</param>
        public void Offset(PointD6D pt)
        {
            _X += pt._X;
            _Y += pt._Y;
            _Z += pt._Z;
            _U += pt._U;
            _V += pt._V;
            _W += pt._W;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的位移将此 PointD6D 结构的所有分量平移指定的量的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>PointD6D 结构，表示按双精度浮点数表示的位移将此 PointD6D 结构的所有分量平移指定的量得到的结果。</returns>
        public PointD6D OffsetCopy(double d)
        {
            return new PointD6D(_X + d, _Y + d, _Z + d, _U + d, _V + d, _W + d);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标位移、Y 坐标位移、Z 坐标位移、U 坐标位移、V 坐标位移与 W 坐标位移将此 PointD6D 结构平移指定的量的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标位移。</param>
        /// <param name="du">双精度浮点数表示的 U 坐标位移。</param>
        /// <param name="dv">双精度浮点数表示的 V 坐标位移。</param>
        /// <param name="dw">双精度浮点数表示的 W 坐标位移。</param>
        /// <returns>PointD6D 结构，表示按双精度浮点数表示的 X 坐标位移、Y 坐标位移、Z 坐标位移、U 坐标位移、V 坐标位移与 W 坐标位移将此 PointD6D 结构平移指定的量得到的结果。</returns>
        public PointD6D OffsetCopy(double dx, double dy, double dz, double du, double dv, double dw)
        {
            return new PointD6D(_X + dx, _Y + dy, _Z + dz, _U + du, _V + dv, _W + dw);
        }

        /// <summary>
        /// 返回按 PointD6D 结构表示的位移将此 PointD6D 结构平移指定的量的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构表示的位移。</param>
        /// <returns>PointD6D 结构，表示按 PointD6D 结构表示的位移将此 PointD6D 结构平移指定的量得到的结果。</returns>
        public PointD6D OffsetCopy(PointD6D pt)
        {
            return new PointD6D(_X + pt._X, _Y + pt._Y, _Z + pt._Z, _U + pt._U, _V + pt._V, _W + pt._W);
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的缩放因数将此 PointD6D 结构的所有分量缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public void Scale(double s)
        {
            _X *= s;
            _Y *= s;
            _Z *= s;
            _U *= s;
            _V *= s;
            _W *= s;
        }

        /// <summary>
        /// 按双精度浮点数表示的 X 坐标缩放因数、Y 坐标缩放因数、Z 坐标缩放因数、U 坐标缩放因数、V 坐标缩放因数与 W 坐标缩放因数将此 PointD6D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因数。</param>
        /// <param name="su">双精度浮点数表示的 U 坐标缩放因数。</param>
        /// <param name="sv">双精度浮点数表示的 V 坐标缩放因数。</param>
        /// <param name="sw">双精度浮点数表示的 W 坐标缩放因数。</param>
        public void Scale(double sx, double sy, double sz, double su, double sv, double sw)
        {
            _X *= sx;
            _Y *= sy;
            _Z *= sz;
            _U *= su;
            _V *= sv;
            _W *= sw;
        }

        /// <summary>
        /// 按 PointD6D 结构表示的缩放因数将此 PointD6D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="pt">PointD6D 结构表示的缩放因数。</param>
        public void Scale(PointD6D pt)
        {
            _X *= pt._X;
            _Y *= pt._Y;
            _Z *= pt._Z;
            _U *= pt._U;
            _V *= pt._V;
            _W *= pt._W;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数将此 PointD6D 结构的所有分量缩放指定的倍数的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>PointD6D 结构，表示按双精度浮点数表示的缩放因数将此 PointD6D 结构的所有分量缩放指定的倍数得到的结果。</returns>
        public PointD6D ScaleCopy(double s)
        {
            return new PointD6D(_X * s, _Y * s, _Z * s, _U * s, _V * s, _W * s);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标缩放因数、Y 坐标缩放因数、Z 坐标缩放因数、U 坐标缩放因数、V 坐标缩放因数与 W 坐标缩放因数将此 PointD6D 结构缩放指定的倍数的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因数。</param>
        /// <param name="su">双精度浮点数表示的 U 坐标缩放因数。</param>
        /// <param name="sv">双精度浮点数表示的 V 坐标缩放因数。</param>
        /// <param name="sw">双精度浮点数表示的 W 坐标缩放因数。</param>
        /// <returns>PointD6D 结构，表示按双精度浮点数表示的 X 坐标缩放因数、Y 坐标缩放因数、Z 坐标缩放因数、U 坐标缩放因数、V 坐标缩放因数与 W 坐标缩放因数将此 PointD6D 结构缩放指定的倍数得到的结果。</returns>
        public PointD6D ScaleCopy(double sx, double sy, double sz, double su, double sv, double sw)
        {
            return new PointD6D(_X * sx, _Y * sy, _Z * sz, _U * su, _V * sv, _W * sw);
        }

        /// <summary>
        /// 返回按 PointD6D 结构表示的缩放因数将此 PointD6D 结构缩放指定的倍数的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构表示的缩放因数。</param>
        /// <returns>PointD6D 结构，表示按 PointD6D 结构表示的缩放因数将此 PointD6D 结构缩放指定的倍数得到的结果。</returns>
        public PointD6D ScaleCopy(PointD6D pt)
        {
            return new PointD6D(_X * pt._X, _Y * pt._Y, _Z * pt._Z, _U * pt._U, _V * pt._V, _W * pt._W);
        }

        //

        /// <summary>
        /// 将此 PointD6D 结构在指定的基向量方向的分量翻转。
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
                case 5: _W = -_W; break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 返回将此 PointD6D 结构在指定的基向量方向的分量翻转的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        /// <returns>PointD6D 结构，表示将此 PointD6D 结构在指定的基向量方向的分量翻转得到的结果。</returns>
        public PointD6D ReflectCopy(int index)
        {
            switch (index)
            {
                case 0: return new PointD6D(-_X, _Y, _Z, _U, _V, _W);
                case 1: return new PointD6D(_X, -_Y, _Z, _U, _V, _W);
                case 2: return new PointD6D(_X, _Y, -_Z, _U, _V, _W);
                case 3: return new PointD6D(_X, _Y, _Z, -_U, _V, _W);
                case 4: return new PointD6D(_X, _Y, _Z, _U, -_V, _W);
                case 5: return new PointD6D(_X, _Y, _Z, _U, _V, -_W);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD6D 结构剪切指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向剪切的角度（弧度）。</param>
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
                _W = result[5];
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD6D 结构剪切指定的角度的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向剪切的角度（弧度）。</param>
        /// <returns>PointD6D 结构，表示按双精度浮点数表示的弧度将此 PointD6D 结构剪切指定的角度得到的结果。</returns>
        public PointD6D ShearCopy(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().ShearCopy(index1, index2, angle);

            if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }
            else
            {
                return new PointD6D(result[0], result[1], result[2], result[3], result[4], result[5]);
            }
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD6D 结构旋转指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
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
                _W = result[5];
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD6D 结构旋转指定的角度的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        /// <returns>PointD6D 结构，表示按双精度浮点数表示的弧度将此 PointD6D 结构旋转指定的角度得到的结果。</returns>
        public PointD6D RotateCopy(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().RotateCopy(index1, index2, angle);

            if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }
            else
            {
                return new PointD6D(result[0], result[1], result[2], result[3], result[4], result[5]);
            }
        }

        //

        /// <summary>
        /// 按 PointD6D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量、V 基向量、W 基向量与偏移向量将此 PointD6D 结构进行仿射变换。
        /// </summary>
        /// <param name="ex">PointD6D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD6D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD6D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD6D 结构表示的 U 基向量。</param>
        /// <param name="ev">PointD6D 结构表示的 V 基向量。</param>
        /// <param name="ew">PointD6D 结构表示的 W 基向量。</param>
        /// <param name="offset">PointD6D 结构表示的偏移向量。</param>
        public void AffineTransform(PointD6D ex, PointD6D ey, PointD6D ez, PointD6D eu, PointD6D ev, PointD6D ew, PointD6D offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[_Dimension + 1, _Dimension + 1]
            {
                { ex._X, ex._Y, ex._Z, ex._U, ex._V, ex._W, 0 },
                { ey._X, ey._Y, ey._Z, ey._U, ey._V, ey._W, 0 },
                { ez._X, ez._Y, ez._Z, ez._U, ez._V, ez._W, 0 },
                { eu._X, eu._Y, eu._Z, eu._U, eu._V, eu._W, 0 },
                { ev._X, ev._Y, ev._Z, ev._U, ev._V, ev._W, 0 },
                { ew._X, ew._Y, ew._Z, ew._U, ew._V, ew._W, 0 },
                { offset._X, offset._Y, offset._Z, offset._U, offset._V, offset._W, 1 }
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
                _W = result[5];
            }
        }

        /// <summary>
        /// 按 Matrix 对象表示的 7x7 仿射矩阵（左矩阵）将此 PointD6D 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 7x7 仿射矩阵（左矩阵）。</param>
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
                    _W = result[5];
                }
            }
        }

        /// <summary>
        /// 按 Matrix 对象列表表示的 7x7 仿射矩阵（左矩阵）列表将此 PointD6D 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 7x7 仿射矩阵（左矩阵）列表。</param>
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
                    _W = result[5];
                }
            }
        }

        /// <summary>
        /// 返回按 PointD6D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量、V 基向量、W 基向量与偏移向量将此 PointD6D 结构进行仿射变换的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="ex">PointD6D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD6D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD6D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD6D 结构表示的 U 基向量。</param>
        /// <param name="ev">PointD6D 结构表示的 V 基向量。</param>
        /// <param name="ew">PointD6D 结构表示的 W 基向量。</param>
        /// <param name="offset">PointD6D 结构表示的偏移向量。</param>
        /// <returns>PointD6D 结构，表示按 PointD6D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量、V 基向量、W 基向量与偏移向量将此 PointD6D 结构进行仿射变换得到的结果。</returns>
        public PointD6D AffineTransformCopy(PointD6D ex, PointD6D ey, PointD6D ez, PointD6D eu, PointD6D ev, PointD6D ew, PointD6D offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[_Dimension + 1, _Dimension + 1]
            {
                { ex._X, ex._Y, ex._Z, ex._U, ex._V, ex._W, 0 },
                { ey._X, ey._Y, ey._Z, ey._U, ey._V, ey._W, 0 },
                { ez._X, ez._Y, ez._Z, ez._U, ez._V, ez._W, 0 },
                { eu._X, eu._Y, eu._Z, eu._U, eu._V, eu._W, 0 },
                { ev._X, ev._Y, ev._Z, ev._U, ev._V, ev._W, 0 },
                { ew._X, ew._Y, ew._Z, ew._U, ew._V, ew._W, 0 },
                { offset._X, offset._Y, offset._Z, offset._U, offset._V, offset._W, 1 }
            });

            Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

            if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }
            else
            {
                return new PointD6D(result[0], result[1], result[2], result[3], result[4], result[5]);
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的 7x7 仿射矩阵（左矩阵）将此 PointD6D 结构进行仿射变换的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 7x7 仿射矩阵（左矩阵）。</param>
        /// <returns>PointD6D 结构，表示按 Matrix 对象表示的 7x7 仿射矩阵（左矩阵）将此 PointD6D 结构进行仿射变换得到的结果。</returns>
        public PointD6D AffineTransformCopy(Matrix matrixLeft)
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
                    return new PointD6D(result[0], result[1], result[2], result[3], result[4], result[5]);
                }
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 7x7 仿射矩阵（左矩阵）列表将此 PointD6D 结构进行仿射变换的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 7x7 仿射矩阵（左矩阵）列表。</param>
        /// <returns>PointD6D 结构，表示按 Matrix 对象列表表示的 7x7 仿射矩阵（左矩阵）列表将此 PointD6D 结构进行仿射变换得到的结果。</returns>
        public PointD6D AffineTransformCopy(List<Matrix> matrixLeftList)
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
                    return new PointD6D(result[0], result[1], result[2], result[3], result[4], result[5]);
                }
            }
        }

        /// <summary>
        /// 按 PointD6D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量、V 基向量、W 基向量与偏移向量将此 PointD6D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="ex">PointD6D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD6D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD6D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD6D 结构表示的 U 基向量。</param>
        /// <param name="ev">PointD6D 结构表示的 V 基向量。</param>
        /// <param name="ew">PointD6D 结构表示的 W 基向量。</param>
        /// <param name="offset">PointD6D 结构表示的偏移向量。</param>
        public void InverseAffineTransform(PointD6D ex, PointD6D ey, PointD6D ez, PointD6D eu, PointD6D ev, PointD6D ew, PointD6D offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[_Dimension + 1, _Dimension + 1]
            {
                { ex._X, ex._Y, ex._Z, ex._U, ex._V, ex._W, 0 },
                { ey._X, ey._Y, ey._Z, ey._U, ey._V, ey._W, 0 },
                { ez._X, ez._Y, ez._Z, ez._U, ez._V, ez._W, 0 },
                { eu._X, eu._Y, eu._Z, eu._U, eu._V, eu._W, 0 },
                { ev._X, ev._Y, ev._Z, ev._U, ev._V, ev._W, 0 },
                { ew._X, ew._Y, ew._Z, ew._U, ew._V, ew._W, 0 },
                { offset._X, offset._Y, offset._Z, offset._U, offset._V, offset._W, 1 }
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
                _W = result[5];
            }
        }

        /// <summary>
        /// 按 Matrix 对象表示的 7x7 仿射矩阵（左矩阵）将此 PointD6D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 7x7 仿射矩阵（左矩阵）。</param>
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
                    _W = result[5];
                }
            }
        }

        /// <summary>
        /// 按 Matrix 对象列表表示的 7x7 仿射矩阵（左矩阵）列表将此 PointD6D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 7x7 仿射矩阵（左矩阵）列表。</param>
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
                    _W = result[5];
                }
            }
        }

        /// <summary>
        /// 返回按 PointD6D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量、V 基向量、W 基向量与偏移向量将此 PointD6D 结构进行逆仿射变换的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="ex">PointD6D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD6D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD6D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD6D 结构表示的 U 基向量。</param>
        /// <param name="ev">PointD6D 结构表示的 V 基向量。</param>
        /// <param name="ew">PointD6D 结构表示的 W 基向量。</param>
        /// <param name="offset">PointD6D 结构表示的偏移向量。</param>
        /// <returns>PointD6D 结构，表示按 PointD6D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量、V 基向量、W 基向量与偏移向量将此 PointD6D 结构进行逆仿射变换得到的结果。</returns>
        public PointD6D InverseAffineTransformCopy(PointD6D ex, PointD6D ey, PointD6D ez, PointD6D eu, PointD6D ev, PointD6D ew, PointD6D offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[_Dimension + 1, _Dimension + 1]
            {
                { ex._X, ex._Y, ex._Z, ex._U, ex._V, ex._W, 0 },
                { ey._X, ey._Y, ey._Z, ey._U, ey._V, ey._W, 0 },
                { ez._X, ez._Y, ez._Z, ez._U, ez._V, ez._W, 0 },
                { eu._X, eu._Y, eu._Z, eu._U, eu._V, eu._W, 0 },
                { ev._X, ev._Y, ev._Z, ev._U, ev._V, ev._W, 0 },
                { ew._X, ew._Y, ew._Z, ew._U, ew._V, ew._W, 0 },
                { offset._X, offset._Y, offset._Z, offset._U, offset._V, offset._W, 1 }
            });

            Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

            if (Vector.IsNullOrEmpty(result) || result.Dimension != _Dimension)
            {
                throw new ArithmeticException();
            }
            else
            {
                return new PointD6D(result[0], result[1], result[2], result[3], result[4], result[5]);
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的 7x7 仿射矩阵（左矩阵）将此 PointD6D 结构进行逆仿射变换的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 7x7 仿射矩阵（左矩阵）。</param>
        /// <returns>PointD6D 结构，表示按 Matrix 对象表示的 7x7 仿射矩阵（左矩阵）将此 PointD6D 结构进行逆仿射变换得到的结果。</returns>
        public PointD6D InverseAffineTransformCopy(Matrix matrixLeft)
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
                    return new PointD6D(result[0], result[1], result[2], result[3], result[4], result[5]);
                }
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 7x7 仿射矩阵（左矩阵）列表将此 PointD6D 结构进行逆仿射变换的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 7x7 仿射矩阵（左矩阵）列表。</param>
        /// <returns>PointD6D 结构，表示按 Matrix 对象列表表示的 7x7 仿射矩阵（左矩阵）列表将此 PointD6D 结构进行逆仿射变换得到的结果。</returns>
        public PointD6D InverseAffineTransformCopy(List<Matrix> matrixLeftList)
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
                    return new PointD6D(result[0], result[1], result[2], result[3], result[4], result[5]);
                }
            }
        }

        //

        /// <summary>
        /// 返回将此 PointD6D 结构投影至平行于 XYZUV 空间的投影空间的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD6D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        /// <returns>PointD5D 结构，表示将此 PointD6D 结构投影至平行于 XYZUV 空间的投影空间得到的结果。</returns>
        public PointD5D ProjectToXYZUV(PointD6D prjCenter, double trueLenDist)
        {
            if (trueLenDist == 0)
            {
                return XYZUV;
            }
            else
            {
                if (_W == prjCenter._W)
                {
                    return PointD5D.NaN;
                }
                else
                {
                    double Scale = trueLenDist / (_W - prjCenter._W);

                    if (InternalMethod.IsNaNOrInfinity(Scale) || Scale <= 0)
                    {
                        return PointD5D.NaN;
                    }
                    else
                    {
                        return (Scale * XYZUV + (1 - Scale) * prjCenter.XYZUV);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 PointD6D 结构投影至平行于 YZUVW 空间的投影空间的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD6D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        /// <returns>PointD5D 结构，表示将此 PointD6D 结构投影至平行于 YZUVW 空间的投影空间得到的结果。</returns>
        public PointD5D ProjectToYZUVW(PointD6D prjCenter, double trueLenDist)
        {
            if (trueLenDist == 0)
            {
                return YZUVW;
            }
            else
            {
                if (_X == prjCenter._X)
                {
                    return PointD5D.NaN;
                }
                else
                {
                    double Scale = trueLenDist / (_X - prjCenter._X);

                    if (InternalMethod.IsNaNOrInfinity(Scale) || Scale <= 0)
                    {
                        return PointD5D.NaN;
                    }
                    else
                    {
                        return (Scale * YZUVW + (1 - Scale) * prjCenter.YZUVW);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 PointD6D 结构投影至平行于 ZUVWX 空间的投影空间的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD6D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        /// <returns>PointD5D 结构，表示将此 PointD6D 结构投影至平行于 ZUVWX 空间的投影空间得到的结果。</returns>
        public PointD5D ProjectToZUVWX(PointD6D prjCenter, double trueLenDist)
        {
            if (trueLenDist == 0)
            {
                return ZUVWX;
            }
            else
            {
                if (_Y == prjCenter._Y)
                {
                    return PointD5D.NaN;
                }
                else
                {
                    double Scale = trueLenDist / (_Y - prjCenter._Y);

                    if (InternalMethod.IsNaNOrInfinity(Scale) || Scale <= 0)
                    {
                        return PointD5D.NaN;
                    }
                    else
                    {
                        return (Scale * ZUVWX + (1 - Scale) * prjCenter.ZUVWX);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 PointD6D 结构投影至平行于 UVWXY 空间的投影空间的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD6D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        /// <returns>PointD5D 结构，表示将此 PointD6D 结构投影至平行于 UVWXY 空间的投影空间得到的结果。</returns>
        public PointD5D ProjectToUVWXY(PointD6D prjCenter, double trueLenDist)
        {
            if (trueLenDist == 0)
            {
                return UVWXY;
            }
            else
            {
                if (_Z == prjCenter._Z)
                {
                    return PointD5D.NaN;
                }
                else
                {
                    double Scale = trueLenDist / (_Z - prjCenter._Z);

                    if (InternalMethod.IsNaNOrInfinity(Scale) || Scale <= 0)
                    {
                        return PointD5D.NaN;
                    }
                    else
                    {
                        return (Scale * UVWXY + (1 - Scale) * prjCenter.UVWXY);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 PointD6D 结构投影至平行于 VWXYZ 空间的投影空间的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD6D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        /// <returns>PointD5D 结构，表示将此 PointD6D 结构投影至平行于 VWXYZ 空间的投影空间得到的结果。</returns>
        public PointD5D ProjectToVWXYZ(PointD6D prjCenter, double trueLenDist)
        {
            if (trueLenDist == 0)
            {
                return VWXYZ;
            }
            else
            {
                if (_U == prjCenter._U)
                {
                    return PointD5D.NaN;
                }
                else
                {
                    double Scale = trueLenDist / (_U - prjCenter._U);

                    if (InternalMethod.IsNaNOrInfinity(Scale) || Scale <= 0)
                    {
                        return PointD5D.NaN;
                    }
                    else
                    {
                        return (Scale * VWXYZ + (1 - Scale) * prjCenter.VWXYZ);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将此 PointD6D 结构投影至平行于 WXYZU 空间的投影空间的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD6D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        /// <returns>PointD5D 结构，表示将此 PointD6D 结构投影至平行于 WXYZU 空间的投影空间得到的结果。</returns>
        public PointD5D ProjectToWXYZU(PointD6D prjCenter, double trueLenDist)
        {
            if (trueLenDist == 0)
            {
                return WXYZU;
            }
            else
            {
                if (_V == prjCenter._V)
                {
                    return PointD5D.NaN;
                }
                else
                {
                    double Scale = trueLenDist / (_V - prjCenter._V);

                    if (InternalMethod.IsNaNOrInfinity(Scale) || Scale <= 0)
                    {
                        return PointD5D.NaN;
                    }
                    else
                    {
                        return (Scale * WXYZU + (1 - Scale) * prjCenter.WXYZU);
                    }
                }
            }
        }

        //

        /// <summary>
        /// 返回将此 PointD6D 结构转换为列向量的 Vector 的新实例。
        /// </summary>
        /// <returns>Vector 对象，表示将此 PointD6D 结构转换为列向量得到的结果。</returns>
        public Vector ToColumnVector()
        {
            return Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, _X, _Y, _Z, _U, _V, _W);
        }

        /// <summary>
        /// 返回将此 PointD6D 结构转换为行向量的 Vector 的新实例。
        /// </summary>
        /// <returns>Vector 对象，表示将此 PointD6D 结构转换为行向量得到的结果。</returns>
        public Vector ToRowVector()
        {
            return Vector.UnsafeCreateInstance(Vector.Type.RowVector, _X, _Y, _Z, _U, _V, _W);
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断两个 PointD6D 结构是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 PointD6D 结构。</param>
        /// <param name="right">用于比较的第二个 PointD6D 结构。</param>
        /// <returns>布尔值，表示两个 PointD6D 结构是否相等。</returns>
        public static bool Equals(PointD6D left, PointD6D right)
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
        /// 比较两个 PointD6D 结构的次序。
        /// </summary>
        /// <param name="left">用于比较的第一个 PointD6D 结构。</param>
        /// <param name="right">用于比较的第二个 PointD6D 结构。</param>
        /// <returns>32 位整数，表示将两个 PointD6D 结构进行次序比较得到的结果。</returns>
        public static int Compare(PointD6D left, PointD6D right)
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
        /// 返回单位矩阵，表示不对 PointD6D 结构进行仿射变换的 7x7 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <returns>Matrix 对象，表示不对 PointD6D 结构进行仿射变换的 7x7 仿射矩阵（左矩阵）。</returns>
        public static Matrix IdentityMatrix()
        {
            return Matrix.Identity(_Dimension + 1);
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的位移将 PointD6D 结构的所有分量平移指定的量的 7x7 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的位移将 PointD6D 结构的所有分量平移指定的量的 7x7 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(double d)
        {
            return Vector.OffsetMatrix(Vector.Type.ColumnVector, _Dimension, d);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的 X 坐标位移、Y 坐标位移、Z 坐标位移、U 坐标位移、V 坐标位移与 W 坐标位移将 PointD6D 结构平移指定的量的 7x7 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标位移。</param>
        /// <param name="du">双精度浮点数表示的 U 坐标位移。</param>
        /// <param name="dv">双精度浮点数表示的 V 坐标位移。</param>
        /// <param name="dw">双精度浮点数表示的 W 坐标位移。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的 X 坐标位移、Y 坐标位移、Z 坐标位移、U 坐标位移、V 坐标位移与 W 坐标位移将 PointD6D 结构平移指定的量的 7x7 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(double dx, double dy, double dz, double du, double dv, double dw)
        {
            return Vector.OffsetMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, dx, dy, dz, du, dv, dw));
        }

        /// <summary>
        /// 返回表示按 PointD6D 结构表示的位移将 PointD6D 结构平移指定的量的 7x7 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构表示的位移。</param>
        /// <returns>Matrix 对象，表示按 PointD6D 结构表示的位移将 PointD6D 结构平移指定的量的 7x7 仿射矩阵（左矩阵）。</returns>
        public static Matrix OffsetMatrix(PointD6D pt)
        {
            return Vector.OffsetMatrix(pt.ToColumnVector());
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的缩放因数将 PointD6D 结构的所有分量缩放指定的倍数的 7x7 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的缩放因数将 PointD6D 结构的所有分量缩放指定的倍数的 7x7 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(double s)
        {
            return Vector.ScaleMatrix(Vector.Type.ColumnVector, _Dimension, s);
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的 X 坐标缩放因数、Y 坐标缩放因数、Z 坐标缩放因数、U 坐标缩放因数、V 坐标缩放因数与 W 坐标缩放因数将 PointD6D 结构缩放指定的倍数的 7x7 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因数。</param>
        /// <param name="su">双精度浮点数表示的 U 坐标缩放因数。</param>
        /// <param name="sv">双精度浮点数表示的 V 坐标缩放因数。</param>
        /// <param name="sw">双精度浮点数表示的 W 坐标缩放因数。</param>
        /// <returns>Matrix 对象，表示按双精度浮点数表示的 X 坐标缩放因数、Y 坐标缩放因数、Z 坐标缩放因数、U 坐标缩放因数、V 坐标缩放因数与 W 坐标缩放因数将 PointD6D 结构缩放指定的倍数的 7x7 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(double sx, double sy, double sz, double su, double sv, double sw)
        {
            return Vector.ScaleMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, sx, sy, sz, su, sv, sw));
        }

        /// <summary>
        /// 返回表示按 PointD6D 结构表示的缩放因数将 PointD6D 结构缩放指定的倍数的 7x7 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示按 PointD6D 结构表示的缩放因数将 PointD6D 结构缩放指定的倍数的 7x7 仿射矩阵（左矩阵）。</returns>
        public static Matrix ScaleMatrix(PointD6D pt)
        {
            return Vector.ScaleMatrix(pt.ToColumnVector());
        }

        //

        /// <summary>
        /// 返回表示用于翻转 PointD6D 结构的 7x7 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        /// <returns>Matrix 对象，表示用于翻转 PointD6D 结构的 7x7 仿射矩阵（左矩阵）。</returns>
        public static Matrix ReflectMatrix(int index)
        {
            return Vector.ReflectMatrix(Vector.Type.ColumnVector, _Dimension, index);
        }

        //

        /// <summary>
        /// 返回表示用于剪切 PointD6D 结构的 7x7 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向剪切的角度（弧度）。</param>
        /// <returns>Matrix 对象，表示用于剪切 PointD6D 结构的 7x7 仿射矩阵（左矩阵）。</returns>
        public static Matrix ShearMatrix(int index1, int index2, double angle)
        {
            return Vector.ShearMatrix(Vector.Type.ColumnVector, _Dimension, index1, index2, angle);
        }

        //

        /// <summary>
        /// 返回表示用于旋转 PointD6D 结构的 7x7 仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示 PointD6D 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        /// <returns>Matrix 对象，表示用于旋转 PointD6D 结构的 7x7 仿射矩阵（左矩阵）。</returns>
        public static Matrix RotateMatrix(int index1, int index2, double angle)
        {
            return Vector.RotateMatrix(Vector.Type.ColumnVector, _Dimension, index1, index2, angle);
        }

        //

        /// <summary>
        /// 返回两个 PointD6D 结构之间的距离。
        /// </summary>
        /// <param name="left">第一个 PointD6D 结构。</param>
        /// <param name="right">第二个 PointD6D 结构。</param>
        /// <returns>双精度浮点数，表示两个 PointD6D 结构之间的距离。</returns>
        public static double DistanceBetween(PointD6D left, PointD6D right)
        {
            double AbsDx = Math.Abs(left._X - right._X);
            double AbsDy = Math.Abs(left._Y - right._Y);
            double AbsDz = Math.Abs(left._Z - right._Z);
            double AbsDu = Math.Abs(left._U - right._U);
            double AbsDv = Math.Abs(left._V - right._V);
            double AbsDw = Math.Abs(left._W - right._W);

            double AbsMax = Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(AbsDx, AbsDy), AbsDz), AbsDu), AbsDv), AbsDw);

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
                AbsDw /= AbsMax;

                double SqrSum = AbsDx * AbsDx + AbsDy * AbsDy + AbsDz * AbsDz + AbsDu * AbsDu + AbsDv * AbsDv + AbsDw * AbsDw;

                return (AbsMax * Math.Sqrt(SqrSum));
            }
        }

        /// <summary>
        /// 返回两个 PointD6D 结构之间的夹角（弧度）。
        /// </summary>
        /// <param name="left">第一个 PointD6D 结构。</param>
        /// <param name="right">第二个 PointD6D 结构。</param>
        /// <returns>双精度浮点数，表示两个 PointD6D 结构之间的夹角（弧度）。</returns>
        public static double AngleBetween(PointD6D left, PointD6D right)
        {
            if (left.IsZero || right.IsZero)
            {
                return 0;
            }
            else
            {
                double ModProduct = left.Module * right.Module;
                double CosA = left._X * right._X / ModProduct + left._Y * right._Y / ModProduct + left._Z * right._Z / ModProduct + left._U * right._U / ModProduct + left._V * right._V / ModProduct + left._W * right._W / ModProduct;

                return Math.Acos(CosA);
            }
        }

        //

        /// <summary>
        /// 返回两个 PointD6D 结构的数量积。
        /// </summary>
        /// <param name="left">第一个 PointD6D 结构。</param>
        /// <param name="right">第二个 PointD6D 结构。</param>
        /// <returns>双精度浮点数，表示两个 PointD6D 结构的数量积。</returns>
        public static double DotProduct(PointD6D left, PointD6D right)
        {
            return Vector.DotProduct(left.ToColumnVector(), right.ToColumnVector());
        }

        /// <summary>
        /// 返回两个 PointD6D 结构的向量积。该向量积为一个十五维向量，其所有分量的数值依次为 X∧Y 基向量、X∧Z 基向量、X∧U 基向量、X∧V 基向量、X∧W 基向量、Y∧Z 基向量、Y∧U 基向量、Y∧V 基向量、Y∧W 基向量、Z∧U 基向量、Z∧V 基向量、Z∧W 基向量、U∧V 基向量、U∧W 基向量与 V∧W 基向量的系数。
        /// </summary>
        /// <param name="left">第一个 PointD6D 结构。</param>
        /// <param name="right">第二个 PointD6D 结构。</param>
        /// <returns>Vector 对象，表示两个 PointD6D 结构的向量积。</returns>
        public static Vector CrossProduct(PointD6D left, PointD6D right)
        {
            return Vector.CrossProduct(left.ToColumnVector(), right.ToColumnVector());
        }

        //

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量取符号数得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于转换的结构。</param>
        /// <returns>PointD6D 结构，表示将 PointD6D 结构的所有分量取符号数得到的结果</returns>
        public static PointD6D Sign(PointD6D pt)
        {
            return new PointD6D((double.IsNaN(pt._X) ? 0 : Math.Sign(pt._X)), (double.IsNaN(pt._Y) ? 0 : Math.Sign(pt._Y)), (double.IsNaN(pt._Z) ? 0 : Math.Sign(pt._Z)), (double.IsNaN(pt._U) ? 0 : Math.Sign(pt._U)), (double.IsNaN(pt._V) ? 0 : Math.Sign(pt._V)), (double.IsNaN(pt._W) ? 0 : Math.Sign(pt._W)));
        }

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量取绝对值得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于转换的结构。</param>
        /// <returns>PointD6D 结构，表示将 PointD6D 结构的所有分量取绝对值得到的结果</returns>
        public static PointD6D Abs(PointD6D pt)
        {
            return new PointD6D(Math.Abs(pt._X), Math.Abs(pt._Y), Math.Abs(pt._Z), Math.Abs(pt._U), Math.Abs(pt._V), Math.Abs(pt._W));
        }

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量舍入到较大的整数值得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于转换的结构。</param>
        /// <returns>PointD6D 结构，表示将 PointD6D 结构的所有分量舍入到较大的整数值得到的结果</returns>
        public static PointD6D Ceiling(PointD6D pt)
        {
            return new PointD6D(Math.Ceiling(pt._X), Math.Ceiling(pt._Y), Math.Ceiling(pt._Z), Math.Ceiling(pt._U), Math.Ceiling(pt._V), Math.Ceiling(pt._W));
        }

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量舍入到较小的整数值得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于转换的结构。</param>
        /// <returns>PointD6D 结构，表示将 PointD6D 结构的所有分量舍入到较小的整数值得到的结果</returns>
        public static PointD6D Floor(PointD6D pt)
        {
            return new PointD6D(Math.Floor(pt._X), Math.Floor(pt._Y), Math.Floor(pt._Z), Math.Floor(pt._U), Math.Floor(pt._V), Math.Floor(pt._W));
        }

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量舍入到最接近的整数值得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于转换的结构。</param>
        /// <returns>PointD6D 结构，表示将 PointD6D 结构的所有分量舍入到最接近的整数值得到的结果</returns>
        public static PointD6D Round(PointD6D pt)
        {
            return new PointD6D(Math.Round(pt._X), Math.Round(pt._Y), Math.Round(pt._Z), Math.Round(pt._U), Math.Round(pt._V), Math.Round(pt._W));
        }

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量截断小数部分取整得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于转换的结构。</param>
        /// <returns>PointD6D 结构，表示将 PointD6D 结构的所有分量截断小数部分取整得到的结果</returns>
        public static PointD6D Truncate(PointD6D pt)
        {
            return new PointD6D(Math.Truncate(pt._X), Math.Truncate(pt._Y), Math.Truncate(pt._Z), Math.Truncate(pt._U), Math.Truncate(pt._V), Math.Truncate(pt._W));
        }

        /// <summary>
        /// 返回将两个 PointD6D 结构的所有分量分别取最大值得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD6D 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD6D 结构，用于比较的第二个结构。</param>
        /// <returns>PointD6D 结构，表示将两个 PointD6D 结构的所有分量分别取最大值得到的结果</returns>
        public static PointD6D Max(PointD6D left, PointD6D right)
        {
            return new PointD6D(Math.Max(left._X, right._X), Math.Max(left._Y, right._Y), Math.Max(left._Z, right._Z), Math.Max(left._U, right._U), Math.Max(left._V, right._V), Math.Max(left._W, right._W));
        }

        /// <summary>
        /// 返回将两个 PointD6D 结构的所有分量分别取最小值得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD6D 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD6D 结构，用于比较的第二个结构。</param>
        /// <returns>PointD6D 结构，表示将两个 PointD6D 结构的所有分量分别取最小值得到的结果</returns>
        public static PointD6D Min(PointD6D left, PointD6D right)
        {
            return new PointD6D(Math.Min(left._X, right._X), Math.Min(left._Y, right._Y), Math.Min(left._Z, right._Z), Math.Min(left._U, right._U), Math.Min(left._V, right._V), Math.Min(left._W, right._W));
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 PointD6D 结构是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD6D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD6D 结构。</param>
        /// <returns>布尔值，表示两个 PointD6D 结构是否相等。</returns>
        public static bool operator ==(PointD6D left, PointD6D right)
        {
            return (left._X == right._X && left._Y == right._Y && left._Z == right._Z && left._U == right._U && left._V == right._V && left._W == right._W);
        }

        /// <summary>
        /// 判断两个 PointD6D 结构是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD6D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD6D 结构。</param>
        /// <returns>布尔值，表示两个 PointD6D 结构是否不相等。</returns>
        public static bool operator !=(PointD6D left, PointD6D right)
        {
            return (left._X != right._X || left._Y != right._Y || left._Z != right._Z || left._U != right._U || left._V != right._V || left._W != right._W);
        }

        /// <summary>
        /// 判断两个 PointD6D 结构的字典序是否前者小于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD6D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD6D 结构。</param>
        /// <returns>布尔值，表示两个 PointD6D 结构的字典序是否前者小于后者。</returns>
        public static bool operator <(PointD6D left, PointD6D right)
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
        /// 判断两个 PointD6D 结构的字典序是否前者大于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD6D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD6D 结构。</param>
        /// <returns>布尔值，表示两个 PointD6D 结构的字典序是否前者大于后者。</returns>
        public static bool operator >(PointD6D left, PointD6D right)
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
        /// 判断两个 PointD6D 结构的字典序是否前者小于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD6D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD6D 结构。</param>
        /// <returns>布尔值，表示两个 PointD6D 结构的字典序是否前者小于或等于后者。</returns>
        public static bool operator <=(PointD6D left, PointD6D right)
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
        /// 判断两个 PointD6D 结构的字典序是否前者大于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD6D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD6D 结构。</param>
        /// <returns>布尔值，表示两个 PointD6D 结构的字典序是否前者大于或等于后者。</returns>
        public static bool operator >=(PointD6D left, PointD6D right)
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
        /// 返回在 PointD6D 结构的所有分量前添加正号得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">运算符右侧的 PointD6D 结构。</param>
        /// <returns>PointD6D 结构，表示在 PointD6D 结构的所有分量前添加正号得到的结果。</returns>
        public static PointD6D operator +(PointD6D pt)
        {
            return pt;
        }

        /// <summary>
        /// 返回在 PointD6D 结构的所有分量前添加负号得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">运算符右侧的 PointD6D 结构。</param>
        /// <returns>PointD6D 结构，表示在 PointD6D 结构的所有分量前添加负号得到的结果。</returns>
        public static PointD6D operator -(PointD6D pt)
        {
            return new PointD6D(-pt._X, -pt._Y, -pt._Z, -pt._U, -pt._V, -pt._W);
        }

        //

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量与双精度浮点数相加得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，表示被加数。</param>
        /// <param name="n">双精度浮点数，表示加数。</param>
        /// <returns>PointD6D 结构，表示将 PointD6D 结构的所有分量与双精度浮点数相加得到的结果。</returns>
        public static PointD6D operator +(PointD6D pt, double n)
        {
            return new PointD6D(pt._X + n, pt._Y + n, pt._Z + n, pt._U + n, pt._V + n, pt._W + n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD6D 结构的所有分量相加得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被加数。</param>
        /// <param name="pt">PointD6D 结构，表示加数。</param>
        /// <returns>PointD6D 结构，表示将双精度浮点数与 PointD6D 结构的所有分量相加得到的结果。</returns>
        public static PointD6D operator +(double n, PointD6D pt)
        {
            return new PointD6D(n + pt._X, n + pt._Y, n + pt._Z, n + pt._U, n + pt._V, n + pt._W);
        }

        /// <summary>
        /// 返回将 PointD6D 结构与 PointD6D 结构的所有分量对应相加得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD6D 结构，表示被加数。</param>
        /// <param name="right">PointD6D 结构，表示加数。</param>
        /// <returns>PointD6D 结构，表示将 PointD6D 结构与 PointD6D 结构的所有分量对应相加得到的结果。</returns>
        public static PointD6D operator +(PointD6D left, PointD6D right)
        {
            return new PointD6D(left._X + right._X, left._Y + right._Y, left._Z + right._Z, left._U + right._U, left._V + right._V, left._W + right._W);
        }

        //

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量与双精度浮点数相减得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，表示被减数。</param>
        /// <param name="n">双精度浮点数，表示减数。</param>
        /// <returns>PointD6D 结构，表示将 PointD6D 结构的所有分量与双精度浮点数相减得到的结果。</returns>
        public static PointD6D operator -(PointD6D pt, double n)
        {
            return new PointD6D(pt._X - n, pt._Y - n, pt._Z - n, pt._U - n, pt._V - n, pt._W - n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD6D 结构的所有分量相减得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被减数。</param>
        /// <param name="pt">PointD6D 结构，表示减数。</param>
        /// <returns>PointD6D 结构，表示将双精度浮点数与 PointD6D 结构的所有分量相减得到的结果。</returns>
        public static PointD6D operator -(double n, PointD6D pt)
        {
            return new PointD6D(n - pt._X, n - pt._Y, n - pt._Z, n - pt._U, n - pt._V, n - pt._W);
        }

        /// <summary>
        /// 返回将 PointD6D 结构与 PointD6D 结构的所有分量对应相减得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD6D 结构，表示被减数。</param>
        /// <param name="right">PointD6D 结构，表示减数。</param>
        /// <returns>PointD6D 结构，表示将 PointD6D 结构与 PointD6D 结构的所有分量对应相减得到的结果。</returns>
        public static PointD6D operator -(PointD6D left, PointD6D right)
        {
            return new PointD6D(left._X - right._X, left._Y - right._Y, left._Z - right._Z, left._U - right._U, left._V - right._V, left._W - right._W);
        }

        //

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量与双精度浮点数相乘得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，表示被乘数。</param>
        /// <param name="n">双精度浮点数，表示乘数。</param>
        /// <returns>PointD6D 结构，表示将 PointD6D 结构的所有分量与双精度浮点数相乘得到的结果。</returns>
        public static PointD6D operator *(PointD6D pt, double n)
        {
            return new PointD6D(pt._X * n, pt._Y * n, pt._Z * n, pt._U * n, pt._V * n, pt._W * n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD6D 结构的所有分量相乘得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被乘数。</param>
        /// <param name="pt">PointD6D 结构，表示乘数。</param>
        /// <returns>PointD6D 结构，表示将双精度浮点数与 PointD6D 结构的所有分量相乘得到的结果。</returns>
        public static PointD6D operator *(double n, PointD6D pt)
        {
            return new PointD6D(n * pt._X, n * pt._Y, n * pt._Z, n * pt._U, n * pt._V, n * pt._W);
        }

        /// <summary>
        /// 返回将 PointD6D 结构与 PointD6D 结构的所有分量对应相乘得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD6D 结构，表示被乘数。</param>
        /// <param name="right">PointD6D 结构，表示乘数。</param>
        /// <returns>PointD6D 结构，表示将 PointD6D 结构与 PointD6D 结构的所有分量对应相乘得到的结果。</returns>
        public static PointD6D operator *(PointD6D left, PointD6D right)
        {
            return new PointD6D(left._X * right._X, left._Y * right._Y, left._Z * right._Z, left._U * right._U, left._V * right._V, left._W * right._W);
        }

        //

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量与双精度浮点数相除得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，表示被除数。</param>
        /// <param name="n">双精度浮点数，表示除数。</param>
        /// <returns>PointD6D 结构，表示将 PointD6D 结构的所有分量与双精度浮点数相除得到的结果。</returns>
        public static PointD6D operator /(PointD6D pt, double n)
        {
            return new PointD6D(pt._X / n, pt._Y / n, pt._Z / n, pt._U / n, pt._V / n, pt._W / n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD6D 结构的所有分量相除得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被除数。</param>
        /// <param name="pt">PointD6D 结构，表示除数。</param>
        /// <returns>PointD6D 结构，表示将双精度浮点数与 PointD6D 结构的所有分量相除得到的结果。</returns>
        public static PointD6D operator /(double n, PointD6D pt)
        {
            return new PointD6D(n / pt._X, n / pt._Y, n / pt._Z, n / pt._U, n / pt._V, n / pt._W);
        }

        /// <summary>
        /// 返回将 PointD6D 结构与 PointD6D 结构的所有分量对应相除得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD6D 结构，表示被除数。</param>
        /// <param name="right">PointD6D 结构，表示除数。</param>
        /// <returns>PointD6D 结构，表示将 PointD6D 结构与 PointD6D 结构的所有分量对应相除得到的结果。</returns>
        public static PointD6D operator /(PointD6D left, PointD6D right)
        {
            return new PointD6D(left._X / right._X, left._Y / right._Y, left._Z / right._Z, left._U / right._U, left._V / right._V, left._W / right._W);
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
            this = new PointD6D();
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
            private PointD6D _Pt;
            private int _Index;

            internal Enumerator(PointD6D pt)
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
            this = new PointD6D();
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
            private PointD6D _Pt;
            private int _Index;

            internal GenericEnumerator(PointD6D pt)
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