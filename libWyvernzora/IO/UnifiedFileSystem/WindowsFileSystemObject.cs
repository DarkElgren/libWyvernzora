// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/WindowsFileSystemObject.cs
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
using System.IO;
using System.Linq;

namespace libWyvernzora.IO.UnifiedFileSystem
{
    /// <summary>
    ///     Represents a file stored in a local directory.
    ///     Case insensitive.
    /// </summary>
    public class WindowsFileSystemObject : FileSystemObject
    {
        private readonly WindowsFileSystem fileSystem;
        private readonly FileSystemInfo finfo;
        private WindowsFileSystemObject parent;

        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="fs">WindowsFileSystem that owns this WindowsFileSystemObject.</param>
        /// <param name="file">FileInfo object to wrap.</param>
        public WindowsFileSystemObject(WindowsFileSystem fs, FileInfo file)
            : base(file.Name, FileSystemObjectType.File, file.Length)
        {
            fileSystem = fs;
            finfo = file;
        }

        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="fs">WindowsFileSystem that owns this WindowsFileSystemObject.</param>
        /// <param name="dir">DirectoryInfo object to be wrapped.</param>
        public WindowsFileSystemObject(WindowsFileSystem fs, DirectoryInfo dir)
            : base(dir.Name, FileSystemObjectType.Directory, -1)
        {
            fileSystem = fs;
            finfo = dir;
        }

        #region Overrides

        public override FileSystemObject Parent
        {
            get
            {
                if (parent == null)
                {
                    String absPath = PathEx.GetParentDirectory(finfo.FullName);
                    if (fileSystem.CanAccessAbsolutePath(absPath))
                        parent = new WindowsFileSystemObject(fileSystem, new DirectoryInfo(absPath));
                }

                return parent;
            }
            set
            {
                /* silently suppress */
            }
        }

        public override string Path
        {
            get { return PathEx.GetRelativePath(finfo.FullName, fileSystem.RootDirectoryPath); }
        }

        public override long Length
        {
            get
            {
                FileInfo info = finfo as FileInfo;
                if (info == null) return -1;
                return info.Length;
            }
            set
            {
                /* silently suppress */
            }
        }

        public override IList<FileSystemObject> GetChildren()
        {
            List<FileSystemObject> children = new List<FileSystemObject>();
            DirectoryInfo info = finfo as DirectoryInfo;
            if (info != null)
            {
                children.AddRange(from d in info.GetDirectories() select new WindowsFileSystemObject(fileSystem, d));
                children.AddRange(from d in info.GetFiles() select new WindowsFileSystemObject(fileSystem, d));
            }
            return children;
        }

        public override bool HasChild(string name)
        {
            DirectoryInfo info = finfo as DirectoryInfo;
            if (info == null) return false;

            String newPath = PathEx.CombinePath(info.FullName, name);
            return File.Exists(newPath) || Directory.Exists(newPath);
        }

        public override FileSystemObject GetChild(string name)
        {
            DirectoryInfo info = finfo as DirectoryInfo;
            if (info == null) throw new InvalidOperationException("File does not have children!");

            // TODO Security Check to disallow access above root
            String newPath = PathEx.CombinePath(info.FullName, name);
            if (File.Exists(newPath))
                return new WindowsFileSystemObject(fileSystem, new FileInfo(newPath));
            if (Directory.Exists(newPath))
                return new WindowsFileSystemObject(fileSystem, new DirectoryInfo(newPath));
            throw new Exception("Child not found!");
        }

        #endregion

        #region Windows File Specific Properties

        /// <summary>
        ///     Gets FileSystemInfo wrapped in the FileSystemObject instance.
        /// </summary>
        public FileSystemInfo FileSystemInfo
        {
            get { return finfo; }
        }

        /// <summary>
        ///     Gets a value indicating whether the file exists.
        /// </summary>
        public Boolean Exists
        {
            get { return finfo.Exists; }
        }

        /// <summary>
        ///     Gets the full path of the directory or file.
        /// </summary>
        /// <remarks>
        ///     WindowsFileSystem does not allow visiting files above the root
        ///     directory of the file system, therefore FullName is not absolute
        ///     path but relative path.
        /// </remarks>
        public String FullName
        {
            get { return Path; }
        }

        /// <summary>
        ///     Gets or sets the creation time of the current file.
        /// </summary>
        public DateTime CreationTime
        {
            get { return finfo.CreationTime; }
            set { finfo.CreationTime = value; }
        }

        /// <summary>
        ///     Gets or sets the creation time, in coordinated universal time (UTC), of the current file.
        /// </summary>
        public DateTime CreationTimeUtc
        {
            get { return finfo.CreationTimeUtc; }
            set { finfo.CreationTimeUtc = value; }
        }

        /// <summary>
        ///     Gets or sets the time the current file was last accessed.
        /// </summary>
        public DateTime LastAccessTime
        {
            get { return finfo.LastAccessTime; }
            set { finfo.LastAccessTime = value; }
        }

        /// <summary>
        ///     Gets or sets the time, in coordinated universal time (UTC), that the current file was last accessed.
        /// </summary>
        public DateTime LastAccessTimeUtc
        {
            get { return finfo.LastAccessTimeUtc; }
            set { finfo.LastAccessTimeUtc = value; }
        }

        /// <summary>
        ///     Gets or sets the time when the current file was last written to.
        /// </summary>
        public DateTime LastWriteTime
        {
            get { return finfo.LastWriteTime; }
            set { finfo.LastWriteTime = value; }
        }

        /// <summary>
        ///     Gets or sets the time, in coordinated universal time (UTC), when the current file was last written to.
        /// </summary>
        public DateTime LastWriteTimeUtc
        {
            get { return finfo.LastWriteTimeUtc; }
            set { finfo.LastWriteTimeUtc = value; }
        }

        #endregion
    }
}