/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.BitOperation
Version 18.9.28.2200

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
        public static byte GetBinary8WithSingleBit1(int bit)
        {
            try
            {
                if (bit >= 0 && bit < 8)
                {
                    return (byte)(((uint)1) << bit);
                }

                return byte.MinValue;
            }
            catch
            {
                return byte.MinValue;
            }
        }

        /// <summary>
        /// 返回一个仅指定二进制位为 0 的 8 位无符号整数。
        /// </summary>
        /// <param name="bit">指定的二进制位。</param>
        public static byte GetBinary8WithSingleBit0(int bit)
        {
            try
            {
                if (bit >= 0 && bit < 8)
                {
                    return (byte)(~(((uint)1) << bit));
                }

                return byte.MaxValue;
            }
            catch
            {
                return byte.MaxValue;
            }
        }

        /// <summary>
        /// 将 8 位无符号整数的指定二进制位设为 1。
        /// </summary>
        /// <param name="bin">8 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void AddBitToBinary(ref byte bin, int bit)
        {
            try
            {
                if (bit >= 0 && bit < 8)
                {
                    bin |= (byte)(((uint)1) << bit);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 8 位无符号整数的指定二进制位设为 0。
        /// </summary>
        /// <param name="bin">8 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void RemoveBitFromBinary(ref byte bin, int bit)
        {
            try
            {
                if (bit >= 0 && bit < 8)
                {
                    bin &= (byte)(~(((uint)1) << bit));
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 8 位无符号整数的指定二进制位反转。
        /// </summary>
        /// <param name="bin">8 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void InverseBitOfBinary(ref byte bin, int bit)
        {
            try
            {
                if (bit >= 0 && bit < 8)
                {
                    bin ^= (byte)(((uint)1) << bit);
                }
            }
            catch { }
        }

        /// <summary>
        /// 判断 8 位无符号整数的指定二进制位是否为 1。
        /// </summary>
        /// <param name="bin">8 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static bool BinaryHasBit(byte bin, int bit)
        {
            try
            {
                if (bit >= 0 && bit < 8)
                {
                    return ((bin & (((uint)1) << bit)) != 0);
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算 8 位无符号整数的值为 1 的二进制位的数量。
        /// </summary>
        /// <param name="bin">8 位无符号整数。</param>
        public static int GetBit1CountOfBinary(byte bin)
        {
            try
            {
                int Count = 0;

                while (bin > 0)
                {
                    bin &= (byte)(bin - ((uint)1));

                    Count++;
                }

                return Count;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 计算 8 位无符号整数的值为 0 的二进制位的数量。
        /// </summary>
        /// <param name="bin">8 位无符号整数。</param>
        public static int GetBit0CountOfBinary(byte bin)
        {
            try
            {
                return (8 - GetBit1CountOfBinary(bin));
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 返回一个整数列表，列表元素表示 8 位无符号整数值为 1 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。
        /// </summary>
        /// <param name="bin">8 位无符号整数。</param>
        public static List<int> GetBit1IndexOfBinary(byte bin)
        {
            try
            {
                List<int> result = new List<int>(8);

                for (int i = 0; i < 8; i++)
                {
                    if ((bin & ((uint)1)) != 0)
                    {
                        result.Add(i);
                    }

                    bin >>= 1;
                }

                return result;
            }
            catch
            {
                return new List<int>(0);
            }
        }

        /// <summary>
        /// 返回一个整数列表，列表元素表示 8 位无符号整数值为 0 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。
        /// </summary>
        /// <param name="bin">8 位无符号整数。</param>
        public static List<int> GetBit0IndexOfBinary(byte bin)
        {
            try
            {
                List<int> result = new List<int>(8);

                for (int i = 0; i < 8; i++)
                {
                    if ((bin & ((uint)1)) == 0)
                    {
                        result.Add(i);
                    }

                    bin >>= 1;
                }

                return result;
            }
            catch
            {
                return new List<int>(0);
            }
        }

        #endregion

        #region 16 位

        /// <summary>
        /// 返回一个仅指定二进制位为 1 的 16 位无符号整数。
        /// </summary>
        /// <param name="bit">指定的二进制位。</param>
        public static ushort GetBinary16WithSingleBit1(int bit)
        {
            try
            {
                if (bit >= 0 && bit < 16)
                {
                    return (ushort)(((uint)1) << bit);
                }

                return ushort.MinValue;
            }
            catch
            {
                return ushort.MinValue;
            }
        }

        /// <summary>
        /// 返回一个仅指定二进制位为 0 的 16 位无符号整数。
        /// </summary>
        /// <param name="bit">指定的二进制位。</param>
        public static ushort GetBinary16WithSingleBit0(int bit)
        {
            try
            {
                if (bit >= 0 && bit < 16)
                {
                    return (ushort)(~(((uint)1) << bit));
                }

                return ushort.MaxValue;
            }
            catch
            {
                return ushort.MaxValue;
            }
        }

        /// <summary>
        /// 将 16 位无符号整数的指定二进制位设为 1。
        /// </summary>
        /// <param name="bin">16 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void AddBitToBinary(ref ushort bin, int bit)
        {
            try
            {
                if (bit >= 0 && bit < 16)
                {
                    bin |= (ushort)(((uint)1) << bit);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 16 位无符号整数的指定二进制位设为 0。
        /// </summary>
        /// <param name="bin">16 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void RemoveBitFromBinary(ref ushort bin, int bit)
        {
            try
            {
                if (bit >= 0 && bit < 16)
                {
                    bin &= (ushort)(~(((uint)1) << bit));
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 16 位无符号整数的指定二进制位反转。
        /// </summary>
        /// <param name="bin">16 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void InverseBitOfBinary(ref ushort bin, int bit)
        {
            try
            {
                if (bit >= 0 && bit < 16)
                {
                    bin ^= (ushort)(((uint)1) << bit);
                }
            }
            catch { }
        }

        /// <summary>
        /// 判断 16 位无符号整数的指定二进制位是否为 1。
        /// </summary>
        /// <param name="bin">16 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static bool BinaryHasBit(ushort bin, int bit)
        {
            try
            {
                if (bit >= 0 && bit < 16)
                {
                    return ((bin & (((uint)1) << bit)) != 0);
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算 16 位无符号整数的值为 1 的二进制位的数量。
        /// </summary>
        /// <param name="bin">16 位无符号整数。</param>
        public static int GetBit1CountOfBinary(ushort bin)
        {
            try
            {
                int Count = 0;

                while (bin > 0)
                {
                    bin &= (ushort)(bin - ((uint)1));

                    Count++;
                }

                return Count;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 计算 16 位无符号整数的值为 0 的二进制位的数量。
        /// </summary>
        /// <param name="bin">16 位无符号整数。</param>
        public static int GetBit0CountOfBinary(ushort bin)
        {
            try
            {
                return (16 - GetBit1CountOfBinary(bin));
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 返回一个整数列表，列表元素表示 16 位无符号整数值为 1 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。
        /// </summary>
        /// <param name="bin">16 位无符号整数。</param>
        public static List<int> GetBit1IndexOfBinary(ushort bin)
        {
            try
            {
                List<int> result = new List<int>(16);

                for (int i = 0; i < 16; i++)
                {
                    if ((bin & ((uint)1)) != 0)
                    {
                        result.Add(i);
                    }

                    bin >>= 1;
                }

                return result;
            }
            catch
            {
                return new List<int>(0);
            }
        }

        /// <summary>
        /// 返回一个整数列表，列表元素表示 16 位无符号整数值为 0 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。
        /// </summary>
        /// <param name="bin">16 位无符号整数。</param>
        public static List<int> GetBit0IndexOfBinary(ushort bin)
        {
            try
            {
                List<int> result = new List<int>(16);

                for (int i = 0; i < 16; i++)
                {
                    if ((bin & ((uint)1)) == 0)
                    {
                        result.Add(i);
                    }

                    bin >>= 1;
                }

                return result;
            }
            catch
            {
                return new List<int>(0);
            }
        }

        #endregion

        #region 32 位

        /// <summary>
        /// 返回一个仅指定二进制位为 1 的 32 位无符号整数。
        /// </summary>
        /// <param name="bit">指定的二进制位。</param>
        public static uint GetBinary32WithSingleBit1(int bit)
        {
            try
            {
                if (bit >= 0 && bit < 32)
                {
                    return (((uint)1) << bit);
                }

                return uint.MinValue;
            }
            catch
            {
                return uint.MinValue;
            }
        }

        /// <summary>
        /// 返回一个仅指定二进制位为 0 的 32 位无符号整数。
        /// </summary>
        /// <param name="bit">指定的二进制位。</param>
        public static uint GetBinary32WithSingleBit0(int bit)
        {
            try
            {
                if (bit >= 0 && bit < 32)
                {
                    return (~(((uint)1) << bit));
                }

                return uint.MaxValue;
            }
            catch
            {
                return uint.MaxValue;
            }
        }

        /// <summary>
        /// 将 32 位无符号整数的指定二进制位设为 1。
        /// </summary>
        /// <param name="bin">32 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void AddBitToBinary(ref uint bin, int bit)
        {
            try
            {
                if (bit >= 0 && bit < 32)
                {
                    bin |= (((uint)1) << bit);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 32 位无符号整数的指定二进制位设为 0。
        /// </summary>
        /// <param name="bin">32 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void RemoveBitFromBinary(ref uint bin, int bit)
        {
            try
            {
                if (bit >= 0 && bit < 32)
                {
                    bin &= (~(((uint)1) << bit));
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 32 位无符号整数的指定二进制位反转。
        /// </summary>
        /// <param name="bin">32 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void InverseBitOfBinary(ref uint bin, int bit)
        {
            try
            {
                if (bit >= 0 && bit < 32)
                {
                    bin ^= (((uint)1) << bit);
                }
            }
            catch { }
        }

        /// <summary>
        /// 判断 32 位无符号整数的指定二进制位是否为 1。
        /// </summary>
        /// <param name="bin">32 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static bool BinaryHasBit(uint bin, int bit)
        {
            try
            {
                if (bit >= 0 && bit < 32)
                {
                    return ((bin & (((uint)1) << bit)) != 0);
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算 32 位无符号整数的值为 1 的二进制位的数量。
        /// </summary>
        /// <param name="bin">32 位无符号整数。</param>
        public static int GetBit1CountOfBinary(uint bin)
        {
            try
            {
                int Count = 0;

                while (bin > 0)
                {
                    bin &= (bin - 1);

                    Count++;
                }

                return Count;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 计算 32 位无符号整数的值为 0 的二进制位的数量。
        /// </summary>
        /// <param name="bin">32 位无符号整数。</param>
        public static int GetBit0CountOfBinary(uint bin)
        {
            try
            {
                return (32 - GetBit1CountOfBinary(bin));
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 返回一个整数列表，列表元素表示 32 位无符号整数值为 1 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。
        /// </summary>
        /// <param name="bin">32 位无符号整数。</param>
        public static List<int> GetBit1IndexOfBinary(uint bin)
        {
            try
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
            catch
            {
                return new List<int>(0);
            }
        }

        /// <summary>
        /// 返回一个整数列表，列表元素表示 32 位无符号整数值为 0 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。
        /// </summary>
        /// <param name="bin">32 位无符号整数。</param>
        public static List<int> GetBit0IndexOfBinary(uint bin)
        {
            try
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
            catch
            {
                return new List<int>(0);
            }
        }

        #endregion

        #region 64 位

        /// <summary>
        /// 返回一个仅指定二进制位为 1 的 64 位无符号整数。
        /// </summary>
        /// <param name="bit">指定的二进制位。</param>
        public static ulong GetBinary64WithSingleBit1(int bit)
        {
            try
            {
                if (bit >= 0 && bit < 64)
                {
                    return (((ulong)1) << bit);
                }

                return ulong.MinValue;
            }
            catch
            {
                return ulong.MinValue;
            }
        }

        /// <summary>
        /// 返回一个仅指定二进制位为 0 的 64 位无符号整数。
        /// </summary>
        /// <param name="bit">指定的二进制位。</param>
        public static ulong GetBinary64WithSingleBit0(int bit)
        {
            try
            {
                if (bit >= 0 && bit < 64)
                {
                    return (~(((ulong)1) << bit));
                }

                return ulong.MaxValue;
            }
            catch
            {
                return ulong.MaxValue;
            }
        }

        /// <summary>
        /// 将 64 位无符号整数的指定二进制位设为 1。
        /// </summary>
        /// <param name="bin">64 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void AddBitToBinary(ref ulong bin, int bit)
        {
            try
            {
                if (bit >= 0 && bit < 64)
                {
                    bin |= (((ulong)1) << bit);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 64 位无符号整数的指定二进制位设为 0。
        /// </summary>
        /// <param name="bin">64 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void RemoveBitFromBinary(ref ulong bin, int bit)
        {
            try
            {
                if (bit >= 0 && bit < 64)
                {
                    bin &= (~(((ulong)1) << bit));
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 64 位无符号整数的指定二进制位反转。
        /// </summary>
        /// <param name="bin">64 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static void InverseBitOfBinary(ref ulong bin, int bit)
        {
            try
            {
                if (bit >= 0 && bit < 64)
                {
                    bin ^= (((ulong)1) << bit);
                }
            }
            catch { }
        }

        /// <summary>
        /// 判断 64 位无符号整数的指定二进制位是否为 1。
        /// </summary>
        /// <param name="bin">64 位无符号整数。</param>
        /// <param name="bit">需要处理的二进制位。</param>
        public static bool BinaryHasBit(ulong bin, int bit)
        {
            try
            {
                if (bit >= 0 && bit < 64)
                {
                    return ((bin & (((ulong)1) << bit)) != 0);
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算 64 位无符号整数的值为 1 的二进制位的数量。
        /// </summary>
        /// <param name="bin">64 位无符号整数。</param>
        public static int GetBit1CountOfBinary(ulong bin)
        {
            try
            {
                int Count = 0;

                while (bin > 0)
                {
                    bin &= (bin - 1);

                    Count++;
                }

                return Count;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 计算 64 位无符号整数的值为 0 的二进制位的数量。
        /// </summary>
        /// <param name="bin">64 位无符号整数。</param>
        public static int GetBit0CountOfBinary(ulong bin)
        {
            try
            {
                return (64 - GetBit1CountOfBinary(bin));
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 返回一个整数列表，列表元素表示 64 位无符号整数值为 1 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。
        /// </summary>
        /// <param name="bin">64 位无符号整数。</param>
        public static List<int> GetBit1IndexOfBinary(ulong bin)
        {
            try
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
            catch
            {
                return new List<int>(0);
            }
        }

        /// <summary>
        /// 返回一个整数列表，列表元素表示 64 位无符号整数值为 0 的二进制位在该整数的二进制序列中从低位到高位的次序数（最低位的次序数为 0）。
        /// </summary>
        /// <param name="bin">64 位无符号整数。</param>
        public static List<int> GetBit0IndexOfBinary(ulong bin)
        {
            try
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
            catch
            {
                return new List<int>(0);
            }
        }

        #endregion
    }
}