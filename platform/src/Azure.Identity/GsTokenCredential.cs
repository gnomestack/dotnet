using Azure.Core;
using Azure.Identity;

namespace GnomeStack.Azure.Identity;

public class GsTokenCredential : ChainedTokenCredential
{
    public GsTokenCredential()
        : base(Create())
    {
    }

    private static TokenCredential[] Create()
    {
        var list = new List<TokenCredential>()
        {
            new EnvironmentCredential(),
            new ManagedIdentityCredential(),
            new AzureCliCredential(),
            new AzurePowerShellCredential(),
        };

        return list.ToArray();
    }
}