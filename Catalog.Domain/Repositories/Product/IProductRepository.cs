using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;

namespace Catalog.Domain.Repositories.Product;

public interface IProductRepository : IRepository<ProductEntity>
{
    Task<ProductEntity?> GetProductByIdAsync(int id, CancellationToken cancellationToken);

    Task<List<ProductEntity>> GetProductByIdWithCategoriesAsync(
        int id,
        CancellationToken cancellationToken);
}