using System.Collections;
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using Pulumi;

namespace Pulumi;

[SuppressMessage("Minor Code Smell", "S3220:Method calls should not resolve ambiguously to overloads with \"params\"")]
public class GsInputList<T> : IAsyncEnumerable<Input<T>>, IEnumerable
{
    private readonly InputList<T> list;

    public GsInputList(InputList<T> list)
    {
        this.list = list;
    }

    internal GsInputList(ISet<T> items)
    {
        var list = new InputList<T>();
        foreach (var item in items)
        {
            list.Add(item);
        }

        this.list = list;
    }

    internal GsInputList(ISet<Input<T>> items)
    {
        var list = new InputList<T>();
        foreach (var item in items)
        {
            list.Add(item);
        }

        this.list = list;
    }

    internal GsInputList(Input<ImmutableArray<T>> items)
    {
        this.list = items.Apply(l => new List<T>(l));
    }

    public static implicit operator InputList<T>(GsInputList<T> list)
    {
        return list.list;
    }

    public static implicit operator GsInputList<T>(Output<T> list)
    {
        return new GsInputList<T>(list);
    }

    public static implicit operator GsInputList<T>(Output<T>[] list)
    {
        return new GsInputList<T>(list);
    }

    public static implicit operator GsInputList<T>(InputList<T> list)
    {
        return new GsInputList<T>(list);
    }

    public static implicit operator GsInputList<T>(T[] items)
    {
        return new GsInputList<T>(items);
    }

    public static implicit operator GsInputList<T>(List<T> items)
    {
        return new GsInputList<T>(items);
    }

    public static implicit operator GsInputList<T>(HashSet<T> items)
    {
        return new GsInputList<T>(items);
    }

    public static implicit operator GsInputList<T>(Input<T>[] items)
    {
        return new GsInputList<T>(items);
    }

    public static implicit operator GsInputList<T>(List<Output<T>> items)
    {
        return new GsInputList<T>(items.ToArray());
    }

    public static implicit operator GsInputList<T>(HashSet<Output<T>> items)
    {
        return new GsInputList<T>(items.ToArray());
    }

    public static implicit operator GsInputList<T>(List<Input<T>> items)
    {
        return new GsInputList<T>(items);
    }

#if NET8_0_OR_GREATER
    public static implicit operator GsInputList<T>(FrozenSet<T> items)
    {
        return new GsInputList<T>(items);
    }
#endif

    public static implicit operator GsInputList<T>(HashSet<Input<T>> items)
    {
        return new GsInputList<T>(items);
    }

    public static implicit operator GsInputList<T>(ImmutableArray<T> items)
    {
        return new GsInputList<T>(items);
    }

    public static implicit operator GsInputList<T>(Input<ImmutableArray<T>> items)
    {
        return new GsInputList<T>(items);
    }

    public void Add(T item)
    {
        this.list.Add(item);
    }

    public void Add(Input<T> item)
    {
        this.list.Add(item);
    }

    public void Add(Output<T> item)
    {
        this.list.Add(item);
    }

    public void AddRange(IEnumerable<T> items)
    {
        foreach (T next in items)
        {
            this.list.Add(next);
        }
    }

    public void AddRange(IEnumerable<Input<T>> items)
    {
        foreach (var next in items)
        {
            this.list.Add(next);
        }
    }

    public IAsyncEnumerator<Input<T>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => this.list.GetAsyncEnumerator(cancellationToken);

    public IEnumerator GetEnumerator()
    {
        // this will throw
        return ((IEnumerable)this.list).GetEnumerator();
    }
}