// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/ArrayStream.cs
// --------------------------------------------------------------------------------
// Copyright (c) 2013, Jieni Luchijinzhou a.k.a Aragorn Wyvernzora
// 
// This file is a part of libWyvernzora.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to do 
// so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

using System;
using libWyvernzora.Core;

namespace libWyvernzora.Collections
{
    /// <summary>
    ///     Generic stream-like interface wrapper for arrays.
    /// </summary>
    /// <typeparam name="T">Type of elements in the stream</typeparam>
    public class ArrayStream<T> : IDisposable
    {
        private readonly Int32 length;
        private readonly Int32 offset;
        private T[] array;
        private Boolean closed;
        private Int32 position;


        /// <summary>
        ///     Overloaded.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="length">Length of the ArrayStream.</param>
        public ArrayStream(Int32 length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length");

            array = new T[length];
            offset = 0;
            position = 0;
            this.length = length;
            closed = false;
        }

        /// <summary>
        ///     Overloaded.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="array">Base array.</param>
        /// <param name="offset">Beginning offset in the base array</param>
        public ArrayStream(T[] array, Int32 offset = 0)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (offset < 0 || offset > array.Length) throw new ArgumentOutOfRangeException("offset");

            this.array = array;
            this.offset = offset;
            position = 0;
            length = array.Length - offset;
            closed = false;
        }

        /// <summary>
        ///     Overloaded.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="array">Base array.</param>
        /// <param name="offset">Beginning offset in the base array</param>
        /// <param name="length">Length of the ArrayStream.</param>
        public ArrayStream(T[] array, Int32 offset, Int32 length)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (length < 0) throw new ArgumentOutOfRangeException("length");
            if (offset < 0 || offset + length > array.Length) throw new ArgumentOutOfRangeException("offset");

            this.array = array;
            this.offset = offset;
            this.length = length;
            position = offset;
            closed = false;
        }

        /// <summary>
        ///     Gets the length of the ArrayStream
        /// </summary>
        public virtual Int64 Length
        {
            get { return length; }
        }

        /// <summary>
        ///     Gets the current position of the ArrayStream
        /// </summary>
        public virtual Int32 Position
        {
            get { return position; }
        }

        /// <summary>
        ///     Reads an element from ArrayStream.
        /// </summary>
        /// <returns></returns>
        public virtual T ReadElement()
        {
            return array[position++];
        }

        /// <summary>
        ///     Writes an element to ArrayStream.
        /// </summary>
        /// <param name="t"></param>
        public virtual void WriteElement(T t)
        {
            array[position++] = t;
        }

        /// <summary>
        ///     Overloaded.
        ///     Reads elements to buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public virtual void Read(T[] buffer, Int32 offset, Int32 count)
        {
            if (count < 0 || position + count > this.offset + length) throw new ArgumentOutOfRangeException();
            Array.Copy(array, position, buffer, offset, count);
            position += count;
        }

        /// <summary>
        ///     Overloaded.
        ///     Reads elements to buffer.
        /// </summary>
        /// <param name="buffer"></param>
        public void Read(T[] buffer)
        {
            Read(buffer, 0, buffer.Length);
        }

        /// <summary>
        ///     Overloaded.
        ///     Reads elements to buffer.
        /// </summary>
        /// <param name="count">Number of elements to read.</param>
        public T[] Read(Int32 count)
        {
            T[] tmp = new T[count];
            Read(tmp, 0, count);
            return tmp;
        }

        /// <summary>
        ///     Overloaded.
        ///     Writes elements to stream.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public virtual void Write(T[] buffer, Int32 offset, Int32 count)
        {
            if (count < 0 || position + count > this.offset + length) throw new ArgumentOutOfRangeException();
            Array.Copy(buffer, offset, array, this.offset, count);
            position += count;
        }

        /// <summary>
        ///     Overloaded.
        ///     Writes elements to stream.
        /// </summary>
        /// <param name="buffer"></param>
        public void Write(T[] buffer)
        {
            Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        ///     Reads contents to another stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="count"></param>
        public void ReadToStream(ArrayStream<T> stream, Int32 count)
        {
            if (count <= 0) return;

            T[] buffer = new T[NumericOps.Min(count, 4096)];
            for (int i = 0; i < count - buffer.Length; i += buffer.Length)
            {
                Read(buffer);
                stream.Write(buffer);
            }
            Int32 remaining = count % buffer.Length;
            Read(buffer, 0, remaining);
            stream.Write(buffer, 0, remaining);
        }

        /// <summary>
        ///     Writes contents from another strea,.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="count"></param>
        public void WriteFromStream(ArrayStream<T> stream, Int32 count)
        {
            if (count <= 0) return;

            T[] buffer = new T[NumericOps.Min(count, 4096)];
            for (int i = 0; i <= count - buffer.Length; i += buffer.Length)
            {
                stream.Read(buffer);
                Write(buffer);
            }
            Int32 remaining = count % buffer.Length;
            stream.Read(buffer, 0, remaining);
            Write(buffer, 0, remaining);
        }


        /// <summary>
        ///     Force flushes buffer data.
        /// </summary>
        public virtual void Flush()
        {
        }

        /// <summary>
        ///     Closes the stream.
        /// </summary>
        public virtual void Close()
        {
        }

        /// <summary>
        ///     Writes the stream contents to an array, regardless of the Position property.
        /// </summary>
        /// <returns>A new array of T.</returns>
        public T[] ToArray()
        {
            return array.SubArray(offset, length);
        }

        #region IDisposable

        private Boolean disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Disposes the stream
        /// </summary>
        /// <remarks>
        ///     NOTE: Call Dispose(true) in derived classes instead of base.Dispose().
        /// </remarks>
        /// <param name="disposing"></param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                // Dispose Managed Objects
            }

            // Dispose Unmanaged Objects
            // Set large fields to null

            array = null;
            disposed = true;
        }

        #endregion
    }
}