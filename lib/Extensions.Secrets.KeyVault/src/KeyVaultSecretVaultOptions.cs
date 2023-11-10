using Azure.Core;
using Azure.Security.KeyVault.Secrets;

namespace GnomeStack.Extensions.Secrets.KeyVault;

public class KeyVaultSecretVaultOptions : SecretVaultOptions
{
    [CLSCompliant(false)]
    public TokenCredential? TokenCredential { get; set; }

    public Uri? VaultUri { get; set; }

    [CLSCompliant(false)]
    public SecretClient? SecretClient { get; set; }

    public override char[] Delimiter { get; set; } = new[] { '-' };
}