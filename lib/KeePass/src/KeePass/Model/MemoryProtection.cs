namespace GnomeStack.KeePass.Model;

public class MemoryProtection
{
    public bool ProtectedTitle { get; set; }

    public bool ProtectUserName { get; set; }

    public bool ProtectPassword { get; set; } = true;

    public bool ProtectUrl { get; set; }

    public bool ProtectNotes { get; set; }
}