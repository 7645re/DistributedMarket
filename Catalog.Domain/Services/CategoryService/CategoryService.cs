using Catalog.Domain.Dto.Category;
using Catalog.Domain.Mappers;
using Catalog.Domain.Models;
using Catalog.Domain.UnitOfWork;
using Catalog.Domain.Validators.Category;
using Catalog.Messaging.Events.Category;
using MassTransit.KafkaIntegration;

namespace Catalog.Domain.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryValidator _categoryValidator;
    private readonly ITopicProducer<Guid, CategoryCreateEvent> _categoryCreateProducer;
    private readonly ITopicProducer<Guid, CategoryUpdateEvent> _categoryUpdateProducer;
    private readonly ITopicProducer<Guid, CategoryDeleteEvent> _categoryDeleteProducer;

    public CategoryService(
        IUnitOfWork unitOfWork,
        ICategoryValidator categoryValidator,
        ITopicProducer<Guid, CategoryCreateEvent> categoryCreateProducer,
        ITopicProducer<Guid, CategoryUpdateEvent> categoryUpdateProducer,
        ITopicProducer<Guid, CategoryDeleteEvent> categoryDeleteProducer)
    {
        _unitOfWork = unitOfWork;
        _categoryValidator = categoryValidator;
        _categoryCreateProducer = categoryCreateProducer;
        _categoryUpdateProducer = categoryUpdateProducer;
        _categoryDeleteProducer = categoryDeleteProducer;
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
        
        var categoryCreateEvent = categoryEntity.ToCategoryCreateEvent();
        await _categoryCreateProducer.Produce(Guid.NewGuid(), categoryCreateEvent, cancellationToken);
        return categoryEntity.ToCategory();
    }

    public async Task<Category> UpdateCategoryAsync(CategoryUpdate categoryUpdate, CancellationToken cancellationToken)
    {
        var categoryEntity = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryUpdate.Id, cancellationToken);
        if (categoryEntity is null)
            throw new InvalidOperationException($"Category with id {categoryUpdate.Id} not found");

        await _categoryValidator.ValidateAsync(categoryEntity, categoryUpdate, cancellationToken);
        var categoryEntityForEvent = categoryEntity.Clone();
        
        UpdateChangedFields(categoryUpdate, categoryEntity);
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            _unitOfWork.CategoryRepository.Update(categoryEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }, cancellationToken);

        var ev = categoryEntityForEvent.ToCategoryUpdateEvent(categoryUpdate);
        await _categoryUpdateProducer.Produce(Guid.NewGuid(), ev, cancellationToken);
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

        await _categoryDeleteProducer.Produce(Guid.NewGuid(), new CategoryDeleteEvent
        {
            Id = id,
            Timestamp = DateTimeOffset.Now
        }, cancellationToken);
    }

    private void UpdateChangedFields(CategoryUpdate categoryUpdate, CategoryEntity categoryEntity)
    {
        if (categoryUpdate.Name is not null)
            categoryEntity.Name = categoryUpdate.Name;
    }
}