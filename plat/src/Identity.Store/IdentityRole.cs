namespace GnomeStack.Identity.Store;

public class IdentityRole
{
    public int Id { get; set; }

    public Guid Uid { get; set; }

    public Guid TenantId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public HashSet<IdentityPermission> Permissions { get; set; } = new();

    public HashSet<IdentityServicePrincipal> ServicePrincipals { get; set; } = new();

    public HashSet<IdentityUser> Users { get; set; } = new();

    public HashSet<IdentityGroup> Groups { get; set; } = new();

    public HashSet<IdentityApiKey> ApiKeys { get; set; } = new();
}