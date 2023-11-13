using GnomeStack.Data.SqlServer.Management;
using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

public class MssqlSelectLoginProcessIds : SqlStatementBuilder
{
    public MssqlSelectLoginProcessIds()
    {
    }

    public MssqlSelectLoginProcessIds(string loginName)
    {
        this.LoginName = loginName;
    }

    public string LoginName { get; set; } = string.Empty;

    public override Result<(string, object?), Exception> Build()
    {
        if (!MssqlValidate.UserName(this.LoginName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid user name {this.LoginName}");

        var sql = $"""
                  SELECT session_id
                  FROM sys.dm_exec_sessions
                  WHERE login_name = '{this.LoginName}'
                  """;
        return (sql, null);
    }
}