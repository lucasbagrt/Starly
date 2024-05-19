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
using Starly.CrossCutting.Azure;
using Microsoft.Extensions.Configuration;
using Starly.Service.Validators;
using Microsoft.AspNetCore.Http;
using Businesses.Domain.Interfaces.Integration;

namespace Businesses.Service.Services;

public class BusinessService : BaseService, IBusinessService
{
    private readonly IConfiguration _configuration;
    private readonly NotificationContext _notificationContext;
    private readonly IBusinessRepository _businessRepository;
    private readonly IBusinessPhotoRepository _businessPhotoRepository;
    private readonly IMapper _mapper;
    private readonly string _blobUrl;
    private readonly string _containerName;
    private readonly AzureBlobClient _blobClient;
    private readonly IReviewIntegration _reviewIntegration;

    public BusinessService(NotificationContext notificationContext,
        IBusinessRepository businessRepository, IMapper mapper, IConfiguration configuration,
        IBusinessPhotoRepository businessPhotoRepository, IReviewIntegration reviewIntegration)
    {
        _configuration = configuration;
        _notificationContext = notificationContext;
        _businessRepository = businessRepository;
        _mapper = mapper;
        _containerName = configuration["Azure:BlobStorage:ContainerName:BusinessPhoto"];
        _blobUrl = configuration["Azure:BlobStorage:UrlBlob"];
        _blobClient = new AzureBlobClient(configuration["Azure:BlobStorage:ConnectionString"]);
        _businessPhotoRepository = businessPhotoRepository;
        _reviewIntegration = reviewIntegration;
    }

    public async Task<bool> ExistsById(int id)
    {
        var business = (await _businessRepository.SelectAsync(id));
        return business != null;
    }

    public async Task<DefaultServiceResponseDto> DeletePhoto(int photoId)
    {
        var businessPhoto = await _businessPhotoRepository.SelectAsync(photoId);
        if (businessPhoto == null)
        {
            _notificationContext.AddNotification(StaticNotifications.BusinessPhotoNotFound.Key, StaticNotifications.BusinessPhotoNotFound.Message);
            return default;
        }

        await DeletePhotosAsync(businessPhoto);
        await _businessPhotoRepository.DeleteAsync(photoId);

        return DefaultResponse(StaticNotifications.BusinessPhotoDeleted.Message, true);
    }

    public async Task<DefaultServiceResponseDto> UploadPhoto(UploadPhotoDto uploadPhotoDto)
    {
        var validationResult = Validate(uploadPhotoDto, Activator.CreateInstance<UploadPhotoValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; }

        var business = await _businessRepository.SelectAsync(uploadPhotoDto.BusinessId);
        if (business == null)
        {
            _notificationContext.AddNotification(StaticNotifications.BusinessNotFound.Key, StaticNotifications.BusinessNotFound.Message);
            return default;
        }

        var uploadedPhotos = await UploadPhotosAsync(uploadPhotoDto.Photos, business);
        if (!uploadedPhotos)
            return default;

        await _businessRepository.UpdateAsync(business);

        return DefaultResponse(StaticNotifications.BusinessPhotoUploaded.Message, true);
    }

    public async Task<DefaultServiceResponseDto> Update(UpdateBusinessDto updateBusinessDto)
    {
        var validationResult = Validate(updateBusinessDto, Activator.CreateInstance<UpdateBusinessValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; }

        var businessEntity = await _businessRepository.SelectAsync(updateBusinessDto.Id);
        if (businessEntity == null)
            return DefaultResponse(StaticNotifications.BusinessNotFound.Message, false);

        _mapper.Map(updateBusinessDto, businessEntity);
        SetHasOvernight(businessEntity);

        businessEntity.UpdatedAt = DateTime.Now;
        await _businessRepository.UpdateAsync(businessEntity);
        return DefaultResponse(StaticNotifications.BusinessUpdated.Message, true);
    }

    public async Task<DefaultServiceResponseDto> Delete(int id)
    {
        var business = await _businessRepository.SelectAsync(id);
        if (business == null)
            return DefaultResponse(StaticNotifications.BusinessNotFound.Message, false);

        if (business.Photos != null)
            foreach (var photo in business.Photos)
                await DeletePhotosAsync(photo);

        await _businessRepository.DeleteAsync(id);
        return DefaultResponse(StaticNotifications.BusinessDeleted.Message, true);
    }

    public async Task<DefaultServiceResponseDto> Create(CreateBusinessDto createBusinessDto)
    {
        if (!IsValidBusiness(createBusinessDto))
            return default;

        var business = _mapper.Map<Business>(createBusinessDto);
        SetHasOvernight(business);

        await _businessRepository.InsertAsync(business);
        return DefaultResponse(business.Id > 0 ? StaticNotifications.BusinessSuccess.Message : StaticNotifications.BusinessError.Message, business.Id > 0);
    }

    public async Task<ICollection<BusinessDto>> GetAllAsync(BusinessFilter filter, string accessToken)
    {
        var businesses = _businessRepository
            .GetQueryable()
            .OrderByDescending(u => u.CreatedAt)
            .ApplyFilter(filter);

        var response = _mapper.Map<List<BusinessDto>>(businesses);

        foreach (var business in response)
        {
            business.IsOpenNow = IsBusinessOpenNow(businesses.FirstOrDefault(b => b.Id == business.Id)?.Hours.ToList());
            var reviewCountAndRating = await _reviewIntegration.GetReviewCountAndRatingAsync(business.Id, accessToken);
            if (reviewCountAndRating != null)
            {
                business.ReviewCount = reviewCountAndRating.Item1;
                business.AverageRating = reviewCountAndRating.Item2;
            }
        }

        return await Task.FromResult(response);
    }

    public async Task<BusinessByIdResponseDto> GetById(int id, string accessToken)
    {
        var business = (await _businessRepository
           .SelectAsync(id));

        if (business == null) return null;

        var response = _mapper.Map<BusinessByIdResponseDto>(business);
        response.IsOpenNow = IsBusinessOpenNow(business.Hours.ToList());
        response.Location = JsonSerializer.Deserialize<BusinessLocationDto>(business.Location);
        response.Reviews = await _reviewIntegration.GetAllAsync(id, accessToken);
        if (response.Reviews != null && response.Reviews.Any())
        {
            response.ReviewCount = response.Reviews.Count;
            response.AverageRating = response.Reviews.Average(t => t.Rating);
        }

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

    private void SetHasOvernight(Business business)
    {
        if (business.Hours != null && business.Hours.Any())
            foreach (var hour in business.Hours)
                hour.IsOvernight = IsOvernight(hour.Start, hour.End);
    }

    private async Task<bool> UploadPhotosAsync(List<IFormFile> photos, Business business)
    {
        if (photos == null || !photos.Any())
            return true;

        var index = 0;
        foreach (var photo in photos)
        {
            var validationResult = Validate(photo, Activator.CreateInstance<PhotoValidator>());
            if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return false; }

            var fileExtension = Path.GetExtension(photo.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var stream = photo.OpenReadStream();
            await _blobClient.Upload(stream, fileName, _containerName);

            business.Photos.Add(
                new BusinessPhoto
                {
                    PhotoUrl = _blobUrl + "/" + _containerName + "/" + fileName,
                    Default = index == 0
                });
            index++;
        }
        return true;
    }

    private async Task DeletePhotosAsync(BusinessPhoto photo)
    {
        if (photo == null)
            return;

        await _blobClient.Delete(photo.PhotoUrl.Split("/").LastOrDefault(), _containerName);
    }
    #endregion
}
