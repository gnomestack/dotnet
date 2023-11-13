using GnomeStack.Functional;
using GnomeStack.Secrets;
using GnomeStack.Text;

namespace GnomeStack.Data.Mssql.Management;

public class MssqlCreateLogin : SqlStatementBuilder
{
    public MssqlCreateLogin()
    {
    }

    public MssqlCreateLogin(string userName)
    {
        this.UserName = userName;
        this.GeneratePassword = true;
    }

    public MssqlCreateLogin(string userName, string password)
    {
        this.UserName = userName;
        this.Password = password;
    }

    public bool CheckExists { get; set; } = true;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool GeneratePassword { get; set; } = false;

    public override Result<(string, object?), Exception> Build()
    {
        if (!MssqlValidate.UserName(this.UserName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid user name {this.UserName}");

        if (this.Password.IsNullOrWhiteSpace() && this.GeneratePassword)
        {
            var sg = new SecretGenerator()
                .AddDefaults();
            this.Password = sg.GenerateAsString(16);
        }

        var sb = StringBuilderCache.Acquire();

        if (this.CheckExists)
        {
            sb.Append("IF NOT EXISTS(SELECT * FROM sys.server_principals WHERE name = '")
                .Append(this.UserName)
                .Append("')")
                .AppendLine()
                .Append("BEGIN")
                .AppendLine();
        }

        sb.Append("    CREATE LOGIN ")
            .Append(MssqlQuote.Identifier(this.UserName.AsSpan()))
            .Append(" WITH PASSWORD = '")
            .Append(this.Password)
            .Append("';");

        if (this.CheckExists)
        {
            sb.AppendLine()
                .Append("END");
        }

        return (StringBuilderCache.GetStringAndRelease(sb), null);
    }
}