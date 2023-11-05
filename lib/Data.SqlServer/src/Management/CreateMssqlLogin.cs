using GnomeStack.Functional;
using GnomeStack.Secrets;
using GnomeStack.Text;

namespace GnomeStack.Data.SqlServer.Management;

public class CreateMssqlLogin : SqlStatementBuilder
{
    public bool CheckExists { get; set; } = true;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool GeneratePassword { get; set; } = false;

    public static implicit operator string(CreateMssqlLogin cmd)
        => cmd.Build().Unwrap();

    public override Result<string, Exception> Build()
    {
        if (!Validate.Identifier(this.UserName.AsSpan()))
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
            .Append(Quote.Identifier(this.UserName.AsSpan()))
            .Append(" WITH PASSWORD = '")
            .Append(this.Password)
            .Append("';");

        if (this.CheckExists)
        {
            sb.AppendLine()
                .Append("END");
        }

        return StringBuilderCache.GetStringAndRelease(sb);
    }
}