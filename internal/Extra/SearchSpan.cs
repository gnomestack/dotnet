namespace GnomeStack.Extras.Strings;

/// <summary>
/// Struct <see cref="SearchSpan"/> represents the location of a span of text
/// within a larger body of text.
/// </summary>
#if DFX_CORE
public
#else
internal
#endif
    readonly struct SearchSpan
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchSpan"/> struct.
    /// </summary>
    /// <param name="start">The start index of the span.</param>
    /// <param name="length">The length of the span.</param>
    public SearchSpan(int start, int length)
    {
        this.Start = start;
        this.End = start + length;
        this.Length = length;
        this.IsEmpty = length == 0;
    }

    /// <summary>
    /// Gets a value indicating whether the span is empty.
    /// </summary>
    public bool IsEmpty { get; }

    /// <summary>
    /// Gets the length of the span.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Gets the start index of the span.
    /// </summary>
    public int Start { get; }

    /// <summary>
    /// Gets the end index of the span.
    /// </summary>
    public int End { get; }
}