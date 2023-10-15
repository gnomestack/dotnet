// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;
#pragma warning disable CS0649

internal static partial class Interop
{
    internal static partial class Sys
    {
#if NET7_0_OR_GREATER
        [LibraryImport(Libraries.SystemNative, StringMarshalling = StringMarshalling.Utf8, EntryPoint = "SystemNative_GetPwUidR", SetLastError = false)]
        internal static unsafe partial int GetPwUidR(uint uid, out Passwd pwd, byte* buf, int bufLen);

        [LibraryImport(Libraries.SystemNative, StringMarshalling = StringMarshalling.Utf8, EntryPoint = "SystemNative_GetPwNamR", SetLastError = false)]
        internal static unsafe partial int GetPwNamR(string name, out Passwd pwd, byte* buf, int bufLen);
#else
        [DllImport(Libraries.SystemNative, EntryPoint = "SystemNative_GetPwUidR", SetLastError = false)]
        internal static extern unsafe int GetPwUidR(uint uid, out Passwd pwd, byte* buf, int bufLen);

        [DllImport(Libraries.SystemNative, EntryPoint = "SystemNative_GetPwNamR", SetLastError = false)]
        internal static extern unsafe int GetPwNamR(string name, out Passwd pwd, byte* buf, int bufLen);
#endif
#pragma warning disable S2344
#pragma warning disable S2346
#pragma warning disable SA1025
#pragma warning disable SA1310

        internal unsafe struct Passwd
        {
            internal const int InitialBufferSize = 256;

            internal byte* Name;
            internal byte* Password;
            internal uint UserId;
            internal uint GroupId;
            internal byte* UserInfo;
            internal byte* HomeDirectory;
            internal byte* Shell;
        }
    }
}