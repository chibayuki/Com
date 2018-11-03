﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.IVector
Version 18.10.31.0000

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
    /// 表示向量。向量是包含确定数量与值的元素的可迭代的有限有序集合。
    /// </summary>
    public interface IVector<T> : IList, IList<T>
    {
        /// <summary>
        /// 获取或设置向量包含的元素数量。
        /// </summary>
        int Size { get; set; }

        /// <summary>
        /// 获取向量在其内部数据结构不调整大小的情况下能够容纳的元素数量。
        /// </summary>
        int Capacity { get; }

        //

        /// <summary>
        /// 获取表示向量是否不包含任何元素的布尔值。
        /// </summary>
        bool IsEmpty { get; }

        //

        /// <summary>
        /// 释放向量内部数据结构的冗余空间。
        /// </summary>
        void Trim();

        //

        /// <summary>
        /// 将向量转换为数组。
        /// </summary>
        T[] ToArray();

        /// <summary>
        /// 将向量转换为列表。
        /// </summary>
        List<T> ToList();
    }
}