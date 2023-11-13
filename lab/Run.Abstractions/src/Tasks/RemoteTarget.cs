namespace GnomeStack.Run.Tasks;

public class RemoteTarget
{
    public string Host { get; set; } = string.Empty;

    public int Port { get; set; } = 22;

    public string Kind { get; set; } = "ssh-native";

    public string User { get; set; } = string.Empty;

    public string? Password { get; set; }

    public string? KeyFile { get; set; }

    public string? Key { get; set; }
}