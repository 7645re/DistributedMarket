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
}