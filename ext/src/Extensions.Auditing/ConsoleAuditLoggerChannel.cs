using System.Diagnostics;

using GnomeStack.Standard;

using Microsoft.Extensions.Logging;

namespace GnomeStack.Extensions.Auditing;

public class ConsoleAuditLoggerChannel : AuditLoggerChannel
{
    private readonly bool displayJson;

    public ConsoleAuditLoggerChannel(ILogger<ConsoleAuditLoggerChannel> logger, ConsoleAuditLoggerChannelOptions options)
        : base(logger, options)
    {
        this.displayJson = options.DisplayJson;
    }

    protected override Task<bool> TransmitAsync(AuditRecord[]? records, Activity? activity)
    {
        if (records is null || records.Length == 0)
            return Task.FromResult(false);

        foreach (var a in records)
        {
            Console.WriteLine($"{a.Name} {a.EventId} {a.Timestamp} {a.Duration} {a.OperationName}");
            if (this.displayJson)
                Console.WriteLine(Json.Stringify(a));
        }

        return Task.FromResult(true);
    }
}