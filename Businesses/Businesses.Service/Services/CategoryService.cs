using AutoMapper;
using Businesses.Domain.Dtos;
using Businesses.Domain.Entities;
using Businesses.Domain.Filters;
using Businesses.Domain.Interfaces.Repositories;
using Businesses.Domain.Interfaces.Services;
using Businesses.Service.Validators;
using Starly.CrossCutting.Notifications;
using Starly.Domain.Dtos.Default;
using Starly.Domain.Extensions;
using Starly.Service.Services;

namespace Businesses.Service.Services;

public class CategoryService : BaseService, ICategoryService
{
    private readonly NotificationContext _notificationContext;
    private readonly IMapper _mapper;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBusinessCategoryRepository _businessCategoryRepository;

    public CategoryService(NotificationContext notificationContext, IMapper mapper,
        ICategoryRepository categoryRepository, IBusinessCategoryRepository businessCategoryRepository)
    {
        _notificationContext = notificationContext;
        _mapper = mapper;
        _categoryRepository = categoryRepository;
        _businessCategoryRepository = businessCategoryRepository;
    }

    public async Task<DefaultServiceResponseDto> Delete(int id)
    {
        var category = await _categoryRepository.SelectAsync(id);
        if (category == null)
            return DefaultResponse(StaticNotifications.CategoryNotFound.Message, false);

        if (category.BusinessCategories != null && category.BusinessCategories.Any())
            foreach (var businessCategory in category.BusinessCategories)
                await _businessCategoryRepository.DeleteAsync(businessCategory.Id);

        await _categoryRepository.DeleteAsync(id);
        return DefaultResponse(StaticNotifications.CategoryDeleted.Message, true);
    }

    public async Task<DefaultServiceResponseDto> Create(CreateCategoryDto categoryDto)
    {
        var validationResult = Validate(categoryDto, Activator.CreateInstance<CreateCategoryValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; }

        var category = _mapper.Map<Category>(categoryDto);
        await _categoryRepository.InsertAsync(category);

        return DefaultResponse(category.Id > 0 ? StaticNotifications.CategorySuccess.Message :
            StaticNotifications.CategoryError.Message, category.Id > 0);
    }

    public async Task<ICollection<CategoryDto>> GetAllAsync(CategoryFilter filter)
    {
        var categories = (await _categoryRepository
           .SelectAsync())
           .AsQueryable()
           .ApplyFilter(filter);

        var response = _mapper.Map<List<CategoryDto>>(categories);
        return response;
    }

    public async Task<CategoryDto> GetById(int id)
    {
        var category = (await _categoryRepository
           .SelectAsync(id));

        var response = _mapper.Map<CategoryDto>(category);
        return response;
    }

    public async Task<DefaultServiceResponseDto> Update(CategoryDto categoryDto)
    {
        var category = await _categoryRepository.SelectAsync(categoryDto.Id);
        if (category == null)
            return DefaultResponse(StaticNotifications.CategoryNotFound.Message, false);

        var categoryEntity = _mapper.Map<Category>(categoryDto);
        await _categoryRepository.UpdateAsync(categoryEntity);
        return DefaultResponse(StaticNotifications.CategoryUpdated.Message, true);
    }
}
