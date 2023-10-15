using System;
using System.Threading.Tasks;

using Xunit.Sdk;

namespace Xunit;

public partial interface IAssert
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
    RaisedEvent<T> Raises<T>(Action<EventHandler<T>> attach, Action<EventHandler<T>> detach, Action testCode);

    /// <summary>
    /// Verifies that an event with the exact or a derived event args is raised.
    /// </summary>
    /// <typeparam name="T">The type of the event arguments to expect.</typeparam>
    /// <param name="attach">Code to attach the event handler.</param>
    /// <param name="detach">Code to detach the event handler.</param>
    /// <param name="testCode">A delegate to the code to be tested.</param>
    /// <returns>The event sender and arguments wrapped in an object.</returns>
    /// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
    RaisedEvent<T> RaisesAny<T>(Action<EventHandler<T>> attach, Action<EventHandler<T>> detach, Action testCode);

    /// <summary>
    /// Verifies that a event with the exact event args (and not a derived type) is raised.
    /// </summary>
    /// <typeparam name="T">The type of the event arguments to expect.</typeparam>
    /// <param name="attach">Code to attach the event handler.</param>
    /// <param name="detach">Code to detach the event handler.</param>
    /// <param name="testCode">A delegate to the code to be tested.</param>
    /// <returns>The event sender and arguments wrapped in an object.</returns>
    /// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
    Task<RaisedEvent<T>> RaisesAsync<T>(Action<EventHandler<T>> attach, Action<EventHandler<T>> detach, Func<Task> testCode);

    /// <summary>
    /// Verifies that an event with the exact or a derived event args is raised.
    /// </summary>
    /// <typeparam name="T">The type of the event arguments to expect.</typeparam>
    /// <param name="attach">Code to attach the event handler.</param>
    /// <param name="detach">Code to detach the event handler.</param>
    /// <param name="testCode">A delegate to the code to be tested.</param>
    /// <returns>The event sender and arguments wrapped in an object.</returns>
    /// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
    Task<RaisedEvent<T>> RaisesAnyAsync<T>(Action<EventHandler<T>> attach, Action<EventHandler<T>> detach, Func<Task> testCode);
}