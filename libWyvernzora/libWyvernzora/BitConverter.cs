/* =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
 * libWyvernzora.BitConverter.cs
 *      - Extension methods for converting numeric types to/from binary form
 *      - Extension methods for converting numeric types to padded hexadecimal string
 * --------------------------------------------------------------------------------
 * Copyright (C) 2010-2012, Jieni Luchijinzhou a.k.a Aragorn Wyvernzora
 * All rights reserved.
 * 
 * This code file is published under terms of BSD 3-Clause License.
 * For more information please refer to License.txt distributed with this code file.
 * 
 * =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
 */

using System;
using System.Runtime.InteropServices;

namespace libWyvernzora
{
    /// <summary>
    /// Represents "Endianness" of a number value
    /// </summary>
    public enum BitSequence
    { LittleEndian = 0, BigEndian = 1 }

    /// <summary>
    /// Static class containing extension methods for converting 
    /// numeric types to/from binary form
    /// </summary>
    public static class BitConverter
    {
        #region Internal Structures

        //Decimal -> Hexadecimal
        [StructLayout(LayoutKind.Explicit)]
        private struct MidSingle
        {
            [FieldOffset(0)]
            public UInt32 intValue;
            [FieldOffset(0)]
            public Single singleValue;
            public static MidSingle Init()
            { MidSingle m; m.intValue = 0; m.singleValue = 0; return m; }
        }
        [StructLayout(LayoutKind.Explicit)]
        private struct MidDouble
        {
            [FieldOffset(0)]
            public UInt64 intValue;
            [FieldOffset(0)]
            public Double doubleValue;
            public static MidDouble Init()
            { MidDouble m; m.intValue = 0; m.doubleValue = 0; return m; }
        }

        #endregion

        public static SByte Sign(this Byte i)
        { return (SByte)i; }
        public static Int16 Sign(this UInt16 i)
        { return (Int16)i; }
        public static Int32 Sign(this UInt32 i)
        { return (Int32)i; }
        public static Int64 Sign(this UInt64 i)
        { return (Int64)i; }
        public static Byte Unsign(this SByte i)
        { return (Byte)i; }
        public static UInt16 Unsign(this Int16 i)
        { return (UInt16)i; }
        public static UInt32 Unsign(this Int32 i)
        { return (UInt32)i; }
        public static UInt64 Unsign(this Int64 i)
        { return (UInt64)i; }

        public static Byte[] ToBinary(this UInt16 src, BitSequence seq = BitSequence.LittleEndian)
        {
            Byte[] result = new Byte[2];
            result[1] = Convert.ToByte(src & 0xFF);
            result[0] = Convert.ToByte((src >> 8) & 0xFF);

            if (seq == BitSequence.LittleEndian)
            { Array.Reverse(result); }
            return result;
        }
        public static Byte[] ToBinary(this Int16 src, BitSequence seq = BitSequence.LittleEndian)
        { return Unsign(src).ToBinary(seq); }
        public static Byte[] ToBinary(this UInt32 src, BitSequence seq = BitSequence.LittleEndian)
        {
            Byte[] result = new Byte[4];
            for (int i = 0; i < 4; i++)
            { result[3 - i] = Convert.ToByte(src & 0xFF); src >>= 8; }

            if (seq == BitSequence.LittleEndian)
            { Array.Reverse(result); }
            return result;
        }
        public static Byte[] ToBinary(this Int32 src, BitSequence seq = BitSequence.LittleEndian)
        { return Unsign(src).ToBinary(seq); }
        public static Byte[] ToBinary(this UInt64 src, BitSequence seq = BitSequence.LittleEndian)
        {
            Byte[] result = new Byte[8];
            for (int i = 0; i < 8; i++)
            { result[7 - i] = Convert.ToByte(src & 0xFF); src >>= 8; }

            if (seq == BitSequence.LittleEndian)
            { Array.Reverse(result); }
            return result;
        }
        public static Byte[] ToBinary(this Int64 src, BitSequence seq = BitSequence.LittleEndian)
        { return Unsign(src).ToBinary(seq); }
        public static Byte[] ToBinary(this Single src, BitSequence seq = BitSequence.LittleEndian)
        {
            MidSingle m = MidSingle.Init();
            m.singleValue = src;
            return m.intValue.ToBinary(seq);
        }
        public static Byte[] ToBinary(this Double src, BitSequence seq = BitSequence.LittleEndian)
        {
            MidDouble m = MidDouble.Init();
            m.doubleValue = src;
            return m.intValue.ToBinary(seq);
        }

        public static UInt16 ToUInt16(this Byte[] src, Int32 offset, BitSequence seq = BitSequence.LittleEndian)
        {
            if (offset < 0 || offset > src.Length - 2)
                throw new ArgumentOutOfRangeException("offset", offset, "Byte[].ToUInt16(): Offset out of source array bounds");

            UInt16 result;
            if (seq == BitSequence.LittleEndian)
            {
                result = src[0];
                result |= Convert.ToUInt16(Convert.ToUInt16(src[1]) << 8);
            }
            else
            {
                result = src[1];
                result |= Convert.ToUInt16(Convert.ToUInt16(src[0]) << 8);
            }

            return result;
        }
        public static Int16 ToInt16(this Byte[] src, Int32 offset, BitSequence seq = BitSequence.LittleEndian)
        {
            if (offset < 0 || offset > src.Length - 2)
                throw new ArgumentOutOfRangeException("offset", offset, "Byte[].ToInt16(): Offset out of source array bounds");

            return Sign(src.ToUInt16(offset, seq));
        }
        public static UInt32 ToUInt32(this Byte[] src, Int32 offset, BitSequence seq = BitSequence.LittleEndian)
        {
            if (offset < 0 || offset > src.Length - 4)
                throw new ArgumentOutOfRangeException("offset", offset, "Byte[].ToUInt32(): Offset out of source array bounds");

            UInt32 result;
            if (seq == BitSequence.LittleEndian)
            {
                result = src[0];
                for (int i = 1; i < 4; i++)
                { result |= Convert.ToUInt32(Convert.ToUInt32(src[i]) << (i * 8)); }
            }
            else
            {
                result = src[3];
                for (int i = 1; i < 4; i++)
                { result |= Convert.ToUInt32(Convert.ToUInt32(src[3 - i]) << (i * 8)); }
            }
            return result;
        }
        public static Int32 ToInt32(this Byte[] src, Int32 offset, BitSequence seq = BitSequence.LittleEndian)
        {
            if (offset < 0 || offset > src.Length - 4)
                throw new ArgumentOutOfRangeException("offset", offset, "Byte[].ToInt32(): Offset out of source array bounds");

            return Sign(src.ToUInt32(offset, seq));
        }
        public static UInt64 ToUInt64(this Byte[] src, Int32 offset, BitSequence seq = BitSequence.LittleEndian)
        {
            if (offset < 0 || offset > src.Length - 8)
                throw new ArgumentOutOfRangeException("offset", offset, "Byte[].ToUInt64(): Offset out of source array bounds");

            UInt64 result;
            if (seq == BitSequence.LittleEndian)
            {
                result = src[0];
                for (int i = 1; i < 8; i++)
                { result |= Convert.ToUInt64(Convert.ToUInt64(src[i]) << (i * 8)); }
            }
            else
            {
                result = src[7];
                for (int i = 1; i < 8; i++)
                { result |= Convert.ToUInt64(Convert.ToUInt64(src[7 - i]) << (i * 8)); }
            }

            return result;
        }
        public static Int64 ToInt64(this Byte[] src, Int32 offset, BitSequence seq = BitSequence.LittleEndian)
        {
            if (offset < 0 || offset > src.Length - 8)
                throw new ArgumentOutOfRangeException("offset", offset, "Byte[].ToInt64(): Offset out of source array bounds");

            return Sign(src.ToUInt64(offset, seq));
        }
        public static Single ToSingle(this Byte[] src, Int32 offset, BitSequence seq = BitSequence.LittleEndian)
        {
            if (offset < 0 || offset > src.Length - 4)
                throw new ArgumentOutOfRangeException("offset", offset, "Byte[].ToSingle(): Offset out of source array bounds");

            MidSingle m = MidSingle.Init();
            m.intValue = src.ToUInt32(offset, seq);
            return m.singleValue;
        }
        public static Double ToDouble(this Byte[] src, Int32 offset, BitSequence seq = BitSequence.LittleEndian)
        {
            if (offset < 0 || offset > src.Length - 4)
                throw new ArgumentOutOfRangeException("offset", offset, "Byte[].ToDouble(): Offset out of source array bounds");

            MidDouble m = MidDouble.Init();
            m.intValue = src.ToUInt64(offset, seq);
            return m.doubleValue;
        }

        public static String ToHexString(Int64 src, Int32 padding)
        {
            string r = Convert.ToString(src, 16).ToUpper();
            r = r.PadLeft(padding, '0');
            return r;
        }
    }
}