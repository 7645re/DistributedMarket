using Microsoft.AspNetCore.Mvc;

namespace Carts.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CartsController : ControllerBase
{
    public CartsController()
    {
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}