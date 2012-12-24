/* =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
 * libWyvernzora.IO.StreamTools.cs
 *      - Extension methods for reading/writing numeric data to byte streams
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
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace libWyvernzora
{
    /// <summary>
    /// Class that extends I/O ability of Stream-derived classes
    /// </summary>
    public static class StreamTools
    {
        public static Byte[] ReadBytes(this Stream s, Int32 count)
        {
            Byte[] buffer = new Byte[count];
            s.Read(buffer, 0, count);
            return buffer;
        }
        public static void WriteBytes(this Stream s, Byte[] src)
        { s.Write(src, 0, src.Length); }

        //Advanced reading functions that require explicit byte-sequence param
        public static SByte ReadSByte(this Stream s)
        { return Convert.ToByte(s.ReadByte()).Sign(); }
        public static Int16 ReadInt16(this Stream s, BitSequence seq = BitSequence.LittleEndian)
        { return ReadBytes(s, 2).ToInt16(0, seq); }
        public static UInt16 ReadUInt16(this Stream s, BitSequence seq = BitSequence.LittleEndian)
        { return ReadBytes(s,2).ToUInt16(0, seq); }
        public static Int32 ReadInt32(this Stream s, BitSequence seq = BitSequence.LittleEndian)
        { return ReadBytes(s, 4).ToInt32(0, seq); }
        public static UInt32 ReadUInt32(this Stream s, BitSequence seq = BitSequence.LittleEndian)
        { return ReadBytes(s, 4).ToUInt32(0, seq); }
        public static Int64 ReadInt64(this Stream s, BitSequence seq = BitSequence.LittleEndian)
        { return ReadBytes(s, 8).ToInt64(0, seq); }
        public static UInt64 ReadUInt64(this Stream s, BitSequence seq = BitSequence.LittleEndian)
        { return ReadBytes(s, 8).ToUInt64(0, seq); }
        public static Single ReadSingle(this Stream s, BitSequence seq = BitSequence.LittleEndian)
        { return ReadBytes(s, 4).ToSingle(0, seq); }
        public static Double ReadDouble(this Stream s, BitSequence seq = BitSequence.LittleEndian)
        { return ReadBytes(s, 8).ToDouble(0, seq); }

        //Advanced writing functions that require eplicit byte-sequence param
        public static void Write(this Stream s, SByte i)
        { s.WriteByte(i.Unsign()); }
        public static void Write(this Stream s, Int16 i, BitSequence seq = BitSequence.LittleEndian)
        { s.Write(i.ToBinary(seq), 0, 2); }
        public static void Write(this Stream s, UInt16 i, BitSequence seq = BitSequence.LittleEndian)
        { s.Write(i.ToBinary(seq), 0, 2); }
        public static void Write(this Stream s, Int32 i, BitSequence seq = BitSequence.LittleEndian)
        { s.Write(i.ToBinary(seq), 0, 4); }
        public static void Write(this Stream s, UInt32 i, BitSequence seq = BitSequence.LittleEndian)
        { s.Write(i.ToBinary(seq), 0, 4); }
        public static void Write(this Stream s, Int64 i, BitSequence seq = BitSequence.LittleEndian)
        { s.Write(i.ToBinary(seq), 0, 8); }
        public static void Write(this Stream s, UInt64 i, BitSequence seq = BitSequence.LittleEndian)
        { s.Write(i.ToBinary(seq), 0, 8); }
        public static void Write(this Stream s, Single i, BitSequence seq = BitSequence.LittleEndian)
        { s.Write(i.ToBinary(seq), 0, 4); }
        public static void Write(this Stream s, Double i, BitSequence seq = BitSequence.LittleEndian)
        { s.Write(i.ToBinary(seq), 0, 8); }

        //String-related functions, basically I/O for ASCII and UTF-8 strings
        //TO BE FINISHED LATER
        public static String ReadASCII(this Stream s, Int32 count)
        {
            return new string(Encoding.ASCII.GetChars(ReadBytes(s,count)));
        }
        public static Int32 WriteASCII(this Stream s, String src)
        {
            Byte[] tmp = Encoding.ASCII.GetBytes(src);
            s.Write(tmp, 0, tmp.Length);
            return tmp.Length;
        }
        public static String ReadString(this Stream s, Int32 count)
        { return new string(Encoding.UTF8.GetChars(ReadBytes(s,count))); }
        public static Int32 WriteString(this Stream s, string src)
        {
            Byte[] tmp = Encoding.UTF8.GetBytes(src);
            s.Write(tmp, 0, tmp.Length);
            return tmp.Length;
        }
        public static void WriteStringWithMetadata(this Stream s, String src)
        {
            Byte[] tmp = Encoding.UTF8.GetBytes(src);
            Write(s, (UInt16)tmp.Length);
            s.Write(tmp, 0, tmp.Length);
        }
        public static String ReadStringWithMetadata(this Stream s)
        {
            UInt16 count = ReadUInt16(s);
            return ReadString(s, count);
        }
        public static Int32 Peek(this Stream s, Byte[] array, Int32 offset, Int32 count)
        {
            Int64 c_pos = s.Position;
            Int32 response = s.Read(array, offset, count);
            s.Position = c_pos;
            return response;
        }

        //Inter-stream I/O
        public static void WriteTo(this Stream s, Stream d)
        {
            if (s.CanSeek) { s.Seek(0, SeekOrigin.Begin); }
            Byte[] buffer = new Byte[4096];
            while (true)
            {
                Int32 count = s.Read(buffer, 0, 4096);
                d.Write(buffer, 0, count);
                if (count < 4096)
                    break;
            }
        }
    }
}
