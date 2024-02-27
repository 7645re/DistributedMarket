using Carts.Domain.Models;

namespace Carts.Domain.Repositories.CartByProduct;

public interface ICartsByProductRepository : IBaseRedisRepository<CartsByProductIdEntity>
{
}