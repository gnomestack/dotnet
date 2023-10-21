using System.Runtime.InteropServices;
using System.Security.Cryptography;

using GnomeStack.Security.Cryptography;

namespace GnomeStack.KeePass.Cryptography;

/// <summary>
/// A delegate for encrypting or decrypting binary data, so that encryption methods can
/// be swapped out.
/// </summary>
/// <param name="binary">The binary data that will be encrypted or decrypted.</param>
/// <param name="state">state that helps with the encryption / decryption process.</param>
/// <param name="action">The type of action for the delegate to perform.</param>
/// <returns>Binary data that is encrypted or decrypted. </returns>
public delegate byte[] ProtectDataAction(ReadOnlySpan<byte> binary, object state, ProtectDataOperation action);

/// <summary>
/// The type of action for the delegate <see cref="ProtectDataAction"/>.
/// </summary>
public enum ProtectDataOperation
{
    Encrypt,
    Decrypt,
}

#pragma warning disable SA1649
internal static class DpSettings
{
    private static Lazy<ProtectDataAction> s_action = new(() =>
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return (bytes, state, action) =>
            {
                if (action == ProtectDataOperation.Encrypt)
                {
                    var copy = bytes.ToArray();
                    try
                    {
                        return ProtectedData.Protect(bytes.ToArray(), null, DataProtectionScope.CurrentUser);
                    }
                    finally
                    {
                        Array.Clear(copy, 0, copy.Length);
                    }
                }
                else
                {
                    var copy = bytes.ToArray();
                    try
                    {
                        return ProtectedData.Unprotect(bytes.ToArray(), null, DataProtectionScope.CurrentUser);
                    }
                    finally
                    {
                        Array.Clear(copy, 0, copy.Length);
                    }
                }
            };
        }

        var salsa20 = Salsa20.Create();
        salsa20.GenerateKey();
        var key = salsa20.Key;

        // This is defaulted to the Salsa20 Stream Cipher
        // because the ProtectedMemory Api is windows specific.
        return (data, state, operation) =>
        {
            var protectedData = (ShroudedBytes)state;
            var copy = data.ToArray();
            try
            {
                var transform = operation == ProtectDataOperation.Encrypt
                    ? salsa20.CreateEncryptor(key, protectedData.Id.ToByteArray())
                    : salsa20.CreateDecryptor(key, protectedData.Id.ToByteArray());

                return transform.TransformFinalBlock(copy, 0, data.Length);
            }
            finally
            {
                Array.Clear(copy, 0, copy.Length);
            }
        };
    });

    public static ProtectDataAction ProtectDataAction
    {
        get => s_action.Value;
        set
        {
            if (s_action.IsValueCreated)
                throw new NotSupportedException("Cannot change the DataProtectionAction after it has been used.");

            s_action = new Lazy<ProtectDataAction>(() => value);
        }
    }
}