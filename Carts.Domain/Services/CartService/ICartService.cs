using Carts.Domain.Dto;

namespace Carts.Domain.Services.CartService;

public interface ICartService
{
    Task<Cart> GetCartAsync(int cartId, CancellationToken cancellationToken);
    Task CreateCartAsync(CartCreate cartCreate, CancellationToken cancellationToken);
    Task UpdateCartAsync(CartUpdate cartUpdate, CancellationToken cancellationToken);
    Task DeleteDependencyProductAsync(int productId, CancellationToken cancellationToken);
}