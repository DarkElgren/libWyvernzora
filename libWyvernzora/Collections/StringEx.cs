using System;
using System.Collections.Generic;
using System.Linq;

namespace libWyvernzora.Collections
{
    /// <summary>
    /// Generic List-based String.
    /// Similar to System.String, but instead is a generic collection.
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
            foreach (var v in this.data)
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

            if (data.Count!= obj.data.Count) return false;

            var ec = EqualityComparer<T>.Default;
            var en =
                new EnumeratorEnumerable<Boolean>(new ZippedEnumerator<T, T, Boolean>(GetEnumerator(), obj.GetEnumerator(), ec.Equals));
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

            var cmp = Comparer<T>.Default;
            var enu = new ZippedEnumerator<T, T, Int32>(left.GetEnumerator(), right.GetEnumerator(), cmp.Compare);
            Int32 r2 = (new EnumeratorEnumerable<Int32>(enu)).FirstOrDefault(x => x != 0);
            return r2 != 0 ? r2 : r1;
        }

        #endregion

        #region IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
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




    }
}
