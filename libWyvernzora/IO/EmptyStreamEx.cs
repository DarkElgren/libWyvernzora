// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/EmptyStreamEx.cs
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

namespace libWyvernzora.IO
{
    /// <summary>
    ///     Wrapper for StreamEx that enforces it to be empty.
    /// </summary>
    public sealed class EmptyStreamEx : StreamExEnforcer
    {
        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="stream"></param>
        public EmptyStreamEx(StreamEx stream) : base(stream)
        {
        }

        protected override void ValidateStream(StreamEx stream)
        {
            if (stream.Length != 0)
                throw new InvalidOperationException("StreamEx is not empty!");
            if (stream.Position != 0)
                throw new InvalidOperationException("StreamEx Position is not at zero!");
        }

        /// <summary>
        ///     Implicitly wraps StreamEx into an EmptyStreamEx.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static implicit operator EmptyStreamEx(StreamEx stream)
        {
            return new EmptyStreamEx(stream);
        }
    }
}