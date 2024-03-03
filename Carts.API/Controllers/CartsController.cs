using Carts.API.Dto;
using Carts.API.Mappers;
using Carts.Domain.Services.CartService;
using Microsoft.AspNetCore.Mvc;
using Shared.DiagnosticContext;

namespace Carts.API.Controllers;

[ApiController]
[Route("carts")]
public class CartsController : ControllerBase
{
    private readonly ICartService _cartService;
    
    private readonly IDiagnosticContext _diagnosticContext;
    
    public CartsController(
        ICartService cartService,
        IDiagnosticContext diagnosticContext)
    {
        _cartService = cartService;
        _diagnosticContext = diagnosticContext;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CartsController)}.{nameof(Get)}"))
        {
            var result = await _cartService.GetCartAsync(id, cancellationToken);
            return Ok(result);    
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateCart([FromBody] CartUpdateRequest cartUpdateRequest, int id,
        CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CartsController)}.{nameof(UpdateCart)}"))
        {
            await _cartService.UpdateCartAsync(cartUpdateRequest.ToCartUpdate(id), cancellationToken);
            return Ok();    
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCart([FromBody] CartCreateRequest cartCreateRequest,
        CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CartsController)}.{nameof(CreateCart)}"))
        {
            await _cartService.CreateCartAsync(cartCreateRequest.ToCartCreate(), cancellationToken);
            return Ok();    
        }
    }
}