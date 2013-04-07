// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/ArrayStreamTest.cs
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using libWyvernzora.Collections;

namespace libWyvernzora.Test.Collections
{
    [TestClass]
    public class ArrayStreamTest
    {
        private static readonly Int32[] Data = new[]
            {12, 97, 43, 85, 21, 43, 62, 16, 28, 37, 65, 38, 43, 68, 95, 05, 16};

        [TestMethod]
        public void ArrayStream_ConstructorTest()
        {
            // Test the first overload
            ArrayStream<Int32> stream1 = new ArrayStream<int>(100);
            Assert.AreEqual(100, stream1.Length);

            // Test the second overload
            ArrayStream<Int32> stream2 = new ArrayStream<int>(Data, 0);
            ArrayStream<Int32> stream3 = new ArrayStream<int>(Data, 8);
            Assert.AreEqual(Data.Length, stream2.Length);
            Assert.AreEqual(Data.Length - 8, stream3.Length);

            // Test the second overload
            ArrayStream<Int32> stream4 = new ArrayStream<int>(Data, 8, 2);
            Assert.AreEqual(2, stream4.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArrayStream_ConstructorArrayNullTest()
        {
            ArrayStream<Int32> stream = new ArrayStream<int>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ArrayStream_ConstructorLargeOffsetTest()
        {
            ArrayStream<Int32> stream = new ArrayStream<int>(Data, Data.Length + 1);
        }

        [TestMethod]
        public void ArrayStream_WriteTest()
        {
            // Test WriteElement
            ArrayStream<Int32> stream1 = new ArrayStream<int>(Data.Length);
            foreach (int v in Data) stream1.WriteElement(v);
            CollectionAssert.AreEqual(Data, stream1.ToArray());

            // Test Write
            ArrayStream<Int32> stream2 = new ArrayStream<int>(Data.Length);
            stream2.Write(Data);
            CollectionAssert.AreEqual(Data, stream2.ToArray());

            // Test Write
            ArrayStream<Int32> stream3 = new ArrayStream<int>(Data.Length);
            stream3.Write(Data, 0, Data.Length);
            CollectionAssert.AreEqual(Data, stream3.ToArray());
        }

        [TestMethod]
        public void ArrayStream_ReadTest()
        {
            ArrayStream<Int32> stream = new ArrayStream<int>(Data);

            // Test ReadElement
            Int32[] temp1 = new int[Data.Length];
            for (int i = 0; i < temp1.Length; i++)
                temp1[i] = stream.ReadElement();
            CollectionAssert.AreEqual(Data, temp1);

            // Test Read
            stream.Position = 0;
            Int32[] temp2 = new Int32[Data.Length];
            stream.Read(temp2);
            CollectionAssert.AreEqual(Data, temp2);

            // Test Read
            stream.Position = 0;
            Int32[] temp3 = new Int32[Data.Length];
            stream.Read(temp3, 0, Data.Length);
            CollectionAssert.AreEqual(Data, temp3);
        }
    }
}