using Catalog.Domain.Dto.Product;
using Catalog.Domain.Models;

namespace Catalog.Domain.Validators.Product;

public interface IProductValidator
{
    Task ValidateAsync(ProductCreate productCreate, CancellationToken cancellationToken);
    Task ValidateAsync(ProductUpdate productUpdate, ProductEntity productEntity, CancellationToken cancellationToken);
}