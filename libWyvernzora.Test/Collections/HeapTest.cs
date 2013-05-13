using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using libWyvernzora.Collections;

namespace libWyvernzora.Test.Collections
{
    [TestClass]
    public class HeapTest
    {
        [TestMethod]
        public void HeapOperationsTest()
        {
            Heap<Int32> heap = new Heap<int>();
            Int32[] arr = TestUtilities.GenerateRandomArray(10000);

            foreach (int t in arr)
                heap.Insert(t);

            Int32 last = Int32.MaxValue;
            for (int i = 0; i < arr.Length - 1; i++)
            {
                Int32 value = heap.DeleteRoot();
                Assert.IsTrue(value < last);
                last = value;
            }
        }
    }
}
