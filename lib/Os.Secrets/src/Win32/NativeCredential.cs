using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace GnomeStack.Os.Secrets.Win32;

[CLSCompliant(false)]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct NativeCredential
{
    public int Flags;
    public WinCredType Type;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string TargetName;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string Comment;
    public FILETIME LastWritten;
    public int CredentialBlobSize;
    public IntPtr CredentialBlob;
    public WinCredPersistence Persist;
    public int AttributeCount;
    public IntPtr Attributes;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string TargetAlias;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string UserName;
}