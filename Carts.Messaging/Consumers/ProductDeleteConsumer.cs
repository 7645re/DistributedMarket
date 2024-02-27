using Carts.Domain.Services.CartService;
using Catalog.Messaging.Events.Product;
using MassTransit;

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