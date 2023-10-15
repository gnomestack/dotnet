// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

internal static partial class Interop
{
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static partial class Kernel32
    {
        internal const int STD_OUTPUT_HANDLE = -11;

        internal const int STD_INPUT_HANDLE = -10;

        internal const int STD_ERROR_HANDLE = -12;

        internal const int ENABLE_PROCESSED_INPUT = 0x0001;

        internal const int ENABLE_LINE_INPUT = 0x0002;

        internal const int ENABLE_ECHO_INPUT = 0x0004;

        internal const int ENABLE_WINDOW_INPUT = 0x0008;

        internal const int ENABLE_MOUSE_INPUT = 0x0010;

        internal const int ENABLE_INSERT_MODE = 0x0020;

        internal const int ENABLE_QUICK_EDIT_MODE = 0x0040;

        internal const int ENABLE_EXTENDED_FLAGS = 0x0080;

        internal const int ENABLE_AUTO_POSITION = 0x0100;

        internal const int ENABLE_PROCESSED_OUTPUT = 0x0001;

        internal const int ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002;

        internal const int ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

        internal const int DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

        internal const int ENABLE_LVB_GRID_WORLDWIDE = 0x0010;

#if NET7_0_OR_GREATER
        [LibraryImport(Libraries.Kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool GetConsoleMode(IntPtr handle, out int mode);

        internal static bool IsGetConsoleModeCallSuccessful(IntPtr handle)
        {
            return GetConsoleMode(handle, out _);
        }

        [LibraryImport(Libraries.Kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool SetConsoleMode(IntPtr handle, int mode);
#else
        [DllImport(Libraries.Kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetConsoleMode(IntPtr handle, out int mode);

        internal static bool IsGetConsoleModeCallSuccessful(IntPtr handle)
        {
            return GetConsoleMode(handle, out _);
        }

        [DllImport(Libraries.Kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetConsoleMode(IntPtr handle, int mode);
#endif
    }
}