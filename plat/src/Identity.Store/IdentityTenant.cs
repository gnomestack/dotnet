namespace GnomeStack.Identity.Store;

public class IdentityTenant
{
    public int Id { get; set; }

    public Guid TenantId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public HashSet<IdentityTenantDomain> Domains { get; set; } = new();
}