using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

public class DropMssqlDatabase : SqlStatementBuilder
{
    public string DatabaseName { get; set; } = string.Empty;

    public static implicit operator string(DropMssqlDatabase cmd)
        => cmd.Build().Unwrap();

    public override Result<string, Exception> Build()
    {
        if (!Validate.Identifier(this.DatabaseName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid database name {this.DatabaseName}");

        return $"DROP DATABASE IF EXISTS {Quote.Identifier(this.DatabaseName.AsSpan())};";
    }
}