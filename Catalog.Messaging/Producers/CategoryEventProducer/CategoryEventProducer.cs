using MassTransit.KafkaIntegration;
using Shared.Messaging.Events.Category;

namespace Catalog.Messaging.Producers.CategoryEventProducer;

public class CategoryEventProducer : ICategoryEventProducer
{
    private readonly ITopicProducer<Guid, CategoryCreateEvent> _categoryCreateProducer;
    private readonly ITopicProducer<Guid, CategoryUpdateEvent> _categoryUpdateProducer;
    private readonly ITopicProducer<Guid, CategoryDeleteEvent> _categoryDeleteProducer;
    
    public CategoryEventProducer(
        ITopicProducer<Guid, CategoryCreateEvent> categoryCreateProducer,
        ITopicProducer<Guid, CategoryUpdateEvent> categoryUpdateProducer,
        ITopicProducer<Guid, CategoryDeleteEvent> categoryDeleteProducer)
    {
        _categoryCreateProducer = categoryCreateProducer;
        _categoryUpdateProducer = categoryUpdateProducer;
        _categoryDeleteProducer = categoryDeleteProducer;
    }

    public async Task ProduceCreateEventAsync(CategoryCreateEvent createEvent, CancellationToken cancellationToken)
    {
        await _categoryCreateProducer.Produce(Guid.NewGuid(), createEvent, cancellationToken);
    }

    public async Task ProduceUpdateEventAsync(CategoryUpdateEvent updateEvent, CancellationToken cancellationToken)
    {
        await _categoryUpdateProducer.Produce(Guid.NewGuid(), updateEvent, cancellationToken);
    }

    public async Task ProduceDeleteEventAsync(CategoryDeleteEvent deleteEvent, CancellationToken cancellationToken)
    {
        await _categoryDeleteProducer.Produce(Guid.NewGuid(), deleteEvent, cancellationToken);
    }
}