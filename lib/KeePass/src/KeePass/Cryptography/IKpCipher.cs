namespace GnomeStack.KeePass.Cryptography;

public interface IKpCipher
{
    Kpid Id { get;  }

    Stream CreateCryptoStream(
        Stream stream,
        bool encrypt,
        ReadOnlySpan<byte> key,
        ReadOnlySpan<byte> iv);
}