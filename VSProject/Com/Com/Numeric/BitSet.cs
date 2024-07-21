/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.BitSet
Version 24.7.21.1040

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Runtime.CompilerServices;

namespace Com
{
    /// <summary>
    /// 以数组的形式管理位值的集合，位值 1 与 0 分别以布尔值 true 与 false 表示。
    /// </summary>
    public sealed class BitSet : IEquatable<BitSet>, IComparable, IComparable<BitSet>, IVector<bool>
    {
        #region 非公开成员

        private const int _BitsPerByte = 8; // 8 位无符号整数包含的位值数量。
        private const int _BitsPerUshort = 16; // 16 位无符号整数包含的位值数量。
        private const int _BitsPerUint = 32; // 32 位无符号整数包含的位值数量。
        private const int _BitsPerUlong = 64; // 64 位无符号整数包含的位值数量。

        private const int _BytesPerUint = _BitsPerUint / _BitsPerByte; // 32 位无符号整数包含的 8 位无符号整数数量。
        private const int _UshortsPerUint = _BitsPerUint / _BitsPerUshort; // 32 位无符号整数包含的 16 位无符号整数数量。
        private const int _UintsPerUlong = _BitsPerUlong / _BitsPerUint; // 64 位无符号整数包含的 32 位无符号整数数量。

        private const uint _TrueUint = uint.MaxValue, _FalseUint = uint.MinValue; // 所有位值为 true 或 false 的 32 位无符号整数。

        private const int _MaxSize = 2146434944; // BitSet 允许包含的最大元素数量，等于 Floor(System.Array.MaxArrayLength / Com.BitSet._BitsPerUint / 4) * Com.BitSet._BitsPerUint * 4。

        private static readonly uint[] _EmptyData = Array.Empty<uint>(); // 表示 Empty 对象的 32 位无符号整数数组。

        //

        // 根据位值的数量计算 32 位无符号整数的数量。
        private static int _GetUintNumOfBitNum(int bitNum)
        {
            if (bitNum <= 0)
            {
                return 0;
            }
            else
            {
                return (bitNum - 1) / _BitsPerUint + 1;
            }
        }

        // 根据位值的数量计算 32 位无符号整数数组的长度。
        private static int _GetUintArrayLengthOfBitNum(int bitNum)
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

        //

        // 获取元素，不做索引越界检查。
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool _GetItemWithoutCheckBounds(int index) => (_UintArray[index / _BitsPerUint] & (((uint)1) << (index % _BitsPerUint))) != 0;

        // 设置元素，不做索引越界检查。
        private void _SetItemWithoutCheckBounds(int index, bool bitValue)
        {
            if (bitValue)
            {
                _UintArray[index / _BitsPerUint] |= ((uint)1) << (index % _BitsPerUint);
            }
            else
            {
                _UintArray[index / _BitsPerUint] &= ~(((uint)1) << (index % _BitsPerUint));
            }
        }

        #endregion

        #region 构造函数

        // 不使用任何参数初始化 BitSet 的新实例。
        private BitSet() { }

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
                _UintArray = _EmptyData;
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
                _UintArray = _EmptyData;
            }
            else
            {
                _Size = length;
                _UintArray = new uint[_GetUintArrayLengthOfBitNum(_Size)];

                if (bitValue)
                {
                    int len = _GetUintNumOfBitNum(_Size);

                    for (int i = 0; i < len; i++)
                    {
                        _UintArray[i] = _TrueUint;
                    }

                    if (_Size % _BitsPerUint != 0)
                    {
                        _UintArray[len - 1] >>= _BitsPerUint - _Size % _BitsPerUint;
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
                _UintArray = _EmptyData;
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
                _UintArray = _EmptyData;
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
                    _UintArray[i / _BytesPerUint] |= ((uint)values[i]) << (_BitsPerByte * (i % _BytesPerUint));
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
                _UintArray = _EmptyData;
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
                    _UintArray[i / _UshortsPerUint] |= ((uint)values[i]) << (_BitsPerUshort * (i % _UshortsPerUint));
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
                _UintArray = _EmptyData;
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

                int len = _GetUintNumOfBitNum(_Size);

                Array.Copy(values, _UintArray, len);
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
                _UintArray = _EmptyData;
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
                if (index < 0 || index >= _Size)
                {
                    throw new IndexOutOfRangeException(nameof(index));
                }

                //

                return _GetItemWithoutCheckBounds(index);
            }

            set
            {
                if (index < 0 || index >= _Size)
                {
                    throw new IndexOutOfRangeException(nameof(index));
                }

                //

                _SetItemWithoutCheckBounds(index, value);
            }
        }

        //

        /// <summary>
        /// 获取表示此 BitSet 是否不包含任何元素的布尔值。
        /// </summary>
        public bool IsEmpty => _Size <= 0;

        /// <summary>
        /// 获取表示此 BitSet 是否只读的布尔值。
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// 获取表示此 BitSet 是否具有固定的大小的布尔值。
        /// </summary>
        public bool IsFixedSize => false;

        //

        /// <summary>
        /// 获取或设置此 BitSet 包含的元素数量。
        /// </summary>
        public int Size
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
                    _UintArray = _EmptyData;
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

                        int len = _GetUintNumOfBitNum(_Size);

                        int NewArrayLength = _GetUintArrayLengthOfBitNum(_Size);

                        if (NewArrayLength > _UintArray.Length || NewArrayLength < _UintArray.Length / 2 || NewArrayLength < _UintArray.Length - 256)
                        {
                            uint[] NewUintArray = new uint[NewArrayLength];

                            int LenMin = Math.Min(len, _GetUintNumOfBitNum(OldSize));

                            Array.Copy(_UintArray, NewUintArray, LenMin);

                            _UintArray = NewUintArray;
                        }

                        if (_Size < OldSize)
                        {
                            if (_Size % _BitsPerUint != 0)
                            {
                                int Size = len * _BitsPerUint;

                                for (int i = _Size; i < Size; i++)
                                {
                                    Set(i, false);
                                }
                            }

                            for (int i = len; i < _UintArray.Length; i++)
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
        public int Count => Size;

        /// <summary>
        /// 获取此 BitSet 在其内部数据结构不调整大小的情况下能够容纳的元素数量。
        /// </summary>
        public int Capacity
        {
            get
            {
                if (_UintArray is null)
                {
                    return 0;
                }
                else
                {
                    return _UintArray.Length * _BitsPerUint;
                }
            }
        }

        #endregion

        #region 静态属性

        /// <summary>
        /// 获取表示不包含任何元素的 BitSet 的新实例。
        /// </summary>
        public static BitSet Empty => new BitSet()
        {
            _Size = 0,
            _UintArray = _EmptyData
        };

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
            else if (obj is null || !(obj is BitSet))
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
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// 将此 BitSet 转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 BitSet 的字符串形式。</returns>
        public override string ToString()
        {
            string str = string.Empty;

            if (IsEmpty)
            {
                str = "Empty";
            }
            else
            {
                str = $"Count={_Size}";
            }

            return $"{base.GetType().Name} [{str}]";
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
            else if (bitSet is null)
            {
                return false;
            }
            else if (_Size != bitSet._Size || _UintArray.Length != bitSet._UintArray.Length)
            {
                return false;
            }
            else
            {
                uint[] arrayR = bitSet._UintArray;

                for (int i = 0; i < _UintArray.Length; i++)
                {
                    if (_UintArray[i] != arrayR[i])
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
            else if (obj is null)
            {
                return 1;
            }
            else if (!(obj is BitSet))
            {
                throw new ArgumentException();
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
            else if (bitSet is null)
            {
                return 1;
            }
            else
            {
                int last1L = LastIndexOf(true), last1R = bitSet.LastIndexOf(true);

                if (last1L != last1R)
                {
                    if (last1L < last1R)
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
                    int len = _GetUintNumOfBitNum(Math.Min(last1L, last1R) + 1);

                    uint[] arrayR = bitSet._UintArray;

                    for (int i = len - 1; i >= 0; i--)
                    {
                        int result = _UintArray[i].CompareTo(arrayR[i]);

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
            if (IsEmpty)
            {
                return Empty;
            }
            else
            {
                BitSet result = new BitSet(_Size);

                int len = _GetUintNumOfBitNum(_Size);

                Array.Copy(_UintArray, result._UintArray, len);

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
            if (IsEmpty)
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
            if (startIndex < 0 || startIndex >= _Size)
            {
                throw new IndexOutOfRangeException(nameof(startIndex));
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
            if (startIndex < 0 || startIndex >= _Size)
            {
                throw new IndexOutOfRangeException(nameof(startIndex));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
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
                        if (_GetItemWithoutCheckBounds(j))
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
                                if (_GetItemWithoutCheckBounds(j))
                                {
                                    return j;
                                }
                            }
                        }
                        else if (i == _Right)
                        {
                            for (int j = _Right * _BitsPerUint; j < startIndex + count; j++)
                            {
                                if (_GetItemWithoutCheckBounds(j))
                                {
                                    return j;
                                }
                            }
                        }
                        else if (_UintArray[i] != _FalseUint)
                        {
                            for (int j = i * _BitsPerUint; j < (i + 1) * _BitsPerUint; j++)
                            {
                                if (_GetItemWithoutCheckBounds(j))
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
                        if (!_GetItemWithoutCheckBounds(j))
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
                                if (!_GetItemWithoutCheckBounds(j))
                                {
                                    return j;
                                }
                            }
                        }
                        else if (i == _Right)
                        {
                            for (int j = _Right * _BitsPerUint; j < startIndex + count; j++)
                            {
                                if (!_GetItemWithoutCheckBounds(j))
                                {
                                    return j;
                                }
                            }
                        }
                        else if (_UintArray[i] != _TrueUint)
                        {
                            for (int j = i * _BitsPerUint; j < (i + 1) * _BitsPerUint; j++)
                            {
                                if (!_GetItemWithoutCheckBounds(j))
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
            if (IsEmpty)
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
            if (startIndex < 0 || startIndex >= _Size)
            {
                throw new IndexOutOfRangeException(nameof(startIndex));
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
            if (startIndex < 0 || startIndex >= _Size)
            {
                throw new IndexOutOfRangeException(nameof(startIndex));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
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
                        if (_GetItemWithoutCheckBounds(j))
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
                                if (_GetItemWithoutCheckBounds(j))
                                {
                                    return j;
                                }
                            }
                        }
                        else if (i == _Left)
                        {
                            for (int j = (_Left + 1) * _BitsPerUint - 1; j > startIndex - count; j--)
                            {
                                if (_GetItemWithoutCheckBounds(j))
                                {
                                    return j;
                                }
                            }
                        }
                        else if (_UintArray[i] != _FalseUint)
                        {
                            for (int j = (i + 1) * _BitsPerUint - 1; j >= i * _BitsPerUint; j--)
                            {
                                if (_GetItemWithoutCheckBounds(j))
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
                        if (!_GetItemWithoutCheckBounds(j))
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
                                if (!_GetItemWithoutCheckBounds(j))
                                {
                                    return j;
                                }
                            }
                        }
                        else if (i == _Left)
                        {
                            for (int j = (_Left + 1) * _BitsPerUint - 1; j > startIndex - count; j--)
                            {
                                if (!_GetItemWithoutCheckBounds(j))
                                {
                                    return j;
                                }
                            }
                        }
                        else if (_UintArray[i] != _TrueUint)
                        {
                            for (int j = (i + 1) * _BitsPerUint - 1; j >= i * _BitsPerUint; j--)
                            {
                                if (!_GetItemWithoutCheckBounds(j))
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
            if (IsEmpty)
            {
                return false;
            }
            else
            {
                int len = _GetUintNumOfBitNum(_Size);

                if (item)
                {
                    for (int i = 0; i < len; i++)
                    {
                        if (i == len - 1)
                        {
                            for (int j = i * _BitsPerUint; j < _Size; j++)
                            {
                                if (_GetItemWithoutCheckBounds(j))
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
                    for (int i = 0; i < len; i++)
                    {
                        if (i == len - 1)
                        {
                            for (int j = i * _BitsPerUint; j < _Size; j++)
                            {
                                if (!_GetItemWithoutCheckBounds(j))
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
            if (IsEmpty)
            {
                return Array.Empty<bool>();
            }
            else
            {
                bool[] result = new bool[_Size];

                for (int i = 0; i < _Size; i++)
                {
                    result[i] = _GetItemWithoutCheckBounds(i);
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
            if (IsEmpty)
            {
                return new List<bool>(0);
            }
            else
            {
                List<bool> result = new List<bool>(_Size);

                for (int i = 0; i < _Size; i++)
                {
                    result.Add(_GetItemWithoutCheckBounds(i));
                }

                return result;
            }
        }

        //

        /// <summary>
        /// 将此 BitSet 的所有位值复制到布尔值数组中。
        /// </summary>
        /// <param name="array">布尔值数组。</param>
        /// <param name="index">布尔值数组的起始索引。</param>
        public void CopyTo(bool[] array, int index)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Length - index < _Size)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }

            //

            ToArray().CopyTo(array, index);
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

                    int len = _GetUintNumOfBitNum(_Size);

                    Array.Copy(_UintArray, NewUintArray, len);

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
            if (index < 0 || index >= _Size)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }

            //

            return _GetItemWithoutCheckBounds(index);
        }

        /// <summary>
        /// 设置此 BitSet 指定索引位置的位值。
        /// </summary>
        /// <param name="index">索引。</param>
        /// <param name="bitValue">位值。</param>
        public void Set(int index, bool bitValue)
        {
            if (index < 0 || index >= _Size)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }

            //

            _SetItemWithoutCheckBounds(index, bitValue);
        }

        /// <summary>
        /// 设置此 BitSet 的所有位值。
        /// </summary>
        /// <param name="bitValue">位值。</param>
        public void SetAll(bool bitValue)
        {
            if (_Size > 0)
            {
                int len = _GetUintNumOfBitNum(_Size);

                if (bitValue)
                {
                    for (int i = 0; i < len; i++)
                    {
                        _UintArray[i] = _TrueUint;
                    }

                    if (_Size % _BitsPerUint != 0)
                    {
                        _UintArray[len - 1] >>= _BitsPerUint - _Size % _BitsPerUint;
                    }
                }
                else
                {
                    for (int i = 0; i < len; i++)
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
            if (index < 0 || index >= _Size)
            {
                throw new IndexOutOfRangeException(nameof(index));
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
            if (index < 0 || index >= _Size)
            {
                throw new IndexOutOfRangeException(nameof(index));
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
            if (index < 0 || index >= _Size)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }

            //

            _UintArray[index / _BitsPerUint] ^= ((uint)1) << (index % _BitsPerUint);
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
                int len = _GetUintNumOfBitNum(_Size);

                for (int i = 0; i < len; i++)
                {
                    _UintArray[i] = ~_UintArray[i];
                }

                if (_Size % _BitsPerUint != 0)
                {
                    int Size = len * _BitsPerUint;

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
            if (IsEmpty)
            {
                return 0;
            }
            else
            {
                int Count = 0;

                int len = _GetUintNumOfBitNum(_Size);

                for (int i = 0; i < len; i++)
                {
                    uint Bin = _UintArray[i];

                    while (Bin > 0)
                    {
                        Bin &= Bin - 1;

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
            if (IsEmpty)
            {
                return 0;
            }
            else
            {
                return _Size - TrueBitCount();
            }
        }

        /// <summary>
        /// 返回包含此 BitSet 值为 true 的位值的索引的数组。
        /// </summary>
        /// <returns>32 位整数数组，该数组包含此 BitSet 值为 true 的位值的索引。</returns>
        public int[] TrueBitIndex()
        {
            if (IsEmpty)
            {
                return Array.Empty<int>();
            }
            else
            {
                List<int> result = new List<int>(_Size);

                int len = _GetUintNumOfBitNum(_Size);

                for (int i = 0; i < len; i++)
                {
                    uint Bin = _UintArray[i];

                    for (int j = 0; j < _BitsPerUint; j++)
                    {
                        if ((Bin & 1) != 0)
                        {
                            if (i >= len - 1 && _Size % _BitsPerUint != 0 && j > _BitsPerUint - _Size % _BitsPerUint)
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
            if (IsEmpty)
            {
                return Array.Empty<int>();
            }
            else
            {
                List<int> result = new List<int>(_Size);

                int len = _GetUintNumOfBitNum(_Size);

                for (int i = 0; i < len; i++)
                {
                    uint Bin = _UintArray[i];

                    for (int j = 0; j < _BitsPerUint; j++)
                    {
                        if ((Bin & 1) == 0)
                        {
                            if (i >= len - 1 && _Size % _BitsPerUint != 0 && j > _BitsPerUint - _Size % _BitsPerUint)
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
            if (bitSet is null)
            {
                throw new ArgumentNullException(nameof(bitSet));
            }

            //

            bool LIsEmpty = IsEmpty;
            bool RIsEmpty = bitSet.IsEmpty;

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
            else if (_Size != bitSet._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                BitSet result = new BitSet(_Size);

                int len = _GetUintNumOfBitNum(_Size);

                uint[] arrayR = bitSet._UintArray;

                for (int i = 0; i < len; i++)
                {
                    result._UintArray[i] = _UintArray[i] & arrayR[i];
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
            if (bitSet is null)
            {
                throw new ArgumentNullException(nameof(bitSet));
            }

            //

            bool LIsEmpty = IsEmpty;
            bool RIsEmpty = bitSet.IsEmpty;

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
            else if (_Size != bitSet._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                BitSet result = new BitSet(_Size);

                int len = _GetUintNumOfBitNum(_Size);

                uint[] arrayR = bitSet._UintArray;

                for (int i = 0; i < len; i++)
                {
                    result._UintArray[i] = _UintArray[i] | arrayR[i];
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
            if (bitSet is null)
            {
                throw new ArgumentNullException(nameof(bitSet));
            }

            //

            bool LIsEmpty = IsEmpty;
            bool RIsEmpty = bitSet.IsEmpty;

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
            else if (_Size != bitSet._Size)
            {
                throw new ArithmeticException();
            }
            else
            {
                BitSet result = new BitSet(_Size);

                int len = _GetUintNumOfBitNum(_Size);

                uint[] arrayR = bitSet._UintArray;

                for (int i = 0; i < len; i++)
                {
                    result._UintArray[i] = _UintArray[i] ^ arrayR[i];
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
            if (IsEmpty)
            {
                return Empty;
            }
            else
            {
                BitSet result = new BitSet(_Size);

                int len = _GetUintNumOfBitNum(_Size);

                for (int i = 0; i < len; i++)
                {
                    result._UintArray[i] = ~_UintArray[i];
                }

                if (_Size % _BitsPerUint != 0)
                {
                    int Size = len * _BitsPerUint;

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
            if (IsEmpty)
            {
                return string.Empty;
            }
            else
            {
                char[] BitCharArray = new char[_Size];

                for (int i = 0; i < _Size; i++)
                {
                    BitCharArray[_Size - 1 - i] = _GetItemWithoutCheckBounds(i) ? '1' : '0';
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
        public static bool IsNullOrEmpty(BitSet bitSet) => bitSet is null || bitSet._Size <= 0;

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
            else if (left is null)
            {
                return -1;
            }
            else if (right is null)
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
                int last1L = left.LastIndexOf(true), last1R = right.LastIndexOf(true);

                if (last1L != last1R)
                {
                    return false;
                }
                else
                {
                    int len = _GetUintNumOfBitNum(Math.Min(last1L, last1R) + 1);

                    uint[] arrayL = left._UintArray, arrayR = right._UintArray;

                    for (int i = 0; i < len; i++)
                    {
                        if (arrayL[i] != arrayR[i])
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
                int last1L = left.LastIndexOf(true), last1R = right.LastIndexOf(true);

                if (last1L != last1R)
                {
                    return true;
                }
                else
                {
                    int len = _GetUintNumOfBitNum(Math.Min(last1L, last1R) + 1);

                    uint[] arrayL = left._UintArray, arrayR = right._UintArray;

                    for (int i = 0; i < len; i++)
                    {
                        if (arrayL[i] != arrayR[i])
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
                int last1L = left.LastIndexOf(true), last1R = right.LastIndexOf(true);

                if (last1L != last1R)
                {
                    return last1L < last1R;
                }
                else
                {
                    int len = _GetUintNumOfBitNum(Math.Min(last1L, last1R) + 1);

                    uint[] arrayL = left._UintArray, arrayR = right._UintArray;

                    for (int i = len - 1; i >= 0; i--)
                    {
                        if (arrayL[i] != arrayR[i])
                        {
                            return arrayL[i] < arrayR[i];
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
                int last1L = left.LastIndexOf(true), last1R = right.LastIndexOf(true);

                if (last1L != last1R)
                {
                    return last1L > last1R;
                }
                else
                {
                    int len = _GetUintNumOfBitNum(Math.Min(last1L, last1R) + 1);

                    uint[] arrayL = left._UintArray, arrayR = right._UintArray;

                    for (int i = len - 1; i >= 0; i--)
                    {
                        if (arrayL[i] != arrayR[i])
                        {
                            return arrayL[i] > arrayR[i];
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
                int last1L = left.LastIndexOf(true), last1R = right.LastIndexOf(true);

                if (last1L != last1R)
                {
                    return last1L < last1R;
                }
                else
                {
                    int len = _GetUintNumOfBitNum(Math.Min(last1L, last1R) + 1);

                    uint[] arrayL = left._UintArray, arrayR = right._UintArray;

                    for (int i = len - 1; i >= 0; i--)
                    {
                        if (arrayL[i] != arrayR[i])
                        {
                            return arrayL[i] < arrayR[i];
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
                int last1L = left.LastIndexOf(true), last1R = right.LastIndexOf(true);

                if (last1L != last1R)
                {
                    return last1L > last1R;
                }
                else
                {
                    int len = _GetUintNumOfBitNum(Math.Min(last1L, last1R) + 1);

                    uint[] arrayL = left._UintArray, arrayR = right._UintArray;

                    for (int i = len - 1; i >= 0; i--)
                    {
                        if (arrayL[i] != arrayR[i])
                        {
                            return arrayL[i] > arrayR[i];
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
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            //

            return left.And(right);
        }

        /// <summary>
        /// 返回将 BitSet 对象与 BitSet 对象按位或得到的 BitSet 的新实例。
        /// </summary>
        /// <param name="left">运算符左侧的 BitSet 对象。</param>
        /// <param name="right">运算符右侧的 BitSet 对象。</param>
        /// <returns>BitSet 对象，表示将 BitSet 对象与 BitSet 对象按位或得到的结果。</returns>
        public static BitSet operator |(BitSet left, BitSet right)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            //

            return left.Or(right);
        }

        /// <summary>
        /// 返回将 BitSet 对象与 BitSet 对象按位异或得到的 BitSet 的新实例。
        /// </summary>
        /// <param name="left">运算符左侧的 BitSet 对象。</param>
        /// <param name="right">运算符右侧的 BitSet 对象。</param>
        /// <returns>BitSet 对象，表示将 BitSet 对象与 BitSet 对象按位异或得到的结果。</returns>
        public static BitSet operator ^(BitSet left, BitSet right)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            //

            return left.Xor(right);
        }

        /// <summary>
        /// 返回将 BitSet 对象按位取反得到的 BitSet 的新实例。
        /// </summary>
        /// <param name="bitSet">运算符右侧的 BitSet 对象。</param>
        /// <returns>BitSet 对象，表示将 BitSet 对象按位取反得到的结果。</returns>
        public static BitSet operator ~(BitSet bitSet)
        {
            if (bitSet is null)
            {
                throw new ArgumentNullException(nameof(bitSet));
            }

            //

            return bitSet.Not();
        }

        #endregion

        #region 显式接口成员实现

        #region System.Collections.IList

        object IList.this[int index]
        {
            get => Get(index);

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!(value is bool))
                {
                    throw new ArgumentException();
                }

                //

                Set(index, (bool)value);
            }
        }

        int IList.Add(object item) => throw new NotSupportedException();

        void IList.Clear()
        {
            if (_Size > 0)
            {
                _Size = 0;
                _UintArray = _EmptyData;
            }
        }

        bool IList.Contains(object item)
        {
            if (item is null || !(item is bool))
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
            if (item is null || !(item is bool))
            {
                return -1;
            }
            else
            {
                return IndexOf((bool)item);
            }
        }

        void IList.Insert(int index, object item) => throw new NotSupportedException();

        void IList.Remove(object item) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();

        #endregion

        #region System.Collections.ICollection

        object ICollection.SyncRoot => this;

        bool ICollection.IsSynchronized => false;

        void ICollection.CopyTo(Array array, int index)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Rank != 1)
            {
                throw new RankException();
            }

            if (array.Length - index < _Size)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }

            //

            ToArray().CopyTo(array, index);
        }

        #endregion

        #region System.Collections.IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

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
                    if (_BitSet is null)
                    {
                        throw new ArgumentNullException(nameof(_BitSet));
                    }

                    if (_Index < 0 || _Index >= _BitSet._Size)
                    {
                        throw new IndexOutOfRangeException(nameof(_Index));
                    }

                    //

                    return _BitSet._GetItemWithoutCheckBounds(_Index);
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

            void IEnumerator.Reset() => _Index = -1;
        }

        #endregion

        #region System.Collections.Generic.IList<T>

        void IList<bool>.Insert(int index, bool item) => throw new NotSupportedException();

        void IList<bool>.RemoveAt(int index) => throw new NotSupportedException();

        #endregion

        #region System.Collections.Generic.ICollection<T>

        void ICollection<bool>.Add(bool item) => throw new NotSupportedException();

        void ICollection<bool>.Clear()
        {
            if (_Size > 0)
            {
                _Size = 0;
                _UintArray = _EmptyData;
            }
        }

        bool ICollection<bool>.Remove(bool item) => throw new NotSupportedException();

        #endregion

        #region System.Collections.Generic.IEnumerable<out T>

        IEnumerator<bool> IEnumerable<bool>.GetEnumerator() => new GenericEnumerator(this);

        private sealed class GenericEnumerator : IEnumerator<bool> // 实现 System.Collections.Generic.IEnumerator<out T> 的迭代器。
        {
            private BitSet _BitSet;
            private int _Index;

            internal GenericEnumerator(BitSet bitSet)
            {
                _BitSet = bitSet;
                _Index = -1;
            }

            void IDisposable.Dispose() => _BitSet = null;

            object IEnumerator.Current
            {
                get
                {
                    if (_BitSet is null)
                    {
                        throw new ArgumentNullException(nameof(_BitSet));
                    }

                    if (_Index < 0 || _Index >= _BitSet._Size)
                    {
                        throw new IndexOutOfRangeException(nameof(_Index));
                    }

                    //

                    return _BitSet._GetItemWithoutCheckBounds(_Index);
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

            void IEnumerator.Reset() => _Index = -1;

            bool IEnumerator<bool>.Current
            {
                get
                {
                    if (_BitSet is null)
                    {
                        throw new ArgumentNullException(nameof(_BitSet));
                    }

                    if (_Index < 0 || _Index >= _BitSet._Size)
                    {
                        throw new IndexOutOfRangeException(nameof(_Index));
                    }

                    //

                    return _BitSet._GetItemWithoutCheckBounds(_Index);
                }
            }
        }

        #endregion

        #endregion
    }
}