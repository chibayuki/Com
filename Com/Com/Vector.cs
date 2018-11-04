/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.Vector
Version 18.9.28.2200

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
    public sealed class Vector : IEquatable<Vector>, IEuclideanVector<Vector>, ILinearAlgebraVector<Vector>, IAffine<Vector>
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

        #region 私有与内部成员

        private const int _MaxSize = 2146435071; // Vector 允许包含的最大元素数量，等于 System.Array.MaxArrayLength。

        //

        private Type _Type; // 此 Vector 的向量类型。

        private int _Size; // 此 Vector 存储的向量维度。

        private double[] _VArray = null; // 用于存储向量在各基向量方向的分量的数组。

        //

        private Matrix _ToMatrixForAffineTransform() // 获取此 Vector 用于仿射变换的矩阵。
        {
            if (_Size > 0)
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

            return Matrix.Empty;
        }

        //

        private static Vector _GetZeroVector(Type type, int dimension) // 获取指定向量类型与维度的零向量。
        {
            if (dimension > 0)
            {
                Vector result = Empty;

                result._Type = type;
                result._Size = Math.Min(_MaxSize, dimension);
                result._VArray = new double[result._Size];

                return result;
            }

            return Empty;
        }

        //

        internal static Vector UnsafeCreateInstance(Type type, params double[] values) // 以不安全方式创建 Vector 的新实例。
        {
            Vector result = Empty;

            result._Type = type;

            if (!InternalMethod.IsNullOrEmpty(values))
            {
                int length = values.Length;

                if (length <= _MaxSize)
                {
                    result._Size = length;
                    result._VArray = values;
                }
            }

            return result;
        }

        #endregion

        #region 构造函数

        internal Vector() // 不使用任何参数初始化 Vector 的新实例。
        {
            _Type = Type.ColumnVector;
            _Size = 0;
            _VArray = new double[0];
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

                if (length <= _MaxSize)
                {
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

                if (length <= _MaxSize)
                {
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
            else
            {
                _Size = 0;
                _VArray = new double[0];
            }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置此 Vector 指定索引的基向量方向的分量。
        /// </summary>
        /// <param name="index">索引。</param>
        public double this[int index]
        {
            get
            {
                if (_Size > 0 && (index >= 0 && index < _Size))
                {
                    return _VArray[index];
                }

                return double.NaN;
            }

            set
            {
                if (_Size > 0 && (index >= 0 && index < _Size))
                {
                    _VArray[index] = value;
                }
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
                if (_Size > 0)
                {
                    return _Size;
                }

                return 0;
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
                if (_Size > 0)
                {
                    bool result = true;

                    for (int i = 0; i < _Size; i++)
                    {
                        if (_VArray[i] != 0)
                        {
                            result = false;

                            break;
                        }
                    }

                    return result;
                }

                return false;
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
                if (_Size > 0)
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

                return false;
            }
        }

        /// <summary>
        /// 获取表示此 Vector 是否包含无穷大分量的布尔值。
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                if (_Size > 0)
                {
                    for (int i = 0; i < _Size; i++)
                    {
                        if (double.IsNaN(_VArray[i]))
                        {
                            return false;
                        }
                    }

                    for (int i = 0; i < _Size; i++)
                    {
                        if (double.IsInfinity(_VArray[i]))
                        {
                            return true;
                        }
                    }

                    return false;
                }

                return false;
            }
        }

        /// <summary>
        /// 获取表示此 Vector 是否包含非数字或无穷大分量的布尔值。
        /// </summary>
        public bool IsNaNOrInfinity
        {
            get
            {
                if (_Size > 0)
                {
                    for (int i = 0; i < _Size; i++)
                    {
                        if (double.IsNaN(_VArray[i]) || double.IsInfinity(_VArray[i]))
                        {
                            return true;
                        }
                    }

                    return false;
                }

                return false;
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
                if (_Size > 0)
                {
                    return Math.Sqrt(ModuleSquared);
                }

                return double.NaN;
            }
        }

        /// <summary>
        /// 获取此 Vector 的模平方。
        /// </summary>
        public double ModuleSquared
        {
            get
            {
                if (_Size > 0)
                {
                    double SqrSum = 0;

                    for (int i = 0; i < _Size; i++)
                    {
                        SqrSum += _VArray[i] * _VArray[i];
                    }

                    return SqrSum;
                }

                return double.NaN;
            }
        }

        //

        /// <summary>
        /// 获取此 Vector 的相反向量。
        /// </summary>
        public Vector Negate
        {
            get
            {
                if (_Size > 0)
                {
                    Vector result = _GetZeroVector(_Type, _Size);

                    for (int i = 0; i < _Size; i++)
                    {
                        result._VArray[i] = -_VArray[i];
                    }

                    return result;
                }

                return Empty;
            }
        }

        /// <summary>
        /// 获取此 Vector 的规范化向量。
        /// </summary>
        public Vector Normalize
        {
            get
            {
                if (_Size > 0)
                {
                    double Mod = Module;

                    if (Mod > 0)
                    {
                        Vector result = _GetZeroVector(_Type, _Size);

                        for (int i = 0; i < _Size; i++)
                        {
                            result._VArray[i] = _VArray[i] / Mod;
                        }

                        return result;
                    }
                }

                return Empty;
            }
        }

        /// <summary>
        /// 获取此 Vector 的转置向量。
        /// </summary>
        public Vector Transport
        {
            get
            {
                if (_Size > 0)
                {
                    return new Vector((_Type == Type.ColumnVector ? Type.RowVector : Type.ColumnVector), _VArray);
                }

                return Empty;
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
                return new Vector();
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 Vector 是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Vector))
            {
                return false;
            }

            return Equals((Vector)obj);
        }

        /// <summary>
        /// 返回此 Vector 的哈希代码。
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 Vector 转换为字符串。
        /// </summary>
        public override string ToString()
        {
            string Str = string.Empty;

            if (_Size > 0)
            {
                Str = string.Concat("Type=", (_Type == Type.ColumnVector ? "ColumnVector" : "RowVector"), ", Dimension=", _Size);
            }
            else
            {
                Str = "NonVector";
            }

            return string.Concat(base.GetType().Name, " [", Str, "]");
        }

        //

        /// <summary>
        /// 判断此 Vector 是否与指定的 Vector 对象相等。
        /// </summary>
        /// <param name="vector">用于比较的 Vector 对象。</param>
        public bool Equals(Vector vector)
        {
            if (_Size <= 0 || IsNullOrEmpty(vector) || _Type != vector._Type || _Size != vector._Size)
            {
                return false;
            }

            for (int i = 0; i < _Size; i++)
            {
                if (!_VArray[i].Equals(vector._VArray[i]))
                {
                    return false;
                }
            }

            return true;
        }

        //

        /// <summary>
        /// 获取此 Vector 的副本。
        /// </summary>
        public Vector Copy()
        {
            if (_Size > 0)
            {
                Vector result = new Vector(_Type, _VArray);

                return result;
            }

            return Empty;
        }

        //

        /// <summary>
        /// 遍历此 Vector 的所有分量并返回第一个与指定值相等的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        public int IndexOf(double item)
        {
            if (_Size > 0)
            {
                return Array.IndexOf(_VArray, item, 0, _Size);
            }

            return -1;
        }

        /// <summary>
        /// 从指定的索引开始遍历此 Vector 的所有分量并返回第一个与指定值相等的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        public int IndexOf(double item, int startIndex)
        {
            if (_Size > 0 && (startIndex >= 0 && startIndex < _Size))
            {
                return Array.IndexOf(_VArray, item, startIndex, _Size - startIndex);
            }

            return -1;
        }

        /// <summary>
        /// 从指定的索引开始遍历此 Vector 指定数量的分量并返回第一个与指定值相等的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <param name="count">遍历的分量数量。</param>
        public int IndexOf(double item, int startIndex, int count)
        {
            if (_Size > 0 && (startIndex >= 0 && startIndex < _Size) && count > 0)
            {
                count = Math.Min(_Size - startIndex, count);

                return Array.IndexOf(_VArray, item, startIndex, count);
            }

            return -1;
        }

        /// <summary>
        /// 逆序遍历此 Vector 的所有分量并返回第一个与指定值相等的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        public int LastIndexOf(double item)
        {
            if (_Size > 0)
            {
                return Array.LastIndexOf(_VArray, item, _Size - 1, _Size);
            }

            return -1;
        }

        /// <summary>
        /// 从指定的索引开始逆序遍历此 Vector 的所有分量并返回第一个与指定值相等的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        public int LastIndexOf(double item, int startIndex)
        {
            if (_Size > 0 && (startIndex >= 0 && startIndex < _Size))
            {
                return Array.IndexOf(_VArray, item, startIndex, startIndex + 1);
            }

            return -1;
        }

        /// <summary>
        /// 从指定的索引开始逆序遍历此 Vector 指定数量的分量并返回第一个与指定值相等的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <param name="count">遍历的分量数量。</param>
        public int LastIndexOf(double item, int startIndex, int count)
        {
            if (_Size > 0 && (startIndex >= 0 && startIndex < _Size) && count > 0)
            {
                count = Math.Min(startIndex + 1, count);

                return Array.IndexOf(_VArray, item, startIndex, count);
            }

            return -1;
        }

        /// <summary>
        /// 遍历此 Vector 的所有分量并返回表示是否存在与指定值相等的分量的布尔值。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        public bool Contains(double item)
        {
            if (_Size > 0)
            {
                for (int i = 0; i < _Size; i++)
                {
                    if (_VArray[i].Equals(item))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //

        /// <summary>
        /// 将此 Vector 转换为双精度浮点数数组。
        /// </summary>
        public double[] ToArray()
        {
            if (_Size > 0)
            {
                double[] result = new double[_Size];

                Array.Copy(_VArray, result, _Size);

                return result;
            }

            return new double[0];
        }

        /// <summary>
        /// 将此 Vector 转换为双精度浮点数列表。
        /// </summary>
        public List<double> ToList()
        {
            if (_Size > 0)
            {
                List<double> result = new List<double>(_VArray);

                return result;
            }

            return new List<double>(0);
        }

        /// <summary>
        /// 返回将此 Vector 转换为矩阵的 Matrix 的新实例。
        /// </summary>
        public Matrix ToMatrix()
        {
            if (_Size > 0)
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

            return Matrix.Empty;
        }

        //

        /// <summary>
        /// 返回将此 Vector 表示的直角坐标系坐标转换为极坐标系、球坐标系或超球坐标系坐标的新实例。
        /// </summary>
        public Vector ToSpherical()
        {
            if (_Size > 0)
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
                                return (Angle + Math.PI);
                            }
                            else if (y < 0)
                            {
                                return (Angle + 2 * Math.PI);
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
                                    return (Angle + Math.PI);
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

            return Empty;
        }

        /// <summary>
        /// 返回将此 Vector 表示的极坐标系、球坐标系或超球坐标系坐标转换为直角坐标系坐标的新实例。
        /// </summary>
        public Vector ToCartesian()
        {
            if (_Size > 0)
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

            return Empty;
        }

        //

        /// <summary>
        /// 返回此 Vector 与指定的 Vector 对象之间的距离。
        /// </summary>
        /// <param name="vector">Vector 对象，表示起始向量。</param>
        public double DistanceFrom(Vector vector)
        {
            if (_Size > 0 && !IsNullOrEmpty(vector) && _Size == vector._Size)
            {
                double SqrSum = 0;

                for (int i = 0; i < _Size; i++)
                {
                    SqrSum += (_VArray[i] - vector._VArray[i]) * (_VArray[i] - vector._VArray[i]);
                }

                return Math.Sqrt(SqrSum);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回此 Vector 与指定的 Vector 对象之间的夹角（弧度）。
        /// </summary>
        /// <param name="vector">Vector 对象，表示起始向量。</param>
        public double AngleFrom(Vector vector)
        {
            if (_Size > 0 && !IsNullOrEmpty(vector) && _Size == vector._Size)
            {
                if (IsZero || vector.IsZero)
                {
                    return 0;
                }

                double DotProduct = 0;

                for (int i = 0; i < _Size; i++)
                {
                    DotProduct += _VArray[i] * vector._VArray[i];
                }

                return Math.Acos(DotProduct / Module / vector.Module);
            }

            return double.NaN;
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
        /// 按 Vector 对象将此 Vector 平移指定的量。
        /// </summary>
        /// <param name="vector">Vector 对象，用于平移此 Vector。</param>
        public void Offset(Vector vector)
        {
            if (_Size > 0 && !IsNullOrEmpty(vector) && _Size == vector._Size)
            {
                for (int i = 0; i < _Size; i++)
                {
                    _VArray[i] += vector._VArray[i];
                }
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的位移将此 Vector 的副本的所有分量平移指定的量的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public Vector OffsetCopy(double d)
        {
            if (_Size > 0)
            {
                Vector result = Copy();

                for (int i = 0; i < result._Size; i++)
                {
                    result._VArray[i] += d;
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回按 Vector 对象表示的位移将此 Vector 的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象表示的位移。</param>
        public Vector OffsetCopy(Vector vector)
        {
            if (_Size > 0 && !IsNullOrEmpty(vector) && _Size == vector._Size)
            {
                Vector result = Copy();

                for (int i = 0; i < result._Size; i++)
                {
                    result._VArray[i] += vector._VArray[i];
                }

                return result;
            }

            return Empty;
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
            if (_Size > 0 && !IsNullOrEmpty(vector) && _Size == vector._Size)
            {
                for (int i = 0; i < _Size; i++)
                {
                    _VArray[i] *= vector._VArray[i];
                }
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的缩放因数将此 Vector 的副本的所有分量缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public Vector ScaleCopy(double s)
        {
            if (_Size > 0)
            {
                Vector result = Copy();

                for (int i = 0; i < result._Size; i++)
                {
                    result._VArray[i] *= s;
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回按 Vector 对象将此 Vector 的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象，用于缩放此 Vector。</param>
        public Vector ScaleCopy(Vector vector)
        {
            if (_Size > 0 && !IsNullOrEmpty(vector) && _Size == vector._Size)
            {
                Vector result = Copy();

                for (int i = 0; i < result._Size; i++)
                {
                    result._VArray[i] *= vector._VArray[i];
                }

                return result;
            }

            return Empty;
        }

        //

        /// <summary>
        /// 将此 Vector 的指定的基向量方向的分量翻转。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        public void Reflect(int index)
        {
            if (_Size > 0 && (index >= 0 && index < _Size))
            {
                _VArray[index] = -_VArray[index];
            }
        }

        /// <summary>
        /// 返回将此 Vector 的副本的由指定的基向量方向的分量翻转的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        public Vector ReflectCopy(int index)
        {
            if (_Size > 0 && (index >= 0 && index < _Size))
            {
                Vector result = Copy();

                result._VArray[index] = -result._VArray[index];

                return result;
            }

            return Empty;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度将此 Vector 剪切指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向平行且同方向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向垂直且共平面的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 Vector 的副本沿平行于索引 index1 指定的基向量且与之同方向以及垂直于 index2 指定的基向量且与之共平面的方向剪切的角度（弧度）。</param>
        public void Shear(int index1, int index2, double angle)
        {
            if (_Size >= 2 && (index1 >= 0 && index1 < _Size) && (index2 >= 0 && index2 < _Size) && index1 != index2)
            {
                _VArray[index1] += _VArray[index2] * Math.Tan(angle);
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 Vector 的副本剪切指定的角度的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定与剪切方向平行且同方向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向垂直且共平面的基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 Vector 的副本沿平行于索引 index1 指定的基向量且与之同方向以及垂直于 index2 指定的基向量且与之共平面的方向剪切的角度（弧度）。</param>
        public Vector ShearCopy(int index1, int index2, double angle)
        {
            if (_Size >= 2 && (index1 >= 0 && index1 < _Size) && (index2 >= 0 && index2 < _Size) && index1 != index2)
            {
                Vector result = Copy();

                result._VArray[index1] += result._VArray[index2] * Math.Tan(angle);

                return result;
            }

            return Empty;
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
            if (_Size >= 2 && (index1 >= 0 && index1 < _Size) && (index2 >= 0 && index2 < _Size) && index1 != index2)
            {
                Matrix matrixRotate = RotateMatrix(_Type, _Size, index1, index2, angle);
                Matrix matrixVector = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    Matrix result = Matrix.Multiply(matrixRotate, matrixVector);

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(1, _Size + 1))
                    {
                        Array.Copy(result.GetColumn(0)._VArray, _VArray, _Size);
                    }
                }
                else
                {
                    Matrix result = Matrix.Multiply(matrixVector, matrixRotate);

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(_Size + 1, 1))
                    {
                        Array.Copy(result.GetRow(0)._VArray, _VArray, _Size);
                    }
                }
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的弧度将此 Vector 的副本旋转指定的角度的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示此 Vector 的副本绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        public Vector RotateCopy(int index1, int index2, double angle)
        {
            if (_Size >= 2 && (index1 >= 0 && index1 < _Size) && (index2 >= 0 && index2 < _Size) && index1 != index2)
            {
                Matrix matrixRotate = RotateMatrix(_Type, _Size, index1, index2, angle);
                Matrix matrixVector = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    Matrix result = Matrix.Multiply(matrixRotate, matrixVector);

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(1, _Size + 1))
                    {
                        Vector vector = _GetZeroVector(_Type, _Size);

                        Array.Copy(result.GetColumn(0)._VArray, vector._VArray, _Size);

                        return vector;
                    }
                }
                else
                {
                    Matrix result = Matrix.Multiply(matrixVector, matrixRotate);

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(_Size + 1, 1))
                    {
                        Vector vector = _GetZeroVector(_Type, _Size);

                        Array.Copy(result.GetRow(0)._VArray, vector._VArray, _Size);

                        return vector;
                    }
                }
            }

            return Empty;
        }

        //

        /// <summary>
        /// 按 Matrix 对象表示的仿射矩阵将此 Vector 进行仿射变换。
        /// </summary>
        /// <param name="matrix">Matrix 对象表示的仿射矩阵，对于列向量应为左矩阵，对于行向量应为右矩阵。</param>
        public void AffineTransform(Matrix matrix)
        {
            if (_Size > 0 && !Matrix.IsNullOrEmpty(matrix) && matrix.Size == new Size(_Size + 1, _Size + 1))
            {
                Matrix matrixVector = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    Matrix result = Matrix.Multiply(matrix, matrixVector);

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(1, _Size + 1))
                    {
                        Array.Copy(result.GetColumn(0)._VArray, _VArray, _Size);
                    }
                }
                else
                {
                    Matrix result = Matrix.Multiply(matrixVector, matrix);

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(_Size + 1, 1))
                    {
                        Array.Copy(result.GetRow(0)._VArray, _VArray, _Size);
                    }
                }
            }
        }

        /// <summary>
        /// 按 Matrix 对象列表表示的仿射矩阵列表将此 Vector 进行仿射变换。
        /// </summary>
        /// <param name="matrixList">Matrix 对象列表表示的仿射矩阵列表，对于列向量应全部为左矩阵，对于行向量应全部为右矩阵。</param>
        public void AffineTransform(List<Matrix> matrixList)
        {
            if (_Size > 0 && !InternalMethod.IsNullOrEmpty(matrixList))
            {
                Matrix result = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    for (int i = 0; i < matrixList.Count; i++)
                    {
                        Matrix matrix = matrixList[i];

                        bool flag = (!Matrix.IsNullOrEmpty(matrix) && matrix.Size == new Size(_Size + 1, _Size + 1));

                        if (flag)
                        {
                            result = Matrix.Multiply(matrix, result);

                            flag = (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(1, _Size + 1));
                        }

                        if (!flag)
                        {
                            return;
                        }
                    }

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(1, _Size + 1))
                    {
                        Array.Copy(result.GetColumn(0)._VArray, _VArray, _Size);
                    }
                }
                else
                {
                    for (int i = 0; i < matrixList.Count; i++)
                    {
                        Matrix matrix = matrixList[i];

                        bool flag = (!Matrix.IsNullOrEmpty(matrix) && matrix.Size == new Size(_Size + 1, _Size + 1));

                        if (flag)
                        {
                            result = Matrix.Multiply(result, matrix);

                            flag = (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(_Size + 1, 1));
                        }

                        if (!flag)
                        {
                            return;
                        }
                    }

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(_Size + 1, 1))
                    {
                        Array.Copy(result.GetRow(0)._VArray, _VArray, _Size);
                    }
                }
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的仿射矩阵将此 Vector 的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrix">Matrix 对象表示的仿射矩阵，对于列向量应为左矩阵，对于行向量应为右矩阵。</param>
        public Vector AffineTransformCopy(Matrix matrix)
        {
            if (_Size > 0 && !Matrix.IsNullOrEmpty(matrix) && matrix.Size == new Size(_Size + 1, _Size + 1))
            {
                Matrix matrixVector = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    Matrix result = Matrix.Multiply(matrix, matrixVector);

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(1, _Size + 1))
                    {
                        Vector vector = _GetZeroVector(_Type, _Size);

                        Array.Copy(result.GetColumn(0)._VArray, vector._VArray, _Size);

                        return vector;
                    }
                }
                else
                {
                    Matrix result = Matrix.Multiply(matrixVector, matrix);

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(_Size + 1, 1))
                    {
                        Vector vector = _GetZeroVector(_Type, _Size);

                        Array.Copy(result.GetRow(0)._VArray, vector._VArray, _Size);

                        return vector;
                    }
                }
            }

            return Empty;
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的仿射矩阵列表将此 Vector 的副本进行仿射变换的新实例。
        /// </summary>
        /// <param name="matrixList">Matrix 对象列表表示的仿射矩阵列表，对于列向量应全部为左矩阵，对于行向量应全部为右矩阵。</param>
        public Vector AffineTransformCopy(List<Matrix> matrixList)
        {
            if (_Size > 0 && !InternalMethod.IsNullOrEmpty(matrixList))
            {
                Matrix result = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    for (int i = 0; i < matrixList.Count; i++)
                    {
                        Matrix matrix = matrixList[i];

                        bool flag = (!Matrix.IsNullOrEmpty(matrix) && matrix.Size == new Size(_Size + 1, _Size + 1));

                        if (flag)
                        {
                            result = Matrix.Multiply(matrix, result);

                            flag = (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(1, _Size + 1));
                        }

                        if (!flag)
                        {
                            return Empty;
                        }
                    }

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(1, _Size + 1))
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
                        Matrix matrix = matrixList[i];

                        bool flag = (!Matrix.IsNullOrEmpty(matrix) && matrix.Size == new Size(_Size + 1, _Size + 1));

                        if (flag)
                        {
                            result = Matrix.Multiply(result, matrix);

                            flag = (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(_Size + 1, 1));
                        }

                        if (!flag)
                        {
                            return Empty;
                        }
                    }

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(_Size + 1, 1))
                    {
                        Vector vector = _GetZeroVector(_Type, _Size);

                        Array.Copy(result.GetRow(0)._VArray, vector._VArray, _Size);

                        return vector;
                    }
                }
            }

            return Empty;
        }

        /// <summary>
        /// 按 Matrix 对象表示的仿射矩阵将此 Vector 进行逆仿射变换。
        /// </summary>
        /// <param name="matrix">Matrix 对象表示的仿射矩阵，对于列向量应为左矩阵，对于行向量应为右矩阵。</param>
        public void InverseAffineTransform(Matrix matrix)
        {
            if (_Size > 0 && !Matrix.IsNullOrEmpty(matrix) && matrix.Size == new Size(_Size + 1, _Size + 1))
            {
                Matrix matrixVector = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    Matrix result = Matrix.DivideLeft(matrix, matrixVector);

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(1, _Size + 1))
                    {
                        Array.Copy(result.GetColumn(0)._VArray, _VArray, _Size);
                    }
                }
                else
                {
                    Matrix result = Matrix.DivideRight(matrixVector, matrix);

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(_Size + 1, 1))
                    {
                        Array.Copy(result.GetRow(0)._VArray, _VArray, _Size);
                    }
                }
            }
        }

        /// <summary>
        /// 按 Matrix 对象列表表示的仿射矩阵列表将此 Vector 进行逆仿射变换。
        /// </summary>
        /// <param name="matrixList">Matrix 对象列表表示的仿射矩阵列表，对于列向量应全部为左矩阵，对于行向量应全部为右矩阵。</param>
        public void InverseAffineTransform(List<Matrix> matrixList)
        {
            if (_Size > 0 && !InternalMethod.IsNullOrEmpty(matrixList))
            {
                Matrix result = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    for (int i = matrixList.Count - 1; i >= 0; i--)
                    {
                        Matrix matrix = matrixList[i];

                        bool flag = (!Matrix.IsNullOrEmpty(matrix) && matrix.Size == new Size(_Size + 1, _Size + 1));

                        if (flag)
                        {
                            result = Matrix.DivideLeft(matrix, result);

                            flag = (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(1, _Size + 1));
                        }

                        if (!flag)
                        {
                            return;
                        }
                    }

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(1, _Size + 1))
                    {
                        Array.Copy(result.GetColumn(0)._VArray, _VArray, _Size);
                    }
                }
                else
                {
                    for (int i = matrixList.Count - 1; i >= 0; i--)
                    {
                        Matrix matrix = matrixList[i];

                        bool flag = (!Matrix.IsNullOrEmpty(matrix) && matrix.Size == new Size(_Size + 1, _Size + 1));

                        if (flag)
                        {
                            result = Matrix.DivideRight(result, matrix);

                            flag = (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(_Size + 1, 1));
                        }

                        if (!flag)
                        {
                            return;
                        }
                    }

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(_Size + 1, 1))
                    {
                        Array.Copy(result.GetRow(0)._VArray, _VArray, _Size);
                    }
                }
            }
        }

        /// <summary>
        /// 返回按 Matrix 对象表示的仿射矩阵将此 Vector 的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrix">Matrix 对象表示的仿射矩阵，对于列向量应为左矩阵，对于行向量应为右矩阵。</param>
        public Vector InverseAffineTransformCopy(Matrix matrix)
        {
            if (_Size > 0 && !Matrix.IsNullOrEmpty(matrix) && matrix.Size == new Size(_Size + 1, _Size + 1))
            {
                Matrix matrixVector = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    Matrix result = Matrix.DivideLeft(matrix, matrixVector);

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(1, _Size + 1))
                    {
                        Vector vector = _GetZeroVector(_Type, _Size);

                        Array.Copy(result.GetColumn(0)._VArray, vector._VArray, _Size);

                        return vector;
                    }
                }
                else
                {
                    Matrix result = Matrix.DivideRight(matrixVector, matrix);

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(_Size + 1, 1))
                    {
                        Vector vector = _GetZeroVector(_Type, _Size);

                        Array.Copy(result.GetRow(0)._VArray, vector._VArray, _Size);

                        return vector;
                    }
                }
            }

            return Empty;
        }

        /// <summary>
        /// 返回按 Matrix 对象列表表示的仿射矩阵列表将此 Vector 的副本进行逆仿射变换的新实例。
        /// </summary>
        /// <param name="matrixList">Matrix 对象列表表示的仿射矩阵列表，对于列向量应全部为左矩阵，对于行向量应全部为右矩阵。</param>
        public Vector InverseAffineTransformCopy(List<Matrix> matrixList)
        {
            if (_Size > 0 && !InternalMethod.IsNullOrEmpty(matrixList))
            {
                Matrix result = _ToMatrixForAffineTransform();

                if (_Type == Type.ColumnVector)
                {
                    for (int i = matrixList.Count - 1; i >= 0; i--)
                    {
                        Matrix matrix = matrixList[i];

                        bool flag = (!Matrix.IsNullOrEmpty(matrix) && matrix.Size == new Size(_Size + 1, _Size + 1));

                        if (flag)
                        {
                            result = Matrix.DivideLeft(matrix, result);

                            flag = (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(1, _Size + 1));
                        }

                        if (!flag)
                        {
                            return Empty;
                        }
                    }

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(1, _Size + 1))
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
                        Matrix matrix = matrixList[i];

                        bool flag = (!Matrix.IsNullOrEmpty(matrix) && matrix.Size == new Size(_Size + 1, _Size + 1));

                        if (flag)
                        {
                            result = Matrix.DivideRight(result, matrix);

                            flag = (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(_Size + 1, 1));
                        }

                        if (!flag)
                        {
                            return Empty;
                        }
                    }

                    if (!Matrix.IsNullOrEmpty(result) && result.Size == new Size(_Size + 1, 1))
                    {
                        Vector vector = _GetZeroVector(_Type, _Size);

                        Array.Copy(result.GetRow(0)._VArray, vector._VArray, _Size);

                        return vector;
                    }
                }
            }

            return Empty;
        }

        //

        /// <summary>
        /// 返回此 Vector 与指定索引的基向量之间的夹角（弧度）。
        /// </summary>
        /// <param name="index">索引。</param>
        public double AngleOfBasis(int index)
        {
            if (_Size > 0 && (index >= 0 && index < _Size))
            {
                if (IsZero)
                {
                    return 0;
                }

                return AngleFrom(_VArray[index] >= 0 ? Base(_Size, index) : Base(_Size, index).Negate);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回此 Vector 与正交于指定索引的基向量的子空间之间的夹角（弧度）。
        /// </summary>
        /// <param name="index">索引。</param>
        public double AngleOfSpace(int index)
        {
            if (_Size > 0 && (index >= 0 && index < _Size))
            {
                if (IsZero)
                {
                    return 0;
                }

                return (Math.PI / 2 - AngleOfBasis(index));
            }

            return double.NaN;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断指定的 Vector 是否为 null 或表示空向量。
        /// </summary>
        /// <param name="vector">用于判断的 Vector 对象。</param>
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
        public static bool Equals(Vector left, Vector right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right))
            {
                return false;
            }

            return left.Equals(right);
        }

        //

        /// <summary>
        /// 返回表示零向量的 Vector 的新实例。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="dimension">向量维度。</param>
        public static Vector Zero(Type type, int dimension)
        {
            if (dimension > 0)
            {
                return _GetZeroVector(type, dimension);
            }

            return Empty;
        }

        /// <summary>
        /// 返回表示零向量的 Vector 的新实例。
        /// </summary>
        /// <param name="dimension">向量维度。</param>
        public static Vector Zero(int dimension)
        {
            if (dimension > 0)
            {
                return _GetZeroVector(Type.ColumnVector, dimension);
            }

            return Empty;
        }

        /// <summary>
        /// 返回表示指定索引的基向量的 Vector 的新实例。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="dimension">向量维度。</param>
        /// <param name="index">索引。</param>
        public static Vector Base(Type type, int dimension, int index)
        {
            if (dimension > 0 && (index >= 0 && index < dimension))
            {
                Vector result = _GetZeroVector(type, dimension);

                result._VArray[index] = 1;

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回表示指定索引的基向量的 Vector 的新实例。
        /// </summary>
        /// <param name="dimension">向量维度。</param>
        /// <param name="index">索引。</param>
        public static Vector Base(int dimension, int index)
        {
            if (dimension > 0 && (index >= 0 && index < dimension))
            {
                Vector result = _GetZeroVector(Type.ColumnVector, dimension);

                result._VArray[index] = 1;

                return result;
            }

            return Empty;
        }

        //

        /// <summary>
        /// 返回表示用于平移 Vector 对象的仿射矩阵的 Matrix 的新实例，对于列向量将返回左矩阵，对于行向量将返回右矩阵。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="dimension">向量维度。</param>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public static Matrix OffsetMatrix(Type type, int dimension, double d)
        {
            if (dimension > 0)
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

            return Matrix.Empty;
        }

        /// <summary>
        /// 返回表示用于平移 Vector 对象的仿射矩阵的 Matrix 的新实例，对于列向量将返回左矩阵，对于行向量将返回右矩阵。
        /// </summary>
        /// <param name="vector">Vector 对象表示的位移。</param>
        public static Matrix OffsetMatrix(Vector vector)
        {
            if (!IsNullOrEmpty(vector))
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

            return Matrix.Empty;
        }

        /// <summary>
        /// 返回表示用于缩放 Vector 对象的仿射矩阵的 Matrix 的新实例，对于列向量将返回左矩阵，对于行向量将返回右矩阵。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="dimension">向量维度。</param>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public static Matrix ScaleMatrix(Type type, int dimension, double s)
        {
            if (dimension > 0)
            {
                Matrix result = Matrix.Identity(dimension + 1);

                for (int i = 0; i < dimension; i++)
                {
                    result[i, i] = s;
                }

                return result;
            }

            return Matrix.Empty;
        }

        /// <summary>
        /// 返回表示用于缩放 Vector 对象的仿射矩阵的 Matrix 的新实例，对于列向量将返回左矩阵，对于行向量将返回右矩阵。
        /// </summary>
        /// <param name="vector">Vector 对象表示的缩放因数。</param>
        public static Matrix ScaleMatrix(Vector vector)
        {
            if (!IsNullOrEmpty(vector))
            {
                int dimension = vector._Size;

                Matrix result = Matrix.Identity(dimension + 1);

                for (int i = 0; i < dimension; i++)
                {
                    result[i, i] = vector._VArray[i];
                }

                return result;
            }

            return Matrix.Empty;
        }

        /// <summary>
        /// 返回表示用于翻转 Vector 对象的仿射矩阵的 Matrix 的新实例，对于列向量将返回左矩阵，对于行向量将返回右矩阵。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="dimension">向量维度。</param>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        public static Matrix ReflectMatrix(Type type, int dimension, int index)
        {
            if (dimension > 0 && (index >= 0 && index < dimension))
            {
                Matrix result = Matrix.Identity(dimension + 1);

                result[index, index] = -1;

                return result;
            }

            return Matrix.Empty;
        }

        /// <summary>
        /// 返回表示用于剪切 Vector 对象的仿射矩阵的 Matrix 的新实例，对于列向量将返回左矩阵，对于行向量将返回右矩阵。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="dimension">向量维度。</param>
        /// <param name="index1">索引，用于指定与剪切方向平行且同方向的基向量。</param>
        /// <param name="index2">索引，用于指定与剪切方向垂直且共平面的基向量。</param>
        /// <param name="angle">双精度浮点数，表示 Vector 对象沿平行于索引 index1 指定的基向量且与之同方向以及垂直于 index2 指定的基向量且与之共平面的方向剪切的角度（弧度）。</param>
        public static Matrix ShearMatrix(Type type, int dimension, int index1, int index2, double angle)
        {
            if (dimension >= 2 && (index1 >= 0 && index1 < dimension) && (index2 >= 0 && index2 < dimension) && index1 != index2)
            {
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

            return Matrix.Empty;
        }

        /// <summary>
        /// 返回表示用于旋转 Vector 对象的仿射矩阵的 Matrix 的新实例，对于列向量将返回左矩阵，对于行向量将返回右矩阵。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="dimension">向量维度。</param>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示 Vector 对象绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        public static Matrix RotateMatrix(Type type, int dimension, int index1, int index2, double angle)
        {
            if (dimension >= 2 && (index1 >= 0 && index1 < dimension) && (index2 >= 0 && index2 < dimension) && index1 != index2)
            {
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

            return Matrix.Empty;
        }

        //

        /// <summary>
        /// 返回两个 Vector 对象之间的距离。
        /// </summary>
        /// <param name="left">第一个 Vector 对象。</param>
        /// <param name="right">第二个 Vector 对象。</param>
        public static double DistanceBetween(Vector left, Vector right)
        {
            if (!IsNullOrEmpty(left) && !IsNullOrEmpty(right) && left._Type == right._Type && left._Size == right._Size)
            {
                double SqrSum = 0;

                for (int i = 0; i < left._Size; i++)
                {
                    SqrSum += (left._VArray[i] - right._VArray[i]) * (left._VArray[i] - right._VArray[i]);
                }

                return Math.Sqrt(SqrSum);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回两个 Vector 对象之间的夹角（弧度）。
        /// </summary>
        /// <param name="left">第一个 Vector 对象。</param>
        /// <param name="right">第二个 Vector 对象。</param>
        public static double AngleBetween(Vector left, Vector right)
        {
            if (!IsNullOrEmpty(left) && !IsNullOrEmpty(right) && left._Type == right._Type && left._Size == right._Size)
            {
                if (left.IsZero || right.IsZero)
                {
                    return 0;
                }

                double DotProduct = 0;

                for (int i = 0; i < left._Size; i++)
                {
                    DotProduct += left._VArray[i] * right._VArray[i];
                }

                return Math.Acos(DotProduct / left.Module / right.Module);
            }

            return double.NaN;
        }

        //

        /// <summary>
        /// 返回两个 Vector 对象的数量积。
        /// </summary>
        /// <param name="left">第一个 Vector 对象。</param>
        /// <param name="right">第二个 Vector 对象。</param>
        public static double DotProduct(Vector left, Vector right)
        {
            if (!IsNullOrEmpty(left) && !IsNullOrEmpty(right) && left._Type == right._Type && left._Size == right._Size)
            {
                double SqrSum = 0;

                for (int i = 0; i < left._Size; i++)
                {
                    SqrSum += left._VArray[i] * right._VArray[i];
                }

                return SqrSum;
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回两个 Vector 对象的向量积。
        /// </summary>
        /// <param name="left">第一个 Vector 对象。</param>
        /// <param name="right">第二个 Vector 对象。</param>
        public static Vector CrossProduct(Vector left, Vector right)
        {
            if (!IsNullOrEmpty(left) && !IsNullOrEmpty(right) && left._Type == right._Type && left._Size == right._Size)
            {
                if (left._Size > 1)
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

            return Empty;
        }

        //

        /// <summary>
        /// 返回将 Vector 对象的所有分量取绝对值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        public static Vector Abs(Vector vector)
        {
            if (!IsNullOrEmpty(vector))
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = Math.Abs(vector._VArray[i]);
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回将 Vector 对象的所有分量取符号数得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        public static Vector Sign(Vector vector)
        {
            if (!IsNullOrEmpty(vector))
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = Math.Sign(vector._VArray[i]);
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回将 Vector 对象的所有分量舍入到较大的整数值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        public static Vector Ceiling(Vector vector)
        {
            if (!IsNullOrEmpty(vector))
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = Math.Ceiling(vector._VArray[i]);
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回将 Vector 对象的所有分量舍入到较小的整数值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        public static Vector Floor(Vector vector)
        {
            if (!IsNullOrEmpty(vector))
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = Math.Floor(vector._VArray[i]);
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回将 Vector 对象的所有分量舍入到最接近的整数值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        public static Vector Round(Vector vector)
        {
            if (!IsNullOrEmpty(vector))
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = Math.Round(vector._VArray[i]);
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回将 Vector 对象的所有分量截断小数部分取整得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        public static Vector Truncate(Vector vector)
        {
            if (!IsNullOrEmpty(vector))
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = Math.Truncate(vector._VArray[i]);
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回将两个 Vector 对象的所有分量分别取最大值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">用于比较的第一个 Vector 对象。</param>
        /// <param name="right">用于比较的第二个 Vector 对象。</param>
        public static Vector Max(Vector left, Vector right)
        {
            if (!IsNullOrEmpty(left) && !IsNullOrEmpty(right) && left._Type == right._Type && left._Size == right._Size)
            {
                Vector result = _GetZeroVector(left._Type, left._Size);

                for (int i = 0; i < left._Size; i++)
                {
                    result._VArray[i] = Math.Max(left._VArray[i], right._VArray[i]);
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回将两个 Vector 对象的所有分量分别取最小值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">用于比较的第一个 Vector 对象。</param>
        /// <param name="right">用于比较的第二个 Vector 对象。</param>
        public static Vector Min(Vector left, Vector right)
        {
            if (!IsNullOrEmpty(left) && !IsNullOrEmpty(right) && left._Type == right._Type && left._Size == right._Size)
            {
                Vector result = _GetZeroVector(left._Type, left._Size);

                for (int i = 0; i < left._Size; i++)
                {
                    result._VArray[i] = Math.Min(left._VArray[i], right._VArray[i]);
                }

                return result;
            }

            return Empty;
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 Vector 对象是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Vector 对象。</param>
        /// <param name="right">运算符右侧比较的 Vector 对象。</param>
        public static bool operator ==(Vector left, Vector right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Type != right._Type || left._Size != right._Size)
            {
                return false;
            }

            for (int i = 0; i < left._Size; i++)
            {
                if (left._VArray[i] != right._VArray[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 判断两个 Vector 对象是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Vector 对象。</param>
        /// <param name="right">运算符右侧比较的 Vector 对象。</param>
        public static bool operator !=(Vector left, Vector right)
        {
            if ((object)left == null && (object)right == null)
            {
                return false;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return false;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Type != right._Type || left._Size != right._Size)
            {
                return true;
            }

            for (int i = 0; i < left._Size; i++)
            {
                if (left._VArray[i] != right._VArray[i])
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断两个 Vector 对象的模平方是否前者小于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Vector 对象。</param>
        /// <param name="right">运算符右侧比较的 Vector 对象。</param>
        public static bool operator <(Vector left, Vector right)
        {
            if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Type != right._Type || left._Size != right._Size)
            {
                return false;
            }

            return (left.ModuleSquared < right.ModuleSquared);
        }

        /// <summary>
        /// 判断两个 Vector 对象的模平方是否前者大于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Vector 对象。</param>
        /// <param name="right">运算符右侧比较的 Vector 对象。</param>
        public static bool operator >(Vector left, Vector right)
        {
            if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Type != right._Type || left._Size != right._Size)
            {
                return false;
            }

            return (left.ModuleSquared > right.ModuleSquared);
        }

        /// <summary>
        /// 判断两个 Vector 对象的模平方是否前者小于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Vector 对象。</param>
        /// <param name="right">运算符右侧比较的 Vector 对象。</param>
        public static bool operator <=(Vector left, Vector right)
        {
            if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Type != right._Type || left._Size != right._Size)
            {
                return false;
            }

            return (left.ModuleSquared <= right.ModuleSquared);
        }

        /// <summary>
        /// 判断两个 Vector 对象的模平方是否前者大于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Vector 对象。</param>
        /// <param name="right">运算符右侧比较的 Vector 对象。</param>
        public static bool operator >=(Vector left, Vector right)
        {
            if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Type != right._Type || left._Size != right._Size)
            {
                return false;
            }

            return (left.ModuleSquared >= right.ModuleSquared);
        }

        //

        /// <summary>
        /// 返回在 Vector 对象的所有分量前添加正号得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        public static Vector operator +(Vector vector)
        {
            if (!IsNullOrEmpty(vector))
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = +vector._VArray[i];
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回在 Vector 对象的所有分量前添加负号得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        public static Vector operator -(Vector vector)
        {
            if (!IsNullOrEmpty(vector))
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = -vector._VArray[i];
                }

                return result;
            }

            return Empty;
        }

        //

        /// <summary>
        /// 返回将 Vector 对象的所有分量与双精度浮点数相加得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象，表示被加数。</param>
        /// <param name="n">双精度浮点数，表示加数。</param>
        public static Vector operator +(Vector vector, double n)
        {
            if (!IsNullOrEmpty(vector))
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = vector._VArray[i] + n;
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回将双精度浮点数与 Vector 对象的所有分量相加得到的 Vector 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被加数。</param>
        /// <param name="vector">Vector 对象，表示加数。</param>
        public static Vector operator +(double n, Vector vector)
        {
            if (!IsNullOrEmpty(vector))
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = n + vector._VArray[i];
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回将 Vector 对象与 Vector 对象的所有分量对应相加得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">Vector 对象，表示被加数。</param>
        /// <param name="right">Vector 对象，表示加数。</param>
        public static Vector operator +(Vector left, Vector right)
        {
            if (!IsNullOrEmpty(left) && !IsNullOrEmpty(right) && left._Type == right._Type && left._Size == right._Size)
            {
                Vector result = _GetZeroVector(left._Type, left._Size);

                for (int i = 0; i < left._Size; i++)
                {
                    result._VArray[i] = left._VArray[i] + right._VArray[i];
                }

                return result;
            }

            return Empty;
        }

        //

        /// <summary>
        /// 返回将 Vector 对象的所有分量与双精度浮点数相减得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象，表示被减数。</param>
        /// <param name="n">双精度浮点数，表示减数。</param>
        public static Vector operator -(Vector vector, double n)
        {
            if (!IsNullOrEmpty(vector))
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = vector._VArray[i] - n;
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回将双精度浮点数与 Vector 对象的所有分量相减得到的 Vector 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被减数。</param>
        /// <param name="vector">Vector 对象，表示减数。</param>
        public static Vector operator -(double n, Vector vector)
        {
            if (!IsNullOrEmpty(vector))
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = n - vector._VArray[i];
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回将 Vector 对象与 Vector 对象的所有分量对应相减得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">Vector 对象，表示被减数。</param>
        /// <param name="right">Vector 对象，表示减数。</param>
        public static Vector operator -(Vector left, Vector right)
        {
            if (!IsNullOrEmpty(left) && !IsNullOrEmpty(right) && left._Type == right._Type && left._Size == right._Size)
            {
                Vector result = _GetZeroVector(left._Type, left._Size);

                for (int i = 0; i < left._Size; i++)
                {
                    result._VArray[i] = left._VArray[i] - right._VArray[i];
                }

                return result;
            }

            return Empty;
        }

        //

        /// <summary>
        /// 返回将 Vector 对象的所有分量与双精度浮点数相乘得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象，表示被乘数。</param>
        /// <param name="n">双精度浮点数，表示乘数。</param>
        public static Vector operator *(Vector vector, double n)
        {
            if (!IsNullOrEmpty(vector))
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = vector._VArray[i] * n;
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回将双精度浮点数与 Vector 对象的所有分量相乘得到的 Vector 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被乘数。</param>
        /// <param name="vector">Vector 对象，表示乘数。</param>
        public static Vector operator *(double n, Vector vector)
        {
            if (!IsNullOrEmpty(vector))
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = n * vector._VArray[i];
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回将 Vector 对象与 Vector 对象的所有分量对应相乘得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">Vector 对象，表示被乘数。</param>
        /// <param name="right">Vector 对象，表示乘数。</param>
        public static Vector operator *(Vector left, Vector right)
        {
            if (!IsNullOrEmpty(left) && !IsNullOrEmpty(right) && left._Type == right._Type && left._Size == right._Size)
            {
                Vector result = _GetZeroVector(left._Type, left._Size);

                for (int i = 0; i < left._Size; i++)
                {
                    result._VArray[i] = left._VArray[i] * right._VArray[i];
                }

                return result;
            }

            return Empty;
        }

        //

        /// <summary>
        /// 返回将 Vector 对象的所有分量与双精度浮点数相除得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象，表示被除数。</param>
        /// <param name="n">双精度浮点数，表示除数。</param>
        public static Vector operator /(Vector vector, double n)
        {
            if (!IsNullOrEmpty(vector))
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = vector._VArray[i] / n;
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回将双精度浮点数与 Vector 对象的所有分量相除得到的 Vector 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被除数。</param>
        /// <param name="vector">Vector 对象，表示除数。</param>
        public static Vector operator /(double n, Vector vector)
        {
            if (!IsNullOrEmpty(vector))
            {
                Vector result = _GetZeroVector(vector._Type, vector._Size);

                for (int i = 0; i < vector._Size; i++)
                {
                    result._VArray[i] = n / vector._VArray[i];
                }

                return result;
            }

            return Empty;
        }

        /// <summary>
        /// 返回将 Vector 对象与 Vector 对象的所有分量对应相除得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">Vector 对象，表示被除数。</param>
        /// <param name="right">Vector 对象，表示除数。</param>
        public static Vector operator /(Vector left, Vector right)
        {
            if (!IsNullOrEmpty(left) && !IsNullOrEmpty(right) && left._Type == right._Type && left._Size == right._Size)
            {
                Vector result = _GetZeroVector(left._Type, left._Size);

                for (int i = 0; i < left._Size; i++)
                {
                    result._VArray[i] = left._VArray[i] / right._VArray[i];
                }

                return result;
            }

            return Empty;
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
                for (int i = 0; i < _Size; i++)
                {
                    _VArray[i] = default(double);
                }
            }
        }

        bool IList.Contains(object item)
        {
            if (item != null && item is double)
            {
                return Contains((double)item);
            }

            return false;
        }

        int IList.IndexOf(object item)
        {
            if (item != null && item is double)
            {
                return IndexOf((double)item);
            }

            return -1;
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
            if (_Size > 0 && (array != null && array.Rank == 1 && array.Length >= _Size))
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
            private int _Index = -1;

            internal Enumerator(Vector vector)
            {
                _Vector = vector;
            }

            object IEnumerator.Current
            {
                get
                {
                    if (_Vector._Size > 0 && (_Index >= 0 && _Index < _Vector._Size))
                    {
                        return _Vector._VArray[_Index];
                    }

                    return null;
                }
            }

            bool IEnumerator.MoveNext()
            {
                if (_Index < _Vector._Size - 1)
                {
                    _Index++;

                    return true;
                }

                return false;
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
                return _Size;
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
                for (int i = 0; i < _Size; i++)
                {
                    _VArray[i] = default(double);
                }
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
            private int _Index = -1;

            internal GenericEnumerator(Vector vector)
            {
                _Vector = vector;
            }

            void IDisposable.Dispose()
            {
                _Vector = null;
            }

            object IEnumerator.Current
            {
                get
                {
                    if (_Vector._Size > 0 && (_Index >= 0 && _Index < _Vector._Size))
                    {
                        return _Vector._VArray[_Index];
                    }

                    return null;
                }
            }

            bool IEnumerator.MoveNext()
            {
                if (_Index < _Vector._Size - 1)
                {
                    _Index++;

                    return true;
                }

                return false;
            }

            void IEnumerator.Reset()
            {
                _Index = -1;
            }

            double IEnumerator<double>.Current
            {
                get
                {
                    if (_Vector._Size > 0 && (_Index >= 0 && _Index < _Vector._Size))
                    {
                        return _Vector._VArray[_Index];
                    }

                    return double.NaN;
                }
            }
        }

        #endregion

        #endregion
    }
}