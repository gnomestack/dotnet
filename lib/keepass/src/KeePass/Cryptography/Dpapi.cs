using System.Diagnostics;
using System.Security.Cryptography;

using GnomeStack.Security.Cryptography;
using GnomeStack.Standard;

namespace GnomeStack.KeePass.Cryptography;

public static class Dpapi
{
    private static bool? s_isSupported;

    public static bool IsSupported
    {
        get
        {
            if (s_isSupported.HasValue)
                return s_isSupported.Value;

            if (!Env.IsWindows)
            {
                s_isSupported = false;
                return false;
            }

            using var rng = new Csrng();
            var data = new byte[200];
            rng.GetBytes(data);
            var entropy = new byte[31];
            rng.GetBytes(entropy);

            // it works if it not equal
            try
            {
                var protectedData = ProtectedData.Protect(data, entropy, DataProtectionScope.CurrentUser);
                s_isSupported = !protectedData.SequenceEqual(data);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                s_isSupported = false;
            }

            return s_isSupported.Value;
        }
    }

    public static ReadOnlySpan<byte> Protect(ReadOnlySpan<byte> userData, ReadOnlySpan<byte> optionalEntropy, bool isLocalMachine = false)
    {
        if (userData.IsEmpty)
            return Array.Empty<byte>();

        if (!IsSupported)
            return userData;

        var data = userData.ToArray();
        var entropy = optionalEntropy.ToArray();
        try
        {
            return ProtectedData.Protect(data, entropy, isLocalMachine ? DataProtectionScope.LocalMachine : DataProtectionScope.CurrentUser);
        }
        finally
        {
            Array.Clear(data, 0, data.Length);
            Array.Clear(entropy, 0, entropy.Length);
        }
    }

    public static ReadOnlySpan<byte> Unprotect(ReadOnlySpan<byte> userData, ReadOnlySpan<byte> optionalEntropy, bool isLocalMachine = false)
    {
        if (userData.IsEmpty)
            return Array.Empty<byte>();

        if (!IsSupported)
            return userData;

        var data = userData.ToArray();
        var entropy = optionalEntropy.ToArray();
        try
        {
            return ProtectedData.Unprotect(data, entropy, isLocalMachine ? DataProtectionScope.LocalMachine : DataProtectionScope.CurrentUser);
        }
        finally
        {
            Array.Clear(data, 0, data.Length);
            Array.Clear(entropy, 0, entropy.Length);
        }
    }
}