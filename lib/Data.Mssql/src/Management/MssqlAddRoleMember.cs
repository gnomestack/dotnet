using GnomeStack.Functional;

namespace GnomeStack.Data.Mssql.Management;

public class MssqlAddRoleMember : SqlStatementBuilder
{
    public MssqlAddRoleMember()
    {
    }

    public MssqlAddRoleMember(string roleName, string userName)
    {
        this.RoleName = roleName;
        this.UserName = userName;
    }

    public string RoleName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public bool UseStoredProc { get; set; } = false;

    public override Result<(string, object?), Exception> Build()
    {
        if (!MssqlValidate.Identifier(this.UserName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid user name {this.UserName}");

        if (!MssqlValidate.Identifier(this.RoleName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid role name {this.RoleName}");

        if (this.UseStoredProc)
        {
            var sql = $"EXEC sp_addrolemember {MssqlQuote.Identifier(this.RoleName.AsSpan())}, {MssqlQuote.Identifier(this.UserName.AsSpan())};";
            return (sql, null);
        }

        var sql2 = $"ALTER ROLE {MssqlQuote.Identifier(this.RoleName.AsSpan())} ADD MEMBER {MssqlQuote.Identifier(this.UserName.AsSpan())};";

        return (sql2, null);
    }
}