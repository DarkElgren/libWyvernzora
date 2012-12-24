/* =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
 * libWyvernzora.Range.cs
 *      - Represents a range of numbers
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
using libWyvernzora.IO;

namespace libWyvernzora
{
    public class Range : 
        IComparable<Range>, IEquatable<Range>
    {
        public Int64 Lower { get; set; }
        public Int64 Higher { get; set; }
        public Int64 Length
        {
            get
            {
                return Higher - Lower;
            }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException();
                Higher = Lower + value;
            }
        }

        public Range()
        { Lower = 0; Higher= 0; }
        public Range(Int64 low, Int64 high)
        { Lower = low; Higher = high; }

        //IComparable<Range>
        public int CompareTo(Range other)
        {
            if (Lower > other.Lower) return 1;
            else if (Lower < other.Lower) return -1;
            else
            {
                if (Higher > other.Higher) return 1;
                else if (Higher < other.Higher) return -1;
                else return 0;
            }
        }

        //IEquatable<Range>
        public bool Equals(Range other)
        {
            return (Lower == other.Lower && Higher == other.Higher);
        }

        //Functions
        public Boolean HasIntersection(Range other)
        {
            if (this.Lower == other.Lower) return true;
            else if (this.Lower > other.Lower)
            {
                return other.Higher > this.Lower;
            }
            else
            {
                return this.Higher > other.Lower;
            }
        }
        public static Boolean operator *(Range lhs, Range rhs)
        {
            return lhs.HasIntersection(rhs);
        }
    }

    public class RangeCollection : ICollection<Range>
    {
        protected SortedDictionary<Int64, Range> m_data = new SortedDictionary<long, Range>();
        protected Boolean m_allowIntersection = false;
        public Boolean AllowIntersection
        { get { return m_allowIntersection; } set { m_allowIntersection = value; } }

        public void Add(Range item)
        {
            if (!AllowIntersection && HasIntersection(item))
                throw new ArgumentException("INTERSECTION");
            m_data.Add(item.Lower, item);
        }
        public void Clear()
        {
            m_data.Clear();
        }
        public bool Contains(Range item)
        {
            if (!m_data.ContainsKey(item.Lower)) return false;
            else return item.Equals(m_data[item.Lower]);
        }
        public void CopyTo(Range[] array, int arrayIndex)
        {
            Int32 t_pt = arrayIndex;
            foreach (Range r in m_data.Values)
            { array[t_pt] = r; t_pt++; if (t_pt == array.Length) break; }
        }
        public int Count
        {
            get { return m_data.Count; }
        }
        public bool IsReadOnly
        {
            get { return false; }
        }
        public bool Remove(Range item)
        {
            if (!m_data.ContainsKey(item.Lower)) return false;
            else if (m_data[item.Lower].Equals(item))
            { m_data.Remove(item.Lower); return true; }
            return false;
        }
        public IEnumerator<Range> GetEnumerator()
        {
            return m_data.Values.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_data.Values.GetEnumerator();
        }

        public Boolean HasIntersection(Range obj)
        {
            foreach (Range rng in m_data.Values)
            { if (rng * obj) return true; }
            return false;
        }
        public Boolean HasIntersection()
        {
            foreach (Range rng0 in m_data.Values)
            {
                foreach (Range rng1 in m_data.Values)
                {
                    if (rng0 * rng1) return true;
                }
            }
            return false;
        }
    }
}
