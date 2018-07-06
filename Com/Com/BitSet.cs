/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.BitSet
Version 18.7.6.2250

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
    /// 以数组的形式管理位值的集合，位值 1 与 0 分别以布尔值 true 与 false 表示。
    /// </summary>
    public sealed class BitSet
    {
        #region 私有与内部成员

        private const int _BitsPerUint = 32; // 32 位无符号整数包含的位值数量。

        private const uint _TrueUint = uint.MaxValue, _FalseUint = uint.MinValue; // 所有位值为 true 或 false 的 32 位无符号整数。

        private const int _MaxSize = 2147483520; // BitSet 能够包含的最大元素数量。

        //

        private int _Size; // 此 BitSet 包含的元素数量。

        private uint[] _UintArray; // 用于存储位值的 32 位无符号整数数组。

        //

        private int _GetUintNumOfBitNum(int bitNum) // 根据位值的数量计算 32 位无符号整数的数量。
        {
            if (bitNum > 0)
            {
                return ((bitNum - 1) / _BitsPerUint + 1);
            }

            return 0;
        }

        private int _GetUintArrayLengthOfBitNum(int bitNum) // 根据位值的数量计算 32 位无符号整数数组的长度。
        {
            if (bitNum > 0)
            {
                int ArrayLength = _GetUintNumOfBitNum(bitNum);

                if (ArrayLength > 2)
                {
                    ArrayLength = ((ArrayLength - 1) / 4 + 1) * 4;
                }

                return ArrayLength;
            }

            return 0;
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用指定的元素数量与默认的位值（false）初始化 BitSet 的新实例。
        /// </summary>
        /// <param name="length">元素数量。</param>
        public BitSet(int length)
        {
            if (length > 0)
            {
                _Size = Math.Min(_MaxSize, length);
                _UintArray = new uint[_GetUintArrayLengthOfBitNum(length)];
            }
            else
            {
                _Size = 0;
                _UintArray = null;
            }
        }

        /// <summary>
        /// 使用指定的元素数量与位值初始化 BitSet 的新实例。
        /// </summary>
        /// <param name="length">元素数量。</param>
        /// <param name="bitValue">位值。</param>
        public BitSet(int length, bool bitValue)
        {
            if (length > 0)
            {
                _Size = Math.Min(_MaxSize, length);
                _UintArray = new uint[_GetUintArrayLengthOfBitNum(length)];

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
            else
            {
                _Size = 0;
                _UintArray = null;
            }
        }

        /// <summary>
        /// 使用表示位值的布尔值数组初始化 BitSet 的新实例。
        /// </summary>
        /// <param name="array">表示位值的布尔值数组。</param>
        public BitSet(bool[] array)
        {
            if (array != null && array.Length > 0)
            {
                _Size = Math.Min(_MaxSize, array.Length);
                _UintArray = new uint[_GetUintArrayLengthOfBitNum(array.Length)];

                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i])
                    {
                        Set(i, true);
                    }
                }
            }
            else
            {
                _Size = 0;
                _UintArray = null;
            }
        }

        /// <summary>
        /// 使用表示位值的 32 位整数数组初始化 BitSet 的新实例。
        /// </summary>
        /// <param name="array">表示位值的 32 位整数数组。</param>
        public BitSet(int[] array)
        {
            if (array != null && array.Length > 0)
            {
                _Size = Math.Min(_MaxSize, array.Length);
                _UintArray = new uint[_GetUintArrayLengthOfBitNum(array.Length)];

                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != 0)
                    {
                        Set(i, true);
                    }
                }
            }
            else
            {
                _Size = 0;
                _UintArray = null;
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
        /// 获取或设置此 BitSet 包含的元素数量。
        /// </summary>
        public int Size
        {
            get
            {
                return _Size;
            }

            set
            {
                if (value > 0)
                {
                    if (_Size != value)
                    {
                        int SizeOld = _Size;

                        _Size = Math.Min(_MaxSize, value);

                        int Len = _GetUintNumOfBitNum(_Size);

                        int NewArrayLength = _GetUintArrayLengthOfBitNum(_Size);

                        if (NewArrayLength > _UintArray.Length || NewArrayLength < _UintArray.Length / 2 || NewArrayLength < _UintArray.Length - 256)
                        {
                            uint[] NewUintArray = new uint[NewArrayLength];

                            int LenMin = Math.Min(Len, _GetUintNumOfBitNum(SizeOld));

                            for (int i = 0; i < LenMin; i++)
                            {
                                NewUintArray[i] = _UintArray[i];
                            }

                            _UintArray = NewUintArray;
                        }

                        if (_Size < SizeOld)
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
                else
                {
                    _Size = 0;
                    _UintArray = null;
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
                return _Size;
            }
        }

        /// <summary>
        /// 获取此 BitSet 包含的元素数量。
        /// </summary>
        public int Length
        {
            get
            {
                return _Size;
            }
        }

        /// <summary>
        /// 获取此 BitSet 在其内部数据结构不调整大小的情况下能够容纳的元素数量。
        /// </summary>
        public int Capacity
        {
            get
            {
                if (_UintArray != null)
                {
                    return (_UintArray.Length * _BitsPerUint);
                }

                return 0;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取此 BitSet 的副本。
        /// </summary>
        public BitSet Copy()
        {
            BitSet Result = new BitSet(_Size);

            if (_Size > 0)
            {
                int Len = _GetUintNumOfBitNum(_Size);

                for (int i = 0; i < Len; i++)
                {
                    Result._UintArray[i] = _UintArray[i];
                }
            }

            return Result;
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

                    for (int i = 0; i < Len; i++)
                    {
                        NewUintArray[i] = _UintArray[i];
                    }

                    _UintArray = NewUintArray;
                }
            }
        }

        //

        /// <summary>
        /// 获取此 BitSet 指定索引位置的位值。
        /// </summary>
        /// <param name="index">索引。</param>
        public bool Get(int index)
        {
            if (_Size > 0)
            {
                if (index >= 0 && index < _Size)
                {
                    return ((_UintArray[index / _BitsPerUint] & (((uint)1) << (index % _BitsPerUint))) != 0);
                }
            }

            return false;
        }

        /// <summary>
        /// 设置此 BitSet 指定索引位置的位值。
        /// </summary>
        /// <param name="index">索引。</param>
        /// <param name="bitValue">位值。</param>
        public void Set(int index, bool bitValue)
        {
            if (_Size > 0)
            {
                if (index >= 0 && index < _Size)
                {
                    if (bitValue)
                    {
                        _UintArray[index / _BitsPerUint] |= (((uint)1) << (index % _BitsPerUint));
                    }
                    else
                    {
                        _UintArray[index / _BitsPerUint] &= (~(((uint)1) << (index % _BitsPerUint)));
                    }
                }
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

                for (int i = 0; i < Len; i++)
                {
                    _UintArray[i] = (bitValue ? _TrueUint : _FalseUint);
                }

                if (bitValue)
                {
                    if (_Size % _BitsPerUint != 0)
                    {
                        _UintArray[Len - 1] >>= (_BitsPerUint - _Size % _BitsPerUint);
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
            if (_Size > 0)
            {
                if (index >= 0 && index < _Size)
                {
                    Set(index, true);
                }
            }
        }

        /// <summary>
        /// 将此 BitSet 指定索引位置的位值设为 false。
        /// </summary>
        /// <param name="index">索引。</param>
        public void FalseForBit(int index)
        {
            if (_Size > 0)
            {
                if (index >= 0 && index < _Size)
                {
                    Set(index, false);
                }
            }
        }

        /// <summary>
        /// 将此 BitSet 指定索引位置的位值取反。
        /// </summary>
        /// <param name="index">索引。</param>
        public void InverseBit(int index)
        {
            if (_Size > 0)
            {
                if (index >= 0 && index < _Size)
                {
                    _UintArray[index / _BitsPerUint] ^= (((uint)1) << (index % _BitsPerUint));
                }
            }
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
        /// 获取此 BitSet 值为 true 的位值的数量。
        /// </summary>
        public int TrueBitCount()
        {
            if (_Size > 0)
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

            return 0;
        }

        /// <summary>
        /// 获取此 BitSet 值为 false 的位值的数量。
        /// </summary>
        public int FalseBitCount()
        {
            if (_Size > 0)
            {
                return (_Size - TrueBitCount());
            }

            return 0;
        }

        /// <summary>
        /// 获取包含此 BitSet 值为 true 的位值的索引的数组。
        /// </summary>
        public int[] TrueBitIndex()
        {
            if (_Size > 0)
            {
                List<int> Ary = new List<int>(_Size);

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

                            Ary.Add(i * _BitsPerUint + j);
                        }

                        Bin >>= 1;
                    }
                }

                return Ary.ToArray();
            }

            return null;
        }

        /// <summary>
        /// 获取包含此 BitSet 值为 true 的位值的索引的数组。
        /// </summary>
        public int[] FalseBitIndex()
        {
            if (_Size > 0)
            {
                List<int> Ary = new List<int>(_Size);

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

                            Ary.Add(i * _BitsPerUint + j);
                        }

                        Bin >>= 1;
                    }
                }

                return Ary.ToArray();
            }

            return null;
        }

        //

        /// <summary>
        /// 返回将此 BitSet 与指定的 BitSet 按位与得到的 BitSet 的新实例。
        /// </summary>
        /// <param name="bitSet">运算符右侧的 BitSet。</param>
        public BitSet And(BitSet bitSet)
        {
            if (!IsNullOrEmpty(bitSet) && _Size == bitSet._Size)
            {
                BitSet Result = new BitSet(_Size);

                int Len = _GetUintNumOfBitNum(_Size);

                for (int i = 0; i < Len; i++)
                {
                    Result._UintArray[i] = (_UintArray[i] & bitSet._UintArray[i]);
                }

                return Result;
            }

            return null;
        }

        /// <summary>
        /// 返回将此 BitSet 与指定的 BitSet 按位或得到的 BitSet 的新实例。
        /// </summary>
        /// <param name="bitSet">运算符右侧的 BitSet。</param>
        public BitSet Or(BitSet bitSet)
        {
            if (!IsNullOrEmpty(bitSet) && _Size == bitSet._Size)
            {
                BitSet Result = new BitSet(_Size);

                int Len = _GetUintNumOfBitNum(_Size);

                for (int i = 0; i < Len; i++)
                {
                    Result._UintArray[i] = (_UintArray[i] | bitSet._UintArray[i]);
                }

                return Result;
            }

            return null;
        }

        /// <summary>
        /// 返回将此 BitSet 与指定的 BitSet 按位异或得到的 BitSet 的新实例。
        /// </summary>
        /// <param name="bitSet">运算符右侧的 BitSet。</param>
        public BitSet Xor(BitSet bitSet)
        {
            if (!IsNullOrEmpty(bitSet) && _Size == bitSet._Size)
            {
                BitSet Result = new BitSet(_Size);

                int Len = _GetUintNumOfBitNum(_Size);

                for (int i = 0; i < Len; i++)
                {
                    Result._UintArray[i] = (_UintArray[i] ^ bitSet._UintArray[i]);
                }

                return Result;
            }

            return null;
        }

        /// <summary>
        /// 返回将此 BitSet 按位取反得到的 BitSet 的新实例。
        /// </summary>
        public BitSet Not()
        {
            BitSet Result = new BitSet(_Size);

            int Len = _GetUintNumOfBitNum(_Size);

            for (int i = 0; i < Len; i++)
            {
                Result._UintArray[i] = (~_UintArray[i]);
            }

            if (_Size % _BitsPerUint != 0)
            {
                int Size = Len * _BitsPerUint;

                for (int i = _Size; i < Size; i++)
                {
                    Result.Set(i, false);
                }
            }

            return Result;
        }

        //

        /// <summary>
        /// 返回将此 BitSet 转换为布尔值的数组。
        /// </summary>
        public bool[] ToBoolArray()
        {
            if (_Size > 0)
            {
                bool[] Result = new bool[_Size];

                for (int i = 0; i < _Size; i++)
                {
                    Result[i] = Get(i);
                }

                return Result;
            }

            return null;
        }

        /// <summary>
        /// 返回将此 BitSet 转换为 32 位整数的数组。
        /// </summary>
        public int[] ToIntArray()
        {
            if (_Size > 0)
            {
                int[] Result = new int[_Size];

                for (int i = 0; i < _Size; i++)
                {
                    Result[i] = (Get(i) ? 1 : 0);
                }

                return Result;
            }

            return null;
        }

        //

        /// <summary>
        /// 将此 BitSet 的所有位值转换为字符串。
        /// </summary>
        public string ToBitString()
        {
            if (_Size > 0)
            {
                char[] BitCharArray = new char[_Size];

                for (int i = 0; i < _Size; i++)
                {
                    BitCharArray[i] = (Get(i) ? '1' : '0');
                }

                return new string(BitCharArray);
            }

            return string.Empty;
        }

        #endregion

        #region 基类方法

        /// <summary>
        /// 判断此 BitSet 是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is BitSet))
            {
                return false;
            }

            return Equals((BitSet)obj);
        }

        /// <summary>
        /// 判断此 BitSet 是否与指定的 BitSet 对象相等。
        /// </summary>
        /// <param name="bitSet">用于比较的 BitSet 对象。</param>
        public bool Equals(BitSet bitSet)
        {
            if (bitSet == null)
            {
                return false;
            }

            if (_Size != bitSet._Size || _UintArray == null || bitSet._UintArray == null || _UintArray.Length != bitSet._UintArray.Length)
            {
                return false;
            }

            for (int i = 0; i < _UintArray.Length; i++)
            {
                if (_UintArray[i] != bitSet._UintArray[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 返回此 BitSet 的哈希代码。
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 BitSet 转换为字符串。
        /// </summary>
        public override string ToString()
        {
            string Str = string.Empty;

            if (_Size > 0)
            {
                Str = string.Concat("Size=", _Size);
            }
            else
            {
                Str = "Empty";
            }

            return string.Concat(base.GetType().Name, " [", Str, "]");
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断指定的 BitSet 是否为 null 或包含的元素数量为 0。
        /// </summary>
        /// <param name="bitSet">用于判断的 BitSet 对象。</param>
        public static bool IsNullOrEmpty(BitSet bitSet)
        {
            return (bitSet == null || bitSet._Size == 0);
        }

        #endregion
    }
}