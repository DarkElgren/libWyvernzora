// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/FileSystemObject.cs
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
using System.Diagnostics.CodeAnalysis;

namespace libWyvernzora.IO.FileSystemService
{
    /// <summary>
    ///     Represents an Object in a File System.
    ///     Please derive this class for custom file system services.
    /// </summary>
    public abstract class FileSystemObject
    {
        protected String dispName;
        public List<FileSystemObject> Children { get; private set; }
        public Dictionary<String, FileSystemObjectType> ChildrenMap { get; private set; }
        public FileSystemObject Parent { get; set; }

        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="name">Name of the FileSystemObject.</param>
        /// <param name="type">Type of the FileSystemObject.</param>
        /// <param name="length">Length of the FileSystemObject.</param>
        /// <param name="dispName">Display Name of the FileSystemObject.</param>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected FileSystemObject(String name, FileSystemObjectType type, Int64 length, String dispName = "")
        {
            Name = name;
            Type = type;
            Length = length;
            DisplayName = dispName;

            Children = new List<FileSystemObject>();
            ChildrenMap =
                new Dictionary<string, FileSystemObjectType>(IsCaseSensitive
                                                                 ? StringComparer.CurrentCulture
                                                                 : StringComparer.CurrentCultureIgnoreCase);
        }

        /// <summary>
        ///     Determines whether FileSystemObject is case sensitive.
        /// </summary>
        public abstract Boolean IsCaseSensitive { get; }

        /// <summary>
        ///     Internal name of the FileSystemObject.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        ///     Display name of the FileSystemObject.
        /// </summary>
        public String DisplayName
        {
            get { return String.IsNullOrEmpty(dispName) ? Name : dispName; }
            set { dispName = value; }
        }

        /// <summary>
        ///     Length of the FileSystemObject.
        /// </summary>
        public Int64 Length { get; set; }

        /// <summary>
        ///     Type of the FileSystemObject.
        /// </summary>
        public FileSystemObjectType Type { get; set; }

        /// <summary>
        ///     Path of the FileSystemObject, relative to the root of the File System.
        /// </summary>
        public String Path
        {
            get
            {
                FileSystemObject temp = Parent;
                String path = Name;
                while (temp != null)
                {
                    path = PathEx.CombinePath(temp.Name, path);
                    temp = temp.Parent;
                }
                return path;
            }
        }
    }
}