namespace GnomeStack.Run.Tasks;

public class Target
{
    public string Host { get; set; } = string.Empty;

    public int Port { get; set; } = 22;

    public string User { get; set; } = string.Empty;

    public string? Password { get; set; } = string.Empty;

    public string? Key { get; set; }
}