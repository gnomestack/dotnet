using System.Collections;

namespace GnomeStack.Extras.KpcLib;

public ref struct QueryPath
{
    private readonly ReadOnlySpan<char> path;

    public QueryPath(string path)
        : this(path.AsSpan())
    {
    }

    public QueryPath(ReadOnlySpan<char> path)
    {
        this.path = path;
    }

    public bool IsEmpty => this.path.IsEmpty;

    public int Length => this.path.Length;

    public char this[int index] => this.path[index];

    public static implicit operator QueryPath(string path)
        => new QueryPath(path);

    public static implicit operator QueryPath(ReadOnlySpan<char> path)
        => new QueryPath(path);

    public IReadOnlyList<string> ToSegments()
    {
        return this.ToString().Split('/', StringSplitOptions.RemoveEmptyEntries);
    }

    public QueryPath Join(ReadOnlySpan<char> segment)
    {
        var span = new Span<char>(new char[this.path.Length + segment.Length + 1]);
        this.path.CopyTo(span);
        span[this.path.Length] = '/';
        segment.CopyTo(span.Slice(this.path.Length + 1));
        return new QueryPath(span);
    }

    public QueryPath Join(string segment)
        => this.Join(segment.AsSpan());

    public QueryPath Join(ReadOnlySpan<char> segment1, ReadOnlySpan<char> segment2)
    {
        var span = new Span<char>(new char[this.path.Length + segment1.Length + segment2.Length + 2]);
        this.path.CopyTo(span);
        span[this.path.Length] = '/';
        segment1.CopyTo(span.Slice(this.path.Length + 1));
        span[this.path.Length + segment1.Length + 1] = '/';
        segment2.CopyTo(span.Slice(this.path.Length + segment1.Length + 2));
        return new QueryPath(span);
    }

    public QueryPath Join(string segment1, string segment2)
        => this.Join(segment1.AsSpan(), segment2.AsSpan());

    public QueryPath Join(
        ReadOnlySpan<char> segment1,
        ReadOnlySpan<char> segment2,
        ReadOnlySpan<char> segment3)
    {
        var span = new Span<char>(new char[this.path.Length + segment1.Length + segment2.Length + segment3.Length + 3]);
        this.path.CopyTo(span);
        span[this.path.Length] = '/';
        segment1.CopyTo(span.Slice(this.path.Length + 1));
        span[this.path.Length + segment1.Length + 1] = '/';
        segment2.CopyTo(span.Slice(this.path.Length + segment1.Length + 2));
        span[this.path.Length + segment1.Length + segment2.Length + 2] = '/';
        segment3.CopyTo(span.Slice(this.path.Length + segment1.Length + segment2.Length + 3));
        return new QueryPath(span);
    }

    public QueryPath Join(string segment1, string segment2, string segment3)
        => this.Join(segment1.AsSpan(), segment2.AsSpan(), segment3.AsSpan());

    public (string, string) Pop()
    {
        var span = this.path;
        var index = span.LastIndexOf('/');
        if (index == -1)
            return (this.ToString(), string.Empty);

        var first = span.Slice(0, index);
        var second = span.Slice(index + 1);
#if NETLEGACY
        return (new string(first.ToArray()), new string(second.ToArray()));
#else
        return (first.ToString(), second.ToString());
#endif
    }

    public QueryPath Trim()
    {
        var span = this.path.Trim().TrimEnd('/');
        return new QueryPath(span);
    }

    public override string ToString()
    {
        var str = string.Empty;
#if NETLEGACY
        str = new string(this.path.ToArray());
#else
        str = this.path.ToString();
#endif

        return str;
    }
}