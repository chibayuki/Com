/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.ColorX
Version 19.5.11.1720

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Com
{
    /// <summary>
    /// 以双精度浮点数在 RGB、HSV、HSL、CMYK、LAB 等色彩空间表示的颜色。
    /// </summary>
    public struct ColorX : IEquatable<ColorX>
    {
        #region 私有成员与内部成员

        private const int _32BitARGBAlphaShift = 24; // 32 位 ARGB 颜色的 Alpha 分量（A）的位偏移量。
        private const int _32BitARGBRedShift = 16; // 32 位 ARGB 颜色的红色分量（R）的位偏移量。
        private const int _32BitARGBGreenShift = 8; // 32 位 ARGB 颜色的绿色分量（G）的位偏移量。
        private const int _32BitARGBBlueShift = 0; // 32 位 ARGB 颜色的蓝色分量（B）的位偏移量。

        //

        private const double _MinOpacity = 0, _MaxOpacity = 100; // 不透明度的最小值与最大值。
        private const double _MinAlpha = 0, _MaxAlpha = 255; // Alpha 通道（A）的最小值与最大值。

        private const double _MinRed = 0, _MaxRed = 255; // RGB 色彩空间的红色通道（R）的最小值与最大值。
        private const double _MinGreen = 0, _MaxGreen = 255; // RGB 色彩空间的绿色通道（G）的最小值与最大值。
        private const double _MinBlue = 0, _MaxBlue = 255; // RGB 色彩空间的蓝色通道（B）的最小值与最大值。

        private const double _MinHue_HSV = 0, _MaxHue_HSV = 360; // HSV 色彩空间的色相（H）的最小值与最大值。
        private const double _MinSaturation_HSV = 0, _MaxSaturation_HSV = 100; // HSV 色彩空间的饱和度（S）的最小值与最大值。
        private const double _MinBrightness = 0, _MaxBrightness = 100; // HSV 色彩空间的亮度（V）的最小值与最大值。

        private const double _MinHue_HSL = 0, _MaxHue_HSL = 360; // HSL 色彩空间的色相（H）的最小值与最大值。
        private const double _MinSaturation_HSL = 0, _MaxSaturation_HSL = 100; // HSL 色彩空间的饱和度（S）的最小值与最大值。
        private const double _MinLightness_HSL = 0, _MaxLightness_HSL = 100; // HSL 色彩空间的明度（L）的最小值与最大值。

        private const double _MinCyan = 0, _MaxCyan = 100; // CMYK 色彩空间的青色通道（C）的最小值与最大值。
        private const double _MinMagenta = 0, _MaxMagenta = 100; // CMYK 色彩空间的洋红色通道（M）的最小值与最大值。
        private const double _MinYellow = 0, _MaxYellow = 100; // CMYK 色彩空间的黄色通道（Y）的最小值与最大值。
        private const double _MinBlack = 0, _MaxBlack = 100; // CMYK 色彩空间的黑色通道（K）的最小值与最大值。

        private const double _MinLightness_LAB = 0, _MaxLightness_LAB = 100; // LAB 色彩空间的明度（L）的最小值与最大值。
        private const double _MinGreenRed = -128, _MaxGreenRed = 128; // LAB 色彩空间的绿色-红色通道（A）的最小值与最大值。
        private const double _MinBlueYellow = -128, _MaxBlueYellow = 128; // LAB 色彩空间的蓝色-黄色通道（B）的最小值与最大值。

        //

        private const double _MinOpacity_FloDev = _MinOpacity - 5E-13, _MaxOpacity_FloDev = _MaxOpacity + 5E-11; // 不透明度的最小值与最大值，包含浮点偏差。
        private const double _MinAlpha_FloDev = _MinAlpha - 5E-13, _MaxAlpha_FloDev = _MaxAlpha + 5E-11; // Alpha 通道（A）的最小值与最大值，包含浮点偏差。

        private const double _MinRed_FloDev = _MinRed - 5E-13, _MaxRed_FloDev = _MaxRed + 5E-11; // RGB 色彩空间的红色通道（R）的最小值与最大值，包含浮点偏差。
        private const double _MinGreen_FloDev = _MinGreen - 5E-13, _MaxGreen_FloDev = _MaxGreen + 5E-11; // RGB 色彩空间的绿色通道（G）的最小值与最大值，包含浮点偏差。
        private const double _MinBlue_FloDev = _MinBlue - 5E-13, _MaxBlue_FloDev = _MaxBlue + 5E-11; // RGB 色彩空间的蓝色通道（B）的最小值与最大值，包含浮点偏差。

        private const double _MinHue_HSV_FloDev = _MinHue_HSV - 5E-13, _MaxHue_HSV_FloDev = _MaxHue_HSV + 5E-11; // HSV 色彩空间的色相（H）的最小值与最大值，包含浮点偏差。
        private const double _MinSaturation_HSV_FloDev = _MinSaturation_HSV - 5E-13, _MaxSaturation_HSV_FloDev = _MaxSaturation_HSV + 5E-11; // HSV 色彩空间的饱和度（S）的最小值与最大值，包含浮点偏差。
        private const double _MinBrightness_FloDev = _MinBrightness - 5E-13, _MaxBrightness_FloDev = _MaxBrightness + 5E-11; // HSV 色彩空间的亮度（V）的最小值与最大值，包含浮点偏差。

        private const double _MinHue_HSL_FloDev = _MinHue_HSL - 5E-13, _MaxHue_HSL_FloDev = _MaxHue_HSL + 5E-11; // HSL 色彩空间的色相（H）的最小值与最大值，包含浮点偏差。
        private const double _MinSaturation_HSL_FloDev = _MinSaturation_HSL - 5E-13, _MaxSaturation_HSL_FloDev = _MaxSaturation_HSL + 5E-11; // HSL 色彩空间的饱和度（S）的最小值与最大值，包含浮点偏差。
        private const double _MinLightness_HSL_FloDev = _MinLightness_HSL - 5E-13, _MaxLightness_HSL_FloDev = _MaxLightness_HSL + 5E-11; // HSL 色彩空间的明度（L）的最小值与最大值，包含浮点偏差。

        private const double _MinCyan_FloDev = _MinCyan - 5E-13, _MaxCyan_FloDev = _MaxCyan + 5E-11; // CMYK 色彩空间的青色通道（C）的最小值与最大值，包含浮点偏差。
        private const double _MinMagenta_FloDev = _MinMagenta - 5E-13, _MaxMagenta_FloDev = _MaxMagenta + 5E-11; // CMYK 色彩空间的洋红色通道（M）的最小值与最大值，包含浮点偏差。
        private const double _MinYellow_FloDev = _MinYellow - 5E-13, _MaxYellow_FloDev = _MaxYellow + 5E-11; // CMYK 色彩空间的黄色通道（Y）的最小值与最大值，包含浮点偏差。
        private const double _MinBlack_FloDev = _MinBlack - 5E-13, _MaxBlack_FloDev = _MaxBlack + 5E-11; // CMYK 色彩空间的黑色通道（K）的最小值与最大值，包含浮点偏差。

        private const double _MinLightness_LAB_FloDev = _MinLightness_LAB - 5E-13, _MaxLightness_LAB_FloDev = _MaxLightness_LAB + 5E-11; // LAB 色彩空间的明度（L）的最小值与最大值，包含浮点偏差。
        private const double _MinGreenRed_FloDev = _MinGreenRed - 5E-11, _MaxGreenRed_FloDev = _MaxGreenRed + 5E-11; // LAB 色彩空间的绿色-红色通道（A）的最小值与最大值，包含浮点偏差。
        private const double _MinBlueYellow_FloDev = _MinBlueYellow - 5E-11, _MaxBlueYellow_FloDev = _MaxBlueYellow + 5E-11; // LAB 色彩空间的蓝色-黄色通道（B）的最小值与最大值，包含浮点偏差。

        //

        private static double _CheckOpacity(double opacity) // 对颜色的不透明度的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(opacity))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (opacity < _MinOpacity)
            {
                if (opacity <= _MinOpacity_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinOpacity;
            }
            else if (opacity > _MaxOpacity)
            {
                if (opacity >= _MaxOpacity_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxOpacity;
            }
            else
            {
                return opacity;
            }
        }

        private static double _CheckAlpha(double alpha) // 对颜色的 Alpha 通道（A）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(alpha))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (alpha < _MinAlpha)
            {
                if (alpha <= _MinAlpha_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinAlpha;
            }
            else if (alpha > _MaxAlpha)
            {
                if (alpha >= _MaxAlpha_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxAlpha;
            }
            else
            {
                return alpha;
            }
        }

        private static double _CheckRed(double red) // 对颜色在 RGB 色彩空间的红色通道（R）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(red))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (red < _MinRed)
            {
                if (red <= _MinRed_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinRed;
            }
            else if (red > _MaxRed)
            {
                if (red >= _MaxRed_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxRed;
            }
            else
            {
                return red;
            }
        }

        private static double _CheckGreen(double green) // 对颜色在 RGB 色彩空间的绿色通道（G）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(green))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (green < _MinGreen)
            {
                if (green <= _MinGreen_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinGreen;
            }
            else if (green > _MaxGreen)
            {
                if (green >= _MaxGreen_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxGreen;
            }
            else
            {
                return green;
            }
        }

        private static double _CheckBlue(double blue) // 对颜色在 RGB 色彩空间的蓝色通道（B）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(blue))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (blue < _MinBlue)
            {
                if (blue <= _MinBlue_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinBlue;
            }
            else if (blue > _MaxBlue)
            {
                if (blue >= _MaxBlue_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxBlue;
            }
            else
            {
                return blue;
            }
        }

        private static double _CheckHue_HSV(double hue) // 对颜色在 HSV 色彩空间的色相（H）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(hue))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (hue < _MinHue_HSV)
            {
                if (hue <= _MinHue_HSV_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinHue_HSV;
            }
            else if (hue > _MaxHue_HSV)
            {
                if (hue >= _MaxHue_HSV_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxHue_HSV;
            }
            else
            {
                return hue;
            }
        }

        private static double _CheckSaturation_HSV(double saturation) // 对颜色在 HSV 色彩空间的饱和度（S）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(saturation))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (saturation < _MinSaturation_HSV)
            {
                if (saturation <= _MinSaturation_HSV_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinSaturation_HSV;
            }
            else if (saturation > _MaxSaturation_HSV)
            {
                if (saturation >= _MaxSaturation_HSV_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxSaturation_HSV;
            }
            else
            {
                return saturation;
            }
        }

        private static double _CheckBrightness(double brightness) // 对颜色在 HSV 色彩空间的亮度（V）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(brightness))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (brightness < _MinBrightness)
            {
                if (brightness <= _MinBrightness_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinBrightness;
            }
            else if (brightness > _MaxBrightness)
            {
                if (brightness >= _MaxBrightness_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxBrightness;
            }
            else
            {
                return brightness;
            }
        }

        private static double _CheckHue_HSL(double hue) // 对颜色在 HSL 色彩空间的色相（H）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(hue))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (hue < _MinHue_HSL)
            {
                if (hue <= _MinHue_HSL_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinHue_HSL;
            }
            else if (hue > _MaxHue_HSL)
            {
                if (hue >= _MaxHue_HSL_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxHue_HSL;
            }
            else
            {
                return hue;
            }
        }

        private static double _CheckSaturation_HSL(double saturation) // 对颜色在 HSL 色彩空间的饱和度（S）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(saturation))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (saturation < _MinSaturation_HSL)
            {
                if (saturation <= _MinSaturation_HSL_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinSaturation_HSL;
            }
            else if (saturation > _MaxSaturation_HSL)
            {
                if (saturation >= _MaxSaturation_HSL_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxSaturation_HSL;
            }
            else
            {
                return saturation;
            }
        }

        private static double _CheckLightness_HSL(double lightness) // 对颜色在 HSL 色彩空间的明度（L）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(lightness))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (lightness < _MinLightness_HSL)
            {
                if (lightness <= _MinLightness_HSL_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinLightness_HSL;
            }
            else if (lightness > _MaxLightness_HSL)
            {
                if (lightness >= _MaxLightness_HSL_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxLightness_HSL;
            }
            else
            {
                return lightness;
            }
        }

        private static double _CheckCyan(double cyan) // 对颜色在 CMYK 色彩空间的青色通道（C）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(cyan))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (cyan < _MinCyan)
            {
                if (cyan <= _MinCyan_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinCyan;
            }
            else if (cyan > _MaxCyan)
            {
                if (cyan >= _MaxCyan_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxCyan;
            }
            else
            {
                return cyan;
            }
        }

        private static double _CheckMagenta(double magenta) // 对颜色在 CMYK 色彩空间的洋红色通道（M）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(magenta))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (magenta < _MinMagenta)
            {
                if (magenta <= _MinMagenta_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinMagenta;
            }
            else if (magenta > _MaxMagenta)
            {
                if (magenta >= _MaxMagenta_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxMagenta;
            }
            else
            {
                return magenta;
            }
        }

        private static double _CheckYellow(double yellow) // 对颜色在 CMYK 色彩空间的黄色通道（Y）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(yellow))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (yellow < _MinYellow)
            {
                if (yellow <= _MinYellow_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinYellow;
            }
            else if (yellow > _MaxYellow)
            {
                if (yellow >= _MaxYellow_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxYellow;
            }
            else
            {
                return yellow;
            }
        }

        private static double _CheckBlack(double black) // 对颜色在 CMYK 色彩空间的黑色通道（K）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(black))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (black < _MinBlack)
            {
                if (black <= _MinBlack_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinBlack;
            }
            else if (black > _MaxBlack)
            {
                if (black >= _MaxBlack_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxBlack;
            }
            else
            {
                return black;
            }
        }

        private static double _CheckLightness_LAB(double lightness) // 对颜色在 LAB 色彩空间的明度（L）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(lightness))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (lightness < _MinLightness_LAB)
            {
                if (lightness <= _MinLightness_LAB_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinLightness_LAB;
            }
            else if (lightness > _MaxLightness_LAB)
            {
                if (lightness >= _MaxLightness_LAB_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxLightness_LAB;
            }
            else
            {
                return lightness;
            }
        }

        private static double _CheckGreenRed(double greenRed) // 对颜色在 LAB 色彩空间的绿色-红色通道（A）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(greenRed))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (greenRed < _MinGreenRed)
            {
                if (greenRed <= _MinGreenRed_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinGreenRed;
            }
            else if (greenRed > _MaxGreenRed)
            {
                if (greenRed >= _MaxGreenRed_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxGreenRed;
            }
            else
            {
                return greenRed;
            }
        }

        private static double _CheckBlueYellow(double blueYellow) // 对颜色在 LAB 色彩空间的蓝色-黄色通道（B）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(blueYellow))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (blueYellow < _MinBlueYellow)
            {
                if (blueYellow <= _MinBlueYellow_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinBlueYellow;
            }
            else if (blueYellow > _MaxBlueYellow)
            {
                if (blueYellow >= _MaxBlueYellow_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxBlueYellow;
            }
            else
            {
                return blueYellow;
            }
        }

        //

        private static void _OpacityToAlpha(double opacity, out double alpha) // 将颜色的不透明度转换为 Alpha 通道（A）的值。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            alpha = opacity / _MaxOpacity * _MaxAlpha;

            if (alpha < _MinAlpha)
            {
                alpha = _MinAlpha;
            }
            else if (alpha > _MaxAlpha)
            {
                alpha = _MaxAlpha;
            }
        }

        private static void _AlphaToOpacity(double alpha, out double opacity) // 将颜色的 Alpha 通道（A）的值转换为不透明度。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            opacity = alpha / _MaxAlpha * _MaxOpacity;

            if (opacity < _MinOpacity)
            {
                opacity = _MinOpacity;
            }
            else if (opacity > _MaxOpacity)
            {
                opacity = _MaxOpacity;
            }
        }

        private static void _RGBToHSV(double red, double green, double blue, out double hue, out double saturation, out double brightness) // 将颜色在 RGB 色彩空间的各分量转换为在 HSV 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            red /= _MaxRed;
            green /= _MaxGreen;
            blue /= _MaxBlue;

            double Max = red, Min = red;

            if (Max < green)
            {
                Max = green;
            }

            if (Max < blue)
            {
                Max = blue;
            }

            if (Min > green)
            {
                Min = green;
            }

            if (Min > blue)
            {
                Min = blue;
            }

            if (Max == Min)
            {
                hue = 0;
            }
            else
            {
                double dH = 1.0 / 6 / (Max - Min);

                if (Max == red)
                {
                    hue = dH * (green - blue);

                    if (hue < 0)
                    {
                        hue += 1;
                    }
                }
                else if (Max == green)
                {
                    hue = dH * (blue - red) + 1.0 / 3;
                }
                else
                {
                    hue = dH * (red - green) + 2.0 / 3;
                }
            }

            if (Max == 0)
            {
                saturation = 0;
            }
            else
            {
                saturation = 1 - Min / Max;
            }

            brightness = Max;

            hue *= _MaxHue_HSV;
            saturation *= _MaxSaturation_HSV;
            brightness *= _MaxBrightness;

            if (hue < _MinHue_HSV)
            {
                hue = _MinHue_HSV;
            }
            else if (hue > _MaxHue_HSV)
            {
                hue = _MaxHue_HSV;
            }

            if (saturation < _MinSaturation_HSV)
            {
                saturation = _MinSaturation_HSV;
            }
            else if (saturation > _MaxSaturation_HSV)
            {
                saturation = _MaxSaturation_HSV;
            }

            if (brightness < _MinBrightness)
            {
                brightness = _MinBrightness;
            }
            else if (brightness > _MaxBrightness)
            {
                brightness = _MaxBrightness;
            }
        }

        private static void _HSVToRGB(double hue, double saturation, double brightness, out double red, out double green, out double blue) // 将颜色在 HSV 色彩空间的各分量转换为在 RGB 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            hue /= _MaxHue_HSV;
            saturation /= _MaxSaturation_HSV;
            brightness /= _MaxBrightness;

            int Hi = (int)Math.Floor(hue * 6);

            if (Hi == 6)
            {
                hue = 0;
            }

            Hi %= 6;

            double Hf = hue * 6 - Hi;

            double C = brightness * saturation;

            double X = C * (1 - Math.Abs(Hi % 2 + Hf - 1));

            switch (Hi)
            {
                case 0:
                    {
                        red = C;
                        green = X;
                        blue = 0;
                    }
                    break;

                case 1:
                    {
                        red = X;
                        green = C;
                        blue = 0;
                    }
                    break;

                case 2:
                    {
                        red = 0;
                        green = C;
                        blue = X;
                    }
                    break;

                case 3:
                    {
                        red = 0;
                        green = X;
                        blue = C;
                    }
                    break;

                case 4:
                    {
                        red = X;
                        green = 0;
                        blue = C;
                    }
                    break;

                case 5:
                    {
                        red = C;
                        green = 0;
                        blue = X;
                    }
                    break;

                default:
                    {
                        red = 0;
                        green = 0;
                        blue = 0;
                    }
                    break;
            }

            double M = brightness - C;

            red += M;
            green += M;
            blue += M;

            red *= _MaxRed;
            green *= _MaxGreen;
            blue *= _MaxBlue;

            if (red < _MinRed)
            {
                red = _MinRed;
            }
            else if (red > _MaxRed)
            {
                red = _MaxRed;
            }

            if (green < _MinGreen)
            {
                green = _MinGreen;
            }
            else if (green > _MaxGreen)
            {
                green = _MaxGreen;
            }

            if (blue < _MinBlue)
            {
                blue = _MinBlue;
            }
            else if (blue > _MaxBlue)
            {
                blue = _MaxBlue;
            }
        }

        private static void _RGBToHSL(double red, double green, double blue, out double hue, out double saturation, out double lightness) // 将颜色在 RGB 色彩空间的各分量转换为在 HSL 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            red /= _MaxRed;
            green /= _MaxGreen;
            blue /= _MaxBlue;

            double Max = red, Min = red;

            if (Max < green)
            {
                Max = green;
            }

            if (Max < blue)
            {
                Max = blue;
            }

            if (Min > green)
            {
                Min = green;
            }

            if (Min > blue)
            {
                Min = blue;
            }

            if (Max == Min)
            {
                hue = 0;
            }
            else
            {
                double dH = 1.0 / 6 / (Max - Min);

                if (Max == red)
                {
                    hue = dH * (green - blue);

                    if (hue < 0)
                    {
                        hue += 1;
                    }
                }
                else if (Max == green)
                {
                    hue = dH * (blue - red) + 1.0 / 3;
                }
                else
                {
                    hue = dH * (red - green) + 2.0 / 3;
                }
            }

            lightness = (Max + Min) / 2;

            if (lightness == 0 || Max == Min)
            {
                saturation = 0;
            }
            else
            {
                saturation = (Max - Min) / (1 - Math.Abs(2 * lightness - 1));
            }

            hue *= _MaxHue_HSL;
            saturation *= _MaxSaturation_HSL;
            lightness *= _MaxLightness_HSL;

            if (hue < _MinHue_HSL)
            {
                hue = _MinHue_HSL;
            }
            else if (hue > _MaxHue_HSL)
            {
                hue = _MaxHue_HSL;
            }

            if (saturation < _MinSaturation_HSL)
            {
                saturation = _MinSaturation_HSL;
            }
            else if (saturation > _MaxSaturation_HSL)
            {
                saturation = _MaxSaturation_HSL;
            }

            if (lightness < _MinLightness_HSL)
            {
                lightness = _MinLightness_HSL;
            }
            else if (lightness > _MaxLightness_HSL)
            {
                lightness = _MaxLightness_HSL;
            }
        }

        private static void _HSLToRGB(double hue, double saturation, double lightness, out double red, out double green, out double blue) // 将颜色在 HSL 色彩空间的各分量转换为在 RGB 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            hue /= _MaxHue_HSL;
            saturation /= _MaxSaturation_HSL;
            lightness /= _MaxLightness_HSL;

            int Hi = (int)Math.Floor(hue * 6);

            if (Hi == 6)
            {
                hue = 0;
            }

            Hi %= 6;

            double Hf = hue * 6 - Hi;

            double C = (1 - Math.Abs(2 * lightness - 1)) * saturation;

            double X = C * (1 - Math.Abs(Hi % 2 + Hf - 1));

            switch (Hi)
            {
                case 0:
                    {
                        red = C;
                        green = X;
                        blue = 0;
                    }
                    break;

                case 1:
                    {
                        red = X;
                        green = C;
                        blue = 0;
                    }
                    break;

                case 2:
                    {
                        red = 0;
                        green = C;
                        blue = X;
                    }
                    break;

                case 3:
                    {
                        red = 0;
                        green = X;
                        blue = C;
                    }
                    break;

                case 4:
                    {
                        red = X;
                        green = 0;
                        blue = C;
                    }
                    break;

                case 5:
                    {
                        red = C;
                        green = 0;
                        blue = X;
                    }
                    break;

                default:
                    {
                        red = 0;
                        green = 0;
                        blue = 0;
                    }
                    break;
            }

            double M = lightness - C / 2;

            red += M;
            green += M;
            blue += M;

            red *= _MaxRed;
            green *= _MaxGreen;
            blue *= _MaxBlue;

            if (red < _MinRed)
            {
                red = _MinRed;
            }
            else if (red > _MaxRed)
            {
                red = _MaxRed;
            }

            if (green < _MinGreen)
            {
                green = _MinGreen;
            }
            else if (green > _MaxGreen)
            {
                green = _MaxGreen;
            }

            if (blue < _MinBlue)
            {
                blue = _MinBlue;
            }
            else if (blue > _MaxBlue)
            {
                blue = _MaxBlue;
            }
        }

        private static void _RGBToCMYK(double red, double green, double blue, out double cyan, out double magenta, out double yellow, out double black) // 将颜色在 RGB 色彩空间的各分量转换为在 CMYK 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            red /= _MaxRed;
            green /= _MaxGreen;
            blue /= _MaxBlue;

            double RgbMax = red;

            if (RgbMax < green)
            {
                RgbMax = green;
            }

            if (RgbMax < blue)
            {
                RgbMax = blue;
            }

            if (RgbMax == 0)
            {
                cyan = 0;
                magenta = 0;
                yellow = 0;
                black = 1;
            }
            else
            {
                cyan = (RgbMax - red) / RgbMax;
                magenta = (RgbMax - green) / RgbMax;
                yellow = (RgbMax - blue) / RgbMax;
                black = 1 - RgbMax;
            }

            cyan *= _MaxCyan;
            magenta *= _MaxMagenta;
            yellow *= _MaxYellow;
            black *= _MaxBlack;

            if (cyan < _MinCyan)
            {
                cyan = _MinCyan;
            }
            else if (cyan > _MaxCyan)
            {
                cyan = _MaxCyan;
            }

            if (magenta < _MinMagenta)
            {
                magenta = _MinMagenta;
            }
            else if (magenta > _MaxMagenta)
            {
                magenta = _MaxMagenta;
            }

            if (yellow < _MinYellow)
            {
                yellow = _MinYellow;
            }
            else if (yellow > _MaxYellow)
            {
                yellow = _MaxYellow;
            }

            if (black < _MinBlack)
            {
                black = _MinBlack;
            }
            else if (black > _MaxBlack)
            {
                black = _MaxBlack;
            }
        }

        private static void _CMYKToRGB(double cyan, double magenta, double yellow, double black, out double red, out double green, out double blue) // 将颜色在 CMYK 色彩空间的各分量转换为在 RGB 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            cyan /= _MaxCyan;
            magenta /= _MaxMagenta;
            yellow /= _MaxYellow;
            black /= _MaxBlack;

            if (black == 1)
            {
                red = 0;
                green = 0;
                blue = 0;
            }
            else
            {
                red = (1 - cyan) * (1 - black);
                green = (1 - magenta) * (1 - black);
                blue = (1 - yellow) * (1 - black);
            }

            red *= _MaxRed;
            green *= _MaxGreen;
            blue *= _MaxBlue;

            if (red < _MinRed)
            {
                red = _MinRed;
            }
            else if (red > _MaxRed)
            {
                red = _MaxRed;
            }

            if (green < _MinGreen)
            {
                green = _MinGreen;
            }
            else if (green > _MaxGreen)
            {
                green = _MaxGreen;
            }

            if (blue < _MinBlue)
            {
                blue = _MinBlue;
            }
            else if (blue > _MaxBlue)
            {
                blue = _MaxBlue;
            }
        }

        private static void _RGBToLAB(double red, double green, double blue, out double lightness, out double greenRed, out double blueYellow) // 将颜色在 RGB 色彩空间的各分量转换为在 LAB 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            red /= _MaxRed;
            green /= _MaxGreen;
            blue /= _MaxBlue;

            Func<double, double> Frgb = (t) =>
            {
                if (t > 0.04045)
                {
                    return Math.Pow((t + 0.055) / 1.055, 2.4);
                }
                else
                {
                    return (t / 12.92);
                }
            };

            double R = Frgb(red);
            double G = Frgb(green);
            double B = Frgb(blue);

            double X = 0.433953 * R + 0.376219 * G + 0.189828 * B;
            double Y = 0.212671 * R + 0.715160 * G + 0.072169 * B;
            double Z = 0.017758 * R + 0.109477 * G + 0.872765 * B;

            Func<double, double> Fxyz = (t) =>
            {
                const double Delta = 6.0 / 29;

                if (t > Delta * Delta * Delta)
                {
                    return Math.Pow(t, 1.0 / 3);
                }
                else
                {
                    return (t / (3 * Delta * Delta) + 4.0 / 29);
                }
            };

            double Fx = Fxyz(X);
            double Fy = Fxyz(Y);
            double Fz = Fxyz(Z);

            lightness = 116 * Fy - 16;
            greenRed = 500 * (Fx - Fy);
            blueYellow = 200 * (Fy - Fz);

            if (lightness < _MinLightness_LAB)
            {
                lightness = _MinLightness_LAB;
            }
            else if (lightness > _MaxLightness_LAB)
            {
                lightness = _MaxLightness_LAB;
            }

            if (greenRed < _MinGreenRed)
            {
                greenRed = _MinGreenRed;
            }
            else if (greenRed > _MaxGreenRed)
            {
                greenRed = _MaxGreenRed;
            }

            if (blueYellow < _MinBlueYellow)
            {
                blueYellow = _MinBlueYellow;
            }
            else if (blueYellow > _MaxBlueYellow)
            {
                blueYellow = _MaxBlueYellow;
            }
        }

        private static void _LABToRGB(double lightness, double greenRed, double blueYellow, out double red, out double green, out double blue) // 将颜色在 LAB 色彩空间的各分量转换为在 RGB 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            double L = (lightness + 16) / 116;
            double a = greenRed / 500;
            double b = blueYellow / 200;

            Func<double, double> Fxyz = (t) =>
            {
                const double Delta = 6.0 / 29;

                if (t > Delta)
                {
                    return (t * t * t);
                }
                else
                {
                    return ((t - 4.0 / 29) * (3 * Delta * Delta));
                }
            };

            double X = Fxyz(L + a);
            double Y = Fxyz(L);
            double Z = Fxyz(L - b);

            double R = 3.079932708424 * X - 1.537150 * Y - 0.54278197539 * Z;
            double G = -0.921235180736 * X + 1.875991 * Y + 0.045244261224 * Z;
            double B = 0.052890975488 * X - 0.204043 * Y + 1.151151580494 * Z;

            Func<double, double> Frgb = (t) =>
            {
                if (t > 0.04045 / 12.92)
                {
                    return (Math.Pow(t, 1 / 2.4) * 1.055 - 0.055);
                }
                else
                {
                    return (t * 12.92);
                }
            };

            red = Frgb(R);
            green = Frgb(G);
            blue = Frgb(B);

            red *= _MaxRed;
            green *= _MaxGreen;
            blue *= _MaxBlue;

            if (red < _MinRed)
            {
                red = _MinRed;
            }
            else if (red > _MaxRed)
            {
                red = _MaxRed;
            }

            if (green < _MinGreen)
            {
                green = _MinGreen;
            }
            else if (green > _MaxGreen)
            {
                green = _MaxGreen;
            }

            if (blue < _MinBlue)
            {
                blue = _MinBlue;
            }
            else if (blue > _MaxBlue)
            {
                blue = _MaxBlue;
            }
        }

        //

        private bool _Initialized; // 表示此 ColorX 结构是否已初始化。

        private double _Opacity; // 颜色的不透明度。
        private double _Alpha; // 颜色的 Alpha 通道（A）的值。

        private double _Red; // 颜色在 RGB 色彩空间的红色通道（R）的值。
        private double _Green; // 颜色在 RGB 色彩空间的绿色通道（G）的值。
        private double _Blue; // 颜色在 RGB 色彩空间的蓝色通道（B）的值。

        private double _Hue_HSV; // 颜色在 HSV 色彩空间的色相（H）。
        private double _Saturation_HSV; // 颜色在 HSV 色彩空间的饱和度（S）。
        private double _Brightness; // 颜色在 HSV 色彩空间的亮度（V）。

        private double _Hue_HSL; // 颜色在 HSL 色彩空间的色相（H）。
        private double _Saturation_HSL; // 颜色在 HSL 色彩空间的饱和度（S）。
        private double _Lightness_HSL; // 颜色在 HSL 色彩空间的明度（L）。

        private double _Cyan; // 颜色在 CMYK 色彩空间的青色通道（C）的值。
        private double _Magenta; // 颜色在 CMYK 色彩空间的洋红色通道（M）的值。
        private double _Yellow; // 颜色在 CMYK 色彩空间的黄色通道（Y）的值。
        private double _Black; // 颜色在 CMYK 色彩空间的黑色通道（K）的值。

        private double _Lightness_LAB; // 颜色在 LAB 色彩空间的明度（L）。
        private double _GreenRed; // 颜色在 LAB 色彩空间的绿色-红色通道（A）的值。   
        private double _BlueYellow; // 颜色在 LAB 色彩空间的蓝色-黄色通道（B）的值。

        //

        private void _CtorRGB(double alpha, double red, double green, double blue) // 为以颜色在 RGB 色彩空间的各分量为参数的构造函数提供实现。
        {
            _Alpha = _CheckAlpha(alpha);
            _Red = _CheckRed(red);
            _Green = _CheckGreen(green);
            _Blue = _CheckBlue(blue);

            _AlphaToOpacity(_Alpha, out _Opacity);
            _RGBToHSV(_Red, _Green, _Blue, out _Hue_HSV, out _Saturation_HSV, out _Brightness);
            _RGBToHSL(_Red, _Green, _Blue, out _Hue_HSL, out _Saturation_HSL, out _Lightness_HSL);
            _RGBToCMYK(_Red, _Green, _Blue, out _Cyan, out _Magenta, out _Yellow, out _Black);
            _RGBToLAB(_Red, _Green, _Blue, out _Lightness_LAB, out _GreenRed, out _BlueYellow);

            //

            _Initialized = true;
        }

        private void _CtorRGB(double red, double green, double blue) // 为以颜色在 RGB 色彩空间的各分量为参数的构造函数提供实现。
        {
            _Red = _CheckRed(red);
            _Green = _CheckGreen(green);
            _Blue = _CheckBlue(blue);

            _RGBToHSV(_Red, _Green, _Blue, out _Hue_HSV, out _Saturation_HSV, out _Brightness);
            _RGBToHSL(_Red, _Green, _Blue, out _Hue_HSL, out _Saturation_HSL, out _Lightness_HSL);
            _RGBToCMYK(_Red, _Green, _Blue, out _Cyan, out _Magenta, out _Yellow, out _Black);
            _RGBToLAB(_Red, _Green, _Blue, out _Lightness_LAB, out _GreenRed, out _BlueYellow);

            //

            _Initialized = true;
        }

        private void _CtorHSV(double hue, double saturation, double brightness, double opacity) // 为以颜色在 HSV 色彩空间的各分量为参数的构造函数提供实现。
        {
            _Opacity = _CheckOpacity(opacity);
            _Hue_HSV = _CheckHue_HSV(hue);
            _Saturation_HSV = _CheckSaturation_HSV(saturation);
            _Brightness = _CheckBrightness(brightness);

            _OpacityToAlpha(_Opacity, out _Alpha);
            _HSVToRGB(_Hue_HSV, _Saturation_HSV, _Brightness, out _Red, out _Green, out _Blue);
            _RGBToHSL(_Red, _Green, _Blue, out _Hue_HSL, out _Saturation_HSL, out _Lightness_HSL);
            _RGBToCMYK(_Red, _Green, _Blue, out _Cyan, out _Magenta, out _Yellow, out _Black);
            _RGBToLAB(_Red, _Green, _Blue, out _Lightness_LAB, out _GreenRed, out _BlueYellow);

            //

            _Initialized = true;
        }

        private void _CtorHSV(double hue, double saturation, double brightness) // 为以颜色在 HSV 色彩空间的各分量为参数的构造函数提供实现。
        {
            _Hue_HSV = _CheckHue_HSV(hue);
            _Saturation_HSV = _CheckSaturation_HSV(saturation);
            _Brightness = _CheckBrightness(brightness);

            _HSVToRGB(_Hue_HSV, _Saturation_HSV, _Brightness, out _Red, out _Green, out _Blue);
            _RGBToHSL(_Red, _Green, _Blue, out _Hue_HSL, out _Saturation_HSL, out _Lightness_HSL);
            _RGBToCMYK(_Red, _Green, _Blue, out _Cyan, out _Magenta, out _Yellow, out _Black);
            _RGBToLAB(_Red, _Green, _Blue, out _Lightness_LAB, out _GreenRed, out _BlueYellow);

            //

            _Initialized = true;
        }

        private void _CtorHSL(double hue, double saturation, double lightness, double opacity) // 为以颜色在 HSL 色彩空间的各分量为参数的构造函数提供实现。
        {
            _Opacity = _CheckOpacity(opacity);
            _Hue_HSL = _CheckHue_HSL(hue);
            _Saturation_HSL = _CheckSaturation_HSL(saturation);
            _Lightness_HSL = _CheckLightness_HSL(lightness);

            _OpacityToAlpha(_Opacity, out _Alpha);
            _HSLToRGB(_Hue_HSL, _Saturation_HSL, _Lightness_HSL, out _Red, out _Green, out _Blue);
            _RGBToHSV(_Red, _Green, _Blue, out _Hue_HSV, out _Saturation_HSV, out _Brightness);
            _RGBToCMYK(_Red, _Green, _Blue, out _Cyan, out _Magenta, out _Yellow, out _Black);
            _RGBToLAB(_Red, _Green, _Blue, out _Lightness_LAB, out _GreenRed, out _BlueYellow);

            //

            _Initialized = true;
        }

        private void _CtorHSL(double hue, double saturation, double lightness) // 为以颜色在 HSL 色彩空间的各分量为参数的构造函数提供实现。
        {
            _Hue_HSL = _CheckHue_HSL(hue);
            _Saturation_HSL = _CheckSaturation_HSL(saturation);
            _Lightness_HSL = _CheckLightness_HSL(lightness);

            _HSLToRGB(_Hue_HSL, _Saturation_HSL, _Lightness_HSL, out _Red, out _Green, out _Blue);
            _RGBToHSV(_Red, _Green, _Blue, out _Hue_HSV, out _Saturation_HSV, out _Brightness);
            _RGBToCMYK(_Red, _Green, _Blue, out _Cyan, out _Magenta, out _Yellow, out _Black);
            _RGBToLAB(_Red, _Green, _Blue, out _Lightness_LAB, out _GreenRed, out _BlueYellow);

            //

            _Initialized = true;
        }

        private void _CtorCMYK(double cyan, double magenta, double yellow, double black, double opacity) // 为以颜色在 CMYK 色彩空间的各分量为参数的构造函数提供实现。
        {
            _Opacity = _CheckOpacity(opacity);
            _Cyan = _CheckCyan(cyan);
            _Magenta = _CheckMagenta(magenta);
            _Yellow = _CheckYellow(yellow);
            _Black = _CheckBlack(black);

            _OpacityToAlpha(_Opacity, out _Alpha);
            _CMYKToRGB(_Cyan, _Magenta, _Yellow, _Black, out _Red, out _Green, out _Blue);
            _RGBToHSV(_Red, _Green, _Blue, out _Hue_HSV, out _Saturation_HSV, out _Brightness);
            _RGBToHSL(_Red, _Green, _Blue, out _Hue_HSL, out _Saturation_HSL, out _Lightness_HSL);
            _RGBToLAB(_Red, _Green, _Blue, out _Lightness_LAB, out _GreenRed, out _BlueYellow);

            //

            _Initialized = true;
        }

        private void _CtorCMYK(double cyan, double magenta, double yellow, double black) // 为以颜色在 CMYK 色彩空间的各分量为参数的构造函数提供实现。
        {
            _Cyan = _CheckCyan(cyan);
            _Magenta = _CheckMagenta(magenta);
            _Yellow = _CheckYellow(yellow);
            _Black = _CheckBlack(black);

            _CMYKToRGB(_Cyan, _Magenta, _Yellow, _Black, out _Red, out _Green, out _Blue);
            _RGBToHSV(_Red, _Green, _Blue, out _Hue_HSV, out _Saturation_HSV, out _Brightness);
            _RGBToHSL(_Red, _Green, _Blue, out _Hue_HSL, out _Saturation_HSL, out _Lightness_HSL);
            _RGBToLAB(_Red, _Green, _Blue, out _Lightness_LAB, out _GreenRed, out _BlueYellow);

            //

            _Initialized = true;
        }

        private void _CtorLAB(double lightness, double greenRed, double blueYellow, double opacity) // 为以颜色在 LAB 色彩空间的各分量为参数的构造函数提供实现。
        {
            _Opacity = _CheckOpacity(opacity);
            _Lightness_LAB = _CheckLightness_LAB(lightness);
            _GreenRed = _CheckGreenRed(greenRed);
            _BlueYellow = _CheckBlueYellow(blueYellow);

            _OpacityToAlpha(_Opacity, out _Alpha);
            _LABToRGB(_Lightness_LAB, _GreenRed, _BlueYellow, out _Red, out _Green, out _Blue);
            _RGBToHSV(_Red, _Green, _Blue, out _Hue_HSV, out _Saturation_HSV, out _Brightness);
            _RGBToHSL(_Red, _Green, _Blue, out _Hue_HSL, out _Saturation_HSL, out _Lightness_HSL);
            _RGBToCMYK(_Red, _Green, _Blue, out _Cyan, out _Magenta, out _Yellow, out _Black);

            //

            _Initialized = true;
        }

        private void _CtorLAB(double lightness, double greenRed, double blueYellow) // 为以颜色在 LAB 色彩空间的各分量为参数的构造函数提供实现。
        {
            _Lightness_LAB = _CheckLightness_LAB(lightness);
            _GreenRed = _CheckGreenRed(greenRed);
            _BlueYellow = _CheckBlueYellow(blueYellow);

            _LABToRGB(_Lightness_LAB, _GreenRed, _BlueYellow, out _Red, out _Green, out _Blue);
            _RGBToHSV(_Red, _Green, _Blue, out _Hue_HSV, out _Saturation_HSV, out _Brightness);
            _RGBToHSL(_Red, _Green, _Blue, out _Hue_HSL, out _Saturation_HSL, out _Lightness_HSL);
            _RGBToCMYK(_Red, _Green, _Blue, out _Cyan, out _Magenta, out _Yellow, out _Black);

            //

            _Initialized = true;
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用 Color 结构初始化 ColorX 结构的新实例。
        /// </summary>
        /// <param name="color">Color 结构。</param>
        public ColorX(Color color) : this()
        {
            _CtorRGB(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// 使用颜色在 RGB 色彩空间的 32 位 ARGB 值初始化 ColorX 结构的新实例。
        /// </summary>
        /// <param name="argb">颜色在 RGB 色彩空间的 32 位 ARGB 值。</param>
        public ColorX(int argb) : this()
        {
            _CtorRGB(((uint)argb >> _32BitARGBAlphaShift) & 0xFFU, ((uint)argb >> _32BitARGBRedShift) & 0xFFU, ((uint)argb >> _32BitARGBGreenShift) & 0xFFU, ((uint)argb >> _32BitARGBBlueShift) & 0xFFU);
        }

        /// <summary>
        /// 使用颜色的 16 进制 ARGB 码或 RGB 码初始化 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hexCode">颜色的 16 进制 ARGB 码或 RGB 码。</param>
        public ColorX(string hexCode) : this()
        {
            if (string.IsNullOrEmpty(hexCode))
            {
                throw new ArgumentNullException();
            }

            string HexCode = new Regex(@"[^A-Fa-f\d]").Replace(hexCode, string.Empty);

            if (string.IsNullOrEmpty(HexCode))
            {
                throw new ArgumentNullException();
            }

            if (HexCode.Length > 8)
            {
                throw new OverflowException();
            }

            //

            int Argb = int.Parse(HexCode, NumberStyles.HexNumber);

            _CtorRGB(((uint)Argb >> _32BitARGBAlphaShift) & 0xFFU, ((uint)Argb >> _32BitARGBRedShift) & 0xFFU, ((uint)Argb >> _32BitARGBGreenShift) & 0xFFU, ((uint)Argb >> _32BitARGBBlueShift) & 0xFFU);
        }

        #endregion

        #region 字段

        /// <summary>
        /// 表示所有属性为其数据类型的默认值的 ColorX 结构的实例。
        /// </summary>
        public static readonly ColorX Empty = new ColorX();

        /// <summary>
        /// 表示透明色的 ColorX 结构的实例。
        /// </summary>
        public static readonly ColorX Transparent = FromRGB(_MinAlpha, _MaxRed, _MaxGreen, _MaxBlue);

        #endregion

        #region 属性

        /// <summary>
        /// 获取表示此 ColorX 结构是否未初始化的布尔值。
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (!_Initialized);
            }
        }

        /// <summary>
        /// 获取表示此 ColorX 结构是否为透明色的布尔值。
        /// </summary>
        public bool IsTransparent
        {
            get
            {
                return (_Opacity == _MinOpacity);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构的不透明度。
        /// </summary>
        public double Opacity
        {
            get
            {
                return _Opacity;
            }

            set
            {
                _Opacity = _CheckOpacity(value);

                _OpacityToAlpha(_Opacity, out _Alpha);

                _Alpha = _CheckAlpha(_Alpha);

                //

                if (!_Initialized)
                {
                    _Black = _MaxBlack;

                    _Initialized = true;
                }
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构的 Alpha 通道（A）的值。
        /// </summary>
        public double Alpha
        {
            get
            {
                return _Alpha;
            }

            set
            {
                _Alpha = _CheckAlpha(value);

                _AlphaToOpacity(_Alpha, out _Opacity);

                _Opacity = _CheckOpacity(_Opacity);

                //

                if (!_Initialized)
                {
                    _Black = _MaxBlack;

                    _Initialized = true;
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构在 RGB 色彩空间的红色通道（R）的值。
        /// </summary>
        public double Red
        {
            get
            {
                return _Red;
            }

            set
            {
                _CtorRGB(value, _Green, _Blue);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 RGB 色彩空间的绿色通道（G）的值。
        /// </summary>
        public double Green
        {
            get
            {
                return _Green;
            }

            set
            {
                _CtorRGB(_Red, value, _Blue);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 RGB 色彩空间的蓝色通道（B）的值。
        /// </summary>
        public double Blue
        {
            get
            {
                return _Blue;
            }

            set
            {
                _CtorRGB(_Red, _Green, value);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSV 色彩空间的色相（H）。
        /// </summary>
        public double Hue_HSV
        {
            get
            {
                return _Hue_HSV;
            }

            set
            {
                _CtorHSV(value, _Saturation_HSV, _Brightness);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSV 色彩空间的饱和度（S）。
        /// </summary>
        public double Saturation_HSV
        {
            get
            {
                return _Saturation_HSV;
            }

            set
            {
                _CtorHSV(_Hue_HSV, value, _Brightness);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSV 色彩空间的亮度（V）。
        /// </summary>
        public double Brightness
        {
            get
            {
                return _Brightness;
            }

            set
            {
                _CtorHSV(_Hue_HSV, _Saturation_HSV, value);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSL 色彩空间的色相（H）。
        /// </summary>
        public double Hue_HSL
        {
            get
            {
                return _Hue_HSL;
            }

            set
            {
                _CtorHSL(value, _Saturation_HSL, _Lightness_HSL);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSL 色彩空间的饱和度（S）。
        /// </summary>
        public double Saturation_HSL
        {
            get
            {
                return _Saturation_HSL;
            }

            set
            {
                _CtorHSL(_Hue_HSL, value, _Lightness_HSL);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSL 色彩空间的明度（L）。
        /// </summary>
        public double Lightness_HSL
        {
            get
            {
                return _Lightness_HSL;
            }

            set
            {
                _CtorHSL(_Hue_HSL, _Saturation_HSL, value);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构在 CMYK 色彩空间的青色通道（C）的值。
        /// </summary>
        public double Cyan
        {
            get
            {
                return _Cyan;
            }

            set
            {
                _CtorCMYK(value, _Magenta, _Yellow, Black);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 CMYK 色彩空间的洋红色通道（M）的值。
        /// </summary>
        public double Magenta
        {
            get
            {
                return _Magenta;
            }

            set
            {
                _CtorCMYK(_Cyan, value, _Yellow, Black);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 CMYK 色彩空间的黄色通道（Y）的值。
        /// </summary>
        public double Yellow
        {
            get
            {
                return _Yellow;
            }

            set
            {
                _CtorCMYK(_Cyan, _Magenta, value, Black);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 CMYK 色彩空间的黑色通道（K）的值。
        /// </summary>
        public double Black
        {
            get
            {
                if (!_Initialized)
                {
                    return _MaxBlack;
                }
                else
                {
                    return _Black;
                }
            }

            set
            {
                _CtorCMYK(_Cyan, _Magenta, _Yellow, value);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构在 LAB 色彩空间的明度（L）。
        /// </summary>
        public double Lightness_LAB
        {
            get
            {
                return _Lightness_LAB;
            }

            set
            {
                _CtorLAB(value, _GreenRed, _BlueYellow);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 LAB 色彩空间的绿色-红色通道（A）的值。
        /// </summary>
        public double GreenRed
        {
            get
            {
                return _GreenRed;
            }

            set
            {
                _CtorLAB(_Lightness_LAB, value, _BlueYellow);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 LAB 色彩空间的蓝色-黄色通道（B）的值。
        /// </summary>
        public double BlueYellow
        {
            get
            {
                return _BlueYellow;
            }

            set
            {
                _CtorLAB(_Lightness_LAB, _GreenRed, value);
            }
        }

        //

        /// <summary>
        /// 获取或设置表示此 ColorX 结构在 RGB 色彩空间的各分量的 PointD3D 结构。
        /// </summary>
        public PointD3D RGB
        {
            get
            {
                return new PointD3D(_Red, _Green, _Blue);
            }

            set
            {
                _CtorRGB(value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// 获取或设置表示此 ColorX 结构在 HSV 色彩空间的各分量的 PointD3D 结构。
        /// </summary>
        public PointD3D HSV
        {
            get
            {
                return new PointD3D(_Hue_HSV, _Saturation_HSV, _Brightness);
            }

            set
            {
                _CtorHSV(value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// 获取或设置表示此 ColorX 结构在 HSL 色彩空间的各分量的 PointD3D 结构。
        /// </summary>
        public PointD3D HSL
        {
            get
            {
                return new PointD3D(_Hue_HSL, _Saturation_HSL, _Lightness_HSL);
            }

            set
            {
                _CtorHSL(value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// 获取或设置表示此 ColorX 结构在 CMYK 色彩空间的各分量的 PointD4D 结构。
        /// </summary>
        public PointD4D CMYK
        {
            get
            {
                return new PointD4D(_Cyan, _Magenta, _Yellow, Black);
            }

            set
            {
                _CtorCMYK(value.X, value.Y, value.Z, value.U);
            }
        }

        /// <summary>
        /// 获取或设置表示此 ColorX 结构在 LAB 色彩空间的各分量的 PointD3D 结构。
        /// </summary>
        public PointD3D LAB
        {
            get
            {
                return new PointD3D(_Lightness_LAB, _GreenRed, _BlueYellow);
            }

            set
            {
                _CtorLAB(value.X, value.Y, value.Z);
            }
        }

        //

        /// <summary>
        /// 获取此 ColorX 结构的互补色。
        /// </summary>
        public ColorX ComplementaryColor
        {
            get
            {
                ColorX color = new ColorX();

                color._CtorRGB(_Alpha, _MaxRed - _Red, _MaxGreen - _Green, _MaxBlue - _Blue);

                return color;
            }
        }

        /// <summary>
        /// 获取此 ColorX 结构的灰度颜色。
        /// </summary>
        public ColorX GrayscaleColor
        {
            get
            {
                ColorX color = new ColorX();

                double Y = 0.299 * _Red + 0.587 * _Green + 0.114 * _Blue;

                color._CtorRGB(_Alpha, Y, Y, Y);

                return color;
            }
        }

        //

        /// <summary>
        /// 获取此 ColorX 结构的 16 进制 ARGB 码。
        /// </summary>
        public string ARGBHexCode
        {
            get
            {
                int Argb = (int)(((uint)Math.Round(_Alpha) << _32BitARGBAlphaShift) | ((uint)Math.Round(_Red) << _32BitARGBRedShift) | ((uint)Math.Round(_Green) << _32BitARGBGreenShift) | ((uint)Math.Round(_Blue) << _32BitARGBBlueShift));

                string HexCode = Convert.ToString(Argb, 16).ToUpper();

                int Len = HexCode.Length;

                if (Len < 8)
                {
                    return ("#" + HexCode.PadLeft(8, '0'));
                }
                else
                {
                    return ("#" + HexCode);
                }
            }
        }

        /// <summary>
        /// 获取此 ColorX 结构的 16 进制 RGB 码。
        /// </summary>
        public string RGBHexCode
        {
            get
            {
                int Rgb = (int)(((uint)Math.Round(_Red) << _32BitARGBRedShift) | ((uint)Math.Round(_Green) << _32BitARGBGreenShift) | ((uint)Math.Round(_Blue) << _32BitARGBBlueShift));

                string HexCode = Convert.ToString(Rgb, 16).ToUpper();

                int Len = HexCode.Length;

                if (Len < 6)
                {
                    return ("#" + HexCode.PadLeft(6, '0'));
                }
                else
                {
                    return ("#" + HexCode);
                }
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 ColorX 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>布尔值，表示此 ColorX 结构是否与指定的对象相等。</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ColorX))
            {
                return false;
            }
            else if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else
            {
                return Equals((ColorX)obj);
            }
        }

        /// <summary>
        /// 返回此 ColorX 结构的哈希代码。
        /// </summary>
        /// <returns>32 位整数，表示此 ColorX 结构的哈希代码。</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 ColorX 结构转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 ColorX 结构的字符串形式。</returns>
        public override string ToString()
        {
            string Str = string.Empty;

            if (!_Initialized)
            {
                Str = "Empty";
            }
            else
            {
                Str = string.Concat("A=", Alpha, ", R=", Red, ", G=", Green, ", B=", Blue);
            }

            return string.Concat(base.GetType().Name, " [", Str, "]");
        }

        //

        /// <summary>
        /// 判断此 ColorX 结构是否与指定的 ColorX 结构相等。
        /// </summary>
        /// <param name="color">用于比较的 ColorX 结构。</param>
        /// <returns>布尔值，表示此 ColorX 结构是否与指定的 ColorX 结构相等。</returns>
        public bool Equals(ColorX color)
        {
            return (_Initialized.Equals(color._Initialized) && _Opacity.Equals(color._Opacity) && (_Alpha.Equals(color._Alpha) && _Red.Equals(color._Red) && _Green.Equals(color._Green) && _Blue.Equals(color._Blue)) && (_Hue_HSV.Equals(color._Hue_HSV) && _Saturation_HSV.Equals(color._Saturation_HSV) && _Brightness.Equals(color._Brightness)) && (_Hue_HSL.Equals(color._Hue_HSL) && _Saturation_HSL.Equals(color._Saturation_HSL) && _Lightness_HSL.Equals(color._Lightness_HSL)) && (_Cyan.Equals(color._Cyan) && _Magenta.Equals(color._Magenta) && _Yellow.Equals(color._Yellow) && _Black.Equals(color._Black)) && (_Lightness_LAB.Equals(color._Lightness_LAB) && _GreenRed.Equals(color._GreenRed) && _BlueYellow.Equals(color._BlueYellow)));
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构转换为 Color 结构的新实例。
        /// </summary>
        /// <returns>Color 结构，表示将此 ColorX 结构转换为 Color 结构得到的结果。</returns>
        public Color ToColor()
        {
            if (!_Initialized)
            {
                return Color.Empty;
            }
            else
            {
                return Color.FromArgb((int)Math.Round(_Alpha), (int)Math.Round(_Red), (int)Math.Round(_Green), (int)Math.Round(_Blue));
            }
        }

        /// <summary>
        /// 返回将此 ColorX 结构转换为 Color 结构的 32 位 ARGB 值。
        /// </summary>
        /// <returns>32 位整数，表示将此 ColorX 结构转换为 Color 结构的 32 位 ARGB 值。</returns>
        public int ToARGB()
        {
            return (int)(((uint)Math.Round(_Alpha) << _32BitARGBAlphaShift) | ((uint)Math.Round(_Red) << _32BitARGBRedShift) | ((uint)Math.Round(_Green) << _32BitARGBGreenShift) | ((uint)Math.Round(_Blue) << _32BitARGBBlueShift));
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构的不透明度更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构的不透明度更改为指定值得到的结果。</returns>
        public ColorX AtOpacity(double opacity)
        {
            ColorX color = this;

            color.Opacity = opacity;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构的 Alpha 通道（A）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="alpha">颜色的 Alpha 通道（A）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构的 Alpha 通道（A）的值更改为指定值得到的结果。</returns>
        public ColorX AtAlpha(double alpha)
        {
            ColorX color = this;

            color.Alpha = alpha;

            return color;
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构在 RGB 色彩空间的红色通道（R）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="red">颜色在 RGB 色彩空间的红色通道（R）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 RGB 色彩空间的红色通道（R）的值更改为指定值得到的结果。</returns>
        public ColorX AtRed(double red)
        {
            ColorX color = this;

            color.Red = red;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 RGB 色彩空间的绿色通道（G）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="green">颜色在 RGB 色彩空间的绿色通道（G）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 RGB 色彩空间的绿色通道（G）的值更改为指定值得到的结果。</returns>
        public ColorX AtGreen(double green)
        {
            ColorX color = this;

            color.Green = green;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 RGB 色彩空间的蓝色通道（B）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="blue">颜色在 RGB 色彩空间的蓝色通道（B）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 RGB 色彩空间的蓝色通道（B）的值更改为指定值得到的结果。</returns>
        public ColorX AtBlue(double blue)
        {
            ColorX color = this;

            color.Blue = blue;

            return color;
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构在 HSV 色彩空间的色相（H）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSV 色彩空间的色相（H）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 HSV 色彩空间的色相（H）更改为指定值得到的结果。</returns>
        public ColorX AtHue_HSV(double hue)
        {
            ColorX color = this;

            color.Hue_HSV = hue;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 HSV 色彩空间的饱和度（S）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="saturation">颜色在 HSV 色彩空间的饱和度（S）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 HSV 色彩空间的饱和度（S）更改为指定值得到的结果。</returns>
        public ColorX AtSaturation_HSV(double saturation)
        {
            ColorX color = this;

            color.Saturation_HSV = saturation;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 HSV 色彩空间的亮度（V）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="brightness">颜色在 HSV 色彩空间的亮度（V）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 HSV 色彩空间的亮度（V）更改为指定值得到的结果。</returns>
        public ColorX AtBrightness(double brightness)
        {
            ColorX color = this;

            color.Brightness = brightness;

            return color;
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构在 HSL 色彩空间的色相（H）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSL 色彩空间的色相（H）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 HSL 色彩空间的色相（H）更改为指定值得到的结果。</returns>
        public ColorX AtHue_HSL(double hue)
        {
            ColorX color = this;

            color.Hue_HSL = hue;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 HSL 色彩空间的饱和度（S）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="saturation">颜色在 HSL 色彩空间的饱和度（S）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 HSL 色彩空间的饱和度（S）更改为指定值得到的结果。</returns>
        public ColorX AtSaturation_HSL(double saturation)
        {
            ColorX color = this;

            color.Saturation_HSL = saturation;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 HSL 色彩空间的明度（L）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lightness">颜色在 HSL 色彩空间的明度（L）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 HSL 色彩空间的明度（L）更改为指定值得到的结果。</returns>
        public ColorX AtLightness_HSL(double lightness)
        {
            ColorX color = this;

            color.Lightness_HSL = lightness;

            return color;
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构在 CMYK 色彩空间的青色通道（C）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="cyan">颜色在 CMYK 色彩空间的青色通道（C）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 CMYK 色彩空间的青色通道（C）的值更改为指定值得到的结果。</returns>
        public ColorX AtCyan(double cyan)
        {
            ColorX color = this;

            color.Cyan = cyan;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 CMYK 色彩空间的洋红色通道（M）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="magenta">颜色在 CMYK 色彩空间的洋红色通道（M）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 CMYK 色彩空间的洋红色通道（M）的值更改为指定值得到的结果。</returns>
        public ColorX AtMagenta(double magenta)
        {
            ColorX color = this;

            color.Magenta = magenta;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 CMYK 色彩空间的黄色通道（Y）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="yellow">颜色在 CMYK 色彩空间的黄色通道（Y）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 CMYK 色彩空间的黄色通道（Y）的值更改为指定值得到的结果。</returns>
        public ColorX AtYellow(double yellow)
        {
            ColorX color = this;

            color.Yellow = yellow;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 CMYK 色彩空间的黑色通道（K）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="black">颜色在 CMYK 色彩空间的黑色通道（K）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 CMYK 色彩空间的黑色通道（K）的值更改为指定值得到的结果。</returns>
        public ColorX AtBlack(double black)
        {
            ColorX color = this;

            color.Black = black;

            return color;
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构在 LAB 色彩空间的明度（L）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lightness">颜色在 LAB 色彩空间的明度（L）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 LAB 色彩空间的明度（L）更改为指定值得到的结果。</returns>
        public ColorX AtLightness_LAB(double lightness)
        {
            ColorX color = this;

            color.Lightness_LAB = lightness;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 LAB 色彩空间的绿色-红色通道（A）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="greenRed">颜色在 LAB 色彩空间的绿色-红色通道（A）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 LAB 色彩空间的绿色-红色通道（A）的值更改为指定值得到的结果。</returns>
        public ColorX AtGreenRed(double greenRed)
        {
            ColorX color = this;

            color.GreenRed = greenRed;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 LAB 色彩空间的蓝色-黄色通道（B）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="blueYellow">颜色在 LAB 色彩空间的蓝色-黄色通道（B）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 LAB 色彩空间的蓝色-黄色通道（B）的值更改为指定值得到的结果。</returns>
        public ColorX AtBlueYellow(double blueYellow)
        {
            ColorX color = this;

            color.BlueYellow = blueYellow;

            return color;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断两个 ColorX 结构是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 ColorX 结构。</param>
        /// <param name="right">用于比较的第二个 ColorX 结构。</param>
        /// <returns>布尔值，表示两个 ColorX 结构是否相等。</returns>
        public static bool Equals(ColorX left, ColorX right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            else if ((object)left == null || (object)right == null)
            {
                return false;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else
            {
                return left.Equals(right);
            }
        }

        //

        /// <summary>
        /// 返回将 Color 结构转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="alpha">Alpha 通道（A）的值。</param>
        /// <param name="color">Color 结构。</param>
        /// <returns>ColorX 结构，表示将 Color 结构转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromColor(int alpha, Color color)
        {
            return new ColorX(Color.FromArgb(alpha, color));
        }

        /// <summary>
        /// 返回将 Color 结构转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="color">Color 结构。</param>
        /// <returns>ColorX 结构，表示将 Color 结构转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromColor(Color color)
        {
            return new ColorX(color);
        }

        //

        /// <summary>
        /// 返回将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="alpha">颜色的 Alpha 通道（A）的值。</param>
        /// <param name="red">颜色在 RGB 色彩空间的红色通道（R）的值。</param>
        /// <param name="green">颜色在 RGB 色彩空间的绿色通道（G）的值。</param>
        /// <param name="blue">颜色在 RGB 色彩空间的蓝色通道（B）的值。</param>
        /// <returns>ColorX 结构，表示将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromRGB(double alpha, double red, double green, double blue)
        {
            ColorX color = new ColorX();

            color._CtorRGB(alpha, red, green, blue);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="red">颜色在 RGB 色彩空间的红色通道（R）的值。</param>
        /// <param name="green">颜色在 RGB 色彩空间的绿色通道（G）的值。</param>
        /// <param name="blue">颜色在 RGB 色彩空间的蓝色通道（B）的值。</param>
        /// <returns>ColorX 结构，表示将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromRGB(double red, double green, double blue)
        {
            ColorX color = new ColorX();

            color._CtorRGB(_MaxAlpha, red, green, blue);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="alpha">颜色的 Alpha 通道（A）的值。</param>
        /// <param name="rgb">表示颜色在 RGB 色彩空间的各分量的 PointD3D 结构。</param>
        /// <returns>ColorX 结构，表示将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromRGB(double alpha, PointD3D rgb)
        {
            ColorX color = new ColorX();

            color._CtorRGB(alpha, rgb.X, rgb.Y, rgb.Z);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="rgb">表示颜色在 RGB 色彩空间的各分量的 PointD3D 结构。</param>
        /// <returns>ColorX 结构，表示将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromRGB(PointD3D rgb)
        {
            ColorX color = new ColorX();

            color._CtorRGB(_MaxAlpha, rgb.X, rgb.Y, rgb.Z);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 RGB 色彩空间的 32 位 ARGB 值转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="argb">颜色在 RGB 色彩空间的 32 位 ARGB 值。</param>
        /// <returns>ColorX 结构，表示将颜色在 RGB 色彩空间的 32 位 ARGB 值转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromRGB(int argb)
        {
            return new ColorX(argb);
        }

        //

        /// <summary>
        /// 返回将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSV 色彩空间的色相（H）。</param>
        /// <param name="saturation">颜色在 HSV 色彩空间的饱和度（S）。</param>
        /// <param name="brightness">颜色在 HSV 色彩空间的亮度（V）。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSV(double hue, double saturation, double brightness, double opacity)
        {
            ColorX color = new ColorX();

            color._CtorHSV(hue, saturation, brightness, opacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSV 色彩空间的色相（H）。</param>
        /// <param name="saturation">颜色在 HSV 色彩空间的饱和度（S）。</param>
        /// <param name="brightness">颜色在 HSV 色彩空间的亮度（V）。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSV(double hue, double saturation, double brightness)
        {
            ColorX color = new ColorX();

            color._CtorHSV(hue, saturation, brightness, _MaxOpacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hsv">表示颜色在 HSV 色彩空间的各分量的 PointD3D 结构。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSV(PointD3D hsv, double opacity)
        {
            ColorX color = new ColorX();

            color._CtorHSV(hsv.X, hsv.Y, hsv.Z, opacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hsv">表示颜色在 HSV 色彩空间的各分量的 PointD3D 结构。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSV(PointD3D hsv)
        {
            ColorX color = new ColorX();

            color._CtorHSV(hsv.X, hsv.Y, hsv.Z, _MaxOpacity);

            return color;
        }

        //

        /// <summary>
        /// 返回将颜色在 HSL 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSL 色彩空间的色相（H）。</param>
        /// <param name="saturation">颜色在 HSL 色彩空间的饱和度（S）。</param>
        /// <param name="lightness">颜色在 HSL 色彩空间的明度（L）。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSL 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSL(double hue, double saturation, double lightness, double opacity)
        {
            ColorX color = new ColorX();

            color._CtorHSL(hue, saturation, lightness, opacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSL 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSL 色彩空间的色相（H）。</param>
        /// <param name="saturation">颜色在 HSL 色彩空间的饱和度（S）。</param>
        /// <param name="lightness">颜色在 HSL 色彩空间的明度（L）。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSL 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSL(double hue, double saturation, double lightness)
        {
            ColorX color = new ColorX();

            color._CtorHSL(hue, saturation, lightness, _MaxOpacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSL 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hsl">表示颜色在 HSL 色彩空间的各分量的 PointD3D 结构。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSL 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSL(PointD3D hsl, double opacity)
        {
            ColorX color = new ColorX();

            color._CtorHSL(hsl.X, hsl.Y, hsl.Z, opacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSL 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hsl">表示颜色在 HSL 色彩空间的各分量的 PointD3D 结构。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSL 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSL(PointD3D hsl)
        {
            ColorX color = new ColorX();

            color._CtorHSL(hsl.X, hsl.Y, hsl.Z, _MaxOpacity);

            return color;
        }

        //

        /// <summary>
        /// 返回将颜色在 CMYK 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="cyan">颜色在 CMYK 色彩空间的青色通道（C）的值。</param>
        /// <param name="magenta">颜色在 CMYK 色彩空间的洋红色通道（M）的值。</param>
        /// <param name="yellow">颜色在 CMYK 色彩空间的黄色通道（Y）的值。</param>
        /// <param name="black">颜色在 CMYK 色彩空间的黑色通道（K）的值。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 CMYK 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromCMYK(double cyan, double magenta, double yellow, double black, double opacity)
        {
            ColorX color = new ColorX();

            color._CtorCMYK(cyan, magenta, yellow, black, opacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 CMYK 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="cyan">颜色在 CMYK 色彩空间的青色通道（C）的值。</param>
        /// <param name="magenta">颜色在 CMYK 色彩空间的洋红色通道（M）的值。</param>
        /// <param name="yellow">颜色在 CMYK 色彩空间的黄色通道（Y）的值。</param>
        /// <param name="black">颜色在 CMYK 色彩空间的黑色通道（K）的值。</param>
        /// <returns>ColorX 结构，表示将颜色在 CMYK 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromCMYK(double cyan, double magenta, double yellow, double black)
        {
            ColorX color = new ColorX();

            color._CtorCMYK(cyan, magenta, yellow, black, _MaxOpacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 CMYK 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="cmyk">表示颜色在 CMYK 色彩空间的各分量的 PointD4D 结构。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 CMYK 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromCMYK(PointD4D cmyk, double opacity)
        {
            ColorX color = new ColorX();

            color._CtorCMYK(cmyk.X, cmyk.Y, cmyk.Z, cmyk.U, opacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 CMYK 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="cmyk">表示颜色在 CMYK 色彩空间的各分量的 PointD4D 结构。</param>
        /// <returns>ColorX 结构，表示将颜色在 CMYK 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromCMYK(PointD4D cmyk)
        {
            ColorX color = new ColorX();

            color._CtorCMYK(cmyk.X, cmyk.Y, cmyk.Z, cmyk.U, _MaxOpacity);

            return color;
        }

        //

        /// <summary>
        /// 返回将颜色在 LAB 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lightness">颜色在 LAB 色彩空间的明度（L）。</param>
        /// <param name="greenRed">颜色在 LAB 色彩空间的绿色-红色通道（A）的值。</param>
        /// <param name="blueYellow">颜色在 LAB 色彩空间的蓝色-黄色通道（B）的值。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 LAB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromLAB(double lightness, double greenRed, double blueYellow, double opacity)
        {
            ColorX color = new ColorX();

            color._CtorLAB(lightness, greenRed, blueYellow, opacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 LAB 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lightness">颜色在 LAB 色彩空间的明度（L）。</param>
        /// <param name="greenRed">颜色在 LAB 色彩空间的绿色-红色通道（A）的值。</param>
        /// <param name="blueYellow">颜色在 LAB 色彩空间的蓝色-黄色通道（B）的值。</param>
        /// <returns>ColorX 结构，表示将颜色在 LAB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromLAB(double lightness, double greenRed, double blueYellow)
        {
            ColorX color = new ColorX();

            color._CtorLAB(lightness, greenRed, blueYellow, _MaxOpacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 LAB 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lab">表示颜色在 LAB 色彩空间的各分量的 PointD3D 结构。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 LAB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromLAB(PointD3D lab, double opacity)
        {
            ColorX color = new ColorX();

            color._CtorLAB(lab.X, lab.Y, lab.Z, opacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 LAB 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lab">表示颜色在 LAB 色彩空间的各分量的 PointD3D 结构。</param>
        /// <returns>ColorX 结构，表示将颜色在 LAB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromLAB(PointD3D lab)
        {
            ColorX color = new ColorX();

            color._CtorLAB(lab.X, lab.Y, lab.Z, _MaxOpacity);

            return color;
        }

        //

        /// <summary>
        /// 返回将颜色的 16 进制 ARGB 码或 RGB 码转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hexCode">颜色的 16 进制 ARGB 码或 RGB 码。</param>
        /// <returns>ColorX 结构，表示将颜色的 16 进制 ARGB 码或 RGB 码转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHexCode(string hexCode)
        {
            return new ColorX(hexCode);
        }

        //

        /// <summary>
        /// 返回一个不透明度为 100%，其他分量为随机数的 ColorX 结构的新实例。
        /// </summary>
        /// <returns>ColorX 结构，表示不透明度为 100%，其他分量为随机数的 ColorX 结构。</returns>
        public static ColorX RandomColor()
        {
            return FromRGB(Statistics.RandomDouble(_MinRed, _MaxRed), Statistics.RandomDouble(_MinGreen, _MaxGreen), Statistics.RandomDouble(_MinBlue, _MaxBlue));
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 ColorX 结构是否表示相同的颜色。
        /// </summary>
        /// <param name="left">运算符左侧比较的 ColorX 结构。</param>
        /// <param name="right">运算符右侧比较的 ColorX 结构。</param>
        /// <returns>布尔值，表示两个 ColorX 结构是否表示相同的颜色。</returns>
        public static bool operator ==(ColorX left, ColorX right)
        {
            if (!left._Initialized || !right._Initialized)
            {
                return false;
            }
            else
            {
                return (left.Alpha == right.Alpha && left.Red == right.Red && left.Green == right.Green && left.Blue == right.Blue);
            }
        }

        /// <summary>
        /// 判断两个 ColorX 结构是否表示不同的颜色。
        /// </summary>
        /// <param name="left">运算符左侧比较的 ColorX 结构。</param>
        /// <param name="right">运算符右侧比较的 ColorX 结构。</param>
        /// <returns>布尔值，表示两个 ColorX 结构是否表示不同的颜色。</returns>
        public static bool operator !=(ColorX left, ColorX right)
        {
            if (!left._Initialized || !right._Initialized)
            {
                return true;
            }
            else
            {
                return (left.Alpha != right.Alpha || left.Red != right.Red || left.Green != right.Green || left.Blue != right.Blue);
            }
        }

        //

        /// <summary>
        /// 将指定的 ColorX 结构显式转换为 Color 结构。
        /// </summary>
        /// <param name="color">用于转换的 ColorX 结构。</param>
        /// <returns>Color 结构，表示显式转换的结果。</returns>
        public static explicit operator Color(ColorX color)
        {
            return color.ToColor();
        }

        /// <summary>
        /// 将指定的 Color 结构隐式转换为 ColorX 结构。
        /// </summary>
        /// <param name="color">用于转换的 Color 结构。</param>
        /// <returns>ColorX 结构，表示隐式转换的结果。</returns>
        public static implicit operator ColorX(Color color)
        {
            return new ColorX(color);
        }

        #endregion
    }
}