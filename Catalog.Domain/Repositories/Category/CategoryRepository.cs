using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Shared.DiagnosticContext;

namespace Catalog.Domain.Repositories.Category;

public class CategoryRepository : BaseRepository<CategoryEntity>, ICategoryRepository
{
    public CategoryRepository(CatalogDbContext context, IDiagnosticContextStorage diagnosticContextStorage) 
        : base(context, diagnosticContextStorage)
    {
    }

    public async Task<IList<CategoryEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Set
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IList<CategoryEntity>> GetByIdsAsync(
        IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        return await Set
            .AsNoTracking()
            .Where(c => ids.Contains(c.Id))
            .ToListAsync(cancellationToken);
    }
    
    public async Task<CategoryEntity?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await Set
            .AsNoTracking()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CategoryEntity?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken)
    {
        return await Set
            .AsNoTracking()
            .Where(c => c.Name == name)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void DeleteById(int id)
    {
        Set.Remove(Set.First(c => c.Id == id));
    }
}