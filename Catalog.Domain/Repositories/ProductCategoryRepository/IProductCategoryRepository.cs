using Catalog.Domain.Models;

namespace Catalog.Domain.Repositories.ProductCategoryRepository;

public interface IProductCategoryRepository
{
    Task<List<ProductEntityCategoryEntity>> GetProductsCategoriesByCategoryIdAsync(
        int id,
        CancellationToken cancellationToken);
}