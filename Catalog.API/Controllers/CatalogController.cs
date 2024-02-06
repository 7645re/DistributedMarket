using Catalog.Domain.Services.CategoryService;
using Catalog.Domain.Services.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private readonly IProductService _productService;

    private readonly ICategoryService _categoryService;

    public CatalogController(
        IProductService productService,
        ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts([FromQuery] int? categoryId, CancellationToken cancellationToken)
    {
        if (categoryId is not null)
        {
            var productsByCategory = await _productService
                .GetProductsByCategoryIdAsync((int) categoryId, cancellationToken);
            return Ok(productsByCategory);
        }

        var products = await _productService.GetProductsAsync(cancellationToken);
        return Ok(products);
    }

    [HttpGet("products/{id:int}")]
    public async Task<IActionResult> GetProduct(int id, CancellationToken cancellationToken)
    {
        var products = await _productService.GetProductWithCategoriesAsync(id, cancellationToken);
        return Ok(products);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetCategoriesAsync(cancellationToken);
        return Ok(categories);
    }
    
    [HttpGet("categories/{id:int}")]
    public async Task<IActionResult> GetCategoryById(int id, CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);
        return Ok(categories);
    }
}