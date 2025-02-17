﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TcUnit.TestAdapter.Models
{
    public class AdsFileSystemTypes
    {
        /// <summary>
        /// E_FopenMode
        /// Contains the mode in which the file is to be opened. 
        /// The codes listed below are the various opening modes which are already pre-defined as constants in the library and which can accordingly be passed symbolically to the block.
        /// </summary>
        public enum AdsFileOpenMode : uint
        {
            FOPEN_MODEREAD = 0x1,    // "r": Opens for reading. If the file does not exist or cannot be found, the call fails.
            FOPEN_MODEWRITE = 0x2,    // "w": Opens an empty file for writing. If the given file exists, its contents are destroyed.
            FOPEN_MODEAPPEND = 0x4,    // "a": Opens for writing at the end of the file (appending) without removing the EOF marker before writing new data to the file; creates the file first if it doesn’t exist.
            FOPEN_MODEPLUS = 0x8,    // "+": Opens for reading AND writing
            FOPEN_MODEBINARY = 0x10,   // "b": Open in binary (untranslated) mode.
            FOPEN_MODETEXT = 0x20    // "t": Open in text (translated) mode.
        }

        /// <summary>
        /// E_OpenPath
        /// File open path
        /// </summary>
        public enum AdsDirectory : int
        {
            PATH_GENERIC = 1,       // search/open/create files in selected/generic folder 
            PATH_BOOTPRJ,           // search/open/create files in TwinCAT boot project folder and adds the *.wbp extension 
            PATH_BOOTDATA,          // reserved for future use
            PATH_BOOTPATH,          // refers to the TwinCAT/Boot directory without adding an extension (.wbp) 
            PATH_USERPATH1 = 11,    // reserved for future use
            PATH_USERPATH2,         // reserved for future use
            PATH_USERPATH3,         // reserved for future use
            PATH_USERPATH4,         // reserved for future use
            PATH_USERPATH5,         // reserved for future use
            PATH_USERPATH6,         // reserved for future use
            PATH_USERPATH7,         // reserved for future use
            PATH_USERPATH8,         // reserved for future use
            PATH_USERPATH9          // reserved for future use
        }

        /// <summary>
        /// E_SeekOrigin
        /// File seek origin constants
        /// </summary>
        public enum AdsFileSystemSeekOrigin : int
        {
            SEEK_SET = 0,   // Seek from beginning of file 
            SEEK_CUR,           // Seek from current position of file pointer 
            SEEK_END            // Seek from the end of file 
        }

        /// <summary>
        /// E_IdxGrp
        /// Enumeration for special ADS-IndexGroups for Fileoperations
        /// </summary>
        public enum AdsIndexGroup : int
        {
            //System service index groups
            SYSTEMSERVICE_OPENCREATE = 100,
            SYSTEMSERVICE_OPENREAD = 101,
            SYSTEMSERVICE_OPENWRITE = 102,
            SYSTEMSERVICE_CREATEFILE = 110,
            SYSTEMSERVICE_CLOSEHANDLE = 111,
            SYSTEMSERVICE_FOPEN = 120,
            SYSTEMSERVICE_FCLOSE = 121,
            SYSTEMSERVICE_FREAD = 122,
            SYSTEMSERVICE_FWRITE = 123,
            SYSTEMSERVICE_FSEEK = 124,
            SYSTEMSERVICE_FTELL = 125,
            SYSTEMSERVICE_FGETS = 126,
            SYSTEMSERVICE_FPUTS = 127,
            SYSTEMSERVICE_FSCANF = 128,
            SYSTEMSERVICE_FPRINTF = 129,
            SYSTEMSERVICE_FEOF = 130,
            SYSTEMSERVICE_FDELETE = 131,
            SYSTEMSERVICE_FRENAME = 132,

            SYSTEMSERVICE_FFILEFIND = 133,

            SYSTEMSERVICE_MKDIR = 138,
            SYSTEMSERVICE_RMDIR = 139,
        }

        /// <summary>
        /// E_EnumCmdType
        /// Enumeration control function type
        /// </summary>
        public enum AdsFileSystemCommandType
        {
            eEnumCmd_First = 0, // Enumerate first element
            eEnumCmd_Next, // Enumerate next element
            eEnumCmd_Abort // Abort enumeration, close enumeration handle
        }

        /// <summary>
        /// ST_FileAttributes
        /// Find file attributes
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct AdsFileSystemEntryAttributes
        {
            [MarshalAs(UnmanagedType.I1)]
            public bool ReadOnly;              // FILE_ATTRIBUTE_READONLY. The file or directory is read-only. Applications can read the file but cannot write to it or delete it. In the case of a directory, applications cannot delete it.
            [MarshalAs(UnmanagedType.I1)]
            public bool Hidden;                // FILE_ATTRIBUTE_HIDDEN. The file or directory is hidden. It is not included in an ordinary directory listing.
            [MarshalAs(UnmanagedType.I1)]
            public bool System;                // FILE_ATTRIBUTE_SYSTEM. The file or directory is part of the operating system or is used exclusively by the operating system.
            [MarshalAs(UnmanagedType.I1)]
            public bool Directory;             // FILE_ATTRIBUTE_DIRECTORY.The handle identifies a directory.
            [MarshalAs(UnmanagedType.I1)]
            public bool Archive;               // FILE_ATTRIBUTE_ARCHIVE. The file or directory is an archive file or directory. Applications use this attribute to mark files for backup or removal.
            [MarshalAs(UnmanagedType.I1)]
            public bool Device;                // FILE_ATTRIBUTE_DEVICE. Under CE: FILE_ATTRIBUTE_INROM or FILE_ATTRIBUTE_ENCRYPTED 
            [MarshalAs(UnmanagedType.I1)]
            public bool Normal;                // FILE_ATTRIBUTE_NORMAL. The file or directory has no other attributes set. This attribute is valid only if used alone.
            [MarshalAs(UnmanagedType.I1)]
            public bool Temporary;             // FILE_ATTRIBUTE_TEMPORARY. The file is being used for temporary storage. File systems attempt to keep all of the data in memory for quicker access, rather than flushing it back to mass storage. A temporary file should be deleted by the application as soon as it is no longer needed.
            [MarshalAs(UnmanagedType.I1)]
            public bool SparseFile;            // FILE_ATTRIBUTE_SPARSE_FILE. The file is a sparse file.
            [MarshalAs(UnmanagedType.I1)]
            public bool ReparsePoint;          // FILE_ATTRIBUTE_REPARSE_POINT. The file has an associated reparse point.
            [MarshalAs(UnmanagedType.I1)]
            public bool Compressed;            // FILE_ATTRIBUTE_COMPRESSED. The file or directory is compressed. For a file, this means that all of the data in the file is compressed. For a directory, this means that compression is the default for newly created files and subdirectories.
            [MarshalAs(UnmanagedType.I1)]
            public bool Offline;               // FILE_ATTRIBUTE_OFFLINE. Under CE: FILE_ATTRIBUTE_ROMSTATICREF. The file data is not immediately available. This attribute indicates that the file data has been physically moved to offline storage. This attribute is used by Remote Storage, the hierarchical storage management software.
            [MarshalAs(UnmanagedType.I1)]
            public bool NotContentIndexed;     // FILE_ATTRIBUTE_NOT_CONTENT_INDEXED. Under CE: FILE_ATTRIBUTE_ROMMODULE  
            [MarshalAs(UnmanagedType.I1)]
            public bool Encrypted;             // FILE_ATTRIBUTE_ENCRYPTED. The file or directory is encrypted. For a file, this means that all data in the file is encrypted. For a directory, this means that encryption is the default for newly created files and subdirectories.
        }

        /// <summary>
        /// T_FILETIME
        /// The FILETIME structure is a 64-bit value representing the number of 100-nanosecond intervals since January 1, 1601 (UTC).
        /// </summary>
        public struct AdsFileTime
        {
            public uint LowDateTime;        // Specifies the low-order 32 bits of the file time. 
            public uint HighDateTime;       // Specifies the high-order 32 bits of the file time. 
        }

        /// <summary>
        /// ST_FindFileEntry
        /// Find file information
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct AdsFileSystemEntry
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]   // evtl. 255
            public string FileName;                                // A null-terminated string that is the name of the file.
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]    // evtl. 13
            public string AlternateFileName;                       // A null-terminated string that is an alternative name for the file. This name is in the classic 8.3 (filename.ext) file name format.
            public AdsFileSystemEntryAttributes fileAttributes;                // Specifies the file attributes of the file found.
            public ulong FileSize;                                 // 64 bit unsigned integer. The size of the file is equal to (dwHighPart * (0xFFFFFFFF+1)) + dwLowPart.
            public AdsFileTime CreationTime;                         // For both files and directories, the structure specifies when the file or directory was created. 
            public AdsFileTime LastAccessTime;                       // For a file, the structure specifies when the file was last read from or written to.For a directory, the structure specifies when the directory was created.
            public AdsFileTime LastWriteTime;                        //  For a file, the structure specifies when the file was last written to. For a directory, the structure specifies when the directory was created.
        }

        /// <summary>
        /// ST_AmsFindFileSystemEntry
        /// FindFile ads request 4 byte aligned // todo: check alignment
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct AmsFileSystemEntry
        {
            public ushort File;                // File handle (4 byte) 
            public ushort Reserved;

            public uint FileAttributes;             //   4
            public AdsFileTime CreationTime;             //   8 
            public AdsFileTime LastAccessTime;           //   8 
            public AdsFileTime LastWriteTime;            //   8 
            public uint FileSizeHigh;                //   4 
            public uint FileSizeLow;                 //   4 
            public uint Reserved0;                   //   4 
            public uint Reserved1;                   //   4 
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string FileName;                    // 256 
                                                        // C struct uses char[260] --> nReserved2 added 
            public uint Reserved2;                   //   4 
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string AlternateFileName;           //  14 
                                                        // C struct has 4 byte alignment --> nReserved3 added 
            public ushort Reserved3;                   //   2 
                                                        //=324 
        }

        /// <summary>
        /// ST_FindFileResult
        /// Contains returndata from FB_EnumFindFileEntry
        /// </summary>
        public struct AdsFindFileSystemEntryResult
        {
            public AdsFileSystemEntry FileEntry;
            public ushort File;
            public bool EOF;
            public bool Error;
            public uint ErrorID;
        }
    }
}
