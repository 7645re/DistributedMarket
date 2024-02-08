using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;

namespace Catalog.Domain.Repositories.ProductCategory;

public interface IProductCategoryRepository : IRepository<ProductEntityCategoryEntity>
{
    Task<List<ProductEntityCategoryEntity>> GetProductsCategoriesByCategoryIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task CreateProductsCategoriesAsync(
        IEnumerable<ProductEntityCategoryEntity> productEntityCategoryEntities,
        CancellationToken cancellationToken);
}