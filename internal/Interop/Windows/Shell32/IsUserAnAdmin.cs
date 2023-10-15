using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Shell32
    {
#if NET7_0_OR_GREATER
        [LibraryImport(Libraries.Shell32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool IsUserAnAdmin();
#else
        [DllImport(Libraries.Shell32, SetLastError = true)]
        public static extern bool IsUserAnAdmin();
#endif
    }
}