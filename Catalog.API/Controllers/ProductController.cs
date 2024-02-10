using Catalog.API.Dto.Requests;
using Catalog.API.Dto.Requests.Product;
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductById(int id, CancellationToken cancellationToken)
    {
        var products = await _productService.GetProductWithCategoriesByIdAsync(id, cancellationToken);
        return Ok(products);
    }

    [HttpPost]
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

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProductById(
        int id,
        [FromBody] ProductUpdate productUpdate,
        CancellationToken cancellationToken)
    {
        var result = await _productService.UpdateProductByIdAsync(
            productUpdate.ToProduct(id),
            cancellationToken);
        
        return Ok(result);
    }
}