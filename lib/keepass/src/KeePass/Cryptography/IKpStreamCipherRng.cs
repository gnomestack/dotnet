namespace GnomeStack.KeePass.Cryptography;

/// <summary>
/// KeePass uses streaming ciphers to generate random bytes. This contract
/// is for implementing a random byte generator engine.
/// </summary>
public interface IKpStreamCipherRng
{
    int Id { get; }

    ReadOnlySpan<byte> NextBytes(int count);
}

#pragma warning disable SA1201 // Elements should appear in the correct order
public enum KpStreamCipherRng
{
    None = 0,

    Arc4 = 1,

    Salsa20 = 2,

    ChaCha20 = 3,
}