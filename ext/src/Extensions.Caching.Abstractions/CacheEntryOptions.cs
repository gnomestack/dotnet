namespace GnomeStack.Extensions.Caching;

public class CacheEntryOptions
{
    public DateTimeOffset? AbsoluteExpiration { get; set; }

    public TimeSpan? SlidingExpiration { get; set; }

    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
}