using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

[SuppressMessage("ReSharper", "InconsistentNaming", Scope = "global")]
internal static partial class Interop
{
    internal static partial class Kernel32
    {
#if NET7_0_OR_GREATER
        [LibraryImport(Libraries.Kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool AttachConsole(uint dwProcessId);
#else
        [DllImport(Libraries.Kernel32, SetLastError = true)]
        public static extern bool AttachConsole(uint dwProcessId);
#endif
    }
}