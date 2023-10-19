namespace GnomeStack.Identity.Store;

public class IdentityUser
{
    public int Id { get; set; }

    public Guid Uid { get; set; }

    public Guid TenantId { get; set; }

    public string Upn { get; set; } = string.Empty;

    public string Pseudonym { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? LastLoginAt { get; set; }

    public string LastLoginIp { get; set; } = string.Empty;

    public HashSet<IdentityApiKey> ApiKeys { get; set; } = new();

    public HashSet<IdentityRole> Roles { get; set; } = new();

    public HashSet<IdentityGroup> Groups { get; set; } = new();
}