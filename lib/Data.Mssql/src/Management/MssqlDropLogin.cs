using GnomeStack.Functional;
using GnomeStack.Text;

namespace GnomeStack.Data.Mssql.Management;

public class MssqlDropLogin : SqlStatementBuilder
{
    public MssqlDropLogin()
    {
    }

    public MssqlDropLogin(string loginName)
    {
        this.LoginName = loginName;
    }

    public string LoginName { get; set; } = string.Empty;

    public bool CheckExists { get; set; } = true;

    public override Result<(string, object?), Exception> Build()
    {
        if (!MssqlValidate.UserName(this.LoginName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid login name {this.LoginName}");

        var sb = StringBuilderCache.Acquire();
        if (this.CheckExists)
        {
            sb.Append("IF EXISTS  (SELECT name FROM master.sys.server_principals WHERE name ='");
            sb.Append(this.LoginName)
                .Append("\')")
                .AppendLine()
                .AppendLine("BEGIN");
        }

        sb.Append("    DROP LOGIN ")
            .Append(MssqlQuote.Identifier(this.LoginName.AsSpan()));

        if (this.CheckExists)
        {
            sb.AppendLine().Append("END");
        }

        return (StringBuilderCache.GetStringAndRelease(sb), null);
    }
}