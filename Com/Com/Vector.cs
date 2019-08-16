/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.Vector
Version 19.8.16.0000

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Drawing;

namespace Com
{
    /// <summary>
    /// 以一组有序的双精度浮点数表示的向量。
    /// </summary>
    public sealed class Vector : IEquatable<Vector>, IComparable, IComparable<Vector>, IEuclideanVector<Vector>, ILinearAlgebraVector<Vector>, IAffine<Vector>
    {
        /// <summary>
        /// 向量类型。
        /// </summary>
        public enum Type
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

        #region 私有成员与内部成员

        private const int _MaxSize = 2146435071; // Vector 允许包含的最大元素数量，等于 System.Array.MaxArrayLength。

        //

        private static Vector _GetZeroVector(Type type, int dimension) // 获取指定向量类型与维度的零向量。
        {
            if (dimension < 0 || dimension > _MaxSize)
            {
                throw new OverflowException();
            }

            //

            if (dimension == 0)
            {
                return Empty;
            }
            else
            {
                return new Vector()
                {
                    _Type = type,
                    _Size = dimension,
                    _VArray = new double[dimension]
                };
            }
        }

        //

        internal static Vector UnsafeCreateInstance(Type type, params double[] values) // 以不安全方式创建 Vector 的新实例。
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                return new Vector()
                {
                    _Type = type,
                    _Size = 0,
                    _VArray = new double[0]
                };
            }
            else
            {
                int length = values.Length;

                if (length > _MaxSize)
                {
                    throw new OverflowException();
                }

                //

                return new Vector()
                {
                    _Type = type,
                    _Size = length,
                    _VArray = values
                };
            }
        }

        //

        private Type _Type; // 此 Vector 的向量类型。

        private int _Size; // 此 Vector 的向量维度。

        private double[] _VArray; // 用于存储向量在各基向量方向的分量的数组。

        //

        private Matrix _ToMatrixForAffineTransform() // 获取此 Vector 用于仿射变换的矩阵。
        {
            if (_Size <= 0)
            {
                return Matrix.Empty;
            }
            else
            {
                if (_Type == Type.ColumnVector)
                {
                    double[,] values = new double[1, _Size + 1];

                    for (int i = 0; i < _Size; i++)
                    {
                        values[0, i] = _VArray[i];
                    }

                    values[0, _Size] = 1;

                    return Matrix.UnsafeCreateInstance(values);
                }
                else
                {
                    double[,] values = new double[_Size + 1, 1];

                    for (int i = 0; i < _Size; i++)
                    {
                        values[i, 0] = _VArray[i];
                    }

                    values[_Size, 0] = 1;

                    return Matrix.UnsafeCreateInstance(values);
                }
            }
        }

        #endregion

        #region 构造函数

        private Vector() // 不使用任何参数初始化 Vector 的新实例。
        {
        }

        /// <summary>
        /// 使用双精度浮点数表示的各基向量方向的分量初始化 Vector 的新实例。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="values">各基向量方向的分量。</param>
        public Vector(Type type, params double[] values)
        {
            _Type = type;

            if (!InternalMethod.IsNullOrEmpty(values))
            {
                int length = values.Length;

                if (length > _MaxSize)
                {
                    throw new OverflowException();
                }

                //

                _Size = length;
                _VArray = new double[_Size];

                Array.Copy(values, _VArray, _Size);
            }
            else
            {
                _Size = 0;
                _VArray = new double[0];
            }
        }

        /// <summary>
        /// 使用双精度浮点数表示的各基向量方向的分量初始化 Vector 的新实例。
        /// </summary>
        /// <param name="values">各基向量方向的分量。</param>
        public Vector(params double[] values)
        {
            _Type = Type.ColumnVector;

            if (!InternalMethod.IsNullOrEmpty(values))
            {
                int length = values.Length;

                if (length > _MaxSize)
                {
                    throw new OverflowException();
                }

                //

                _Size = length;
                _VArray = new double[_Size];

                Array.Copy(values, _VArray, _Size);
            }
            else
            {
                _Size = 0;
                _VArray = new double[0];
            }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置此 Vector 在指定的基向量方向的分量。
        /// </summary>
        /// <param name="index">索引。</param>
        public double this[int index]
        {
            get
            {
                if (_Size <= 0 || (index < 0 || index >= _Size))
                {
                    throw new IndexOutOfRangeException();
                }

                //

                return _VArray[index];
            }

            set
            {
                if (_Size <= 0 || (index < 0 || index >= _Size))
                {
                    throw new IndexOutOfRangeException();
                }

                //

                _VArray[index] = value;
            }
        }

        //

        /// <summary>
        /// 获取此 Vector 的维度。
        /// </summary>
        public int Dimension
        {
            get
            {
                if (_Size <= 0)
                {
                    return 0;
                }
                else
                {
                    return _Size;
                }
            }
        }

        //

        /// <summary>
        /// 获取表示此 Vector 是否为空向量的布尔值。
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (_Size <= 0);
            }
        }

        /// <summary>
        /// 获取表示此 Vector 是否为零向量的布尔值。
        /// </summary>
        public bool IsZero
        {
            get
            {
                if (_Size <= 0)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < _Size; i++)
                    {
                        if (_VArray[i] != 0)
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }
        }

        /// <summary>
        /// 获取表示此 Vector 是否为列向量的布尔值。
        /// </summary>
        public bool IsColumnVector
        {
            get
            {
                return (_Size > 0 && _Type == Type.ColumnVector);
            }
        }

        /// <summary>
        /// 获取表示此 Vector 是否为行向量的布尔值。
        /// </summary>
        public bool IsRowVector
        {
            get
            {
                return (_Size > 0 && _Type == Type.RowVector);
            }
        }

        /// <summary>
        /// 获取表示此 Vector 是否只读的布尔值。
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获取表示此 Vector 是否具有固定的维度的布尔值。
        /// </summary>
        public bool IsFixedSize
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 获取表示此 Vector 是否包含非数字分量的布尔值。
        /// </summary>
        public bool IsNaN
        {
            get
            {
                if (_Size <= 0)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < _Size; i++)
                    {
                        if (double.IsNaN(_VArray[i]))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
        }

        /// <summary>
        /// 获取表示此 Vector 是否包含无穷大分量的布尔值。
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                if (_Size <= 0)
                {
                    return false;
                }
                else
                {
                    bool result = false;

                    for (int i = 0; i < _Size; i++)
                    {
                        if (double.IsNaN(_VArray[i]))
                        {
                            return false;
                        }
                        else if (double.IsInfinity(_VArray[i]))
                        {
                            result = true;
                        }
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// 获取表示此 Vector 是否包含非数字或无穷大分量的布尔值。
        /// </summary>
        public bool IsNaNOrInfinity
        {
            get
            {
                if (_Size <= 0)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < _Size; i++)
                    {
                        if (InternalMethod.IsNaNOrInfinity(_VArray[i]))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
        }

        //

        /// <summary>
        /// 获取此 Vector 的模。
        /// </summary>
        public double Module
        {
            get
            {
                if (_Size <= 0)
                {
                    return double.NaN;
                }
                else
                {
                    double AbsMax = 0;

                    for (int i = 0; i < _Size; i++)
                    {
                        AbsMax = Math.Max(AbsMax, Math.Abs(_VArray[i]));
                    }

                    if (AbsMax == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        double SqrSum = 0;

                        for (int i = 0; i < _Size; i++)
                        {
                            double Factor = _VArray[i] / AbsMax;

                            SqrSum += Factor * Factor;
                        }

                        return (AbsMax * Math.Sqrt(SqrSum));
                    }
                }
            }
        }

        /// <summary>
        /// 获取此 Vector 的模的平方。
        /// </summary>
        public double ModuleSquared
        {
            get
            {
                if (_Size <= 0)
                {
                    return double.NaN;
                }
                else
                {
                    double SqrSum = 0;

                    for (int i = 0; i < _Size; i++)
                    {
                        SqrSum += _VArray[i] * _VArray[i];
                    }

                    return SqrSum;
                }
            }
        }

        //

        /// <summary>
        /// 获取此 Vector 的相反向量。
        /// </summary>
        public Vector Opposite
        {
            get
            {
                if (_Size <= 0)
                {
                    return Empty;
                }
                else
                {
                    Vector result = _GetZeroVector(_Type, _Size);

                    for (int i = 0; i < _Size; i++)
                    {
                        result._VArray[i] = -_VArray[i];
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// 获取此 Vector 的规范化向量。
        /// </summary>
        public Vector Normalize
        {
            get
            {
                if (_Size <= 0)
                {
                    return Empty;
                }
                else
                {
                    double Mod = Module;

                    if (Mod <= 0)
                    {
                        return Empty;
                    }
                    else
                    {
                        Vector result = _GetZeroVector(_Type, _Size);

                        for (int i = 0; i < _Size; i++)
                        {
                            result._VArray[i] = _VArray[i] / Mod;
                        }

                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// 获取此 Vector 的转置向量。
        /// </summary>
        public Vector Transport
        {
            get
            {
                if (_Size <= 0)
                {
                    return Empty;
                }
                else
                {
                    return new Vector((_Type == Type.ColumnVector ? Type.RowVector : Type.ColumnVector), _VArray);
                }
            }
        }

        #endregion

        #region 静态属性

        /// <summary>
        /// 获取表示空向量的 Vector 的新实例。
        /// </summary>
        public static Vector Empty
        {
            get
            {
                return new Vector()
                {
                    _Type = Type.ColumnVector,
                    _Size = 0,
                    _VArray = new double[0]
                };
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 Vector 是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>布尔值，表示此 Vector 是否与指定的对象相等。</returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj == null || !(obj is Vector))
            {
                return false;
            }
            else
            {
                return Equals((Vector)obj);
            }
        }

        /// <summary>
        /// 返回此 Vector 的哈希代码。
        /// </summary>
        /// <returns>32 位整数，表示此 Vector 的哈希代码。</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 Vector 转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 Vector 的字符串形式。</returns>
        public override string ToString()
        {
            string Str = string.Empty;

            if (_Size <= 0)
            {
                Str = "Empty";
            }
            else
            {
                Str = string.Concat("Type=", (_Type == Type.ColumnVector ? "ColumnVector" : "RowVector"), ", Dimension=", _Size);
            }

            return string.Concat(base.GetType().Name, " [", Str, "]");
        }

        //

        /// <summary>
        /// 判断此 Vector 是否与指定的 Vector 对象相等。
        /// </summary>
        /// <param name="vector">用于比较的 Vector 对象。</param>
        /// <returns>布尔值，表示此 Vector 是否与指定的 Vector 对象相等。</returns>
        public bool Equals(Vector vector)
        {
            if (object.ReferenceEquals(this, vector))
            {
                return true;
            }
            else if ((object)vector == null)
            {
                return false;
            }
            else if (_Type != vector._Type || _Size != vector._Size)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < _Size; i++)
                {
                    if (!_VArray[i].Equals(vector._VArray[i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        //

        /// <summary>
        /// 将此 Vector 与指定的对象进行次序比较。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>32 位整数，表示将此 Vector 与指定的对象进行次序比较得到的结果。</returns>
        public int CompareTo(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return 0;
            }
            else if (obj == null || !(obj is Vector))
            {
                return 1;
            }
            else
            {
                return CompareTo((Vector)obj);
            }
        }

        /// <summary>
        /// 将此 Vector 与指定的 Vector 对象进行次序比较。
        /// </summary>
        /// <param name="vector">用于比较的 Vector 对象。</param>
        /// <returns>32 位整数，表示将此 Vector 与指定的 Vector 对象进行次序比较得到的结果。</returns>
        public int CompareTo(Vector vector)
        {
            if (object.ReferenceEquals(this, vector))
            {
                return 0;
            }
            else if ((object)vector == null)
            {
                return 1;
            }
            else if (_Size != vector._Size)
            {
                if (_Size < vector._Size)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                for (int i = 0; i < _Size; i++)
                {
                    int result = _VArray[i].CompareTo(vector._VArray[i]);

                    if (result != 0)
                    {
                        return result;
                    }
                }

                return 0;
            }
        }

        //

        /// <summary>
        /// 获取此 Vector 的副本。
        /// </summary>
        /// <returns>Vector 对象，表示此 Vector 的副本。</returns>
        public Vector Copy()
        {
            if (_Size <= 0)
            {
                return Empty;
            }
            else
            {
                return new Vector(_Type, _VArray);
            }
        }

        //

        /// <summary>
        /// 遍历此 Vector 的所有分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int IndexOf(double item)
        {
            if (_Size <= 0)
            {
                return -1;
            }
            else
            {
                return Array.IndexOf(_VArray, item, 0, _Size);
            }
        }

        /// <summary>
        /// 从指定的索引开始遍历此 Vector 的所有分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int IndexOf(double item, int startIndex)
        {
            if (_Size <= 0 || (startIndex < 0 || startIndex >= _Size))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return Array.IndexOf(_VArray, item, startIndex, _Size - startIndex);
        }

        /// <summary>
        /// 从指定的索引开始遍历此 Vector 指定数量的分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <param name="count">遍历的分量数量。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int IndexOf(double item, int startIndex, int count)
        {
            if (_Size <= 0 || (startIndex < 0 || startIndex >= _Size) || count <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return Array.IndexOf(_VArray, item, startIndex, Math.Min(_Size - startIndex, count));
        }

        /// <summary>
        /// 逆序遍历此 Vector 的所有分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int LastIndexOf(double item)
        {
            if (_Size <= 0)
            {
                return -1;
            }
            else
            {
                return Array.LastIndexOf(_VArray, item, _Size - 1, _Size);
            }
        }

        /// <summary>
        /// 从指定的索引开始逆序遍历此 Vector 的所有分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int LastIndexOf(double item, int startIndex)
        {
            if (_Size <= 0 || (startIndex < 0 || startIndex >= _Size))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return Array.LastIndexOf(_VArray, item, startIndex, startIndex + 1);
        }

        /// <summary>
        /// 从指定的索引开始逆序遍历此 Vector 指定数量的分量并返回第一个与指定值相等的分量的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <param name="count">遍历的分量数量。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的分量的索引。</returns>
        public int LastIndexOf(double item, int startIndex, int count)
        {
            if (_Size <= 0 || (startIndex < 0 || startIndex >= _Size) || count <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return Array.LastIndexOf(_VArray, item, startIndex, Math.Min(startIndex + 1, count));
        }

        /// <summary>
        /// 遍历此 Vector 的所有分量并返回表示是否存在与指定值相等的分量的布尔值。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>布尔值，表示是否存在与指定值相等的分量。</returns>
        public bool Contains(double item)
        {
            if (_Size <= 0)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < _Size; i++)
                {
                    if (_VArray[i].Equals(item))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        //

        /// <summary>
        /// 将此 Vector 转换为双精度浮点数数组。
        /// </summary>
        /// <returns>双精度浮点数数组，数组元素表示此 Vector 的分量。</returns>
        public double[] ToArray()
        {
            if (_Size <= 0)
            {
                return new double[0];
            }
            else
            {
                double[] result = new double[_Size];

                Array.Copy(_VArray, result, _Size);

                return result;
            }
        }

        /// <summary>
        /// 将此 Vector 转换为双精度浮点数列表。
        /// </summary>
        /// <returns>双精度浮点数列表，列表元素表示此 Vector 的分量。</returns>
        public List<double> ToList()
        {
            if (_Size <= 0)
            {
                return new List<double>(0);
            }
            else
            {
                return new List<double>(_VArray);
            }
        }

        /// <summary>
        /// 返回将此 Vector 转换为矩阵的 Matrix 的新实例。
        /// </summary>
        /// <returns>Matrix 对象，表示将此 Vector 转换为矩阵的结果。</returns>
        public Matrix ToMatrix()
        {
            if (_Size <= 0)
            {
                return Matrix.Empty;
            }
            else
            {
                if (_Type == Type.ColumnVector)
                {
                    double[,] values = new double[1, _Size];

                    for (int i = 0; i < _Size; i++)
                    {
                        values[0, i] = _VArray[i];
                    }

                    return Matrix.UnsafeCreateInstance(values);
                }
                else
                {
                    double[,] values = new double[_Size, 1];

                    for (int i = 0; i < _Size; i++)
                    {
                        values[i, 0] = _VArray[i];
                    }

                    return Matrix.UnsafeCreateInstance(values);
                }
            }
        }

        //

        /// <summary>
        /// 返回将此 Vector 表示的直角坐标系坐标转换为极坐标系、球坐标系或超球坐标系坐标的新实例。
        /// </summary>
        /// <returns>Vector 对象，表示将此 Vector 表示的直角坐标系坐标转换为极坐标系、球坐标系或超球坐标系坐标得到的结果。</returns>
        public Vector ToSpherical()
        {
            if (_Size <= 0)
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(_Type, _Size);

                result._VArray[0] = Module;

                if (_Size > 1)
                {
                    Func<double, double, double> VectorAngle2PI = (x, y) =>
                    {
                        if (x == 0 && y == 0)
                        {
                            return 0;
                        }
                        else
                        {
                            double Angle = Math.Atan(y / x);

                            if (x < 0)
                            {
                                return (Angle + Constant.Pi);
                            }
                            else if (y < 0)
                            {
                                return (Angle + Constant.DoublePi);
                            }
                            else
                            {
                                return Angle;
                            }
                        }
                    };

                    result._VArray[_Size - 1] = VectorAngle2PI(_VArray[_Size - 2], _VArray[_Size - 1]);

                    if (_Size > 2)
                    {
                        Func<double, double, double> VectorAnglePI = (x, y) =>
                        {
                            if (x == 0 && y == 0)
                            {
                                return 0;
                            }
                            else
                            {
                                double Angle = Math.Atan(y / x);

                                if (x < 0)
                                {
                                    return (Angle + Constant.Pi);
                                }
                                else
                                {
                                    return Angle;
                                }
                            }
                        };

                        for (int i = 0; i < _Size - 2; i++)
                        {
                            double SqrY = 0;

                            for (int j = i + 1; j < _Size; j++)
                            {
                                SqrY += _VArray[j] * _VArray[j];
                            }

                            result._VArray[i + 1] = VectorAnglePI(_VArray[i], Math.Sqrt(SqrY));
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将此 Vector 表示的极坐标系、球坐标系或超球坐标系坐标转换为直角坐标系坐标的新实例。
        /// </summary>
        /// <returns>Vector 对象，表示将此 Vector 表示的极坐标系、球坐标系或超球坐标系坐标转换为直角坐标系坐标得到的结果。</returns>
        public Vector ToCartesian()
        {
            if (_Size <= 0)
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(_Type, _Size);

                if (_Size == 1)
                {
                    result._VArray[0] = _VArray[0];
                }
                else
                {
                    result._VArray[0] = _VArray[0] * Math.Cos(_VArray[1]);

                    if (_Size == 2)
                    {
                        result._VArray[1] = _VArray[0] * Math.Sin(_VArray[1]);
                    }
                    else
                    {
                        result._VArray[_Size - 1] = _VArray[0];

                        for (int i = 1; i < _Size; i++)
                        {
                            result._VArray[_Size - 1] *= Math.Sin(_VArray[i]);
                        }

                        for (int i = 1; i < _Size - 1; i++)
                        {
                            result._VArray[i] = _VArray[0] * Math.Cos(_VArray[i + 1]);

                            for (int j = 1; j < i + 1; j++)
                            {
                                result._VArray[i] *= Math.Sin(_VArray[j]);
                            }
                        }
                    }
                }

                return result;
            }
        }

        //

        /// <summary>
        /// 返回此 Vector 与指定的 Vector 对象之间的距离。
        /// </summary>
        /// <param name="vector">Vector 对象，表示另一个向量。</param>
        /// <returns>双精度浮点数，表示此 Vector 与指定的 Vector 对象之间的距离。</returns>
        public double DistanceFrom(Vector vector)
        {
            bool ThisIsNOrE = (_Size <= 0);
            bool OtherIsNOrE = IsNullOrEmpty(vector);

            if (ThisIsNOrE && OtherIsNOrE)
            {
                return double.NaN;
            }
            else if (ThisIsNOrE || OtherIsNOrE)
            {
                throw new ArithmeticException();
            }
            else if (_Size != vector._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                double[] Delta = new double[_Size];
                double AbsMax = 0;

                for (int i = 0; i < Delta.Length; i++)
                {
                    Delta[i] = _VArray[i] - vector._VArray[i];
                    AbsMax = Math.Max(AbsMax, Math.Abs(Delta[i]));
                }

                if (AbsMax == 0)
                {
                    return 0;
                }
                else
                {
                    double SqrSum = 0;

                    for (int i = 0; i < Delta.Length; i++)
                    {
                        double Factor = Delta[i] / AbsMax;

                        SqrSum += Factor * Factor;
                    }

                    return (AbsMax * Math.Sqrt(SqrSum));
                }
            }
        }

        /// <summary>
        /// 返回此 Vector 与指定的 Vector 对象之间的夹角（弧度）。
        /// </summary>
        /// <param name="vector">Vector 对象，表示另一个向量。</param>
        /// <returns>双精度浮点数，表示此 Vector 与指定的 Vector 对象之间的夹角（弧度）。</returns>
        public double AngleFrom(Vector vector)
        {
            bool ThisIsNOrE = (_Size <= 0);
            bool OtherIsNOrE = IsNullOrEmpty(vector);

            if (ThisIsNOrE && OtherIsNOrE)
            {
                return double.NaN;
            }
            else if (ThisIsNOrE || OtherIsNOrE)
            {
                throw new ArithmeticException();
            }
            else if (_Size != vector._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                if (IsZero || vector.IsZero)
                {
                    return 0;
                }
                else
                {
                    double ModProduct = Module * vector.Module;
                    double CosA = 0;

                    for (int i = 0; i < _Size; i++)
                    {
                        CosA += _VArray[i] * vector._VArray[i] / ModProduct;
                    }

                    return Math.Acos(CosA);
                }
            }
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的位移将此 Vector 的所有分量平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public void Offset(double d)
        {
            if (_Size > 0)
            {
                for (int i = 0; i < _Size; i++)
                {
                    _VArray[i] += d;
                }
            }
        }

        /// <summary>
        /// 按 Vector 对象表示的位移将此 Vector 平移指定的量。
        /// </summary>
        /// <param name="vector">Vector 对象表示的位移。</param>
        public void Offset(Vector vector)
        {
            bool ThisIsNOrE = (_Size <= 0);
            bool OtherIsNOrE = IsNullOrEmpty(vector);

            if (ThisIsNOrE && OtherIsNOrE)
            {
                return;
            }
            else if (ThisIsNOrE || OtherIsNOrE)
            {
                throw new ArithmeticException();
            }
            else if (_Size != vector._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                for (int i = 0; i < _Size; i++)
                {
                    _VArray[i] += vector._VArray[i];
                }
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的位移将此 Vector 的所有分量平移指定的量的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>Vector 对象，表示按双精度浮点数表示的位移将此 Vector 的所有分量平移指定的量得到的结果。</returns>
        public Vector OffsetCopy(double d)
        {
            if (_Size <= 0)
            {
                return Empty;
            }
            else
            {
                Vector result = Copy();

                for (int i = 0; i < result._Size; i++)
                {
                    result._VArray[i] += d;
                }

                return result;
            }
        }

        /// <summary>
        /// 返回按 Vector 对象表示的位移将此 Vector 平移指定的量的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象表示的位移。</param>
        /// <returns>Vector 对象，表示按 Vector 对象表示的位移将此 Vector 平移指定的量得到的结果。</returns>
        public Vector OffsetCopy(Vector vector)
        {
            bool ThisIsNOrE = (_Size <= 0);
            bool OtherIsNOrE = IsNullOrEmpty(vector);

            if (ThisIsNOrE && OtherIsNOrE)
            {
                return Empty;
            }
            else if (ThisIsNOrE || OtherIsNOrE)
            {
                throw new ArithmeticException();
            }
            else if (_Size != vector._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                Vector result = Copy();

                for (int i = 0; i < result._Size; i++)
                {
                    result._VArray[i] += vector._VArray[i];
                }

                return result;
            }
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的缩放因数将此 Vector 的所有分量缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public void Scale(double s)
        {
            if (_Size > 0)
            {
                for (int i = 0; i < _Size; i++)
                {
                    _VArray[i] *= s;
                }
            }
        }

        /// <summary>
        /// 按 Vector 对象表示的缩放因数将此 Vector 缩放指定的倍数。
        /// </summary>
        /// <param name="vector">Vector 对象表示的缩放因数。</param>
        public void Scale(Vector vector)
        {
            bool ThisIsNOrE = (_Size <= 0);
            bool OtherIsNOrE = IsNullOrEmpty(vector);

            if (ThisIsNOrE && OtherIsNOrE)
            {
                return;
            }
            else if (ThisIsNOrE || OtherIsNOrE)
            {
                throw new ArithmeticException();
            }
            else if (_Size != vector._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                for (int i = 0; i < _Size; i++)
                {
                    _VArray[i] *= vector._VArray[i];
                }
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数将此 Vector 的所有分量缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>Vector 对象，表示按双精度浮点数表示的缩放因数将此 Vector 的所有分量缩放指定的倍数得到的结果。</returns>
        public Vector ScaleCopy(double s)
        {
            if (_Size <= 0)
            {
                return Empty;
            }
            else
            {
                Vector result = Copy();

                for (int i = 0; i < result._Size; i++)
                {
                    result._VArray[i] *= s;
                }

                return result;
            }
        }

        /// <summary>
        /// 返回按 Vector 对象表示的缩放因数将此 Vector 缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象表示的缩放因数。</param>
        /// <returns>Vector 对象，表示按 Vector 对象表示的缩放因数将此 Vector 缩放指定的倍数得到的结果。</returns>
        public Vector ScaleCopy(Vector vector)
        {
            bool ThisIsNOrE = (_Size <= 0);
            bool OtherIsNOrE = IsNullOrEmpty(vector);

            if (ThisIsNOrE && OtherIsNOrE)
            {
                return Empty;
            }
            else if (ThisIsNOrE || OtherIsNOrE)
            {
                throw new ArithmeticException();
            }
            else if (_Size != vector._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                Vector result = Copy();

                for (int i = 0; i < result._Size; i++)
                {
                    result._VArray[i] *= vector._VArray[i];
                }

                return result;
            }
        }

        //

        /// <summary>
        /// 将此 Vector 在指定的基向量方向的分量翻转。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        public void Reflect(int index)
        {
            if (_Size <= 0 || (index < 0 || index >= _Size))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            _VArray[index] = -_VArray[index];
        }

        /// <summary>
        /// 返回将此 Vector 在指定的基向量方向的分量翻转的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        /// <returns>Vector 对象，表示将此 Vector 在指定的基向量方向的分量翻转得到的结果。</returns>
        public Vector ReflectCopy(int index)
        {
            if (_Size <= 0 || (index < 0 || index >= _Size))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            Vector result = Copy();

            result._VArray[index] = -result._VArray[index];

            return result;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 Vector 剪切指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 Vector 沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向剪切的角度（弧度）。</param>
        public void Shear(int index1, int index2, double angle)
        {
            if (_Size < 2 || (index1 < 0 || index1 >= _Size) || (index2 < 0 || index2 >= _Size))
            {
                throw new ArgumentOutOfRangeException();
            }
            else if (index1 == index2)
            {
                throw new ArgumentException();
            }

            //

            _VArray[index1] += _VArray[index2] * Math.Tan(angle);
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 Vector 剪切指定的角度的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 Vector 沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向剪切的角度（弧度）。</param>
        /// <returns>Vector 对象，表示按双精度浮点数表示的弧度将此 Vector 剪切指定的角度得到的结果。</returns>
        public Vector ShearCopy(int index1, int index2, double angle)
        {
            if (_Size < 2 || (index1 < 0 || index1 >= _Size) || (index2 < 0 || index2 >= _Size))
            {
                throw new ArgumentOutOfRangeException();
            }
            else if (index1 == index2)
            {
                throw new ArgumentException();
            }

            //

            Vector result = Copy();

            result._VArray[index1] += result._VArray[index2] * Math.Tan(angle);

            return result;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 Vector 旋转指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 Vector 绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        public void Rotate(int index1, int index2, double angle)
        {
            if (_Size < 2 || index1 < 0 || index1 >= _Size || index2 < 0 || index2 >= _Size)
            {
                throw new ArgumentOutOfRangeException();
            }
            else if (index1 == index2)
            {
                throw new ArgumentException();
            }

            //

            double Value1 = _VArray[index1];
            double Value2 = _VArray[index2];

            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            _VArray[index1] = Value1 * CosA - Value2 * SinA;
            _VArray[index2] = Value2 * CosA + Value1 * SinA;
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 Vector 旋转指定的角度的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 Vector 绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        /// <returns>Vector 对象，表示按双精度浮点数表示的弧度将此 Vector 旋转指定的角度得到的结果。</returns>
        public Vector RotateCopy(int index1, int index2, double angle)
        {
            if (_Size < 2 || index1 < 0 || index1 >= _Size || index2 < 0 || index2 >= _Size)
            {
                throw new ArgumentOutOfRangeException();
            }
            else if (index1 == index2)
            {
                throw new ArgumentException();
            }

            //

            Vector result = Copy();

            double Value1 = _VArray[index1];
            double Value2 = _VArray[index2];

            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            result._VArray[index1] = Value1 * CosA - Value2 * SinA;
            result._VArray[index2] = Value2 * CosA + Value1 * SinA;

            return result;
        }

        //

        /// <summary>
        /// 按 Matrix 对象表示的仿射矩阵将此 Vector 进行仿射变换。
        /// </summary>
        /// <param name="matrix">Matrix 对象表示的仿射矩阵，对于列向量应为左矩阵，对于行向量应为右矩阵。</param>
        public void AffineTransform(Matrix matrix)
        {
            bool VIsNOrE = (_Size <= 0);
            bool MIsNOrE = Matrix.IsNullOrEmpty(matrix);

            if (VIsNOrE && MIsNOrE)
            {
                return;
            }
            else if (VIsNOrE || MIsNOrE)
            {
                throw new ArithmeticException();
            }
            else if (matrix.Size != new Size(_Size + 1, _Size + 1))
            {
                throw new ArithmeticException();
            }
            else
            {
                Matrix matrixVector = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    Matrix result = Matrix.Multiply(matrix, matrixVector);

                    if (Matrix.IsNullOrEmpty(result) || result.Size != new Size(1, _Size + 1))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        Array.Copy(result.GetColumn(0)._VArray, _VArray, _Size);
                    }
                }
                else
                {
                    Matrix result = Matrix.Multiply(matrixVector, matrix);

                    if (Matrix.IsNullOrEmpty(result) || result.Size != new Size(_Size + 1, 1))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        Array.Copy(result.GetRow(0)._VArray, _VArray, _Size);
                    }
                }
            }
        }

        /// <summary>
        /// 按 Matrix 对象列表表示的仿射矩阵列表将此 Vector 进行仿射变换。
        /// </summary>
        /// <param name="matrixList">Matrix 对象列表，对于列向量应全部为左矩阵，对于行向量应全部为右矩阵。</param>
        public void AffineTransform(List<Matrix> matrixList)
        {
            if (_Size > 0 && !InternalMethod.IsNullOrEmpty(matrixList))
            {
                Matrix result = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    for (int i = 0; i < matrixList.Count; i++)
                    {
                        result = Matrix.Multiply(matrixList[i], result);
                    }

                    if (Matrix.IsNullOrEmpty(result) || result.Size != new Size(1, _Size + 1))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        Array.Copy(result.GetColumn(0)._VArray, _VArray, _Size);
                    }
                }
                else
                {
                    for (int i = 0; i < matrixList.Count; i++)
                    {
                        result = Matrix.Multiply(result, matrixList[i]);
                    }

                    if (Matrix.IsNullOrEmpty(result) || result.Size != new Size(_Size + 1, 1))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        Array.Copy(result.GetRow(0)._VArray, _VArray, _Size);
                    }
                }
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的仿射矩阵将此 Vector 进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrix">Matrix 对象表示的仿射矩阵，对于列向量应为左矩阵，对于行向量应为右矩阵。</param>
        /// <returns>Vector 对象，表示按 Matrix 对象表示的仿射矩阵将此 Vector 进行仿射变换得到的结果。</returns>
        public Vector AffineTransformCopy(Matrix matrix)
        {
            bool VIsNOrE = (_Size <= 0);
            bool MIsNOrE = Matrix.IsNullOrEmpty(matrix);

            if (VIsNOrE && MIsNOrE)
            {
                return Empty;
            }
            else if (VIsNOrE || MIsNOrE)
            {
                throw new ArithmeticException();
            }
            else if (matrix.Size != new Size(_Size + 1, _Size + 1))
            {
                throw new ArithmeticException();
            }
            else
            {
                Matrix matrixVector = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    Matrix result = Matrix.Multiply(matrix, matrixVector);

                    if (Matrix.IsNullOrEmpty(result) || result.Size != new Size(1, _Size + 1))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        Vector vector = _GetZeroVector(_Type, _Size);

                        Array.Copy(result.GetColumn(0)._VArray, vector._VArray, _Size);

                        return vector;
                    }
                }
                else
                {
                    Matrix result = Matrix.Multiply(matrixVector, matrix);

                    if (Matrix.IsNullOrEmpty(result) || result.Size != new Size(_Size + 1, 1))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        Vector vector = _GetZeroVector(_Type, _Size);

                        Array.Copy(result.GetRow(0)._VArray, vector._VArray, _Size);

                        return vector;
                    }
                }
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的仿射矩阵列表将此 Vector 进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrixList">Matrix 对象列表，对于列向量应全部为左矩阵，对于行向量应全部为右矩阵。</param>
        /// <returns>Vector 对象，表示按 Matrix 对象列表表示的仿射矩阵列表将此 Vector 进行仿射变换得到的结果。</returns>
        public Vector AffineTransformCopy(List<Matrix> matrixList)
        {
            if (_Size <= 0)
            {
                return Empty;
            }
            else if (InternalMethod.IsNullOrEmpty(matrixList))
            {
                return Copy();
            }
            else
            {
                Matrix result = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    for (int i = 0; i < matrixList.Count; i++)
                    {
                        result = Matrix.Multiply(matrixList[i], result);
                    }

                    if (Matrix.IsNullOrEmpty(result) || result.Size != new Size(1, _Size + 1))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        Vector vector = _GetZeroVector(_Type, _Size);

                        Array.Copy(result.GetColumn(0)._VArray, vector._VArray, _Size);

                        return vector;
                    }
                }
                else
                {
                    for (int i = 0; i < matrixList.Count; i++)
                    {
                        result = Matrix.Multiply(result, matrixList[i]);
                    }

                    if (Matrix.IsNullOrEmpty(result) || result.Size != new Size(_Size + 1, 1))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        Vector vector = _GetZeroVector(_Type, _Size);

                        Array.Copy(result.GetRow(0)._VArray, vector._VArray, _Size);

                        return vector;
                    }
                }
            }
        }

        /// <summary>
        /// 按 Matrix 对象表示的仿射矩阵将此 Vector 进行逆仿射变换。
        /// </summary>
        /// <param name="matrix">Matrix 对象表示的仿射矩阵，对于列向量应为左矩阵，对于行向量应为右矩阵。</param>
        public void InverseAffineTransform(Matrix matrix)
        {
            bool VIsNOrE = (_Size <= 0);
            bool MIsNOrE = Matrix.IsNullOrEmpty(matrix);

            if (VIsNOrE && MIsNOrE)
            {
                return;
            }
            else if (VIsNOrE || MIsNOrE)
            {
                throw new ArithmeticException();
            }
            else if (matrix.Size != new Size(_Size + 1, _Size + 1))
            {
                throw new ArithmeticException();
            }
            else
            {
                Matrix matrixVector = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    Matrix result = Matrix.DivideLeft(matrix, matrixVector);

                    if (Matrix.IsNullOrEmpty(result) || result.Size != new Size(1, _Size + 1))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        Array.Copy(result.GetColumn(0)._VArray, _VArray, _Size);
                    }
                }
                else
                {
                    Matrix result = Matrix.DivideRight(matrixVector, matrix);

                    if (Matrix.IsNullOrEmpty(result) || result.Size != new Size(_Size + 1, 1))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        Array.Copy(result.GetRow(0)._VArray, _VArray, _Size);
                    }
                }
            }
        }

        /// <summary>
        /// 按 Matrix 对象列表表示的仿射矩阵列表将此 Vector 进行逆仿射变换。
        /// </summary>
        /// <param name="matrixList">Matrix 对象列表，对于列向量应全部为左矩阵，对于行向量应全部为右矩阵。</param>
        public void InverseAffineTransform(List<Matrix> matrixList)
        {
            if (_Size > 0 && !InternalMethod.IsNullOrEmpty(matrixList))
            {
                Matrix result = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    for (int i = matrixList.Count - 1; i >= 0; i--)
                    {
                        result = Matrix.DivideLeft(matrixList[i], result);
                    }

                    if (Matrix.IsNullOrEmpty(result) || result.Size != new Size(1, _Size + 1))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        Array.Copy(result.GetColumn(0)._VArray, _VArray, _Size);
                    }
                }
                else
                {
                    for (int i = matrixList.Count - 1; i >= 0; i--)
                    {
                        result = Matrix.DivideRight(result, matrixList[i]);
                    }

                    if (Matrix.IsNullOrEmpty(result) || result.Size != new Size(_Size + 1, 1))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        Array.Copy(result.GetRow(0)._VArray, _VArray, _Size);
                    }
                }
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的仿射矩阵将此 Vector 进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrix">Matrix 对象表示的仿射矩阵，对于列向量应为左矩阵，对于行向量应为右矩阵。</param>
        /// <returns>Vector 对象，表示按 Matrix 对象表示的仿射矩阵将此 Vector 进行逆仿射变换得到的结果。</returns>
        public Vector InverseAffineTransformCopy(Matrix matrix)
        {
            bool VIsNOrE = (_Size <= 0);
            bool MIsNOrE = Matrix.IsNullOrEmpty(matrix);

            if (VIsNOrE && MIsNOrE)
            {
                return Empty;
            }
            else if (VIsNOrE || MIsNOrE)
            {
                throw new ArithmeticException();
            }
            else if (matrix.Size != new Size(_Size + 1, _Size + 1))
            {
                throw new ArithmeticException();
            }
            else
            {
                Matrix matrixVector = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    Matrix result = Matrix.DivideLeft(matrix, matrixVector);

                    if (Matrix.IsNullOrEmpty(result) || result.Size != new Size(1, _Size + 1))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        Vector vector = _GetZeroVector(_Type, _Size);

                        Array.Copy(result.GetColumn(0)._VArray, vector._VArray, _Size);

                        return vector;
                    }
                }
                else
                {
                    Matrix result = Matrix.DivideRight(matrixVector, matrix);

                    if (Matrix.IsNullOrEmpty(result) || result.Size != new Size(_Size + 1, 1))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        Vector vector = _GetZeroVector(_Type, _Size);

                        Array.Copy(result.GetRow(0)._VArray, vector._VArray, _Size);

                        return vector;
                    }
                }
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的仿射矩阵列表将此 Vector 进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrixList">Matrix 对象列表，对于列向量应全部为左矩阵，对于行向量应全部为右矩阵。</param>
        /// <returns>Vector 对象，表示按 Matrix 对象列表表示的仿射矩阵列表将此 Vector 进行逆仿射变换得到的结果。</returns>
        public Vector InverseAffineTransformCopy(List<Matrix> matrixList)
        {
            if (_Size <= 0)
            {
                return Empty;
            }
            else if (InternalMethod.IsNullOrEmpty(matrixList))
            {
                return Copy();
            }
            else
            {
                Matrix result = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    for (int i = matrixList.Count - 1; i >= 0; i--)
                    {
                        result = Matrix.DivideLeft(matrixList[i], result);
                    }

                    if (Matrix.IsNullOrEmpty(result) || result.Size != new Size(1, _Size + 1))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        Vector vector = _GetZeroVector(_Type, _Size);

                        Array.Copy(result.GetColumn(0)._VArray, vector._VArray, _Size);

                        return vector;
                    }
                }
                else
                {
                    for (int i = matrixList.Count - 1; i >= 0; i--)
                    {
                        result = Matrix.DivideRight(result, matrixList[i]);
                    }

                    if (Matrix.IsNullOrEmpty(result) || result.Size != new Size(_Size + 1, 1))
                    {
                        throw new ArithmeticException();
                    }
                    else
                    {
                        Vector vector = _GetZeroVector(_Type, _Size);

                        Array.Copy(result.GetRow(0)._VArray, vector._VArray, _Size);

                        return vector;
                    }
                }
            }
        }

        //

        /// <summary>
        /// 返回此 Vector 与指定索引的基向量之间的夹角（弧度）。
        /// </summary>
        /// <param name="index">索引。</param>
        /// <returns>双精度浮点数，表示此 Vector 与指定索引的基向量之间的夹角（弧度）。</returns>
        public double AngleFromBase(int index)
        {
            if (_Size <= 0 || (index < 0 || index >= _Size))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (IsZero)
            {
                return 0;
            }
            else
            {
                return AngleFrom(_VArray[index] >= 0 ? Base(_Size, index) : Base(_Size, index).Opposite);
            }
        }

        /// <summary>
        /// 返回此 Vector 与正交于指定索引的基向量的子空间之间的夹角（弧度）。
        /// </summary>
        /// <param name="index">索引。</param>
        /// <returns>双精度浮点数，表示此 Vector 与正交于指定索引的基向量的子空间之间的夹角（弧度）。</returns>
        public double AngleFromSpace(int index)
        {
            if (_Size <= 0 || (index < 0 || index >= _Size))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (IsZero)
            {
                return 0;
            }
            else
            {
                return (Constant.HalfPi - AngleFromBase(index));
            }
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断指定的 Vector 是否为 null 或表示空向量。
        /// </summary>
        /// <param name="vector">用于判断的 Vector 对象。</param>
        /// <returns>布尔值，表示指定的 Vector 是否为 null 或表示空向量。</returns>
        public static bool IsNullOrEmpty(Vector vector)
        {
            return ((object)vector == null || vector._Size <= 0);
        }

        //

        /// <summary>
        /// 判断两个 Vector 对象是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 Vector 对象。</param>
        /// <param name="right">用于比较的第二个 Vector 对象。</param>
        /// <returns>布尔值，表示两个 Vector 对象是否相等。</returns>
        public static bool Equals(Vector left, Vector right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if ((object)left == null || (object)right == null)
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
        /// 比较两个 Vector 对象的次序。
        /// </summary>
        /// <param name="left">用于比较的第一个 Vector 对象。</param>
        /// <param name="right">用于比较的第二个 Vector 对象。</param>
        /// <returns>32 位整数，表示将两个 Vector 对象进行次序比较得到的结果。</returns>
        public static int Compare(Vector left, Vector right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return 0;
            }
            else if ((object)left == null)
            {
                return -1;
            }
            else if ((object)right == null)
            {
                return 1;
            }
            else
            {
                return left.CompareTo(right);
            }
        }

        //

        /// <summary>
        /// 返回表示零向量的 Vector 的新实例。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="dimension">向量维度。</param>
        /// <returns>Vector 对象，表示零向量。</returns>
        public static Vector Zero(Type type, int dimension)
        {
            if (dimension < 0)
            {
                throw new OverflowException();
            }

            //

            if (dimension == 0)
            {
                return Empty;
            }
            else
            {
                return _GetZeroVector(type, dimension);
            }
        }

        /// <summary>
        /// 返回表示零向量的 Vector 的新实例。
        /// </summary>
        /// <param name="dimension">向量维度。</param>
        /// <returns>Vector 对象，表示零向量。</returns>
        public static Vector Zero(int dimension)
        {
            if (dimension < 0)
            {
                throw new OverflowException();
            }

            //

            if (dimension == 0)
            {
                return Empty;
            }
            else
            {
                return _GetZeroVector(Type.ColumnVector, dimension);
            }
        }

        /// <summary>
        /// 返回表示指定索引的基向量的 Vector 的新实例。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="dimension">向量维度。</param>
        /// <param name="index">索引。</param>
        /// <returns>Vector 对象，表示指定索引的基向量。</returns>
        public static Vector Base(Type type, int dimension, int index)
        {
            if (dimension < 0 || (index < 0 || index >= dimension))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (dimension == 0)
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(type, dimension);

                result._VArray[index] = 1;

                return result;
            }
        }

        /// <summary>
        /// 返回表示指定索引的基向量的 Vector 的新实例。
        /// </summary>
        /// <param name="dimension">向量维度。</param>
        /// <param name="index">索引。</param>
        /// <returns>Vector 对象，表示指定索引的基向量。</returns>
        public static Vector Base(int dimension, int index)
        {
            if (dimension <= 0 || (index < 0 || index >= dimension))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (dimension == 0)
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(Type.ColumnVector, dimension);

                result._VArray[index] = 1;

                return result;
            }
        }

        //

        /// <summary>
        /// 返回表示用于平移 Vector 对象的仿射矩阵的 Matrix 的新实例，对于列向量将返回左矩阵，对于行向量将返回右矩阵。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="dimension">向量维度。</param>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>Matrix 对象，表示用于平移 Vector 对象的仿射矩阵。</returns>
        public static Matrix OffsetMatrix(Type type, int dimension, double d)
        {
            if (dimension < 0)
            {
                throw new OverflowException();
            }

            //

            if (dimension == 0)
            {
                return Matrix.Empty;
            }
            else
            {
                Matrix result = Matrix.Identity(dimension + 1);

                if (type == Type.ColumnVector)
                {
                    for (int i = 0; i < dimension; i++)
                    {
                        result[dimension, i] = d;
                    }
                }
                else
                {
                    for (int i = 0; i < dimension; i++)
                    {
                        result[i, dimension] = d;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// 返回表示用于平移 Vector 对象的仿射矩阵的 Matrix 的新实例，对于列向量将返回左矩阵，对于行向量将返回右矩阵。
        /// </summary>
        /// <param name="vector">Vector 对象表示的位移。</param>
        /// <returns>Matrix 对象，表示用于平移 Vector 对象的仿射矩阵。</returns>
        public static Matrix OffsetMatrix(Vector vector)
        {
            if (IsNullOrEmpty(vector))
            {
                return Matrix.Empty;
            }
            else
            {
                int dimension = vector._Size;

                Matrix result = Matrix.Identity(dimension + 1);

                if (vector._Type == Type.ColumnVector)
                {
                    for (int i = 0; i < dimension; i++)
                    {
                        result[dimension, i] = vector._VArray[i];
                    }
                }
                else
                {
                    for (int i = 0; i < dimension; i++)
                    {
                        result[i, dimension] = vector._VArray[i];
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// 返回表示用于缩放 Vector 对象的仿射矩阵的 Matrix 的新实例，对于列向量将返回左矩阵，对于行向量将返回右矩阵。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="dimension">向量维度。</param>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示用于缩放 Vector 对象的仿射矩阵。</returns>
        public static Matrix ScaleMatrix(Type type, int dimension, double s)
        {
            if (dimension < 0)
            {
                throw new OverflowException();
            }

            //

            if (dimension == 0)
            {
                return Matrix.Empty;
            }
            else
            {
                Matrix result = Matrix.Identity(dimension + 1);

                for (int i = 0; i < dimension; i++)
                {
                    result[i, i] = s;
                }

                return result;
            }
        }

        /// <summary>
        /// 返回表示用于缩放 Vector 对象的仿射矩阵的 Matrix 的新实例，对于列向量将返回左矩阵，对于行向量将返回右矩阵。
        /// </summary>
        /// <param name="vector">Vector 对象表示的缩放因数。</param>
        /// <returns>Matrix 对象，表示用于缩放 Vector 对象的仿射矩阵。</returns>
        public static Matrix ScaleMatrix(Vector vector)
        {
            if (IsNullOrEmpty(vector))
            {
                return Matrix.Empty;
            }
            else
            {
                int dimension = vector._Size;

                Matrix result = Matrix.Identity(dimension + 1);

                for (int i = 0; i < dimension; i++)
                {
                    result[i, i] = vector._VArray[i];
                }

                return result;
            }
        }

        /// <summary>
        /// 返回表示用于翻转 Vector 对象的仿射矩阵的 Matrix 的新实例，对于列向量将返回左矩阵，对于行向量将返回右矩阵。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="dimension">向量维度。</param>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        /// <returns>Matrix 对象，表示用于翻转 Vector 对象的仿射矩阵。</returns>
        public static Matrix ReflectMatrix(Type type, int dimension, int index)
        {
            if (dimension <= 0 || (index < 0 || index >= dimension))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            Matrix result = Matrix.Identity(dimension + 1);

            result[index, index] = -1;

            return result;
        }

        /// <summary>
        /// 返回表示用于剪切 Vector 对象的仿射矩阵的 Matrix 的新实例，对于列向量将返回左矩阵，对于行向量将返回右矩阵。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="dimension">向量维度。</param>
        /// <param name="index1">索引，用于指定与剪切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示 Vector 对象沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向剪切的角度（弧度）。</param>
        /// <returns>Matrix 对象，表示用于剪切 Vector 对象的仿射矩阵。</returns>
        public static Matrix ShearMatrix(Type type, int dimension, int index1, int index2, double angle)
        {
            if (dimension < 2 || (index1 < 0 || index1 >= dimension) || (index2 < 0 || index2 >= dimension))
            {
                throw new ArgumentOutOfRangeException();
            }
            else if (index1 == index2)
            {
                throw new ArgumentException();
            }

            //

            Matrix result = Matrix.Identity(dimension + 1);

            double TanA = Math.Tan(angle);

            if (type == Type.ColumnVector)
            {
                result[index2, index1] = TanA;
            }
            else
            {
                result[index1, index2] = TanA;
            }

            return result;
        }

        /// <summary>
        /// 返回表示用于旋转 Vector 对象的仿射矩阵的 Matrix 的新实例，对于列向量将返回左矩阵，对于行向量将返回右矩阵。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="dimension">向量维度。</param>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示 Vector 对象绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        /// <returns>Matrix 对象，表示用于旋转 Vector 对象的仿射矩阵。</returns>
        public static Matrix RotateMatrix(Type type, int dimension, int index1, int index2, double angle)
        {
            if (dimension < 2 || (index1 < 0 || index1 >= dimension) || (index2 < 0 || index2 >= dimension))
            {
                throw new ArgumentOutOfRangeException();
            }
            else if (index1 == index2)
            {
                throw new ArgumentException();
            }

            //

            Matrix result = Matrix.Identity(dimension + 1);

            double CosA = Math.Cos(angle);
            double SinA = Math.Sin(angle);

            result[index1, index1] = CosA;
            result[index2, index2] = CosA;

            if (type == Type.ColumnVector)
            {
                result[index1, index2] = SinA;
                result[index2, index1] = -SinA;
            }
            else
            {
                result[index2, index1] = SinA;
                result[index1, index2] = -SinA;
            }

            return result;
        }

        //

        /// <summary>
        /// 返回两个 Vector 对象之间的距离。
        /// </summary>
        /// <param name="left">第一个 Vector 对象。</param>
        /// <param name="right">第二个 Vector 对象。</param>
        /// <returns>双精度浮点数，表示两个 Vector 对象之间的距离。</returns>
        public static double DistanceBetween(Vector left, Vector right)
        {
            bool LIsNOrE = IsNullOrEmpty(left);
            bool RIsNOrE = IsNullOrEmpty(right);

            if (LIsNOrE && RIsNOrE)
            {
                return double.NaN;
            }
            else if (LIsNOrE || RIsNOrE)
            {
                throw new ArithmeticException();
            }
            else if (left._Size != right._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                double[] Delta = new double[left._Size];
                double AbsMax = 0;

                for (int i = 0; i < Delta.Length; i++)
                {
                    Delta[i] = left._VArray[i] - right._VArray[i];
                    AbsMax = Math.Max(AbsMax, Math.Abs(Delta[i]));
                }

                if (AbsMax == 0)
                {
                    return 0;
                }
                else
                {
                    double SqrSum = 0;

                    for (int i = 0; i < Delta.Length; i++)
                    {
                        double Factor = Delta[i] / AbsMax;

                        SqrSum += Factor * Factor;
                    }

                    return (AbsMax * Math.Sqrt(SqrSum));
                }
            }
        }

        /// <summary>
        /// 返回两个 Vector 对象之间的夹角（弧度）。
        /// </summary>
        /// <param name="left">第一个 Vector 对象。</param>
        /// <param name="right">第二个 Vector 对象。</param>
        /// <returns>双精度浮点数，表示两个 Vector 对象之间的夹角（弧度）。</returns>
        public static double AngleBetween(Vector left, Vector right)
        {
            bool LIsNOrE = IsNullOrEmpty(left);
            bool RIsNOrE = IsNullOrEmpty(right);

            if (LIsNOrE && RIsNOrE)
            {
                return double.NaN;
            }
            else if (LIsNOrE || RIsNOrE)
            {
                throw new ArithmeticException();
            }
            else if (left._Size != right._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                if (left.IsZero || right.IsZero)
                {
                    return 0;
                }
                else
                {
                    double ModProduct = left.Module * right.Module;
                    double CosA = 0;

                    for (int i = 0; i < left._Size; i++)
                    {
                        CosA += left._VArray[i] * right._VArray[i] / ModProduct;
                    }

                    return Math.Acos(CosA);
                }
            }
        }

        //

        /// <summary>
        /// 返回两个 Vector 对象的数量积。
        /// </summary>
        /// <param name="left">第一个 Vector 对象。</param>
        /// <param name="right">第二个 Vector 对象。</param>
        /// <returns>双精度浮点数，表示两个 Vector 对象的数量积。</returns>
        public static double DotProduct(Vector left, Vector right)
        {
            bool LIsNOrE = IsNullOrEmpty(left);
            bool RIsNOrE = IsNullOrEmpty(right);

            if (LIsNOrE && RIsNOrE)
            {
                return double.NaN;
            }
            else if (LIsNOrE || RIsNOrE)
            {
                throw new ArithmeticException();
            }
            else if (left._Size != right._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                double result = 0;

                for (int i = 0; i < left._Size; i++)
                {
                    result += left._VArray[i] * right._VArray[i];
                }

                return result;
            }
        }

        /// <summary>
        /// 返回两个 Vector 对象的向量积。
        /// </summary>
        /// <param name="left">第一个 Vector 对象。</param>
        /// <param name="right">第二个 Vector 对象。</param>
        /// <returns>Vector 对象，表示两个 Vector 对象的向量积。</returns>
        public static Vector CrossProduct(Vector left, Vector right)
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
            else if (left._Type != right._Type || left._Size != right._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                if (left._Size <= 1)
                {
                    return Empty;
                }
                else
                {
                    Vector result = _GetZeroVector(left._Type, left._Size * (left._Size - 1) / 2);

                    int i = 0;

                    for (int j = 0; j < left._Size - 1; j++)
                    {
                        for (int k = j + 1; k < left._Size; k++)
                        {
                            result._VArray[i] = left._VArray[j] * right._VArray[k] - left._VArray[k] * right._VArray[j];

                            i++;
                        }
                    }

                    return result;
                }
            }
        }

        //

        /// <summary>
        /// 返回将 Vector 对象的所有分量取符号数得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        /// <returns>Vector 对象，表示将 Vector 对象的所有分量取符号数得到的结果</returns>
        public static Vector Sign(Vector vector)
        {
            if (IsNullOrEmpty(vector))
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = (double.IsNaN(vector._VArray[i]) ? 0 : Math.Sign(vector._VArray[i]));
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将 Vector 对象的所有分量取绝对值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        /// <returns>Vector 对象，表示将 Vector 对象的所有分量取绝对值得到的结果</returns>
        public static Vector Abs(Vector vector)
        {
            if (IsNullOrEmpty(vector))
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = Math.Abs(vector._VArray[i]);
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将 Vector 对象的所有分量舍入到较大的整数值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        /// <returns>Vector 对象，表示将 Vector 对象的所有分量舍入到较大的整数值得到的结果</returns>
        public static Vector Ceiling(Vector vector)
        {
            if (IsNullOrEmpty(vector))
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = Math.Ceiling(vector._VArray[i]);
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将 Vector 对象的所有分量舍入到较小的整数值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        /// <returns>Vector 对象，表示将 Vector 对象的所有分量舍入到较小的整数值得到的结果</returns>
        public static Vector Floor(Vector vector)
        {
            if (IsNullOrEmpty(vector))
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = Math.Floor(vector._VArray[i]);
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将 Vector 对象的所有分量舍入到最接近的整数值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        /// <returns>Vector 对象，表示将 Vector 对象的所有分量舍入到最接近的整数值得到的结果</returns>
        public static Vector Round(Vector vector)
        {
            if (IsNullOrEmpty(vector))
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = Math.Round(vector._VArray[i]);
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将 Vector 对象的所有分量截断小数部分取整得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        /// <returns>Vector 对象，表示将 Vector 对象的所有分量截断小数部分取整得到的结果</returns>
        public static Vector Truncate(Vector vector)
        {
            if (IsNullOrEmpty(vector))
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = Math.Truncate(vector._VArray[i]);
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将两个 Vector 对象的所有分量分别取最大值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">用于比较的第一个 Vector 对象。</param>
        /// <param name="right">用于比较的第二个 Vector 对象。</param>
        /// <returns>Vector 对象，表示将两个 Vector 对象的所有分量分别取最大值得到的结果</returns>
        public static Vector Max(Vector left, Vector right)
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
            else if (left._Type != right._Type || left._Size != right._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                Vector result = _GetZeroVector(left._Type, left._Size);

                for (int i = 0; i < left._Size; i++)
                {
                    result._VArray[i] = Math.Max(left._VArray[i], right._VArray[i]);
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将两个 Vector 对象的所有分量分别取最小值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">用于比较的第一个 Vector 对象。</param>
        /// <param name="right">用于比较的第二个 Vector 对象。</param>
        /// <returns>Vector 对象，表示将两个 Vector 对象的所有分量分别取最小值得到的结果</returns>
        public static Vector Min(Vector left, Vector right)
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
            else if (left._Type != right._Type || left._Size != right._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                Vector result = _GetZeroVector(left._Type, left._Size);

                for (int i = 0; i < left._Size; i++)
                {
                    result._VArray[i] = Math.Min(left._VArray[i], right._VArray[i]);
                }

                return result;
            }
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 Vector 对象是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Vector 对象。</param>
        /// <param name="right">运算符右侧比较的 Vector 对象。</param>
        /// <returns>布尔值，表示两个 Vector 对象是否相等。</returns>
        public static bool operator ==(Vector left, Vector right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Type != right._Type || left._Size != right._Size)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < left._Size; i++)
                {
                    if (left._VArray[i] != right._VArray[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// 判断两个 Vector 对象是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Vector 对象。</param>
        /// <param name="right">运算符右侧比较的 Vector 对象。</param>
        /// <returns>布尔值，表示两个 Vector 对象是否不相等。</returns>
        public static bool operator !=(Vector left, Vector right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return false;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Type != right._Type || left._Size != right._Size)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < left._Size; i++)
                {
                    if (left._VArray[i] != right._VArray[i])
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// 判断两个 Vector 对象的维度与字典序是否前者小于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Vector 对象。</param>
        /// <param name="right">运算符右侧比较的 Vector 对象。</param>
        /// <returns>布尔值，表示两个 Vector 对象的维度与字典序是否前者小于后者。</returns>
        public static bool operator <(Vector left, Vector right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return false;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Type != right._Type)
            {
                return false;
            }
            else if (left._Size != right._Size)
            {
                return (left._Size < right._Size);
            }
            else
            {
                for (int i = 0; i < left._Size; i++)
                {
                    if (left._VArray[i] != right._VArray[i])
                    {
                        return (left._VArray[i] < right._VArray[i]);
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// 判断两个 Vector 对象的维度与字典序是否前者大于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Vector 对象。</param>
        /// <param name="right">运算符右侧比较的 Vector 对象。</param>
        /// <returns>布尔值，表示两个 Vector 对象的维度与字典序是否前者大于后者。</returns>
        public static bool operator >(Vector left, Vector right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return false;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Type != right._Type)
            {
                return false;
            }
            else if (left._Size != right._Size)
            {
                return (left._Size > right._Size);
            }
            else
            {
                for (int i = 0; i < left._Size; i++)
                {
                    if (left._VArray[i] != right._VArray[i])
                    {
                        return (left._VArray[i] > right._VArray[i]);
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// 判断两个 Vector 对象的维度与字典序是否前者小于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Vector 对象。</param>
        /// <param name="right">运算符右侧比较的 Vector 对象。</param>
        /// <returns>布尔值，表示两个 Vector 对象的维度与字典序是否前者小于或等于后者。</returns>
        public static bool operator <=(Vector left, Vector right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Type != right._Type)
            {
                return false;
            }
            else if (left._Size != right._Size)
            {
                return (left._Size < right._Size);
            }
            else
            {
                for (int i = 0; i < left._Size; i++)
                {
                    if (left._VArray[i] != right._VArray[i])
                    {
                        return (left._VArray[i] < right._VArray[i]);
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// 判断两个 Vector 对象的维度与字典序是否前者大于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Vector 对象。</param>
        /// <param name="right">运算符右侧比较的 Vector 对象。</param>
        /// <returns>布尔值，表示两个 Vector 对象的维度与字典序是否前者大于或等于后者。</returns>
        public static bool operator >=(Vector left, Vector right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Type != right._Type)
            {
                return false;
            }
            else if (left._Size != right._Size)
            {
                return (left._Size > right._Size);
            }
            else
            {
                for (int i = 0; i < left._Size; i++)
                {
                    if (left._VArray[i] != right._VArray[i])
                    {
                        return (left._VArray[i] > right._VArray[i]);
                    }
                }

                return true;
            }
        }

        //

        /// <summary>
        /// 返回在 Vector 对象的所有分量前添加正号得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">运算符右侧的 Vector 对象。</param>
        /// <returns>Vector 对象，表示在 Vector 对象的所有分量前添加正号得到的结果。</returns>
        public static Vector operator +(Vector vector)
        {
            if (IsNullOrEmpty(vector))
            {
                return Empty;
            }
            else
            {
                return vector.Copy();
            }
        }

        /// <summary>
        /// 返回在 Vector 对象的所有分量前添加负号得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">运算符右侧的 Vector 对象。</param>
        /// <returns>Vector 对象，表示在 Vector 对象的所有分量前添加负号得到的结果。</returns>
        public static Vector operator -(Vector vector)
        {
            if (IsNullOrEmpty(vector))
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = -vector._VArray[i];
                }

                return result;
            }
        }

        //

        /// <summary>
        /// 返回将 Vector 对象的所有分量与双精度浮点数相加得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象，表示被加数。</param>
        /// <param name="n">双精度浮点数，表示加数。</param>
        /// <returns>Vector 对象，表示将 Vector 对象的所有分量与双精度浮点数相加得到的结果。</returns>
        public static Vector operator +(Vector vector, double n)
        {
            if (IsNullOrEmpty(vector))
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = vector._VArray[i] + n;
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将双精度浮点数与 Vector 对象的所有分量相加得到的 Vector 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被加数。</param>
        /// <param name="vector">Vector 对象，表示加数。</param>
        /// <returns>Vector 对象，表示将双精度浮点数与 Vector 对象的所有分量相加得到的结果。</returns>
        public static Vector operator +(double n, Vector vector)
        {
            if (IsNullOrEmpty(vector))
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = n + vector._VArray[i];
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将 Vector 对象与 Vector 对象的所有分量对应相加得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">Vector 对象，表示被加数。</param>
        /// <param name="right">Vector 对象，表示加数。</param>
        /// <returns>Vector 对象，表示将 Vector 对象与 Vector 对象的所有分量对应相加得到的结果。</returns>
        public static Vector operator +(Vector left, Vector right)
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
            else if (left._Type != right._Type || left._Size != right._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                Vector result = _GetZeroVector(left._Type, left._Size);

                for (int i = 0; i < left._Size; i++)
                {
                    result._VArray[i] = left._VArray[i] + right._VArray[i];
                }

                return result;
            }
        }

        //

        /// <summary>
        /// 返回将 Vector 对象的所有分量与双精度浮点数相减得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象，表示被减数。</param>
        /// <param name="n">双精度浮点数，表示减数。</param>
        /// <returns>Vector 对象，表示将 Vector 对象的所有分量与双精度浮点数相减得到的结果。</returns>
        public static Vector operator -(Vector vector, double n)
        {
            if (IsNullOrEmpty(vector))
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = vector._VArray[i] - n;
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将双精度浮点数与 Vector 对象的所有分量相减得到的 Vector 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被减数。</param>
        /// <param name="vector">Vector 对象，表示减数。</param>
        /// <returns>Vector 对象，表示将双精度浮点数与 Vector 对象的所有分量相减得到的结果。</returns>
        public static Vector operator -(double n, Vector vector)
        {
            if (IsNullOrEmpty(vector))
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = n - vector._VArray[i];
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将 Vector 对象与 Vector 对象的所有分量对应相减得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">Vector 对象，表示被减数。</param>
        /// <param name="right">Vector 对象，表示减数。</param>
        /// <returns>Vector 对象，表示将 Vector 对象与 Vector 对象的所有分量对应相减得到的结果。</returns>
        public static Vector operator -(Vector left, Vector right)
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
            else if (left._Type != right._Type || left._Size != right._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                Vector result = _GetZeroVector(left._Type, left._Size);

                for (int i = 0; i < left._Size; i++)
                {
                    result._VArray[i] = left._VArray[i] - right._VArray[i];
                }

                return result;
            }
        }

        //

        /// <summary>
        /// 返回将 Vector 对象的所有分量与双精度浮点数相乘得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象，表示被乘数。</param>
        /// <param name="n">双精度浮点数，表示乘数。</param>
        /// <returns>Vector 对象，表示将 Vector 对象的所有分量与双精度浮点数相乘得到的结果。</returns>
        public static Vector operator *(Vector vector, double n)
        {
            if (IsNullOrEmpty(vector))
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = vector._VArray[i] * n;
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将双精度浮点数与 Vector 对象的所有分量相乘得到的 Vector 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被乘数。</param>
        /// <param name="vector">Vector 对象，表示乘数。</param>
        /// <returns>Vector 对象，表示将双精度浮点数与 Vector 对象的所有分量相乘得到的结果。</returns>
        public static Vector operator *(double n, Vector vector)
        {
            if (IsNullOrEmpty(vector))
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = n * vector._VArray[i];
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将 Vector 对象与 Vector 对象的所有分量对应相乘得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">Vector 对象，表示被乘数。</param>
        /// <param name="right">Vector 对象，表示乘数。</param>
        /// <returns>Vector 对象，表示将 Vector 对象与 Vector 对象的所有分量对应相乘得到的结果。</returns>
        public static Vector operator *(Vector left, Vector right)
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
            else if (left._Type != right._Type || left._Size != right._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                Vector result = _GetZeroVector(left._Type, left._Size);

                for (int i = 0; i < left._Size; i++)
                {
                    result._VArray[i] = left._VArray[i] * right._VArray[i];
                }

                return result;
            }
        }

        //

        /// <summary>
        /// 返回将 Vector 对象的所有分量与双精度浮点数相除得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象，表示被除数。</param>
        /// <param name="n">双精度浮点数，表示除数。</param>
        /// <returns>Vector 对象，表示将 Vector 对象的所有分量与双精度浮点数相除得到的结果。</returns>
        public static Vector operator /(Vector vector, double n)
        {
            if (IsNullOrEmpty(vector))
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = vector._VArray[i] / n;
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将双精度浮点数与 Vector 对象的所有分量相除得到的 Vector 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被除数。</param>
        /// <param name="vector">Vector 对象，表示除数。</param>
        /// <returns>Vector 对象，表示将双精度浮点数与 Vector 对象的所有分量相除得到的结果。</returns>
        public static Vector operator /(double n, Vector vector)
        {
            if (IsNullOrEmpty(vector))
            {
                return Empty;
            }
            else
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = n / vector._VArray[i];
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将 Vector 对象与 Vector 对象的所有分量对应相除得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">Vector 对象，表示被除数。</param>
        /// <param name="right">Vector 对象，表示除数。</param>
        /// <returns>Vector 对象，表示将 Vector 对象与 Vector 对象的所有分量对应相除得到的结果。</returns>
        public static Vector operator /(Vector left, Vector right)
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
            else if (left._Type != right._Type || left._Size != right._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                Vector result = _GetZeroVector(left._Type, left._Size);

                for (int i = 0; i < left._Size; i++)
                {
                    result._VArray[i] = left._VArray[i] / right._VArray[i];
                }

                return result;
            }
        }

        #endregion

        #region 显式接口成员实现

        #region Com.IVector<T>

        int IVector<double>.Size
        {
            get
            {
                return Dimension;
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        int IVector<double>.Capacity
        {
            get
            {
                return Dimension;
            }
        }

        void IVector<double>.Trim()
        {
            throw new NotSupportedException();
        }

        #endregion

        #region System.Collections.IList

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                if (value == null || !(value is double))
                {
                    throw new ArgumentNullException();
                }

                //

                this[index] = (double)value;
            }
        }

        int IList.Add(object item)
        {
            throw new NotSupportedException();
        }

        void IList.Clear()
        {
            if (_Size > 0)
            {
                Array.Clear(_VArray, 0, _Size);
            }
        }

        bool IList.Contains(object item)
        {
            if (item == null || !(item is double))
            {
                return false;
            }
            else
            {
                return Contains((double)item);
            }
        }

        int IList.IndexOf(object item)
        {
            if (item == null || !(item is double))
            {
                return -1;
            }
            else
            {
                return IndexOf((double)item);
            }
        }

        void IList.Insert(int index, object item)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(object item)
        {
            throw new NotSupportedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region System.Collections.ICollection

        int ICollection.Count
        {
            get
            {
                return Dimension;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return this;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }

            if (array.Rank != 1)
            {
                throw new RankException();
            }

            if (array.Length < _Size)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (_Size > 0)
            {
                _VArray.CopyTo(array, index);
            }
        }

        #endregion

        #region System.Collections.IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        private sealed class Enumerator : IEnumerator // 实现 System.Collections.IEnumerator 的迭代器。
        {
            private Vector _Vector;
            private int _Index;

            internal Enumerator(Vector vector)
            {
                _Vector = vector;
                _Index = -1;
            }

            object IEnumerator.Current
            {
                get
                {
                    if (IsNullOrEmpty(_Vector) || (_Index < 0 || _Index >= _Vector._Size))
                    {
                        throw new IndexOutOfRangeException();
                    }

                    //

                    return _Vector._VArray[_Index];
                }
            }

            bool IEnumerator.MoveNext()
            {
                if (IsNullOrEmpty(_Vector) || _Index >= _Vector._Size - 1)
                {
                    return false;
                }
                else
                {
                    _Index++;

                    return true;
                }
            }

            void IEnumerator.Reset()
            {
                _Index = -1;
            }
        }

        #endregion

        #region System.Collections.Generic.IList<T>

        void IList<double>.Insert(int index, double item)
        {
            throw new NotSupportedException();
        }

        void IList<double>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region System.Collections.Generic.ICollection<T>

        int ICollection<double>.Count
        {
            get
            {
                return Dimension;
            }
        }

        void ICollection<double>.Add(double item)
        {
            throw new NotSupportedException();
        }

        void ICollection<double>.Clear()
        {
            if (_Size > 0)
            {
                Array.Clear(_VArray, 0, _Size);
            }
        }

        void ICollection<double>.CopyTo(double[] array, int index)
        {
            if (_Size > 0 && (array != null && array.Length >= _Size))
            {
                _VArray.CopyTo(array, index);
            }
        }

        bool ICollection<double>.Remove(double item)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region System.Collections.Generic.IEnumerable<out T>

        IEnumerator<double> IEnumerable<double>.GetEnumerator()
        {
            return new GenericEnumerator(this);
        }

        private sealed class GenericEnumerator : IEnumerator<double> // 实现 System.Collections.Generic.IEnumerator<out T> 的迭代器。
        {
            private Vector _Vector;
            private int _Index;

            internal GenericEnumerator(Vector vector)
            {
                _Vector = vector;
                _Index = -1;
            }

            void IDisposable.Dispose()
            {
                _Vector = null;
            }

            object IEnumerator.Current
            {
                get
                {
                    if (IsNullOrEmpty(_Vector) || _Index < 0 || _Index >= _Vector._Size)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    //

                    return _Vector._VArray[_Index];
                }
            }

            bool IEnumerator.MoveNext()
            {
                if (IsNullOrEmpty(_Vector) || _Index >= _Vector._Size - 1)
                {
                    return false;
                }
                else
                {
                    _Index++;

                    return true;
                }
            }

            void IEnumerator.Reset()
            {
                _Index = -1;
            }

            double IEnumerator<double>.Current
            {
                get
                {
                    if (IsNullOrEmpty(_Vector) || (_Index < 0 || _Index >= _Vector._Size))
                    {
                        throw new IndexOutOfRangeException();
                    }

                    //

                    return _Vector._VArray[_Index];
                }
            }
        }

        #endregion

        #endregion
    }
}