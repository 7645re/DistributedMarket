using Shared.DiagnosticContext;
using Shared.Messaging.Events.Product;

namespace Catalog.Messaging.Producers.ProductEventProducer;

public class ProductEventProducerDecorator : IProductEventProducer
{
    private readonly IProductEventProducer _productEventProducer;
    
    private readonly IDiagnosticContext _diagnosticContext;

    public ProductEventProducerDecorator(
        IProductEventProducer productEventProducer,
        IDiagnosticContext diagnosticContext)
    {
        _productEventProducer = productEventProducer;
        _diagnosticContext = diagnosticContext;
    }

    public async Task ProduceCreateEventAsync(ProductCreateEvent createEvent, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(ProductEventProducer)}.{nameof(ProduceCreateEventAsync)}"))
            await _productEventProducer.ProduceCreateEventAsync(createEvent, cancellationToken); 
    }

    public async Task ProduceUpdateEventAsync(ProductUpdateEvent updateEvent, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(ProductEventProducer)}.{nameof(ProduceUpdateEventAsync)}"))
            await _productEventProducer.ProduceUpdateEventAsync(updateEvent, cancellationToken); 
    }

    public async Task ProduceDeleteEventAsync(ProductDeleteEvent deleteEvent, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(ProductEventProducer)}.{nameof(ProduceDeleteEventAsync)}"))
            await _productEventProducer.ProduceDeleteEventAsync(deleteEvent, cancellationToken);
    }
}