using System.Runtime.InteropServices;

namespace GnomeStack.Os.Secrets.Win32;

[CLSCompliant(false)]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct NativeCredential
{
    public uint Flags;
    public uint Type;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string TargetName;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string Comment;
    public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
    public uint CredentialBlobSize;
    public IntPtr CredentialBlob;
    public uint Persist;
    public uint AttributeCount;
    public IntPtr? Attributes;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string TargetAlias;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string UserName;
}