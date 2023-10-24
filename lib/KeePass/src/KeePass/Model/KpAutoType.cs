namespace GnomeStack.KeePass.Model;

public class KpAutoType
{
    public bool Enabled { get; set; }

    public int DataTransferObfuscation { get; set; }

    public KpAssociation Association { get; set; } = new();
}