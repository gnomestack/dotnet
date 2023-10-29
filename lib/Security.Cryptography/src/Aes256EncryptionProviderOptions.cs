namespace GnomeStack.Security.Cryptography;

public class Aes256EncryptionProviderOptions
{
    public int Iterations { get; set; } = 60000;

    public int SaltSize { get; set; } = 64;

    public KeyedHashAlgorithmType KeyedHashedAlgorithm { get; set; }
        = KeyedHashAlgorithmType.HMACSHA256;

    public byte[] Key { get; set; } = Array.Empty<byte>();
}