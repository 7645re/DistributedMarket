using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;
using Catalog.Domain.Repositories.ProductCategory;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Domain.Repositories.Product;

public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    
    public ProductRepository(CatalogDbContext context) : base(context)
    {
        _productCategoryRepository = new ProductCategoryRepository(context);
    }

    public async Task<ProductEntity?> GetProductByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await Set
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Include(p => p.Categories)
            .SingleAsync(cancellationToken);
    }

    public async Task<List<ProductEntity>> GetProductByIdWithCategoriesAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await Set
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Include(p => p.Categories)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteProductByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        await Set.Where(p => p.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
}