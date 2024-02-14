using Catalog.Domain.Dto.Category;
using Catalog.Domain.Models;

namespace Catalog.Domain.Validators.Category;

public interface ICategoryValidator
{
    Task ValidateAsync(CategoryCreate categoryCreate, CancellationToken cancellationToken);

    Task ValidateAsync(
        CategoryEntity categoryEntity, CategoryUpdate categoryUpdate, CancellationToken cancellationToken);
}