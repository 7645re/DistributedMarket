namespace Catalog.Kafka;

public interface IKafkaMessageBus<TK, TV>
{
    Task PublishAsync(TK key, TV message, CancellationToken cancellationToken);
}