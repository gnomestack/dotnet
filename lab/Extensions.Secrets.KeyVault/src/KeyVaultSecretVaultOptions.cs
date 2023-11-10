namespace GnomeStack.Extensions.Secrets.KeyVault;

public class KeyVaultSecretVaultOptions : SecretVaultOptions
{
    public byte[] Password { get; set; } = Array.Empty<byte>();

    public string KeyFilePath { get; set; } = string.Empty;

    public char[] Delimiter { get; set; } = new[] { '/' };
}