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

    public async Task<ICollection<BusinessResponseDto>> GetAllAsync(BusinessFilter filter)
    {
        var businesses = (await _businessRepository
            .SelectAsync())
            .AsQueryable()
            .OrderByDescending(u => u.CreatedAt)
            .ApplyFilter(filter);

        var response = new List<BusinessResponseDto>();

        foreach (var business in businesses)
        {
            var businessDto = _mapper.Map<BusinessResponseDto>(business);
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

    private bool IsBusinessOpenNow(Business business)
    {
        var currentDayOfWeek = DateTime.UtcNow.DayOfWeek;
        var currentTime = DateTime.UtcNow.TimeOfDay;

        var todaysBusinessHours = business.Hours.FirstOrDefault(bh => bh.DayOfWeek == (short)currentDayOfWeek);

        if (todaysBusinessHours == null)
            return false;

        var startTime = TimeSpan.ParseExact(todaysBusinessHours.Start, "hhmm", CultureInfo.InvariantCulture);
        var endTime = TimeSpan.ParseExact(todaysBusinessHours.End, "hhmm", CultureInfo.InvariantCulture);

        if (todaysBusinessHours.IsOvernight)
        {
            var adjustedStartTime = startTime;
            if (startTime > endTime)
            {
                adjustedStartTime = adjustedStartTime.Subtract(new TimeSpan(1, 0, 0, 0));
            }

            var adjustedEndTime = endTime;
            if (endTime < startTime)
            {
                adjustedEndTime = adjustedEndTime.Add(new TimeSpan(1, 0, 0, 0));
            }

            if (currentTime >= adjustedStartTime && currentTime <= adjustedEndTime)
                return true;

            return false;
        }
        else
        {
            if (currentTime >= startTime && currentTime <= endTime)
                return true;

            return false;
        }
    }
}
