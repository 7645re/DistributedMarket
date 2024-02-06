using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;

namespace Catalog.Domain.Repositories.Category;

public interface ICategoryRepository : IRepository<CategoryEntity>
{
    Task<CategoryEntity?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken);
}