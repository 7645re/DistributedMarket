using Carts.Domain.Options;
using Carts.Domain.Repositories.Cart;
using Carts.Domain.Repositories.CartByProduct;
using Carts.Domain.Services.CartService;
using Carts.Messaging.Consumers;
using Catalog.Messaging.Events.Product;
using Catalog.Messaging.Options;
using MassTransit;

namespace Carts.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ICartService, CartService>();
        return serviceCollection;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICartRepository, CartRepository>();
        serviceCollection.AddScoped<ICartsByProductRepository, CartsByProductRepository>();
        return serviceCollection;
    }
    
    public static IServiceCollection AddRedis(this IServiceCollection serviceCollection,
        WebApplicationBuilder builder)
    {
        var redisOptions = builder.Configuration.GetRequiredSection("Redis").Get<RedisOptions>()!;
        serviceCollection.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = redisOptions.ConnectionString;
        });
        return serviceCollection;
    }
    
    public static IServiceCollection AddKafka(this IServiceCollection serviceCollection,
        WebApplicationBuilder builder)
    {
        var kafkaOptions = builder
            .Configuration
            .GetSection("Kafka")
            .Get<KafkaOptions>()!;

        return serviceCollection
            .AddMassTransitHostedService()
            .AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingInMemory();
                
                x.AddRider(r =>
                {
                    r.AddConsumer<ProductDeleteConsumer>();
                    
                    r.UsingKafka((context, cfg) =>
                    {
                        cfg.TopicEndpoint<Guid, ProductDeleteEvent>(
                            kafkaOptions.ProductDeleteTopic,
                            "consumer-group-1", e =>
                            {
                                e.ConfigureConsumer<ProductDeleteConsumer>(context);
                            });
                        
                        cfg.Host(kafkaOptions.GetHost());
                    });
                });
            });
    }

    public static IServiceCollection AddOptions(this IServiceCollection serviceCollection,
        WebApplicationBuilder builder)
    {
        serviceCollection.Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));
        return serviceCollection;
    }
}