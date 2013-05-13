// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/Pair.cs
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

namespace libWyvernzora.Core
{
    /// <summary>
    ///     Represents a pair of objects.
    /// </summary>
    /// <typeparam name="TFirst">Type of the first object in pair</typeparam>
    /// <typeparam name="TSecond">Type of the second object in pair</typeparam>
    public class Pair<TFirst, TSecond> : ICloneable, IEquatable<Pair<TFirst, TSecond>>
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="f">First object in the pair</param>
        /// <param name="s">Second object in the pair</param>
        public Pair(TFirst f, TSecond s)
        {
            First = f;
            Second = s;
        }

        /// <summary>
        ///     First object in the pair
        /// </summary>
        public TFirst First { get; set; }

        /// <summary>
        ///     Second object in the pair
        /// </summary>
        public TSecond Second { get; set; }

        /// <summary>
        ///     Clones the pair.
        /// </summary>
        /// <returns>A shallow copy of the pair object</returns>
        public object Clone()
        {
            return new Pair<TFirst, TSecond>(First, Second);
        }

        /// <summary>
        ///     Checks whether two pairs are the same
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Pair<TFirst, TSecond> other)
        {
            return First.Equals(other.First) && Second.Equals(other.Second);
        }
    }
}