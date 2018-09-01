﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.PointD5D
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
    /// 以一组有序的双精度浮点数表示的五维直角坐标系坐标。
    /// </summary>
    public struct PointD5D
    {
        #region 私有与内部成员

        private double _X; // X 坐标。
        private double _Y; // Y 坐标。
        private double _Z; // Z 坐标。
        private double _U; // U 坐标。
        private double _V; // V 坐标。

        #endregion

        #region 常量与只读成员

        /// <summary>
        /// 表示所有属性为其数据类型的默认值的 PointD5D 结构的实例。
        /// </summary>
        public static readonly PointD5D Empty = default(PointD5D);

        /// <summary>
        /// 表示所有属性为非数字的 PointD5D 结构的实例。
        /// </summary>
        public static readonly PointD5D NaN = new PointD5D(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);

        //

        /// <summary>
        /// 表示零向量的 PointD5D 结构的实例。
        /// </summary>
        public static readonly PointD5D Zero = new PointD5D(0, 0, 0, 0, 0);

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

        #region 构造函数

        /// <summary>
        /// 使用双精度浮点数表示的 X 坐标、Y 坐标、Z 坐标与 U 坐标初始化 PointD5D 结构的新实例。
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

        #region 属性

        /// <summary>
        /// 获取或设置此 PointD5D 结构在指定索引的坐标轴的分量。
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
                    case 3: _U = value; break;
                    case 4: _V = value; break;
                }
            }
        }

        //

        /// <summary>
        /// 获取表示此 PointD5D 结构是否为 Empty 的布尔值。
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (_X == Empty._X && _Y == Empty._Y && _Z == Empty._Z && _U == Empty._U && _V == Empty._V);
            }
        }

        /// <summary>
        /// 获取表示此 PointD5D 结构是否为 NaN 的布尔值。
        /// </summary>
        public bool IsNaN
        {
            get
            {
                return (double.IsNaN(_X) || double.IsNaN(_Y) || double.IsNaN(_Z) || double.IsNaN(_U) || double.IsNaN(_V));
            }
        }

        /// <summary>
        /// 获取表示此 PointD5D 结构是否为 Infinity 的布尔值。
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return ((!double.IsNaN(_X) && !double.IsNaN(_Y) && !double.IsNaN(_Z) && !double.IsNaN(_U) && !double.IsNaN(_V)) && (double.IsInfinity(_X) || double.IsInfinity(_Y) || double.IsInfinity(_Z) || double.IsInfinity(_U) || double.IsInfinity(_V)));
            }
        }

        /// <summary>
        /// 获取表示此 PointD5D 结构是否为 NaN 或 Infinity 的布尔值。
        /// </summary>
        public bool IsNaNOrInfinity
        {
            get
            {
                return ((double.IsNaN(_X) || double.IsNaN(_Y) || double.IsNaN(_Z) || double.IsNaN(_U) || double.IsNaN(_V)) || (double.IsInfinity(_X) || double.IsInfinity(_Y) || double.IsInfinity(_Z) || double.IsInfinity(_U) || double.IsInfinity(_V)));
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
        /// 获取此 PointD5D 结构表示的向量与 X 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleX
        {
            get
            {
                return AngleFrom(_X >= 0 ? Ex : -Ex);
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构表示的向量与 Y 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleY
        {
            get
            {
                return AngleFrom(_Y >= 0 ? Ey : -Ey);
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构表示的向量与 Z 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleZ
        {
            get
            {
                return AngleFrom(_Z >= 0 ? Ez : -Ez);
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构表示的向量与 U 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleU
        {
            get
            {
                return AngleFrom(_U >= 0 ? Eu : -Eu);
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构表示的向量与 V 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleV
        {
            get
            {
                return AngleFrom(_V >= 0 ? Ev : -Ev);
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构表示的向量与 XYZU 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleXYZU
        {
            get
            {
                return (Math.PI / 2 - AngleV);
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构表示的向量与 YZUV 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleYZUV
        {
            get
            {
                return (Math.PI / 2 - AngleX);
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构表示的向量与 ZUVX 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleZUVX
        {
            get
            {
                return (Math.PI / 2 - AngleY);
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构表示的向量与 UVXY 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleUVXY
        {
            get
            {
                return (Math.PI / 2 - AngleZ);
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构表示的向量与 VXYZ 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleVXYZ
        {
            get
            {
                return (Math.PI / 2 - AngleU);
            }
        }

        //

        /// <summary>
        /// 获取此 PointD5D 结构表示的向量的模。
        /// </summary>
        public double VectorModule
        {
            get
            {
                return Math.Sqrt(_X * _X + _Y * _Y + _Z * _Z + _U * _U + _V * _V);
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构表示的向量的模平方。
        /// </summary>
        public double VectorModuleSquared
        {
            get
            {
                return (_X * _X + _Y * _Y + _Z * _Z + _U * _U + _V * _V);
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构表示的向量与 +X 轴之间的夹角（弧度）（以 +X 轴为 0 弧度，远离 +X 轴的方向为正方向）。
        /// </summary>
        public double VectorAngleX
        {
            get
            {
                double _YZUV = Math.Sqrt(_Y * _Y + _Z * _Z + _U * _U + _V * _V);

                if (_X == 0 && _YZUV == 0)
                {
                    return 0;
                }
                else
                {
                    double Angle = Math.Atan(_YZUV / _X);

                    if (_X < 0)
                    {
                        return (Angle + Math.PI);
                    }
                    else
                    {
                        return Angle;
                    }
                }
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构表示的向量与 +Y 轴之间的夹角（弧度）（以 +Y 轴为 0 弧度，远离 +Y 轴的方向为正方向）。
        /// </summary>
        public double VectorAngleY
        {
            get
            {
                double _ZUV = Math.Sqrt(_Z * _Z + _U * _U + _V * _V);

                if (_Y == 0 && _ZUV == 0)
                {
                    return 0;
                }
                else
                {
                    double Angle = Math.Atan(_ZUV / _Y);

                    if (_Y < 0)
                    {
                        return (Angle + Math.PI);
                    }
                    else
                    {
                        return Angle;
                    }
                }
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构表示的向量与 +Z 轴之间的夹角（弧度）（以 +Z 轴为 0 弧度，远离 +Z 轴的方向为正方向）。
        /// </summary>
        public double VectorAngleZ
        {
            get
            {
                double _UV = Math.Sqrt(_U * _U + _V * _V);

                if (_Z == 0 && _UV == 0)
                {
                    return 0;
                }
                else
                {
                    double Angle = Math.Atan(_UV / _Z);

                    if (_Z < 0)
                    {
                        return (Angle + Math.PI);
                    }
                    else
                    {
                        return Angle;
                    }
                }
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构表示的向量在 UV 平面内的投影与 +U 轴之间的夹角（弧度）（以 +U 轴为 0 弧度，从 +U 轴指向 +V 轴的方向为正方向）。
        /// </summary>
        public double VectorAngleUV
        {
            get
            {
                if (_U == 0 && _V == 0)
                {
                    return 0;
                }
                else
                {
                    double Angle = Math.Atan(_V / _U);

                    if (_U < 0)
                    {
                        return (Angle + Math.PI);
                    }
                    else if (_V < 0)
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
        /// 获取此 PointD5D 结构表示的向量的相反向量。
        /// </summary>
        public PointD5D VectorNegate
        {
            get
            {
                return new PointD5D(-_X, -_Y, -_Z, -_U, -_V);
            }
        }

        /// <summary>
        /// 获取此 PointD5D 结构表示的向量的规范化向量。
        /// </summary>
        public PointD5D VectorNormalize
        {
            get
            {
                double Mod = VectorModule;

                if (Mod > 0)
                {
                    return new PointD5D(_X / Mod, _Y / Mod, _Z / Mod, _U / Mod, _V / Mod);
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
        /// 判断此 PointD5D 结构是否与指定的 PointD5D 结构相等。
        /// </summary>
        /// <param name="pt">用于比较的 PointD5D 结构。</param>
        public bool Equals(PointD5D pt)
        {
            if ((object)pt == null)
            {
                return false;
            }

            return (_X.Equals(pt._X) && _Y.Equals(pt._Y) && _Z.Equals(pt._Z) && _U.Equals(pt._U) && _V.Equals(pt._V));
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的所有坐标偏移量将此 PointD5D 结构平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的所有坐标偏移量。</param>
        public void Offset(double d)
        {
            _X += d;
            _Y += d;
            _Z += d;
            _U += d;
            _V += d;
        }

        /// <summary>
        /// 按双精度浮点数表示的 X 坐标偏移量、Y 坐标偏移量、Z 坐标偏移量、U 坐标偏移量与 V 坐标偏移量将此 PointD5D 结构平移指定的量。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标偏移量。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标偏移量。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标偏移量。</param>
        /// <param name="du">双精度浮点数表示的 U 坐标偏移量。</param>
        /// <param name="dv">双精度浮点数表示的 V 坐标偏移量。</param>
        public void Offset(double dx, double dy, double dz, double du, double dv)
        {
            _X += dx;
            _Y += dy;
            _Z += dz;
            _U += du;
            _V += dv;
        }

        /// <summary>
        /// 按 PointD5D 结构将此 PointD5D 结构平移指定的量。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于平移此 PointD5D 结构。</param>
        public void Offset(PointD5D pt)
        {
            if ((object)pt != null)
            {
                _X += pt._X;
                _Y += pt._Y;
                _Z += pt._Z;
                _U += pt._U;
                _V += pt._V;
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标偏移量将此 PointD5D 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的所有坐标偏移量。</param>
        public PointD5D OffsetCopy(double d)
        {
            return new PointD5D(_X + d, _Y + d, _Z + d, _U + d, _V + d);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标偏移量、Y 坐标偏移量、Z 坐标偏移量、U 坐标偏移量与 V 坐标偏移量将此 PointD5D 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标偏移量。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标偏移量。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标偏移量。</param>
        /// <param name="du">双精度浮点数表示的 U 坐标偏移量。</param>
        /// <param name="dv">双精度浮点数表示的 V 坐标偏移量。</param>
        public PointD5D OffsetCopy(double dx, double dy, double dz, double du, double dv)
        {
            return new PointD5D(_X + dx, _Y + dy, _Z + dz, _U + du, _V + dv);
        }

        /// <summary>
        /// 返回按 PointD5D 结构将此 PointD5D 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于平移此 PointD5D 结构。</param>
        public PointD5D OffsetCopy(PointD5D pt)
        {
            if ((object)pt != null)
            {
                return new PointD5D(_X + pt._X, _Y + pt._Y, _Z + pt._Z, _U + pt._U, _V + pt._V);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的所有坐标缩放因子将此 PointD5D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的所有坐标缩放因子。</param>
        public void Scale(double s)
        {
            _X *= s;
            _Y *= s;
            _Z *= s;
            _U *= s;
            _V *= s;
        }

        /// <summary>
        /// 按双精度浮点数表示的 X 坐标缩放因子、Y 坐标缩放因子、Z 坐标缩放因子与 U 坐标缩放因子将此 PointD5D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因子。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因子。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因子。</param>
        /// <param name="su">双精度浮点数表示的 U 坐标缩放因子。</param>
        /// <param name="sv">双精度浮点数表示的 V 坐标缩放因子。</param>
        public void Scale(double sx, double sy, double sz, double su, double sv)
        {
            _X *= sx;
            _Y *= sy;
            _Z *= sz;
            _U *= su;
            _V *= sv;
        }

        /// <summary>
        /// 按 PointD5D 结构将此 PointD5D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于缩放此 PointD5D 结构。</param>
        public void Scale(PointD5D pt)
        {
            if ((object)pt != null)
            {
                _X *= pt._X;
                _Y *= pt._Y;
                _Z *= pt._Z;
                _U *= pt._U;
                _V *= pt._V;
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标缩放因子将此 PointD5D 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的所有坐标缩放因子。</param>
        public PointD5D ScaleCopy(double s)
        {
            return new PointD5D(_X * s, _Y * s, _Z * s, _U * s, _V * s);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标缩放因子、Y 坐标缩放因子、Z 坐标缩放因子与 U 坐标缩放因子将此 PointD5D 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因子。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因子。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因子。</param>
        /// <param name="su">双精度浮点数表示的 U 坐标缩放因子。</param>
        /// <param name="sv">双精度浮点数表示的 V 坐标缩放因子。</param>
        public PointD5D ScaleCopy(double sx, double sy, double sz, double su, double sv)
        {
            return new PointD5D(_X * sx, _Y * sy, _Z * sz, _U * su, _V * sv);
        }

        /// <summary>
        /// 返回按 PointD5D 结构将此 PointD5D 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于缩放此 PointD5D 结构。</param>
        public PointD5D ScaleCopy(PointD5D pt)
        {
            if ((object)pt != null)
            {
                return new PointD5D(_X * pt._X, _Y * pt._Y, _Z * pt._Z, _U * pt._U, _V * pt._V);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD5D 结构绕 XY 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 XY 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public void RotateXY(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(0, 1, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
                _U = result[3];
                _V = result[4];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD5D 结构绕 XZ 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 XZ 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Z 轴的方向为正方向）。</param>
        public void RotateXZ(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(0, 2, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
                _U = result[3];
                _V = result[4];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD5D 结构绕 XU 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 XU 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateXU(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(0, 3, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
                _U = result[3];
                _V = result[4];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD5D 结构绕 XV 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 XV 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateXV(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(0, 4, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
                _U = result[3];
                _V = result[4];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD5D 结构绕 YZ 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 YZ 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        public void RotateYZ(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(1, 2, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
                _U = result[3];
                _V = result[4];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD5D 结构绕 YU 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 YU 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateYU(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(1, 3, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
                _U = result[3];
                _V = result[4];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD5D 结构绕 YV 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 YV 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateYV(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(1, 4, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
                _U = result[3];
                _V = result[4];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD5D 结构绕 ZU 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 ZU 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateZU(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(2, 3, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
                _U = result[3];
                _V = result[4];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD5D 结构绕 ZV 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 ZV 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateZV(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(2, 4, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
                _U = result[3];
                _V = result[4];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD5D 结构绕 UV 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 UV 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateUV(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(3, 4, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                _X = result[0];
                _Y = result[1];
                _Z = result[2];
                _U = result[3];
                _V = result[4];
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD5D 结构的副本绕 XY 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 XY 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public PointD5D RotateXYCopy(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(0, 1, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD5D 结构的副本绕 XZ 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 XZ 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Z 轴的方向为正方向）。</param>
        public PointD5D RotateXZCopy(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(0, 2, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD5D 结构的副本绕 XU 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 XU 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +U 轴的方向为正方向）。</param>
        public PointD5D RotateXUCopy(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(0, 3, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD5D 结构的副本绕 XV 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 XV 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +U 轴的方向为正方向）。</param>
        public PointD5D RotateXVCopy(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(0, 4, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD5D 结构的副本绕 YZ 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 YZ 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        public PointD5D RotateYZCopy(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(1, 2, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD5D 结构的副本绕 YU 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 YU 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +U 轴的方向为正方向）。</param>
        public PointD5D RotateYUCopy(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(1, 3, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD5D 结构的副本绕 YV 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 YV 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +U 轴的方向为正方向）。</param>
        public PointD5D RotateYVCopy(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(1, 4, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD5D 结构的副本绕 ZU 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 ZU 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public PointD5D RotateZUCopy(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(2, 3, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD5D 结构的副本绕 ZV 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 ZV 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public PointD5D RotateZVCopy(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(2, 4, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD5D 结构的副本绕 UV 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD5D 结构绕 UV 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public PointD5D RotateUVCopy(double angle)
        {
            Vector result = ToVectorColumn().RotateCopy(3, 4, angle);

            if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
            {
                return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 按 PointD5D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量与偏移向量将此 PointD5D 结构进行仿射变换。
        /// </summary>
        /// <param name="ex">PointD5D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD5D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD5D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD5D 结构表示的 U 基向量。</param>
        /// <param name="ev">PointD5D 结构表示的 V 基向量。</param>
        /// <param name="offset">PointD5D 结构表示的偏移向量。</param>
        public void AffineTransform(PointD5D ex, PointD5D ey, PointD5D ez, PointD5D eu, PointD5D ev, PointD5D offset)
        {
            if ((object)ex != null && (object)ey != null && (object)ez != null && (object)eu != null && (object)offset != null)
            {
                Matrix matrixLeft = new Matrix(new double[6, 6]
                {
                    { ex._X, ex._Y, ex._Z, ex._U, ex._V, 0 },
                    { ey._X, ey._Y, ey._Z, ey._U, ey._V, 0 },
                    { ez._X, ez._Y, ez._Z, ez._U, ez._V, 0 },
                    { eu._X, eu._Y, eu._Z, eu._U, eu._V, 0 },
                    { ev._X, ev._Y, ev._Z, ev._U, ev._V, 0 },
                    { offset._X, offset._Y, offset._Z, offset._U, offset._V, 1 }
                });

                Vector result = ToVectorColumn().AffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
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
        /// 按 Matrix 对象表示的 6x6 仿射矩阵（左矩阵）将此 PointD5D 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 6x6 仿射矩阵（左矩阵）。</param>
        public void AffineTransform(Matrix matrixLeft)
        {
            if (!Matrix.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(6, 6))
            {
                Vector result = ToVectorColumn().AffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
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
                Vector result = ToVectorColumn().AffineTransformCopy(matrixLeftList);

                if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
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
        /// 返回按 PointD5D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量与偏移向量将此 PointD5D 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="ex">PointD5D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD5D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD5D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD5D 结构表示的 U 基向量。</param>
        /// <param name="ev">PointD5D 结构表示的 V 基向量。</param>
        /// <param name="offset">PointD5D 结构表示的偏移向量。</param>
        public PointD5D AffineTransformCopy(PointD5D ex, PointD5D ey, PointD5D ez, PointD5D eu, PointD5D ev, PointD5D offset)
        {
            if ((object)ex != null && (object)ey != null && (object)ez != null && (object)eu != null && (object)offset != null)
            {
                Matrix matrixLeft = new Matrix(new double[6, 6]
                {
                    { ex._X, ex._Y, ex._Z, ex._U, ex._V, 0 },
                    { ey._X, ey._Y, ey._Z, ey._U, ey._V, 0 },
                    { ez._X, ez._Y, ez._Z, ez._U, ez._V, 0 },
                    { eu._X, eu._Y, eu._Z, eu._U, eu._V, 0 },
                    { ev._X, ev._Y, ev._Z, ev._U, ev._V, 0 },
                    { offset._X, offset._Y, offset._Z, offset._U, offset._V, 1 }
                });

                Vector result = ToVectorColumn().AffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
                {
                    return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的 6x6 仿射矩阵（左矩阵）将此 PointD5D 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 6x6 仿射矩阵（左矩阵）。</param>
        public PointD5D AffineTransformCopy(Matrix matrixLeft)
        {
            if (!Matrix.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(6, 6))
            {
                Vector result = ToVectorColumn().AffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
                {
                    return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 6x6 仿射矩阵（左矩阵）列表将此 PointD5D 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 6x6 仿射矩阵（左矩阵）列表。</param>
        public PointD5D AffineTransformCopy(List<Matrix> matrixLeftList)
        {
            if (!InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                Vector result = ToVectorColumn().AffineTransformCopy(matrixLeftList);

                if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
                {
                    return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 按 PointD5D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量与偏移向量将此 PointD5D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="ex">PointD5D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD5D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD5D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD5D 结构表示的 U 基向量。</param>
        /// <param name="ev">PointD5D 结构表示的 V 基向量。</param>
        /// <param name="offset">PointD5D 结构表示的偏移向量。</param>
        public void InverseAffineTransform(PointD5D ex, PointD5D ey, PointD5D ez, PointD5D eu, PointD5D ev, PointD5D offset)
        {
            if ((object)ex != null && (object)ey != null && (object)ez != null && (object)eu != null && (object)offset != null)
            {
                Matrix matrixLeft = new Matrix(new double[6, 6]
                {
                    { ex._X, ex._Y, ex._Z, ex._U, ex._V, 0 },
                    { ey._X, ey._Y, ey._Z, ey._U, ey._V, 0 },
                    { ez._X, ez._Y, ez._Z, ez._U, ez._V, 0 },
                    { eu._X, eu._Y, eu._Z, eu._U, eu._V, 0 },
                    { ev._X, ev._Y, ev._Z, ev._U, ev._V, 0 },
                    { offset._X, offset._Y, offset._Z, offset._U, offset._V, 1 }
                });

                Vector result = ToVectorColumn().InverseAffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
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
        /// 按 Matrix 对象表示的 6x6 仿射矩阵（左矩阵）将此 PointD5D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 6x6 仿射矩阵（左矩阵）。</param>
        public void InverseAffineTransform(Matrix matrixLeft)
        {
            if (!Matrix.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(6, 6))
            {
                Vector result = ToVectorColumn().InverseAffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
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
                Vector result = ToVectorColumn().InverseAffineTransformCopy(matrixLeftList);

                if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
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
        /// 返回按 PointD5D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量与偏移向量将此 PointD5D 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="ex">PointD5D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD5D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD5D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD5D 结构表示的 U 基向量。</param>
        /// <param name="ev">PointD5D 结构表示的 V 基向量。</param>
        /// <param name="offset">PointD5D 结构表示的偏移向量。</param>
        public PointD5D InverseAffineTransformCopy(PointD5D ex, PointD5D ey, PointD5D ez, PointD5D eu, PointD5D ev, PointD5D offset)
        {
            if ((object)ex != null && (object)ey != null && (object)ez != null && (object)eu != null && (object)offset != null)
            {
                Matrix matrixLeft = new Matrix(new double[6, 6]
                {
                    { ex._X, ex._Y, ex._Z, ex._U, ex._V, 0 },
                    { ey._X, ey._Y, ey._Z, ey._U, ey._V, 0 },
                    { ez._X, ez._Y, ez._Z, ez._U, ez._V, 0 },
                    { eu._X, eu._Y, eu._Z, eu._U, eu._V, 0 },
                    { ev._X, ev._Y, ev._Z, ev._U, ev._V, 0 },
                    { offset._X, offset._Y, offset._Z, offset._U, offset._V, 1 }
                });

                Vector result = ToVectorColumn().InverseAffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
                {
                    return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的 6x6 仿射矩阵（左矩阵）将此 PointD5D 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeft">Matrix 对象，表示 6x6 仿射矩阵（左矩阵）。</param>
        public PointD5D InverseAffineTransformCopy(Matrix matrixLeft)
        {
            if (!Matrix.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(6, 6))
            {
                Vector result = ToVectorColumn().InverseAffineTransformCopy(matrixLeft);

                if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
                {
                    return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的 6x6 仿射矩阵（左矩阵）列表将此 PointD5D 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeftList">Matrix 对象列表，表示 6x6 仿射矩阵（左矩阵）列表。</param>
        public PointD5D InverseAffineTransformCopy(List<Matrix> matrixLeftList)
        {
            if (!InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                Vector result = ToVectorColumn().InverseAffineTransformCopy(matrixLeftList);

                if (!Vector.IsNullOrNonVector(result) && result.Dimension == 5)
                {
                    return new PointD5D(result[0], result[1], result[2], result[3], result[4]);
                }
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将此 PointD5D 结构投影至平行于 XYZU 空间的投影空间的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD5D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        public PointD4D ProjectToXYZU(PointD5D prjCenter, double trueLenDist)
        {
            if ((object)prjCenter != null && (!InternalMethod.IsNaNOrInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return XYZU;
                }
                else
                {
                    if (_V != prjCenter._V)
                    {
                        double Scale = trueLenDist / (_V - prjCenter._V);

                        if ((!InternalMethod.IsNaNOrInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * XYZU + (1 - Scale) * prjCenter.XYZU);
                        }
                    }
                }
            }

            return PointD4D.NaN;
        }

        /// <summary>
        /// 返回将此 PointD5D 结构投影至平行于 YZUV 空间的投影空间的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD5D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        public PointD4D ProjectToYZUV(PointD5D prjCenter, double trueLenDist)
        {
            if ((object)prjCenter != null && (!InternalMethod.IsNaNOrInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return YZUV;
                }
                else
                {
                    if (_X != prjCenter._X)
                    {
                        double Scale = trueLenDist / (_X - prjCenter._X);

                        if ((!InternalMethod.IsNaNOrInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * YZUV + (1 - Scale) * prjCenter.YZUV);
                        }
                    }
                }
            }

            return PointD4D.NaN;
        }

        /// <summary>
        /// 返回将此 PointD5D 结构投影至平行于 ZUVX 空间的投影空间的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD5D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        public PointD4D ProjectToZUVX(PointD5D prjCenter, double trueLenDist)
        {
            if ((object)prjCenter != null && (!InternalMethod.IsNaNOrInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return ZUVX;
                }
                else
                {
                    if (_Y != prjCenter._Y)
                    {
                        double Scale = trueLenDist / (_Y - prjCenter._Y);

                        if ((!InternalMethod.IsNaNOrInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * ZUVX + (1 - Scale) * prjCenter.ZUVX);
                        }
                    }
                }
            }

            return PointD4D.NaN;
        }

        /// <summary>
        /// 返回将此 PointD5D 结构投影至平行于 UVXY 空间的投影空间的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD5D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        public PointD4D ProjectToUVXY(PointD5D prjCenter, double trueLenDist)
        {
            if ((object)prjCenter != null && (!InternalMethod.IsNaNOrInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return UVXY;
                }
                else
                {
                    if (_Z != prjCenter._Z)
                    {
                        double Scale = trueLenDist / (_Z - prjCenter._Z);

                        if ((!InternalMethod.IsNaNOrInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * UVXY + (1 - Scale) * prjCenter.UVXY);
                        }
                    }
                }
            }

            return PointD4D.NaN;
        }

        /// <summary>
        /// 返回将此 PointD5D 结构投影至平行于 VXYZ 空间的投影空间的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD5D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        public PointD4D ProjectToVXYZ(PointD5D prjCenter, double trueLenDist)
        {
            if ((object)prjCenter != null && (!InternalMethod.IsNaNOrInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return VXYZ;
                }
                else
                {
                    if (_U != prjCenter._U)
                    {
                        double Scale = trueLenDist / (_U - prjCenter._U);

                        if ((!InternalMethod.IsNaNOrInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * VXYZ + (1 - Scale) * prjCenter.VXYZ);
                        }
                    }
                }
            }

            return PointD4D.NaN;
        }

        //

        /// <summary>
        /// 返回此 PointD5D 结构与指定的 PointD5D 结构之间的距离。
        /// </summary>
        /// <param name="pt">PointD5D 结构，表示起始点。</param>
        public double DistanceFrom(PointD5D pt)
        {
            if ((object)pt != null)
            {
                double dx = _X - pt._X, dy = _Y - pt._Y, dz = _Z - pt._Z, du = _U - pt._U, dv = _V - pt._V;

                return Math.Sqrt(dx * dx + dy * dy + dz * dz + du * du + dv * dv);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回此 PointD5D 结构表示的向量与指定的 PointD5D 结构表示的向量之间的夹角（弧度）。
        /// </summary>
        /// <param name="pt">PointD5D 结构，表示起始向量。</param>
        public double AngleFrom(PointD5D pt)
        {
            if ((object)pt != null)
            {
                if (_X == 0 && _Y == 0 && _Z == 0 && _U == 0 && _V == 0)
                {
                    _X = 1;
                }

                if (pt._X == 0 && pt._Y == 0 && pt._Z == 0 && pt._U == 0 && pt._V == 0)
                {
                    pt._X = 1;
                }

                double DotProduct = _X * pt._X + _Y * pt._Y + _Z * pt._Z + _U * pt._U + _V * pt._V;
                double ModProduct = VectorModule * pt.VectorModule;

                return Math.Acos(DotProduct / ModProduct);
            }

            return double.NaN;
        }

        //

        /// <summary>
        /// 返回将此 PointD5D 结构表示的直角坐标系坐标转换为超球坐标系坐标的新实例。
        /// </summary>
        public PointD5D ToSpherical()
        {
            return new PointD5D(VectorModule, VectorAngleX, VectorAngleY, VectorAngleZ, VectorAngleUV);
        }

        /// <summary>
        /// 返回将此 PointD5D 结构表示的超球坐标系坐标转换为直角坐标系坐标的新实例。
        /// </summary>
        public PointD5D ToCartesian()
        {
            return new PointD5D(_X * Math.Cos(_Y), _X * Math.Sin(_Y) * Math.Cos(_Z), _X * Math.Sin(_Y) * Math.Sin(_Z) * Math.Cos(_U), _X * Math.Sin(_Y) * Math.Sin(_Z) * Math.Sin(_U) * Math.Cos(_V), _X * Math.Sin(_Y) * Math.Sin(_Z) * Math.Sin(_U) * Math.Sin(_V));
        }

        //

        /// <summary>
        /// 返回将此 PointD5D 结构转换为向量（列向量）的 Vector 的新实例。
        /// </summary>
        public Vector ToVector()
        {
            return new Vector(_X, _Y, _Z, _U, _V);
        }

        /// <summary>
        /// 返回将此 PointD5D 结构转换为列向量的 Vector 的新实例。
        /// </summary>
        public Vector ToVectorColumn()
        {
            return new Vector(Vector.Type.ColumnVector, _X, _Y, _Z, _U, _V);
        }

        /// <summary>
        /// 返回将此 PointD5D 结构转换为行向量的 Vector 的新实例。
        /// </summary>
        public Vector ToVectorRow()
        {
            return new Vector(Vector.Type.RowVector, _X, _Y, _Z, _U, _V);
        }

        //

        /// <summary>
        /// 将此 PointD5D 结构转换为双精度浮点数数组。
        /// </summary>
        public double[] ToArray()
        {
            return new double[5] { _X, _Y, _Z, _U, _V };
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断两个 PointD5D 结构是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 PointD5D 结构。</param>
        /// <param name="right">用于比较的第二个 PointD5D 结构。</param>
        public static bool Equals(PointD5D left, PointD5D right)
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
        /// 返回单位矩阵，表示不对 PointD5D 结构进行仿射变换的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        public static Matrix IdentityMatrix()
        {
            return Matrix.Identity(6);
        }

        //

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标偏移量将 PointD5D 结构平移指定的量的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的所有坐标偏移量。</param>
        public static Matrix OffsetMatrix(double d)
        {
            return Vector.OffsetMatrix(Vector.Type.ColumnVector, 5, d);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标偏移量、Y 坐标偏移量与 Z 坐标偏移量将 PointD5D 结构平移指定的量的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标偏移量。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标偏移量。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标偏移量。</param>
        /// <param name="du">双精度浮点数表示的 U 坐标偏移量。</param>
        /// <param name="dv">双精度浮点数表示的 V 坐标偏移量。</param>
        public static Matrix OffsetMatrix(double dx, double dy, double dz, double du, double dv)
        {
            return Vector.OffsetMatrix(new Vector(Vector.Type.ColumnVector, dx, dy, dz, du, dv));
        }

        /// <summary>
        /// 返回按 PointD5D 结构将 PointD5D 结构平移指定的量的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于平移 PointD5D 结构。</param>
        public static Matrix OffsetMatrix(PointD5D pt)
        {
            if ((object)pt != null)
            {
                return Vector.OffsetMatrix(pt.ToVectorColumn());
            }

            return Matrix.NonMatrix;
        }

        //

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标缩放因子将 PointD5D 结构缩放指定的倍数的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的所有坐标缩放因子。</param>
        public static Matrix ScaleMatrix(double s)
        {
            return Vector.ScaleMatrix(Vector.Type.ColumnVector, 5, s);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标缩放因子、Y 坐标缩放因子与 Z 坐标缩放因子将 PointD5D 结构缩放指定的倍数的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因子。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因子。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因子。</param>
        /// <param name="su">双精度浮点数表示的 U 坐标缩放因子。</param>
        /// <param name="sv">双精度浮点数表示的 V 坐标缩放因子。</param>
        public static Matrix ScaleMatrix(double sx, double sy, double sz, double su, double sv)
        {
            return Vector.ScaleMatrix(new Vector(Vector.Type.ColumnVector, sx, sy, sz, su, sv));
        }

        /// <summary>
        /// 返回按 PointD5D 结构将 PointD5D 结构缩放指定的倍数的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于缩放 PointD5D 结构。</param>
        public static Matrix ScaleMatrix(PointD5D pt)
        {
            if ((object)pt != null)
            {
                return Vector.ScaleMatrix(pt.ToVectorColumn());
            }

            return Matrix.NonMatrix;
        }

        //

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD5D 结构绕 XY 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD5D 结构绕 XY 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public static Matrix RotateXYMatrix(double angle)
        {
            return Vector.RotateMatrix(Vector.Type.ColumnVector, 5, 0, 1, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD5D 结构绕 XZ 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD5D 结构绕 XZ 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Z 轴的方向为正方向）。</param>
        public static Matrix RotateXZMatrix(double angle)
        {
            return Vector.RotateMatrix(Vector.Type.ColumnVector, 5, 0, 2, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD5D 结构绕 XU 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD5D 结构绕 XU 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +U 轴的方向为正方向）。</param>
        public static Matrix RotateXUMatrix(double angle)
        {
            return Vector.RotateMatrix(Vector.Type.ColumnVector, 5, 0, 3, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD5D 结构绕 XV 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD5D 结构绕 XV 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +V 轴的方向为正方向）。</param>
        public static Matrix RotateXVMatrix(double angle)
        {
            return Vector.RotateMatrix(Vector.Type.ColumnVector, 5, 0, 4, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD5D 结构绕 YZ 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD5D 结构绕 YZ 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        public static Matrix RotateYZMatrix(double angle)
        {
            return Vector.RotateMatrix(Vector.Type.ColumnVector, 5, 1, 2, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD5D 结构绕 YU 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD5D 结构绕 YU 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +U 轴的方向为正方向）。</param>
        public static Matrix RotateYUMatrix(double angle)
        {
            return Vector.RotateMatrix(Vector.Type.ColumnVector, 5, 1, 3, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD5D 结构绕 YV 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD5D 结构绕 YV 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +V 轴的方向为正方向）。</param>
        public static Matrix RotateYVMatrix(double angle)
        {
            return Vector.RotateMatrix(Vector.Type.ColumnVector, 5, 1, 4, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD5D 结构绕 ZU 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD5D 结构绕 ZU 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public static Matrix RotateZUMatrix(double angle)
        {
            return Vector.RotateMatrix(Vector.Type.ColumnVector, 5, 2, 3, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD5D 结构绕 ZV 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD5D 结构绕 ZV 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +V 轴的方向为正方向）。</param>
        public static Matrix RotateZVMatrix(double angle)
        {
            return Vector.RotateMatrix(Vector.Type.ColumnVector, 5, 2, 4, angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD5D 结构绕 UV 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）的 Matrix 的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD5D 结构绕 UV 平面的法向空间旋转的角度（弧度）（以 +U 轴为 0 弧度，从 +U 轴指向 +V 轴的方向为正方向）。</param>
        public static Matrix RotateUVMatrix(double angle)
        {
            return Vector.RotateMatrix(Vector.Type.ColumnVector, 5, 3, 4, angle);
        }

        //

        /// <summary>
        /// 返回两个 PointD5D 结构之间的距离。
        /// </summary>
        /// <param name="left">PointD5D 结构，表示第一个点。</param>
        /// <param name="right">PointD5D 结构，表示第二个点。</param>
        public static double DistanceBetween(PointD5D left, PointD5D right)
        {
            if ((object)left != null && (object)right != null)
            {
                double dx = left._X - right._X, dy = left._Y - right._Y, dz = left._Z - right._Z, du = left._U - right._U, dv = left._V - right._V;

                return Math.Sqrt(dx * dx + dy * dy + dz * dz + du * du + dv * dv);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回 PointD5D 结构表示的两个向量之间的夹角（弧度）。
        /// </summary>
        /// <param name="left">PointD5D 结构，表示第一个向量。</param>
        /// <param name="right">PointD5D 结构，表示第二个向量。</param>
        public static double AngleBetween(PointD5D left, PointD5D right)
        {
            if ((object)left != null && (object)right != null)
            {
                if (left._X == 0 && left._Y == 0 && left._Z == 0 && left._U == 0 && left._V == 0)
                {
                    left._X = 1;
                }

                if (right._X == 0 && right._Y == 0 && right._Z == 0 && right._U == 0 && right._V == 0)
                {
                    right._X = 1;
                }

                double DotProduct = left._X * right._X + left._Y * right._Y + left._Z * right._Z + left._U * right._U + left._V * right._V;
                double ModProduct = left.VectorModule * right.VectorModule;

                return Math.Acos(DotProduct / ModProduct);
            }

            return double.NaN;
        }

        //

        /// <summary>
        /// 返回 PointD5D 结构表示的两个向量的数量积。
        /// </summary>
        /// <param name="left">PointD5D 结构，表示第一个向量。</param>
        /// <param name="right">PointD5D 结构，表示第二个向量。</param>
        public static double DotProduct(PointD5D left, PointD5D right)
        {
            if ((object)left != null && (object)right != null)
            {
                return (left._X * right._X + left._Y * right._Y + left._Z * right._Z + left._U * right._U + left._V * right._V);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回 PointD5D 结构表示的两个向量的向量积，该向量积为一个十维向量，其所有分量的数值依次为 X∧Y 基向量、X∧Z 基向量、X∧U 基向量、X∧V 基向量、Y∧Z 基向量、Y∧U 基向量、Y∧V 基向量、Z∧U 基向量、Z∧V 基向量与 U∧V 基向量的系数。
        /// </summary>
        /// <param name="left">PointD5D 结构，表示左向量。</param>
        /// <param name="right">PointD5D 结构，表示右向量。</param>
        public static Vector CrossProduct(PointD5D left, PointD5D right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new Vector(Vector.Type.ColumnVector, left._X * right._Y - left._Y * right._X, left._X * right._Z - left._Z * right._X, left._X * right._U - left._U * right._X, left._X * right._V - left._V * right._X, left._Y * right._Z - left._Z * right._Y, left._Y * right._U - left._U * right._Y, left._Y * right._V - left._V * right._Y, left._Z * right._U - left._U * right._Z, left._Z * right._V - left._V * right._Z, left._U * right._V - left._V * right._U);
            }

            return Vector.NonVector;
        }

        //

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量取绝对值得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于转换的结构。</param>
        public static PointD5D Abs(PointD5D pt)
        {
            if ((object)pt != null)
            {
                return new PointD5D(Math.Abs(pt._X), Math.Abs(pt._Y), Math.Abs(pt._Z), Math.Abs(pt._U), Math.Abs(pt._V));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量取符号数得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于转换的结构。</param>
        public static PointD5D Sign(PointD5D pt)
        {
            if ((object)pt != null)
            {
                return new PointD5D(Math.Sign(pt._X), Math.Sign(pt._Y), Math.Sign(pt._Z), Math.Sign(pt._U), Math.Sign(pt._V));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量舍入到较大的整数值得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于转换的结构。</param>
        public static PointD5D Ceiling(PointD5D pt)
        {
            if ((object)pt != null)
            {
                return new PointD5D(Math.Ceiling(pt._X), Math.Ceiling(pt._Y), Math.Ceiling(pt._Z), Math.Ceiling(pt._U), Math.Ceiling(pt._V));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量舍入到较小的整数值得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于转换的结构。</param>
        public static PointD5D Floor(PointD5D pt)
        {
            if ((object)pt != null)
            {
                return new PointD5D(Math.Floor(pt._X), Math.Floor(pt._Y), Math.Floor(pt._Z), Math.Floor(pt._U), Math.Floor(pt._V));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量舍入到最接近的整数值得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于转换的结构。</param>
        public static PointD5D Round(PointD5D pt)
        {
            if ((object)pt != null)
            {
                return new PointD5D(Math.Round(pt._X), Math.Round(pt._Y), Math.Round(pt._Z), Math.Round(pt._U), Math.Round(pt._V));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量截断小数部分取整得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于转换的结构。</param>
        public static PointD5D Truncate(PointD5D pt)
        {
            if ((object)pt != null)
            {
                return new PointD5D(Math.Truncate(pt._X), Math.Truncate(pt._Y), Math.Truncate(pt._Z), Math.Truncate(pt._U), Math.Truncate(pt._V));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将两个 PointD5D 结构的所有分量分别取最大值得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD5D 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD5D 结构，用于比较的第二个结构。</param>
        public static PointD5D Max(PointD5D left, PointD5D right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD5D(Math.Max(left._X, right._X), Math.Max(left._Y, right._Y), Math.Max(left._Z, right._Z), Math.Max(left._U, right._U), Math.Max(left._V, right._V));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将两个 PointD5D 结构的所有分量分别取最小值得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD5D 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD5D 结构，用于比较的第二个结构。</param>
        public static PointD5D Min(PointD5D left, PointD5D right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD5D(Math.Min(left._X, right._X), Math.Min(left._Y, right._Y), Math.Min(left._Z, right._Z), Math.Min(left._U, right._U), Math.Min(left._V, right._V));
            }

            return NaN;
        }

        #endregion

        #region 基类方法

        /// <summary>
        /// 判断此 PointD5D 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is PointD5D))
            {
                return false;
            }

            return Equals((PointD5D)obj);
        }

        /// <summary>
        /// 返回此 PointD5D 结构的哈希代码。
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 PointD5D 结构转换为字符串。
        /// </summary>
        public override string ToString()
        {
            return string.Concat("{X=", _X, ", Y=", _Y, ", Z=", _Z, ", U=", _U, ", V=", _V, "}");
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 PointD5D 结构是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD5D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD5D 结构。</param>
        public static bool operator ==(PointD5D left, PointD5D right)
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

            return (left._X == right._X && left._Y == right._Y && left._Z == right._Z && left._U == right._U && left._V == right._V);
        }

        /// <summary>
        /// 判断两个 PointD5D 结构是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD5D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD5D 结构。</param>
        public static bool operator !=(PointD5D left, PointD5D right)
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

            return (left._X != right._X || left._Y != right._Y || left._Z != right._Z || left._U != right._U || left._V != right._V);
        }

        /// <summary>
        /// 判断两个 PointD5D 结构表示的向量的模平方是否前者小于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD5D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD5D 结构。</param>
        public static bool operator <(PointD5D left, PointD5D right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared < right.VectorModuleSquared);
        }

        /// <summary>
        /// 判断两个 PointD5D 结构表示的向量的模平方是否前者大于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD5D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD5D 结构。</param>
        public static bool operator >(PointD5D left, PointD5D right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared > right.VectorModuleSquared);
        }

        /// <summary>
        /// 判断两个 PointD5D 结构表示的向量的模平方是否前者小于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD5D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD5D 结构。</param>
        public static bool operator <=(PointD5D left, PointD5D right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared <= right.VectorModuleSquared);
        }

        /// <summary>
        /// 判断两个 PointD5D 结构表示的向量的模平方是否前者大于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD5D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD5D 结构。</param>
        public static bool operator >=(PointD5D left, PointD5D right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared >= right.VectorModuleSquared);
        }

        //

        /// <summary>
        /// 返回在 PointD5D 结构的所有分量前添加正号得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于转换的结构。</param>
        public static PointD5D operator +(PointD5D pt)
        {
            if ((object)pt != null)
            {
                return new PointD5D(+pt._X, +pt._Y, +pt._Z, +pt._U, +pt._V);
            }

            return NaN;
        }

        /// <summary>
        /// 返回在 PointD5D 结构的所有分量前添加负号得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，用于转换的结构。</param>
        public static PointD5D operator -(PointD5D pt)
        {
            if ((object)pt != null)
            {
                return new PointD5D(-pt._X, -pt._Y, -pt._Z, -pt._U, -pt._V);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量与双精度浮点数相加得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，表示被加数。</param>
        /// <param name="n">双精度浮点数，表示加数。</param>
        public static PointD5D operator +(PointD5D pt, double n)
        {
            if ((object)pt != null)
            {
                return new PointD5D(pt._X + n, pt._Y + n, pt._Z + n, pt._U + n, pt._V + n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD5D 结构的所有分量相加得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被加数。</param>
        /// <param name="pt">PointD5D 结构，表示加数。</param>
        public static PointD5D operator +(double n, PointD5D pt)
        {
            if ((object)pt != null)
            {
                return new PointD5D(n + pt._X, n + pt._Y, n + pt._Z, n + pt._U, n + pt._V);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD5D 结构与 PointD5D 结构的所有分量对应相加得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD5D 结构，表示被加数。</param>
        /// <param name="right">PointD5D 结构，表示加数。</param>
        public static PointD5D operator +(PointD5D left, PointD5D right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD5D(left._X + right._X, left._Y + right._Y, left._Z + right._Z, left._U + right._U, left._V + right._V);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量与双精度浮点数相减得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，表示被减数。</param>
        /// <param name="n">双精度浮点数，表示减数。</param>
        public static PointD5D operator -(PointD5D pt, double n)
        {
            if ((object)pt != null)
            {
                return new PointD5D(pt._X - n, pt._Y - n, pt._Z - n, pt._U - n, pt._V - n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD5D 结构的所有分量相减得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被减数。</param>
        /// <param name="pt">PointD5D 结构，表示减数。</param>
        public static PointD5D operator -(double n, PointD5D pt)
        {
            if ((object)pt != null)
            {
                return new PointD5D(n - pt._X, n - pt._Y, n - pt._Z, n - pt._U, n - pt._V);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD5D 结构与 PointD5D 结构的所有分量对应相减得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD5D 结构，表示被减数。</param>
        /// <param name="right">PointD5D 结构，表示减数。</param>
        public static PointD5D operator -(PointD5D left, PointD5D right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD5D(left._X - right._X, left._Y - right._Y, left._Z - right._Z, left._U - right._U, left._V - right._V);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量与双精度浮点数相乘得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，表示被乘数。</param>
        /// <param name="n">双精度浮点数，表示乘数。</param>
        public static PointD5D operator *(PointD5D pt, double n)
        {
            if ((object)pt != null)
            {
                return new PointD5D(pt._X * n, pt._Y * n, pt._Z * n, pt._U * n, pt._V * n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD5D 结构的所有分量相乘得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被乘数。</param>
        /// <param name="pt">PointD5D 结构，表示乘数。</param>
        public static PointD5D operator *(double n, PointD5D pt)
        {
            if ((object)pt != null)
            {
                return new PointD5D(n * pt._X, n * pt._Y, n * pt._Z, n * pt._U, n * pt._V);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD5D 结构与 PointD5D 结构的所有分量对应相乘得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD5D 结构，表示被乘数。</param>
        /// <param name="right">PointD5D 结构，表示乘数。</param>
        public static PointD5D operator *(PointD5D left, PointD5D right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD5D(left._X * right._X, left._Y * right._Y, left._Z * right._Z, left._U * right._U, left._V * right._V);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD5D 结构的所有分量与双精度浮点数相除得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD5D 结构，表示被除数。</param>
        /// <param name="n">双精度浮点数，表示除数。</param>
        public static PointD5D operator /(PointD5D pt, double n)
        {
            if ((object)pt != null)
            {
                return new PointD5D(pt._X / n, pt._Y / n, pt._Z / n, pt._U / n, pt._V / n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD5D 结构的所有分量相除得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被除数。</param>
        /// <param name="pt">PointD5D 结构，表示除数。</param>
        public static PointD5D operator /(double n, PointD5D pt)
        {
            if ((object)pt != null)
            {
                return new PointD5D(n / pt._X, n / pt._Y, n / pt._Z, n / pt._U, n / pt._V);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD5D 结构与 PointD5D 结构的所有分量对应相除得到的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD5D 结构，表示被除数。</param>
        /// <param name="right">PointD5D 结构，表示除数。</param>
        public static PointD5D operator /(PointD5D left, PointD5D right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD5D(left._X / right._X, left._Y / right._Y, left._Z / right._Z, left._U / right._U, left._V / right._V);
            }

            return NaN;
        }

        #endregion
    }
}