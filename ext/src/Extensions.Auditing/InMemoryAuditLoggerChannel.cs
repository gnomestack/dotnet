using System.Collections;
using System.Diagnostics;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace GnomeStack.Extensions.Auditing;

public class InMemoryAuditLoggerChannel : AuditLoggerChannel, IEnumerable<AuditRecord>
{
    private readonly List<AuditRecord> flushedRecords = new();

    public InMemoryAuditLoggerChannel(ILogger<InMemoryAuditLoggerChannel> logger, AuditLoggerChannelOptions options)
        : base(logger, options)
    {
    }

    public IEnumerator<AuditRecord> GetEnumerator()
        => this.flushedRecords.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    [CLSCompliant(false)]
    protected override Task<bool> TransmitAsync(AuditRecord[]? records, Activity? activity)
    {
        if (records is null || records.Length == 0)
            return Task.FromResult(false);

        this.flushedRecords.AddRange(records);
        return Task.FromResult(true);
    }
}