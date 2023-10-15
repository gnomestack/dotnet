// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Sys
    {
#pragma warning disable S2344
#pragma warning disable S2346
#pragma warning disable SA1025
#pragma warning disable S1939
        internal enum Signals : int
        {
            None = 0,
            SIGHUP = 1,
            SIGINT = 2,
            SIGQUIT = 3,
            SIGILL = 4,
            SIGTRAP = 5,
            SIGABRT = 6,
            SIGBUS = 7,
            SIGFPE = 8,
            SIGKILL = 9,
            SIGUSR1 = 10,
            SIGSEGV = 11,
            SIGUSR2 = 12,
            SIGPIPE = 13,
            SIGALRM = 14,
            SIGTERM = 15,
            SIGSTKFLT = 16,
            SIGCHLD = 17,
            SIGCONT = 18,
            SIGSTOP = 19,
            SIGTSTP = 20,
            SIGTTIN = 21,
            SIGTTOU = 22,
            SIGURG = 23,
            SIGXCPU = 24,
            SIGXFSZ = 25,
            SIGVTALRM = 26,
        }

#if NET7_0_OR_GREATER
        [LibraryImport(Libraries.SystemNative, EntryPoint = "SystemNative_Kill", SetLastError = true)]
        internal static partial int Kill(int pid, Signals signal);
#else
        [DllImport(Libraries.SystemNative, EntryPoint = "SystemNative_Kill", SetLastError = true)]
        internal static extern int Kill(int pid, Signals signal);
#endif
    }
}