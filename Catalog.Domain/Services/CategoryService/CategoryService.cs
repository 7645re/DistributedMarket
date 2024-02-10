using Catalog.Domain.Dto.Category;
using Catalog.Domain.Mappers;
using Catalog.Domain.UnitOfWork;

namespace Catalog.Domain.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        var categoryEntities = await _unitOfWork.CategoryRepository.GetAllAsync(cancellationToken);
        return categoryEntities.ToCategories();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        var categoryEntity = await _unitOfWork.CategoryRepository.GetByIdAsync(id, cancellationToken);
        return categoryEntity?.ToCategory();
    }

    public async Task<Category> CreateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        await CheckCategoryIsUnique(category, cancellationToken);
        var categoryEntity = _unitOfWork.CategoryRepository.Add(category.ToCategoryEntity());
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return categoryEntity.ToCategory();
    }

    public async Task<Category> UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        var existingCategoryEntityById = await _unitOfWork
            .CategoryRepository
            .GetByIdAsync(category.Id, cancellationToken);
        if (existingCategoryEntityById is null)
            throw new InvalidOperationException($"Category with id {category.Id} doesnt exist");

        await CheckCategoryIsUnique(category, cancellationToken);
        
        existingCategoryEntityById.Name = category.Name;
        
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            _unitOfWork
                .CategoryRepository
                .Update(existingCategoryEntityById);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }, cancellationToken);

        return existingCategoryEntityById.ToCategory();
    }

    public async Task DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        var categoriesWithRelations = await _unitOfWork
            .ProductCategoryRepository
            .GetProductCategoryByCategoryId(id, cancellationToken);
        if (categoriesWithRelations.Any())
            throw new InvalidOperationException("Category cannot be deleted because it has a dependency");

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            _unitOfWork.CategoryRepository.DeleteById(id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }, cancellationToken);
    }

    private async Task CheckCategoryIsUnique(Category category, CancellationToken cancellationToken)
    {
        var categoryEntityWithSameName = await _unitOfWork
            .CategoryRepository
            .GetByNameAsync(category.Name, cancellationToken);
        if (categoryEntityWithSameName is not null && categoryEntityWithSameName.Id != category.Id) 
            throw new InvalidOperationException($"Category with name {category.Name} already exists");
        if (categoryEntityWithSameName is not null && categoryEntityWithSameName.Id == category.Id)
            throw new InvalidOperationException($"Category already have this name");
    }
}