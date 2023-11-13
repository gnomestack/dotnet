using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GnomeStack.EntityFrameworkCore.UniversalResources;

public class TagValue
{
    public long Id { get; set; }

    public Guid Uid { get; set; } = Guid.NewGuid();

    public long TagNameId { get; set; }

    public TagName TagName { get; set; } = null!;

    [NotMapped]
    public string Name => this.TagName.Name;

    [StringLength(512)]
    public string Value { get; set; } = string.Empty;
}