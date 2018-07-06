/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.Matrix2D
Version 18.7.6.2250

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Com
{
    /// <summary>
    /// 为以双精度浮点数数组表示的二维矩阵的基本计算提供静态方法。
    /// </summary>
    public static class Matrix2D
    {
        private static bool IsEmpty(double n) // 判断双精度浮点数是否为非数字或无穷大。
        {
            try
            {
                return (double.IsNaN(n) || double.IsInfinity(n));
            }
            catch
            {
                return true;
            }
        }

        private static bool IsEmpty(double[] array) // 判断数组是否为空。
        {
            try
            {
                return (array == null || array.Length == 0);
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// 判断矩阵是否为空。
        /// </summary>
        /// <param name="matrix">矩阵。</param>
        public static bool IsEmpty(double[,] matrix)
        {
            try
            {
                return (matrix == null || matrix.Length == 0);
            }
            catch
            {
                return true;
            }
        }

        //

        /// <summary>
        /// 获取矩阵的宽度（列数）与高度（行数）。
        /// </summary>
        /// <param name="matrix">矩阵。</param>
        public static Size GetSize(double[,] matrix)
        {
            try
            {
                if (IsEmpty(matrix))
                {
                    return Size.Empty;
                }

                return new Size(matrix.GetLength(0), matrix.GetLength(1));
            }
            catch
            {
                return Size.Empty;
            }
        }

        /// <summary>
        /// 获取矩阵的秩。
        /// </summary>
        /// <param name="matrix">矩阵。</param>
        public static int GetRank(double[,] matrix)
        {
            try
            {
                if (IsEmpty(matrix))
                {
                    return -1;
                }

                Size size = GetSize(matrix);

                int order = Math.Min(size.Width, size.Height);

                int result = 0;

                for (int i = 1; i <= order; i++)
                {
                    int r = 0;

                    for (int x = 0; x < size.Width - i + 1; x++)
                    {
                        for (int y = 0; y < size.Height - i + 1; y++)
                        {
                            double[,] sub;

                            if (!SubMatrix(matrix, x, y, i, i, out sub))
                            {
                                break;
                            }

                            double det;

                            if (!Determinant(sub, out det))
                            {
                                break;
                            }

                            if (det != 0)
                            {
                                r++;
                            }
                        }
                    }

                    if (r == 0)
                    {
                        break;
                    }
                    else
                    {
                        result++;
                    }
                }

                return result;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 判断两个矩阵是否相等。
        /// </summary>
        /// <param name="matrixLeft">左矩阵。</param>
        /// <param name="matrixRight">右矩阵。</param>
        public static bool Equals(double[,] matrixLeft, double[,] matrixRight)
        {
            try
            {
                if (IsEmpty(matrixLeft) || IsEmpty(matrixRight))
                {
                    return false;
                }

                Size sizeLeft = GetSize(matrixLeft);
                Size sizeRight = GetSize(matrixRight);

                if (sizeLeft != sizeRight)
                {
                    return false;
                }

                for (int x = 0; x < sizeLeft.Width; x++)
                {
                    for (int y = 0; y < sizeLeft.Height; y++)
                    {
                        if (matrixLeft[x, y] != matrixRight[x, y])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        //

        /// <summary>
        /// 创建矩阵的副本，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrix">矩阵。</param>
        /// <param name="result">计算结果，表示矩阵的副本。</param>
        public static bool Copy(double[,] matrix, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(matrix))
                {
                    return false;
                }

                Size size = GetSize(matrix);

                result = new double[size.Width, size.Height];

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = matrix[x, y];
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 将矩阵的所有元素设为 0，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrix">矩阵。</param>
        public static bool Clear(ref double[,] matrix)
        {
            try
            {
                if (IsEmpty(matrix))
                {
                    return false;
                }

                Size size = GetSize(matrix);

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        matrix[x, y] = 0;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        //

        /// <summary>
        /// 创建一个单位矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="order">矩阵的阶数。</param>
        /// <param name="result">计算结果，表示单位矩阵。</param>
        public static bool Identity(int order, out double[,] result)
        {
            result = null;

            try
            {
                if (order <= 0)
                {
                    return false;
                }

                result = new double[order, order];

                for (int i = 0; i < order; i++)
                {
                    result[i, i] = 1;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建一个全 0 矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="width">矩阵的宽度（列数）。</param>
        /// <param name="height">矩阵的高度（行数）。</param>
        /// <param name="result">计算结果，表示全 0 矩阵。</param>
        public static bool Zeros(int width, int height, out double[,] result)
        {
            result = null;

            try
            {
                if (width <= 0 || height <= 0)
                {
                    return false;
                }

                result = new double[width, height];

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建一个全 1 矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="width">矩阵的宽度（列数）。</param>
        /// <param name="height">矩阵的高度（行数）。</param>
        /// <param name="result">计算结果，表示全 1 矩阵。</param>
        public static bool Ones(int width, int height, out double[,] result)
        {
            result = null;

            try
            {
                if (width <= 0 || height <= 0)
                {
                    return false;
                }

                result = new double[width, height];

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        result[x, y] = 1;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建一个对角矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="array">包含对角元素的数组。</param>
        /// <param name="rowsUponMainDiag">对角元素在矩阵中位于主对角线上方的行数。</param>
        /// <param name="result">计算结果，表示对角矩阵。</param>
        public static bool Diagonal(double[] array, int rowsUponMainDiag, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(array))
                {
                    return false;
                }

                int order = array.Length + Math.Abs(rowsUponMainDiag);

                result = new double[order, order];

                for (int i = 0; i < array.Length; i++)
                {
                    result[(rowsUponMainDiag >= 0 ? i + rowsUponMainDiag : i), (rowsUponMainDiag >= 0 ? i : i - rowsUponMainDiag)] = array[i];
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建一个列向量，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="array">包含向量元素的数组。</param>
        /// <param name="result">计算结果，表示列向量。</param>
        public static bool VectorColumn(double[] array, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(array))
                {
                    return false;
                }

                int height = array.Length;

                result = new double[1, height];

                for (int i = 0; i < height; i++)
                {
                    result[0, i] = array[i];
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建一个行向量，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="array">包含向量元素的数组。</param>
        /// <param name="result">计算结果，表示行向量。</param>
        public static bool VectorRow(double[] array, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(array))
                {
                    return false;
                }

                int width = array.Length;

                result = new double[width, 1];

                for (int i = 0; i < width; i++)
                {
                    result[i, 0] = array[i];
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        //

        /// <summary>
        /// 创建矩阵的子矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrix">矩阵。</param>
        /// <param name="x">子矩阵首列位于原矩阵的 X 索引。</param>
        /// <param name="y">子矩阵首行位于原矩阵的 Y 索引。</param>
        /// <param name="width">子矩阵的宽度（列数）。</param>
        /// <param name="height">子矩阵的高度（行数）。</param>
        /// <param name="result">计算结果，表示子矩阵。</param>
        public static bool SubMatrix(double[,] matrix, int x, int y, int width, int height, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(matrix))
                {
                    return false;
                }

                Size size = GetSize(matrix);

                if (x + width > size.Width || y + height > size.Height)
                {
                    return false;
                }

                result = new double[width, height];

                for (int _x = 0; _x < width; _x++)
                {
                    for (int _y = 0; _y < height; _y++)
                    {
                        result[_x, _y] = matrix[x + _x, y + _y];
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建增广矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrixLeft">左矩阵。</param>
        /// <param name="matrixRight">右矩阵。</param>
        /// <param name="result">计算结果，表示增广矩阵。</param>
        public static bool AugmentedMatrix(double[,] matrixLeft, double[,] matrixRight, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(matrixLeft) || IsEmpty(matrixRight))
                {
                    return false;
                }

                Size sizeLeft = GetSize(matrixLeft);
                Size sizeRight = GetSize(matrixRight);

                if (sizeLeft.Height != sizeRight.Height)
                {
                    return false;
                }

                Size size = new Size(sizeLeft.Width + sizeRight.Width, sizeLeft.Height);

                result = new double[size.Width, size.Height];

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = (x < sizeLeft.Width ? matrixLeft[x, y] : matrixRight[x - sizeLeft.Width, y]);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        //

        /// <summary>
        /// 计算矩阵与标量相加得到的矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrix">矩阵，表示被加数。</param>
        /// <param name="n">标量，表示加数。</param>
        /// <param name="result">计算结果，表示矩阵与标量相加的和。</param>
        public static bool Add(double[,] matrix, double n, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(matrix) || IsEmpty(n))
                {
                    return false;
                }

                Size size = GetSize(matrix);

                result = new double[size.Width, size.Height];

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = matrix[x, y] + n;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算标量与矩阵相加得到的矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="n">标量，表示被加数。</param>
        /// <param name="matrix">矩阵，表示加数。</param>
        /// <param name="result">计算结果，表示标量与矩阵相加的和。</param>
        public static bool Add(double n, double[,] matrix, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(n) || IsEmpty(matrix))
                {
                    return false;
                }

                Size size = GetSize(matrix);

                result = new double[size.Width, size.Height];

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = n + matrix[x, y];
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算矩阵与矩阵相加得到的矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrixLeft">左矩阵，表示被加数。</param>
        /// <param name="matrixRight">右矩阵，表示加数。</param>
        /// <param name="result">计算结果，表示矩阵与矩阵相加的和。</param>
        public static bool Add(double[,] matrixLeft, double[,] matrixRight, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(matrixLeft) || IsEmpty(matrixRight))
                {
                    return false;
                }

                Size sizeLeft = GetSize(matrixLeft);
                Size sizeRight = GetSize(matrixRight);

                if (sizeLeft != sizeRight)
                {
                    return false;
                }

                Size size = sizeLeft;

                result = new double[size.Width, size.Height];

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = matrixLeft[x, y] + matrixRight[x, y];
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算矩阵与标量相减得到的矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrix">矩阵，表示被减数。</param>
        /// <param name="n">标量，表示减数。</param>
        /// <param name="result">计算结果，表示矩阵与标量相减的差。</param>
        public static bool Subtract(double[,] matrix, double n, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(matrix) || IsEmpty(n))
                {
                    return false;
                }

                Size size = GetSize(matrix);

                result = new double[size.Width, size.Height];

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = matrix[x, y] - n;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算标量与矩阵相减得到的矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="n">标量，表示被减数。</param>
        /// <param name="matrix">矩阵，表示减数。</param>
        /// <param name="result">计算结果，表示标量与矩阵相减的差。</param>
        public static bool Subtract(double n, double[,] matrix, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(n) || IsEmpty(matrix))
                {
                    return false;
                }

                Size size = GetSize(matrix);

                result = new double[size.Width, size.Height];

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = n - matrix[x, y];
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算矩阵与矩阵相减得到的矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrixLeft">左矩阵，表示被减数。</param>
        /// <param name="matrixRight">右矩阵，表示减数。</param>
        /// <param name="result">计算结果，表示矩阵与矩阵相减的差。</param>
        public static bool Subtract(double[,] matrixLeft, double[,] matrixRight, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(matrixLeft) || IsEmpty(matrixRight))
                {
                    return false;
                }

                Size sizeLeft = GetSize(matrixLeft);
                Size sizeRight = GetSize(matrixRight);

                if (sizeLeft != sizeRight)
                {
                    return false;
                }

                Size size = sizeLeft;

                result = new double[size.Width, size.Height];

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = matrixLeft[x, y] - matrixRight[x, y];
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算矩阵与标量相乘得到的矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrix">矩阵，表示被乘数。</param>
        /// <param name="n">标量，表示乘数。</param>
        /// <param name="result">计算结果，表示矩阵与标量相乘的积。</param>
        public static bool Multiply(double[,] matrix, double n, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(matrix) || IsEmpty(n))
                {
                    return false;
                }

                Size size = GetSize(matrix);

                result = new double[size.Width, size.Height];

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = matrix[x, y] * n;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算标量与矩阵相乘得到的矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="n">标量，表示被乘数。</param>
        /// <param name="matrix">矩阵，表示乘数。</param>
        /// <param name="result">计算结果，表示标量与矩阵相乘的积。</param>
        public static bool Multiply(double n, double[,] matrix, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(n) || IsEmpty(matrix))
                {
                    return false;
                }

                Size size = GetSize(matrix);

                result = new double[size.Width, size.Height];

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = n * matrix[x, y];
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算矩阵与矩阵相乘得到的矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrixLeft">左矩阵，表示被乘数。</param>
        /// <param name="matrixRight">右矩阵，表示乘数。</param>
        /// <param name="result">计算结果，表示矩阵与矩阵相乘的积。</param>
        public static bool Multiply(double[,] matrixLeft, double[,] matrixRight, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(matrixLeft) || IsEmpty(matrixRight))
                {
                    return false;
                }

                Size sizeLeft = GetSize(matrixLeft);
                Size sizeRight = GetSize(matrixRight);

                if (sizeLeft.Width != sizeRight.Height)
                {
                    return false;
                }

                int height = sizeLeft.Width;

                result = new double[sizeRight.Width, sizeLeft.Height];

                for (int x = 0; x < sizeRight.Width; x++)
                {
                    for (int y = 0; y < sizeLeft.Height; y++)
                    {
                        result[x, y] = 0;

                        for (int i = 0; i < height; i++)
                        {
                            result[x, y] += (matrixLeft[i, y] * matrixRight[x, i]);
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算将列表中所有矩阵依次左乘得到的矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrixLeftList">左矩阵列表。</param>
        /// <param name="result">计算结果，表示将列表中所有矩阵依次左乘的积。</param>
        public static bool MultiplyLeft(List<double[,]> matrixLeftList, out double[,] result)
        {
            result = null;

            try
            {
                if (matrixLeftList.Count > 0)
                {
                    if (matrixLeftList.Count == 1)
                    {
                        if (Copy(matrixLeftList[0], out result))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        double[,] matrixRight;

                        if (Copy(matrixLeftList[0], out matrixRight))
                        {
                            for (int i = 1; i < matrixLeftList.Count; i++)
                            {
                                bool flag = true;

                                if (flag)
                                {
                                    flag = Multiply(matrixLeftList[i], matrixRight, out result);
                                }

                                if (flag)
                                {
                                    flag = Copy(result, out matrixRight);
                                }

                                if (!flag)
                                {
                                    return false;
                                }
                            }

                            return true;
                        }
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算将列表中所有矩阵依次右乘得到的矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrixRightList">右矩阵列表。</param>
        /// <param name="result">计算结果，表示将列表中所有矩阵依次右乘的积。</param>
        public static bool MultiplyRight(List<double[,]> matrixRightList, out double[,] result)
        {
            result = null;

            try
            {
                if (matrixRightList.Count > 0)
                {
                    if (matrixRightList.Count == 1)
                    {
                        if (Copy(matrixRightList[0], out result))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        double[,] matrixLeft;

                        if (Copy(matrixRightList[0], out matrixLeft))
                        {
                            for (int i = 1; i < matrixRightList.Count; i++)
                            {
                                bool flag = true;

                                if (flag)
                                {
                                    flag = Multiply(matrixLeft, matrixRightList[i], out result);
                                }

                                if (flag)
                                {
                                    flag = Copy(result, out matrixLeft);
                                }

                                if (!flag)
                                {
                                    return false;
                                }
                            }

                            return true;
                        }
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算矩阵与标量相除得到的矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrix">矩阵，表示被除数。</param>
        /// <param name="n">标量，表示除数。</param>
        /// <param name="result">计算结果，表示矩阵与标量相除的商。</param>
        public static bool Divide(double[,] matrix, double n, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(matrix) || IsEmpty(n))
                {
                    return false;
                }

                Size size = GetSize(matrix);

                result = new double[size.Width, size.Height];

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = matrix[x, y] / n;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算标量与矩阵相除得到的矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="n">标量，表示被除数。</param>
        /// <param name="matrix">矩阵，表示除数。</param>
        /// <param name="result">计算结果，表示标量与矩阵相除的商。</param>
        public static bool Divide(double n, double[,] matrix, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(n) || IsEmpty(matrix))
                {
                    return false;
                }

                Size size = GetSize(matrix);

                result = new double[size.Width, size.Height];

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = n / matrix[x, y];
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算矩阵与矩阵左除得到的矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrixLeft">左矩阵，表示被除数。</param>
        /// <param name="matrixRight">右矩阵，表示除数。</param>
        /// <param name="result">计算结果，表示矩阵与矩阵左除的商。</param>
        public static bool DivideLeft(double[,] matrixLeft, double[,] matrixRight, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(matrixLeft) || IsEmpty(matrixRight))
                {
                    return false;
                }

                Size sizeLeft = GetSize(matrixLeft);
                Size sizeRight = GetSize(matrixRight);

                if (sizeLeft.Width != sizeLeft.Height || sizeLeft.Width != sizeRight.Height)
                {
                    return false;
                }

                double[,] invLeft;

                if (!Invert(matrixLeft, out invLeft))
                {
                    return false;
                }

                if (!Multiply(invLeft, matrixRight, out result))
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算矩阵与矩阵右除得到的矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrixLeft">左矩阵，表示被除数。</param>
        /// <param name="matrixRight">右矩阵，表示除数。</param>
        /// <param name="result">计算结果，表示矩阵与矩阵右除的商。</param>
        public static bool DivideRight(double[,] matrixLeft, double[,] matrixRight, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(matrixLeft) || IsEmpty(matrixRight))
                {
                    return false;
                }

                Size sizeLeft = GetSize(matrixLeft);
                Size sizeRight = GetSize(matrixRight);

                if (sizeRight.Width != sizeRight.Height || sizeLeft.Width != sizeRight.Height)
                {
                    return false;
                }

                double[,] invRight;

                if (!Invert(matrixRight, out invRight))
                {
                    return false;
                }

                if (!Multiply(matrixLeft, invRight, out result))
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        //

        /// <summary>
        /// 计算矩阵的转置，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrix">矩阵。</param>
        /// <param name="result">计算结果，表示转置矩阵。</param>
        public static bool Transport(double[,] matrix, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(matrix))
                {
                    return false;
                }

                Size size = GetSize(matrix);

                result = new double[size.Height, size.Width];

                for (int x = 0; x < size.Height; x++)
                {
                    for (int y = 0; y < size.Width; y++)
                    {
                        result[x, y] = matrix[y, x];
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算矩阵的行列式值，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrix">矩阵。</param>
        /// <param name="result">计算结果，表示矩阵的行列式值。</param>
        public static bool Determinant(double[,] matrix, out double result)
        {
            result = double.NaN;

            try
            {
                if (IsEmpty(matrix))
                {
                    return false;
                }

                Size size = GetSize(matrix);

                if (size.Width != size.Height)
                {
                    return false;
                }

                int order = size.Width;

                if (order == 0)
                {
                    result = 0;

                    return true;
                }
                else if (order == 1)
                {
                    result = matrix[0, 0];

                    return true;
                }
                else if (order == 2)
                {
                    result = (matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1]);

                    return true;
                }

                double det = 0, detSign = 1;

                for (int i = 0; i < order; i++)
                {
                    Size sizeTemp = new Size(order - 1, order - 1);

                    double[,] temp = new double[sizeTemp.Width, sizeTemp.Height];

                    for (int x = 0; x < sizeTemp.Width; x++)
                    {
                        for (int y = 0; y < sizeTemp.Height; y++)
                        {
                            temp[x, y] = matrix[x + 1, (y >= i ? y + 1 : y)];
                        }
                    }

                    double detTemp;

                    if (!Determinant(temp, out detTemp))
                    {
                        return false;
                    }

                    det += (matrix[0, i] * detSign * detTemp);
                    detSign = detSign * -1;
                }

                result = det;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算伴随矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrix">矩阵。</param>
        /// <param name="result">计算结果，表示伴随矩阵。</param>
        public static bool Adjoint(double[,] matrix, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(matrix))
                {
                    return false;
                }

                Size size = GetSize(matrix);

                if (size.Width != size.Height)
                {
                    return false;
                }

                result = new double[size.Width, size.Height];

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        double[,] temp = new double[size.Width - 1, size.Height - 1];

                        for (int _x = 0; _x < size.Width - 1; _x++)
                        {
                            for (int _y = 0; _y < size.Height - 1; _y++)
                            {
                                temp[_x, _y] = matrix[(_x < x ? _x : _x + 1), (_y < y ? _y : _y + 1)];
                            }
                        }

                        double det;

                        if (!Determinant(temp, out det))
                        {
                            result = null;

                            return false;
                        }

                        result[y, x] = ((x + y) % 2 == 0 ? 1 : -1) * det;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算逆矩阵，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrix">矩阵。</param>
        /// <param name="result">计算结果，表示逆矩阵。</param>
        public static bool Invert(double[,] matrix, out double[,] result)
        {
            result = null;

            try
            {
                if (IsEmpty(matrix))
                {
                    return false;
                }

                Size size = GetSize(matrix);

                if (size.Width != size.Height)
                {
                    return false;
                }

                if (!Adjoint(matrix, out result))
                {
                    return false;
                }

                double det;

                if (!Determinant(matrix, out det))
                {
                    return false;
                }

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] /= det;

                        if (IsEmpty(result[x, y]))
                        {
                            result = null;

                            return false;
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        //

        /// <summary>
        /// 计算非齐次线性方程组的解向量，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="matrix">系数矩阵。</param>
        /// <param name="array">常数项。</param>
        /// <param name="result">计算结果，表示解向量。</param>
        public static bool SolveLinearEquation(double[,] matrix, double[] array, out double[] result)
        {
            result = null;

            try
            {
                if (IsEmpty(matrix) || IsEmpty(array))
                {
                    return false;
                }

                Size size = GetSize(matrix);

                if (size.Width != size.Height)
                {
                    return false;
                }

                int order = array.Length;

                if (order != size.Height)
                {
                    return false;
                }

                double[,] vectorColumn;

                if (!VectorColumn(array, out vectorColumn))
                {
                    return false;
                }

                double[,] solution;

                if (!DivideLeft(matrix, vectorColumn, out solution))
                {
                    return false;
                }

                if (GetSize(solution) != new Size(1, order))
                {
                    return false;
                }

                result = new double[order];

                for (int i = 0; i < order; i++)
                {
                    result[i] = solution[0, i];
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}