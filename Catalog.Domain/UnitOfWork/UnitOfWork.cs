using Catalog.Domain.Repositories.Category;
using Catalog.Domain.Repositories.Product;
using Catalog.Domain.Repositories.ProductCategory;
using Microsoft.EntityFrameworkCore;
using Shared.DiagnosticContext;

namespace Catalog.Domain.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    public ICategoryRepository CategoryRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IProductCategoryRepository ProductCategoryRepository { get; }
    private readonly CatalogDbContext _context;
    private readonly IDiagnosticContext _diagnosticContext;
    
    public UnitOfWork(
        CatalogDbContext context,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IProductCategoryRepository productCategoryRepository,
        IDiagnosticContext diagnosticContext)
    {
        _context = context;
        CategoryRepository = categoryRepository;
        ProductRepository = productRepository;
        ProductCategoryRepository = productCategoryRepository;
        _diagnosticContext = diagnosticContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(UnitOfWork)}.{nameof(SaveChangesAsync)}"))
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
        using (_diagnosticContext.Measure($"{nameof(UnitOfWork)}.{nameof(ExecuteInTransactionAsync)}"))
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
}