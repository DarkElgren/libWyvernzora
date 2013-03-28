using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using libWyvernzora.Collections;

namespace libWyvernzora.Test.Collections
{
    [TestClass]
    public class ArrayStreamTest
    {
        static readonly Int32[] Data = new Int32[] {12, 97, 43, 85, 21, 43, 62, 16, 28, 37, 65, 38, 43, 68, 95, 05, 16};

        [TestMethod]
        public void ConstructorTest()
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
        public void ConstructorArrayNullTest()
        {
            ArrayStream<Int32> stream = new ArrayStream<int>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructorLargeOffsetTest()
        {
            ArrayStream<Int32> stream = new ArrayStream<int>(Data, Data.Length + 1);
        }

        [TestMethod]
        public void WriteTest()
        {
            // Test WriteElement
            ArrayStream<Int32> stream1 = new ArrayStream<int>(Data.Length);
            foreach (var v in Data) stream1.WriteElement(v);
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

    }
}
