using Catalog.Domain.Dto;

namespace Catalog.Domain.Services.CategoryService;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken);

    Task<Category?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken);
}