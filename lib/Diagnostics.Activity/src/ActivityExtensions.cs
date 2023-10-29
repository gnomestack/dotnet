using System.Diagnostics;

namespace GnomeStack.Diagnostics;

public static class ActivityExtensions
{
    public static ActivitySpanId GetParentId(this Activity? activity)
        => activity?.ParentSpanId ?? default;

    public static Activity? WithName(this Activity? activity, string name)
    {
        if (activity is not null)
            activity.DisplayName = name;

        return activity;
    }

    public static Activity? WithTag(this Activity? activity, string name, string? value)
    {
        activity?.SetTag(name, value);
        return activity;
    }

    public static Activity? WithTag(this Activity? activity, string name, bool value)
    {
        activity?.SetTag(name, value);
        return activity;
    }

    public static Activity? WithTag(this Activity? activity, string name, int value)
    {
        activity?.SetTag(name, value);
        return activity;
    }

    public static Activity? WithTag(this Activity? activity, string name, double value)
    {
        activity?.SetTag(name, value);
        return activity;
    }

    public static Activity? WithTag(this Activity? activity, string name, string?[]? value)
    {
        activity?.SetTag(name, value);
        return activity;
    }

    public static Activity? WithTag(this Activity? activity, string name, bool[]? value)
    {
        activity?.SetTag(name, value);
        return activity;
    }

    public static Activity? WithTag(this Activity? activity, string name, int[]? value)
    {
        activity?.SetTag(name, value);
        return activity;
    }

    public static Activity? WithTag(this Activity? activity, string name, double[]? value)
    {
        activity?.SetTag(name, value);
        return activity;
    }

    public static Activity? WithTagSet(this Activity? activity, IDictionary<string, object?> tags)
    {
        if (activity is null)
            return activity;

        foreach (var key in tags.Keys)
            activity?.SetTag(key, tags[key]);

        return activity;
    }

    public static Activity? WithStatus(this Activity? activity, ActivityStatus status)
    {
        activity?.SetStatus(status.Code, status.Description);
        return activity;
    }

    public static Activity? Ok(this Activity? activity)
    {
        activity?.SetStatus(ActivityStatusCode.Ok);
        return activity;
    }

    public static Activity? Error(this Activity? activity, string? description = null)
    {
        activity?.WithStatus(ActivityStatus.Error.WithDescription(description));
        return activity;
    }

    public static Activity? Error(this Activity? activity, Exception ex)
    {
        activity.RecordException(ex);
        activity?.WithStatus(ActivityStatus.Error.WithDescription(ex.Message));
        return activity;
    }

    public static Activity? Unset(this Activity? activity)
    {
        activity?.SetStatus(ActivityStatusCode.Unset);
        return activity;
    }

    public static bool IsRecording(this Activity? activity)
    {
        return activity?.IsAllDataRequested == true;
    }

    public static Activity? RecordException(this Activity? activity, Exception? ex)
    {
        if (ex is null || activity is null)
            return activity;

        return activity?.RecordException(ex.GetType().FullName, ex.Message, ex.StackTrace);
    }

    public static Activity? RecordException(this Activity? activity, Exception? ex, ActivityTagsCollection tags)
    {
        if (ex is null || activity is null)
            return activity;

        return activity?.RecordException(ex.GetType().FullName, ex.Message, ex.StackTrace, tags);
    }

    public static Activity? RecordException(this Activity? activity, string? type, string? message, string? stackTrace)
    {
        var tags = new ActivityTagsCollection();
        activity?.RecordException(type, message, stackTrace, tags);
        return activity;
    }

    public static Activity? RecordException(this Activity? activity, string? type, string? message, string? stackTrace, ActivityTagsCollection tags)
    {
        if (!type.IsNullOrWhiteSpace())
            tags.Add(TraceTags.ExceptionType, type);

        if (!message.IsNullOrWhiteSpace())
            tags.Add(TraceTags.ExceptionMessage, message);

        if (!stackTrace.IsNullOrWhiteSpace())
            tags.Add(TraceTags.ExceptionStacktrace, stackTrace);

        if (tags.Count == 0)
            return activity;

        activity?.AddEvent(TraceTags.EventException, tags: tags);
        return activity;
    }

    public static Activity? AddEvent(
        this Activity? activity,
        string eventName)
    {
        activity?.AddEvent(new ActivityEvent(eventName, default));
        return activity;
    }

    public static Activity? AddEvent(
        this Activity? activity,
        string eventName,
        ActivityTagsCollection tags)
    {
        activity?.AddEvent(new ActivityEvent(eventName, default, tags));
        return activity;
    }

    public static Activity? AddEvent(
        this Activity? activity,
        string eventName,
        DateTimeOffset timestamp)
    {
        activity?.AddEvent(new ActivityEvent(eventName, timestamp));
        return activity;
    }

    public static Activity? AddEvent(
        this Activity? activity,
        string eventName,
        DateTimeOffset timestamp,
        ActivityTagsCollection tags)
    {
        activity?.AddEvent(new ActivityEvent(eventName, timestamp, tags));
        return activity;
    }
}