namespace GnomeStack.Identity.Store;

public class IdentityTenantDomain
{
    public int Id { get; set; }

    public Guid TenantId { get; set; }

    public string Domain { get; set; } = string.Empty;

    public IdentityTenant Tenant { get; set; } = default!;
}