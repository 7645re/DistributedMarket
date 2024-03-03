using Carts.Domain.Models;
using Polly;
using Polly.Retry;

namespace Carts.Domain.Repositories.CartByProduct;

public class CartsByProductRepositoryRetryDecorator : ICartsByProductRepository
{
    private readonly ICartsByProductRepository _cartsByProductRepository;
    private readonly AsyncRetryPolicy _retryPolicy;

    public CartsByProductRepositoryRetryDecorator(
        ICartsByProductRepository cartsByProductRepository)
    {
        _cartsByProductRepository = cartsByProductRepository;
        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3, 
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, calculatedWaitDuration, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount} due to {exception.Message}." +
                                      $" Waiting for {calculatedWaitDuration}");
                });;
    }

    public async Task<CartsByProductIdEntity?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _retryPolicy.ExecuteAsync(async () => 
            await _cartsByProductRepository.GetByKeyAsync(key, cancellationToken));
    }

    public async Task CreateAsync(string key, CartsByProductIdEntity entity, CancellationToken cancellationToken = default)
    {
        await _retryPolicy.ExecuteAsync(async () =>
            await _cartsByProductRepository.CreateAsync(key, entity, cancellationToken));
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _retryPolicy.ExecuteAsync(async () =>
            await _cartsByProductRepository.RemoveAsync(key, cancellationToken));
    }
}