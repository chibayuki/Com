/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.PointD3D
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
    /// 以一组有序的双精度浮点数表示的三维直角坐标系坐标。
    /// </summary>
    public struct PointD3D : IEquatable<PointD3D>, IComparable, IComparable<PointD3D>, IEuclideanVector<PointD3D>, IAffine<PointD3D>
    {
        #region 私有成员与内部成员

        private double _X; // X 坐标。
        private double _Y; // Y 坐标。
        private double _Z; // Z 坐标。

        #endregion

        #region 常量与只读字段

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

        #region 属性

        /// <summary>
        /// 获取或设置此 PointD3D 结构在指定索引的坐标轴的分量。
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
                    default: return double.NaN;
                }
            }

            set
            {
                switch (index)
                {
                    case 0: _X = value; break;
                    case 1: _Y = value; break;
                    case 2: _Z = value; break;
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
                return 3;
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
                return (_X == Zero._X && _Y == Zero._Y && _Z == Zero._Z);
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
                return ((!double.IsNaN(_X) && !double.IsNaN(_Y) && !double.IsNaN(_Z)) && (double.IsInfinity(_X) || double.IsInfinity(_Y) || double.IsInfinity(_Z)));
            }
        }

        /// <summary>
        /// 获取表示此 PointD3D 结构是否包含非数字或无穷大分量的布尔值。
        /// </summary>
        public bool IsNaNOrInfinity
        {
            get
            {
                return ((double.IsNaN(_X) || double.IsNaN(_Y) || double.IsNaN(_Z)) || (double.IsInfinity(_X) || double.IsInfinity(_Y) || double.IsInfinity(_Z)));
            }
        }

        //

        /// <summary>
        /// 获取此 PointD3D 结构表示的向量的模。
        /// </summary>
        public double Module
        {
            get
            {
                return Math.Sqrt(ModuleSquared);
            }
        }

        /// <summary>
        /// 获取此 PointD3D 结构表示的向量的模平方。
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
        /// 获取此 PointD3D 结构表示的向量的相反向量。
        /// </summary>
        public PointD3D Negate
        {
            get
            {
                return new PointD3D(-_X, -_Y, -_Z);
            }
        }

        /// <summary>
        /// 获取此 PointD3D 结构表示的向量的规范化向量。
        /// </summary>
        public PointD3D Normalize
        {
            get
            {
                double Mod = Module;

                if (Mod > 0)
                {
                    return new PointD3D(_X / Mod, _Y / Mod, _Z / Mod);
                }
                else
                {
                    return Ex;
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
        /// 获取此 PointD3D 结构表示的向量与 X 轴之间的夹角（弧度）。
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
        /// 获取此 PointD3D 结构表示的向量与 Y 轴之间的夹角（弧度）。
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

        /// <summary>
        /// 获取此 PointD3D 结构表示的向量与 Z 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleFromZ
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }

                return AngleFrom(_Z >= 0 ? Ez : -Ez);
            }
        }

        /// <summary>
        /// 获取此 PointD3D 结构表示的向量与 XY 平面之间的夹角（弧度）。
        /// </summary>
        public double AngleFromXY
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }

                return (Math.PI / 2 - AngleFromZ);
            }
        }

        /// <summary>
        /// 获取此 PointD3D 结构表示的向量与 YZ 平面之间的夹角（弧度）。
        /// </summary>
        public double AngleFromYZ
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }

                return (Math.PI / 2 - AngleFromX);
            }
        }

        /// <summary>
        /// 获取此 PointD3D 结构表示的向量与 ZX 平面之间的夹角（弧度）。
        /// </summary>
        public double AngleFromZX
        {
            get
            {
                if (IsZero)
                {
                    return 0;
                }

                return (Math.PI / 2 - AngleFromY);
            }
        }

        //

        /// <summary>
        /// 获取此 PointD3D 结构表示的向量的仰角。仰角是向量与 +Z 轴之间的夹角（弧度）（以 +Z 轴为 0 弧度，远离 +Z 轴的方向为正方向）。
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
        /// 获取此 PointD3D 结构表示的向量的方位角。方位角是向量在 XY 平面内的投影与 +X 轴之间的夹角（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。
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
        /// 判断此 PointD3D 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is PointD3D))
            {
                return false;
            }
            else if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals((PointD3D)obj);
        }

        /// <summary>
        /// 返回此 PointD3D 结构的哈希代码。
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 PointD3D 结构转换为字符串。
        /// </summary>
        public override string ToString()
        {
            return string.Concat("{X=", _X, ", Y=", _Y, ", Z=", _Z, "}");
        }

        //

        /// <summary>
        /// 判断此 PointD3D 结构是否与指定的 PointD3D 结构相等。
        /// </summary>
        /// <param name="pt">用于比较的 PointD3D 结构。</param>
        public bool Equals(PointD3D pt)
        {
            if ((object)pt == null)
            {
                return false;
            }
            else if (object.ReferenceEquals(this, pt))
            {
                return true;
            }

            return (_X.Equals(pt._X) && _Y.Equals(pt._Y) && _Z.Equals(pt._Z));
        }

        //

        /// <summary>
        /// 将此 PointD3D 结构与指定的对象进行次序比较。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is PointD3D))
            {
                return 1;
            }
            else if (object.ReferenceEquals(this, obj))
            {
                return 0;
            }

            return CompareTo((PointD3D)obj);
        }

        /// <summary>
        /// 将此 PointD3D 结构与指定的 PointD3D 结构进行次序比较。
        /// </summary>
        /// <param name="pt">用于比较的 PointD3D 结构。</param>
        public int CompareTo(PointD3D pt)
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
        /// 遍历此 PointD3D 结构的所有分量并返回第一个与指定值相等的索引。
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
            else if (_Z.Equals(item))
            {
                return 2;
            }

            return -1;
        }

        /// <summary>
        /// 遍历此 PointD3D 结构的所有分量并返回表示是否存在与指定值相等的分量的布尔值。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        public bool Contains(double item)
        {
            if (_X.Equals(item) || _Y.Equals(item) || _Z.Equals(item))
            {
                return true;
            }

            return false;
        }

        //

        /// <summary>
        /// 将此 PointD3D 结构转换为双精度浮点数数组。
        /// </summary>
        public double[] ToArray()
        {
            return new double[3] { _X, _Y, _Z };
        }

        /// <summary>
        /// 将此 PointD3D 结构转换为双精度浮点数列表。
        /// </summary>
        public List<double> ToList()
        {
            return new List<double>(3) { _X, _Y, _Z };
        }

        //

        /// <summary>
        /// 返回将此 PointD3D 结构表示的直角坐标系坐标转换为球坐标系坐标的新实例。
        /// </summary>
        public PointD3D ToSpherical()
        {
            return new PointD3D(Module, Zenith, Azimuth);
        }

        /// <summary>
        /// 返回将此 PointD3D 结构表示的球坐标系坐标转换为直角坐标系坐标的新实例。
        /// </summary>
        public PointD3D ToCartesian()
        {
            return new PointD3D(_X * Math.Sin(_Y) * Math.Cos(_Z), _X * Math.Sin(_Y) * Math.Sin(_Z), _X * Math.Cos(_Y));
        }

        //

        /// <summary>
        /// 返回此 PointD3D 结构与指定的 PointD3D 结构之间的距离。
        /// </summary>
        /// <param name="pt">PointD3D 结构，表示起始点。</param>
        public double DistanceFrom(PointD3D pt)
        {
            double dx = _X - pt._X, dy = _Y - pt._Y, dz = _Z - pt._Z;

            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// 返回此 PointD3D 结构表示的向量与指定的 PointD3D 结构表示的向量之间的夹角（弧度）。
        /// </summary>
        /// <param name="pt">PointD3D 结构，表示起始向量。</param>
        public double AngleFrom(PointD3D pt)
        {
            if (IsZero || pt.IsZero)
            {
                return 0;
            }

            double DotProduct = _X * pt._X + _Y * pt._Y + _Z * pt._Z;

            return Math.Acos(DotProduct / Module / pt.Module);
        }

        //

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
        /// 按 PointD3D 结构将此 PointD3D 结构平移指定的量。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于平移此 PointD3D 结构。</param>
        public void Offset(PointD3D pt)
        {
            _X += pt._X;
            _Y += pt._Y;
            _Z += pt._Z;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的位移将此 PointD3D 结构的副本的所有分量平移指定的量的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public PointD3D OffsetCopy(double d)
        {
            return new PointD3D(_X + d, _Y + d, _Z + d);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标位移、Y 坐标位移与 Z 坐标位移将此 PointD3D 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标位移。</param>
        public PointD3D OffsetCopy(double dx, double dy, double dz)
        {
            return new PointD3D(_X + dx, _Y + dy, _Z + dz);
        }

        /// <summary>
        /// 返回按 PointD3D 结构将此 PointD3D 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于平移此 PointD3D 结构。</param>
        public PointD3D OffsetCopy(PointD3D pt)
        {
            return new PointD3D(_X + pt._X, _Y + pt._Y, _Z + pt._Z);
        }

        //

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
        /// 按 PointD3D 结构将此 PointD3D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于缩放此 PointD3D 结构。</param>
        public void Scale(PointD3D pt)
        {
            _X *= pt._X;
            _Y *= pt._Y;
            _Z *= pt._Z;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数将此 PointD3D 结构的副本的所有分量缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public PointD3D ScaleCopy(double s)
        {
            return new PointD3D(_X * s, _Y * s, _Z * s);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标缩放因数、Y 坐标缩放因数与 Z 坐标缩放因数将此 PointD3D 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因数。</param>
        public PointD3D ScaleCopy(double sx, double sy, double sz)
        {
            return new PointD3D(_X * sx, _Y * sy, _Z * sz);
        }

        /// <summary>
        /// 返回按 PointD3D 结构将此 PointD3D 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于缩放此 PointD3D 结构。</param>
        public PointD3D ScaleCopy(PointD3D pt)
        {
            return new PointD3D(_X * pt._X, _Y * pt._Y, _Z * pt._Z);
        }

        //

        /// <summary>
        /// 将此 PointD3D 结构的指定的基向量方向的分量翻转。
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
        /// 返回将此 PointD3D 结构的副本的由指定的基向量方向的分量翻转的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        public PointD3D ReflectCopy(int index)
        {
            switch (index)
            {
                case 0: return new PointD3D(-_X, _Y, _Z);
                case 1: return new PointD3D(_X, -_Y, _Z);
                case 2: return new PointD3D(_X, _Y, -_Z);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将此 PointD3D 结构的副本在 X 轴的分量翻转的新实例。
        /// </summary>
        public PointD3D ReflectXCopy()
        {
            return new PointD3D(-_X, _Y, _Z);
        }

        /// <summary>
        /// 返回将此 PointD3D 结构的副本在 Y 轴的分量翻转的新实例。
        /// </summary>
        public PointD3D ReflectYCopy()
        {
            return new PointD3D(_X, -_Y, _Z);
        }

        /// <summary>
        /// 返回将此 PointD3D 结构的副本在 Z 轴的分量翻转的新实例。
        /// </summary>
        public PointD3D ReflectZCopy()
        {
            return new PointD3D(_X, _Y, -_Z);
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构剪切指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向平行且同方向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向垂直且共平面的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构的副本沿平行于索引 index1 指定的基向量且与之同方向以及垂直于 index2 指定的基向量且与之共平面的方向剪切的角度（弧度）。</param>
        public void Shear(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().ShearCopy(index1, index2, angle);

            if (!Vector.IsNullOrEmpty(result) && result.Dimension == 3)
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构在 XY 平面向 +X 轴剪切指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 XY 平面向 +X 轴剪切的角度（弧度）。</param>
        public void ShearXY(double angle)
        {
            Shear(0, 1, angle);
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构在 XY 平面向 +Y 轴剪切指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 XY 平面向 +Y 轴剪切的角度（弧度）。</param>
        public void ShearYX(double angle)
        {
            Shear(1, 0, angle);
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构在 YZ 平面向 +Y 轴剪切指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 YZ 平面向 +Y 轴剪切的角度（弧度）。</param>
        public void ShearYZ(double angle)
        {
            Shear(1, 2, angle);
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构在 YZ 平面向 +Z 轴剪切指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 YZ 平面向 +Z 轴剪切的角度（弧度）。</param>
        public void ShearZY(double angle)
        {
            Shear(2, 1, angle);
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构在 ZX 平面向 +Z 轴剪切指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 ZX 平面向 +Z 轴剪切的角度（弧度）。</param>
        public void ShearZX(double angle)
        {
            Shear(2, 0, angle);
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构在 ZX 平面向 +X 轴剪切指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 ZX 平面向 +X 轴剪切的角度（弧度）。</param>
        public void ShearXZ(double angle)
        {
            Shear(0, 2, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构的副本剪切指定的角度的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向平行且同方向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向垂直且共平面的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构的副本沿平行于索引 index1 指定的基向量且与之同方向以及垂直于 index2 指定的基向量且与之共平面的方向剪切的角度（弧度）。</param>
        public PointD3D ShearCopy(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().ShearCopy(index1, index2, angle);

            if (!Vector.IsNullOrEmpty(result) && result.Dimension == 3)
            {
                return new PointD3D(result[0], result[1], result[2]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构的副本在 XY 平面向 +X 轴剪切指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构的副本在 XY 平面向 +X 轴剪切的角度（弧度）。</param>
        public PointD3D ShearXYCopy(double angle)
        {
            return ShearCopy(0, 1, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构的副本在 XY 平面向 +Y 轴剪切指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构的副本在 XY 平面向 +Y 轴剪切的角度（弧度）。</param>
        public PointD3D ShearYXCopy(double angle)
        {
            return ShearCopy(1, 0, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构的副本在 YZ 平面向 +Y 轴剪切指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构的副本在 YZ 平面向 +Y 轴剪切的角度（弧度）。</param>
        public PointD3D ShearYZCopy(double angle)
        {
            return ShearCopy(1, 2, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构的副本在 YZ 平面向 +Z 轴剪切指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构的副本在 YZ 平面向 +Z 轴剪切的角度（弧度）。</param>
        public PointD3D ShearZYCopy(double angle)
        {
            return ShearCopy(2, 1, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构的副本在 ZX 平面向 +Z 轴剪切指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构的副本在 ZX 平面向 +Z 轴剪切的角度（弧度）。</param>
        public PointD3D ShearZXCopy(double angle)
        {
            return ShearCopy(2, 0, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构的副本在 ZX 平面向 +X 轴剪切指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构的副本在 ZX 平面向 +X 轴剪切的角度（弧度）。</param>
        public PointD3D ShearXZCopy(double angle)
        {
            return ShearCopy(0, 2, angle);
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

            if (!Vector.IsNullOrEmpty(result) && result.Dimension == 3)
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构绕 X 轴旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构绕 X 轴旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        public void RotateX(double angle)
        {
            Rotate(1, 2, angle);
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构绕 Y 轴旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构绕 Y 轴旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +X 轴的方向为正方向）。</param>
        public void RotateY(double angle)
        {
            Rotate(2, 0, angle);
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构绕 Z 轴旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构绕 Z 轴旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public void RotateZ(double angle)
        {
            Rotate(0, 1, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构的副本旋转指定的角度的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构的副本绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        public PointD3D RotateCopy(int index1, int index2, double angle)
        {
            Vector result = ToColumnVector().RotateCopy(index1, index2, angle);

            if (!Vector.IsNullOrEmpty(result) && result.Dimension == 3)
            {
                return new PointD3D(result[0], result[1], result[2]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构的副本绕 X 轴旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构的副本绕 X 轴旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        public PointD3D RotateXCopy(double angle)
        {
            return RotateCopy(1, 2, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构的副本绕 Y 轴旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构的副本绕 Y 轴旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +X 轴的方向为正方向）。</param>
        public PointD3D RotateYCopy(double angle)
        {
            return RotateCopy(2, 0, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构的副本的副本绕 Z 轴旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构的副本绕 Z 轴旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public PointD3D RotateZCopy(double angle)
        {
            return RotateCopy(0, 1, angle);
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
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[4, 4]
            {
                { ex._X, ex._Y, ex._Z, 0 },
                { ey._X, ey._Y, ey._Z, 0 },
                { ez._X, ez._Y, ez._Z, 0 },
                { offset._X, offset._Y, offset._Z, 1 }
            });

            Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

            if (!Vector.IsNullOrEmpty(result) && result.Dimension == 3)
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
            }
        }

        /// <summary>
        /// 按 Matrix 对象表示的 4x4 仿射矩阵（左矩阵）将此 PointD3D 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 4x4 仿射矩阵（左矩阵）。</param>
        public void AffineTransform(Matrix matrixLeft)
        {
            if (!Matrix.IsNullOrEmpty(matrixLeft) && matrixLeft.Size == new Size(4, 4))
            {
                Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 3)
                {
                    _X = result[0];
                    _Y = result[1];
                    _Z = result[2];
                }
            }
        }

        /// <summary>
        /// 按 Matrix 对象列表表示的 4x4 仿射矩阵（左矩阵）列表将此 PointD3D 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 4x4 仿射矩阵（左矩阵）列表。</param>
        public void AffineTransform(List<Matrix> matrixLeftList)
        {
            if (!InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                Vector result = ToColumnVector().AffineTransformCopy(matrixLeftList);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 3)
                {
                    _X = result[0];
                    _Y = result[1];
                    _Z = result[2];
                }
            }
        }

        /// <summary>
        /// 返回按 PointD3D 结构表示的 X 基向量、Y 基向量、Z 基向量与偏移向量将此 PointD3D 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="ex">PointD3D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD3D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD3D 结构表示的 Z 基向量。</param>
        /// <param name="offset">PointD3D 结构表示的偏移向量。</param>
        public PointD3D AffineTransformCopy(PointD3D ex, PointD3D ey, PointD3D ez, PointD3D offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[4, 4]
            {
                { ex._X, ex._Y, ex._Z, 0 },
                { ey._X, ey._Y, ey._Z, 0 },
                { ez._X, ez._Y, ez._Z, 0 },
                { offset._X, offset._Y, offset._Z, 1 }
            });

            Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

            if (!Vector.IsNullOrEmpty(result) && result.Dimension == 3)
            {
                return new PointD3D(result[0], result[1], result[2]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的 4x4 仿射矩阵（左矩阵）将此 PointD3D 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 4x4 仿射矩阵（左矩阵）。</param>
        public PointD3D AffineTransformCopy(Matrix matrixLeft)
        {
            if (!Matrix.IsNullOrEmpty(matrixLeft) && matrixLeft.Size == new Size(4, 4))
            {
                Vector result = ToColumnVector().AffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 3)
                {
                    return new PointD3D(result[0], result[1], result[2]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 4x4 仿射矩阵（左矩阵）列表将此 PointD3D 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 4x4 仿射矩阵（左矩阵）列表。</param>
        public PointD3D AffineTransformCopy(List<Matrix> matrixLeftList)
        {
            if (!InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                Vector result = ToColumnVector().AffineTransformCopy(matrixLeftList);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 3)
                {
                    return new PointD3D(result[0], result[1], result[2]);
                }
            }

            return NaN;
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
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[4, 4]
            {
                { ex._X, ex._Y, ex._Z, 0 },
                { ey._X, ey._Y, ey._Z, 0 },
                { ez._X, ez._Y, ez._Z, 0 },
                { offset._X, offset._Y, offset._Z, 1 }
            });

            Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

            if (!Vector.IsNullOrEmpty(result) && result.Dimension == 3)
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
            }
        }

        /// <summary>
        /// 按 Matrix 对象表示的 4x4 仿射矩阵（左矩阵）将此 PointD3D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 4x4 仿射矩阵（左矩阵）。</param>
        public void InverseAffineTransform(Matrix matrixLeft)
        {
            if (!Matrix.IsNullOrEmpty(matrixLeft) && matrixLeft.Size == new Size(4, 4))
            {
                Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 3)
                {
                    _X = result[0];
                    _Y = result[1];
                    _Z = result[2];
                }
            }
        }

        /// <summary>
        /// 按 Matrix 对象列表表示的 4x4 仿射矩阵（左矩阵）列表将此 PointD3D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 4x4 仿射矩阵（左矩阵）列表。</param>
        public void InverseAffineTransform(List<Matrix> matrixLeftList)
        {
            if (!InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeftList);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 3)
                {
                    _X = result[0];
                    _Y = result[1];
                    _Z = result[2];
                }
            }
        }

        /// <summary>
        /// 返回按 PointD3D 结构表示的 X 基向量、Y 基向量、Z 基向量与偏移向量将此 PointD3D 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="ex">PointD3D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD3D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD3D 结构表示的 Z 基向量。</param>
        /// <param name="offset">PointD3D 结构表示的偏移向量。</param>
        public PointD3D InverseAffineTransformCopy(PointD3D ex, PointD3D ey, PointD3D ez, PointD3D offset)
        {
            Matrix matrixLeft = Matrix.UnsafeCreateInstance(new double[4, 4]
            {
                { ex._X, ex._Y, ex._Z, 0 },
                { ey._X, ey._Y, ey._Z, 0 },
                { ez._X, ez._Y, ez._Z, 0 },
                { offset._X, offset._Y, offset._Z, 1 }
            });

            Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

            if (!Vector.IsNullOrEmpty(result) && result.Dimension == 3)
            {
                return new PointD3D(result[0], result[1], result[2]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的 4x4 仿射矩阵（左矩阵）将此 PointD3D 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 4x4 仿射矩阵（左矩阵）。</param>
        public PointD3D InverseAffineTransformCopy(Matrix matrixLeft)
        {
            if (!Matrix.IsNullOrEmpty(matrixLeft) && matrixLeft.Size == new Size(4, 4))
            {
                Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 3)
                {
                    return new PointD3D(result[0], result[1], result[2]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 4x4 仿射矩阵（左矩阵）列表将此 PointD3D 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 4x4 仿射矩阵（左矩阵）列表。</param>
        public PointD3D InverseAffineTransformCopy(List<Matrix> matrixLeftList)
        {
            if (!InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                Vector result = ToColumnVector().InverseAffineTransformCopy(matrixLeftList);

                if (!Vector.IsNullOrEmpty(result) && result.Dimension == 3)
                {
                    return new PointD3D(result[0], result[1], result[2]);
                }
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将此 PointD3D 结构投影至平行于 XY 平面的投影面的 PointD 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD3D 结构，表示投射中心在投影面的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影面的一维度量其真实尺度与投影尺度的比值等于其到投影面的距离与此距离的比值。</param>
        public PointD ProjectToXY(PointD3D prjCenter, double trueLenDist)
        {
            if (trueLenDist == 0)
            {
                return XY;
            }
            else
            {
                if (_Z != prjCenter._Z)
                {
                    double Scale = trueLenDist / (_Z - prjCenter._Z);

                    if ((!InternalMethod.IsNaNOrInfinity(Scale)) && Scale > 0)
                    {
                        return (Scale * XY + (1 - Scale) * prjCenter.XY);
                    }
                }
            }

            return PointD.NaN;
        }

        /// <summary>
        /// 返回将此 PointD3D 结构投影至平行于 YZ 平面的投影面的 PointD 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD3D 结构，表示投射中心在投影面的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影面的一维度量其真实尺度与投影尺度的比值等于其到投影面的距离与此距离的比值。</param>
        public PointD ProjectToYZ(PointD3D prjCenter, double trueLenDist)
        {
            if (trueLenDist == 0)
            {
                return YZ;
            }
            else
            {
                if (_X != prjCenter._X)
                {
                    double Scale = trueLenDist / (_X - prjCenter._X);

                    if ((!InternalMethod.IsNaNOrInfinity(Scale)) && Scale > 0)
                    {
                        return (Scale * YZ + (1 - Scale) * prjCenter.YZ);
                    }
                }
            }

            return PointD.NaN;
        }

        /// <summary>
        /// 返回将此 PointD3D 结构投影至平行于 ZX 平面的投影面的 PointD 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD3D 结构，表示投射中心在投影面的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影面的一维度量其真实尺度与投影尺度的比值等于其到投影面的距离与此距离的比值。</param>
        public PointD ProjectToZX(PointD3D prjCenter, double trueLenDist)
        {
            if (trueLenDist == 0)
            {
                return ZX;
            }
            else
            {
                if (_Y != prjCenter._Y)
                {
                    double Scale = trueLenDist / (_Y - prjCenter._Y);

                    if ((!InternalMethod.IsNaNOrInfinity(Scale)) && Scale > 0)
                    {
                        return (Scale * ZX + (1 - Scale) * prjCenter.ZX);
                    }
                }
            }

            return PointD.NaN;
        }

        //

        /// <summary>
        /// 返回将此 PointD3D 结构转换为列向量的 Vector 的新实例。
        /// </summary>
        public Vector ToColumnVector()
        {
            return Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, _X, _Y, _Z);
        }

        /// <summary>
        /// 返回将此 PointD3D 结构转换为行向量的 Vector 的新实例。
        /// </summary>
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
        public static bool Equals(PointD3D left, PointD3D right)
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
        /// 比较两个 PointD3D 结构的次序。
        /// </summary>
        /// <param name="left">用于比较的第一个 PointD3D 结构。</param>
        /// <param name="right">用于比较的第二个 PointD3D 结构。</param>
        public static int Compare(PointD3D left, PointD3D right)
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
        /// 返回单位矩阵，表示不对 PointD3D 结构进行仿射变换的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        public static Matrix IdentityMatrix()
        {
            return Matrix.Identity(4);
        }

        //

        /// <summary>
        /// 返回按双精度浮点数表示的位移将 PointD3D 结构的所有分量平移指定的量的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public static Matrix OffsetMatrix(double d)
        {
            return Vector.OffsetMatrix(Vector.Type.ColumnVector, 3, d);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标位移、Y 坐标位移与 Z 坐标位移将 PointD3D 结构平移指定的量的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标位移。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标位移。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标位移。</param>
        public static Matrix OffsetMatrix(double dx, double dy, double dz)
        {
            return Vector.OffsetMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, dx, dy, dz));
        }

        /// <summary>
        /// 返回按 PointD3D 结构将 PointD3D 结构平移指定的量的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于平移 PointD3D 结构。</param>
        public static Matrix OffsetMatrix(PointD3D pt)
        {
            return Vector.OffsetMatrix(pt.ToColumnVector());
        }

        //

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数将 PointD3D 结构的所有分量缩放指定的倍数的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public static Matrix ScaleMatrix(double s)
        {
            return Vector.ScaleMatrix(Vector.Type.ColumnVector, 3, s);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标缩放因数、Y 坐标缩放因数与 Z 坐标缩放因数将 PointD3D 结构缩放指定的倍数的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因数。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因数。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因数。</param>
        public static Matrix ScaleMatrix(double sx, double sy, double sz)
        {
            return Vector.ScaleMatrix(Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, sx, sy, sz));
        }

        /// <summary>
        /// 返回按 PointD3D 结构将 PointD3D 结构缩放指定的倍数的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于缩放 PointD3D 结构。</param>
        public static Matrix ScaleMatrix(PointD3D pt)
        {
            return Vector.ScaleMatrix(pt.ToColumnVector());
        }

        //

        /// <summary>
        /// 返回表示用于翻转 PointD3D 结构的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        public static Matrix ReflectMatrix(int index)
        {
            return Vector.ReflectMatrix(Vector.Type.ColumnVector, 3, index);
        }

        /// <summary>
        /// 返回将 PointD3D 结构在 X 轴的分量翻转的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        public static Matrix ReflectXMatrix()
        {
            return ReflectMatrix(0);
        }

        /// <summary>
        /// 返回将 PointD3D 结构在 Y 轴的分量翻转的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        public static Matrix ReflectYMatrix()
        {
            return ReflectMatrix(1);
        }

        /// <summary>
        /// 返回将 PointD3D 结构在 Z 轴的分量翻转的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        public static Matrix ReflectZMatrix()
        {
            return ReflectMatrix(2);
        }

        //

        /// <summary>
        /// 返回表示用于剪切 PointD3D 结构的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示 PointD3D 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        public static Matrix ShearMatrix(int index1, int index2, double angle)
        {
            return Vector.ShearMatrix(Vector.Type.ColumnVector, 3, index1, index2, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD3D 结构在 XY 平面向 +X 轴剪切指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 XY 平面向 +X 轴剪切的角度（弧度）。</param>
        public static Matrix ShearXYMatrix(double angle)
        {
            return ShearMatrix(0, 1, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD3D 结构在 XY 平面向 +Y 轴剪切指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 XY 平面向 +Y 轴剪切的角度（弧度）。</param>
        public static Matrix ShearYXMatrix(double angle)
        {
            return ShearMatrix(1, 0, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD3D 结构在 YZ 平面向 +Y 轴剪切指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 YZ 平面向 +Y 轴剪切的角度（弧度）。</param>
        public static Matrix ShearYZMatrix(double angle)
        {
            return ShearMatrix(1, 2, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD3D 结构在 YZ 平面向 +Z 轴剪切指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 YZ 平面向 +Z 轴剪切的角度（弧度）。</param>
        public static Matrix ShearZYMatrix(double angle)
        {
            return ShearMatrix(2, 1, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD3D 结构在 ZX 平面向 +Z 轴剪切指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 ZX 平面向 +Z 轴剪切的角度（弧度）。</param>
        public static Matrix ShearZXMatrix(double angle)
        {
            return ShearMatrix(2, 0, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD3D 结构在 ZX 平面向 +X 轴剪切指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD3D 结构在 ZX 平面向 +X 轴剪切的角度（弧度）。</param>
        public static Matrix ShearXZMatrix(double angle)
        {
            return ShearMatrix(0, 2, angle);
        }

        //

        /// <summary>
        /// 返回表示用于旋转 PointD3D 结构的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示 PointD3D 结构绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        public static Matrix RotateMatrix(int index1, int index2, double angle)
        {
            return Vector.RotateMatrix(Vector.Type.ColumnVector, 3, index1, index2, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD3D 结构绕 X 轴旋转指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD3D 结构绕 X 轴旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        public static Matrix RotateXMatrix(double angle)
        {
            return RotateMatrix(1, 2, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD3D 结构绕 Y 轴旋转指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD3D 结构绕 Y 轴旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +X 轴的方向为正方向）。</param>
        public static Matrix RotateYMatrix(double angle)
        {
            return RotateMatrix(2, 0, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD3D 结构绕 Z 轴旋转指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD3D 结构绕 Z 轴旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public static Matrix RotateZMatrix(double angle)
        {
            return RotateMatrix(0, 1, angle);
        }

        //

        /// <summary>
        /// 返回两个 PointD3D 结构之间的距离。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示第一个点。</param>
        /// <param name="right">PointD3D 结构，表示第二个点。</param>
        public static double DistanceBetween(PointD3D left, PointD3D right)
        {
            double dx = left._X - right._X, dy = left._Y - right._Y, dz = left._Z - right._Z;

            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// 返回 PointD3D 结构表示的两个向量之间的夹角（弧度）。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示第一个向量。</param>
        /// <param name="right">PointD3D 结构，表示第二个向量。</param>
        public static double AngleBetween(PointD3D left, PointD3D right)
        {
            if (left.IsZero || right.IsZero)
            {
                return 0;
            }

            double DotProduct = left._X * right._X + left._Y * right._Y + left._Z * right._Z;

            return Math.Acos(DotProduct / left.Module / right.Module);
        }

        //

        /// <summary>
        /// 返回 PointD3D 结构表示的两个向量的数量积。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示第一个向量。</param>
        /// <param name="right">PointD3D 结构，表示第二个向量。</param>
        public static double DotProduct(PointD3D left, PointD3D right)
        {
            return (left._X * right._X + left._Y * right._Y + left._Z * right._Z);
        }

        /// <summary>
        /// 返回 PointD3D 结构表示的两个向量的向量积。该向量积为一个三维向量，其所有分量的数值依次为 X 基向量、Y 基向量与 Z 基向量的系数。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示左向量。</param>
        /// <param name="right">PointD3D 结构，表示右向量。</param>
        public static PointD3D CrossProduct(PointD3D left, PointD3D right)
        {
            return new PointD3D(left._Y * right._Z - left._Z * right._Y, left._Z * right._X - left._X * right._Z, left._X * right._Y - left._Y * right._X);
        }

        //

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量取绝对值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        public static PointD3D Abs(PointD3D pt)
        {
            return new PointD3D(Math.Abs(pt._X), Math.Abs(pt._Y), Math.Abs(pt._Z));
        }

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量取符号数得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        public static PointD3D Sign(PointD3D pt)
        {
            return new PointD3D(Math.Sign(pt._X), Math.Sign(pt._Y), Math.Sign(pt._Z));
        }

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量舍入到较大的整数值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        public static PointD3D Ceiling(PointD3D pt)
        {
            return new PointD3D(Math.Ceiling(pt._X), Math.Ceiling(pt._Y), Math.Ceiling(pt._Z));
        }

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量舍入到较小的整数值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        public static PointD3D Floor(PointD3D pt)
        {
            return new PointD3D(Math.Floor(pt._X), Math.Floor(pt._Y), Math.Floor(pt._Z));
        }

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量舍入到最接近的整数值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        public static PointD3D Round(PointD3D pt)
        {
            return new PointD3D(Math.Round(pt._X), Math.Round(pt._Y), Math.Round(pt._Z));
        }

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量截断小数部分取整得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        public static PointD3D Truncate(PointD3D pt)
        {
            return new PointD3D(Math.Truncate(pt._X), Math.Truncate(pt._Y), Math.Truncate(pt._Z));
        }

        /// <summary>
        /// 返回将两个 PointD3D 结构的所有分量分别取最大值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD3D 结构，用于比较的第二个结构。</param>
        public static PointD3D Max(PointD3D left, PointD3D right)
        {
            return new PointD3D(Math.Max(left._X, right._X), Math.Max(left._Y, right._Y), Math.Max(left._Z, right._Z));
        }

        /// <summary>
        /// 返回将两个 PointD3D 结构的所有分量分别取最小值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD3D 结构，用于比较的第二个结构。</param>
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
        public static bool operator ==(PointD3D left, PointD3D right)
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

            return (left._X == right._X && left._Y == right._Y && left._Z == right._Z);
        }

        /// <summary>
        /// 判断两个 PointD3D 结构是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD3D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD3D 结构。</param>
        public static bool operator !=(PointD3D left, PointD3D right)
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

            return (left._X != right._X || left._Y != right._Y || left._Z != right._Z);
        }

        /// <summary>
        /// 判断两个 PointD3D 结构表示的向量的模平方是否前者小于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD3D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD3D 结构。</param>
        public static bool operator <(PointD3D left, PointD3D right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return false;
            }

            return (left.ModuleSquared < right.ModuleSquared);
        }

        /// <summary>
        /// 判断两个 PointD3D 结构表示的向量的模平方是否前者大于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD3D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD3D 结构。</param>
        public static bool operator >(PointD3D left, PointD3D right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return false;
            }

            return (left.ModuleSquared > right.ModuleSquared);
        }

        /// <summary>
        /// 判断两个 PointD3D 结构表示的向量的模平方是否前者小于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD3D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD3D 结构。</param>
        public static bool operator <=(PointD3D left, PointD3D right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return true;
            }

            return (left.ModuleSquared <= right.ModuleSquared);
        }

        /// <summary>
        /// 判断两个 PointD3D 结构表示的向量的模平方是否前者大于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD3D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD3D 结构。</param>
        public static bool operator >=(PointD3D left, PointD3D right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return true;
            }

            return (left.ModuleSquared >= right.ModuleSquared);
        }

        //

        /// <summary>
        /// 返回在 PointD3D 结构的所有分量前添加正号得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        public static PointD3D operator +(PointD3D pt)
        {
            return new PointD3D(+pt._X, +pt._Y, +pt._Z);
        }

        /// <summary>
        /// 返回在 PointD3D 结构的所有分量前添加负号得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
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
        public static PointD3D operator +(PointD3D pt, double n)
        {
            return new PointD3D(pt._X + n, pt._Y + n, pt._Z + n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD3D 结构的所有分量相加得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被加数。</param>
        /// <param name="pt">PointD3D 结构，表示加数。</param>
        public static PointD3D operator +(double n, PointD3D pt)
        {
            return new PointD3D(n + pt._X, n + pt._Y, n + pt._Z);
        }

        /// <summary>
        /// 返回将 PointD3D 结构与 PointD3D 结构的所有分量对应相加得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示被加数。</param>
        /// <param name="right">PointD3D 结构，表示加数。</param>
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
        public static PointD3D operator -(PointD3D pt, double n)
        {
            return new PointD3D(pt._X - n, pt._Y - n, pt._Z - n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD3D 结构的所有分量相减得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被减数。</param>
        /// <param name="pt">PointD3D 结构，表示减数。</param>
        public static PointD3D operator -(double n, PointD3D pt)
        {
            return new PointD3D(n - pt._X, n - pt._Y, n - pt._Z);
        }

        /// <summary>
        /// 返回将 PointD3D 结构与 PointD3D 结构的所有分量对应相减得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示被减数。</param>
        /// <param name="right">PointD3D 结构，表示减数。</param>
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
        public static PointD3D operator *(PointD3D pt, double n)
        {
            return new PointD3D(pt._X * n, pt._Y * n, pt._Z * n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD3D 结构的所有分量相乘得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被乘数。</param>
        /// <param name="pt">PointD3D 结构，表示乘数。</param>
        public static PointD3D operator *(double n, PointD3D pt)
        {
            return new PointD3D(n * pt._X, n * pt._Y, n * pt._Z);
        }

        /// <summary>
        /// 返回将 PointD3D 结构与 PointD3D 结构的所有分量对应相乘得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示被乘数。</param>
        /// <param name="right">PointD3D 结构，表示乘数。</param>
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
        public static PointD3D operator /(PointD3D pt, double n)
        {
            return new PointD3D(pt._X / n, pt._Y / n, pt._Z / n);
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD3D 结构的所有分量相除得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被除数。</param>
        /// <param name="pt">PointD3D 结构，表示除数。</param>
        public static PointD3D operator /(double n, PointD3D pt)
        {
            return new PointD3D(n / pt._X, n / pt._Y, n / pt._Z);
        }

        /// <summary>
        /// 返回将 PointD3D 结构与 PointD3D 结构的所有分量对应相除得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示被除数。</param>
        /// <param name="right">PointD3D 结构，表示除数。</param>
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
            this = default(PointD3D);
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
            this = default(PointD3D);
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