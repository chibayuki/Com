/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.Text
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
using System.Windows.Forms;

namespace Com
{
    /// <summary>
    /// 为文本处理提供静态方法。
    /// </summary>
    public static class Text
    {
        #region 科学计数法

        private const string _PositiveMagnitudeOrderCode = "kMGTPEZY"; // 正千进制数量级符号。
        private const string _NegativeMagnitudeOrderCode = "mμnpfazy"; // 负千进制数量级符号。

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="significance">有效数字位数，0 表示保留所有有效数字。</param>
        /// <param name="useNaturalExpression">是否使用 "m×10^n" 的格式，而不是 "mE+n"。</param>
        /// <param name="useMagnitudeOrderCode">对于数量级介于 ±24 的数值，是否使用千进制数量级符号。</param>
        /// <param name="unit">单位。</param>
        /// <returns>字符串，表示以科学记数法表示的数值。</returns>
        public static string GetScientificNotationString(double value, int significance, bool useNaturalExpression, bool useMagnitudeOrderCode, string unit)
        {
            if (InternalMethod.IsNaNOrInfinity(value))
            {
                return "N/A";
            }
            else
            {
                string part1 = string.Empty, part2 = string.Empty, part3 = string.Empty, part4 = string.Empty;

                int sign = Math.Sign(value);

                part1 = (sign < 0 ? "-" : string.Empty);

                value = Math.Abs(value);

                significance = Math.Max(0, significance);

                int exp = (int)Math.Floor(Math.Log10(value));

                if (significance > 0)
                {
                    exp -= (significance - 1);

                    value = Math.Round(value / Math.Pow(10, exp));
                }
                else
                {
                    value /= Math.Pow(10, exp);
                }

                while (value >= 10)
                {
                    value /= 10;
                    exp++;
                }

                if (useMagnitudeOrderCode)
                {
                    if (exp >= -24 && exp < 27)
                    {
                        int mod = 0;

                        if (exp >= 0)
                        {
                            mod = exp % 3;
                        }
                        else
                        {
                            mod = (-exp) % 3;

                            if (mod > 0)
                            {
                                mod = 3 - mod;
                            }
                        }

                        if (mod > 0)
                        {
                            value *= Math.Pow(10, mod);
                        }

                        part2 = (significance > 0 ? value.ToString("N" + Math.Max(0, significance - 1 - mod)) : value.ToString());

                        int mag = 0;

                        if (exp >= 0)
                        {
                            mag = exp / 3;
                        }
                        else
                        {
                            mag = (exp + 1) / 3 - 1;
                        }

                        string magCode = (mag > 0 ? _PositiveMagnitudeOrderCode[mag - 1].ToString() : (mag < 0 ? _NegativeMagnitudeOrderCode[-mag - 1].ToString() : string.Empty));

                        if (string.IsNullOrEmpty(unit))
                        {
                            part3 = magCode;
                            part4 = string.Empty;
                        }
                        else
                        {
                            part3 = " " + magCode;
                            part4 = unit;
                        }
                    }
                    else
                    {
                        part2 = (significance > 0 ? value.ToString("N" + Math.Max(0, significance - 1)) : value.ToString());
                        part3 = (useNaturalExpression ? "×10^" + exp : (exp > 0 ? "E+" + exp : "E" + exp));
                        part4 = (string.IsNullOrEmpty(unit) ? string.Empty : " " + unit);
                    }
                }
                else
                {
                    part2 = (significance > 0 ? value.ToString("N" + Math.Max(0, significance - 1)) : value.ToString());
                    part3 = (exp == 0 ? string.Empty : (useNaturalExpression ? "×10^" + exp : (exp > 0 ? "E+" + exp : "E" + exp)));
                    part4 = (string.IsNullOrEmpty(unit) ? string.Empty : " " + unit);
                }

                return string.Concat(part1, part2, part3, part4);
            }
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="significance">有效数字位数，0 表示保留所有有效数字。</param>
        /// <param name="useNaturalExpression">是否使用 "m×10^n" 的格式，而不是 "mE+n"。</param>
        /// <param name="unit">单位。</param>
        /// <returns>字符串，表示以科学记数法表示的数值。</returns>
        public static string GetScientificNotationString(double value, int significance, bool useNaturalExpression, string unit)
        {
            return GetScientificNotationString(value, significance, useNaturalExpression, false, unit);
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="significance">有效数字位数，0 表示保留所有有效数字。</param>
        /// <param name="useNaturalExpression">是否使用 "m×10^n" 的格式，而不是 "mE+n"。</param>
        /// <param name="useMagnitudeOrderCode">对于数量级介于 ±24 的数值，是否使用千进制数量级符号。</param>
        /// <returns>字符串，表示以科学记数法表示的数值。</returns>
        public static string GetScientificNotationString(double value, int significance, bool useNaturalExpression, bool useMagnitudeOrderCode)
        {
            return GetScientificNotationString(value, significance, useNaturalExpression, useMagnitudeOrderCode, null);
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="significance">有效数字位数，0 表示保留所有有效数字。</param>
        /// <param name="useNaturalExpression">是否使用 "m×10^n" 的格式，而不是 "mE+n"。</param>
        /// <returns>字符串，表示以科学记数法表示的数值。</returns>
        public static string GetScientificNotationString(double value, int significance, bool useNaturalExpression)
        {
            return GetScientificNotationString(value, significance, useNaturalExpression, false, null);
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="useNaturalExpression">是否使用 "m×10^n" 的格式，而不是 "mE+n"。</param>
        /// <param name="useMagnitudeOrderCode">对于数量级介于 ±24 的数值，是否使用千进制数量级符号。</param>
        /// <param name="unit">单位。</param>
        /// <returns>字符串，表示以科学记数法表示的数值。</returns>
        public static string GetScientificNotationString(double value, bool useNaturalExpression, bool useMagnitudeOrderCode, string unit)
        {
            return GetScientificNotationString(value, 0, useNaturalExpression, useMagnitudeOrderCode, unit);
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="useNaturalExpression">是否使用 "m×10^n" 的格式，而不是 "mE+n"。</param>
        /// <param name="unit">单位。</param>
        /// <returns>字符串，表示以科学记数法表示的数值。</returns>
        public static string GetScientificNotationString(double value, bool useNaturalExpression, string unit)
        {
            return GetScientificNotationString(value, 0, useNaturalExpression, false, unit);
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="significance">有效数字位数，0 表示保留所有有效数字。</param>
        /// <param name="unit">单位。</param>
        /// <returns>字符串，表示以科学记数法表示的数值。</returns>
        public static string GetScientificNotationString(double value, int significance, string unit)
        {
            return GetScientificNotationString(value, significance, false, false, unit);
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="significance">有效数字位数，0 表示保留所有有效数字。</param>
        /// <returns>字符串，表示以科学记数法表示的数值。</returns>
        public static string GetScientificNotationString(double value, int significance)
        {
            return GetScientificNotationString(value, significance, false, false, null);
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="useNaturalExpression">是否使用 "m×10^n" 的格式，而不是 "mE+n"。</param>
        /// <param name="useMagnitudeOrderCode">对于数量级介于 ±24 的数值，是否使用千进制数量级符号。</param>
        /// <returns>字符串，表示以科学记数法表示的数值。</returns>
        public static string GetScientificNotationString(double value, bool useNaturalExpression, bool useMagnitudeOrderCode)
        {
            return GetScientificNotationString(value, 0, useNaturalExpression, useMagnitudeOrderCode, null);
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="useNaturalExpression">是否使用 "m×10^n" 的格式，而不是 "mE+n"。</param>
        /// <returns>字符串，表示以科学记数法表示的数值。</returns>
        public static string GetScientificNotationString(double value, bool useNaturalExpression)
        {
            return GetScientificNotationString(value, 0, useNaturalExpression, false, null);
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="unit">单位。</param>
        /// <returns>字符串，表示以科学记数法表示的数值。</returns>
        public static string GetScientificNotationString(double value, string unit)
        {
            return GetScientificNotationString(value, 0, false, false, unit);
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <returns>字符串，表示以科学记数法表示的数值。</returns>
        public static string GetScientificNotationString(double value)
        {
            return GetScientificNotationString(value, 0, false, false, null);
        }

        #endregion

        #region 字符串处理

        /// <summary>
        /// 获取源字符串中位于两个指定字符串之间的部分。
        /// </summary>
        /// <param name="sourceString">源字符串。</param>
        /// <param name="startString">起始字符串。</param>
        /// <param name="endString">结尾字符串。</param>
        /// <param name="includeStartString">是否包含起始字符串。</param>
        /// <param name="includeEndString">是否包含结尾字符串。</param>
        /// <returns>字符串，表示源字符串中位于两个指定字符串之间的部分。</returns>
        public static string GetIntervalString(string sourceString, string startString, string endString, bool includeStartString, bool includeEndString)
        {
            if (string.IsNullOrEmpty(sourceString) || string.IsNullOrEmpty(startString) || string.IsNullOrEmpty(endString))
            {
                return string.Empty;
            }
            else
            {
                if (!sourceString.Contains(startString) || !sourceString.Contains(endString))
                {
                    return string.Empty;
                }
                else
                {
                    int StartIndex = sourceString.IndexOf(startString) + (includeStartString ? 0 : startString.Length);
                    int EndIndex = sourceString.IndexOf(endString) + (includeEndString ? endString.Length : 0);

                    if (StartIndex >= EndIndex)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return sourceString.Substring(StartIndex, EndIndex - StartIndex);
                    }
                }
            }
        }

        /// <summary>
        /// 返回按照指定字体与绘图宽度截取后的字符串。
        /// </summary>
        /// <param name="text">字符串。</param>
        /// <param name="font">字体。</param>
        /// <param name="width">绘图宽度（像素）。</param>
        /// <returns>字符串，表示按照指定字体与绘图宽度截取后的字符串。</returns>
        public static string StringIntercept(string text, Font font, int width)
        {
            if (string.IsNullOrEmpty(text) || font == null)
            {
                return string.Empty;
            }
            else
            {
                SizeF StrSz = TextRenderer.MeasureText(text, font);

                if (StrSz.Width > width && text.Length > 1)
                {
                    for (int i = text.Length - 1; i >= 1; i--)
                    {
                        text = text.Substring(0, i);

                        StrSz = TextRenderer.MeasureText(string.Concat(text, "..."), font);

                        if (StrSz.Width <= width)
                        {
                            text = string.Concat(text, "...");

                            break;
                        }
                    }
                }

                return text;
            }
        }

        /// <summary>
        /// 返回一个字体（仅改变当前字体的大小），使字符串的绘图区域不超过指定的大小。
        /// </summary>
        /// <param name="text">字符串。</param>
        /// <param name="font">字体。</param>
        /// <param name="size">绘图区域的大小（像素）。</param>
        /// <returns>Font 对象，表示使字符串的绘图区域不超过指定大小的字体。</returns>
        public static Font GetSuitableFont(string text, Font font, SizeF size)
        {
            if (string.IsNullOrEmpty(text) || font == null)
            {
                return font;
            }
            else
            {
                Font Ft = font;
                SizeF Sz = TextRenderer.MeasureText(text, Ft);

                while (Sz.Width <= size.Width || Sz.Height <= size.Height)
                {
                    Ft = new Font(Ft.Name, Ft.Size * 1.05F, Ft.Style, Ft.Unit, Ft.GdiCharSet);
                    Sz = TextRenderer.MeasureText(text, Ft);
                }

                while (Sz.Width > size.Width || Sz.Height > size.Height)
                {
                    if (Ft.Size <= 1F)
                    {
                        break;
                    }

                    Ft = new Font(Ft.Name, Ft.Size / 1.05F, Ft.Style, Ft.Unit, Ft.GdiCharSet);
                    Sz = TextRenderer.MeasureText(text, Ft);
                }

                return Ft;
            }
        }

        #endregion

        #region 转换为字符串

        /// <summary>
        /// 返回一个以 "hh:mm:ss.ms" 格式表示时间间隔的字符串。
        /// </summary>
        /// <param name="timeSpan">时间间隔。</param>
        /// <returns>字符串，表示以 "hh:mm:ss.ms" 格式表示的时间间隔。</returns>
        public static string GetLongTimeStringFromTimeSpan(TimeSpan timeSpan)
        {
            return string.Concat(timeSpan.Days, ":", timeSpan.Hours.ToString("D2"), ":", timeSpan.Minutes.ToString("D2"), ":", timeSpan.Seconds.ToString("D2"), ".", timeSpan.Milliseconds.ToString("D3"));
        }

        /// <summary>
        /// 返回一个以 "h 小时 m 分 s 秒" 格式表示时间间隔的字符串。
        /// </summary>
        /// <param name="timeSpan">时间间隔。</param>
        /// <returns>字符串，表示以 "h 小时 m 分 s 秒" 格式表示的时间间隔。</returns>
        public static string GetTimeStringFromTimeSpan(TimeSpan timeSpan)
        {
            return (timeSpan.TotalHours >= 1 ? string.Concat(Math.Floor(timeSpan.TotalHours), " 小时 ", timeSpan.Minutes, " 分 ", timeSpan.Seconds, " 秒") : (timeSpan.TotalMinutes >= 1 ? string.Concat(timeSpan.Minutes, " 分 ", timeSpan.Seconds, " 秒") : string.Concat(timeSpan.Seconds, (timeSpan.TotalSeconds > 0 ? string.Concat(".", timeSpan.Milliseconds.ToString("D3").Substring(0, timeSpan.Seconds >= 10 ? 1 : (timeSpan.Seconds >= 1 ? 2 : 3))) : string.Empty), " 秒")));
        }

        /// <summary>
        /// 返回将一个时间间隔在合适的单位下保留指定位数有效数字的数值。
        /// </summary>
        /// <param name="seconds">时间间隔（秒）。</param>
        /// <param name="significance">有效数字位数，0 表示保留所有有效数字。</param>
        /// <returns>双精度浮点数，表示将一个时间间隔在合适的单位下保留指定位数有效数字的数值。</returns>
        public static double GetStandardizationTimespanOfSecond(double seconds, int significance)
        {
            if (InternalMethod.IsNaNOrInfinity(seconds))
            {
                return double.NaN;
            }
            else
            {
                if (seconds != 0 && significance > 0)
                {
                    int sign = Math.Sign(seconds);

                    seconds = Math.Abs(seconds);

                    double unit = 1;

                    if (seconds >= 31557600.0)
                    {
                        seconds /= (unit = 31557600.0);
                    }
                    else if (seconds >= 86400.0)
                    {
                        seconds /= (unit = 86400.0);
                    }
                    else if (seconds >= 3600.0)
                    {
                        seconds /= (unit = 3600.0);
                    }
                    else if (seconds >= 60.0)
                    {
                        seconds /= (unit = 60.0);
                    }
                    else if (seconds < 1)
                    {
                        seconds /= (unit = 0.001);
                    }

                    int exp = (int)Math.Floor(Math.Log10(Math.Abs(seconds))) - Math.Max(0, significance - 1);

                    seconds = Math.Round(seconds / Math.Pow(10, exp));

                    while (seconds >= 10)
                    {
                        seconds /= 10;
                        exp++;
                    }

                    seconds *= (sign * Math.Pow(10, exp) * unit);
                }

                return seconds;
            }
        }

        /// <summary>
        /// 返回一个表示时间间隔的字符串。
        /// </summary>
        /// <param name="seconds">时间间隔（秒）。</param>
        /// <returns>字符串，表示时间间隔。</returns>
        public static string GetLargeTimespanStringOfSecond(double seconds)
        {
            if (InternalMethod.IsNaNOrInfinity(seconds))
            {
                return "N/A";
            }
            else
            {
                if (seconds == 0)
                {
                    return "0";
                }
                else
                {
                    int Sign = Math.Sign(seconds);

                    seconds = Math.Abs(seconds);

                    return string.Concat((Sign == -1 ? "-" : string.Empty), (seconds >= 31557600E12 ? string.Concat(seconds / (31557600E12), " Ta") : (seconds >= 31557600E9 ? string.Concat(seconds / (31557600E9), " Ga") : (seconds >= 31557600E6 ? string.Concat(seconds / (31557600E6), " Ma") : (seconds >= 31557600E3 ? string.Concat(seconds / (31557600E3), " ka") : (seconds >= 31557600.0 ? string.Concat(seconds / 31557600.0, " a") : (seconds >= 86400.0 ? string.Concat(seconds / 86400.0, " d") : (seconds >= 3600.0 ? string.Concat(seconds / 3600.0, " h") : (seconds >= 60 ? string.Concat(seconds / 60.0, " min") : (seconds >= 1 ? string.Concat(seconds, " s") : string.Concat(seconds * 1000.0, " ms")))))))))));
                }
            }
        }

        /// <summary>
        /// 返回将一个距离在合适的单位下保留指定位数有效数字的数值。
        /// </summary>
        /// <param name="meters">距离（米）。</param>
        /// <param name="significance">有效数字位数，0 表示保留所有有效数字。</param>
        /// <returns>双精度浮点数，表示将一个距离在合适的单位下保留指定位数有效数字的数值。</returns>
        public static double GetStandardizationDistanceOfMeter(double meters, int significance)
        {
            if (InternalMethod.IsNaNOrInfinity(meters))
            {
                return double.NaN;
            }
            else
            {
                if (meters != 0 && significance > 0)
                {
                    int sign = Math.Sign(meters);

                    meters = Math.Abs(meters);

                    double unit = 1;

                    if (meters >= 9460730472580800.0)
                    {
                        meters /= (unit = 9460730472580800.0);
                    }
                    else if (meters >= 149597870700.0)
                    {
                        meters /= (unit = 149597870700.0);
                    }
                    else if (meters >= 1000.0)
                    {
                        meters /= (unit = 1000.0);
                    }

                    int exp = (int)Math.Floor(Math.Log10(Math.Abs(meters))) - Math.Max(0, significance - 1);

                    meters = Math.Round(meters / Math.Pow(10, exp));

                    while (meters >= 10)
                    {
                        meters /= 10;
                        exp++;
                    }

                    meters *= (sign * Math.Pow(10, exp) * unit);
                }

                return meters;
            }
        }

        /// <summary>
        /// 返回一个表示距离的字符串。
        /// </summary>
        /// <param name="meters">距离（米）。</param>
        /// <returns>字符串，表示距离。</returns>
        public static string GetLargeDistanceStringOfMeter(double meters)
        {
            if (InternalMethod.IsNaNOrInfinity(meters))
            {
                return "N/A";
            }
            else
            {
                if (meters == 0)
                {
                    return "0";
                }
                else
                {
                    int Sign = Math.Sign(meters);

                    meters = Math.Abs(meters);

                    return string.Concat((Sign == -1 ? "-" : string.Empty), (meters >= 9460730472580800E12 ? string.Concat(meters / 9460730472580800E12, " Tly") : (meters >= 9460730472580800E9 ? string.Concat(meters / 9460730472580800E9, " Gly") : (meters >= 9460730472580800E6 ? string.Concat(meters / 9460730472580800E6, " Mly") : (meters >= 9460730472580800E3 ? string.Concat(meters / 9460730472580800E3, " kly") : (meters >= 9460730472580800.0 ? string.Concat(meters / 9460730472580800.0, " ly") : (meters >= 149597870700.0 ? string.Concat(meters / 149597870700.0, " AU") : (meters >= 1000.0 ? string.Concat(meters / 1000.0, " km") : string.Concat(meters, " m")))))))));
                }
            }
        }

        /// <summary>
        /// 返回一个以 "d° m′ s″ " 格式表示角度的字符串。
        /// </summary>
        /// <param name="degree">以角度为单位的角度。</param>
        /// <param name="decimalDigits">保留的小数位数，0 表示不保留小数，-1 表示保留所有小数。</param>
        /// <param name="cutdownIdleZeros">是否删除小数点后所有不必要的 "0"。</param>
        /// <returns>字符串，表示以 "d° m′ s″ " 格式表示的角度。</returns>
        public static string GetAngleStringOfDegree(double degree, int decimalDigits, bool cutdownIdleZeros)
        {
            if (InternalMethod.IsNaNOrInfinity(degree))
            {
                return "N/A";
            }
            else
            {
                string Sign = (degree < 0 ? "-" : string.Empty);
                degree = Math.Abs(degree);

                double DegF = degree;
                double DegD = Math.Floor(DegF);
                double MinF = (DegF - DegD) * 60;
                double MinD = Math.Floor(MinF);
                double SecF = (MinF - MinD) * 60;

                string DegStr = string.Concat(DegD, "° ");
                string MinStr = string.Concat(MinD, "′ ");
                string SecStr = string.Concat(SecF.ToString(decimalDigits >= 0 ? string.Concat("N", Math.Min(16, decimalDigits)) : string.Empty), "″ ");

                if (cutdownIdleZeros)
                {
                    int DotIndex = SecStr.IndexOf('.');

                    if (DotIndex >= 0)
                    {
                        int LastPartIndex = DotIndex + 1;

                        while (LastPartIndex < SecStr.Length && char.IsDigit(SecStr, LastPartIndex))
                        {
                            LastPartIndex++;
                        }

                        string HeadPart = SecStr.Substring(0, DotIndex);
                        string MiddlePart = SecStr.Substring(DotIndex, LastPartIndex - DotIndex);
                        string LastPart = (LastPartIndex < SecStr.Length ? SecStr.Substring(LastPartIndex) : string.Empty);

                        if (MiddlePart.Length > 0)
                        {
                            while (MiddlePart[MiddlePart.Length - 1] == '0')
                            {
                                MiddlePart = MiddlePart.Substring(0, MiddlePart.Length - 1);
                            }

                            if (MiddlePart.Length == 1)
                            {
                                MiddlePart = string.Empty;
                            }
                        }

                        SecStr = string.Concat(HeadPart, MiddlePart, LastPart);
                    }
                }

                return string.Concat(Sign, DegStr, MinStr, SecStr);
            }
        }

        /// <summary>
        /// 返回一个表示存储大小的字符串。
        /// </summary>
        /// <param name="bytes">存储大小（字节）。</param>
        /// <returns>字符串，表示存储大小。</returns>
        public static string GetSize64StringFromByte(long bytes)
        {
            bytes = Math.Abs(bytes);

            if (bytes < 1E3)
            {
                return string.Concat(bytes, " B");
            }
            else if (bytes < 1E6)
            {
                return string.Concat((bytes / Math.Pow(2, 10)).ToString("N1"), " KB");
            }
            else if (bytes < 1E9)
            {
                return string.Concat((bytes / Math.Pow(2, 20)).ToString("N1"), " MB");
            }
            else if (bytes < 1E12)
            {
                return string.Concat((bytes / Math.Pow(2, 30)).ToString("N1"), " GB");
            }
            else if (bytes < 1E15)
            {
                return string.Concat((bytes / Math.Pow(2, 40)).ToString("N1"), " TB");
            }
            else if (bytes < 1E18)
            {
                return string.Concat((bytes / Math.Pow(2, 50)).ToString("N1"), " PB");
            }
            else if (bytes < 1E21)
            {
                return string.Concat((bytes / Math.Pow(2, 60)).ToString("N1"), " EB");
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion
    }
}