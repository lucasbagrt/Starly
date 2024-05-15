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
using System.Text.Json;

namespace Businesses.Service.Services;

public class BusinessService : BaseService, IBusinessService
{
    private readonly NotificationContext _notificationContext;
    private readonly IBusinessRepository _businessRepository;
    private readonly IMapper _mapper;

    public BusinessService(NotificationContext notificationContext,
        IBusinessRepository businessRepository, IMapper mapper)
    {
        _notificationContext = notificationContext;
        _businessRepository = businessRepository;
        _mapper = mapper;
    }

    public async Task<DefaultServiceResponseDto> Update(UpdateBusinessDto updateBusinessDto)
    {
        var businessEntity = await _businessRepository.SelectAsync(updateBusinessDto.Id);
        if (businessEntity == null)
            return DefaultResponse(StaticNotifications.BusinessNotFound.Message, false);        

        _mapper.Map(updateBusinessDto, businessEntity);
        if (businessEntity.Hours != null && businessEntity.Hours.Any())
            foreach (var hour in businessEntity.Hours)
                hour.IsOvernight = IsOvernight(hour.Start, hour.End);

        businessEntity.UpdatedAt = DateTime.Now;
        await _businessRepository.UpdateAsync(businessEntity);
        return DefaultResponse(StaticNotifications.BusinessUpdated.Message, true);
    }

    public async Task<DefaultServiceResponseDto> Delete(int id)
    {
        var business = await _businessRepository.SelectAsync(id);
        if (business == null)
            return DefaultResponse(StaticNotifications.BusinessNotFound.Message, false);

        await _businessRepository.DeleteAsync(id);
        return DefaultResponse(StaticNotifications.BusinessDeleted.Message, true);
    }

    public async Task<DefaultServiceResponseDto> Create(CreateBusinessDto createBusinessDto)
    {
        if (!IsValidBusiness(createBusinessDto))
            return default;

        var business = _mapper.Map<Business>(createBusinessDto);
        if (business.Hours != null && business.Hours.Any())
            foreach (var hour in business.Hours)
                hour.IsOvernight = IsOvernight(hour.Start, hour.End);

        await _businessRepository.InsertAsync(business);
        return DefaultResponse(business.Id > 0 ? StaticNotifications.BusinessSuccess.Message : StaticNotifications.BusinessError.Message, business.Id > 0);
    }

    public async Task<ICollection<BusinessDto>> GetAllAsync(BusinessFilter filter)
    {
        var businesses = _businessRepository
            .GetQueryable() 
            .OrderByDescending(u => u.CreatedAt)
            .ApplyFilter(filter);

        var response = _mapper.Map<List<BusinessDto>>(businesses);

        foreach (var business in response)
            business.IsOpenNow = IsBusinessOpenNow(businesses.FirstOrDefault(b => b.Id == business.Id)?.Hours.ToList());        

        return await Task.FromResult(response);
    }

    public async Task<BusinessByIdResponseDto> GetById(int id)
    {
        var business = (await _businessRepository
           .SelectAsync(id));

        var response = _mapper.Map<BusinessByIdResponseDto>(business);
        response.IsOpenNow = IsBusinessOpenNow(business.Hours.ToList());
        response.Location = JsonSerializer.Deserialize<BusinessLocationDto>(business.Location);
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

    private bool IsBusinessOpenNow(List<BusinessHour> businessHour)
    {
        var currentDateTime = DateTime.Now;
        var currentDayOfWeek = (short)currentDateTime.DayOfWeek;
        var currentTime = currentDateTime.TimeOfDay;

        var todaysBusinessHours = businessHour.FirstOrDefault(bh => bh.DayOfWeek == currentDayOfWeek);

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
    #endregion
}
