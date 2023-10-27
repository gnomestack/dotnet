using System.Collections.ObjectModel;

namespace GnomeStack.FluentBuilder;

public class CollectionBuilder<TCollection, TItem>
    : Builder<TCollection>
    where TCollection : ICollection<TItem>, new()
{
    public CollectionBuilder()
    {
    }

    public CollectionBuilder(TCollection instance)
    {
        this.Instance = instance;
    }

    public void Add(TItem item)
    {
        this.Instance.Add(item);
    }

    public void Add(Func<TItem> func)
    {
        var result = func();
        this.Instance.Add(result);
    }

    public void AddRange(IEnumerable<TItem> items)
    {
        foreach (var item in items)
        {
            this.Instance.Add(item);
        }
    }

    public void AddRange(params TItem[] items)
    {
        foreach (var item in items)
        {
            this.Instance.Add(item);
        }
    }

    public override TCollection Build(bool createNew = false)
    {
        var value = this.Instance;
        if (!createNew)
        {
            return this.OnAfterBuild(value);
        }

        var col = this.CreateInstance();
        foreach (var item in value)
        {
            col.Add(item);
        }

        return this.OnAfterBuild(value);
    }

    protected override TCollection CreateInstance()
        => new TCollection();
}

public class CollectionBuilder<TItem>
    : CollectionBuilder<Collection<TItem>, TItem>
{
    public CollectionBuilder()
    {
    }

    public CollectionBuilder(Collection<TItem> instance)
        : base(instance)
    {
    }
}

public class ListBuilder<TItem>
    : CollectionBuilder<List<TItem>, TItem>
{
    public ListBuilder()
    {
    }

    public ListBuilder(List<TItem> instance)
        : base(instance)
    {
    }

    public TItem this[int index]
    {
        get => this.Instance[index];
        set => this.Instance[index] = value;
    }

    public ListBuilder<TItem> Set(int index, TItem value)
    {
        this.Instance[index] = value;
        return this;
    }
}

public class HashSetBuilder<TItem>
    : CollectionBuilder<HashSet<TItem>, TItem>
{
    public HashSetBuilder()
    {
    }

    public HashSetBuilder(HashSet<TItem> instance)
        : base(instance)
    {
    }
}