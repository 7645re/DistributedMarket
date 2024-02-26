using Carts.Domain.Dto;

namespace Carts.Domain.Services;

public interface ICartService
{
    Task<Cart> GetCartAsync(int cartId, CancellationToken cancellationToken);
    Task CreateCartAsync(CartCreate cartCreate, CancellationToken cancellationToken);
    Task ClearCartAsync(int cartId, CancellationToken cancellationToken);
    Task UpdateCartAsync(CartUpdate cartUpdate, CancellationToken cancellationToken);
}