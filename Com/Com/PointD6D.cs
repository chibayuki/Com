/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.PointD6D
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
    /// 以一组有序的双精度浮点数表示的六维直角坐标系坐标。
    /// </summary>
    public struct PointD6D
    {
        #region 私有与内部成员

        private double _X; // X 坐标。
        private double _Y; // Y 坐标。
        private double _Z; // Z 坐标。
        private double _U; // U 坐标。
        private double _V; // V 坐标。
        private double _W; // W 坐标。

        //

        private Matrix2D _ToMatrixForAffineTransform() // 获取此 PointD6D 结构用于仿射变换的矩阵。
        {
            return new Matrix2D(new double[1, 7]
            {
                { _X, _Y, _Z, _U, _V, _W, 1 }
            });
        }

        #endregion

        #region 常量与只读成员

        /// <summary>
        /// 表示所有属性为其数据类型的默认值的 PointD6D 结构的实例。
        /// </summary>
        public static readonly PointD6D Empty = default(PointD6D);

        /// <summary>
        /// 表示所有属性为非数字的 PointD6D 结构的实例。
        /// </summary>
        public static readonly PointD6D NaN = new PointD6D(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);

        //

        /// <summary>
        /// 表示零向量的 PointD6D 结构的实例。
        /// </summary>
        public static readonly PointD6D Zero = new PointD6D(0, 0, 0, 0, 0, 0);

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

        #region 构造函数

        /// <summary>
        /// 使用双精度浮点数表示的 X 坐标、Y 坐标、Z 坐标与 U 坐标初始化 PointD6D 结构的新实例。
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

        #region 属性

        /// <summary>
        /// 获取或设置此 PointD6D 结构在指定索引的坐标轴的分量。
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
                    case 5: _W = value; break;
                }
            }
        }

        //

        /// <summary>
        /// 获取表示此 PointD6D 结构是否为 Empty 的布尔值。
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (_X == Empty._X && _Y == Empty._Y && _Z == Empty._Z && _U == Empty._U && _V == Empty._V && _W == Empty._W);
            }
        }

        /// <summary>
        /// 获取表示此 PointD6D 结构是否为 NaN 的布尔值。
        /// </summary>
        public bool IsNaN
        {
            get
            {
                return (double.IsNaN(_X) || double.IsNaN(_Y) || double.IsNaN(_Z) || double.IsNaN(_U) || double.IsNaN(_V) || double.IsNaN(_W));
            }
        }

        /// <summary>
        /// 获取表示此 PointD6D 结构是否为 Infinity 的布尔值。
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return ((!double.IsNaN(_X) && !double.IsNaN(_Y) && !double.IsNaN(_Z) && !double.IsNaN(_U) && !double.IsNaN(_V) && !double.IsNaN(_W)) && (double.IsInfinity(_X) || double.IsInfinity(_Y) || double.IsInfinity(_Z) || double.IsInfinity(_U) || double.IsInfinity(_V) || double.IsInfinity(_W)));
            }
        }

        /// <summary>
        /// 获取表示此 PointD6D 结构是否为 NaN 或 Infinity 的布尔值。
        /// </summary>
        public bool IsNaNOrInfinity
        {
            get
            {
                return ((double.IsNaN(_X) || double.IsNaN(_Y) || double.IsNaN(_Z) || double.IsNaN(_U) || double.IsNaN(_V) || double.IsNaN(_W)) || (double.IsInfinity(_X) || double.IsInfinity(_Y) || double.IsInfinity(_Z) || double.IsInfinity(_U) || double.IsInfinity(_V) || double.IsInfinity(_W)));
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
        /// 获取此 PointD6D 结构表示的向量与 X 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleX
        {
            get
            {
                return AngleFrom(_X >= 0 ? Ex : -Ex);
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构表示的向量与 Y 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleY
        {
            get
            {
                return AngleFrom(_Y >= 0 ? Ey : -Ey);
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构表示的向量与 Z 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleZ
        {
            get
            {
                return AngleFrom(_Z >= 0 ? Ez : -Ez);
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构表示的向量与 U 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleU
        {
            get
            {
                return AngleFrom(_U >= 0 ? Eu : -Eu);
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构表示的向量与 V 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleV
        {
            get
            {
                return AngleFrom(_V >= 0 ? Ev : -Ev);
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构表示的向量与 W 轴之间的夹角（弧度）。
        /// </summary>
        public double AngleW
        {
            get
            {
                return AngleFrom(_W >= 0 ? Ew : -Ew);
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构表示的向量与 XYZUV 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleXYZUV
        {
            get
            {
                return (Math.PI / 2 - AngleW);
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构表示的向量与 YZUVW 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleYZUVW
        {
            get
            {
                return (Math.PI / 2 - AngleX);
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构表示的向量与 ZUVWX 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleZUVWX
        {
            get
            {
                return (Math.PI / 2 - AngleY);
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构表示的向量与 UVWXY 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleUVWXY
        {
            get
            {
                return (Math.PI / 2 - AngleZ);
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构表示的向量与 VWXYZ 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleVWXYZ
        {
            get
            {
                return (Math.PI / 2 - AngleU);
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构表示的向量与 WXYZU 空间之间的夹角（弧度）。
        /// </summary>
        public double AngleWXYZU
        {
            get
            {
                return (Math.PI / 2 - AngleV);
            }
        }

        //

        /// <summary>
        /// 获取此 PointD6D 结构表示的向量的模。
        /// </summary>
        public double VectorModule
        {
            get
            {
                return Math.Sqrt(_X * _X + _Y * _Y + _Z * _Z + _U * _U + _V * _V + _W * _W);
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构表示的向量的模平方。
        /// </summary>
        public double VectorModuleSquared
        {
            get
            {
                return (_X * _X + _Y * _Y + _Z * _Z + _U * _U + _V * _V + _W * _W);
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构表示的向量与 +X 轴之间的夹角（弧度）（以 +X 轴为 0 弧度，远离 +X 轴的方向为正方向）。
        /// </summary>
        public double VectorAngleX
        {
            get
            {
                double _YZUVW = Math.Sqrt(_Y * _Y + _Z * _Z + _U * _U + _V * _V + _W * _W);

                if (_X == 0 && _YZUVW == 0)
                {
                    return 0;
                }
                else
                {
                    double Angle = Math.Atan(_YZUVW / _X);

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
        /// 获取此 PointD6D 结构表示的向量与 +Y 轴之间的夹角（弧度）（以 +Y 轴为 0 弧度，远离 +Y 轴的方向为正方向）。
        /// </summary>
        public double VectorAngleY
        {
            get
            {
                double _ZUVW = Math.Sqrt(_Z * _Z + _U * _U + _V * _V + _W * _W);

                if (_Y == 0 && _ZUVW == 0)
                {
                    return 0;
                }
                else
                {
                    double Angle = Math.Atan(_ZUVW / _Y);

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
        /// 获取此 PointD6D 结构表示的向量与 +Z 轴之间的夹角（弧度）（以 +Z 轴为 0 弧度，远离 +Z 轴的方向为正方向）。
        /// </summary>
        public double VectorAngleZ
        {
            get
            {
                double _UVW = Math.Sqrt(_U * _U + _V * _V + _W * _W);

                if (_Z == 0 && _UVW == 0)
                {
                    return 0;
                }
                else
                {
                    double Angle = Math.Atan(_UVW / _Z);

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
        /// 获取此 PointD6D 结构表示的向量与 +U 轴之间的夹角（弧度）（以 +U 轴为 0 弧度，远离 +U 轴的方向为正方向）。
        /// </summary>
        public double VectorAngleU
        {
            get
            {
                double _VW = Math.Sqrt(_V * _V + _W * _W);

                if (_U == 0 && _VW == 0)
                {
                    return 0;
                }
                else
                {
                    double Angle = Math.Atan(_VW / _U);

                    if (_U < 0)
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
        /// 获取此 PointD6D 结构表示的向量在 VW 平面内的投影与 +V 轴之间的夹角（弧度）（以 +V 轴为 0 弧度，从 +V 轴指向 +W 轴的方向为正方向）。
        /// </summary>
        public double VectorAngleVW
        {
            get
            {
                if (_V == 0 && _W == 0)
                {
                    return 0;
                }
                else
                {
                    double Angle = Math.Atan(_W / _V);

                    if (_V < 0)
                    {
                        return (Angle + Math.PI);
                    }
                    else if (_W < 0)
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
        /// 获取此 PointD6D 结构表示的向量的相反向量。
        /// </summary>
        public PointD6D VectorNegate
        {
            get
            {
                return new PointD6D(-_X, -_Y, -_Z, -_U, -_V, -_W);
            }
        }

        /// <summary>
        /// 获取此 PointD6D 结构表示的向量的规范化向量。
        /// </summary>
        public PointD6D VectorNormalize
        {
            get
            {
                double Mod = VectorModule;

                if (Mod > 0)
                {
                    return new PointD6D(_X / Mod, _Y / Mod, _Z / Mod, _U / Mod, _V / Mod, _W / Mod);
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
        /// 判断此 PointD6D 结构是否与指定的 PointD6D 结构相等。
        /// </summary>
        /// <param name="pt">用于比较的 PointD6D 结构。</param>
        public bool Equals(PointD6D pt)
        {
            if ((object)pt == null)
            {
                return false;
            }

            return (_X.Equals(pt._X) && _Y.Equals(pt._Y) && _Z.Equals(pt._Z) && _U.Equals(pt._U) && _V.Equals(pt._V) && _W.Equals(pt._W));
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的所有坐标偏移量将此 PointD6D 结构平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的所有坐标偏移量。</param>
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
        /// 按双精度浮点数表示的 X 坐标偏移量、Y 坐标偏移量、Z 坐标偏移量与 U 坐标偏移量将此 PointD6D 结构平移指定的量。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标偏移量。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标偏移量。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标偏移量。</param>
        /// <param name="du">双精度浮点数表示的 U 坐标偏移量。</param>
        /// <param name="dv">双精度浮点数表示的 V 坐标偏移量。</param>
        /// <param name="dw">双精度浮点数表示的 W 坐标偏移量。</param>
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
        /// 按 PointD6D 结构将此 PointD6D 结构平移指定的量。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于平移此 PointD6D 结构。</param>
        public void Offset(PointD6D pt)
        {
            if ((object)pt != null)
            {
                _X += pt._X;
                _Y += pt._Y;
                _Z += pt._Z;
                _U += pt._U;
                _V += pt._V;
                _W += pt._W;
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标偏移量将此 PointD6D 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的所有坐标偏移量。</param>
        public PointD6D OffsetCopy(double d)
        {
            return new PointD6D(_X + d, _Y + d, _Z + d, _U + d, _V + d, _W + d);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标偏移量、Y 坐标偏移量、Z 坐标偏移量与 U 坐标偏移量将此 PointD6D 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标偏移量。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标偏移量。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标偏移量。</param>
        /// <param name="du">双精度浮点数表示的 U 坐标偏移量。</param>
        /// <param name="dv">双精度浮点数表示的 V 坐标偏移量。</param>
        /// <param name="dw">双精度浮点数表示的 W 坐标偏移量。</param>
        public PointD6D OffsetCopy(double dx, double dy, double dz, double du, double dv, double dw)
        {
            return new PointD6D(_X + dx, _Y + dy, _Z + dz, _U + du, _V + du, _W + du);
        }

        /// <summary>
        /// 返回按 PointD6D 结构将此 PointD6D 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于平移此 PointD6D 结构。</param>
        public PointD6D OffsetCopy(PointD6D pt)
        {
            if ((object)pt != null)
            {
                return new PointD6D(_X + pt._X, _Y + pt._Y, _Z + pt._Z, _U + pt._U, _V + pt._V, _W + pt._W);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的所有坐标缩放因子将此 PointD6D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的所有坐标缩放因子。</param>
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
        /// 按双精度浮点数表示的 X 坐标缩放因子、Y 坐标缩放因子、Z 坐标缩放因子与 U 坐标缩放因子将此 PointD6D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因子。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因子。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因子。</param>
        /// <param name="su">双精度浮点数表示的 U 坐标缩放因子。</param>
        /// <param name="sv">双精度浮点数表示的 V 坐标缩放因子。</param>
        /// <param name="sw">双精度浮点数表示的 W 坐标缩放因子。</param>
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
        /// 按 PointD6D 结构将此 PointD6D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于缩放此 PointD6D 结构。</param>
        public void Scale(PointD6D pt)
        {
            if ((object)pt != null)
            {
                _X *= pt._X;
                _Y *= pt._Y;
                _Z *= pt._Z;
                _U *= pt._U;
                _V *= pt._V;
                _W *= pt._W;
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标缩放因子将此 PointD6D 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的所有坐标缩放因子。</param>
        public PointD6D ScaleCopy(double s)
        {
            return new PointD6D(_X * s, _Y * s, _Z * s, _U * s, _V * s, _W * s);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标缩放因子、Y 坐标缩放因子、Z 坐标缩放因子与 U 坐标缩放因子将此 PointD6D 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因子。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因子。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因子。</param>
        /// <param name="su">双精度浮点数表示的 U 坐标缩放因子。</param>
        /// <param name="sv">双精度浮点数表示的 V 坐标缩放因子。</param>
        /// <param name="sw">双精度浮点数表示的 W 坐标缩放因子。</param>
        public PointD6D ScaleCopy(double sx, double sy, double sz, double su, double sv, double sw)
        {
            return new PointD6D(_X * sx, _Y * sy, _Z * sz, _U * su, _V * sv, _W * sv);
        }

        /// <summary>
        /// 返回按 PointD6D 结构将此 PointD6D 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于缩放此 PointD6D 结构。</param>
        public PointD6D ScaleCopy(PointD6D pt)
        {
            if ((object)pt != null)
            {
                return new PointD6D(_X * pt._X, _Y * pt._Y, _Z * pt._Z, _U * pt._U, _V * pt._V, _W * pt._W);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD6D 结构绕 XY 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 XY 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public void RotateXY(double angle)
        {
            Matrix2D matrixLeft = RotateXYMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
                _V = result[0, 4];
                _W = result[0, 5];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD6D 结构绕 XZ 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 XZ 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Z 轴的方向为正方向）。</param>
        public void RotateXZ(double angle)
        {
            Matrix2D matrixLeft = RotateXZMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
                _V = result[0, 4];
                _W = result[0, 5];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD6D 结构绕 XU 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 XU 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateXU(double angle)
        {
            Matrix2D matrixLeft = RotateXUMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
                _V = result[0, 4];
                _W = result[0, 5];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD6D 结构绕 XV 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 XV 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateXV(double angle)
        {
            Matrix2D matrixLeft = RotateXVMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
                _V = result[0, 4];
                _W = result[0, 5];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD6D 结构绕 XW 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 XW 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateXW(double angle)
        {
            Matrix2D matrixLeft = RotateXWMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
                _V = result[0, 4];
                _W = result[0, 5];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD6D 结构绕 YZ 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 YZ 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        public void RotateYZ(double angle)
        {
            Matrix2D matrixLeft = RotateYZMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
                _V = result[0, 4];
                _W = result[0, 5];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD6D 结构绕 YU 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 YU 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateYU(double angle)
        {
            Matrix2D matrixLeft = RotateYUMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
                _V = result[0, 4];
                _W = result[0, 5];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD6D 结构绕 YV 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 YV 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateYV(double angle)
        {
            Matrix2D matrixLeft = RotateYVMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
                _V = result[0, 4];
                _W = result[0, 5];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD6D 结构绕 YW 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 YW 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateYW(double angle)
        {
            Matrix2D matrixLeft = RotateYWMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
                _V = result[0, 4];
                _W = result[0, 5];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD6D 结构绕 ZU 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 ZU 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateZU(double angle)
        {
            Matrix2D matrixLeft = RotateZUMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
                _V = result[0, 4];
                _W = result[0, 5];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD6D 结构绕 ZV 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 ZV 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateZV(double angle)
        {
            Matrix2D matrixLeft = RotateZVMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
                _V = result[0, 4];
                _W = result[0, 5];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD6D 结构绕 ZW 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 ZW 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateZW(double angle)
        {
            Matrix2D matrixLeft = RotateZWMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
                _V = result[0, 4];
                _W = result[0, 5];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD6D 结构绕 UV 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 UV 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateUV(double angle)
        {
            Matrix2D matrixLeft = RotateUVMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
                _V = result[0, 4];
                _W = result[0, 5];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD6D 结构绕 UW 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 UW 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateUW(double angle)
        {
            Matrix2D matrixLeft = RotateUWMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
                _V = result[0, 4];
                _W = result[0, 5];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD6D 结构绕 VW 平面的法向空间旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 VW 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateVW(double angle)
        {
            Matrix2D matrixLeft = RotateVWMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
                _V = result[0, 4];
                _W = result[0, 5];
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD6D 结构的副本绕 XY 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 XY 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public PointD6D RotateXYCopy(double angle)
        {
            Matrix2D matrixLeft = RotateXYMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD6D 结构的副本绕 XZ 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 XZ 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Z 轴的方向为正方向）。</param>
        public PointD6D RotateXZCopy(double angle)
        {
            Matrix2D matrixLeft = RotateXZMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD6D 结构的副本绕 XU 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 XU 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +U 轴的方向为正方向）。</param>
        public PointD6D RotateXUCopy(double angle)
        {
            Matrix2D matrixLeft = RotateXUMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD6D 结构的副本绕 XV 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 XV 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +U 轴的方向为正方向）。</param>
        public PointD6D RotateXVCopy(double angle)
        {
            Matrix2D matrixLeft = RotateXVMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD6D 结构的副本绕 XW 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 XW 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +U 轴的方向为正方向）。</param>
        public PointD6D RotateXWCopy(double angle)
        {
            Matrix2D matrixLeft = RotateXWMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD6D 结构的副本绕 YZ 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 YZ 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        public PointD6D RotateYZCopy(double angle)
        {
            Matrix2D matrixLeft = RotateYZMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD6D 结构的副本绕 YU 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 YU 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +U 轴的方向为正方向）。</param>
        public PointD6D RotateYUCopy(double angle)
        {
            Matrix2D matrixLeft = RotateYUMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD6D 结构的副本绕 YV 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 YV 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +U 轴的方向为正方向）。</param>
        public PointD6D RotateYVCopy(double angle)
        {
            Matrix2D matrixLeft = RotateYVMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD6D 结构的副本绕 YW 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 YW 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +U 轴的方向为正方向）。</param>
        public PointD6D RotateYWCopy(double angle)
        {
            Matrix2D matrixLeft = RotateYWMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD6D 结构的副本绕 ZU 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 ZU 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public PointD6D RotateZUCopy(double angle)
        {
            Matrix2D matrixLeft = RotateZUMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD6D 结构的副本绕 ZV 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 ZV 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public PointD6D RotateZVCopy(double angle)
        {
            Matrix2D matrixLeft = RotateZVMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD6D 结构的副本绕 ZW 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 ZW 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public PointD6D RotateZWCopy(double angle)
        {
            Matrix2D matrixLeft = RotateZWMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD6D 结构的副本绕 UV 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 UV 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public PointD6D RotateUVCopy(double angle)
        {
            Matrix2D matrixLeft = RotateUVMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD6D 结构的副本绕 UW 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 UW 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public PointD6D RotateUWCopy(double angle)
        {
            Matrix2D matrixLeft = RotateUWMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD6D 结构的副本绕 VW 平面的法向空间旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示此 PointD6D 结构绕 VW 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public PointD6D RotateVWCopy(double angle)
        {
            Matrix2D matrixLeft = RotateVWMatrix(angle);
            Matrix2D matrixRight = _ToMatrixForAffineTransform();

            Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

            if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
            {
                return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 按 PointD6D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量与偏移向量将此 PointD6D 结构进行仿射变换。
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
            if ((object)ex != null && (object)ey != null && (object)ez != null && (object)eu != null && (object)offset != null)
            {
                Matrix2D matrixLeft = new Matrix2D(new double[7, 7]
                {
                    { ex._X, ex._Y, ex._Z, ex._U, ex._V, ex._W, 0 },
                    { ey._X, ey._Y, ey._Z, ey._U, ey._V, ey._W, 0 },
                    { ez._X, ez._Y, ez._Z, ez._U, ez._V, ez._W, 0 },
                    { eu._X, eu._Y, eu._Z, eu._U, eu._V, eu._W, 0 },
                    { ev._X, ev._Y, ev._Z, ev._U, ev._V, ev._W, 0 },
                    { ew._X, ew._Y, ew._Z, ew._U, ew._V, ew._W, 0 },
                    { offset._X, offset._Y, offset._Z, offset._U, offset._V, offset._W, 1 }
                });
                Matrix2D matrixRight = _ToMatrixForAffineTransform();

                Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

                if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                    _Z = result[0, 2];
                    _U = result[0, 3];
                    _V = result[0, 4];
                    _W = result[0, 5];
                }
            }
        }

        /// <summary>
        /// 按双精度浮点数数组表示的 7x7 仿射矩阵（左矩阵）将此 PointD6D 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeft">双精度浮点数数组表示的 7x7 仿射矩阵（左矩阵）。</param>
        public void AffineTransform(Matrix2D matrixLeft)
        {
            if (!Matrix2D.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(7, 7))
            {
                Matrix2D matrixRight = _ToMatrixForAffineTransform();

                Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

                if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                    _Z = result[0, 2];
                    _U = result[0, 3];
                    _V = result[0, 4];
                    _W = result[0, 5];
                }
            }
        }

        /// <summary>
        /// 按双精度浮点数数组表示的 7x7 仿射矩阵（左矩阵）列表将此 PointD6D 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">双精度浮点数数组表示的 7x7 仿射矩阵（左矩阵）列表。</param>
        public void AffineTransform(List<Matrix2D> matrixLeftList)
        {
            if (!InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                Matrix2D result = _ToMatrixForAffineTransform();

                for (int i = 0; i < matrixLeftList.Count; i++)
                {
                    Matrix2D matrixLeft = matrixLeftList[i];

                    bool flag = (!Matrix2D.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(7, 7));

                    if (flag)
                    {
                        result = Matrix2D.Multiply(matrixLeft, result);

                        flag = (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7));
                    }

                    if (!flag)
                    {
                        return;
                    }
                }

                if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                    _Z = result[0, 2];
                    _U = result[0, 3];
                    _V = result[0, 4];
                    _W = result[0, 5];
                }
            }
        }

        /// <summary>
        /// 返回按 PointD6D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量与偏移向量将此 PointD6D 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="ex">PointD6D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD6D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD6D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD6D 结构表示的 U 基向量。</param>
        /// <param name="ev">PointD6D 结构表示的 V 基向量。</param>
        /// <param name="ew">PointD6D 结构表示的 W 基向量。</param>
        /// <param name="offset">PointD6D 结构表示的偏移向量。</param>
        public PointD6D AffineTransformCopy(PointD6D ex, PointD6D ey, PointD6D ez, PointD6D eu, PointD6D ev, PointD6D ew, PointD6D offset)
        {
            if ((object)ex != null && (object)ey != null && (object)ez != null && (object)eu != null && (object)offset != null)
            {
                Matrix2D matrixLeft = new Matrix2D(new double[7, 7]
                {
                    { ex._X, ex._Y, ex._Z, ex._U, ex._V, ex._W, 0 },
                    { ey._X, ey._Y, ey._Z, ey._U, ey._V, ey._W, 0 },
                    { ez._X, ez._Y, ez._Z, ez._U, ez._V, ez._W, 0 },
                    { eu._X, eu._Y, eu._Z, eu._U, eu._V, eu._W, 0 },
                    { ev._X, ev._Y, ev._Z, ev._U, ev._V, ev._W, 0 },
                    { ew._X, ew._Y, ew._Z, ew._U, ew._V, ew._W, 0 },
                    { offset._X, offset._Y, offset._Z, offset._U, offset._V, offset._W, 1 }
                });
                Matrix2D matrixRight = _ToMatrixForAffineTransform();

                Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

                if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
                {
                    return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数数组表示的 7x7 仿射矩阵（左矩阵）将此 PointD6D 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeft">双精度浮点数数组表示的 7x7 仿射矩阵（左矩阵）。</param>
        public PointD6D AffineTransformCopy(Matrix2D matrixLeft)
        {
            if (!Matrix2D.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(7, 7))
            {
                Matrix2D matrixRight = _ToMatrixForAffineTransform();

                Matrix2D result = Matrix2D.Multiply(matrixLeft, matrixRight);

                if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
                {
                    return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数数组表示的 7x7 仿射矩阵（左矩阵）列表将此 PointD6D 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeftList">双精度浮点数数组表示的 7x7 仿射矩阵（左矩阵）列表。</param>
        public PointD6D AffineTransformCopy(List<Matrix2D> matrixLeftList)
        {
            if (!InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                Matrix2D result = _ToMatrixForAffineTransform();

                for (int i = 0; i < matrixLeftList.Count; i++)
                {
                    Matrix2D matrixLeft = matrixLeftList[i];

                    bool flag = (!Matrix2D.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(7, 7));

                    if (flag)
                    {
                        result = Matrix2D.Multiply(matrixLeft, result);

                        flag = (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7));
                    }

                    if (!flag)
                    {
                        return NaN;
                    }
                }

                if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
                {
                    return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 按 PointD6D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量与偏移向量将此 PointD6D 结构进行逆仿射变换。
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
            if ((object)ex != null && (object)ey != null && (object)ez != null && (object)eu != null && (object)offset != null)
            {
                Matrix2D matrixLeft = new Matrix2D(new double[7, 7]
                {
                    { ex._X, ex._Y, ex._Z, ex._U, ex._V, ex._W, 0 },
                    { ey._X, ey._Y, ey._Z, ey._U, ey._V, ey._W, 0 },
                    { ez._X, ez._Y, ez._Z, ez._U, ez._V, ez._W, 0 },
                    { eu._X, eu._Y, eu._Z, eu._U, eu._V, eu._W, 0 },
                    { ev._X, ev._Y, ev._Z, ev._U, ev._V, ev._W, 0 },
                    { ew._X, ew._Y, ew._Z, ew._U, ew._V, ew._W, 0 },
                    { offset._X, offset._Y, offset._Z, offset._U, offset._V, offset._W, 1 }
                });
                Matrix2D matrixRight = _ToMatrixForAffineTransform();

                Matrix2D result = Matrix2D.DivideLeft(matrixLeft, matrixRight);

                if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                    _Z = result[0, 2];
                    _U = result[0, 3];
                    _V = result[0, 4];
                    _W = result[0, 5];
                }
            }
        }

        /// <summary>
        /// 按双精度浮点数数组表示的 7x7 仿射矩阵（左矩阵）将此 PointD6D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeft">双精度浮点数数组表示的 7x7 仿射矩阵（左矩阵）。</param>
        public void InverseAffineTransform(Matrix2D matrixLeft)
        {
            if (!Matrix2D.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(7, 7))
            {
                Matrix2D matrixRight = _ToMatrixForAffineTransform();

                Matrix2D result = Matrix2D.DivideLeft(matrixLeft, matrixRight);

                if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                    _Z = result[0, 2];
                    _U = result[0, 3];
                    _V = result[0, 4];
                    _W = result[0, 5];
                }
            }
        }

        /// <summary>
        /// 按双精度浮点数数组表示的 7x7 仿射矩阵（左矩阵）列表将此 PointD6D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">双精度浮点数数组表示的 7x7 仿射矩阵（左矩阵）列表。</param>
        public void InverseAffineTransform(List<Matrix2D> matrixLeftList)
        {
            if (!InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                Matrix2D result = _ToMatrixForAffineTransform();

                for (int i = matrixLeftList.Count - 1; i >= 0; i--)
                {
                    Matrix2D matrixLeft = matrixLeftList[i];

                    bool flag = (!Matrix2D.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(7, 7));

                    if (flag)
                    {
                        result = Matrix2D.DivideLeft(matrixLeft, result);

                        flag = (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7));
                    }

                    if (!flag)
                    {
                        return;
                    }
                }

                if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                    _Z = result[0, 2];
                    _U = result[0, 3];
                    _V = result[0, 4];
                    _W = result[0, 5];
                }
            }
        }

        /// <summary>
        /// 返回按 PointD6D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量与偏移向量将此 PointD6D 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="ex">PointD6D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD6D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD6D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD6D 结构表示的 U 基向量。</param>
        /// <param name="ev">PointD6D 结构表示的 V 基向量。</param>
        /// <param name="ew">PointD6D 结构表示的 W 基向量。</param>
        /// <param name="offset">PointD6D 结构表示的偏移向量。</param>
        public PointD6D InverseAffineTransformCopy(PointD6D ex, PointD6D ey, PointD6D ez, PointD6D eu, PointD6D ev, PointD6D ew, PointD6D offset)
        {
            if ((object)ex != null && (object)ey != null && (object)ez != null && (object)eu != null && (object)offset != null)
            {
                Matrix2D matrixLeft = new Matrix2D(new double[7, 7]
                {
                    { ex._X, ex._Y, ex._Z, ex._U, ex._V, ex._W, 0 },
                    { ey._X, ey._Y, ey._Z, ey._U, ey._V, ey._W, 0 },
                    { ez._X, ez._Y, ez._Z, ez._U, ez._V, ez._W, 0 },
                    { eu._X, eu._Y, eu._Z, eu._U, eu._V, eu._W, 0 },
                    { ev._X, ev._Y, ev._Z, ev._U, ev._V, ev._W, 0 },
                    { ew._X, ew._Y, ew._Z, ew._U, ew._V, ew._W, 0 },
                    { offset._X, offset._Y, offset._Z, offset._U, offset._V, offset._W, 1 }
                });
                Matrix2D matrixRight = _ToMatrixForAffineTransform();

                Matrix2D result = Matrix2D.DivideLeft(matrixLeft, matrixRight);

                if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
                {
                    return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数数组表示的 7x7 仿射矩阵（左矩阵）将此 PointD6D 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeft">双精度浮点数数组表示的 7x7 仿射矩阵（左矩阵）。</param>
        public PointD6D InverseAffineTransformCopy(Matrix2D matrixLeft)
        {
            if (!Matrix2D.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(7, 7))
            {
                Matrix2D matrixRight = _ToMatrixForAffineTransform();

                Matrix2D result = Matrix2D.DivideLeft(matrixLeft, matrixRight);

                if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
                {
                    return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数数组表示的 7x7 仿射矩阵（左矩阵）列表将此 PointD6D 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeftList">双精度浮点数数组表示的 7x7 仿射矩阵（左矩阵）列表。</param>
        public PointD6D InverseAffineTransformCopy(List<Matrix2D> matrixLeftList)
        {
            if (!InternalMethod.IsNullOrEmpty(matrixLeftList))
            {
                Matrix2D result = _ToMatrixForAffineTransform();

                for (int i = matrixLeftList.Count - 1; i >= 0; i--)
                {
                    Matrix2D matrixLeft = matrixLeftList[i];

                    bool flag = (!Matrix2D.IsNullOrNonMatrix(matrixLeft) && matrixLeft.Size == new Size(7, 7));

                    if (flag)
                    {
                        result = Matrix2D.DivideLeft(matrixLeft, result);

                        flag = (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7));
                    }

                    if (!flag)
                    {
                        return NaN;
                    }
                }

                if (!Matrix2D.IsNullOrNonMatrix(result) && result.Size == new Size(1, 7))
                {
                    return new PointD6D(result[0, 0], result[0, 1], result[0, 2], result[0, 3], result[0, 4], result[0, 5]);
                }
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将此 PointD6D 结构投影至平行于 XYZUV 空间的投影空间的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD6D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        public PointD5D ProjectToXYZUV(PointD6D prjCenter, double trueLenDist)
        {
            if ((object)prjCenter != null && (!InternalMethod.IsNaNOrInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return XYZUV;
                }
                else
                {
                    if (_W != prjCenter._W)
                    {
                        double Scale = trueLenDist / (_W - prjCenter._W);

                        if ((!InternalMethod.IsNaNOrInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * XYZUV + (1 - Scale) * prjCenter.XYZUV);
                        }
                    }
                }
            }

            return PointD5D.NaN;
        }

        /// <summary>
        /// 返回将此 PointD6D 结构投影至平行于 YZUVW 空间的投影空间的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD6D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        public PointD5D ProjectToYZUVW(PointD6D prjCenter, double trueLenDist)
        {
            if ((object)prjCenter != null && (!InternalMethod.IsNaNOrInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return YZUVW;
                }
                else
                {
                    if (_X != prjCenter._X)
                    {
                        double Scale = trueLenDist / (_X - prjCenter._X);

                        if ((!InternalMethod.IsNaNOrInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * YZUVW + (1 - Scale) * prjCenter.YZUVW);
                        }
                    }
                }
            }

            return PointD5D.NaN;
        }

        /// <summary>
        /// 返回将此 PointD6D 结构投影至平行于 ZUVWX 空间的投影空间的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD6D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        public PointD5D ProjectToZUVWX(PointD6D prjCenter, double trueLenDist)
        {
            if ((object)prjCenter != null && (!InternalMethod.IsNaNOrInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return ZUVWX;
                }
                else
                {
                    if (_Y != prjCenter._Y)
                    {
                        double Scale = trueLenDist / (_Y - prjCenter._Y);

                        if ((!InternalMethod.IsNaNOrInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * ZUVWX + (1 - Scale) * prjCenter.ZUVWX);
                        }
                    }
                }
            }

            return PointD5D.NaN;
        }

        /// <summary>
        /// 返回将此 PointD6D 结构投影至平行于 UVWXY 空间的投影空间的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD6D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        public PointD5D ProjectToUVWXY(PointD6D prjCenter, double trueLenDist)
        {
            if ((object)prjCenter != null && (!InternalMethod.IsNaNOrInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return UVWXY;
                }
                else
                {
                    if (_Z != prjCenter._Z)
                    {
                        double Scale = trueLenDist / (_Z - prjCenter._Z);

                        if ((!InternalMethod.IsNaNOrInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * UVWXY + (1 - Scale) * prjCenter.UVWXY);
                        }
                    }
                }
            }

            return PointD5D.NaN;
        }

        /// <summary>
        /// 返回将此 PointD6D 结构投影至平行于 VWXYZ 空间的投影空间的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD6D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        public PointD5D ProjectToVWXYZ(PointD6D prjCenter, double trueLenDist)
        {
            if ((object)prjCenter != null && (!InternalMethod.IsNaNOrInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return VWXYZ;
                }
                else
                {
                    if (_U != prjCenter._U)
                    {
                        double Scale = trueLenDist / (_U - prjCenter._U);

                        if ((!InternalMethod.IsNaNOrInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * VWXYZ + (1 - Scale) * prjCenter.VWXYZ);
                        }
                    }
                }
            }

            return PointD5D.NaN;
        }

        /// <summary>
        /// 返回将此 PointD6D 结构投影至平行于 WXYZU 空间的投影空间的 PointD5D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD6D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        public PointD5D ProjectToWXYZU(PointD6D prjCenter, double trueLenDist)
        {
            if ((object)prjCenter != null && (!InternalMethod.IsNaNOrInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return WXYZU;
                }
                else
                {
                    if (_V != prjCenter._V)
                    {
                        double Scale = trueLenDist / (_V - prjCenter._V);

                        if ((!InternalMethod.IsNaNOrInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * WXYZU + (1 - Scale) * prjCenter.WXYZU);
                        }
                    }
                }
            }

            return PointD5D.NaN;
        }

        //

        /// <summary>
        /// 返回此 PointD6D 结构与指定的 PointD6D 结构之间的距离。
        /// </summary>
        /// <param name="pt">PointD6D 结构，表示起始点。</param>
        public double DistanceFrom(PointD6D pt)
        {
            if ((object)pt != null)
            {
                double dx = _X - pt._X, dy = _Y - pt._Y, dz = _Z - pt._Z, du = _U - pt._U, dv = _V - pt._V, dw = _W - pt._W;

                return Math.Sqrt(dx * dx + dy * dy + dz * dz + du * du + dv * dv + dw * dw);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回此 PointD6D 结构表示的向量与指定的 PointD6D 结构表示的向量之间的夹角（弧度）。
        /// </summary>
        /// <param name="pt">PointD6D 结构，表示起始向量。</param>
        public double AngleFrom(PointD6D pt)
        {
            if ((object)pt != null)
            {
                if (_X == 0 && _Y == 0 && _Z == 0 && _U == 0 && _V == 0 && _W == 0)
                {
                    _X = 1;
                }

                if (pt._X == 0 && pt._Y == 0 && pt._Z == 0 && pt._U == 0 && pt._V == 0 && pt._W == 0)
                {
                    pt._X = 1;
                }

                double DotProduct = _X * pt._X + _Y * pt._Y + _Z * pt._Z + _U * pt._U + _V * pt._V + _W * pt._W;
                double ModProduct = VectorModule * pt.VectorModule;

                return Math.Acos(DotProduct / ModProduct);
            }

            return double.NaN;
        }

        //

        /// <summary>
        /// 返回将此 PointD6D 结构表示的直角坐标系坐标转换为超球坐标系坐标的新实例。
        /// </summary>
        public PointD6D ToSpherical()
        {
            return new PointD6D(VectorModule, VectorAngleX, VectorAngleY, VectorAngleZ, VectorAngleU, VectorAngleVW);
        }

        /// <summary>
        /// 返回将此 PointD6D 结构表示的超球坐标系坐标转换为直角坐标系坐标的新实例。
        /// </summary>
        public PointD6D ToCartesian()
        {
            return new PointD6D(_X * Math.Cos(_Y), _X * Math.Sin(_Y) * Math.Cos(_Z), _X * Math.Sin(_Y) * Math.Sin(_Z) * Math.Cos(_U), _X * Math.Sin(_Y) * Math.Sin(_Z) * Math.Sin(_U) * Math.Cos(_V), _X * Math.Sin(_Y) * Math.Sin(_Z) * Math.Sin(_U) * Math.Sin(_V) * Math.Cos(_W), _X * Math.Sin(_Y) * Math.Sin(_Z) * Math.Sin(_U) * Math.Sin(_V) * Math.Sin(_W));
        }

        //

        /// <summary>
        /// 返回将此 PointD6D 结构转换为向量的 Vector 的新实例。
        /// </summary>
        public Vector ToVector()
        {
            return new Vector(_X, _Y, _Z, _U, _V, _W);
        }

        /// <summary>
        /// 返回将此 PointD6D 结构转换为列向量的 Vector 的新实例。
        /// </summary>
        public Vector ToVectorColumn()
        {
            return new Vector(Vector.Type.ColumnVector, _X, _Y, _Z, _U, _V, _W);
        }

        /// <summary>
        /// 返回将此 PointD6D 结构转换为行向量的 Vector 的新实例。
        /// </summary>
        public Vector ToVectorRow()
        {
            return new Vector(Vector.Type.RowVector, _X, _Y, _Z, _U, _V, _W);
        }

        //

        /// <summary>
        /// 将此 PointD6D 结构转换为双精度浮点数数组。
        /// </summary>
        public double[] ToArray()
        {
            return new double[6] { _X, _Y, _Z, _U, _V, _W };
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断两个 PointD6D 结构是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 PointD6D 结构。</param>
        /// <param name="right">用于比较的第二个 PointD6D 结构。</param>
        public static bool Equals(PointD6D left, PointD6D right)
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
        /// 返回单位矩阵，表示不对 PointD6D 结构进行仿射变换的仿射矩阵（左矩阵）。
        /// </summary>
        public static Matrix2D IdentityMatrix()
        {
            return Matrix2D.Identity(7);
        }

        //

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标偏移量将 PointD6D 结构平移指定的量的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="d">双精度浮点数表示的所有坐标偏移量。</param>
        public static Matrix2D OffsetMatrix(double d)
        {
            return new Matrix2D(new double[7, 7]
            {
                { 1, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { d, d, d, d, d, d, 1 }
            });
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标偏移量、Y 坐标偏移量与 Z 坐标偏移量将 PointD6D 结构平移指定的量的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标偏移量。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标偏移量。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标偏移量。</param>
        /// <param name="du">双精度浮点数表示的 U 坐标偏移量。</param>
        /// <param name="dv">双精度浮点数表示的 V 坐标偏移量。</param>
        /// <param name="dw">双精度浮点数表示的 W 坐标偏移量。</param>
        public static Matrix2D OffsetMatrix(double dx, double dy, double dz, double du, double dv, double dw)
        {
            return new Matrix2D(new double[7, 7]
            {
                { 1, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { dx, dy, dz, du, dv, dw, 1 }
            });
        }

        /// <summary>
        /// 返回按 PointD6D 结构将 PointD6D 结构平移指定的量的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于平移 PointD6D 结构。</param>
        public static Matrix2D OffsetMatrix(PointD6D pt)
        {
            if ((object)pt != null)
            {
                return new Matrix2D(new double[7, 7]
                {
                    { 1, 0, 0, 0, 0, 0, 0 },
                    { 0, 1, 0, 0, 0, 0, 0 },
                    { 0, 0, 1, 0, 0, 0, 0 },
                    { 0, 0, 0, 1, 0, 0, 0 },
                    { 0, 0, 0, 0, 1, 0, 0 },
                    { 0, 0, 0, 0, 0, 1, 0 },
                    { pt._X, pt._Y, pt._Z, pt._U, pt._V, pt._W, 1 }
                });
            }

            return Matrix2D.NonMatrix;
        }

        //

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标缩放因子将 PointD6D 结构缩放指定的倍数的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="s">双精度浮点数表示的所有坐标缩放因子。</param>
        public static Matrix2D ScaleMatrix(double s)
        {
            return new Matrix2D(new double[7, 7]
            {
                { s, 0, 0, 0, 0, 0, 0 },
                { 0, s, 0, 0, 0, 0, 0 },
                { 0, 0, s, 0, 0, 0, 0 },
                { 0, 0, 0, s, 0, 0, 0 },
                { 0, 0, 0, 0, s, 0, 0 },
                { 0, 0, 0, 0, 0, s, 0 },
                { 0, 0, 0, 0, 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标缩放因子、Y 坐标缩放因子与 Z 坐标缩放因子将 PointD6D 结构缩放指定的倍数的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因子。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因子。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因子。</param>
        /// <param name="su">双精度浮点数表示的 U 坐标缩放因子。</param>
        /// <param name="sv">双精度浮点数表示的 V 坐标缩放因子。</param>
        /// <param name="sw">双精度浮点数表示的 W 坐标缩放因子。</param>
        public static Matrix2D ScaleMatrix(double sx, double sy, double sz, double su, double sv, double sw)
        {
            return new Matrix2D(new double[7, 7]
            {
                { sx, 0, 0, 0, 0, 0, 0 },
                { 0, sy, 0, 0, 0, 0, 0 },
                { 0, 0, sz, 0, 0, 0, 0 },
                { 0, 0, 0, su, 0, 0, 0 },
                { 0, 0, 0, 0, sv, 0, 0 },
                { 0, 0, 0, 0, 0, sw, 0 },
                { 0, 0, 0, 0, 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回按 PointD6D 结构将 PointD6D 结构缩放指定的倍数的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于缩放 PointD6D 结构。</param>
        public static Matrix2D ScaleMatrix(PointD6D pt)
        {
            if ((object)pt != null)
            {
                return new Matrix2D(new double[7, 7]
                {
                    { pt._X, 0, 0, 0, 0, 0, 0 },
                    { 0, pt._Y, 0, 0, 0, 0, 0 },
                    { 0, 0, pt._Z, 0, 0, 0, 0 },
                    { 0, 0, 0, pt._U, 0, 0, 0 },
                    { 0, 0, 0, 0, pt._V, 0, 0 },
                    { 0, 0, 0, 0, 0, pt._W, 0 },
                    { 0, 0, 0, 0, 0, 0, 1 }
                });
            }

            return Matrix2D.NonMatrix;
        }

        //

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD6D 结构绕 XY 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD6D 结构绕 XY 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public static Matrix2D RotateXYMatrix(double angle)
        {
            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            return new Matrix2D(new double[7, 7]
            {
                { CosA, SinA, 0, 0, 0, 0, 0 },
                { -SinA, CosA, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD6D 结构绕 XZ 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD6D 结构绕 XZ 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Z 轴的方向为正方向）。</param>
        public static Matrix2D RotateXZMatrix(double angle)
        {
            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            return new Matrix2D(new double[7, 7]
            {
                { CosA, 0, SinA, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0 },
                { -SinA, 0, CosA, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD6D 结构绕 XU 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD6D 结构绕 XU 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +U 轴的方向为正方向）。</param>
        public static Matrix2D RotateXUMatrix(double angle)
        {
            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            return new Matrix2D(new double[7, 7]
            {
                { CosA, 0, 0, SinA, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0 },
                { -SinA, 0, 0, CosA, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD6D 结构绕 XV 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD6D 结构绕 XV 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +V 轴的方向为正方向）。</param>
        public static Matrix2D RotateXVMatrix(double angle)
        {
            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            return new Matrix2D(new double[7, 7]
            {
                { CosA, 0, 0, 0, SinA, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
                { -SinA, 0, 0, 0, CosA, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD6D 结构绕 XW 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD6D 结构绕 XW 平面的法向空间旋转的角度（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +W 轴的方向为正方向）。</param>
        public static Matrix2D RotateXWMatrix(double angle)
        {
            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            return new Matrix2D(new double[7, 7]
            {
                { CosA, 0, 0, 0, 0, SinA, 0 },
                { 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0 },
                { -SinA, 0, 0, 0, 0, CosA, 0 },
                { 0, 0, 0, 0, 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD6D 结构绕 YZ 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD6D 结构绕 YZ 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        public static Matrix2D RotateYZMatrix(double angle)
        {
            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            return new Matrix2D(new double[7, 7]
            {
                { 1, 0, 0, 0, 0, 0, 0 },
                { 0, CosA, SinA, 0, 0, 0, 0 },
                { 0, -SinA, CosA, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD6D 结构绕 YU 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD6D 结构绕 YU 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +U 轴的方向为正方向）。</param>
        public static Matrix2D RotateYUMatrix(double angle)
        {
            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            return new Matrix2D(new double[7, 7]
            {
                { 1, 0, 0, 0, 0, 0, 0 },
                { 0, CosA, 0, SinA, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0 },
                { 0, -SinA, 0, CosA, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD6D 结构绕 YV 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD6D 结构绕 YV 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +V 轴的方向为正方向）。</param>
        public static Matrix2D RotateYVMatrix(double angle)
        {
            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            return new Matrix2D(new double[7, 7]
            {
                { 1, 0, 0, 0, 0, 0, 0 },
                { 0, CosA, 0, 0, SinA, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, -SinA, 0, 0, CosA, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD6D 结构绕 YW 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD6D 结构绕 YW 平面的法向空间旋转的角度（弧度）（以 +Y 轴为 0 弧度，从 +Y 轴指向 +W 轴的方向为正方向）。</param>
        public static Matrix2D RotateYWMatrix(double angle)
        {
            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            return new Matrix2D(new double[7, 7]
            {
                { 1, 0, 0, 0, 0, 0, 0 },
                { 0, CosA, 0, 0, 0, SinA, 0 },
                { 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0 },
                { 0, -SinA, 0, 0, 0, CosA, 0 },
                { 0, 0, 0, 0, 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD6D 结构绕 ZU 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD6D 结构绕 ZU 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public static Matrix2D RotateZUMatrix(double angle)
        {
            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            return new Matrix2D(new double[7, 7]
            {
                { 1, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, CosA, SinA, 0, 0, 0 },
                { 0, 0, -SinA, CosA, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD6D 结构绕 ZV 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD6D 结构绕 ZV 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +V 轴的方向为正方向）。</param>
        public static Matrix2D RotateZVMatrix(double angle)
        {
            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            return new Matrix2D(new double[7, 7]
            {
                { 1, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, CosA, 0, SinA, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, -SinA, 0, CosA, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD6D 结构绕 ZW 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD6D 结构绕 ZW 平面的法向空间旋转的角度（弧度）（以 +Z 轴为 0 弧度，从 +Z 轴指向 +W 轴的方向为正方向）。</param>
        public static Matrix2D RotateZWMatrix(double angle)
        {
            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            return new Matrix2D(new double[7, 7]
            {
                { 1, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, CosA, 0, 0, SinA, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, -SinA, 0, 0, CosA, 0 },
                { 0, 0, 0, 0, 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD6D 结构绕 UV 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD6D 结构绕 UV 平面的法向空间旋转的角度（弧度）（以 +U 轴为 0 弧度，从 +U 轴指向 +V 轴的方向为正方向）。</param>
        public static Matrix2D RotateUVMatrix(double angle)
        {
            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            return new Matrix2D(new double[7, 7]
            {
                { 1, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, CosA, SinA, 0, 0 },
                { 0, 0, 0, -SinA, CosA, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD6D 结构绕 UW 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD6D 结构绕 UW 平面的法向空间旋转的角度（弧度）（以 +U 轴为 0 弧度，从 +U 轴指向 +W 轴的方向为正方向）。</param>
        public static Matrix2D RotateUWMatrix(double angle)
        {
            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            return new Matrix2D(new double[7, 7]
            {
                { 1, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, CosA, 0, SinA, 0 },
                { 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, -SinA, 0, CosA, 0 },
                { 0, 0, 0, 0, 0, 0, 1 }
            });
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD6D 结构绕 VW 平面的法向空间旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数，表示 PointD6D 结构绕 VW 平面的法向空间旋转的角度（弧度）（以 +V 轴为 0 弧度，从 +V 轴指向 +W 轴的方向为正方向）。</param>
        public static Matrix2D RotateVWMatrix(double angle)
        {
            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            return new Matrix2D(new double[7, 7]
            {
                { 1, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, CosA, SinA, 0 },
                { 0, 0, 0, 0, -SinA, CosA, 0 },
                { 0, 0, 0, 0, 0, 0, 1 }
            });
        }

        //

        /// <summary>
        /// 返回两个 PointD6D 结构之间的距离。
        /// </summary>
        /// <param name="left">PointD6D 结构，表示第一个点。</param>
        /// <param name="right">PointD6D 结构，表示第二个点。</param>
        public static double DistanceBetween(PointD6D left, PointD6D right)
        {
            if ((object)left != null && (object)right != null)
            {
                double dx = left._X - right._X, dy = left._Y - right._Y, dz = left._Z - right._Z, du = left._U - right._U, dv = left._V - right._V, dw = left._W - right._W;

                return Math.Sqrt(dx * dx + dy * dy + dz * dz + du * du + dv * dv + dw * dw);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回 PointD6D 结构表示的两个向量之间的夹角（弧度）。
        /// </summary>
        /// <param name="left">PointD6D 结构，表示第一个向量。</param>
        /// <param name="right">PointD6D 结构，表示第二个向量。</param>
        public static double AngleBetween(PointD6D left, PointD6D right)
        {
            if ((object)left != null && (object)right != null)
            {
                if (left._X == 0 && left._Y == 0 && left._Z == 0 && left._U == 0 && left._V == 0 && left._W == 0)
                {
                    left._X = 1;
                }

                if (right._X == 0 && right._Y == 0 && right._Z == 0 && right._U == 0 && right._V == 0 && right._W == 0)
                {
                    right._X = 1;
                }

                double DotProduct = left._X * right._X + left._Y * right._Y + left._Z * right._Z + left._U * right._U + left._V * right._V + left._W * right._W;
                double ModProduct = left.VectorModule * right.VectorModule;

                return Math.Acos(DotProduct / ModProduct);
            }

            return double.NaN;
        }

        //

        /// <summary>
        /// 返回 PointD6D 结构表示的两个向量的数量积。
        /// </summary>
        /// <param name="left">PointD6D 结构，表示第一个向量。</param>
        /// <param name="right">PointD6D 结构，表示第二个向量。</param>
        public static double DotProduct(PointD6D left, PointD6D right)
        {
            if ((object)left != null && (object)right != null)
            {
                return (left._X * right._X + left._Y * right._Y + left._Z * right._Z + left._U * right._U + left._V * right._V + left._W * right._W);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回 PointD6D 结构表示的两个向量的向量积，该向量积为一个十五维向量，其所有分量的数值依次为 X∧Y 基向量、X∧Z 基向量、X∧U 基向量、X∧V 基向量、X∧W 基向量、Y∧Z 基向量、Y∧U 基向量、Y∧V 基向量、Y∧W 基向量、Z∧U 基向量、Z∧V 基向量、Z∧W 基向量、U∧V 基向量、U∧W 基向量与 V∧W 基向量的系数。
        /// </summary>
        /// <param name="left">PointD6D 结构，表示左向量。</param>
        /// <param name="right">PointD6D 结构，表示右向量。</param>
        public static Vector CrossProduct(PointD6D left, PointD6D right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new Vector(Vector.Type.ColumnVector, left._X * right._Y - left._Y * right._X, left._X * right._Z - left._Z * right._X, left._X * right._U - left._U * right._X, left._X * right._V - left._V * right._X, left._X * right._W - left._W * right._X, left._Y * right._Z - left._Z * right._Y, left._Y * right._U - left._U * right._Y, left._Y * right._V - left._V * right._Y, left._Y * right._W - left._W * right._Y, left._Z * right._U - left._U * right._Z, left._Z * right._V - left._V * right._Z, left._Z * right._W - left._W * right._Z, left._U * right._V - left._V * right._U, left._U * right._W - left._W * right._U, left._V * right._W - left._W * right._V);
            }

            return Vector.NonVector;
        }

        //

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量取绝对值得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于转换的结构。</param>
        public static PointD6D Abs(PointD6D pt)
        {
            if ((object)pt != null)
            {
                return new PointD6D(Math.Abs(pt._X), Math.Abs(pt._Y), Math.Abs(pt._Z), Math.Abs(pt._U), Math.Abs(pt._V), Math.Abs(pt._W));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量取符号数得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于转换的结构。</param>
        public static PointD6D Sign(PointD6D pt)
        {
            if ((object)pt != null)
            {
                return new PointD6D(Math.Sign(pt._X), Math.Sign(pt._Y), Math.Sign(pt._Z), Math.Sign(pt._U), Math.Sign(pt._V), Math.Sign(pt._W));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量舍入到较大的整数值得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于转换的结构。</param>
        public static PointD6D Ceiling(PointD6D pt)
        {
            if ((object)pt != null)
            {
                return new PointD6D(Math.Ceiling(pt._X), Math.Ceiling(pt._Y), Math.Ceiling(pt._Z), Math.Ceiling(pt._U), Math.Ceiling(pt._V), Math.Ceiling(pt._W));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量舍入到较小的整数值得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于转换的结构。</param>
        public static PointD6D Floor(PointD6D pt)
        {
            if ((object)pt != null)
            {
                return new PointD6D(Math.Floor(pt._X), Math.Floor(pt._Y), Math.Floor(pt._Z), Math.Floor(pt._U), Math.Floor(pt._V), Math.Floor(pt._W));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量舍入到最接近的整数值得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于转换的结构。</param>
        public static PointD6D Round(PointD6D pt)
        {
            if ((object)pt != null)
            {
                return new PointD6D(Math.Round(pt._X), Math.Round(pt._Y), Math.Round(pt._Z), Math.Round(pt._U), Math.Round(pt._V), Math.Round(pt._W));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量截断小数部分取整得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于转换的结构。</param>
        public static PointD6D Truncate(PointD6D pt)
        {
            if ((object)pt != null)
            {
                return new PointD6D(Math.Truncate(pt._X), Math.Truncate(pt._Y), Math.Truncate(pt._Z), Math.Truncate(pt._U), Math.Truncate(pt._V), Math.Truncate(pt._W));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将两个 PointD6D 结构的所有分量分别取最大值得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD6D 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD6D 结构，用于比较的第二个结构。</param>
        public static PointD6D Max(PointD6D left, PointD6D right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD6D(Math.Max(left._X, right._X), Math.Max(left._Y, right._Y), Math.Max(left._Z, right._Z), Math.Max(left._U, right._U), Math.Max(left._V, right._V), Math.Max(left._W, right._W));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将两个 PointD6D 结构的所有分量分别取最小值得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD6D 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD6D 结构，用于比较的第二个结构。</param>
        public static PointD6D Min(PointD6D left, PointD6D right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD6D(Math.Min(left._X, right._X), Math.Min(left._Y, right._Y), Math.Min(left._Z, right._Z), Math.Min(left._U, right._U), Math.Min(left._V, right._V), Math.Min(left._W, right._W));
            }

            return NaN;
        }

        #endregion

        #region 基类方法

        /// <summary>
        /// 判断此 PointD6D 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is PointD6D))
            {
                return false;
            }

            return Equals((PointD6D)obj);
        }

        /// <summary>
        /// 返回此 PointD6D 结构的哈希代码。
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 PointD6D 结构转换为字符串。
        /// </summary>
        public override string ToString()
        {
            return string.Concat("{X=", _X, ", Y=", _Y, ", Z=", _Z, ", U=", _U, ", V=", _V, ", W=", _W, "}");
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 PointD6D 结构是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD6D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD6D 结构。</param>
        public static bool operator ==(PointD6D left, PointD6D right)
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

            return (left._X == right._X && left._Y == right._Y && left._Z == right._Z && left._U == right._U && left._V == right._V && left._W == right._W);
        }

        /// <summary>
        /// 判断两个 PointD6D 结构是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD6D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD6D 结构。</param>
        public static bool operator !=(PointD6D left, PointD6D right)
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

            return (left._X != right._X || left._Y != right._Y || left._Z != right._Z || left._U != right._U || left._V != right._V || left._W != right._W);
        }

        /// <summary>
        /// 判断两个 PointD6D 结构表示的向量的模平方是否前者小于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD6D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD6D 结构。</param>
        public static bool operator <(PointD6D left, PointD6D right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared < right.VectorModuleSquared);
        }

        /// <summary>
        /// 判断两个 PointD6D 结构表示的向量的模平方是否前者大于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD6D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD6D 结构。</param>
        public static bool operator >(PointD6D left, PointD6D right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared > right.VectorModuleSquared);
        }

        /// <summary>
        /// 判断两个 PointD6D 结构表示的向量的模平方是否前者小于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD6D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD6D 结构。</param>
        public static bool operator <=(PointD6D left, PointD6D right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared <= right.VectorModuleSquared);
        }

        /// <summary>
        /// 判断两个 PointD6D 结构表示的向量的模平方是否前者大于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 PointD6D 结构。</param>
        /// <param name="right">运算符右侧比较的 PointD6D 结构。</param>
        public static bool operator >=(PointD6D left, PointD6D right)
        {
            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared >= right.VectorModuleSquared);
        }

        //

        /// <summary>
        /// 返回在 PointD6D 结构的所有分量前添加正号得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于转换的结构。</param>
        public static PointD6D operator +(PointD6D pt)
        {
            if ((object)pt != null)
            {
                return new PointD6D(+pt._X, +pt._Y, +pt._Z, +pt._U, +pt._V, +pt._W);
            }

            return NaN;
        }

        /// <summary>
        /// 返回在 PointD6D 结构的所有分量前添加负号得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，用于转换的结构。</param>
        public static PointD6D operator -(PointD6D pt)
        {
            if ((object)pt != null)
            {
                return new PointD6D(-pt._X, -pt._Y, -pt._Z, -pt._U, -pt._V, -pt._W);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量与双精度浮点数相加得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，表示被加数。</param>
        /// <param name="n">双精度浮点数，表示加数。</param>
        public static PointD6D operator +(PointD6D pt, double n)
        {
            if ((object)pt != null)
            {
                return new PointD6D(pt._X + n, pt._Y + n, pt._Z + n, pt._U + n, pt._V + n, pt._W + n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD6D 结构的所有分量相加得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被加数。</param>
        /// <param name="pt">PointD6D 结构，表示加数。</param>
        public static PointD6D operator +(double n, PointD6D pt)
        {
            if ((object)pt != null)
            {
                return new PointD6D(n + pt._X, n + pt._Y, n + pt._Z, n + pt._U, n + pt._V, n + pt._W);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD6D 结构与 PointD6D 结构的所有分量对应相加得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD6D 结构，表示被加数。</param>
        /// <param name="right">PointD6D 结构，表示加数。</param>
        public static PointD6D operator +(PointD6D left, PointD6D right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD6D(left._X + right._X, left._Y + right._Y, left._Z + right._Z, left._U + right._U, left._V + right._V, left._W + right._W);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量与双精度浮点数相减得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，表示被减数。</param>
        /// <param name="n">双精度浮点数，表示减数。</param>
        public static PointD6D operator -(PointD6D pt, double n)
        {
            if ((object)pt != null)
            {
                return new PointD6D(pt._X - n, pt._Y - n, pt._Z - n, pt._U - n, pt._V - n, pt._W - n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD6D 结构的所有分量相减得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被减数。</param>
        /// <param name="pt">PointD6D 结构，表示减数。</param>
        public static PointD6D operator -(double n, PointD6D pt)
        {
            if ((object)pt != null)
            {
                return new PointD6D(n - pt._X, n - pt._Y, n - pt._Z, n - pt._U, n - pt._V, n - pt._W);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD6D 结构与 PointD6D 结构的所有分量对应相减得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD6D 结构，表示被减数。</param>
        /// <param name="right">PointD6D 结构，表示减数。</param>
        public static PointD6D operator -(PointD6D left, PointD6D right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD6D(left._X - right._X, left._Y - right._Y, left._Z - right._Z, left._U - right._U, left._V - right._V, left._W - right._W);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量与双精度浮点数相乘得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，表示被乘数。</param>
        /// <param name="n">双精度浮点数，表示乘数。</param>
        public static PointD6D operator *(PointD6D pt, double n)
        {
            if ((object)pt != null)
            {
                return new PointD6D(pt._X * n, pt._Y * n, pt._Z * n, pt._U * n, pt._V * n, pt._W * n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD6D 结构的所有分量相乘得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被乘数。</param>
        /// <param name="pt">PointD6D 结构，表示乘数。</param>
        public static PointD6D operator *(double n, PointD6D pt)
        {
            if ((object)pt != null)
            {
                return new PointD6D(n * pt._X, n * pt._Y, n * pt._Z, n * pt._U, n * pt._V, n * pt._W);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD6D 结构与 PointD6D 结构的所有分量对应相乘得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD6D 结构，表示被乘数。</param>
        /// <param name="right">PointD6D 结构，表示乘数。</param>
        public static PointD6D operator *(PointD6D left, PointD6D right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD6D(left._X * right._X, left._Y * right._Y, left._Z * right._Z, left._U * right._U, left._V * right._V, left._W * right._W);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD6D 结构的所有分量与双精度浮点数相除得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD6D 结构，表示被除数。</param>
        /// <param name="n">双精度浮点数，表示除数。</param>
        public static PointD6D operator /(PointD6D pt, double n)
        {
            if ((object)pt != null)
            {
                return new PointD6D(pt._X / n, pt._Y / n, pt._Z / n, pt._U / n, pt._V / n, pt._W / n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD6D 结构的所有分量相除得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被除数。</param>
        /// <param name="pt">PointD6D 结构，表示除数。</param>
        public static PointD6D operator /(double n, PointD6D pt)
        {
            if ((object)pt != null)
            {
                return new PointD6D(n / pt._X, n / pt._Y, n / pt._Z, n / pt._U, n / pt._V, n / pt._W);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD6D 结构与 PointD6D 结构的所有分量对应相除得到的 PointD6D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD6D 结构，表示被除数。</param>
        /// <param name="right">PointD6D 结构，表示除数。</param>
        public static PointD6D operator /(PointD6D left, PointD6D right)
        {
            if ((object)left != null && (object)right != null)
            {
                return new PointD6D(left._X / right._X, left._Y / right._Y, left._Z / right._Z, left._U / right._U, left._V / right._V, left._W / right._W);
            }

            return NaN;
        }

        #endregion
    }
}