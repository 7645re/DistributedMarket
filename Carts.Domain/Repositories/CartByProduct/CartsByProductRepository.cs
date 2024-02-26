using Carts.Domain.Models;
using Carts.Domain.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Carts.Domain.Repositories.CartByProduct;

public class CartsByProductRepository : BaseRedisRepository<CartsByProductIdEntity>, ICartsByProductRepository
{
    private readonly RedisOptions _redisOptions;
    
    public CartsByProductRepository(IDistributedCache cache, IOptions<RedisOptions> redisOptions) : base(cache)
    {
        _redisOptions = redisOptions.Value;
    }

    public Task CreateAsync(string key, CartsByProductIdEntity entity, CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(key, entity, TimeSpan.FromMinutes(_redisOptions.CartsByProductExpiryInMinutes), cancellationToken); 
    }
}