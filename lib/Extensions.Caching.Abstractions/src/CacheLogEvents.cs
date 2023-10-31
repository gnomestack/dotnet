using Microsoft.Extensions.Logging;

namespace GnomeStack.Extensions.Caching;

public static class CacheLogEvents
{
    public static EventId GetCache { get; } = new(833, "gs-cache-get");

    public static EventId SetCache { get; } = new(834, "gs-cache-set");

    public static EventId RemoveCache { get; } = new(835, "gs-cache-remove");

    public static EventId EmptyCache { get; } = new(836, "gs-cache-empty");
}