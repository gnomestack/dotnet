using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

public class DropMssqlUser : SqlStatementBuilder
{
    public string UserName { get; set; } = string.Empty;

    public static implicit operator string(DropMssqlUser cmd)
        => cmd.Build().Unwrap();

    public override Result<string, Exception> Build()
    {
        if (!Validate.UserName(this.UserName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid user name {this.UserName}");

        return $"DROP USER IF EXISTS {Quote.Identifier(this.UserName.AsSpan())};";
    }
}