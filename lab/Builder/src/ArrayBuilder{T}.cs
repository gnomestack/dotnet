namespace GnomeStack.Builder;

public class ArrayBuilder<T>
{
    private readonly List<T> list = new();

    public T this[int index]
    {
        get => this.list[index];
        set => this.list[index] = value;
    }

    public static implicit operator T[](ArrayBuilder<T> builder)
    {
        return builder.Build();
    }

    public ArrayBuilder<T> Set(int index, T value)
    {
        this.list[index] = value;
        return this;
    }

    public ArrayBuilder<T> Add(T item)
    {
        this.list.Add(item);
        return this;
    }

    public ArrayBuilder<T> Add(Func<T> factory)
    {
        this.list.Add(factory());
        return this;
    }

    public ArrayBuilder<T> AddRange(IEnumerable<T> items)
    {
        this.list.AddRange(items);
        return this;
    }

    public ArrayBuilder<T> AddRange(params T[] items)
    {
        this.list.AddRange(items);
        return this;
    }

    public virtual T[] Build()
    {
        var copy = this.list.ToArray();
        return this.OnAfterBuild(copy);
    }

    protected virtual T[] OnAfterBuild(T[] value)
        => value;
}