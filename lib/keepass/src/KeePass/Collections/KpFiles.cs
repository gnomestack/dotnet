using System.Collections;
using System.Collections.ObjectModel;

using GnomeStack.KeePass.Cryptography;

namespace GnomeStack.KeePass.Collections;

public class KpFiles : IEnumerable<KeyValuePair<string, ShroudedBytes>>
{
    private readonly SortedDictionary<string, ShroudedBytes> map = new(StringComparer.OrdinalIgnoreCase);

    public KpFiles()
    {
    }

    public int Count => this.map.Count;

    public ICollection<string> Keys => this.map.Keys;

    public ShroudedBytes this[string key]
    {
        get => this.map[key];
        set
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (this.map.ContainsKey(key))
            {
                this.map[key] = value;
                return;
            }

            this.map.Add(key, value);
        }
    }

    public void Add(string key, ShroudedBytes value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (this.map.ContainsKey(key))
        {
            return;
        }

        this.map.Add(key, value);
    }

    public bool ContainsKey(string key)
        => this.map.ContainsKey(key);

    public void Set(string key, ShroudedBytes value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        this.map[key] = value;
    }

    public ShroudedBytes GetOrCreate(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (!this.map.ContainsKey(key))
        {
            var value = new ShroudedBytes();
            this.map.Add(key, value);
            return value;
        }

        return this.map[key];
    }

    public ReadOnlySpan<byte> ReadBytes(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        if (!this.map.ContainsKey(key))
            return ReadOnlySpan<byte>.Empty;

        return this.map[key].Read();
    }

    public bool Remove(string key)
        => this.map.Remove(key);

    public bool TryGetValue(string key, out ShroudedBytes value)
    {
        return this.map.TryGetValue(key, out value);
    }

    public IEnumerator<KeyValuePair<string, ShroudedBytes>> GetEnumerator()
        => this.map.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();
}