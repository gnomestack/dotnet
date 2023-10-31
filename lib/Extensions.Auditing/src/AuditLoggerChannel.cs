using System.Diagnostics;

using GnomeStack.Diagnostics;
using GnomeStack.Extensions.Logging;

using Microsoft.Extensions.Logging;

namespace GnomeStack.Extensions.Auditing;

public abstract class AuditLoggerChannel : IAuditLoggerChannel, IDisposable
{
    private readonly object syncLock = new();

    private readonly TimeSpan flushSpan;

    private readonly List<AuditRecord> buffer = new();

    private readonly ILogger logger;

    private DateTime lastFlush;

    protected AuditLoggerChannel(ILogger logger, AuditLoggerChannelOptions options)
    {
        this.logger = logger;
        this.flushSpan = options.FlushSpan;
    }

    public void Send(AuditRecord auditRecord)
    {
        bool shouldFlush = false;
        lock (this.syncLock)
        {
            this.buffer.Add(auditRecord);

            var now = DateTime.UtcNow;
            var ceiling = this.lastFlush.Add(this.flushSpan);
            shouldFlush = now > ceiling;
        }

        if (shouldFlush)
            this.Flush();
    }

    public void Flush()
    {
        lock (this.syncLock)
        {
            if (this.buffer.Count == 0)
                return;

            var records = this.buffer.ToArray();
            var activity = AuditingActivity.Source.CreateActivity("audit.flush", ActivityKind.Internal);
            activity?.Start();
            try
            {
                var success = this.TransmitAsync(records, activity)
                    .GetAwaiter()
                    .GetResult();

                if (success)
                {
                    foreach (var record in records)
                        this.buffer.Remove(record);

                    activity.Ok();
                }
            }
            catch (Exception ex)
            {
                activity?.Error(ex);
            }
            finally
            {
                activity?.Stop();
            }
        }

        this.lastFlush = DateTime.UtcNow;
    }

    public async Task FlushAsync()
    {
        AuditRecord[]? records = null;
        lock (this.syncLock)
        {
            if (this.buffer.Count == 0)
                return;

            records = this.buffer.ToArray();
        }

        var activity = AuditingActivity.Source.CreateActivity("audit.flush", ActivityKind.Internal);
        try
        {
            var success = await this.TransmitAsync(records, activity).ConfigureAwait(false);
            lock (this.syncLock)
            {
                if (success)
                {
                    foreach (var record in records)
                        this.buffer.Remove(record);
                }
            }

            activity?.Stop();
            activity?.SetStatus(ActivityStatusCode.Ok, "Audit records flushed.");
        }
        catch (Exception ex)
        {
            this.logger.Critical(ex, "Failed to flush audit records.");
            activity?.SetStatus(ActivityStatusCode.Error, "Flushing audit records threw an exception.");
            activity?.RecordException(ex);
        }

        this.lastFlush = DateTime.UtcNow;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    [CLSCompliant(false)]
    protected abstract Task<bool> TransmitAsync(AuditRecord[]? records, Activity? activity);

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
            return;

        try
        {
            this.Flush();
            lock (this.syncLock)
            {
                this.buffer.Clear();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}