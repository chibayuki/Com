﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.ILinearAlgebraVector
Version 24.7.21.1040

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

namespace Com
{
    /// <summary>
    /// 表示线性代数向量。
    /// </summary>
    public interface ILinearAlgebraVector : IList, IList<double>, IVector<double>
    {
        /// <summary>
        /// 获取向量的维度。
        /// </summary>
        int Dimension { get; }

        //

        /// <summary>
        /// 获取表示向量是否不包含任何元素的布尔值。
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// 获取表示向量是否为列向量的布尔值。
        /// </summary>
        bool IsColumnVector { get; }

        /// <summary>
        /// 获取表示向量是否为行向量的布尔值。
        /// </summary>
        bool IsRowVector { get; }

        //

        /// <summary>
        /// 将向量转换为数组。
        /// </summary>
        /// <returns>T 数组，表示转换的结果。</returns>
        double[] ToArray();

        /// <summary>
        /// 将向量转换为矩阵。
        /// </summary>
        /// <returns>Matrix 对象，表示转换的结果。</returns>
        Matrix ToMatrix();
    }

    /// <summary>
    /// 表示线性代数向量。
    /// </summary>
    public interface ILinearAlgebraVector<T> : ILinearAlgebraVector where T : ILinearAlgebraVector<T>
    {
        /// <summary>
        /// 获取转置向量。
        /// </summary>
        T Transport { get; }
    }
}