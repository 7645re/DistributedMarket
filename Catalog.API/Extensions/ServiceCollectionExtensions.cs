using Catalog.Domain;
using Catalog.Domain.Repositories.Category;
using Catalog.Domain.Repositories.Product;
using Catalog.Domain.Repositories.ProductCategory;
using Catalog.Domain.Services.CategoryService;
using Catalog.Domain.Services.ProductService;
using Catalog.Domain.UnitOfWork;
using Catalog.Domain.Validators.Product;
using Catalog.Migrator.Options;
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
        serviceCollection.AddScoped<IProductValidator, IProductValidator>();
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
}