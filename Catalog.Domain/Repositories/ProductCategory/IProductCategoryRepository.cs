using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;

namespace Catalog.Domain.Repositories.ProductCategory;

public interface IProductCategoryRepository : IRepository<ProductEntityCategoryEntity>
{
    void DeleteByProductId(int id, CancellationToken cancellationToken);

    void DeleteByProductIdAndCategoriesIds(
        int productId,
        int[] categoriesId,
        CancellationToken cancellationToken);

    Task<List<ProductEntityCategoryEntity>> GetProductCategoryByCategoryId(
        int categoryId,
        CancellationToken cancellationToken);
}