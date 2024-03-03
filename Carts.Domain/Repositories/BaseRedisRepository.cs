using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Shared.DiagnosticContext;

namespace Carts.Domain.Repositories;

public abstract class BaseRedisRepository<T>
{
    private readonly IDistributedCache _cache;

    protected BaseRedisRepository(
        IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var redisKey = $"{typeof(T).Name}:{key}";
        var entity = await _cache.GetStringAsync(redisKey, cancellationToken);
        if (entity is null)
            return default;

        return JsonSerializer.Deserialize<T>(entity);    
    }

    public virtual async Task CreateAsync(
        string key,
        T entity,
        TimeSpan? expiry = null,
        CancellationToken cancellationToken = default)
    {
        var redisKey = $"{typeof(T).Name}:{key}";
        var serializedEntity = JsonSerializer.Serialize(entity);
        await _cache.SetStringAsync(redisKey, serializedEntity, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry
        }, cancellationToken);    
    }

    public virtual async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        var redisKey = $"{typeof(T).Name}:{key}";
        await _cache.RemoveAsync(redisKey, cancellationToken);    
    }
}