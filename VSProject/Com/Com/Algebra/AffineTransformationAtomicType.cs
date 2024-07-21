/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.AffineTransformationAtomicType
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
    // 仿射变换的原子操作类型。
    internal enum AffineTransformationAtomicType
    {
        Offset = 1, // 平移变换。
        OffsetMulti = 2, // 平移变换（复合）。

        Scale = 4, // 缩放变换。
        ScaleMulti = 8, // 缩放变换（复合）。

        Reflect = 16, // 翻转变换。

        Shear = 32, // 错切变换。

        Rotate = 64, // 旋转变换。

        Matrix = 128 // 以矩阵为参数的仿射变换。
    }
}