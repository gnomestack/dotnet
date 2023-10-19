namespace GnomeStack.Identity.Store;

public class IdentityService
{
    public int Id { get; set; }

    public Guid Uid { get; set; }

    public string Name { get; set; } = string.Empty;
}