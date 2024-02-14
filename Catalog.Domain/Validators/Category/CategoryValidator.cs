using Catalog.Domain.Dto.Category;
using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Category;

namespace Catalog.Domain.Validators.Category;

public class CategoryValidator : ICategoryValidator
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task ValidateAsync(CategoryCreate categoryCreate, CancellationToken cancellationToken)
    {
        NameLengthShouldBeMoreZero(categoryCreate.Name);
        await NameShouldBeUniqueAsync(categoryCreate.Name, cancellationToken); 
    }
    
    public async Task ValidateAsync(
        CategoryEntity categoryEntity, CategoryUpdate categoryUpdate, CancellationToken cancellationToken)
    {
        AtLeastOneFieldMustChanged(categoryEntity, categoryUpdate);
        if (categoryUpdate.Name is not null)
        {
            NameLengthShouldBeMoreZero(categoryUpdate.Name);
            IfNameNotNullShouldBeNew(categoryEntity, categoryUpdate);
            await NameShouldBeUniqueAsync(categoryUpdate.Name, cancellationToken);
        }
    }

    private void AtLeastOneFieldMustChanged(CategoryEntity categoryEntity, CategoryUpdate categoryUpdate)
    {
        var categoryEntityForCompare = new CategoryForCompare(categoryEntity.Name);
        var categoryUpdateForCompare = new CategoryForCompare(categoryUpdate.Name ?? categoryEntity.Name);
        
        if (categoryEntityForCompare.Equals(categoryUpdateForCompare))
            throw new InvalidOperationException("No changes.");
    }

    private void IfNameNotNullShouldBeNew(CategoryEntity categoryEntity, CategoryUpdate categoryUpdate)
    {
        if (categoryEntity.Name == categoryUpdate.Name)
            throw new InvalidOperationException("No changes.");
    }
    
    private void NameLengthShouldBeMoreZero(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException("The category name cannot be empty or consist" +
                                                " of only whitespace characters.");
    }

    private async Task NameShouldBeUniqueAsync(string name, CancellationToken cancellationToken)
    {
        var categoryWithSameName = await _categoryRepository.GetByNameAsync(name, cancellationToken);
        if (categoryWithSameName is not null)
            throw new InvalidOperationException($"A category with that name: {name} already exists.");
    }
}

record CategoryForCompare(string Name);