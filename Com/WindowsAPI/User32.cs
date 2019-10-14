/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.User32
Version 19.10.14.2100

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Runtime.InteropServices;

namespace Com
{
    internal static class User32 // User32 接口。
    {
        [DllImport("user32", CharSet = CharSet.Ansi, EntryPoint = "GetWindowLongA", ExactSpelling = true, SetLastError = true)]
        public static extern int GetWindowLongA(int hWnd, int nlndex);

        [DllImport("user32", CharSet = CharSet.Ansi, EntryPoint = "SetWindowLongA", ExactSpelling = true, SetLastError = true)]
        public static extern int SetWindowLongA(int hWnd, int nlndex, int dwNewLong);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetDC", SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "ReleaseDC", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "UpdateLayeredWindow", SetLastError = true)]
        public static extern int UpdateLayeredWindow(IntPtr hWnd, IntPtr hDcDst, ref Point pPtDst, ref Size pSize, IntPtr hDcSrc, ref Point pPtSrc, int crKey, ref BLENDFUNCTION pBlend, int dwFlags);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }
    }
}