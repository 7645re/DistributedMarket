using Microsoft.EntityFrameworkCore;
using Shared.DiagnosticContext;

namespace Catalog.Domain.Repositories.Base;

public abstract class BaseRepository<TEntity> where TEntity : class
{
    protected CatalogDbContext Context { get; set; }

    protected DbSet<TEntity> Set { get; set; }
    
    protected IDiagnosticContextStorage DiagnosticContextStorage { get; set; }
    
    protected BaseRepository(
        CatalogDbContext context,
        IDiagnosticContextStorage diagnosticContextStorage)
    {
        DiagnosticContextStorage = diagnosticContextStorage;
        using (DiagnosticContextStorage.Measure($"{GetType().Name}.ctor"))
        {
            Context = context;
            Set = Context.Set<TEntity>();
        }
    }
    
    public TEntity Add(TEntity entity)
    {
        using (DiagnosticContextStorage.Measure($"{GetType().Name}.{nameof(Add)}"))
        {
            Set.Add(entity);
            return entity;
        }
    }

    public TEntity Update(TEntity entity)
    {
        using (DiagnosticContextStorage.Measure($"{GetType().Name}.{nameof(Update)}"))
        {
            Set.Update(entity);
            return entity;
        }
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        using (DiagnosticContextStorage.Measure($"{GetType().Name}.{nameof(AddRange)}"))
        {
            Set.AddRange(entities);
        }
    }

    public void Remove(TEntity entity)
    {
        using (DiagnosticContextStorage.Measure($"{GetType().Name}.{nameof(Remove)}"))
        {
            Set.Remove(entity);
        }
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        using (DiagnosticContextStorage.Measure($"{GetType().Name}.{nameof(RemoveRange)}"))
        {
            Set.RemoveRange(entities);
        }
    }
}