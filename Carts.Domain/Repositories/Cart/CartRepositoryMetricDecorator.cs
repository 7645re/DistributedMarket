using Carts.Domain.Models;
using Shared.DiagnosticContext;

namespace Carts.Domain.Repositories.Cart;

public class CartRepositoryMetricDecorator : ICartRepository
{
    private readonly ICartRepository _cartRepository;
    
    private readonly IDiagnosticContext _diagnosticContext;

    public CartRepositoryMetricDecorator(
        ICartRepository cartRepository,
        IDiagnosticContext diagnosticContext)
    {
        _cartRepository = cartRepository;
        _diagnosticContext = diagnosticContext;
    }

    public async Task<CartEntity?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        using (_diagnosticContext.Measure($"{nameof(CartRepositoryMetricDecorator)}.{nameof(GetByKeyAsync)}")) 
            return await _cartRepository.GetByKeyAsync(key, cancellationToken);
    }

    public async Task CreateAsync(string key, CartEntity entity, CancellationToken cancellationToken = default)
    {
        using (_diagnosticContext.Measure($"{nameof(CartRepositoryMetricDecorator)}.{nameof(CreateAsync)}"))
            await _cartRepository.CreateAsync(key, entity, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        using (_diagnosticContext.Measure($"{nameof(CartRepositoryMetricDecorator)}.{nameof(RemoveAsync)}"))
            await _cartRepository.RemoveAsync(key, cancellationToken);
    }
}