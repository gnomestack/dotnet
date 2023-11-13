using System.ComponentModel.DataAnnotations;

namespace GnomeStack.EntityFrameworkCore.UniversalResources;

public class TagName
{
    public long Id { get; set; }

    public Guid Uid { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(128)]
    public string Name { get; set; } = string.Empty;

    [StringLength(128)]
    public string Slug { get; set; } = string.Empty;
}