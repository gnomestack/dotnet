using GnomeStack.Functional;
using GnomeStack.Text;

namespace GnomeStack.Data.SqlServer.Management;

public class MssqlGrantStatement : SqlStatementBuilder
{
    public List<string> Permissions { get; set; } = new List<string>();

    public string ObjectName { get; set; } = string.Empty;

    public string ObjectType { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public bool GrantOption { get; set; } = false;

    public override Result<(string, object?), Exception> Build()
    {
        if (!MssqlValidate.UserName(this.UserName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid user name: {this.UserName}");

        if (!this.ObjectType.IsNullOrWhiteSpace() && !MssqlValidate.Identifier(this.ObjectName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid object type: {this.ObjectName}");

        if (!MssqlValidate.Identifier(this.ObjectName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid object name: {this.ObjectName}");

        foreach (var permission in this.Permissions)
        {
            if (!MssqlValidate.PermissionName(permission.AsSpan()))
            {
                return new InvalidDbIdentifierException($"Invalid permission: {permission}");
            }
        }

        var sb = StringBuilderCache.Acquire();
        sb.Append("GRANT ");
        sb.Append(string.Join(", ", this.Permissions.Select(o => o.ToUpperInvariant())));
        sb.Append(" ON ");
        if (!this.ObjectType.IsNullOrWhiteSpace())
        {
            sb.Append(this.ObjectType.ToUpperInvariant());
            sb.Append("::");
        }

        sb.Append(this.ObjectName);
        sb.Append(" TO ");
        sb.Append(this.UserName);
        sb.Append(" ");
        if (this.GrantOption)
        {
            sb.Append("WITH GRANT OPTION");
        }

        var sql = StringBuilderCache.GetStringAndRelease(sb);
        return (sql, null);
    }
}