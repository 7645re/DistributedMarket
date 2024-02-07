using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;

namespace Catalog.Domain.Repositories.Category;

public interface ICategoryRepository : IRepository<CategoryEntity>
{
    Task<CategoryEntity?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken);

    Task DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken);

    Task<List<CategoryEntity>> GetCategoriesByIdsAsync(IEnumerable<int> categoriesIds,
        CancellationToken cancellationToken);
}