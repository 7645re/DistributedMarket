using Catalog.API.Dto.Requests;
using Catalog.API.Mappers;
using Catalog.Domain.Services.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpPost]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int? categoryId,
        CancellationToken cancellationToken)
    {
        if (categoryId is not null)
        {
            var productsByCategory = await _productService
                .GetProductsByCategoryIdAsync(categoryId.Value, cancellationToken);
            return Ok(productsByCategory);
        }

        var products = await _productService.GetProductsAsync(cancellationToken);
        return Ok(products);
    }

    [HttpGet("products/{id:int}")]
    public async Task<IActionResult> GetProductById(int id, CancellationToken cancellationToken)
    {
        var products = await _productService.GetProductWithCategoriesAsync(id, cancellationToken);
        return Ok(products);
    }

    [HttpPost("products")]
    public async Task<IActionResult> CreateProduct(
        [FromBody] ProductCreate productCreate,
        CancellationToken cancellationToken)
    {
        var createdProduct = await _productService.CreateProductAsync(
            productCreate.ToProduct(),
            cancellationToken);
        return Ok(createdProduct);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProductById(
        int id,
        CancellationToken cancellationToken)
    {
        await _productService.DeleteProductByIdAsync(id, cancellationToken);
        return Ok();
    }
}