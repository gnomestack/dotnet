using Microsoft.Extensions.Logging;

namespace GnomeStack.Extensions.Caching;

public static class CacheLogEvents
{
    public static EventId GetCache { get; } = new EventId(833, "gs-cache-get");

    public static EventId SetCache { get; } = new EventId(834, "gs-cache-set");

    public static EventId RemoveCache { get; } = new EventId(835, "gs-cache-remove");

    public static EventId EmptyCache { get; } = new EventId(836, "gs-cache-empty");
}