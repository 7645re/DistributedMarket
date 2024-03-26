using Carts.Domain.Models;
using Polly;
using Polly.Retry;
using Shared.DiagnosticContext;

namespace Carts.Domain.Repositories.Cart;

public class CartRepositoryRetryDecorator : ICartRepository
{
    private readonly ICartRepository _cartRepository;
    private readonly IDiagnosticContext _diagnosticContext;
    private readonly AsyncRetryPolicy _retryPolicy;

    public CartRepositoryRetryDecorator(ICartRepository cartRepository, IDiagnosticContext diagnosticContext)
    {
        _cartRepository = cartRepository;
        _diagnosticContext = diagnosticContext;

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3, 
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, calculatedWaitDuration, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount} due to {exception.Message}." +
                                      $" Waiting for {calculatedWaitDuration}");
                });
    }

    public async Task<CartEntity?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        using (_diagnosticContext.Measure($"{nameof(CartRepositoryRetryDecorator)}.{nameof(GetByKeyAsync)}"))
        {
            return await _retryPolicy.ExecuteAsync(async () => await _cartRepository.GetByKeyAsync(key, cancellationToken));
        }
    }

    public async Task CreateAsync(string key, CartEntity entity, CancellationToken cancellationToken = default)
    {
        using (_diagnosticContext.Measure($"{nameof(CartRepositoryRetryDecorator)}.{nameof(CreateAsync)}"))
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                await _cartRepository.CreateAsync(key, entity, cancellationToken);
            });    
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        using (_diagnosticContext.Measure($"{nameof(CartRepositoryRetryDecorator)}.{nameof(RemoveAsync)}"))
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                await _cartRepository.RemoveAsync(key, cancellationToken);
            });    
        }
    }
}