using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Domain.Repositories.Product;

public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
{
    public ProductRepository(CatalogDbContext context) : base(context)
    {
    }

    public async Task<ProductEntity?> GetProductByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await Set
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
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
}