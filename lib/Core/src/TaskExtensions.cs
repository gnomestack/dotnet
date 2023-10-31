using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GnomeStack;

// https://github.com/awaescher/ObviousAwait/blob/master/ObviousAwait/ObviousExtensions.cs
// under MIT
public static class TaskExtensions
{
    /// <summary>
    /// Configures an awaiter to await a given task and captures the current context to make sure that
    /// subsequent code runs on the same context, like the UI thread of example.
    /// You may want to use this if your async code is coupled tightly to a user interface.
    /// Equals to .ConfigureAwait(true).
    /// </summary>
    /// <param name="t">The task to configure an awaiter for.</param>
    /// <returns>An object used to await this task.</returns>
    public static ConfiguredTaskAwaitable Cap(this Task t) => t.ConfigureAwait(true);

    /// <summary>
    /// Configures an awaiter to await a given task and captures the current context to make sure that
    /// subsequent code runs on the same context, like the UI thread of example.
    /// You may want to use this if your async code is coupled tightly to a user interface.
    /// Equals to .ConfigureAwait(true).
    /// </summary>
    /// <typeparam name="T">The type of the result of the task to wait for.</typeparam>
    /// <param name="t">The task to configure an awaiter for.</param>
    /// <returns>An object used to await this task.</returns>
    public static ConfiguredTaskAwaitable<T> Cap<T>(this Task<T> t) => t.ConfigureAwait(true);

    /// <summary>
    /// Configures an awaiter to await a given task and captures the current context to make sure that
    /// subsequent code runs on the same context, like the UI thread of example.
    /// You may want to use this if your async code is coupled tightly to a user interface.
    /// Equals to .ConfigureAwait(true).
    /// </summary>
    /// <typeparam name="T">The type of the result of the task to wait for.</typeparam>
    /// <param name="t">The task to configure an awaiter for.</param>
    /// <returns>An object used to await this task.</returns>
    public static ConfiguredValueTaskAwaitable<T> Cap<T>(this ValueTask<T> t) => t.ConfigureAwait(true);

    /// <summary>
    /// Configures an awaiter to await a given task and captures the current context to make sure that
    /// subsequent code runs on the same context, like the UI thread of example.
    /// You may want to use this if your async code is coupled tightly to a user interface.
    /// Equals to .ConfigureAwait(true).
    /// </summary>
    /// <param name="t">The task to configure an awaiter for.</param>
    /// <returns>An object used to await this task.</returns>
    public static ConfiguredValueTaskAwaitable Cap(this ValueTask t) => t.ConfigureAwait(true);

    /// <summary>
    /// Configures an awaiter to await a given task without capturing a context. This means that
    /// subsequent code is likely to run on another context than the one the awaited task was called from.
    /// You should use this method on almost every awaited method. But pay attention if your async code is coupled tightly to a user interface.
    /// Equals to .ConfigureAwait(false).
    /// </summary>
    /// <param name="t">The task to configure an awaiter for.</param>
    /// <returns>An object used to await this task.</returns>
    public static ConfiguredTaskAwaitable NoCap(this Task t) => t.ConfigureAwait(false);

    /// <summary>
    /// Configures an awaiter to await a given task without capturing a context. This means that
    /// subsequent code is likely to run on another context than the one the awaited task was called from.
    /// You should use this method on almost every awaited method. But pay attention if your async code is coupled tightly to a user interface.
    /// Equals to .ConfigureAwait(false).
    /// </summary>
    /// <typeparam name="T">The type of the result of the task to wait for.</typeparam>
    /// <param name="t">The task to configure an awaiter for.</param>
    /// <returns>An object used to await this task.</returns>
    public static ConfiguredTaskAwaitable<T> NoCap<T>(this Task<T> t) => t.ConfigureAwait(false);

    /// <summary>
    /// Configures an awaiter to await a given task without capturing a context. This means that
    /// subsequent code is likely to run on another context than the one the awaited task was called from.
    /// You should use this method on almost every awaited method. But pay attention if your async code is coupled tightly to a user interface.
    /// Equals to .ConfigureAwait(false).
    /// </summary>
    /// <param name="t">The task to configure an awaiter for.</param>
    /// <returns>An object used to await this task.</returns>
    public static ConfiguredValueTaskAwaitable NoCap(this ValueTask t) => t.ConfigureAwait(false);

    /// <summary>
    /// Configures an awaiter to await a given task without capturing a context. This means that
    /// subsequent code is likely to run on another context than the one the awaited task was called from.
    /// You should use this method on almost every awaited method. But pay attention if your async code is coupled tightly to a user interface.
    /// Equals to .ConfigureAwait(false).
    /// </summary>
    /// <typeparam name="T">The type of the result of the task to wait for.</typeparam>
    /// <param name="t">The task to configure an awaiter for.</param>
    /// <returns>An object used to await this task.</returns>
    public static ConfiguredValueTaskAwaitable<T> NoCap<T>(this ValueTask<T> t) => t.ConfigureAwait(false);
}