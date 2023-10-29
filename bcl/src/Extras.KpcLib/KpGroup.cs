using System.Collections;

using KeePassLib;

namespace GnomeStack.Extras.KpcLib;

public class KpGroup : IEnumerable<KpGroup>, IEnumerable<KpEntry>, IEquatable<KpGroup>
{
    private readonly PwGroup group;

    private readonly Lazy<List<KpGroup>> groupSet;

    private readonly Lazy<List<KpEntry>> entitySet;

    public KpGroup()
        : this(true, true)
    {
    }

    public KpGroup(bool generateUuid, bool setTimes)
        : this(new PwGroup(generateUuid, setTimes))
    {
    }

    [CLSCompliant(false)]
    public KpGroup(string name, PwIcon icon = PwIcon.Folder, bool generateUuid = true, bool setTimes = true)
        : this(new PwGroup(generateUuid, setTimes, name, icon))
    {
    }

    public KpGroup(string name, KpGroup parent)
    {
        this.group = new PwGroup(true, true, name, PwIcon.Folder);
        this.Parent = parent;
        this.groupSet = new(() =>
        {
            var set = new List<KpGroup>();
            foreach (var n in this.group.Groups)
            {
                set.Add(new KpGroup(n));
            }

            return set;
        });

        this.entitySet = new(() =>
        {
            var set = new List<KpEntry>();
            foreach (var n in this.group.Entries)
            {
                set.Add(new KpEntry(n));
            }

            return set;
        });

        if (!parent.Groups.Contains(this))
        {
            parent.Add(this);
        }
    }

    internal KpGroup(PwGroup group, KpGroup? parent = null)
    {
        this.group = group;
        this.Parent = parent;
        this.groupSet = new(() =>
        {
            var set = new List<KpGroup>();
            foreach (var n in this.group.Groups)
            {
                set.Add(new KpGroup(n));
            }

            return set;
        });

        this.entitySet = new(() =>
        {
            var set = new List<KpEntry>();
            foreach (var n in this.group.Entries)
            {
                set.Add(new KpEntry(n));
            }

            return set;
        });
    }

    public string Description
    {
        get => this.group.Description;
    }

    public string Name
    {
        get => this.group.Name;
        set => this.group.Name = value;
    }

    public string Notes
    {
        get => this.group.Notes;
        set => this.group.Notes = value;
    }

    public KpGroup? Parent { get; internal set; }

    public IReadOnlyList<KpEntry> Entries
        => this.entitySet.Value;

    public IReadOnlyList<KpGroup> Groups
        => this.groupSet.Value;

    [CLSCompliant(false)]
    public static implicit operator PwGroup(KpGroup group)
        => group.group;

    public void Add(KpGroup group)
        => this.Add(group, true);

    public void Add(KpGroup group, bool takeOwnership)
    {
        if (!this.group.Groups.Contains(group))
        {
            this.group.AddGroup(group, takeOwnership);
        }

        if (takeOwnership)
            group.Parent = this;

        this.groupSet.Value.Add(group);
    }

    public void Add(KpEntry entry)
        => this.Add(entry, true);

    public void Add(KpEntry entry, bool takeOwnership)
    {
        this.group.AddEntry(entry, takeOwnership);
        if (takeOwnership)
            entry.Parent = this;

        this.entitySet.Value.Add(entry);
    }

    public void Delete()
        => this.Parent?.Remove(this);

    public void Remove(KpGroup group)
    {
        this.group.Groups.Remove(group);
        this.groupSet.Value.Remove(group);
    }

    public void Remove(KpEntry entry)
    {
        this.group.Entries.Remove(entry);
        this.entitySet.Value.Remove(entry);
    }

    IEnumerator<KpGroup> IEnumerable<KpGroup>.GetEnumerator()
        => this.Groups.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this.Groups.GetEnumerator();

    IEnumerator<KpEntry> IEnumerable<KpEntry>.GetEnumerator()
        => this.Entries.GetEnumerator();

    public bool Equals(KpGroup other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return this.group.Equals(other.group);
    }
}