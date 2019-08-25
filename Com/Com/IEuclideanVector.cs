/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.IEuclideanVector
Version 19.8.25.2000

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
    /// 表示欧几里得向量。
    /// </summary>
    public interface IEuclideanVector : IVector<double>
    {
        /// <summary>
        /// 获取向量的维度。
        /// </summary>
        int Dimension { get; }

        //

        /// <summary>
        /// 获取表示向量是否为零向量的布尔值。
        /// </summary>
        bool IsZero { get; }

        //

        /// <summary>
        /// 获取向量的模。
        /// </summary>
        double Module { get; }

        /// <summary>
        /// 获取向量的模的平方。
        /// </summary>
        double ModuleSquared { get; }
    }

    /// <summary>
    /// 表示欧几里得向量。
    /// </summary>
    public interface IEuclideanVector<T> : IEuclideanVector where T : IEuclideanVector<T>
    {
        /// <summary>
        /// 获取相反向量。
        /// </summary>
        T Opposite { get; }

        /// <summary>
        /// 获取规范化向量。
        /// </summary>
        T Normalize { get; }

        //

        /// <summary>
        /// 返回将向量由直角坐标系转换到极坐标系、球坐标系或超球坐标系得到的向量。
        /// </summary>
        /// <returns>T，表示将向量由直角坐标系转换到极坐标系、球坐标系或超球坐标系得到的向量。</returns>
        T ToSpherical();

        /// <summary>
        /// 返回将向量由极坐标系、球坐标系或超球坐标系转换到直角坐标系得到的向量。
        /// </summary>
        /// <returns>T，表示将向量由极坐标系、球坐标系或超球坐标系转换到直角坐标系得到的向量。</returns>
        T ToCartesian();

        //

        /// <summary>
        /// 返回与指定向量之间的距离。
        /// </summary>
        /// <param name="vector">表示起始向量。</param>
        /// <returns>双精度浮点数，表示与指定向量之间的距离。</returns>
        double DistanceFrom(T vector);

        /// <summary>
        /// 返回与指定向量之间的夹角（弧度）。
        /// </summary>
        /// <param name="vector">表示起始向量。</param>
        /// <returns>双精度浮点数，表示与指定向量之间的夹角（弧度）。</returns>
        double AngleFrom(T vector);
    }
}