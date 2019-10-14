/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.InternalMethod
Version 19.10.14.2100

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
    internal static class InternalMethod // 内部方法。
    {
        public static bool IsNaNOrInfinity(double n) // 判断双精度浮点数是否为非数字或无穷大。
        {
            return (double.IsNaN(n) || double.IsInfinity(n));
        }

        public static bool IsNaNOrInfinity(float n) // 判断单精度浮点数是否为非数字或无穷大。
        {
            return (float.IsNaN(n) || float.IsInfinity(n));
        }

        //

        public static bool IsNullOrEmpty(Array array) // 判断数组是否为 null 或不包含任何元素。
        {
            return (array == null || array.Length <= 0);
        }

        public static bool IsNullOrEmpty<T>(List<T> list) // 判断列表是否为 null 或不包含任何元素。
        {
            return (list == null || list.Count <= 0);
        }
    }
}