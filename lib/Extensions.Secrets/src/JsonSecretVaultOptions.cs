using GnomeStack.Security.Cryptography;

namespace GnomeStack.Extensions.Secrets;

public class JsonSecretVaultOptions : SecretVaultOptions
{
    public override Type SecretVaultType => typeof(JsonSecretVault);

    public string Path { get; set; } = string.Empty;

    public byte[] Key { get; set; } = Array.Empty<byte>();

    public IEncryptionProvider? EncryptionProvider { get; set; }
}