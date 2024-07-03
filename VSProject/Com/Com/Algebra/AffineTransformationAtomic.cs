/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.AffineTransformationAtomic
Version 20.10.27.1900

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.CompilerServices;

namespace Com
{
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
        private AffineTransformationAtomic() { }

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
            string str = string.Empty;

            if (_IsInverse)
            {
                str = $"Type={_Type},Inverse";
            }
            else
            {
                str = $"Type={_Type}";
            }

            return $"{base.GetType().Name} [{str}]";
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double UnsafeGetValue() => _Value;

        // 以不安全方式获取此 AffineTransformationAtomic 的第一个基向量索引。
        [InternalUnsafeCall(InternalUnsafeCallType.WillNotCheckState)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int UnsafeGetIndex1() => _Index1;

        // 以不安全方式获取此 AffineTransformationAtomic 的第二个基向量索引。
        [InternalUnsafeCall(InternalUnsafeCallType.WillNotCheckState)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int UnsafeGetIndex2() => _Index2;

        // 以不安全方式获取此 AffineTransformationAtomic 的矩阵。
        [InternalUnsafeCall(InternalUnsafeCallType.WillNotCheckState | InternalUnsafeCallType.OutputAddress)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix UnsafeGetMatrix() => _Matrix;

        //

        // 返回将此 AffineTransformationAtomic 转换为矩阵的 Matrix 的新实例。
        public Matrix ToMatrix(VectorType type, int dimension)
        {
            if (_IsInverse)
            {
                switch (_Type)
                {
                    case AffineTransformationAtomicType.Offset: return Vector.OffsetMatrix(type, dimension, _Index1, -_Value);
                    case AffineTransformationAtomicType.OffsetMulti: return Vector.OffsetMatrix(type, dimension, -_Value);

                    case AffineTransformationAtomicType.Scale: return Vector.ScaleMatrix(dimension, _Index1, 1 / _Value);
                    case AffineTransformationAtomicType.ScaleMulti: return Vector.ScaleMatrix(dimension, 1 / _Value);

                    case AffineTransformationAtomicType.Reflect: return Vector.ReflectMatrix(dimension, _Index1);

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

                    case AffineTransformationAtomicType.Scale: return Vector.ScaleMatrix(dimension, _Index1, _Value);
                    case AffineTransformationAtomicType.ScaleMulti: return Vector.ScaleMatrix(dimension, _Value);

                    case AffineTransformationAtomicType.Reflect: return Vector.ReflectMatrix(dimension, _Index1);

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
}