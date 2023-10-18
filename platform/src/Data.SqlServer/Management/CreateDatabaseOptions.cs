namespace GnomeStack.Data.SqlServer.Management;

public class CreateDatabaseOptions
{
    public string? Name { get; set; }

    public string? Collation { get; set; }

    public string? DataFile { get; set; }

    public string? LogFile { get; set; }

    public string? ElasticPoolName { get; set; }

    public string? ElasticPoolEdition { get; set; }

    public string? ElasticPoolServiceObjective { get; set; }

    public string? CopyFromDatabase { get; set; }
}