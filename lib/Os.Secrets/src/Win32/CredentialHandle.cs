using System.Runtime.InteropServices;
using System.Text;

namespace GnomeStack.Os.Secrets.Win32;

public class CredentialHandle : Microsoft.Win32.SafeHandles.CriticalHandleMinusOneIsInvalid
{
    public CredentialHandle(IntPtr handle)
    {
        this.SetHandle(handle);
    }

    public WinCredSecret AllocateCredential()
    {
        if (this.IsInvalid)
            throw new InvalidOperationException($"{typeof(CriticalHandle).FullName} handle is invalid");

        return this.AllocateCredentialFromHandle(this.handle);
    }

    public WinCredSecret[] AllocateCredentials(int count)
    {
        if (this.IsInvalid)
            throw new InvalidOperationException("Invalid CriticalHandle!");

        var credentials = new WinCredSecret[count];
        for (int i = 0; i < count; i++)
        {
            IntPtr nextPointer = Marshal.ReadIntPtr(this.handle, i * IntPtr.Size);
            var credential = this.AllocateCredentialFromHandle(nextPointer);
            var index = credential.Service.IndexOf(":target=", StringComparison.InvariantCulture);
            if (index > -1)
            {
                var key = credential.Service;
                key = key.Substring(index + 1);
                credentials[i] = new WinCredSecret(
                    credential.Type,
                    key,
                    credential.Account,
                    credential.Password,
                    credential.Comment);
                continue;
            }

            credentials[i] = credential;
        }

        return credentials;
    }

    public byte[] GetSecretAsBytes()
    {
        if (this.IsInvalid)
            throw new InvalidOperationException("Invalid CriticalHandle!");

        var native = Marshal.PtrToStructure<NativeCredential>(this.handle);
        var data = new byte[native.CredentialBlobSize];
        Marshal.Copy(native.CredentialBlob, data, 0, (int)native.CredentialBlobSize);
        return data;
    }

    public string GetSecret()
    {
        if (this.IsInvalid)
            throw new InvalidOperationException("Invalid CriticalHandle!");

        var native = Marshal.PtrToStructure<NativeCredential>(this.handle);
        if (native.CredentialBlob == IntPtr.Zero || native.CredentialBlob.ToInt32() == -1)
            return string.Empty;

        if (native.CredentialBlobSize == 0)
            return string.Empty;

        var data = new byte[native.CredentialBlobSize];
        Marshal.Copy(native.CredentialBlob, data, 0, (int)native.CredentialBlobSize);
        return Encoding.Unicode.GetString(data);
    }

    protected override bool ReleaseHandle()
    {
        if (this.IsInvalid)
            return false;

        NativeMethods.FreeCredential(this.handle);
        this.SetHandleAsInvalid();
        return true;
    }

    private WinCredSecret AllocateCredentialFromHandle(IntPtr handle)
    {
        var native = Marshal.PtrToStructure<NativeCredential>(handle);

        // var fileTime = (((long)native.LastWritten.dwHighDateTime) << 32) + native.LastWritten.dwLowDateTime;
        byte[] data = Array.Empty<byte>();

        if (native.CredentialBlobSize > 0)
        {
            data = new byte[native.CredentialBlobSize];
            Marshal.Copy(native.CredentialBlob, data, 0, (int)native.CredentialBlobSize);
        }

        var service = Marshal.PtrToStringUni(native.TargetName) ?? string.Empty;
        var account = Marshal.PtrToStringUni(native.UserName);

        return new WinCredSecret(
            (WinCredType)native.Type,
            service,
            account,
            data != null ? Encoding.Unicode.GetString(data) : null,
            Marshal.PtrToStringUni(native.Comment));
    }
}