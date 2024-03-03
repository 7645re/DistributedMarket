using Carts.Domain.Dto;
using Shared.DiagnosticContext;

namespace Carts.Domain.Services.CartService;

public class CartServiceDecorator : ICartService
{
    private readonly IDiagnosticContext _diagnosticContext;

    private readonly ICartService _cartService;

    public CartServiceDecorator(
        IDiagnosticContext diagnosticContext,
        ICartService cartService)
    {
        _diagnosticContext = diagnosticContext;
        _cartService = cartService;
    }

    public async Task<Cart> GetCartAsync(int cartId, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CartServiceDecorator)}.{nameof(GetCartAsync)}"))
            return await _cartService.GetCartAsync(cartId, cancellationToken);
    }

    public async Task CreateCartAsync(CartCreate cartCreate, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CartServiceDecorator)}.{nameof(CreateCartAsync)}"))
            await _cartService.CreateCartAsync(cartCreate, cancellationToken);
    }

    public async Task UpdateCartAsync(CartUpdate cartUpdate, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CartServiceDecorator)}.{nameof(UpdateCartAsync)}"))
            await _cartService.UpdateCartAsync(cartUpdate, cancellationToken);
    }

    public async Task DeleteDependencyProductAsync(int productId, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CartServiceDecorator)}.{nameof(DeleteDependencyProductAsync)}"))
            await _cartService.DeleteDependencyProductAsync(productId, cancellationToken);
    }
}