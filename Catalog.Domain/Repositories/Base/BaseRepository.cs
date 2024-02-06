using Microsoft.EntityFrameworkCore;

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
    
    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Set.Add(entity);
        await Context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Set.Update(entity);
        await Context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Set.Remove(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Set.AsNoTracking().ToListAsync(cancellationToken);
    }
}