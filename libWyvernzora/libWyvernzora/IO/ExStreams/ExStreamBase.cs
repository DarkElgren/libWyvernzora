/* =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
 * libWyvernzora.IO.ExStreamBase.cs
 *      - Abstract base class for all extended streams
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
using System.IO;

namespace libWyvernzora.IO
{
    /// <summary>
    /// Base class for all Extended Stream classes
    /// </summary>
    public abstract class ExStreamBase : Stream
    {
        //Base Stream
        protected Stream m_base;
        
        //Concrete Functions
        public Byte[] ReadBytes(Int32 count)
        {
            Byte[] t_b = new Byte[count];
            this.Read(t_b, 0, count);
            return t_b;
        }
        public new Byte ReadByte()
        {
            Byte[] t_b = ReadBytes(1);
            return t_b[0];
        }
        public SByte ReadSByte()
        { return ReadByte().Sign(); }
        public UInt16 ReadUInt16(BitSequence seq = BitSequence.LittleEndian)
        { return ReadBytes(2).ToUInt16(0, seq); }
        public Int16 ReadInt16(BitSequence seq = BitSequence.LittleEndian)
        { return ReadBytes(2).ToInt16(0, seq); }
        public UInt32 ReadUInt32(BitSequence seq = BitSequence.LittleEndian)
        { return ReadBytes(4).ToUInt32(0, seq); }
        public Int32 ReadInt32(BitSequence seq = BitSequence.LittleEndian)
        { return ReadBytes(4).ToInt32(0, seq); }
        public UInt64 ReadUInt64(BitSequence seq = BitSequence.LittleEndian)
        { return ReadBytes(8).ToUInt64(0, seq); }
        public Int64 ReadInt64(BitSequence seq = BitSequence.LittleEndian)
        { return ReadBytes(8).ToInt64(0, seq); }



        public void WriteBytes(Byte[] b)
        { Write(b, 0, b.Length); }
        public void WriteSByte(SByte b)
        { WriteByte(b.Unsign()); }
        public void WriteUShort(UInt16 b, BitSequence seq = BitSequence.LittleEndian)
        { Write(b.ToBinary(seq), 0, 2); }
        public void WriteShort(Int16 b, BitSequence seq = BitSequence.LittleEndian)
        { Write(b.ToBinary(seq), 0, 2); }
        public void WriteUInt(UInt32 b, BitSequence seq = BitSequence.LittleEndian)
        { Write(b.ToBinary(seq), 0, 4); }
        public void WriteInt(Int32 b, BitSequence seq = BitSequence.LittleEndian)
        { Write(b.ToBinary(seq), 0, 4); }
        public void WriteULong(UInt64 b, BitSequence seq = BitSequence.LittleEndian)
        { Write(b.ToBinary(seq), 0, 8); }
        public void WriteLong(Int64 b, BitSequence seq = BitSequence.LittleEndian)
        { Write(b.ToBinary(seq), 0, 8); }

        public void WriteTo(Stream dest, int buffer = 0x1000)
        {
            if (this.CanSeek) this.Seek(0, SeekOrigin.Begin);
            Byte[] BUFFER;
            while (true)
            {
                BUFFER = new Byte[buffer];
                Int32 READ_COUNT = Read(BUFFER, 0, buffer);
                dest.Write(BUFFER, 0, READ_COUNT);
                if (READ_COUNT < buffer) break;
            }
        }

        public override void Close()
        { if (m_base != null) m_base.Close(); }
        public new void Dispose()
        { if (m_base != null)  m_base.Close(); }
    }
}
