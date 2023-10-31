using GnomeStack.Functional;

namespace GnomeStack.Extensions.Caching;

public interface ICacheSync
{
    public Result<T, Exception> Get<T>(string key)
        where T : notnull;

    public Result<Nil, Exception> Set<T>(string key, T value, CacheEntryOptions? options = null);

    public Result<Nil, Exception> Remove(string key);
}