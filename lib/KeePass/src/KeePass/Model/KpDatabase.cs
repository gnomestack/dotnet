namespace GnomeStack.KeePass.Model;

public class KpDatabase
{
    public KpDatabase()
    {
    }

    public KpGroup RootGroup { get; set; } = new("root");
}