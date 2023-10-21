using GnomeStack.KeePass.Cryptography;

namespace GnomeStack.KeePass.Model;

public class StringMapping
{
    public int Id { get; set; }

    public string Key { get; set; } = string.Empty;

    public ShroudedChars Value { get; set; }
}