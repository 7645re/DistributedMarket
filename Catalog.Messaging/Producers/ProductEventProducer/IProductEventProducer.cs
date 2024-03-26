using Shared.Messaging.Events.Product;

namespace Catalog.Messaging.Producers.ProductEventProducer;

public interface IProductEventProducer
{
    Task ProduceCreateEventAsync(ProductCreateEvent createEvent, CancellationToken cancellationToken);
    Task ProduceUpdateEventAsync(ProductUpdateEvent updateEvent, CancellationToken cancellationToken);
    Task ProduceDeleteEventAsync(ProductDeleteEvent deleteEvent, CancellationToken cancellationToken);
}