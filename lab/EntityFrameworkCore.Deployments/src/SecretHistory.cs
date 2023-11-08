namespace GnomeStack.EntityFrameworkCore.Deployments;

public class SecretHistory
{
    public int SecretId { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Value { get; set; } = null!;
    
    public DateTimeOffset? ExpiresAt { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
}