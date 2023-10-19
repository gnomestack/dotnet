using System.ComponentModel.DataAnnotations.Schema;

namespace GnomeStack.Identity.Store;

public class IdentityResource
{
    public long Id { get; set; }

    public Guid Uid { get; set; }

    public Guid TenantId { get; set; }

    public string Kind { get; set; } = string.Empty;

    [NotMapped]
    public IdentityResourceId ResourceId => new(this.TenantId, this.Kind, this.Uid);
}