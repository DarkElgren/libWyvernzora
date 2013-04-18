// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/WindowsFileSystem.cs
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using libWyvernzora.Collections;

namespace libWyvernzora.IO.UnifiedFileSystem
{
    /// <summary>
    ///     IFileSystemService wrapper for local directories.
    ///     Case insensitive.
    /// </summary>
    public sealed class WindowsFileSystem : IFileSystem<WindowsFileSystemObject>
    {
        private readonly WindowsFileSystemObject rootDirectory;

        /// <summary>
        ///     Constructor.
        ///     Initializes a new instance.
        /// </summary>
        /// <param name="directory">Path of the root directory of the file system.</param>
        public WindowsFileSystem(String directory)
        {
            if (!Directory.Exists(directory))
                throw new Exception("Cannot create WindowsFileSystem: cannot find root directory.");

            RootDirectoryPath = directory;
            rootDirectory = new WindowsFileSystemObject(this, new DirectoryInfo(RootDirectoryPath));
        }

        /// <summary>
        ///     Gets absolute path of the file system root.
        /// </summary>
        public string RootDirectoryPath { get; private set; }

        #region IFileSystem Members

        public bool IsReadOnly { get; private set; }

        public IEnumerable<WindowsFileSystemObject> Files
        {
            get
            {
                return
                    new EnumeratorEnumerable<WindowsFileSystemObject>(
                        new BfsDirectoryEnumerator<WindowsFileSystemObject>(rootDirectory));
            }
        }

        public WindowsFileSystemObject Root
        {
            get { return rootDirectory; }
        }

        public WindowsFileSystemObject GetFileSystemObject(string path)
        {
            if (CanAccessRelativePath(path))
                return !Root.HasChild(path) ? null : (WindowsFileSystemObject) Root.GetChild(path);

            throw new UnauthorizedAccessException("WindowsFileSystem does not allow access beyond file system root!");
        }

        public StreamEx OpenFileSystemObject(WindowsFileSystemObject obj, FileAccess mode)
        {
            FileInfo info = obj.FileSystemInfo as FileInfo;
            if (info == null) return null;
            if (!CanAccessAbsolutePath(info.DirectoryName))
                throw new UnauthorizedAccessException("Cannot open file that is beyond file system root!");

            if (IsReadOnly) mode = FileAccess.Read;

            return new StreamEx(info.FullName, FileMode.Open, mode);
        }

        public void Close()
        {
        }

        public void Dispose()
        {
        }

        public void DeleteFileSystemObject(WindowsFileSystemObject obj)
        {
            if (CanAccessAbsolutePath(obj.FileSystemInfo.FullName))
            {
                if (obj.Type == FileSystemObjectType.File) obj.FileSystemInfo.Delete();
                else
                {
                    Action<String> rm = null;
                    rm = (@s) =>
                        {
                            String[] files = Directory.GetFiles(s);
                            foreach (var f in files) File.Delete(f);

                            String[] dirs = Directory.GetDirectories(s);
                            foreach (var d in dirs) rm(d);

                            Directory.Delete(s);
                        };
                    rm(obj.FileSystemInfo.FullName);
                }
            }
        }

        public void MoveFileSystemObject(WindowsFileSystemObject obj, string newPath)
        {
            throw new NotSupportedException("WindowsFileSystem does not support moving files at this point");
        }


        IEnumerable<FileSystemObject> IFileSystem.Files
        {
            get { return Files; }
        }

        FileSystemObject IFileSystem.Root
        {
            get { return Root; }
        }

        FileSystemObject IFileSystem.GetFileSystemObject(string path)
        {
            return GetFileSystemObject(path);
        }


        public StreamEx OpenFileSystemObject(FileSystemObject obj, FileAccess mode)
        {
            var wobj = obj as WindowsFileSystemObject;
            if (wobj == null) throw new ArgumentException("FileSystemObject must be a WindowsFileSystemObject!");

            return OpenFileSystemObject(wobj, mode);
        }

        public void CreateFileSystemObject(string path, FileSystemObjectType type)
        {
            if (type == FileSystemObjectType.Directory) CreateDirectory(path);
            else CreateFile(path);
        }

        public void DeleteFileSystemObject(FileSystemObject obj)
        {
            var wobj = obj as WindowsFileSystemObject;
            if (wobj == null) throw new ArgumentException("FileSystemObject must be a WindowsFileSystemObject!");
            
            DeleteFileSystemObject(wobj);
        }

        public void MoveFileSystemObject(FileSystemObject obj, string newPath)
        {
            var wobj = obj as WindowsFileSystemObject;
            if (wobj == null) throw new ArgumentException("FileSystemObject must be a WindowsFileSystemObject!");

            MoveFileSystemObject(wobj, newPath);
        }

        #endregion

        #region WindowsFileSystem specific methods

        /// <summary>
        ///     Creates a directory in the WindowsFileSystem.
        /// </summary>
        /// <param name="path">Path of the new directory, relative to the file system root.</param>
        /// <returns>WindowsFileSystemObject wrapping around DirectoryInfo of the created directory.</returns>
        public WindowsFileSystemObject CreateDirectory(String path)
        {
            if (!CanAccessRelativePath(PathEx.GetParentDirectory(path)))
                throw new UnauthorizedAccessException(
                    "WindowsFileSystem does not allow aceess beyond the file system root!");

            String abspath = PathEx.GetAbsolutePath(path, RootDirectoryPath);
            if (!Directory.Exists(abspath)) Directory.CreateDirectory(abspath);

            return new WindowsFileSystemObject(this, new DirectoryInfo(abspath));
        }

        /// <summary>
        ///     Creates an empty file in the WindowsFileSystem.
        /// </summary>
        /// <param name="path">Path of the new file, relative to the file system root.</param>
        /// <returns>WindowsFileSystemObject wrapping around FileInfo of the created directory.</returns>
        public WindowsFileSystemObject CreateFile(String path)
        {
            if (!CanAccessRelativePath(PathEx.GetParentDirectory(path)))
                throw new UnauthorizedAccessException(
                    "WindowsFileSystem does not allow aceess beyond the file system root!");

            String abspath = PathEx.GetAbsolutePath(path, RootDirectoryPath);

            if (!Directory.Exists(PathEx.GetParentDirectory(abspath)))
                Directory.CreateDirectory(PathEx.GetParentDirectory(abspath));

            if (!File.Exists(abspath))
                File.Create(abspath).Dispose();

            return new WindowsFileSystemObject(this, new FileInfo(abspath));
        }

        // TODO Delete and Move

        #endregion

        #region Utility Methods

        /// <summary>
        ///     Checks whether a relative path leads outside of the file system.
        /// </summary>
        /// <param name="relDirectoryPath">Relative path to check, must be a directory path!</param>
        /// <returns>True if the specified directory is within the file system; false otherwise.</returns>
        public Boolean CanAccessRelativePath(String relDirectoryPath)
        {
            String newPath = PathEx.GetAbsolutePath(relDirectoryPath, RootDirectoryPath);
            return CanAccessAbsolutePath(newPath);
        }

        /// <summary>
        ///     Checks whether a path is outside of the file system.
        /// </summary>
        /// <param name="absDirectoryPath">Relative path to check, must be a directory path!</param>
        /// <returns>True if the specified directory is within the file system; false otherwise.</returns>
        public Boolean CanAccessAbsolutePath(String absDirectoryPath)
        {
            return (absDirectoryPath.StartsWith(RootDirectoryPath, StringComparison.CurrentCultureIgnoreCase));
        }

        #endregion

        private class BfsDirectoryEnumerator<T> : IEnumerator<T> where T : FileSystemObject
        {
            private readonly T root;
            private Queue<T> queue;

            public BfsDirectoryEnumerator(T obj)
            {
                root = obj;
                queue = new Queue<T>();
                foreach (FileSystemObject child in obj.GetChildren())
                    queue.Enqueue((T) child);
            }

            public T Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
                queue = null;
            }

            public bool MoveNext()
            {
                Current = queue.Dequeue();
                if (Current.Type == FileSystemObjectType.Directory)
                {
                    foreach (FileSystemObject child in Current.GetChildren())
                        queue.Enqueue((T) child);
                }
                return queue.Count > 0;
            }

            public void Reset()
            {
                queue.Clear();
                Current = null;
                queue.Enqueue(root);
            }
        }



    }
}