/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.VectorType
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
    /// <summary>
    /// 向量类型。
    /// </summary>
    public enum VectorType
    {
        /// <summary>
        /// 列向量。
        /// </summary>
        ColumnVector = 0,

        /// <summary>
        /// 行向量。
        /// </summary>
        RowVector
    }
}