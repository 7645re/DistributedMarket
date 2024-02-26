using Catalog.Messaging.Events.Product;
using MassTransit;

namespace Carts.Messaging.Consumers;

public class ProductCreateConsumer : IConsumer<ProductCreateEvent>
{
    public Task Consume(ConsumeContext<ProductCreateEvent> context)
    {
        throw new NotImplementedException();
    }
}