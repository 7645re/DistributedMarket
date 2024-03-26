using Catalog.API.Dto.Product;
using Catalog.API.Mappers;
using Catalog.Domain.Dto.Product;
using Catalog.Domain.Repositories;
using Catalog.Domain.Services.ProductService;
using Microsoft.AspNetCore.Mvc;
using Shared.DiagnosticContext;

namespace Catalog.API.Controllers;

[ApiController]
[Route("products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    
    private readonly IDiagnosticContext _diagnosticContext;
    
    public ProductController(
        IProductService productService,
        IDiagnosticContext diagnosticContext)
    {
        _productService = productService;
        _diagnosticContext = diagnosticContext;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<Product>>> GetProducts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        using (_diagnosticContext.Measure($"{nameof(ProductController)}.{nameof(GetProducts)}"))
        {
            var products = await _productService.GetAllPagedAsync(page, pageSize, cancellationToken);
            return Ok(products);
        }
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductById(int id, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(ProductController)}.{nameof(GetProductById)}"))
        {
            var products = await _productService.GetProductByIdAsync(id, cancellationToken);
            return Ok(products.ToProductGetResponse());
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(
        [FromBody] ProductCreateRequest productCreateRequest,
        CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(ProductController)}.{nameof(CreateProduct)}"))
        {
            var createdProduct = await _productService.CreateProductAsync(
                productCreateRequest.ToProductCreate(),
                cancellationToken);
            return Ok(createdProduct.ToProductCreateResponse());
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProductById(
        int id,
        CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(ProductController)}.{nameof(DeleteProductById)}"))
        {
            await _productService.DeleteProductByIdAsync(id, cancellationToken);
            return Ok();
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProductById(
        int id,
        [FromBody] ProductUpdateRequest productUpdateRequest,
        CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(ProductController)}.{nameof(UpdateProductById)}"))
        {
            var result = await _productService.UpdateProductAsync(
                productUpdateRequest.ToProductUpdate(id),
                cancellationToken);
            return Ok(result.ToProductUpdateResponse());
        }
    }
}