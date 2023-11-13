using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

public class MssqlDropUser : SqlStatementBuilder
{
    public string UserName { get; set; } = string.Empty;

    public override Result<(string, object?), Exception> Build()
    {
        if (!Validate.UserName(this.UserName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid user name {this.UserName}");

        var sql = $"DROP USER IF EXISTS {Quote.Identifier(this.UserName.AsSpan())};";
        return (sql, null);
    }
}