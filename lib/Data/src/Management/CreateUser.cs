using GnomeStack.Secrets;

namespace GnomeStack.Data.Management;

public abstract class CreateUser : SqlStatementBuilder
{
    public string UserName { get; set; } = string.Empty;

    public string? Password { get; set; }

    public bool GeneratePassword { get; set; } = false;

    protected virtual string GenerateSecurePassword()
    {
        var sg = new SecretGenerator();
        sg.AddDefaults();
        return sg.GenerateAsString(16);
    }
}