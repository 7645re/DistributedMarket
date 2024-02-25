using Customers.Domain.Dto.Customer;

namespace Customers.Domain.Services.CustomerService;

public interface ICustomerService
{
    Task<Customer> GetCustomerAsync(int id, CancellationToken cancellationToken);
}