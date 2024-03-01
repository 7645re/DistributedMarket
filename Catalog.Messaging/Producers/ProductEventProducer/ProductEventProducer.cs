using MassTransit.KafkaIntegration;
using Shared.Messaging.Events.Product;

namespace Catalog.Messaging.Producers.ProductEventProducer;

public class ProductEventProducer : IProductEventProducer
{
    private readonly ITopicProducer<Guid, ProductCreateEvent> _productCreateProducer;
    private readonly ITopicProducer<Guid, ProductUpdateEvent> _productUpdateProducer;
    private readonly ITopicProducer<Guid, ProductDeleteEvent> _productDeleteProducer;

    public ProductEventProducer(
        ITopicProducer<Guid, ProductCreateEvent> productCreateProducer,
        ITopicProducer<Guid, ProductUpdateEvent> productUpdateProducer,
        ITopicProducer<Guid, ProductDeleteEvent> productDeleteProducer)
    {
        _productCreateProducer = productCreateProducer;
        _productUpdateProducer = productUpdateProducer;
        _productDeleteProducer = productDeleteProducer;
    }

    public async Task ProduceCreateEventAsync(ProductCreateEvent createEvent, CancellationToken cancellationToken)
    {
        await _productCreateProducer.Produce(Guid.NewGuid(), createEvent, cancellationToken);
    }

    public async Task ProduceUpdateEventAsync(ProductUpdateEvent updateEvent, CancellationToken cancellationToken)
    {
        await _productUpdateProducer.Produce(Guid.NewGuid(), updateEvent, cancellationToken);
    }

    public async Task ProduceDeleteEventAsync(ProductDeleteEvent deleteEvent, CancellationToken cancellationToken)
    {
        await _productDeleteProducer.Produce(Guid.NewGuid(), deleteEvent, cancellationToken);
    }
}