using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

public class MssqlDropRoleMember : SqlStatementBuilder
{
    public string RoleName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public override Result<(string, object?), Exception> Build()
    {
        if (!Validate.Identifier(this.UserName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid user name {this.UserName}");

        if (!Validate.Identifier(this.RoleName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid role name {this.RoleName}");

        var sql = $"ALTER ROLE {Quote.Identifier(this.RoleName.AsSpan())} DROP MEMBER {Quote.Identifier(this.UserName.AsSpan())};";
        return (sql, null);
    }
}