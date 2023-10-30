using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

public class DropMssqlRoleMember : SqlStatementBuilder
{
    public string RoleName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public static implicit operator string(DropMssqlRoleMember cmd)
        => cmd.Build().Unwrap();

    public override Result<string, Exception> Build()
    {
        if (!Validate.Identifier(this.UserName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid user name {this.UserName}");

        if (!Validate.Identifier(this.RoleName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid role name {this.RoleName}");

        return $"ALTER ROLE {Quote.Identifier(this.RoleName.AsSpan())} DROP MEMBER {Quote.Identifier(this.UserName.AsSpan())};";
    }
}