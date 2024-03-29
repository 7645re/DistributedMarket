using Catalog.Domain.Repositories.Category;
using Catalog.Domain.Repositories.Product;
using Catalog.Domain.Repositories.ProductCategory;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Domain.UnitOfWork;

public interface IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }

    IProductRepository ProductRepository { get; }

    IProductCategoryRepository ProductCategoryRepository { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
    Task ExecuteInTransactionAsync(
        Func<Task> action,
        CancellationToken cancellationToken,
        Action<DbUpdateException>? onException = null);
}