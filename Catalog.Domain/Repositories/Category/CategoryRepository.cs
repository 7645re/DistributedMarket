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
        using (DiagnosticContextStorage.Measure($"{nameof(CategoryRepository)}.{nameof(GetAllAsync)}"))
            return await Set
                .AsNoTracking()
                .ToListAsync(cancellationToken);
    }

    public async Task<IList<CategoryEntity>> GetByIdsAsync(
        IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        using (DiagnosticContextStorage.Measure($"{nameof(CategoryRepository)}.{nameof(GetByIdsAsync)}"))
            return await Set
                .AsNoTracking()
                .Where(c => ids.Contains(c.Id))
                .ToListAsync(cancellationToken);
    }
    
    public async Task<CategoryEntity?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        using (DiagnosticContextStorage.Measure($"{nameof(CategoryRepository)}.{nameof(GetByIdAsync)}"))
            return await Set
                .AsNoTracking()
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CategoryEntity?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken)
    {
        using (DiagnosticContextStorage.Measure($"{nameof(CategoryRepository)}.{nameof(GetByNameAsync)}"))
            return await Set
                .AsNoTracking()
                .Where(c => c.Name == name)
                .FirstOrDefaultAsync(cancellationToken);
    }

    public void DeleteById(int id)
    {
        Set.Remove(Set.First(c => c.Id == id));
    }
    
    public async Task<IEnumerable<CategoryEntity>> GetAllPagedAsync(
        int page, int pageSize, CancellationToken cancellationToken)
    {
        using (DiagnosticContextStorage.Measure($"{nameof(CategoryRepository)}.{nameof(GetAllPagedAsync)}"))
            return await Set
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
    }
}