using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;

namespace Catalog.Domain.Repositories.Category;

public interface ICategoryRepository : IRepository<CategoryEntity>
{
    Task<IList<CategoryEntity>> GetByIdsAsync(
        IEnumerable<int> ids, CancellationToken cancellationToken);

    Task<CategoryEntity?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken);

    Task<CategoryEntity?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<IList<CategoryEntity>> GetAllAsync(CancellationToken cancellationToken);

    void DeleteById(int id);
}