/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.ColorManipulation
Version 18.9.28.2200

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
            PropertyInfo[] PInfo = typeof(Color).GetProperties();

            foreach (PropertyInfo PropInfo in PInfo)
            {
                string Name = PropInfo.Name;
                Color Cr = Color.FromName(Name);
                KnownColor KC = Cr.ToKnownColor();

                if ((KC >= KnownColor.Transparent && KC <= KnownColor.YellowGreen) && argb == Cr.ToArgb())
                {
                    return string.Concat(Name, " (", hexCode, ")");
                }
            }

            return hexCode;
        }

        /// <summary>
        /// 获取 ColorX 结构表示的颜色的名称。
        /// </summary>
        /// <param name="color">ColorX 结构表示的颜色。</param>
        /// <returns>字符串，表示指定颜色的名称。</returns>
        public static string GetColorName(ColorX color)
        {
            return _GetColorName(color.ToARGB(), color.ARGBHexCode);
        }

        /// <summary>
        /// 获取 Color 结构表示的颜色的名称。
        /// </summary>
        /// <param name="color">Color 结构表示的颜色。</param>
        /// <returns>字符串，表示指定颜色的名称。</returns>
        public static string GetColorName(Color color)
        {
            return GetColorName((ColorX)color);
        }

        //

        /// <summary>
        /// 返回一个 ColorX 结构表示的随机颜色。
        /// </summary>
        /// <returns>ColorX 结构，表示随机颜色。</returns>
        public static ColorX GetRandomColorX()
        {
            return ColorX.RandomColor();
        }

        /// <summary>
        /// 返回一个 Color 结构表示的随机颜色。
        /// </summary>
        /// <returns>Color 结构，表示随机颜色。</returns>
        public static Color GetRandomColor()
        {
            return GetRandomColorX().ToColor();
        }

        //

        /// <summary>
        /// 返回 ColorX 结构表示的颜色的互补色。
        /// </summary>
        /// <param name="color">ColorX 结构表示的颜色。</param>
        /// <returns>ColorX 结构，表示指定颜色的互补色。</returns>
        public static ColorX GetComplementaryColor(ColorX color)
        {
            return color.ComplementaryColor;
        }

        /// <summary>
        /// 返回 Color 结构表示的颜色的互补色。
        /// </summary>
        /// <param name="color">Color 结构表示的颜色。</param>
        /// <returns>Color 结构，表示指定颜色的互补色。</returns>
        public static Color GetComplementaryColor(Color color)
        {
            return GetComplementaryColor((ColorX)color).ToColor();
        }

        /// <summary>
        /// 返回 ColorX 结构表示的颜色的灰度颜色。
        /// </summary>
        /// <param name="color">ColorX 结构表示的颜色。</param>
        /// <returns>ColorX 结构，表示指定颜色的灰度颜色。</returns>
        public static ColorX GetGrayscaleColor(ColorX color)
        {
            return color.GrayscaleColor;
        }

        /// <summary>
        /// 返回 Color 结构表示的颜色的灰度颜色。
        /// </summary>
        /// <param name="color">Color 结构表示的颜色。</param>
        /// <returns>Color 结构，表示指定颜色的灰度颜色。</returns>
        public static Color GetGrayscaleColor(Color color)
        {
            return GetGrayscaleColor((ColorX)color).ToColor();
        }

        //

        private static double _CheckProportion(double proportion) // 对双精度浮点数表示的比例的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(proportion))
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
        /// <returns>ColorX 结构，表示将两种颜色在 RGB 色彩空间按指定比例线性混合得到的颜色。</returns>
        public static ColorX BlendByRGB(ColorX color1, ColorX color2, double proportion)
        {
            proportion = _CheckProportion(proportion);

            return ColorX.FromRGB(color1.Alpha * proportion + color2.Alpha * (1 - proportion), color1.RGB * proportion + color2.RGB * (1 - proportion));
        }

        /// <summary>
        /// 返回将 Color 结构表示的两种颜色在 RGB 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">Color 结构表示的第一种颜色。</param>
        /// <param name="color2">Color 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        /// <returns>Color 结构，表示将两种颜色在 RGB 色彩空间按指定比例线性混合得到的颜色。</returns>
        public static Color BlendByRGB(Color color1, Color color2, double proportion)
        {
            return BlendByRGB((ColorX)color1, (ColorX)color2, proportion).ToColor();
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的两种颜色在 HSV 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">ColorX 结构表示的第一种颜色。</param>
        /// <param name="color2">ColorX 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        /// <returns>ColorX 结构，表示将两种颜色在 HSV 色彩空间按指定比例线性混合得到的颜色。</returns>
        public static ColorX BlendByHSV(ColorX color1, ColorX color2, double proportion)
        {
            proportion = _CheckProportion(proportion);

            return ColorX.FromHSV(color1.HSV * proportion + color2.HSV * (1 - proportion), color1.Opacity * proportion + color2.Opacity * (1 - proportion));
        }

        /// <summary>
        /// 返回将 Color 结构表示的两种颜色在 HSV 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">Color 结构表示的第一种颜色。</param>
        /// <param name="color2">Color 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        /// <returns>Color 结构，表示将两种颜色在 HSV 色彩空间按指定比例线性混合得到的颜色。</returns>
        public static Color BlendByHSV(Color color1, Color color2, double proportion)
        {
            return BlendByHSV((ColorX)color1, (ColorX)color2, proportion).ToColor();
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的两种颜色在 HSL 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">ColorX 结构表示的第一种颜色。</param>
        /// <param name="color2">ColorX 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        /// <returns>ColorX 结构，表示将两种颜色在 HSL 色彩空间按指定比例线性混合得到的颜色。</returns>
        public static ColorX BlendByHSL(ColorX color1, ColorX color2, double proportion)
        {
            proportion = _CheckProportion(proportion);

            return ColorX.FromHSL(color1.HSL * proportion + color2.HSL * (1 - proportion), color1.Opacity * proportion + color2.Opacity * (1 - proportion));
        }

        /// <summary>
        /// 返回将 Color 结构表示的两种颜色在 HSL 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">Color 结构表示的第一种颜色。</param>
        /// <param name="color2">Color 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        /// <returns>Color 结构，表示将两种颜色在 HSL 色彩空间按指定比例线性混合得到的颜色。</returns>
        public static Color BlendByHSL(Color color1, Color color2, double proportion)
        {
            return BlendByHSL((ColorX)color1, (ColorX)color2, proportion).ToColor();
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的两种颜色在 CMYK 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">ColorX 结构表示的第一种颜色。</param>
        /// <param name="color2">ColorX 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        /// <returns>ColorX 结构，表示将两种颜色在 CMYK 色彩空间按指定比例线性混合得到的颜色。</returns>
        public static ColorX BlendByCMYK(ColorX color1, ColorX color2, double proportion)
        {
            proportion = _CheckProportion(proportion);

            return ColorX.FromCMYK(color1.CMYK * proportion + color2.CMYK * (1 - proportion), color1.Opacity * proportion + color2.Opacity * (1 - proportion));
        }

        /// <summary>
        /// 返回将 Color 结构表示的两种颜色在 CMYK 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">Color 结构表示的第一种颜色。</param>
        /// <param name="color2">Color 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        /// <returns>Color 结构，表示将两种颜色在 CMYK 色彩空间按指定比例线性混合得到的颜色。</returns>
        public static Color BlendByCMYK(Color color1, Color color2, double proportion)
        {
            return BlendByCMYK((ColorX)color1, (ColorX)color2, proportion).ToColor();
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的两种颜色在 LAB 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">ColorX 结构表示的第一种颜色。</param>
        /// <param name="color2">ColorX 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        /// <returns>ColorX 结构，表示将两种颜色在 LAB 色彩空间按指定比例线性混合得到的颜色。</returns>
        public static ColorX BlendByLAB(ColorX color1, ColorX color2, double proportion)
        {
            proportion = _CheckProportion(proportion);

            return ColorX.FromLAB(color1.LAB * proportion + color2.LAB * (1 - proportion), color1.Opacity * proportion + color2.Opacity * (1 - proportion));
        }

        /// <summary>
        /// 返回将 Color 结构表示的两种颜色在 LAB 色彩空间按指定比例线性混合得到的颜色。
        /// </summary>
        /// <param name="color1">Color 结构表示的第一种颜色。</param>
        /// <param name="color2">Color 结构表示的第二种颜色。</param>
        /// <param name="proportion">第一种颜色所占的比例，取值范围为 [0, 1] 或 (1, 100]。</param>
        /// <returns>Color 结构，表示将两种颜色在 LAB 色彩空间按指定比例线性混合得到的颜色。</returns>
        public static Color BlendByLAB(Color color1, Color color2, double proportion)
        {
            return BlendByLAB((ColorX)color1, (ColorX)color2, proportion).ToColor();
        }

        //

        private static double _CheckLevel(double level) // 对双精度浮点数表示的调整程度的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(level))
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
        /// <returns>ColorX 结构，表示将指定颜色在 HSV 色彩空间调整明度得到的颜色。</returns>
        public static ColorX ShiftLightnessByHSV(ColorX color, double level)
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

        /// <summary>
        /// 返回将 Color 结构表示的颜色在 HSV 色彩空间调整明度得到的颜色。
        /// </summary>
        /// <param name="color">Color 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        /// <returns>Color 结构，表示将指定颜色在 HSV 色彩空间调整明度得到的颜色。</returns>
        public static Color ShiftLightnessByHSV(Color color, double level)
        {
            return ShiftLightnessByHSV((ColorX)color, level).ToColor();
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的颜色在 HSL 色彩空间调整明度得到的颜色。
        /// </summary>
        /// <param name="color">ColorX 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        /// <returns>ColorX 结构，表示将指定颜色在 HSL 色彩空间调整明度得到的颜色。</returns>
        public static ColorX ShiftLightnessByHSL(ColorX color, double level)
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

        /// <summary>
        /// 返回将 Color 结构表示的颜色在 HSL 色彩空间调整明度得到的颜色。
        /// </summary>
        /// <param name="color">Color 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        /// <returns>Color 结构，表示将指定颜色在 HSL 色彩空间调整明度得到的颜色。</returns>
        public static Color ShiftLightnessByHSL(Color color, double level)
        {
            return ShiftLightnessByHSL((ColorX)color, level).ToColor();
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的颜色在 LAB 色彩空间调整明度得到的颜色。
        /// </summary>
        /// <param name="color">ColorX 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        /// <returns>ColorX 结构，表示将指定颜色在 LAB 色彩空间调整明度得到的颜色。</returns>
        public static ColorX ShiftLightnessByLAB(ColorX color, double level)
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

        /// <summary>
        /// 返回将 Color 结构表示的颜色在 LAB 色彩空间调整明度得到的颜色。
        /// </summary>
        /// <param name="color">Color 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        /// <returns>Color 结构，表示将指定颜色在 LAB 色彩空间调整明度得到的颜色。</returns>
        public static Color ShiftLightnessByLAB(Color color, double level)
        {
            return ShiftLightnessByLAB((ColorX)color, level).ToColor();
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的颜色在 HSV 色彩空间调整饱和度得到的颜色。
        /// </summary>
        /// <param name="color">ColorX 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        /// <returns>ColorX 结构，表示将指定颜色在 HSV 色彩空间调整饱和度得到的颜色。</returns>
        public static ColorX ShiftSaturationByHSV(ColorX color, double level)
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

        /// <summary>
        /// 返回将 Color 结构表示的颜色在 HSV 色彩空间调整饱和度得到的颜色。
        /// </summary>
        /// <param name="color">Color 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        /// <returns>Color 结构，表示将指定颜色在 HSV 色彩空间调整饱和度得到的颜色。</returns>
        public static Color ShiftSaturationByHSV(Color color, double level)
        {
            return ShiftSaturationByHSV((ColorX)color, level).ToColor();
        }

        /// <summary>
        /// 返回将 ColorX 结构表示的颜色在 HSL 色彩空间调整饱和度得到的颜色。
        /// </summary>
        /// <param name="color">ColorX 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        /// <returns>ColorX 结构，表示将指定颜色在 HSL 色彩空间调整饱和度得到的颜色。</returns>
        public static ColorX ShiftSaturationByHSL(ColorX color, double level)
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

        /// <summary>
        /// 返回将 Color 结构表示的颜色在 HSL 色彩空间调整饱和度得到的颜色。
        /// </summary>
        /// <param name="color">Color 结构表示的颜色。</param>
        /// <param name="level">调整的程度，取值范围为 [-1, 1] 或 [-100, -1) ∪ (1, 100]。</param>
        /// <returns>Color 结构，表示将指定颜色在 HSL 色彩空间调整饱和度得到的颜色。</returns>
        public static Color ShiftSaturationByHSL(Color color, double level)
        {
            return ShiftSaturationByHSL((ColorX)color, level).ToColor();
        }
    }
}