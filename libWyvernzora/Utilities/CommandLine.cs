// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/CommandLine.cs
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
using System.Linq;
using System.Text.RegularExpressions;

namespace libWyvernzora.Utilities
{
    /// <summary>
    ///     Command Line Arguments
    /// </summary>
    public class CommandLine
    {
        #region Nested Data Types

        /// <summary>
        ///     Represents type of command line argument
        /// </summary>
        public enum Type
        {
            /// <summary>
            ///     Argument is an unnamed string passed via command line
            ///     <c>program.exe argument</c>
            /// </summary>
            Argument,

            /// <summary>
            ///     Option is a named switch passed via command line
            ///     Option may contain arguments of its own
            ///     <c>program.exe /option:arg0,arg1...</c>
            /// </summary>
            Option
        }

        /// <summary>
        ///     Represents a command line argument or option
        /// </summary>
        public class Argument
        {
            /// <summary>
            ///     Constructor.
            /// </summary>
            /// <param name="name">Name of the option; contents of the argument</param>
            /// <param name="args">Arguments of the option</param>
            /// <param name="type">Type of the argument</param>
            public Argument(String name, String[] args, Type type)
            {
                Name = name;
                Arguments = args;
                Type = type;
            }

            /// <summary>
            ///     Gets name of the option or contents of the argument
            /// </summary>
            public String Name { get; private set; }

            /// <summary>
            ///     Gets arguments of the option or an empty array if this is an argument
            /// </summary>
            public String[] Arguments { get; private set; }

            /// <summary>
            ///     Gets the type ot this CommandLineArgument object
            /// </summary>
            public Type Type { get; private set; }
        }

        /// <summary>
        /// Exception for unexpected command line arguments.
        /// </summary>
        public class CommandLineArgumentException : Exception
        {
            /// <summary>
            /// Constructor.
            /// Initializes a new instance.
            /// </summary>
            /// <param name="optionName"></param>
            /// <param name="message"></param>
            /// <param name="args"></param>
            public CommandLineArgumentException(String optionName, String message, params Object[] args)
                : base(String.Format(message, args))
            {
                ArgumentName = optionName;
            }

            /// <summary>
            /// Gets the argument name associated with the error.
            /// </summary>
            public String ArgumentName { get; set; }
        }

        #endregion

        /// <summary>
        ///     Overloaded.
        ///     Initializes a new instace from Environment.CommandLine.
        /// </summary>
        public CommandLine()
            : this(Environment.CommandLine)
        {
        }

        /// <summary>
        ///     Overloaded.
        ///     Initializes a new instance from specified command line string.
        /// </summary>
        /// <param name="cmdLine">Command line string</param>
        public CommandLine(String cmdLine)
        {
            List<String> argv = new List<string>();
            Int32 nextStart = 0;
            Boolean suppressFirst = false;

            foreach (Match m in Regex.Matches(cmdLine, "(\"[^\"]*\"|([^\" ])+)+", RegexOptions.ExplicitCapture))
            {
                if (m.Index != nextStart &&
                    !cmdLine.Substring(nextStart, m.Index - nextStart).All((char c) => { return c == ' '; }))
                    throw new InvalidOperationException();

                nextStart = m.Index + m.Length;

                if (!suppressFirst)
                {
                    suppressFirst = true;
                    continue;
                }

                if (m.Success)
                    argv.Add(DescapeQuotes(m.Value));
                else
                    throw new InvalidOperationException();
            }

            if (cmdLine.Length != nextStart && !cmdLine.Substring(nextStart).All((char c) => { return c == ' '; }))
                throw new InvalidOperationException();

            List<Argument> args = new List<Argument>();

            foreach (string arg in argv)
            {
                if (arg.StartsWith("/") || arg.StartsWith("-"))
                {
                    String narg = arg.Substring(1);
                    String name;
                    String opt;
                    Int32 index = narg.IndexOf(":");
                    if (index >= 0)
                    {
                        name = DescapeQuotes(narg.Substring(0, index));
                        opt = DescapeQuotes(narg.Substring(index + 1));
                    }
                    else
                    {
                        name = DescapeQuotes(narg);
                        opt = "";
                    }

                    args.Add(new Argument(name, (from s in opt.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                                                 select s.Trim()).ToArray(), Type.Option));
                }
                else
                    args.Add(new Argument(arg, new string[0], Type.Argument));
            }

            Arguments = args.ToArray();
        }


        /// <summary>
        ///     Command Line Arguments.
        /// </summary>
        public Argument[] Arguments { get; private set; }

        /// <summary>
        ///     Gets the specified command line argument
        /// </summary>
        /// <param name="n">Index of the argument to retrieve</param>
        /// <returns>Argument object</returns>
        public Argument this[Int32 n]
        {
            get { return Arguments[n]; }
        }

        /// <summary>
        ///     Gets all arguments with the specified name.
        /// </summary>
        /// <param name="name">Name of the argument, case insensitive.</param>
        /// <returns>Array of matching arguments.</returns>
        public Argument[] this[String name]
        {
            get
            {
                return
                    (from a in Arguments
                     where StringComparer.CurrentCultureIgnoreCase.Compare(a.Name, name) == 0
                     select a).ToArray();
            }
        }

        /// <summary>
        /// Gets the number of arguments.
        /// </summary>
        public Int32 Count
        { get { return Arguments.Length; } }


        /// <summary>
        /// Gets an array of argument strings.
        /// </summary>
        /// <returns></returns>
        public String[] GetArguments()
        {
            return (from a in Arguments where a.Type == Type.Argument select a.Name).ToArray();
        }

        /// <summary>
        /// Gets parameters of an option, or default values if the option
        /// does not exist.
        /// </summary>
        /// <param name="optionName">Name of the option.</param>
        /// <param name="def">Default value, defaults to null.</param>
        /// <returns></returns>
        public String[] GetOptionParams(String optionName, String[] def = null)
        {
            Argument arg =
                Arguments.FirstOrDefault(a => StringComparer.CurrentCultureIgnoreCase.Equals(optionName, a.Name));

            if (arg == null && def == null)
                throw new CommandLineArgumentException(optionName, "Could not find the required option.");
            return def ?? arg.Arguments;
        }

        /// <summary>
        /// Gets the first parameter of an option.
        /// </summary>
        /// <param name="optionName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public String GetOptionSrting(String optionName, String def = null)
        {
            Argument arg =
                Arguments.FirstOrDefault(a => StringComparer.CurrentCultureIgnoreCase.Equals(optionName, a.Name));
            if (def == null)
            {
                if (arg == null)
                    throw new CommandLineArgumentException(optionName, "Could not find the required option.");
                if (arg.Arguments.Length == 0)
                    throw new CommandLineArgumentException(optionName, "Expected at least one parameter for the option.");
            }
            else
            {
                if (arg != null && arg.Arguments.Length > 0) return arg.Arguments[0];
                return def;
            }

            return arg.Arguments[0];
        }

        /// <summary>
        /// Gets the first parameted of an option of as an integer.
        /// </summary>
        /// <param name="optionName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public Int32 GetOptionInt(String optionName, Int32 def = 0)
        {
            Argument arg =
    Arguments.FirstOrDefault(a => StringComparer.CurrentCultureIgnoreCase.Equals(optionName, a.Name));

            if (arg == null) return def;
            if (arg.Arguments.Length == 0) 
                throw new CommandLineArgumentException(optionName, "Expected an integer parameter for the option.");
            int r;
            if (!Int32.TryParse(arg.Arguments[0], out r)) 
                throw new CommandLineArgumentException(optionName, "Expected an integer parameter but could not parse it.");
            return r;
        }

        private static String DescapeQuotes(String str)
        {
            if (str == "") return "";
            if (str.StartsWith("\"") && str.EndsWith("\""))
                return str.Substring(1, str.Length - 2).Replace("\"\"", "\"");
            return str;
        }
    }
}