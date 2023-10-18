using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace GnomeStack.Text.DotEnv.Document;

public class EnvDocument : IEnumerable<EnvDocumentEntry>,
    IDictionary<string, string>,
    IReadOnlyDictionary<string, string>
{
    private readonly List<EnvDocumentEntry> entries = new List<EnvDocumentEntry>();

    private readonly Dictionary<string, EnvNameValuePair> nameValuePairs = new(StringComparer.OrdinalIgnoreCase);

    private int version;

    public int Count => this.entries.Count;

    bool ICollection<KeyValuePair<string, string>>.IsReadOnly => false;

    ICollection<string> IDictionary<string, string>.Keys => new OrderedKeyCollection(this);

    IEnumerable<string> IReadOnlyDictionary<string, string>.Keys => new OrderedKeyCollection(this);

    IEnumerable<string> IReadOnlyDictionary<string, string>.Values => new OrderedValueCollection(this);

    ICollection<string> IDictionary<string, string>.Values => new OrderedValueCollection(this);

    public EnvDocumentEntry this[int index]
    {
        get => this.entries[index];
        set => this.entries[index] = value;
    }

    string IReadOnlyDictionary<string, string>.this[string name]
    {
        get
        {
            if (this.nameValuePairs.TryGetValue(name, out var entry) && entry is not null)
            {
                return entry.GetRawValueAsString();
            }

            return string.Empty;
        }
    }

    string IDictionary<string, string>.this[string name]
    {
        get
        {
            if (this.nameValuePairs.TryGetValue(name, out var entry) && entry is not null)
            {
                return entry.GetRawValueAsString();
            }

            return string.Empty;
        }

        set
        {
            if (!this.nameValuePairs.TryGetValue(name, out var entry) || entry is null)
            {
                entry = new EnvNameValuePair(name.AsSpan(), value.AsSpan());
                this.Add(entry);
                return;
            }

            entry.RawValue = value.ToCharArray();
        }
    }

    public string? this[string name]
    {
        get
        {
            if (this.nameValuePairs.TryGetValue(name, out var entry) && entry is not null)
            {
                return entry.GetRawValueAsString();
            }

            return default;
        }

        set
        {
            if (!this.nameValuePairs.TryGetValue(name, out var entry) || entry is null)
            {
                entry = new EnvNameValuePair(name.AsSpan(), value.AsSpan());
                this.Add(entry);
                return;
            }

            entry.RawValue = (value ?? string.Empty).ToCharArray();
        }
    }

    void ICollection<KeyValuePair<string, string>>.Clear()
        => this.Clear();

    bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> item)
    {
        var value = item.Value.ToCharArray();
        if (!this.nameValuePairs.TryGetValue(item.Key, out var entry) || entry is null)
              return false;

        return entry.RawValue.SequenceEqual(value);
    }

    void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item)
    {
        if (!this.nameValuePairs.TryGetValue(item.Key, out var entry) || entry is null)
            return false;

        var value = item.Value.ToCharArray();
        if (!entry.RawValue.SequenceEqual(value))
            return false;

        this.entries.Remove(entry);
        this.nameValuePairs.Remove(entry.Name);
        return true;
    }

    void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> item)
        => this.Add(item.Key, item.Value);

    public void Add(EnvDocumentEntry entry)
    {
        this.entries.Add(entry);
        if (entry is EnvNameValuePair nameValuePair)
            this.nameValuePairs[nameValuePair.Name] = nameValuePair;

        this.version++;
    }

    public void Add(string name, ReadOnlySpan<char> value)
    {
        var entry = new EnvNameValuePair(name.AsSpan(), value);
        this.entries.Add(entry);
        this.nameValuePairs.Add(entry.Name, entry);
        this.version++;
    }

    public void Add(string name, string value)
    {
        var entry = new EnvNameValuePair(name.AsSpan(), value.AsSpan());
        this.entries.Add(entry);
        this.nameValuePairs.Add(entry.Name, entry);
        this.version++;
    }

    public void Add(string name, char[] value)
    {
        this.version++;
        var entry = new EnvNameValuePair(name.AsSpan(), value);
        this.entries.Add(entry);
        this.nameValuePairs.Add(entry.Name, entry);
    }

    public void AddEmptyLine()
    {
        this.version++;
        this.entries.Add(new EnvEmptyLine());
    }

    public void AddComment(ReadOnlySpan<char> comment)
    {
        this.version++;
        this.entries.Add(new EnvComment(comment));
    }

    public void AddComment(string comment)
    {
        this.version++;
        this.entries.Add(new EnvComment(comment.AsSpan()));
    }

    public void Clear(bool clearRawData = false)
    {
        this.version++;
        if (clearRawData)
        {
            foreach (var entry in this.entries)
            {
                if (entry.RawValue.Length == 0)
                    continue;

                Array.Clear(entry.RawValue, 0, entry.RawValue.Length);
            }
        }

        this.entries.Clear();
        this.nameValuePairs.Clear();
    }

    public bool ContainsKey(string key)
    {
        return this.nameValuePairs.ContainsKey(key);
    }

    IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
    {
        foreach (var entry in this.entries)
        {
            if (entry is EnvNameValuePair nameValuePair)
                yield return new KeyValuePair<string, string>(nameValuePair.Name, nameValuePair.GetRawValueAsString());
        }
    }

    public IEnumerable<EnvNameValuePair> AsNameValuePairEnumerator()
    {
        foreach (var entry in this.entries)
        {
            if (entry is EnvNameValuePair nameValuePair)
                yield return nameValuePair;
        }
    }

    public IDictionary<string, string> AsDictionary()
    {
        return this;
    }

    /// <summary>
    /// Copies the <see cref="EnvNameValuePair"/> elements of this document to a dictionary.
    /// </summary>
    /// <returns>a dictionary.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        var dictionary = new Dictionary<string, string>();
        foreach (var entry in this.entries)
        {
            if (entry is EnvNameValuePair nameValuePair)
                dictionary.Add(nameValuePair.Name, nameValuePair.GetRawValueAsString());
        }

        return dictionary;
    }

    public ConcurrentDictionary<string, string> ToConcurrentDictionary()
    {
        var dictionary = new ConcurrentDictionary<string, string>();
        foreach (var entry in this.entries)
        {
            if (entry is EnvNameValuePair nameValuePair)
                dictionary.TryAdd(nameValuePair.Name, nameValuePair.GetRawValueAsString());
        }

        return dictionary;
    }

    public OrderedDictionary ToOrderedDictionary()
    {
        var dictionary = new OrderedDictionary();
        foreach (var entry in this.entries)
        {
            if (entry is EnvNameValuePair nameValuePair)
                dictionary.Add(nameValuePair.Name, nameValuePair.GetRawValueAsString());
        }

        return dictionary;
    }

    public IEnumerator<EnvDocumentEntry> GetEnumerator()
        => this.entries.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();

    bool IReadOnlyDictionary<string, string>.ContainsKey(string key)
        => this.nameValuePairs.ContainsKey(key);

    bool IDictionary<string, string>.ContainsKey(string key)
        => this.nameValuePairs.ContainsKey(key);

    public void Insert(int index, EnvDocumentEntry entry)
    {
        if (entry is EnvNameValuePair nameValuePair)
        {
            if (this.nameValuePairs.ContainsKey(nameValuePair.Name))
            {
                throw new InvalidOperationException(
                    "An EnvNameValuePair with the same key already exists. Remove the existing entry first.");
            }

            this.nameValuePairs.Add(nameValuePair.Name, nameValuePair);
        }

        this.entries.Insert(index, entry);
    }

    public bool Remove(string key)
    {
        if (this.nameValuePairs.TryGetValue(key, out var entry) && entry is not null)
        {
            this.entries.Remove(entry);
            this.nameValuePairs.Remove(key);
            this.version++;
            return true;
        }

        return false;
    }

    public bool Remove(EnvDocumentEntry entry)
    {
        var removed = this.entries.Remove(entry);
        if (removed)
        {
            this.version++;
            if (entry is EnvNameValuePair nameValuePair)
                this.nameValuePairs.Remove(nameValuePair.Name);
        }

        return removed;
    }

    bool IDictionary<string, string>.Remove(string key)
        => this.Remove(key);

    bool IReadOnlyDictionary<string, string>.TryGetValue(string key, out string value)
    {
        value = string.Empty;
        if (this.nameValuePairs.TryGetValue(key, out var entry) && entry is not null)
        {
            value = entry.GetRawValueAsString();
            return true;
        }

        return false;
    }

    bool IDictionary<string, string>.TryGetValue(string key, out string value)
    {
        value = string.Empty;
        if (this.nameValuePairs.TryGetValue(key, out var entry) && entry is not null)
        {
            value = entry.GetRawValueAsString();
            return true;
        }

        return false;
    }

    public bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
    {
        value = default;
        if (this.nameValuePairs.TryGetValue(key, out var entry) && entry is not null)
        {
            value = entry.GetRawValueAsString();
            return true;
        }

        return false;
    }

    public bool TryGetNameValuePair(string key, out EnvNameValuePair? value)
    {
        value = default;
        if (this.nameValuePairs.TryGetValue(key, out var entry) && entry is not null)
        {
            value = entry;
            return true;
        }

        return false;
    }

    public sealed class OrderedKeyCollection : ICollection<string>
    {
        private readonly EnvDocument document;

        public OrderedKeyCollection(EnvDocument document)
        {
            this.document = document;
        }

        public int Count => this.document.nameValuePairs.Count;

        public bool IsReadOnly => true;

        public IEnumerator<string> GetEnumerator()
            => new OrderedEnumerator(this.document);

        IEnumerator IEnumerable.GetEnumerator()
            => new OrderedEnumerator(this.document);

        public void Add(string item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(string item)
            => this.document.nameValuePairs.ContainsKey(item);

        public void CopyTo(string[] array, int arrayIndex)
        {
            var enumerator = new OrderedEnumerator(this.document);
            while (enumerator.MoveNext() && arrayIndex < array.Length)
            {
                array[arrayIndex++] = enumerator.Current;
            }
        }

        public bool Remove(string item)
        {
            throw new NotImplementedException();
        }

        public sealed class OrderedEnumerator : IEnumerator<string>
        {
            private readonly int version;

            private readonly EnvDocument document;

            private int position = -1;

            internal OrderedEnumerator(EnvDocument document)
            {
                this.document = document;
                this.version = this.document.version;
                this.Current = string.Empty;
            }

            public string Current { get; private set; }

            object IEnumerator.Current => this.Current;

            public bool MoveNext()
            {
                while (true)
                {
                    if (this.document.version != this.version)
                        throw new InvalidOperationException("The dictionary has changed");

                    this.position++;
                    if (this.position >= this.document.entries.Count)
                        return false;

                    var entry = this.document.entries[this.position];
                    if (entry is EnvNameValuePair record)
                    {
                        this.Current = record.Name;
                        return true;
                    }
                }
            }

            public void Reset()
            {
                this.position = -1;
            }

            public void Dispose()
            {
                // NOOP
            }
        }
    }

    public sealed class OrderedValueCollection : ICollection<string>
    {
        private readonly EnvDocument document;

        internal OrderedValueCollection(EnvDocument document)
        {
            this.document = document;
        }

        public int Count => this.document.nameValuePairs.Count;

        public bool IsReadOnly => true;

        public IEnumerator<string> GetEnumerator()
            => new OrderedEnumerator(this.document);

        IEnumerator IEnumerable.GetEnumerator()
            => new OrderedEnumerator(this.document);

        public void Add(string item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(string item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            var enumerator = new OrderedEnumerator(this.document);
            while (enumerator.MoveNext() && arrayIndex < array.Length)
            {
                array[arrayIndex++] = enumerator.Current;
            }
        }

        public bool Remove(string item)
        {
            throw new NotImplementedException();
        }

        // ReSharper disable once MemberHidesStaticFromOuterClass
        public sealed class OrderedEnumerator : IEnumerator<string>
        {
            private readonly int version;

            private readonly EnvDocument document;

            private int position = -1;

            internal OrderedEnumerator(EnvDocument document)
            {
                this.document = document;
                this.version = this.document.version;
                this.Current = default!;
            }

            public string Current { get; private set; }

            object IEnumerator.Current => this.Current;

            public bool MoveNext()
            {
                while (true)
                {
                    if (this.document.version != this.version)
                        throw new InvalidOperationException("The dictionary has changed");

                    this.position++;
                    if (this.position >= this.document.entries.Count)
                        return false;

                    var entry = this.document.entries[this.position];
                    if (entry is EnvNameValuePair record)
                    {
                        this.Current = record.GetRawValueAsString();
                        return true;
                    }
                }
            }

            public void Reset()
            {
                this.position = -1;
            }

            public void Dispose()
            {
                // NOOP
            }
        }
    }
}