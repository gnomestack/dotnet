using GnomeStack.Functional;

namespace GnomeStack.Extensions.Caching;

public interface ICacheAsync
{
    public Task<Result<T, Exception>> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        where T : notnull;

    public Task<Result<Nil, Exception>> SetAsync<T>(string key, T value, CacheEntryOptions? options = null, CancellationToken cancellationToken = default);

    public Task<Result<Nil, Exception>> RemoveAsync(string key, CancellationToken cancellationToken = default);
}