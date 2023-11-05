using GnomeStack.Data.Management;
using GnomeStack.Functional;
using GnomeStack.Text;

using StringExtensions = GnomeStack.Extras.Strings.StringExtensions;

namespace GnomeStack.Data.SqlServer.Management;

public class CreateMssqlUser : CreateUser
{
    public string? LoginName { get; set; }

    public bool UseFrom { get; set; }

    public bool CheckExists { get; set; } = true;

    public bool UseFromExternalProvider { get; set; }

    public string? Schema { get; set; }

    public static implicit operator string(CreateMssqlUser cmd)
        => cmd.Build().Unwrap();

    public override Result<string, Exception> Build()
    {
        var sb = StringBuilderCache.Acquire();

        if (this.UserName.IsNullOrWhiteSpace())
            return new InvalidDbIdentifierException("User name cannot be empty");

        if (!Validate.Identifier(this.UserName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid user name {this.UserName}");

        if (!this.LoginName.IsNullOrWhiteSpace() && !Validate.Identifier(this.LoginName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid login name {this.LoginName}");

        if (this.CheckExists)
        {
            sb.Append("IF DATABASE_PRINCIPAL_ID('")
                .Append(this.UserName)
                .Append("') IS NULL")
                .AppendLine()
                .Append("BEGIN")
                .AppendLine();
        }

        sb.Append("    CREATE USER ")
            .Append(Quote.Identifier(this.UserName.AsSpan()));

        if (this.UseFromExternalProvider)
        {
            sb.Append("FROM EXTERNAL PROVIDER");
            return StringBuilderCache.GetStringAndRelease(sb);
        }

        if (this.Password.IsNullOrWhiteSpace() && this.GeneratePassword)
        {
            this.Password = this.GenerateSecurePassword();
        }

        if (!this.Password.IsNullOrWhiteSpace())
        {
            sb.AppendLine()
                .Append("    WITH PASSWORD = '")
                .Append(this.Password)
                .Append('\'');

            if (this.CheckExists)
            {
                sb.AppendLine()
                    .Append("END");
            }

            return StringBuilderCache.GetStringAndRelease(sb);
        }

        if (!this.LoginName.IsNullOrWhiteSpace())
        {
            sb.AppendLine();
            if (this.UseFrom)
            {
                sb.Append("    FROM LOGIN ")
                    .Append(Quote.Identifier(this.LoginName.AsSpan()));
            }
            else
            {
                sb.Append("    FOR LOGIN ")
                    .Append(Quote.Identifier(this.LoginName.AsSpan()));
            }
        }

        if (!this.Schema.IsNullOrWhiteSpace())
        {
            sb.AppendLine()
              .Append("    WITH DEFAULT_SCHEMA = ")
              .Append(Quote.Identifier(this.Schema.AsSpan()));
        }

        if (this.CheckExists)
        {
            sb.AppendLine()
                .Append("END");
        }

        return StringBuilderCache.GetStringAndRelease(sb);
    }
}