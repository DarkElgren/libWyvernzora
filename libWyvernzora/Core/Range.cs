// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/Range.cs
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
    ///     Represents a range of integers.
    /// </summary>
    public class Range
    {
        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="lower">Lower bound of the range.</param>
        /// <param name="upper">Upper bound of the range.</param>
        public Range(Int32 lower, Int32 upper)
        {
            if (upper < lower) throw new ArgumentOutOfRangeException();
            Upper = upper;
            Lower = lower;
        }

        /// <summary>
        ///     Gets or sets the lower bound of the range.
        /// </summary>
        public Int32 Lower { get; set; }

        /// <summary>
        ///     Gets or sets the upper bound of the range.
        /// </summary>
        public Int32 Upper { get; set; }

        /// <summary>
        ///     Gets or sets the number of integers in the range.
        /// </summary>
        public Int32 Count
        {
            get { return Upper - Lower + 1; }
            set { Upper = Lower + value - 1; }
        }

        /// <summary>
        ///     Checks if the range contains the specified integer.
        /// </summary>
        /// <param name="i">Integer to look for.</param>
        /// <returns>True if the range contains the specified integer; false otherwise.</returns>
        public Boolean Contains(Int32 i)
        {
            return i >= Lower && i <= Upper;
        }
    }
}