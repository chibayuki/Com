/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

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

        private const string _PositiveMagnitudeOrderCode = "kMGTPEZY"; // 正数量级符号。
        private const string _NegativeMagnitudeOrderCode = "mμnpfazy"; // 负数量级符号。

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="significance">有效数字位数，0 表示保留所有有效数字。</param>
        /// <param name="useNaturalExpression">是否使用 "m×10^n" 的格式，而不是 "mE+n"。</param>
        /// <param name="useMagnitudeOrderCode">对于数量级介于 ±24 的数值，是否使用数量级符号。</param>
        /// <param name="unit">单位。</param>
        public static string GetScientificNotationString(double value, int significance, bool useNaturalExpression, bool useMagnitudeOrderCode, string unit)
        {
            if (!InternalMethod.IsNaNOrInfinity(value))
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

            return "N/A";
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="significance">有效数字位数，0 表示保留所有有效数字。</param>
        /// <param name="useNaturalExpression">是否使用 "m×10^n" 的格式，而不是 "mE+n"。</param>
        /// <param name="unit">单位。</param>
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
        /// <param name="useMagnitudeOrderCode">对于数量级介于 ±24 的数值，是否使用数量级符号。</param>
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
        public static string GetScientificNotationString(double value, int significance, bool useNaturalExpression)
        {
            return GetScientificNotationString(value, significance, useNaturalExpression, false, null);
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="useNaturalExpression">是否使用 "m×10^n" 的格式，而不是 "mE+n"。</param>
        /// <param name="useMagnitudeOrderCode">对于数量级介于 ±24 的数值，是否使用数量级符号。</param>
        /// <param name="unit">单位。</param>
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
        public static string GetScientificNotationString(double value, int significance, string unit)
        {
            return GetScientificNotationString(value, significance, false, false, unit);
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="significance">有效数字位数，0 表示保留所有有效数字。</param>
        public static string GetScientificNotationString(double value, int significance)
        {
            return GetScientificNotationString(value, significance, false, false, null);
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="useNaturalExpression">是否使用 "m×10^n" 的格式，而不是 "mE+n"。</param>
        /// <param name="useMagnitudeOrderCode">对于数量级介于 ±24 的数值，是否使用数量级符号。</param>
        public static string GetScientificNotationString(double value, bool useNaturalExpression, bool useMagnitudeOrderCode)
        {
            return GetScientificNotationString(value, 0, useNaturalExpression, useMagnitudeOrderCode, null);
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="useNaturalExpression">是否使用 "m×10^n" 的格式，而不是 "mE+n"。</param>
        public static string GetScientificNotationString(double value, bool useNaturalExpression)
        {
            return GetScientificNotationString(value, 0, useNaturalExpression, false, null);
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
        /// <param name="unit">单位。</param>
        public static string GetScientificNotationString(double value, string unit)
        {
            return GetScientificNotationString(value, 0, false, false, unit);
        }

        /// <summary>
        /// 返回一个以科学记数法表示数值的字符串。
        /// </summary>
        /// <param name="value">数值。</param>
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
        public static string GetIntervalString(string sourceString, string startString, string endString, bool includeStartString, bool includeEndString)
        {
            if (!string.IsNullOrEmpty(sourceString) && !string.IsNullOrEmpty(startString) && !string.IsNullOrEmpty(endString))
            {
                if (sourceString.Contains(startString) && sourceString.Contains(endString))
                {
                    int StartIndex = sourceString.IndexOf(startString) + (includeStartString ? 0 : startString.Length);
                    int EndIndex = sourceString.IndexOf(endString) + (includeEndString ? endString.Length : 0);

                    if (StartIndex < EndIndex)
                    {
                        return sourceString.Substring(StartIndex, EndIndex - StartIndex);
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 返回按照指定字体与绘图宽度截取后的字符串。
        /// </summary>
        /// <param name="text">字符串。</param>
        /// <param name="font">字体。</param>
        /// <param name="width">绘图宽度（像素）。</param>
        public static string StringIntercept(string text, Font font, int width)
        {
            if (!string.IsNullOrEmpty(text) && font != null)
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

            return string.Empty;
        }

        /// <summary>
        /// 返回一个字体（仅改变当前字体的大小），使字符串的绘图区域不超过指定的大小。
        /// </summary>
        /// <param name="text">字符串。</param>
        /// <param name="font">字体。</param>
        /// <param name="size">绘图区域的大小（像素）。</param>
        public static Font GetSuitableFont(string text, Font font, SizeF size)
        {
            if (!string.IsNullOrEmpty(text) && font != null)
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

            return font;
        }

        #endregion

        #region 转换为字符串

        /// <summary>
        /// 返回一个以 "h:m:s.ms" 格式表示时间间隔的字符串。
        /// </summary>
        /// <param name="timeSpan">时间间隔。</param>
        public static string GetLongTimeStringFromTimeSpan(TimeSpan timeSpan)
        {
            return string.Concat(timeSpan.Days, ":", timeSpan.Hours.ToString("D2"), ":", timeSpan.Minutes.ToString("D2"), ":", timeSpan.Seconds.ToString("D2"), ".", timeSpan.Milliseconds.ToString("D3"));
        }

        /// <summary>
        /// 返回一个以 "h 小时 m 分 s 秒" 格式表示时间间隔的字符串。
        /// </summary>
        /// <param name="timeSpan">时间间隔。</param>
        public static string GetTimeStringFromTimeSpan(TimeSpan timeSpan)
        {
            return (timeSpan.TotalHours >= 1 ? string.Concat(Math.Floor(timeSpan.TotalHours), " 小时 ", timeSpan.Minutes, " 分 ", timeSpan.Seconds, " 秒") : (timeSpan.TotalMinutes >= 1 ? string.Concat(timeSpan.Minutes, " 分 ", timeSpan.Seconds, " 秒") : string.Concat(timeSpan.Seconds, (timeSpan.TotalSeconds > 0 ? string.Concat(".", timeSpan.Milliseconds.ToString("D3").Substring(0, timeSpan.Seconds >= 10 ? 1 : (timeSpan.Seconds >= 1 ? 2 : 3))) : string.Empty), " 秒")));
        }

        /// <summary>
        /// 返回将一个时间间隔（秒）在合适的单位下保留指定位数有效数字的数值。
        /// </summary>
        /// <param name="second">时间间隔（秒）。</param>
        /// <param name="significance">有效数字位数，0 表示保留所有有效数字。</param>
        public static double GetStandardizationTimespanOfSecond(double second, int significance)
        {
            if (InternalMethod.IsNaNOrInfinity(second))
            {
                return double.NaN;
            }
            else
            {
                if (second != 0 && significance > 0)
                {
                    int sign = Math.Sign(second);

                    second = Math.Abs(second);

                    double unit = 1;

                    if (second >= 31557600.0)
                    {
                        second /= (unit = 31557600.0);
                    }
                    else if (second >= 86400.0)
                    {
                        second /= (unit = 86400.0);
                    }
                    else if (second >= 3600.0)
                    {
                        second /= (unit = 3600.0);
                    }
                    else if (second >= 60.0)
                    {
                        second /= (unit = 60.0);
                    }
                    else if (second < 1)
                    {
                        second /= (unit = 0.001);
                    }

                    int exp = (int)Math.Floor(Math.Log10(Math.Abs(second))) - Math.Max(0, significance - 1);

                    second = Math.Round(second / Math.Pow(10, exp));

                    while (second >= 10)
                    {
                        second /= 10;
                        exp++;
                    }

                    second *= (sign * Math.Pow(10, exp) * unit);
                }

                return second;
            }
        }

        /// <summary>
        /// 返回一个表示时间间隔（秒）的字符串。
        /// </summary>
        /// <param name="second">时间间隔（秒）。</param>
        public static string GetLargeTimespanStringOfSecond(double second)
        {
            if (InternalMethod.IsNaNOrInfinity(second))
            {
                return "N/A";
            }
            else
            {
                if (second != 0)
                {
                    int Sign = Math.Sign(second);

                    second = Math.Abs(second);

                    return string.Concat((Sign == -1 ? "-" : string.Empty), (second >= 31557600E12 ? string.Concat(second / (31557600E12), " Ta") : (second >= 31557600E9 ? string.Concat(second / (31557600E9), " Ga") : (second >= 31557600E6 ? string.Concat(second / (31557600E6), " Ma") : (second >= 31557600E3 ? string.Concat(second / (31557600E3), " ka") : (second >= 31557600.0 ? string.Concat(second / 31557600.0, " a") : (second >= 86400.0 ? string.Concat(second / 86400.0, " d") : (second >= 3600.0 ? string.Concat(second / 3600.0, " h") : (second >= 60 ? string.Concat(second / 60.0, " min") : (second >= 1 ? string.Concat(second, " s") : string.Concat(second * 1000.0, " ms")))))))))));
                }

                return "0";
            }
        }

        /// <summary>
        /// 返回将一个距离（米）在合适的单位下保留指定位数有效数字的数值。
        /// </summary>
        /// <param name="meter">距离（米）。</param>
        /// <param name="significance">有效数字位数，0 表示保留所有有效数字。</param>
        public static double GetStandardizationDistanceOfMeter(double meter, int significance)
        {
            if (InternalMethod.IsNaNOrInfinity(meter))
            {
                return double.NaN;
            }
            else
            {
                if (meter != 0 && significance > 0)
                {
                    int sign = Math.Sign(meter);

                    meter = Math.Abs(meter);

                    double unit = 1;

                    if (meter >= 9460730472580800.0)
                    {
                        meter /= (unit = 9460730472580800.0);
                    }
                    else if (meter >= 149597870700.0)
                    {
                        meter /= (unit = 149597870700.0);
                    }
                    else if (meter >= 1000.0)
                    {
                        meter /= (unit = 1000.0);
                    }

                    int exp = (int)Math.Floor(Math.Log10(Math.Abs(meter))) - Math.Max(0, significance - 1);

                    meter = Math.Round(meter / Math.Pow(10, exp));

                    while (meter >= 10)
                    {
                        meter /= 10;
                        exp++;
                    }

                    meter *= (sign * Math.Pow(10, exp) * unit);
                }

                return meter;
            }
        }

        /// <summary>
        /// 返回一个表示距离（米）的字符串。
        /// </summary>
        /// <param name="meter">距离（米）。</param>
        public static string GetLargeDistanceStringOfMeter(double meter)
        {
            if (InternalMethod.IsNaNOrInfinity(meter))
            {
                return "N/A";
            }
            else
            {
                if (meter != 0)
                {
                    int Sign = Math.Sign(meter);

                    meter = Math.Abs(meter);

                    return string.Concat((Sign == -1 ? "-" : string.Empty), (meter >= 9460730472580800E12 ? string.Concat(meter / 9460730472580800E12, " Tly") : (meter >= 9460730472580800E9 ? string.Concat(meter / 9460730472580800E9, " Gly") : (meter >= 9460730472580800E6 ? string.Concat(meter / 9460730472580800E6, " Mly") : (meter >= 9460730472580800E3 ? string.Concat(meter / 9460730472580800E3, " kly") : (meter >= 9460730472580800.0 ? string.Concat(meter / 9460730472580800.0, " ly") : (meter >= 149597870700.0 ? string.Concat(meter / 149597870700.0, " AU") : (meter >= 1000.0 ? string.Concat(meter / 1000.0, " km") : string.Concat(meter, " m")))))))));
                }

                return "0";
            }
        }

        /// <summary>
        /// 返回一个以 "d° m′ s″ " 格式表示角度（角度）的字符串。
        /// </summary>
        /// <param name="degree">以角度为单位的角度。</param>
        /// <param name="decimalDigits">保留的小数位数，0 表示不保留小数，-1 表示保留所有小数。</param>
        /// <param name="cutdownIdleZeros">是否删除小数点后所有不必要的 "0"。</param>
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
        /// 返回一个表示存储大小（字节）的字符串。
        /// </summary>
        /// <param name="b">存储大小（字节）。</param>
        public static string GetSize64StringFromByte(long b)
        {
            b = Math.Abs(b);

            if (b < 1E3)
            {
                return string.Concat(b, " B");
            }
            else if (b < 1E6)
            {
                return string.Concat((b / Math.Pow(2, 10)).ToString("N1"), " KB");
            }
            else if (b < 1E9)
            {
                return string.Concat((b / Math.Pow(2, 20)).ToString("N1"), " MB");
            }
            else if (b < 1E12)
            {
                return string.Concat((b / Math.Pow(2, 30)).ToString("N1"), " GB");
            }
            else if (b < 1E15)
            {
                return string.Concat((b / Math.Pow(2, 40)).ToString("N1"), " TB");
            }
            else if (b < 1E18)
            {
                return string.Concat((b / Math.Pow(2, 50)).ToString("N1"), " PB");
            }
            else if (b < 1E21)
            {
                return string.Concat((b / Math.Pow(2, 60)).ToString("N1"), " EB");
            }

            return string.Empty;
        }

        #endregion
    }
}