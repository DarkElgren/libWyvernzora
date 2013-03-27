// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/PartialList.cs
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
using System.Linq;

namespace libWyvernzora.Collections
{
    public class PartialList<T> : IList<T>, ICloneable
    {
        private readonly Int32 length;
        private readonly IList<T> list;
        private readonly Int32 offset;

        /// <summary>
        ///     Overloaded.
        ///     Initializes a new instance.
        ///     In order to maintain efficiency, data is not copied therefore it will change if modified elsewhere.
        /// </summary>
        /// <param name="data">Base list.</param>
        public PartialList(IList<T> data)
        {
            if (data == null) throw new ArgumentNullException("data");
            list = data;
            offset = 0;
            length = data.Count;
        }

        /// <summary>
        ///     Overloaded.
        ///     Initializes a new instance.
        ///     In order to maintain efficiency, data is not copied therefore it will change if modified elsewhere.
        /// </summary>
        /// <param name="data">Base list.</param>
        /// <param name="offset">Offset in base list where to start the partial list.</param>
        /// <param name="length">Length of the partial list.</param>
        public PartialList(IList<T> data, Int32 offset, Int32 length)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (offset < 0 || offset >= data.Count) throw new ArgumentOutOfRangeException();
            list = data;
            this.offset = offset;
            this.length = length;
        }

        #region ICloneable

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        #region IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
        {
            return new MappedEnumerator<Int32, T>(Enumerable.Range(offset, length).GetEnumerator(), @i => list[i]);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IList<T>

        // ========== Not Supported ==========
        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }


        // ==========   Supported   ==========

        public bool IsReadOnly
        {
            get { return true; }
        }

        public int Count
        {
            get { return length; }
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int n = 0; n < length; n++)
                array[arrayIndex + n] = list[offset + n];
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= length) throw new IndexOutOfRangeException();
                return list[offset + index];
            }
            set
            {
                if (index < 0 || index >= length) throw new IndexOutOfRangeException();
                list[index + offset] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Overloaded.
        ///     Searches for the specified object and returns the zero-based index of the first
        ///     occurrence within the PartialList&lt;T&gt;.
        /// </summary>
        /// <param name="item">The object to locate in the PartialList&lt;T&gt;. The value can be null for reference types.</param>
        /// <returns>The zero-based index of the first occurrence of item, if found; otherwise, –1.</returns>
        public Int32 IndexOf(T item)
        {
            return IndexOf(item, 0, length);
        }

        /// <summary>
        ///     Overloaded.
        ///     Reverses the order of the elements in the entire PartialList&lt;T&gt;.
        /// </summary>
        public void Reverse()
        {
            Reverse(0, length);
        }

        /// <summary>
        ///     Overloaded.
        ///     Reverses the order of the elements in the specified range.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to reverse.</param>
        /// <param name="count">The number of elements in the range to reverse.</param>
        public void Reverse(Int32 index, Int32 count)
        {
            if (index < 0 || index >= length) throw new ArgumentOutOfRangeException();
            if (index + count > length) throw new ArgumentOutOfRangeException();

            for (int i = 0; i < count / 2; i++)
            {
                T tmp = list[index + i];
                list[index + i] = list[index + count - i - 1];
                list[index + count - i - 1] = tmp;
            }
        }


        /// <summary>
        ///     Overloaded.
        ///     Searches for the specified object and returns the zero-based index of the first
        ///     occurrence within the PartialList&lt;T&gt; starting at the specified index.
        /// </summary>
        /// <param name="item">The object to locate in the PartialList&lt;T&gt;. The value can be null for reference types.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements that starts at index, if found; otherwise, –1.</returns>
        public Int32 IndexOf(T item, Int32 index)
        {
            return IndexOf(item, index, length - index);
        }

        /// <summary>
        ///     Overloaded.
        ///     Searches for the specified object and returns the zero-based index of the first
        ///     occurrence within the range of elements in the PartialList&lt;T&gt; that starts at the specified
        ///     index and contains the specified number of elements.
        /// </summary>
        /// <param name="item">The object to locate in the PartialList&lt;T&gt;. The value can be null for reference types.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements that starts at index and contains count number of elements, if found; otherwise, –1.</returns>
        public Int32 IndexOf(T item, Int32 index, Int32 count)
        {
            if (index < 0 || index >= length) throw new ArgumentOutOfRangeException();
            if (index + count > length) throw new ArgumentOutOfRangeException();

            EqualityComparer<T> ec = EqualityComparer<T>.Default;
            for (int n = index; n < index + count; n++)
                if (ec.Equals(this[n], item)) return n;
            return -1;
        }

        /// <summary>
        ///     Overloaded.
        ///     Searches for the specified object sequence and returns the zero-based index of the first
        ///     occurrence within the PartialList&lt;T&gt;.
        /// </summary>
        /// <param name="value">The IList of objects to locate in the PartialList&lt;T&gt;.</param>
        /// <returns>The zero-based index of the first occurrence of the sequence, if found; otherwise, –1.</returns>
        public Int32 IndexOf(IList<T> value)
        {
            return IndexOf(value, 0, length);
        }

        /// <summary>
        ///     Overloaded.
        ///     Searches for the specified object sequence and returns the zero-based index of the first
        ///     occurrence within the PartialList&lt;T&gt; starting at the specified index.
        /// </summary>
        /// <param name="value">The sequence to locate in the PartialList&lt;T&gt;.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <returns>The zero-based index of the first occurrence of the sequence within the range of elements that starts at index, if found; otherwise, –1.</returns>
        public Int32 IndexOf(IList<T> value, Int32 index)
        {
            return IndexOf(value, index, length - index);
        }

        /// <summary>
        ///     Overloaded.
        ///     Searches for the specified object sequence and returns the zero-based index of the first
        ///     occurrence within the range of elements in the PartialList&lt;T&gt; that starts at the specified
        ///     index and contains the specified number of elements.
        /// </summary>
        /// <param name="value">The sequence to locate in the PartialList&lt;T&gt;.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The zero-based index of the first occurrence of the sequence within the range of elements that starts at index and contains count number of elements, if found; otherwise, –1.</returns>
        public Int32 IndexOf(IList<T> value, Int32 index, Int32 count)
        {
            if (index < 0 || index >= length) throw new ArgumentOutOfRangeException();
            if (index + count > length) throw new ArgumentOutOfRangeException();

            EqualityComparer<T> ec = EqualityComparer<T>.Default;
            for (int n = index; n <= index + count - value.Count; n++)
            {
                Boolean flag = !value.Where((t, k) => !ec.Equals(this[n + k], t)).Any();
                if (flag) return n;
            }

            return -1;
        }

        /// <summary>
        ///     Overloaded.
        ///     Searches for the specified object and returns the zero-based index
        ///     of the last occurrence within the range of elements in the PartialList&lt;T&gt;.
        /// </summary>
        /// <param name="value">The object to locate. The value can be null for reference types.</param>
        /// <returns>
        ///     The zero-based index of the last occurrence of item, if found; otherwise, –1.
        /// </returns>
        public Int32 LastIndexOf(T value)
        {
            return LastIndexOf(value, length - 1, length);
        }

        /// <summary>
        ///     Overloaded.
        ///     Searches for the specified object and returns the zero-based index
        ///     of the last occurrence within the range of elements in the PartialList&lt;T&gt;
        ///     that ends at the specified index.
        /// </summary>
        /// <param name="value">The object to locate. The value can be null for reference types.</param>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <returns>
        ///     The zero-based index of the last occurrence of item within the range of elements that
        ///     ends at index, if found; otherwise, –1.
        /// </returns>
        public Int32 LastIndexOf(T value, Int32 index)
        {
            return LastIndexOf(value, index, index + 1);
        }

        /// <summary>
        ///     Overloaded.
        ///     Searches for the specified object and returns the zero-based index
        ///     of the last occurrence within the range of elements in the PartialList&lt;T&gt;
        ///     that contains the specified number of elements and ends at the
        ///     specified index.
        /// </summary>
        /// <param name="value">The object to locate. The value can be null for reference types.</param>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>
        ///     The zero-based index of the last occurrence of item within the range of elements that
        ///     contains count number of elements and ends at index, if found; otherwise, –1.
        /// </returns>
        public Int32 LastIndexOf(T value, Int32 index, Int32 count)
        {
            if (index < 0 || index >= length) throw new ArgumentOutOfRangeException();

            EqualityComparer<T> ec = EqualityComparer<T>.Default;
            for (int n = length; n > index - count; n--)
            {
                if (ec.Equals(this[n], value)) return n;
            }
            return -1;
        }

        /// <summary>
        ///     Overloaded.
        ///     Searches for the specified object sequence and returns the zero-based index
        ///     of the last occurrence within the range of elements in the PartialList&lt;T&gt;.
        /// </summary>
        /// <param name="value">The object sequence to locate.</param>
        /// <returns>
        ///     The zero-based index of the last occurrence of the sequence, if found; otherwise, –1.
        /// </returns>
        public Int32 LastIndexOf(IList<T> value)
        {
            return LastIndexOf(value, length - 1, length);
        }

        /// <summary>
        ///     Overloaded.
        ///     Searches for the specified object sequence and returns the zero-based index
        ///     of the last occurrence within the range of elements in the PartialList&lt;T&gt;
        ///     that ends at the specified index.
        /// </summary>
        /// <param name="value">The object sequence to locate.</param>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <returns>
        ///     The zero-based index of the last occurrence of the sequence within the range of elements that
        ///     ends at index, if found; otherwise, –1.
        /// </returns>
        public Int32 LastIndexOf(IList<T> value, Int32 index)
        {
            return LastIndexOf(value, index, index + 1);
        }

        /// <summary>
        ///     Overloaded.
        ///     Searches for the specified object sequence and returns the zero-based index
        ///     of the last occurrence within the range of elements in the PartialList&lt;T&gt;
        ///     that contains the specified number of elements and ends at the
        ///     specified index.
        /// </summary>
        /// <param name="value">The object sequence to locate.</param>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>
        ///     The zero-based index of the last occurrence of the sequence within the range of elements that
        ///     contains count number of elements and ends at index, if found; otherwise, –1.
        /// </returns>
        public Int32 LastIndexOf(IList<T> value, Int32 index, Int32 count)
        {
            if (index < 0 || index >= length) throw new ArgumentOutOfRangeException();

            EqualityComparer<T> ec = EqualityComparer<T>.Default;

            for (int n = index - value.Count + 1; n > index - count; n--)
            {
                Boolean flag = !value.Where((t, k) => !ec.Equals(this[n + k], t)).Any();
                if (flag) return n;
            }

            return -1;
        }

        #endregion
    }
}