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
using libWyvernzora.Test;

namespace libWyvernzora.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamEx ex = new StreamEx("data.bin", FileMode.Create);

            String str =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean aliquet ullamcorper augue ac blandit. Sed eget est et ante ultricies pulvinar quis nec augue. Pellentesque ac sem lectus, vitae interdum neque. Vestibulum tincidunt, nibh viverra commodo adipiscing, felis quam suscipit libero, nec porta magna neque nec risus. Vestibulum vehicula dolor vel erat ultrices imperdiet. Maecenas vitae nisl in nisi sagittis tempor quis eu justo. Ut ut congue lorem. Nullam sit amet purus ac dui varius molestie.";

            ex.WriteString(str);

            ex.Position = 0;

            String str2 = ex.ReadString();

            Console.WriteLine(str.Equals(str2));

            Console.ReadLine();
        }

    }
}
