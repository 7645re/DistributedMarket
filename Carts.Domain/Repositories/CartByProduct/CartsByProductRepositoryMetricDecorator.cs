using Carts.Domain.Models;
using Shared.DiagnosticContext;

namespace Carts.Domain.Repositories.CartByProduct;

public class CartsByProductRepositoryMetricDecorator : ICartsByProductRepository
{
    private readonly ICartsByProductRepository _cartsByProductRepository;
    private readonly IDiagnosticContext _diagnosticContext;

    public CartsByProductRepositoryMetricDecorator(
        ICartsByProductRepository cartsByProductRepository,
        IDiagnosticContext diagnosticContext)
    {
        _cartsByProductRepository = cartsByProductRepository;
        _diagnosticContext = diagnosticContext;
    }

    public async Task<CartsByProductIdEntity?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        using (_diagnosticContext.Measure($"{nameof(CartsByProductRepositoryMetricDecorator)}.{nameof(GetByKeyAsync)}"))
            return await _cartsByProductRepository.GetByKeyAsync(key, cancellationToken);
    }

    public async Task CreateAsync(string key, CartsByProductIdEntity entity, CancellationToken cancellationToken = default)
    {
        using (_diagnosticContext.Measure($"{nameof(CartsByProductRepositoryMetricDecorator)}.{nameof(CreateAsync)}"))
            await _cartsByProductRepository.CreateAsync(key, entity, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        using (_diagnosticContext.Measure($"{nameof(CartsByProductRepositoryMetricDecorator)}.{nameof(RemoveAsync)}"))
            await _cartsByProductRepository.RemoveAsync(key, cancellationToken);
    }
}