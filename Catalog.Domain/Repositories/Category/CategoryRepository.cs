using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Domain.Repositories.Category;

public class CategoryRepository : BaseRepository<CategoryEntity>, ICategoryRepository
{
    public CategoryRepository(CatalogDbContext context) : base(context)
    {
    }

    public async Task<CategoryEntity?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await Set.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        await Set.Where(c => c.Id == id).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<List<CategoryEntity>> GetCategoriesByIdsAsync(
        IEnumerable<int> categoriesIds,
        CancellationToken cancellationToken)
    {
        return await Set
            .AsNoTracking()
            .Where(c => categoriesIds.Contains(c.Id))
            .ToListAsync(cancellationToken);
    }
}