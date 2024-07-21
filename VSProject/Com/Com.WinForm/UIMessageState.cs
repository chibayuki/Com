/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.WinForm.UIMessageState
Version 24.7.21.1040

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
    /// 界面消息状态。
    /// </summary>
    public enum UIMessageState
    {
        /// <summary>
        /// 已创建消息，但尚未推送给消息处理器。
        /// </summary>
        Created = 0,

        /// <summary>
        /// 此消息已推送给消息处理器，但消息处理器尚未开始处理。
        /// </summary>
        WaitingToProcess,

        /// <summary>
        /// 消息处理器已开始处理此消息，但尚未完成。
        /// </summary>
        Processing,

        /// <summary>
        /// 消息处理器已成功处理此消息。
        /// </summary>
        ProcessCompleted,

        /// <summary>
        /// 消息处理器已处理此消息，但发生了异常。
        /// </summary>
        ProcessFailed,

        /// <summary>
        /// 消息处理器已丢弃此消息。
        /// </summary>
        Discarded
    }
}