namespace GnomeStack.Identity.Store;

public class IdentityResourceId
{
    public IdentityResourceId(string tenantId, string kind, string id)
    {
        this.TenantId = tenantId;
        this.Kind = kind;
        this.Id = id;
    }

    public IdentityResourceId(Guid tenantId, string kind, Guid id)
    {
        this.TenantId = tenantId.ToString();
        this.Kind = kind;
        this.Id = id.ToString();
    }

    public string TenantId { get; set; } = string.Empty;

    public string Kind { get; set; } = string.Empty;

    public string Id { get; set; } = string.Empty;

    public static IdentityResourceId Parse(string value)
    {
        var parts = value.Split('/');
        if (parts.Length != 6)
        {
            throw new ArgumentException("Invalid IdentityResourceId format", nameof(value));
        }

        return new IdentityResourceId(parts[2], parts[4], parts[6]);
    }

    public static bool TryParse(string value, out IdentityResourceId? resourceId)
    {
        resourceId = null;
        try
        {
            var parts = value.Split('/');
            if (parts.Length != 6)
            {
                return false;
            }

            // TODO: try parse Guids
            resourceId = new IdentityResourceId(parts[2], parts[4], parts[6]);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public override string ToString()
    {
        return $"/tenant/{TenantId}/kind/{Kind}/id/{Id}";
    }
}