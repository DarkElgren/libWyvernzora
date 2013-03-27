// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/Indexer.cs
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
using System.IO;
using libWyvernzora.Core;

namespace libWyvernzora.Collections
{
    /// <summary>
    ///     Indexer.
    ///     Provides support for enumerating discrete integer ranges.
    /// </summary>
    public class Indexer : IEnumerator<Int32>, IEnumerable<Int32>
    {
        protected Int32 position;
        protected SortedList<Int32, Range> ranges;
        protected Int32 value;

        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="ranges">Indexer ranges.</param>
        public Indexer(IEnumerable<Range> ranges)
        {
            this.ranges = new SortedList<int, Range>();
            foreach (Range d in ranges)
            {
                if (d.Lower == Int32.MinValue) throw new InvalidDataException();
                this.ranges.Add(d.Lower, d);
            }
            value = Int32.MinValue;
            position = 0;
        }

        /// <summary>
        ///     Adds a range to the indexer.
        /// </summary>
        /// <param name="range"></param>
        public void AddRange(Range range)
        {
            if (range.Lower == Int32.MinValue) throw new InvalidDataException();
            ranges.Add(range.Lower, range);
            position = 0;
        }

        /// <summary>
        ///     Removes a range from the indexer.
        /// </summary>
        /// <param name="range"></param>
        public void RemoveRange(Range range)
        {
            ranges.Remove(range.Lower);
            position = 0;
        }

        /// <summary>
        ///     Checks if the indexer contains an integer.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Boolean Contains(Int32 i)
        {
            if (ranges.Count == 0) return false;

            Int32 u = ranges.Count - 1;
            Int32 m = u / 2;

            while (u > 0)
            {
                if (ranges.Keys[m] <= i)
                    break;
                u = m;
                m = u / 2;
            }
            u = m;
            for (int n = u; n >= 0; n--)
                if (ranges[ranges.Keys[n]].Contains(i)) return true;
            return false;
        }

        #region IEnumerator<Int32>

        public int Current
        {
            get { return value; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            if (ranges.Count == 0) return false;
            Int32 v = value + 1;
            while (v >= ranges.Values[position].Lower + ranges.Values[position].Count)
            {
                position++;
                if (position >= ranges.Count) return false;
            }
            if (v < ranges.Values[position].Lower)
                v = ranges.Values[position].Lower;
            return true;
        }

        public void Reset()
        {
            if (ranges.Count < 0) throw new InvalidOperationException();
            value = Int32.MinValue;
            position = 0;
        }

        public void SetBefore(Int32 i)
        {
            value = i - 1;
        }

        #endregion

        #region IEnumerable<Int32>

        public IEnumerator<int> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
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
            }

            // Dispose Unmanaged Objects
            // Set large fields to null
            ranges = null;

            disposed = true;
        }

        #endregion
    }
}