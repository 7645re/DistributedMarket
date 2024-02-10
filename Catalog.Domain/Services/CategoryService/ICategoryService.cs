using Catalog.Domain.Dto;
using Catalog.Domain.Dto.Category;

namespace Catalog.Domain.Services.CategoryService;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken);

    Task<Category?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken);

    Task<Category> CreateCategoryAsync(Category category, CancellationToken cancellationToken);

    Task<Category> UpdateCategoryAsync(Category category, CancellationToken cancellationToken);

    Task DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken);
}