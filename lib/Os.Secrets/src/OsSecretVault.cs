using System.Runtime.InteropServices;
using System.Security;
using System.Text;

using GnomeStack.Os.Secrets.Darwin;
using GnomeStack.Os.Secrets.Linux;
using GnomeStack.Os.Secrets.Win32;

namespace GnomeStack.Os.Secrets;

public static class OsSecretVault
{
    private static Lazy<bool> s_isOsSupported = new(() =>
    {
        // on .net core, there are more platforms supported like freebsd, browser, wasm, wasm, mobile.
        // not all of them are supported or have been tested yet.
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
               || RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
               || RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    });

    private static Lazy<IOsSecretVault> s_vault = new(() =>
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return new WinOsSecretVault();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return new DarwinOsSecretVault();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return new LinuxOsSecretVault();

        throw new PlatformNotSupportedException("Only Windows, MacOs, and Linux are currently supported");
    });

    public static string? GetSecret(string service, string account)
        => s_vault.Value.GetSecret(service, account);

    public static byte[] GetSecretAsBytes(string service, string account)
        => s_vault.Value.GetSecretAsBytes(service, account);

    public static char[] GetSecretAsChars(string service, string account)
    {
        var bytes = s_vault.Value.GetSecretAsBytes(service, account);
        var chars = Encoding.UTF8.GetChars(bytes);
        Array.Clear(bytes, 0, bytes.Length);
        return chars;
    }

    public static unsafe SecureString GetSecretAsSecureString(string service, string account)
    {
        var bytes = s_vault.Value.GetSecretAsBytes(service, account);
        var utf8Chars = Encoding.UTF8.GetChars(bytes);
        try
        {
            fixed (char* chars = utf8Chars)
            {
                var ss = new SecureString(chars, utf8Chars.Length);
                return ss;
            }
        }
        finally
        {
            Array.Clear(utf8Chars, 0, utf8Chars.Length);
        }
    }

    public static void SetSecret(string service, string account, string secret)
        => s_vault.Value.SetSecret(service, account, secret);

    public static void SetSecret(string service, string account, byte[] secret)
        => s_vault.Value.SetSecret(service, account, secret);

    public static void DeleteSecret(string service, string account)
        => s_vault.Value.DeleteSecret(service, account);
}