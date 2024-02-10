using Catalog.Domain.Repositories.Category;
using Catalog.Domain.Repositories.Product;
using Catalog.Domain.Repositories.ProductCategory;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Domain.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    public ICategoryRepository CategoryRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IProductCategoryRepository ProductCategoryRepository { get; }
    private readonly CatalogDbContext _context;
    
    public UnitOfWork(
        CatalogDbContext context,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IProductCategoryRepository productCategoryRepository)
    {
        _context = context;
        CategoryRepository = categoryRepository;
        ProductRepository = productRepository;
        ProductCategoryRepository = productCategoryRepository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        await _context.Database.BeginTransactionAsync(cancellationToken);
    }
    
    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        await _context.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        await _context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public async Task ExecuteInTransactionAsync(Func<Task> action,
        CancellationToken cancellationToken,
        Action<DbUpdateException>? onException)
    {
        try
        {
            await _context.Database.BeginTransactionAsync(cancellationToken);
            await action();
            await _context.Database.CommitTransactionAsync(cancellationToken);
        }
        catch (DbUpdateException e)
        {
            await _context.Database.RollbackTransactionAsync(cancellationToken);

            if (onException is not null)
                onException(e);
            else
            {
                throw;
            }
        }
    }
}