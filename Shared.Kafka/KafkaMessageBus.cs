using Catalog.Kafka.Producer;

namespace Shared.Kafka;

public class KafkaMessageBus<TK, TV> : IKafkaMessageBus<TK, TV>
{
    public readonly KafkaProducer<TK, TV> Producer;
    public KafkaMessageBus(KafkaProducer<TK, TV> producer)
    {
        Producer = producer;
    }
    public async Task PublishAsync(TK key, TV message, CancellationToken cancellationToken)
    {
        await Producer.ProduceAsync(key, message, cancellationToken);
    }
}