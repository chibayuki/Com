﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.Painting2D
Version 24.7.21.1040

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
using System.Drawing.Text;
using System.Windows.Forms;

namespace Com
{
    /// <summary>
    /// 为 2D 绘图提供静态方法。
    /// </summary>
    public static class Painting2D
    {
        /// <summary>
        /// 绘制一个直线段，并返回表示是否已经实际完成绘图的布尔值。
        /// </summary>
        /// <param name="graph">画布。</param>
        /// <param name="imageSize">图像大小。</param>
        /// <param name="pt1">直线段的第一个端点。</param>
        /// <param name="pt2">直线段的第二个端点。</param>
        /// <param name="color">线条颜色。</param>
        /// <param name="width">线条宽度。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintLine(Graphics graph, Size imageSize, PointD pt1, PointD pt2, Color color, float width)
        {
            try
            {
                if (graph is null || imageSize.Width <= 0 || imageSize.Height <= 0 || pt1.IsNaNOrInfinity || pt2.IsNaNOrInfinity || color.IsEmpty || color.A == 0 || InternalMethod.IsNaNOrInfinity(width) || width <= 0)
                {
                    return false;
                }
                else if (!Geometry.LineIsVisibleInRectangle(pt1, pt2, new RectangleF(new Point(0, 0), imageSize)))
                {
                    return false;
                }
                else
                {
                    PointD rectCenter = new PointD(imageSize.Width * 0.5, imageSize.Height * 0.5);

                    double rectRadius = Math.Sqrt(imageSize.Width * imageSize.Width + imageSize.Height * imageSize.Height) / 2;

                    double dist_RC_P1 = rectCenter.DistanceFrom(pt1);
                    double dist_RC_P2 = rectCenter.DistanceFrom(pt2);

                    if (dist_RC_P1 > rectRadius || dist_RC_P2 > rectRadius)
                    {
                        PointD footPoint = Geometry.GetFootPoint(rectCenter, pt1, pt2);

                        bool footPointInRect = Geometry.PointIsVisibleInRectangle(footPoint, new RectangleF(new Point(0, 0), imageSize));

                        double dist_RC_FP = rectCenter.DistanceFrom(footPoint);
                        double dist_FP_P12 = Math.Sqrt(Math.Pow(rectRadius, 2) - Math.Pow(dist_RC_FP, 2));

                        if (dist_RC_P1 > rectRadius)
                        {
                            if (footPointInRect)
                            {
                                double Angle_FP_P1 = Geometry.GetAngleOfTwoPoints(footPoint, pt1);

                                pt1.X = footPoint.X + dist_FP_P12 * Math.Cos(Angle_FP_P1);
                                pt1.Y = footPoint.Y + dist_FP_P12 * Math.Sin(Angle_FP_P1);
                            }
                            else
                            {
                                pt1 = footPoint;
                            }
                        }

                        if (dist_RC_P2 > rectRadius)
                        {
                            if (footPointInRect)
                            {
                                double Angle_FP_P2 = Geometry.GetAngleOfTwoPoints(footPoint, pt2);

                                pt2.X = footPoint.X + dist_FP_P12 * Math.Cos(Angle_FP_P2);
                                pt2.Y = footPoint.Y + dist_FP_P12 * Math.Sin(Angle_FP_P2);
                            }
                            else
                            {
                                pt2 = footPoint;
                            }
                        }
                    }

                    using (Pen pen = new Pen(color, width))
                    {
                        graph.DrawLine(pen, pt1.ToPointF(), pt2.ToPointF());
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 绘制一个直线段，并返回表示是否已经实际完成绘图的布尔值。
        /// </summary>
        /// <param name="bmp">用于绘制的位图。</param>
        /// <param name="pt1">直线段的第一个端点。</param>
        /// <param name="pt2">直线段的第二个端点。</param>
        /// <param name="color">线条颜色。</param>
        /// <param name="width">线条宽度。</param>
        /// <param name="antiAlias">是否使用抗锯齿模式绘图。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintLine(Bitmap bmp, PointD pt1, PointD pt2, Color color, float width, bool antiAlias)
        {
            try
            {
                if (bmp is null || pt1.IsNaNOrInfinity || pt2.IsNaNOrInfinity || color.IsEmpty || color.A == 0 || InternalMethod.IsNaNOrInfinity(width) || width <= 0)
                {
                    return false;
                }
                else if (!Geometry.LineIsVisibleInRectangle(pt1, pt2, new RectangleF(new Point(0, 0), bmp.Size)))
                {
                    return false;
                }
                else
                {
                    using (Graphics graph = Graphics.FromImage(bmp))
                    {
                        if (antiAlias)
                        {
                            graph.SmoothingMode = SmoothingMode.AntiAlias;
                        }

                        //

                        PaintLine(graph, bmp.Size, pt1, pt2, color, width);
                    }

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 绘制极坐标网格，并返回表示是否已经实际完成绘图的布尔值。
        /// </summary>
        /// <param name="bmp">用于绘制的位图。</param>
        /// <param name="offset">极坐标网格的中心。</param>
        /// <param name="radius">极坐标网格的半径。</param>
        /// <param name="deltaRadius">极坐标网格内部相邻同心圆的半径差。</param>
        /// <param name="normalIncreasePeriod">法线数量增加周期。</param>
        /// <param name="color">线条颜色。</param>
        /// <param name="antiAlias">是否使用抗锯齿模式绘图。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintPolarGrid(Bitmap bmp, PointD offset, double radius, double deltaRadius, int normalIncreasePeriod, Color color, bool antiAlias)
        {
            try
            {
                if (bmp is null || offset.IsNaNOrInfinity || InternalMethod.IsNaNOrInfinity(radius) || radius <= 0 || InternalMethod.IsNaNOrInfinity(deltaRadius) || deltaRadius <= 0 || normalIncreasePeriod <= 0 || color.IsEmpty || color.A == 0)
                {
                    return false;
                }
                else if (!Geometry.CircleInnerIsVisibleInRectangle(offset, radius, new RectangleF(new Point(), bmp.Size)))
                {
                    return false;
                }
                else
                {
                    using (Graphics graph = Graphics.FromImage(bmp))
                    {
                        if (antiAlias)
                        {
                            graph.SmoothingMode = SmoothingMode.AntiAlias;
                        }

                        using (Pen pen = new Pen(color, 1F))
                        {
                            // 绘制等差 15 / Pow(2, n) 度法向直线系（半径从 deltaRadius * normalIncreasePeriod 像素起，每扩大 2 倍，相邻法线之间的圆心角减小 1/2）：

                            for (int i = 0; i < Math.Log(radius / (deltaRadius * normalIncreasePeriod)) / Math.Log(2); i++)
                            {
                                double Rx = deltaRadius * normalIncreasePeriod * Math.Pow(2, i); // 起点的半径。
                                double Tx = 12 * Math.Pow(2, i); // 等分线的圆心角等于 PI / Tx * (2 * n + 1)，n = 1, 2, …, 2 * Tx - 1。

                                for (int j = 1; j <= 2 * Tx - 1; j += 2)
                                {
                                    graph.DrawLine(pen, new PointF((float)(offset.X + Rx * Math.Cos(Constant.Pi / Tx * j)), (float)(offset.Y + Rx * Math.Sin(Constant.Pi / Tx * j))), new PointF((float)(offset.X + radius * Math.Cos(Constant.Pi / Tx * j)), (float)(offset.Y + radius * Math.Sin(Constant.Pi / Tx * j))));
                                }
                            }

                            // 绘制等差 30 度法向直线系：

                            for (int i = 0; i < 12; i++)
                            {
                                graph.DrawLine(pen, new PointF((float)offset.X, (float)offset.Y), new PointF((float)(offset.X + radius * Math.Cos(Constant.Pi * i / 6)), (float)(offset.Y + radius * Math.Sin(Constant.Pi * i / 6))));
                            }

                            // 绘制同心圆：

                            for (int i = 1; i < radius / deltaRadius; i++)
                            {
                                graph.DrawEllipse(pen, new RectangleF((float)(offset.X - deltaRadius * i), (float)(offset.Y - deltaRadius * i), (float)(2 * deltaRadius * i), (float)(2 * deltaRadius * i)));
                            }

                            graph.DrawEllipse(pen, new RectangleF((float)(offset.X - radius), (float)(offset.Y - radius), (float)(2 * radius), (float)(2 * radius)));
                        }
                    }

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 绘制一个圆，并返回表示是否已经实际完成绘图的布尔值。
        /// </summary>
        /// <param name="bmp">用于绘制的位图。</param>
        /// <param name="offset">圆的圆心。</param>
        /// <param name="radius">圆的半径。</param>
        /// <param name="color">线条颜色。</param>
        /// <param name="width">线条宽度，0 表示填充。</param>
        /// <param name="antiAlias">是否使用抗锯齿模式绘图。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintCircle(Bitmap bmp, PointD offset, double radius, Color color, float width, bool antiAlias)
        {
            try
            {
                if (bmp is null || offset.IsNaNOrInfinity || InternalMethod.IsNaNOrInfinity(radius) || radius <= 0 || color.IsEmpty || color.A == 0 || InternalMethod.IsNaNOrInfinity(width) || width < 0)
                {
                    return false;
                }
                else
                {
                    if (width > 0)
                    {
                        if (!Geometry.CircumferenceIsVisibleInRectangle(offset, radius, new RectangleF(new Point(), bmp.Size)))
                        {
                            return false;
                        }
                        else
                        {
                            using (Graphics graph = Graphics.FromImage(bmp))
                            {
                                if (antiAlias)
                                {
                                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                                }

                                //

                                using (Pen pen = new Pen(color, width))
                                {
                                    graph.DrawEllipse(pen, new RectangleF((float)(offset.X - radius), (float)(offset.Y - radius), (float)(radius * 2), (float)(radius * 2)));
                                }
                            }

                            return true;
                        }
                    }
                    else
                    {
                        if (!Geometry.CircleInnerIsVisibleInRectangle(offset, radius, new RectangleF(new Point(), bmp.Size)))
                        {
                            return false;
                        }
                        else
                        {
                            using (Graphics graph = Graphics.FromImage(bmp))
                            {
                                if (antiAlias)
                                {
                                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                                }

                                //

                                using (Brush br = new SolidBrush(color))
                                {
                                    graph.FillEllipse(br, new RectangleF((float)(offset.X - radius), (float)(offset.Y - radius), (float)(radius * 2), (float)(radius * 2)));
                                }
                            }

                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 绘制一个大型圆，并返回表示是否已经实际完成绘图的布尔值。
        /// </summary>
        /// <param name="bmp">用于绘制的位图。</param>
        /// <param name="offset">圆的圆心。</param>
        /// <param name="radius">圆的半径。</param>
        /// <param name="refPhase">参考相位（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <param name="color">线条颜色。</param>
        /// <param name="width">线条宽度，0 表示填充。</param>
        /// <param name="antiAlias">是否使用抗锯齿模式绘图。</param>
        /// <param name="minDiv">在用于绘制的位图的可见范围内将圆周按相位等分的最小数量。</param>
        /// <param name="maxDiv">在用于绘制的位图的可见范围内将圆周按相位等分的最大数量。</param>
        /// <param name="divArc">圆周的任何等分的近似长度（像素）。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintLargeCircle(Bitmap bmp, PointD offset, double radius, double refPhase, Color color, float width, bool antiAlias, int minDiv, int maxDiv, double divArc)
        {
            try
            {
                if (bmp is null || offset.IsNaNOrInfinity || InternalMethod.IsNaNOrInfinity(radius) || radius <= 0 || InternalMethod.IsNaNOrInfinity(refPhase) || color.IsEmpty || color.A == 0 || InternalMethod.IsNaNOrInfinity(width) || width < 0 || minDiv <= 0 || maxDiv <= 0 || minDiv > maxDiv || InternalMethod.IsNaNOrInfinity(divArc) || divArc <= 0)
                {
                    return false;
                }
                else
                {
                    PointD RectCenter = new PointD(bmp.Width / 2, bmp.Height / 2);

                    double RectRadius = Math.Sqrt(Math.Pow(bmp.Width, 2) + Math.Pow(bmp.Height, 2)) / 2;

                    refPhase = Geometry.AngleMapping(refPhase);

                    if (width > 0)
                    {
                        if (!Geometry.CircumferenceIsVisibleInRectangle(offset, radius, new RectangleF(new Point(), bmp.Size)))
                        {
                            return false;
                        }
                        else
                        {
                            using (Graphics graph = Graphics.FromImage(bmp))
                            {
                                if (antiAlias)
                                {
                                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                                }

                                //

                                bool RefPassed = false;

                                PointD RefPoint = new PointD(offset.X + radius * Math.Cos(refPhase), offset.Y + radius * Math.Sin(refPhase));

                                int DivCount = (int)Math.Min(maxDiv, Math.Max(minDiv, Constant.DoublePi * radius / divArc));
                                double DivPhase = 0;

                                if (Geometry.PointIsVisibleInCircle(offset, RectCenter, RectRadius))
                                {
                                    DivPhase = Constant.DoublePi / DivCount;

                                    List<PointF> L_PF = new List<PointF>(DivCount + 2);

                                    for (int i = 0; i <= DivCount; i++)
                                    {
                                        double Ph = DivPhase * i;

                                        if (!RefPassed)
                                        {
                                            if (Ph == refPhase)
                                            {
                                                RefPassed = true;
                                            }
                                            else if (Ph > refPhase)
                                            {
                                                RefPassed = true;

                                                L_PF.Add(RefPoint.ToPointF());
                                            }
                                        }

                                        PointD PD = new PointD(offset.X + radius * Math.Cos(Ph), offset.Y + radius * Math.Sin(Ph));

                                        L_PF.Add(PD.ToPointF());
                                    }

                                    using (Pen pen = new Pen(color, width))
                                    {
                                        if (L_PF.Count == 2)
                                        {
                                            graph.DrawLine(pen, L_PF[0], L_PF[1]);
                                        }
                                        else if (L_PF.Count > 2)
                                        {
                                            graph.DrawLines(pen, L_PF.ToArray());
                                        }
                                    }
                                }
                                else
                                {
                                    double Angle_EC_RC = Geometry.GetAngleOfTwoPoints(offset, RectCenter);
                                    double Angle_Delta = Math.Asin(RectRadius / offset.DistanceFrom(RectCenter));
                                    double Angle_Min = Angle_EC_RC - Angle_Delta;
                                    double Angle_Max = Angle_EC_RC + Angle_Delta;

                                    double Phase_Min = Geometry.AngleMapping(Angle_Min);
                                    double Phase_Max = Geometry.AngleMapping(Angle_Max);

                                    if (Phase_Min > Phase_Max)
                                    {
                                        Phase_Max += Constant.DoublePi;
                                    }

                                    if (!(refPhase >= Phase_Min && refPhase <= Phase_Max))
                                    {
                                        if (refPhase + Constant.DoublePi >= Phase_Min && refPhase + Constant.DoublePi <= Phase_Max)
                                        {
                                            refPhase += Constant.DoublePi;
                                        }
                                        else
                                        {
                                            RefPassed = true;
                                        }
                                    }

                                    DivCount = (DivCount + 1) / 2;
                                    DivPhase = (Phase_Max - Phase_Min) / DivCount;

                                    List<PointD> L_PD = new List<PointD>(DivCount + 2);

                                    for (int i = 0; i <= DivCount; i++)
                                    {
                                        double Ph = Phase_Min + DivPhase * i;

                                        if (!RefPassed)
                                        {
                                            if (Ph == refPhase)
                                            {
                                                RefPassed = true;
                                            }
                                            else if (Ph > refPhase)
                                            {
                                                RefPassed = true;

                                                L_PD.Add(RefPoint);
                                            }
                                        }

                                        PointD PD = new PointD(offset.X + radius * Math.Cos(Ph), offset.Y + radius * Math.Sin(Ph));

                                        L_PD.Add(PD);
                                    }

                                    while (L_PD.Count > 2 && !Geometry.PointIsVisibleInCircle(L_PD[1], RectCenter, RectRadius))
                                    {
                                        L_PD.RemoveAt(0);
                                    }

                                    while (L_PD.Count > 2 && !Geometry.PointIsVisibleInCircle(L_PD[L_PD.Count - 2], RectCenter, RectRadius))
                                    {
                                        L_PD.RemoveAt(L_PD.Count - 1);
                                    }

                                    if (!Geometry.PointIsVisibleInCircle(L_PD[0], RectCenter, RectRadius))
                                    {
                                        PointD FootPoint = Geometry.GetFootPoint(RectCenter, L_PD[1], L_PD[0]);

                                        double Dist_RC_FP = RectCenter.DistanceFrom(FootPoint);
                                        double Dist_FP_P = Math.Sqrt(Math.Pow(RectRadius, 2) - Math.Pow(Dist_RC_FP, 2));

                                        double Angle_FP_P = Geometry.GetAngleOfTwoPoints(FootPoint, L_PD[0]);

                                        L_PD[0] = new PointD(FootPoint.X + Dist_FP_P * Math.Cos(Angle_FP_P), FootPoint.Y + Dist_FP_P * Math.Sin(Angle_FP_P));
                                    }

                                    if (!Geometry.PointIsVisibleInCircle(L_PD[L_PD.Count - 1], RectCenter, RectRadius))
                                    {
                                        PointD FootPoint = Geometry.GetFootPoint(RectCenter, L_PD[L_PD.Count - 2], L_PD[L_PD.Count - 1]);

                                        double Dist_RC_FP = RectCenter.DistanceFrom(FootPoint);
                                        double Dist_FP_P = Math.Sqrt(Math.Pow(RectRadius, 2) - Math.Pow(Dist_RC_FP, 2));

                                        double Angle_FP_P = Geometry.GetAngleOfTwoPoints(FootPoint, L_PD[L_PD.Count - 1]);

                                        L_PD[L_PD.Count - 1] = new PointD(FootPoint.X + Dist_FP_P * Math.Cos(Angle_FP_P), FootPoint.Y + Dist_FP_P * Math.Sin(Angle_FP_P));
                                    }

                                    List<PointF> L_PF = new List<PointF>(L_PD.Count);

                                    for (int i = 0; i < L_PD.Count; i++)
                                    {
                                        L_PF.Add(L_PD[i].ToPointF());
                                    }

                                    using (Pen pen = new Pen(color, width))
                                    {
                                        if (L_PF.Count == 2)
                                        {
                                            graph.DrawLine(pen, L_PF[0], L_PF[1]);
                                        }
                                        else if (L_PF.Count > 2)
                                        {
                                            graph.DrawLines(pen, L_PF.ToArray());
                                        }
                                    }
                                }
                            }

                            return true;
                        }
                    }
                    else
                    {
                        if (!Geometry.CircleInnerIsVisibleInRectangle(offset, radius, new RectangleF(new Point(), bmp.Size)))
                        {
                            return false;
                        }
                        else
                        {
                            using (Graphics graph = Graphics.FromImage(bmp))
                            {
                                if (antiAlias)
                                {
                                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                                }

                                //

                                if (!Geometry.CircumferenceIsVisibleInRectangle(offset, radius, new RectangleF(new Point(), bmp.Size)))
                                {
                                    using (Brush br = new SolidBrush(color))
                                    {
                                        graph.FillRectangle(br, new Rectangle(new Point(), bmp.Size));
                                    }
                                }
                                else
                                {
                                    bool RefPassed = false;

                                    PointD RefPoint = new PointD(offset.X + radius * Math.Cos(refPhase), offset.Y + radius * Math.Sin(refPhase));

                                    int DivCount = (int)Math.Min(maxDiv, Math.Max(minDiv, Constant.DoublePi * radius / divArc));
                                    double DivPhase = 0;

                                    if (Geometry.PointIsVisibleInCircle(offset, RectCenter, RectRadius))
                                    {
                                        DivPhase = Constant.DoublePi / DivCount;

                                        List<PointF> L_PF = new List<PointF>(DivCount + 2);

                                        for (int i = 0; i <= DivCount; i++)
                                        {
                                            double Ph = DivPhase * i;

                                            if (!RefPassed)
                                            {
                                                if (Ph == refPhase)
                                                {
                                                    RefPassed = true;
                                                }
                                                else if (Ph > refPhase)
                                                {
                                                    RefPassed = true;

                                                    L_PF.Add(RefPoint.ToPointF());
                                                }
                                            }

                                            PointD PD = new PointD(offset.X + radius * Math.Cos(Ph), offset.Y + radius * Math.Sin(Ph));

                                            L_PF.Add(PD.ToPointF());
                                        }

                                        if (L_PF.Count == 2)
                                        {
                                            using (Pen pen = new Pen(color, width))
                                            {
                                                graph.DrawLine(pen, L_PF[0], L_PF[1]);
                                            }
                                        }
                                        else if (L_PF.Count > 2)
                                        {
                                            using (Brush br = new SolidBrush(color))
                                            {
                                                graph.FillPolygon(br, L_PF.ToArray());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        double Angle_EC_RC = Geometry.GetAngleOfTwoPoints(offset, RectCenter);
                                        double Angle_Delta = Math.Asin(RectRadius / offset.DistanceFrom(RectCenter));
                                        double Angle_Min = Angle_EC_RC - Angle_Delta;
                                        double Angle_Max = Angle_EC_RC + Angle_Delta;

                                        double Phase_Min = Geometry.AngleMapping(Angle_Min);
                                        double Phase_Max = Geometry.AngleMapping(Angle_Max);

                                        if (Phase_Min > Phase_Max)
                                        {
                                            Phase_Max += Constant.DoublePi;
                                        }

                                        if (!(refPhase >= Phase_Min && refPhase <= Phase_Max))
                                        {
                                            if (refPhase + Constant.DoublePi >= Phase_Min && refPhase + Constant.DoublePi <= Phase_Max)
                                            {
                                                refPhase += Constant.DoublePi;
                                            }
                                            else
                                            {
                                                RefPassed = true;
                                            }
                                        }

                                        DivCount = (DivCount + 1) / 2;
                                        DivPhase = (Phase_Max - Phase_Min) / DivCount;

                                        List<PointD> L_PD = new List<PointD>(DivCount + 2 + 8);

                                        for (int i = 0; i <= DivCount; i++)
                                        {
                                            double Ph = Phase_Min + DivPhase * i;

                                            if (!RefPassed)
                                            {
                                                if (Ph == refPhase)
                                                {
                                                    RefPassed = true;
                                                }
                                                else if (Ph > refPhase)
                                                {
                                                    RefPassed = true;

                                                    L_PD.Add(RefPoint);
                                                }
                                            }

                                            PointD PD = new PointD(offset.X + radius * Math.Cos(Ph), offset.Y + radius * Math.Sin(Ph));

                                            L_PD.Add(PD);
                                        }

                                        while (L_PD.Count > 2 && !Geometry.PointIsVisibleInCircle(L_PD[1], RectCenter, RectRadius))
                                        {
                                            L_PD.RemoveAt(0);
                                        }

                                        while (L_PD.Count > 2 && !Geometry.PointIsVisibleInCircle(L_PD[L_PD.Count - 2], RectCenter, RectRadius))
                                        {
                                            L_PD.RemoveAt(L_PD.Count - 1);
                                        }

                                        if (!Geometry.PointIsVisibleInCircle(L_PD[0], RectCenter, RectRadius))
                                        {
                                            PointD FootPoint = Geometry.GetFootPoint(RectCenter, L_PD[1], L_PD[0]);

                                            double Dist_RC_FP = RectCenter.DistanceFrom(FootPoint);
                                            double Dist_FP_P = Math.Sqrt(Math.Pow(RectRadius, 2) - Math.Pow(Dist_RC_FP, 2));

                                            double Angle_FP_P = Geometry.GetAngleOfTwoPoints(FootPoint, L_PD[0]);

                                            L_PD[0] = new PointD(FootPoint.X + Dist_FP_P * Math.Cos(Angle_FP_P), FootPoint.Y + Dist_FP_P * Math.Sin(Angle_FP_P));
                                        }

                                        if (!Geometry.PointIsVisibleInCircle(L_PD[L_PD.Count - 1], RectCenter, RectRadius))
                                        {
                                            PointD FootPoint = Geometry.GetFootPoint(RectCenter, L_PD[L_PD.Count - 2], L_PD[L_PD.Count - 1]);

                                            double Dist_RC_FP = RectCenter.DistanceFrom(FootPoint);
                                            double Dist_FP_P = Math.Sqrt(Math.Pow(RectRadius, 2) - Math.Pow(Dist_RC_FP, 2));

                                            double Angle_FP_P = Geometry.GetAngleOfTwoPoints(FootPoint, L_PD[L_PD.Count - 1]);

                                            L_PD[L_PD.Count - 1] = new PointD(FootPoint.X + Dist_FP_P * Math.Cos(Angle_FP_P), FootPoint.Y + Dist_FP_P * Math.Sin(Angle_FP_P));
                                        }

                                        PointD[] LTRB = new PointD[] { new PointD(0, 0), new PointD(bmp.Width, 0), new PointD(bmp.Width, bmp.Height), new PointD(0, bmp.Height) };

                                        bool InvisiblePassed = false;

                                        for (int i = 0; i < LTRB.Length; i++)
                                        {
                                            if (!Geometry.PointIsVisibleInCircle(LTRB[i], offset, radius))
                                            {
                                                InvisiblePassed = true;
                                            }
                                            else if (InvisiblePassed)
                                            {
                                                L_PD.Add(LTRB[i]);
                                            }
                                        }

                                        for (int i = 0; i < LTRB.Length; i++)
                                        {
                                            if (Geometry.PointIsVisibleInCircle(LTRB[i], offset, radius))
                                            {
                                                L_PD.Add(LTRB[i]);
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }

                                        List<PointF> L_PF = new List<PointF>(L_PD.Count);

                                        for (int i = 0; i < L_PD.Count; i++)
                                        {
                                            L_PF.Add(L_PD[i].ToPointF());
                                        }

                                        if (L_PF.Count == 2)
                                        {
                                            using (Pen pen = new Pen(color, width))
                                            {
                                                graph.DrawLine(pen, L_PF[0], L_PF[1]);
                                            }
                                        }
                                        else if (L_PF.Count > 2)
                                        {
                                            using (Brush br = new SolidBrush(color))
                                            {
                                                graph.FillPolygon(br, L_PF.ToArray());
                                            }
                                        }
                                    }
                                }
                            }

                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 绘制一个大型圆，并返回表示是否已经实际完成绘图的布尔值。
        /// </summary>
        /// <param name="bmp">用于绘制的位图。</param>
        /// <param name="offset">圆的圆心。</param>
        /// <param name="radius">圆的半径。</param>
        /// <param name="refPhase">参考相位（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <param name="color">线条颜色。</param>
        /// <param name="width">线条宽度，0 表示填充。</param>
        /// <param name="antiAlias">是否使用抗锯齿模式绘图。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintLargeCircle(Bitmap bmp, PointD offset, double radius, double refPhase, Color color, float width, bool antiAlias) => PaintLargeCircle(bmp, offset, radius, refPhase, color, width, antiAlias, 32, 256, 4);

        /// <summary>
        /// 绘制一个大型椭圆，并返回表示是否已经实际完成绘图的布尔值。
        /// </summary>
        /// <param name="bmp">用于绘制的位图。</param>
        /// <param name="offset">椭圆的焦点。</param>
        /// <param name="semiMajorAxis">椭圆的半长轴。</param>
        /// <param name="eccentricity">椭圆的离心率。</param>
        /// <param name="rotateAngle">椭圆的旋转角（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向，焦点到近焦点连线相对于 +X 轴的角度）。</param>
        /// <param name="refPhase">参考相位（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <param name="color">线条颜色。</param>
        /// <param name="width">线条宽度，0 表示填充。</param>
        /// <param name="antiAlias">是否使用抗锯齿模式绘图。</param>
        /// <param name="minDiv">在用于绘制的位图的可见范围内将椭圆周按相位等分的最小数量。</param>
        /// <param name="maxDiv">在用于绘制的位图的可见范围内将椭圆周按相位等分的最大数量。</param>
        /// <param name="divArc">椭圆周的任何等分的近似长度（像素）。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintLargeEllipse(Bitmap bmp, PointD offset, double semiMajorAxis, double eccentricity, double rotateAngle, double refPhase, Color color, float width, bool antiAlias, int minDiv, int maxDiv, double divArc)
        {
            try
            {
                if (bmp is null || offset.IsNaNOrInfinity || InternalMethod.IsNaNOrInfinity(semiMajorAxis) || semiMajorAxis <= 0 || InternalMethod.IsNaNOrInfinity(eccentricity) || eccentricity < 0 || InternalMethod.IsNaNOrInfinity(rotateAngle) || InternalMethod.IsNaNOrInfinity(refPhase) || color.IsEmpty || color.A == 0 || InternalMethod.IsNaNOrInfinity(width) || width < 0 || minDiv <= 0 || maxDiv <= 0 || minDiv > maxDiv || InternalMethod.IsNaNOrInfinity(divArc) || divArc <= 0)
                {
                    return false;
                }
                else
                {
                    PointD RectCenter = new PointD(bmp.Width / 2, bmp.Height / 2);

                    double RectRadius = Math.Sqrt(Math.Pow(bmp.Width, 2) + Math.Pow(bmp.Height, 2)) / 2;

                    rotateAngle = Geometry.AngleMapping(rotateAngle);
                    refPhase = Geometry.AngleMapping(refPhase);

                    PointD EllipseCenter = new PointD(offset.X - semiMajorAxis * eccentricity * Math.Cos(rotateAngle), offset.Y - semiMajorAxis * eccentricity * Math.Sin(rotateAngle));

                    if (width > 0)
                    {
                        if (!Geometry.PointIsVisibleInRhombus(RectCenter, EllipseCenter, Constant.Sqrt2 * semiMajorAxis + RectRadius / Math.Sqrt(1 - Math.Pow(eccentricity, 2)), Constant.Sqrt2 * semiMajorAxis * Math.Sqrt(1 - Math.Pow(eccentricity, 2)) + RectRadius, rotateAngle) || Geometry.PointIsVisibleInRhombus(RectCenter, EllipseCenter, semiMajorAxis - RectRadius / Math.Sqrt(1 - Math.Pow(eccentricity, 2)), semiMajorAxis * Math.Sqrt(1 - Math.Pow(eccentricity, 2)) - RectRadius, rotateAngle))
                        {
                            return false;
                        }
                        else
                        {
                            using (Graphics graph = Graphics.FromImage(bmp))
                            {
                                if (antiAlias)
                                {
                                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                                }

                                //

                                bool RefPassed = false;

                                PointD RefPoint = new PointD();

                                if (eccentricity == 0)
                                {
                                    RefPoint = new PointD(offset.X + semiMajorAxis * Math.Cos(refPhase + rotateAngle), offset.Y + semiMajorAxis * Math.Sin(refPhase + rotateAngle));
                                }
                                else
                                {
                                    RefPoint = new PointD(semiMajorAxis * (Math.Cos(refPhase) - eccentricity), semiMajorAxis * (Math.Sqrt(1 - Math.Pow(eccentricity, 2)) * Math.Sin(refPhase)));
                                    RefPoint.Rotate(rotateAngle);
                                    RefPoint += offset;
                                }

                                int DivCount = (int)Math.Min(maxDiv, Math.Max(minDiv, Constant.DoublePi * semiMajorAxis / divArc));
                                double DivPhase = 0;

                                if (Geometry.PointIsVisibleInCircle(EllipseCenter, RectCenter, RectRadius))
                                {
                                    DivPhase = Constant.DoublePi / DivCount;

                                    List<PointF> L_PF = new List<PointF>(DivCount + 2);

                                    for (int i = 0; i <= DivCount; i++)
                                    {
                                        double Ph = DivPhase * i;

                                        if (!RefPassed)
                                        {
                                            if (Ph == refPhase)
                                            {
                                                RefPassed = true;
                                            }
                                            else if (Ph > refPhase)
                                            {
                                                RefPassed = true;

                                                L_PF.Add(RefPoint.ToPointF());
                                            }
                                        }

                                        PointD PD = new PointD();

                                        if (eccentricity == 0)
                                        {
                                            PD = new PointD(offset.X + semiMajorAxis * Math.Cos(Ph + rotateAngle), offset.Y + semiMajorAxis * Math.Sin(Ph + rotateAngle));
                                        }
                                        else
                                        {
                                            PD = new PointD(semiMajorAxis * (Math.Cos(Ph) - eccentricity), semiMajorAxis * (Math.Sqrt(1 - Math.Pow(eccentricity, 2)) * Math.Sin(Ph)));
                                            PD.Rotate(rotateAngle);
                                            PD += offset;
                                        }

                                        L_PF.Add(PD.ToPointF());
                                    }

                                    using (Pen pen = new Pen(color, width))
                                    {
                                        if (L_PF.Count == 2)
                                        {
                                            graph.DrawLine(pen, L_PF[0], L_PF[1]);
                                        }
                                        else if (L_PF.Count > 2)
                                        {
                                            graph.DrawLines(pen, L_PF.ToArray());
                                        }
                                    }
                                }
                                else
                                {
                                    double Angle_EC_RC = Geometry.GetAngleOfTwoPoints(EllipseCenter, RectCenter) - rotateAngle;
                                    double Angle_Delta = Math.Asin(RectRadius / EllipseCenter.DistanceFrom(RectCenter));
                                    double Angle_Min = Angle_EC_RC - Angle_Delta;
                                    double Angle_Max = Angle_EC_RC + Angle_Delta;

                                    double Phase_Min = Geometry.AngleMapping(Geometry.EllipseCentralAngleToPhase(Angle_Min, eccentricity));
                                    double Phase_Max = Geometry.AngleMapping(Geometry.EllipseCentralAngleToPhase(Angle_Max, eccentricity));

                                    if (Phase_Min > Phase_Max)
                                    {
                                        Phase_Max += Constant.DoublePi;
                                    }

                                    if (!(refPhase >= Phase_Min && refPhase <= Phase_Max))
                                    {
                                        if (refPhase + Constant.DoublePi >= Phase_Min && refPhase + Constant.DoublePi <= Phase_Max)
                                        {
                                            refPhase += Constant.DoublePi;
                                        }
                                        else
                                        {
                                            RefPassed = true;
                                        }
                                    }

                                    DivCount = (DivCount + 1) / 2;
                                    DivPhase = (Phase_Max - Phase_Min) / DivCount;

                                    List<PointD> L_PD = new List<PointD>(DivCount + 2);

                                    for (int i = 0; i <= DivCount; i++)
                                    {
                                        double Ph = Phase_Min + DivPhase * i;

                                        if (!RefPassed)
                                        {
                                            if (Ph == refPhase)
                                            {
                                                RefPassed = true;
                                            }
                                            else if (Ph > refPhase)
                                            {
                                                RefPassed = true;

                                                L_PD.Add(RefPoint);
                                            }
                                        }

                                        PointD PD = new PointD();

                                        if (eccentricity == 0)
                                        {
                                            PD = new PointD(offset.X + semiMajorAxis * Math.Cos(Ph + rotateAngle), offset.Y + semiMajorAxis * Math.Sin(Ph + rotateAngle));
                                        }
                                        else
                                        {
                                            PD = new PointD(semiMajorAxis * (Math.Cos(Ph) - eccentricity), semiMajorAxis * (Math.Sqrt(1 - Math.Pow(eccentricity, 2)) * Math.Sin(Ph)));
                                            PD.Rotate(rotateAngle);
                                            PD += offset;
                                        }

                                        L_PD.Add(PD);
                                    }

                                    while (L_PD.Count > 2 && !Geometry.PointIsVisibleInCircle(L_PD[1], RectCenter, RectRadius))
                                    {
                                        L_PD.RemoveAt(0);
                                    }

                                    while (L_PD.Count > 2 && !Geometry.PointIsVisibleInCircle(L_PD[L_PD.Count - 2], RectCenter, RectRadius))
                                    {
                                        L_PD.RemoveAt(L_PD.Count - 1);
                                    }

                                    if (!Geometry.PointIsVisibleInCircle(L_PD[0], RectCenter, RectRadius))
                                    {
                                        PointD FootPoint = Geometry.GetFootPoint(RectCenter, L_PD[1], L_PD[0]);

                                        double Dist_RC_FP = RectCenter.DistanceFrom(FootPoint);
                                        double Dist_FP_P = Math.Sqrt(Math.Pow(RectRadius, 2) - Math.Pow(Dist_RC_FP, 2));

                                        double Angle_FP_P = Geometry.GetAngleOfTwoPoints(FootPoint, L_PD[0]);

                                        L_PD[0] = new PointD(FootPoint.X + Dist_FP_P * Math.Cos(Angle_FP_P), FootPoint.Y + Dist_FP_P * Math.Sin(Angle_FP_P));
                                    }

                                    if (!Geometry.PointIsVisibleInCircle(L_PD[L_PD.Count - 1], RectCenter, RectRadius))
                                    {
                                        PointD FootPoint = Geometry.GetFootPoint(RectCenter, L_PD[L_PD.Count - 2], L_PD[L_PD.Count - 1]);

                                        double Dist_RC_FP = RectCenter.DistanceFrom(FootPoint);
                                        double Dist_FP_P = Math.Sqrt(Math.Pow(RectRadius, 2) - Math.Pow(Dist_RC_FP, 2));

                                        double Angle_FP_P = Geometry.GetAngleOfTwoPoints(FootPoint, L_PD[L_PD.Count - 1]);

                                        L_PD[L_PD.Count - 1] = new PointD(FootPoint.X + Dist_FP_P * Math.Cos(Angle_FP_P), FootPoint.Y + Dist_FP_P * Math.Sin(Angle_FP_P));
                                    }

                                    List<PointF> L_PF = new List<PointF>(L_PD.Count);

                                    for (int i = 0; i < L_PD.Count; i++)
                                    {
                                        L_PF.Add(L_PD[i].ToPointF());
                                    }

                                    using (Pen pen = new Pen(color, width))
                                    {
                                        if (L_PF.Count == 2)
                                        {
                                            graph.DrawLine(pen, L_PF[0], L_PF[1]);
                                        }
                                        else if (L_PF.Count > 2)
                                        {
                                            graph.DrawLines(pen, L_PF.ToArray());
                                        }
                                    }
                                }
                            }

                            return true;
                        }
                    }
                    else
                    {
                        if (!Geometry.PointIsVisibleInRhombus(RectCenter, EllipseCenter, Constant.Sqrt2 * semiMajorAxis + RectRadius / Math.Sqrt(1 - Math.Pow(eccentricity, 2)), Constant.Sqrt2 * semiMajorAxis * Math.Sqrt(1 - Math.Pow(eccentricity, 2)) + RectRadius, rotateAngle))
                        {
                            return false;
                        }
                        else
                        {
                            using (Graphics graph = Graphics.FromImage(bmp))
                            {
                                if (antiAlias)
                                {
                                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                                }

                                //

                                if (Geometry.PointIsVisibleInRhombus(RectCenter, EllipseCenter, semiMajorAxis - RectRadius / Math.Sqrt(1 - Math.Pow(eccentricity, 2)), semiMajorAxis * Math.Sqrt(1 - Math.Pow(eccentricity, 2)) - RectRadius, rotateAngle))
                                {
                                    using (Brush br = new SolidBrush(color))
                                    {
                                        graph.FillRectangle(br, new Rectangle(new Point(), bmp.Size));
                                    }
                                }
                                else
                                {
                                    bool RefPassed = false;

                                    PointD RefPoint = new PointD();

                                    if (eccentricity == 0)
                                    {
                                        RefPoint = new PointD(offset.X + semiMajorAxis * Math.Cos(refPhase + rotateAngle), offset.Y + semiMajorAxis * Math.Sin(refPhase + rotateAngle));
                                    }
                                    else
                                    {
                                        RefPoint = new PointD(semiMajorAxis * (Math.Cos(refPhase) - eccentricity), semiMajorAxis * (Math.Sqrt(1 - Math.Pow(eccentricity, 2)) * Math.Sin(refPhase)));
                                        RefPoint.Rotate(rotateAngle);
                                        RefPoint += offset;
                                    }

                                    int DivCount = (int)Math.Min(maxDiv, Math.Max(minDiv, Constant.DoublePi * semiMajorAxis / divArc));
                                    double DivPhase = 0;

                                    if (Geometry.PointIsVisibleInCircle(EllipseCenter, RectCenter, RectRadius))
                                    {
                                        DivPhase = Constant.DoublePi / DivCount;

                                        List<PointF> L_PF = new List<PointF>(DivCount + 2);

                                        for (int i = 0; i <= DivCount; i++)
                                        {
                                            double Ph = DivPhase * i;

                                            if (!RefPassed)
                                            {
                                                if (Ph == refPhase)
                                                {
                                                    RefPassed = true;
                                                }
                                                else if (Ph > refPhase)
                                                {
                                                    RefPassed = true;

                                                    L_PF.Add(RefPoint.ToPointF());
                                                }
                                            }

                                            PointD PD = new PointD();

                                            if (eccentricity == 0)
                                            {
                                                PD = new PointD(offset.X + semiMajorAxis * Math.Cos(Ph + rotateAngle), offset.Y + semiMajorAxis * Math.Sin(Ph + rotateAngle));
                                            }
                                            else
                                            {
                                                PD = new PointD(semiMajorAxis * (Math.Cos(Ph) - eccentricity), semiMajorAxis * (Math.Sqrt(1 - Math.Pow(eccentricity, 2)) * Math.Sin(Ph)));
                                                PD.Rotate(rotateAngle);
                                                PD += offset;
                                            }

                                            L_PF.Add(PD.ToPointF());
                                        }

                                        if (L_PF.Count == 2)
                                        {
                                            using (Pen pen = new Pen(color, width))
                                            {
                                                graph.DrawLine(pen, L_PF[0], L_PF[1]);
                                            }
                                        }
                                        else if (L_PF.Count > 2)
                                        {
                                            using (Brush br = new SolidBrush(color))
                                            {
                                                graph.FillPolygon(br, L_PF.ToArray());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        double Angle_EC_RC = Geometry.GetAngleOfTwoPoints(EllipseCenter, RectCenter) - rotateAngle;
                                        double Angle_Delta = Math.Asin(RectRadius / EllipseCenter.DistanceFrom(RectCenter));
                                        double Angle_Min = Angle_EC_RC - Angle_Delta;
                                        double Angle_Max = Angle_EC_RC + Angle_Delta;

                                        double Phase_Min = Geometry.AngleMapping(Geometry.EllipseCentralAngleToPhase(Angle_Min, eccentricity));
                                        double Phase_Max = Geometry.AngleMapping(Geometry.EllipseCentralAngleToPhase(Angle_Max, eccentricity));

                                        if (Phase_Min > Phase_Max)
                                        {
                                            Phase_Max += Constant.DoublePi;
                                        }

                                        if (!(refPhase >= Phase_Min && refPhase <= Phase_Max))
                                        {
                                            if (refPhase + Constant.DoublePi >= Phase_Min && refPhase + Constant.DoublePi <= Phase_Max)
                                            {
                                                refPhase += Constant.DoublePi;
                                            }
                                            else
                                            {
                                                RefPassed = true;
                                            }
                                        }

                                        DivCount = (DivCount + 1) / 2;
                                        DivPhase = (Phase_Max - Phase_Min) / DivCount;

                                        List<PointD> L_PD = new List<PointD>(DivCount + 2 + 8);

                                        for (int i = 0; i <= DivCount; i++)
                                        {
                                            double Ph = Phase_Min + DivPhase * i;

                                            if (!RefPassed)
                                            {
                                                if (Ph == refPhase)
                                                {
                                                    RefPassed = true;
                                                }
                                                else if (Ph > refPhase)
                                                {
                                                    RefPassed = true;

                                                    L_PD.Add(RefPoint);
                                                }
                                            }

                                            PointD PD = new PointD();

                                            if (eccentricity == 0)
                                            {
                                                PD = new PointD(offset.X + semiMajorAxis * Math.Cos(Ph + rotateAngle), offset.Y + semiMajorAxis * Math.Sin(Ph + rotateAngle));
                                            }
                                            else
                                            {
                                                PD = new PointD(semiMajorAxis * (Math.Cos(Ph) - eccentricity), semiMajorAxis * (Math.Sqrt(1 - Math.Pow(eccentricity, 2)) * Math.Sin(Ph)));
                                                PD.Rotate(rotateAngle);
                                                PD += offset;
                                            }

                                            L_PD.Add(PD);
                                        }

                                        while (L_PD.Count > 2 && !Geometry.PointIsVisibleInCircle(L_PD[1], RectCenter, RectRadius))
                                        {
                                            L_PD.RemoveAt(0);
                                        }

                                        while (L_PD.Count > 2 && !Geometry.PointIsVisibleInCircle(L_PD[L_PD.Count - 2], RectCenter, RectRadius))
                                        {
                                            L_PD.RemoveAt(L_PD.Count - 1);
                                        }

                                        if (!Geometry.PointIsVisibleInCircle(L_PD[0], RectCenter, RectRadius))
                                        {
                                            PointD FootPoint = Geometry.GetFootPoint(RectCenter, L_PD[1], L_PD[0]);

                                            double Dist_RC_FP = RectCenter.DistanceFrom(FootPoint);
                                            double Dist_FP_P = Math.Sqrt(Math.Pow(RectRadius, 2) - Math.Pow(Dist_RC_FP, 2));

                                            double Angle_FP_P = Geometry.GetAngleOfTwoPoints(FootPoint, L_PD[0]);

                                            L_PD[0] = new PointD(FootPoint.X + Dist_FP_P * Math.Cos(Angle_FP_P), FootPoint.Y + Dist_FP_P * Math.Sin(Angle_FP_P));
                                        }

                                        if (!Geometry.PointIsVisibleInCircle(L_PD[L_PD.Count - 1], RectCenter, RectRadius))
                                        {
                                            PointD FootPoint = Geometry.GetFootPoint(RectCenter, L_PD[L_PD.Count - 2], L_PD[L_PD.Count - 1]);

                                            double Dist_RC_FP = RectCenter.DistanceFrom(FootPoint);
                                            double Dist_FP_P = Math.Sqrt(Math.Pow(RectRadius, 2) - Math.Pow(Dist_RC_FP, 2));

                                            double Angle_FP_P = Geometry.GetAngleOfTwoPoints(FootPoint, L_PD[L_PD.Count - 1]);

                                            L_PD[L_PD.Count - 1] = new PointD(FootPoint.X + Dist_FP_P * Math.Cos(Angle_FP_P), FootPoint.Y + Dist_FP_P * Math.Sin(Angle_FP_P));
                                        }

                                        PointD[] LTRB = new PointD[] { new PointD(0, 0), new PointD(bmp.Width, 0), new PointD(bmp.Width, bmp.Height), new PointD(0, bmp.Height) };

                                        bool InvisiblePassed = false;

                                        for (int i = 0; i < LTRB.Length; i++)
                                        {
                                            if (!Geometry.PointIsVisibleInEllipse(LTRB[i], offset, semiMajorAxis, eccentricity, rotateAngle))
                                            {
                                                InvisiblePassed = true;
                                            }
                                            else if (InvisiblePassed)
                                            {
                                                L_PD.Add(LTRB[i]);
                                            }
                                        }

                                        for (int i = 0; i < LTRB.Length; i++)
                                        {
                                            if (Geometry.PointIsVisibleInEllipse(LTRB[i], offset, semiMajorAxis, eccentricity, rotateAngle))
                                            {
                                                L_PD.Add(LTRB[i]);
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }

                                        List<PointF> L_PF = new List<PointF>(L_PD.Count);

                                        for (int i = 0; i < L_PD.Count; i++)
                                        {
                                            L_PF.Add(L_PD[i].ToPointF());
                                        }

                                        if (L_PF.Count == 2)
                                        {
                                            using (Pen pen = new Pen(color, width))
                                            {
                                                graph.DrawLine(pen, L_PF[0], L_PF[1]);
                                            }
                                        }
                                        else if (L_PF.Count > 2)
                                        {
                                            using (Brush br = new SolidBrush(color))
                                            {
                                                graph.FillPolygon(br, L_PF.ToArray());
                                            }
                                        }
                                    }
                                }
                            }

                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 绘制一个大型椭圆，并返回表示是否已经实际完成绘图的布尔值。
        /// </summary>
        /// <param name="bmp">用于绘制的位图。</param>
        /// <param name="offset">椭圆的焦点。</param>
        /// <param name="semiMajorAxis">椭圆的半长轴。</param>
        /// <param name="eccentricity">椭圆的离心率。</param>
        /// <param name="rotateAngle">椭圆的旋转角（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向，焦点到近焦点连线相对于 +X 轴的角度）。</param>
        /// <param name="refPhase">参考相位（弧度）（以 +X 轴为 0 弧度，从 +X 轴指向 +Y 轴的方向为正方向）。</param>
        /// <param name="color">线条颜色。</param>
        /// <param name="width">线条宽度，0 表示填充。</param>
        /// <param name="antiAlias">是否使用抗锯齿模式绘图。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintLargeEllipse(Bitmap bmp, PointD offset, double semiMajorAxis, double eccentricity, double rotateAngle, double refPhase, Color color, float width, bool antiAlias) => PaintLargeEllipse(bmp, offset, semiMajorAxis, eccentricity, rotateAngle, refPhase, color, width, antiAlias, 32, 256, 4);

        /// <summary>
        /// 绘制一行带有阴影效果的文本，阴影位于文本右下方，并返回表示是否已经实际完成绘图的布尔值。
        /// </summary>
        /// <param name="bmp">用于绘制的位图。</param>
        /// <param name="text">文本内容。</param>
        /// <param name="font">文本字体。</param>
        /// <param name="frontColor">文本颜色。</param>
        /// <param name="backColor">阴影颜色，0 表示填充。</param>
        /// <param name="pt">文本左上角坐标。</param>
        /// <param name="offset">阴影偏移相对于文本字体大小的比例。</param>
        /// <param name="antiAlias">是否使用抗锯齿模式绘图。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintTextWithShadow(Bitmap bmp, string text, Font font, Color frontColor, Color backColor, PointF pt, float offset, bool antiAlias)
        {
            try
            {
                if (bmp is null || string.IsNullOrWhiteSpace(text) || font is null || frontColor.IsEmpty || frontColor.A == 0 || ((PointD)pt).IsNaNOrInfinity || InternalMethod.IsNaNOrInfinity(offset) || offset < 0)
                {
                    return false;
                }
                else
                {
                    using (Graphics graph = Graphics.FromImage(bmp))
                    {
                        if (antiAlias)
                        {
                            graph.TextRenderingHint = TextRenderingHint.AntiAlias;
                        }

                        //

                        if (!backColor.IsEmpty && backColor.A > 0)
                        {
                            float Off = font.Size * offset;

                            if (Off >= 2)
                            {
                                using (Brush br = new SolidBrush(Color.FromArgb(32, backColor)))
                                {
                                    graph.DrawString(text, font, br, new PointF(pt.X + Off, pt.Y + Off));
                                }
                                using (Brush br = new SolidBrush(Color.FromArgb(64, backColor)))
                                {
                                    graph.DrawString(text, font, br, new PointF(pt.X + Off / 2, pt.Y + Off / 2));
                                }
                                using (Brush br = new SolidBrush(Color.FromArgb(96, backColor)))
                                {
                                    graph.DrawString(text, font, br, new PointF(pt.X + Off / 4, pt.Y + Off / 2));
                                }
                            }
                            else if (Off >= 1)
                            {
                                using (Brush br = new SolidBrush(Color.FromArgb(64, backColor)))
                                {
                                    graph.DrawString(text, font, br, new PointF(pt.X + Off, pt.Y + Off));
                                }
                                using (Brush br = new SolidBrush(Color.FromArgb(128, backColor)))
                                {
                                    graph.DrawString(text, font, br, new PointF(pt.X + Off / 2, pt.Y + Off / 2));
                                }
                            }
                            else if (Off >= 0.5 || frontColor != backColor)
                            {
                                using (Brush br = new SolidBrush(Color.FromArgb(192, backColor)))
                                {
                                    graph.DrawString(text, font, br, new PointF(pt.X + Off, pt.Y + Off));
                                }
                            }
                        }

                        using (Brush br = new SolidBrush(frontColor))
                        {
                            graph.DrawString(text, font, br, pt);
                        }
                    }

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 在透明窗口中绘制图像，并返回表示是否已经实际完成绘图的布尔值。
        /// </summary>
        /// <param name="form">用于绘制的窗口。</param>
        /// <param name="bmp">绘制的图像。</param>
        /// <param name="opacity">绘制图像的不透明度。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintImageOnTransparentForm(Form form, Bitmap bmp, double opacity)
        {
            try
            {
                if (form is null || bmp is null || InternalMethod.IsNaNOrInfinity(opacity) || opacity <= 0)
                {
                    return false;
                }
                else
                {
                    opacity = Math.Max(0, Math.Min(opacity, 100));

                    if (opacity > 1)
                    {
                        opacity /= 100;
                    }

                    //

                    try
                    {
                        User32.SetWindowLongA(form.Handle.ToInt32(), -20, User32.GetWindowLongA(form.Handle.ToInt32(), -20) | 524288);
                    }
                    catch { }

                    if (bmp.PixelFormat != PixelFormat.Format32bppArgb)
                    {
                        return false;
                    }
                    else
                    {
                        IntPtr HDcDst = User32.GetDC(IntPtr.Zero);
                        IntPtr HDcSrc = Gdi32.CreateCompatibleDC(HDcDst);

                        IntPtr Hbitmap = IntPtr.Zero;
                        IntPtr HGdiObj = IntPtr.Zero;

                        try
                        {
                            Hbitmap = bmp.GetHbitmap(Color.FromArgb(0));
                            HGdiObj = Gdi32.SelectObject(HDcSrc, Hbitmap);

                            Point PtDst = form.Location;
                            Size Size = bmp.Size;
                            Point PtSrc = new Point(0, 0);

                            User32.BLENDFUNCTION Blend = new User32.BLENDFUNCTION()
                            {
                                BlendOp = 0,
                                BlendFlags = 0,
                                SourceConstantAlpha = (byte)Math.Max(0, Math.Min((int)(opacity * 255), 255)),
                                AlphaFormat = 1
                            };

                            User32.UpdateLayeredWindow(form.Handle, HDcDst, ref PtDst, ref Size, HDcSrc, ref PtSrc, 0, ref Blend, 2);
                        }
                        finally
                        {
                            User32.ReleaseDC(IntPtr.Zero, HDcDst);

                            if (Hbitmap != IntPtr.Zero)
                            {
                                Gdi32.SelectObject(HDcSrc, HGdiObj);
                                Gdi32.DeleteObject(Hbitmap);
                            }

                            Gdi32.DeleteDC(HDcSrc);
                        }

                        //

                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}