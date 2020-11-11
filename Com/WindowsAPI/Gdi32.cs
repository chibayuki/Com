/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

Com.Gdi32
Version 20.10.27.1900

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
    // Gdi32 接口。
    internal static class Gdi32
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