namespace GnomeStack.Identity.Store;

public class IdentityServicePrincipalKey
{
    public long Id { get; set; }
 
    public Guid TenantId { get; set; }

    public Guid ServicePrincipalId { get; set; }

    public Guid KeyId { get; set; }

    public string Key { get; set; } = string.Empty;

    public DateTimeOffset ExpiresAt { get; set; }

    public IdentityServicePrincipal ServicePrincipal { get; set; } = null!;
}