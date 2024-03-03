using Carts.Domain.Options;
using Carts.Domain.Repositories.Cart;
using Carts.Domain.Repositories.CartByProduct;
using Carts.Domain.Services.CartService;
using Carts.Messaging.Consumers;
using MassTransit;
using Shared.Messaging.Events.Product;
using Shared.Messaging.Options;

namespace Carts.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ICartService, CartService>();
        serviceCollection.Decorate<ICartService, CartServiceDecorator>();
        return serviceCollection;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICartRepository, CartRepository>();
        serviceCollection.Decorate<ICartRepository, CartRepositoryMetricDecorator>();
        serviceCollection.Decorate<ICartRepository, CartRepositoryRetryDecorator>();
        
        serviceCollection.AddScoped<ICartsByProductRepository, CartsByProductRepository>();
        serviceCollection.Decorate<ICartsByProductRepository, CartsByProductRepositoryRetryDecorator>();
        serviceCollection.Decorate<ICartsByProductRepository, CartsByProductRepositoryMetricDecorator>();
        
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

        serviceCollection
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

        return serviceCollection;
    }

    public static IServiceCollection AddOptions(this IServiceCollection serviceCollection,
        WebApplicationBuilder builder)
    {
        serviceCollection.Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));
        return serviceCollection;
    }
}