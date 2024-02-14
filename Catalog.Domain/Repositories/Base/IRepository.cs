namespace Catalog.Domain.Repositories.Base;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity Add(TEntity entity);

    TEntity Update(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);
}