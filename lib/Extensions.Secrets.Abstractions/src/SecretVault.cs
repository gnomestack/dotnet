using System.Text;

namespace GnomeStack.Extensions.Secrets;

public abstract class SecretVault : ISecretVault
{
    public abstract bool SupportsSynchronous { get; }

    public virtual string Name => this.Options.VaultName;

    public abstract string Kind { get; }

    protected SecretVaultOptions Options { get; set; } = new();

    public static string FormatPath(string name, SecretVaultOptions options)
    {
        var sb = new StringBuilder();
        if (options.Prefix.Length > 0)
            sb.Append(options.Prefix);

        for (var i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (char.IsLetterOrDigit(c))
            {
                sb.Append(c);
                continue;
            }

            if (c == options.Delimiter[0])
            {
                if (options.Delimiter.Length == 1)
                {
                    sb.Append(options.Delimiter[0]);
                    continue;
                }

                var j = 1;

                var match = true;
                for (; j < options.Delimiter.Length; j++)
                {
                    if (i + j >= name.Length)
                    {
                        match = false;
                        break;
                    }

                    if (name[i + j] != options.Delimiter[j])
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    sb.Append(options.Delimiter);
                    i += j - 1;
                    continue;
                }
            }

            if (c is '-' or '.' or '/' or '\\' or ':')
            {
                sb.Append(options.Delimiter);
                continue;
            }

            throw new InvalidOperationException($"Cannot use character '{c}' in secret name.");
        }

        var result = sb.ToString();
        sb.Clear();
        return result;
    }

    public virtual Task<IEnumerable<string>> ListNamesAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(this.ListNames());

    public virtual IEnumerable<string> ListNames()
    {
        throw new NotImplementedException();
    }

    public virtual ISecretRecord CreateRecord(string path)
    {
        throw new NotImplementedException();
    }

    public virtual Task<string?> GetSecretValueAsync(string path, CancellationToken cancellationToken = default)
        => Task.FromResult(this.GetSecretValue(path));

    public virtual string? GetSecretValue(string path)
    {
        throw new NotImplementedException();
    }

    public virtual Task<ISecretRecord?> GetSecretAsync(string path, CancellationToken cancellationToken = default)
        => Task.FromResult(this.GetSecret(path));

    public virtual ISecretRecord? GetSecret(string path)
    {
        throw new NotImplementedException();
    }

    public virtual Task SetSecretValueAsync(string path, string secret, CancellationToken cancellationToken = default)
    {
        this.SetSecretValue(path, secret);
        return Task.CompletedTask;
    }

    public virtual void SetSecretValue(string path, string secret)
    {
        throw new NotImplementedException();
    }

    public virtual Task SetSecretAsync(ISecretRecord secret, CancellationToken cancellationToken = default)
    {
        this.SetSecret(secret);
        return Task.CompletedTask;
    }

    public virtual void SetSecret(ISecretRecord secret)
    {
        throw new NotImplementedException();
    }

    public virtual Task DeleteSecretAsync(string path, CancellationToken cancellationToken = default)
    {
        this.DeleteSecret(path);
        return Task.CompletedTask;
    }

    public virtual void DeleteSecret(string path)
    {
        throw new NotImplementedException();
    }
}