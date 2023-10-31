using GnomeStack.Functional;
using GnomeStack.Text;

namespace GnomeStack.Data.SqlServer.Management;

public class DropMssqlLogin : SqlStatementBuilder
{
    public string LoginName { get; set; } = string.Empty;

    public bool CheckExists { get; set; } = true;

    public static implicit operator string(DropMssqlLogin cmd)
        => cmd.Build().Unwrap();

    public override Result<string, Exception> Build()
    {
        if (!Validate.UserName(this.LoginName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid login name {this.LoginName}");

        var sb = StringBuilderCache.Acquire();
        if (this.CheckExists)
        {
            sb.Append("IF EXISTS  (SELECT name FROM master.sys.server_principals WHERE name ='");
            sb.Append(this.LoginName)
                .Append('\'')
                .AppendLine()
                .Append("BEGIN");
        }

        sb.Append("    DROP LOGIN ")
            .Append(Quote.Identifier(this.LoginName.AsSpan()));

        if (this.CheckExists)
        {
            sb.AppendLine()
                .Append("END");
        }

        return StringBuilderCache.GetStringAndRelease(sb);
    }
}