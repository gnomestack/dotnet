namespace GnomeStack.Identity.Store;

public class IdentityPermission
{
    public int Id { get; set; }

    public Guid Uid { get; set; }

    public int ServiceId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}