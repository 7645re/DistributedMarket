using Carts.Domain.Models;

namespace Carts.Domain.Repositories.Cart;

public interface ICartRepository : IBaseRedisRepository<CartEntity>
{
}