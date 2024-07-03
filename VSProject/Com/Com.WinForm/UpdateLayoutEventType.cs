/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.WinForm.UpdateLayoutEventType
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
    [Flags]
    internal enum UpdateLayoutEventType // 更新窗口布局时触发的事件类型。
    {
        None = 0, // 不触发任何事件。

        Move = 256, // 触发 Move 事件。

        Resize = 512, // 触发 Resize 事件。

        LocationChanged = 65536, // 触发 LocationChanged 事件。

        SizeChanged = 131072, // 触发 SizeChanged 事件。

        Process = Move | Resize, // 触发过程事件（Move 与 Resize）。

        Result = LocationChanged | SizeChanged // 触发结果事件（LocationChanged 与 SizeChanged）。
    }
}