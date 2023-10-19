using System.Diagnostics;

using GnomeStack.Extensions.DiagnosticSource;

namespace GnomeStack.Extensions.Auditing;

[ActivitySource("GnomeStack.Extensions.Auditing", "1.0.0")]
public class AuditingActivity
{
    public static ActivitySource Source { get; } = ActivitySourceFactory.Create<AuditingActivity>();
}