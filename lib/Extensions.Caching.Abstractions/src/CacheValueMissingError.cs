using GnomeStack.Functional;

namespace GnomeStack.Extensions.Caching;

public class CacheValueMissingError : Error
{
    public CacheValueMissingError(string? message, IInnerError? innerError = null)
        : base(message ?? "Cached item expired or missing", innerError)
    {
    }
}