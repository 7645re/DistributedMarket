using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Domain.Repositories.ProductCategoryRepository;

public class ProductCategoryRepository : BaseRepository<ProductEntityCategoryEntity>,
    IProductCategoryRepository
{
    public ProductCategoryRepository(CatalogDbContext context) : base(context)
    {
    }

    public async Task<List<ProductEntityCategoryEntity>> GetProductsCategoriesByCategoryIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await Set
            .AsNoTracking()
            .Where(pc => pc.CategoryId == id)
            .ToListAsync(cancellationToken);
    }

    public async Task CreateProductsCategoriesAsync(
        IEnumerable<ProductEntityCategoryEntity> productEntityCategoryEntities,
        CancellationToken cancellationToken)
    {
        await Set.AddRangeAsync(productEntityCategoryEntities, cancellationToken);
    }
}