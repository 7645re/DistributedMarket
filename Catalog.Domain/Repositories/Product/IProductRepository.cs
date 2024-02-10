using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;

namespace Catalog.Domain.Repositories.Product;

public interface IProductRepository : IRepository<ProductEntity>
{
    Task<ProductEntity?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);
}