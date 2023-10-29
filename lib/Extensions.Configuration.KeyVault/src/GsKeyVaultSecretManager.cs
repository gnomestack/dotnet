using System.Text;

using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;

using GnomeStack.Text;

using Microsoft.Extensions.Configuration;

namespace GnomeStack.Extensions.Configuration.KeyVault;

public class GsKeyVaultSecretManager : KeyVaultSecretManager
{
    private readonly string? prefix;

    private readonly string delimiter;

    public GsKeyVaultSecretManager(string? prefix = null, string delimiter = "-")
    {
        if (prefix.IsNullOrWhiteSpace())
            prefix = null;

        if (prefix is not null && prefix.EndsWith("-"))
            prefix = $"{prefix}-";

        if (delimiter.IsNullOrWhiteSpace())
            delimiter = "-";

        this.prefix = prefix;
        this.delimiter = delimiter;
    }

    public override bool Load(SecretProperties secret)
    {
        if (this.prefix is null)
            return true;

        return secret.Name.StartsWith(this.prefix, StringComparison.OrdinalIgnoreCase);
    }

    public override string GetKey(KeyVaultSecret secret)
    {
        var sb = StringBuilderCache.Acquire();
        var span = secret.Name.AsSpan();
        var delimiter = this.delimiter.AsSpan();
        if (this.prefix is not null)
        {
            span = span.Slice(this.prefix.Length);
        }

        var dl = this.delimiter.Length;
        var index = span.IndexOf(delimiter);
        while (index != -1)
        {
            sb.Append(span.Slice(0, index));
            sb.Append(':');
            span = span.Slice(index + dl);
            index = span.IndexOf(delimiter);
        }

        sb.Append(span);
        return StringBuilderCache.GetStringAndRelease(sb);
    }
}