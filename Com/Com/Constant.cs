/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.Constant
Version 19.4.7.1250

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
    internal static class Constant // 常数。
    {
        public const double Sqrt2 = 1.41421356237309505; // 表示 2 的平方根。
        public const double HalfSqrt2 = 0.70710678118654752; // 表示 2 的平方根的 1/2。

        public const double Pi = 3.1415926535897932; // 表示圆周率 π。
        public const double DoublePi = 6.2831853071795865; // 表示圆周率 π 的 2 倍。
        public const double HalfPi = 1.5707963267948966; // 表示圆周率 π 的 1/2。
        public const double SqrtDoublePi = 2.5066282746310004; // 表示圆周率 π 的 2 倍的平方根。
        public const double DegsPerRad = 57.295779513082321; // 表示 1 弧度对应的角度值，即 180 与圆周率 π 的比值。

        public const double E = 2.7182818284590451; // 表示自然常数 E。
        public const double LgE = 0.43429448190325183; // 表示自然常数 E 的常用对数。
        public const double Lg2 = 0.3010299956639812; // 表示 2 的常用对数。
        public const double Ln10 = 2.3025850929940457; // 表示 10 的自然对数。
    }
}