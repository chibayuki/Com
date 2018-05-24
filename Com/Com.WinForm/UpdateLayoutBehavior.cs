﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2013-2018 chibayuki@foxmail.com

Com.WinForm.UpdateLayoutBehavior
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
    internal enum UpdateLayoutBehavior // 更新窗体布局的行为。
    {
        None = 0, // 不更新窗体布局。

        Static, // 以静态效果更新窗体布局。

        Animate // 以动画效果更新窗体布局。
    }
}