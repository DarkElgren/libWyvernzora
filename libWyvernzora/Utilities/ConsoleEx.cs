using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libWyvernzora.Utilities
{
    /// <summary>
    /// Extended Console class.
    /// </summary>
    public static class ConsoleEx
    {

        public static void Write(ConsoleColor color, String format, params Object[] args)
        {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(format, args);
            Console.ForegroundColor = old;
        }

        public static void WriteLine(ConsoleColor color, String format, params Object[] args)
        {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(format, args);
            Console.ForegroundColor = old;
        }

    }
}
