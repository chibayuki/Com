/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2013-2018 chibayuki@foxmail.com

Com.ColorManipulation
Version 18.6.3.0000

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Reflection;

namespace Com
{
    /// <summary>
    /// 为基于 ColorX 结构或 Color 结构的颜色的基本处理提供静态方法。
    /// </summary>
    public static class ColorManipulation
    {
        private static string _GetColorName(int argb, string hexCode) // 根据 32 位 ARGB 值与 16 进制 ARGB 码获取颜色的名称。
        {
            try
            {
                PropertyInfo[] PInfo = typeof(Color).GetProperties();

                foreach (PropertyInfo V in PInfo)
                {
                    string Name = V.Name;
                    Color Cr = Color.FromName(Name);
                    KnownColor KC = Cr.ToKnownColor();

                    if ((KC >= KnownColor.Transparent && KC <= KnownColor.YellowGreen) && argb == Cr.ToArgb())
                    {
                        return string.Concat(Name, " (", hexCode, ")");
                    }
                }

                return hexCode;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取 ColorX 结构表示的颜色的名称。
        /// </summary>
        /// <param name="color">ColorX 结构表示的颜色。</param>
        public static string GetColorName(ColorX color)
        {
            try
            {
                return _GetColorName(color.ToARGB(), color.ARGBHexCode);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取 Color 结构表示的颜色的名称。
        /// </summary>
        /// <param name="color">Color 结构表示的颜色。</param>
        public static string GetColorName(Color color)
        {
            try
            {
                return GetColorName(new ColorX(color));
            }
            catch
            {
                return string.Empty;
            }
        }

        //

        /// <summary>
        /// 返回一个 ColorX 结构表示的随机颜色。
        /// </summary>
        public static ColorX GetRandomColorX()
        {
            try
            {
                return ColorX.RandomColor();
            }
            catch
            {
                return ColorX.Empty;
            }
        }

        /// <summary>
        /// 返回一个 Color 结构表示的随机颜色。
        /// </summary>
        public static Color GetRandomColor()
        {
            try
            {
                return GetRandomColorX().ToColor();
            }
            catch
            {
                return Color.Empty;
            }
        }

        //

        /// <summary>
        /// 返回 ColorX 结构表示的颜色的互补色。
        /// </summary>
        /// <param name="color">ColorX 结构表示的颜色。</param>
        public static ColorX GetComplementaryColor(ColorX color)
        {
            try
            {
                return color.ComplementaryColor;
            }
            catch
            {
                return ColorX.Empty;
            }
        }

        /// <summary>
        /// 返回 Color 结构表示的颜色的互补色。
        /// </summary>
        /// <param name="color">Color 结构表示的颜色。</param>
        public static Color GetComplementaryColor(Color color)
        {
            try
            {
                return GetComplementaryColor(new ColorX(color)).ToColor();
            }
            catch
            {
                return Color.Empty;
            }
        }

        /// <summary>
        /// 返回 ColorX 结构表示的颜色的灰度颜色。
        /// </summary>
        /// <param name="color">ColorX 结构表示的颜色。</param>
        public static ColorX GetGrayscaleColor(ColorX color)
        {
            try
            {
                return color.GrayscaleColor;
            }
            catch
            {
                return ColorX.Empty;
            }
        }

        /// <summary>
        /// 返回 Color 结构表示的颜色的灰度颜色。
        /// </summary>
        /// <param name="color">Color 结构表示的颜色。</param>
        public static Color GetGrayscaleColor(Color color)
        {
            try
            {
                return GetGrayscaleColor(new ColorX(color)).ToColor();
            }
            catch
            {
                return Color.Empty;
            }
        }

        //

        private static double _CheckProportion(double proportion) // 对双精度浮点数表示的比例的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(proportion) || double.IsInfinity(proportion))
            {
                proportion = 1;
            }
            else
            {
                proportion = Math.Max(0, Math.Min(proportion, 100));

                if (proportion > 1)
                {
                    proportion /= 100;
                }
            }

            return proportion;
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的两种颜色在 RGB 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">ColorX 结构表示的第一种颜色。</param>
        /// <param name="color2">ColorX 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        public static ColorX BlendByRGB(ColorX color1, ColorX color2, double proportion)
        {
            try
            {
                proportion = _CheckProportion(proportion);

                return ColorX.FromRGB(color1.Alpha * proportion + color2.Alpha * (1 - proportion), color1.RGB * proportion + color2.RGB * (1 - proportion));
            }
            catch
            {
                return ColorX.Empty;
            }
        }

        /// <summary>
        /// 返回将 Color 结构表示的两种颜色在 RGB 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">Color 结构表示的第一种颜色。</param>
        /// <param name="color2">Color 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        public static Color BlendByRGB(Color color1, Color color2, double proportion)
        {
            try
            {
                return BlendByRGB(new ColorX(color1), new ColorX(color2), proportion).ToColor();
            }
            catch
            {
                return Color.Empty;
            }
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的两种颜色在 HSV 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">ColorX 结构表示的第一种颜色。</param>
        /// <param name="color2">ColorX 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        public static ColorX BlendByHSV(ColorX color1, ColorX color2, double proportion)
        {
            try
            {
                proportion = _CheckProportion(proportion);

                return ColorX.FromHSV(color1.HSV * proportion + color2.HSV * (1 - proportion), color1.Opacity * proportion + color2.Opacity * (1 - proportion));
            }
            catch
            {
                return ColorX.Empty;
            }
        }

        /// <summary>
        /// 返回将 Color 结构表示的两种颜色在 HSV 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">Color 结构表示的第一种颜色。</param>
        /// <param name="color2">Color 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        public static Color BlendByHSV(Color color1, Color color2, double proportion)
        {
            try
            {
                return BlendByHSV(new ColorX(color1), new ColorX(color2), proportion).ToColor();
            }
            catch
            {
                return Color.Empty;
            }
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的两种颜色在 HSL 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">ColorX 结构表示的第一种颜色。</param>
        /// <param name="color2">ColorX 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        public static ColorX BlendByHSL(ColorX color1, ColorX color2, double proportion)
        {
            try
            {
                proportion = _CheckProportion(proportion);

                return ColorX.FromHSL(color1.HSL * proportion + color2.HSL * (1 - proportion), color1.Opacity * proportion + color2.Opacity * (1 - proportion));
            }
            catch
            {
                return ColorX.Empty;
            }
        }

        /// <summary>
        /// 返回将 Color 结构表示的两种颜色在 HSL 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">Color 结构表示的第一种颜色。</param>
        /// <param name="color2">Color 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        public static Color BlendByHSL(Color color1, Color color2, double proportion)
        {
            try
            {
                return BlendByHSL(new ColorX(color1), new ColorX(color2), proportion).ToColor();
            }
            catch
            {
                return Color.Empty;
            }
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的两种颜色在 CMYK 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">ColorX 结构表示的第一种颜色。</param>
        /// <param name="color2">ColorX 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        public static ColorX BlendByCMYK(ColorX color1, ColorX color2, double proportion)
        {
            try
            {
                proportion = _CheckProportion(proportion);

                return ColorX.FromCMYK(color1.CMYK * proportion + color2.CMYK * (1 - proportion), color1.Opacity * proportion + color2.Opacity * (1 - proportion));
            }
            catch
            {
                return ColorX.Empty;
            }
        }

        /// <summary>
        /// 返回将 Color 结构表示的两种颜色在 CMYK 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">Color 结构表示的第一种颜色。</param>
        /// <param name="color2">Color 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        public static Color BlendByCMYK(Color color1, Color color2, double proportion)
        {
            try
            {
                return BlendByCMYK(new ColorX(color1), new ColorX(color2), proportion).ToColor();
            }
            catch
            {
                return Color.Empty;
            }
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的两种颜色在 LAB 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">ColorX 结构表示的第一种颜色。</param>
        /// <param name="color2">ColorX 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        public static ColorX BlendByLAB(ColorX color1, ColorX color2, double proportion)
        {
            try
            {
                proportion = _CheckProportion(proportion);

                return ColorX.FromLAB(color1.LAB * proportion + color2.LAB * (1 - proportion), color1.Opacity * proportion + color2.Opacity * (1 - proportion));
            }
            catch
            {
                return ColorX.Empty;
            }
        }

        /// <summary>
        /// 返回将 Color 结构表示的两种颜色在 LAB 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">Color 结构表示的第一种颜色。</param>
        /// <param name="color2">Color 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        public static Color BlendByLAB(Color color1, Color color2, double proportion)
        {
            try
            {
                return BlendByLAB(new ColorX(color1), new ColorX(color2), proportion).ToColor();
            }
            catch
            {
                return Color.Empty;
            }
        }

        //

        private static double _CheckLevel(double level) // 对双精度浮点数表示的调整程度的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(level) || double.IsInfinity(level))
            {
                level = 0;
            }
            else
            {
                level = Math.Max(-100, Math.Min(level, 100));

                if (level < -1 || level > 1)
                {
                    level /= 100;
                }
            }

            return level;
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的颜色在 HSV 色彩空间调整明度得到的颜色。
        /// </summary>
        /// <param name="color">ColorX 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        public static ColorX ShiftLightnessByHSV(ColorX color, double level)
        {
            try
            {
                level = _CheckLevel(level);

                if (level != 0)
                {
                    if (level < 0)
                    {
                        return color.AtBrightness(color.Brightness * (1 + level));
                    }
                    else
                    {
                        return color.AtBrightness(100 - (100 - color.Brightness) * (1 - level));
                    }
                }

                return color;
            }
            catch
            {
                return ColorX.Empty;
            }
        }

        /// <summary>
        /// 返回将 Color 结构表示的颜色在 HSV 色彩空间调整明度得到的颜色。
        /// </summary>
        /// <param name="color">Color 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        public static Color ShiftLightnessByHSV(Color color, double level)
        {
            try
            {
                return ShiftLightnessByHSV(new ColorX(color), level).ToColor();
            }
            catch
            {
                return Color.Empty;
            }
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的颜色在 HSL 色彩空间调整明度得到的颜色。
        /// </summary>
        /// <param name="color">ColorX 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        public static ColorX ShiftLightnessByHSL(ColorX color, double level)
        {
            try
            {
                level = _CheckLevel(level);

                if (level != 0)
                {
                    if (level < 0)
                    {
                        return color.AtLightness_HSL(color.Lightness_HSL * (1 + level));
                    }
                    else
                    {
                        return color.AtLightness_HSL(100 - (100 - color.Lightness_HSL) * (1 - level));
                    }
                }

                return color;
            }
            catch
            {
                return ColorX.Empty;
            }
        }

        /// <summary>
        /// 返回将 Color 结构表示的颜色在 HSL 色彩空间调整明度得到的颜色。
        /// </summary>
        /// <param name="color">Color 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        public static Color ShiftLightnessByHSL(Color color, double level)
        {
            try
            {
                return ShiftLightnessByHSL(new ColorX(color), level).ToColor();
            }
            catch
            {
                return Color.Empty;
            }
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的颜色在 LAB 色彩空间调整明度得到的颜色。
        /// </summary>
        /// <param name="color">ColorX 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        public static ColorX ShiftLightnessByLAB(ColorX color, double level)
        {
            try
            {
                level = _CheckLevel(level);

                if (level != 0)
                {
                    if (level < 0)
                    {
                        return color.AtLightness_LAB(color.Lightness_LAB * (1 + level));
                    }
                    else
                    {
                        return color.AtLightness_LAB(100 - (100 - color.Lightness_LAB) * (1 - level));
                    }
                }

                return color;
            }
            catch
            {
                return ColorX.Empty;
            }
        }

        /// <summary>
        /// 返回将 Color 结构表示的颜色在 LAB 色彩空间调整明度得到的颜色。
        /// </summary>
        /// <param name="color">Color 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        public static Color ShiftLightnessByLAB(Color color, double level)
        {
            try
            {
                return ShiftLightnessByLAB(new ColorX(color), level).ToColor();
            }
            catch
            {
                return Color.Empty;
            }
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的颜色在 HSV 色彩空间调整饱和度得到的颜色。
        /// </summary>
        /// <param name="color">ColorX 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        public static ColorX ShiftSaturationByHSV(ColorX color, double level)
        {
            try
            {
                level = _CheckLevel(level);

                if (level != 0)
                {
                    if (level < 0)
                    {
                        return color.AtSaturation_HSV(color.Saturation_HSV * (1 + level));
                    }
                    else
                    {
                        return color.AtSaturation_HSV(100 - (100 - color.Saturation_HSV) * (1 - level));
                    }
                }

                return color;
            }
            catch
            {
                return ColorX.Empty;
            }
        }

        /// <summary>
        /// 返回将 Color 结构表示的颜色在 HSV 色彩空间调整饱和度得到的颜色。
        /// </summary>
        /// <param name="color">Color 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        public static Color ShiftSaturationByHSV(Color color, double level)
        {
            try
            {
                return ShiftSaturationByHSV(new ColorX(color), level).ToColor();
            }
            catch
            {
                return Color.Empty;
            }
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的颜色在 HSL 色彩空间调整饱和度得到的颜色。
        /// </summary>
        /// <param name="color">ColorX 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        public static ColorX ShiftSaturationByHSL(ColorX color, double level)
        {
            try
            {
                level = _CheckLevel(level);

                if (level != 0)
                {
                    if (level < 0)
                    {
                        return color.AtSaturation_HSL(color.Saturation_HSL * (1 + level));
                    }
                    else
                    {
                        return color.AtSaturation_HSL(100 - (100 - color.Saturation_HSL) * (1 - level));
                    }
                }

                return color;
            }
            catch
            {
                return ColorX.Empty;
            }
        }

        /// <summary>
        /// 返回将 Color 结构表示的颜色在 HSL 色彩空间调整饱和度得到的颜色。
        /// </summary>
        /// <param name="color">Color 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        public static Color ShiftSaturationByHSL(Color color, double level)
        {
            try
            {
                return ShiftSaturationByHSL(new ColorX(color), level).ToColor();
            }
            catch
            {
                return Color.Empty;
            }
        }
    }
}