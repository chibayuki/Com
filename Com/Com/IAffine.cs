/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.IAffine
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
    /// 表示用于支持仿射变换的方法。
    /// </summary>
    [Obsolete("请改为使用 IAffineTransformable 接口")]
    public interface IAffine
    {
        /// <summary>
        /// 按双精度浮点数表示的位移对所有分量平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        void Offset(double d);

        //

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
        void AffineTransform(Matrix matrix);

        /// <summary>
        /// 按仿射矩阵数组进行仿射变换。
        /// </summary>
        /// <param name="matrices">仿射矩阵数组，对于列向量应全部为左矩阵，对于行向量应全部为右矩阵。</param>
        void AffineTransform(params Matrix[] matrices);

        /// <summary>
        /// 按仿射矩阵列表进行仿射变换。
        /// </summary>
        /// <param name="matrixList">仿射矩阵列表，对于列向量应全部为左矩阵，对于行向量应全部为右矩阵。</param>
        void AffineTransform(List<Matrix> matrixList);

        /// <summary>
        /// 按仿射矩阵进行逆仿射变换。
        /// </summary>
        /// <param name="matrix">仿射矩阵，对于列向量应为左矩阵，对于行向量应为右矩阵。</param>
        void InverseAffineTransform(Matrix matrix);

        /// <summary>
        /// 按仿射矩阵数组进行逆仿射变换。
        /// </summary>
        /// <param name="matrices">仿射矩阵数组，对于列向量应全部为左矩阵，对于行向量应全部为右矩阵。</param>
        void InverseAffineTransform(params Matrix[] matrices);

        /// <summary>
        /// 按仿射矩阵列表进行逆仿射变换。
        /// </summary>
        /// <param name="matrixList">仿射矩阵列表，对于列向量应全部为左矩阵，对于行向量应全部为右矩阵。</param>
        void InverseAffineTransform(List<Matrix> matrixList);
    }

    /// <summary>
    /// 表示用于支持仿射变换的方法。
    /// </summary>
    [Obsolete("请改为使用 IAffineTransformable<T> 接口")]
    public interface IAffine<T> : IAffine where T : IVector<double>
    {
        /// <summary>
        /// 按向量表示的位移平移指定的量。
        /// </summary>
        /// <param name="vector">向量表示的位移。</param>
        void Offset(T vector);

        /// <summary>
        /// 返回按双精度浮点数表示的位移对所有分量平移指定的量得到的向量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>T，表示按双精度浮点数表示的位移对所有分量平移指定的量得到的向量。</returns>
        T OffsetCopy(double d);

        /// <summary>
        /// 返回按向量表示的位移平移指定的量得到的向量。
        /// </summary>
        /// <param name="vector">向量表示的位移。</param>
        /// <returns>T，表示按向量表示的位移对所有分量平移指定的量得到的向量。</returns>
        T OffsetCopy(T vector);

        //

        /// <summary>
        /// 按向量表示的缩放因数缩放指定的倍数。
        /// </summary>
        /// <param name="vector">向量表示的缩放因数。</param>
        void Scale(T vector);

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数对所有分量缩放指定的倍数得到的向量。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>T，表示按双精度浮点数表示的缩放因数对所有分量缩放指定的倍数得到的向量。</returns>
        T ScaleCopy(double s);

        /// <summary>
        /// 返回按向量表示的缩放因数缩放指定的倍数得到的向量。
        /// </summary>
        /// <param name="vector">向量表示的缩放因数。</param>
        /// <returns>T，表示按向量表示的缩放因数缩放指定的倍数得到的向量。</returns>
        T ScaleCopy(T vector);

        //

        /// <summary>
        /// 返回对指定的基向量方向的分量翻转得到的向量。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        /// <returns>T，表示对指定的基向量方向的分量翻转得到的向量。</returns>
        T ReflectCopy(int index);

        //

        /// <summary>
        /// 返回按双精度浮点数表示的弧度错切指定的角度得到的向量。
        /// </summary>
        /// <param name="index1">索引，用于指定与错切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与错切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向错切的角度（弧度）。</param>
        /// <returns>T，表示按双精度浮点数表示的弧度错切指定的角度得到的向量。</returns>
        T ShearCopy(int index1, int index2, double angle);

        //

        /// <summary>
        /// 返回按双精度浮点数表示的弧度旋转指定的角度得到的向量。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        /// <returns>T，表示按双精度浮点数表示的弧度旋转指定的角度得到的向量。</returns>
        T RotateCopy(int index1, int index2, double angle);

        //

        /// <summary>
        /// 返回按仿射矩阵进行仿射变换得到的向量。
        /// </summary>
        /// <param name="matrix">仿射矩阵，对于列向量应为左矩阵，对于行向量应为右矩阵。</param>
        /// <returns>T，表示按仿射矩阵进行仿射变换得到的向量。</returns>
        T AffineTransformCopy(Matrix matrix);

        /// <summary>
        /// 返回按仿射矩阵数组进行仿射变换得到的向量。
        /// </summary>
        /// <param name="matrices">仿射矩阵数组，对于列向量应全部为左矩阵，对于行向量应全部为右矩阵。</param>
        /// <returns>T，表示按仿射矩阵数组进行仿射变换得到的向量。</returns>
        T AffineTransformCopy(params Matrix[] matrices);

        /// <summary>
        /// 返回按仿射矩阵列表进行仿射变换得到的向量。
        /// </summary>
        /// <param name="matrixList">仿射矩阵列表，对于列向量应全部为左矩阵，对于行向量应全部为右矩阵。</param>
        /// <returns>T，表示按仿射矩阵列表进行仿射变换得到的向量。</returns>
        T AffineTransformCopy(List<Matrix> matrixList);

        /// <summary>
        /// 返回按仿射矩阵进行逆仿射变换得到的向量。
        /// </summary>
        /// <param name="matrix">仿射矩阵，对于列向量应为左矩阵，对于行向量应为右矩阵。</param>
        /// <returns>T，表示按仿射矩阵进行逆仿射变换得到的向量。</returns>
        T InverseAffineTransformCopy(Matrix matrix);

        /// <summary>
        /// 返回按仿射矩阵数组进行逆仿射变换得到的向量。
        /// </summary>
        /// <param name="matrices">仿射矩阵数组，对于列向量应全部为左矩阵，对于行向量应全部为右矩阵。</param>
        /// <returns>T，表示按仿射矩阵数组进行逆仿射变换得到的向量。</returns>
        T InverseAffineTransformCopy(params Matrix[] matrices);

        /// <summary>
        /// 返回按仿射矩阵列表进行逆仿射变换得到的向量。
        /// </summary>
        /// <param name="matrixList">仿射矩阵列表，对于列向量应全部为左矩阵，对于行向量应全部为右矩阵。</param>
        /// <returns>T，表示按仿射矩阵列表进行逆仿射变换得到的向量。</returns>
        T InverseAffineTransformCopy(List<Matrix> matrixList);
    }
}