/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.InternalUnsafeCallType
Version 20.10.27.1900

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com
{
    // 表示内部不安全调用的类型。
    [Flags]
    internal enum InternalUnsafeCallType
    {
        ContainsUnsafeCode = 1, // 被调用方法内部使用了指针或不安全代码。
        InputAddress = 2, // 被调用方法要求输入外部对象的引用或指针。
        OutputAddress = 4, // 被调用方法返回或输出内部对象的引用或指针。
        WillNotCheckParam = 8, // 被调用方法在可能引发异常或导致不正确或未定义结果的情况下对参数不进行检查。
        WillNotCheckState = 16 // 被调用方法在可能引发异常或导致不正确或未定义结果的情况下对内部状态不进行检查。
    }
}