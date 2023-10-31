// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;

#pragma warning disable SA1307, S101, SA1202, S3903
internal static partial class Interop
{
    internal static partial class NtDll
    {
    #if NET7_0_OR_GREATER
        [LibraryImport(Libraries.NtDll)]
        private static partial int RtlGetVersion(ref OSVERSIONINFOEX lpVersionInformation);
    #else
        [DllImport(Libraries.NtDll)]
        private static extern int RtlGetVersion(ref OSVERSIONINFOEX lpVersionInformation);
    #endif

        internal static unsafe int RtlGetVersionEx(out OSVERSIONINFOEX osvi)
        {
            osvi = default;
            osvi.dwOSVersionInfoSize = (uint)sizeof(OSVERSIONINFOEX);
            return RtlGetVersion(ref osvi);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal unsafe struct OSVERSIONINFOEX
        {
            public uint dwOSVersionInfoSize;
            public uint dwMajorVersion;
            public uint dwMinorVersion;
            public uint dwBuildNumber;
            public uint dwPlatformId;
            public fixed char szCSDVersion[128];
            public ushort wServicePackMajor;
            public ushort wServicePackMinor;
            public ushort wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }
    }
}