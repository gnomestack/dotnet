using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

public class MssqlSelectConcurrentRequests : SqlStatementBuilder
{
    public string DatabaseName { get; set; } = string.Empty;

    public override Result<(string, object?), Exception> Build()
    {
        if (this.DatabaseName.IsNullOrWhiteSpace())
        {
            var sql = """
                   ---noinspection SqlNoDataSourceInspectionForFile
                   SELECT COUNT(*) AS [concurrent_requests]
                   FROM sys.dm_exec_requests AS R;
                   """;

            return (sql, null);
        }

        if (!Validate.Identifier(this.DatabaseName.AsSpan()))
        {
            return new InvalidDbIdentifierException($"Invalid database name {this.DatabaseName}");
        }

        var sql2 = $"""
                ---noinspection SqlNoDataSourceInspectionForFile
                SELECT COUNT(*) AS [concurrent_requests]
                FROM sys.dm_exec_requests AS R
                    INNER JOIN sys.databases AS D
                        ON D.database_id = R.database_id
                AND D.name = '{this.DatabaseName}'
                """;
        return (sql2, null);
    }
}