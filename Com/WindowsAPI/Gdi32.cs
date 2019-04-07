/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.Gdi32
Version 19.4.7.1250

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace Com
{
    internal static class Gdi32 // Gdi32 接口。
    {
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateCompatibleDC", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDc);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteDC", SetLastError = true)]
        public static extern int DeleteDC(IntPtr hDc);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SelectObject", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDc, IntPtr hGdiObj);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteObject", SetLastError = true)]
        public static extern int DeleteObject(IntPtr hGdiObj);
    }
}