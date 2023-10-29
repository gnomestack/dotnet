namespace GnomeStack.Builder;

public class DictionaryBuilder<TDictionary, TKey, TValue> : Builder<TDictionary>
    where TKey : notnull
    where TDictionary : class, IDictionary<TKey, TValue>
{
    public DictionaryBuilder()
    {
    }

    public DictionaryBuilder(TDictionary dictionary)
    {
        this.Instance = dictionary;
    }

    public TValue this[TKey key]
    {
        get => this.Instance[key];

        // ReSharper disable once PossibleStructMemberModificationOfNonVariableStruct
        set => this.Instance[key] = value;
    }

    public virtual DictionaryBuilder<TDictionary, TKey, TValue> Add(TKey key, TValue value)
    {
        this.Instance.Add(key, value);
        return this;
    }

    public virtual DictionaryBuilder<TDictionary, TKey, TValue> Add(KeyValuePair<TKey, TValue> kvp)
    {
        this.Instance.Add(kvp.Key, kvp.Value);

        return this;
    }

    public virtual DictionaryBuilder<TDictionary, TKey, TValue> Add(Func<KeyValuePair<TKey, TValue>> func)
    {
        var result = func();
        this.Instance.Add(result.Key, result.Value);

        return this;
    }

    public virtual DictionaryBuilder<TDictionary, TKey, TValue> Add((TKey, TValue) tuple)
    {
        this.Instance.Add(tuple.Item1, tuple.Item2);
        return this;
    }

    public virtual DictionaryBuilder<TDictionary, TKey, TValue> AddRange(IEnumerable<KeyValuePair<TKey, TValue>> values)
    {
        foreach (var value in values)
        {
            this.Instance.Add(value.Key, value.Value);
        }

        return this;
    }

    public override TDictionary Build(bool createNew = false)
    {
        var value = this.Instance;
        if (!createNew)
        {
            return this.OnAfterBuild(value);
        }

        var dict = this.CreateInstance();
        foreach (var item in value)
        {
            dict[item.Key] = item.Value;
        }

        return this.OnAfterBuild(value);
    }

    public virtual IDictionary<TKey, TValue> Build(EqualityComparer<TKey> comparer)
    {
        var dictionary = this.CreateInstance(comparer);
        foreach (var item in this.Instance)
        {
            dictionary[item.Key] = item.Value;
        }

        return this.OnAfterBuild(dictionary);
    }

    protected virtual TDictionary CreateInstance(EqualityComparer<TKey> comparer)
    {
        #pragma warning disable REFL001
        var instance = Activator.CreateInstance(typeof(TDictionary), comparer);
        if (instance is null)
        {
            throw new InvalidOperationException($"Failed to create instance of {typeof(TDictionary)}");
        }

        return (TDictionary)instance;
    }
}

public class DictionaryBuilder<TKey, TValue> : DictionaryBuilder<Dictionary<TKey, TValue>, TKey, TValue>
    where TKey : notnull
{
    public DictionaryBuilder()
    {
    }

    public DictionaryBuilder(Dictionary<TKey, TValue> dictionary)
        : base(dictionary)
    {
    }

    protected override Dictionary<TKey, TValue> CreateInstance()
        => new();

    protected override Dictionary<TKey, TValue> CreateInstance(EqualityComparer<TKey> comparer)
    {
        return new Dictionary<TKey, TValue>(comparer);
    }
}