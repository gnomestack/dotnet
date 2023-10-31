namespace GnomeStack.Data.Management;

public abstract class CreateDatabase : SqlStatementBuilder
{
    public string Name { get; set; } = string.Empty;

    public string? Collation { get; set; }
}