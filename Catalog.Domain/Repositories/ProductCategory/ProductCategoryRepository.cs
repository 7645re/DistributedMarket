using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Shared.DiagnosticContext;

namespace Catalog.Domain.Repositories.ProductCategory;

public class ProductCategoryRepository : BaseRepository<ProductEntityCategoryEntity>,
    IProductCategoryRepository
{
    public ProductCategoryRepository(CatalogDbContext context, IDiagnosticContextStorage diagnosticContextStorage) 
        : base(context, diagnosticContextStorage)
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

    public Task<List<ProductEntityCategoryEntity>> GetProductCategoryByCategoryId(
        int categoryId,
        CancellationToken cancellationToken)
    {
        using (DiagnosticContextStorage.Measure($"{nameof(ProductCategoryRepository)}" +
                                                $".{nameof(GetProductCategoryByCategoryId)}"))
            return Set
                .Where(e => e.CategoryId == categoryId)
                .ToListAsync(cancellationToken);
    }
}