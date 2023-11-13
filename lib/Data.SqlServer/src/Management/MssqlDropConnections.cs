using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

/// <summary>
/// Drops all connections to a database by setting it to single user mode.
/// </summary>
public class MssqlDropConnections : SqlStatementBuilder
{
    public string DatabaseName { get; set; } = string.Empty;

    public override Result<(string, object?), Exception> Build()
    {
        if (!Validate.Identifier(this.DatabaseName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid database name {this.DatabaseName}");

        var sql = $"ALTER DATABASE {Quote.Identifier(this.DatabaseName.AsSpan())} SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
        return (sql, null);
    }
}