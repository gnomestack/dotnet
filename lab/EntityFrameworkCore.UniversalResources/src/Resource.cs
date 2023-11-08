using System.ComponentModel.DataAnnotations;

namespace GnomeStack.EntityFrameworkCore.UniversalResources;

public class Resource
{
    public long Id { get; set; }

    public Guid Uid { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(1024)]
    public string Identifier { get; set; } = string.Empty;

    [StringLength(2048)]
    public string? Url { get; set; }

    [StringLength(128)]
    public string? TableName { get; set; }

    public long? TableRowId { get; set; }
}