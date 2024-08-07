﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.InternalMethod
Version 24.7.21.1040

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
    // 内部方法。
    internal static class InternalMethod
    {
        // 判断单精度浮点数是否为非数字或无穷大。
        public static bool IsNaNOrInfinity(float n) => float.IsNaN(n) || float.IsInfinity(n);

        // 判断双精度浮点数是否为非数字或无穷大。
        public static bool IsNaNOrInfinity(double n) => double.IsNaN(n) || double.IsInfinity(n);

        //

        // 判断数组是否为 null 或不包含任何元素。
        public static bool IsNullOrEmpty(Array array) => array is null || array.Length <= 0;

        // 判断一维数组是否为 null 或不包含任何元素。
        public static bool IsNullOrEmpty<T>(T[] array) => array is null || array.Length <= 0;

        // 判断集合是否为 null 或不包含任何元素。
        public static bool IsNullOrEmpty<T>(IReadOnlyCollection<T> obj) => obj is null || obj.Count <= 0;

        // 判断可枚举对象是否为 null 或不包含任何元素。
        public static bool IsNullOrEmpty<T>(IEnumerable<T> obj) => obj is null || !obj.Any();
    }
}