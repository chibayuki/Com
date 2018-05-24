﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2013-2018 chibayuki@foxmail.com

Com.PointD3D
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
    /// 以一组有序的双精度浮点数表示的三维直角坐标系坐标。
    /// </summary>
    public struct PointD3D
    {
        #region 私有与内部成员

        private double _X; // X 坐标。
        private double _Y; // Y 坐标。
        private double _Z; // Z 坐标。

        #endregion

        #region 常量与只读成员

        /// <summary>
        /// 表示所有属性为其数据类型的默认值的 PointD3D 结构的实例。
        /// </summary>
        public static readonly PointD3D Empty = default(PointD3D);

        /// <summary>
        /// 表示所有属性为非数字的 PointD3D 结构的实例。
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

        #region 构造与析构函数

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
        /// 获取表示此 PointD3D 结构是否为 Empty 的布尔值。
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (_X == Empty._X && _Y == Empty._Y && _Z == Empty._Z);
            }
        }

        /// <summary>
        /// 获取表示此 PointD3D 结构是否为 NaN 的布尔值。
        /// </summary>
        public bool IsNaN
        {
            get
            {
                return (double.IsNaN(_X) || double.IsNaN(_Y) || double.IsNaN(_Z));
            }
        }

        /// <summary>
        /// 获取表示此 PointD3D 结构是否为 Infinity 的布尔值。
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return ((!double.IsNaN(_X) && !double.IsNaN(_Y) && !double.IsNaN(_Z)) && (double.IsInfinity(_X) || double.IsInfinity(_Y) || double.IsInfinity(_Z)));
            }
        }

        /// <summary>
        /// 获取表示此 PointD3D 结构是否为 NaN 或 Infinity 的布尔值。
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
        /// 获取此 PointD3D 结构表示的向量的模。
        /// </summary>
        public double VectorModule
        {
            get
            {
                return Math.Sqrt(_X * _X + _Y * _Y + _Z * _Z);
            }
        }

        /// <summary>
        /// 获取此 PointD3D 结构表示的向量的模平方。
        /// </summary>
        public double VectorModuleSquared
        {
            get
            {
                return (_X * _X + _Y * _Y + _Z * _Z);
            }
        }

        /// <summary>
        /// 获取此 PointD3D 结构表示的向量与 +Z 轴之间的夹角（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。
        /// </summary>
        public double VectorAngleZ
        {
            get
            {
                if (_X == 0 && _Y == 0 && _Z == 0)
                {
                    return 0;
                }
                else
                {
                    return Math.Acos(_Z / VectorModule);
                }
            }
        }

        /// <summary>
        /// 获取此 PointD3D 结构表示的向量在 XY 平面内的投影与 +X 轴之间的夹角（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。
        /// </summary>
        public double VectorAngleXY
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

        /// <summary>
        /// 获取此 PointD3D 结构表示的向量的相反向量。
        /// </summary>
        public PointD3D VectorNegate
        {
            get
            {
                return new PointD3D(-_X, -_Y, -_Z);
            }
        }

        /// <summary>
        /// 获取此 PointD3D 结构表示的向量的规范化向量。
        /// </summary>
        public PointD3D VectorNormalize
        {
            get
            {
                double Mod = VectorModule;

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

        #endregion

        #region 方法

        /// <summary>
        /// 按双精度浮点数表示的所有坐标偏移量将此 PointD3D 结构平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的所有坐标偏移量。</param>
        public void Offset(double d)
        {
            _X += d;
            _Y += d;
            _Z += d;
        }

        /// <summary>
        /// 按双精度浮点数表示的 X 坐标偏移量、Y 坐标偏移量与 Z 坐标偏移量将此 PointD3D 结构平移指定的量。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标偏移量。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标偏移量。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标偏移量。</param>
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
            if (pt != null)
            {
                _X += pt.X;
                _Y += pt.Y;
                _Z += pt.Z;
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标偏移量将此 PointD3D 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的所有坐标偏移量。</param>
        public PointD3D OffsetCopy(double d)
        {
            return new PointD3D(_X + d, _Y + d, _Z + d);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标偏移量、Y 坐标偏移量与 Z 坐标偏移量将此 PointD3D 结构的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标偏移量。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标偏移量。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标偏移量。</param>
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
            if (pt != null)
            {
                return new PointD3D(_X + pt.X, _Y + pt.Y, _Z + pt.Z);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的所有坐标缩放因子将此 PointD3D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的所有坐标缩放因子。</param>
        public void Scale(double s)
        {
            _X *= s;
            _Y *= s;
            _Z *= s;
        }

        /// <summary>
        /// 按双精度浮点数表示的 X 坐标缩放因子、Y 坐标缩放因子与 Z 坐标缩放因子将此 PointD3D 结构缩放指定的倍数。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因子。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因子。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因子。</param>
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
            if (pt != null)
            {
                _X *= pt.X;
                _Y *= pt.Y;
                _Z *= pt.Z;
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标缩放因子将此 PointD3D 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的所有坐标缩放因子。</param>
        public PointD3D ScaleCopy(double s)
        {
            return new PointD3D(_X * s, _Y * s, _Z * s);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标缩放因子、Y 坐标缩放因子与 Z 坐标缩放因子将此 PointD3D 结构的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因子。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因子。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因子。</param>
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
            if (pt != null)
            {
                return new PointD3D(_X * pt.X, _Y * pt.Y, _Z * pt.Z);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构绕 X 轴旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕 X 轴旋转的角度（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        public void RotateX(double angle)
        {
            double[,] matrixLeft = new double[4, 4] { { 1, 0, 0, 0 }, { 0, Math.Cos(angle), Math.Sin(angle), 0 }, { 0, -Math.Sin(angle), Math.Cos(angle), 0 }, { 0, 0, 0, 1 } };
            double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
            double[,] result;

            if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构绕 Y 轴旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕 Y 轴旋转的角度（以 +Z 轴为 0 弧度，从 +Z 轴指向 +X 轴的方向为正方向）。</param>
        public void RotateY(double angle)
        {
            double[,] matrixLeft = new double[4, 4] { { Math.Cos(angle), 0, -Math.Sin(angle), 0 }, { 0, 1, 0, 0 }, { Math.Sin(angle), 0, Math.Cos(angle), 0 }, { 0, 0, 0, 1 } };
            double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
            double[,] result;

            if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
            }
        }

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 PointD3D 结构绕 Z 轴旋转指定的角度。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕 Z 轴旋转的角度（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public void RotateZ(double angle)
        {
            double[,] matrixLeft = new double[4, 4] { { Math.Cos(angle), Math.Sin(angle), 0, 0 }, { -Math.Sin(angle), Math.Cos(angle), 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
            double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
            double[,] result;

            if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
            {
                _X = result[0, 0];
                _Y = result[0, 1];
                _Z = result[0, 2];
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构的副本绕 X 轴旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕 X 轴旋转的角度（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        public PointD3D RotateXCopy(double angle)
        {
            double[,] matrixLeft = new double[4, 4] { { 1, 0, 0, 0 }, { 0, Math.Cos(angle), Math.Sin(angle), 0 }, { 0, -Math.Sin(angle), Math.Cos(angle), 0 }, { 0, 0, 0, 1 } };
            double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
            double[,] result;

            if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
            {
                return new PointD3D(result[0, 0], result[0, 1], result[0, 2]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构的副本绕 Y 轴旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕 Y 轴旋转的角度（以 +Z 轴为 0 弧度，从 +Z 轴指向 +X 轴的方向为正方向）。</param>
        public PointD3D RotateYCopy(double angle)
        {
            double[,] matrixLeft = new double[4, 4] { { Math.Cos(angle), 0, -Math.Sin(angle), 0 }, { 0, 1, 0, 0 }, { Math.Sin(angle), 0, Math.Cos(angle), 0 }, { 0, 0, 0, 1 } };
            double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
            double[,] result;

            if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
            {
                return new PointD3D(result[0, 0], result[0, 1], result[0, 2]);
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 PointD3D 结构的副本绕 Z 轴旋转指定的角度的新实例。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示此 PointD 结构绕 Z 轴旋转的角度（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public PointD3D RotateZCopy(double angle)
        {
            double[,] matrixLeft = new double[4, 4] { { Math.Cos(angle), Math.Sin(angle), 0, 0 }, { -Math.Sin(angle), Math.Cos(angle), 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
            double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
            double[,] result;

            if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
            {
                return new PointD3D(result[0, 0], result[0, 1], result[0, 2]);
            }

            return NaN;
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
            if (ex != null && ey != null && ez != null && offset != null)
            {
                double[,] matrixLeft = new double[4, 4] { { ex.X, ex.Y, ex.Z, 0 }, { ey.X, ey.Y, ey.Z, 0 }, { ez.X, ez.Y, ez.Z, 0 }, { offset.X, offset.Y, offset.Z, 1 } };
                double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
                double[,] result;

                if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                    _Z = result[0, 2];
                }
            }
        }

        /// <summary>
        /// 按双精度浮点数数组表示的 4x4 仿射矩阵（左矩阵）将此 PointD3D 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeft">双精度浮点数数组表示的 4x4 仿射矩阵（左矩阵）。</param>
        public void AffineTransform(double[,] matrixLeft)
        {
            if (matrixLeft != null && Matrix2D.GetSize(matrixLeft) == new Size(4, 4))
            {
                double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
                double[,] result;

                if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                    _Z = result[0, 2];
                }
            }
        }

        /// <summary>
        /// 按双精度浮点数数组表示的 4x4 仿射矩阵（左矩阵）列表将此 PointD3D 结构进行仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">双精度浮点数数组表示的 4x4 仿射矩阵（左矩阵）列表。</param>
        public void AffineTransform(List<double[,]> matrixLeftList)
        {
            if (matrixLeftList.Count > 0)
            {
                double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
                double[,] result = null;

                for (int i = 0; i < matrixLeftList.Count; i++)
                {
                    double[,] matrixLeft = matrixLeftList[i];

                    bool flag = (matrixLeft != null && Matrix2D.GetSize(matrixLeft) == new Size(4, 4));

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
            if (ex != null && ey != null && ez != null && offset != null)
            {
                double[,] matrixLeft = new double[4, 4] { { ex.X, ex.Y, ex.Z, 0 }, { ey.X, ey.Y, ey.Z, 0 }, { ez.X, ez.Y, ez.Z, 0 }, { offset.X, offset.Y, offset.Z, 1 } };
                double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
                double[,] result;

                if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
                {
                    return new PointD3D(result[0, 0], result[0, 1], result[0, 2]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数数组表示的 4x4 仿射矩阵（左矩阵）将此 PointD3D 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeft">双精度浮点数数组表示的 4x4 仿射矩阵（左矩阵）。</param>
        public PointD3D AffineTransformCopy(double[,] matrixLeft)
        {
            if (matrixLeft != null && Matrix2D.GetSize(matrixLeft) == new Size(4, 4))
            {
                double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
                double[,] result;

                if (Matrix2D.Multiply(matrixLeft, matrixRight, out result))
                {
                    return new PointD3D(result[0, 0], result[0, 1], result[0, 2]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数数组表示的 4x4 仿射矩阵（左矩阵）列表将此 PointD3D 结构的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeftList">双精度浮点数数组表示的 4x4 仿射矩阵（左矩阵）列表。</param>
        public PointD3D AffineTransformCopy(List<double[,]> matrixLeftList)
        {
            if (matrixLeftList.Count > 0)
            {
                double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
                double[,] result = null;

                for (int i = 0; i < matrixLeftList.Count; i++)
                {
                    double[,] matrixLeft = matrixLeftList[i];

                    bool flag = (matrixLeft != null && Matrix2D.GetSize(matrixLeft) == new Size(4, 4));

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

                return new PointD3D(result[0, 0], result[0, 1], result[0, 2]);
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
            if (ex != null && ey != null && ez != null && offset != null)
            {
                double[,] matrixLeft = new double[4, 4] { { ex.X, ex.Y, ex.Z, 0 }, { ey.X, ey.Y, ey.Z, 0 }, { ez.X, ez.Y, ez.Z, 0 }, { offset.X, offset.Y, offset.Z, 1 } };
                double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
                double[,] result;

                if (Matrix2D.DivideLeft(matrixLeft, matrixRight, out result))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                    _Z = result[0, 2];
                }
            }
        }

        /// <summary>
        /// 按双精度浮点数数组表示的 4x4 仿射矩阵（左矩阵）将此 PointD3D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeft">双精度浮点数数组表示的 4x4 仿射矩阵（左矩阵）。</param>
        public void InverseAffineTransform(double[,] matrixLeft)
        {
            if (matrixLeft != null && Matrix2D.GetSize(matrixLeft) == new Size(4, 4))
            {
                double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
                double[,] result;

                if (Matrix2D.DivideLeft(matrixLeft, matrixRight, out result))
                {
                    _X = result[0, 0];
                    _Y = result[0, 1];
                    _Z = result[0, 2];
                }
            }
        }

        /// <summary>
        /// 按双精度浮点数数组表示的 4x4 仿射矩阵（左矩阵）列表将此 PointD3D 结构进行逆仿射变换。
        /// </summary>
        /// <param name="matrixLeftList">双精度浮点数数组表示的 4x4 仿射矩阵（左矩阵）列表。</param>
        public void InverseAffineTransform(List<double[,]> matrixLeftList)
        {
            if (matrixLeftList.Count > 0)
            {
                double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
                double[,] result = null;

                for (int i = matrixLeftList.Count - 1; i >= 0; i--)
                {
                    double[,] matrixLeft = matrixLeftList[i];

                    bool flag = (matrixLeft != null && Matrix2D.GetSize(matrixLeft) == new Size(4, 4));

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
            if (ex != null && ey != null && ez != null && offset != null)
            {
                double[,] matrixLeft = new double[4, 4] { { ex.X, ex.Y, ex.Z, 0 }, { ey.X, ey.Y, ey.Z, 0 }, { ez.X, ez.Y, ez.Z, 0 }, { offset.X, offset.Y, offset.Z, 1 } };
                double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
                double[,] result;

                if (Matrix2D.DivideLeft(matrixLeft, matrixRight, out result))
                {
                    return new PointD3D(result[0, 0], result[0, 1], result[0, 2]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数数组表示的 4x4 仿射矩阵（左矩阵）将此 PointD3D 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeft">双精度浮点数数组表示的 4x4 仿射矩阵（左矩阵）。</param>
        public PointD3D InverseAffineTransformCopy(double[,] matrixLeft)
        {
            if (matrixLeft != null && Matrix2D.GetSize(matrixLeft) == new Size(4, 4))
            {
                double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
                double[,] result;

                if (Matrix2D.DivideLeft(matrixLeft, matrixRight, out result))
                {
                    return new PointD3D(result[0, 0], result[0, 1], result[0, 2]);
                }
            }

            return NaN;
        }

        /// <summary>
        /// 返回按双精度浮点数数组表示的 4x4 仿射矩阵（左矩阵）列表将此 PointD3D 结构的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrixLeftList">双精度浮点数数组表示的 4x4 仿射矩阵（左矩阵）列表。</param>
        public PointD3D InverseAffineTransformCopy(List<double[,]> matrixLeftList)
        {
            if (matrixLeftList.Count > 0)
            {
                double[,] matrixRight = new double[1, 4] { { _X, _Y, _Z, 1 } };
                double[,] result = null;

                for (int i = matrixLeftList.Count - 1; i >= 0; i--)
                {
                    double[,] matrixLeft = matrixLeftList[i];

                    bool flag = (matrixLeft != null && Matrix2D.GetSize(matrixLeft) == new Size(4, 4));

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

                return new PointD3D(result[0, 0], result[0, 1], result[0, 2]);
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
            if (prjCenter != null && (!double.IsNaN(trueLenDist) && !double.IsInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return XY;
                }
                else
                {
                    if (_Z != prjCenter.Z)
                    {
                        double Scale = trueLenDist / (_Z - prjCenter.Z);

                        if ((!double.IsNaN(Scale) && !double.IsInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * (XY - prjCenter.XY) + prjCenter.XY);
                        }
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
            if (prjCenter != null && (!double.IsNaN(trueLenDist) && !double.IsInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return YZ;
                }
                else
                {
                    if (_X != prjCenter.X)
                    {
                        double Scale = trueLenDist / (_X - prjCenter.X);

                        if ((!double.IsNaN(Scale) && !double.IsInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * (YZ - prjCenter.YZ) + prjCenter.YZ);
                        }
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
            if (prjCenter != null && (!double.IsNaN(trueLenDist) && !double.IsInfinity(trueLenDist)))
            {
                if (trueLenDist == 0)
                {
                    return ZX;
                }
                else
                {
                    if (_Y != prjCenter.Y)
                    {
                        double Scale = trueLenDist / (_Y - prjCenter.Y);

                        if ((!double.IsNaN(Scale) && !double.IsInfinity(Scale)) && Scale > 0)
                        {
                            return (Scale * (ZX - prjCenter.ZX) + prjCenter.ZX);
                        }
                    }
                }
            }

            return PointD.NaN;
        }

        //

        /// <summary>
        /// 返回此 PointD3D 结构与指定的 PointD3D 结构之间的距离。
        /// </summary>
        /// <param name="pt">PointD3D 结构，表示起始点。</param>
        public double DistanceFrom(PointD3D pt)
        {
            if (pt != null)
            {
                double dx = _X - pt.X, dy = _Y - pt.Y, dz = _Z - pt.Z;

                return Math.Sqrt(dx * dx + dy * dy + dz * dz);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回此 PointD3D 结构表示的向量与指定的 PointD3D 结构表示的向量之间的夹角（弧度）。
        /// </summary>
        /// <param name="pt">PointD3D 结构，表示起始向量。</param>
        public double AngleFrom(PointD3D pt)
        {
            if (pt != null)
            {
                if (_X == 0 && _Y == 0 && _Z == 0)
                {
                    _X = 1;
                }

                if (pt.X == 0 && pt.Y == 0 && pt.Z == 0)
                {
                    pt.X = 1;
                }

                double DotProduct = _X * pt.X + _Y * pt.Y + _Z * pt.Z;
                double ModProduct = VectorModule * pt.VectorModule;

                return Math.Acos(DotProduct / ModProduct);
            }

            return double.NaN;
        }

        //

        /// <summary>
        /// 返回将此 PointD3D 结构表示的直角坐标系坐标转换为球坐标系坐标的新实例。
        /// </summary>
        public PointD3D ToSpherical()
        {
            return new PointD3D(VectorModule, VectorAngleZ, VectorAngleXY);
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
        /// 返回将此 PointD3D 结构转换为双精度浮点数数组表示的列向量。
        /// </summary>
        public double[,] ToVectorColumn()
        {
            return new double[1, 3] { { _X, _Y, _Z } };
        }

        /// <summary>
        /// 返回将此 PointD3D 结构转换为双精度浮点数数组表示的行向量。
        /// </summary>
        public double[,] ToVectorRow()
        {
            return new double[3, 1] { { _X }, { _Y }, { _Z } };
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标偏移量将 PointD3D 结构平移指定的量的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="d">双精度浮点数表示的所有坐标偏移量。</param>
        public static double[,] OffsetMatrix(double d)
        {
            return new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { d, d, d, 1 } };
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标偏移量、Y 坐标偏移量与 Z 坐标偏移量将 PointD3D 结构平移指定的量的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="dx">双精度浮点数表示的 X 坐标偏移量。</param>
        /// <param name="dy">双精度浮点数表示的 Y 坐标偏移量。</param>
        /// <param name="dz">双精度浮点数表示的 Z 坐标偏移量。</param>
        public static double[,] OffsetMatrix(double dx, double dy, double dz)
        {
            return new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { dx, dy, dz, 1 } };
        }

        /// <summary>
        /// 返回按 PointD3D 结构将 PointD3D 结构平移指定的量的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于平移 PointD3D 结构。</param>
        public static double[,] OffsetMatrix(PointD3D pt)
        {
            if (pt != null)
            {
                return new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { pt.X, pt.Y, pt.Z, 1 } };
            }

            return null;
        }

        //

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标缩放因子将 PointD3D 结构缩放指定的倍数的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="s">双精度浮点数表示的所有坐标缩放因子。</param>
        public static double[,] ScaleMatrix(double s)
        {
            return new double[4, 4] { { s, 0, 0, 0 }, { 0, s, 0, 0 }, { 0, 0, s, 0 }, { 0, 0, 0, 1 } };
        }

        /// <summary>
        /// 返回按双精度浮点数表示的 X 坐标缩放因子、Y 坐标缩放因子与 Z 坐标缩放因子将 PointD3D 结构缩放指定的倍数的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="sx">双精度浮点数表示的 X 坐标缩放因子。</param>
        /// <param name="sy">双精度浮点数表示的 Y 坐标缩放因子。</param>
        /// <param name="sz">双精度浮点数表示的 Z 坐标缩放因子。</param>
        public static double[,] ScaleMatrix(double sx, double sy, double sz)
        {
            return new double[4, 4] { { sx, 0, 0, 0 }, { 0, sy, 0, 0 }, { 0, 0, sz, 0 }, { 0, 0, 0, 1 } };
        }

        /// <summary>
        /// 返回按 PointD3D 结构将 PointD3D 结构缩放指定的倍数的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于缩放 PointD3D 结构。</param>
        public static double[,] ScaleMatrix(PointD3D pt)
        {
            if (pt != null)
            {
                return new double[4, 4] { { pt.X, 0, 0, 0 }, { 0, pt.Y, 0, 0 }, { 0, 0, pt.Z, 0 }, { 0, 0, 0, 1 } };
            }

            return null;
        }

        //

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD3D 结构绕 X 轴旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示 PointD 结构绕 X 轴旋转的角度（以 +Y 轴为 0 弧度，从 +Y 轴指向 +Z 轴的方向为正方向）。</param>
        public static double[,] RotateXMatrix(double angle)
        {
            return new double[4, 4] { { 1, 0, 0, 0 }, { 0, Math.Cos(angle), Math.Sin(angle), 0 }, { 0, -Math.Sin(angle), Math.Cos(angle), 0 }, { 0, 0, 0, 1 } };
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD3D 结构绕 Y 轴旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示 PointD 结构绕 Y 轴旋转的角度（以 +Z 轴为 0 弧度，从 +Z 轴指向 +X 轴的方向为正方向）。</param>
        public static double[,] RotateYMatrix(double angle)
        {
            return new double[4, 4] { { Math.Cos(angle), 0, -Math.Sin(angle), 0 }, { 0, 1, 0, 0 }, { Math.Sin(angle), 0, Math.Cos(angle), 0 }, { 0, 0, 0, 1 } };
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将 PointD3D 结构绕 Z 轴旋转指定的角度的仿射矩阵（左矩阵）。
        /// </summary>
        /// <param name="angle">双精度浮点数表示的弧度，表示 PointD 结构绕 Z 轴旋转的角度（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        public static double[,] RotateZMatrix(double angle)
        {
            return new double[4, 4] { { Math.Cos(angle), Math.Sin(angle), 0, 0 }, { -Math.Sin(angle), Math.Cos(angle), 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
        }

        //

        /// <summary>
        /// 返回两个 PointD3D 结构之间的距离。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示第一个点。</param>
        /// <param name="right">PointD3D 结构，表示第二个点。</param>
        public static double DistanceBetween(PointD3D left, PointD3D right)
        {
            if (left != null && right != null)
            {
                double dx = left.X - right.X, dy = left.Y - right.Y, dz = left.Z - right.Z;

                return Math.Sqrt(dx * dx + dy * dy + dz * dz);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回此 PointD3D 结构表示的两个向量之间的夹角（弧度）。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示第一个向量。</param>
        /// <param name="right">PointD3D 结构，表示第二个向量。</param>
        public static double AngleBetween(PointD3D left, PointD3D right)
        {
            if (left != null && right != null)
            {
                if (left.X == 0 && left.Y == 0 && left.Z == 0)
                {
                    left.X = 1;
                }

                if (right.X == 0 && right.Y == 0 && right.Z == 0)
                {
                    right.X = 1;
                }

                double DotProduct = left.X * right.X + left.Y * right.Y + left.Z * right.Z;
                double ModProduct = left.VectorModule * right.VectorModule;

                return Math.Acos(DotProduct / ModProduct);
            }

            return double.NaN;
        }

        //

        /// <summary>
        /// 返回 PointD3D 结构表示的两个向量的数量积。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示第一个向量。</param>
        /// <param name="right">PointD3D 结构，表示第二个向量。</param>
        public static double DotProduct(PointD3D left, PointD3D right)
        {
            if (left != null && right != null)
            {
                return (left.X * right.X + left.Y * right.Y + left.Z * right.Z);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回 PointD3D 结构表示的两个向量的向量积，该向量积为一个三维向量，其所有分量的数值依次为 X 基向量、Y 基向量与 Z 基向量的系数。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示左向量。</param>
        /// <param name="right">PointD3D 结构，表示右向量。</param>
        public static PointD3D CrossProduct(PointD3D left, PointD3D right)
        {
            if (left != null && right != null)
            {
                return new PointD3D(left.Y * right.Z - left.Z * right.Y, left.Z * right.X - left.X * right.Z, left.X * right.Y - left.Y * right.X);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量取绝对值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        public static PointD3D Abs(PointD3D pt)
        {
            if (pt != null)
            {
                return new PointD3D(Math.Abs(pt.X), Math.Abs(pt.Y), Math.Abs(pt.Z));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量取符号数得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        public static PointD3D Sign(PointD3D pt)
        {
            if (pt != null)
            {
                return new PointD3D(Math.Sign(pt.X), Math.Sign(pt.Y), Math.Sign(pt.Z));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量舍入到较大的整数值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        public static PointD3D Ceiling(PointD3D pt)
        {
            if (pt != null)
            {
                return new PointD3D(Math.Ceiling(pt.X), Math.Ceiling(pt.Y), Math.Ceiling(pt.Z));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量舍入到较小的整数值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        public static PointD3D Floor(PointD3D pt)
        {
            if (pt != null)
            {
                return new PointD3D(Math.Floor(pt.X), Math.Floor(pt.Y), Math.Floor(pt.Z));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量舍入到最接近的整数值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        public static PointD3D Round(PointD3D pt)
        {
            if (pt != null)
            {
                return new PointD3D(Math.Round(pt.X), Math.Round(pt.Y), Math.Round(pt.Z));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量截断小数部分取整得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        public static PointD3D Truncate(PointD3D pt)
        {
            if (pt != null)
            {
                return new PointD3D(Math.Truncate(pt.X), Math.Truncate(pt.Y), Math.Truncate(pt.Z));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将两个 PointD3D 结构的所有分量分别取最大值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD3D 结构，用于比较的第二个结构。</param>
        public static PointD3D Max(PointD3D left, PointD3D right)
        {
            if (left != null && right != null)
            {
                return new PointD3D(Math.Max(left.X, right.X), Math.Max(left.Y, right.Y), Math.Max(left.Z, right.Z));
            }

            return NaN;
        }

        /// <summary>
        /// 返回将两个 PointD3D 结构的所有分量分别取最小值得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，用于比较的第一个结构。</param>
        /// <param name="right">PointD3D 结构，用于比较的第二个结构。</param>
        public static PointD3D Min(PointD3D left, PointD3D right)
        {
            if (left != null && right != null)
            {
                return new PointD3D(Math.Min(left.X, right.X), Math.Min(left.Y, right.Y), Math.Min(left.Z, right.Z));
            }

            return NaN;
        }

        #endregion

        #region 基类方法

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

            return Equals((PointD3D)obj);
        }

        /// <summary>
        /// 判断此 PointD3D 结构是否与指定的 PointD3D 结构相等。
        /// </summary>
        /// <param name="pointD3D">用于比较的 PointD3D 结构。</param>
        public bool Equals(PointD3D pointD3D)
        {
            if (pointD3D == null)
            {
                return false;
            }

            return (_X == pointD3D._X && _Y == pointD3D._Y && _Z == pointD3D._Z);
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

        #endregion

        #region 运算符

        /// <summary>
        /// 返回在 PointD3D 结构的所有分量前添加正号得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        public static PointD3D operator +(PointD3D pt)
        {
            if (pt != null)
            {
                return new PointD3D(+pt.X, +pt.Y, +pt.Z);
            }

            return NaN;
        }

        /// <summary>
        /// 返回在 PointD3D 结构的所有分量前添加负号得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，用于转换的结构。</param>
        public static PointD3D operator -(PointD3D pt)
        {
            if (pt != null)
            {
                return new PointD3D(-pt.X, -pt.Y, -pt.Z);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量与双精度浮点数相加得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，表示被加数。</param>
        /// <param name="n">双精度浮点数，表示加数。</param>
        public static PointD3D operator +(PointD3D pt, double n)
        {
            if (pt != null)
            {
                return new PointD3D(pt.X + n, pt.Y + n, pt.Z + n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD3D 结构的所有分量相加得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被加数。</param>
        /// <param name="pt">PointD3D 结构，表示加数。</param>
        public static PointD3D operator +(double n, PointD3D pt)
        {
            if (pt != null)
            {
                return new PointD3D(n + pt.X, n + pt.Y, n + pt.Z);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD3D 结构与 PointD3D 结构的所有分量对应相加得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示被加数。</param>
        /// <param name="right">PointD3D 结构，表示加数。</param>
        public static PointD3D operator +(PointD3D left, PointD3D right)
        {
            if (left != null && right != null)
            {
                return new PointD3D(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量与双精度浮点数相减得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，表示被减数。</param>
        /// <param name="n">双精度浮点数，表示减数。</param>
        public static PointD3D operator -(PointD3D pt, double n)
        {
            if (pt != null)
            {
                return new PointD3D(pt.X - n, pt.Y - n, pt.Z - n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD3D 结构的所有分量相减得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被减数。</param>
        /// <param name="pt">PointD3D 结构，表示减数。</param>
        public static PointD3D operator -(double n, PointD3D pt)
        {
            if (pt != null)
            {
                return new PointD3D(n - pt.X, n - pt.Y, n - pt.Z);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD3D 结构与 PointD3D 结构的所有分量对应相减得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示被减数。</param>
        /// <param name="right">PointD3D 结构，表示减数。</param>
        public static PointD3D operator -(PointD3D left, PointD3D right)
        {
            if (left != null && right != null)
            {
                return new PointD3D(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量与双精度浮点数相乘得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，表示被乘数。</param>
        /// <param name="n">双精度浮点数，表示乘数。</param>
        public static PointD3D operator *(PointD3D pt, double n)
        {
            if (pt != null)
            {
                return new PointD3D(pt.X * n, pt.Y * n, pt.Z * n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD3D 结构的所有分量相乘得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被乘数。</param>
        /// <param name="pt">PointD3D 结构，表示乘数。</param>
        public static PointD3D operator *(double n, PointD3D pt)
        {
            if (pt != null)
            {
                return new PointD3D(n * pt.X, n * pt.Y, n * pt.Z);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD3D 结构与 PointD3D 结构的所有分量对应相乘得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示被乘数。</param>
        /// <param name="right">PointD3D 结构，表示乘数。</param>
        public static PointD3D operator *(PointD3D left, PointD3D right)
        {
            if (left != null && right != null)
            {
                return new PointD3D(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 返回将 PointD3D 结构的所有分量与双精度浮点数相除得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="pt">PointD3D 结构，表示被除数。</param>
        /// <param name="n">双精度浮点数，表示除数。</param>
        public static PointD3D operator /(PointD3D pt, double n)
        {
            if (pt != null)
            {
                return new PointD3D(pt.X / n, pt.Y / n, pt.Z / n);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将双精度浮点数与 PointD3D 结构的所有分量相除得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被除数。</param>
        /// <param name="pt">PointD3D 结构，表示除数。</param>
        public static PointD3D operator /(double n, PointD3D pt)
        {
            if (pt != null)
            {
                return new PointD3D(n / pt.X, n / pt.Y, n / pt.Z);
            }

            return NaN;
        }

        /// <summary>
        /// 返回将 PointD3D 结构与 PointD3D 结构的所有分量对应相除得到的 PointD3D 结构的新实例。
        /// </summary>
        /// <param name="left">PointD3D 结构，表示被除数。</param>
        /// <param name="right">PointD3D 结构，表示除数。</param>
        public static PointD3D operator /(PointD3D left, PointD3D right)
        {
            if (left != null && right != null)
            {
                return new PointD3D(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
            }

            return NaN;
        }

        //

        /// <summary>
        /// 判断 PointD3D 结构的两个实例是否相等。
        /// </summary>
        /// <param name="left">PointD3D 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD3D 结构，运算符右侧比较的结构。</param>
        public static bool operator ==(PointD3D left, PointD3D right)
        {
            if (left == null || right == null)
            {
                return false;
            }

            return (left.X == right.X && left.Y == right.Y && left.Z == right.Z);
        }

        /// <summary>
        /// 判断 PointD3D 结构的两个实例是否不相等。
        /// </summary>
        /// <param name="left">PointD3D 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD3D 结构，运算符右侧比较的结构。</param>
        public static bool operator !=(PointD3D left, PointD3D right)
        {
            if (left == null || right == null)
            {
                return true;
            }

            return (left.X != right.X || left.Y != right.Y || left.Z != right.Z);
        }

        /// <summary>
        /// 判断 PointD3D 结构的两个实例表示的向量的模平方是否前者小于后者。
        /// </summary>
        /// <param name="left">PointD3D 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD3D 结构，运算符右侧比较的结构。</param>
        public static bool operator <(PointD3D left, PointD3D right)
        {
            if (left == null || right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared < right.VectorModuleSquared);
        }

        /// <summary>
        /// 判断 PointD3D 结构的两个实例表示的向量的模平方是否前者大于后者。
        /// </summary>
        /// <param name="left">PointD3D 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD3D 结构，运算符右侧比较的结构。</param>
        public static bool operator >(PointD3D left, PointD3D right)
        {
            if (left == null || right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared > right.VectorModuleSquared);
        }

        /// <summary>
        /// 判断 PointD3D 结构的两个实例表示的向量的模平方是否前者小于或等于后者。
        /// </summary>
        /// <param name="left">PointD3D 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD3D 结构，运算符右侧比较的结构。</param>
        public static bool operator <=(PointD3D left, PointD3D right)
        {
            if (left == null || right == null)
            {
                return false;
            }

            return (left.VectorModuleSquared <= right.VectorModuleSquared);
        }

        /// <summary>
        /// 判断 PointD3D 结构的两个实例表示的向量的模平方是否前者大于或等于后者。
        /// </summary>
        /// <param name="left">PointD3D 结构，运算符左侧比较的结构。</param>
        /// <param name="right">PointD3D 结构，运算符右侧比较的结构。</param>
        public static bool operator >=(PointD3D left, PointD3D right)
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