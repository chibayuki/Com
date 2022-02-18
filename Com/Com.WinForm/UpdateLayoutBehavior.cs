/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2022 chibayuki@foxmail.com

Com.WinForm.UpdateLayoutBehavior
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
    internal enum UpdateLayoutBehavior // 更新窗口布局的行为。
    {
        None = 0, // 不更新窗口布局。

        Static, // 以静态效果更新窗口布局。

        Animate // 以动画效果更新窗口布局。
    }
}