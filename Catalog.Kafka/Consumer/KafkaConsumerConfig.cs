using Confluent.Kafka;

namespace Catalog.Kafka.Consumer;

public class KafkaConsumerConfig<TK, TV> : ConsumerConfig
{
    public string Topic { get; set; }
    public KafkaConsumerConfig()
    {
        AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
        EnableAutoOffsetStore = false;
    }
}