using Catalog.Domain.Dto;
using Catalog.Domain.Mappers;
using Catalog.Domain.Repositories.Category;
using Catalog.Domain.UnitOfWork;

namespace Catalog.Domain.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return categories.ToCategories();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(id, cancellationToken);
        return category?.ToCategory();
    }

    public async Task<Category> CreateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        using (_unitOfWork)
        {
            var categoryEntity = _unitOfWork.CategoryRepository.Add(category.ToCategoryEntity());
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return categoryEntity.ToCategory();
        }
    }

    public async Task<Category> UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        using (_unitOfWork)
        {
            var categoryEntity = _unitOfWork.CategoryRepository.Update(category.ToCategoryEntity());
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return categoryEntity.ToCategory();
        }
    }

    public async Task DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        await _categoryRepository.DeleteCategoryByIdAsync(id, cancellationToken);
    }
}