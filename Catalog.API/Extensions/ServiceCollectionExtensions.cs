using Catalog.Domain;
using Catalog.Domain.Options;
using Catalog.Domain.Repositories.Category;
using Catalog.Domain.Repositories.Product;
using Catalog.Domain.Repositories.ProductCategory;
using Catalog.Domain.Services.CategoryService;
using Catalog.Domain.Services.ProductService;
using Catalog.Domain.UnitOfWork;
using Catalog.Domain.Validators.Category;
using Catalog.Domain.Validators.Product;
using Catalog.Messaging.Producers.CategoryEventProducer;
using MassTransit;
using MassTransit.KafkaIntegration;
using Microsoft.EntityFrameworkCore;
using Shared.Messaging.Events.Category;
using Shared.Messaging.Events.Product;
using Shared.Messaging.Options;

namespace Catalog.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IProductService, ProductService>();
        serviceCollection.Decorate<IProductService, ProductServiceDecorator>();

        serviceCollection.AddTransient<ICategoryService, CategoryService>();
        serviceCollection.Decorate<ICategoryService, CategoryServiceDecorator>();

        return serviceCollection;
    }

    public static IServiceCollection AddUnitOfWork(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        return serviceCollection;
    }

    public static IServiceCollection AddValidators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IProductValidator, ProductValidator>();
        serviceCollection.Decorate<IProductValidator, ProductValidatorDecorator>();

        serviceCollection.AddScoped<ICategoryValidator, CategoryValidator>();
        serviceCollection.Decorate<ICategoryValidator, CategoryValidatorDecorator>();
        return serviceCollection;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        serviceCollection.AddScoped<IProductRepository, ProductRepository>();
        serviceCollection.AddScoped<ICategoryRepository, CategoryRepository>();
        return serviceCollection;
    }

    public static IServiceCollection AddDbContext(
        this IServiceCollection serviceCollection,
        WebApplicationBuilder builder)
    {
        return serviceCollection.AddDbContext<CatalogDbContext>(options =>
            options.UseSqlServer(builder
                .Configuration
                .GetSection("Database")
                .Get<DatabaseOptions>()
                ?.ConnectionString)
        );
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
                    r.AddProducer<Guid, ProductCreateEvent>(kafkaOptions.ProductCreateTopic);
                    r.AddProducer<Guid, ProductUpdateEvent>(kafkaOptions.ProductUpdateTopic);
                    r.AddProducer<Guid, ProductDeleteEvent>(kafkaOptions.ProductDeleteTopic);

                    r.AddProducer<Guid, CategoryCreateEvent>(kafkaOptions.CategoryCreateTopic);
                    r.AddProducer<Guid, CategoryUpdateEvent>(kafkaOptions.CategoryUpdateTopic);
                    r.AddProducer<Guid, CategoryDeleteEvent>(kafkaOptions.CategoryDeleteTopic);
                    
                    r.UsingKafka((context, cfg) =>
                    {
                        cfg.Host(kafkaOptions.GetHost());
                    });
                });
            });

        serviceCollection.AddScoped<ICategoryEventProducer, CategoryEventProducer>();
        serviceCollection.Decorate<ICategoryEventProducer, CategoryEventProducerDecorator>();
        
        return serviceCollection;
    }
}