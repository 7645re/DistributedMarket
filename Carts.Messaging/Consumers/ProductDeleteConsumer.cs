using Carts.Domain.Services.CartService;
using MassTransit;
using Shared.Messaging.Events.Product;

namespace Carts.Messaging.Consumers;

public class ProductDeleteConsumer : IConsumer<ProductDeleteEvent>
{
    private readonly ICartService _cartService;

    public ProductDeleteConsumer(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task Consume(ConsumeContext<ProductDeleteEvent> context)
    {
        await _cartService.DeleteDependencyProductAsync(context.Message.Id, CancellationToken.None);
    }
}