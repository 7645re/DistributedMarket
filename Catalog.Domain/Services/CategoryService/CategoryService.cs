using Catalog.Domain.Dto;
using Catalog.Domain.Mappers;
using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Category;

namespace Catalog.Domain.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return categories.ToCategories();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(id, cancellationToken);
        return category?.ToCategory();
    }
}