﻿// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/EnumeratorEnumerableTest.cs
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
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using libWyvernzora.Collections;

namespace libWyvernzora.Test.Collections
{
    /// <summary>
    ///     EnumeratorEnumerable Unit Test
    /// </summary>
    [TestClass]
    public class EnumeratorEnumerableTest
    {
        [TestMethod]
        public void EnumeratorEnumerable_Test()
        {
            List<Int32> collection = new List<Int32>(TestUtilities.GenerateRandomArray(100));
            EnumeratorEnumerable<Int32> enu = new EnumeratorEnumerable<int>(collection.GetEnumerator());

            IEnumerator<Int32> cEnu = collection.GetEnumerator();
            IEnumerator<Int32> eEnu = enu.GetEnumerator();

            while (true)
            {
                Boolean cNxt = cEnu.MoveNext();
                Boolean eNxt = eEnu.MoveNext();

                Assert.AreEqual(cNxt, eNxt);

                if (!cNxt) break;

                Assert.AreEqual(cEnu.Current, eEnu.Current);
            }
        }
    }
}