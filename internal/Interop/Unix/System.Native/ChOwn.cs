// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Sys
    {
#if NET7_0_OR_GREATER
        [LibraryImport(Libraries.Libc, EntryPoint = "chown", StringMarshalling = StringMarshalling.Utf8, SetLastError = true)]
        internal static partial int ChOwn(string path, int owner, int group);

        [LibraryImport(Libraries.Libc, EntryPoint = "lchown", StringMarshalling = StringMarshalling.Utf8, SetLastError = true)]
        internal static partial int LChOwn(string path, int owner, int group);
#else
        [DllImport(Libraries.Libc, EntryPoint = "chown", SetLastError = true)]
        internal static extern int ChOwn(string path, int owner, int group);

        [DllImport(Libraries.Libc, EntryPoint = "chown", SetLastError = true)]
        internal static extern int LChOwn(string path, int owner, int group);
#endif
    }
}