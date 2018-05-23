/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2013-2018 chibayuki@foxmail.com

Com.WinForm.UpdateLayoutEventType
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
    internal enum UpdateLayoutEventType // 更新窗体布局时触发的事件类型。
    {
        None = 0, // 不触发任何事件。

        Manual, // 仅触发手动事件。

        All // 触发所有事件。
    }
}