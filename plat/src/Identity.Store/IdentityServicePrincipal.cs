namespace GnomeStack.Identity.Store;

public class IdentityServicePrincipal
{
    public int Id { get; set; }

    public Guid Uid { get; set; }

    public Guid TenantId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? LastLoginAt { get; set; }

    public HashSet<IdentityServicePrincipalKey> Keys { get; set; } = new();

    public HashSet<IdentityRole> Roles { get; set; } = new();

    public HashSet<IdentityGroup> Groups { get; set; } = new();
}