using GnomeStack.Functional;

namespace GnomeStack.Data.Mssql.Management;

public class MssqlDropUser : SqlStatementBuilder
{
    public MssqlDropUser()
    {
    }

    public MssqlDropUser(string userName)
    {
        this.UserName = userName;
    }

    public string UserName { get; set; } = string.Empty;

    public override Result<(string, object?), Exception> Build()
    {
        if (!MssqlValidate.UserName(this.UserName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid user name {this.UserName}");

        var sql = $"DROP USER IF EXISTS {MssqlQuote.Identifier(this.UserName.AsSpan())};";
        return (sql, null);
    }
}