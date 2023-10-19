using System.Diagnostics.CodeAnalysis;

using GnomeStack.Functional;

namespace GnomeStack.Extensions.Caching;

public interface ICache
{
    public Task<Result<T, Error>> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        where T : notnull;

    public Result<T, Error> Get<T>(string key)
        where T : notnull;

    public Result<Nil, Error> Set<T>(string key, T value, CacheEntryOptions? options = null);

    public Task<Result<Nil, Error>> SetAsync<T>(string key, T value, CacheEntryOptions? options = null, CancellationToken cancellationToken = default);

    public Result<Nil, Error> Remove(string key);

    public Task<Result<Nil, Error>> RemoveAsync(string key, CancellationToken cancellationToken = default);
}