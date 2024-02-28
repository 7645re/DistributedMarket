using Shared.DiagnosticContext;
using Shared.Messaging.Events.Category;

namespace Catalog.Messaging.Producers.CategoryEventProducer;

public class CategoryEventProducerDecorator : ICategoryEventProducer
{
    private readonly ICategoryEventProducer _categoryEventProducer;
    private readonly IDiagnosticContextStorage _diagnosticContextStorage;

    public CategoryEventProducerDecorator(
        ICategoryEventProducer categoryEventProducer,
        IDiagnosticContextStorage diagnosticContextStorage)
    {
        _categoryEventProducer = categoryEventProducer;
        _diagnosticContextStorage = diagnosticContextStorage;
    }

    public async Task ProduceCreateEventAsync(CategoryCreateEvent createEvent, CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure($"{nameof(CategoryEventProducer)}.{nameof(ProduceCreateEventAsync)}"))
            await _categoryEventProducer.ProduceCreateEventAsync(createEvent, cancellationToken);
    }

    public async Task ProduceUpdateEventAsync(CategoryUpdateEvent updateEvent, CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure($"{nameof(CategoryEventProducer)}.{nameof(ProduceUpdateEventAsync)}"))
            await _categoryEventProducer.ProduceUpdateEventAsync(updateEvent, cancellationToken);
    }

    public async Task ProduceDeleteEventAsync(CategoryDeleteEvent deleteEvent, CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure($"{nameof(CategoryEventProducer)}.{nameof(ProduceDeleteEventAsync)}"))
            await _categoryEventProducer.ProduceDeleteEventAsync(deleteEvent, cancellationToken);
    }
}