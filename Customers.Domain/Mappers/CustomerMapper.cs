using Customers.Domain.Dto.Customer;
using Customers.Domain.Models;

namespace Customers.Domain.Mappers;

public static class CustomerMapper
{
    public static Customer ToCustomer(this CustomerEntity customerEntity)
    {
        return new Customer
        {
            Id = customerEntity.Id,
            Name = customerEntity.Name,
            Email = customerEntity.Email
        };
    }
}