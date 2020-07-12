/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

Com.Painting3D
Version 20.7.12.1800

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
        private static readonly PointD3D _Vertex000 = new PointD3D(0, 0, 0) - 0.5; // 正方体的 000 顶点。
        private static readonly PointD3D _Vertex100 = new PointD3D(1, 0, 0) - 0.5; // 正方体的 100 顶点。
        private static readonly PointD3D _Vertex010 = new PointD3D(0, 1, 0) - 0.5; // 正方体的 010 顶点。
        private static readonly PointD3D _Vertex110 = new PointD3D(1, 1, 0) - 0.5; // 正方体的 110 顶点。
        private static readonly PointD3D _Vertex001 = new PointD3D(0, 0, 1) - 0.5; // 正方体的 001 顶点。
        private static readonly PointD3D _Vertex101 = new PointD3D(1, 0, 1) - 0.5; // 正方体的 101 顶点。
        private static readonly PointD3D _Vertex011 = new PointD3D(0, 1, 1) - 0.5; // 正方体的 011 顶点。
        private static readonly PointD3D _Vertex111 = new PointD3D(1, 1, 1) - 0.5; // 正方体的 111 顶点。

        private static readonly PointD3D _NormalVectorNXY = new PointD3D(0, 0, -1); // 正方体表面的 -XY 法向量。
        private static readonly PointD3D _NormalVectorPXY = new PointD3D(0, 0, 1); // 正方体表面的 +XY 法向量。
        private static readonly PointD3D _NormalVectorNYZ = new PointD3D(-1, 0, 0); // 正方体表面的 -YZ 法向量。
        private static readonly PointD3D _NormalVectorPYZ = new PointD3D(1, 0, 0); // 正方体表面的 +YZ 法向量。
        private static readonly PointD3D _NormalVectorNZX = new PointD3D(0, -1, 0); // 正方体表面的 -ZX 法向量。
        private static readonly PointD3D _NormalVectorPZX = new PointD3D(0, 1, 0); // 正方体表面的 +ZX 法向量。

        //

        /// <summary>
        /// 绘制一个长方体，并返回表示是否已经实际完成绘图的布尔值。
        /// </summary>
        /// <param name="bmp">用于绘制的位图。</param>
        /// <param name="center">长方体的中心坐标。</param>
        /// <param name="size">长方体的大小。</param>
        /// <param name="color">长方体的颜色。</param>
        /// <param name="edgeWidth">长方体的棱的宽度。</param>
        /// <param name="affineMatrices">仿射矩阵（左矩阵）列表。</param>
        /// <param name="focalLength">焦距。</param>
        /// <param name="illuminationDirection">光照方向。</param>
        /// <param name="illuminationDirectionIsAfterAffineTransform">光照方向是否基于仿射变换之后的坐标系。</param>
        /// <param name="exposure">曝光，取值范围为 [-100, 100]。</param>
        /// <param name="antiAlias">是否使用抗锯齿模式绘图。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintCuboid(Bitmap bmp, PointD3D center, PointD3D size, Color color, float edgeWidth, List<Matrix> affineMatrices, double focalLength, PointD3D illuminationDirection, bool illuminationDirectionIsAfterAffineTransform, double exposure, bool antiAlias)
        {
            try
            {
                if (bmp is null || center.IsNaNOrInfinity || (size.IsNaNOrInfinity || size.IsZero) || (color.IsEmpty || color.A == 0) || InternalMethod.IsNaNOrInfinity(edgeWidth) || InternalMethod.IsNullOrEmpty(affineMatrices) || (InternalMethod.IsNaNOrInfinity(focalLength) || focalLength < 0) || illuminationDirection.IsNaNOrInfinity || InternalMethod.IsNaNOrInfinity(exposure))
                {
                    return false;
                }
                else
                {
                    PointD3D P3D_000 = _Vertex000;
                    PointD3D P3D_100 = _Vertex100;
                    PointD3D P3D_010 = _Vertex010;
                    PointD3D P3D_110 = _Vertex110;
                    PointD3D P3D_001 = _Vertex001;
                    PointD3D P3D_101 = _Vertex101;
                    PointD3D P3D_011 = _Vertex011;
                    PointD3D P3D_111 = _Vertex111;

                    P3D_000.Scale(size);
                    P3D_100.Scale(size);
                    P3D_010.Scale(size);
                    P3D_110.Scale(size);
                    P3D_001.Scale(size);
                    P3D_101.Scale(size);
                    P3D_011.Scale(size);
                    P3D_111.Scale(size);

                    P3D_000.Offset(center);
                    P3D_100.Offset(center);
                    P3D_010.Offset(center);
                    P3D_110.Offset(center);
                    P3D_001.Offset(center);
                    P3D_101.Offset(center);
                    P3D_011.Offset(center);
                    P3D_111.Offset(center);

                    P3D_000.AffineTransform(affineMatrices);
                    P3D_100.AffineTransform(affineMatrices);
                    P3D_010.AffineTransform(affineMatrices);
                    P3D_110.AffineTransform(affineMatrices);
                    P3D_001.AffineTransform(affineMatrices);
                    P3D_101.AffineTransform(affineMatrices);
                    P3D_011.AffineTransform(affineMatrices);
                    P3D_111.AffineTransform(affineMatrices);

                    PointD3D PrjCenter = new PointD3D(bmp.Width / 2, bmp.Height / 2, 0);

                    PointD P2D_000 = P3D_000.ProjectToXY(PrjCenter, focalLength);
                    PointD P2D_100 = P3D_100.ProjectToXY(PrjCenter, focalLength);
                    PointD P2D_010 = P3D_010.ProjectToXY(PrjCenter, focalLength);
                    PointD P2D_110 = P3D_110.ProjectToXY(PrjCenter, focalLength);
                    PointD P2D_001 = P3D_001.ProjectToXY(PrjCenter, focalLength);
                    PointD P2D_101 = P3D_101.ProjectToXY(PrjCenter, focalLength);
                    PointD P2D_011 = P3D_011.ProjectToXY(PrjCenter, focalLength);
                    PointD P2D_111 = P3D_111.ProjectToXY(PrjCenter, focalLength);

                    PointF P_000 = P2D_000.ToPointF();
                    PointF P_100 = P2D_100.ToPointF();
                    PointF P_010 = P2D_010.ToPointF();
                    PointF P_110 = P2D_110.ToPointF();
                    PointF P_001 = P2D_001.ToPointF();
                    PointF P_101 = P2D_101.ToPointF();
                    PointF P_011 = P2D_011.ToPointF();
                    PointF P_111 = P2D_111.ToPointF();

                    PointD3D[][] Element3D = new PointD3D[][]
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

                    PointF[][] Element2D = new PointF[][]
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

                    bool[] ElementVisible = new bool[Element2D.Length];

                    for (int i = 0; i < Element2D.Length; i++)
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

                        ElementVisible[i] = EVisible;

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
                        double[] IlluminationIntensity = new double[Element3D.Length];

                        double Exposure = Math.Max(-2, Math.Min(exposure / 50, 2));

                        if (illuminationDirection.IsZero)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                IlluminationIntensity[i] = Exposure;
                            }

                            for (int i = 6; i < Element3D.Length; i++)
                            {
                                IlluminationIntensity[i] = Constant.HalfSqrt2 * (Exposure + 1) + (Exposure - 1);
                            }
                        }
                        else
                        {
                            PointD3D[] NormalVector = new PointD3D[]
                            {
                                // XY 面
                                _NormalVectorNXY,
                                _NormalVectorPXY,

                                // YZ 面
                                _NormalVectorNYZ,
                                _NormalVectorPYZ,

                                // ZX 面
                                _NormalVectorNZX,
                                _NormalVectorPZX
                            };

                            if (illuminationDirectionIsAfterAffineTransform)
                            {
                                PointD3D NewOriginOpposite = PointD3D.Zero.AffineTransformCopy(affineMatrices).Opposite;

                                for (int i = 0; i < NormalVector.Length; i++)
                                {
                                    NormalVector[i].AffineTransform(affineMatrices);
                                    NormalVector[i].Offset(NewOriginOpposite);
                                }
                            }

                            double[] Angle = new double[NormalVector.Length];

                            for (int i = 0; i < NormalVector.Length; i++)
                            {
                                Angle[i] = illuminationDirection.AngleFrom(NormalVector[i]);
                            }

                            for (int i = 0; i < Angle.Length; i++)
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

                                IlluminationIntensity[i] = _IlluminationIntensity;
                            }

                            for (int i = 6; i < Element3D.Length; i++)
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

                                IlluminationIntensity[i] = _IlluminationIntensity;
                            }
                        }

                        for (int i = 0; i < IlluminationIntensity.Length; i++)
                        {
                            IlluminationIntensity[i] = Math.Max(-1, Math.Min(IlluminationIntensity[i], 1));
                        }

                        //

                        Color[] ElementColor = new Color[IlluminationIntensity.Length];

                        for (int i = 0; i < IlluminationIntensity.Length; i++)
                        {
                            double _IlluminationIntensity = IlluminationIntensity[i];

                            if (_IlluminationIntensity == 0)
                            {
                                ElementColor[i] = color;
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

                                ElementColor[i] = EColor.ToColor();
                            }
                        }

                        //

                        double[] ElementZAvg = new double[Element3D.Length];

                        for (int i = 0; i < Element3D.Length; i++)
                        {
                            PointD3D[] Element = Element3D[i];

                            double ZAvg = 0;

                            foreach (PointD3D P in Element)
                            {
                                ZAvg += P.Z;
                            }

                            ZAvg /= Element.Length;

                            ElementZAvg[i] = ZAvg;
                        }

                        int[] ElementIndex = new int[ElementZAvg.Length];

                        for (int i = 0; i < ElementZAvg.Length; i++)
                        {
                            ElementIndex[i] = i;
                        }

                        for (int i = 0; i < ElementZAvg.Length; i++)
                        {
                            for (int j = i + 1; j < ElementZAvg.Length; j++)
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

                            for (int i = 0; i < ElementIndex.Length; i++)
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
                                                float EdgeWidth = (focalLength == 0 ? edgeWidth : (float)(focalLength / (ElementZAvg[EIndex] - PrjCenter.Z) * edgeWidth));

                                                try
                                                {
                                                    Brush Br;

                                                    Func<Color, double, int> GetAlpha = (Cr, Z) =>
                                                    {
                                                        int Alpha;

                                                        if (focalLength == 0)
                                                        {
                                                            Alpha = Cr.A;
                                                        }
                                                        else
                                                        {
                                                            if (Z - PrjCenter.Z <= focalLength)
                                                            {
                                                                Alpha = Cr.A;
                                                            }
                                                            else
                                                            {
                                                                Alpha = (int)Math.Max(0, Math.Min(focalLength / (Z - PrjCenter.Z) * Cr.A, 255));
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

                                                    if (!(Br is null))
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
        /// <param name="focalLength">焦距。</param>
        /// <param name="illuminationDirection">光照方向。</param>
        /// <param name="illuminationDirectionIsAfterAffineTransform">光照方向是否基于仿射变换之后的坐标系。</param>
        /// <param name="exposure">曝光，取值范围为 [-100, 100]。</param>
        /// <param name="antiAlias">是否使用抗锯齿模式绘图。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintCuboid(Bitmap bmp, PointD3D center, PointD3D size, Color color, float edgeWidth, Matrix affineMatrix, double focalLength, PointD3D illuminationDirection, bool illuminationDirectionIsAfterAffineTransform, double exposure, bool antiAlias)
        {
            return PaintCuboid(bmp, center, size, color, edgeWidth, new List<Matrix>(1) { affineMatrix }, focalLength, illuminationDirection, illuminationDirectionIsAfterAffineTransform, exposure, antiAlias);
        }

        /// <summary>
        /// 绘制一个长方体，并返回表示是否已经实际完成绘图的布尔值。
        /// </summary>
        /// <param name="bmp">用于绘制的位图。</param>
        /// <param name="center">长方体的中心坐标。</param>
        /// <param name="size">长方体的大小。</param>
        /// <param name="color">长方体的颜色。</param>
        /// <param name="edgeWidth">长方体的棱的宽度。</param>
        /// <param name="affineMatrices">仿射矩阵（左矩阵）列表。</param>
        /// <param name="focalLength">焦距。</param>
        /// <param name="antiAlias">是否使用抗锯齿模式绘图。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintCuboid(Bitmap bmp, PointD3D center, PointD3D size, Color color, float edgeWidth, List<Matrix> affineMatrices, double focalLength, bool antiAlias)
        {
            return PaintCuboid(bmp, center, size, color, edgeWidth, affineMatrices, focalLength, PointD3D.Zero, false, 0, antiAlias);
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
        /// <param name="focalLength">焦距。</param>
        /// <param name="antiAlias">是否使用抗锯齿模式绘图。</param>
        /// <returns>布尔值，表示是否已经实际完成绘图。</returns>
        public static bool PaintCuboid(Bitmap bmp, PointD3D center, PointD3D size, Color color, float edgeWidth, Matrix affineMatrix, double focalLength, bool antiAlias)
        {
            return PaintCuboid(bmp, center, size, color, edgeWidth, new List<Matrix>(1) { affineMatrix }, focalLength, PointD3D.Zero, false, 0, antiAlias);
        }
    }
}