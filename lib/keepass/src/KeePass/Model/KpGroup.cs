namespace GnomeStack.KeePass.Model;

public class KpGroup
{
    private readonly List<KpEntry> entries = new();

    private readonly List<KpGroup> groups = new();

    public KpGroup(string name)
    {
        this.Name = name;
        this.Uuid = Kpid.NewKpid();
    }

    public Kpid Uuid { get; set; }

    public string Name { get; set; }

    public string Notes { get; set; } = string.Empty;

    public int IconId { get; set; } = (int)KpIcons.FolderWithDocument;

    public byte[] CustomIconUuid { get; set; } = Array.Empty<byte>();

    public KpAuditFields AuditFields { get; set; } = new();

    public bool IsExpanded { get; set; }

    public string DefaultAutoTypeSequence { get; set; } = string.Empty;

    public bool? EnableAutoType { get; set; }

    public bool? EnableSearching { get; set; }

    public Kpid LastTopVisibleEntry { get; set; }

    public KpDatabase? Owner { get; internal protected set; }

    public IReadOnlyList<KpEntry> Entries => this.entries;

    public IReadOnlyList<KpGroup> Groups => this.groups;

    public KpGroup? Parent { get; internal protected set; }

    public void Add(KpEntry entry)
    {
        if (entry == null)
        {
            throw new ArgumentNullException(nameof(entry));
        }

        foreach (var n in this.entries)
        {
            if (n.Equals(entry))
                return;
        }

        if (entry.Parent != null)
            entry.Parent = this;

        entry.Owner = this.Owner;
        entry.Parent = this;
        this.entries.Add(entry);
    }

    public void Add(KpGroup group)
    {
        if (group == null)
        {
            throw new ArgumentNullException(nameof(group));
        }

        foreach (var n in this.groups)
        {
            if (n.Equals(group))
                return;
        }

        if (this == group)
            return;

        if (group.Parent != null)
            group.Parent = this;

        group.Owner = this.Owner;
        group.Parent = this;
        this.groups.Add(group);
    }

    public KpGroup CopyTo(KpGroup destinationGroup)
    {
        var sourceGroup = this;

        destinationGroup.Uuid = sourceGroup.Uuid;
        destinationGroup.AuditFields.CreationTime = sourceGroup.AuditFields.CreationTime;
        destinationGroup.AuditFields.Expires = sourceGroup.AuditFields.Expires;
        destinationGroup.AuditFields.ExpiryTime = sourceGroup.AuditFields.ExpiryTime;
        destinationGroup.AuditFields.LastAccessTime = sourceGroup.AuditFields.LastAccessTime;
        destinationGroup.AuditFields.LastModificationTime = sourceGroup.AuditFields.LastModificationTime;
        destinationGroup.AuditFields.LocationChanged = sourceGroup.AuditFields.LocationChanged;
        destinationGroup.AuditFields.UsageCount = sourceGroup.AuditFields.UsageCount;

        destinationGroup.CustomIconUuid = sourceGroup.CustomIconUuid;
        destinationGroup.DefaultAutoTypeSequence = sourceGroup.DefaultAutoTypeSequence;
        destinationGroup.EnableAutoType = sourceGroup.EnableAutoType;
        destinationGroup.EnableSearching = sourceGroup.EnableSearching;
        destinationGroup.IconId = sourceGroup.IconId;
        destinationGroup.IsExpanded = sourceGroup.IsExpanded;
        destinationGroup.LastTopVisibleEntry = sourceGroup.LastTopVisibleEntry;
        destinationGroup.Name = sourceGroup.Name;
        destinationGroup.Notes = sourceGroup.Notes;

        return destinationGroup;
    }
}