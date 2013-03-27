// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/MappedEnumerator.cs
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
using System.Collections;
using System.Collections.Generic;

namespace libWyvernzora.Collections
{
    /// <summary>
    ///     Mapped Enumerator.
    ///     Maps elements in the specified enumerator to other elements and enumerates them.
    /// </summary>
    /// <typeparam name="TKey">Type of elements in the base enumerator.</typeparam>
    /// <typeparam name="TValue">Type of elements in MappedEnumerator.</typeparam>
    public class MappedEnumerator<TKey, TValue> : IEnumerator<TValue>
    {
        private readonly IEnumerator<TKey> enumerator;
        private readonly Func<TKey, TValue> mapping;

        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="enumerator">Base enumerator.</param>
        /// <param name="mapping">Mapping function.</param>
        public MappedEnumerator(IEnumerator<TKey> enumerator, Func<TKey, TValue> mapping)
        {
            this.enumerator = enumerator;
            this.mapping = mapping;
        }

        #region IEnumerator<TValue>

        public TValue Current
        {
            get { return mapping(enumerator.Current); }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }

        public void Reset()
        {
            enumerator.Reset();
        }

        #endregion

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
                enumerator.Dispose();
            }

            // Dispose Unmanaged Objects
            // Set large fields to null

            disposed = true;
        }

        #endregion
    }
}