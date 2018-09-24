/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.Matrix
Version 18.9.24.1600

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
    /// 以双精度浮点数二维数组表示的矩阵。
    /// </summary>
    public sealed class Matrix
    {
        #region 私有与内部成员

        private Size _Size; // 此 Matrix 存储的矩阵大小。

        private double[,] _MArray = null; // 用于存储矩阵元素的数组。

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用指定的宽度（列数）与高度（行数）初始化 Matrix 的新实例。
        /// </summary>
        /// <param name="size">矩阵的宽度（列数）与高度（行数）。</param>
        public Matrix(Size size)
        {
            if (size.Width > 0 && size.Height > 0)
            {
                _Size = size;
                _MArray = new double[_Size.Width, _Size.Height];
            }
            else
            {
                _Size = Size.Empty;
                _MArray = null;
            }
        }

        /// <summary>
        /// 使用指定的宽度（列数）、高度（行数）与所有元素的值初始化 Matrix 的新实例。
        /// </summary>
        /// <param name="size">矩阵的宽度（列数）与高度（行数）。</param>
        /// <param name="value">矩阵的所有元素的值。</param>
        public Matrix(Size size, double value)
        {
            if (size.Width > 0 && size.Height > 0)
            {
                _Size = size;
                _MArray = new double[_Size.Width, _Size.Height];

                for (int x = 0; x < _Size.Width; x++)
                {
                    for (int y = 0; y < _Size.Height; y++)
                    {
                        _MArray[x, y] = value;
                    }
                }
            }
            else
            {
                _Size = Size.Empty;
                _MArray = null;
            }
        }

        /// <summary>
        /// 使用指定的宽度（列数）与高度（行数）初始化 Matrix 的新实例。
        /// </summary>
        /// <param name="width">矩阵的宽度（列数）。</param>
        /// <param name="height">矩阵的高度（行数）。</param>
        public Matrix(int width, int height)
        {
            if (width > 0 && height > 0)
            {
                _Size = new Size(width, height);
                _MArray = new double[_Size.Width, _Size.Height];
            }
            else
            {
                _Size = Size.Empty;
                _MArray = null;
            }
        }

        /// <summary>
        /// 使用指定的宽度（列数）、高度（行数）与所有元素的值初始化 Matrix 的新实例。
        /// </summary>
        /// <param name="width">矩阵的宽度（列数）。</param>
        /// <param name="height">矩阵的高度（行数）。</param>
        /// <param name="value">矩阵的所有元素的值。</param>
        public Matrix(int width, int height, double value)
        {
            if (width > 0 && height > 0)
            {
                _Size = new Size(width, height);
                _MArray = new double[_Size.Width, _Size.Height];

                for (int x = 0; x < _Size.Width; x++)
                {
                    for (int y = 0; y < _Size.Height; y++)
                    {
                        _MArray[x, y] = value;
                    }
                }
            }
            else
            {
                _Size = Size.Empty;
                _MArray = null;
            }
        }

        /// <summary>
        /// 使用二维双精度浮点数数组表示的矩阵元素初始化 Matrix 的新实例。
        /// </summary>
        /// <param name="values">二维数组表示的矩阵元素。</param>
        public Matrix(double[,] values)
        {
            if (!InternalMethod.IsNullOrEmpty(values))
            {
                _Size = new Size(values.GetLength(0), values.GetLength(1));
                _MArray = new double[_Size.Width, _Size.Height];

                for (int x = 0; x < _Size.Width; x++)
                {
                    for (int y = 0; y < _Size.Height; y++)
                    {
                        _MArray[x, y] = values[x, y];
                    }
                }
            }
            else
            {
                _Size = Size.Empty;
                _MArray = null;
            }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置此 Matrix 的指定索引的元素。
        /// </summary>
        /// <param name="x">矩阵的宽度方向（列）的索引。</param>
        /// <param name="y">矩阵的高度方向（行）的索引。</param>
        public double this[int x, int y]
        {
            get
            {
                if (_Size.Width > 0 && _Size.Height > 0 && (x >= 0 && x < _Size.Width) && (y >= 0 && y < _Size.Height))
                {
                    return _MArray[x, y];
                }

                return double.NaN;
            }

            set
            {
                if (_Size.Width > 0 && _Size.Height > 0 && (x >= 0 && x < _Size.Width) && (y >= 0 && y < _Size.Height))
                {
                    _MArray[x, y] = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置此 Matrix 的指定索引的元素。
        /// </summary>
        /// <param name="index">矩阵的宽度方向（列）与高度方向（行）的索引。</param>
        public double this[Point index]
        {
            get
            {
                if (_Size.Width > 0 && _Size.Height > 0 && (index.X >= 0 && index.X < _Size.Width) && (index.Y >= 0 && index.Y < _Size.Height))
                {
                    return _MArray[index.X, index.Y];
                }

                return double.NaN;
            }

            set
            {
                if (_Size.Width > 0 && _Size.Height > 0 && (index.X >= 0 && index.X < _Size.Width) && (index.Y >= 0 && index.Y < _Size.Height))
                {
                    _MArray[index.X, index.Y] = value;
                }
            }
        }

        //

        /// <summary>
        /// 获取表示此 Matrix 是否为非矩阵的布尔值。
        /// </summary>
        public bool IsNonMatrix
        {
            get
            {
                return (_Size.Width <= 0 || _Size.Height <= 0);
            }
        }

        //

        /// <summary>
        /// 获取此 Matrix 的宽度（列数）与高度（行数）。
        /// </summary>
        public Size Size
        {
            get
            {
                if (_Size.Width > 0 && _Size.Height > 0)
                {
                    return _Size;
                }

                return Size.Empty;
            }
        }

        /// <summary>
        /// 获取此 Matrix 的宽度（列数）。
        /// </summary>
        public int Width
        {
            get
            {
                if (_Size.Width > 0 && _Size.Height > 0)
                {
                    return _Size.Width;
                }

                return 0;
            }
        }

        /// <summary>
        /// 获取此 Matrix 的列数（宽度）。
        /// </summary>
        public int Column
        {
            get
            {
                if (_Size.Width > 0 && _Size.Height > 0)
                {
                    return _Size.Width;
                }

                return 0;
            }
        }

        /// <summary>
        /// 获取此 Matrix 的高度（行数）。
        /// </summary>
        public int Height
        {
            get
            {
                if (_Size.Width > 0 && _Size.Height > 0)
                {
                    return _Size.Height;
                }

                return 0;
            }
        }

        /// <summary>
        /// 获取此 Matrix 的行数（高度）。
        /// </summary>
        public int Row
        {
            get
            {
                if (_Size.Width > 0 && _Size.Height > 0)
                {
                    return _Size.Height;
                }

                return 0;
            }
        }

        /// <summary>
        /// 获取此 Matrix 包含的元素数量。
        /// </summary>
        public int Count
        {
            get
            {
                if (_Size.Width > 0 && _Size.Height > 0)
                {
                    return (_Size.Width * _Size.Height);
                }

                return 0;
            }
        }

        //

        /// <summary>
        /// 获取此 Matrix 的行列式值。
        /// </summary>
        public double Determinant
        {
            get
            {
                if (_Size.Width > 0 && _Size.Height > 0 && _Size.Width == _Size.Height)
                {
                    int order = _Size.Width;

                    if (order == 0)
                    {
                        return 0;
                    }
                    else if (order == 1)
                    {
                        return _MArray[0, 0];
                    }
                    else if (order == 2)
                    {
                        return (_MArray[0, 0] * _MArray[1, 1] - _MArray[1, 0] * _MArray[0, 1]);
                    }
                    else
                    {
                        double det = 0;
                        int detSign = 1;

                        for (int i = 0; i < order; i++)
                        {
                            Size sizeTemp = new Size(order - 1, order - 1);

                            Matrix temp = new Matrix(sizeTemp);

                            for (int x = 0; x < sizeTemp.Width; x++)
                            {
                                for (int y = 0; y < sizeTemp.Height; y++)
                                {
                                    temp._MArray[x, y] = _MArray[x + 1, (y >= i ? y + 1 : y)];
                                }
                            }

                            double detTemp = temp.Determinant;

                            if (InternalMethod.IsNaNOrInfinity(detTemp))
                            {
                                return double.NaN;
                            }

                            det += (_MArray[0, i] * detSign * detTemp);
                            detSign = -detSign;
                        }

                        return det;
                    }
                }

                return double.NaN;
            }
        }

        /// <summary>
        /// 获取此 Matrix 的秩。
        /// </summary>
        public int Rank
        {
            get
            {
                if (_Size.Width > 0 && _Size.Height > 0)
                {
                    int order = Math.Min(_Size.Width, _Size.Height);

                    int result = 0;

                    for (int i = 1; i <= order; i++)
                    {
                        bool Flag = false;

                        for (int x = 0; x < _Size.Width - i + 1; x++)
                        {
                            for (int y = 0; y < _Size.Height - i + 1; y++)
                            {
                                Matrix sub = SubMatrix(x, y, i, i);

                                if (IsNullOrNonMatrix(sub))
                                {
                                    break;
                                }

                                double det = sub.Determinant;

                                if (InternalMethod.IsNaNOrInfinity(det))
                                {
                                    break;
                                }

                                if (det != 0)
                                {
                                    Flag = true;

                                    break;
                                }
                            }

                            if (Flag)
                            {
                                break;
                            }
                        }

                        if (Flag)
                        {
                            result++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    return result;
                }

                return -1;
            }
        }

        /// <summary>
        /// 获取此 Matrix 的转置矩阵。
        /// </summary>
        public Matrix Transport
        {
            get
            {
                if (_Size.Width > 0 && _Size.Height > 0)
                {
                    Matrix result = new Matrix(_Size.Height, _Size.Width);

                    for (int x = 0; x < _Size.Height; x++)
                    {
                        for (int y = 0; y < _Size.Width; y++)
                        {
                            result._MArray[x, y] = _MArray[y, x];
                        }
                    }

                    return result;
                }

                return NonMatrix;
            }
        }

        /// <summary>
        /// 获取此 Matrix 的伴随矩阵。
        /// </summary>
        public Matrix Adjoint
        {
            get
            {
                if (_Size.Width > 0 && _Size.Height > 0 && _Size.Width == _Size.Height)
                {
                    Matrix result = new Matrix(_Size);

                    for (int x = 0; x < _Size.Width; x++)
                    {
                        for (int y = 0; y < _Size.Height; y++)
                        {
                            Size sizeTemp = new Size(_Size.Width - 1, _Size.Height - 1);

                            Matrix temp = new Matrix(sizeTemp);

                            for (int _x = 0; _x < sizeTemp.Width; _x++)
                            {
                                for (int _y = 0; _y < sizeTemp.Height; _y++)
                                {
                                    temp._MArray[_x, _y] = _MArray[(_x < x ? _x : _x + 1), (_y < y ? _y : _y + 1)];
                                }
                            }

                            double det = temp.Determinant;

                            result._MArray[y, x] = ((x + y) % 2 == 0 ? 1 : -1) * det;
                        }
                    }

                    return result;
                }

                return NonMatrix;
            }
        }

        /// <summary>
        /// 获取此 Matrix 的逆矩阵。
        /// </summary>
        public Matrix Invert
        {
            get
            {
                if (_Size.Width > 0 && _Size.Height > 0 && _Size.Width == _Size.Height)
                {
                    Matrix result = Adjoint;

                    if (!IsNullOrNonMatrix(result))
                    {
                        double det = Determinant;

                        if (InternalMethod.IsNaNOrInfinity(det))
                        {
                            return NonMatrix;
                        }

                        for (int x = 0; x < _Size.Width; x++)
                        {
                            for (int y = 0; y < _Size.Height; y++)
                            {
                                result._MArray[x, y] /= det;

                                if (InternalMethod.IsNaNOrInfinity(result._MArray[x, y]))
                                {
                                    return NonMatrix;
                                }
                            }
                        }

                        return result;
                    }
                }

                return NonMatrix;
            }
        }

        #endregion

        #region 静态属性

        /// <summary>
        /// 获取表示非矩阵的 Matrix 的新实例。
        /// </summary>
        public static Matrix NonMatrix
        {
            get
            {
                return new Matrix(null);
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 Matrix 是否与指定的 Matrix 对象相等。
        /// </summary>
        /// <param name="matrix">用于比较的 Matrix 对象。</param>
        public bool Equals(Matrix matrix)
        {
            if (_Size.Width <= 0 || _Size.Height <= 0 || IsNullOrNonMatrix(matrix) || _Size != matrix._Size)
            {
                return false;
            }

            for (int x = 0; x < _Size.Width; x++)
            {
                for (int y = 0; y < _Size.Height; y++)
                {
                    if (!_MArray[x, y].Equals(matrix._MArray[x, y]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //

        /// <summary>
        /// 获取此 Matrix 的副本。
        /// </summary>
        public Matrix Copy()
        {
            if (_Size.Width > 0 && _Size.Height > 0)
            {
                Matrix result = new Matrix(_MArray);

                return result;
            }

            return NonMatrix;
        }

        //

        /// <summary>
        /// 获取表示此 Matrix 的子矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="index">子矩阵首列与首行在此 Matrix 的宽度方向（列）与高度方向（行）的索引。</param>
        /// <param name="size">子矩阵的宽度（列数）与高度（行数）。</param>
        public Matrix SubMatrix(Point index, Size size)
        {
            if ((_Size.Width > 0 && _Size.Height > 0) && (size.Width > 0 && size.Height > 0) && (index.X >= 0 && index.X + size.Width <= _Size.Width) && (index.Y >= 0 && index.Y + size.Height <= _Size.Height))
            {
                Matrix result = new Matrix(size);

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result._MArray[x, y] = _MArray[index.X + x, index.Y + y];
                    }
                }

                return result;
            }

            return NonMatrix;
        }

        /// <summary>
        /// 获取表示此 Matrix 的子矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="x">子矩阵首列在此 Matrix 的宽度方向（列）的索引。</param>
        /// <param name="y">子矩阵首行在此 Matrix 的高度方向（行）的索引。</param>
        /// <param name="width">子矩阵的宽度（列数）。</param>
        /// <param name="height">子矩阵的高度（行数）。</param>
        public Matrix SubMatrix(int x, int y, int width, int height)
        {
            if ((_Size.Width > 0 && _Size.Height > 0) && (width > 0 && height > 0) && (x >= 0 && x + width <= _Size.Width) && (y >= 0 && y + height <= _Size.Height))
            {
                Matrix result = new Matrix(width, height);

                for (int _x = 0; _x < width; _x++)
                {
                    for (int _y = 0; _y < height; _y++)
                    {
                        result._MArray[_x, _y] = _MArray[x + _x, y + _y];
                    }
                }

                return result;
            }

            return NonMatrix;
        }

        //

        /// <summary>
        /// 获取表示此 Matrix 的指定列的 Vector 的新实例。
        /// </summary>
        /// <param name="x">指定列在此 Matrix 的宽度方向（列）的索引。</param>
        public Vector GetColumn(int x)
        {
            if ((_Size.Width > 0 && _Size.Height > 0) && (x >= 0 && x < _Size.Width))
            {
                double[] values = new double[_Size.Height];

                for (int i = 0; i < _Size.Height; i++)
                {
                    values[i] = _MArray[x, i];
                }

                return new Vector(Vector.Type.ColumnVector, values);
            }

            return Vector.NonVector;
        }

        /// <summary>
        /// 获取表示此 Matrix 的指定行的 Vector 的新实例。
        /// </summary>
        /// <param name="y">指定行在此 Matrix 的高度方向（行）的索引。</param>
        public Vector GetRow(int y)
        {
            if ((_Size.Width > 0 && _Size.Height > 0) && (y >= 0 && y < _Size.Height))
            {
                double[] values = new double[_Size.Width];

                for (int i = 0; i < _Size.Width; i++)
                {
                    values[i] = _MArray[i, y];
                }

                return new Vector(Vector.Type.RowVector, values);
            }

            return Vector.NonVector;
        }

        //

        /// <summary>
        /// 将此 Matrix 转换为双精度浮点数二维数组。
        /// </summary>
        public double[,] ToArray()
        {
            if (_Size.Width > 0 && _Size.Height > 0)
            {
                double[,] result = new double[_Size.Width, _Size.Height];

                for (int x = 0; x < _Size.Height; x++)
                {
                    for (int y = 0; y < _Size.Width; y++)
                    {
                        result[x, y] = _MArray[x, y];
                    }
                }

                return result;
            }

            return new double[0, 0];
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断指定的 Matrix 是否为 null 或表示非矩阵。
        /// </summary>
        /// <param name="matrix">用于判断的 Matrix 对象。</param>
        public static bool IsNullOrNonMatrix(Matrix matrix)
        {
            return ((object)matrix == null || matrix._Size.Width <= 0 || matrix._Size.Height <= 0);
        }

        //

        /// <summary>
        /// 判断两个 Matrix 对象是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 Matrix 对象。</param>
        /// <param name="right">用于比较的第二个 Matrix 对象。</param>
        public static bool Equals(Matrix left, Matrix right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (IsNullOrNonMatrix(left) || IsNullOrNonMatrix(right))
            {
                return false;
            }

            return left.Equals(right);
        }

        //

        /// <summary>
        /// 返回表示单位矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="order">矩阵的阶数。</param>
        public static Matrix Identity(int order)
        {
            if (order > 0)
            {
                Matrix result = new Matrix(order, order);

                for (int i = 0; i < order; i++)
                {
                    result._MArray[i, i] = 1;
                }

                return result;
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回表示零矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="size">矩阵的宽度（列数）与高度（行数）。</param>
        public static Matrix Zeros(Size size)
        {
            if (size.Width > 0 && size.Height > 0)
            {
                return new Matrix(size);
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回表示零矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="width">矩阵的宽度（列数）。</param>
        /// <param name="height">矩阵的高度（行数）。</param>
        public static Matrix Zeros(int width, int height)
        {
            if (width > 0 && height > 0)
            {
                return new Matrix(width, height);
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回表示一矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="size">矩阵的宽度（列数）与高度（行数）。</param>
        public static Matrix Ones(Size size)
        {
            if (size.Width > 0 && size.Height > 0)
            {
                return new Matrix(size, 1);
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回表示一矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="width">矩阵的宽度（列数）。</param>
        /// <param name="height">矩阵的高度（行数）。</param>
        public static Matrix Ones(int width, int height)
        {
            if (width > 0 && height > 0)
            {
                return new Matrix(width, height, 1);
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回表示对角矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="array">包含对角元素的数组。</param>
        /// <param name="rowsUponMainDiag">对角元素在矩阵中位于主对角线上方的行数。</param>
        public static Matrix Diagonal(double[] array, int rowsUponMainDiag)
        {
            if (!InternalMethod.IsNullOrEmpty(array))
            {
                int order = array.Length + Math.Abs(rowsUponMainDiag);

                Matrix result = new Matrix(order, order);

                for (int i = 0; i < array.Length; i++)
                {
                    result._MArray[(rowsUponMainDiag >= 0 ? i + rowsUponMainDiag : i), (rowsUponMainDiag >= 0 ? i : i - rowsUponMainDiag)] = array[i];
                }

                return result;
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回表示对角矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="array">包含对角元素的数组。</param>
        public static Matrix Diagonal(double[] array)
        {
            if (!InternalMethod.IsNullOrEmpty(array))
            {
                int order = array.Length;

                Matrix result = new Matrix(order, order);

                for (int i = 0; i < array.Length; i++)
                {
                    result._MArray[i, i] = array[i];
                }

                return result;
            }

            return NonMatrix;
        }

        //

        /// <summary>
        /// 返回表示由 2 个 Matrix 对象组成的增广矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="left">左矩阵。</param>
        /// <param name="right">右矩阵。</param>
        public static Matrix Augment(Matrix left, Matrix right)
        {
            if (!IsNullOrNonMatrix(left) && !IsNullOrNonMatrix(right))
            {
                Size sizeLeft = left.Size;
                Size sizeRight = right.Size;

                if (sizeLeft == sizeRight)
                {
                    Size size = new Size(sizeLeft.Width + sizeRight.Width, sizeLeft.Height);

                    Matrix result = new Matrix(size);

                    for (int x = 0; x < size.Width; x++)
                    {
                        for (int y = 0; y < size.Height; y++)
                        {
                            result._MArray[x, y] = (x < sizeLeft.Width ? left._MArray[x, y] : right._MArray[x - sizeLeft.Width, y]);
                        }
                    }

                    return result;
                }
            }

            return NonMatrix;
        }

        //

        /// <summary>
        /// 返回将 Matrix 对象与双精度浮点数相加得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrix">Matrix 对象，表示被加数。</param>
        /// <param name="n">双精度浮点数，表示加数。</param>
        public static Matrix Add(Matrix matrix, double n)
        {
            if (!IsNullOrNonMatrix(matrix))
            {
                Size size = matrix.Size;

                Matrix result = new Matrix(size);

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result._MArray[x, y] = matrix._MArray[x, y] + n;
                    }
                }

                return result;
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回将双精度浮点数与 Matrix 对象相加得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被加数。</param>
        /// <param name="matrix">Matrix 对象，表示加数。</param>
        public static Matrix Add(double n, Matrix matrix)
        {
            if (!IsNullOrNonMatrix(matrix))
            {
                Size size = matrix.Size;

                Matrix result = new Matrix(size);

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result._MArray[x, y] = n + matrix._MArray[x, y];
                    }
                }

                return result;
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回将 Matrix 对象与 Matrix 对象相加得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="left">Matrix 对象，表示被加数。</param>
        /// <param name="right">Matrix 对象，表示加数。</param>
        public static Matrix Add(Matrix left, Matrix right)
        {
            if (!IsNullOrNonMatrix(left) && !IsNullOrNonMatrix(right))
            {
                Size sizeLeft = left.Size;
                Size sizeRight = right.Size;

                if (sizeLeft == sizeRight)
                {
                    Size size = sizeLeft;

                    Matrix result = new Matrix(size);

                    for (int x = 0; x < size.Width; x++)
                    {
                        for (int y = 0; y < size.Height; y++)
                        {
                            result._MArray[x, y] = left._MArray[x, y] + right._MArray[x, y];
                        }
                    }

                    return result;
                }
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回将 Matrix 对象与双精度浮点数相减得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrix">Matrix 对象，表示被减数。</param>
        /// <param name="n">双精度浮点数，表示减数。</param>
        public static Matrix Subtract(Matrix matrix, double n)
        {
            if (!IsNullOrNonMatrix(matrix))
            {
                Size size = matrix.Size;

                Matrix result = new Matrix(size);

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result._MArray[x, y] = matrix._MArray[x, y] - n;
                    }
                }

                return result;
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回将双精度浮点数与 Matrix 对象相减得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被减数。</param>
        /// <param name="matrix">Matrix 对象，表示减数。</param>
        public static Matrix Subtract(double n, Matrix matrix)
        {
            if (!IsNullOrNonMatrix(matrix))
            {
                Size size = matrix.Size;

                Matrix result = new Matrix(size);

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result._MArray[x, y] = n - matrix._MArray[x, y];
                    }
                }

                return result;
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回将 Matrix 对象与 Matrix 对象相减得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="left">Matrix 对象，表示被减数。</param>
        /// <param name="right">Matrix 对象，表示减数。</param>
        public static Matrix Subtract(Matrix left, Matrix right)
        {
            if (!IsNullOrNonMatrix(left) && !IsNullOrNonMatrix(right))
            {
                Size sizeLeft = left.Size;
                Size sizeRight = right.Size;

                if (sizeLeft == sizeRight)
                {
                    Size size = sizeLeft;

                    Matrix result = new Matrix(size);

                    for (int x = 0; x < size.Width; x++)
                    {
                        for (int y = 0; y < size.Height; y++)
                        {
                            result._MArray[x, y] = left._MArray[x, y] - right._MArray[x, y];
                        }
                    }

                    return result;
                }
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回将 Matrix 对象与双精度浮点数相乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrix">Matrix 对象，表示被乘数。</param>
        /// <param name="n">双精度浮点数，表示乘数。</param>
        public static Matrix Multiply(Matrix matrix, double n)
        {
            if (!IsNullOrNonMatrix(matrix))
            {
                Size size = matrix.Size;

                Matrix result = new Matrix(size);

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result._MArray[x, y] = matrix._MArray[x, y] * n;
                    }
                }

                return result;
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回将双精度浮点数与 Matrix 对象相乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被乘数。</param>
        /// <param name="matrix">Matrix 对象，表示乘数。</param>
        public static Matrix Multiply(double n, Matrix matrix)
        {
            if (!IsNullOrNonMatrix(matrix))
            {
                Size size = matrix.Size;

                Matrix result = new Matrix(size);

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result._MArray[x, y] = n * matrix._MArray[x, y];
                    }
                }

                return result;
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回将 Matrix 对象与 Matrix 对象相乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="left">Matrix 对象，表示被乘数。</param>
        /// <param name="right">Matrix 对象，表示乘数。</param>
        public static Matrix Multiply(Matrix left, Matrix right)
        {
            if (!IsNullOrNonMatrix(left) && !IsNullOrNonMatrix(right))
            {
                Size sizeLeft = left.Size;
                Size sizeRight = right.Size;

                if (sizeLeft.Width == sizeRight.Height)
                {
                    int height = sizeLeft.Width;

                    Size size = new Size(sizeRight.Width, sizeLeft.Height);

                    Matrix result = new Matrix(size);

                    for (int x = 0; x < size.Width; x++)
                    {
                        for (int y = 0; y < size.Height; y++)
                        {
                            result._MArray[x, y] = 0;

                            for (int i = 0; i < height; i++)
                            {
                                result._MArray[x, y] += (left._MArray[i, y] * right._MArray[x, i]);
                            }
                        }
                    }

                    return result;
                }
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回将列表中所有 Matrix 对象依次左乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="list">左矩阵列表。</param>
        public static Matrix MultiplyLeft(List<Matrix> list)
        {
            if (list.Count > 0)
            {
                if (!IsNullOrNonMatrix(list[0]))
                {
                    Matrix result = list[0].Copy();

                    if (list.Count == 1)
                    {
                        return result;
                    }
                    else
                    {
                        for (int i = 1; i < list.Count; i++)
                        {
                            if (!IsNullOrNonMatrix(list[i]))
                            {
                                result = Multiply(list[i], result);

                                if (IsNullOrNonMatrix(result))
                                {
                                    return NonMatrix;
                                }
                            }
                            else
                            {
                                return NonMatrix;
                            }
                        }

                        return result;
                    }
                }
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回将列表中所有 Matrix 对象依次右乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="list">右矩阵列表。</param>
        public static Matrix MultiplyRight(List<Matrix> list)
        {
            if (list.Count > 0)
            {
                if (!IsNullOrNonMatrix(list[0]))
                {
                    Matrix result = list[0].Copy();

                    if (list.Count == 1)
                    {
                        return result;
                    }
                    else
                    {
                        for (int i = 1; i < list.Count; i++)
                        {
                            if (!IsNullOrNonMatrix(list[i]))
                            {
                                result = Multiply(result, list[i]);

                                if (IsNullOrNonMatrix(result))
                                {
                                    return NonMatrix;
                                }
                            }
                            else
                            {
                                return NonMatrix;
                            }
                        }

                        return result;
                    }
                }
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回将 Matrix 对象与双精度浮点数相除得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrix">Matrix 对象，表示被除数。</param>
        /// <param name="n">双精度浮点数，表示除数。</param>
        public static Matrix Divide(Matrix matrix, double n)
        {
            if (!IsNullOrNonMatrix(matrix))
            {
                Size size = matrix.Size;

                Matrix result = new Matrix(size);

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result._MArray[x, y] = matrix._MArray[x, y] / n;
                    }
                }

                return result;
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回将双精度浮点数与 Matrix 对象相除得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被除数。</param>
        /// <param name="matrix">Matrix 对象，表示除数。</param>
        public static Matrix Divide(double n, Matrix matrix)
        {
            if (!IsNullOrNonMatrix(matrix))
            {
                Size size = matrix.Size;

                Matrix result = new Matrix(size);

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result._MArray[x, y] = n / matrix._MArray[x, y];
                    }
                }

                return result;
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回将 Matrix 对象与 Matrix 对象左除得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="left">Matrix 对象，表示被除数。</param>
        /// <param name="right">Matrix 对象，表示除数。</param>
        public static Matrix DivideLeft(Matrix left, Matrix right)
        {
            if (!IsNullOrNonMatrix(left) && !IsNullOrNonMatrix(right))
            {
                Size sizeLeft = left.Size;
                Size sizeRight = right.Size;

                if (sizeLeft.Width == sizeLeft.Height && sizeLeft.Width == sizeRight.Height)
                {
                    Matrix invLeft = left.Invert;

                    if (!IsNullOrNonMatrix(invLeft))
                    {
                        return Multiply(invLeft, right);
                    }
                }
            }

            return NonMatrix;
        }

        /// <summary>
        /// 返回将 Matrix 对象与 Matrix 对象右除得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="left">Matrix 对象，表示被除数。</param>
        /// <param name="right">Matrix 对象，表示除数。</param>
        public static Matrix DivideRight(Matrix left, Matrix right)
        {
            if (!IsNullOrNonMatrix(left) && !IsNullOrNonMatrix(right))
            {
                Size sizeLeft = left.Size;
                Size sizeRight = right.Size;

                if (sizeRight.Width == sizeRight.Height && sizeLeft.Width == sizeRight.Height)
                {
                    Matrix invRight = right.Invert;

                    if (!IsNullOrNonMatrix(invRight))
                    {
                        return Multiply(left, invRight);
                    }
                }
            }

            return NonMatrix;
        }

        //

        /// <summary>
        /// 返回由 Matrix 对象与 Vector 对象指定的非齐次线性方程组的解向量。
        /// </summary>
        /// <param name="matrix">Matrix 对象，表示系数矩阵。</param>
        /// <param name="vector">Vector 对象，表示常数项。</param>
        public static Vector SolveLinearEquation(Matrix matrix, Vector vector)
        {
            if (!IsNullOrNonMatrix(matrix) && !Vector.IsNullOrNonVector(vector) && vector.IsColumnVector)
            {
                Size size = matrix.Size;

                if (size.Width == size.Height)
                {
                    int order = vector.Dimension;

                    if (order == size.Height)
                    {
                        Matrix solution = DivideLeft(matrix, vector.ToMatrix());

                        if (!IsNullOrNonMatrix(solution) && solution.Size == new Size(1, order))
                        {
                            return solution.GetColumn(0);
                        }
                    }
                }
            }

            return Vector.NonVector;
        }

        #endregion

        #region 基类方法

        /// <summary>
        /// 判断此 Matrix 是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Matrix))
            {
                return false;
            }

            return Equals((Matrix)obj);
        }

        /// <summary>
        /// 返回此 Matrix 的哈希代码。
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 Matrix 转换为字符串。
        /// </summary>
        public override string ToString()
        {
            string Str = string.Empty;

            if (_Size.Width > 0 && _Size.Height > 0)
            {
                Str = string.Concat("Column=", _Size.Width, ", Row=", _Size.Height);
            }
            else
            {
                Str = "NonMatrix";
            }

            return string.Concat(base.GetType().Name, " [", Str, "]");
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 Matrix 对象是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Matrix 对象。</param>
        /// <param name="right">运算符右侧比较的 Matrix 对象。</param>
        public static bool operator ==(Matrix left, Matrix right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (IsNullOrNonMatrix(left) || IsNullOrNonMatrix(right) || left._Size != right._Size)
            {
                return false;
            }

            for (int x = 0; x < left._Size.Width; x++)
            {
                for (int y = 0; y < left._Size.Height; y++)
                {
                    if (left._MArray[x, y] != right._MArray[x, y])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 判断两个 Matrix 对象是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Matrix 对象。</param>
        /// <param name="right">运算符右侧比较的 Matrix 对象。</param>
        public static bool operator !=(Matrix left, Matrix right)
        {
            if ((object)left == null && (object)right == null)
            {
                return false;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return false;
            }
            else if (IsNullOrNonMatrix(left) || IsNullOrNonMatrix(right) || left._Size != right._Size)
            {
                return true;
            }

            for (int x = 0; x < left._Size.Width; x++)
            {
                for (int y = 0; y < left._Size.Height; y++)
                {
                    if (left._MArray[x, y] != right._MArray[x, y])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion
    }
}