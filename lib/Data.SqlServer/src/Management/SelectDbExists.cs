using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

public class SelectDbExists : SqlStatementBuilder
{
    public string DatabaseName { get; set; } = string.Empty;

    public static implicit operator string(SelectDbExists cmd)
        => cmd.Build().Unwrap();

    public override Result<string, Exception> Build()
    {
        if (!Validate.Identifier(this.DatabaseName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid database name {this.DatabaseName}");

        var query = $"""
                     ---noinspection SqlNoDataSourceInspectionForFile
                     SELECT IIF(COUNT(*) > 0, 1, 0)
                        FROM sys.databases
                        WHERE name = '{this.DatabaseName}'
                     """;

        return query;
    }
}