namespace GnomeStack.Identity.Store;

public class IdentityApiKey
{
    public long Id { get; set; }

    public Guid Uid { get; set; }

    public Guid TenantId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? RevokedAt { get; set; }

    public DateTimeOffset? ExpiresAt { get; set; }

    public DateTimeOffset? LastLoginAt { get; set; }

    public HashSet<IdentityRole> Roles { get; set; } = new();
}