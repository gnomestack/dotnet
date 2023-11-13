using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

public class MssqlSelectLoginExists : SqlStatementBuilder
{
    public string LoginName { get; set; } = string.Empty;

    public override Result<(string, object?), Exception> Build()
    {
        if (!Validate.Identifier(this.LoginName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid login name {this.LoginName}");

        var query = $"""
                     IF (EXISTS (SELECT name  FROM master.sys.sql_logins WITH (NOLOCK) WHERE name = '{this.LoginName}'))
                         SELECT 1
                     ELSE
                         SELECT 0;
                     """;
        return (query, null);
    }
}