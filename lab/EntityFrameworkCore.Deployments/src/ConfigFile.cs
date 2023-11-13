using System.ComponentModel.DataAnnotations;

namespace GnomeStack.EntityFrameworkCore.Deployments;

public class ConfigFile
{
    public int Id { get; set; }
    
    public Guid Uid { get; set; } = Guid.NewGuid();
    
    [Required]
    [StringLength(256)]
    public string Name { get; set; } = null!;
    
    public string Value { get; set; } = null!;
    
    public bool IsEncrypted { get; set; }
    
    public bool IsDeleted { get; set; }
    
    [StringLength(128)]
    public string ContentType { get; set; } = null!;
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
}