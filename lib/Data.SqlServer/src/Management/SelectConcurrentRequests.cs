using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

public class SelectConcurrentRequests : SqlStatementBuilder
{
    public string DatabaseName { get; set; } = string.Empty;

    public override Result<string, Exception> Build()
    {
        if (this.DatabaseName.IsNullOrWhiteSpace())
        {
            return """
                   ---noinspection SqlNoDataSourceInspectionForFile
                   SELECT COUNT(*) AS [concurrent_requests]
                   FROM sys.dm_exec_requests AS R;
                   """;
        }

        if (!Validate.Identifier(this.DatabaseName.AsSpan()))
        {
            return new InvalidDbIdentifierException($"Invalid database name {this.DatabaseName}");
        }

        return $"""
                ---noinspection SqlNoDataSourceInspectionForFile
                SELECT COUNT(*) AS [concurrent_requests]
                FROM sys.dm_exec_requests AS R
                    INNER JOIN sys.databases AS D
                        ON D.database_id = R.database_id
                AND D.name = '{this.DatabaseName}'
                """;
    }
}