#if NETLEGACY

using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace : used to polyfill.
#pragma warning disable SA1649
namespace System;

/// <summary>Represent a type can be used to index a collection either from the start or the end.</summary>
/// <remarks>
/// Index is used by the C# compiler to support the new index syntax.
/// <code>
/// int[] someArray = new int[5] { 1, 2, 3, 4, 5 } ;
/// int lastElement = someArray[^1]; // lastElement = 5
/// </code>
/// </remarks>
internal readonly struct Index : IEquatable<Index>
{
    private readonly int value;

    /// <summary>Initializes a new instance of the <see cref="Index"/> struct.</summary>
    /// <param name="value">The index value. it has to be zero or positive number.</param>
    /// <param name="fromEnd">Indicating if the index is from the start or from the end.</param>
    /// <remarks>
    /// If the Index constructed from the end, index value 1 means pointing at the last element and index value 0 means pointing at beyond last element.
    /// </remarks>
#if !NET35
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public Index(int value, bool fromEnd = false)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "value must be non-negative");
        }

        if (fromEnd)
            this.value = ~value;
        else
            this.value = value;
    }

    /// <summary>Gets create an Index pointing at first element.</summary>
    public static Index Start => new Index(0);

    /// <summary>Gets create an Index pointing at beyond last element.</summary>
    public static Index End => new Index(~0);

    /// <summary>Gets the index value.</summary>
    public int Value
    {
        get
        {
            if (this.value < 0)
                return ~this.value;
            else
                return this.value;
        }
    }

    /// <summary>Gets a value indicating whether indicates whether the index is from the start or the end.</summary>
    public bool IsFromEnd => this.value < 0;

    /// <summary>Converts integer number to an Index.</summary>
    /// <param name="value">The start value.</param>
    public static implicit operator Index(int value) => FromStart(value);

    /// <summary>Create an Index from the start at the position indicated by the value.</summary>
    /// <param name="value">The index value from the start.</param>
    /// <returns>The index.</returns>
#if !NET35
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Index FromStart(int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "value must be non-negative");
        }

        return new Index(value);
    }

    /// <summary>Create an Index from the end at the position indicated by the value.</summary>
    /// <param name="value">The index value from the end.</param>
    /// <returns>The index.</returns>
#if !NET35
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Index FromEnd(int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "value must be non-negative");
        }

        return new Index(~value);
    }

    /// <summary>Calculate the offset from the start using the giving collection length.</summary>
    /// <param name="length">The length of the collection that the Index will be used with. length has to be a positive value.</param>
    /// <remarks>
    /// For performance reason, we don't validate the input length parameter and the returned offset value against negative values.
    /// we don't validate either the returned offset is greater than the input length.
    /// It is expected Index will be used with collections which always have non negative length/count. If the returned offset is negative and
    /// then used to index a collection will get out of range exception which will be same affect as the validation.
    /// </remarks>
    /// <returns>The offset.</returns>
#if !NET35
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public int GetOffset(int length)
    {
        int offset = this.value;
        if (this.IsFromEnd)
        {
            // offset = length - (~value)
            // offset = length + (~(~value) + 1)
            // offset = length + value + 1
            offset += length + 1;
        }

        return offset;
    }

    /// <summary>Indicates whether the current Index object is equal to another object of the same type.</summary>
    /// <param name="obj">An object to compare with this object.</param>
    /// <returns><see langword="true" /> when equal; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? obj) => obj is Index && this.value == ((Index)obj).value;

    /// <summary>Indicates whether the current Index object is equal to another Index object.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><see langword="true" /> when equal; otherwise, <see langword="false" />.</returns>
    public bool Equals(Index other) => this.value == other.value;

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>The hashcode.</returns>
    public override int GetHashCode() => this.value;

    /// <summary>Converts the value of the current Index object to its equivalent string representation.</summary>
    /// <returns>The string representation of the index.</returns>
    public override string ToString()
    {
        if (this.IsFromEnd)
            return "^" + ((uint)this.Value).ToString();

        return ((uint)this.Value).ToString();
    }
}

/// <summary>Represent a range has start and end indexes.</summary>
/// <remarks>
/// Range is used by the C# compiler to support the range syntax.
/// <code>
/// int[] someArray = new int[5] { 1, 2, 3, 4, 5 };
/// int[] subArray1 = someArray[0..2]; // { 1, 2 }
/// int[] subArray2 = someArray[1..^0]; // { 2, 3, 4, 5 }
/// </code>
/// </remarks>
internal readonly struct Range : IEquatable<Range>
{
    /// <summary>Initializes a new instance of the <see cref="Range"/> struct.
    /// Construct a Range object using the start and end indexes.</summary>
    /// <param name="start">Represent the inclusive start index of the range.</param>
    /// <param name="end">Represent the exclusive end index of the range.</param>
    public Range(Index start, Index end)
    {
        this.Start = start;
        this.End = end;
    }

    /// <summary>Gets a new Range object starting from first element to the end.</summary>
    /// <returns>The range.</returns>
    public static Range All => new Range(Index.Start, Index.End);

    /// <summary>Gets the inclusive start index of the Range.</summary>
    public Index Start { get; }

    /// <summary>Gets the exclusive end index of the Range.</summary>
    public Index End { get; }

    /// <summary>Create a Range object starting from start index to the end of the collection.</summary>
    /// <param name="start">The start index.</param>
    /// <returns>The range.</returns>
    public static Range StartAt(Index start) => new Range(start, Index.End);

    /// <summary>Create a Range object starting from first element in the collection to the end Index.</summary>
    /// <param name="end">The end index.</param>
    /// <returns>The range.</returns>
    public static Range EndAt(Index end) => new Range(Index.Start, end);

    /// <summary>Indicates whether the current Range object is equal to another object of the same type.</summary>
    /// <param name="obj">An object to compare with this object.</param>
    /// <returns><see langword="true" /> when equal; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? obj) =>
        obj is Range r &&
        r.Start.Equals(this.Start) &&
        r.End.Equals(this.End);

    /// <summary>Indicates whether the current Range object is equal to another Range object.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><see langword="true" /> when equal; otherwise, <see langword="false" />.</returns>
    public bool Equals(Range other) => other.Start.Equals(this.Start) && other.End.Equals(this.End);

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>The hashcode.</returns>
    public override int GetHashCode()
    {
        return (this.Start.GetHashCode() * 31) + this.End.GetHashCode();
    }

    /// <summary>Converts the value of the current Range object to its equivalent string representation.</summary>
    /// <returns>The string representation of the range.</returns>
    public override string ToString()
    {
        return this.Start + ".." + this.End;
    }

    /// <summary>Calculate the start offset and length of range object using a collection length.</summary>
    /// <param name="length">The length of the collection that the range will be used with. length has to be a positive value.</param>
    /// <remarks>
    /// For performance reason, we don't validate the input length parameter against negative values.
    /// It is expected Range will be used with collections which always have non negative length/count.
    /// We validate the range is inside the length scope though.
    /// </remarks>
    /// <returns>A value tuple with of offset and length.</returns>
#if !NET35
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public (int Offset, int Length) GetOffsetAndLength(int length)
    {
        int start;
        Index startIndex = this.Start;
        if (startIndex.IsFromEnd)
            start = length - startIndex.Value;
        else
            start = startIndex.Value;

        int end;
        Index endIndex = this.End;
        if (endIndex.IsFromEnd)
            end = length - endIndex.Value;
        else
            end = endIndex.Value;

        if ((uint)end > (uint)length || (uint)start > (uint)end)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        return (start, end - start);
    }
}
#endif