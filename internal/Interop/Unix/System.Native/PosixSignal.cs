// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Sys
    {
#if NET7_0 || NET7_0_OR_GREATER
        [LibraryImport(Libraries.SystemNative, EntryPoint = "SystemNative_SetPosixSignalHandler")]
        [SuppressGCTransition]
        internal static unsafe partial void SetPosixSignalHandler(delegate* unmanaged<int, PosixSignal, int> handler);

        [LibraryImport(Libraries.SystemNative, EntryPoint = "SystemNative_EnablePosixSignalHandling", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool EnablePosixSignalHandling(int signal);

        [LibraryImport(Libraries.SystemNative, EntryPoint = "SystemNative_DisablePosixSignalHandling")]
        internal static partial void DisablePosixSignalHandling(int signal);

        [LibraryImport(Libraries.SystemNative, EntryPoint = "SystemNative_HandleNonCanceledPosixSignal")]
        internal static partial void HandleNonCanceledPosixSignal(int signal);

        [LibraryImport(Libraries.SystemNative, EntryPoint = "SystemNative_GetPlatformSignalNumber")]
        [SuppressGCTransition]
        internal static partial int GetPlatformSignalNumber(PosixSignal signal);
#elif NET6_0
        [DllImport(Libraries.SystemNative, EntryPoint = "SystemNative_SetPosixSignalHandler")]
        [SuppressGCTransition]
        internal static extern unsafe void SetPosixSignalHandler(delegate* unmanaged<int, PosixSignal, int> handler);

        [DllImport(Libraries.SystemNative, EntryPoint = "SystemNative_EnablePosixSignalHandling", SetLastError = true)]
        internal static extern bool EnablePosixSignalHandling(int signal);

        [DllImport(Libraries.SystemNative, EntryPoint = "SystemNative_DisablePosixSignalHandling")]
        internal static extern void DisablePosixSignalHandling(int signal);

        [DllImport(Libraries.SystemNative, EntryPoint = "SystemNative_HandleNonCanceledPosixSignal")]
        internal static extern void HandleNonCanceledPosixSignal(int signal);

        [DllImport(Libraries.SystemNative, EntryPoint = "SystemNative_GetPlatformSignalNumber")]
        [SuppressGCTransition]
        internal static extern int GetPlatformSignalNumber(PosixSignal signal);
#endif
    }
}