/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

Com.InternalUnsafeCallAttribute
Version 20.5.28.2000

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

    // 表示内部不安全调用。
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    internal sealed class InternalUnsafeCallAttribute : Attribute
    {
        private readonly InternalUnsafeCallType _Type; // 此内部不安全调用的类型。
        private string _Message; // 此内部不安全调用的说明信息。

        //

        // 使用 InternalUnsafeCallType 初始化 InternalUnsafeCallAttribute 的新实例。
        public InternalUnsafeCallAttribute(InternalUnsafeCallType type)
        {
            _Type = type;
            _Message = string.Empty;
        }

        //

        // 获取此内部不安全调用的类型。
        public InternalUnsafeCallType Type
        {
            get
            {
                return _Type;
            }
        }

        // 获取或设置此内部不安全调用的说明信息。
        public string Message
        {
            get
            {
                return _Message;
            }

            set
            {
                _Message = value;
            }
        }
    }
}