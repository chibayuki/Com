/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.Text
Version 18.9.15.2000

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
            try
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

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 返回按照指定字体与绘图宽度截取后的字符串。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <param name="strFont">字体。</param>
        /// <param name="width">绘图宽度。</param>
        public static string StringIntercept(string str, Font strFont, int width)
        {
            try
            {
                SizeF StrSzF = TextRenderer.MeasureText(str, strFont);

                if (StrSzF.Width > width && str.Length > 1)
                {
                    for (int i = str.Length - 1; i >= 1; i--)
                    {
                        str = str.Substring(0, i);

                        StrSzF = TextRenderer.MeasureText(string.Concat(str, "..."), strFont);

                        if (StrSzF.Width <= width)
                        {
                            str = string.Concat(str, "...");

                            break;
                        }
                    }
                }

                return str;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 返回一个新的字体（仅改变当前字体的大小），使字符串的绘制范围不超过限定的大小。
        /// </summary>
        /// <param name="text">字符串。</param>
        /// <param name="font">当前字体。</param>
        /// <param name="size">绘制范围的大小。</param>
        public static Font GetSuitableFont(string text, Font font, SizeF size)
        {
            try
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
            catch
            {
                return font;
            }
        }

        /// <summary>
        /// 获取日期时间精确到秒的 64 位二进制值。
        /// </summary>
        /// <param name="dateTime">日期时间。</param>
        public static Int64 GetBinaryFromDateTime(DateTime dateTime)
        {
            try
            {
                return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second).ToBinary();
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取时间间隔的长格式字符串。
        /// </summary>
        /// <param name="timeSpan">时间间隔。</param>
        public static string GetLongTimeStringFromTimeSpan(TimeSpan timeSpan)
        {
            try
            {
                return string.Concat(timeSpan.Days, ":", timeSpan.Hours.ToString("D2"), ":", timeSpan.Minutes.ToString("D2"), ":", timeSpan.Seconds.ToString("D2"), ".", timeSpan.Milliseconds.ToString("D3"));
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取时间间隔的字符串。
        /// </summary>
        /// <param name="timeSpan">时间间隔。</param>
        public static string GetTimeStringFromTimeSpan(TimeSpan timeSpan)
        {
            try
            {
                return (timeSpan.TotalHours >= 1 ? string.Concat(Math.Floor(timeSpan.TotalHours), " 小时 ", timeSpan.Minutes, " 分 ", timeSpan.Seconds, " 秒") : (timeSpan.TotalMinutes >= 1 ? string.Concat(timeSpan.Minutes, " 分 ", timeSpan.Seconds, " 秒") : string.Concat(timeSpan.Seconds, (timeSpan.TotalSeconds > 0 ? string.Concat(".", timeSpan.Milliseconds.ToString("D3").Substring(0, timeSpan.Seconds >= 10 ? 1 : (timeSpan.Seconds >= 1 ? 2 : 3))) : string.Empty), " 秒")));
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 返回一个在合适的单位下其科学记数法具有指定位位有效数字的时间间隔（秒）。
        /// </summary>
        /// <param name="second">以秒为单位的时间间隔。</param>
        /// <param name="significance">有效数字位数。</param>
        public static double GetStandardizationTimespanOfSecond(double second, int significance)
        {
            try
            {
                if (second != 0)
                {
                    int Sign = Math.Sign(second);

                    second = Math.Abs(second);

                    double Unit = 1;

                    if (second >= 31557600.0)
                    {
                        second /= (Unit = 31557600.0);
                    }
                    else if (second >= 86400.0)
                    {
                        second /= (Unit = 86400.0);
                    }
                    else if (second >= 3600.0)
                    {
                        second /= (Unit = 3600.0);
                    }
                    else if (second >= 60.0)
                    {
                        second /= (Unit = 60.0);
                    }
                    else if (second < 1)
                    {
                        second /= (Unit = 0.001);
                    }

                    double Exp = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(second))) - Math.Max(0, significance - 1));

                    return (Sign * Math.Floor(second / Exp) * Exp * Unit);
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取以秒为单位的一个时间间隔的字符串格式（秒）。
        /// </summary>
        /// <param name="second">以秒为单位的时间间隔。</param>
        public static string GetLargeTimespanStringOfSecond(double second)
        {
            try
            {
                if (second != 0)
                {
                    int Sign = Math.Sign(second);

                    second = Math.Abs(second);

                    return string.Concat((Sign == -1 ? "-" : string.Empty), (second >= 31557600E12 ? string.Concat(second / (31557600E12), " Ta") : (second >= 31557600E9 ? string.Concat(second / (31557600E9), " Ga") : (second >= 31557600E6 ? string.Concat(second / (31557600E6), " Ma") : (second >= 31557600E3 ? string.Concat(second / (31557600E3), " ka") : (second >= 31557600.0 ? string.Concat(second / 31557600.0, " a") : (second >= 86400.0 ? string.Concat(second / 86400.0, " d") : (second >= 3600.0 ? string.Concat(second / 3600.0, " h") : (second >= 60 ? string.Concat(second / 60.0, " min") : (second >= 1 ? string.Concat(second, " s") : string.Concat(second * 1000.0, " ms")))))))))));
                }

                return "0";
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 返回一个在合适的单位下其科学记数法具有指定位有效数字的距离（米）。
        /// </summary>
        /// <param name="meter">以米为单位的距离。</param>
        /// <param name="significance">有效数字位数。</param>
        public static double GetStandardizationDistanceOfMeter(double meter, int significance)
        {
            try
            {
                if (meter != 0)
                {
                    int Sign = Math.Sign(meter);

                    meter = Math.Abs(meter);

                    double Unit = 1;

                    if (meter >= 9460730472580800.0)
                    {
                        meter /= (Unit = 9460730472580800.0);
                    }
                    else if (meter >= 149597870700.0)
                    {
                        meter /= (Unit = 149597870700.0);
                    }
                    else if (meter >= 1000.0)
                    {
                        meter /= (Unit = 1000.0);
                    }

                    double Exp = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(meter))) - Math.Max(0, significance - 1));

                    return (Sign * Math.Floor(meter / Exp) * Exp * Unit);
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取以米为单位的一个距离的字符串格式。
        /// </summary>
        /// <param name="meter">以米为单位的距离。</param>
        public static string GetLargeDistanceStringOfMeter(double meter)
        {
            try
            {
                if (meter != 0)
                {
                    int Sign = Math.Sign(meter);

                    meter = Math.Abs(meter);

                    return string.Concat((Sign == -1 ? "-" : string.Empty), (meter >= 9460730472580800E12 ? string.Concat(meter / 9460730472580800E12, " Tly") : (meter >= 9460730472580800E9 ? string.Concat(meter / 9460730472580800E9, " Gly") : (meter >= 9460730472580800E6 ? string.Concat(meter / 9460730472580800E6, " Mly") : (meter >= 9460730472580800E3 ? string.Concat(meter / 9460730472580800E3, " kly") : (meter >= 9460730472580800.0 ? string.Concat(meter / 9460730472580800.0, " ly") : (meter >= 149597870700.0 ? string.Concat(meter / 149597870700.0, " AU") : (meter >= 1000.0 ? string.Concat(meter / 1000.0, " km") : string.Concat(meter, " m")))))))));
                }

                return "0";
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取以度分秒表示的一个以角度为单位的角度的字符串格式。
        /// </summary>
        /// <param name="degree">以角度为单位的角度。</param>
        /// <param name="decimalDigits">保留的小数位数，0 表示不保留小数，-1 表示保留所有小数。</param>
        /// <param name="cutdownIdleZeros">是否删除小数点后所有不必要的 "0"。</param>
        public static string GetAngleStringOfDegree(double degree, int decimalDigits, bool cutdownIdleZeros)
        {
            try
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
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取 64 位存储大小的字符串格式。
        /// </summary>
        /// <param name="b">存储大小的字节数。</param>
        public static string GetSize64StringFromByte(Int64 b)
        {
            try
            {
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
            catch
            {
                return string.Empty;
            }
        }
    }
}