using System;
using System.Diagnostics;
using System.Linq;

namespace GnomeStack.Extensions.DiagnosticSource;

public static class ActivityExtensions
{
    public static void RecordException(this Activity? activity, Exception ex, DateTimeOffset timestamp = default, bool escaped = false)
    {
        if (activity is null)
            return;
        var tags = new ActivityTagsCollection(new Dictionary<string, object?>()
        {
            ["exception.message"] = ex.Message,
            ["exception.stacktrace"] = ex.StackTrace,
            ["exception.type"] = ex.GetType().FullName,
            ["exception.source"] = ex.Source,
            ["exception.hresult"] = (long)ex.HResult,
            ["exception.escaped"] = escaped,
        });

        activity?.AddEvent(new ActivityEvent("exception", timestamp, tags: tags));
    }
}