﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2013-2018 chibayuki@foxmail.com

Com.PointD4D
Version 18.5.23.0000

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
    /// 以一组有序的双精度浮点数表示的四维直角坐标系坐标。
    /// </summary>
    public struct PointD4D
    {
        #region 私有与内部成员

        private double _X; // X 坐标。
        private double _Y; // Y 坐标。
        private double _Z; // Z 坐标。
        private double _U; // U 坐标。

        #endregion

        #region 常量与只读成员

        /// <summary>
        /// 表示所有属性为其数据类型的默认值的 PointD4D 结构的实例。
        /// </summary>
        public static readonly PointD4D Empty = default(PointD4D);

        /// <summary>
        /// 表示所有属性为非数字的 PointD4D 结构的实例。
        /// </summary>
        public static readonly PointD4D NaN = new PointD4D(double.NaN, double.NaN, double.NaN, double.NaN);

        //

        /// <summary>
        /// 表示 X 基向量的 PointD4D 结构的实例。
        /// </summary>
        public static readonly PointD4D Ex = new PointD4D(1, 0, 0, 0);

        /// <summary>
        /// 表示 Y 基向量的 PointD4D 结构的实例。
        /// </summary>
        public static readonly PointD4D Ey = new PointD4D(0, 1, 0, 0);

        /// <summary>
        /// 表示 Z 基向量的 PointD4D 结构的实例。
        /// </summary>
        public static readonly PointD4D Ez = new PointD4D(0, 0, 1, 0);

        /// <summary>
        /// 表示 U 基向量的 PointD4D 结构的实例。
        /// </summary>
        public static readonly PointD4D Eu = new PointD4D(0, 0, 0, 1);

        #endregion

        #region 构造与析构函数

        /// <summary>
        /// 使用双精度浮点数表示的 X 坐标、Y 坐标、Z 坐标与 U 坐标初始化 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="x">双精度浮点数表示的 X 坐标。</param>
        /// <param name="y">双精度浮点数表示的 Y 坐标。</param>
        /// <param name="z">双精度浮点数表示的 Z 坐标。</param>
        /// <param name="u">双精度浮点数表示的 U 坐标。</param>
        public PointD4D(double x, double y, double z, double u)
        {
            _X = x;
            _Y = y;
            _Z = z;
            _U = u;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取表示此 PointD4D 结构是否为 Empty 的布尔值。
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (_X == Empty._X && _Y == Empty._Y && _Z == Empty._Z && _U == Empty._U);
            }
        }

        /// <summary>
        /// 获取表示此 PointD4D 结构是否为 NaN 的布尔值。
        /// </summary>
        public bool IsNaN
        {
            get
            {
                return (double.IsNaN(_X) || double.IsNaN(_Y) || double.IsNaN(_Z) || double.IsNaN(_U));
            }
        }

        /// <summary>
        /// 获取表示此 PointD4D 结构是否为 Infinity 的布尔值。
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return ((!double.IsNaN(_X) && !double.IsNaN(_Y) && !double.IsNaN(_Z) && !double.IsNaN(_U)) && (double.IsInfinity(_X) || double.IsInfinity(_Y) || double.IsInfinity(_Z) || double.IsInfinity(_U)));
            }
        }

        /// <summary>
        /// 获取表示此 PointD4D 结构是否为 NaN 或 Infinity 的布尔值。
        /// </summary>
        public bool IsNaNOrInfinity
        {
            get
            {
                return ((double.IsNaN(_X) || double.IsNaN(_Y) || double.IsNaN(_Z) || double.IsNaN(_U)) || (double.IsInfinity(_X) || double.IsInfinity(_Y) || double.IsInfinity(_Z) || double.IsInfinity(_U)));
            }
        }

        //

        /// <summary>
        /// 获取或设置此 PointD4D 结构在 X 轴的分量。
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
        /// 获取或设置此 PointD4D 结构在 Y 轴的分量。
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
        /// 获取或设置此 PointD4D 结构在 Z 轴的分量。
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
        /// 获取或设置此 PointD4D 结构在 U 轴的分量。
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

        //

        /// <summary>
        /// 获取或设置此 PointD4D 结构在 XY 平面的分量。
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
        /// 获取或设置此 PointD4D 结构在 YZ 平面的分量。
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
        /// 获取或设置此 PointD4D 结构在 ZU 平面的分量。
        /// </summary>
        public PointD ZU
        {
            get
            {
                return new PointD(_Z, _U);
            }

            set
            {
                _Z = value.X;
                _U = value.Y;
            }
        }

        /// <summary>
        /// 获取或设置此 PointD4D 结构在 UX 平面的分量。
        /// </summary>
        public PointD UX
        {
            get
            {
                return new PointD(_U, _X);
            }

            set
            {
                _U = value.X;
                _X = value.Y;
            }
        }

        //

        /// <summary>
        /// 获取或设置此 PointD4D 结构在 XYZ 空间的分量。
        /// </summary>
        public PointD3D XYZ
        {
            get
            {
                return new PointD3D(_X, _Y, _Z);
            }

            set
            {
                _X = value.X;
                _Y = value.Y;
                _Z = value.Z;
            }
        }

        /// <summary>
        /// 获取或设置此 PointD4D 结构在 YZU 空间的分量。
        /// </summary>
        public PointD3D YZU
        {
            get
            {
                return new PointD3D(_Y, _Z, _U);
            }

            set
            {
                _Y = value.X;
                _Z = value.Y;
                _U = value.Z;
            }
        }

        /// <summary>
        /// 获取或设置此 PointD4D 结构在 ZUX 空间的分量。
        /// </summary>
        public PointD3D ZUX
        {
            get
            {
                return new PointD3D(_Z, _U, _X);
            }

            set
            {
                _Z = value.X;
                _U = value.Y;
                _X = value.Z;
            }
        }

        /// <summary>
        /// 获取或设置此 PointD4D 结构在 UXY 空间的分量。
        /// </summary>
        public PointD3D UXY
        {
            get
            {
                return new PointD3D(_U, _X, _Y);
            }

            set
            {
                _U = value.X;
                _X = value.Y;
                _Y = value.Z;
            }
        }

        //

        /// <summary>
        /// 获取此 PointD4D 结构表示的向量的模。
        /// </summary>
        public double VectorModule
        {
            get
            {
                return Math.Sqrt(_X * _X + _Y * _Y + _Z * _Z + _U * _U);
            }
        }

        /// <summary>
        /// 获取此 PointD4D 结构表示的向量的模平方。
        /// </summary>
        public double VectorModuleSquared
        {
            get
            {
                return (_X * _X + _Y * _Y + _Z * _Z + _U * _U);
            }
        }

        /// <summary>
        /// 获取此 PointD4D 结构表示的向量的相反向量。
        /// </summary>
        public PointD4D VectorNegate
        {
            get
            {
                return new PointD4D(-_X, -_Y, -_Z, -_U);
            }
        }

        /// <summary>
        /// 获取此 PointD4D 结构表示的向量的规范化向量。
        /// </summary>
        public PointD4D VectorNormalize
        {
            get
            {
                double Mod = VectorModule;

                if (Mod > 0)
                {
                    return new PointD4D(_X / Mod, _Y / Mod, _Z / Mod, _U / Mod);
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
        /// 按双精度浮点数表示的所有坐标偏移量将此 PointD4D 结构平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的所有坐标偏移量。</param>
        public void Offset(double d)
        {
            _X += d;
            _Y += d;
            _Z += d;
            _U += d;
        }

        /// <summary>
        /// 按双精度浮点数表示的 X 坐标偏移量、Y 坐标偏移量、Z 坐标偏移量与 U 坐标偏移量将此 PointD4D 结构平移指定的量。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标偏移量。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标偏移量。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标偏移量。</param>
        /// <param name="du">双精度浮点数表示的 U 坐标偏移量。</param>
        public void Offset(double dx, double dy, double dz, double du)
        {
            _X += dx;
            _Y += dy;
            _Z += dz;
            _U += du;
        }

        /// <summary>
        /// 按 PointD4D 结构将此 PointD4D 结构平移指定的量。
        /// </summary>
        /// <param name="pt">PointD4D 结构，用于平移此 PointD4D 结构。</param>
        public void Offset(PointD4D pt)
        {
            if (pt != null)
            {
                _X += pt.X;
                _Y += pt.Y;
                _Z += pt.Z;
                _U += pt.U;
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标偏移量将此 PointD4D 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的所有坐标偏移量。</param>
        public PointD4D OffsetCopy(double d)
        {
            return new PointD4D(_X + d, _Y + d, _Z + d, _U + d);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标偏移量、Y 坐标偏移量、Z 坐标偏移量与 U 坐标偏移量将此 PointD4D 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标偏移量。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标偏移量。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标偏移量。</param>
        /// <param name="du">双精度浮点数表示的 U 坐标偏移量。</param>
        public PointD4D OffsetCopy(double dx, double dy, double dz, double du)
        {
            return new PointD4D(_X + dx, _Y + dy, _Z + dz, _U + du);
        }

        /// <summary>
        /// 返回按 PointD4D 结构将此 PointD4D 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="pt">PointD4D 结构，用于平移此 PointD4D 结构。</param>
        public PointD4D OffsetCopy(PointD4D pt)
        {
            if (pt != null)
            {
                return new PointD4D(_X + pt.X, _Y + pt.Y, _Z + pt.Z, _U + pt.U);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的所有坐标缩放因子将此 PointD4D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的所有坐标缩放因子。</param>
        public void Scale(double s)
        {
            _X *= s;
            _Y *= s;
            _Z *= s;
            _U *= s;
        }

        /// <summary>
        /// 按双精度浮点数表示的 X 坐标缩放因子、Y 坐标缩放因子、Z 坐标缩放因子与 U 坐标缩放因子将此 PointD4D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因子。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因子。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因子。</param>
        /// <param name="su">双精度浮点数表示的 U 坐标缩放因子。</param>
        public void Scale(double sx, double sy, double sz, double su)
        {
            _X *= sx;
            _Y *= sy;
            _Z *= sz;
            _U *= su;
        }

        /// <summary>
        /// 按 PointD4D 结构将此 PointD4D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="pt">PointD4D 结构，用于缩放此 PointD4D 结构。</param>
        public void Scale(PointD4D pt)
        {
            if (pt != null)
            {
                _X *= pt.X;
                _Y *= pt.Y;
                _Z *= pt.Z;
                _U *= pt.U;
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标缩放因子将此 PointD4D 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的所有坐标缩放因子。</param>
        public PointD4D ScaleCopy(double s)
        {
            return new PointD4D(_X * s, _Y * s, _Z * s, _U * s);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标缩放因子、Y 坐标缩放因子、Z 坐标缩放因子与 U 坐标缩放因子将此 PointD4D 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因子。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因子。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因子。</param>
        /// <param name="su">双精度浮点数表示的 U 坐标缩放因子。</param>
        public PointD4D ScaleCopy(double sx, double sy, double sz, double su)
        {
            return new PointD4D(_X * sx, _Y * sy, _Z * sz, _U * su);
        }

        /// <summary>
        /// 返回按 PointD4D 结构将此 PointD4D 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="pt">PointD4D 结构，用于缩放此 PointD4D 结构。</param>
        public PointD4D ScaleCopy(PointD4D pt)
        {
            if (pt != null)
            {
                return new PointD4D(_X * pt.X, _Y * pt.Y, _Z * pt.Z, _U * pt.U);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD4D 结构绕 XY 平面旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕 XY 平面旋转的角度（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public void RotateXY(double angle)
        {
            double[,] matrixLeft = new double[5, 5] { { 1, 0, 0, 0, 0 }, { 0, 1, 0, 0, 0 }, { 0, 0, Math.Cos(angle), Math.Sin(angle), 0 }, { 0, 0, -Math.Sin(angle), Math.Cos(angle), 0 }, { 0, 0, 0, 0, 1 } };
            double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
            double[,] result;

            if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD4D 结构绕 YZ 平面旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕 YZ 平面旋转的角度（以 +U 轴为 0 弧度，从 +U 轴指向 +X 轴的方向为正方向）。</param>
        public void RotateYZ(double angle)
        {
            double[,] matrixLeft = new double[5, 5] { { Math.Cos(angle), 0, 0, -Math.Sin(angle), 0 }, { 0, 1, 0, 0, 0 }, { 0, 0, 1, 0, 0 }, { Math.Sin(angle), 0, 0, Math.Cos(angle), 0 }, { 0, 0, 0, 0, 1 } };
            double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
            double[,] result;

            if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD4D 结构绕 ZU 平面旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕 ZU 平面旋转的角度（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public void RotateZU(double angle)
        {
            double[,] matrixLeft = new double[5, 5] { { Math.Cos(angle), Math.Sin(angle), 0, 0, 0 }, { -Math.Sin(angle), Math.Cos(angle), 0, 0, 0 }, { 0, 0, 1, 0, 0 }, { 0, 0, 0, 1, 0 }, { 0, 0, 0, 0, 1 } };
            double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
            double[,] result;

            if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD4D 结构绕 UX 平面旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕 UX 平面旋转的角度（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        public void RotateUX(double angle)
        {
            double[,] matrixLeft = new double[5, 5] { { 1, 0, 0, 0, 0 }, { 0, Math.Cos(angle), Math.Sin(angle), 0, 0 }, { 0, -Math.Sin(angle), Math.Cos(angle), 0, 0 }, { 0, 0, 0, 1, 0 }, { 0, 0, 0, 0, 1 } };
            double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
            double[,] result;

            if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD4D 结构的副本绕 XY 平面旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕 XY 平面旋转的角度（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public PointD4D RotateXYCopy(double angle)
        {
            double[,] matrixLeft = new double[5, 5] { { 1, 0, 0, 0, 0 }, { 0, 1, 0, 0, 0 }, { 0, 0, Math.Cos(angle), Math.Sin(angle), 0 }, { 0, 0, -Math.Sin(angle), Math.Cos(angle), 0 }, { 0, 0, 0, 0, 1 } };
            double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
            double[,] result;

            if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
            {
                return new PointD4D(result[0, 0], result[0, 1], result[0, 2], result[0, 3]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD4D 结构的副本绕 YZ 平面旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕 YZ 平面旋转的角度（以 +U 轴为 0 弧度，从 +U 轴指向 +X 轴的方向为正方向）。</param>
        public PointD4D RotateYZCopy(double angle)
        {
            double[,] matrixLeft = new double[5, 5] { { Math.Cos(angle), 0, 0, -Math.Sin(angle), 0 }, { 0, 1, 0, 0, 0 }, { 0, 0, 1, 0, 0 }, { Math.Sin(angle), 0, 0, Math.Cos(angle), 0 }, { 0, 0, 0, 0, 1 } };
            double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
            double[,] result;

            if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
            {
                return new PointD4D(result[0, 0], result[0, 1], result[0, 2], result[0, 3]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD4D 结构的副本绕 ZU 平面旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕 ZU 平面旋转的角度（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public PointD4D RotateZUCopy(double angle)
        {
            double[,] matrixLeft = new double[5, 5] { { Math.Cos(angle), Math.Sin(angle), 0, 0, 0 }, { -Math.Sin(angle), Math.Cos(angle), 0, 0, 0 }, { 0, 0, 1, 0, 0 }, { 0, 0, 0, 1, 0 }, { 0, 0, 0, 0, 1 } };
            double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
            double[,] result;

            if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
            {
                return new PointD4D(result[0, 0], result[0, 1], result[0, 2], result[0, 3]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD4D 结构的副本绕 UX 平面旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕 UX 平面旋转的角度（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        public PointD4D RotateUXCopy(double angle)
        {
            double[,] matrixLeft = new double[5, 5] { { 1, 0, 0, 0, 0 }, { 0, Math.Cos(angle), Math.Sin(angle), 0, 0 }, { 0, -Math.Sin(angle), Math.Cos(angle), 0, 0 }, { 0, 0, 0, 1, 0 }, { 0, 0, 0, 0, 1 } };
            double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
            double[,] result;

            if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
            {
                return new PointD4D(result[0, 0], result[0, 1], result[0, 2], result[0, 3]);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 按 PointD4D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量与偏移向量将此 PointD4D 结构进行仿射变换。
        /// </summary>
        /// <param name="ex">PointD4D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD4D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD4D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD4D 结构表示的 U 基向量。</param>
        /// <param name="offset">PointD4D 结构表示的偏移向量。</param>
        public void AffineTransform(PointD4D ex, PointD4D ey, PointD4D ez, PointD4D eu, PointD4D offset)
        {
            if (ex != null && ey != null && ez != null && eu != null && offset != null)
            {
                double[,] matrixLeft = new double[5, 5] { { ex.X, ex.Y, ex.Z, ex.U, 0 }, { ey.X, ey.Y, ey.Z, ey.U, 0 }, { ez.X, ez.Y, ez.Z, ez.U, 0 }, { eu.X, eu.Y, eu.Z, eu.U, 0 }, { offset.X, offset.Y, offset.Z, offset.U, 1 } };
                double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
                double[,] result;

                if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                    _Z = result[0, 2];
                    _U = result[0, 3];
                }
            }
        }

        /// <summary>
        /// 按双精度浮点数数组表示的 5x5 仿射矩阵（左矩阵）将此 PointD4D 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeft">双精度浮点数数组表示的 5x5 仿射矩阵（左矩阵）。</param>
        public void AffineTransform(double[,] matrixLeft)
        {
            if (matrixLeft != null && Matrix2D.GetSize(matrixLeft) == new Size(5, 5))
            {
                double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
                double[,] result;

                if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                    _Z = result[0, 2];
                    _U = result[0, 3];
                }
            }
        }

        /// <summary>
        /// 按双精度浮点数数组表示的 5x5 仿射矩阵（左矩阵）列表将此 PointD3D 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">双精度浮点数数组表示的 5x5 仿射矩阵（左矩阵）列表。</param>
        public void AffineTransform(List<double[,]> matrixLeftList)
        {
            if (matrixLeftList.Count > 0)
            {
                double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
                double[,] result = null;

                for (int i = 0; i < matrixLeftList.Count; i++)
                {
                    double[,] matrixLeft = matrixLeftList[i];

                    bool flag = (matrixLeft != null && Matrix2D.GetSize(matrixLeft) == new Size(5, 5));

                    if (flag)
                    {
                        flag = Matrix2D.Multiply(matrixLeft, matrixRight, out result);
                    }

                    if (flag)
                    {
                        flag = Matrix2D.Copy(result, out matrixRight);
                    }

                    if (!flag)
                    {
                        return;
                    }
                }

                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
            }
        }

        /// <summary>
        /// 返回按 PointD4D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量与偏移向量将此 PointD4D 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="ex">PointD4D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD4D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD4D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD4D 结构表示的 U 基向量。</param>
        /// <param name="offset">PointD4D 结构表示的偏移向量。</param>
        public PointD4D AffineTransformCopy(PointD4D ex, PointD4D ey, PointD4D ez, PointD4D eu, PointD4D offset)
        {
            if (ex != null && ey != null && ez != null && eu != null && offset != null)
            {
                double[,] matrixLeft = new double[5, 5] { { ex.X, ex.Y, ex.Z, ex.U, 0 }, { ey.X, ey.Y, ey.Z, ey.U, 0 }, { ez.X, ez.Y, ez.Z, ez.U, 0 }, { eu.X, eu.Y, eu.Z, eu.U, 0 }, { offset.X, offset.Y, offset.Z, offset.U, 1 } };
                double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
                double[,] result;

                if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
                {
                    return new PointD4D(result[0, 0], result[0, 1], result[0, 2], result[0, 3]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数数组表示的 5x5 仿射矩阵（左矩阵）将此 PointD4D 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeft">双精度浮点数数组表示的 5x5 仿射矩阵（左矩阵）。</param>
        public PointD4D AffineTransformCopy(double[,] matrixLeft)
        {
            if (matrixLeft != null && Matrix2D.GetSize(matrixLeft) == new Size(5, 5))
            {
                double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
                double[,] result;

                if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
                {
                    return new PointD4D(result[0, 0], result[0, 1], result[0, 2], result[0, 3]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数数组表示的 5x5 仿射矩阵（左矩阵）列表将此 PointD4D 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeftList">双精度浮点数数组表示的 5x5 仿射矩阵（左矩阵）列表。</param>
        public PointD4D AffineTransformCopy(List<double[,]> matrixLeftList)
        {
            if (matrixLeftList.Count > 0)
            {
                double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
                double[,] result = null;

                for (int i = 0; i < matrixLeftList.Count; i++)
                {
                    double[,] matrixLeft = matrixLeftList[i];

                    bool flag = (matrixLeft != null && Matrix2D.GetSize(matrixLeft) == new Size(5, 5));

                    if (flag)
                    {
                        flag = Matrix2D.Multiply(matrixLeft, matrixRight, out result);
                    }

                    if (flag)
                    {
                        flag = Matrix2D.Copy(result, out matrixRight);
                    }

                    if (!flag)
                    {
                        return NaN;
                    }
                }

                return new PointD4D(result[0, 0], result[0, 1], result[0, 2], result[0, 3]);
            }

            return NaN;
        }

        /// <summary>
        /// 按 PointD4D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量与偏移向量将此 PointD4D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="ex">PointD4D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD4D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD4D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD4D 结构表示的 U 基向量。</param>
        /// <param name="offset">PointD4D 结构表示的偏移向量。</param>
        public void InverseAffineTransform(PointD4D ex, PointD4D ey, PointD4D ez, PointD4D eu, PointD4D offset)
        {
            if (ex != null && ey != null && ez != null && eu != null && offset != null)
            {
                double[,] matrixLeft = new double[5, 5] { { ex.X, ex.Y, ex.Z, ex.U, 0 }, { ey.X, ey.Y, ey.Z, ey.U, 0 }, { ez.X, ez.Y, ez.Z, ez.U, 0 }, { eu.X, eu.Y, eu.Z, eu.U, 0 }, { offset.X, offset.Y, offset.Z, offset.U, 1 } };
                double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
                double[,] result;

                if (Matrix2D.DivideLeft(matrixLeft, matrixRight, out result))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                    _Z = result[0, 2];
                    _U = result[0, 3];
                }
            }
        }

        /// <summary>
        /// 按双精度浮点数数组表示的 5x5 仿射矩阵（左矩阵）将此 PointD4D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeft">双精度浮点数数组表示的 5x5 仿射矩阵（左矩阵）。</param>
        public void InverseAffineTransform(double[,] matrixLeft)
        {
            if (matrixLeft != null && Matrix2D.GetSize(matrixLeft) == new Size(5, 5))
            {
                double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
                double[,] result;

                if (Matrix2D.DivideLeft(matrixLeft, matrixRight, out result))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                    _Z = result[0, 2];
                    _U = result[0, 3];
                }
            }
        }

        /// <summary>
        /// 按双精度浮点数数组表示的 5x5 仿射矩阵（左矩阵）列表将此 PointD3D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">双精度浮点数数组表示的 5x5 仿射矩阵（左矩阵）列表。</param>
        public void InverseAffineTransform(List<double[,]> matrixLeftList)
        {
            if (matrixLeftList.Count > 0)
            {
                double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
                double[,] result = null;

                for (int i = matrixLeftList.Count - 1; i >= 0; i--)
                {
                    double[,] matrixLeft = matrixLeftList[i];

                    bool flag = (matrixLeft != null && Matrix2D.GetSize(matrixLeft) == new Size(5, 5));

                    if (flag)
                    {
                        flag = Matrix2D.DivideLeft(matrixLeft, matrixRight, out result);
                    }

                    if (flag)
                    {
                        flag = Matrix2D.Copy(result, out matrixRight);
                    }

                    if (!flag)
                    {
                        return;
                    }
                }

                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
                _U = result[0, 3];
            }
        }

        /// <summary>
        /// 返回按 PointD4D 结构表示的 X 基向量、Y 基向量、Z 基向量、U 基向量与偏移向量将此 PointD4D 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="ex">PointD4D 结构表示的 X 基向量。</param>
        /// <param name="ey">PointD4D 结构表示的 Y 基向量。</param>
        /// <param name="ez">PointD4D 结构表示的 Z 基向量。</param>
        /// <param name="eu">PointD4D 结构表示的 U 基向量。</param>
        /// <param name="offset">PointD4D 结构表示的偏移向量。</param>
        public PointD4D InverseAffineTransformCopy(PointD4D ex, PointD4D ey, PointD4D ez, PointD4D eu, PointD4D offset)
        {
            if (ex != null && ey != null && ez != null && eu != null && offset != null)
            {
                double[,] matrixLeft = new double[5, 5] { { ex.X, ex.Y, ex.Z, ex.U, 0 }, { ey.X, ey.Y, ey.Z, ey.U, 0 }, { ez.X, ez.Y, ez.Z, ez.U, 0 }, { eu.X, eu.Y, eu.Z, eu.U, 0 }, { offset.X, offset.Y, offset.Z, offset.U, 1 } };
                double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
                double[,] result;

                if (Matrix2D.DivideLeft(matrixLeft, matrixRight, out result))
                {
                    return new PointD4D(result[0, 0], result[0, 1], result[0, 2], result[0, 3]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数数组表示的 5x5 仿射矩阵（左矩阵）将此 PointD4D 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeft">双精度浮点数数组表示的 5x5 仿射矩阵（左矩阵）。</param>
        public PointD4D InverseAffineTransformCopy(double[,] matrixLeft)
        {
            if (matrixLeft != null && Matrix2D.GetSize(matrixLeft) == new Size(5, 5))
            {
                double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
                double[,] result;

                if (Matrix2D.DivideLeft(matrixLeft, matrixRight, out result))
                {
                    return new PointD4D(result[0, 0], result[0, 1], result[0, 2], result[0, 3]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数数组表示的 5x5 仿射矩阵（左矩阵）列表将此 PointD4D 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeftList">双精度浮点数数组表示的 5x5 仿射矩阵（左矩阵）列表。</param>
        public PointD4D InverseAffineTransformCopy(List<double[,]> matrixLeftList)
        {
            if (matrixLeftList.Count > 0)
            {
                double[,] matrixRight = new double[1, 5] { { _X, _Y, _Z, _U, 1 } };
                double[,] result = null;

                for (int i = matrixLeftList.Count - 1; i >= 0; i--)
                {
                    double[,] matrixLeft = matrixLeftList[i];

                    bool flag = (matrixLeft != null && Matrix2D.GetSize(matrixLeft) == new Size(5, 5));

                    if (flag)
                    {
                        flag = Matrix2D.DivideLeft(matrixLeft, matrixRight, out result);
                    }

                    if (flag)
                    {
                        flag = Matrix2D.Copy(result, out matrixRight);
                    }

                    if (!flag)
                    {
                        return NaN;
                    }
                }

                return new PointD4D(result[0, 0], result[0, 1], result[0, 2], result[0, 3]);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将此 PointD4D 结构投影至平行于 XYZ 空间的投影空间的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD4D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        public PointD3D ProjectToXYZ(PointD4D prjCenter, double trueLenDist)
        {
            if (prjCenter != null && (!double.IsNaN(trueLenDist) && !double.IsInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return XYZ;
                }
                else
                {
                    if (_U != prjCenter.U)
                    {
                        double Scale = trueLenDist / (_U - prjCenter.U);

                        if ((!double.IsNaN(Scale) && !double.IsInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * (XYZ - prjCenter.XYZ) + prjCenter.XYZ);
                        }
                    }
                }
            }

            return PointD3D.NaN;
        }

        /// <summary>
        /// 返回将此 PointD4D 结构投影至平行于 YZU 空间的投影空间的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD4D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        public PointD3D ProjectToYZU(PointD4D prjCenter, double trueLenDist)
        {
            if (prjCenter != null && (!double.IsNaN(trueLenDist) && !double.IsInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return YZU;
                }
                else
                {
                    if (_X != prjCenter.X)
                    {
                        double Scale = trueLenDist / (_X - prjCenter.X);

                        if ((!double.IsNaN(Scale) && !double.IsInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * (YZU - prjCenter.YZU) + prjCenter.YZU);
                        }
                    }
                }
            }

            return PointD3D.NaN;
        }

        /// <summary>
        /// 返回将此 PointD4D 结构投影至平行于 ZUX 空间的投影空间的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD4D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        public PointD3D ProjectToZUX(PointD4D prjCenter, double trueLenDist)
        {
            if (prjCenter != null && (!double.IsNaN(trueLenDist) && !double.IsInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return ZUX;
                }
                else
                {
                    if (_Y != prjCenter.Y)
                    {
                        double Scale = trueLenDist / (_Y - prjCenter.Y);

                        if ((!double.IsNaN(Scale) && !double.IsInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * (ZUX - prjCenter.ZUX) + prjCenter.ZUX);
                        }
                    }
                }
            }

            return PointD3D.NaN;
        }

        /// <summary>
        /// 返回将此 PointD4D 结构投影至平行于 UXY 空间的投影空间的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="prjCenter">PointD4D 结构，表示投射中心在投影空间的正投影在原坐标系的坐标。</param>
        /// <param name="trueLenDist">双精度浮点数表示的距离，平行于投影空间的一维度量其真实尺度与投影尺度的比值等于其到投影空间的距离与此距离的比值。</param>
        public PointD3D ProjectToUXY(PointD4D prjCenter, double trueLenDist)
        {
            if (prjCenter != null && (!double.IsNaN(trueLenDist) && !double.IsInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return UXY;
                }
                else
                {
                    if (_Z != prjCenter.Z)
                    {
                        double Scale = trueLenDist / (_Z - prjCenter.Z);

                        if ((!double.IsNaN(Scale) && !double.IsInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * (UXY - prjCenter.UXY) + prjCenter.UXY);
                        }
                    }
                }
            }

            return PointD3D.NaN;
        }

        //

        /// <summary>
        /// 返回此 PointD4D 结构与指定的 PointD4D 结构之间的距离。
        /// </summary>
        /// <param name="pt">PointD4D 结构，表示起始点。</param>
        public double DistanceFrom(PointD4D pt)
        {
            if (pt != null)
            {
                double dx = _X - pt.X, dy = _Y - pt.Y, dz = _Z - pt.Z, du = _U - pt.U;

                return Math.Sqrt(dx * dx + dy * dy + dz * dz + du * du);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回此 PointD4D 结构表示的向量与指定的 PointD4D 结构表示的向量之间的夹角（弧度）。
        /// </summary>
        /// <param name="pt">PointD4D 结构，表示起始向量。</param>
        public double AngleFrom(PointD4D pt)
        {
            if (pt != null)
            {
                if (_X == 0 && _Y == 0 && _Z == 0 && _U == 0)
                {
                    _X = 1;
                }

                if (pt.X == 0 && pt.Y == 0 && pt.Z == 0 && pt.U == 0)
                {
                    pt.X = 1;
                }

                double DotProduct = _X * pt.X + _Y * pt.Y + _Z * pt.Z + _U * pt.U;
                double ModProduct = VectorModule * pt.VectorModule;

                return Math.Acos(DotProduct / ModProduct);
            }

            return double.NaN;
        }

        //

        /// <summary>
        /// 返回将此 PointD4D 结构转换为双精度浮点数数组表示的列向量。
        /// </summary>
        public double[,] ToVectorColumn()
        {
            return new double[1, 4] { { _X, _Y, _Z, _U } };
        }

        /// <summary>
        /// 返回将此 PointD4D 结构转换为双精度浮点数数组表示的行向量。
        /// </summary>
        public double[,] ToVectorRow()
        {
            return new double[4, 1] { { _X }, { _Y }, { _Z }, { _U } };
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标偏移量将 PointD4D 结构平移指定的量的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="d">双精度浮点数表示的所有坐标偏移量。</param>
        public static double[,] OffsetMatrix(double d)
        {
            return new double[5, 5] { { 1, 0, 0, 0, 0 }, { 0, 1, 0, 0, 0 }, { 0, 0, 1, 0, 0 }, { 0, 0, 0, 1, 0 }, { d, d, d, d, 1 } };
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标偏移量、Y 坐标偏移量与 Z 坐标偏移量将 PointD4D 结构平移指定的量的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标偏移量。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标偏移量。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标偏移量。</param>
        /// <param name="du">双精度浮点数表示的 U 坐标偏移量。</param>
        public static double[,] OffsetMatrix(double dx, double dy, double dz, double du)
        {
            return new double[5, 5] { { 1, 0, 0, 0, 0 }, { 0, 1, 0, 0, 0 }, { 0, 0, 1, 0, 0 }, { 0, 0, 0, 1, 0 }, { dx, dy, dz, du, 1 } };
        }

        /// <summary>
        /// 返回按 PointD4D 结构将 PointD4D 结构平移指定的量的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="pt">PointD4D 结构，用于平移 PointD4D 结构。</param>
        public static double[,] OffsetMatrix(PointD4D pt)
        {
            if (pt != null)
            {
                return new double[5, 5] { { 1, 0, 0, 0, 0 }, { 0, 1, 0, 0, 0 }, { 0, 0, 1, 0, 0 }, { 0, 0, 0, 1, 0 }, { pt.X, pt.Y, pt.Z, pt.U, 1 } };
            }

            return null;
        }

        //

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标缩放因子将 PointD4D 结构缩放指定的倍数的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="s">双精度浮点数表示的所有坐标缩放因子。</param>
        public static double[,] ScaleMatrix(double s)
        {
            return new double[5, 5] { { s, 0, 0, 0, 0 }, { 0, s, 0, 0, 0 }, { 0, 0, s, 0, 0 }, { 0, 0, 0, s, 0 }, { 0, 0, 0, 0, 1 } };
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标缩放因子、Y 坐标缩放因子与 Z 坐标缩放因子将 PointD4D 结构缩放指定的倍数的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因子。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因子。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因子。</param>
        /// <param name="su">双精度浮点数表示的 U 坐标缩放因子。</param>
        public static double[,] ScaleMatrix(double sx, double sy, double sz, double su)
        {
            return new double[5, 5] { { sx, 0, 0, 0, 0 }, { 0, sy, 0, 0, 0 }, { 0, 0, sz, 0, 0 }, { 0, 0, 0, su, 0 }, { 0, 0, 0, 0, 1 } };
        }

        /// <summary>
        /// 返回按 PointD4D 结构将 PointD4D 结构缩放指定的倍数的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="pt">PointD4D 结构，用于缩放 PointD4D 结构。</param>
        public static double[,] ScaleMatrix(PointD4D pt)
        {
            if (pt != null)
            {
                return new double[5, 5] { { pt.X, 0, 0, 0, 0 }, { 0, pt.Y, 0, 0, 0 }, { 0, 0, pt.Z, 0, 0 }, { 0, 0, 0, pt.U, 0 }, { 0, 0, 0, 0, 1 } };
            }

            return null;
        }

        //

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD4D 结构绕 XY 平面旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示 PointD 结构绕 XY 平面旋转的角度（以 +Z 轴为 0 弧度，从 +Z 轴指向 +U 轴的方向为正方向）。</param>
        public static double[,] RotateXYMatrix(double angle)
        {
            return new double[5, 5] { { 1, 0, 0, 0, 0 }, { 0, 1, 0, 0, 0 }, { 0, 0, Math.Cos(angle), Math.Sin(angle), 0 }, { 0, 0, -Math.Sin(angle), Math.Cos(angle), 0 }, { 0, 0, 0, 0, 1 } };
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD4D 结构绕 YZ 平面旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示 PointD 结构绕 YZ 平面旋转的角度（以 +U 轴为 0 弧度，从 +U 轴指向 +X 轴的方向为正方向）。</param>
        public static double[,] RotateYZMatrix(double angle)
        {
            return new double[5, 5] { { Math.Cos(angle), 0, 0, -Math.Sin(angle), 0 }, { 0, 1, 0, 0, 0 }, { 0, 0, 1, 0, 0 }, { Math.Sin(angle), 0, 0, Math.Cos(angle), 0 }, { 0, 0, 0, 0, 1 } };
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD4D 结构绕 ZU 平面旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示 PointD 结构绕 ZU 平面旋转的角度（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public static double[,] RotateZUMatrix(double angle)
        {
            return new double[5, 5] { { Math.Cos(angle), Math.Sin(angle), 0, 0, 0 }, { -Math.Sin(angle), Math.Cos(angle), 0, 0, 0 }, { 0, 0, 1, 0, 0 }, { 0, 0, 0, 1, 0 }, { 0, 0, 0, 0, 1 } };
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD4D 结构绕 UX 平面旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示 PointD 结构绕 UX 平面旋转的角度（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        public static double[,] RotateUXMatrix(double angle)
        {
            return new double[5, 5] { { 1, 0, 0, 0, 0 }, { 0, Math.Cos(angle), Math.Sin(angle), 0, 0 }, { 0, -Math.Sin(angle), Math.Cos(angle), 0, 0 }, { 0, 0, 0, 1, 0 }, { 0, 0, 0, 0, 1 } };
        }

        //

        /// <summary>
        /// 返回两个 PointD4D 结构之间的距离。
        /// </summary>
        /// <param name="left">PointD4D 结构，表示第一个点。</param>
        /// <param name="right">PointD4D 结构，表示第二个点。</param>
        public static double DistanceBetween(PointD4D left, PointD4D right)
        {
            if (left != null && right != null)
            {
                double dx = left.X - right.X, dy = left.Y - right.Y, dz = left.Z - right.Z, du = left.U - right.U;

                return Math.Sqrt(dx * dx + dy * dy + dz * dz + du * du);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回此 PointD4D 结构表示的两个向量之间的夹角（弧度）。
        /// </summary>
        /// <param name="left">PointD4D 结构，表示第一个向量。</param>
        /// <param name="right">PointD4D 结构，表示第二个向量。</param>
        public static double AngleBetween(PointD4D left, PointD4D right)
        {
            if (left != null && right != null)
            {
                if (left.X == 0 && left.Y == 0 && left.Z == 0 && left.U == 0)
                {
                    left.X = 1;
                }

                if (right.X == 0 && right.Y == 0 && right.Z == 0 && right.U == 0)
                {
                    right.X = 1;
                }

                double DotProduct = left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.U * right.U;
                double ModProduct = left.VectorModule * right.VectorModule;

                return Math.Acos(DotProduct / ModProduct);
            }

            return double.NaN;
        }

        //

        /// <summary>
        /// 返回 PointD4D 结构表示的两个向量的数量积。
        /// </summary>
        /// <param name="left">PointD4D 结构，表示第一个向量。</param>
        /// <param name="right">PointD4D 结构，表示第二个向量。</param>
        public static double DotProduct(PointD4D left, PointD4D right)
        {
            if (left != null && right != null)
            {
                return (left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.U * right.U);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回 PointD4D 结构表示的两个向量的向量积，该向量积为一个六维向量，其所有分量的数值依次为 X∧Y 基向量、X∧Z 基向量、X∧U 基向量、Y∧Z 基向量、Y∧U 基向量与 Z∧U 基向量的系数。
        /// </summary>
        /// <param name="left">PointD4D 结构，表示左向量。</param>
        /// <param name="right">PointD4D 结构，表示右向量。</param>
        public static double[] CrossProduct(PointD4D left, PointD4D right)
        {
            if (left != null && right != null)
            {
                return new double[6] { left.X * right.Y - left.Y * right.X, left.X * right.Z - left.Z * right.X, left.X * right.U - left.U * right.X, left.Y * right.Z - left.Z * right.Y, left.Y * right.U - left.U * right.Y, left.Z * right.U - left.U * right.Z };
            }

            return null;
        }

        //

        /// <summary>
        /// 返回将 PointD4D 结构的所有分量取绝对值得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD4D 结构，用于转换的结构。</param>
        public static PointD4D Abs(PointD4D pt)
        {
            if (pt != null)
            {
                return new PointD4D(Math.Abs(pt.X), Math.Abs(pt.Y), Math.Abs(pt.Z), Math.Abs(pt.U));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD4D 结构的所有分量取符号数得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD4D 结构，用于转换的结构。</param>
        public static PointD4D Sign(PointD4D pt)
        {
            if (pt != null)
            {
                return new PointD4D(Math.Sign(pt.X), Math.Sign(pt.Y), Math.Sign(pt.Z), Math.Sign(pt.U));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD4D 结构的所有分量舍入到较大的整数值得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD4D 结构，用于转换的结构。</param>
        public static PointD4D Ceiling(PointD4D pt)
        {
            if (pt != null)
            {
                return new PointD4D(Math.Ceiling(pt.X), Math.Ceiling(pt.Y), Math.Ceiling(pt.Z), Math.Ceiling(pt.U));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD4D 结构的所有分量舍入到较小的整数值得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD4D 结构，用于转换的结构。</param>
        public static PointD4D Floor(PointD4D pt)
        {
            if (pt != null)
            {
                return new PointD4D(Math.Floor(pt.X), Math.Floor(pt.Y), Math.Floor(pt.Z), Math.Floor(pt.U));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD4D 结构的所有分量舍入到最接近的整数值得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD4D 结构，用于转换的结构。</param>
        public static PointD4D Round(PointD4D pt)
        {
            if (pt != null)
            {
                return new PointD4D(Math.Round(pt.X), Math.Round(pt.Y), Math.Round(pt.Z), Math.Round(pt.U));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD4D 结构的所有分量截断小数部分取整得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD4D 结构，用于转换的结构。</param>
        public static PointD4D Truncate(PointD4D pt)
        {
            if (pt != null)
            {
                return new PointD4D(Math.Truncate(pt.X), Math.Truncate(pt.Y), Math.Truncate(pt.Z), Math.Truncate(pt.U));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将两个 PointD4D 结构的所有分量分别取最大值得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD4D 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD4D 结构，用于比较的第二个结构。</param>
        public static PointD4D Max(PointD4D left, PointD4D right)
        {
            if (left != null && right != null)
            {
                return new PointD4D(Math.Max(left.X, right.X), Math.Max(left.Y, right.Y), Math.Max(left.Z, right.Z), Math.Max(left.U, right.U));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将两个 PointD4D 结构的所有分量分别取最小值得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD4D 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD4D 结构，用于比较的第二个结构。</param>
        public static PointD4D Min(PointD4D left, PointD4D right)
        {
            if (left != null && right != null)
            {
                return new PointD4D(Math.Min(left.X, right.X), Math.Min(left.Y, right.Y), Math.Min(left.Z, right.Z), Math.Min(left.U, right.U));
            }

            return NaN;
        }

        #endregion

        #region 基类方法

        /// <summary>
        /// 判断此 PointD4D 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is PointD4D))
            {
                return false;
            }

            return Equals((PointD4D)obj);
        }

        /// <summary>
        /// 判断此 PointD4D 结构是否与指定的 PointD4D 结构相等。
        /// </summary>
        /// <param name="pointD4D">用于比较的 PointD4D 结构。</param>
        public bool Equals(PointD4D pointD4D)
        {
            if (pointD4D == null)
            {
                return false;
            }

            return (_X == pointD4D._X && _Y == pointD4D._Y && _Z == pointD4D._Z && _U == pointD4D._U);
        }

        /// <summary>
        /// 返回此 PointD4D 结构的哈希代码。
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 PointD4D 结构转换为字符串。
        /// </summary>
        public override string ToString()
        {
            return string.Concat("{X=", _X, ", Y=", _Y, ", Z=", _Z, ", U=", _U, "}");
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 返回在 PointD4D 结构的所有分量前添加正号得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD4D 结构，用于转换的结构。</param>
        public static PointD4D operator +(PointD4D pt)
        {
            if (pt != null)
            {
                return new PointD4D(+pt.X, +pt.Y, +pt.Z, +pt.U);
            }

            return NaN;
        }

        /// <summary>
        /// 返回在 PointD4D 结构的所有分量前添加负号得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD4D 结构，用于转换的结构。</param>
        public static PointD4D operator -(PointD4D pt)
        {
            if (pt != null)
            {
                return new PointD4D(-pt.X, -pt.Y, -pt.Z, -pt.U);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD4D 结构的所有分量与双精度浮点数相加得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD4D 结构，表示被加数。</param>
        /// <param name="n">双精度浮点数，表示加数。</param>
        public static PointD4D operator +(PointD4D pt, double n)
        {
            if (pt != null)
            {
                return new PointD4D(pt.X + n, pt.Y + n, pt.Z + n, pt.U + n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD4D 结构的所有分量相加得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被加数。</param>
        /// <param name="pt">PointD4D 结构，表示加数。</param>
        public static PointD4D operator +(double n, PointD4D pt)
        {
            if (pt != null)
            {
                return new PointD4D(n + pt.X, n + pt.Y, n + pt.Z, n + pt.U);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD4D 结构与 PointD4D 结构的所有分量对应相加得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD4D 结构，表示被加数。</param>
        /// <param name="right">PointD4D 结构，表示加数。</param>
        public static PointD4D operator +(PointD4D left, PointD4D right)
        {
            if (left != null && right != null)
            {
                return new PointD4D(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.U + right.U);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD4D 结构的所有分量与双精度浮点数相减得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD4D 结构，表示被减数。</param>
        /// <param name="n">双精度浮点数，表示减数。</param>
        public static PointD4D operator -(PointD4D pt, double n)
        {
            if (pt != null)
            {
                return new PointD4D(pt.X - n, pt.Y - n, pt.Z - n, pt.U - n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD4D 结构的所有分量相减得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被减数。</param>
        /// <param name="pt">PointD4D 结构，表示减数。</param>
        public static PointD4D operator -(double n, PointD4D pt)
        {
            if (pt != null)
            {
                return new PointD4D(n - pt.X, n - pt.Y, n - pt.Z, n - pt.U);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD4D 结构与 PointD4D 结构的所有分量对应相减得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD4D 结构，表示被减数。</param>
        /// <param name="right">PointD4D 结构，表示减数。</param>
        public static PointD4D operator -(PointD4D left, PointD4D right)
        {
            if (left != null && right != null)
            {
                return new PointD4D(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.U - right.U);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD4D 结构的所有分量与双精度浮点数相乘得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD4D 结构，表示被乘数。</param>
        /// <param name="n">双精度浮点数，表示乘数。</param>
        public static PointD4D operator *(PointD4D pt, double n)
        {
            if (pt != null)
            {
                return new PointD4D(pt.X * n, pt.Y * n, pt.Z * n, pt.U * n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD4D 结构的所有分量相乘得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被乘数。</param>
        /// <param name="pt">PointD4D 结构，表示乘数。</param>
        public static PointD4D operator *(double n, PointD4D pt)
        {
            if (pt != null)
            {
                return new PointD4D(n * pt.X, n * pt.Y, n * pt.Z, n * pt.U);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD4D 结构与 PointD4D 结构的所有分量对应相乘得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD4D 结构，表示被乘数。</param>
        /// <param name="right">PointD4D 结构，表示乘数。</param>
        public static PointD4D operator *(PointD4D left, PointD4D right)
        {
            if (left != null && right != null)
            {
                return new PointD4D(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.U * right.U);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD4D 结构的所有分量与双精度浮点数相除得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD4D 结构，表示被除数。</param>
        /// <param name="n">双精度浮点数，表示除数。</param>
        public static PointD4D operator /(PointD4D pt, double n)
        {
            if (pt != null)
            {
                return new PointD4D(pt.X / n, pt.Y / n, pt.Z / n, pt.U / n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD4D 结构的所有分量相除得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被除数。</param>
        /// <param name="pt">PointD4D 结构，表示除数。</param>
        public static PointD4D operator /(double n, PointD4D pt)
        {
            if (pt != null)
            {
                return new PointD4D(n / pt.X, n / pt.Y, n / pt.Z, n / pt.U);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD4D 结构与 PointD4D 结构的所有分量对应相除得到的 PointD4D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD4D 结构，表示被除数。</param>
        /// <param name="right">PointD4D 结构，表示除数。</param>
        public static PointD4D operator /(PointD4D left, PointD4D right)
        {
            if (left != null && right != null)
            {
                return new PointD4D(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.U / right.U);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 判断 PointD4D 结构的两个实例是否相等。
        /// </summary>
        /// <param name="left">PointD4D 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD4D 结构，运算符右侧比较的结构。</param>
        public static bool operator ==(PointD4D left, PointD4D right)
        {
            if (left == null || right == null)
            {
                return false;
            }

            return (left.X == right.X && left.Y == right.Y && left.Z == right.Z && left.U == right.U);
        }

        /// <summary>
        /// 判断 PointD4D 结构的两个实例是否不相等。
        /// </summary>
        /// <param name="left">PointD4D 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD4D 结构，运算符右侧比较的结构。</param>
        public static bool operator !=(PointD4D left, PointD4D right)
        {
            if (left == null || right == null)
            {
                return true;
            }

            return (left.X != right.X || left.Y != right.Y || left.Z != right.Z || left.U != right.U);
        }

        /// <summary>
        /// 判断 PointD4D 结构的两个实例表示的向量的模平方是否前者小于后者。
        /// </summary>
        /// <param name="left">PointD4D 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD4D 结构，运算符右侧比较的结构。</param>
        public static bool operator <(PointD4D left, PointD4D right)
        {
            if (left == null || right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared < right.VectorModuleSquared);
        }

        /// <summary>
        /// 判断 PointD4D 结构的两个实例表示的向量的模平方是否前者大于后者。
        /// </summary>
        /// <param name="left">PointD4D 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD4D 结构，运算符右侧比较的结构。</param>
        public static bool operator >(PointD4D left, PointD4D right)
        {
            if (left == null || right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared > right.VectorModuleSquared);
        }

        /// <summary>
        /// 判断 PointD4D 结构的两个实例表示的向量的模平方是否前者小于或等于后者。
        /// </summary>
        /// <param name="left">PointD4D 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD4D 结构，运算符右侧比较的结构。</param>
        public static bool operator <=(PointD4D left, PointD4D right)
        {
            if (left == null || right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared <= right.VectorModuleSquared);
        }

        /// <summary>
        /// 判断 PointD4D 结构的两个实例表示的向量的模平方是否前者大于或等于后者。
        /// </summary>
        /// <param name="left">PointD4D 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD4D 结构，运算符右侧比较的结构。</param>
        public static bool operator >=(PointD4D left, PointD4D right)
        {
            if (left == null || right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared >= right.VectorModuleSquared);
        }

        #endregion
    }
}