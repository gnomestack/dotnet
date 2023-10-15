namespace GnomeStack.Diagnostics;

public static class ProcessExtensions
{
#if NETLEGACY
    /// <summary>
    /// Waits asynchronously for the process to exit.
    /// </summary>
    /// <param name="process">The process to wait for cancellation.</param>
    /// <param name="cancellationToken">
    /// A cancellation token. If invoked, the task will return
    /// immediately as canceled.</param>
    /// <returns>A Task representing waiting for the process to end.</returns>
    public static Task WaitForExitAsync(
        this System.Diagnostics.Process process,
        CancellationToken cancellationToken = default)
    {
        if (process.HasExited)
            return Task.CompletedTask;

        var tcs = new TaskCompletionSource<object>();
        process.EnableRaisingEvents = true;

        process.Exited += (sender, args) => tcs.TrySetResult(true);
        if (cancellationToken != default)
            cancellationToken.Register(() => tcs.SetCanceled());

        return process.HasExited ? Task.CompletedTask : tcs.Task;
    }
#endif
}