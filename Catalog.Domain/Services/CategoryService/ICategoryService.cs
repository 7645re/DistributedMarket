using Catalog.Domain.Dto.Category;

namespace Catalog.Domain.Services.CategoryService;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken);

    Task<Category?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken);

    Task<Category> CreateCategoryAsync(
        CategoryCreate categoryCreate, CancellationToken cancellationToken);

    Task<Category> UpdateCategoryAsync(CategoryUpdate categoryUpdate, CancellationToken cancellationToken);

    Task DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken);
}