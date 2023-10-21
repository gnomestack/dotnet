using System.Collections;

using GnomeStack.KeePass.Cryptography;

namespace GnomeStack.KeePass.Collections;

public sealed class KpSortedBinaryMap : IDictionary<string, ShroudedBytes>, IEquatable<KpSortedBinaryMap>
{
    private readonly SortedDictionary<string, ShroudedBytes> map = new(StringComparer.OrdinalIgnoreCase);

    public int Count => this.map.Count;

    public bool IsReadOnly => false;

    public ICollection<string> Keys => this.map.Keys;

    public ICollection<ShroudedBytes> Values => this.map.Values;

    public ShroudedBytes this[string key]
    {
        get => this.map[key];
        set => this.map[key] = value;
    }

    public static bool operator ==(KpSortedBinaryMap? left, KpSortedBinaryMap? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(KpSortedBinaryMap? left, KpSortedBinaryMap? right)
    {
        return !Equals(left, right);
    }

    public void Clear()
        => this.map.Clear();

    public void Add(string key, ShroudedBytes value)
        => this.map.Add(key, value);

    public bool ContainsKey(string key)
        => this.map.ContainsKey(key);

    public KpSortedBinaryMap Clone()
    {
        var ret = new KpSortedBinaryMap();
        foreach (var kvp in this.map)
            ret.Add(kvp.Key, kvp.Value);
        return ret;
    }

    public IEnumerator<KeyValuePair<string, ShroudedBytes>> GetEnumerator()
        => this.map.GetEnumerator();

    public bool Remove(string key)
        => this.map.Remove(key);

    public bool TryGetValue(string key, out ShroudedBytes value)
        => this.map.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();

    void ICollection<KeyValuePair<string, ShroudedBytes>>.Add(KeyValuePair<string, ShroudedBytes> item)
        => ((ICollection<KeyValuePair<string, ShroudedBytes>>)this.map).Add(item);

    void ICollection<KeyValuePair<string, ShroudedBytes>>.CopyTo(
        KeyValuePair<string, ShroudedBytes>[] array,
        int arrayIndex)
        => ((ICollection<KeyValuePair<string, ShroudedBytes>>)this.map).CopyTo(array, arrayIndex);

    bool ICollection<KeyValuePair<string, ShroudedBytes>>.Contains(KeyValuePair<string, ShroudedBytes> item)
        => ((ICollection<KeyValuePair<string, ShroudedBytes>>)this.map).Contains(item);

    bool ICollection<KeyValuePair<string, ShroudedBytes>>.Remove(KeyValuePair<string, ShroudedBytes> item)
        => ((ICollection<KeyValuePair<string, ShroudedBytes>>)this.map).Remove(item);

    public bool Equals(KpSortedBinaryMap? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (this.Count != other.Count)
            return false;

        foreach (var kvp in this.map)
        {
            if (!other.map.TryGetValue(kvp.Key, out var otherValue) || !kvp.Value.Equals(otherValue))
                return false;
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is KpSortedBinaryMap map && this.Equals(map);
    }

    public override int GetHashCode()
    {
        return this.map.GetHashCode();
    }
}