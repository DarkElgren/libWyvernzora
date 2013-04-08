// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/ListStringEx.cs
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

// TODO Work on documentation

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace libWyvernzora.Collections
{
    /// <summary>
    ///     Generic List-based String.
    ///     Similar to System.String, but instead is a generic collection.
    /// </summary>
    public class ListStringEx<T>
        : ICollection<T>, IEquatable<ListStringEx<T>>, IComparable<ListStringEx<T>>, ICloneable
    {
        private readonly IList<T> data;
        private readonly Int32 hash;


        public ListStringEx(IList<T> data)
        {
            if (data == null) throw new ArgumentNullException();
            this.data = data;

            hash = 1315423911;
            foreach (T v in this.data)
                hash = ((hash << 5) ^ v.GetHashCode() ^ ((hash >> 2) & 0x3FFFFFFF));
        }

        public ListStringEx(IList<T> data, Int32 hash)
        {
            if (data == null) throw new ArgumentNullException();
            this.data = data;

            this.hash = hash;
        }

        #region IEquatable<StringEx<T>> and Overrides

        public bool Equals(ListStringEx<T> obj)
        {
            if (obj == null) return false;

            if (data.Count != obj.data.Count) return false;

            EqualityComparer<T> ec = EqualityComparer<T>.Default;
            EnumeratorEnumerable<bool> en =
                new EnumeratorEnumerable<Boolean>(new ZippedEnumerator<T, T, Boolean>(GetEnumerator(),
                                                                                      obj.GetEnumerator(), ec.Equals));
            return en.All(a => a);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            ListStringEx<T> s = obj as ListStringEx<T>;
            return s != null && Equals(s);
        }

        public override int GetHashCode()
        {
            return hash;
        }

        #endregion

        #region IComparable<StringEx<T>> and Overrides

        public int CompareTo(ListStringEx<T> other)
        {
            if (other == null) return Int32.MinValue;

            IEnumerable<T> left = this;
            IEnumerable<T> right = other;
            Int32 leftCount = Count;
            Int32 rightCount = other.Count;
            Int32 r1 = leftCount - rightCount;

            if (r1 < 0) right = right.Take(leftCount);
            else if (r1 > 0) left = left.Take(rightCount);

            Comparer<T> cmp = Comparer<T>.Default;
            ZippedEnumerator<T, T, int> enu = new ZippedEnumerator<T, T, Int32>(left.GetEnumerator(),
                                                                                right.GetEnumerator(), cmp.Compare);
            Int32 r2 = (new EnumeratorEnumerable<Int32>(enu)).FirstOrDefault(x => x != 0);
            return r2 != 0 ? r2 : r1;
        }

        #endregion

        #region IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ICloneable

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        #region ICollection<T>

        public T this[Int32 index]
        {
            get { return data[index]; }
            //set { data[index] = value; }
        }

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            return data.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < data.Count(); i++)
                array[arrayIndex + 1] = data[i];
        }

        public int Count
        {
            get { return data.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
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
            return IndexOf(item, 0, data.Count);
        }

        /// <summary>
        ///     Overloaded.
        ///     Reverses the order of the elements in the entire PartialList&lt;T&gt;.
        /// </summary>
        public void Reverse()
        {
            Reverse(0, data.Count);
        }

        /// <summary>
        ///     Overloaded.
        ///     Reverses the order of the elements in the specified range.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to reverse.</param>
        /// <param name="count">The number of elements in the range to reverse.</param>
        public void Reverse(Int32 index, Int32 count)
        {
            if (index < 0 || index >= data.Count) throw new ArgumentOutOfRangeException();
            if (index + count > data.Count) throw new ArgumentOutOfRangeException();

            for (int i = 0; i < count / 2; i++)
            {
                T tmp = data[index + i];
                data[index + i] = data[index + count - i - 1];
                data[index + count - i - 1] = tmp;
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
            return IndexOf(item, index, data.Count - index);
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
            if (index < 0 || index >= data.Count) throw new ArgumentOutOfRangeException();
            if (index + count > data.Count) throw new ArgumentOutOfRangeException();

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
            return IndexOf(value, 0, data.Count);
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
            return IndexOf(value, index, data.Count - index);
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
            if (index < 0 || index >= data.Count) throw new ArgumentOutOfRangeException();
            if (index + count > data.Count) throw new ArgumentOutOfRangeException();

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
            return LastIndexOf(value, data.Count - 1, data.Count);
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
            if (index < 0 || index >= data.Count) throw new ArgumentOutOfRangeException();

            EqualityComparer<T> ec = EqualityComparer<T>.Default;
            for (int n = data.Count; n > index - count; n--)
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
            return LastIndexOf(value, data.Count - 1, data.Count);
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
            if (index < 0 || index >= data.Count) throw new ArgumentOutOfRangeException();

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