using GnomeStack.Data.Mssql.Management;
using GnomeStack.Functional;

namespace GnomeStack.Data.Mssql.Management;

public class MssqlSelectUserExists : SqlStatementBuilder
{
    public MssqlSelectUserExists()
    {
    }

    public MssqlSelectUserExists(string userName)
    {
        this.UserName = userName;
    }

    public string UserName { get; set; } = string.Empty;

    public override Result<(string, object?), Exception> Build()
    {
        if (!MssqlValidate.UserName(this.UserName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid user name {this.UserName}");

        var sql = $"""
                   IF EXISTS(SELECT principal_id FROM sys.database_principals WHERE name = '{this.UserName}')
                       SELECT 1;
                   ELSE
                       SELECT 0;
                   """;

        return (sql, null);
    }
}