namespace GnomeStack.Extensions.Secrets.KeePass;

public interface IKeePassRecord : ISecretRecord
{
    string Path { get; set; }

    string? Url { get; set; }

    string? UserName { get; set; }

    string? Notes { get; set; }
}