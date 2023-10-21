using System.Collections;
using System.Collections.ObjectModel;

using GnomeStack.KeePass.Cryptography;

namespace GnomeStack.KeePass.Collections;

public class KpFields : IEnumerable<KeyValuePair<string, ShroudedChars>>
{
    private readonly SortedDictionary<string, ShroudedChars> map = new(StringComparer.OrdinalIgnoreCase);

    public KpFields()
    {
    }

    public int Count => this.map.Count;

    public ICollection<string> Keys => this.map.Keys;

    public ShroudedChars Title
    {
        get => this.GetOrCreate("Title");
        set => this.Set("Title", value);
    }

    public ShroudedChars UserName
    {
        get => this.GetOrCreate("UserName");
        set => this.Set("UserName", value);
    }

    public ShroudedChars Password
    {
        get => this.GetOrCreate("Password");
        set => this.Set("Password", value);
    }

    public ShroudedChars Url
    {
        get => this.GetOrCreate("URL");
        set => this.Set("URL", value);
    }

    public ShroudedChars Notes
    {
        get => this.GetOrCreate("Notes");
        set => this.Set("Notes", value);
    }

    public ShroudedChars this[string key]
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

    public void Add(string key, ShroudedChars value)
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

    public void Set(string key, ShroudedChars value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        this.map[key] = value;
    }

    public ShroudedChars GetOrCreate(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (!this.map.ContainsKey(key))
        {
            var value = new ShroudedChars();
            this.map.Add(key, value);
            return value;
        }

        return this.map[key];
    }

    public ReadOnlySpan<char> Read(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        if (!this.map.ContainsKey(key))
            return ReadOnlySpan<char>.Empty;

        return this.map[key].Read();
    }

    public ReadOnlySpan<byte> ReadBytes(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        if (!this.map.ContainsKey(key))
            return ReadOnlySpan<byte>.Empty;

        return this.map[key].ReadBytes();
    }

    public string ReadString(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        if (!this.map.ContainsKey(key))
            return string.Empty;

        return this.map[key].ReadString();
    }

    public bool Remove(string key)
        => this.map.Remove(key);

    public bool TryGetValue(string key, out ShroudedChars value)
    {
        return this.map.TryGetValue(key, out value);
    }

    public IEnumerator<KeyValuePair<string, ShroudedChars>> GetEnumerator()
        => this.map.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();
}