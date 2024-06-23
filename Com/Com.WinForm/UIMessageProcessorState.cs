/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.WinForm.UIMessageProcessorState
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
    /// 界面消息处理器状态。
    /// </summary>
    public enum UIMessageProcessorState
    {
        /// <summary>
        /// 消息处理器尚未开始运行消息循环或已停止运行消息循环。
        /// </summary>
        Stopped = 0,

        /// <summary>
        /// 消息处理器正在运行消息循环。
        /// </summary>
        Running,

        /// <summary>
        /// 消息处理器正在停止运行消息循环，仍有未处理完的消息在处理。
        /// </summary>
        Stopping
    }
}