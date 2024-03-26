using Carts.Domain.Services.CartService;
using MassTransit;
using Shared.DiagnosticContext;
using Shared.Messaging.Events.Product;

namespace Carts.Messaging.Consumers;

public class ProductDeleteConsumer : IConsumer<ProductDeleteEvent>
{
    private readonly ICartService _cartService;
    
    private readonly IDiagnosticContext _diagnosticContext;

    public ProductDeleteConsumer(
        ICartService cartService,
        IDiagnosticContext diagnosticContext)
    {
        _cartService = cartService;
        _diagnosticContext = diagnosticContext;
    }

    public async Task Consume(ConsumeContext<ProductDeleteEvent> context)
    {
        using (_diagnosticContext.Measure($"{nameof(ProductDeleteConsumer)}.{nameof(Consume)}"))
            await _cartService.DeleteDependencyProductAsync(context.Message.Id, CancellationToken.None);
    }
}