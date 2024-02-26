using Carts.Domain.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Carts.Domain.Repositories.Cart;

public class CartRepository : BaseRedisRepository<CartEntity>, ICartRepository
{
    public CartRepository(IDistributedCache cache) : base(cache)
    {
    }
}