using System;
using System.Threading.Tasks;

using Xunit.Sdk;

namespace Xunit;
#pragma warning disable S4457

public partial class FlexAssert
{
    /// <summary>
    /// Verifies that a event with the exact event args is raised.
    /// </summary>
    /// <typeparam name="T">The type of the event arguments to expect.</typeparam>
    /// <param name="attach">Code to attach the event handler.</param>
    /// <param name="detach">Code to detach the event handler.</param>
    /// <param name="testCode">A delegate to the code to be tested.</param>
    /// <returns>The event sender and arguments wrapped in an object.</returns>
    /// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
    public RaisedEvent<T> Raises<T>(Action<EventHandler<T>> attach, Action<EventHandler<T>> detach, Action testCode)
    {
        var raisedEvent = RaisesInternal(attach, detach, testCode);

        if (raisedEvent == null)
            throw new RaisesException(typeof(T));

        if (raisedEvent.Arguments != null && !raisedEvent.Arguments.GetType().Equals(typeof(T)))
            throw new RaisesException(typeof(T), raisedEvent.Arguments.GetType());

        return raisedEvent;
    }

    /// <summary>
    /// Verifies that an event with the exact or a derived event args is raised.
    /// </summary>
    /// <typeparam name="T">The type of the event arguments to expect.</typeparam>
    /// <param name="attach">Code to attach the event handler.</param>
    /// <param name="detach">Code to detach the event handler.</param>
    /// <param name="testCode">A delegate to the code to be tested.</param>
    /// <returns>The event sender and arguments wrapped in an object.</returns>
    /// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
    public RaisedEvent<T> RaisesAny<T>(Action<EventHandler<T>> attach, Action<EventHandler<T>> detach, Action testCode)
    {
        var raisedEvent = RaisesInternal(attach, detach, testCode);

        if (raisedEvent == null)
            throw new RaisesException(typeof(T));

        return raisedEvent;
    }

    /// <summary>
    /// Verifies that a event with the exact event args (and not a derived type) is raised.
    /// </summary>
    /// <typeparam name="T">The type of the event arguments to expect.</typeparam>
    /// <param name="attach">Code to attach the event handler.</param>
    /// <param name="detach">Code to detach the event handler.</param>
    /// <param name="testCode">A delegate to the code to be tested.</param>
    /// <returns>The event sender and arguments wrapped in an object.</returns>
    /// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
    public async Task<RaisedEvent<T>> RaisesAsync<T>(Action<EventHandler<T>> attach, Action<EventHandler<T>> detach, Func<Task> testCode)
    {
        var raisedEvent = await RaisesAsyncInternal(attach, detach, testCode);

        if (raisedEvent == null)
            throw new RaisesException(typeof(T));

        if (raisedEvent.Arguments != null && !raisedEvent.Arguments.GetType().Equals(typeof(T)))
            throw new RaisesException(typeof(T), raisedEvent.Arguments.GetType());

        return raisedEvent;
    }

    /// <summary>
    /// Verifies that an event with the exact or a derived event args is raised.
    /// </summary>
    /// <typeparam name="T">The type of the event arguments to expect.</typeparam>
    /// <param name="attach">Code to attach the event handler.</param>
    /// <param name="detach">Code to detach the event handler.</param>
    /// <param name="testCode">A delegate to the code to be tested.</param>
    /// <returns>The event sender and arguments wrapped in an object.</returns>
    /// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
    public async Task<RaisedEvent<T>> RaisesAnyAsync<T>(Action<EventHandler<T>> attach, Action<EventHandler<T>> detach, Func<Task> testCode)
    {
        var raisedEvent = await RaisesAsyncInternal(attach, detach, testCode);

        if (raisedEvent == null)
            throw new RaisesException(typeof(T));

        return raisedEvent;
    }

    private static RaisedEvent<T>? RaisesInternal<T>(Action<EventHandler<T>> attach, Action<EventHandler<T>> detach, Action testCode)
    {
        if (attach is null)
            throw new ArgumentNullException(nameof(attach));

        if (detach is null)
            throw new ArgumentNullException(nameof(detach));

        if (testCode is null)
            throw new ArgumentNullException(nameof(testCode));

        RaisedEvent<T>? raisedEvent = null;
        void Handler(object? s, T args) => raisedEvent = new RaisedEvent<T>(s, args);

        attach(Handler);
        testCode();
        detach(Handler);
        return raisedEvent;
    }

    private static async Task<RaisedEvent<T>?> RaisesAsyncInternal<T>(Action<EventHandler<T>> attach, Action<EventHandler<T>> detach, Func<Task> testCode)
    {
        if (attach is null)
            throw new ArgumentNullException(nameof(attach));

        if (detach is null)
            throw new ArgumentNullException(nameof(detach));

        if (testCode is null)
            throw new ArgumentNullException(nameof(testCode));

        RaisedEvent<T>? raisedEvent = null;
        void Handler(object? s, T args) => raisedEvent = new RaisedEvent<T>(s, args);

        attach(Handler);
        await testCode();
        detach(Handler);
        return raisedEvent;
    }
}