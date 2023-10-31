using Azure.Core;

namespace GnomeStack.Extensions.Azure.Identity;

public class AzureIdentityOptions
{
    public TokenCredential? TokenCredential { get; set; }

    public Func<IServiceProvider, TokenCredential>? TokenCredentialFactory { get; set; }
}