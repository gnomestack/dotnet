namespace GnomeStack.EntityFrameworkCore.Deployments;

public class ConfigFileHistory
{
    public int ConfigFileId { get; set; }
    
    public Guid ConfigFileUid { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Value { get; set; } = null!;
    
    public bool Encrypted { get; set; }
    
    public string ContentType { get; set; } = null!;
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
}