/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.InternalUnsafeCallAttribute
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
        public InternalUnsafeCallType Type => _Type;

        // 获取或设置此内部不安全调用的说明信息。
        public string Message
        {
            get => _Message;
            set => _Message = value;
        }
    }
}