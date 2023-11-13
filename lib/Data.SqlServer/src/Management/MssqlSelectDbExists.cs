using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

public class MssqlSelectDbExists : SqlStatementBuilder
{
    public MssqlSelectDbExists()
    {
    }

    public MssqlSelectDbExists(string databaseName)
    {
        this.DatabaseName = databaseName;
    }

    public string DatabaseName { get; set; } = string.Empty;

    public override Result<(string, object?), Exception> Build()
    {
        if (!MssqlValidate.Identifier(this.DatabaseName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid database name {this.DatabaseName}");

        var query = $"""
                     ---noinspection SqlNoDataSourceInspectionForFile
                     SELECT IIF(COUNT(*) > 0, 1, 0)
                        FROM sys.databases
                        WHERE name = '{this.DatabaseName}'
                     """;

        return (query, null);
    }
}