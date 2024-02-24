using Catalog.API.Options;
using Catalog.Domain;
using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Category;
using Catalog.Domain.Repositories.Product;
using Catalog.Domain.Repositories.ProductCategory;
using Catalog.Domain.Services.CategoryService;
using Catalog.Domain.Services.ProductService;
using Catalog.Domain.UnitOfWork;
using Catalog.Domain.Validators.Category;
using Catalog.Domain.Validators.Product;
using MassTransit;
using MassTransit.KafkaIntegration;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IProductService, ProductService>();
        serviceCollection.AddTransient<ICategoryService, CategoryService>();
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
        serviceCollection.AddScoped<ICategoryValidator, CategoryValidator>();
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

    public static IServiceCollection AddKafka(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddMassTransitHostedService()
            .AddMassTransit(x =>
            {
                x.UsingInMemory((context,cfg) => cfg.ConfigureEndpoints(context));
                
                x.AddRider(r =>
                {
                    r.AddProducer<ProductEntity>("ProductEntity");
                    r.UsingKafka((context, cfg) =>
                    {
                        cfg.Host("localhost:9092");
                    });
                });
            });
    }
}