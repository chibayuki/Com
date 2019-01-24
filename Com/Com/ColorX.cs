/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.ColorX
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

        private const double _MinOpacity = 0, _MaxOpacity = 100; // 不透明度的最小值与最大值。

        private const double _MinAlpha = 0, _MaxAlpha = 255; // RGB 色彩空间的 Alpha 通道（A）的最小值与最大值。
        private const double _MinRed = 0, _MaxRed = 255; // RGB 色彩空间的红色通道（R）的最小值与最大值。
        private const double _MinGreen = 0, _MaxGreen = 255; // RGB 色彩空间的绿色通道（G）的最小值与最大值。
        private const double _MinBlue = 0, _MaxBlue = 255; // RGB 色彩空间的蓝色通道（B）的最小值与最大值。

        private const double _MinHue_HSV = 0, _MaxHue_HSV = 360; // HSV 色彩空间的色相（H）的最小值与最大值。
        private const double _MinSaturation_HSV = 0, _MaxSaturation_HSV = 100; // HSV 色彩空间的饱和度（S）的最小值与最大值。
        private const double _MinBrightness = 0, _MaxBrightness = 100; // HSV 色彩空间的明度（V）的最小值与最大值。

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

        private static double _CheckOpacity(double opacity) // 对颜色的不透明度的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(opacity) || (opacity < _MinOpacity || opacity > _MaxOpacity))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return opacity;
        }

        private static double _CheckAlpha(double alpha) // 对颜色在 RGB 色彩空间的 Alpha 通道（A）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(alpha) || (alpha < _MinAlpha || alpha > _MaxAlpha))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return alpha;
        }

        private static double _CheckRed(double red) // 对颜色在 RGB 色彩空间的红色通道（R）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(red) || (red < _MinRed || red > _MaxRed))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return red;
        }

        private static double _CheckGreen(double green) // 对颜色在 RGB 色彩空间的绿色通道（G）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(green) || (green < _MinGreen || green > _MaxGreen))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return green;
        }

        private static double _CheckBlue(double blue) // 对颜色在 RGB 色彩空间的蓝色通道（B）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(blue) || (blue < _MinBlue || blue > _MaxBlue))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return blue;
        }

        private static double _CheckHue_HSV(double hue) // 对颜色在 HSV 色彩空间的色相（H）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(hue) || (hue < _MinHue_HSV || hue > _MaxHue_HSV))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return hue;
        }

        private static double _CheckSaturation_HSV(double saturation) // 对颜色在 HSV 色彩空间的饱和度（S）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(saturation) || (saturation < _MinSaturation_HSV || saturation > _MaxSaturation_HSV))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return saturation;
        }

        private static double _CheckBrightness(double brightness) // 对颜色在 HSV 色彩空间的明度（V）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(brightness) || (brightness < _MinBrightness || brightness > _MaxBrightness))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return brightness;
        }

        private static double _CheckHue_HSL(double hue) // 对颜色在 HSL 色彩空间的色相（H）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(hue) || (hue < _MinHue_HSL || hue > _MaxHue_HSL))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return hue;
        }

        private static double _CheckSaturation_HSL(double saturation) // 对颜色在 HSL 色彩空间的饱和度（S）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(saturation) || (saturation < _MinSaturation_HSL || saturation > _MaxSaturation_HSL))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return saturation;
        }

        private static double _CheckLightness_HSL(double lightness) // 对颜色在 HSL 色彩空间的明度（L）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(lightness) || (lightness < _MinLightness_HSL || lightness > _MaxLightness_HSL))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return lightness;
        }

        private static double _CheckCyan(double cyan) // 对颜色在 CMYK 色彩空间的青色通道（C）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(cyan) || (cyan < _MinCyan || cyan > _MaxCyan))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return cyan;
        }

        private static double _CheckMagenta(double magenta) // 对颜色在 CMYK 色彩空间的洋红色通道（M）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(magenta) || (magenta < _MinMagenta || magenta > _MaxMagenta))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return magenta;
        }

        private static double _CheckYellow(double yellow) // 对颜色在 CMYK 色彩空间的黄色通道（Y）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(yellow) || (yellow < _MinYellow || yellow > _MaxYellow))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return yellow;
        }

        private static double _CheckBlack(double black) // 对颜色在 CMYK 色彩空间的黑色通道（K）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(black) || (black < _MinBlack || black > _MaxBlack))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return black;
        }

        private static double _CheckLightness_LAB(double lightness) // 对颜色在 LAB 色彩空间的明度（L）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(lightness) || (lightness < _MinLightness_LAB || lightness > _MaxLightness_LAB))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return lightness;
        }

        private static double _CheckGreenRed(double greenRed) // 对颜色在 LAB 色彩空间的绿色-红色通道（A）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(greenRed) || (greenRed < _MinGreenRed || greenRed > _MaxGreenRed))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return greenRed;
        }

        private static double _CheckBlueYellow(double blueYellow) // 对颜色在 LAB 色彩空间的蓝色-黄色通道（B）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(blueYellow) || (blueYellow < _MinBlueYellow || blueYellow > _MaxBlueYellow))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return blueYellow;
        }

        //

        private static void _OpacityToAlpha(double opacity, out double alpha) // 将颜色的不透明度转换为在 RGB 色彩空间的 Alpha 通道（A）的值。此函数不对参数进行合法性检查，返回合法的值。
        {
            alpha = opacity / _MaxOpacity * _MaxAlpha;
        }

        private static void _AlphaToOpacity(double alpha, out double opacity) // 将颜色在 RGB 色彩空间的 Alpha 通道（A）的值转换为不透明度。此函数不对参数进行合法性检查，返回合法的值。
        {
            opacity = alpha / _MaxAlpha * _MaxOpacity;
        }

        private static void _RGBToHSV(double red, double green, double blue, out double hue, out double saturation, out double brightness) // 将颜色在 RGB 色彩空间的各分量转换为在 HSV 色彩空间的各分量。此函数不对参数进行合法性检查，返回合法的值。
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

        private static void _HSVToRGB(double hue, double saturation, double brightness, out double red, out double green, out double blue) // 将颜色在 HSV 色彩空间的各分量转换为在 RGB 色彩空间的各分量。此函数不对参数进行合法性检查，返回合法的值。
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

        private static void _RGBToHSL(double red, double green, double blue, out double hue, out double saturation, out double lightness) // 将颜色在 RGB 色彩空间的各分量转换为在 HSL 色彩空间的各分量。此函数不对参数进行合法性检查，返回合法的值。
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

        private static void _HSLToRGB(double hue, double saturation, double lightness, out double red, out double green, out double blue) // 将颜色在 HSL 色彩空间的各分量转换为在 RGB 色彩空间的各分量。此函数不对参数进行合法性检查，返回合法的值。
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

        private static void _RGBToCMYK(double red, double green, double blue, out double cyan, out double magenta, out double yellow, out double black) // 将颜色在 RGB 色彩空间的各分量转换为在 CMYK 色彩空间的各分量。此函数不对参数进行合法性检查，返回合法的值。
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

        private static void _CMYKToRGB(double cyan, double magenta, double yellow, double black, out double red, out double green, out double blue) // 将颜色在 CMYK 色彩空间的各分量转换为在 RGB 色彩空间的各分量。此函数不对参数进行合法性检查，返回合法的值。
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

        private static void _RGBToLAB(double red, double green, double blue, out double lightness, out double greenRed, out double blueYellow) // 将颜色在 RGB 色彩空间的各分量转换为在 LAB 色彩空间的各分量。此函数不对参数进行合法性检查，返回合法的值。
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

        private static void _LABToRGB(double lightness, double greenRed, double blueYellow, out double red, out double green, out double blue) // 将颜色在 LAB 色彩空间的各分量转换为在 RGB 色彩空间的各分量。此函数不对参数进行合法性检查，返回合法的值。
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

        private double _Opacity; // 颜色的不透明度。

        private double _Alpha; // 颜色在 RGB 色彩空间的 Alpha 通道（A）的值。
        private double _Red; // 颜色在 RGB 色彩空间的红色通道（R）的值。
        private double _Green; // 颜色在 RGB 色彩空间的绿色通道（G）的值。
        private double _Blue; // 颜色在 RGB 色彩空间的蓝色通道（B）的值。

        private double _Hue_HSV; // 颜色在 HSV 色彩空间的色相（H）。
        private double _Saturation_HSV; // 颜色在 HSV 色彩空间的饱和度（S）。
        private double _Brightness; // 颜色在 HSV 色彩空间的明度（V）。

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

        private void _CtorHSV(double hue, double saturation, double brightness, double opacity) // 为以颜色在 HSV 色彩空间的各分量为参数的构造函数提供实现。
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

        private void _CtorHSL(double hue, double saturation, double lightness, double opacity) // 为以颜色在 HSL 色彩空间的各分量为参数的构造函数提供实现。
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

        private void _CtorCMYK(double cyan, double magenta, double yellow, double black, double opacity) // 为以颜色在 CMYK 色彩空间的各分量为参数的构造函数提供实现。
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

        private void _CtorLAB(double lightness, double greenRed, double blueYellow, double opacity) // 为以颜色在 LAB 色彩空间的各分量为参数的构造函数提供实现。
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
        /// 使用颜色在 RGB 色彩空间的 32 位 ARGB 值初始化 ColorX 结构的新实例。
        /// </summary>
        /// <param name="argb">颜色在 RGB 色彩空间的 32 位 ARGB 值。</param>
        public ColorX(int argb)
        {
            this = default(ColorX);

            Color color = Color.FromArgb(argb);

            _CtorRGB(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// 使用颜色的 16 进制 ARGB 码或 RGB 码初始化 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hexCode">颜色的 16 进制 ARGB 码或 RGB 码。</param>
        public ColorX(string hexCode)
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

            this = default(ColorX);

            Color color = Color.FromArgb(int.Parse(HexCode, NumberStyles.HexNumber));

            _CtorRGB(color.A, color.R, color.G, color.B);
        }

        #endregion

        #region 字段

        /// <summary>
        /// 表示所有属性为其数据类型的默认值的 ColorX 结构的实例。
        /// </summary>
        public static readonly ColorX Empty = default(ColorX);

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
                return _Opacity;
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
        /// 获取或设置此 ColorX 结构在 RGB 色彩空间的 Alpha 通道（A）的值。
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
            }
        }

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
                _CtorRGB(_Alpha, value, _Green, _Blue);
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
                _CtorRGB(_Alpha, _Red, value, _Blue);
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
                _CtorRGB(_Alpha, _Red, _Green, value);
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
                _CtorHSV(value, _Saturation_HSV, _Brightness, _Opacity);
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
                _CtorHSV(_Hue_HSV, value, _Brightness, _Opacity);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSV 色彩空间的明度（V）。
        /// </summary>
        public double Brightness
        {
            get
            {
                return _Brightness;
            }

            set
            {
                _CtorHSV(_Hue_HSV, _Saturation_HSV, value, _Opacity);
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
                _CtorHSL(value, _Saturation_HSL, _Lightness_HSL, _Opacity);
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
                _CtorHSL(_Hue_HSL, value, _Lightness_HSL, _Opacity);
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
                _CtorHSL(_Hue_HSL, _Saturation_HSL, value, _Opacity);
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
                _CtorCMYK(value, _Magenta, _Yellow, _Black, _Opacity);
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
                _CtorCMYK(_Cyan, value, _Yellow, _Black, _Opacity);
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
                _CtorCMYK(_Cyan, _Magenta, value, _Black, _Opacity);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 CMYK 色彩空间的黑色通道（K）的值。
        /// </summary>
        public double Black
        {
            get
            {
                return _Black;
            }

            set
            {
                _CtorCMYK(_Cyan, _Magenta, _Yellow, value, _Opacity);
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
                _CtorLAB(value, _GreenRed, _BlueYellow, _Opacity);
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
                _CtorLAB(_Lightness_LAB, value, _BlueYellow, _Opacity);
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
                _CtorLAB(_Lightness_LAB, _GreenRed, value, _Opacity);
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
                _CtorRGB(_Alpha, value.X, value.Y, value.Z);
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
                _CtorHSV(value.X, value.Y, value.Z, _Opacity);
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
                _CtorHSL(value.X, value.Y, value.Z, _Opacity);
            }
        }

        /// <summary>
        /// 获取或设置表示此 ColorX 结构在 CMYK 色彩空间的各分量的 PointD4D 结构。
        /// </summary>
        public PointD4D CMYK
        {
            get
            {
                return new PointD4D(_Cyan, _Magenta, _Yellow, _Black);
            }

            set
            {
                _CtorCMYK(value.X, value.Y, value.Z, value.U, _Opacity);
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

                double Y = 0.2126 * _Red + 0.7152 * _Green + 0.0722 * _Blue;

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
                int Argb = (int)Math.Round(_Alpha) * 16777216 + (int)Math.Round(_Red) * 65536 + (int)Math.Round(_Green) * 256 + (int)Math.Round(_Blue);

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
                int Rgb = (int)Math.Round(_Red) * 65536 + (int)Math.Round(_Green) * 256 + (int)Math.Round(_Blue);

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

        //

        /// <summary>
        /// 判断此 ColorX 结构是否与指定的 ColorX 结构相等。
        /// </summary>
        /// <param name="color">用于比较的 ColorX 结构。</param>
        /// <returns>布尔值，表示此 ColorX 结构是否与指定的 ColorX 结构相等。</returns>
        public bool Equals(ColorX color)
        {
            return (_Opacity.Equals(color._Opacity) && (_Alpha.Equals(color._Alpha) && _Red.Equals(color._Red) && _Green.Equals(color._Green) && _Blue.Equals(color._Blue)) && (_Hue_HSV.Equals(color._Hue_HSV) && _Saturation_HSV.Equals(color._Saturation_HSV) && _Brightness.Equals(color._Brightness)) && (_Hue_HSL.Equals(color._Hue_HSL) && _Saturation_HSL.Equals(color._Saturation_HSL) && _Lightness_HSL.Equals(color._Lightness_HSL)) && (_Cyan.Equals(color._Cyan) && _Magenta.Equals(color._Magenta) && _Yellow.Equals(color._Yellow) && _Black.Equals(color._Black)) && (_Lightness_LAB.Equals(color._Lightness_LAB) && _GreenRed.Equals(color._GreenRed) && _BlueYellow.Equals(color._BlueYellow)));
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构转换为 Color 结构的新实例。
        /// </summary>
        /// <returns>Color 结构，表示将此 ColorX 结构转换为 Color 结构得到的结果。</returns>
        public Color ToColor()
        {
            if (IsEmpty)
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
            return ToColor().ToArgb();
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

        //

        /// <summary>
        /// 返回将此 ColorX 结构在 RGB 色彩空间的 Alpha 通道（A）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="alpha">颜色在 RGB 色彩空间的 Alpha 通道（A）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 RGB 色彩空间的 Alpha 通道（A）的值更改为指定值得到的结果。</returns>
        public ColorX AtAlpha(double alpha)
        {
            ColorX color = this;

            color.Alpha = alpha;

            return color;
        }

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
        /// 返回将此 ColorX 结构在 HSV 色彩空间的明度（V）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="brightness">颜色在 HSV 色彩空间的明度（V）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 HSV 色彩空间的明度（V）更改为指定值得到的结果。</returns>
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
        /// <param name="alpha">颜色在 RGB 色彩空间的 Alpha 通道（A）的值。</param>
        /// <param name="red">颜色在 RGB 色彩空间的红色通道（R）的值。</param>
        /// <param name="green">颜色在 RGB 色彩空间的绿色通道（G）的值。</param>
        /// <param name="blue">颜色在 RGB 色彩空间的蓝色通道（B）的值。</param>
        /// <returns>ColorX 结构，表示将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromRGB(double alpha, double red, double green, double blue)
        {
            ColorX color = default(ColorX);

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
            ColorX color = default(ColorX);

            color._CtorRGB(_MaxAlpha, red, green, blue);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="alpha">颜色在 RGB 色彩空间的 Alpha 通道（A）的值。</param>
        /// <param name="rgb">表示颜色在 RGB 色彩空间的各分量的 PointD3D 结构。</param>
        /// <returns>ColorX 结构，表示将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromRGB(double alpha, PointD3D rgb)
        {
            ColorX color = default(ColorX);

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
            ColorX color = default(ColorX);

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
        /// <param name="brightness">颜色在 HSV 色彩空间的明度（V）。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSV(double hue, double saturation, double brightness, double opacity)
        {
            ColorX color = default(ColorX);

            color._CtorHSV(hue, saturation, brightness, opacity);

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSV 色彩空间的色相（H）。</param>
        /// <param name="saturation">颜色在 HSV 色彩空间的饱和度（S）。</param>
        /// <param name="brightness">颜色在 HSV 色彩空间的明度（V）。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSV(double hue, double saturation, double brightness)
        {
            ColorX color = default(ColorX);

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
            ColorX color = default(ColorX);

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
            ColorX color = default(ColorX);

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
            ColorX color = default(ColorX);

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
            ColorX color = default(ColorX);

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
            ColorX color = default(ColorX);

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
            ColorX color = default(ColorX);

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
            ColorX color = default(ColorX);

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
            ColorX color = default(ColorX);

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
            ColorX color = default(ColorX);

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
            ColorX color = default(ColorX);

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
            ColorX color = default(ColorX);

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
            ColorX color = default(ColorX);

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
            ColorX color = default(ColorX);

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
            ColorX color = default(ColorX);

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
        /// <returns>ColorX 结构，表示不透明度为 100%，其他分量为随机数的 ColorX 结构得到的结果。</returns>
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
            if (left.IsEmpty || right.IsEmpty)
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
            if (left.IsEmpty || right.IsEmpty)
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