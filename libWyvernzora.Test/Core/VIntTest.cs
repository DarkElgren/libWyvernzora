using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using libWyvernzora.Core;

namespace libWyvernzora.Test.Core
{
    [TestClass]
    public class VIntTest
    {
        [TestMethod]
        public void VIntTest_EncodeDecode()
        {
            for (int i = 0; i < 1000000; i++)
            {
                Int64 number = libWyvernzora.Utilities.RandomEx.Instance.NextInt64();

                if (number > VInt.MaxValue || number < VInt.MinValue) continue;

                VInt x = new VInt(number);

                Byte[] b = x.Encode();

                VInt y = new VInt(b);

                Assert.AreEqual(number, y);
            }

            for (int i = 0; i < 1000000; i++)
            {
                Int32 number = libWyvernzora.Utilities.RandomEx.Instance.NextInt32();

                if (number > VInt.MaxValue || number < VInt.MinValue) continue;

                VInt x = new VInt(number);

                Byte[] b = x.Encode();

                VInt y = new VInt(b);

                Assert.AreEqual(number, y);
            }

            for (int i = 0; i < 1000000; i++)
            {
                Int16 number = libWyvernzora.Utilities.RandomEx.Instance.NextInt16();

                if (number > VInt.MaxValue || number < VInt.MinValue) continue;

                VInt x = new VInt(number);

                Byte[] b = x.Encode();

                VInt y = new VInt(b);

                Assert.AreEqual(number, y);
            }
        }
    }
}
