// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/RandomEx.cs
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
using libWyvernzora.Core;

// TODO Add Ranged Random Generation Methods

namespace libWyvernzora.Utilities
{
    /// <summary>
    ///     Extended Wrapper for the Random Class.
    ///     Not intended to be cryptographically secure.
    /// </summary>
    /// <remarks>
    ///     One key difference between RandomEx and Random is that it also
    ///     generates negative values for signed types.
    ///     For cryptographically secure random numbers, consider using
    ///     System.Security.Cryptography.RandomNumberGenerator deriviatives.
    /// </remarks>
    public class RandomEx
    {
        #region Singleton

        private static RandomEx instance;

        /// <summary>
        ///     Singleton pattern.
        ///     Gets the singleton instance of this class.
        /// </summary>
        public static RandomEx Instance
        {
            get { return instance ?? (instance = new RandomEx()); }
        }

        #endregion

        private readonly Random rand; // wrapped instance

        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        public RandomEx()
            : this(new Random())
        {
        }

        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="rand"></param>
        public RandomEx(Random rand)
        {
            this.rand = rand;
        }

        #region Generate Random Numbers

        /// <summary>
        ///     Returns a random number.
        ///     Generated number may be negative.
        /// </summary>
        /// <returns>A 8-bit unsigned integer greater than or equal to Byte.MinValue and less than or equal to Byte.MaxValue.</returns>
        public Byte NextByte()
        {
            Byte[] buffer = new byte[2];
            rand.NextBytes(buffer);
            return buffer[0];
        }

        /// <summary>
        ///     Returns a random number.
        ///     Generated number may be negative.
        /// </summary>
        /// <returns>A 8-bit signed integer greater than or equal to SByte.MinValue and less than or equal to SByte.MaxValue.</returns>
        public SByte NextSByte()
        {
            Byte[] buffer = new byte[1];
            rand.NextBytes(buffer);
            return buffer[0].Sign();
        }

        /// <summary>
        ///     Returns a random number.
        ///     Generated number may be negative.
        /// </summary>
        /// <returns>A 16-bit unsigned integer greater than or equal to UInt16.MinValue and less than or equal to UInt16.MaxValue.</returns>
        public UInt16 NextUInt16()
        {
            Byte[] buffer = new byte[2];
            rand.NextBytes(buffer);
            return buffer.ToUInt16(0);
        }

        /// <summary>
        ///     Returns a random number.
        ///     Generated number may be negative.
        /// </summary>
        /// <returns>A 16-bit signed integer greater than or equal to Int16.MinValue and less than or equal to Int16.MaxValue.</returns>
        public Int16 NextInt16()
        {
            Byte[] buffer = new byte[2];
            rand.NextBytes(buffer);
            return buffer.ToInt16(0);
        }

        /// <summary>
        ///     Returns a random number.
        ///     Generated number may be negative.
        /// </summary>
        /// <returns>A 32-bit unsigned integer greater than or equal to UInt32.MinValue and less than or equal to UInt32.MaxValue.</returns>
        public UInt32 NextUInt32()
        {
            Byte[] buffer = new byte[4];
            rand.NextBytes(buffer);
            return buffer.ToUInt32(0);
        }

        /// <summary>
        ///     Returns a random number.
        ///     Generated number may be negative.
        /// </summary>
        /// <returns>A 32-bit signed integer greater than or equal to Int32.MinValue and less than or equal to Int32.MaxValue.</returns>
        public Int32 NextInt32()
        {
            Byte[] buffer = new byte[4];
            rand.NextBytes(buffer);
            return buffer.ToInt32(0);
        }

        /// <summary>
        ///     Returns a random number.
        ///     Generated number may be negative.
        /// </summary>
        /// <returns>A 64-bit unsigned integer greater than or equal to UInt64.MinValue and less than or equal to UInt64.MaxValue.</returns>
        public UInt64 NextUInt64()
        {
            Byte[] buffer = new byte[8];
            rand.NextBytes(buffer);
            return buffer.ToUInt64(0);
        }

        /// <summary>
        ///     Returns a random number.
        ///     Generated number may be negative.
        /// </summary>
        /// <returns>A 64-bit signed integer greater than or equal to Int64.MinValue and less than or equal to Int64.MaxValue.</returns>
        public Int64 NextInt64()
        {
            Byte[] buffer = new byte[8];
            rand.NextBytes(buffer);
            return buffer.ToInt64(0);
        }

        #endregion

        #region Generate Random Arrays

        /// <summary>
        ///     Fills the array with random values, starting at <paramref name="offset" /> and continuing for
        ///     <paramref name="count" /> elements.
        /// </summary>
        /// <param name="arr">The array to fill.</param>
        /// <param name="offset">Starting element of the filling range.</param>
        /// <param name="count">Number of elements in the filling range.</param>
        public void FillRandom(Byte[] arr, Int32 offset, Int32 count)
        {
            if (offset + count > arr.Length)
                throw new ArgumentOutOfRangeException(
                    "RandomEx.Fill(Byte[], Int32, Int32) : Offset and count out of bounds.");
            for (int i = offset; i < offset + count; i++) arr[i] = NextByte();
        }

        /// <summary>
        ///     Fills the array with random values, starting at <paramref name="offset" /> and continuing for
        ///     <paramref name="count" /> elements.
        /// </summary>
        /// <param name="arr">The array to fill.</param>
        /// <param name="offset">Starting element of the filling range.</param>
        /// <param name="count">Number of elements in the filling range.</param>
        public void FillRandom(SByte[] arr, Int32 offset, Int32 count)
        {
            if (offset + count > arr.Length)
                throw new ArgumentOutOfRangeException(
                    "RandomEx.Fill(SByte[], Int32, Int32) : Offset and count out of bounds.");
            for (int i = offset; i < offset + count; i++) arr[i] = NextSByte();
        }

        /// <summary>
        ///     Fills the array with random values, starting at <paramref name="offset" /> and continuing for
        ///     <paramref name="count" /> elements.
        /// </summary>
        /// <param name="arr">The array to fill.</param>
        /// <param name="offset">Starting element of the filling range.</param>
        /// <param name="count">Number of elements in the filling range.</param>
        public void FillRandom(UInt16[] arr, Int32 offset, Int32 count)
        {
            if (offset + count > arr.Length)
                throw new ArgumentOutOfRangeException(
                    "RandomEx.Fill(UInt16[], Int32, Int32) : Offset and count out of bounds.");
            for (int i = offset; i < offset + count; i++) arr[i] = NextUInt16();
        }

        /// <summary>
        ///     Fills the array with random values, starting at <paramref name="offset" /> and continuing for
        ///     <paramref name="count" /> elements.
        /// </summary>
        /// <param name="arr">The array to fill.</param>
        /// <param name="offset">Starting element of the filling range.</param>
        /// <param name="count">Number of elements in the filling range.</param>
        public void FillRandom(Int16[] arr, Int32 offset, Int32 count)
        {
            if (offset + count > arr.Length)
                throw new ArgumentOutOfRangeException(
                    "RandomEx.Fill(Int16[], Int32, Int32) : Offset and count out of bounds.");
            for (int i = offset; i < offset + count; i++) arr[i] = NextInt16();
        }

        /// <summary>
        ///     Fills the array with random values, starting at <paramref name="offset" /> and continuing for
        ///     <paramref name="count" /> elements.
        /// </summary>
        /// <param name="arr">The array to fill.</param>
        /// <param name="offset">Starting element of the filling range.</param>
        /// <param name="count">Number of elements in the filling range.</param>
        public void FillRandom(UInt32[] arr, Int32 offset, Int32 count)
        {
            if (offset + count > arr.Length)
                throw new ArgumentOutOfRangeException(
                    "RandomEx.Fill(UInt32[], Int32, Int32) : Offset and count out of bounds.");
            for (int i = offset; i < offset + count; i++) arr[i] = NextUInt32();
        }

        /// <summary>
        ///     Fills the array with random values, starting at <paramref name="offset" /> and continuing for
        ///     <paramref name="count" /> elements.
        /// </summary>
        /// <param name="arr">The array to fill.</param>
        /// <param name="offset">Starting element of the filling range.</param>
        /// <param name="count">Number of elements in the filling range.</param>
        public void FillRandom(Int32[] arr, Int32 offset, Int32 count)
        {
            if (offset + count > arr.Length)
                throw new ArgumentOutOfRangeException(
                    "RandomEx.Fill(Int32[], Int32, Int32) : Offset and count out of bounds.");
            for (int i = offset; i < offset + count; i++) arr[i] = NextInt32();
        }

        /// <summary>
        ///     Fills the array with random values, starting at <paramref name="offset" /> and continuing for
        ///     <paramref name="count" /> elements.
        /// </summary>
        /// <param name="arr">The array to fill.</param>
        /// <param name="offset">Starting element of the filling range.</param>
        /// <param name="count">Number of elements in the filling range.</param>
        public void FillRandom(UInt64[] arr, Int32 offset, Int32 count)
        {
            if (offset + count > arr.Length)
                throw new ArgumentOutOfRangeException(
                    "RandomEx.Fill(UInt64[], Int32, Int32) : Offset and count out of bounds.");
            for (int i = offset; i < offset + count; i++) arr[i] = NextUInt64();
        }

        /// <summary>
        ///     Fills the array with random values, starting at <paramref name="offset" /> and continuing for
        ///     <paramref name="count" /> elements.
        /// </summary>
        /// <param name="arr">The array to fill.</param>
        /// <param name="offset">Starting element of the filling range.</param>
        /// <param name="count">Number of elements in the filling range.</param>
        public void FillRandom(Int64[] arr, Int32 offset, Int32 count)
        {
            if (offset + count > arr.Length)
                throw new ArgumentOutOfRangeException(
                    "RandomEx.Fill(Int64[], Int32, Int32) : Offset and count out of bounds.");
            for (int i = offset; i < offset + count; i++) arr[i] = NextInt64();
        }

        #endregion
    }
}