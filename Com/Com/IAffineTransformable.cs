/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2022 chibayuki@foxmail.com

Com.IAffineTransform
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
    /// 表示支持仿射变换。
    /// </summary>
    public interface IAffineTransformable
    {
        /// <summary>
        /// 按双精度浮点数表示的位移对指定的基向量方向的分量平移指定的量。
        /// </summary>
        /// <param name="index">索引，用于指定平移的分量所在方向的基向量。</param>
        /// <param name="d">双精度浮点数表示的位移。</param>
        void Offset(int index, double d);

        /// <summary>
        /// 按双精度浮点数表示的位移对所有分量平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        void Offset(double d);

        //

        /// <summary>
        /// 按双精度浮点数表示的缩放因数对指定的基向量方向的分量缩放指定的倍数。
        /// </summary>
        /// <param name="index">索引，用于指定缩放的分量所在方向的基向量。</param>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        void Scale(int index, double s);

        /// <summary>
        /// 按双精度浮点数表示的缩放因数对所有分量缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        void Scale(double s);

        //

        /// <summary>
        /// 对指定的基向量方向的分量翻转。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        void Reflect(int index);

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度错切指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定与错切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与错切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向错切的角度（弧度）。</param>
        void Shear(int index1, int index2, double angle);

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度旋转指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        void Rotate(int index1, int index2, double angle);

        //

        /// <summary>
        /// 按仿射矩阵进行仿射变换。
        /// </summary>
        /// <param name="matrix">仿射矩阵，对于列向量应为左矩阵，对于行向量应为右矩阵。</param>
        void MatrixTransform(Matrix matrix);

        //

        /// <summary>
        /// 按 AffineTransformation 对象进行仿射变换。
        /// </summary>
        /// <param name="affineTransformation">AffineTransformation 对象。</param>
        void AffineTransform(AffineTransformation affineTransformation);
    }

    /// <summary>
    /// 表示支持仿射变换。
    /// </summary>
    public interface IAffineTransformable<T> : IAffineTransformable where T : IAffineTransformable<T>
    {
        /// <summary>
        /// 返回按双精度浮点数表示的位移对指定的基向量方向的分量平移指定的量得到的结果。
        /// </summary>
        /// <param name="index">索引，用于指定平移的分量所在方向的基向量。</param>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>T，表示按双精度浮点数表示的位移对指定的基向量方向的分量平移指定的量得到的结果。</returns>
        T OffsetCopy(int index, double d);

        /// <summary>
        /// 返回按双精度浮点数表示的位移对所有分量平移指定的量得到的结果。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>T，表示按双精度浮点数表示的位移对所有分量平移指定的量得到的结果。</returns>
        T OffsetCopy(double d);

        //

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数对指定的基向量方向的分量缩放指定的倍数得到的结果。
        /// </summary>
        /// <param name="index">索引，用于指定缩放的分量所在方向的基向量。</param>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>T，表示按双精度浮点数表示的缩放因数对指定的基向量方向的分量缩放指定的倍数得到的结果。</returns>
        T ScaleCopy(int index, double s);

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数对所有分量缩放指定的倍数得到的结果。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>T，表示按双精度浮点数表示的缩放因数对所有分量缩放指定的倍数得到的结果。</returns>
        T ScaleCopy(double s);

        //

        /// <summary>
        /// 返回对指定的基向量方向的分量翻转得到的结果。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        /// <returns>T，表示对指定的基向量方向的分量翻转得到的结果。</returns>
        T ReflectCopy(int index);

        //

        /// <summary>
        /// 返回按双精度浮点数表示的弧度错切指定的角度得到的结果。
        /// </summary>
        /// <param name="index1">索引，用于指定与错切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与错切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向错切的角度（弧度）。</param>
        /// <returns>T，表示按双精度浮点数表示的弧度错切指定的角度得到的结果。</returns>
        T ShearCopy(int index1, int index2, double angle);

        //

        /// <summary>
        /// 返回按双精度浮点数表示的弧度旋转指定的角度得到的结果。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        /// <returns>T，表示按双精度浮点数表示的弧度旋转指定的角度得到的结果。</returns>
        T RotateCopy(int index1, int index2, double angle);

        //

        /// <summary>
        /// 返回按仿射矩阵进行仿射变换得到的结果。
        /// </summary>
        /// <param name="matrix">仿射矩阵，对于列向量应为左矩阵，对于行向量应为右矩阵。</param>
        /// <returns>T，表示按仿射矩阵进行仿射变换得到的结果。</returns>
        T MatrixTransformCopy(Matrix matrix);

        //

        /// <summary>
        /// 返回按 AffineTransformation 对象进行仿射变换得到的结果。
        /// </summary>
        /// <param name="affineTransformation">AffineTransformation 对象。</param>
        /// <returns>T，表示按 AffineTransformation 对象进行仿射变换得到的结果。</returns>
        T AffineTransformCopy(AffineTransformation affineTransformation);
    }
}