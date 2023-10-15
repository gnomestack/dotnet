namespace Xunit.Sdk;

/// <summary>
/// Represents a raised event after the fact.
/// </summary>
/// <typeparam name="T">The type of the event arguments.</typeparam>
public class RaisedEvent<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RaisedEvent{T}" /> class.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="args">The event arguments.</param>
    public RaisedEvent(object? sender, T args)
    {
        this.Sender = sender;
        this.Arguments = args;
    }

    /// <summary>
    /// Gets the sender of the event.
    /// </summary>
    public object? Sender { get; }

    /// <summary>
    /// Gets the event arguments.
    /// </summary>
    public T Arguments { get; }
}