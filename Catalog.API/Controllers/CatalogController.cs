using Catalog.API.Dto.Requests;
using Catalog.API.Dto.Requests.Category;
using Catalog.API.Mappers;
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
        var createdProduct = await _productService.CreateProductAsync(productCreate.ToProduct(), cancellationToken);
        return Ok(createdProduct);
    }

    [HttpDelete("products/{id:int}")]
    public async Task<IActionResult> DeleteProduct(
        int id,
        CancellationToken cancellationToken)
    {
        await _productService.DeleteProductByIdAsync(id, cancellationToken);
        return Ok();
    }
    
    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetCategoriesAsync(cancellationToken);
        return Ok(categories);
    }
    
    [HttpGet("categories/{id:int}")]
    public async Task<IActionResult> GetCategoryById(
        int id,
        CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetCategoryByIdAsync(
            id,
            cancellationToken);
        return Ok(categories);
    }

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory(
        [FromBody] CategoryCreate categoryCreate,
        CancellationToken cancellationToken)
    {
        var category = await _categoryService.CreateCategoryAsync(
            categoryCreate.ToCategory(),
            cancellationToken);
        return Ok(category);
    }
    
    [HttpPatch("categories/{id:int}")]
    public async Task<IActionResult> UpdateCategoryById(
        int id,
        [FromBody] CategoryUpdate categoryUpdate,
        CancellationToken cancellationToken)
    {
        var category = await _categoryService.UpdateCategoryAsync(
            categoryUpdate.ToCategory(id),
            cancellationToken);
        return Ok(category);
    }

    [HttpDelete("categories/{id:int}")]
    public async Task<IActionResult> DeleteCategoryById(
        int id,
        CancellationToken cancellationToken)
    {
        await _categoryService.DeleteCategoryByIdAsync(id, cancellationToken);
        return Ok();
    }
}