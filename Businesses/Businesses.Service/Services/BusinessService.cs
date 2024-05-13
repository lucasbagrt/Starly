using Businesses.Domain.Filters;
using Businesses.Domain.Interfaces.Repositories;
using Businesses.Domain.Interfaces.Services;
using Starly.CrossCutting.Notifications;
using Starly.Service.Services;
using Starly.Domain.Extensions;
using AutoMapper;
using Businesses.Domain.Entities;
using System.Globalization;
using Businesses.Domain.Dtos;
using Starly.Domain.Dtos.Default;
using Businesses.Service.Validators;

namespace Businesses.Service.Services;

public class BusinessService : BaseService, IBusinessService
{
    private readonly NotificationContext _notificationContext;
    private readonly IBusinessRepository _businessRepository;
    private readonly IBusinessCategoryRepository _businessCategoryRepository;
    private readonly IBusinessHourRepository _businessHourRepository;
    private readonly IMapper _mapper;

    public BusinessService(NotificationContext notificationContext,
        IBusinessRepository businessRepository, IMapper mapper,
        IBusinessCategoryRepository businessCategoryRepository, IBusinessHourRepository businessHourRepository)
    {
        _notificationContext = notificationContext;
        _businessRepository = businessRepository;
        _mapper = mapper;
        _businessCategoryRepository = businessCategoryRepository;
        _businessHourRepository = businessHourRepository;
    }

    public async Task<DefaultServiceResponseDto> Update(UpdateBusinessDto updateBusinessDto)
    {
        var businessEntity = await _businessRepository.SelectAsync(updateBusinessDto.Id);
        if (businessEntity == null)
            return DefaultResponse(StaticNotifications.BusinessNotFound.Message, false);

        var createBusinessDto = _mapper.Map<CreateBusinessDto>(updateBusinessDto);
        if (!IsValidBusiness(createBusinessDto))
            return default;
         
        await DeleteCategoriesAsync(businessEntity);
        await DeleteHoursAsync(businessEntity);        

        var business = _mapper.Map<Business>(createBusinessDto);
        business.Id = updateBusinessDto.Id;

        await _businessRepository.InsertAsync(business);
        
        await InsertCategoriesAsync(business);
        await InsertHoursAsync(business);

        return DefaultResponse(business.Id > 0 ? StaticNotifications.BusinessSuccess.Message : StaticNotifications.BusinessError.Message, business.Id > 0);
    }

    public async Task<DefaultServiceResponseDto> Delete(int id)
    {
        var business = _businessRepository.SelectAsync(id);
        if (business == null)
            return DefaultResponse(StaticNotifications.BusinessNotFound.Message, false);

        await _businessRepository.DeleteAsync(id);
        return DefaultResponse(StaticNotifications.BusinessDeleted.Message, false);
    }

    public async Task<DefaultServiceResponseDto> Create(CreateBusinessDto createBusinessDto)
    {
        if (!IsValidBusiness(createBusinessDto))
            return default;

        var business = _mapper.Map<Business>(createBusinessDto);
        business.Active = true;
        business.CreatedAt = DateTime.Now;

        await _businessRepository.InsertAsync(business);        
        await InsertCategoriesAsync(business);
        await InsertHoursAsync(business);

        return DefaultResponse(business.Id > 0 ? StaticNotifications.BusinessSuccess.Message : StaticNotifications.BusinessError.Message, business.Id > 0);
    }

    public async Task<ICollection<BusinessDto>> GetAllAsync(BusinessFilter filter)
    {
        var businesses = (await _businessRepository
            .SelectAsync())
            .AsQueryable()
            .OrderByDescending(u => u.CreatedAt)
            .ApplyFilter(filter);

        var response = new List<BusinessDto>();

        foreach (var business in businesses)
        {
            var businessDto = _mapper.Map<BusinessDto>(business);
            businessDto.IsOpenNow = IsBusinessOpenNow(business);
            businessDto.Photo = business.Photos.FirstOrDefault(t => t.Default)?.PhotoUrl;
            response.Add(businessDto);
        }

        return response;
    }

    public async Task<BusinessByIdResponseDto> GetById(int id)
    {
        var business = (await _businessRepository
           .SelectAsync(id));

        var response = _mapper.Map<BusinessByIdResponseDto>(business);
        response.IsOpenNow = IsBusinessOpenNow(business);
        return response;
    }

    #region [Private]    
    private bool IsValidBusiness(CreateBusinessDto createBusinessDto)
    {
        var validationResult = Validate(createBusinessDto, Activator.CreateInstance<CreateBusinessValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return false; }

        foreach (var category in createBusinessDto.Categories)
        {
            validationResult = Validate(category, Activator.CreateInstance<CreateBusinessCategoryValidator>());
            if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return false; }
        }

        foreach (var hour in createBusinessDto.Hours)
        {
            validationResult = Validate(hour, Activator.CreateInstance<CreateBusinessHourValidator>());
            if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return false; }
        }

        return true;
    }

    private bool IsBusinessOpenNow(Business business)
    {
        var currentDateTime = DateTime.UtcNow;
        var currentDayOfWeek = (short)currentDateTime.DayOfWeek;
        var currentTime = currentDateTime.TimeOfDay;

        var todaysBusinessHours = business.Hours.FirstOrDefault(bh => bh.DayOfWeek == currentDayOfWeek);

        if (todaysBusinessHours == null)
            return false;

        var (startTime, endTime) = GetAdjustedTimeSpan(todaysBusinessHours.Start, todaysBusinessHours.End, todaysBusinessHours.IsOvernight);

        return IsTimeInRange(currentTime, startTime, endTime);
    }

    private (TimeSpan startTime, TimeSpan endTime) GetAdjustedTimeSpan(string start, string end, bool isOvernight)
    {
        var startTime = TimeSpan.ParseExact(start, "hhmm", CultureInfo.InvariantCulture);
        var endTime = TimeSpan.ParseExact(end, "hhmm", CultureInfo.InvariantCulture);

        if (isOvernight)
        {
            startTime = startTime > endTime ? startTime.Subtract(new TimeSpan(1, 0, 0, 0)) : startTime;
            endTime = endTime < startTime ? endTime.Add(new TimeSpan(1, 0, 0, 0)) : endTime;
        }

        return (startTime, endTime);
    }

    private bool IsTimeInRange(TimeSpan time, TimeSpan startTime, TimeSpan endTime)
    {
        return time >= startTime && time <= endTime;
    }

    private bool IsOvernight(string start, string end)
    {
        var startTime = TimeSpan.ParseExact(start, "hhmm", CultureInfo.InvariantCulture);
        var endTime = TimeSpan.ParseExact(end, "hhmm", CultureInfo.InvariantCulture);

        return startTime > endTime;
    }

    private async Task InsertHoursAsync(Business business)
    {
        if (business.Hours == null || !business.Hours.Any())
            return;

        foreach (var hour in business.Hours)
        {
            hour.BusinessId = business.Id;
            hour.IsOvernight = IsOvernight(hour.Start, hour.End);
            await _businessHourRepository.InsertAsync(hour);
        }
    }

    private async Task InsertCategoriesAsync(Business business)
    {
        if (business.Categories == null || !business.Categories.Any())
            return;

        foreach (var category in business.Categories)
        {
            category.BusinessId = business.Id;
            await _businessCategoryRepository.InsertAsync(category);
        }
    }

    private async Task DeleteCategoriesAsync(Business business)
    {
        if (business.Categories == null || !business.Categories.Any())
            return;

        foreach (var category in business.Categories)
            await _businessCategoryRepository.DeleteAsync(category.Id);
    }

    private async Task DeleteHoursAsync(Business business)
    {
        if (business.Hours == null || !business.Hours.Any())
            return;

        foreach (var hour in business.Hours)
            await _businessHourRepository.DeleteAsync(hour.Id);
    }
    #endregion
}
