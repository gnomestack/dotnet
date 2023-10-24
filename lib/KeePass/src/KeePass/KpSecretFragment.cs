using System.Runtime.InteropServices;
using System.Security;
using System.Text;

using GnomeStack.Extra.Arrays;
using GnomeStack.KeePass.Cryptography;

namespace GnomeStack.KeePass;

public class KpSecretFragment : KpKeyFragment
{
    public KpSecretFragment(SecureString secureString)
    {
        if (secureString == null)
            throw new ArgumentNullException(nameof(secureString));

        IntPtr bstr = IntPtr.Zero;
        char[] charArray = new char[secureString.Length];

        try
        {
            bstr = Marshal.SecureStringToBSTR(secureString);
            Marshal.Copy(bstr, charArray, 0, charArray.Length);

            var bytes = Encoding.UTF8.GetBytes(charArray);
            this.SetData(bytes.ToSha256());

            bytes.Clear();
            charArray.Clear();
        }
        finally
        {
            Marshal.ZeroFreeBSTR(bstr);
        }
    }

    public KpSecretFragment(byte[] password)
    {
        if (password == null)
            throw new ArgumentNullException(nameof(password));

        if (password.Length == 0)
            throw new ArgumentException("password must be greater than 0 characters", nameof(password));

        var bytes = password.ToSha256();
        this.SetData(bytes);
    }

    public KpSecretFragment(ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length == 0)
            throw new ArgumentException("password must be greater than 0 characters", nameof(bytes));

        this.SetData(bytes.ToSha256());
    }

    public KpSecretFragment(string password)
    {
        if (password == null)
            throw new ArgumentNullException(nameof(password));

        if (password.Length == 0)
            throw new ArgumentException("password must be greater than 0 characters");

        var bytes = Encoding.UTF8.GetBytes(password).ToSha256();
        this.SetData(bytes);
    }

    public KpSecretFragment(ReadOnlySpan<char> password)
    {
        if (password.Length == 0)
            throw new ArgumentException("password must be greater than 0 characters");

        this.SetData(Utils.Utf8NoBom.GetBytesAsSpan(password).ToSha256());
    }
}