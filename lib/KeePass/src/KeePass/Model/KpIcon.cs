namespace GnomeStack.KeePass.Model;

public class KpIcon
{
    public Kpid Uuid { get; set; }

    public string Name { get; set; } = string.Empty;

    public byte[] Data { get; set; } = Array.Empty<byte>();
}