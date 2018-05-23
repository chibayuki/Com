/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2013-2018 chibayuki@foxmail.com

Com.WinForm.FormState
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
    /// 窗体状态枚举。
    /// </summary>
    public enum FormState
    {
        /// <summary>
        /// 普通大小的窗口。
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 最小化的窗口。
        /// </summary>
        Minimized,

        /// <summary>
        /// 最大化的窗口。
        /// </summary>
        Maximized,

        /// <summary>
        /// 全屏幕的窗口。
        /// </summary>
        FullScreen,

        /// <summary>
        /// 与桌面高度相同的窗口。
        /// </summary>
        HighAsScreen,

        /// <summary>
        /// 占据桌面四分之一的窗口。
        /// </summary>
        QuarterScreen
    }
}