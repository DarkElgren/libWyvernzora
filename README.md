libWyvernzora
=============

**libWyvernzora** is my personal library with a big bunch of stuff that comes in handy whenever I code something.

A very big chunk of code in libWyvernzora is re-implementation of Firefly Framework by F.R.C., but I am constantly making improvements over it. This library will eventually include everything I would ever need to reuse from my older projects: data structures, extension methods, custom services and much more.

Right now this library is still work in progress.

Here is a list of ideas already implemented in the **libWyvernzora**:
 - [Bit conversion supporting both Little Endian and Big Endian](Documentation/BitConversion.md) ( _**libWyvernzora.Core.DirectIntConv** class_ )
 - Advanced Enumerators ( _**libWyvernzora.Collections** namespace_ )
 - Generic String ( _**libWyvernzora.Collections.ListStringEx<T>** class_ )
 - Partial List ( _**libWyvernzora.Collections.PartialList<T>** class_ )
 - Stream-like Array Interface ( _**libWyvernzora.Collections.ArrayStream<T>** class_ )
 - File Extension Analysis ( _**libWyvernzora.Collections.FileNameDescriptor** class_ )
 - Extended Stream ( _**libWyvernzora.IO.StreamEx** class_ )
 - Concurrent Partial Stream ( _**libWyvernzora.IO.PartialStreamEx** class_ )
 - Unified File System ( _**libWyvernzora.IO.UnifiedFileSystem** namespace_ )
 - Action Lock ( _**libWyvernzora.Utilities.ActionLock** class_ )
 - Command Line Utility ( _**libWyvernzora.Utilities.CommandLine** class_ )
 - Extended Random Number Generator ( _**libWyvernzora.Utilities.RandomEx** class_ )