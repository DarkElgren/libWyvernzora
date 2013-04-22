// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/CircularQueue.cs
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
    ///     Queue ADT implementation based on a circular buffer.
    ///     The capacity is fixed when the queue is created, though
    ///     it is possible to resize it.
    /// </summary>
    /// <remarks>
    ///     CircularQueue provides Θ(1) enqueue and dequeue operations over large
    ///     number of elements. When enqueueing item into a full queue, the oldest
    ///     item is forcibly dequeued and returned by Enqueue() method.
    /// </remarks>
    /// <typeparam name="T">Type of items in the CircularQueue.</typeparam>
    public class CircularQueue<T> : IEnumerable<T>, ICollection
    {
        protected Int32 count; // Number of elements
        protected T[] data; // Element Array
        protected Int32 start; // Start index

        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="capacity">Capacity of the CircularQueue.</param>
        /// <param name="value">
        ///     Default value to initialize the queue with, defaults to <c>default(T)</c>.
        /// </param>
        public CircularQueue(Int32 capacity, T value = default(T))
        {
            data = new T[capacity];
            start = 0;
            count = 0;

            if (EqualityComparer<T>.Default.Equals(value, default(T)))
                for (int i = 0; i < capacity; i++)
                    data[i] = value;
        }

        #region Properties

        /// <summary>
        ///     Gets the capacity of the queue.
        /// </summary>
        public Int32 Capacity
        {
            get { return data.Length; }
        }

        /// <summary>
        ///     Gets a value indicating whether the queue is empty.
        /// </summary>
        public Boolean IsEmpty
        {
            get { return count == 0; }
        }

        /// <summary>
        ///     Gets a value indicating whether the queue is full.
        /// </summary>
        public Boolean IsFull
        {
            get { return count == data.Length; }
        }

        /// <summary>
        ///     Gets or sets the n-th element of the queue.
        /// </summary>
        /// <param name="n">Index of the queue item.</param>
        /// <returns>The n-th element of the queue.</returns>
        public T this[Int32 n]
        {
            get
            {
                if (n < 0 || n > count) throw new IndexOutOfRangeException();
                return data[(start + n) % data.Length];
            }
            set
            {
                if (n < 0 || n > count) throw new IndexOutOfRangeException();
                data[(start + n) % data.Length] = value;
            }
        }

        /// <summary>
        ///     Gets the number of elements in the queue.
        /// </summary>
        public Int32 Count
        {
            get { return count; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Removes all items from the queue.
        /// </summary>
        public void Clear()
        {
            data = new T[data.Length];
            start = 0;
            count = 0;
        }

        /// <summary>
        ///     Searches for the specified item in the queue.
        /// </summary>
        /// <param name="obj">The item to look for.</param>
        /// <returns>True if the item is found; false otherwise.</returns>
        public Boolean Contains(T obj)
        {
            for (int i = 0; i < count; i++)
                if (EqualityComparer<T>.Default.Equals(this[i], obj))
                    return true;
            return false;
        }

        /// <summary>
        ///     Dequeues the oldest item from the queue.
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            if (IsEmpty)
                throw new InvalidOperationException("CircularQueue.Dequeue() : Empty queue!");
            T item = data[start++];
            if (start >= data.Length)
                start = 0;
            return item;
        }

        /// <summary>
        ///     Enqueues an item into the queue.
        ///     In case if the queue is full, the oldest item is forcibly
        ///     dequeued and returned; if the queue is not full, this method
        ///     returns <c>default(T)</c>.
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        /// <returns></returns>
        public T Enqueue(T item)
        {
            T old = this[count];
            this[count++] = item;
            return old;
        }

        /// <summary>
        ///     Gets the first element from the queue without removing it.
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            return data[start];
        }

        /// <summary>
        ///     Copies contents of the queue into an array.
        /// </summary>
        /// <param name="array">Array where to copy contents of the queue.</param>
        /// <param name="index">Array index where to start copying.</param>
        public void CopyTo(T[] array, Int32 index)
        {
            for (int i = 0; i < NumericOps.Min(array.Length - index, count); i++)
                array[index + i] = this[i];
        }

        /// <summary>
        ///     Creates an array and copies all the elements in the queue.
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            T[] array = new T[count];
            CopyTo(array, 0);
            return array;
        }

        /// <summary>
        ///     Changes the capacity of the queue.
        ///     Cannot shrink the queue so that its new capacity is less than its current
        ///     element count.
        /// </summary>
        /// <param name="capacity"></param>
        public void SetCapacity(Int32 capacity)
        {
            if (capacity < count)
                throw new InvalidOperationException(
                    "CircularQueue.SetCapacity() : Cannot shrink the queue with data loss!");
            T[] array = new T[capacity];
            CopyTo(array, 0);
            data = array;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return new CircularQueueEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CircularQueueEnumerator<T>(this);
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            Object[] arr = (Object[]) array;
            for (int i = 0; i < NumericOps.Min(array.Length - index, count); i++)
                arr[index + 1] = this[i];
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        #endregion

        #region Nested Data Types

        private sealed class CircularQueueEnumerator<TValue> : IEnumerator<TValue>
        {
            private readonly CircularQueue<TValue> queue;
            private Int32 index = -1;

            public CircularQueueEnumerator(CircularQueue<TValue> queue)
            {
                this.queue = queue;
            }


            public TValue Current
            {
                get { return queue[index]; }
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get { return queue[index]; }
            }

            public bool MoveNext()
            {
                index++;
                return index < queue.Count;
            }

            public void Reset()
            {
                index = -1;
            }
        }

        #endregion
    }
}