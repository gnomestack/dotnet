using System.Collections.Concurrent;

namespace GnomeStack.Extensions.Secrets;

public class MemorySecretVault : SecretVault
{
    private readonly ConcurrentDictionary<string, MemorySecretRecord> secrets = new();

    public override bool SupportsSynchronous => true;

    public override string Name => this.Options.VaultName;

    public override string Kind => "memory-vault";

    public override IEnumerable<string> ListNames()
    {
        return this.secrets.Keys.ToArray();
    }

    public override ISecretRecord CreateRecord(string name)
    {
        return new MemorySecretRecord(this.FormatName(name));
    }

    public override string? GetSecretValue(string name)
    {
        name = this.FormatName(name);
        if (this.secrets.TryGetValue(name, out var secret)
            && secret is not null)
        {
            return secret.Value;
        }

        return null;
    }

    public override ISecretRecord? GetSecret(string name)
    {
        name = this.FormatName(name);
        if (this.secrets.TryGetValue(name, out var secret)
            && secret is not null)
        {
            return secret;
        }

        return null;
    }

    public override void SetSecretValue(string name, string secret)
    {
        name = this.FormatName(name);
        if (this.secrets.TryGetValue(name, out var record)
            && record is not null)
        {
            record.Value = secret;
            record.WithUpdatedAt(DateTime.UtcNow);
        }
        else
        {
            record = new MemorySecretRecord(name)
            {
                Value = secret,
            }.WithCreatedAt(DateTime.UtcNow);
            this.secrets.TryAdd(name, record);
        }
    }

    public override void SetSecret(ISecretRecord secret)
    {
        var name = this.FormatName(secret.Name);
        if (this.secrets.TryGetValue(name, out var existing)
            && existing is not null)
        {
            existing.Value = secret.Value;
            existing.ExpiresAt = secret.ExpiresAt;
            existing.WithUpdatedAt(DateTime.UtcNow);
            existing.UpdateTags(secret.Tags);
        }
        else
        {
            existing = new MemorySecretRecord(secret)
            {
                Value = secret.Value,
                ExpiresAt = secret.ExpiresAt,
            }.WithCreatedAt(DateTime.UtcNow);
            this.secrets.TryAdd(name, existing);
        }
    }

    public override void DeleteSecret(string name)
    {
        name = this.FormatName(name);
        this.secrets.TryRemove(name, out _);
    }

    internal class MemorySecretRecord : SecretRecord
    {
        public MemorySecretRecord(string name)
            : base(name)
        {
        }

        public MemorySecretRecord(ISecretRecord record)
            : base(record)
        {
        }

        internal MemorySecretRecord WithUpdatedAt(DateTime? updatedAt)
        {
            this.UpdatedAt = updatedAt;
            return this;
        }

        internal MemorySecretRecord WithCreatedAt(DateTime? createdAt)
        {
            this.CreatedAt = createdAt;
            return this;
        }

        internal void UpdateTags(IDictionary<string, string?> tags)
        {
            this.Tags = new Dictionary<string, string?>(tags, StringComparer.OrdinalIgnoreCase);
        }
    }
}