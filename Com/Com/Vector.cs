/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.Vector
Version 18.7.27.0000

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
    /// 以一组有序的双精度浮点数表示的向量。
    /// </summary>
    public sealed class Vector
    {
        #region 私有与内部成员

        private int _Size; // 此 Vector 存储的向量维度。

        private double[] _XArray = null; // 用于存储向量在各基向量方向的分量系数的数组。

        //

        private static bool _IsNullOrEmpty(Array array) // 判断数组是否为空。
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

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用双精度浮点数表示的各基向量方向的分量系数初始化 Vector 的新实例。
        /// </summary>
        /// <param name="x">各基向量方向的分量系数。</param>
        public Vector(params double[] x)
        {
            if (!_IsNullOrEmpty(x))
            {
                _Size = x.Length;
                _XArray = new double[x.Length];

                Array.Copy(x, _XArray, x.Length);
            }
            else
            {
                _Size = 0;
                _XArray = null;
            }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取表示非向量的 Vector 的实例。
        /// </summary>
        public static Vector NonVector
        {
            get
            {
                return new Vector(null);
            }
        }

        //

        /// <summary>
        /// 获取此 Vector 的维度。
        /// </summary>
        public double Dimension
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
        /// 获取或设置此 Vector 指定索引的基向量方向的分量系数。
        /// </summary>
        /// <param name="index">索引。</param>
        public double this[int index]
        {
            get
            {
                if (_Size > 0 && (index >= 0 && index < _Size))
                {
                    return _XArray[index];
                }

                return double.NaN;
            }

            set
            {
                if (_Size > 0 && (index >= 0 && index < _Size))
                {
                    _XArray[index] = value;
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
                if (_Size > 0)
                {
                    double SqrSum = 0;

                    for (int i = 0; i < _Size; i++)
                    {
                        SqrSum += _XArray[i] * _XArray[i];
                    }

                    return Math.Sqrt(SqrSum);
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
                        SqrSum += _XArray[i] * _XArray[i];
                    }

                    return SqrSum;
                }

                return double.NaN;
            }
        }

        /// <summary>
        /// 获取此 Vector 的相反向量。
        /// </summary>
        public Vector Negate
        {
            get
            {
                if (_Size > 0)
                {
                    Vector Result = NonVector;

                    Result._XArray = new double[_Size];

                    for (int i = 0; i < _Size; i++)
                    {
                        Result._XArray[i] = -_XArray[i];
                    }

                    return Result;
                }

                return NonVector;
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
                        Vector Result = NonVector;

                        Result._XArray = new double[_Size];

                        for (int i = 0; i < _Size; i++)
                        {
                            Result._XArray[i] = _XArray[i] / Mod;
                        }

                        return Result;
                    }
                }

                return NonVector;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取此 Vector 的副本。
        /// </summary>
        public Vector Copy()
        {
            if (_Size > 0)
            {
                Vector Result = new Vector(_XArray);

                return Result;
            }

            return NonVector;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的所有坐标偏移量将此 Vector 平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的所有坐标偏移量。</param>
        public void Offset(double d)
        {
            if (_Size > 0)
            {
                for (int i = 0; i < _Size; i++)
                {
                    _XArray[i] += d;
                }
            }
        }

        /// <summary>
        /// 按 Vector 对象将此 Vector 平移指定的量。
        /// </summary>
        /// <param name="vector">Vector 对象，用于平移此 Vector。</param>
        public void Offset(Vector vector)
        {
            if (_Size > 0 && !IsNullOrNonVector(vector) && _Size == vector._Size)
            {
                for (int i = 0; i < _Size; i++)
                {
                    _XArray[i] += vector._XArray[i];
                }
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标偏移量将此 Vector 的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的所有坐标偏移量。</param>
        public Vector OffsetCopy(double d)
        {
            if (_Size > 0)
            {
                Vector Result = Copy();

                for (int i = 0; i < Result._Size; i++)
                {
                    Result._XArray[i] += d;
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回按 Vector 对象将此 Vector 的副本平移指定的量的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象，用于平移此 Vector。</param>
        public Vector OffsetCopy(Vector vector)
        {
            if (_Size > 0 && !IsNullOrNonVector(vector) && _Size == vector._Size)
            {
                Vector Result = Copy();

                for (int i = 0; i < Result._Size; i++)
                {
                    Result._XArray[i] += vector._XArray[i];
                }

                return Result;
            }

            return NonVector;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的所有坐标缩放因子将此 Vector 缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的所有坐标缩放因子。</param>
        public void Scale(double s)
        {
            if (_Size > 0)
            {
                for (int i = 0; i < _Size; i++)
                {
                    _XArray[i] *= s;
                }
            }
        }

        /// <summary>
        /// 按 Vector 对象将此 Vector 缩放指定的倍数。
        /// </summary>
        /// <param name="vector">Vector 对象，用于缩放此 Vector。</param>
        public void Scale(Vector vector)
        {
            if (_Size > 0 && !IsNullOrNonVector(vector) && _Size == vector._Size)
            {
                for (int i = 0; i < _Size; i++)
                {
                    _XArray[i] *= vector._XArray[i];
                }
            }
        }

        /// <summary>
        /// 返回按双精度浮点数表示的所有坐标缩放因子将此 Vector 的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的所有坐标缩放因子。</param>
        public Vector ScaleCopy(double s)
        {
            if (_Size > 0)
            {
                Vector Result = Copy();

                for (int i = 0; i < Result._Size; i++)
                {
                    Result._XArray[i] *= s;
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回按 Vector 对象将此 Vector 的副本缩放指定的倍数的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象，用于缩放此 Vector。</param>
        public Vector ScaleCopy(Vector vector)
        {
            if (_Size > 0 && !IsNullOrNonVector(vector) && _Size == vector._Size)
            {
                Vector Result = Copy();

                for (int i = 0; i < Result._Size; i++)
                {
                    Result._XArray[i] *= vector._XArray[i];
                }

                return Result;
            }

            return NonVector;
        }

        //

        /// <summary>
        /// 返回此 Vector 与指定的 Vector 对象之间的距离。
        /// </summary>
        /// <param name="vector">Vector 对象，表示起始点。</param>
        public double DistanceFrom(Vector vector)
        {
            if (_Size > 0 && !IsNullOrNonVector(vector) && _Size == vector._Size)
            {
                double SqrSum = 0;

                for (int i = 0; i < _Size; i++)
                {
                    SqrSum += (_XArray[i] - vector._XArray[i]) * (_XArray[i] - vector._XArray[i]);
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
            if (_Size > 0 && !IsNullOrNonVector(vector) && _Size == vector._Size)
            {
                bool Flag1 = true;

                for (int i = 0; i < _Size; i++)
                {
                    if (_XArray[i] != 0)
                    {
                        Flag1 = false;

                        break;
                    }
                }

                if (Flag1)
                {
                    _XArray[0] = 1;
                }

                bool Flag2 = true;

                for (int i = 0; i < vector._Size; i++)
                {
                    if (vector._XArray[i] != 0)
                    {
                        Flag2 = false;

                        break;
                    }
                }

                if (Flag2)
                {
                    vector._XArray[0] = 1;
                }

                double DotProduct = 0;

                for (int i = 0; i < _Size; i++)
                {
                    DotProduct += _XArray[i] * vector._XArray[i];
                }

                double ModProduct = Module * vector.Module;

                return Math.Acos(DotProduct / ModProduct);
            }

            return double.NaN;
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
                return AngleFrom(_XArray[index] >= 0 ? Basis(_Size, index) : Basis(_Size, index).Negate);
            }

            return double.NaN;
        }

        /// <summary>
        /// 返回此 Vector 与正交于指定索引的基向量的子空间之间的夹角（弧度）。
        /// </summary>
        /// <param name="index">索引。</param>
        public double AngleOfSpace(int index)
        {
            return (Math.PI / 2 - AngleOfBasis(index));
        }

        //

        /// <summary>
        /// 返回将此 Vector 表示的直角坐标系坐标转换为极坐标系、球坐标系或超球坐标系坐标的新实例。
        /// </summary>
        public Vector ToSpherical()
        {
            if (_Size > 0)
            {
                Vector Result = NonVector;

                Result._XArray = new double[_Size];

                Result._XArray[0] = Module;

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

                    Result._XArray[_Size - 1] = VectorAngle2PI(_XArray[_Size - 2], _XArray[_Size - 1]);

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
                                SqrY += _XArray[j] * _XArray[j];
                            }

                            Result._XArray[i + 1] = VectorAnglePI(_XArray[i], Math.Sqrt(SqrY));
                        }
                    }
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回将此 Vector 表示的极坐标系、球坐标系或超球坐标系坐标转换为直角坐标系坐标的新实例。
        /// </summary>
        public Vector ToCartesian()
        {
            if (_Size > 0)
            {
                Vector Result = NonVector;

                Result._XArray = new double[_Size];

                if (_Size == 1)
                {
                    Result._XArray[0] = _XArray[0];
                }
                else
                {
                    Result._XArray[0] = _XArray[0] * Math.Cos(_XArray[1]);

                    if (_Size == 2)
                    {
                        Result._XArray[1] = _XArray[0] * Math.Sin(_XArray[1]);
                    }
                    else
                    {
                        Result._XArray[_Size - 1] = _XArray[0];

                        for (int i = 1; i < _Size; i++)
                        {
                            Result._XArray[_Size - 1] *= Math.Sin(_XArray[i]);
                        }

                        for (int i = 1; i < _Size - 1; i++)
                        {
                            Result._XArray[i] = _XArray[0] * Math.Cos(_XArray[i + 1]);

                            for (int j = 1; j < i + 1; j++)
                            {
                                Result._XArray[i] *= Math.Sin(_XArray[j]);
                            }
                        }
                    }
                }

                return Result;
            }

            return NonVector;
        }

        #endregion

        #region 基类方法

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
        /// 判断此 Vector 是否与指定的 Vector 对象相等。
        /// </summary>
        /// <param name="vector">用于比较的 Vector 对象。</param>
        public bool Equals(Vector vector)
        {
            if (_Size <= 0 || IsNullOrNonVector(vector) || _Size != vector._Size)
            {
                return false;
            }

            for (int i = 0; i < _Size; i++)
            {
                if (_XArray[i] != vector._XArray[i])
                {
                    return false;
                }
            }

            return true;
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
                Str = string.Concat("Dimension=", _Size);
            }
            else
            {
                Str = "NonVector";
            }

            return string.Concat(base.GetType().Name, " [", Str, "]");
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断指定的 Vector 是否为 null 或表示非向量。
        /// </summary>
        /// <param name="vector">用于判断的 Vector 对象。</param>
        public static bool IsNullOrNonVector(Vector vector)
        {
            return ((object)vector == null || vector._Size <= 0);
        }

        //

        /// <summary>
        /// 返回表示零向量的 Vector 的实例。
        /// </summary>
        /// <param name="dimension">维度。</param>
        public static Vector Zero(int dimension)
        {
            if (dimension > 0)
            {
                Vector Result = NonVector;

                Result._XArray = new double[dimension];

                for (int i = 0; i < Result._Size; i++)
                {
                    Result._XArray[i] = 0;
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回表示指定索引的基向量的 Vector 的实例。
        /// </summary>
        /// <param name="dimension">维度。</param>
        /// <param name="index">索引。</param>
        public static Vector Basis(int dimension, int index)
        {
            if (dimension > 0 && (index >= 0 && index < dimension))
            {
                Vector Result = NonVector;

                Result._XArray = new double[dimension];

                for (int i = 0; i < Result._Size; i++)
                {
                    Result._XArray[i] = 0;
                }

                Result._XArray[index] = 1;

                return Result;
            }

            return NonVector;
        }

        //

        /// <summary>
        /// 返回两个 Vector 对象之间的距离。
        /// </summary>
        /// <param name="left">第一个 Vector 对象。</param>
        /// <param name="right">第二个 Vector 对象。</param>
        public static double DistanceBetween(Vector left, Vector right)
        {
            if (!IsNullOrNonVector(left) && !IsNullOrNonVector(right) && left._Size == right._Size)
            {
                double SqrSum = 0;

                for (int i = 0; i < left._Size; i++)
                {
                    SqrSum += (left._XArray[i] - right._XArray[i]) * (left._XArray[i] - right._XArray[i]);
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
            if (!IsNullOrNonVector(left) && !IsNullOrNonVector(right) && left._Size == right._Size)
            {
                bool Flag1 = true;

                for (int i = 0; i < left._Size; i++)
                {
                    if (left._XArray[i] != 0)
                    {
                        Flag1 = false;

                        break;
                    }
                }

                if (Flag1)
                {
                    left._XArray[0] = 1;
                }

                bool Flag2 = true;

                for (int i = 0; i < right._Size; i++)
                {
                    if (right._XArray[i] != 0)
                    {
                        Flag2 = false;

                        break;
                    }
                }

                if (Flag2)
                {
                    right._XArray[0] = 1;
                }

                double DotProduct = 0;

                for (int i = 0; i < left._Size; i++)
                {
                    DotProduct += left._XArray[i] * right._XArray[i];
                }

                double ModProduct = left.Module * right.Module;

                return Math.Acos(DotProduct / ModProduct);
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
            if (!IsNullOrNonVector(left) && !IsNullOrNonVector(right) && left._Size == right._Size)
            {
                double SqrSum = 0;

                for (int i = 0; i < left._Size; i++)
                {
                    SqrSum += left._XArray[i] * right._XArray[i];
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
            if (!IsNullOrNonVector(left) && !IsNullOrNonVector(right) && left._Size == right._Size)
            {
                if (left._Size > 1)
                {
                    Vector Result = NonVector;

                    Result._XArray = new double[left._Size * (left._Size - 1) / 2];

                    int i = 0;

                    for (int j = 0; j < left._Size - 1; j++)
                    {
                        for (int k = j + 1; k < left._Size; k++)
                        {
                            Result._XArray[i] = left._XArray[j] * right._XArray[k] - left._XArray[k] * right._XArray[j];

                            i++;
                        }
                    }

                    return Result;
                }
            }

            return NonVector;
        }

        //

        /// <summary>
        /// 返回将 Vector 对象的所有分量取绝对值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        public static Vector Abs(Vector vector)
        {
            if (!IsNullOrNonVector(vector))
            {
                Vector Result = NonVector;

                Result._XArray = new double[vector._Size];

                for (int i = 0; i < vector._Size; i++)
                {
                    Result._XArray[i] = Math.Abs(vector._XArray[i]);
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回将 Vector 对象的所有分量取符号数得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        public static Vector Sign(Vector vector)
        {
            if (!IsNullOrNonVector(vector))
            {
                Vector Result = NonVector;

                Result._XArray = new double[vector._Size];

                for (int i = 0; i < vector._Size; i++)
                {
                    Result._XArray[i] = Math.Sign(vector._XArray[i]);
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回将 Vector 对象的所有分量舍入到较大的整数值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        public static Vector Ceiling(Vector vector)
        {
            if (!IsNullOrNonVector(vector))
            {
                Vector Result = NonVector;

                Result._XArray = new double[vector._Size];

                for (int i = 0; i < vector._Size; i++)
                {
                    Result._XArray[i] = Math.Ceiling(vector._XArray[i]);
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回将 Vector 对象的所有分量舍入到较小的整数值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        public static Vector Floor(Vector vector)
        {
            if (!IsNullOrNonVector(vector))
            {
                Vector Result = NonVector;

                Result._XArray = new double[vector._Size];

                for (int i = 0; i < vector._Size; i++)
                {
                    Result._XArray[i] = Math.Floor(vector._XArray[i]);
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回将 Vector 对象的所有分量舍入到最接近的整数值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        public static Vector Round(Vector vector)
        {
            if (!IsNullOrNonVector(vector))
            {
                Vector Result = NonVector;

                Result._XArray = new double[vector._Size];

                for (int i = 0; i < vector._Size; i++)
                {
                    Result._XArray[i] = Math.Round(vector._XArray[i]);
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回将 Vector 对象的所有分量截断小数部分取整得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        public static Vector Truncate(Vector vector)
        {
            if (!IsNullOrNonVector(vector))
            {
                Vector Result = NonVector;

                Result._XArray = new double[vector._Size];

                for (int i = 0; i < vector._Size; i++)
                {
                    Result._XArray[i] = Math.Truncate(vector._XArray[i]);
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回将两个 Vector 对象的所有分量分别取最大值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">用于比较的第一个 Vector 对象。</param>
        /// <param name="right">用于比较的第二个 Vector 对象。</param>
        public static Vector Max(Vector left, Vector right)
        {
            if (!IsNullOrNonVector(left) && !IsNullOrNonVector(right) && left._Size == right._Size)
            {
                Vector Result = NonVector;

                Result._XArray = new double[left._Size];

                for (int i = 0; i < left._Size; i++)
                {
                    Result._XArray[i] = Math.Max(left._XArray[i], right._XArray[i]);
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回将两个 Vector 对象的所有分量分别取最小值得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">用于比较的第一个 Vector 对象。</param>
        /// <param name="right">用于比较的第二个 Vector 对象。</param>
        public static Vector Min(Vector left, Vector right)
        {
            if (!IsNullOrNonVector(left) && !IsNullOrNonVector(right) && left._Size == right._Size)
            {
                Vector Result = NonVector;

                Result._XArray = new double[left._Size];

                for (int i = 0; i < left._Size; i++)
                {
                    Result._XArray[i] = Math.Min(left._XArray[i], right._XArray[i]);
                }

                return Result;
            }

            return NonVector;
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 返回在 Vector 对象的所有分量前添加正号得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        public static Vector operator +(Vector vector)
        {
            if (!IsNullOrNonVector(vector))
            {
                Vector Result = NonVector;

                Result._XArray = new double[vector._Size];

                for (int i = 0; i < vector._Size; i++)
                {
                    Result._XArray[i] = +vector._XArray[i];
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回在 Vector 对象的所有分量前添加负号得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">用于转换的 Vector 对象。</param>
        public static Vector operator -(Vector vector)
        {
            if (!IsNullOrNonVector(vector))
            {
                Vector Result = NonVector;

                Result._XArray = new double[vector._Size];

                for (int i = 0; i < vector._Size; i++)
                {
                    Result._XArray[i] = -vector._XArray[i];
                }

                return Result;
            }

            return NonVector;
        }

        //

        /// <summary>
        /// 返回将 Vector 对象的所有分量与双精度浮点数相加得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象，表示被加数。</param>
        /// <param name="n">双精度浮点数，表示加数。</param>
        public static Vector operator +(Vector vector, double n)
        {
            if (!IsNullOrNonVector(vector))
            {
                Vector Result = NonVector;

                Result._XArray = new double[vector._Size];

                for (int i = 0; i < vector._Size; i++)
                {
                    Result._XArray[i] = vector._XArray[i] + n;
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回将双精度浮点数与 Vector 对象的所有分量相加得到的 Vector 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被加数。</param>
        /// <param name="vector">Vector 对象，表示加数。</param>
        public static Vector operator +(double n, Vector vector)
        {
            if (!IsNullOrNonVector(vector))
            {
                Vector Result = NonVector;

                Result._XArray = new double[vector._Size];

                for (int i = 0; i < vector._Size; i++)
                {
                    Result._XArray[i] = n + vector._XArray[i];
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回将 Vector 对象与 Vector 对象的所有分量对应相加得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">Vector 对象，表示被加数。</param>
        /// <param name="right">Vector 对象，表示加数。</param>
        public static Vector operator +(Vector left, Vector right)
        {
            if (!IsNullOrNonVector(left) && !IsNullOrNonVector(right) && left._Size == right._Size)
            {
                Vector Result = NonVector;

                Result._XArray = new double[left._Size];

                for (int i = 0; i < left._Size; i++)
                {
                    Result._XArray[i] = left._XArray[i] + right._XArray[i];
                }

                return Result;
            }

            return NonVector;
        }

        //

        /// <summary>
        /// 返回将 Vector 对象的所有分量与双精度浮点数相减得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象，表示被减数。</param>
        /// <param name="n">双精度浮点数，表示减数。</param>
        public static Vector operator -(Vector vector, double n)
        {
            if (!IsNullOrNonVector(vector))
            {
                Vector Result = NonVector;

                Result._XArray = new double[vector._Size];

                for (int i = 0; i < vector._Size; i++)
                {
                    Result._XArray[i] = vector._XArray[i] - n;
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回将双精度浮点数与 Vector 对象的所有分量相减得到的 Vector 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被减数。</param>
        /// <param name="vector">Vector 对象，表示减数。</param>
        public static Vector operator -(double n, Vector vector)
        {
            if (!IsNullOrNonVector(vector))
            {
                Vector Result = NonVector;

                Result._XArray = new double[vector._Size];

                for (int i = 0; i < vector._Size; i++)
                {
                    Result._XArray[i] = n - vector._XArray[i];
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回将 Vector 对象与 Vector 对象的所有分量对应相减得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">Vector 对象，表示被减数。</param>
        /// <param name="right">Vector 对象，表示减数。</param>
        public static Vector operator -(Vector left, Vector right)
        {
            if (!IsNullOrNonVector(left) && !IsNullOrNonVector(right) && left._Size == right._Size)
            {
                Vector Result = NonVector;

                Result._XArray = new double[left._Size];

                for (int i = 0; i < left._Size; i++)
                {
                    Result._XArray[i] = left._XArray[i] - right._XArray[i];
                }

                return Result;
            }

            return NonVector;
        }

        //

        /// <summary>
        /// 返回将 Vector 对象的所有分量与双精度浮点数相乘得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象，表示被乘数。</param>
        /// <param name="n">双精度浮点数，表示乘数。</param>
        public static Vector operator *(Vector vector, double n)
        {
            if (!IsNullOrNonVector(vector))
            {
                Vector Result = NonVector;

                Result._XArray = new double[vector._Size];

                for (int i = 0; i < vector._Size; i++)
                {
                    Result._XArray[i] = vector._XArray[i] * n;
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回将双精度浮点数与 Vector 对象的所有分量相乘得到的 Vector 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被乘数。</param>
        /// <param name="vector">Vector 对象，表示乘数。</param>
        public static Vector operator *(double n, Vector vector)
        {
            if (!IsNullOrNonVector(vector))
            {
                Vector Result = NonVector;

                Result._XArray = new double[vector._Size];

                for (int i = 0; i < vector._Size; i++)
                {
                    Result._XArray[i] = n * vector._XArray[i];
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回将 Vector 对象与 Vector 对象的所有分量对应相乘得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">Vector 对象，表示被乘数。</param>
        /// <param name="right">Vector 对象，表示乘数。</param>
        public static Vector operator *(Vector left, Vector right)
        {
            if (!IsNullOrNonVector(left) && !IsNullOrNonVector(right) && left._Size == right._Size)
            {
                Vector Result = NonVector;

                Result._XArray = new double[left._Size];

                for (int i = 0; i < left._Size; i++)
                {
                    Result._XArray[i] = left._XArray[i] * right._XArray[i];
                }

                return Result;
            }

            return NonVector;
        }

        //

        /// <summary>
        /// 返回将 Vector 对象的所有分量与双精度浮点数相除得到的 Vector 的新实例。
        /// </summary>
        /// <param name="vector">Vector 对象，表示被除数。</param>
        /// <param name="n">双精度浮点数，表示除数。</param>
        public static Vector operator /(Vector vector, double n)
        {
            if (!IsNullOrNonVector(vector))
            {
                Vector Result = NonVector;

                Result._XArray = new double[vector._Size];

                for (int i = 0; i < vector._Size; i++)
                {
                    Result._XArray[i] = vector._XArray[i] / n;
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回将双精度浮点数与 Vector 对象的所有分量相除得到的 Vector 的新实例。
        /// </summary>
        /// <param name="n">双精度浮点数，表示被除数。</param>
        /// <param name="vector">Vector 对象，表示除数。</param>
        public static Vector operator /(double n, Vector vector)
        {
            if (!IsNullOrNonVector(vector))
            {
                Vector Result = NonVector;

                Result._XArray = new double[vector._Size];

                for (int i = 0; i < vector._Size; i++)
                {
                    Result._XArray[i] = n / vector._XArray[i];
                }

                return Result;
            }

            return NonVector;
        }

        /// <summary>
        /// 返回将 Vector 对象与 Vector 对象的所有分量对应相除得到的 Vector 的新实例。
        /// </summary>
        /// <param name="left">Vector 对象，表示被除数。</param>
        /// <param name="right">Vector 对象，表示除数。</param>
        public static Vector operator /(Vector left, Vector right)
        {
            if (!IsNullOrNonVector(left) && !IsNullOrNonVector(right) && left._Size == right._Size)
            {
                Vector Result = NonVector;

                Result._XArray = new double[left._Size];

                for (int i = 0; i < left._Size; i++)
                {
                    Result._XArray[i] = left._XArray[i] / right._XArray[i];
                }

                return Result;
            }

            return NonVector;
        }

        //

        /// <summary>
        /// 判断两个 Vector 对象是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 Vector 对象。</param>
        /// <param name="right">运算符右侧比较的 Vector 对象。</param>
        public static bool operator ==(Vector left, Vector right)
        {
            if (IsNullOrNonVector(left) || IsNullOrNonVector(right) || left._Size != right._Size)
            {
                return false;
            }

            for (int i = 0; i < left._Size; i++)
            {
                if (left._XArray[i] != right._XArray[i])
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
            if (IsNullOrNonVector(left) || IsNullOrNonVector(right) || left._Size != right._Size)
            {
                return true;
            }

            for (int i = 0; i < left._Size; i++)
            {
                if (left._XArray[i] != right._XArray[i])
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
            if (IsNullOrNonVector(left) || IsNullOrNonVector(right) || left._Size != right._Size)
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
            if (IsNullOrNonVector(left) || IsNullOrNonVector(right) || left._Size != right._Size)
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
            if (IsNullOrNonVector(left) || IsNullOrNonVector(right) || left._Size != right._Size)
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
            if (IsNullOrNonVector(left) || IsNullOrNonVector(right) || left._Size != right._Size)
            {
                return false;
            }

            return (left.ModuleSquared >= right.ModuleSquared);
        }

        #endregion
    }
}