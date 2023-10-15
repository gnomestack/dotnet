using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Kernel32
    {
#if NET7_0_OR_GREATER
        [LibraryImport(Libraries.Kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GenerateConsoleCtrlEvent(int dwCtrlEvent, uint dwProcessGroupId);
#else
        [DllImport(Libraries.Kernel32, SetLastError = true)]
        public static extern bool GenerateConsoleCtrlEvent(int dwCtrlEvent, uint dwProcessGroupId);
#endif
    }
}