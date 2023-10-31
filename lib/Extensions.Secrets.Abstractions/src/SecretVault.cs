using System.Text;

namespace GnomeStack.Extensions.Secrets;

public abstract class SecretVault : ISecretVault
{
    public abstract bool SupportsSynchronous { get; }

    public virtual string Name => this.Options.VaultName;

    public abstract string Kind { get; }

    protected SecretVaultOptions Options { get; set; } = new();

    public virtual Task<IEnumerable<string>> ListNamesAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(this.ListNames());

    public virtual IEnumerable<string> ListNames()
    {
        throw new NotImplementedException();
    }

    public virtual ISecretRecord CreateRecord(string name)
    {
        throw new NotImplementedException();
    }

    public virtual Task<string?> GetSecretValueAsync(string name, CancellationToken cancellationToken = default)
        => Task.FromResult(this.GetSecretValue(name));

    public virtual string? GetSecretValue(string name)
    {
        throw new NotImplementedException();
    }

    public virtual Task<ISecretRecord?> GetSecretAsync(string name, CancellationToken cancellationToken = default)
        => Task.FromResult(this.GetSecret(name));

    public virtual ISecretRecord? GetSecret(string name)
    {
        throw new NotImplementedException();
    }

    public virtual Task SetSecretValueAsync(string name, string secret, CancellationToken cancellationToken = default)
    {
        this.SetSecretValue(name, secret);
        return Task.CompletedTask;
    }

    public virtual void SetSecretValue(string name, string secret)
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

    public virtual Task DeleteSecretAsync(string name, CancellationToken cancellationToken = default)
    {
        this.DeleteSecret(name);
        return Task.CompletedTask;
    }

    public virtual void DeleteSecret(string name)
    {
        throw new NotImplementedException();
    }

    protected virtual string FormatName(string name)
    {
        var sb = new StringBuilder();
        if (this.Options.Prefix.Length > 0)
            sb.Append(this.Options.Prefix);

        foreach (var c in name)
        {
            if (char.IsLetterOrDigit(c) || c == '-')
            {
                sb.Append(c);
                continue;
            }

            if (char.IsWhiteSpace(c) || c is '_' or '.' or '/' or '\\' or ':')
            {
                sb.Append('-');
                continue;
            }

            throw new InvalidOperationException($"Cannot use character '{c}' in secret name.");
        }

        var result = sb.ToString();
        sb.Clear();
        return result;
    }
}