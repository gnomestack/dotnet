using GnomeStack.Extras.KpcLib;

namespace GnomeStack.Extensions.Secrets.KeePass;

public class KeePassSecretVaultOptions : SecretVaultOptions
{
    public string? KdbxFile { get; set; }

    public string? Password { get; set; }

    public string? KeyFilePath { get; set; }

    [CLSCompliant(false)]
    public KpDatabase? Database { get; set; }
}