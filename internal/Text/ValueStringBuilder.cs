using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GnomeStack.Text;

#pragma warning disable SA1405
internal ref partial struct ValueStringBuilder
{
    private char[]? arrayToReturnToPool;
    private Span<char> chars;
    private int pos;

    public ValueStringBuilder(Span<char> initialBuffer)
    {
        this.arrayToReturnToPool = null;
        this.chars = initialBuffer;
        this.pos = 0;
    }

    public ValueStringBuilder(int initialCapacity)
    {
        this.arrayToReturnToPool = ArrayPool<char>.Shared.Rent(initialCapacity);
        this.chars = this.arrayToReturnToPool;
        this.pos = 0;
    }

    public int Length
    {
        get => this.pos;
        set
        {
            Debug.Assert(value >= 0);
            Debug.Assert(value <= this.chars.Length);
            this.pos = value;
        }
    }

    public int Capacity => this.chars.Length;

    /// <summary>Gets the underlying storage of the builder.</summary>
    public Span<char> RawChars => this.chars;

    public ref char this[int index]
    {
        get
        {
            Debug.Assert(index < this.pos);
            return ref this.chars[index];
        }
    }

    public void EnsureCapacity(int capacity)
    {
        // This is not expected to be called this with negative capacity
        Debug.Assert(capacity >= 0);

        // If the caller has a bug and calls this with negative capacity, make sure to call Grow to throw an exception.
        if ((uint)capacity > (uint)this.chars.Length)
            this.Grow(capacity - this.pos);
    }

    /// <summary>
    /// Get a pinnable reference to the builder.
    /// Does not ensure there is a null char after <see cref="Length"/>
    /// This overload is pattern matched in the C# 7.3+ compiler so you can omit
    /// the explicit method call, and write eg "fixed (char* c = builder)".
    /// </summary>
    /// <returns>A pinnable reference.</returns>
    public ref char GetPinnableReference()
    {
        return ref MemoryMarshal.GetReference(this.chars);
    }

    /// <summary>
    /// Get a pinnable reference to the builder.
    /// </summary>
    /// <param name="terminate">Ensures that the builder has a null char after <see cref="Length"/>.</param>
    /// <returns>A pinnable reference.</returns>
    public ref char GetPinnableReference(bool terminate)
    {
        if (terminate)
        {
            this.EnsureCapacity(this.Length + 1);
            this.chars[this.Length] = '\0';
        }

        return ref MemoryMarshal.GetReference(this.chars);
    }

    public override string ToString()
    {
        string s = this.chars.Slice(0, this.pos).ToString();
        this.Dispose();
        return s;
    }

    /// <summary>
    /// Returns a span around the contents of the builder.
    /// </summary>
    /// <param name="terminate">Ensures that the builder has a null char after <see cref="Length"/>.</param>
    /// <returns>A readonly span.</returns>
    public ReadOnlySpan<char> AsSpan(bool terminate)
    {
        if (terminate)
        {
            this.EnsureCapacity(this.Length + 1);
            this.chars[this.Length] = '\0';
        }

        return this.chars.Slice(0, this.pos);
    }

    public ReadOnlySpan<char> AsSpan()
        => this.chars.Slice(0, this.pos);

    public ReadOnlySpan<char> AsSpan(int start)
        => this.chars.Slice(start, this.pos - start);

    public ReadOnlySpan<char> AsSpan(int start, int length)
        => this.chars.Slice(start, length);

    public bool TryCopyTo(Span<char> destination, out int charsWritten)
    {
        if (this.chars.Slice(0, this.pos).TryCopyTo(destination))
        {
            charsWritten = this.pos;
            this.Dispose();
            return true;
        }
        else
        {
            charsWritten = 0;
            this.Dispose();
            return false;
        }
    }

    public void Insert(int index, char value, int count)
    {
        if (this.pos > this.chars.Length - count)
        {
            this.Grow(count);
        }

        int remaining = this.pos - index;
        this.chars.Slice(index, remaining).CopyTo(this.chars.Slice(index + count));
        this.chars.Slice(index, count).Fill(value);
        this.pos += count;
    }

    public void Insert(int index, string? s)
    {
        if (s == null)
        {
            return;
        }

        int count = s.Length;

        if (this.pos > (this.chars.Length - count))
        {
            this.Grow(count);
        }

        int remaining = this.pos - index;
        this.chars.Slice(index, remaining).CopyTo(this.chars.Slice(index + count));
        s
#if !NET6_0_OR_GREATER
            .AsSpan()
#endif
            .CopyTo(this.chars.Slice(index));
        this.pos += count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(char c)
    {
        int pos = this.pos;
        if ((uint)pos < (uint)this.chars.Length)
        {
            this.chars[pos] = c;
            this.pos = pos + 1;
        }
        else
        {
            this.GrowAndAppend(c);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(string? s)
    {
        if (s == null)
        {
            return;
        }

        int pos = this.pos;
        if (s.Length == 1 && (uint)pos < (uint)this.chars.Length)
        {
            this.chars[pos] = s[0];
            this.pos = pos + 1;
        }
        else
        {
            this.AppendSlow(s);
        }
    }

    public void Append(char c, int count)
    {
        if (this.pos > this.chars.Length - count)
        {
            this.Grow(count);
        }

        Span<char> dst = this.chars.Slice(this.pos, count);
        for (int i = 0; i < dst.Length; i++)
        {
            dst[i] = c;
        }

        this.pos += count;
    }

    public unsafe void Append(char* value, int length)
    {
        int pos = this.pos;
        if (pos > this.chars.Length - length)
        {
            this.Grow(length);
        }

        Span<char> dst = this.chars.Slice(this.pos, length);
        for (int i = 0; i < dst.Length; i++)
        {
            dst[i] = *value++;
        }

        this.pos += length;
    }

    public void Append(ReadOnlySpan<char> value)
    {
        int pos = this.pos;
        if (pos > this.chars.Length - value.Length)
        {
            this.Grow(value.Length);
        }

        value.CopyTo(this.chars.Slice(this.pos));
        this.pos += value.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<char> AppendSpan(int length)
    {
        int origPos = this.pos;
        if (origPos > this.chars.Length - length)
        {
            this.Grow(length);
        }

        this.pos = origPos + length;
        return this.chars.Slice(origPos, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        char[]? toReturn = this.arrayToReturnToPool;
        this = default; // for safety, to avoid using pooled array if this instance is erroneously appended to again
        if (toReturn != null)
        {
            ArrayPool<char>.Shared.Return(toReturn);
        }
    }

    private void AppendSlow(string s)
    {
        int pos = this.pos;
        if (pos > this.chars.Length - s.Length)
        {
            this.Grow(s.Length);
        }

        s
#if !NET6_0_OR_GREATER
            .AsSpan()
#endif
            .CopyTo(this.chars.Slice(pos));
        this.pos += s.Length;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowAndAppend(char c)
    {
        this.Grow(1);
        this.Append(c);
    }

    /// <summary>
    /// Resize the internal buffer either by doubling current buffer size or
    /// by adding <paramref name="additionalCapacityBeyondPos"/> to
    /// <see cref="pos"/> whichever is greater.
    /// </summary>
    /// <param name="additionalCapacityBeyondPos">
    /// Number of chars requested beyond current position.
    /// </param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void Grow(int additionalCapacityBeyondPos)
    {

        Debug.Assert(additionalCapacityBeyondPos > 0);
#pragma warning restore SA1405
        Debug.Assert(this.pos > this.chars.Length - additionalCapacityBeyondPos, "Grow called incorrectly, no resize is needed.");

        // Make sure to let Rent throw an exception if the caller has a bug and the desired capacity is negative
        char[] poolArray = ArrayPool<char>.Shared.Rent(
            (int)Math.Max((uint)(this.pos + additionalCapacityBeyondPos), (uint)this.chars.Length * 2));

        this.chars.Slice(0, this.pos).CopyTo(poolArray);

        char[]? toReturn = this.arrayToReturnToPool;
        this.chars = this.arrayToReturnToPool = poolArray;
        if (toReturn != null)
        {
            ArrayPool<char>.Shared.Return(toReturn);
        }
    }
}