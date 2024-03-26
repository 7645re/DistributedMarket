using Shared.DiagnosticContext;
using Shared.Messaging.Events.Category;

namespace Catalog.Messaging.Producers.CategoryEventProducer;

public class CategoryEventProducerDecorator : ICategoryEventProducer
{
    private readonly ICategoryEventProducer _categoryEventProducer;
    private readonly IDiagnosticContext _diagnosticContext;

    public CategoryEventProducerDecorator(
        ICategoryEventProducer categoryEventProducer,
        IDiagnosticContext diagnosticContext)
    {
        _categoryEventProducer = categoryEventProducer;
        _diagnosticContext = diagnosticContext;
    }

    public async Task ProduceCreateEventAsync(CategoryCreateEvent createEvent, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CategoryEventProducer)}.{nameof(ProduceCreateEventAsync)}"))
            await _categoryEventProducer.ProduceCreateEventAsync(createEvent, cancellationToken);
    }

    public async Task ProduceUpdateEventAsync(CategoryUpdateEvent updateEvent, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CategoryEventProducer)}.{nameof(ProduceUpdateEventAsync)}"))
            await _categoryEventProducer.ProduceUpdateEventAsync(updateEvent, cancellationToken);
    }

    public async Task ProduceDeleteEventAsync(CategoryDeleteEvent deleteEvent, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CategoryEventProducer)}.{nameof(ProduceDeleteEventAsync)}"))
            await _categoryEventProducer.ProduceDeleteEventAsync(deleteEvent, cancellationToken);
    }
}