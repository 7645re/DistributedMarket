using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Catalog.Domain.Repositories.Base;

public abstract class BaseRepository<TEntity> where TEntity : class
{
    protected CatalogDbContext Context { get; set; }

    protected DbSet<TEntity> Set { get; set; }
    
    protected BaseRepository(CatalogDbContext context)
    {
        Context = context;
        Set = Context.Set<TEntity>();
    }
    
    public TEntity Add(TEntity entity)
    {
        Set.Add(entity);
        return entity;
    }
    
    public void AddRange(IEnumerable<TEntity> entities)
    {
        Set.AddRange(entities);
    }

    public TEntity Update(TEntity entity)
    {
        Set.Update(entity);
        return entity;
    }

    public EntityEntry<TEntity> Delete(TEntity entity)
    {
        return Set.Remove(entity);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Set.AsNoTracking().ToListAsync(cancellationToken);
    }
}