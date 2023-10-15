// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Sys
    {
#if NET7_0_OR_GREATER
        [LibraryImport(Libraries.SystemNative, EntryPoint = "SystemNative_GetEGid")]
        internal static partial uint GetEGid();
#else
        [DllImport(Libraries.SystemNative, EntryPoint = "SystemNative_GetEGid")]
        internal static extern uint GetEGid();
#endif
    }
}