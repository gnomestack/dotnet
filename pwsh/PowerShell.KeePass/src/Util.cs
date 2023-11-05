using System.Runtime.InteropServices;
using System.Security;

namespace GnomeStack.PowerShell.KeePass;

internal static class Util
{
    public static unsafe string ConvertSecureString(SecureString ss)
    {
        var bstr = IntPtr.Zero;
        var chars = new char[ss.Length];
        try
        {
            bstr = Marshal.SecureStringToBSTR(ss);
            Marshal.Copy(bstr, chars, 0, chars.Length);

            var pw = new string(chars);
            Array.Clear(chars, 0, chars.Length);
            return pw;
        }
        finally
        {
            Marshal.ZeroFreeBSTR(bstr);
        }
    }
}