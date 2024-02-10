using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;

namespace Catalog.Domain.Repositories.Category;

public interface ICategoryRepository : IRepository<CategoryEntity>
{
    Task<IEnumerable<CategoryEntity>> GetByIdsAsync(
        IEnumerable<int> ids, CancellationToken cancellationToken);
}