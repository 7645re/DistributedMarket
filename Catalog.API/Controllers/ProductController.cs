using Catalog.API.Dto.Product;
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
        var products = await _productService.GetProductByIdAsync(id, cancellationToken);
        return Ok(products.ToProductGetResponse());
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(
        [FromBody] ProductCreateRequest productCreateRequest,
        CancellationToken cancellationToken)
    {
        var createdProduct = await _productService.CreateProductAsync(
            productCreateRequest.ToProductCreate(),
            cancellationToken);
        return Ok(createdProduct.ToProductCreateResponse());
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
        [FromBody] ProductUpdateRequest productUpdateRequest,
        CancellationToken cancellationToken)
    {
        var result = await _productService.UpdateProductAsync(
            productUpdateRequest.ToProductUpdate(id),
            cancellationToken);
        return Ok(result.ToProductUpdateResponse());
    }
}