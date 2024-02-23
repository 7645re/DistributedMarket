using Catalog.Domain.Dto.Category;
using Catalog.Domain.Mappers;
using Catalog.Domain.Models;
using Catalog.Domain.UnitOfWork;
using Catalog.Domain.Validators.Category;

namespace Catalog.Domain.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryValidator _categoryValidator;

    public CategoryService(IUnitOfWork unitOfWork,
        ICategoryValidator categoryValidator)
    {
        _unitOfWork = unitOfWork;
        _categoryValidator = categoryValidator;
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

    public async Task<Category> CreateCategoryAsync(
        CategoryCreate categoryCreate, CancellationToken cancellationToken)
    {
        await _categoryValidator.ValidateAsync(categoryCreate, cancellationToken);

        var categoryEntity = categoryCreate.ToCategoryEntity();
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            categoryEntity = _unitOfWork.CategoryRepository.Add(categoryEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }, cancellationToken);
        return categoryEntity.ToCategory();
    }

    public async Task<Category> UpdateCategoryAsync(CategoryUpdate categoryUpdate, CancellationToken cancellationToken)
    {
        var categoryEntity = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryUpdate.Id, cancellationToken);
        if (categoryEntity is null)
            throw new InvalidOperationException($"Category with id {categoryUpdate.Id} not found");

        await _categoryValidator.ValidateAsync(categoryEntity, categoryUpdate, cancellationToken);
        UpdateChangedFields(categoryUpdate, categoryEntity);
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            _unitOfWork.CategoryRepository.Update(categoryEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }, cancellationToken);
        return categoryEntity.ToCategory();
    }

    public async Task DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        var existCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(id, cancellationToken);
        if (existCategory is null)
            throw new InvalidOperationException($"Category with id {id} not found");

        var productEntities = await _unitOfWork.ProductRepository.GetByCategoryIdAsync(id, cancellationToken);
        if (productEntities.Any())
            throw new InvalidOperationException($"Category with id {id} has products");

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            _unitOfWork.CategoryRepository.DeleteById(id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }, cancellationToken);
    }

    private void UpdateChangedFields(CategoryUpdate categoryUpdate, CategoryEntity categoryEntity)
    {
        if (categoryUpdate.Name is not null)
            categoryEntity.Name = categoryUpdate.Name;
    }
}