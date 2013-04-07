using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libWyvernzora.Utilities;

namespace libWyvernzora.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            Console.ReadLine();
        }

        static void Test()
        {
            using (new ActionLock(Action, Action))
            {
                Console.WriteLine("test");
                return;
            }
        }

        static void Action()
        {
            Console.WriteLine("ACTION TRIGGERED!!");
        }
    }
}
