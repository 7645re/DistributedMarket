using Customers.API.Options;
using Customers.Domain;
using Customers.Domain.Services.CustomerService;
using Microsoft.EntityFrameworkCore;

namespace Customers.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ICustomerService, CustomerService>();
        return serviceCollection;
    }
    
    public static IServiceCollection AddDbContext(
        this IServiceCollection serviceCollection,
        WebApplicationBuilder builder)
    {
        return serviceCollection.AddDbContext<CustomerDbContext>(options =>
            options.UseSqlServer(builder
                .Configuration
                .GetSection("Database")
                .Get<DatabaseOptions>()
                ?.ConnectionString)
        );
    }
}