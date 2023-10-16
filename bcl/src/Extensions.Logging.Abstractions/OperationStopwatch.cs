using System.Diagnostics;

namespace GnomeStack.Extensions.Logging;

public class OperationStopwatch
{
    private readonly Stopwatch stopwatch = new();

    public long ElapsedMilliseconds => this.stopwatch.ElapsedMilliseconds;

    public long ElapsedTicks => this.stopwatch.ElapsedTicks;

    public TimeSpan Elapsed => this.stopwatch.Elapsed;

    public DateTime StartedAt { get; private set; }

    public static implicit operator Stopwatch(OperationStopwatch operationStopwatch)
    {
        return operationStopwatch.stopwatch;
    }

    public OperationStopwatch Start()
    {
        this.StartedAt = DateTime.UtcNow;
        this.stopwatch.Start();
        return this;
    }

    public OperationStopwatch Stop()
    {
        this.stopwatch.Stop();
        return this;
    }

    public void Reset()
    {
        this.stopwatch.Reset();
    }

    public OperationStopwatch Restart()
    {
        this.StartedAt = DateTime.UtcNow;
        this.stopwatch.Restart();
        return this;
    }
}