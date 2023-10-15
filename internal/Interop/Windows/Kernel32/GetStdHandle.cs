// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Kernel32
    {
#if NET7_0_OR_GREATER

#if !NO_SUPPRESS_GC_TRANSITION
        [SuppressGCTransition]
#endif
        [LibraryImport(Libraries.Kernel32)]
        internal static partial IntPtr GetStdHandle(int nStdHandle);  // param is NOT a handle, but it returns one!
#else
        [DllImport(Libraries.Kernel32)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);  // param is NOT a handle, but it returns one!
#endif
    }
}