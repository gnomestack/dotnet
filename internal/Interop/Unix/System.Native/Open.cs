// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

using Microsoft.Win32.SafeHandles;

internal static partial class Interop
{
    internal static partial class Sys
    {
#if NET7_0_OR_GREATER
        [LibraryImport(Libraries.SystemNative, EntryPoint = "SystemNative_Open", StringMarshalling = StringMarshalling.Utf8, SetLastError = true)]
        internal static partial SafeFileHandle Open(string filename, OpenFlags flags, int mode);
#else
        [DllImport(Libraries.SystemNative, EntryPoint = "SystemNative_Open", SetLastError = true)]
        internal static extern SafeFileHandle Open(string filename, OpenFlags flags, int mode);
#endif
    }
}