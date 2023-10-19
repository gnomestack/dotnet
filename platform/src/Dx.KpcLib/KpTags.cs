using System;
using System.Collections;
using System.Linq;

using KeePassLib;

namespace GnomeStack.Dx.KpcLib;

public class KpTags : IReadOnlyList<string>
{
    private readonly List<string> tags;

    public KpTags(List<string> tags)
    {
        this.tags = tags;
    }

    public int Count => this.tags.Count;

    public string this[int index]
    {
        get => this.tags[index];
        set => this.tags[index] = value;
    }

    public void Add(string tag)
    {
        if (!this.tags.Contains(tag, StringComparer.OrdinalIgnoreCase))
            this.tags.Add(tag);
    }

    public IEnumerator<string> GetEnumerator()
        => this.tags.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();

    public string? Find(string tag)
    {
        foreach (var next in this.tags)
        {
            if (next.Equals(tag, StringComparison.OrdinalIgnoreCase))
                return next;
        }

        return default;
    }

    public int IndexOf(string tag)
    {
        for (int i = 0; i < this.tags.Count; i++)
        {
            var n = this.tags[i];
            if (n.Equals(tag, StringComparison.OrdinalIgnoreCase))
                return i;
        }

        return -1;
    }

    public bool Has(string tag)
    {
        foreach (var next in this.tags)
        {
            if (next.Equals(tag, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    public bool Remove(string tag)
    {
        var index = this.IndexOf(tag);
        if (index > -1)
        {
            this.tags.RemoveAt(index);
            return true;
        }

        return false;
    }
}