/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.AffineTransformation
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
    // 仿射变换的原子操作类型。
    internal enum AffineTransformationAtomicType
    {
        Offset = 1, // 平移变换。
        OffsetMulti = 2, // 平移变换（复合）。

        Scale = 4, // 缩放变换。
        ScaleMulti = 8, // 缩放变换（复合）。

        Reflect = 16, // 翻转变换。

        Shear = 32, // 错切变换。

        Rotate = 64, // 旋转变换。

        Matrix = 128 // 以矩阵为参数的仿射变换。
    }

    // 仿射变换的原子操作。
    internal sealed class AffineTransformationAtomic : IEquatable<AffineTransformationAtomic>
    {
        private AffineTransformationAtomicType _Type; // 此 AffineTransformationAtomic 的类型。
        private bool _IsInverse; // 此 AffineTransformationAtomic 是否表示逆变换。

        private double _Value; // 此 AffineTransformationAtomic 的数值。
        private int _Index1; // 此 AffineTransformationAtomic 的第一个基向量索引。
        private int _Index2; // 此 AffineTransformationAtomic 的第二个基向量索引。

        private Matrix _Matrix; // 此 AffineTransformationAtomic 的仿射矩阵。

        //

        // 不使用任何参数初始化 AffineTransformationAtomic 的新实例。
        private AffineTransformationAtomic()
        {
        }

        // 使用类型、表示此 AffineTransformationAtomic 是否表示逆变换的布尔值与数值初始化 AffineTransformationAtomic 的新实例。
        private AffineTransformationAtomic(AffineTransformationAtomicType type, bool isInverse, double value)
        {
            _Type = type;
            _IsInverse = isInverse;

            _Value = value;
            _Index1 = -1;
            _Index2 = -1;

            _Matrix = null;
        }

        // 使用类型、表示此 AffineTransformationAtomic 是否表示逆变换的布尔值、数值与基向量索引初始化 AffineTransformationAtomic 的新实例。
        private AffineTransformationAtomic(AffineTransformationAtomicType type, bool isInverse, double value, int index)
        {
            _Type = type;
            _IsInverse = isInverse;

            _Value = value;
            _Index1 = index;
            _Index2 = -1;

            _Matrix = null;
        }

        // 使用类型、表示此 AffineTransformationAtomic 是否表示逆变换的布尔值、数值、第一个基向量索引与第二个基向量索引初始化 AffineTransformationAtomic 的新实例。
        private AffineTransformationAtomic(AffineTransformationAtomicType type, bool isInverse, double value, int index1, int index2)
        {
            _Type = type;
            _IsInverse = isInverse;

            _Value = value;
            _Index1 = index1;
            _Index2 = index2;

            _Matrix = null;
        }

        // 使用类型、表示此 AffineTransformationAtomic 是否表示逆变换的布尔值与矩阵初始化 AffineTransformationAtomic 的新实例。
        private AffineTransformationAtomic(AffineTransformationAtomicType type, bool isInverse, Matrix matrix)
        {
            _Type = type;
            _IsInverse = isInverse;

            _Value = double.NaN;
            _Index1 = -1;
            _Index2 = -1;

            _Matrix = matrix;
        }

        //

        // 获取此 AffineTransformationAtomic 的类型。
        public AffineTransformationAtomicType Type => _Type;

        // 获取表示此 AffineTransformationAtomic 是否表示逆变换的布尔值。
        public bool IsInverse => _IsInverse;

        // 获取此 AffineTransformationAtomic 的逆变换。
        public AffineTransformationAtomic Invert => new AffineTransformationAtomic()
        {
            _Type = this._Type,
            _IsInverse = !this._IsInverse,

            _Value = this._Value,
            _Index1 = this._Index1,
            _Index2 = this._Index2,

            _Matrix = this._Matrix
        };

        //

        // 判断此 AffineTransformationAtomic 是否与指定的对象相等。
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj is null || !(obj is AffineTransformationAtomic))
            {
                return false;
            }
            else
            {
                return Equals((AffineTransformationAtomic)obj);
            }
        }

        // 返回此 AffineTransformationAtomic 的哈希代码。
        public override int GetHashCode() => base.GetHashCode();

        // 将此 AffineTransformationAtomic 转换为字符串。
        public override string ToString()
        {
            string Str = string.Empty;

            if (_IsInverse)
            {
                Str = string.Concat("Type=", _Type.ToString(), ",Inverse");
            }
            else
            {
                Str = string.Concat("Type=", _Type.ToString());
            }

            return string.Concat(base.GetType().Name, " [", Str, "]");
        }

        //

        // 判断此 AffineTransformationAtomic 是否与指定的 AffineTransformationAtomic 对象相等。
        public bool Equals(AffineTransformationAtomic atomic)
        {
            if (object.ReferenceEquals(this, atomic))
            {
                return true;
            }
            else if (atomic is null)
            {
                return false;
            }
            else
            {
                return _Type == atomic._Type && _IsInverse == atomic._IsInverse && _Value.Equals(atomic._Value) && _Index1 == atomic._Index1 && _Index2 == atomic._Index2 && Matrix.Equals(_Matrix, atomic._Matrix);
            }
        }

        //

        // 以不安全方式获取此 AffineTransformationAtomic 的数值。
        [InternalUnsafeCall(InternalUnsafeCallType.WillNotCheckState)]
        public double UnsafeGetValue() => _Value;

        // 以不安全方式获取此 AffineTransformationAtomic 的第一个基向量索引。
        [InternalUnsafeCall(InternalUnsafeCallType.WillNotCheckState)]
        public int UnsafeGetIndex1() => _Index1;

        // 以不安全方式获取此 AffineTransformationAtomic 的第二个基向量索引。
        [InternalUnsafeCall(InternalUnsafeCallType.WillNotCheckState)]
        public int UnsafeGetIndex2() => _Index2;

        // 以不安全方式获取此 AffineTransformationAtomic 的矩阵。
        [InternalUnsafeCall(InternalUnsafeCallType.WillNotCheckState | InternalUnsafeCallType.OutputAddress)]
        public Matrix UnsafeGetMatrix() => _Matrix;

        //

        // 返回将此 AffineTransformationAtomic 转换为矩阵的 Matrix 的新实例。
        public Matrix ToMatrix(Vector.Type type, int dimension)
        {
            if (_IsInverse)
            {
                switch (_Type)
                {
                    case AffineTransformationAtomicType.Offset: return Vector.OffsetMatrix(type, dimension, _Index1, -_Value);
                    case AffineTransformationAtomicType.OffsetMulti: return Vector.OffsetMatrix(type, dimension, -_Value);

                    case AffineTransformationAtomicType.Scale: return Vector.ScaleMatrix(type, dimension, _Index1, 1 / _Value);
                    case AffineTransformationAtomicType.ScaleMulti: return Vector.ScaleMatrix(type, dimension, 1 / _Value);

                    case AffineTransformationAtomicType.Reflect: return Vector.ReflectMatrix(type, dimension, _Index1);

                    case AffineTransformationAtomicType.Shear: return Vector.ShearMatrix(type, dimension, _Index1, _Index2, -_Value);

                    case AffineTransformationAtomicType.Rotate: return Vector.RotateMatrix(type, dimension, _Index1, _Index2, -_Value);

                    case AffineTransformationAtomicType.Matrix: return _Matrix?.Invert;

                    default: throw new ArithmeticException();
                }
            }
            else
            {
                switch (_Type)
                {
                    case AffineTransformationAtomicType.Offset: return Vector.OffsetMatrix(type, dimension, _Index1, _Value);
                    case AffineTransformationAtomicType.OffsetMulti: return Vector.OffsetMatrix(type, dimension, _Value);

                    case AffineTransformationAtomicType.Scale: return Vector.ScaleMatrix(type, dimension, _Index1, _Value);
                    case AffineTransformationAtomicType.ScaleMulti: return Vector.ScaleMatrix(type, dimension, _Value);

                    case AffineTransformationAtomicType.Reflect: return Vector.ReflectMatrix(type, dimension, _Index1);

                    case AffineTransformationAtomicType.Shear: return Vector.ShearMatrix(type, dimension, _Index1, _Index2, _Value);

                    case AffineTransformationAtomicType.Rotate: return Vector.RotateMatrix(type, dimension, _Index1, _Index2, _Value);

                    case AffineTransformationAtomicType.Matrix: return _Matrix?.Copy();

                    default: throw new ArithmeticException();
                }
            }
        }

        //

        // 判断两个 AffineTransformationAtomic 对象是否相等。
        public static bool Equals(AffineTransformationAtomic left, AffineTransformationAtomic right)
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

        // 返回表示平移变换的 AffineTransformationAtomic 的新实例。
        public static AffineTransformationAtomic FromOffset(int index, double d, bool isInverse = false) => new AffineTransformationAtomic(AffineTransformationAtomicType.Offset, isInverse, d, index);

        // 返回表示平移变换（复合）的 AffineTransformationAtomic 的新实例。
        public static AffineTransformationAtomic FromOffset(double d, bool isInverse = false) => new AffineTransformationAtomic(AffineTransformationAtomicType.OffsetMulti, isInverse, d);

        //

        // 返回表示缩放变换的 AffineTransformationAtomic 的新实例。
        public static AffineTransformationAtomic FromScale(int index, double s, bool isInverse = false) => new AffineTransformationAtomic(AffineTransformationAtomicType.Scale, isInverse, s, index);

        // 返回表示缩放变换（复合）的 AffineTransformationAtomic 的新实例。
        public static AffineTransformationAtomic FromScale(double s, bool isInverse = false) => new AffineTransformationAtomic(AffineTransformationAtomicType.ScaleMulti, isInverse, s);

        //

        // 返回表示翻转变换的 AffineTransformationAtomic 的新实例。
        public static AffineTransformationAtomic FromReflect(int index, bool isInverse = false) => new AffineTransformationAtomic(AffineTransformationAtomicType.Reflect, isInverse, double.NaN, index);

        //

        // 返回表示错切变换的 AffineTransformationAtomic 的新实例。
        public static AffineTransformationAtomic FromShear(int index1, int index2, double angle, bool isInverse = false) => new AffineTransformationAtomic(AffineTransformationAtomicType.Shear, isInverse, angle, index1, index2);

        //

        // 返回表示旋转变换的 AffineTransformationAtomic 的新实例。
        public static AffineTransformationAtomic FromRotate(int index1, int index2, double angle, bool isInverse = false) => new AffineTransformationAtomic(AffineTransformationAtomicType.Rotate, isInverse, angle, index1, index2);

        //

        // 返回表示以矩阵为参数的仿射变换的 AffineTransformationAtomic 的新实例。
        public static AffineTransformationAtomic FromMatrix(Matrix matrix, bool isInverse = false) => new AffineTransformationAtomic(AffineTransformationAtomicType.Matrix, isInverse, matrix?.Copy());

        //

        // 判断两个 AffineTransformation 对象是否相等。
        public static bool operator ==(AffineTransformationAtomic left, AffineTransformationAtomic right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (left is null || right is null)
            {
                return false;
            }
            else if (left._Type != right._Type || left._IsInverse != right._IsInverse)
            {
                return false;
            }
            else
            {
                switch (left._Type)
                {
                    case AffineTransformationAtomicType.Offset: return left._Index1 == right._Index1 && left._Value == right._Value;
                    case AffineTransformationAtomicType.OffsetMulti: return left._Value == right._Value;

                    case AffineTransformationAtomicType.Scale: return left._Index1 == right._Index1 && left._Value == right._Value;
                    case AffineTransformationAtomicType.ScaleMulti: return left._Value == right._Value;

                    case AffineTransformationAtomicType.Reflect: return left._Index1 == right._Index1;

                    case AffineTransformationAtomicType.Shear: return left._Index1 == right._Index1 && left._Index2 == right._Index2 && left._Value == right._Value;

                    case AffineTransformationAtomicType.Rotate: return left._Index1 == right._Index1 && left._Index2 == right._Index2 && left._Value == right._Value;

                    case AffineTransformationAtomicType.Matrix: return left._Matrix == right._Matrix;

                    default: throw new ArithmeticException();
                }
            }
        }

        // 判断两个 AffineTransformation 对象是否不相等。
        public static bool operator !=(AffineTransformationAtomic left, AffineTransformationAtomic right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return false;
            }
            else if (left is null || right is null)
            {
                return true;
            }
            else if (left._Type != right._Type || left._IsInverse != right._IsInverse)
            {
                return true;
            }
            else
            {
                switch (left._Type)
                {
                    case AffineTransformationAtomicType.Offset: return !(left._Index1 == right._Index1 && left._Value == right._Value);
                    case AffineTransformationAtomicType.OffsetMulti: return !(left._Value == right._Value);

                    case AffineTransformationAtomicType.Scale: return !(left._Index1 == right._Index1 && left._Value == right._Value);
                    case AffineTransformationAtomicType.ScaleMulti: return !(left._Value == right._Value);

                    case AffineTransformationAtomicType.Reflect: return !(left._Index1 == right._Index1);

                    case AffineTransformationAtomicType.Shear: return !(left._Index1 == right._Index1 && left._Index2 == right._Index2 && left._Value == right._Value);

                    case AffineTransformationAtomicType.Rotate: return !(left._Index1 == right._Index1 && left._Index2 == right._Index2 && left._Value == right._Value);

                    case AffineTransformationAtomicType.Matrix: return !(left._Matrix == right._Matrix);

                    default: throw new ArithmeticException();
                }
            }
        }
    }

    /// <summary>
    /// 表示一个仿射变换，或者由多个仿射变换构成的序列。
    /// </summary>
    public sealed class AffineTransformation : IEquatable<AffineTransformation>, IAffineTransformable<AffineTransformation>
    {
        #region 非公开成员

        // 以不安全方式创建 AffineTransformation 的新实例。
        [InternalUnsafeCall(InternalUnsafeCallType.InputAddress)]
        private static AffineTransformation _UnsafeCreateInstance(List<AffineTransformationAtomic> sequence)
        {
            if (sequence is null)
            {
                return Empty;
            }
            else
            {
                return new AffineTransformation()
                {
                    _Sequence = sequence
                };
            }
        }

        //

        private List<AffineTransformationAtomic> _Sequence; // 此 AffineTransformation 的原子操作序列。

        //

        // 以不安全方式获取此 AffineTransformation 的原子操作序列。
        [InternalUnsafeCall(InternalUnsafeCallType.OutputAddress)]
        internal List<AffineTransformationAtomic> UnsafeGetSequence() => _Sequence;

        //

        // 获取表示将此 AffineTransformation 压缩为单一变换的 AffineTransformationAtomic 的新实例。
        private AffineTransformationAtomic _CompressAsAtomic(Vector.Type type, int dimension)
        {
            if (_Sequence.Count <= 0)
            {
                return null;
            }
            else if (_Sequence.Count == 1)
            {
                return _Sequence[0];
            }
            else
            {
                AffineTransformationAtomic atomic = _Sequence[0];

                bool canOnlyToMatrix = false;

                if (atomic.Type == AffineTransformationAtomicType.Matrix)
                {
                    canOnlyToMatrix = true;
                }
                else
                {
                    for (int i = 1; i < _Sequence.Count; i++)
                    {
                        if (_Sequence[i].Type == AffineTransformationAtomicType.Matrix || _Sequence[i].Type != atomic.Type)
                        {
                            canOnlyToMatrix = true;

                            break;
                        }
                        else
                        {
                            switch (atomic.Type)
                            {
                                case AffineTransformationAtomicType.Offset:
                                case AffineTransformationAtomicType.Scale:
                                case AffineTransformationAtomicType.Reflect:
                                    if (_Sequence[i].UnsafeGetIndex1() != atomic.UnsafeGetIndex1())
                                    {
                                        canOnlyToMatrix = true;

                                        goto DO_COMPRESS;
                                    }
                                    break;

                                case AffineTransformationAtomicType.Shear:
                                case AffineTransformationAtomicType.Rotate:
                                    if (_Sequence[i].UnsafeGetIndex1() != atomic.UnsafeGetIndex1() || _Sequence[i].UnsafeGetIndex2() != atomic.UnsafeGetIndex2())
                                    {
                                        canOnlyToMatrix = true;

                                        goto DO_COMPRESS;
                                    }
                                    break;
                            }
                        }
                    }
                }

                DO_COMPRESS:
                if (canOnlyToMatrix)
                {
                    return AffineTransformationAtomic.FromMatrix(ToMatrix(type, dimension));
                }
                else
                {
                    switch (atomic.Type)
                    {
                        case AffineTransformationAtomicType.Offset:
                        case AffineTransformationAtomicType.OffsetMulti:
                            {
                                double value = 0;

                                for (int i = 0; i < _Sequence.Count; i++)
                                {
                                    if (_Sequence[i].IsInverse)
                                    {
                                        value -= _Sequence[i].UnsafeGetValue();
                                    }
                                    else
                                    {
                                        value += _Sequence[i].UnsafeGetValue();
                                    }
                                }

                                if (value == 0)
                                {
                                    return null;
                                }
                                else
                                {
                                    if (atomic.Type == AffineTransformationAtomicType.Offset)
                                    {
                                        return AffineTransformationAtomic.FromOffset(atomic.UnsafeGetIndex1(), value);
                                    }
                                    else
                                    {
                                        return AffineTransformationAtomic.FromOffset(value);
                                    }
                                }
                            }

                        case AffineTransformationAtomicType.Scale:
                        case AffineTransformationAtomicType.ScaleMulti:
                            {
                                double value = 1;

                                for (int i = 0; i < _Sequence.Count; i++)
                                {
                                    if (_Sequence[i].IsInverse)
                                    {
                                        value /= _Sequence[i].UnsafeGetValue();
                                    }
                                    else
                                    {
                                        value *= _Sequence[i].UnsafeGetValue();
                                    }
                                }

                                if (value == 1)
                                {
                                    return null;
                                }
                                else
                                {
                                    if (atomic.Type == AffineTransformationAtomicType.Offset)
                                    {
                                        return AffineTransformationAtomic.FromScale(atomic.UnsafeGetIndex1(), value);
                                    }
                                    else
                                    {
                                        return AffineTransformationAtomic.FromScale(value);
                                    }
                                }
                            }

                        case AffineTransformationAtomicType.Reflect:
                            if (_Sequence.Count % 2 == 0)
                            {
                                return null;
                            }
                            else
                            {
                                return AffineTransformationAtomic.FromReflect(atomic.UnsafeGetIndex1());
                            }

                        case AffineTransformationAtomicType.Shear:
                            {
                                double value = 0;

                                for (int i = 0; i < _Sequence.Count; i++)
                                {
                                    if (_Sequence[i].IsInverse)
                                    {
                                        value -= Math.Tan(_Sequence[i].UnsafeGetValue());
                                    }
                                    else
                                    {
                                        value += Math.Tan(_Sequence[i].UnsafeGetValue());
                                    }
                                }

                                if (value == 0)
                                {
                                    return null;
                                }
                                else
                                {
                                    return AffineTransformationAtomic.FromShear(atomic.UnsafeGetIndex1(), atomic.UnsafeGetIndex2(), Math.Atan(value));
                                }
                            }

                        case AffineTransformationAtomicType.Rotate:
                            {
                                double value = 0;

                                for (int i = 0; i < _Sequence.Count; i++)
                                {
                                    if (_Sequence[i].IsInverse)
                                    {
                                        value -= _Sequence[i].UnsafeGetValue();
                                    }
                                    else
                                    {
                                        value += _Sequence[i].UnsafeGetValue();
                                    }
                                }

                                if (value == 0)
                                {
                                    return null;
                                }
                                else
                                {
                                    return AffineTransformationAtomic.FromRotate(atomic.UnsafeGetIndex1(), atomic.UnsafeGetIndex2(), value);
                                }
                            }

                        default: throw new ArithmeticException();
                    }
                }
            }
        }

        #endregion

        #region 构造函数

        // 不使用任何参数初始化 AffineTransformation 的新实例。
        private AffineTransformation()
        {
        }

        // 使用 AffineTransformationAtomic 对象初始化 AffineTransformation 的新实例。
        private AffineTransformation(AffineTransformationAtomic atomic)
        {
            _Sequence = new List<AffineTransformationAtomic>();
            _Sequence.Add(atomic);
        }

        // 使用 AffineTransformationAtomic 对象枚举容器初始化 AffineTransformation 的新实例。
        private AffineTransformation(IEnumerable<AffineTransformationAtomic> atomics)
        {
            _Sequence = new List<AffineTransformationAtomic>(atomics);
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取表示此 AffineTransformation 是否为空变换的布尔值。
        /// </summary>
        public bool IsEmpty => _Sequence.Count <= 0;

        //

        /// <summary>
        /// 获取表示此 AffineTransformation 是否为单一变换的布尔值。
        /// </summary>
        public bool IsSingle => _Sequence.Count == 1;

        /// <summary>
        /// 获取表示此 AffineTransformation 是否为复合变换的布尔值。
        /// </summary>
        public bool IsMultiple => _Sequence.Count > 1;

        #endregion

        #region 静态属性

        /// <summary>
        /// 获取表示空变换的 AffineTransformation 的新实例。
        /// </summary>
        public static AffineTransformation Empty => new AffineTransformation() { _Sequence = new List<AffineTransformationAtomic>() };

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 AffineTransformation 是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>布尔值，表示此 AffineTransformation 是否与指定的对象相等。</returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj is null || !(obj is AffineTransformation))
            {
                return false;
            }
            else
            {
                return Equals((AffineTransformation)obj);
            }
        }

        /// <summary>
        /// 返回此 AffineTransformation 的哈希代码。
        /// </summary>
        /// <returns>32 位整数，表示此 AffineTransformation 的哈希代码。</returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// 将此 AffineTransformation 转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 AffineTransformation 的字符串形式。</returns>
        public override string ToString()
        {
            string Str = string.Empty;

            if (IsEmpty)
            {
                Str = "Empty";
            }
            else
            {
                Str = string.Concat("Type=", IsSingle ? "Single" : "Multiple");
            }

            return string.Concat(base.GetType().Name, " [", Str, "]");
        }

        //

        /// <summary>
        /// 判断此 AffineTransformation 是否与指定的 AffineTransformation 对象相等。
        /// </summary>
        /// <param name="affineTransformation">用于比较的 AffineTransformation 对象。</param>
        /// <returns>布尔值，表示此 AffineTransformation 是否与指定的 AffineTransformation 对象相等。</returns>
        public bool Equals(AffineTransformation affineTransformation)
        {
            if (object.ReferenceEquals(this, affineTransformation))
            {
                return true;
            }
            else if (affineTransformation is null)
            {
                return false;
            }
            else if (_Sequence.Count != affineTransformation._Sequence.Count)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < _Sequence.Count; i++)
                {
                    if (AffineTransformationAtomic.Equals(_Sequence[i], affineTransformation._Sequence[i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        //

        /// <summary>
        /// 获取此 AffineTransformation 的副本。
        /// </summary>
        /// <returns>AffineTransformation 对象，表示此 AffineTransformation 的副本。</returns>
        public AffineTransformation Copy()
        {
            if (_Sequence.Count <= 0)
            {
                return Empty;
            }
            else
            {
                return new AffineTransformation(_Sequence);
            }
        }

        //

        /// <summary>
        /// 返回将此 AffineTransformation 分割为表示一系列单一变换的 AffineTransformation 数组。
        /// </summary>
        /// <returns>AffineTransformation 数组，表示将此 AffineTransformation 分割的结果。</returns>
        public AffineTransformation[] Split()
        {
            if (_Sequence.Count <= 0)
            {
                return new AffineTransformation[0];
            }
            else
            {
                AffineTransformation[] result = new AffineTransformation[_Sequence.Count];

                for (int i = 0; i < _Sequence.Count; i++)
                {
                    result[i] = new AffineTransformation(_Sequence[i]);
                }

                return result;
            }
        }

        //

        /// <summary>
        /// 返回将此 AffineTransformation 转换为矩阵的 Matrix 的新实例。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="dimension">向量维度。</param>
        /// <returns>Matrix 对象，表示将此 AffineTransformation 转换为矩阵的结果。</returns>
        public Matrix ToMatrix(Vector.Type type, int dimension)
        {
            if (_Sequence.Count <= 0)
            {
                return Matrix.Identity(dimension + 1);
            }
            else if (_Sequence.Count == 1)
            {
                return _Sequence[0].ToMatrix(type, dimension);
            }
            else
            {
                Matrix[] matrices = new Matrix[_Sequence.Count];

                for (int i = 0; i < _Sequence.Count; i++)
                {
                    AffineTransformationAtomic atomic = _Sequence[i];

                    if (!atomic.IsInverse && atomic.Type == AffineTransformationAtomicType.Matrix)
                    {
                        matrices[i] = atomic.UnsafeGetMatrix();
                    }
                    else
                    {
                        matrices[i] = atomic.ToMatrix(type, dimension);
                    }

                    if (matrices[i] is null)
                    {
                        throw new ArgumentNullException();
                    }
                }

                if (type == Vector.Type.ColumnVector)
                {
                    return Matrix.MultiplyLeft(matrices);
                }
                else
                {
                    return Matrix.MultiplyRight(matrices);
                }
            }
        }

        //

        /// <summary>
        /// 将此 AffineTransformation 压缩为单一变换。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="dimension">向量维度。</param>
        public void Compress(Vector.Type type, int dimension)
        {
            if (_Sequence.Count > 0)
            {
                AffineTransformationAtomic atomic = _CompressAsAtomic(type, dimension);

                _Sequence = new List<AffineTransformationAtomic>();

                if (atomic != null)
                {
                    _Sequence.Add(atomic);
                }
            }
        }

        /// <summary>
        /// 获取表示将此 AffineTransformation 压缩为单一变换的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="type">向量类型。</param>
        /// <param name="dimension">向量维度。</param>
        /// <returns>AffineTransformation 对象，表示将此 AffineTransformation 压缩后的单一变换。</returns>
        public AffineTransformation CompressCopy(Vector.Type type, int dimension)
        {
            if (_Sequence.Count <= 0)
            {
                return Empty;
            }
            else
            {
                AffineTransformationAtomic atomic = _CompressAsAtomic(type, dimension);

                if (atomic is null)
                {
                    return Empty;
                }
                else
                {
                    return new AffineTransformation(atomic);
                }
            }
        }

        //

        /// <summary>
        /// 将此 AffineTransformation 进行逆变换。
        /// </summary>
        public void InverseTransform()
        {
            if (_Sequence.Count > 0)
            {
                _Sequence.Reverse();

                for (int i = 0; i < _Sequence.Count; i++)
                {
                    _Sequence[i] = _Sequence[i].Invert;
                }
            }
        }

        /// <summary>
        /// 获取表示将此 AffineTransformation 进行逆变换的 AffineTransformation 的新实例。
        /// </summary>
        /// <returns>AffineTransformation 对象，表示此 AffineTransformation 的逆变换。</returns>
        public AffineTransformation InverseTransformCopy()
        {
            if (_Sequence.Count <= 0)
            {
                return Empty;
            }
            else
            {
                List<AffineTransformationAtomic> sequence = new List<AffineTransformationAtomic>(_Sequence.Count);

                for (int i = _Sequence.Count - 1; i >= 0; i--)
                {
                    sequence.Add(_Sequence[i].Invert);
                }

                return _UnsafeCreateInstance(sequence);
            }
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的位移对指定的基向量方向的分量平移指定的量。
        /// </summary>
        /// <param name="index">索引，用于指定平移的分量所在方向的基向量。</param>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public void Offset(int index, double d) => _Sequence.Add(AffineTransformationAtomic.FromOffset(index, d));

        /// <summary>
        /// 按双精度浮点数表示的位移对所有分量平移指定的量。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        public void Offset(double d) => _Sequence.Add(AffineTransformationAtomic.FromOffset(d));

        /// <summary>
        /// 返回表示按双精度浮点数表示的位移对指定的基向量方向的分量平移指定的量的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定平移的分量所在方向的基向量。</param>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>AffineTransformation 对象，表示按双精度浮点数表示的位移对指定的基向量方向的分量平移指定的量。</returns>
        public AffineTransformation OffsetCopy(int index, double d)
        {
            AffineTransformation result = Copy();

            result._Sequence.Add(AffineTransformationAtomic.FromOffset(index, d));

            return result;
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的位移对所有分量平移指定的量的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>AffineTransformation 对象，表示按双精度浮点数表示的位移对所有分量平移指定的量。</returns>
        public AffineTransformation OffsetCopy(double d)
        {
            AffineTransformation result = Copy();

            result._Sequence.Add(AffineTransformationAtomic.FromOffset(d));

            return result;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的缩放因数对指定的基向量方向的分量缩放指定的倍数。
        /// </summary>
        /// <param name="index">索引，用于指定缩放的分量所在方向的基向量。</param>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public void Scale(int index, double s) => _Sequence.Add(AffineTransformationAtomic.FromScale(index, s));

        /// <summary>
        /// 按双精度浮点数表示的缩放因数对所有分量缩放指定的倍数。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        public void Scale(double s) => _Sequence.Add(AffineTransformationAtomic.FromScale(s));

        /// <summary>
        /// 返回表示按双精度浮点数表示的缩放因数对指定的基向量方向的分量缩放指定的倍数的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定缩放的分量所在方向的基向量。</param>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>AffineTransformation 对象，表示按双精度浮点数表示的缩放因数对指定的基向量方向的分量缩放指定的倍数。</returns>
        public AffineTransformation ScaleCopy(int index, double s)
        {
            AffineTransformation result = Copy();

            result._Sequence.Add(AffineTransformationAtomic.FromScale(index, s));

            return result;
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的缩放因数对所有分量缩放指定的倍数的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>AffineTransformation 对象，表示按双精度浮点数表示的缩放因数对所有分量缩放指定的倍数。</returns>
        public AffineTransformation ScaleCopy(double s)
        {
            AffineTransformation result = Copy();

            result._Sequence.Add(AffineTransformationAtomic.FromScale(s));

            return result;
        }

        //

        /// <summary>
        /// 对指定的基向量方向的分量翻转。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        public void Reflect(int index) => _Sequence.Add(AffineTransformationAtomic.FromReflect(index));

        /// <summary>
        /// 返回表示对指定的基向量方向的分量翻转的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        /// <returns>AffineTransformation 对象，表示对指定的基向量方向的分量翻转。</returns>
        public AffineTransformation ReflectCopy(int index)
        {
            AffineTransformation result = Copy();

            result._Sequence.Add(AffineTransformationAtomic.FromReflect(index));

            return result;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度错切指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定与错切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与错切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向错切的角度（弧度）。</param>
        public void Shear(int index1, int index2, double angle) => _Sequence.Add(AffineTransformationAtomic.FromShear(index1, index2, angle));

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度错切指定的角度的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定与错切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与错切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向错切的角度（弧度）。</param>
        /// <returns>AffineTransformation 对象，表示按双精度浮点数表示的弧度错切指定的角度。</returns>
        public AffineTransformation ShearCopy(int index1, int index2, double angle)
        {
            AffineTransformation result = Copy();

            result._Sequence.Add(AffineTransformationAtomic.FromShear(index1, index2, angle));

            return result;
        }

        //

        /// <summary>
        /// 按双精度浮点数表示的弧度旋转指定的角度。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        public void Rotate(int index1, int index2, double angle) => _Sequence.Add(AffineTransformationAtomic.FromRotate(index1, index2, angle));

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度旋转指定的角度的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        /// <returns>AffineTransformation 对象，表示按双精度浮点数表示的弧度旋转指定的角度。</returns>
        public AffineTransformation RotateCopy(int index1, int index2, double angle)
        {
            AffineTransformation result = Copy();

            result._Sequence.Add(AffineTransformationAtomic.FromRotate(index1, index2, angle));

            return result;
        }

        //

        /// <summary>
        /// 按仿射矩阵进行仿射变换。
        /// </summary>
        /// <param name="matrix">仿射矩阵，对于列向量应为左矩阵，对于行向量应为右矩阵。</param>
        public void MatrixTransform(Matrix matrix) => _Sequence.Add(AffineTransformationAtomic.FromMatrix(matrix));

        /// <summary>
        /// 返回表示按仿射矩阵进行仿射变换的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="matrix">仿射矩阵，对于列向量应为左矩阵，对于行向量应为右矩阵。</param>
        /// <returns>AffineTransformation 对象，表示按仿射矩阵进行仿射变换。</returns>
        public AffineTransformation MatrixTransformCopy(Matrix matrix)
        {
            AffineTransformation result = Copy();

            result._Sequence.Add(AffineTransformationAtomic.FromMatrix(matrix));

            return result;
        }

        //

        /// <summary>
        /// 按 AffineTransformation 对象进行仿射变换。
        /// </summary>
        /// <param name="affineTransformation">AffineTransformation 对象。</param>
        public void AffineTransform(AffineTransformation affineTransformation)
        {
            if (affineTransformation is null)
            {
                throw new ArgumentNullException();
            }

            //

            _Sequence.AddRange(affineTransformation._Sequence);
        }

        /// <summary>
        /// 返回表示按 AffineTransformation 对象进行仿射变换的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="affineTransformation">AffineTransformation 对象。</param>
        /// <returns>AffineTransformation 对象，表示按 AffineTransformation 对象进行仿射变换。</returns>
        public AffineTransformation AffineTransformCopy(AffineTransformation affineTransformation)
        {
            if (affineTransformation is null)
            {
                throw new ArgumentNullException();
            }

            //

            AffineTransformation result = Copy();

            result._Sequence.AddRange(affineTransformation._Sequence);

            return result;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断指定的 AffineTransformation 是否为 null 或表示空向量。
        /// </summary>
        /// <param name="affineTransformation">用于判断的 AffineTransformation 对象。</param>
        /// <returns>布尔值，表示指定的 AffineTransformation 是否为 null 或表示空变换。</returns>
        public static bool IsNullOrEmpty(AffineTransformation affineTransformation) => affineTransformation is null || affineTransformation._Sequence.Count <= 0;

        //

        /// <summary>
        /// 判断两个 AffineTransformation 对象是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 AffineTransformation 对象。</param>
        /// <param name="right">用于比较的第二个 AffineTransformation 对象。</param>
        /// <returns>布尔值，表示两个 AffineTransformation 对象是否相等。</returns>
        public static bool Equals(AffineTransformation left, AffineTransformation right)
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
        /// 返回将 AffineTransformation 对象数组合并为表示复合变换的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="affineTransformations">AffineTransformation 对象数组。</param>
        /// <returns>AffineTransformation 对象，表示将 AffineTransformation 对象数组合并的结果。</returns>
        public static AffineTransformation Join(params AffineTransformation[] affineTransformations)
        {
            if (affineTransformations is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (affineTransformations.Length <= 0)
            {
                return Empty;
            }
            else
            {
                int capacity = 0;

                for (int i = 0; i < affineTransformations.Length; i++)
                {
                    if (affineTransformations[i] is null)
                    {
                        throw new ArgumentNullException();
                    }
                    else
                    {
                        capacity += affineTransformations[i]._Sequence.Count;
                    }
                }

                List<AffineTransformationAtomic> sequence = new List<AffineTransformationAtomic>(capacity);

                for (int i = 0; i < affineTransformations.Length; i++)
                {
                    sequence.AddRange(affineTransformations[i]._Sequence);
                }

                return _UnsafeCreateInstance(sequence);
            }
        }

        /// <summary>
        /// 返回将 AffineTransformation 对象枚举容器合并为表示复合变换的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="affineTransformations">AffineTransformation 对象枚举容器。</param>
        /// <returns>AffineTransformation 对象，表示将 AffineTransformation 对象枚举容器合并的结果。</returns>
        public static AffineTransformation Join(IEnumerable<AffineTransformation> affineTransformations)
        {
            if (affineTransformations is null)
            {
                throw new ArgumentNullException();
            }

            //

            return Join(affineTransformations.ToArray());
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的位移对指定的基向量方向的分量平移指定的量的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定平移的分量所在方向的基向量。</param>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>AffineTransformation 对象，表示按双精度浮点数表示的位移对指定的基向量方向的分量平移指定的量。</returns>
        public static AffineTransformation FromOffset(int index, double d)
        {
            AffineTransformation result = Empty;

            result.Offset(index, d);

            return result;
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的位移对所有分量平移指定的量的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="d">双精度浮点数表示的位移。</param>
        /// <returns>AffineTransformation 对象，表示按双精度浮点数表示的位移对所有分量平移指定的量。</returns>
        public static AffineTransformation FromOffset(double d)
        {
            AffineTransformation result = Empty;

            result.Offset(d);

            return result;
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的缩放因数对指定的基向量方向的分量缩放指定的倍数的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定缩放的分量所在方向的基向量。</param>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>AffineTransformation 对象，表示按双精度浮点数表示的缩放因数对指定的基向量方向的分量缩放指定的倍数。</returns>
        public static AffineTransformation FromScale(int index, double s)
        {
            AffineTransformation result = Empty;

            result.Scale(index, s);

            return result;
        }

        /// <summary>
        /// 返回表示按双精度浮点数表示的缩放因数对所有分量缩放指定的倍数的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="s">双精度浮点数表示的缩放因数。</param>
        /// <returns>AffineTransformation 对象，表示按双精度浮点数表示的缩放因数对所有分量缩放指定的倍数。</returns>
        public static AffineTransformation FromScale(double s)
        {
            AffineTransformation result = Empty;

            result.Scale(s);

            return result;
        }

        //

        /// <summary>
        /// 返回表示对指定的基向量方向的分量翻转的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="index">索引，用于指定翻转的分量所在方向的基向量。</param>
        /// <returns>AffineTransformation 对象，表示对指定的基向量方向的分量翻转。</returns>
        public static AffineTransformation FromReflect(int index)
        {
            AffineTransformation result = Empty;

            result.Reflect(index);

            return result;
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度错切指定的角度的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定与错切方向同向的基向量。</param>
        /// <param name="index2">索引，用于指定与错切方向共面正交的基向量。</param>
        /// <param name="angle">双精度浮点数，表示沿索引 index1 指定的基向量方向且共面正交于 index2 指定的基向量方向错切的角度（弧度）。</param>
        /// <returns>AffineTransformation 对象，表示按双精度浮点数表示的弧度错切指定的角度。</returns>
        public static AffineTransformation FromShear(int index1, int index2, double angle)
        {
            AffineTransformation result = Empty;

            result.Shear(index1, index2, angle);

            return result;
        }

        //

        /// <summary>
        /// 返回表示按双精度浮点数表示的弧度旋转指定的角度的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="index1">索引，用于指定构成旋转轨迹所在平面的第一个基向量。</param>
        /// <param name="index2">索引，用于指定构成旋转轨迹所在平面的第二个基向量。</param>
        /// <param name="angle">双精度浮点数，表示绕索引 index1 与 index2 指定的基向量构成的平面的法向空间旋转的角度（弧度）（以索引 index1 指定的基向量为 0 弧度，从索引 index1 指定的基向量指向索引 index2 指定的基向量的方向为正方向）。</param>
        /// <returns>AffineTransformation 对象，表示按双精度浮点数表示的弧度旋转指定的角度。</returns>
        public static AffineTransformation FromRotate(int index1, int index2, double angle)
        {
            AffineTransformation result = Empty;

            result.Rotate(index1, index2, angle);

            return result;
        }

        //

        /// <summary>
        /// 返回表示按仿射矩阵进行仿射变换的 AffineTransformation 的新实例。
        /// </summary>
        /// <param name="matrix">仿射矩阵，对于列向量应为左矩阵，对于行向量应为右矩阵。</param>
        /// <returns>AffineTransformation 对象，表示按仿射矩阵进行仿射变换。</returns>
        public static AffineTransformation FromMatrixTransform(Matrix matrix)
        {
            AffineTransformation result = Empty;

            result.MatrixTransform(matrix);

            return result;
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 AffineTransformation 对象是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 AffineTransformation 对象。</param>
        /// <param name="right">运算符右侧比较的 AffineTransformation 对象。</param>
        /// <returns>布尔值，表示两个 AffineTransformation 对象是否相等。</returns>
        public static bool operator ==(AffineTransformation left, AffineTransformation right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Sequence.Count != right._Sequence.Count)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < left._Sequence.Count; i++)
                {
                    if (left._Sequence[i] == right._Sequence[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// 判断两个 AffineTransformation 对象是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 AffineTransformation 对象。</param>
        /// <param name="right">运算符右侧比较的 AffineTransformation 对象。</param>
        /// <returns>布尔值，表示两个 AffineTransformation 对象是否不相等。</returns>
        public static bool operator !=(AffineTransformation left, AffineTransformation right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return false;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Sequence.Count != right._Sequence.Count)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < left._Sequence.Count; i++)
                {
                    if (left._Sequence[i] == right._Sequence[i])
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        #endregion
    }
}