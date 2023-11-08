using System.ComponentModel.DataAnnotations;

namespace GnomeStack.EntityFrameworkCore.Deployments;

public class ConfigValue
{
    public int Id { get; set; }

    public Guid Uid { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(512)]
    public string Name { get; set; } = null!;

    [StringLength(2048)]
    public string Value { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
}