using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

/// <summary>
/// Drops all connections to a database by setting it to single user mode.
/// </summary>
public class DropMssqlConnections : SqlStatementBuilder
{
    public string DatabaseName { get; set; } = string.Empty;

    public static implicit operator string(DropMssqlConnections cmd)
        => cmd.Build().Unwrap();

    public override Result<string, Exception> Build()
    {
        if (!Validate.Identifier(this.DatabaseName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid database name {this.DatabaseName}");

        return $"ALTER DATABASE {Quote.Identifier(this.DatabaseName.AsSpan())} SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
    }
}