using Confluent.Kafka;

namespace Catalog.Kafka.Producer;

public class KafkaProducerConfig : ProducerConfig
{
    public string Topic { get; set; } = string.Empty;
}
