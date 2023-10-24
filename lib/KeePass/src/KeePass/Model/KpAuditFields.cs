using System.Diagnostics.CodeAnalysis;

namespace GnomeStack.KeePass.Model;

[SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
public sealed class KpAuditFields : IEquatable<KpAuditFields>
{
    public KpAuditFields()
    {
        // TODO: figure out if KeePass uses UTC time.
        var now = DateTime.UtcNow;
        this.CreationTime = now;
        this.LastModificationTime = now;
        this.LastAccessTime = now;
        this.ExpiryTime = now;
        this.LocationChanged = now;
        this.Expires = false;
        this.UsageCount = 0;
    }

    public DateTime CreationTime { get; set; }

    public DateTime LastModificationTime { get; set; }

    public DateTime LastAccessTime { get; set; }

    public DateTime ExpiryTime { get; set; }

    public bool Expires { get; set; }

    public int UsageCount { get; set; }

    public DateTime LocationChanged { get; set; }

    public static bool operator ==(KpAuditFields left, KpAuditFields right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(KpAuditFields left, KpAuditFields right)
    {
        return !(left == right);
    }

    public bool Equals(KpAuditFields? other)
    {
        return other is not null
            && this.CreationTime == other.CreationTime
            && this.LastModificationTime == other.LastModificationTime
            && this.LastAccessTime == other.LastAccessTime
            && this.ExpiryTime == other.ExpiryTime
            && this.LocationChanged == other.LocationChanged
            && this.Expires == other.Expires
            && this.UsageCount == other.UsageCount;
    }

    public override bool Equals(object? obj)
    {
        return obj is KpAuditFields other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            this.CreationTime,
            this.LastModificationTime,
            this.LastAccessTime,
            this.ExpiryTime,
            this.LocationChanged,
            this.Expires,
            this.UsageCount);
    }
}