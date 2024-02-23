using Customers.Domain.Services.CustomerService;
using Microsoft.AspNetCore.Mvc;

namespace Customers.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    
    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var result = await _customerService.GetCustomerAsync(id, cancellationToken);
        return Ok(result);
    }
}