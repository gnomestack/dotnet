namespace GnomeStack.Security.Cryptography;

public interface IEncryptionStreamProvider
{
    void Encrypt(Stream input, Stream output);

    Stream Encrypt(Stream input);

    void Decrypt(Stream input, Stream output);

    Stream Decrypt(Stream input);
}