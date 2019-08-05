/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.BitSet
Version 19.8.5.2000

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

namespace Com
{
    /// <summary>
    /// 以数组的形式管理位值的集合，位值 1 与 0 分别以布尔值 true 与 false 表示。
    /// </summary>
    public sealed class BitSet : IEquatable<BitSet>, IComparable, IComparable<BitSet>, IVector<bool>
    {
        #region 私有成员与内部成员

        private const int _BitsPerByte = 8; // 8 位无符号整数包含的位值数量。
        private const int _BitsPerUshort = 16; // 16 位无符号整数包含的位值数量。
        private const int _BitsPerUint = 32; // 32 位无符号整数包含的位值数量。
        private const int _BitsPerUlong = 64; // 64 位无符号整数包含的位值数量。

        private const int _BytesPerUint = _BitsPerUint / _BitsPerByte; // 32 位无符号整数包含的 8 位无符号整数数量。
        private const int _UshortsPerUint = _BitsPerUint / _BitsPerUshort; // 32 位无符号整数包含的 16 位无符号整数数量。
        private const int _UintsPerUlong = _BitsPerUlong / _BitsPerUint; // 64 位无符号整数包含的 32 位无符号整数数量。

        private const uint _TrueUint = uint.MaxValue, _FalseUint = uint.MinValue; // 所有位值为 true 或 false 的 32 位无符号整数。

        private const int _MaxSize = 2146434944; // BitSet 允许包含的最大元素数量，等于 (System.Array.MaxArrayLength / Com.BitSet._BitsPerUint / 4) * Com.BitSet._BitsPerUint * 4。

        //

        private static int _GetUintNumOfBitNum(int bitNum) // 根据位值的数量计算 32 位无符号整数的数量。
        {
            if (bitNum <= 0)
            {
                return 0;
            }
            else
            {
                return ((bitNum - 1) / _BitsPerUint + 1);
            }
        }

        private static int _GetUintArrayLengthOfBitNum(int bitNum) // 根据位值的数量计算 32 位无符号整数数组的长度。
        {
            if (bitNum <= 0)
            {
                return 0;
            }
            else
            {
                int ArrayLength = _GetUintNumOfBitNum(bitNum);

                if (ArrayLength > 2)
                {
                    ArrayLength = ((ArrayLength - 1) / 4 + 1) * 4;
                }

                return ArrayLength;
            }
        }

        //

        private int _Size; // 此 BitSet 包含的元素数量。

        private uint[] _UintArray; // 用于存储位值的 32 位无符号整数数组。

        #endregion

        #region 构造函数

        private BitSet() // 不使用任何参数初始化 BitSet 的新实例。
        {
        }

        /// <summary>
        /// 使用指定的元素数量与默认的位值（false）初始化 BitSet 的新实例。
        /// </summary>
        /// <param name="length">元素数量。</param>
        public BitSet(int length)
        {
            if (length < 0 || length > _MaxSize)
            {
                throw new OverflowException();
            }

            //

            if (length == 0)
            {
                _Size = 0;
                _UintArray = new uint[0];
            }
            else
            {
                _Size = length;
                _UintArray = new uint[_GetUintArrayLengthOfBitNum(_Size)];
            }
        }

        /// <summary>
        /// 使用指定的元素数量与位值初始化 BitSet 的新实例。
        /// </summary>
        /// <param name="length">元素数量。</param>
        /// <param name="bitValue">位值。</param>
        public BitSet(int length, bool bitValue)
        {
            if (length < 0 || length > _MaxSize)
            {
                throw new OverflowException();
            }

            //

            if (length == 0)
            {
                _Size = 0;
                _UintArray = new uint[0];
            }
            else
            {
                _Size = length;
                _UintArray = new uint[_GetUintArrayLengthOfBitNum(_Size)];

                if (bitValue)
                {
                    int Len = _GetUintNumOfBitNum(_Size);

                    for (int i = 0; i < Len; i++)
                    {
                        _UintArray[i] = _TrueUint;
                    }

                    if (_Size % _BitsPerUint != 0)
                    {
                        _UintArray[Len - 1] >>= (_BitsPerUint - _Size % _BitsPerUint);
                    }
                }
            }
        }

        /// <summary>
        /// 使用表示位值的布尔值数组初始化 BitSet 的新实例。
        /// </summary>
        /// <param name="values">表示位值的布尔值数组。</param>
        public BitSet(params bool[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                _Size = 0;
                _UintArray = new uint[0];
            }
            else
            {
                int length = values.Length;

                if (length > _MaxSize)
                {
                    throw new OverflowException();
                }

                //

                _Size = length;
                _UintArray = new uint[_GetUintArrayLengthOfBitNum(_Size)];

                for (int i = 0; i < length; i++)
                {
                    if (values[i])
                    {
                        Set(i, true);
                    }
                }
            }
        }

        /// <summary>
        /// 使用 8 位无符号整数数组初始化 BitSet 的新实例。
        /// </summary>
        /// <param name="values">8 位无符号整数数组。</param>
        public BitSet(params byte[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                _Size = 0;
                _UintArray = new uint[0];
            }
            else
            {
                int length = values.Length;

                if ((long)length * _BitsPerByte > _MaxSize)
                {
                    throw new OverflowException();
                }

                //

                _Size = length * _BitsPerByte;
                _UintArray = new uint[_GetUintArrayLengthOfBitNum(_Size)];

                for (int i = 0; i < length; i++)
                {
                    _UintArray[i / _BytesPerUint] |= (((uint)values[i]) << (_BitsPerByte * (i % _BytesPerUint)));
                }
            }
        }

        /// <summary>
        /// 使用 16 位无符号整数数组初始化 BitSet 的新实例。
        /// </summary>
        /// <param name="values">16 位无符号整数数组。</param>
        public BitSet(params ushort[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                _Size = 0;
                _UintArray = new uint[0];
            }
            else
            {
                int length = values.Length;

                if ((long)length * _BitsPerUshort > _MaxSize)
                {
                    throw new OverflowException();
                }

                //

                _Size = length * _BitsPerUshort;
                _UintArray = new uint[_GetUintArrayLengthOfBitNum(_Size)];

                for (int i = 0; i < length; i++)
                {
                    _UintArray[i / _UshortsPerUint] |= (((uint)values[i]) << (_BitsPerUshort * (i % _UshortsPerUint)));
                }
            }
        }

        /// <summary>
        /// 使用 32 位无符号整数数组初始化 BitSet 的新实例。
        /// </summary>
        /// <param name="values">32 位无符号整数数组。</param>
        public BitSet(params uint[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                _Size = 0;
                _UintArray = new uint[0];
            }
            else
            {
                int length = values.Length;

                if ((long)length * _BitsPerUint > _MaxSize)
                {
                    throw new OverflowException();
                }

                //

                _Size = length * _BitsPerUint;
                _UintArray = new uint[_GetUintArrayLengthOfBitNum(_Size)];

                int Len = _GetUintNumOfBitNum(_Size);

                Array.Copy(values, _UintArray, Len);
            }
        }

        /// <summary>
        /// 使用 64 位无符号整数数组初始化 BitSet 的新实例。
        /// </summary>
        /// <param name="values">64 位无符号整数数组。</param>
        public BitSet(params ulong[] values)
        {
            if (InternalMethod.IsNullOrEmpty(values))
            {
                _Size = 0;
                _UintArray = new uint[0];
            }
            else
            {
                int length = values.Length;

                if ((long)length * _BitsPerUlong > _MaxSize)
                {
                    throw new OverflowException();
                }

                //

                _Size = length * _BitsPerUlong;
                _UintArray = new uint[_GetUintArrayLengthOfBitNum(_Size)];

                for (int i = 0; i < length; i++)
                {
                    _UintArray[i * _UintsPerUlong] = (uint)values[i];
                    _UintArray[i * _UintsPerUlong + 1] = (uint)(values[i] >> _BitsPerUint);
                }
            }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置此 BitSet 指定索引位置的位值。
        /// </summary>
        /// <param name="index">索引。</param>
        public bool this[int index]
        {
            get
            {
                return Get(index);
            }

            set
            {
                Set(index, value);
            }
        }

        //

        /// <summary>
        /// 获取表示此 BitSet 是否不包含任何元素的布尔值。
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (_Size <= 0);
            }
        }

        /// <summary>
        /// 获取表示此 BitSet 是否只读的布尔值。
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获取表示此 BitSet 是否具有固定的大小的布尔值。
        /// </summary>
        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        //

        /// <summary>
        /// 获取或设置此 BitSet 包含的元素数量。
        /// </summary>
        public int Size
        {
            get
            {
                if (_Size < 0)
                {
                    return 0;
                }
                else
                {
                    return _Size;
                }
            }

            set
            {
                if (value < 0)
                {
                    throw new OverflowException();
                }

                //

                if (value == 0)
                {
                    _Size = 0;
                    _UintArray = new uint[0];
                }
                else
                {
                    if (_Size != value)
                    {
                        if (_Size > _MaxSize)
                        {
                            throw new OverflowException();
                        }

                        //

                        int OldSize = _Size;

                        _Size = value;

                        int Len = _GetUintNumOfBitNum(_Size);

                        int NewArrayLength = _GetUintArrayLengthOfBitNum(_Size);

                        if (NewArrayLength > _UintArray.Length || NewArrayLength < _UintArray.Length / 2 || NewArrayLength < _UintArray.Length - 256)
                        {
                            uint[] NewUintArray = new uint[NewArrayLength];

                            int LenMin = Math.Min(Len, _GetUintNumOfBitNum(OldSize));

                            Array.Copy(_UintArray, NewUintArray, LenMin);

                            _UintArray = NewUintArray;
                        }

                        if (_Size < OldSize)
                        {
                            if (_Size % _BitsPerUint != 0)
                            {
                                int Size = Len * _BitsPerUint;

                                for (int i = _Size; i < Size; i++)
                                {
                                    Set(i, false);
                                }
                            }

                            for (int i = Len; i < _UintArray.Length; i++)
                            {
                                _UintArray[i] = _FalseUint;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取此 BitSet 包含的元素数量。
        /// </summary>
        public int Count
        {
            get
            {
                return Size;
            }
        }

        /// <summary>
        /// 获取此 BitSet 在其内部数据结构不调整大小的情况下能够容纳的元素数量。
        /// </summary>
        public int Capacity
        {
            get
            {
                if (_UintArray == null)
                {
                    return 0;
                }
                else
                {
                    return (_UintArray.Length * _BitsPerUint);
                }
            }
        }

        #endregion

        #region 静态属性

        /// <summary>
        /// 获取表示不包含任何元素的 BitSet 的新实例。
        /// </summary>
        public static BitSet Empty
        {
            get
            {
                return new BitSet()
                {
                    _Size = 0,
                    _UintArray = new uint[0]
                };
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 BitSet 是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>布尔值，表示此 BitSet 是否与指定的对象相等。</returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj == null || !(obj is BitSet))
            {
                return false;
            }
            else
            {
                return Equals((BitSet)obj);
            }
        }

        /// <summary>
        /// 返回此 BitSet 的哈希代码。
        /// </summary>
        /// <returns>32 位整数，表示此 BitSet 的哈希代码。</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 BitSet 转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 BitSet 的字符串形式。</returns>
        public override string ToString()
        {
            string Str = string.Empty;

            if (_Size <= 0)
            {
                Str = "Empty";
            }
            else
            {
                Str = "Count=" + _Size;
            }

            return string.Concat(base.GetType().Name, " [", Str, "]");
        }

        //

        /// <summary>
        /// 判断此 BitSet 是否与指定的 BitSet 对象相等。
        /// </summary>
        /// <param name="bitSet">用于比较的 BitSet 对象。</param>
        /// <returns>布尔值，表示此 BitSet 是否与指定的 BitSet 对象相等。</returns>
        public bool Equals(BitSet bitSet)
        {
            if (object.ReferenceEquals(this, bitSet))
            {
                return true;
            }
            else if ((object)bitSet == null)
            {
                return false;
            }
            else if (_Size != bitSet._Size || _UintArray.Length != bitSet._UintArray.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < _UintArray.Length; i++)
                {
                    if (_UintArray[i] != bitSet._UintArray[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        //

        /// <summary>
        /// 将此 BitSet 与指定的对象进行次序比较。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>32 位整数，表示此 BitSet 与指定的对象进行次序比较的结果。</returns>
        public int CompareTo(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return 0;
            }
            else if (obj == null || !(obj is BitSet))
            {
                return 1;
            }
            else
            {
                return CompareTo((BitSet)obj);
            }
        }

        /// <summary>
        /// 将此 BitSet 与指定的 BitSet 对象进行次序比较。
        /// </summary>
        /// <param name="bitSet">用于比较的 BitSet 对象。</param>
        /// <returns>32 位整数，表示此 BitSet 与指定的 BitSet 对象进行次序比较的结果。</returns>
        public int CompareTo(BitSet bitSet)
        {
            if (object.ReferenceEquals(this, bitSet))
            {
                return 0;
            }
            else if ((object)bitSet == null)
            {
                return 1;
            }
            else
            {
                int Last1L = LastIndexOf(true), Last1R = bitSet.LastIndexOf(true);

                if (Last1L != Last1R)
                {
                    if (Last1L < Last1R)
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
                    int Len = _GetUintNumOfBitNum(Math.Min(Last1L, Last1R) + 1);

                    for (int i = Len - 1; i >= 0; i--)
                    {
                        int result = _UintArray[i].CompareTo(bitSet._UintArray[i]);

                        if (result != 0)
                        {
                            return result;
                        }
                    }

                    if (_Size == bitSet._Size)
                    {
                        return 0;
                    }
                    else
                    {
                        if (_Size < bitSet._Size)
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
            }
        }

        //

        /// <summary>
        /// 获取此 BitSet 的副本。
        /// </summary>
        /// <returns>BitSet 对象，表示此 BitSet 的副本。</returns>
        public BitSet Copy()
        {
            if (_Size <= 0)
            {
                return Empty;
            }
            else
            {
                BitSet result = new BitSet(_Size);

                int Len = _GetUintNumOfBitNum(_Size);

                Array.Copy(_UintArray, result._UintArray, Len);

                return result;
            }
        }

        //

        /// <summary>
        /// 遍历此 BitSet 的所有位值并返回第一个与指定值相等的位值的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的位值的索引。</returns>
        public int IndexOf(bool item)
        {
            if (_Size <= 0)
            {
                return -1;
            }
            else
            {
                return IndexOf(item, 0, _Size);
            }
        }

        /// <summary>
        /// 从指定的索引开始遍历此 BitSet 的所有位值并返回第一个与指定值相等的位值的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的位值的索引。</returns>
        public int IndexOf(bool item, int startIndex)
        {
            if (_Size <= 0 || (startIndex < 0 || startIndex >= _Size))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return IndexOf(item, startIndex, _Size - startIndex);
        }

        /// <summary>
        /// 从指定的索引开始遍历此 BitSet 指定数量的位值并返回第一个与指定值相等的位值的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <param name="count">遍历的位值数量。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的位值的索引。</returns>
        public int IndexOf(bool item, int startIndex, int count)
        {
            if (_Size <= 0 || (startIndex < 0 || startIndex >= _Size) || count <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            count = Math.Min(_Size - startIndex, count);

            int _Left = startIndex / _BitsPerUint, _Right = (startIndex + count - 1) / _BitsPerUint;

            if (item)
            {
                if (_Left == _Right)
                {
                    for (int j = startIndex; j < startIndex + count; j++)
                    {
                        if (Get(j))
                        {
                            return j;
                        }
                    }
                }
                else
                {
                    for (int i = _Left; i <= _Right; i++)
                    {
                        if (i == _Left)
                        {
                            for (int j = startIndex; j < (_Left + 1) * _BitsPerUint; j++)
                            {
                                if (Get(j))
                                {
                                    return j;
                                }
                            }
                        }
                        else if (i == _Right)
                        {
                            for (int j = _Right * _BitsPerUint; j < startIndex + count; j++)
                            {
                                if (Get(j))
                                {
                                    return j;
                                }
                            }
                        }
                        else if (_UintArray[i] != _FalseUint)
                        {
                            for (int j = i * _BitsPerUint; j < (i + 1) * _BitsPerUint; j++)
                            {
                                if (Get(j))
                                {
                                    return j;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (_Left == _Right)
                {
                    for (int j = startIndex; j < startIndex + count; j++)
                    {
                        if (!Get(j))
                        {
                            return j;
                        }
                    }
                }
                else
                {
                    for (int i = _Left; i <= _Right; i++)
                    {
                        if (i == _Left)
                        {
                            for (int j = startIndex; j < (_Left + 1) * _BitsPerUint; j++)
                            {
                                if (!Get(j))
                                {
                                    return j;
                                }
                            }
                        }
                        else if (i == _Right)
                        {
                            for (int j = _Right * _BitsPerUint; j < startIndex + count; j++)
                            {
                                if (!Get(j))
                                {
                                    return j;
                                }
                            }
                        }
                        else if (_UintArray[i] != _TrueUint)
                        {
                            for (int j = i * _BitsPerUint; j < (i + 1) * _BitsPerUint; j++)
                            {
                                if (!Get(j))
                                {
                                    return j;
                                }
                            }
                        }
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// 逆序遍历此 BitSet 的所有位值并返回第一个与指定值相等的位值的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的位值的索引。</returns>
        public int LastIndexOf(bool item)
        {
            if (_Size <= 0)
            {
                return -1;
            }
            else
            {
                return LastIndexOf(item, _Size - 1, _Size);
            }
        }

        /// <summary>
        /// 从指定的索引开始逆序遍历此 BitSet 的所有位值并返回第一个与指定值相等的位值的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的位值的索引。</returns>
        public int LastIndexOf(bool item, int startIndex)
        {
            if (_Size <= 0 || (startIndex < 0 || startIndex >= _Size))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return LastIndexOf(item, startIndex, startIndex + 1);
        }

        /// <summary>
        /// 从指定的索引开始逆序遍历此 BitSet 指定数量的位值并返回第一个与指定值相等的位值的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <param name="count">遍历的位值数量。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的位值的索引。</returns>
        public int LastIndexOf(bool item, int startIndex, int count)
        {
            if (_Size <= 0 || (startIndex < 0 || startIndex >= _Size) || count <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            count = Math.Min(startIndex + 1, count);

            int _Right = startIndex / _BitsPerUint, _Left = (startIndex - count + 1) / _BitsPerUint;

            if (item)
            {
                if (_Left == _Right)
                {
                    for (int j = startIndex; j > startIndex - count; j--)
                    {
                        if (Get(j))
                        {
                            return j;
                        }
                    }
                }
                else
                {
                    for (int i = _Right; i >= _Left; i--)
                    {
                        if (i == _Right)
                        {
                            for (int j = startIndex; j >= _Right * _BitsPerUint; j--)
                            {
                                if (Get(j))
                                {
                                    return j;
                                }
                            }
                        }
                        else if (i == _Left)
                        {
                            for (int j = (_Left + 1) * _BitsPerUint - 1; j > startIndex - count; j--)
                            {
                                if (Get(j))
                                {
                                    return j;
                                }
                            }
                        }
                        else if (_UintArray[i] != _FalseUint)
                        {
                            for (int j = (i + 1) * _BitsPerUint - 1; j >= i * _BitsPerUint; j--)
                            {
                                if (Get(j))
                                {
                                    return j;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (_Left == _Right)
                {
                    for (int j = startIndex; j > startIndex - count; j--)
                    {
                        if (!Get(j))
                        {
                            return j;
                        }
                    }
                }
                else
                {
                    for (int i = _Right; i >= _Left; i--)
                    {
                        if (i == _Right)
                        {
                            for (int j = startIndex; j >= _Right * _BitsPerUint; j--)
                            {
                                if (!Get(j))
                                {
                                    return j;
                                }
                            }
                        }
                        else if (i == _Left)
                        {
                            for (int j = (_Left + 1) * _BitsPerUint - 1; j > startIndex - count; j--)
                            {
                                if (!Get(j))
                                {
                                    return j;
                                }
                            }
                        }
                        else if (_UintArray[i] != _TrueUint)
                        {
                            for (int j = (i + 1) * _BitsPerUint - 1; j >= i * _BitsPerUint; j--)
                            {
                                if (!Get(j))
                                {
                                    return j;
                                }
                            }
                        }
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// 遍历此 BitSet 的所有位值并返回表示是否存在与指定值相等的位值的布尔值。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>布尔值，表示是否存在与指定值相等的位值。</returns>
        public bool Contains(bool item)
        {
            if (_Size <= 0)
            {
                return false;
            }
            else
            {
                int Len = _GetUintNumOfBitNum(_Size);

                if (item)
                {
                    for (int i = 0; i < Len; i++)
                    {
                        if (i == Len - 1)
                        {
                            for (int j = i * _BitsPerUint; j < _Size; j++)
                            {
                                if (Get(j))
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            if (_UintArray[i] != _FalseUint)
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Len; i++)
                    {
                        if (i == Len - 1)
                        {
                            for (int j = i * _BitsPerUint; j < _Size; j++)
                            {
                                if (!Get(j))
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            if (_UintArray[i] != _TrueUint)
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
        }

        //

        /// <summary>
        /// 将此 BitSet 转换为布尔值数组。
        /// </summary>
        /// <returns>布尔值数组，数组元素表示此 BitSet 的位值的布尔值形式。</returns>
        public bool[] ToArray()
        {
            if (_Size <= 0)
            {
                return new bool[0];
            }
            else
            {
                bool[] result = new bool[_Size];

                for (int i = 0; i < _Size; i++)
                {
                    result[i] = Get(i);
                }

                return result;
            }
        }

        /// <summary>
        /// 将此 BitSet 转换为布尔值列表。
        /// </summary>
        /// <returns>布尔值列表，列表元素表示此 BitSet 的位值的布尔值形式。</returns>
        public List<bool> ToList()
        {
            if (_Size <= 0)
            {
                return new List<bool>(0);
            }
            else
            {
                List<bool> result = new List<bool>(_Size);

                for (int i = 0; i < _Size; i++)
                {
                    result.Add(Get(i));
                }

                return result;
            }
        }

        //

        /// <summary>
        /// 释放此 BitSet 内部数据结构的冗余空间。
        /// </summary>
        public void Trim()
        {
            if (_Size > 0)
            {
                int NewArrayLength = _GetUintArrayLengthOfBitNum(_Size);

                if (NewArrayLength != _UintArray.Length)
                {
                    uint[] NewUintArray = new uint[NewArrayLength];

                    int Len = _GetUintNumOfBitNum(_Size);

                    Array.Copy(_UintArray, NewUintArray, Len);

                    _UintArray = NewUintArray;
                }
            }
        }

        //

        /// <summary>
        /// 获取此 BitSet 指定索引位置的位值。
        /// </summary>
        /// <param name="index">索引。</param>
        /// <returns>布尔值，表示此 BitSet 指定索引位置的位值。</returns>
        public bool Get(int index)
        {
            if (_Size <= 0 || (index < 0 || index >= _Size))
            {
                throw new IndexOutOfRangeException();
            }

            //

            return ((_UintArray[index / _BitsPerUint] & (((uint)1) << (index % _BitsPerUint))) != 0);
        }

        /// <summary>
        /// 设置此 BitSet 指定索引位置的位值。
        /// </summary>
        /// <param name="index">索引。</param>
        /// <param name="bitValue">位值。</param>
        public void Set(int index, bool bitValue)
        {
            if (_Size <= 0 || (index < 0 || index >= _Size))
            {
                throw new IndexOutOfRangeException();
            }

            //

            if (bitValue)
            {
                _UintArray[index / _BitsPerUint] |= (((uint)1) << (index % _BitsPerUint));
            }
            else
            {
                _UintArray[index / _BitsPerUint] &= (~(((uint)1) << (index % _BitsPerUint)));
            }
        }

        /// <summary>
        /// 设置此 BitSet 的所有位值。
        /// </summary>
        /// <param name="bitValue">位值。</param>
        public void SetAll(bool bitValue)
        {
            if (_Size > 0)
            {
                int Len = _GetUintNumOfBitNum(_Size);

                if (bitValue)
                {
                    for (int i = 0; i < Len; i++)
                    {
                        _UintArray[i] = _TrueUint;
                    }

                    if (_Size % _BitsPerUint != 0)
                    {
                        _UintArray[Len - 1] >>= (_BitsPerUint - _Size % _BitsPerUint);
                    }
                }
                else
                {
                    for (int i = 0; i < Len; i++)
                    {
                        _UintArray[i] = _FalseUint;
                    }
                }
            }
        }

        //

        /// <summary>
        /// 将此 BitSet 指定索引位置的位值设为 true。
        /// </summary>
        /// <param name="index">索引。</param>
        public void TrueForBit(int index)
        {
            if (_Size <= 0 || (index < 0 || index >= _Size))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            Set(index, true);
        }

        /// <summary>
        /// 将此 BitSet 指定索引位置的位值设为 false。
        /// </summary>
        /// <param name="index">索引。</param>
        public void FalseForBit(int index)
        {
            if (_Size <= 0 || (index < 0 || index >= _Size))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            Set(index, false);
        }

        /// <summary>
        /// 将此 BitSet 指定索引位置的位值取反。
        /// </summary>
        /// <param name="index">索引。</param>
        public void InverseBit(int index)
        {
            if (_Size <= 0 || (index < 0 || index >= _Size))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            _UintArray[index / _BitsPerUint] ^= (((uint)1) << (index % _BitsPerUint));
        }

        /// <summary>
        /// 将此 BitSet 的所有位值设为 true。
        /// </summary>
        public void TrueForAll()
        {
            if (_Size > 0)
            {
                SetAll(true);
            }
        }

        /// <summary>
        /// 将此 BitSet 的所有位值设为 false。
        /// </summary>
        public void FalseForAll()
        {
            if (_Size > 0)
            {
                SetAll(false);
            }
        }

        /// <summary>
        /// 将此 BitSet 的所有位值取反。
        /// </summary>
        public void InverseAll()
        {
            if (_Size > 0)
            {
                int Len = _GetUintNumOfBitNum(_Size);

                for (int i = 0; i < Len; i++)
                {
                    _UintArray[i] = (~_UintArray[i]);
                }

                if (_Size % _BitsPerUint != 0)
                {
                    int Size = Len * _BitsPerUint;

                    for (int i = _Size; i < Size; i++)
                    {
                        Set(i, false);
                    }
                }
            }
        }

        //

        /// <summary>
        /// 返回此 BitSet 值为 true 的位值的数量。
        /// </summary>
        /// <returns>32 位整数，表示此 BitSet 值为 true 的位值的数量。</returns>
        public int TrueBitCount()
        {
            if (_Size <= 0)
            {
                return 0;
            }
            else
            {
                int Count = 0;

                int Len = _GetUintNumOfBitNum(_Size);

                for (int i = 0; i < Len; i++)
                {
                    uint Bin = _UintArray[i];

                    while (Bin > 0)
                    {
                        Bin &= (Bin - 1);

                        Count++;
                    }
                }

                return Count;
            }
        }

        /// <summary>
        /// 返回此 BitSet 值为 false 的位值的数量。
        /// </summary>
        /// <returns>32 位整数，表示此 BitSet 值为 false 的位值的数量。</returns>
        public int FalseBitCount()
        {
            if (_Size <= 0)
            {
                return 0;
            }
            else
            {
                return (_Size - TrueBitCount());
            }
        }

        /// <summary>
        /// 返回包含此 BitSet 值为 true 的位值的索引的数组。
        /// </summary>
        /// <returns>32 位整数数组，该数组包含此 BitSet 值为 true 的位值的索引。</returns>
        public int[] TrueBitIndex()
        {
            if (_Size <= 0)
            {
                return new int[0];
            }
            else
            {
                List<int> result = new List<int>(_Size);

                int Len = _GetUintNumOfBitNum(_Size);

                for (int i = 0; i < Len; i++)
                {
                    uint Bin = _UintArray[i];

                    for (int j = 0; j < _BitsPerUint; j++)
                    {
                        if ((Bin & 1) != 0)
                        {
                            if (i >= Len - 1 && _Size % _BitsPerUint != 0 && j > _BitsPerUint - _Size % _BitsPerUint)
                            {
                                break;
                            }

                            result.Add(i * _BitsPerUint + j);
                        }

                        Bin >>= 1;
                    }
                }

                return result.ToArray();
            }
        }

        /// <summary>
        /// 返回包含此 BitSet 值为 false 的位值的索引的数组。
        /// </summary>
        /// <returns>32 位整数数组，该数组包含此 BitSet 值为 false 的位值的索引。</returns>
        public int[] FalseBitIndex()
        {
            if (_Size <= 0)
            {
                return new int[0];
            }
            else
            {
                List<int> result = new List<int>(_Size);

                int Len = _GetUintNumOfBitNum(_Size);

                for (int i = 0; i < Len; i++)
                {
                    uint Bin = _UintArray[i];

                    for (int j = 0; j < _BitsPerUint; j++)
                    {
                        if ((Bin & 1) == 0)
                        {
                            if (i >= Len - 1 && _Size % _BitsPerUint != 0 && j > _BitsPerUint - _Size % _BitsPerUint)
                            {
                                break;
                            }

                            result.Add(i * _BitsPerUint + j);
                        }

                        Bin >>= 1;
                    }
                }

                return result.ToArray();
            }
        }

        //

        /// <summary>
        /// 返回将此 BitSet 与指定的 BitSet 按位与得到的 BitSet 的新实例。
        /// </summary>
        /// <param name="bitSet">运算符右侧的 BitSet。</param>
        /// <returns>BitSet 对象，表示将此 BitSet 与指定的 BitSet 按位与得到的结果。</returns>
        public BitSet And(BitSet bitSet)
        {
            if (_Size <= 0 || IsNullOrEmpty(bitSet) || _Size != bitSet._Size)
            {
                return Empty;
            }
            else
            {
                BitSet result = new BitSet(_Size);

                int Len = _GetUintNumOfBitNum(_Size);

                for (int i = 0; i < Len; i++)
                {
                    result._UintArray[i] = (_UintArray[i] & bitSet._UintArray[i]);
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将此 BitSet 与指定的 BitSet 按位或得到的 BitSet 的新实例。
        /// </summary>
        /// <param name="bitSet">运算符右侧的 BitSet。</param>
        /// <returns>BitSet 对象，表示将此 BitSet 与指定的 BitSet 按位或得到的结果。</returns>
        public BitSet Or(BitSet bitSet)
        {
            if (_Size <= 0 || IsNullOrEmpty(bitSet) || _Size != bitSet._Size)
            {
                return Empty;
            }
            else
            {
                BitSet result = new BitSet(_Size);

                int Len = _GetUintNumOfBitNum(_Size);

                for (int i = 0; i < Len; i++)
                {
                    result._UintArray[i] = (_UintArray[i] | bitSet._UintArray[i]);
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将此 BitSet 与指定的 BitSet 按位异或得到的 BitSet 的新实例。
        /// </summary>
        /// <param name="bitSet">运算符右侧的 BitSet。</param>
        /// <returns>BitSet 对象，表示将此 BitSet 与指定的 BitSet 按位异或得到的结果。</returns>
        public BitSet Xor(BitSet bitSet)
        {
            if (_Size <= 0 || IsNullOrEmpty(bitSet) || _Size != bitSet._Size)
            {
                return Empty;
            }
            else
            {
                BitSet result = new BitSet(_Size);

                int Len = _GetUintNumOfBitNum(_Size);

                for (int i = 0; i < Len; i++)
                {
                    result._UintArray[i] = (_UintArray[i] ^ bitSet._UintArray[i]);
                }

                return result;
            }
        }

        /// <summary>
        /// 返回将此 BitSet 按位取反得到的 BitSet 的新实例。
        /// </summary>
        /// <returns>BitSet 对象，表示将此 BitSet 按位取反得到的结果。</returns>
        public BitSet Not()
        {
            if (_Size <= 0)
            {
                return Empty;
            }
            else
            {
                BitSet result = new BitSet(_Size);

                int Len = _GetUintNumOfBitNum(_Size);

                for (int i = 0; i < Len; i++)
                {
                    result._UintArray[i] = (~_UintArray[i]);
                }

                if (_Size % _BitsPerUint != 0)
                {
                    int Size = Len * _BitsPerUint;

                    for (int i = _Size; i < Size; i++)
                    {
                        result.Set(i, false);
                    }
                }

                return result;
            }
        }

        //

        /// <summary>
        /// 将此 BitSet 转换为二进制形式的字符串。
        /// </summary>
        /// <returns>字符串，表示此 BitSet 的二进制形式。</returns>
        public string ToBinaryString()
        {
            if (_Size <= 0)
            {
                return string.Empty;
            }
            else
            {
                char[] BitCharArray = new char[_Size];

                for (int i = 0; i < _Size; i++)
                {
                    BitCharArray[_Size - 1 - i] = (Get(i) ? '1' : '0');
                }

                return new string(BitCharArray);
            }
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断指定的 BitSet 是否为 null 或不包含任何元素。
        /// </summary>
        /// <param name="bitSet">用于判断的 BitSet 对象。</param>
        /// <returns>布尔值，表示指定的 BitSet 是否为 null 或不包含任何元素。</returns>
        public static bool IsNullOrEmpty(BitSet bitSet)
        {
            return ((object)bitSet == null || bitSet._Size <= 0);
        }

        //

        /// <summary>
        /// 判断两个 BitSet 对象是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 BitSet 对象。</param>
        /// <param name="right">用于比较的第二个 BitSet 对象。</param>
        /// <returns>布尔值，表示两个 BitSet 对象是否相等。</returns>
        public static bool Equals(BitSet left, BitSet right)
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
        /// 比较两个 BitSet 对象的次序。
        /// </summary>
        /// <param name="left">用于比较的第一个 BitSet 对象。</param>
        /// <param name="right">用于比较的第二个 BitSet 对象。</param>
        /// <returns>32 位整数，表示对两个 BitSet 对象进行次序比较的结果。</returns>
        public static int Compare(BitSet left, BitSet right)
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

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 BitSet 对象表示的整数是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 BitSet 对象。</param>
        /// <param name="right">运算符右侧比较的 BitSet 对象。</param>
        /// <returns>布尔值，表示两个 BitSet 对象表示的整数是否相等。</returns>
        public static bool operator ==(BitSet left, BitSet right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right))
            {
                return false;
            }
            else
            {
                int Last1L = left.LastIndexOf(true), Last1R = right.LastIndexOf(true);

                if (Last1L != Last1R)
                {
                    return false;
                }
                else
                {
                    int Len = _GetUintNumOfBitNum(Math.Min(Last1L, Last1R) + 1);

                    for (int i = 0; i < Len; i++)
                    {
                        if (left._UintArray[i] != right._UintArray[i])
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }
        }

        /// <summary>
        /// 判断两个 BitSet 对象表示的整数是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 BitSet 对象。</param>
        /// <param name="right">运算符右侧比较的 BitSet 对象。</param>
        /// <returns>布尔值，表示两个 BitSet 对象表示的整数是否不相等。</returns>
        public static bool operator !=(BitSet left, BitSet right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return false;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right))
            {
                return true;
            }
            else
            {
                int Last1L = left.LastIndexOf(true), Last1R = right.LastIndexOf(true);

                if (Last1L != Last1R)
                {
                    return true;
                }
                else
                {
                    int Len = _GetUintNumOfBitNum(Math.Min(Last1L, Last1R) + 1);

                    for (int i = 0; i < Len; i++)
                    {
                        if (left._UintArray[i] != right._UintArray[i])
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
        }

        /// <summary>
        /// 判断两个 BitSet 对象表示的整数是否前者小于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 BitSet 对象。</param>
        /// <param name="right">运算符右侧比较的 BitSet 对象。</param>
        /// <returns>布尔值，表示两个 BitSet 对象表示的整数是否前者小于后者。</returns>
        public static bool operator <(BitSet left, BitSet right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return false;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right))
            {
                return false;
            }
            else
            {
                int Last1L = left.LastIndexOf(true), Last1R = right.LastIndexOf(true);

                if (Last1L != Last1R)
                {
                    return (Last1L < Last1R);
                }
                else
                {
                    int Len = _GetUintNumOfBitNum(Math.Min(Last1L, Last1R) + 1);

                    for (int i = Len - 1; i >= 0; i--)
                    {
                        if (left._UintArray[i] != right._UintArray[i])
                        {
                            return (left._UintArray[i] < right._UintArray[i]);
                        }
                    }

                    return false;
                }
            }
        }

        /// <summary>
        /// 判断两个 BitSet 对象表示的整数是否前者大于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 BitSet 对象。</param>
        /// <param name="right">运算符右侧比较的 BitSet 对象。</param>
        /// <returns>布尔值，表示两个 BitSet 对象表示的整数是否前者大于后者。</returns>
        public static bool operator >(BitSet left, BitSet right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return false;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right))
            {
                return false;
            }
            else
            {
                int Last1L = left.LastIndexOf(true), Last1R = right.LastIndexOf(true);

                if (Last1L != Last1R)
                {
                    return (Last1L > Last1R);
                }
                else
                {
                    int Len = _GetUintNumOfBitNum(Math.Min(Last1L, Last1R) + 1);

                    for (int i = Len - 1; i >= 0; i--)
                    {
                        if (left._UintArray[i] != right._UintArray[i])
                        {
                            return (left._UintArray[i] > right._UintArray[i]);
                        }
                    }

                    return false;
                }
            }
        }

        /// <summary>
        /// 判断两个 BitSet 对象表示的整数是否前者小于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 BitSet 对象。</param>
        /// <param name="right">运算符右侧比较的 BitSet 对象。</param>
        /// <returns>布尔值，表示两个 BitSet 对象表示的整数是否前者小于或等于后者。</returns>
        public static bool operator <=(BitSet left, BitSet right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right))
            {
                return false;
            }
            else
            {
                int Last1L = left.LastIndexOf(true), Last1R = right.LastIndexOf(true);

                if (Last1L != Last1R)
                {
                    return (Last1L < Last1R);
                }
                else
                {
                    int Len = _GetUintNumOfBitNum(Math.Min(Last1L, Last1R) + 1);

                    for (int i = Len - 1; i >= 0; i--)
                    {
                        if (left._UintArray[i] != right._UintArray[i])
                        {
                            return (left._UintArray[i] < right._UintArray[i]);
                        }
                    }

                    return true;
                }
            }
        }

        /// <summary>
        /// 判断两个 BitSet 对象表示的整数是否前者大于或等于后者。
        /// </summary>
        /// <param name="left">运算符左侧比较的 BitSet 对象。</param>
        /// <param name="right">运算符右侧比较的 BitSet 对象。</param>
        /// <returns>布尔值，表示两个 BitSet 对象表示的整数是否前者大于或等于后者。</returns>
        public static bool operator >=(BitSet left, BitSet right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (IsNullOrEmpty(left) || IsNullOrEmpty(right))
            {
                return false;
            }
            else
            {
                int Last1L = left.LastIndexOf(true), Last1R = right.LastIndexOf(true);

                if (Last1L != Last1R)
                {
                    return (Last1L > Last1R);
                }
                else
                {
                    int Len = _GetUintNumOfBitNum(Math.Min(Last1L, Last1R) + 1);

                    for (int i = Len - 1; i >= 0; i--)
                    {
                        if (left._UintArray[i] != right._UintArray[i])
                        {
                            return (left._UintArray[i] > right._UintArray[i]);
                        }
                    }

                    return true;
                }
            }
        }

        //

        /// <summary>
        /// 返回将 BitSet 对象与 BitSet 对象按位与得到的 BitSet 的新实例。
        /// </summary>
        /// <param name="left">运算符左侧的 BitSet 对象。</param>
        /// <param name="right">运算符右侧的 BitSet 对象。</param>
        /// <returns>BitSet 对象，表示将 BitSet 对象与 BitSet 对象按位与得到的结果。</returns>
        public static BitSet operator &(BitSet left, BitSet right)
        {
            if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Size != right._Size)
            {
                return Empty;
            }
            else
            {
                return left.And(right);
            }
        }

        /// <summary>
        /// 返回将 BitSet 对象与 BitSet 对象按位或得到的 BitSet 的新实例。
        /// </summary>
        /// <param name="left">运算符左侧的 BitSet 对象。</param>
        /// <param name="right">运算符右侧的 BitSet 对象。</param>
        /// <returns>BitSet 对象，表示将 BitSet 对象与 BitSet 对象按位或得到的结果。</returns>
        public static BitSet operator |(BitSet left, BitSet right)
        {
            if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Size != right._Size)
            {
                return Empty;
            }
            else
            {
                return left.Or(right);
            }
        }

        /// <summary>
        /// 返回将 BitSet 对象与 BitSet 对象按位异或得到的 BitSet 的新实例。
        /// </summary>
        /// <param name="left">运算符左侧的 BitSet 对象。</param>
        /// <param name="right">运算符右侧的 BitSet 对象。</param>
        /// <returns>BitSet 对象，表示将 BitSet 对象与 BitSet 对象按位异或得到的结果。</returns>
        public static BitSet operator ^(BitSet left, BitSet right)
        {
            if (IsNullOrEmpty(left) || IsNullOrEmpty(right) || left._Size != right._Size)
            {
                return Empty;
            }
            else
            {
                return left.Xor(right);
            }
        }

        /// <summary>
        /// 返回将 BitSet 对象按位取反得到的 BitSet 的新实例。
        /// </summary>
        /// <param name="bitSet">运算符右侧的 BitSet 对象。</param>
        /// <returns>BitSet 对象，表示将 BitSet 对象按位取反得到的结果。</returns>
        public static BitSet operator ~(BitSet bitSet)
        {
            if (IsNullOrEmpty(bitSet))
            {
                return Empty;
            }
            else
            {
                return bitSet.Not();
            }
        }

        #endregion

        #region 显式接口成员实现

        #region System.Collections.IList

        object IList.this[int index]
        {
            get
            {
                return Get(index);
            }

            set
            {
                if (value == null || !(value is bool))
                {
                    throw new ArgumentNullException();
                }

                //

                Set(index, (bool)value);
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
                _Size = 0;
                _UintArray = new uint[0];
            }
        }

        bool IList.Contains(object item)
        {
            if (item == null || !(item is bool))
            {
                return false;
            }
            else
            {
                return Contains((bool)item);
            }
        }

        int IList.IndexOf(object item)
        {
            if (item == null || !(item is bool))
            {
                return -1;
            }
            else
            {
                return IndexOf((bool)item);
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
                return Size;
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
            throw new NotSupportedException();
        }

        #endregion

        #region System.Collections.IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        private sealed class Enumerator : IEnumerator // 实现 System.Collections.IEnumerator 的迭代器。
        {
            private BitSet _BitSet;
            private int _Index;

            internal Enumerator(BitSet bitSet)
            {
                _BitSet = bitSet;
                _Index = -1;
            }

            object IEnumerator.Current
            {
                get
                {
                    if (IsNullOrEmpty(_BitSet) || (_Index < 0 || _Index >= _BitSet._Size))
                    {
                        throw new IndexOutOfRangeException();
                    }

                    //

                    return _BitSet.Get(_Index);
                }
            }

            bool IEnumerator.MoveNext()
            {
                if (IsNullOrEmpty(_BitSet) || _Index >= _BitSet._Size - 1)
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

        void IList<bool>.Insert(int index, bool item)
        {
            throw new NotSupportedException();
        }

        void IList<bool>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region System.Collections.Generic.ICollection<T>

        int ICollection<bool>.Count
        {
            get
            {
                return Size;
            }
        }

        void ICollection<bool>.Add(bool item)
        {
            throw new NotSupportedException();
        }

        void ICollection<bool>.Clear()
        {
            if (_Size > 0)
            {
                _Size = 0;
                _UintArray = new uint[0];
            }
        }

        void ICollection<bool>.CopyTo(bool[] array, int index)
        {
            throw new NotSupportedException();
        }

        bool ICollection<bool>.Remove(bool item)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region System.Collections.Generic.IEnumerable<out T>

        IEnumerator<bool> IEnumerable<bool>.GetEnumerator()
        {
            return new GenericEnumerator(this);
        }

        private sealed class GenericEnumerator : IEnumerator<bool> // 实现 System.Collections.Generic.IEnumerator<out T> 的迭代器。
        {
            private BitSet _BitSet;
            private int _Index;

            internal GenericEnumerator(BitSet bitSet)
            {
                _BitSet = bitSet;
                _Index = -1;
            }

            void IDisposable.Dispose()
            {
                _BitSet = null;
            }

            object IEnumerator.Current
            {
                get
                {
                    if (IsNullOrEmpty(_BitSet) || (_Index < 0 || _Index >= _BitSet._Size))
                    {
                        throw new IndexOutOfRangeException();
                    }

                    //

                    return _BitSet.Get(_Index);
                }
            }

            bool IEnumerator.MoveNext()
            {
                if (IsNullOrEmpty(_BitSet) || _Index >= _BitSet._Size - 1)
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

            bool IEnumerator<bool>.Current
            {
                get
                {
                    if (IsNullOrEmpty(_BitSet) || (_Index < 0 || _Index >= _BitSet._Size))
                    {
                        throw new IndexOutOfRangeException();
                    }

                    //

                    return _BitSet.Get(_Index);
                }
            }
        }

        #endregion

        #endregion
    }
}