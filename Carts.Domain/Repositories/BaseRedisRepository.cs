using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Carts.Domain.Repositories;

public abstract class BaseRedisRepository<T> : IBaseRedisRepository<T>
{
    private readonly IDistributedCache _cache;

    protected BaseRedisRepository(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var entity = await _cache.GetStringAsync(key, cancellationToken);
        if (entity is null)
            return default;

        return JsonSerializer.Deserialize<T>(entity);
    }

    public async Task CreateAsync(
        string key,
        T entity,
        TimeSpan? expiry = null,
        CancellationToken cancellationToken = default)
    {
        var serializedEntity = JsonSerializer.Serialize(entity);
        await _cache.SetStringAsync(key, serializedEntity, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry
        }, cancellationToken);
    }

    public async Task UpdateAsync(string key, T entity, CancellationToken cancellationToken = default)
    {
        var serializedEntity = JsonSerializer.Serialize(entity);
        await _cache.SetStringAsync(key, serializedEntity, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(key, cancellationToken);
    }
}