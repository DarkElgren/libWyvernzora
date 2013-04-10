using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using libWyvernzora.Core;
using libWyvernzora.IO;
using libWyvernzora.Utilities;
using libWyvernzora.IO.UnifiedFileSystem;
using libWyvernzora.Test;

namespace libWyvernzora.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamEx ex = new StreamEx("data.bin", FileMode.Create);

            Int32[] array = TestUtilities.GenerateRandomArray(1000);

            foreach (var i in array) ex.WriteVInt(new VInt(i));

            ex.Position = 0;

            Int32[] array2 = new Int32[1000];

            for (int i = 0; i < array2.Length; i++)
            {
                VInt x = ex.ReadVInt();
                array2[i] = (Int32)x.Value;
            }

            for (int i = 0; i < array2.Length; i++)
            {
                if (array[i] != array2[i])
                    System.Diagnostics.Debugger.Break();
            }
        }
    }
}
