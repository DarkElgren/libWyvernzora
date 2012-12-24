/* =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
 * libWyvernzora.IO.ExStream.cs
 *      - Wrapper for any System.IO.Stream object that provides extended read/write
 *          functionality and ExPartStream support.
 *      - Can be used for creating both FileStream and MemoryStream objects
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

namespace libWyvernzora.IO
{
    /// <summary>
    /// System.IO.Stream object wrapper with extended read/write methods
    /// and support for ExPartStreams.
    /// </summary>
    public class ExStream : ExStreamBase
    {
        public ExStream(Stream bSream)
        {
            m_base = bSream;
        }
        public ExStream(String path, FileMode fmode = FileMode.Open, FileAccess faccess = FileAccess.ReadWrite)
        {
            m_base = new FileStream(path, fmode, faccess);
        }
        public ExStream()
        {
            m_base = new MemoryStream();
        }

        public override bool CanRead
        {
            get { return m_base.CanRead; }
        }
        public override bool CanSeek
        {
            get { return m_base.CanSeek; }
        }
        public override bool CanWrite
        {
            get { return m_base.CanWrite; }
        }
        public override void Flush()
        {
            m_base.Flush();
        }
        public override long Length
        {
            get { return m_base.Length; }
        }
        public override long Position
        {
            get
            {
                return m_base.Position;
            }
            set
            {
                m_base.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return m_base.Read(buffer, offset, count);
        }
        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            return m_base.Seek(offset, origin);
        }
        public override void SetLength(long value)
        {
            m_base.SetLength(value);
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            m_base.Write(buffer, offset, count);
        }
        public override void Close()
        {
            m_base.Close();
        }
    }
}
