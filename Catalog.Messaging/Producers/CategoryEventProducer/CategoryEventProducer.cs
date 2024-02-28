using Catalog.Messaging.Events.Category;
using MassTransit.KafkaIntegration;
using Shared.DiagnosticContext;

namespace Catalog.Messaging.Producers.CategoryEventProducer;

public class CategoryEventProducer : ICategoryEventProducer
{
    private readonly ITopicProducer<Guid, CategoryCreateEvent> _categoryCreateProducer;
    private readonly ITopicProducer<Guid, CategoryUpdateEvent> _categoryUpdateProducer;
    private readonly ITopicProducer<Guid, CategoryDeleteEvent> _categoryDeleteProducer;
    private readonly IDiagnosticContextStorage _diagnosticContextStorage;
    
    public CategoryEventProducer(
        ITopicProducer<Guid, CategoryCreateEvent> categoryCreateProducer,
        ITopicProducer<Guid, CategoryUpdateEvent> categoryUpdateProducer,
        ITopicProducer<Guid, CategoryDeleteEvent> categoryDeleteProducer,
        IDiagnosticContextStorage diagnosticContextStorage)
    {
        _categoryCreateProducer = categoryCreateProducer;
        _categoryUpdateProducer = categoryUpdateProducer;
        _categoryDeleteProducer = categoryDeleteProducer;
        _diagnosticContextStorage = diagnosticContextStorage;
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