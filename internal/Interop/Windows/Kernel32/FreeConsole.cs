using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

[SuppressMessage("ReSharper", "InconsistentNaming", Scope = "global")]
internal static partial class Interop
{
    internal static partial class Kernel32
    {
#if NET7_0_OR_GREATER
        [LibraryImport(Libraries.Kernel32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool FreeConsole();
#else
        [DllImport(Libraries.Kernel32)] // no SetLastError since we don't care if this fails
        public static extern bool FreeConsole();
#endif
    }
}