using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Domain.Repositories.Category;

public class CategoryRepository : BaseRepository<CategoryEntity>, ICategoryRepository
{
    public CategoryRepository(CatalogDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CategoryEntity>> GetByIdsAsync(
        IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        return await Set
            .AsNoTracking()
            .Where(c => ids.Contains(c.Id))
            .ToListAsync(cancellationToken);
    }
}