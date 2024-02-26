using Carts.Domain.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Carts.Domain.Repositories.CartByProduct;

public class CartsByProductRepository : BaseRedisRepository<CartsByProductIdEntity>, ICartsByProductRepository
{
    public CartsByProductRepository(IDistributedCache cache) : base(cache)
    {
    }
}