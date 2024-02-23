using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Catalog.Kafka.Producer;

public class KafkaProducer<TK, TV> : IDisposable
{
    private readonly IProducer<TK, TV> _producer;
    private readonly string _topic;

    public KafkaProducer(IOptions<KafkaProducerConfig> topicOptions, IProducer<TK, TV> producer)
    {
        _topic = topicOptions.Value.Topic;
        _producer = producer;
    }

    public async Task ProduceAsync(TK key, TV value, CancellationToken cancellationToken)
    {
        await _producer.ProduceAsync(_topic, new Message<TK, TV> { Key = key, Value = value }, cancellationToken);
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}