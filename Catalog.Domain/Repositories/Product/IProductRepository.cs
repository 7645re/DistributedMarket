using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;

namespace Catalog.Domain.Repositories.Product;

public interface IProductRepository : IRepository<ProductEntity>
{
    Task<ProductEntity?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<ProductEntity?> GetByIdWithCategoriesAsync(
        int id,
        CancellationToken cancellationToken);

    Task<ProductEntity?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken);

    Task<List<ProductEntityCategoryEntity>> GetByCategoryIdAsync(
        int categoryId, CancellationToken cancellationToken);
    
    Task<List<ProductEntityCategoryEntity>> GetByCategoriesIdsAsync(
        IEnumerable<int> categoryId, CancellationToken cancellationToken);

    Task<int> GetTotalCountAsync(CancellationToken cancellationToken);

    void DeleteById(int id);

    Task<IEnumerable<ProductEntity>> GetAllPagedAsync(
        int page, int pageSize, CancellationToken cancellationToken);
}