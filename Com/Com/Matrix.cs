/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.Matrix
Version 19.11.6.0000

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
    public sealed class Matrix : IEquatable<Matrix>
    {
        #region 私有成员与内部成员

        private const int _MaxSize = 2146435071; // Matrix 允许包含的最大元素数量，等于 System.Array.MaxArrayLength。

        //

        [InternalUnsafeCall(InternalUnsafeCallType.InputAddress)]
        internal static Matrix UnsafeCreateInstance(double[,] values) // 以不安全方式创建 Matrix 的新实例。
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                return new Matrix()
                {
                    _Size = Size.Empty,
                    _MArray = new double[0, 0]
                };
            }
            else
            {
                int width = values.GetLength(0);
                int height = values.GetLength(1);

                if ((long)width * height > _MaxSize)
                {
                    throw new OverflowException();
                }

                //

                return new Matrix()
                {
                    _Size = new Size(width, height),
                    _MArray = values
                };
            }
        }

        //

        private Size _Size; // 此 Matrix 的矩阵大小。

        private double[,] _MArray; // 用于存储矩阵元素的数组。

        //

        [InternalUnsafeCall(InternalUnsafeCallType.OutputAddress)]
        internal double[,] UnsafeGetData() // 以不安全方式获取此 Matrix 的内部数据结构。
        {
            return _MArray;
        }

        #endregion

        #region 构造函数

        private Matrix() // 不使用任何参数初始化 Matrix 的新实例。
        {
        }

        /// <summary>
        /// 使用指定的宽度（列数）与高度（行数）初始化 Matrix 的新实例。
        /// </summary>
        /// <param name="size">矩阵的宽度（列数）与高度（行数）。</param>
        public Matrix(Size size)
        {
            if (size.Width < 0 || size.Height < 0 || (long)size.Width * size.Height > _MaxSize)
            {
                throw new OverflowException();
            }

            //

            if (size.Width == 0 || size.Height == 0)
            {
                _Size = Size.Empty;
                _MArray = new double[0, 0];
            }
            else
            {
                _Size = size;
                _MArray = new double[_Size.Width, _Size.Height];
            }
        }

        /// <summary>
        /// 使用指定的宽度（列数）、高度（行数）与所有元素的值初始化 Matrix 的新实例。
        /// </summary>
        /// <param name="size">矩阵的宽度（列数）与高度（行数）。</param>
        /// <param name="value">矩阵的所有元素的值。</param>
        public Matrix(Size size, double value)
        {
            if (size.Width < 0 || size.Height < 0 || (long)size.Width * size.Height > _MaxSize)
            {
                throw new OverflowException();
            }

            //

            if (size.Width == 0 || size.Height == 0)
            {
                _Size = Size.Empty;
                _MArray = new double[0, 0];
            }
            else
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
        }

        /// <summary>
        /// 使用指定的宽度（列数）与高度（行数）初始化 Matrix 的新实例。
        /// </summary>
        /// <param name="width">矩阵的宽度（列数）。</param>
        /// <param name="height">矩阵的高度（行数）。</param>
        public Matrix(int width, int height)
        {
            if (width < 0 || height < 0 || (long)width * height > _MaxSize)
            {
                throw new OverflowException();
            }

            //

            if (width == 0 || height == 0)
            {
                _Size = Size.Empty;
                _MArray = new double[0, 0];
            }
            else
            {
                _Size = new Size(width, height);
                _MArray = new double[_Size.Width, _Size.Height];
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
            if (width < 0 || height < 0 || (long)width * height > _MaxSize)
            {
                throw new OverflowException();
            }

            //

            if (width == 0 || height == 0)
            {
                _Size = Size.Empty;
                _MArray = new double[0, 0];
            }
            else
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
        }

        /// <summary>
        /// 使用双精度浮点数二维数组表示的矩阵元素初始化 Matrix 的新实例。
        /// </summary>
        /// <param name="values">双精度浮点数二维数组表示的矩阵元素。</param>
        public Matrix(double[,] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                _Size = Size.Empty;
                _MArray = new double[0, 0];
            }
            else
            {
                int width = values.GetLength(0);
                int height = values.GetLength(1);

                if ((long)width * height > _MaxSize)
                {
                    throw new OverflowException();
                }

                //

                _Size = new Size(width, height);
                _MArray = new double[_Size.Width, _Size.Height];

                Array.Copy(values, _MArray, _Size.Width * _Size.Height);
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
                if (IsEmpty || (x < 0 || x >= _Size.Width) || (y < 0 || y >= _Size.Height))
                {
                    throw new IndexOutOfRangeException();
                }

                //

                return _MArray[x, y];
            }

            set
            {
                if (IsEmpty || (x < 0 || x >= _Size.Width) || (y < 0 || y >= _Size.Height))
                {
                    throw new IndexOutOfRangeException();
                }

                //

                _MArray[x, y] = value;
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
                if (IsEmpty || (index.X < 0 || index.X >= _Size.Width) || (index.Y < 0 || index.Y >= _Size.Height))
                {
                    throw new IndexOutOfRangeException();
                }

                //

                return _MArray[index.X, index.Y];
            }

            set
            {
                if (IsEmpty || (index.X < 0 || index.X >= _Size.Width) || (index.Y < 0 || index.Y >= _Size.Height))
                {
                    throw new IndexOutOfRangeException();
                }

                //

                _MArray[index.X, index.Y] = value;
            }
        }

        //

        /// <summary>
        /// 获取表示此 Matrix 是否为空矩阵的布尔值。
        /// </summary>
        public bool IsEmpty
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
                if (IsEmpty)
                {
                    return Size.Empty;
                }
                else
                {
                    return _Size;
                }
            }
        }

        /// <summary>
        /// 获取此 Matrix 的宽度（列数）。
        /// </summary>
        public int Width
        {
            get
            {
                return Size.Width;
            }
        }

        /// <summary>
        /// 获取此 Matrix 的列数（宽度）。
        /// </summary>
        public int Column
        {
            get
            {
                return Size.Width;
            }
        }

        /// <summary>
        /// 获取此 Matrix 的高度（行数）。
        /// </summary>
        public int Height
        {
            get
            {
                return Size.Height;
            }
        }

        /// <summary>
        /// 获取此 Matrix 的行数（高度）。
        /// </summary>
        public int Row
        {
            get
            {
                return Size.Height;
            }
        }

        /// <summary>
        /// 获取此 Matrix 包含的元素数量。
        /// </summary>
        public int Count
        {
            get
            {
                if (IsEmpty)
                {
                    return 0;
                }
                else
                {
                    return (_Size.Width * _Size.Height);
                }
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
                if (IsEmpty || _Size.Width != _Size.Height)
                {
                    throw new ArithmeticException();
                }

                //

                int order = _Size.Width;

                if (order == 1)
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

                        for (int y = 0; y < i; y++)
                        {
                            for (int x = 0; x < sizeTemp.Width; x++)
                            {
                                temp._MArray[x, y] = _MArray[x + 1, y];
                            }
                        }

                        for (int y = i; y < sizeTemp.Height; y++)
                        {
                            for (int x = 0; x < sizeTemp.Width; x++)
                            {
                                temp._MArray[x, y] = _MArray[x + 1, y + 1];
                            }
                        }

                        double detTemp = temp.Determinant;

                        if (double.IsNaN(detTemp))
                        {
                            return double.NaN;
                        }
                        else
                        {
                            det += (_MArray[0, i] * detSign * detTemp);
                            detSign = -detSign;
                        }
                    }

                    return det;
                }
            }
        }

        /// <summary>
        /// 获取此 Matrix 的秩。
        /// </summary>
        public int Rank
        {
            get
            {
                if (IsEmpty)
                {
                    return -1;
                }
                else
                {
                    int order = Math.Min(_Size.Width, _Size.Height);

                    if (_Size.Width == _Size.Height && Determinant != 0)
                    {
                        return order;
                    }
                    else
                    {
                        bool allZero = true;

                        for (int x = 0; x < _Size.Width; x++)
                        {
                            for (int y = 0; y < _Size.Height; y++)
                            {
                                if (_MArray[x, y] != 0)
                                {
                                    allZero = false;

                                    goto ALL_ZERO;
                                }
                            }
                        }

                        ALL_ZERO:
                        if (allZero)
                        {
                            return 0;
                        }
                        else
                        {
                            for (int rank = order; rank > 1; rank--)
                            {
                                for (int x = 0; x < _Size.Width - rank + 1; x++)
                                {
                                    for (int y = 0; y < _Size.Height - rank + 1; y++)
                                    {
                                        if (SubMatrix(x, y, rank, rank).Determinant != 0)
                                        {
                                            return rank;
                                        }
                                    }
                                }
                            }

                            return 1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取此 Matrix 的转置矩阵。
        /// </summary>
        public Matrix Transport
        {
            get
            {
                if (IsEmpty)
                {
                    return Empty;
                }
                else
                {
                    double[,] result = new double[_Size.Width, _Size.Height];

                    for (int x = 0; x < _Size.Height; x++)
                    {
                        for (int y = 0; y < _Size.Width; y++)
                        {
                            result[x, y] = _MArray[y, x];
                        }
                    }

                    return UnsafeCreateInstance(result);
                }
            }
        }

        /// <summary>
        /// 获取此 Matrix 的伴随矩阵。
        /// </summary>
        public Matrix Adjoint
        {
            get
            {
                if (IsEmpty || _Size.Width != _Size.Height)
                {
                    throw new ArithmeticException();
                }

                //

                double[,] result = new double[_Size.Width, _Size.Height];

                Size sizeTemp = new Size(_Size.Width - 1, _Size.Height - 1);

                double[,] temp = new double[sizeTemp.Width, sizeTemp.Height];

                for (int x = 0; x < _Size.Width; x++)
                {
                    for (int y = 0; y < _Size.Height; y++)
                    {
                        for (int _x = 0; _x < sizeTemp.Width; _x++)
                        {
                            for (int _y = 0; _y < sizeTemp.Height; _y++)
                            {
                                temp[_x, _y] = _MArray[(_x < x ? _x : _x + 1), (_y < y ? _y : _y + 1)];
                            }
                        }

                        double det = UnsafeCreateInstance(temp).Determinant;

                        result[y, x] = ((x + y) % 2 == 0 ? 1 : -1) * det;
                    }
                }

                return UnsafeCreateInstance(result);
            }
        }

        /// <summary>
        /// 获取此 Matrix 的逆矩阵。
        /// </summary>
        public Matrix Invert
        {
            get
            {
                if (IsEmpty || _Size.Width != _Size.Height)
                {
                    throw new ArithmeticException();
                }

                //

                double det = Determinant;

                if (InternalMethod.IsNaNOrInfinity(det) || det == 0)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    Matrix result = Adjoint;

                    for (int x = 0; x < _Size.Width; x++)
                    {
                        for (int y = 0; y < _Size.Height; y++)
                        {
                            result._MArray[x, y] /= det;

                            if (InternalMethod.IsNaNOrInfinity(result._MArray[x, y]))
                            {
                                throw new ArithmeticException();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        #endregion

        #region 静态属性

        /// <summary>
        /// 获取表示空矩阵的 Matrix 的新实例。
        /// </summary>
        public static Matrix Empty
        {
            get
            {
                return new Matrix()
                {
                    _Size = Size.Empty,
                    _MArray = new double[0, 0]
                };
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 Matrix 是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>布尔值，表示此 Matrix 是否与指定的对象相等。</returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj is null || !(obj is Matrix))
            {
                return false;
            }
            else
            {
                return Equals((Matrix)obj);
            }
        }

        /// <summary>
        /// 返回此 Matrix 的哈希代码。
        /// </summary>
        /// <returns>32 位整数，表示此 Matrix 的哈希代码。</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 Matrix 转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 Matrix 的字符串形式。</returns>
        public override string ToString()
        {
            string Str = string.Empty;

            if (IsEmpty)
            {
                Str = "Empty";
            }
            else
            {
                Str = string.Concat("Column=", _Size.Width, ", Row=", _Size.Height);
            }

            return string.Concat(base.GetType().Name, " [", Str, "]");
        }

        //

        /// <summary>
        /// 判断此 Matrix 是否与指定的 Matrix 对象相等。
        /// </summary>
        /// <param name="matrix">用于比较的 Matrix 对象。</param>
        /// <returns>布尔值，表示此 Matrix 是否与指定的 Matrix 对象相等。</returns>
        public bool Equals(Matrix matrix)
        {
            if (object.ReferenceEquals(this, matrix))
            {
                return true;
            }
            else if (matrix is null)
            {
                return false;
            }
            else if (_Size != matrix._Size)
            {
                return false;
            }
            else
            {
                double[,] arrayR = matrix._MArray;

                for (int x = 0; x < _Size.Width; x++)
                {
                    for (int y = 0; y < _Size.Height; y++)
                    {
                        if (!_MArray[x, y].Equals(arrayR[x, y]))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        //

        /// <summary>
        /// 获取此 Matrix 的副本。
        /// </summary>
        /// <returns>Matrix 对象，表示此 Matrix 的副本。</returns>
        public Matrix Copy()
        {
            if (IsEmpty)
            {
                return Empty;
            }
            else
            {
                return new Matrix(_MArray);
            }
        }

        //

        /// <summary>
        /// 获取表示此 Matrix 的子矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="index">子矩阵首列与首行在此 Matrix 的宽度方向（列）与高度方向（行）的索引。</param>
        /// <param name="size">子矩阵的宽度（列数）与高度（行数）。</param>
        /// <returns>Matrix 对象，表示此 Matrix 的子矩阵。</returns>
        public Matrix SubMatrix(Point index, Size size)
        {
            if (IsEmpty || (size.Width <= 0 || size.Height <= 0) || (index.X < 0 || index.X + size.Width > _Size.Width) || (index.Y < 0 || index.Y + size.Height > _Size.Height))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            double[,] result = new double[size.Width, size.Height];

            for (int x = 0; x < size.Width; x++)
            {
                for (int y = 0; y < size.Height; y++)
                {
                    result[x, y] = _MArray[index.X + x, index.Y + y];
                }
            }

            return UnsafeCreateInstance(result);
        }

        /// <summary>
        /// 获取表示此 Matrix 的子矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="x">子矩阵首列在此 Matrix 的宽度方向（列）的索引。</param>
        /// <param name="y">子矩阵首行在此 Matrix 的高度方向（行）的索引。</param>
        /// <param name="width">子矩阵的宽度（列数）。</param>
        /// <param name="height">子矩阵的高度（行数）。</param>
        /// <returns>Matrix 对象，表示此 Matrix 的子矩阵。</returns>
        public Matrix SubMatrix(int x, int y, int width, int height)
        {
            if (IsEmpty || (width <= 0 || height <= 0) || (x < 0 || x + width > _Size.Width) || (y < 0 || y + height > _Size.Height))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            double[,] result = new double[width, height];

            for (int _x = 0; _x < width; _x++)
            {
                for (int _y = 0; _y < height; _y++)
                {
                    result[_x, _y] = _MArray[x + _x, y + _y];
                }
            }

            return UnsafeCreateInstance(result);
        }

        //

        /// <summary>
        /// 获取表示此 Matrix 的指定列的 Vector 的新实例。
        /// </summary>
        /// <param name="x">指定列在此 Matrix 的宽度方向（列）的索引。</param>
        /// <returns>Vector 对象，表示此 Matrix 的指定列。</returns>
        public Vector GetColumn(int x)
        {
            if (IsEmpty || (x < 0 || x >= _Size.Width))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            double[] values = new double[_Size.Height];

            for (int i = 0; i < _Size.Height; i++)
            {
                values[i] = _MArray[x, i];
            }

            return Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, values);
        }

        /// <summary>
        /// 按 Vector 对象表示的列向量设置表示此 Matrix 的指定列。
        /// </summary>
        /// <param name="x">指定列在此 Matrix 的宽度方向（列）的索引。</param>
        /// <param name="vector">Vector 对象，表示的列向量。</param>
        public void SetColumn(int x, Vector vector)
        {
            if (vector is null)
            {
                throw new ArgumentNullException();
            }

            if (IsEmpty || (x < 0 || x >= _Size.Width))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (vector.IsEmpty || !vector.IsColumnVector || vector.Dimension != _Size.Height)
            {
                throw new ArithmeticException();
            }

            //

            double[] values = vector.ToArray();

            for (int i = 0; i < _Size.Height; i++)
            {
                _MArray[x, i] = values[i];
            }
        }

        /// <summary>
        /// 获取表示此 Matrix 的指定行的 Vector 的新实例。
        /// </summary>
        /// <param name="y">指定行在此 Matrix 的高度方向（行）的索引。</param>
        /// <returns>Vector 对象，表示此 Matrix 的指定行。</returns>
        public Vector GetRow(int y)
        {
            if (IsEmpty || (y < 0 || y >= _Size.Height))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            double[] values = new double[_Size.Width];

            for (int i = 0; i < _Size.Width; i++)
            {
                values[i] = _MArray[i, y];
            }

            return Vector.UnsafeCreateInstance(Vector.Type.RowVector, values);
        }

        /// <summary>
        /// 按 Vector 对象表示的行向量设置表示此 Matrix 的指定行。
        /// </summary>
        /// <param name="y">指定行在此 Matrix 的高度方向（行）的索引。</param>
        /// <param name="vector">Vector 对象，表示的行向量。</param>
        public void SetRow(int y, Vector vector)
        {
            if (vector is null)
            {
                throw new ArgumentNullException();
            }

            if (IsEmpty || (y < 0 || y >= _Size.Height))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (vector.IsEmpty || !vector.IsRowVector || vector.Dimension != _Size.Width)
            {
                throw new ArithmeticException();
            }

            //

            double[] values = vector.ToArray();

            for (int i = 0; i < _Size.Width; i++)
            {
                _MArray[i, y] = values[i];
            }
        }

        //

        /// <summary>
        /// 将此 Matrix 转换为双精度浮点数二维数组。
        /// </summary>
        /// <returns>双精度浮点数二维数组，表示转换的结果。</returns>
        public double[,] ToArray()
        {
            if (IsEmpty)
            {
                return new double[0, 0];
            }
            else
            {
                double[,] result = new double[_Size.Width, _Size.Height];

                Array.Copy(_MArray, result, _Size.Width * _Size.Height);

                return result;
            }
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断指定的 Matrix 是否为 null 或表示空矩阵。
        /// </summary>
        /// <param name="matrix">用于判断的 Matrix 对象。</param>
        /// <returns>布尔值，表示指定的 Matrix 是否为 null 或表示空矩阵。</returns>
        public static bool IsNullOrEmpty(Matrix matrix)
        {
            return (matrix is null || (matrix._Size.Width <= 0 || matrix._Size.Height <= 0));
        }

        //

        /// <summary>
        /// 判断两个 Matrix 对象是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 Matrix 对象。</param>
        /// <param name="right">用于比较的第二个 Matrix 对象。</param>
        /// <returns>布尔值，表示两个 Matrix 对象是否相等。</returns>
        public static bool Equals(Matrix left, Matrix right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (left is null || right is null)
            {
                return false;
            }
            else
            {
                return left.Equals(right);
            }
        }

        //

        /// <summary>
        /// 返回表示单位矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="order">矩阵的阶数。</param>
        /// <returns>Matrix 对象，表示单位矩阵。</returns>
        public static Matrix Identity(int order)
        {
            if (order < 0)
            {
                throw new OverflowException();
            }
            else if (order == 0)
            {
                throw new ArithmeticException();
            }

            //

            double[,] result = new double[order, order];

            for (int i = 0; i < order; i++)
            {
                result[i, i] = 1;
            }

            return UnsafeCreateInstance(result);
        }

        /// <summary>
        /// 返回表示全零矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="size">矩阵的宽度（列数）与高度（行数）。</param>
        /// <returns>Matrix 对象，表示全零矩阵。</returns>
        public static Matrix Zeros(Size size)
        {
            if (size.Width < 0 || size.Height < 0)
            {
                throw new OverflowException();
            }
            else if (size.Width == 0 || size.Height == 0)
            {
                throw new ArithmeticException();
            }

            //

            return new Matrix(size);
        }

        /// <summary>
        /// 返回表示全零矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="width">矩阵的宽度（列数）。</param>
        /// <param name="height">矩阵的高度（行数）。</param>
        /// <returns>Matrix 对象，表示全零矩阵。</returns>
        public static Matrix Zeros(int width, int height)
        {
            if (width < 0 || height < 0)
            {
                throw new OverflowException();
            }
            else if (width == 0 || height == 0)
            {
                throw new ArithmeticException();
            }

            //

            return new Matrix(width, height);
        }

        /// <summary>
        /// 返回表示全一矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="size">矩阵的宽度（列数）与高度（行数）。</param>
        /// <returns>Matrix 对象，表示全一矩阵。</returns>
        public static Matrix Ones(Size size)
        {
            if (size.Width < 0 || size.Height < 0)
            {
                throw new OverflowException();
            }
            else if (size.Width == 0 || size.Height == 0)
            {
                throw new ArithmeticException();
            }

            //

            return new Matrix(size, 1);
        }

        /// <summary>
        /// 返回表示全一矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="width">矩阵的宽度（列数）。</param>
        /// <param name="height">矩阵的高度（行数）。</param>
        /// <returns>Matrix 对象，表示全一矩阵。</returns>
        public static Matrix Ones(int width, int height)
        {
            if (width < 0 || height < 0)
            {
                throw new OverflowException();
            }
            else if (width == 0 || height == 0)
            {
                throw new ArithmeticException();
            }

            //

            return new Matrix(width, height, 1);
        }

        /// <summary>
        /// 返回表示对角矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="array">包含对角元素的双精度浮点数数组。</param>
        /// <param name="rowsUponMainDiag">对角元素在矩阵中位于主对角线上方的行数。</param>
        /// <returns>Matrix 对象，表示对角矩阵。</returns>
        public static Matrix Diagonal(double[] array, int rowsUponMainDiag)
        {
            if (InternalMethod.IsNullOrEmpty(array))
            {
                return Empty;
            }
            else
            {
                int order = array.Length + Math.Abs(rowsUponMainDiag);

                double[,] result = new double[order, order];

                if (rowsUponMainDiag >= 0)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        result[i + rowsUponMainDiag, i] = array[i];
                    }
                }
                else
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        result[i, i - rowsUponMainDiag] = array[i];
                    }
                }

                return UnsafeCreateInstance(result);
            }
        }

        /// <summary>
        /// 返回表示对角矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="array">包含对角元素的双精度浮点数数组。</param>
        /// <returns>Matrix 对象，表示对角矩阵。</returns>
        public static Matrix Diagonal(double[] array)
        {
            if (InternalMethod.IsNullOrEmpty(array))
            {
                return Empty;
            }
            else
            {
                int order = array.Length;

                double[,] result = new double[order, order];

                for (int i = 0; i < array.Length; i++)
                {
                    result[i, i] = array[i];
                }

                return UnsafeCreateInstance(result);
            }
        }

        //

        /// <summary>
        /// 返回表示由 2 个 Matrix 对象组成的增广矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="left">Matrix 对象，表示左侧的矩阵。</param>
        /// <param name="right">Matrix 对象，表示右侧的矩阵。</param>
        /// <returns>Matrix 对象，表示由 2 个 Matrix 对象组成的增广矩阵。</returns>
        public static Matrix Augment(Matrix left, Matrix right)
        {
            if (left is null || right is null)
            {
                throw new ArgumentNullException();
            }

            //

            bool LIsEmpty = left.IsEmpty;
            bool RIsEmpty = right.IsEmpty;

            if (LIsEmpty || RIsEmpty)
            {
                if (LIsEmpty && RIsEmpty)
                {
                    return Empty;
                }
                else if (LIsEmpty)
                {
                    return right.Copy();
                }
                else
                {
                    return left.Copy();
                }
            }
            else
            {
                Size sizeL = left.Size;
                Size sizeR = right.Size;

                if (sizeL.Height != sizeR.Height)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    Size size = new Size(sizeL.Width + sizeR.Width, sizeL.Height);

                    double[,] result = new double[size.Width, size.Height];

                    double[,] arrayL = left._MArray, arrayR = right._MArray;

                    for (int x = 0; x < sizeL.Width; x++)
                    {
                        for (int y = 0; y < size.Height; y++)
                        {
                            result[x, y] = arrayL[x, y];
                        }
                    }

                    for (int x = sizeL.Width; x < size.Width; x++)
                    {
                        for (int y = 0; y < size.Height; y++)
                        {
                            result[x, y] = arrayR[x - sizeL.Width, y];
                        }
                    }

                    return UnsafeCreateInstance(result);
                }
            }
        }

        //

        /// <summary>
        /// 返回将 Matrix 对象与双精度浮点数相加得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrix">Matrix 对象，表示被加数。</param>
        /// <param name="n">双精度浮点数，表示加数。</param>
        /// <returns>Matrix 对象，表示将 Matrix 对象与双精度浮点数相加得到的结果。</returns>
        public static Matrix Add(Matrix matrix, double n)
        {
            if (matrix is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (matrix.IsEmpty)
            {
                return Empty;
            }
            else
            {
                Size size = matrix.Size;

                double[,] result = new double[size.Width, size.Height];

                double[,] arrayL = matrix._MArray;

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = arrayL[x, y] + n;
                    }
                }

                return UnsafeCreateInstance(result);
            }
        }

        /// <summary>
        /// 返回将双精度浮点数与 Matrix 对象相加得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被加数。</param>
        /// <param name="matrix">Matrix 对象，表示加数。</param>
        /// <returns>Matrix 对象，表示将双精度浮点数与 Matrix 对象相加得到的结果。</returns>
        public static Matrix Add(double n, Matrix matrix)
        {
            if (matrix is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (matrix.IsEmpty)
            {
                return Empty;
            }
            else
            {
                Size size = matrix.Size;

                double[,] result = new double[size.Width, size.Height];

                double[,] arrayR = matrix._MArray;

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = n + arrayR[x, y];
                    }
                }

                return UnsafeCreateInstance(result);
            }
        }

        /// <summary>
        /// 返回将 Matrix 对象与 Matrix 对象相加得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="left">Matrix 对象，表示被加数。</param>
        /// <param name="right">Matrix 对象，表示加数。</param>
        /// <returns>Matrix 对象，表示将 Matrix 对象与 Matrix 对象相加得到的结果。</returns>
        public static Matrix Add(Matrix left, Matrix right)
        {
            if (left is null || right is null)
            {
                throw new ArgumentNullException();
            }

            //

            bool LIsEmpty = left.IsEmpty;
            bool RIsEmpty = right.IsEmpty;

            if (LIsEmpty || RIsEmpty)
            {
                if (LIsEmpty && RIsEmpty)
                {
                    return Empty;
                }
                else
                {
                    throw new ArithmeticException();
                }
            }
            else
            {
                Size sizeL = left.Size;
                Size sizeR = right.Size;

                if (sizeL != sizeR)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    Size size = sizeL;

                    double[,] result = new double[size.Width, size.Height];

                    double[,] arrayL = left._MArray, arrayR = right._MArray;

                    for (int x = 0; x < size.Width; x++)
                    {
                        for (int y = 0; y < size.Height; y++)
                        {
                            result[x, y] = arrayL[x, y] + arrayR[x, y];
                        }
                    }

                    return UnsafeCreateInstance(result);
                }
            }
        }

        /// <summary>
        /// 返回将 Matrix 对象与双精度浮点数相减得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrix">Matrix 对象，表示被减数。</param>
        /// <param name="n">双精度浮点数，表示减数。</param>
        /// <returns>Matrix 对象，表示将 Matrix 对象与双精度浮点数相减得到的结果。</returns>
        public static Matrix Subtract(Matrix matrix, double n)
        {
            if (matrix is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (matrix.IsEmpty)
            {
                return Empty;
            }
            else
            {
                Size size = matrix.Size;

                double[,] result = new double[size.Width, size.Height];

                double[,] arrayL = matrix._MArray;

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = arrayL[x, y] - n;
                    }
                }

                return UnsafeCreateInstance(result);
            }
        }

        /// <summary>
        /// 返回将双精度浮点数与 Matrix 对象相减得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被减数。</param>
        /// <param name="matrix">Matrix 对象，表示减数。</param>
        /// <returns>Matrix 对象，表示将双精度浮点数与 Matrix 对象相减得到的结果。</returns>
        public static Matrix Subtract(double n, Matrix matrix)
        {
            if (matrix is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (matrix.IsEmpty)
            {
                return Empty;
            }
            else
            {
                Size size = matrix.Size;

                double[,] result = new double[size.Width, size.Height];

                double[,] arrayR = matrix._MArray;

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = n - arrayR[x, y];
                    }
                }

                return UnsafeCreateInstance(result);
            }
        }

        /// <summary>
        /// 返回将 Matrix 对象与 Matrix 对象相减得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="left">Matrix 对象，表示被减数。</param>
        /// <param name="right">Matrix 对象，表示减数。</param>
        /// <returns>Matrix 对象，表示将 Matrix 对象与 Matrix 对象相减得到的结果。</returns>
        public static Matrix Subtract(Matrix left, Matrix right)
        {
            if (left is null || right is null)
            {
                throw new ArgumentNullException();
            }

            //

            bool LIsEmpty = left.IsEmpty;
            bool RIsEmpty = right.IsEmpty;

            if (LIsEmpty || RIsEmpty)
            {
                if (LIsEmpty && RIsEmpty)
                {
                    return Empty;
                }
                else
                {
                    throw new ArithmeticException();
                }
            }
            else
            {
                Size sizeL = left.Size;
                Size sizeR = right.Size;

                if (sizeL != sizeR)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    Size size = sizeL;

                    double[,] result = new double[size.Width, size.Height];

                    double[,] arrayL = left._MArray, arrayR = right._MArray;

                    for (int x = 0; x < size.Width; x++)
                    {
                        for (int y = 0; y < size.Height; y++)
                        {
                            result[x, y] = arrayL[x, y] - arrayR[x, y];
                        }
                    }

                    return UnsafeCreateInstance(result);
                }
            }
        }

        /// <summary>
        /// 返回将 Matrix 对象与双精度浮点数相乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrix">Matrix 对象，表示被乘数。</param>
        /// <param name="n">双精度浮点数，表示乘数。</param>
        /// <returns>Matrix 对象，表示将 Matrix 对象与双精度浮点数相乘得到的结果。</returns>
        public static Matrix Multiply(Matrix matrix, double n)
        {
            if (matrix is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (matrix.IsEmpty)
            {
                return Empty;
            }
            else
            {
                Size size = matrix.Size;

                double[,] result = new double[size.Width, size.Height];

                double[,] arrayL = matrix._MArray;

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = arrayL[x, y] * n;
                    }
                }

                return UnsafeCreateInstance(result);
            }
        }

        /// <summary>
        /// 返回将双精度浮点数与 Matrix 对象相乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被乘数。</param>
        /// <param name="matrix">Matrix 对象，表示乘数。</param>
        /// <returns>Matrix 对象，表示将双精度浮点数与 Matrix 对象相乘得到的结果。</returns>
        public static Matrix Multiply(double n, Matrix matrix)
        {
            if (matrix is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (matrix.IsEmpty)
            {
                return Empty;
            }
            else
            {
                Size size = matrix.Size;

                double[,] result = new double[size.Width, size.Height];

                double[,] arrayR = matrix._MArray;

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = n * arrayR[x, y];
                    }
                }

                return UnsafeCreateInstance(result);
            }
        }

        /// <summary>
        /// 返回将 Matrix 对象与 Matrix 对象相乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="left">Matrix 对象，表示被乘数。</param>
        /// <param name="right">Matrix 对象，表示乘数。</param>
        /// <returns>Matrix 对象，表示将 Matrix 对象与 Matrix 对象相乘得到的结果。</returns>
        public static Matrix Multiply(Matrix left, Matrix right)
        {
            if (left is null || right is null)
            {
                throw new ArgumentNullException();
            }

            //

            bool LIsEmpty = left.IsEmpty;
            bool RIsEmpty = right.IsEmpty;

            if (LIsEmpty || RIsEmpty)
            {
                return Empty;
            }
            else
            {
                Size sizeL = left.Size;
                Size sizeR = right.Size;

                if (sizeL.Width != sizeR.Height)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    int height = sizeL.Width;

                    Size size = new Size(sizeR.Width, sizeL.Height);

                    double[,] result = new double[size.Width, size.Height];

                    double[,] arrayL = left._MArray, arrayR = right._MArray;

                    if (size.Width <= size.Height)
                    {
                        for (int x = 0; x < size.Width; x++)
                        {
                            for (int i = 0; i < height; i++)
                            {
                                double Rxi = arrayR[x, i];

                                if (Rxi != 0)
                                {
                                    for (int y = 0; y < size.Height; y++)
                                    {
                                        result[x, y] += (arrayL[i, y] * Rxi);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int y = 0; y < size.Height; y++)
                        {
                            for (int i = 0; i < height; i++)
                            {
                                double Liy = arrayL[i, y];

                                if (Liy != 0)
                                {
                                    for (int x = 0; x < size.Width; x++)
                                    {
                                        result[x, y] += (Liy * arrayR[x, i]);
                                    }
                                }
                            }
                        }
                    }

                    return UnsafeCreateInstance(result);
                }
            }
        }

        /// <summary>
        /// 返回将 Matrix 对象与 Vector 对象相乘得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">Matrix 对象，表示被乘数。</param>
        /// <param name="right">Vector 对象，表示乘数。</param>
        /// <returns>Vector 对象，表示将 Matrix 对象与 Vector 对象相乘得到的结果。</returns>
        public static Vector Multiply(Matrix left, Vector right)
        {
            if (left is null || right is null)
            {
                throw new ArgumentNullException();
            }

            //

            bool LIsEmpty = left.IsEmpty;
            bool RIsEmpty = right.IsEmpty;

            if (LIsEmpty || RIsEmpty)
            {
                if (LIsEmpty && RIsEmpty)
                {
                    return Vector.Empty;
                }
                else if (LIsEmpty)
                {
                    return Vector.Empty;
                }
                else
                {
                    throw new ArithmeticException();
                }
            }
            else
            {
                if (!right.IsColumnVector)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    Size sizeL = left.Size;
                    int heightR = right.Dimension;

                    if (sizeL.Width != heightR)
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        double[] result = new double[sizeL.Height];

                        double[,] arrayL = left._MArray;
                        double[] arrayR = right.UnsafeGetData();

                        for (int i = 0; i < heightR; i++)
                        {
                            double Ri = arrayR[i];

                            if (Ri != 0)
                            {
                                for (int y = 0; y < sizeL.Height; y++)
                                {
                                    result[y] += (arrayL[i, y] * Ri);
                                }
                            }
                        }

                        return Vector.UnsafeCreateInstance(Vector.Type.ColumnVector, result);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将 Vector 对象与 Matrix 对象相乘得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">Vector 对象，表示被乘数。</param>
        /// <param name="right">Matrix 对象，表示乘数。</param>
        /// <returns>Vector 对象，表示将 Vector 对象与 Matrix 对象相乘得到的结果。</returns>
        public static Vector Multiply(Vector left, Matrix right)
        {
            if (left is null || right is null)
            {
                throw new ArgumentNullException();
            }

            //

            bool LIsEmpty = left.IsEmpty;
            bool RIsEmpty = right.IsEmpty;

            if (LIsEmpty || RIsEmpty)
            {
                if (LIsEmpty && RIsEmpty)
                {
                    return Vector.Empty;
                }
                else if (LIsEmpty)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    return Vector.Empty;
                }
            }
            else
            {
                if (!left.IsRowVector)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    int widthL = left.Dimension;
                    Size sizeR = right.Size;

                    if (widthL != sizeR.Height)
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        double[] result = new double[sizeR.Width];

                        double[] arrayL = left.UnsafeGetData();
                        double[,] arrayR = right._MArray;

                        for (int i = 0; i < widthL; i++)
                        {
                            double Li = arrayL[i];

                            if (Li != 0)
                            {
                                for (int x = 0; x < sizeR.Width; x++)
                                {
                                    result[x] += (arrayR[x, i] * Li);
                                }
                            }
                        }

                        return Vector.UnsafeCreateInstance(Vector.Type.RowVector, result);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将数组中所有 Matrix 对象依次左乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrices">左矩阵数组。</param>
        /// <returns>Matrix 对象，表示将数组中所有 Matrix 对象依次左乘得到的结果。</returns>
        public static Matrix MultiplyLeft(params Matrix[] matrices)
        {
            if (InternalMethod.IsNullOrEmpty(matrices))
            {
                throw new ArgumentNullException();
            }

            for (int i = 0; i < matrices.Length; i++)
            {
                if (matrices[i] is null)
                {
                    throw new ArgumentNullException();
                }
            }

            //

            if (matrices.Length == 1)
            {
                return matrices[0].Copy();
            }
            else
            {
                Matrix result = matrices[0];

                for (int i = 1; i < matrices.Length; i++)
                {
                    result = Multiply(matrices[i], result);

                    if (result.IsEmpty)
                    {
                        return Empty;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将枚举容器中所有 Matrix 对象依次左乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrices">左矩阵枚举容器。</param>
        /// <returns>Matrix 对象，表示将枚举容器中所有 Matrix 对象依次左乘得到的结果。</returns>
        public static Matrix MultiplyLeft(IEnumerable<Matrix> matrices)
        {
            if (matrices is null)
            {
                throw new ArgumentNullException();
            }

            //

            return MultiplyLeft(matrices.ToArray());
        }

        /// <summary>
        /// 返回将列表中所有 Matrix 对象依次左乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrices">左矩阵列表。</param>
        /// <returns>Matrix 对象，表示将列表中所有 Matrix 对象依次左乘得到的结果。</returns>
        [Obsolete]
        public static Matrix MultiplyLeft(List<Matrix> matrices)
        {
            if (InternalMethod.IsNullOrEmpty(matrices))
            {
                throw new ArgumentNullException();
            }

            //

            return MultiplyLeft(matrices.ToArray());
        }

        /// <summary>
        /// 返回将数组中所有 Matrix 对象依次右乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrices">右矩阵数组。</param>
        /// <returns>Matrix 对象，表示将数组中所有 Matrix 对象依次右乘得到的结果。</returns>
        public static Matrix MultiplyRight(params Matrix[] matrices)
        {
            if (InternalMethod.IsNullOrEmpty(matrices))
            {
                throw new ArgumentNullException();
            }

            for (int i = 0; i < matrices.Length; i++)
            {
                if (matrices[i] is null)
                {
                    throw new ArgumentNullException();
                }
            }

            //

            if (matrices.Length == 1)
            {
                return matrices[0].Copy();
            }
            else
            {
                Matrix result = matrices[0];

                for (int i = 1; i < matrices.Length; i++)
                {
                    result = Multiply(result, matrices[i]);

                    if (result.IsEmpty)
                    {
                        return Empty;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将枚举容器中所有 Matrix 对象依次右乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrices">右矩阵枚举容器。</param>
        /// <returns>Matrix 对象，表示将枚举容器中所有 Matrix 对象依次右乘得到的结果。</returns>
        public static Matrix MultiplyRight(IEnumerable<Matrix> matrices)
        {
            return MultiplyRight(matrices.ToArray());
        }

        /// <summary>
        /// 返回将列表中所有 Matrix 对象依次右乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrices">右矩阵列表。</param>
        /// <returns>Matrix 对象，表示将列表中所有 Matrix 对象依次右乘得到的结果。</returns>
        [Obsolete]
        public static Matrix MultiplyRight(List<Matrix> matrices)
        {
            return MultiplyRight(matrices.ToArray());
        }

        /// <summary>
        /// 返回将 Matrix 对象与双精度浮点数相除得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrix">Matrix 对象，表示被除数。</param>
        /// <param name="n">双精度浮点数，表示除数。</param>
        /// <returns>Matrix 对象，表示将 Matrix 对象与双精度浮点数相除得到的结果。</returns>
        public static Matrix Divide(Matrix matrix, double n)
        {
            if (matrix is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (matrix.IsEmpty)
            {
                return Empty;
            }
            else
            {
                Size size = matrix.Size;

                double[,] result = new double[size.Width, size.Height];

                double[,] arrayL = matrix._MArray;

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = arrayL[x, y] / n;
                    }
                }

                return UnsafeCreateInstance(result);
            }
        }

        /// <summary>
        /// 返回将双精度浮点数与 Matrix 对象相除得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被除数。</param>
        /// <param name="matrix">Matrix 对象，表示除数。</param>
        /// <returns>Matrix 对象，表示将双精度浮点数与 Matrix 对象相除得到的结果。</returns>
        public static Matrix Divide(double n, Matrix matrix)
        {
            if (matrix is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (matrix.IsEmpty)
            {
                return Empty;
            }
            else
            {
                Size size = matrix.Size;

                double[,] result = new double[size.Width, size.Height];

                double[,] arrayR = matrix._MArray;

                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        result[x, y] = n / arrayR[x, y];
                    }
                }

                return UnsafeCreateInstance(result);
            }
        }

        /// <summary>
        /// 返回将 Matrix 对象与 Matrix 对象左除得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="left">Matrix 对象，表示被除数。</param>
        /// <param name="right">Matrix 对象，表示除数。</param>
        /// <returns>Matrix 对象，表示将 Matrix 对象与 Matrix 对象左除得到的结果。</returns>
        public static Matrix DivideLeft(Matrix left, Matrix right)
        {
            if (left is null || right is null)
            {
                throw new ArgumentNullException();
            }

            //

            bool LIsEmpty = left.IsEmpty;
            bool RIsEmpty = right.IsEmpty;

            if (LIsEmpty || RIsEmpty)
            {
                if (LIsEmpty)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    _ = left.Invert;

                    return Empty;
                }
            }
            else
            {
                Size sizeL = left.Size;
                Size sizeR = right.Size;

                if (sizeL.Width != sizeL.Height || sizeL.Width != sizeR.Height)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    return Multiply(left.Invert, right);
                }
            }
        }

        /// <summary>
        /// 返回将 Matrix 对象与 Vector 对象左除得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">Matrix 对象，表示被除数。</param>
        /// <param name="right">Vector 对象，表示除数。</param>
        /// <returns>Vector 对象，表示将 Matrix 对象与 Vector 对象左除得到的结果。</returns>
        public static Vector DivideLeft(Matrix left, Vector right)
        {
            if (left is null || right is null)
            {
                throw new ArgumentNullException();
            }

            //

            bool LIsEmpty = left.IsEmpty;
            bool RIsEmpty = right.IsEmpty;

            if (LIsEmpty || RIsEmpty)
            {
                throw new ArithmeticException();
            }
            else
            {
                if (!right.IsRowVector)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    Size sizeL = left.Size;
                    int heightR = right.Dimension;

                    if (sizeL.Width != sizeL.Height || sizeL.Width != heightR)
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        return Multiply(left.Invert, right);
                    }
                }
            }
        }

        /// <summary>
        /// 返回将 Matrix 对象与 Matrix 对象右除得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="left">Matrix 对象，表示被除数。</param>
        /// <param name="right">Matrix 对象，表示除数。</param>
        /// <returns>Matrix 对象，表示将 Matrix 对象与 Matrix 对象右除得到的结果。</returns>
        public static Matrix DivideRight(Matrix left, Matrix right)
        {
            if (left is null || right is null)
            {
                throw new ArgumentNullException();
            }

            //

            bool LIsEmpty = left.IsEmpty;
            bool RIsEmpty = right.IsEmpty;

            if (LIsEmpty || RIsEmpty)
            {
                if (RIsEmpty)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    _ = right.Invert;

                    return Empty;
                }
            }
            else
            {
                Size sizeL = left.Size;
                Size sizeR = right.Size;

                if (sizeR.Width != sizeR.Height || sizeL.Width != sizeR.Height)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    return Multiply(left, right.Invert);
                }
            }
        }

        /// <summary>
        /// 返回将 Vector 对象与 Matrix 对象右除得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">Vector 对象，表示被除数。</param>
        /// <param name="right">Matrix 对象，表示除数。</param>
        /// <returns>Vector 对象，表示将 Vector 对象与 Matrix 对象右除得到的结果。</returns>
        public static Vector DivideRight(Vector left, Matrix right)
        {
            if (left is null || right is null)
            {
                throw new ArgumentNullException();
            }

            //

            bool LIsEmpty = left.IsEmpty;
            bool RIsEmpty = right.IsEmpty;

            if (LIsEmpty || RIsEmpty)
            {
                throw new ArithmeticException();
            }
            else
            {
                if (!left.IsRowVector)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    int widthL = left.Dimension;
                    Size sizeR = right.Size;

                    if (sizeR.Width != sizeR.Height || widthL != sizeR.Height)
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        return Multiply(left, right.Invert);
                    }
                }
            }
        }

        //

        /// <summary>
        /// 返回由 Matrix 对象表示的系数矩阵与 Vector 对象表示的常数项指定的非齐次线性方程组的解向量。
        /// </summary>
        /// <param name="matrix">Matrix 对象，表示系数矩阵。</param>
        /// <param name="vector">Vector 对象，表示常数项。</param>
        /// <returns>Vector 对象，表示由 Matrix 对象与 Vector 对象指定的非齐次线性方程组的解向量。</returns>
        public static Vector SolveLinearEquation(Matrix matrix, Vector vector)
        {
            if (matrix is null || vector is null)
            {
                throw new ArgumentNullException();
            }

            //

            bool LIsEmpty = matrix.IsEmpty;
            bool RIsEmpty = vector.IsEmpty;

            if (LIsEmpty || RIsEmpty)
            {
                throw new ArithmeticException();
            }
            else
            {
                if (!vector.IsRowVector)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    Size sizeL = matrix.Size;
                    int heightR = vector.Dimension;

                    if (sizeL.Width != sizeL.Height || sizeL.Width != heightR)
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        try
                        {
                            return Multiply(matrix.Invert, vector);
                        }
                        catch
                        {
                            return Vector.Empty;
                        }
                    }
                }
            }
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 Matrix 对象是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Matrix 对象。</param>
        /// <param name="right">运算符右侧比较的 Matrix 对象。</param>
        /// <returns>布尔值，表示两个 Matrix 对象是否相等。</returns>
        public static bool operator ==(Matrix left, Matrix right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Size != right._Size)
            {
                return false;
            }
            else
            {
                double[,] arrayL = left._MArray, arrayR = right._MArray;

                for (int x = 0; x < left._Size.Width; x++)
                {
                    for (int y = 0; y < left._Size.Height; y++)
                    {
                        if (arrayL[x, y] != arrayR[x, y])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// 判断两个 Matrix 对象是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Matrix 对象。</param>
        /// <param name="right">运算符右侧比较的 Matrix 对象。</param>
        /// <returns>布尔值，表示两个 Matrix 对象是否不相等。</returns>
        public static bool operator !=(Matrix left, Matrix right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return false;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Size != right._Size)
            {
                return true;
            }
            else
            {
                double[,] arrayL = left._MArray, arrayR = right._MArray;

                for (int x = 0; x < left._Size.Width; x++)
                {
                    for (int y = 0; y < left._Size.Height; y++)
                    {
                        if (arrayL[x, y] != arrayR[x, y])
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        #endregion
    }
}