using Confluent.Kafka;

namespace Catalog.Kafka.Producer;

public class KafkaProducerConfig<TK, TV> : ProducerConfig
{
    public string Topic { get; set; } = string.Empty;
}
