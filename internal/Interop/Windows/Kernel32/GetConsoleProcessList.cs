using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Kernel32
    {
#if NET7_0_OR_GREATER
        [LibraryImport(Libraries.Kernel32, SetLastError = true)]
        public static partial uint GetConsoleProcessList(uint[] lpdwProcessList, uint dwProcessCount);
#else
        [DllImport(Libraries.Kernel32, SetLastError = true)]
        public static extern uint GetConsoleProcessList(uint[] lpdwProcessList, uint dwProcessCount);
#endif
    }
}