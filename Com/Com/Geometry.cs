/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.Geometry
Version 19.8.10.1600

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Com
{
    /// <summary>
    /// 为几何学的基本计算提供静态方法。
    /// </summary>
    public static class Geometry
    {
        #region 平面直角坐标系

        /// <summary>
        /// 计算平面直角坐标系中过两个定点的直线的一般式方程的参数。
        /// </summary>
        /// <param name="pt1">第一个定点。</param>
        /// <param name="pt2">第二个定点。</param>
        /// <param name="A">直线的一般式方程的第一个参数。</param>
        /// <param name="B">直线的一般式方程的第二个参数。</param>
        /// <param name="C">直线的一般式方程的第三个参数。</param>
        public static void CalcLineGeneralFunction(PointD pt1, PointD pt2, out double A, out double B, out double C)
        {
            if (pt1.IsNaNOrInfinity || pt2.IsNaNOrInfinity || pt1 == pt2)
            {
                A = double.NaN;
                B = double.NaN;
                C = double.NaN;
            }
            else
            {
                A = pt2.Y - pt1.Y;
                B = pt1.X - pt2.X;
                C = pt2.X * pt1.Y - pt1.X * pt2.Y;
            }
        }

        /// <summary>
        /// 计算平面直角坐标系中过一个定点到一条直线的距离。
        /// </summary>
        /// <param name="pt">定点。</param>
        /// <param name="A">直线的一般式方程的第一个参数。</param>
        /// <param name="B">直线的一般式方程的第二个参数。</param>
        /// <param name="C">直线的一般式方程的第三个参数。</param>
        /// <returns>双精度浮点数，表示平面直角坐标系中过一个定点到一条直线的距离。</returns>
        public static double GetDistanceBetweenPointAndLine(PointD pt, double A, double B, double C)
        {
            if (pt.IsNaNOrInfinity || InternalMethod.IsNaNOrInfinity(A) || InternalMethod.IsNaNOrInfinity(B) || InternalMethod.IsNaNOrInfinity(C))
            {
                return double.NaN;
            }
            else
            {
                if (A == 0 && B == 0)
                {
                    return double.NaN;
                }
                else
                {
                    return ((A * pt.X + B * pt.Y + C) / Math.Sqrt(A * A + B * B));
                }
            }
        }

        /// <summary>
        /// 计算平面直角坐标系中过一个定点到一条直线的距离。
        /// </summary>
        /// <param name="pt">定点。</param>
        /// <param name="pt1">直线上的第一个点。</param>
        /// <param name="pt2">直线上的第二个点。</param>
        /// <returns>双精度浮点数，表示平面直角坐标系中过一个定点到一条直线的距离。</returns>
        public static double GetDistanceBetweenPointAndLine(PointD pt, PointD pt1, PointD pt2)
        {
            if (pt.IsNaNOrInfinity || pt1.IsNaNOrInfinity || pt2.IsNaNOrInfinity)
            {
                return double.NaN;
            }
            else
            {
                if (pt1 == pt2)
                {
                    return pt.DistanceFrom(pt1);
                }
                else
                {
                    double A, B, C;

                    CalcLineGeneralFunction(pt1, pt2, out A, out B, out C);

                    return GetDistanceBetweenPointAndLine(pt, A, B, C);
                }
            }
        }

        /// <summary>
        /// 计算平面直角坐标系中过一个定点到一条直线的垂足。
        /// </summary>
        /// <param name="pt">定点。</param>
        /// <param name="pt1">直线上的第一个点。</param>
        /// <param name="pt2">直线上的第二个点。</param>
        /// <returns>PointD 结构，表示平面直角坐标系中过一个定点到一条直线的垂足。</returns>
        public static PointD GetFootPoint(PointD pt, PointD pt1, PointD pt2)
        {
            if (pt.IsNaNOrInfinity || pt1.IsNaNOrInfinity || pt2.IsNaNOrInfinity)
            {
                return PointD.NaN;
            }
            else
            {
                PointD foot = new PointD();

                if (pt1 == pt2)
                {
                    foot = pt1;
                }
                else if (pt1.X == pt2.X)
                {
                    foot.X = pt1.X;
                    foot.Y = pt.Y;
                }
                else if (pt1.Y == pt2.Y)
                {
                    foot.X = pt.X;
                    foot.Y = pt1.Y;
                }
                else
                {
                    double K = (pt2.Y - pt1.Y) / (pt2.X - pt1.X);

                    foot.X = (pt.Y - pt1.Y + K * pt1.X + pt.X / K) / (K + 1 / K);
                    foot.Y = pt.Y - (foot.X - pt.X) / K;
                }

                return foot;
            }
        }

        #endregion

        #region 角度

        /// <summary>
        /// 计算平面直角坐标系中由指定的起点与终点确定的向量与 +X 轴之间的夹角（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。
        /// </summary>
        /// <param name="pt1">向量的起点。</param>
        /// <param name="pt2">向量的终点。</param>
        /// <returns>双精度浮点数，表示平面直角坐标系中由指定的起点与终点确定的向量与 +X 轴之间的夹角（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</returns>
        public static double GetAngleOfTwoPoints(PointD pt1, PointD pt2)
        {
            if (pt1.IsNaNOrInfinity || pt2.IsNaNOrInfinity)
            {
                return double.NaN;
            }
            else
            {
                return (pt2 - pt1).Azimuth;
            }
        }

        /// <summary>
        /// 将一个角度（弧度）映射到 [0, 2 * PI) 区间。
        /// </summary>
        /// <param name="angle">角度（弧度）。</param>
        /// <returns>双精度浮点数，表示将一个角度（弧度）映射到 [0, 2 * PI) 区间得到的结果。</returns>
        public static double AngleMapping(double angle)
        {
            if (InternalMethod.IsNaNOrInfinity(angle))
            {
                return double.NaN;
            }
            else
            {
                if (angle >= 0 && angle < Constant.DoublePi)
                {
                    return angle;
                }
                else
                {
                    return (angle - Math.Floor(angle / Constant.DoublePi) * Constant.DoublePi);
                }
            }
        }

        #endregion

        #region 控件

        /// <summary>
        /// 获取鼠标相对于控件的坐标。
        /// </summary>
        /// <param name="ctrl">控件。</param>
        /// <returns>Point 结构，表示鼠标相对于控件的坐标。</returns>
        public static Point GetCursorPositionOfControl(Control ctrl)
        {
            if (ctrl == null)
            {
                return Point.Empty;
            }
            else
            {
                return ctrl.PointToClient(Cursor.Position);
            }
        }

        /// <summary>
        /// 判断鼠标指针是否在控件内部。
        /// </summary>
        /// <param name="ctrl">控件。</param>
        /// <returns>布尔值，表示鼠标指针是否在控件内部。</returns>
        public static bool CursorIsInControl(Control ctrl)
        {
            if (ctrl == null)
            {
                return false;
            }
            else
            {
                Point PTC = ctrl.PointToClient(Cursor.Position);

                return (PTC.X >= 0 && PTC.X < ctrl.Width && PTC.Y >= 0 && PTC.Y < ctrl.Height);
            }
        }

        /// <summary>
        /// 判断控件坐标系中的一个点是否在控件内部。
        /// </summary>
        /// <param name="pt">控件坐标系中的点。</param>
        /// <param name="ctrl">控件。</param>
        /// <returns>布尔值，表示控件坐标系中的一个点是否在控件内部。</returns>
        public static bool PointIsInControl(Point pt, Control ctrl)
        {
            if (ctrl == null)
            {
                return false;
            }
            else
            {
                return (pt.X >= 0 && pt.X < ctrl.Width && pt.Y >= 0 && pt.Y < ctrl.Height);
            }
        }

        /// <summary>
        /// 判断屏幕坐标系中的一个点是否在控件内部。
        /// </summary>
        /// <param name="pt">屏幕坐标系中的点。</param>
        /// <param name="ctrl">控件。</param>
        /// <returns>布尔值，表示屏幕坐标系中的一个点是否在控件内部。</returns>
        public static bool ScreenPointIsInControl(Point pt, Control ctrl)
        {
            if (ctrl == null)
            {
                return false;
            }
            else
            {
                Point PTC = ctrl.PointToClient(pt);

                return (PTC.X >= 0 && PTC.X < ctrl.Width && PTC.Y >= 0 && PTC.Y < ctrl.Height);
            }
        }

        /// <summary>
        /// 计算同一容器中一组控件的最小外接矩形。
        /// </summary>
        /// <param name="ctrls">控件数组。</param>
        /// <param name="edgeDist">矩形的每边到控件边缘的距离。</param>
        /// <returns>Point 结构，表示同一容器中一组控件的最小外接矩形。</returns>
        public static Rectangle GetMinimumBoundingRectangleOfControls(Control[] ctrls, int edgeDist)
        {
            if (InternalMethod.IsNullOrEmpty(ctrls))
            {
                return Rectangle.Empty;
            }
            else
            {
                int L = int.MaxValue, R = int.MinValue, T = int.MaxValue, B = int.MinValue;

                foreach (Control Ctrl in ctrls)
                {
                    if (Ctrl != null)
                    {
                        L = Math.Min(L, Ctrl.Left);
                        T = Math.Min(T, Ctrl.Top);
                        R = Math.Max(R, Ctrl.Right);
                        B = Math.Max(B, Ctrl.Bottom);
                    }
                }

                edgeDist = Math.Max(0, edgeDist);

                return Rectangle.FromLTRB(L - edgeDist, T - edgeDist, R + edgeDist - 1, B + edgeDist - 1);
            }
        }

        /// <summary>
        /// 计算同一容器中一组控件的最小外接矩形。
        /// </summary>
        /// <param name="ctrls">控件数组。</param>
        /// <returns>Point 结构，表示同一容器中一组控件的最小外接矩形。</returns>
        public static Rectangle GetMinimumBoundingRectangleOfControls(Control[] ctrls)
        {
            if (InternalMethod.IsNullOrEmpty(ctrls))
            {
                return Rectangle.Empty;
            }
            else
            {
                return GetMinimumBoundingRectangleOfControls(ctrls, 0);
            }
        }

        #endregion

        #region 图形可见性

        /// <summary>
        /// 判断在一个矩形内部能否看到一个点。
        /// </summary>
        /// <param name="pt">点。</param>
        /// <param name="rect">矩形。</param>
        /// <returns>布尔值，表示在一个矩形内部能否看到一个点。</returns>
        public static bool PointIsVisibleInRectangle(PointD pt, RectangleF rect)
        {
            if (pt.IsNaNOrInfinity || rect.Size.IsEmpty)
            {
                return false;
            }
            else
            {
                return (pt.X >= rect.X && pt.X < rect.Right && pt.Y >= rect.Y && pt.Y < rect.Bottom);
            }
        }

        /// <summary>
        /// 判断在一个圆内部能否看到一个点。
        /// </summary>
        /// <param name="pt">点。</param>
        /// <param name="offset">圆的圆心。</param>
        /// <param name="radius">圆的半径。</param>
        /// <returns>布尔值，表示在一个圆内部能否看到一个点。</returns>
        public static bool PointIsVisibleInCircle(PointD pt, PointD offset, double radius)
        {
            if (pt.IsNaNOrInfinity || offset.IsNaNOrInfinity || (InternalMethod.IsNaNOrInfinity(radius) || radius <= 0))
            {
                return false;
            }
            else
            {
                return (pt.DistanceFrom(offset) < radius);
            }
        }

        /// <summary>
        /// 判断在一个椭圆内部能否看到一个点。
        /// </summary>
        /// <param name="pt">点。</param>
        /// <param name="offset">椭圆的焦点。</param>
        /// <param name="semiMajorAxis">椭圆的半长轴。</param>
        /// <param name="eccentricity">椭圆的离心率。</param>
        /// <param name="rotateAngle">椭圆的旋转角（弧度）（以焦点为中心，以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向，焦点到近焦点连线相对于 +X 轴的角度）。</param>
        /// <returns>布尔值，表示在一个椭圆内部能否看到一个点。</returns>
        public static bool PointIsVisibleInEllipse(PointD pt, PointD offset, double semiMajorAxis, double eccentricity, double rotateAngle)
        {
            if (pt.IsNaNOrInfinity || offset.IsNaNOrInfinity || (InternalMethod.IsNaNOrInfinity(semiMajorAxis) || semiMajorAxis <= 0) || (InternalMethod.IsNaNOrInfinity(eccentricity) || eccentricity < 0 || eccentricity >= 1) || InternalMethod.IsNaNOrInfinity(rotateAngle))
            {
                return false;
            }
            else
            {
                PointD _Offset = new PointD(offset.X - 2 * semiMajorAxis * eccentricity * Math.Cos(rotateAngle), offset.Y - 2 * semiMajorAxis * eccentricity * Math.Sin(rotateAngle));

                double FocalDistSum = pt.DistanceFrom(offset) + pt.DistanceFrom(_Offset);

                return (FocalDistSum < 2 * semiMajorAxis);
            }
        }

        /// <summary>
        /// 判断在一个菱形内部能否看到一个点。
        /// </summary>
        /// <param name="pt">点。</param>
        /// <param name="offset">菱形的中心。</param>
        /// <param name="semiMajorAxis">菱形的半长轴。</param>
        /// <param name="semiMinorAxis">菱形的半短轴。</param>
        /// <param name="rotateAngle">菱形的旋转角（弧度）（以焦点为中心，以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向，半长轴相对于 +X 轴的角度）。</param>
        /// <returns>布尔值，表示在一个菱形内部能否看到一个点。</returns>
        public static bool PointIsVisibleInRhombus(PointD pt, PointD offset, double semiMajorAxis, double semiMinorAxis, double rotateAngle)
        {
            if (pt.IsNaNOrInfinity || offset.IsNaNOrInfinity || (InternalMethod.IsNaNOrInfinity(semiMajorAxis) || semiMajorAxis <= 0) || (InternalMethod.IsNaNOrInfinity(semiMinorAxis) || semiMinorAxis <= 0) || InternalMethod.IsNaNOrInfinity(rotateAngle))
            {
                return false;
            }
            else
            {
                pt.Rotate(-rotateAngle, offset);

                return (Math.Abs(pt.X - offset.X) * semiMinorAxis + Math.Abs(pt.Y - offset.Y) * semiMajorAxis < semiMajorAxis * semiMinorAxis);
            }
        }

        /// <summary>
        /// 判断在一个矩形内部能否看到一个直线段的部分或全部。
        /// </summary>
        /// <param name="pt1">直线段的第一个端点。</param>
        /// <param name="pt2">直线段的第二个端点。</param>
        /// <param name="rect">矩形。</param>
        /// <returns>布尔值，表示在一个矩形内部能否看到一个直线段的部分或全部。</returns>
        public static bool LineIsVisibleInRectangle(PointD pt1, PointD pt2, RectangleF rect)
        {
            if (pt1.IsNaNOrInfinity || pt2.IsNaNOrInfinity || rect.Size.IsEmpty)
            {
                return false;
            }
            else
            {
                PointD RectCenter = new PointD(rect.Width / 2, rect.Height / 2);

                double RectRadius = Math.Sqrt(Math.Pow(rect.Width, 2) + Math.Pow(rect.Height, 2)) / 2;

                double Dist_RC_P1 = RectCenter.DistanceFrom(pt1);
                double Dist_RC_P2 = RectCenter.DistanceFrom(pt2);

                if (Dist_RC_P1 <= RectRadius || Dist_RC_P2 <= RectRadius)
                {
                    return true;
                }
                else
                {
                    double A, B, C;

                    CalcLineGeneralFunction(pt1, pt2, out A, out B, out C);

                    double Dist_RC_FP = GetDistanceBetweenPointAndLine(RectCenter, A, B, C);

                    if (Dist_RC_FP > RectRadius)
                    {
                        return false;
                    }
                    else
                    {
                        if ((A * pt1.X + B * pt1.Y + C) * (A * pt2.X + B * pt2.Y + C) > 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 判断在一个圆内部能否看到一个直线段的部分或全部。
        /// </summary>
        /// <param name="pt1">直线段的第一个端点。</param>
        /// <param name="pt2">直线段的第二个端点。</param>
        /// <param name="offset">圆的圆心。</param>
        /// <param name="radius">圆的半径。</param>
        /// <returns>布尔值，表示在一个圆内部能否看到一个直线段的部分或全部。</returns>
        public static bool LineIsVisibleInCircle(PointD pt1, PointD pt2, PointD offset, double radius)
        {
            if (pt1.IsNaNOrInfinity || pt2.IsNaNOrInfinity || offset.IsNaNOrInfinity || (InternalMethod.IsNaNOrInfinity(radius) || radius <= 0))
            {
                return false;
            }
            else
            {
                double Dist_Off_P1 = offset.DistanceFrom(pt1);
                double Dist_Off_P2 = offset.DistanceFrom(pt2);

                if (Dist_Off_P1 <= radius || Dist_Off_P2 <= radius)
                {
                    return true;
                }
                else
                {
                    double A, B, C;

                    CalcLineGeneralFunction(pt1, pt2, out A, out B, out C);

                    double Dist_Off_FP = GetDistanceBetweenPointAndLine(offset, A, B, C);

                    if (Dist_Off_FP > radius)
                    {
                        return false;
                    }
                    else
                    {
                        if ((A * pt1.X + B * pt1.Y + C) * (A * pt2.X + B * pt2.Y + C) > 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 判断在一个矩形内部能否看到一个圆的内部或者圆周的部分或全部。
        /// </summary>
        /// <param name="offset">圆的圆心。</param>
        /// <param name="radius">圆的半径。</param>
        /// <param name="rect">矩形。</param>
        /// <returns>布尔值，表示在一个矩形内部能否看到一个圆的内部或者圆周的部分或全部。</returns>
        public static bool CircleInnerIsVisibleInRectangle(PointD offset, double radius, RectangleF rect)
        {
            if (offset.IsNaNOrInfinity || (InternalMethod.IsNaNOrInfinity(radius) || radius <= 0) || rect.Size.IsEmpty)
            {
                return false;
            }
            else
            {
                if (offset.X < rect.X && offset.Y < rect.Y)
                {
                    if (Math.Pow(offset.X - rect.X, 2) + Math.Pow(offset.Y - rect.Y, 2) < Math.Pow(radius, 2))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (offset.X > rect.Right && offset.Y < rect.Y)
                {
                    if (Math.Pow(offset.X - rect.Right, 2) + Math.Pow(offset.Y - rect.Y, 2) < Math.Pow(radius, 2))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (offset.X < rect.X && offset.Y > rect.Bottom)
                {
                    if (Math.Pow(offset.X - rect.X, 2) + Math.Pow(offset.Y - rect.Bottom, 2) < Math.Pow(radius, 2))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (offset.X > rect.Right && offset.Y > rect.Bottom)
                {
                    if (Math.Pow(offset.X - rect.Right, 2) + Math.Pow(offset.Y - rect.Bottom, 2) < Math.Pow(radius, 2))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (offset.X >= rect.X - radius && offset.X <= rect.Right + radius && offset.Y >= rect.Y - radius && offset.Y <= rect.Bottom + radius)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 判断在一个矩形内部能否看到一个圆的圆周的部分或全部。
        /// </summary>
        /// <param name="offset">圆的圆心。</param>
        /// <param name="radius">圆的半径。</param>
        /// <param name="rect">矩形。</param>
        /// <returns>布尔值，表示在一个矩形内部能否看到一个圆的圆周的部分或全部。</returns>
        public static bool CircumferenceIsVisibleInRectangle(PointD offset, double radius, RectangleF rect)
        {
            if (offset.IsNaNOrInfinity || (InternalMethod.IsNaNOrInfinity(radius) || radius <= 0) || rect.Size.IsEmpty)
            {
                return false;
            }
            else
            {
                if (offset.X < rect.X && offset.Y < rect.Y)
                {
                    if (Math.Pow(offset.X - rect.X, 2) + Math.Pow(offset.Y - rect.Y, 2) < Math.Pow(radius, 2) && Math.Pow(offset.X - rect.Right, 2) + Math.Pow(offset.Y - rect.Bottom, 2) > Math.Pow(radius, 2))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (offset.X > rect.Right && offset.Y < rect.Y)
                {
                    if (Math.Pow(offset.X - rect.Right, 2) + Math.Pow(offset.Y - rect.Y, 2) < Math.Pow(radius, 2) && Math.Pow(offset.X - rect.X, 2) + Math.Pow(offset.Y - rect.Bottom, 2) > Math.Pow(radius, 2))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (offset.X < rect.X && offset.Y > rect.Bottom)
                {
                    if (Math.Pow(offset.X - rect.X, 2) + Math.Pow(offset.Y - rect.Bottom, 2) < Math.Pow(radius, 2) && Math.Pow(offset.X - rect.Right, 2) + Math.Pow(offset.Y - rect.Y, 2) > Math.Pow(radius, 2))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (offset.X > rect.Right && offset.Y > rect.Bottom)
                {
                    if (Math.Pow(offset.X - rect.Right, 2) + Math.Pow(offset.Y - rect.Bottom, 2) < Math.Pow(radius, 2) && Math.Pow(offset.X - rect.X, 2) + Math.Pow(offset.Y - rect.Y, 2) > Math.Pow(radius, 2))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (2 * radius >= Math.Sqrt(Math.Pow(rect.Width, 2) + Math.Pow(rect.Height, 2)))
                    {
                        if (((offset.X >= rect.X - radius && offset.X <= rect.Right + radius) && (offset.Y >= rect.Y - radius && offset.Y <= rect.Bottom + radius)) && !((offset.X >= rect.Right - Constant.HalfSqrt2 * radius && offset.X <= rect.X + Constant.HalfSqrt2 * radius) && (offset.Y >= rect.Bottom - Constant.HalfSqrt2 * radius && offset.Y <= rect.Y + Constant.HalfSqrt2 * radius)))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (offset.X >= rect.X - radius && offset.X <= rect.Right + radius && offset.Y >= rect.Y - radius && offset.Y <= rect.Bottom + radius)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        #endregion

        #region 圆锥曲线

        /// <summary>
        /// 计算椭圆在指定相位的半径。
        /// </summary>
        /// <param name="semiMajorAxis">椭圆的半长轴。</param>
        /// <param name="eccentricity">椭圆的离心率。</param>
        /// <param name="phase">椭圆的相位（弧度）（以近焦点相位为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <returns>双精度浮点数，表示椭圆在指定相位的半径。</returns>
        public static double GetRadiusOfEllipse(double semiMajorAxis, double eccentricity, double phase)
        {
            if ((InternalMethod.IsNaNOrInfinity(semiMajorAxis) || semiMajorAxis <= 0) || (InternalMethod.IsNaNOrInfinity(eccentricity) || eccentricity < 0 || eccentricity >= 1) || InternalMethod.IsNaNOrInfinity(phase))
            {
                return double.NaN;
            }
            else
            {
                return (semiMajorAxis * Math.Sqrt(Math.Pow(Math.Cos(phase), 2) + (1 - Math.Pow(eccentricity, 2)) * Math.Pow(Math.Sin(phase), 2)));
            }
        }

        /// <summary>
        /// 计算椭圆在指定相位的焦半径。
        /// </summary>
        /// <param name="semiMajorAxis">椭圆的半长轴。</param>
        /// <param name="eccentricity">椭圆的离心率。</param>
        /// <param name="phase">椭圆的相位（弧度）（以近焦点相位为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <returns>双精度浮点数，表示椭圆在指定相位的焦半径。</returns>
        public static double GetFocalRadiusOfEllipse(double semiMajorAxis, double eccentricity, double phase)
        {
            if ((InternalMethod.IsNaNOrInfinity(semiMajorAxis) || semiMajorAxis <= 0) || (InternalMethod.IsNaNOrInfinity(eccentricity) || eccentricity < 0 || eccentricity >= 1) || InternalMethod.IsNaNOrInfinity(phase))
            {
                return double.NaN;
            }
            else
            {
                return (semiMajorAxis * Math.Sqrt(Math.Pow(Math.Cos(phase) - eccentricity, 2) + (1 - Math.Pow(eccentricity, 2)) * Math.Pow(Math.Sin(phase), 2)));
            }
        }

        /// <summary>
        /// 将椭圆的圆心角转换为相位（弧度）（以近焦点相位为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。
        /// </summary>
        /// <param name="centralAngle">椭圆的圆心角（弧度）。</param>
        /// <param name="eccentricity">椭圆的离心率。</param>
        /// <returns>双精度浮点数，表示将椭圆的圆心角转换为相位（弧度）（以近焦点相位为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）得到的结果。</returns>
        public static double EllipseCentralAngleToPhase(double centralAngle, double eccentricity)
        {
            if (InternalMethod.IsNaNOrInfinity(centralAngle) || (InternalMethod.IsNaNOrInfinity(eccentricity) || eccentricity < 0 || eccentricity >= 1))
            {
                return double.NaN;
            }
            else
            {
                return (Math.Atan(Math.Tan(centralAngle) / Math.Sqrt(1 - Math.Pow(eccentricity, 2))) + Math.Round(centralAngle / Constant.Pi) * Constant.Pi);
            }
        }

        #endregion

        #region 位图

        /// <summary>
        /// 将位图的副本按顺时针方向旋转一个角度，返回旋转后得到的位图。
        /// </summary>
        /// <param name="bmp">用于旋转的位图。</param>
        /// <param name="rotateAngle">按顺时针方向旋转的角度（弧度）。</param>
        /// <param name="antiAlias">是否使用抗锯齿模式绘图。</param>
        /// <returns>Bitmap 对象，表示将位图的副本按顺时针方向旋转一个角度得到的结果。</returns>
        public static Bitmap RotateBitmap(Bitmap bmp, double rotateAngle, bool antiAlias)
        {
            if (bmp == null || InternalMethod.IsNaNOrInfinity(rotateAngle))
            {
                return null;
            }
            else
            {
                int W = bmp.Width + 2;
                int H = bmp.Height + 2;

                using (Bitmap Bmp = new Bitmap(W, H, PixelFormat.Format32bppArgb))
                {
                    using (Graphics Grph = Graphics.FromImage(Bmp))
                    {
                        if (antiAlias)
                        {
                            Grph.SmoothingMode = SmoothingMode.AntiAlias;
                        }

                        //

                        Grph.DrawImage(bmp, new Point(1, 1));
                    }

                    RectangleF Rect = new RectangleF();

                    using (GraphicsPath Path = new GraphicsPath())
                    {
                        Path.AddRectangle(new RectangleF(0, 0, W, H));

                        System.Drawing.Drawing2D.Matrix Mtrx = new System.Drawing.Drawing2D.Matrix();
                        Mtrx.Rotate((float)(rotateAngle * Constant.DegsPerRad));

                        Rect = Path.GetBounds(Mtrx);
                    }

                    Bitmap Dst = new Bitmap((int)Rect.Width, (int)Rect.Height, PixelFormat.Format32bppArgb);

                    using (Graphics Grph = Graphics.FromImage(Dst))
                    {
                        if (antiAlias)
                        {
                            Grph.SmoothingMode = SmoothingMode.AntiAlias;
                        }

                        //

                        Grph.TranslateTransform(-Rect.X, -Rect.Y);
                        Grph.RotateTransform((float)(rotateAngle * Constant.DegsPerRad));
                        Grph.InterpolationMode = InterpolationMode.HighQualityBilinear;
                        Grph.DrawImage(Bmp, new Point(0, 0));
                    }

                    return Dst;
                }
            }
        }

        #endregion

        #region 路径

        /// <summary>
        /// 创建一个表示圆角矩形的路径，此圆角矩形包含 4 个半径相同的圆角。
        /// </summary>
        /// <param name="rect">矩形。</param>
        /// <param name="cornerRadius">圆角的半径。</param>
        /// <returns>GraphicsPath 对象，表示圆角矩形的路径，此圆角矩形包含 4 个半径相同的圆角。</returns>
        public static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius)
        {
            GraphicsPath RoundedRect = new GraphicsPath();

            cornerRadius = Math.Max(0, Math.Min(Math.Min(rect.Width, rect.Height) / 2, cornerRadius));

            if (cornerRadius > 0)
            {
                RoundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180F, 90F);
                RoundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius, rect.Y);
                RoundedRect.AddArc(rect.Right - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270F, 90F);
                RoundedRect.AddLine(rect.Right, rect.Y + cornerRadius, rect.Right, rect.Bottom - cornerRadius);
                RoundedRect.AddArc(rect.Right - cornerRadius * 2, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0F, 90F);
                RoundedRect.AddLine(rect.Right - cornerRadius, rect.Bottom, rect.X + cornerRadius, rect.Bottom);
                RoundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90F, 90F);
                RoundedRect.AddLine(rect.X, rect.Bottom - cornerRadius, rect.X, rect.Y + cornerRadius);
            }
            else
            {
                RoundedRect.AddRectangle(rect);
            }

            RoundedRect.CloseFigure();

            return RoundedRect;
        }

        /// <summary>
        /// 创建一个表示圆角矩形的路径，此圆角矩形包含 4 个半径不同的圆角。
        /// </summary>
        /// <param name="rect">矩形。</param>
        /// <param name="cornerRadiusLT">左上圆角的半径。</param>
        /// <param name="cornerRadiusRT">右上圆角的半径。</param>
        /// <param name="cornerRadiusRB">右下圆角的半径。</param>
        /// <param name="cornerRadiusLB">左下圆角的半径。</param>
        /// <returns>GraphicsPath 对象，表示圆角矩形的路径，此圆角矩形包含 4 个半径不同的圆角。</returns>
        public static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadiusLT, int cornerRadiusRT, int cornerRadiusRB, int cornerRadiusLB)
        {
            GraphicsPath RoundedRect = new GraphicsPath();

            cornerRadiusLT = Math.Max(0, Math.Min(Math.Min(rect.Width, rect.Height) / 2, cornerRadiusLT));
            cornerRadiusRT = Math.Max(0, Math.Min(Math.Min(rect.Width, rect.Height) / 2, cornerRadiusRT));
            cornerRadiusRB = Math.Max(0, Math.Min(Math.Min(rect.Width, rect.Height) / 2, cornerRadiusRB));
            cornerRadiusLB = Math.Max(0, Math.Min(Math.Min(rect.Width, rect.Height) / 2, cornerRadiusLB));

            if (cornerRadiusLT > 0)
            {
                RoundedRect.AddArc(rect.X, rect.Y, cornerRadiusLT * 2, cornerRadiusLT * 2, 180F, 90F);
            }

            RoundedRect.AddLine(rect.X + cornerRadiusLT, rect.Y, rect.Right - cornerRadiusLT, rect.Y);

            if (cornerRadiusRT > 0)
            {
                RoundedRect.AddArc(rect.Right - cornerRadiusRT * 2, rect.Y, cornerRadiusRT * 2, cornerRadiusRT * 2, 270F, 90F);
            }

            RoundedRect.AddLine(rect.Right, rect.Y + cornerRadiusRT, rect.Right, rect.Bottom - cornerRadiusRT);

            if (cornerRadiusRB > 0)
            {
                RoundedRect.AddArc(rect.Right - cornerRadiusRB * 2, rect.Bottom - cornerRadiusRB * 2, cornerRadiusRB * 2, cornerRadiusRB * 2, 0F, 90F);
            }

            RoundedRect.AddLine(rect.Right - cornerRadiusRB, rect.Bottom, rect.X + cornerRadiusRB, rect.Bottom);

            if (cornerRadiusLB > 0)
            {
                RoundedRect.AddArc(rect.X, rect.Bottom - cornerRadiusLB * 2, cornerRadiusLB * 2, cornerRadiusLB * 2, 90F, 90F);
            }

            RoundedRect.AddLine(rect.X, rect.Bottom - cornerRadiusLB, rect.X, rect.Y + cornerRadiusLB);

            RoundedRect.CloseFigure();

            return RoundedRect;
        }

        /// <summary>
        /// 创建一组表示矩形减去圆角矩形所剩区域的路径，此圆角矩形包含 4 个半径相同的圆角。
        /// </summary>
        /// <param name="rect">矩形。</param>
        /// <param name="cornerRadius">圆角的半径。</param>
        /// <returns>GraphicsPath 对象数组，数组元素表示矩形减去圆角矩形所剩区域的路径，此圆角矩形包含 4 个半径相同的圆角。</returns>
        public static GraphicsPath[] CreateRoundedRectangleOuterPaths(Rectangle rect, int cornerRadius)
        {
            GraphicsPath[] RoundedRectOuter = new GraphicsPath[4] { new GraphicsPath(), new GraphicsPath(), new GraphicsPath(), new GraphicsPath() };

            cornerRadius = Math.Max(0, Math.Min(Math.Min(rect.Width, rect.Height) / 2, cornerRadius));

            if (cornerRadius > 0)
            {
                RoundedRectOuter[0].AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180F, 90F);
                RoundedRectOuter[0].AddLine(rect.X + cornerRadius, rect.Y, rect.X, rect.Y);
                RoundedRectOuter[0].AddLine(rect.X, rect.Y, rect.X, rect.Y + cornerRadius);
                RoundedRectOuter[0].CloseFigure();

                RoundedRectOuter[1].AddArc(rect.Right - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270F, 90F);
                RoundedRectOuter[1].AddLine(rect.Right, rect.Y + cornerRadius, rect.Right, rect.Y);
                RoundedRectOuter[1].AddLine(rect.Right, rect.Y, rect.Right - cornerRadius, rect.Y);
                RoundedRectOuter[1].CloseFigure();

                RoundedRectOuter[2].AddArc(rect.Right - cornerRadius * 2, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0F, 90F);
                RoundedRectOuter[2].AddLine(rect.Right - cornerRadius, rect.Bottom, rect.Right, rect.Bottom);
                RoundedRectOuter[2].AddLine(rect.Right, rect.Bottom, rect.Right, rect.Bottom - cornerRadius);
                RoundedRectOuter[2].CloseFigure();

                RoundedRectOuter[3].AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90F, 90F);
                RoundedRectOuter[3].AddLine(rect.X, rect.Bottom - cornerRadius, rect.X, rect.Bottom);
                RoundedRectOuter[3].AddLine(rect.X, rect.Bottom, rect.X + cornerRadius, rect.Bottom);
                RoundedRectOuter[3].CloseFigure();
            }

            return RoundedRectOuter;
        }

        /// <summary>
        /// 创建一组表示矩形减去圆角矩形所剩区域的路径，此圆角矩形包含 4 个半径不同的圆角。
        /// </summary>
        /// <param name="rect">矩形。</param>
        /// <param name="cornerRadiusLT">左上圆角的半径。</param>
        /// <param name="cornerRadiusRT">右上圆角的半径。</param>
        /// <param name="cornerRadiusRB">右下圆角的半径。</param>
        /// <param name="cornerRadiusLB">左下圆角的半径。</param>
        /// <returns>GraphicsPath 对象数组，数组元素表示矩形减去圆角矩形所剩区域的路径，此圆角矩形包含 4 个半径不同的圆角。</returns>
        public static GraphicsPath[] CreateRoundedRectangleOuterPaths(Rectangle rect, int cornerRadiusLT, int cornerRadiusRT, int cornerRadiusRB, int cornerRadiusLB)
        {
            GraphicsPath[] RoundedRectOuter = new GraphicsPath[4] { new GraphicsPath(), new GraphicsPath(), new GraphicsPath(), new GraphicsPath() };

            cornerRadiusLT = Math.Max(0, Math.Min(Math.Min(rect.Width, rect.Height) / 2, cornerRadiusLT));
            cornerRadiusRT = Math.Max(0, Math.Min(Math.Min(rect.Width, rect.Height) / 2, cornerRadiusRT));
            cornerRadiusRB = Math.Max(0, Math.Min(Math.Min(rect.Width, rect.Height) / 2, cornerRadiusRB));
            cornerRadiusLB = Math.Max(0, Math.Min(Math.Min(rect.Width, rect.Height) / 2, cornerRadiusLB));

            if (cornerRadiusLT > 0)
            {
                RoundedRectOuter[0].AddArc(rect.X, rect.Y, cornerRadiusLT * 2, cornerRadiusLT * 2, 180F, 90F);
                RoundedRectOuter[0].AddLine(rect.X + cornerRadiusLT, rect.Y, rect.X, rect.Y);
                RoundedRectOuter[0].AddLine(rect.X, rect.Y, rect.X, rect.Y + cornerRadiusLT);
                RoundedRectOuter[0].CloseFigure();
            }

            if (cornerRadiusRT > 0)
            {
                RoundedRectOuter[1].AddArc(rect.Right - cornerRadiusRT * 2, rect.Y, cornerRadiusRT * 2, cornerRadiusRT * 2, 270F, 90F);
                RoundedRectOuter[1].AddLine(rect.Right, rect.Y + cornerRadiusRT, rect.Right, rect.Y);
                RoundedRectOuter[1].AddLine(rect.Right, rect.Y, rect.Right - cornerRadiusRT, rect.Y);
                RoundedRectOuter[1].CloseFigure();
            }

            if (cornerRadiusRB > 0)
            {
                RoundedRectOuter[2].AddArc(rect.Right - cornerRadiusRB * 2, rect.Bottom - cornerRadiusRB * 2, cornerRadiusRB * 2, cornerRadiusRB * 2, 0F, 90F);
                RoundedRectOuter[2].AddLine(rect.Right - cornerRadiusRB, rect.Bottom, rect.Right, rect.Bottom);
                RoundedRectOuter[2].AddLine(rect.Right, rect.Bottom, rect.Right, rect.Bottom - cornerRadiusRB);
                RoundedRectOuter[2].CloseFigure();
            }

            if (cornerRadiusLB > 0)
            {
                RoundedRectOuter[3].AddArc(rect.X, rect.Bottom - cornerRadiusLB * 2, cornerRadiusLB * 2, cornerRadiusLB * 2, 90F, 90F);
                RoundedRectOuter[3].AddLine(rect.X, rect.Bottom - cornerRadiusLB, rect.X, rect.Bottom);
                RoundedRectOuter[3].AddLine(rect.X, rect.Bottom, rect.X + cornerRadiusLB, rect.Bottom);
                RoundedRectOuter[3].CloseFigure();
            }

            return RoundedRectOuter;
        }

        #endregion
    }
}