using Carts.Domain.Models;
using Carts.Domain.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Carts.Domain.Repositories.Cart;

public class CartRepository : BaseRedisRepository<CartEntity>, ICartRepository
{
    private readonly RedisOptions _redisOptions;
    
    public CartRepository(IDistributedCache cache, IOptions<RedisOptions> redisOptions) : base(cache)
    {
        _redisOptions = redisOptions.Value;
    }

    public Task CreateAsync(string key, CartEntity entity, CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(key, entity, TimeSpan.FromMinutes(_redisOptions.CartExpiryInMinutes), cancellationToken); 
    }
}