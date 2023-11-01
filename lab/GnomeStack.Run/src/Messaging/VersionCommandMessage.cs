namespace GnomeStack.Run.Messaging;

public class VersionCommandMessage : CommandMessage
{
    public VersionCommandMessage(string version)
        : base("version")
    {
        this.Version = version;
    }

    public string Version { get; set; }
}