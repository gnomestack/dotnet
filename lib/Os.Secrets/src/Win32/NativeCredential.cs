using System.Runtime.InteropServices;

namespace GnomeStack.Os.Secrets.Win32;

[CLSCompliant(false)]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct NativeCredential
{
    public uint Flags;
    public uint Type;
    public IntPtr TargetName;
    public IntPtr Comment;
    public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
    public uint CredentialBlobSize;
    public IntPtr CredentialBlob;
    public uint Persist;
    public uint AttributeCount;
    public IntPtr? Attributes;
    public IntPtr TargetAlias;
    public IntPtr UserName;
}