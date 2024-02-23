using Catalog.Kafka.Producer;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Catalog.Kafka.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaMessageBus(this IServiceCollection serviceCollection)
        => serviceCollection.AddSingleton(typeof(IKafkaMessageBus<,>), typeof(KafkaMessageBus<,>));

    // public static IServiceCollection AddKafkaConsumer<Tk, Tv, THandler>(this IServiceCollection services,
    //     Action<KafkaConsumerConfig<Tk, Tv>> configAction) where THandler : class, IKafkaHandler<Tk, Tv>
    // {
    //     services.AddScoped<IKafkaHandler<Tk, Tv>, THandler>();
    //
    //     services.AddHostedService<BackGroundKafkaConsumer<Tk, Tv>>();
    //
    //     services.Configure(configAction);
    //
    //     return services;
    // }

    public static IServiceCollection AddKafkaProducer<TK, TV>(this IServiceCollection services,
        Action<KafkaProducerConfig> configAction)
    {
        services.AddConfluentKafkaProducer<TK, TV>();
        services.AddSingleton<KafkaProducer<TK, TV>>();
        services.Configure(configAction);

        return services;
    }

    private static IServiceCollection AddConfluentKafkaProducer<TK, TV>(this IServiceCollection services)
    {
        services.AddSingleton(
            sp =>
            {
                var config = sp.GetRequiredService<IOptions<KafkaProducerConfig>>();
                var builder = new ProducerBuilder<TK, TV>(config.Value).SetValueSerializer(new KafkaSerializer<TV>());
                return builder.Build();
            });

        return services;
    }
}
