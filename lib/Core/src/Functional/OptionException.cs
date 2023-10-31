using System;

namespace GnomeStack.Functional;

/// <summary>
/// An exception that is thrown when an option is <c>None</c>.
/// </summary>
[Serializable]
public class OptionException : System.Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OptionException"/> class.
    /// </summary>
    public OptionException()
        : base("Option is None.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public OptionException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="inner">The inner exception.</param>
    public OptionException(string message, System.Exception inner)
        : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionException"/> class
    /// with the serialized data.
    /// </summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    protected OptionException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>
    /// Throws an <see cref="OptionException"/> if the
    /// <paramref name="optional"/> is <c>None</c>.
    /// </summary>
    /// <param name="optional">The option to evaluate.</param>
    /// <exception cref="OptionException">
    /// Throws when the <paramref name="optional"/> is <c>None</c>.
    /// </exception>
    public static void ThrowIfNone(IOptional optional)
    {
        if (optional.IsNone)
            throw new OptionException($"{optional.GetType().FullName} is None.");
    }

    /// <summary>
    /// Throws an <see cref="OptionException"/> if the
    /// <paramref name="optional"/> is <c>None</c>.
    /// </summary>
    /// <param name="optional">The option to evaluate.</param>
    /// <typeparam name="T">The type of the option.</typeparam>
    /// <exception cref="OptionException">
    /// Throws when the <paramref name="optional"/> is <c>None</c>.
    /// </exception>
    public static void ThrowIfNone<T>(IOptional<T> optional)
        where T : notnull
    {
        if (optional.IsNone)
            throw new OptionException($"IOptional<{typeof(T).Name}> is None.");
    }

    /// <summary>
    /// Throws an <see cref="OptionException"/> if the
    /// <paramref name="optional"/> is <c>None</c>.
    /// </summary>
    /// <param name="optional">The option to evaluate.</param>
    /// <typeparam name="T">The type of the option.</typeparam>
    /// <exception cref="OptionException">
    /// Throws when the <paramref name="optional"/> is <c>None</c>.
    /// </exception>
    public static void ThrowIfNone<T>(Option<T> optional)
        where T : notnull
    {
        if (optional.IsNone)
            throw new OptionException($"Option<{typeof(T).Name}> is None.");
    }
}