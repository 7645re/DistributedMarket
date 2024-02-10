using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;
using Catalog.Domain.Repositories.ProductCategory;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Domain.Repositories.Product;

public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
{
    private readonly IProductCategoryRepository _productCategoryRepository;

    public ProductRepository(
        CatalogDbContext context,
        IProductCategoryRepository productCategoryRepository) : base(context)
    {
        _productCategoryRepository = productCategoryRepository;
    }

    public async Task<ProductEntity?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await Set
            .AsNoTracking()
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync(cancellationToken);
    }
}