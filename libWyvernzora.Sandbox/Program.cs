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
            WindowsFileSystem wfs = new WindowsFileSystem("G:\\MyPictures");

            var fso = wfs.GetFileSystemObject("wp\\kanon_shiori.png");
            var exs = wfs.OpenFileSystemObject(fso, FileAccess.Read);
            
            Console.ReadKey();
        }
    }
}
