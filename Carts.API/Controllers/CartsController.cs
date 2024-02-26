using Carts.API.Dto;
using Carts.API.Mappers;
using Carts.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Carts.API.Controllers;

[ApiController]
[Route("carts")]
public class CartsController : ControllerBase
{
    private readonly ICartService _cartService;
    
    public CartsController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var result = await _cartService.GetCartAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateCart([FromBody] CartUpdateRequest cartUpdateRequest, int id)
    {
        await _cartService.UpdateCartAsync(cartUpdateRequest.ToCartUpdate(id), CancellationToken.None);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCart([FromBody] CartCreateRequest cartCreateRequest)
    {
        await _cartService.CreateCartAsync(cartCreateRequest.ToCartCreate(), CancellationToken.None);
        return Ok();
    }
}