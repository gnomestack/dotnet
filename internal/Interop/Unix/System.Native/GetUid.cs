// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Sys
    {
#if NET7_0_OR_GREATER
        [LibraryImport(Libraries.Libc, EntryPoint = "getuid")]
        internal static partial uint GetUid();
#else
        [DllImport(Libraries.SystemNative, EntryPoint = "getuid")]
        internal static extern uint GetUid();
#endif
    }
}