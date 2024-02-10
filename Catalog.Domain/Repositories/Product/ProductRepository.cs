using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Domain.Repositories.Product;

public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
{
    public ProductRepository(CatalogDbContext context) : base(context)
    {
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
}