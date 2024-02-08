
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Catalog.Domain.Repositories.Base;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity Add(TEntity entity);

    TEntity Update(TEntity entity);

    EntityEntry<TEntity> Delete(TEntity entity);

    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    void AddRange(IEnumerable<TEntity> entities);
}