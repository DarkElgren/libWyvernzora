// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/Heap.cs
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
using libWyvernzora.Core;

namespace libWyvernzora.Collections
{
    /// <summary>
    ///     Basic Binary Heap implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Heap<T> : ICollection<T>, ICloneable
    {
        #region Constants

        private const Int32 InitialCapacity = 10;

        #endregion

        private readonly IComparer<T> comparer;

        private Int32 count;
        private T[] data;
        private Int32 growth;

        #region Constructors

        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        public Heap()
            : this(InitialCapacity, Comparer<T>.Default)
        {
        }

        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="comparer">Comparer to use in the heap.</param>
        public Heap(IComparer<T> comparer)
            : this(InitialCapacity, comparer)
        {
        }

        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="capacity">Initial heap capacity.</param>
        public Heap(Int32 capacity)
            : this(capacity, Comparer<T>.Default)
        {
        }

        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="capacity">Initial heap capacity.</param>
        /// <param name="comparer">Comparer to use in the heap.</param>
        public Heap(Int32 capacity, IComparer<T> comparer)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException("capacity");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            data = new T[capacity];
            this.comparer = comparer;
            growth = -1;
        }

        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="items">IList of items to put into the heap.</param>
        public Heap(IList<T> items)
            : this(items, Comparer<T>.Default)
        {
        }

        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="items">IList of items to put into the heap.</param>
        /// <param name="comparer">Comparer to use in the heap.</param>
        public Heap(IList<T> items, IComparer<T> comparer)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            data = new T[items.Count];
            count = items.Count;
            this.comparer = comparer;
            growth = -1;

            // Copy items to the data array
            for (int i = 0; i < items.Count; i++)
                data[i] = items[i];

            // Build Heap
            RebuildHeap();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a value indicating whether the heap is empty.
        /// </summary>
        public Boolean IsEmpty
        {
            get { return count > 0; }
        }

        /// <summary>
        ///     Gets or sets the rate of growth of the heap.
        ///     When less than or equal to zero, the heap capacity doubles each time it grows.
        ///     Useful on limited-memory devices.
        /// </summary>
        public Int32 Growth
        {
            get { return growth; }
            set { growth = value; }
        }

        #endregion

        #region Heap ADT

        /// <summary>
        ///     Inserts an item into the heap.
        /// </summary>
        /// <param name="item">The item to insert.</param>
        public void Insert(T item)
        {
            if (count >= data.Length)
                Resize(growth <= 0 ? data.Length * 2 : data.Length + growth);

            data[count] = item;
            HeapifyUp(count++);
        }

        /// <summary>
        ///     Removes the root item of the heap and returns it.
        /// </summary>
        /// <returns>The root item of the heap.</returns>
        public T DeleteRoot()
        {
            T tmp = data[0];
            data[0] = data[count - 1];
            data[count--] = default(T);

            HeapifyDown(0);

            return tmp;
        }

        /// <summary>
        ///     Returns the root item of the heap without removing it.
        /// </summary>
        /// <returns>The root item of the heap.</returns>
        public T Peek()
        {
            return data[0];
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            Insert(item);
        }

        public void Clear()
        {
            data = new T[InitialCapacity];
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(data, 0, array, arrayIndex, NumericOps.Min(array.Length - arrayIndex, count));
        }

        public int Count
        {
            get { return count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            Int32 index = IndexOf(item);

            if (index < 0)
                return false;

            data[index] = data[count - 1];
            data[count--] = default(T);

            int i = HeapifyUp(index);
            if (i == index) HeapifyDown(index);

            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>) data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        #endregion

        #region Utility Methods

        // Rebuilds the heap from an unsorted array.
        private void RebuildHeap()
        {
            for (int i = count >> 1; i >= 0; i--)
                HeapifyDown(i);
        }

        // Rebuilds the heap from the 'pos' node and up,
        // returns the index where the method stops.
        private Int32 HeapifyUp(Int32 pos)
        {
            if (pos >= count) return -1;

            while (pos > 0)
            {
                Int32 parent = (pos - 1) / 2;

                if (comparer.Compare(data[parent], data[pos]) >= 0)
                    break;

                Swap(parent, pos);
                pos = parent;
            }

            return pos;
        }

        // Rebuilds the heap from the 'pos' node and down.
        private void HeapifyDown(Int32 pos)
        {
            if (pos >= count) return;

            while (true)
            {
                Int32 current = pos;
                Int32 left = pos * 2 + 1;
                Int32 right = pos * 2 + 2;

                if (left < count && comparer.Compare(data[current], data[left]) < 0)
                    current = left;
                if (right < count && comparer.Compare(data[current], data[right]) < 0)
                    current = right;

                if (current == pos)
                    break;

                Swap(current, pos);
                pos = current;
            }
        }

        // Resizes the heap array. If the new size is smaller than
        // current size, extra elements are discarted.
        private void Resize(Int32 newSize)
        {
            T[] array = new T[newSize];
            Array.Copy(data, array, NumericOps.Min(newSize, data.Length));
            data = array;
        }

        // Swaps two elements in the data array.
        private void Swap(Int32 lhs, Int32 rhs)
        {
            if (lhs < 0 || lhs >= count)
                throw new ArgumentOutOfRangeException("lhs");
            if (rhs < 0 || rhs >= count)
                throw new ArgumentOutOfRangeException("rhs");

            if (lhs == rhs) return;

            T value = data[lhs];
            data[lhs] = data[rhs];
            data[rhs] = value;
        }

        // Performs a linear search on the data array for the specified
        // element. Returns index, or -1 if the element was not found.
        private Int32 IndexOf(T item)
        {
            for (int i = 0; i < count; i++)
                if (comparer.Compare(item, data[i]) == 0)
                    return i;
            return -1;
        }

        #endregion

        public object Clone()
        {
            Heap<T> heap = new Heap<T>(data.Length) {count = count, growth = growth};
            Array.Copy(data, heap.data, data.Length);

            return heap;
        }
    }
}