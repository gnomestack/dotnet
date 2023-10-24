using GnomeStack.KeePass.Cryptography;

namespace GnomeStack.KeePass.Model;

public class BinaryMapping
{
    public int Id { get; set; }

    public string Key { get; set; } = string.Empty;

    public ShroudedBytes Value { get; set; }
}