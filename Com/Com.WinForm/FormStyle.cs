/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2013-2018 chibayuki@foxmail.com

Com.WinForm.FormStyle
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
    /// <summary>
    /// 窗体样式枚举。
    /// </summary>
    public enum FormStyle
    {
        /// <summary>
        /// 大小可调的窗体。
        /// </summary>
        Sizable = 0,

        /// <summary>
        /// 大小固定的窗体。
        /// </summary>
        Fixed,

        /// <summary>
        /// 大小固定的对话框。
        /// </summary>
        Dialog
    }
}