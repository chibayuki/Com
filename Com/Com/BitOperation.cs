/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.BitOperation
Version 19.10.14.2100

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
    /// 为 8 位、16 位、32 位与 64 位无符号整数的位运算提供静态方法。
    /// </summary>
    public static class BitOperation
    {
        #region 8 位

        /// <summary>
        /// 返回一个仅指定二进制位为 1 的 8 位无符号整数。
        /// </summary>
        /// <param name="bit">指定的二进制位。</param>
        /// <returns>8 位无符号整数，该 8 位无符号整数仅指定二进制位为 1。</returns>
        public static byte GetBinary8WithSingleBit1(int bit)
        {
            if (bit < 0 || bit >= 8)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return (byte)(1U << bit);
        }

        /// <summary>
        /// 返回一个仅指定二进制位为 0 的 8 位无符号整数。
        /// </summary>
        /// <param name="bit">指定的二进制位。</param>
        /// <returns>8 位无符号整数，该 8 位无符号整数仅指定二进制位为 0。</returns>
        public static byte GetBinary8WithSingleBit0(int bit)
        {
            if (bit < 0 || bit >= 8)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return (byte)(~(1U << bit));
        }

        /// <summary>
        /// 将 8 位无符号整数的指定二进制位设为 1。
        /// </summary>
        /// <param name="bin">8 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void AddBitToBinary(ref byte bin, int bit)
        {
            if (bit < 0 || bit >= 8)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            bin |= (byte)(1U << bit);
        }

        /// <summary>
        /// 将 8 位无符号整数的指定二进制位设为 0。
        /// </summary>
        /// <param name="bin">8 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void RemoveBitFromBinary(ref byte bin, int bit)
        {
            if (bit < 0 || bit >= 8)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            bin &= (byte)(~(1U << bit));
        }

        /// <summary>
        /// 将 8 位无符号整数的指定二进制位反转。
        /// </summary>
        /// <param name="bin">8 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void InverseBitOfBinary(ref byte bin, int bit)
        {
            if (bit < 0 || bit >= 8)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            bin ^= (byte)(1U << bit);
        }

        /// <summary>
        /// 判断 8 位无符号整数的指定二进制位是否为 1。
        /// </summary>
        /// <param name="bin">8 位无符号整数。</param>
        /// <param name="bit">指定的二进制位。</param>
        /// <returns>布尔值，表示 8 位无符号整数的指定二进制位是否为 1。</returns>
        public static bool BinaryHasBit(byte bin, int bit)
        {
            if (bit < 0 || bit >= 8)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return ((bin & (1U << bit)) != 0);
        }

        /// <summary>
        /// 计算 8 位无符号整数的值为 1 的二进制位的数量。
        /// </summary>
        /// <param name="bin">8 位无符号整数。</param>
        /// <returns>8 位无符号整数的值为 1 的二进制位的数量。</returns>
        public static int GetBit1CountOfBinary(byte bin)
        {
            int Count = 0;

            while (bin > 0)
            {
                bin &= (byte)(bin - 1U);

                Count++;
            }

            return Count;
        }

        /// <summary>
        /// 计算 8 位无符号整数的值为 0 的二进制位的数量。
        /// </summary>
        /// <param name="bin">8 位无符号整数。</param>
        /// <returns>8 位无符号整数的值为 0 的二进制位的数量。</returns>
        public static int GetBit0CountOfBinary(byte bin)
        {
            return (8 - GetBit1CountOfBinary(bin));
        }

        /// <summary>
        /// 返回一个列表，列表元素表示 8 位无符号整数值为 1 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。
        /// </summary>
        /// <param name="bin">8 位无符号整数。</param>
        /// <returns>32 位整数列表，列表元素表示 8 位无符号整数值为 1 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。</returns>
        public static List<int> GetBit1IndexOfBinary(byte bin)
        {
            List<int> result = new List<int>(8);

            for (int i = 0; i < 8; i++)
            {
                if ((bin & 1U) != 0)
                {
                    result.Add(i);
                }

                bin >>= 1;
            }

            return result;
        }

        /// <summary>
        /// 返回一个列表，列表元素表示 8 位无符号整数值为 0 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。
        /// </summary>
        /// <param name="bin">8 位无符号整数。</param>
        /// <returns>32 位整数列表，列表元素表示 8 位无符号整数值为 0 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。</returns>
        public static List<int> GetBit0IndexOfBinary(byte bin)
        {
            List<int> result = new List<int>(8);

            for (int i = 0; i < 8; i++)
            {
                if ((bin & 1U) == 0)
                {
                    result.Add(i);
                }

                bin >>= 1;
            }

            return result;
        }

        #endregion

        #region 16 位

        /// <summary>
        /// 返回一个仅指定二进制位为 1 的 16 位无符号整数。
        /// </summary>
        /// <param name="bit">指定的二进制位。</param>
        /// <returns>16 位无符号整数，该 16 位无符号整数仅指定二进制位为 1。</returns>
        public static ushort GetBinary16WithSingleBit1(int bit)
        {
            if (bit < 0 || bit >= 16)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return (ushort)(1U << bit);
        }

        /// <summary>
        /// 返回一个仅指定二进制位为 0 的 16 位无符号整数。
        /// </summary>
        /// <param name="bit">指定的二进制位。</param>
        /// <returns>16 位无符号整数，该 16 位无符号整数仅指定二进制位为 0。</returns>
        public static ushort GetBinary16WithSingleBit0(int bit)
        {
            if (bit < 0 || bit >= 16)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return (ushort)(~(1U << bit));
        }

        /// <summary>
        /// 将 16 位无符号整数的指定二进制位设为 1。
        /// </summary>
        /// <param name="bin">16 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void AddBitToBinary(ref ushort bin, int bit)
        {
            if (bit < 0 || bit >= 16)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            bin |= (ushort)(1U << bit);
        }

        /// <summary>
        /// 将 16 位无符号整数的指定二进制位设为 0。
        /// </summary>
        /// <param name="bin">16 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void RemoveBitFromBinary(ref ushort bin, int bit)
        {
            if (bit < 0 || bit >= 16)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            bin &= (ushort)(~(1U << bit));
        }

        /// <summary>
        /// 将 16 位无符号整数的指定二进制位反转。
        /// </summary>
        /// <param name="bin">16 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void InverseBitOfBinary(ref ushort bin, int bit)
        {
            if (bit < 0 || bit >= 16)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            bin ^= (ushort)(1U << bit);
        }

        /// <summary>
        /// 判断 16 位无符号整数的指定二进制位是否为 1。
        /// </summary>
        /// <param name="bin">16 位无符号整数。</param>
        /// <param name="bit">指定的二进制位。</param>
        /// <returns>布尔值，表示 16 位无符号整数的指定二进制位是否为 1。</returns>
        public static bool BinaryHasBit(ushort bin, int bit)
        {
            if (bit < 0 || bit >= 16)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return ((bin & (1U << bit)) != 0);
        }

        /// <summary>
        /// 计算 16 位无符号整数的值为 1 的二进制位的数量。
        /// </summary>
        /// <param name="bin">16 位无符号整数。</param>
        /// <returns>16 位无符号整数的值为 1 的二进制位的数量。</returns>
        public static int GetBit1CountOfBinary(ushort bin)
        {
            int Count = 0;

            while (bin > 0)
            {
                bin &= (ushort)(bin - 1U);

                Count++;
            }

            return Count;
        }

        /// <summary>
        /// 计算 16 位无符号整数的值为 0 的二进制位的数量。
        /// </summary>
        /// <param name="bin">16 位无符号整数。</param>
        /// <returns>16 位无符号整数的值为 0 的二进制位的数量。</returns>
        public static int GetBit0CountOfBinary(ushort bin)
        {
            return (16 - GetBit1CountOfBinary(bin));
        }

        /// <summary>
        /// 返回一个列表，列表元素表示 16 位无符号整数值为 1 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。
        /// </summary>
        /// <param name="bin">16 位无符号整数。</param>
        /// <returns>32 位整数列表，列表元素表示 16 位无符号整数值为 1 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。</returns>
        public static List<int> GetBit1IndexOfBinary(ushort bin)
        {
            List<int> result = new List<int>(16);

            for (int i = 0; i < 16; i++)
            {
                if ((bin & 1U) != 0)
                {
                    result.Add(i);
                }

                bin >>= 1;
            }

            return result;
        }

        /// <summary>
        /// 返回一个列表，列表元素表示 16 位无符号整数值为 0 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。
        /// </summary>
        /// <param name="bin">16 位无符号整数。</param>
        /// <returns>32 位整数列表，列表元素表示 16 位无符号整数值为 0 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。</returns>
        public static List<int> GetBit0IndexOfBinary(ushort bin)
        {
            List<int> result = new List<int>(16);

            for (int i = 0; i < 16; i++)
            {
                if ((bin & 1U) == 0)
                {
                    result.Add(i);
                }

                bin >>= 1;
            }

            return result;
        }

        #endregion

        #region 32 位

        /// <summary>
        /// 返回一个仅指定二进制位为 1 的 32 位无符号整数。
        /// </summary>
        /// <param name="bit">指定的二进制位。</param>
        /// <returns>32 位无符号整数，该 32 位无符号整数仅指定二进制位为 1。</returns>
        public static uint GetBinary32WithSingleBit1(int bit)
        {
            if (bit < 0 || bit >= 32)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return (1U << bit);
        }

        /// <summary>
        /// 返回一个仅指定二进制位为 0 的 32 位无符号整数。
        /// </summary>
        /// <param name="bit">指定的二进制位。</param>
        /// <returns>32 位无符号整数，该 32 位无符号整数仅指定二进制位为 0。</returns>
        public static uint GetBinary32WithSingleBit0(int bit)
        {
            if (bit < 0 || bit >= 32)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return (~(1U << bit));
        }

        /// <summary>
        /// 将 32 位无符号整数的指定二进制位设为 1。
        /// </summary>
        /// <param name="bin">32 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void AddBitToBinary(ref uint bin, int bit)
        {
            if (bit < 0 || bit >= 32)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            bin |= (1U << bit);
        }

        /// <summary>
        /// 将 32 位无符号整数的指定二进制位设为 0。
        /// </summary>
        /// <param name="bin">32 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void RemoveBitFromBinary(ref uint bin, int bit)
        {
            if (bit < 0 || bit >= 32)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            bin &= (~(1U << bit));
        }

        /// <summary>
        /// 将 32 位无符号整数的指定二进制位反转。
        /// </summary>
        /// <param name="bin">32 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void InverseBitOfBinary(ref uint bin, int bit)
        {
            if (bit < 0 || bit >= 32)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            bin ^= (1U << bit);
        }

        /// <summary>
        /// 判断 32 位无符号整数的指定二进制位是否为 1。
        /// </summary>
        /// <param name="bin">32 位无符号整数。</param>
        /// <param name="bit">指定的二进制位。</param>
        /// <returns>布尔值，表示 32 位无符号整数的指定二进制位是否为 1。</returns>
        public static bool BinaryHasBit(uint bin, int bit)
        {
            if (bit < 0 || bit >= 32)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return ((bin & (1U << bit)) != 0);
        }

        /// <summary>
        /// 计算 32 位无符号整数的值为 1 的二进制位的数量。
        /// </summary>
        /// <param name="bin">32 位无符号整数。</param>
        /// <returns>32 位无符号整数的值为 1 的二进制位的数量。</returns>
        public static int GetBit1CountOfBinary(uint bin)
        {
            int Count = 0;

            while (bin > 0)
            {
                bin &= (bin - 1);

                Count++;
            }

            return Count;
        }

        /// <summary>
        /// 计算 32 位无符号整数的值为 0 的二进制位的数量。
        /// </summary>
        /// <param name="bin">32 位无符号整数。</param>
        /// <returns>32 位无符号整数的值为 0 的二进制位的数量。</returns>
        public static int GetBit0CountOfBinary(uint bin)
        {
            return (32 - GetBit1CountOfBinary(bin));
        }

        /// <summary>
        /// 返回一个列表，列表元素表示 32 位无符号整数值为 1 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。
        /// </summary>
        /// <param name="bin">32 位无符号整数。</param>
        /// <returns>32 位整数列表，列表元素表示 32 位无符号整数值为 1 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。</returns>
        public static List<int> GetBit1IndexOfBinary(uint bin)
        {
            List<int> result = new List<int>(32);

            for (int i = 0; i < 32; i++)
            {
                if ((bin & 1) != 0)
                {
                    result.Add(i);
                }

                bin >>= 1;
            }

            return result;
        }

        /// <summary>
        /// 返回一个列表，列表元素表示 32 位无符号整数值为 0 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。
        /// </summary>
        /// <param name="bin">32 位无符号整数。</param>
        /// <returns>32 位整数列表，列表元素表示 32 位无符号整数值为 0 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。</returns>
        public static List<int> GetBit0IndexOfBinary(uint bin)
        {
            List<int> result = new List<int>(32);

            for (int i = 0; i < 32; i++)
            {
                if ((bin & 1) == 0)
                {
                    result.Add(i);
                }

                bin >>= 1;
            }

            return result;
        }

        #endregion

        #region 64 位

        /// <summary>
        /// 返回一个仅指定二进制位为 1 的 64 位无符号整数。
        /// </summary>
        /// <param name="bit">指定的二进制位。</param>
        /// <returns>64 位无符号整数，该 64 位无符号整数仅指定二进制位为 1。</returns>
        public static ulong GetBinary64WithSingleBit1(int bit)
        {
            if (bit < 0 || bit >= 64)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return (1UL << bit);
        }

        /// <summary>
        /// 返回一个仅指定二进制位为 0 的 64 位无符号整数。
        /// </summary>
        /// <param name="bit">指定的二进制位。</param>
        /// <returns>64 位无符号整数，该 64 位无符号整数仅指定二进制位为 0。</returns>
        public static ulong GetBinary64WithSingleBit0(int bit)
        {
            if (bit < 0 || bit >= 64)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return (~(1UL << bit));
        }

        /// <summary>
        /// 将 64 位无符号整数的指定二进制位设为 1。
        /// </summary>
        /// <param name="bin">64 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void AddBitToBinary(ref ulong bin, int bit)
        {
            if (bit < 0 || bit >= 64)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            bin |= (1UL << bit);
        }

        /// <summary>
        /// 将 64 位无符号整数的指定二进制位设为 0。
        /// </summary>
        /// <param name="bin">64 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void RemoveBitFromBinary(ref ulong bin, int bit)
        {
            if (bit < 0 || bit >= 64)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            bin &= (~(1UL << bit));
        }

        /// <summary>
        /// 将 64 位无符号整数的指定二进制位反转。
        /// </summary>
        /// <param name="bin">64 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void InverseBitOfBinary(ref ulong bin, int bit)
        {
            if (bit < 0 || bit >= 64)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            bin ^= (1UL << bit);
        }

        /// <summary>
        /// 判断 64 位无符号整数的指定二进制位是否为 1。
        /// </summary>
        /// <param name="bin">64 位无符号整数。</param>
        /// <param name="bit">指定的二进制位。</param>
        /// <returns>布尔值，表示 64 位无符号整数的指定二进制位是否为 1。</returns>
        public static bool BinaryHasBit(ulong bin, int bit)
        {
            if (bit < 0 || bit >= 64)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return ((bin & (1UL << bit)) != 0);
        }

        /// <summary>
        /// 计算 64 位无符号整数的值为 1 的二进制位的数量。
        /// </summary>
        /// <param name="bin">64 位无符号整数。</param>
        /// <returns>64 位无符号整数的值为 1 的二进制位的数量。</returns>
        public static int GetBit1CountOfBinary(ulong bin)
        {
            int Count = 0;

            while (bin > 0)
            {
                bin &= (bin - 1);

                Count++;
            }

            return Count;
        }

        /// <summary>
        /// 计算 64 位无符号整数的值为 0 的二进制位的数量。
        /// </summary>
        /// <param name="bin">64 位无符号整数。</param>
        /// <returns>64 位无符号整数的值为 0 的二进制位的数量。</returns>
        public static int GetBit0CountOfBinary(ulong bin)
        {
            return (64 - GetBit1CountOfBinary(bin));
        }

        /// <summary>
        /// 返回一个列表，列表元素表示 64 位无符号整数值为 1 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。
        /// </summary>
        /// <param name="bin">64 位无符号整数。</param>
        /// <returns>32 位整数列表，列表元素表示 64 位无符号整数值为 1 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。</returns>
        public static List<int> GetBit1IndexOfBinary(ulong bin)
        {
            List<int> result = new List<int>(64);

            for (int i = 0; i < 64; i++)
            {
                if ((bin & 1) != 0)
                {
                    result.Add(i);
                }

                bin >>= 1;
            }

            return result;
        }

        /// <summary>
        /// 返回一个列表，列表元素表示 64 位无符号整数值为 0 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。
        /// </summary>
        /// <param name="bin">64 位无符号整数。</param>
        /// <returns>32 位整数列表，列表元素表示 64 位无符号整数值为 0 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。</returns>
        public static List<int> GetBit0IndexOfBinary(ulong bin)
        {
            List<int> result = new List<int>(64);

            for (int i = 0; i < 64; i++)
            {
                if ((bin & 1) == 0)
                {
                    result.Add(i);
                }

                bin >>= 1;
            }

            return result;
        }

        #endregion
    }
}