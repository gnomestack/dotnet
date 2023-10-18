// ReSharper disable VirtualMemberCallInConstructor
namespace GnomeStack.Extensions.Secrets;

public abstract class SecretRecord : ISecretRecord, ICloneable
{
    protected SecretRecord(string name)
    {
        this.Name = name;
        this.Tags = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
    }

    protected SecretRecord(ISecretRecord record)
    {
        this.Name = record.Name;
        this.Value = record.Value;
        this.ExpiresAt = record.ExpiresAt;
        this.CreatedAt = record.CreatedAt;
        this.UpdatedAt = record.UpdatedAt;
        this.Tags = new Dictionary<string, string?>(record.Tags, StringComparer.OrdinalIgnoreCase);
    }

    public virtual string Name { get; }

    public virtual string Value { get; set; } = string.Empty;

    public DateTime? ExpiresAt { get; set; }

    public virtual DateTime? CreatedAt { get; internal protected set; }

    public virtual DateTime? UpdatedAt { get; internal protected set; }

    public virtual IDictionary<string, string?> Tags { get; protected set; }

    public virtual object Clone()
    {
        var clone = (SecretRecord)this.MemberwiseClone();
        clone.Tags = new Dictionary<string, string?>(this.Tags, StringComparer.OrdinalIgnoreCase);
        return clone;
    }
}