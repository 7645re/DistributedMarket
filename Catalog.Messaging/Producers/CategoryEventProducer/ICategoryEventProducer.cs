using Shared.Messaging.Events.Category;

namespace Catalog.Messaging.Producers.CategoryEventProducer;

public interface ICategoryEventProducer
{
    Task ProduceCreateEventAsync(CategoryCreateEvent createEvent, CancellationToken cancellationToken);
    Task ProduceUpdateEventAsync(CategoryUpdateEvent updateEvent, CancellationToken cancellationToken);
    Task ProduceDeleteEventAsync(CategoryDeleteEvent deleteEvent, CancellationToken cancellationToken);
}