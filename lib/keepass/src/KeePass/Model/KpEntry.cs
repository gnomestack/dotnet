using GnomeStack.KeePass.Collections;
using GnomeStack.KeePass.Package;

namespace GnomeStack.KeePass.Model;

public class KpEntry
{
    public KpEntry()
    {
        this.Uuid = Kpid.NewKpid();
    }

    public Kpid Uuid { get; set; }

    public KpFields Fields { get; set; } = new();

    public KpFiles Files { get; set; } = new();

    public KpTags Tags { get; set; } = new();

    public string ForegroundColor { get; set; } = string.Empty;

    public string BackgroundColor { get; set; } = string.Empty;

    public string OverrideUrl { get; set; } = string.Empty;

    public bool PreventAutoCreate { get; set; }

    public int IconId { get; set; }

    public Kpid CustomIconUuid { get; set; }

    public KpAutoType AutoType { get; set; } = new();

    public KpAuditFields AuditFields { get; set; } = new();

    public bool IsHistorical { get; set; }

    public List<KpEntry> History { get; set; } = new();

    public KpDatabase? Owner { get; set; }

    public KpGroup? Parent { get; set; }

    public string Name
    {
        get => this.Fields.ReadString("Name");
        set => this.Fields.Set("Name", value);
    }

    public string Notes
    {
        get => this.Fields.ReadString("Notes");
        set => this.Fields.Set("Notes", value);
    }

    public string Url
    {
        get => this.Fields.ReadString("URL");
        set => this.Fields.Set("URL", value);
    }

    public string UserName
    {
        get => this.Fields.ReadString("UserName");
        set => this.Fields.Set("UserName", value);
    }

    public string Password
    {
        get => "************";
        set => this.Fields.Set("Password", value);
    }
}