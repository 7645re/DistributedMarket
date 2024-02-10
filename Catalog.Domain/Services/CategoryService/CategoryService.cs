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
        throw new NotImplementedException();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Category> CreateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        var categoryEntity = _unitOfWork.CategoryRepository.Add(category.ToCategoryEntity());
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return categoryEntity.ToCategory();
    }

    public async Task<Category> UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        var categoryEntity = _unitOfWork.CategoryRepository.Update(category.ToCategoryEntity());
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return categoryEntity.ToCategory();
    }

    public async Task DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}