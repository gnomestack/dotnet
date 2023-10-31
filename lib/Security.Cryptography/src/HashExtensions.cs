using System.Security.Cryptography;

namespace GnomeStack.Security.Cryptography;

public static class HashExtensions
{
    public static byte[] HashData(this byte[] data, HashAlgorithmName name)
    {
        using var algo = name.CreateHashAlgorithm();
        return algo.ComputeHash(data);
    }

    // ReSharper disable once InconsistentNaming
    public static byte[] HashSHA256(this byte[] data)
    {
        using var algo = SHA256.Create();
        return algo.ComputeHash(data);
    }
}