using System.Diagnostics;

using Azure.Core;
using Azure.Identity;

using Microsoft.Extensions.Configuration;

namespace GnomeStack.Azure.Identity;

public class GsConfigTokenCredential : ChainedTokenCredential
{
    public GsConfigTokenCredential(IConfiguration configuration)
        : base(Create(configuration))
    {
    }

    private static TokenCredential[] Create(IConfiguration configuration)
    {
        var list = new List<TokenCredential>()
        {
            new EnvironmentCredential(),
        };

        var section = configuration.GetSection("Azure:Identity");
        if (section.GetSection("mi").Value?.Equals("true", StringComparison.OrdinalIgnoreCase) == true)
        {
            if (!section.GetSection("clientId").Value.IsNullOrWhiteSpace())
            {
                list.Add(new ManagedIdentityCredential(section.GetSection("clientId").Value!));
            }
            else
            {
                list.Add(new ManagedIdentityCredential());
            }
        }
        else
        {
            var tenantId = section.GetSection("tenantId").Value;
            var clientId = section.GetSection("clientId").Value;
            var clientSecret = section.GetSection("clientSecret").Value;
            var clientCertificatePath = section.GetSection("clientCertificate").Value;

            bool configureServicePrincipal = true;
            if (tenantId.IsNullOrWhiteSpace())
            {
                configureServicePrincipal = false;
                Debug.WriteLine("TenantId is missing from Azure:Identity");
                Trace.WriteLine("TenantId is missing from Azure:Identity");
            }

            if (clientId.IsNullOrWhiteSpace())
            {
                configureServicePrincipal = false;
                Debug.WriteLine("ClientId is missing from Azure:Identity");
                Trace.WriteLine("ClientId is missing from Azure:Identity");
            }

            if (configureServicePrincipal && !clientSecret.IsNullOrWhiteSpace())
            {
                list.Add(new ClientSecretCredential(tenantId, clientId, clientSecret));
            }
            else if (configureServicePrincipal && !clientCertificatePath.IsNullOrWhiteSpace())
            {
                list.Add(new ClientCertificateCredential(tenantId, clientId, clientCertificatePath));
            }
        }

        list.Add(new AzureCliCredential());
        list.Add(new AzurePowerShellCredential());

        if (section.GetSection("vs").Value?.Equals("true", StringComparison.OrdinalIgnoreCase) == true)
        {
            list.Add(new VisualStudioCredential());
        }

        if (section.GetSection("vscode").Value?.Equals("true", StringComparison.OrdinalIgnoreCase) == true)
        {
            list.Add(new VisualStudioCodeCredential());
        }

        if (section.GetSection("interactive").Value?.Equals("true", StringComparison.OrdinalIgnoreCase) == true)
        {
            list.Add(new InteractiveBrowserCredential());
        }

        return list.ToArray();
    }
}