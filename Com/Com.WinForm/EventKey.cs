﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2013-2018 chibayuki@foxmail.com

Com.WinForm.EventKey
Version 18.5.23.0000

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.WinForm
{
    internal static class EventKey // 窗体事件键值。
    {
        public static readonly object Loading = new object(); // Loading 事件键值。

        public static readonly object Loaded = new object(); // Loaded 事件键值。

        public static readonly object Closing = new object(); // Closing 事件键值。

        public static readonly object Closed = new object(); // Closed 事件键值。

        public static readonly object Move = new object(); // Move 事件键值。

        public static readonly object LocationChanged = new object(); // LocationChanged 事件键值。

        public static readonly object Resize = new object(); // Resize 事件键值。

        public static readonly object SizeChanged = new object(); // SizeChanged 事件键值。

        public static readonly object FormStyleChanged = new object(); // FormStateChanged 事件键值。

        public static readonly object FormStateChanged = new object(); // FormStateChanged 事件键值。

        public static readonly object EnabledChanged = new object(); // EnabledChanged 事件键值。

        public static readonly object CaptionChanged = new object(); // CaptionChanged 事件键值。

        public static readonly object OpacityChanged = new object(); // OpacityChanged 事件键值。

        public static readonly object ThemeChanged = new object(); // ThemeChanged 事件键值。

        public static readonly object ThemeColorChanged = new object(); // ThemeColorChanged 事件键值。
    }
}