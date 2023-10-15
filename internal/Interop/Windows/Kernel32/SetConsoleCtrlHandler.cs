using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[SuppressMessage("ReSharper", "InconsistentNaming", Scope = "global")]
internal static partial class Interop
{
    internal const int CTRL_C_EVENT = 0;
    internal const int CTRL_BREAK_EVENT = 1;
    internal const int CTRL_CLOSE_EVENT = 2;
    internal const int CTRL_LOGOFF_EVENT = 5;
    internal const int CTRL_SHUTDOWN_EVENT = 6;

    internal delegate bool ConsoleCtrlHandlerRoutine(int controlType);

#if NET7_0_OR_GREATER

    [LibraryImport(Libraries.Kernel32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool SetConsoleCtrlHandler(ConsoleCtrlHandlerRoutine handler, [MarshalAs(UnmanagedType.Bool)] bool addOrRemove);

    [LibraryImport(Libraries.Kernel32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static unsafe partial bool SetConsoleCtrlHandler(delegate* unmanaged<int, BOOL> handlerRoutine, [MarshalAs(UnmanagedType.Bool)] bool add);
#else
    [DllImport(Libraries.Kernel32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetConsoleCtrlHandler(ConsoleCtrlHandlerRoutine handler, [MarshalAs(UnmanagedType.Bool)] bool addOrRemove);

#if NET5_0_OR_GREATER
    [DllImport(Libraries.Kernel32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern unsafe bool SetConsoleCtrlHandler(delegate* unmanaged<int, BOOL> handlerRoutine, bool add);
#endif
#endif
}