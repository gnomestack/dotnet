using System.Diagnostics;

using GnomeStack.Diagnostics;

namespace GnomeStack.Extensions.Auditing;

public static class AuditingActivity
{
    public static ActivitySource Source { get; } = ActivitySourceFactory.CreateFromAssembly(typeof(AuditingActivity));
}