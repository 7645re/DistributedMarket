using Customers.Domain.Dto.Customer;
using Customers.Domain.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Customers.Domain.Services.CustomerService;

public class CustomerService : ICustomerService
{
    private readonly CustomerDbContext _customerDbContext;

    public CustomerService(CustomerDbContext customerDbContext)
    {
        _customerDbContext = customerDbContext;
    }

    public async Task<Customer> GetCustomerAsync(int id, CancellationToken cancellationToken)
    {
        var customer = await _customerDbContext
            .Customers
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (customer == null)
            throw new ArgumentException($"Customer with id {id} not found");

        return customer.ToCustomer();
    }
}