using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Domain.Repositories.ProductCategory;

public class ProductCategoryRepository : BaseRepository<ProductEntityCategoryEntity>,
    IProductCategoryRepository
{
    public ProductCategoryRepository(CatalogDbContext context) : base(context)
    {
    }

    public void DeleteByProductId(int id, CancellationToken cancellationToken)
    {
        Set.RemoveRange(Set.Where(c => c.ProductId == id));
    }

    public void DeleteByProductIdAndCategoriesIds(
        int productId,
        int[] categoriesId,
        CancellationToken cancellationToken)
    {
        Set.RemoveRange(Set
            .Where(c =>
                c.ProductId == productId
                && categoriesId.Contains(c.CategoryId)));
    }
}