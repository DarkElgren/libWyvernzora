// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/ZippedEnumerator.cs
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
    ///     Zipped Enumerator.
    ///     Zips elements from two enumerators using zipping function and enumerates them.
    /// </summary>
    /// <typeparam name="TKeyA">Type of elements in the enumerator A.</typeparam>
    /// <typeparam name="TKeyB">Type of elements in the enumerator B.</typeparam>
    /// <typeparam name="TValue">Type of elements in the ZippedEnumerator.</typeparam>
    public class ZippedEnumerator<TKeyA, TKeyB, TValue> : IEnumerator<TValue>
    {
        private readonly IEnumerator<TKeyA> enumeratorA;
        private readonly IEnumerator<TKeyB> enumeratorB;
        private readonly Func<TKeyA, TKeyB, TValue> zipping;

        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="enumeratorA">Base enumerator A.</param>
        /// <param name="enumeratorB">Base enumerator B.</param>
        /// <param name="zipping">Zip function.</param>
        public ZippedEnumerator(IEnumerator<TKeyA> enumeratorA, IEnumerator<TKeyB> enumeratorB,
                                Func<TKeyA, TKeyB, TValue> zipping)
        {
            this.enumeratorA = enumeratorA;
            this.enumeratorB = enumeratorB;
            this.zipping = zipping;
        }

        #region IEnumerator<TValue>

        public TValue Current
        {
            get { return zipping(enumeratorA.Current, enumeratorB.Current); }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            Boolean a = enumeratorA.MoveNext();
            Boolean b = enumeratorB.MoveNext();
            if (a != b) throw new InvalidOperationException();
            return a;
        }

        public void Reset()
        {
            enumeratorA.Reset();
            enumeratorB.Reset();
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
                enumeratorA.Dispose();
                enumeratorB.Dispose();
            }

            // Dispose Unmanaged Objects
            // Set large fields to null

            disposed = true;
        }

        #endregion
    }
}