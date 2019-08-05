/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.Painting3D
Version 19.8.5.2000

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

namespace Com
{
    /// <summary>
    /// 为 3D 绘图提供静态方法。
    /// </summary>
    public static class Painting3D
    {
        /// <summary>
        /// 绘制一个长方体，并返回表示是否已经实际完成绘图的布尔值。
        /// </summary>
        /// <param name="bmp">用于绘制的位图。</param>
        /// <param name="center">长方体的中心坐标。</param>
        /// <param name="size">长方体的大小。</param>
        /// <param name="color">长方体的颜色。</param>
        /// <param name="edgeWidth">长方体的棱的宽度。</param>
        /// <param name="affineMatrixList">仿射矩阵（左矩阵）列表。</param>
        /// <param name="trueLenDist">真实尺寸距离。</param>
        /// <param name="illuminationDirection">光照方向。</param>
        /// <param name="illuminationDirectionIsAfterAffineTransform">光照方向是否基于仿射变换之后的坐标系。</param>
        /// <param name="exposure">曝光，取值范围为 [-100, 100]。</param>
        /// <param name="antiAlias">是否使用抗锯齿模式绘图。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintCuboid(Bitmap bmp, PointD3D center, PointD3D size, Color color, float edgeWidth, List<Matrix> affineMatrixList, double trueLenDist, PointD3D illuminationDirection, bool illuminationDirectionIsAfterAffineTransform, double exposure, bool antiAlias)
        {
            try
            {
                if (bmp == null || center.IsNaNOrInfinity || (size.IsNaNOrInfinity || size.IsZero) || (color.IsEmpty || color.A == 0) || InternalMethod.IsNaNOrInfinity(edgeWidth) || InternalMethod.IsNullOrEmpty(affineMatrixList) || (InternalMethod.IsNaNOrInfinity(trueLenDist) || trueLenDist < 0) || illuminationDirection.IsNaNOrInfinity || InternalMethod.IsNaNOrInfinity(exposure))
                {
                    return false;
                }
                else
                {
                    PointD3D P3D_000 = new PointD3D(0, 0, 0);
                    PointD3D P3D_100 = new PointD3D(1, 0, 0);
                    PointD3D P3D_010 = new PointD3D(0, 1, 0);
                    PointD3D P3D_110 = new PointD3D(1, 1, 0);
                    PointD3D P3D_001 = new PointD3D(0, 0, 1);
                    PointD3D P3D_101 = new PointD3D(1, 0, 1);
                    PointD3D P3D_011 = new PointD3D(0, 1, 1);
                    PointD3D P3D_111 = new PointD3D(1, 1, 1);

                    P3D_000 = (P3D_000 - 0.5) * size + center;
                    P3D_100 = (P3D_100 - 0.5) * size + center;
                    P3D_010 = (P3D_010 - 0.5) * size + center;
                    P3D_110 = (P3D_110 - 0.5) * size + center;
                    P3D_001 = (P3D_001 - 0.5) * size + center;
                    P3D_101 = (P3D_101 - 0.5) * size + center;
                    P3D_011 = (P3D_011 - 0.5) * size + center;
                    P3D_111 = (P3D_111 - 0.5) * size + center;

                    P3D_000.AffineTransform(affineMatrixList);
                    P3D_100.AffineTransform(affineMatrixList);
                    P3D_010.AffineTransform(affineMatrixList);
                    P3D_110.AffineTransform(affineMatrixList);
                    P3D_001.AffineTransform(affineMatrixList);
                    P3D_101.AffineTransform(affineMatrixList);
                    P3D_011.AffineTransform(affineMatrixList);
                    P3D_111.AffineTransform(affineMatrixList);

                    PointD3D PrjCenter = new PointD3D(bmp.Width / 2, bmp.Height / 2, 0);

                    PointD P2D_000 = P3D_000.ProjectToXY(PrjCenter, trueLenDist);
                    PointD P2D_100 = P3D_100.ProjectToXY(PrjCenter, trueLenDist);
                    PointD P2D_010 = P3D_010.ProjectToXY(PrjCenter, trueLenDist);
                    PointD P2D_110 = P3D_110.ProjectToXY(PrjCenter, trueLenDist);
                    PointD P2D_001 = P3D_001.ProjectToXY(PrjCenter, trueLenDist);
                    PointD P2D_101 = P3D_101.ProjectToXY(PrjCenter, trueLenDist);
                    PointD P2D_011 = P3D_011.ProjectToXY(PrjCenter, trueLenDist);
                    PointD P2D_111 = P3D_111.ProjectToXY(PrjCenter, trueLenDist);

                    PointF P_000 = P2D_000.ToPointF();
                    PointF P_100 = P2D_100.ToPointF();
                    PointF P_010 = P2D_010.ToPointF();
                    PointF P_110 = P2D_110.ToPointF();
                    PointF P_001 = P2D_001.ToPointF();
                    PointF P_101 = P2D_101.ToPointF();
                    PointF P_011 = P2D_011.ToPointF();
                    PointF P_111 = P2D_111.ToPointF();

                    List<PointD3D[]> Element3D = new List<PointD3D[]>(18)
                    {
                        // XY 面
                        new PointD3D[] { P3D_000, P3D_010, P3D_110, P3D_100 },
                        new PointD3D[] { P3D_001, P3D_011, P3D_111, P3D_101 },

                        // YZ 面
                        new PointD3D[] { P3D_000, P3D_001, P3D_011, P3D_010 },
                        new PointD3D[] { P3D_100, P3D_101, P3D_111, P3D_110 },

                        // ZX 面
                        new PointD3D[] { P3D_000, P3D_001, P3D_101, P3D_100 },
                        new PointD3D[] { P3D_010, P3D_011, P3D_111, P3D_110 },
                        
                        // X 棱
                        new PointD3D[] { P3D_000, P3D_100 },
                        new PointD3D[] { P3D_010, P3D_110 },
                        new PointD3D[] { P3D_001, P3D_101 },
                        new PointD3D[] { P3D_011, P3D_111 },

                        // Y 棱
                        new PointD3D[] { P3D_000, P3D_010 },
                        new PointD3D[] { P3D_100, P3D_110 },
                        new PointD3D[] { P3D_001, P3D_011 },
                        new PointD3D[] { P3D_101, P3D_111 },

                        // Z 棱
                        new PointD3D[] { P3D_000, P3D_001 },
                        new PointD3D[] { P3D_100, P3D_101 },
                        new PointD3D[] { P3D_010, P3D_011 },
                        new PointD3D[] { P3D_110, P3D_111 }
                    };

                    List<PointF[]> Element2D = new List<PointF[]>(18)
                    {
                        // XY 面
                        new PointF[] { P_000, P_010, P_110, P_100 },
                        new PointF[] { P_001, P_011, P_111, P_101 },

                        // YZ 面
                        new PointF[] { P_000, P_001, P_011, P_010 },
                        new PointF[] { P_100, P_101, P_111, P_110 },

                        // ZX 面
                        new PointF[] { P_000, P_001, P_101, P_100 },
                        new PointF[] { P_010, P_011, P_111, P_110 },
                        
                        // X 棱
                        new PointF[] { P_000, P_100 },
                        new PointF[] { P_010, P_110 },
                        new PointF[] { P_001, P_101 },
                        new PointF[] { P_011, P_111 },

                        // Y 棱
                        new PointF[] { P_000, P_010 },
                        new PointF[] { P_100, P_110 },
                        new PointF[] { P_001, P_011 },
                        new PointF[] { P_101, P_111 },

                        // Z 棱
                        new PointF[] { P_000, P_001 },
                        new PointF[] { P_100, P_101 },
                        new PointF[] { P_010, P_011 },
                        new PointF[] { P_110, P_111 }
                    };

                    //

                    bool CuboidIsVisible = false;

                    List<bool> ElementVisible = new List<bool>(Element2D.Count);

                    for (int i = 0; i < Element2D.Count; i++)
                    {
                        bool EVisible = false;

                        foreach (PointF P in Element2D[i])
                        {
                            PointD P2D = P;

                            if (P2D.IsNaNOrInfinity)
                            {
                                EVisible = false;

                                break;
                            }
                            else
                            {
                                if (Geometry.PointIsVisibleInRectangle(P2D, new RectangleF(new Point(0, 0), bmp.Size)))
                                {
                                    EVisible = true;
                                }
                            }
                        }

                        ElementVisible.Add(EVisible);

                        if (!CuboidIsVisible && EVisible)
                        {
                            CuboidIsVisible = true;
                        }
                    }

                    if (!CuboidIsVisible)
                    {
                        return false;
                    }
                    else
                    {
                        List<double> IlluminationIntensity = new List<double>(Element3D.Count);

                        double Exposure = Math.Max(-2, Math.Min(exposure / 50, 2));

                        if (illuminationDirection.IsZero)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                IlluminationIntensity.Add(Exposure);
                            }

                            for (int i = 6; i < Element3D.Count; i++)
                            {
                                IlluminationIntensity.Add(Constant.HalfSqrt2 * (Exposure + 1) + (Exposure - 1));
                            }
                        }
                        else
                        {
                            List<PointD3D> NormalVector = new List<PointD3D>(6)
                            {
                                // XY 面
                                new PointD3D(0, 0, -1),
                                new PointD3D(0, 0, 1),

                                // YZ 面
                                new PointD3D(-1, 0, 0),
                                new PointD3D(1, 0, 0),

                                // ZX 面
                                new PointD3D(0, -1, 0),
                                new PointD3D(0, 1, 0),
                            };

                            if (illuminationDirectionIsAfterAffineTransform)
                            {
                                PointD3D NewOrigin = new PointD3D(0, 0, 0).AffineTransformCopy(affineMatrixList);

                                for (int i = 0; i < NormalVector.Count; i++)
                                {
                                    NormalVector[i] = NormalVector[i].AffineTransformCopy(affineMatrixList) - NewOrigin;
                                }
                            }

                            List<double> Angle = new List<double>(NormalVector.Count);

                            for (int i = 0; i < NormalVector.Count; i++)
                            {
                                Angle.Add(illuminationDirection.AngleFrom(NormalVector[i]));
                            }

                            for (int i = 0; i < Angle.Count; i++)
                            {
                                double A = Angle[i];
                                double CosA = Math.Cos(A);
                                double CosSqrA = CosA * CosA;

                                double _IlluminationIntensity = (A < Constant.HalfPi ? -CosSqrA : (A > Constant.HalfPi ? CosSqrA : 0));

                                if (color.A < 255 && A != Constant.HalfPi)
                                {
                                    double Transmittance = 1 - (double)color.A / 255;

                                    if (A < Constant.HalfPi)
                                    {
                                        _IlluminationIntensity += (Transmittance * Math.Abs(CosA) * CosSqrA);
                                    }
                                    else
                                    {
                                        _IlluminationIntensity -= ((1 - Transmittance) * (1 - Math.Abs(CosA)) * CosSqrA);
                                    }
                                }

                                _IlluminationIntensity += Exposure;

                                IlluminationIntensity.Add(_IlluminationIntensity);
                            }

                            for (int i = 6; i < Element3D.Count; i++)
                            {
                                double _IlluminationIntensity = 0;

                                int Num = 0;

                                for (int j = 0; j < 6; j++)
                                {
                                    bool Flag = true;

                                    foreach (PointD3D P in Element3D[i])
                                    {
                                        if (!Element3D[j].Contains(P))
                                        {
                                            Flag = false;

                                            break;
                                        }
                                    }

                                    if (Flag)
                                    {
                                        _IlluminationIntensity += IlluminationIntensity[j];

                                        Num++;
                                    }
                                }

                                _IlluminationIntensity = Constant.HalfSqrt2 * (_IlluminationIntensity / Num + 1) + (Exposure - 1);

                                IlluminationIntensity.Add(_IlluminationIntensity);
                            }
                        }

                        for (int i = 0; i < IlluminationIntensity.Count; i++)
                        {
                            IlluminationIntensity[i] = Math.Max(-1, Math.Min(IlluminationIntensity[i], 1));
                        }

                        //

                        List<Color> ElementColor = new List<Color>(IlluminationIntensity.Count);

                        for (int i = 0; i < IlluminationIntensity.Count; i++)
                        {
                            double _IlluminationIntensity = IlluminationIntensity[i];

                            if (_IlluminationIntensity == 0)
                            {
                                ElementColor.Add(color);
                            }
                            else
                            {
                                ColorX EColor = color;

                                if (_IlluminationIntensity < 0)
                                {
                                    EColor.Lightness_HSL += EColor.Lightness_HSL * _IlluminationIntensity;
                                }
                                else
                                {
                                    EColor.Lightness_HSL += (100 - EColor.Lightness_HSL) * _IlluminationIntensity;
                                }

                                ElementColor.Add(EColor.ToColor());
                            }
                        }

                        //

                        List<double> ElementZAvg = new List<double>(Element3D.Count);

                        for (int i = 0; i < Element3D.Count; i++)
                        {
                            PointD3D[] Element = Element3D[i];

                            double ZAvg = 0;

                            foreach (PointD3D P in Element)
                            {
                                ZAvg += P.Z;
                            }

                            ZAvg /= Element.Length;

                            ElementZAvg.Add(ZAvg);
                        }

                        List<int> ElementIndex = new List<int>(ElementZAvg.Count);

                        for (int i = 0; i < ElementZAvg.Count; i++)
                        {
                            ElementIndex.Add(i);
                        }

                        for (int i = 0; i < ElementZAvg.Count; i++)
                        {
                            for (int j = i + 1; j < ElementZAvg.Count; j++)
                            {
                                if (ElementZAvg[ElementIndex[i]] < ElementZAvg[ElementIndex[j]] || (ElementZAvg[ElementIndex[i]] <= ElementZAvg[ElementIndex[j]] + edgeWidth && Element2D[ElementIndex[i]].Length < Element2D[ElementIndex[j]].Length))
                                {
                                    int Temp = ElementIndex[i];
                                    ElementIndex[i] = ElementIndex[j];
                                    ElementIndex[j] = Temp;
                                }
                            }
                        }

                        //

                        using (Graphics Grph = Graphics.FromImage(bmp))
                        {
                            if (antiAlias)
                            {
                                Grph.SmoothingMode = SmoothingMode.AntiAlias;
                            }

                            //

                            for (int i = 0; i < ElementIndex.Count; i++)
                            {
                                int EIndex = ElementIndex[i];

                                if (ElementVisible[EIndex])
                                {
                                    Color EColor = ElementColor[EIndex];

                                    if (!EColor.IsEmpty && EColor.A > 0)
                                    {
                                        PointF[] Element = Element2D[EIndex];

                                        if (Element.Length >= 3)
                                        {
                                            try
                                            {
                                                using (SolidBrush Br = new SolidBrush(EColor))
                                                {
                                                    Grph.FillPolygon(Br, Element);
                                                }
                                            }
                                            catch { }
                                        }
                                        else if (Element.Length == 2)
                                        {
                                            if (edgeWidth > 0)
                                            {
                                                float EdgeWidth = (trueLenDist == 0 ? edgeWidth : (float)(trueLenDist / (ElementZAvg[EIndex] - PrjCenter.Z) * edgeWidth));

                                                try
                                                {
                                                    Brush Br;

                                                    Func<Color, double, int> GetAlpha = (Cr, Z) =>
                                                    {
                                                        int Alpha;

                                                        if (trueLenDist == 0)
                                                        {
                                                            Alpha = Cr.A;
                                                        }
                                                        else
                                                        {
                                                            if (Z - PrjCenter.Z <= trueLenDist)
                                                            {
                                                                Alpha = Cr.A;
                                                            }
                                                            else
                                                            {
                                                                Alpha = (int)Math.Max(0, Math.Min(trueLenDist / (Z - PrjCenter.Z) * Cr.A, 255));
                                                            }
                                                        }

                                                        if (EdgeWidth < 1)
                                                        {
                                                            Alpha = (int)(Alpha * EdgeWidth);
                                                        }

                                                        return Alpha;
                                                    };

                                                    if (((PointD)Element[0]).DistanceFrom(Element[1]) > 1)
                                                    {
                                                        int Alpha0 = GetAlpha(EColor, Element3D[EIndex][0].Z), Alpha1 = GetAlpha(EColor, Element3D[EIndex][1].Z);

                                                        Br = new LinearGradientBrush(Element[0], Element[1], Color.FromArgb(Alpha0, EColor), Color.FromArgb(Alpha1, EColor));
                                                    }
                                                    else
                                                    {
                                                        int Alpha = GetAlpha(EColor, ElementZAvg[EIndex]);

                                                        Br = new SolidBrush(Color.FromArgb(Alpha, EColor));
                                                    }

                                                    using (Pen Pn = new Pen(Br, EdgeWidth))
                                                    {
                                                        Grph.DrawLines(Pn, Element);
                                                    }

                                                    if (Br != null)
                                                    {
                                                        Br.Dispose();
                                                    }
                                                }
                                                catch { }
                                            }
                                        }
                                    }
                                }
                            }
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

        /// <summary>
        /// 绘制一个长方体，并返回表示是否已经实际完成绘图的布尔值。
        /// </summary>
        /// <param name="bmp">用于绘制的位图。</param>
        /// <param name="center">长方体的中心坐标。</param>
        /// <param name="size">长方体的大小。</param>
        /// <param name="color">长方体的颜色。</param>
        /// <param name="edgeWidth">长方体的棱的宽度。</param>
        /// <param name="affineMatrix">仿射矩阵（左矩阵）。</param>
        /// <param name="trueLenDist">真实尺寸距离。</param>
        /// <param name="illuminationDirection">光照方向。</param>
        /// <param name="illuminationDirectionIsAfterAffineTransform">光照方向是否基于仿射变换之后的坐标系。</param>
        /// <param name="exposure">曝光，取值范围为 [-100, 100]。</param>
        /// <param name="antiAlias">是否使用抗锯齿模式绘图。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintCuboid(Bitmap bmp, PointD3D center, PointD3D size, Color color, float edgeWidth, Matrix affineMatrix, double trueLenDist, PointD3D illuminationDirection, bool illuminationDirectionIsAfterAffineTransform, double exposure, bool antiAlias)
        {
            return PaintCuboid(bmp, center, size, color, edgeWidth, new List<Matrix>(1) { affineMatrix }, trueLenDist, illuminationDirection, illuminationDirectionIsAfterAffineTransform, exposure, antiAlias);
        }

        /// <summary>
        /// 绘制一个长方体，并返回表示是否已经实际完成绘图的布尔值。
        /// </summary>
        /// <param name="bmp">用于绘制的位图。</param>
        /// <param name="center">长方体的中心坐标。</param>
        /// <param name="size">长方体的大小。</param>
        /// <param name="color">长方体的颜色。</param>
        /// <param name="edgeWidth">长方体的棱的宽度。</param>
        /// <param name="affineMatrixList">仿射矩阵（左矩阵）列表。</param>
        /// <param name="trueLenDist">真实尺寸距离。</param>
        /// <param name="antiAlias">是否使用抗锯齿模式绘图。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintCuboid(Bitmap bmp, PointD3D center, PointD3D size, Color color, float edgeWidth, List<Matrix> affineMatrixList, double trueLenDist, bool antiAlias)
        {
            return PaintCuboid(bmp, center, size, color, edgeWidth, affineMatrixList, trueLenDist, PointD3D.Zero, false, 0, antiAlias);
        }

        /// <summary>
        /// 绘制一个长方体，并返回表示是否已经实际完成绘图的布尔值。
        /// </summary>
        /// <param name="bmp">用于绘制的位图。</param>
        /// <param name="center">长方体的中心坐标。</param>
        /// <param name="size">长方体的大小。</param>
        /// <param name="color">长方体的颜色。</param>
        /// <param name="edgeWidth">长方体的棱的宽度。</param>
        /// <param name="affineMatrix">仿射矩阵（左矩阵）。</param>
        /// <param name="trueLenDist">真实尺寸距离。</param>
        /// <param name="antiAlias">是否使用抗锯齿模式绘图。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintCuboid(Bitmap bmp, PointD3D center, PointD3D size, Color color, float edgeWidth, Matrix affineMatrix, double trueLenDist, bool antiAlias)
        {
            return PaintCuboid(bmp, center, size, color, edgeWidth, new List<Matrix>(1) { affineMatrix }, trueLenDist, PointD3D.Zero, false, 0, antiAlias);
        }
    }
}