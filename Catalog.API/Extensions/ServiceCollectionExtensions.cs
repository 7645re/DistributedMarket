using Catalog.Domain;
using Catalog.Domain.Repositories.Category;
using Catalog.Domain.Repositories.Product;
using Catalog.Domain.Repositories.ProductCategoryRepository;
using Catalog.Domain.Services.CategoryService;
using Catalog.Domain.Services.ProductService;
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

    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
        serviceCollection.AddTransient<IProductRepository, ProductRepository>();
        serviceCollection.AddTransient<ICategoryRepository, CategoryRepository>();
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