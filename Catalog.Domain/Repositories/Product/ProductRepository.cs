using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;
using Catalog.Domain.Repositories.ProductCategory;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Domain.Repositories.Product;

public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
{
    private readonly IProductCategoryRepository _productCategoryRepository;

    public ProductRepository(CatalogDbContext context,
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
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ProductEntity?> GetByIdWithCategoriesAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await Set
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ProductEntity?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken)
    {
        return await Set
            .AsNoTracking()
            .Where(p => p.Name == name)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<ProductEntityCategoryEntity>> GetByCategoryIdAsync(
        int categoryId, CancellationToken cancellationToken)
    {
        return await Context
            .ProductCategory
            .Where(x => x.CategoryId == categoryId)
            .Include(x => x.Product)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<ProductEntityCategoryEntity>> GetByCategoriesIdsAsync(
        IEnumerable<int> categoryId, CancellationToken cancellationToken)
    {
        return await Context
            .ProductCategory
            .Where(x => categoryId.Contains(x.CategoryId))
            .Include(x => x.Product)
            .ToListAsync(cancellationToken);
    }
}