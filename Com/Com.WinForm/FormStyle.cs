﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

Com.WinForm.FormStyle
Version 20.7.12.1800

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
    /// <summary>
    /// 窗口样式。
    /// </summary>
    public enum FormStyle
    {
        /// <summary>
        /// 大小可调的窗口。
        /// </summary>
        Sizable = 0,

        /// <summary>
        /// 大小固定的窗口。
        /// </summary>
        Fixed,
    }
}