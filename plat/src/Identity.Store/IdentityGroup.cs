namespace GnomeStack.Identity.Store;

public class IdentityGroup
{
    public long Id { get; set; }

    public Guid Uid { get; set; }

    public Guid TenantId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public HashSet<IdentityRole> Roles { get; set; } = new();

    public HashSet<IdentityUser> Users { get; set; } = new();

    public HashSet<IdentityServicePrincipal> ServicePrincipals { get; set; } = new();
}