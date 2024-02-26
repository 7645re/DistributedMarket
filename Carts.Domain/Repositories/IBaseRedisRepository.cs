namespace Carts.Domain.Repositories;

public interface IBaseRedisRepository<T>
{
    Task<T?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task CreateAsync(string key, T entity, TimeSpan? expiry = null, CancellationToken cancellationToken = default);
    Task UpdateAsync(string key, T entity, CancellationToken cancellationToken = default);
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}