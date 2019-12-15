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
                if ((_Size.Width <= 0 || _Size.Height <= 0) || (x < 0 || x >= _Size.Width) || (y < 0 || y >= _Size.Height))
                {
                    throw new IndexOutOfRangeException();
                }

                //

                return _MArray[x, y];
            }

            set
            {
                if ((_Size.Width <= 0 || _Size.Height <= 0) || (x < 0 || x >= _Size.Width) || (y < 0 || y >= _Size.Height))
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
                if ((_Size.Width <= 0 || _Size.Height <= 0) || (index.X < 0 || index.X >= _Size.Width) || (index.Y < 0 || index.Y >= _Size.Height))
                {
                    throw new IndexOutOfRangeException();
                }

                //

                return _MArray[index.X, index.Y];
            }

            set
            {
                if ((_Size.Width <= 0 || _Size.Height <= 0) || (index.X < 0 || index.X >= _Size.Width) || (index.Y < 0 || index.Y >= _Size.Height))
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
                if (_Size.Width <= 0 || _Size.Height <= 0)
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
                return Width;
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
                return Height;
            }
        }

        /// <summary>
        /// 获取此 Matrix 包含的元素数量。
        /// </summary>
        public int Count
        {
            get
            {
                if (_Size.Width <= 0 || _Size.Height <= 0)
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
                if ((_Size.Width <= 0 || _Size.Height <= 0) || _Size.Width != _Size.Height)
                {
                    return double.NaN;
                }
                else
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
        }

        /// <summary>
        /// 获取此 Matrix 的秩。
        /// </summary>
        public int Rank
        {
            get
            {
                if (_Size.Width <= 0 || _Size.Height <= 0)
                {
                    return -1;
                }
                else
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

                                if (IsNullOrEmpty(sub))
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
            }
        }

        /// <summary>
        /// 获取此 Matrix 的转置矩阵。
        /// </summary>
        public Matrix Transport
        {
            get
            {
                if (_Size.Width <= 0 || _Size.Height <= 0)
                {
                    return Empty;
                }
                else
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
            }
        }

        /// <summary>
        /// 获取此 Matrix 的伴随矩阵。
        /// </summary>
        public Matrix Adjoint
        {
            get
            {
                if ((_Size.Width <= 0 || _Size.Height <= 0) || _Size.Width != _Size.Height)
                {
                    return Empty;
                }
                else
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
            }
        }

        /// <summary>
        /// 获取此 Matrix 的逆矩阵。
        /// </summary>
        public Matrix Invert
        {
            get
            {
                if ((_Size.Width <= 0 || _Size.Height <= 0) || _Size.Width != _Size.Height)
                {
                    return Empty;
                }
                else
                {
                    double det = Determinant;

                    if (InternalMethod.IsNaNOrInfinity(det) || det == 0)
                    {
                        return Empty;
                    }
                    else
                    {
                        Matrix result = Adjoint;

                        if (IsNullOrEmpty(result))
                        {
                            return Empty;
                        }
                        else
                        {
                            for (int x = 0; x < _Size.Width; x++)
                            {
                                for (int y = 0; y < _Size.Height; y++)
                                {
                                    result._MArray[x, y] /= det;

                                    if (InternalMethod.IsNaNOrInfinity(result._MArray[x, y]))
                                    {
                                        return Empty;
                                    }
                                }
                            }

                            return result;
                        }
                    }
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

            if (_Size.Width <= 0 || _Size.Height <= 0)
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
        /// <returns>Matrix 对象，表示此 Matrix 的副本。</returns>
        public Matrix Copy()
        {
            if (_Size.Width <= 0 || _Size.Height <= 0)
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
            if ((_Size.Width <= 0 || _Size.Height <= 0) || (size.Width <= 0 || size.Height <= 0) || (index.X < 0 || index.X + size.Width > _Size.Width) || (index.Y < 0 || index.Y + size.Height > _Size.Height))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

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
            if ((_Size.Width <= 0 || _Size.Height <= 0) || (width <= 0 || height <= 0) || (x < 0 || x + width > _Size.Width) || (y < 0 || y + height > _Size.Height))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

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

        //

        /// <summary>
        /// 获取表示此 Matrix 的指定列的 Vector 的新实例。
        /// </summary>
        /// <param name="x">指定列在此 Matrix 的宽度方向（列）的索引。</param>
        /// <returns>Vector 对象，表示此 Matrix 的指定列。</returns>
        public Vector GetColumn(int x)
        {
            if ((_Size.Width <= 0 || _Size.Height <= 0) || (x < 0 || x >= _Size.Width))
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
        /// 获取表示此 Matrix 的指定行的 Vector 的新实例。
        /// </summary>
        /// <param name="y">指定行在此 Matrix 的高度方向（行）的索引。</param>
        /// <returns>Vector 对象，表示此 Matrix 的指定行。</returns>
        public Vector GetRow(int y)
        {
            if ((_Size.Width <= 0 || _Size.Height <= 0) || (y < 0 || y >= _Size.Height))
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

        //

        /// <summary>
        /// 将此 Matrix 转换为双精度浮点数二维数组。
        /// </summary>
        /// <returns>双精度浮点数二维数组，表示转换的结果。</returns>
        public double[,] ToArray()
        {
            if (_Size.Width <= 0 || _Size.Height <= 0)
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

            //

            if (order == 0)
            {
                return Empty;
            }
            else
            {
                Matrix result = new Matrix(order, order);

                for (int i = 0; i < order; i++)
                {
                    result._MArray[i, i] = 1;
                }

                return result;
            }
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

            //

            if (size.Width == 0 || size.Height == 0)
            {
                return Empty;
            }
            else
            {
                return new Matrix(size);
            }
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

            //

            if (width == 0 || height == 0)
            {
                return Empty;
            }
            else
            {
                return new Matrix(width, height);
            }
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

            //

            if (size.Width == 0 || size.Height == 0)
            {
                return Empty;
            }
            else
            {
                return new Matrix(size, 1);
            }
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

            //

            if (width == 0 || height == 0)
            {
                return Empty;
            }
            else
            {
                return new Matrix(width, height, 1);
            }
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

                Matrix result = new Matrix(order, order);

                if (rowsUponMainDiag >= 0)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        result._MArray[i + rowsUponMainDiag, i] = array[i];
                    }
                }
                else
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        result._MArray[i, i - rowsUponMainDiag] = array[i];
                    }
                }

                return result;
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

                Matrix result = new Matrix(order, order);

                for (int i = 0; i < array.Length; i++)
                {
                    result._MArray[i, i] = array[i];
                }

                return result;
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
            bool LIsNOrE = IsNullOrEmpty(left);
            bool RIsNOrE = IsNullOrEmpty(right);

            if (LIsNOrE && RIsNOrE)
            {
                return Empty;
            }
            else if (LIsNOrE || RIsNOrE)
            {
                throw new ArithmeticException();
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

                    Matrix result = new Matrix(size);

                    for (int x = 0; x < sizeL.Width; x++)
                    {
                        for (int y = 0; y < size.Height; y++)
                        {
                            result._MArray[x, y] = left._MArray[x, y];
                        }
                    }

                    for (int x = sizeL.Width; x < size.Width; x++)
                    {
                        for (int y = 0; y < size.Height; y++)
                        {
                            result._MArray[x, y] = right._MArray[x - sizeL.Width, y];
                        }
                    }

                    return result;
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
            if (IsNullOrEmpty(matrix))
            {
                return Empty;
            }
            else
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
        }

        /// <summary>
        /// 返回将双精度浮点数与 Matrix 对象相加得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被加数。</param>
        /// <param name="matrix">Matrix 对象，表示加数。</param>
        /// <returns>Matrix 对象，表示将双精度浮点数与 Matrix 对象相加得到的结果。</returns>
        public static Matrix Add(double n, Matrix matrix)
        {
            if (IsNullOrEmpty(matrix))
            {
                return Empty;
            }
            else
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
        }

        /// <summary>
        /// 返回将 Matrix 对象与 Matrix 对象相加得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="left">Matrix 对象，表示被加数。</param>
        /// <param name="right">Matrix 对象，表示加数。</param>
        /// <returns>Matrix 对象，表示将 Matrix 对象与 Matrix 对象相加得到的结果。</returns>
        public static Matrix Add(Matrix left, Matrix right)
        {
            bool LIsNOrE = IsNullOrEmpty(left);
            bool RIsNOrE = IsNullOrEmpty(right);

            if (LIsNOrE && RIsNOrE)
            {
                return Empty;
            }
            else if (LIsNOrE || RIsNOrE)
            {
                throw new ArithmeticException();
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
        }

        /// <summary>
        /// 返回将 Matrix 对象与双精度浮点数相减得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrix">Matrix 对象，表示被减数。</param>
        /// <param name="n">双精度浮点数，表示减数。</param>
        /// <returns>Matrix 对象，表示将 Matrix 对象与双精度浮点数相减得到的结果。</returns>
        public static Matrix Subtract(Matrix matrix, double n)
        {
            if (IsNullOrEmpty(matrix))
            {
                return Empty;
            }
            else
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
        }

        /// <summary>
        /// 返回将双精度浮点数与 Matrix 对象相减得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被减数。</param>
        /// <param name="matrix">Matrix 对象，表示减数。</param>
        /// <returns>Matrix 对象，表示将双精度浮点数与 Matrix 对象相减得到的结果。</returns>
        public static Matrix Subtract(double n, Matrix matrix)
        {
            if (IsNullOrEmpty(matrix))
            {
                return Empty;
            }
            else
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
        }

        /// <summary>
        /// 返回将 Matrix 对象与 Matrix 对象相减得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="left">Matrix 对象，表示被减数。</param>
        /// <param name="right">Matrix 对象，表示减数。</param>
        /// <returns>Matrix 对象，表示将 Matrix 对象与 Matrix 对象相减得到的结果。</returns>
        public static Matrix Subtract(Matrix left, Matrix right)
        {
            bool LIsNOrE = IsNullOrEmpty(left);
            bool RIsNOrE = IsNullOrEmpty(right);

            if (LIsNOrE && RIsNOrE)
            {
                return Empty;
            }
            else if (LIsNOrE || RIsNOrE)
            {
                throw new ArithmeticException();
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
        }

        /// <summary>
        /// 返回将 Matrix 对象与双精度浮点数相乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrix">Matrix 对象，表示被乘数。</param>
        /// <param name="n">双精度浮点数，表示乘数。</param>
        /// <returns>Matrix 对象，表示将 Matrix 对象与双精度浮点数相乘得到的结果。</returns>
        public static Matrix Multiply(Matrix matrix, double n)
        {
            if (IsNullOrEmpty(matrix))
            {
                return Empty;
            }
            else
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
        }

        /// <summary>
        /// 返回将双精度浮点数与 Matrix 对象相乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被乘数。</param>
        /// <param name="matrix">Matrix 对象，表示乘数。</param>
        /// <returns>Matrix 对象，表示将双精度浮点数与 Matrix 对象相乘得到的结果。</returns>
        public static Matrix Multiply(double n, Matrix matrix)
        {
            if (IsNullOrEmpty(matrix))
            {
                return Empty;
            }
            else
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
        }

        /// <summary>
        /// 返回将 Matrix 对象与 Matrix 对象相乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="left">Matrix 对象，表示被乘数。</param>
        /// <param name="right">Matrix 对象，表示乘数。</param>
        /// <returns>Matrix 对象，表示将 Matrix 对象与 Matrix 对象相乘得到的结果。</returns>
        public static Matrix Multiply(Matrix left, Matrix right)
        {
            bool LIsNOrE = IsNullOrEmpty(left);
            bool RIsNOrE = IsNullOrEmpty(right);

            if (LIsNOrE && RIsNOrE)
            {
                return Empty;
            }
            else if (LIsNOrE || RIsNOrE)
            {
                throw new ArithmeticException();
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

                    Matrix result = new Matrix(size);

                    if (size.Width <= size.Height)
                    {
                        for (int x = 0; x < size.Width; x++)
                        {
                            for (int i = 0; i < height; i++)
                            {
                                double Rxi = right._MArray[x, i];

                                if (Rxi != 0)
                                {
                                    for (int y = 0; y < size.Height; y++)
                                    {
                                        result._MArray[x, y] += (left._MArray[i, y] * Rxi);
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
                                double Liy = left._MArray[i, y];

                                if (Liy != 0)
                                {
                                    for (int x = 0; x < size.Width; x++)
                                    {
                                        result._MArray[x, y] += (Liy * right._MArray[x, i]);
                                    }
                                }
                            }
                        }
                    }

                    return result;
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
                return Empty;
            }
            else
            {
                if (matrices.Length == 1)
                {
                    if (IsNullOrEmpty(matrices[0]))
                    {
                        return Empty;
                    }
                    else
                    {
                        return matrices[0].Copy();
                    }
                }
                else
                {
                    Matrix result = matrices[0].Copy();

                    for (int i = 1; i < matrices.Length; i++)
                    {
                        result = Multiply(matrices[i], result);
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// 返回将列表中所有 Matrix 对象依次左乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrices">左矩阵列表。</param>
        /// <returns>Matrix 对象，表示将列表中所有 Matrix 对象依次左乘得到的结果。</returns>
        public static Matrix MultiplyLeft(List<Matrix> matrices)
        {
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
                return Empty;
            }
            else
            {
                if (matrices.Length == 1)
                {
                    if (IsNullOrEmpty(matrices[0]))
                    {
                        return Empty;
                    }
                    else
                    {
                        return matrices[0].Copy();
                    }
                }
                else
                {
                    Matrix result = matrices[0].Copy();

                    for (int i = 1; i < matrices.Length; i++)
                    {
                        result = Multiply(result, matrices[i]);
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// 返回将列表中所有 Matrix 对象依次右乘得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="matrices">右矩阵列表。</param>
        /// <returns>Matrix 对象，表示将列表中所有 Matrix 对象依次右乘得到的结果。</returns>
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
            if (IsNullOrEmpty(matrix))
            {
                return Empty;
            }
            else
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
        }

        /// <summary>
        /// 返回将双精度浮点数与 Matrix 对象相除得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被除数。</param>
        /// <param name="matrix">Matrix 对象，表示除数。</param>
        /// <returns>Matrix 对象，表示将双精度浮点数与 Matrix 对象相除得到的结果。</returns>
        public static Matrix Divide(double n, Matrix matrix)
        {
            if (IsNullOrEmpty(matrix))
            {
                return Empty;
            }
            else
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
        }

        /// <summary>
        /// 返回将 Matrix 对象与 Matrix 对象左除得到的 Matrix 的新实例。
        /// </summary>
        /// <param name="left">Matrix 对象，表示被除数。</param>
        /// <param name="right">Matrix 对象，表示除数。</param>
        /// <returns>Matrix 对象，表示将 Matrix 对象与 Matrix 对象左除得到的结果。</returns>
        public static Matrix DivideLeft(Matrix left, Matrix right)
        {
            bool LIsNOrE = IsNullOrEmpty(left);
            bool RIsNOrE = IsNullOrEmpty(right);

            if (LIsNOrE && RIsNOrE)
            {
                return Empty;
            }
            else if (LIsNOrE || RIsNOrE)
            {
                throw new ArithmeticException();
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
                    Matrix invLeft = left.Invert;

                    if (IsNullOrEmpty(invLeft))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        return Multiply(invLeft, right);
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
            bool LIsNOrE = IsNullOrEmpty(left);
            bool RIsNOrE = IsNullOrEmpty(right);

            if (LIsNOrE && RIsNOrE)
            {
                return Empty;
            }
            else if (LIsNOrE || RIsNOrE)
            {
                throw new ArithmeticException();
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
                    Matrix invRight = right.Invert;

                    if (IsNullOrEmpty(invRight))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        return Multiply(left, invRight);
                    }
                }
            }
        }

        //

        /// <summary>
        /// 返回由 Matrix 对象与 Vector 对象指定的非齐次线性方程组的解向量。
        /// </summary>
        /// <param name="matrix">Matrix 对象，表示系数矩阵。</param>
        /// <param name="vector">Vector 对象，表示常数项。</param>
        /// <returns>Vector 对象，表示由 Matrix 对象与 Vector 对象指定的非齐次线性方程组的解向量。</returns>
        public static Vector SolveLinearEquation(Matrix matrix, Vector vector)
        {
            bool MIsNOrE = IsNullOrEmpty(matrix);
            bool VIsNOrE = Vector.IsNullOrEmpty(vector);

            if (MIsNOrE && VIsNOrE)
            {
                return Vector.Empty;
            }
            else if (MIsNOrE || VIsNOrE)
            {
                throw new ArithmeticException();
            }
            else if (!vector.IsColumnVector)
            {
                throw new ArithmeticException();
            }
            else
            {
                Size size = matrix.Size;

                if (size.Width != size.Height)
                {
                    throw new ArithmeticException();
                }
                else
                {
                    int order = vector.Dimension;

                    if (order != size.Height)
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        Matrix solution = DivideLeft(matrix, vector.ToMatrix());

                        if (IsNullOrEmpty(solution) || solution.Size != new Size(1, order))
                        {
                            throw new ArithmeticException();
                        }
                        else
                        {
                            return solution.GetColumn(0);
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
        }

        #endregion
    }
}