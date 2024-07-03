/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.WinForm.Effect
Version 20.10.27.1900

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
    /// 窗口交互过程显示的效果。
    /// </summary>
    [Flags]
    public enum Effect
    {
        /// <summary>
        /// 不显示任何效果。
        /// </summary>
        None = 0,

        /// <summary>
        /// 显示平滑位移效果。
        /// </summary>
        SmoothShift = 256,

        /// <summary>
        /// 显示淡入淡出效果。
        /// </summary>
        Fade = 512,

        /// <summary>
        /// 显示所有效果。
        /// </summary>
        All = SmoothShift | Fade
    }
}