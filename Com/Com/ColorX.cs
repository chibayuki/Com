﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.ColorX
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
using System.Text.RegularExpressions;

namespace Com
{
    /// <summary>
    /// 以双精度浮点数在 RGB、HSV、HSL、CMYK、LAB 等色彩空间中表示的颜色。
    /// </summary>
    public struct ColorX
    {
        #region 私有与内部成员

        private const double _MinOpacity = 0D, _MaxOpacity = 100D; // 颜色的不透明度的最小值与最大值。

        private const double _MinAlpha = 0D, _MaxAlpha = 255D; // 颜色在 RGB 色彩空间中的 Alpha 通道（A）的最小值与最大值。
        private const double _MinRed = 0D, _MaxRed = 255D; // 颜色在 RGB 色彩空间中的红色通道（R）的最小值与最大值。
        private const double _MinGreen = 0D, _MaxGreen = 255D; // 颜色在 RGB 色彩空间中的绿色通道（G）的最小值与最大值。
        private const double _MinBlue = 0D, _MaxBlue = 255D; // 颜色在 RGB 色彩空间中的蓝色通道（B）的最小值与最大值。

        private const double _MinHue_HSV = 0D, _MaxHue_HSV = 360D; // 颜色在 HSV 色彩空间中的色相（H）的最小值与最大值。
        private const double _MinSaturation_HSV = 0D, _MaxSaturation_HSV = 100D; // 颜色在 HSV 色彩空间中的饱和度（S）的最小值与最大值。
        private const double _MinBrightness = 0D, _MaxBrightness = 100D; // 颜色在 HSV 色彩空间中的明度（V）的最小值与最大值。

        private const double _MinHue_HSL = 0D, _MaxHue_HSL = 360D; // 颜色在 HSL 色彩空间中的色相（H）的最小值与最大值。
        private const double _MinSaturation_HSL = 0D, _MaxSaturation_HSL = 100D; // 颜色在 HSL 色彩空间中的饱和度（S）的最小值与最大值。
        private const double _MinLightness_HSL = 0D, _MaxLightness_HSL = 100D; // 颜色在 HSL 色彩空间中的明度（L）的最小值与最大值。

        private const double _MinCyan = 0D, _MaxCyan = 100D; // 颜色在 CMYK 色彩空间中的青色通道（C）的最小值与最大值。
        private const double _MinMagenta = 0D, _MaxMagenta = 100D; // 颜色在 CMYK 色彩空间中的洋红色通道（M）的最小值与最大值。
        private const double _MinYellow = 0D, _MaxYellow = 100D; // 颜色在 CMYK 色彩空间中的黄色通道（Y）的最小值与最大值。
        private const double _MinBlack = 0D, _MaxBlack = 100D; // 颜色在 CMYK 色彩空间中的黑色通道（K）的最小值与最大值。

        private const double _MinLightness_LAB = 0D, _MaxLightness_LAB = 100D; // 颜色在 LAB 色彩空间中的明度（L）的最小值与最大值。
        private const double _MinGreenRed = -128D, _MaxGreenRed = 128D; // 颜色在 LAB 色彩空间中的绿色-红色通道（A）的最小值与最大值。
        private const double _MinBlueYellow = -128D, _MaxBlueYellow = 128D; // 颜色在 LAB 色彩空间中的蓝色-黄色通道（B）的最小值与最大值。

        //

        private static double _CheckOpacity(double opacity) // 对颜色的不透明度的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(opacity) || double.IsInfinity(opacity))
            {
                return _MinOpacity;
            }

            if (opacity < _MinOpacity)
            {
                return _MinOpacity;
            }
            else if (opacity > _MaxOpacity)
            {
                return _MaxOpacity;
            }

            return opacity;
        }

        private static double _CheckAlpha(double alpha) // 对颜色在 RGB 色彩空间中的 Alpha 通道（A）的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(alpha) || double.IsInfinity(alpha))
            {
                return _MinAlpha;
            }

            if (alpha < _MinAlpha)
            {
                return _MinAlpha;
            }
            else if (alpha > _MaxAlpha)
            {
                return _MaxAlpha;
            }

            return alpha;
        }

        private static double _CheckRed(double red) // 对颜色在 RGB 色彩空间中的红色通道（R）的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(red) || double.IsInfinity(red))
            {
                return _MinRed;
            }

            if (red < _MinRed)
            {
                return _MinRed;
            }
            else if (red > _MaxRed)
            {
                return _MaxRed;
            }

            return red;
        }

        private static double _CheckGreen(double green) // 对颜色在 RGB 色彩空间中的绿色通道（G）的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(green) || double.IsInfinity(green))
            {
                return _MinGreen;
            }

            if (green < _MinGreen)
            {
                return _MinGreen;
            }
            else if (green > _MaxGreen)
            {
                return _MaxGreen;
            }

            return green;
        }

        private static double _CheckBlue(double blue) // 对颜色在 RGB 色彩空间中的蓝色通道（B）的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(blue) || double.IsInfinity(blue))
            {
                return _MinBlue;
            }

            if (blue < _MinBlue)
            {
                return _MinBlue;
            }
            else if (blue > _MaxBlue)
            {
                return _MaxBlue;
            }

            return blue;
        }

        private static double _CheckHue_HSV(double hue) // 对颜色在 HSV 色彩空间中的色相（H）的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(hue) || double.IsInfinity(hue))
            {
                return _MinHue_HSV;
            }

            if (hue < _MinHue_HSV)
            {
                return _MinHue_HSV;
            }
            else if (hue > _MaxHue_HSV)
            {
                return _MaxHue_HSV;
            }

            return hue;
        }

        private static double _CheckSaturation_HSV(double saturation) // 对颜色在 HSV 色彩空间中的饱和度（S）的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(saturation) || double.IsInfinity(saturation))
            {
                return _MinSaturation_HSV;
            }

            if (saturation < _MinSaturation_HSV)
            {
                return _MinSaturation_HSV;
            }
            else if (saturation > _MaxSaturation_HSV)
            {
                return _MaxSaturation_HSV;
            }

            return saturation;
        }

        private static double _CheckBrightness(double brightness) // 对颜色在 HSV 色彩空间中的明度（V）的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(brightness) || double.IsInfinity(brightness))
            {
                return _MinBrightness;
            }

            if (brightness < _MinBrightness)
            {
                return _MinBrightness;
            }
            else if (brightness > _MaxBrightness)
            {
                return _MaxBrightness;
            }

            return brightness;
        }

        private static double _CheckHue_HSL(double hue) // 对颜色在 HSL 色彩空间中的色相（H）的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(hue) || double.IsInfinity(hue))
            {
                return _MinHue_HSL;
            }

            if (hue < _MinHue_HSL)
            {
                return _MinHue_HSL;
            }
            else if (hue > _MaxHue_HSL)
            {
                return _MaxHue_HSL;
            }

            return hue;
        }

        private static double _CheckSaturation_HSL(double saturation) // 对颜色在 HSL 色彩空间中的饱和度（S）的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(saturation) || double.IsInfinity(saturation))
            {
                return _MinSaturation_HSL;
            }

            if (saturation < _MinSaturation_HSL)
            {
                return _MinSaturation_HSL;
            }
            else if (saturation > _MaxSaturation_HSL)
            {
                return _MaxSaturation_HSL;
            }

            return saturation;
        }

        private static double _CheckLightness_HSL(double lightness) // 对颜色在 HSL 色彩空间中的明度（L）的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(lightness) || double.IsInfinity(lightness))
            {
                return _MinLightness_HSL;
            }

            if (lightness < _MinLightness_HSL)
            {
                return _MinLightness_HSL;
            }
            else if (lightness > _MaxLightness_HSL)
            {
                return _MaxLightness_HSL;
            }

            return lightness;
        }

        private static double _CheckCyan(double cyan) // 对颜色在 CMYK 色彩空间中的青色通道（C）的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(cyan) || double.IsInfinity(cyan))
            {
                return _MinCyan;
            }

            if (cyan < _MinCyan)
            {
                return _MinCyan;
            }
            else if (cyan > _MaxCyan)
            {
                return _MaxCyan;
            }

            return cyan;
        }

        private static double _CheckMagenta(double magenta) // 对颜色在 CMYK 色彩空间中的洋红色通道（M）的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(magenta) || double.IsInfinity(magenta))
            {
                return _MinMagenta;
            }

            if (magenta < _MinMagenta)
            {
                return _MinMagenta;
            }
            else if (magenta > _MaxMagenta)
            {
                return _MaxMagenta;
            }

            return magenta;
        }

        private static double _CheckYellow(double yellow) // 对颜色在 CMYK 色彩空间中的黄色通道（Y）的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(yellow) || double.IsInfinity(yellow))
            {
                return _MinYellow;
            }

            if (yellow < _MinYellow)
            {
                return _MinYellow;
            }
            else if (yellow > _MaxYellow)
            {
                return _MaxYellow;
            }

            return yellow;
        }

        private static double _CheckBlack(double black) // 对颜色在 CMYK 色彩空间中的黑色通道（K）的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(black) || double.IsInfinity(black))
            {
                return _MaxBlack;
            }

            if (black < _MinBlack)
            {
                return _MinBlack;
            }
            else if (black > _MaxBlack)
            {
                return _MaxBlack;
            }

            return black;
        }

        private static double _CheckLightness_LAB(double lightness) // 对颜色在 LAB 色彩空间中的明度（L）的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(lightness) || double.IsInfinity(lightness))
            {
                return _MinLightness_LAB;
            }

            if (lightness < _MinLightness_LAB)
            {
                return _MinLightness_LAB;
            }
            else if (lightness > _MaxLightness_LAB)
            {
                return _MaxLightness_LAB;
            }

            return lightness;
        }

        private static double _CheckGreenRed(double greenRed) // 对颜色在 LAB 色彩空间中的绿色-红色通道（A）的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(greenRed) || double.IsInfinity(greenRed))
            {
                return 0;
            }

            if (greenRed < _MinGreenRed)
            {
                return _MinGreenRed;
            }
            else if (greenRed > _MaxGreenRed)
            {
                return _MaxGreenRed;
            }

            return greenRed;
        }

        private static double _CheckBlueYellow(double blueYellow) // 对颜色在 LAB 色彩空间中的蓝色-黄色通道（B）的值进行合法性检查，返回合法的值。
        {
            if (double.IsNaN(blueYellow) || double.IsInfinity(blueYellow))
            {
                return 0;
            }

            if (blueYellow < _MinBlueYellow)
            {
                return _MinBlueYellow;
            }
            else if (blueYellow > _MaxBlueYellow)
            {
                return _MaxBlueYellow;
            }

            return blueYellow;
        }

        //

        private static void _OpacityToAlpha(double opacity, out double alpha) // 将颜色的不透明度转换为在 RGB 色彩空间中的 Alpha 通道（A）的值。此函数不对参数进行合法性检查。
        {
            alpha = opacity / _MaxOpacity * _MaxAlpha;
        }

        private static void _AlphaToOpacity(double alpha, out double opacity) // 将颜色在 RGB 色彩空间中的 Alpha 通道（A）的值转换为不透明度。此函数不对参数进行合法性检查。
        {
            opacity = alpha / _MaxAlpha * _MaxOpacity;
        }

        private static void _RGBToHSV(double red, double green, double blue, out double hue, out double saturation, out double brightness) // 将颜色在 RGB 色彩空间中的各分量转换为在 HSV 色彩空间中的各分量。此函数不对参数进行合法性检查。
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

            double dH = 1 / (6 * (Max - Min));

            if (Max == Min)
            {
                hue = 0;
            }
            else
            {
                if (Max == red)
                {
                    hue = dH * (green - blue);

                    if (green < blue)
                    {
                        hue += 1D;
                    }
                }
                else if (Max == green)
                {
                    hue = dH * (blue - red) + 1D / 3D;
                }
                else
                {
                    hue = dH * (red - green) + 2D / 3D;
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
        }

        private static void _HSVToRGB(double hue, double saturation, double brightness, out double red, out double green, out double blue) // 将颜色在 HSV 色彩空间中的各分量转换为在 RGB 色彩空间中的各分量。此函数不对参数进行合法性检查。
        {
            hue /= _MaxHue_HSV;
            saturation /= _MaxSaturation_HSV;
            brightness /= _MaxBrightness;

            int Hi = ((int)Math.Floor(hue * 6)) % 6;
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
        }

        private static void _RGBToHSL(double red, double green, double blue, out double hue, out double saturation, out double lightness) // 将颜色在 RGB 色彩空间中的各分量转换为在 HSL 色彩空间中的各分量。此函数不对参数进行合法性检查。
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

            double dH = 1 / (6 * (Max - Min));

            if (Max == Min)
            {
                hue = 0;
            }
            else
            {
                if (Max == red)
                {
                    hue = dH * (green - blue);

                    if (green < blue)
                    {
                        hue += 1D;
                    }
                }
                else if (Max == green)
                {
                    hue = dH * (blue - red) + 1D / 3D;
                }
                else
                {
                    hue = dH * (red - green) + 2D / 3D;
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
        }

        private static void _HSLToRGB(double hue, double saturation, double lightness, out double red, out double green, out double blue) // 将颜色在 HSL 色彩空间中的各分量转换为在 RGB 色彩空间中的各分量。此函数不对参数进行合法性检查。
        {
            hue /= _MaxHue_HSL;
            saturation /= _MaxSaturation_HSL;
            lightness /= _MaxLightness_HSL;

            int Hi = ((int)Math.Floor(hue * 6)) % 6;
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
        }

        private static void _RGBToCMYK(double red, double green, double blue, out double cyan, out double magenta, out double yellow, out double black) // 将颜色在 RGB 色彩空间中的各分量转换为在 CMYK 色彩空间中的各分量。此函数不对参数进行合法性检查。
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

            if (Max == 0)
            {
                cyan = 0;
                magenta = 0;
                yellow = 0;
                black = 1;
            }
            else
            {
                cyan = (Max - red) / Max;
                magenta = (Max - green) / Max;
                yellow = (Max - blue) / Max;
                black = 1 - Max;
            }

            cyan *= _MaxCyan;
            magenta *= _MaxMagenta;
            yellow *= _MaxYellow;
            black *= _MaxBlack;
        }

        private static void _CMYKToRGB(double cyan, double magenta, double yellow, double black, out double red, out double green, out double blue) // 将颜色在 CMYK 色彩空间中的各分量转换为在 RGB 色彩空间中的各分量。此函数不对参数进行合法性检查。
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
        }

        private static void _RGBToLAB(double red, double green, double blue, out double lightness, out double greenRed, out double blueYellow) // 将颜色在 RGB 色彩空间中的各分量转换为在 LAB 色彩空间中的各分量。此函数不对参数进行合法性检查。
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
                const double Delta = 6D / 29D;

                if (t > Delta * Delta * Delta)
                {
                    return Math.Pow(t, 1D / 3D);
                }
                else
                {
                    return (t / (3 * Delta * Delta) + 4D / 29D);
                }
            };

            double Fx = Fxyz(X);
            double Fy = Fxyz(Y);
            double Fz = Fxyz(Z);

            lightness = 116 * Fy - 16;
            greenRed = 500 * (Fx - Fy);
            blueYellow = 200 * (Fy - Fz);
        }

        private static void _LABToRGB(double lightness, double greenRed, double blueYellow, out double red, out double green, out double blue) // 将颜色在 LAB 色彩空间中的各分量转换为在 RGB 色彩空间中的各分量。此函数不对参数进行合法性检查。
        {
            double L = (lightness + 16) / 116;
            double a = greenRed / 500;
            double b = blueYellow / 200;

            Func<double, double> Fxyz = (t) =>
            {
                const double Delta = 6D / 29D;

                if (t > Delta)
                {
                    return (t * t * t);
                }
                else
                {
                    return ((t - 4D / 29D) * (3 * Delta * Delta));
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
        }

        //

        private static string _ByteToHexString(byte num) // 将 10 进制无符号整数转换为 16 进制字符串。
        {
            int High = num / 16, Low = num % 16;

            Func<int, char> DecToHexChar = (dec) =>
            {
                if (dec >= 0 && dec <= 9)
                {
                    return (char)('0' + dec);
                }
                else if (dec >= 10 && dec <= 15)
                {
                    return (char)('A' + dec - 10);
                }

                return '\0';
            };

            return string.Concat(DecToHexChar(High), DecToHexChar(Low));
        }

        private static byte _HexStringToByte(string str) // 将 16 进制字符串转换为 10 进制无符号整数。
        {
            if (!string.IsNullOrEmpty(str) && str.Length == 2)
            {
                char High = str[0], Low = str[1];

                Func<char, int> HexCharToDec = (hex) =>
                {
                    if (hex >= '0' && hex <= '9')
                    {
                        return (hex - '0');
                    }
                    else if (hex >= 'A' && hex <= 'F')
                    {
                        return (hex - 'A' + 10);
                    }
                    else if (hex >= 'a' && hex <= 'f')
                    {
                        return (hex - 'a' + 10);
                    }

                    return 0;
                };

                return (byte)(HexCharToDec(High) * 16 + HexCharToDec(Low));
            }

            return 0;
        }

        //

        private double _Opacity; // 颜色的不透明度。

        private double _Alpha; // 颜色在 RGB 色彩空间中的 Alpha 通道（A）的值。
        private double _Red; // 颜色在 RGB 色彩空间中的红色通道（R）的值。
        private double _Green; // 颜色在 RGB 色彩空间中的绿色通道（G）的值。
        private double _Blue; // 颜色在 RGB 色彩空间中的蓝色通道（B）的值。

        private double _Hue_HSV; // 颜色在 HSV 色彩空间中的色相（H）。
        private double _Saturation_HSV; // 颜色在 HSV 色彩空间中的饱和度（S）。
        private double _Brightness; // 颜色在 HSV 色彩空间中的明度（V）。

        private double _Hue_HSL; // 颜色在 HSL 色彩空间中的色相（H）。
        private double _Saturation_HSL; // 颜色在 HSL 色彩空间中的饱和度（S）。
        private double _Lightness_HSL; // 颜色在 HSL 色彩空间中的明度（L）。

        private double _Cyan; // 颜色在 CMYK 色彩空间中的青色通道（C）的值。
        private double _Magenta; // 颜色在 CMYK 色彩空间中的洋红色通道（M）的值。
        private double _Yellow; // 颜色在 CMYK 色彩空间中的黄色通道（Y）的值。
        private double _Black; // 颜色在 CMYK 色彩空间中的黑色通道（K）的值。

        private double _Lightness_LAB; // 颜色在 LAB 色彩空间中的明度（L）。
        private double _GreenRed; // 颜色在 LAB 色彩空间中的绿色-红色通道（A）的值。   
        private double _BlueYellow; // 颜色在 LAB 色彩空间中的蓝色-黄色通道（B）的值。

        //

        private void _CtorRGB(double alpha, double red, double green, double blue) // 为以颜色在 RGB 色彩空间中的各分量为参数的构造函数提供实现。
        {
            _Alpha = _CheckAlpha(alpha);
            _Red = _CheckRed(red);
            _Green = _CheckGreen(green);
            _Blue = _CheckBlue(blue);

            _AlphaToOpacity(_Alpha, out _Opacity);
            _Opacity = _CheckOpacity(_Opacity);

            _RGBToHSV(_Red, _Green, _Blue, out _Hue_HSV, out _Saturation_HSV, out _Brightness);
            _Hue_HSV = _CheckHue_HSV(_Hue_HSV);
            _Saturation_HSV = _CheckSaturation_HSV(_Saturation_HSV);
            _Brightness = _CheckBrightness(_Brightness);

            _RGBToHSL(_Red, _Green, _Blue, out _Hue_HSL, out _Saturation_HSL, out _Lightness_HSL);
            _Hue_HSL = _CheckHue_HSL(_Hue_HSL);
            _Saturation_HSL = _CheckSaturation_HSL(_Saturation_HSL);
            _Lightness_HSL = _CheckLightness_HSL(_Lightness_HSL);

            _RGBToCMYK(_Red, _Green, _Blue, out _Cyan, out _Magenta, out _Yellow, out _Black);
            _Cyan = _CheckCyan(_Cyan);
            _Magenta = _CheckMagenta(_Magenta);
            _Yellow = _CheckYellow(_Yellow);
            _Black = _CheckBlack(_Black);

            _RGBToLAB(_Red, _Green, _Blue, out _Lightness_LAB, out _GreenRed, out _BlueYellow);
            _Lightness_LAB = _CheckLightness_LAB(_Lightness_LAB);
            _GreenRed = _CheckGreenRed(_GreenRed);
            _BlueYellow = _CheckBlueYellow(_BlueYellow);
        }

        private void _CtorHSV(double hue, double saturation, double brightness, double opacity) // 为以颜色在 HSV 色彩空间中的各分量为参数的构造函数提供实现。
        {
            _Opacity = _CheckOpacity(opacity);
            _Hue_HSV = _CheckHue_HSV(hue);
            _Saturation_HSV = _CheckSaturation_HSV(saturation);
            _Brightness = _CheckBrightness(brightness);

            _OpacityToAlpha(_Opacity, out _Alpha);
            _Alpha = _CheckAlpha(_Alpha);

            _HSVToRGB(_Hue_HSV, _Saturation_HSV, _Brightness, out _Red, out _Green, out _Blue);
            _Red = _CheckRed(_Red);
            _Green = _CheckGreen(_Green);
            _Blue = _CheckBlue(_Blue);

            _RGBToHSL(_Red, _Green, _Blue, out _Hue_HSL, out _Saturation_HSL, out _Lightness_HSL);
            _Hue_HSL = _CheckHue_HSL(_Hue_HSL);
            _Saturation_HSL = _CheckSaturation_HSL(_Saturation_HSL);
            _Lightness_HSL = _CheckLightness_HSL(_Lightness_HSL);

            _RGBToCMYK(_Red, _Green, _Blue, out _Cyan, out _Magenta, out _Yellow, out _Black);
            _Cyan = _CheckCyan(_Cyan);
            _Magenta = _CheckMagenta(_Magenta);
            _Yellow = _CheckYellow(_Yellow);
            _Black = _CheckBlack(_Black);

            _RGBToLAB(_Red, _Green, _Blue, out _Lightness_LAB, out _GreenRed, out _BlueYellow);
            _Lightness_LAB = _CheckLightness_LAB(_Lightness_LAB);
            _GreenRed = _CheckGreenRed(_GreenRed);
            _BlueYellow = _CheckBlueYellow(_BlueYellow);
        }

        private void _CtorHSL(double hue, double saturation, double lightness, double opacity) // 为以颜色在 HSL 色彩空间中的各分量为参数的构造函数提供实现。
        {
            _Opacity = _CheckOpacity(opacity);
            _Hue_HSL = _CheckHue_HSL(hue);
            _Saturation_HSL = _CheckSaturation_HSL(saturation);
            _Lightness_HSL = _CheckLightness_HSL(lightness);

            _OpacityToAlpha(_Opacity, out _Alpha);
            _Alpha = _CheckAlpha(_Alpha);

            _HSLToRGB(_Hue_HSL, _Saturation_HSL, _Lightness_HSL, out _Red, out _Green, out _Blue);
            _Red = _CheckRed(_Red);
            _Green = _CheckGreen(_Green);
            _Blue = _CheckBlue(_Blue);

            _RGBToHSV(_Red, _Green, _Blue, out _Hue_HSV, out _Saturation_HSV, out _Brightness);
            _Hue_HSV = _CheckHue_HSV(_Hue_HSV);
            _Saturation_HSV = _CheckSaturation_HSV(_Saturation_HSV);
            _Brightness = _CheckBrightness(_Brightness);

            _RGBToCMYK(_Red, _Green, _Blue, out _Cyan, out _Magenta, out _Yellow, out _Black);
            _Cyan = _CheckCyan(_Cyan);
            _Magenta = _CheckMagenta(_Magenta);
            _Yellow = _CheckYellow(_Yellow);
            _Black = _CheckBlack(_Black);

            _RGBToLAB(_Red, _Green, _Blue, out _Lightness_LAB, out _GreenRed, out _BlueYellow);
            _Lightness_LAB = _CheckLightness_LAB(_Lightness_LAB);
            _GreenRed = _CheckGreenRed(_GreenRed);
            _BlueYellow = _CheckBlueYellow(_BlueYellow);
        }

        private void _CtorCMYK(double cyan, double magenta, double yellow, double black, double opacity) // 为以颜色在 CMYK 色彩空间中的各分量为参数的构造函数提供实现。
        {
            _Opacity = _CheckOpacity(opacity);
            _Cyan = _CheckCyan(cyan);
            _Magenta = _CheckMagenta(magenta);
            _Yellow = _CheckYellow(yellow);
            _Black = _CheckBlack(black);

            _OpacityToAlpha(_Opacity, out _Alpha);
            _Alpha = _CheckAlpha(_Alpha);

            _CMYKToRGB(_Cyan, _Magenta, _Yellow, _Black, out _Red, out _Green, out _Blue);
            _Red = _CheckRed(_Red);
            _Green = _CheckGreen(_Green);
            _Blue = _CheckBlue(_Blue);

            _RGBToHSV(_Red, _Green, _Blue, out _Hue_HSV, out _Saturation_HSV, out _Brightness);
            _Hue_HSV = _CheckHue_HSV(_Hue_HSV);
            _Saturation_HSV = _CheckSaturation_HSV(_Saturation_HSV);
            _Brightness = _CheckBrightness(_Brightness);

            _RGBToHSL(_Red, _Green, _Blue, out _Hue_HSL, out _Saturation_HSL, out _Lightness_HSL);
            _Hue_HSL = _CheckHue_HSL(_Hue_HSL);
            _Saturation_HSL = _CheckSaturation_HSL(_Saturation_HSL);
            _Lightness_HSL = _CheckLightness_HSL(_Lightness_HSL);

            _RGBToLAB(_Red, _Green, _Blue, out _Lightness_LAB, out _GreenRed, out _BlueYellow);
            _Lightness_LAB = _CheckLightness_LAB(_Lightness_LAB);
            _GreenRed = _CheckGreenRed(_GreenRed);
            _BlueYellow = _CheckBlueYellow(_BlueYellow);
        }

        private void _CtorLAB(double lightness, double greenRed, double blueYellow, double opacity) // 为以颜色在 LAB 色彩空间中的各分量为参数的构造函数提供实现。
        {
            _Opacity = _CheckOpacity(opacity);
            _Lightness_LAB = _CheckLightness_LAB(lightness);
            _GreenRed = _CheckGreenRed(greenRed);
            _BlueYellow = _CheckBlueYellow(blueYellow);

            _OpacityToAlpha(_Opacity, out _Alpha);
            _Alpha = _CheckAlpha(_Alpha);

            _LABToRGB(_Lightness_LAB, _GreenRed, _BlueYellow, out _Red, out _Green, out _Blue);
            _Red = _CheckRed(_Red);
            _Green = _CheckGreen(_Green);
            _Blue = _CheckBlue(_Blue);

            _RGBToHSV(_Red, _Green, _Blue, out _Hue_HSV, out _Saturation_HSV, out _Brightness);
            _Hue_HSV = _CheckHue_HSV(_Hue_HSV);
            _Saturation_HSV = _CheckSaturation_HSV(_Saturation_HSV);
            _Brightness = _CheckBrightness(_Brightness);

            _RGBToHSL(_Red, _Green, _Blue, out _Hue_HSL, out _Saturation_HSL, out _Lightness_HSL);
            _Hue_HSL = _CheckHue_HSL(_Hue_HSL);
            _Saturation_HSL = _CheckSaturation_HSL(_Saturation_HSL);
            _Lightness_HSL = _CheckLightness_HSL(_Lightness_HSL);

            _RGBToCMYK(_Red, _Green, _Blue, out _Cyan, out _Magenta, out _Yellow, out _Black);
            _Cyan = _CheckCyan(_Cyan);
            _Magenta = _CheckMagenta(_Magenta);
            _Yellow = _CheckYellow(_Yellow);
            _Black = _CheckBlack(_Black);
        }

        #endregion

        #region 常量与只读成员

        /// <summary>
        /// 表示所有属性为其数据类型的默认值的 ColorX 结构的实例。
        /// </summary>
        public static readonly ColorX Empty = default(ColorX);

        /// <summary>
        /// 表示透明色的 ColorX 结构的实例。
        /// </summary>
        public static readonly ColorX Transparent = FromRGB(_MinAlpha, _MaxRed, _MaxGreen, _MaxBlue);

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用 Color 结构初始化 ColorX 结构的新实例。
        /// </summary>
        /// <param name="color">Color 结构。</param>
        public ColorX(Color color)
        {
            this = default(ColorX);

            _CtorRGB(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// 使用颜色在 RGB 色彩空间中的 32 位 ARGB 值初始化 ColorX 结构的新实例。
        /// </summary>
        /// <param name="argb">颜色在 RGB 色彩空间中的 32 位 ARGB 值。</param>
        public ColorX(int argb)
        {
            this = default(ColorX);

            Color color = Color.FromArgb(argb);

            _CtorRGB(color.A, color.R, color.G, color.B);
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取表示此 ColorX 结构是否为 Empty 的布尔值。
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (_Opacity == Empty._Opacity && (_Alpha == Empty._Alpha && _Red == Empty._Red && _Green == Empty._Green && _Blue == Empty._Blue) && (_Hue_HSV == Empty._Hue_HSV && _Saturation_HSV == Empty._Saturation_HSV && _Brightness == Empty._Brightness) && (_Hue_HSL == Empty._Hue_HSL && _Saturation_HSL == Empty._Saturation_HSL && _Lightness_HSL == Empty._Lightness_HSL) && (_Cyan == Empty._Cyan && _Magenta == Empty._Magenta && _Yellow == Empty._Yellow && _Black == Empty._Black) && (_Lightness_LAB == Empty._Lightness_LAB && _GreenRed == Empty._GreenRed && _BlueYellow == Empty._BlueYellow));
            }
        }

        /// <summary>
        /// 获取表示此 ColorX 结构是否为透明色的布尔值。
        /// </summary>
        public bool IsTransparent
        {
            get
            {
                return (_Opacity == Transparent._Opacity);
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
                return _CheckOpacity(_Opacity);
            }

            set
            {
                _Opacity = _CheckOpacity(value);

                _OpacityToAlpha(_Opacity, out _Alpha);

                _Alpha = _CheckAlpha(_Alpha);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构在 RGB 色彩空间中的 Alpha 通道（A）的值。
        /// </summary>
        public double Alpha
        {
            get
            {
                return _CheckAlpha(_Alpha);
            }

            set
            {
                _Alpha = _CheckAlpha(value);

                _AlphaToOpacity(_Alpha, out _Opacity);

                _Opacity = _CheckOpacity(_Opacity);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 RGB 色彩空间中的红色通道（R）的值。
        /// </summary>
        public double Red
        {
            get
            {
                return _CheckRed(_Red);
            }

            set
            {
                _CtorRGB(_Alpha, value, _Green, _Blue);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 RGB 色彩空间中的绿色通道（G）的值。
        /// </summary>
        public double Green
        {
            get
            {
                return _CheckGreen(_Green);
            }

            set
            {
                _CtorRGB(_Alpha, _Red, value, _Blue);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 RGB 色彩空间中的蓝色通道（B）的值。
        /// </summary>
        public double Blue
        {
            get
            {
                return _CheckBlue(_Blue);
            }

            set
            {
                _CtorRGB(_Alpha, _Red, _Green, value);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSV 色彩空间中的色相（H）。
        /// </summary>
        public double Hue_HSV
        {
            get
            {
                return _CheckHue_HSV(_Hue_HSV);
            }

            set
            {
                _CtorHSV(value, _Saturation_HSV, _Brightness, _Opacity);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSV 色彩空间中的饱和度（S）。
        /// </summary>
        public double Saturation_HSV
        {
            get
            {
                return _CheckSaturation_HSV(_Saturation_HSV);
            }

            set
            {
                _CtorHSV(_Hue_HSV, value, _Brightness, _Opacity);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSV 色彩空间中的明度（V）。
        /// </summary>
        public double Brightness
        {
            get
            {
                return _CheckBrightness(_Brightness);
            }

            set
            {
                _CtorHSV(_Hue_HSV, _Saturation_HSV, value, _Opacity);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSL 色彩空间中的色相（H）。
        /// </summary>
        public double Hue_HSL
        {
            get
            {
                return _CheckHue_HSL(_Hue_HSL);
            }

            set
            {
                _CtorHSL(value, _Saturation_HSL, _Lightness_HSL, _Opacity);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSL 色彩空间中的饱和度（S）。
        /// </summary>
        public double Saturation_HSL
        {
            get
            {
                return _CheckSaturation_HSL(_Saturation_HSL);
            }

            set
            {
                _CtorHSL(_Hue_HSL, value, _Lightness_HSL, _Opacity);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSL 色彩空间中的明度（L）。
        /// </summary>
        public double Lightness_HSL
        {
            get
            {
                return _CheckLightness_HSL(_Lightness_HSL);
            }

            set
            {
                _CtorHSL(_Hue_HSL, _Saturation_HSL, value, _Opacity);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构在 CMYK 色彩空间中的青色通道（C）的值。
        /// </summary>
        public double Cyan
        {
            get
            {
                return _CheckCyan(_Cyan);
            }

            set
            {
                _CtorCMYK(value, _Magenta, _Yellow, _Black, _Opacity);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 CMYK 色彩空间中的洋红色通道（M）的值。
        /// </summary>
        public double Magenta
        {
            get
            {
                return _CheckMagenta(_Magenta);
            }

            set
            {
                _CtorCMYK(_Cyan, value, _Yellow, _Black, _Opacity);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 CMYK 色彩空间中的黄色通道（Y）的值。
        /// </summary>
        public double Yellow
        {
            get
            {
                return _CheckYellow(_Yellow);
            }

            set
            {
                _CtorCMYK(_Cyan, _Magenta, value, _Black, _Opacity);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 CMYK 色彩空间中的黑色通道（K）的值。
        /// </summary>
        public double Black
        {
            get
            {
                if (_CheckRed(_Red) == 0 && _CheckGreen(_Green) == 0 && _CheckBlue(_Blue) == 0)
                {
                    return _MaxBlack;
                }

                return _CheckBlack(_Black);
            }

            set
            {
                _CtorCMYK(_Cyan, _Magenta, _Yellow, value, _Opacity);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构在 LAB 色彩空间中的明度（L）。
        /// </summary>
        public double Lightness_LAB
        {
            get
            {
                return _CheckLightness_LAB(_Lightness_LAB);
            }

            set
            {
                _CtorLAB(value, _GreenRed, _BlueYellow, _Opacity);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 LAB 色彩空间中的绿色-红色通道（A）的值。
        /// </summary>
        public double GreenRed
        {
            get
            {
                return _CheckGreenRed(_GreenRed);
            }

            set
            {
                _CtorLAB(_Lightness_LAB, value, _BlueYellow, _Opacity);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 LAB 色彩空间中的蓝色-黄色通道（B）的值。
        /// </summary>
        public double BlueYellow
        {
            get
            {
                return _CheckBlueYellow(_BlueYellow);
            }

            set
            {
                _CtorLAB(_Lightness_LAB, _GreenRed, value, _Opacity);
            }
        }

        //

        /// <summary>
        /// 获取或设置表示此 ColorX 结构在 RGB 色彩空间中的各分量的 PointD3D 结构。
        /// </summary>
        public PointD3D RGB
        {
            get
            {
                return new PointD3D(_CheckRed(_Red), _CheckGreen(_Green), _CheckBlue(_Blue));
            }

            set
            {
                _CtorRGB(_Alpha, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// 获取或设置表示此 ColorX 结构在 HSV 色彩空间中的各分量的 PointD3D 结构。
        /// </summary>
        public PointD3D HSV
        {
            get
            {
                return new PointD3D(_CheckHue_HSV(_Hue_HSV), _CheckSaturation_HSV(_Saturation_HSV), _CheckBrightness(_Brightness));
            }

            set
            {
                _CtorHSV(value.X, value.Y, value.Z, _Opacity);
            }
        }

        /// <summary>
        /// 获取或设置表示此 ColorX 结构在 HSL 色彩空间中的各分量的 PointD3D 结构。
        /// </summary>
        public PointD3D HSL
        {
            get
            {
                return new PointD3D(_CheckHue_HSL(_Hue_HSL), _CheckSaturation_HSL(_Saturation_HSL), _CheckLightness_HSL(_Lightness_HSL));
            }

            set
            {
                _CtorHSL(value.X, value.Y, value.Z, _Opacity);
            }
        }

        /// <summary>
        /// 获取或设置表示此 ColorX 结构在 CMYK 色彩空间中的各分量的 PointD4D 结构。
        /// </summary>
        public PointD4D CMYK
        {
            get
            {
                return new PointD4D(_CheckCyan(_Cyan), _CheckMagenta(_Magenta), _CheckYellow(_Yellow), _CheckBlack(_Black));
            }

            set
            {
                _CtorCMYK(value.X, value.Y, value.Z, value.U, _Opacity);
            }
        }

        /// <summary>
        /// 获取或设置表示此 ColorX 结构在 LAB 色彩空间中的各分量的 PointD3D 结构。
        /// </summary>
        public PointD3D LAB
        {
            get
            {
                return new PointD3D(_CheckLightness_LAB(_Lightness_LAB), _CheckGreenRed(_GreenRed), _CheckBlueYellow(_BlueYellow));
            }

            set
            {
                _CtorLAB(value.X, value.Y, value.Z, _Opacity);
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
                ColorX color = default(ColorX);

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
                ColorX color = default(ColorX);

                double Y = 0.2126 * _CheckRed(_Red) + 0.7152 * _CheckGreen(_Green) + 0.0722 * _CheckBlue(_Blue);

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
                return string.Concat("#", _ByteToHexString((byte)Math.Round(_CheckAlpha(_Alpha))), _ByteToHexString((byte)Math.Round(_CheckRed(_Red))), _ByteToHexString((byte)Math.Round(_CheckGreen(_Green))), _ByteToHexString((byte)Math.Round(_CheckBlue(_Blue))));
            }
        }

        /// <summary>
        /// 获取此 ColorX 结构的 16 进制 RGB 码。
        /// </summary>
        public string RGBHexCode
        {
            get
            {
                return string.Concat("#", _ByteToHexString((byte)Math.Round(_CheckRed(_Red))), _ByteToHexString((byte)Math.Round(_CheckGreen(_Green))), _ByteToHexString((byte)Math.Round(_CheckBlue(_Blue))));
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 ColorX 结构是否与指定的 ColorX 结构相等。
        /// </summary>
        /// <param name="color">用于比较的 ColorX 结构。</param>
        public bool Equals(ColorX color)
        {
            if ((object)color == null)
            {
                return false;
            }

            return (_Opacity == color._Opacity && (_Alpha == color._Alpha && _Red == color._Red && _Green == color._Green && _Blue == color._Blue) && (_Hue_HSV == color._Hue_HSV && _Saturation_HSV == color._Saturation_HSV && _Brightness == color._Brightness) && (_Hue_HSL == color._Hue_HSL && _Saturation_HSL == color._Saturation_HSL && _Lightness_HSL == color._Lightness_HSL) && (_Cyan == color._Cyan && _Magenta == color._Magenta && _Yellow == color._Yellow && _Black == color._Black) && (_Lightness_LAB == color._Lightness_LAB && _GreenRed == color._GreenRed && _BlueYellow == color._BlueYellow));
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构转换为 Color 结构的新实例。
        /// </summary>
        public Color ToColor()
        {
            if (IsEmpty)
            {
                return Color.Empty;
            }

            return Color.FromArgb((int)Math.Round(_CheckAlpha(_Alpha)), (int)Math.Round(_CheckRed(_Red)), (int)Math.Round(_CheckGreen(_Green)), (int)Math.Round(_CheckBlue(_Blue)));
        }

        /// <summary>
        /// 返回将此 ColorX 结构转换为 Color 结构的 32 位 ARGB 值。
        /// </summary>
        public int ToARGB()
        {
            return ToColor().ToArgb();
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构的不透明度更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="opacity">颜色的不透明度。</param>
        public ColorX AtOpacity(double opacity)
        {
            ColorX color = this;

            color.Opacity = opacity;

            return color;
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构在 RGB 色彩空间中的 Alpha 通道（A）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="alpha">颜色在 RGB 色彩空间中的 Alpha 通道（A）的值。</param>
        public ColorX AtAlpha(double alpha)
        {
            ColorX color = this;

            color.Alpha = alpha;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 RGB 色彩空间中的红色通道（R）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="red">颜色在 RGB 色彩空间中的红色通道（R）的值。</param>
        public ColorX AtRed(double red)
        {
            ColorX color = this;

            color.Red = red;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 RGB 色彩空间中的绿色通道（G）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="green">颜色在 RGB 色彩空间中的绿色通道（G）的值。</param>
        public ColorX AtGreen(double green)
        {
            ColorX color = this;

            color.Green = green;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 RGB 色彩空间中的蓝色通道（B）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="blue">颜色在 RGB 色彩空间中的蓝色通道（B）的值。</param>
        public ColorX AtBlue(double blue)
        {
            ColorX color = this;

            color.Blue = blue;

            return color;
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构在 HSV 色彩空间中的色相（H）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSV 色彩空间中的色相（H）。</param>
        public ColorX AtHue_HSV(double hue)
        {
            ColorX color = this;

            color.Hue_HSV = hue;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 HSV 色彩空间中的饱和度（S）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="saturation">颜色在 HSV 色彩空间中的饱和度（S）。</param>
        public ColorX AtSaturation_HSV(double saturation)
        {
            ColorX color = this;

            color.Saturation_HSV = saturation;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 HSV 色彩空间中的明度（V）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="brightness">颜色在 HSV 色彩空间中的明度（V）。</param>
        public ColorX AtBrightness(double brightness)
        {
            ColorX color = this;

            color.Brightness = brightness;

            return color;
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构在 HSL 色彩空间中的色相（H）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSL 色彩空间中的色相（H）。</param>
        public ColorX AtHue_HSL(double hue)
        {
            ColorX color = this;

            color.Hue_HSL = hue;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 HSL 色彩空间中的饱和度（S）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="saturation">颜色在 HSL 色彩空间中的饱和度（S）。</param>
        public ColorX AtSaturation_HSL(double saturation)
        {
            ColorX color = this;

            color.Saturation_HSL = saturation;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 HSL 色彩空间中的明度（L）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lightness">颜色在 HSL 色彩空间中的明度（L）。</param>
        public ColorX AtLightness_HSL(double lightness)
        {
            ColorX color = this;

            color.Lightness_HSL = lightness;

            return color;
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构在 CMYK 色彩空间中的青色通道（C）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="cyan">颜色在 CMYK 色彩空间中的青色通道（C）的值。</param>
        public ColorX AtCyan(double cyan)
        {
            ColorX color = this;

            color.Cyan = cyan;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 CMYK 色彩空间中的洋红色通道（M）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="magenta">颜色在 CMYK 色彩空间中的洋红色通道（M）的值。</param>
        public ColorX AtMagenta(double magenta)
        {
            ColorX color = this;

            color.Magenta = magenta;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 CMYK 色彩空间中的黄色通道（Y）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="yellow">颜色在 CMYK 色彩空间中的黄色通道（Y）的值。</param>
        public ColorX AtYellow(double yellow)
        {
            ColorX color = this;

            color.Yellow = yellow;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 CMYK 色彩空间中的黑色通道（K）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="black">颜色在 CMYK 色彩空间中的黑色通道（K）的值。</param>
        public ColorX AtBlack(double black)
        {
            ColorX color = this;

            color.Black = black;

            return color;
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构在 LAB 色彩空间中的明度（L）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lightness">颜色在 LAB 色彩空间中的明度（L）。</param>
        public ColorX AtLightness_LAB(double lightness)
        {
            ColorX color = this;

            color.Lightness_LAB = lightness;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 LAB 色彩空间中的绿色-红色通道（A）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="greenRed">颜色在 LAB 色彩空间中的绿色-红色通道（A）的值。</param>
        public ColorX AtGreenRed(double greenRed)
        {
            ColorX color = this;

            color.GreenRed = greenRed;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 LAB 色彩空间中的蓝色-黄色通道（B）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="blueYellow">颜色在 LAB 色彩空间中的蓝色-黄色通道（B）的值。</param>
        public ColorX AtBlueYellow(double blueYellow)
        {
            ColorX color = this;

            color.BlueYellow = blueYellow;

            return color;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 返回将 Color 结构转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="alpha">Alpha 通道（A）的值。</param>
        /// <param name="color">Color 结构。</param>
        public static ColorX FromColor(int alpha, Color color)
        {
            return new ColorX(Color.FromArgb((int)Math.Round(_CheckAlpha(alpha)), color));
        }

        /// <summary>
        /// 返回将 Color 结构转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="color">Color 结构。</param>
        public static ColorX FromColor(Color color)
        {
            return new ColorX(color);
        }

        //

        /// <summary>
        /// 返回将颜色在 RGB 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="alpha">颜色在 RGB 色彩空间中的 Alpha 通道（A）的值。</param>
        /// <param name="red">颜色在 RGB 色彩空间中的红色通道（R）的值。</param>
        /// <param name="green">颜色在 RGB 色彩空间中的绿色通道（G）的值。</param>
        /// <param name="blue">颜色在 RGB 色彩空间中的蓝色通道（B）的值。</param>
        public static ColorX FromRGB(double alpha, double red, double green, double blue)
        {
            ColorX color = default(ColorX);

            color._CtorRGB(alpha, red, green, blue);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 RGB 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="red">颜色在 RGB 色彩空间中的红色通道（R）的值。</param>
        /// <param name="green">颜色在 RGB 色彩空间中的绿色通道（G）的值。</param>
        /// <param name="blue">颜色在 RGB 色彩空间中的蓝色通道（B）的值。</param>
        public static ColorX FromRGB(double red, double green, double blue)
        {
            ColorX color = default(ColorX);

            color._CtorRGB(_MaxAlpha, red, green, blue);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 RGB 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="alpha">颜色在 RGB 色彩空间中的 Alpha 通道（A）的值。</param>
        /// <param name="rgb">表示颜色在 RGB 色彩空间中的各分量的 PointD3D 结构。</param>
        public static ColorX FromRGB(double alpha, PointD3D rgb)
        {
            ColorX color = default(ColorX);

            color._CtorRGB(alpha, rgb.X, rgb.Y, rgb.Z);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 RGB 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="rgb">表示颜色在 RGB 色彩空间中的各分量的 PointD3D 结构。</param>
        public static ColorX FromRGB(PointD3D rgb)
        {
            ColorX color = default(ColorX);

            color._CtorRGB(_MaxAlpha, rgb.X, rgb.Y, rgb.Z);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 RGB 色彩空间中的 32 位 ARGB 值转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="argb">颜色在 RGB 色彩空间中的 32 位 ARGB 值。</param>
        public static ColorX FromRGB(int argb)
        {
            return new ColorX(argb);
        }

        //

        /// <summary>
        /// 返回将颜色在 HSV 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSV 色彩空间中的色相（H）。</param>
        /// <param name="saturation">颜色在 HSV 色彩空间中的饱和度（S）。</param>
        /// <param name="brightness">颜色在 HSV 色彩空间中的明度（V）。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        public static ColorX FromHSV(double hue, double saturation, double brightness, double opacity)
        {
            ColorX color = default(ColorX);

            color._CtorHSV(hue, saturation, brightness, opacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSV 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSV 色彩空间中的色相（H）。</param>
        /// <param name="saturation">颜色在 HSV 色彩空间中的饱和度（S）。</param>
        /// <param name="brightness">颜色在 HSV 色彩空间中的明度（V）。</param>
        public static ColorX FromHSV(double hue, double saturation, double brightness)
        {
            ColorX color = default(ColorX);

            color._CtorHSV(hue, saturation, brightness, _MaxOpacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSV 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hsv">表示颜色在 HSV 色彩空间中的各分量的 PointD3D 结构。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        public static ColorX FromHSV(PointD3D hsv, double opacity)
        {
            ColorX color = default(ColorX);

            color._CtorHSV(hsv.X, hsv.Y, hsv.Z, opacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSV 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hsv">表示颜色在 HSV 色彩空间中的各分量的 PointD3D 结构。</param>
        public static ColorX FromHSV(PointD3D hsv)
        {
            ColorX color = default(ColorX);

            color._CtorHSV(hsv.X, hsv.Y, hsv.Z, _MaxOpacity);

            return color;
        }

        //

        /// <summary>
        /// 返回将颜色在 HSL 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSL 色彩空间中的色相（H）。</param>
        /// <param name="saturation">颜色在 HSL 色彩空间中的饱和度（S）。</param>
        /// <param name="lightness">颜色在 HSL 色彩空间中的明度（L）。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        public static ColorX FromHSL(double hue, double saturation, double lightness, double opacity)
        {
            ColorX color = default(ColorX);

            color._CtorHSL(hue, saturation, lightness, opacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSL 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSL 色彩空间中的色相（H）。</param>
        /// <param name="saturation">颜色在 HSL 色彩空间中的饱和度（S）。</param>
        /// <param name="lightness">颜色在 HSL 色彩空间中的明度（L）。</param>
        public static ColorX FromHSL(double hue, double saturation, double lightness)
        {
            ColorX color = default(ColorX);

            color._CtorHSL(hue, saturation, lightness, _MaxOpacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSL 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hsl">表示颜色在 HSL 色彩空间中的各分量的 PointD3D 结构。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        public static ColorX FromHSL(PointD3D hsl, double opacity)
        {
            ColorX color = default(ColorX);

            color._CtorHSL(hsl.X, hsl.Y, hsl.Z, opacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSL 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hsl">表示颜色在 HSL 色彩空间中的各分量的 PointD3D 结构。</param>
        public static ColorX FromHSL(PointD3D hsl)
        {
            ColorX color = default(ColorX);

            color._CtorHSL(hsl.X, hsl.Y, hsl.Z, _MaxOpacity);

            return color;
        }

        //

        /// <summary>
        /// 返回将颜色在 CMYK 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="cyan">颜色在 CMYK 色彩空间中的青色通道（C）的值。</param>
        /// <param name="magenta">颜色在 CMYK 色彩空间中的洋红色通道（M）的值。</param>
        /// <param name="yellow">颜色在 CMYK 色彩空间中的黄色通道（Y）的值。</param>
        /// <param name="black">颜色在 CMYK 色彩空间中的黑色通道（K）的值。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        public static ColorX FromCMYK(double cyan, double magenta, double yellow, double black, double opacity)
        {
            ColorX color = default(ColorX);

            color._CtorCMYK(cyan, magenta, yellow, black, opacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 CMYK 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="cyan">颜色在 CMYK 色彩空间中的青色通道（C）的值。</param>
        /// <param name="magenta">颜色在 CMYK 色彩空间中的洋红色通道（M）的值。</param>
        /// <param name="yellow">颜色在 CMYK 色彩空间中的黄色通道（Y）的值。</param>
        /// <param name="black">颜色在 CMYK 色彩空间中的黑色通道（K）的值。</param>
        public static ColorX FromCMYK(double cyan, double magenta, double yellow, double black)
        {
            ColorX color = default(ColorX);

            color._CtorCMYK(cyan, magenta, yellow, black, _MaxOpacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 CMYK 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="cmyk">表示颜色在 CMYK 色彩空间中的各分量的 PointD4D 结构。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        public static ColorX FromCMYK(PointD4D cmyk, double opacity)
        {
            ColorX color = default(ColorX);

            color._CtorCMYK(cmyk.X, cmyk.Y, cmyk.Z, cmyk.U, opacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 CMYK 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="cmyk">表示颜色在 CMYK 色彩空间中的各分量的 PointD4D 结构。</param>
        public static ColorX FromCMYK(PointD4D cmyk)
        {
            ColorX color = default(ColorX);

            color._CtorCMYK(cmyk.X, cmyk.Y, cmyk.Z, cmyk.U, _MaxOpacity);

            return color;
        }

        //

        /// <summary>
        /// 返回将颜色在 LAB 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lightness">颜色在 LAB 色彩空间中的明度（L）。</param>
        /// <param name="greenRed">颜色在 LAB 色彩空间中的绿色-红色通道（A）的值。</param>
        /// <param name="blueYellow">颜色在 LAB 色彩空间中的蓝色-黄色通道（B）的值。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        public static ColorX FromLAB(double lightness, double greenRed, double blueYellow, double opacity)
        {
            ColorX color = default(ColorX);

            color._CtorLAB(lightness, greenRed, blueYellow, opacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 LAB 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lightness">颜色在 LAB 色彩空间中的明度（L）。</param>
        /// <param name="greenRed">颜色在 LAB 色彩空间中的绿色-红色通道（A）的值。</param>
        /// <param name="blueYellow">颜色在 LAB 色彩空间中的蓝色-黄色通道（B）的值。</param>
        public static ColorX FromLAB(double lightness, double greenRed, double blueYellow)
        {
            ColorX color = default(ColorX);

            color._CtorLAB(lightness, greenRed, blueYellow, _MaxOpacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 LAB 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lab">表示颜色在 LAB 色彩空间中的各分量的 PointD3D 结构。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        public static ColorX FromLAB(PointD3D lab, double opacity)
        {
            ColorX color = default(ColorX);

            color._CtorLAB(lab.X, lab.Y, lab.Z, opacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 LAB 色彩空间中的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lab">表示颜色在 LAB 色彩空间中的各分量的 PointD3D 结构。</param>
        public static ColorX FromLAB(PointD3D lab)
        {
            ColorX color = default(ColorX);

            color._CtorLAB(lab.X, lab.Y, lab.Z, _MaxOpacity);

            return color;
        }

        //

        /// <summary>
        /// 返回将颜色的 16 进制 ARGB 码或 RGB 码转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hexCode">表示颜色的 16 进制 ARGB 码或 RGB 码的字符串。</param>
        public static ColorX FromHexCode(string hexCode)
        {
            if (!string.IsNullOrEmpty(hexCode))
            {
                string HexCode = new Regex(@"[^A-Fa-f\d]").Replace(hexCode, string.Empty).ToUpper();

                if (HexCode.Length == 8)
                {
                    string A = HexCode.Substring(0, 2), R = HexCode.Substring(2, 2), G = HexCode.Substring(4, 2), B = HexCode.Substring(6, 2);

                    return FromRGB(_HexStringToByte(A), _HexStringToByte(R), _HexStringToByte(G), _HexStringToByte(B));
                }
                else if (HexCode.Length == 6)
                {
                    string R = HexCode.Substring(0, 2), G = HexCode.Substring(2, 2), B = HexCode.Substring(4, 2);

                    return FromRGB(_HexStringToByte(R), _HexStringToByte(G), _HexStringToByte(B));
                }
            }

            return Empty;
        }

        //

        /// <summary>
        /// 返回一个不透明度为 100%，其他分量为随机数的 ColorX 结构的新实例。
        /// </summary>
        public static ColorX RandomColor()
        {
            return FromRGB(Statistics.RandomDouble(_MinRed, _MaxRed), Statistics.RandomDouble(_MinGreen, _MaxGreen), Statistics.RandomDouble(_MinBlue, _MaxBlue));
        }

        #endregion

        #region 基类方法

        /// <summary>
        /// 判断此 ColorX 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ColorX))
            {
                return false;
            }

            return Equals((ColorX)obj);
        }

        /// <summary>
        /// 返回此 ColorX 结构的哈希代码。
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 ColorX 结构转换为字符串。
        /// </summary>
        public override string ToString()
        {
            string Str = string.Empty;

            if (IsEmpty)
            {
                Str = "Empty";
            }
            else
            {
                Str = string.Concat("A=", Alpha, ", R=", Red, ", G=", Green, ", B=", Blue);
            }

            return string.Concat(base.GetType().Name, " [", Str, "]");
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 ColorX 结构是否表示相同的颜色。
        /// </summary>
        /// <param name="left">运算符左侧比较的 ColorX 结构。</param>
        /// <param name="right">运算符右侧比较的 ColorX 结构。</param>
        public static bool operator ==(ColorX left, ColorX right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            else if (((object)left == null || left.IsEmpty) || ((object)right == null || right.IsEmpty))
            {
                return false;
            }

            return ((int)Math.Round(left.Alpha) == (int)Math.Round(right.Alpha) && ((int)Math.Round(left.Red) == (int)Math.Round(right.Red) && (int)Math.Round(left.Green) == (int)Math.Round(right.Green) && (int)Math.Round(left.Blue) == (int)Math.Round(right.Blue)));
        }

        /// <summary>
        /// 判断两个 ColorX 结构是否表示不同的颜色。
        /// </summary>
        /// <param name="left">运算符左侧比较的 ColorX 结构。</param>
        /// <param name="right">运算符右侧比较的 ColorX 结构。</param>
        public static bool operator !=(ColorX left, ColorX right)
        {
            if ((object)left == null && (object)right == null)
            {
                return false;
            }
            else if (((object)left == null || left.IsEmpty) || ((object)right == null || right.IsEmpty))
            {
                return true;
            }

            return ((int)Math.Round(left.Alpha) != (int)Math.Round(right.Alpha) || ((int)Math.Round(left.Red) != (int)Math.Round(right.Red) || (int)Math.Round(left.Green) != (int)Math.Round(right.Green) || (int)Math.Round(left.Blue) != (int)Math.Round(right.Blue)));
        }

        #endregion
    }
}