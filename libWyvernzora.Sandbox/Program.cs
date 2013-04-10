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

namespace libWyvernzora.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10000; i++)
            {
                Int64 number = RandomEx.Instance.NextInt64();

                if (number > VInt.MaxValue || number < VInt.MinValue) continue;

                VInt x = new VInt(number);

                Byte[] b = x.Encode();

                VInt y = new VInt(b);

                if (number != y)
                    System.Diagnostics.Debugger.Break();
            }
        }
    }
}
