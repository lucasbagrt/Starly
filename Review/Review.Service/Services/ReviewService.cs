using AutoMapper;
using Review.Domain.Dtos;
using Review.Domain.Interfaces.Repositories;
using Review.Domain.Interfaces.Services;
using Starly.CrossCutting.Notifications;
using Starly.Domain.Dtos.Default;
using Starly.Service.Services;
using Starly.Domain.Extensions;
using Review.Domain.Filters;
using Review.Service.Validators;
using Starly.CrossCutting.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Starly.Service.Validators;
using Review.Domain.Entities;
using Review.Domain.Interfaces.Integration;

namespace Review.Service.Services;

public class ReviewService : BaseService, IReviewService
{
    private readonly IConfiguration _configuration;
    private readonly NotificationContext _notificationContext;
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;
    private readonly string _blobUrl;
    private readonly string _containerName;
    private readonly AzureBlobClient _blobClient;
    private readonly IBusinessIntegration _businessIntegration;
    private readonly IUserIntegration _userIntegration;

    public ReviewService(NotificationContext notificationContext,
        IReviewRepository reviewRepository,
        IMapper mapper,
        IConfiguration configuration,
        IBusinessIntegration businessIntegration,
        IUserIntegration userIntegration)
    {
        _notificationContext = notificationContext;
        _reviewRepository = reviewRepository;
        _mapper = mapper;
        _containerName = configuration["Azure:BlobStorage:ContainerName:ReviewPhoto"];
        _blobUrl = configuration["Azure:BlobStorage:UrlBlob"];
        _blobClient = new AzureBlobClient(configuration["Azure:BlobStorage:ConnectionString"]);
        _configuration = configuration;
        _businessIntegration = businessIntegration;
        _userIntegration = userIntegration;
    }

    public async Task<Tuple<long, double>> GetCountAndRatingByBusiness(int businessId)
    {
        return await _reviewRepository.GetReviewCountAndRatingAsync(businessId);
    }

    public async Task<DefaultServiceResponseDto> UploadPhoto(UploadPhotoDto uploadPhotoDto, int userId)
    {
        var validationResult = Validate(uploadPhotoDto, Activator.CreateInstance<UploadPhotoValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; }

        var review = await _reviewRepository.SelectAsync(uploadPhotoDto.ReviewId);

        if (review == null || review.UserId != userId)
        {
            _notificationContext.AddNotification(StaticNotifications.ReviewNotFound.Key, StaticNotifications.ReviewNotFound.Message);
            return default;
        }

        var uploadedPhotos = await UploadPhotosAsync(uploadPhotoDto.Photos, review);
        if (!uploadedPhotos)
            return default;

        await _reviewRepository.UpdateAsync(review);
        return DefaultResponse(StaticNotifications.ReviewPhotoUploaded.Message, true);
    }

    public async Task<DefaultServiceResponseDto> Create(CreateReviewDto createReviewDto, int userId, string acessToken)
    {
        var validationResult = Validate(createReviewDto, Activator.CreateInstance<CreateReviewValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; }

        if (!(await ValidateBusiness(createReviewDto.BusinessId, acessToken))) return default;

        var review = _mapper.Map<Domain.Entities.Review>(createReviewDto);
        review.UserId = userId;

        await _reviewRepository.InsertAsync(review);
        return DefaultResponse(review.Id > 0 ? StaticNotifications.ReviewSuccess.Message : StaticNotifications.ReviewError.Message, review.Id > 0);
    }

    public async Task<DefaultServiceResponseDto> Delete(int id, int userId)
    {
        var review = await _reviewRepository.SelectAsync(id);
        if (review == null || review.UserId != userId)
            return DefaultResponse(StaticNotifications.ReviewNotFound.Message, false);

        await DeletePhotosAsync(review);
        await _reviewRepository.DeleteAsync(id);
        return DefaultResponse(StaticNotifications.ReviewDeleted.Message, true);
    }

    public async Task<ICollection<ReviewDto>> GetAllAsync(ReviewFilter filter, string acessToken)
    {
        var reviews = _reviewRepository
          .GetQueryable()
          .OrderByDescending(u => u.CreatedAt)
          .ApplyFilter(filter);

        var response = _mapper.Map<List<ReviewDto>>(reviews);

        if (filter.BusinessId > 0)
            response = response.Where(r => r.BusinessId == filter.BusinessId).ToList();

        foreach (var review in response)
            review.User = await _userIntegration.GetUserInfo(review.UserId, acessToken);

        return response;
    }

    public async Task<ReviewDto> GetById(int id, string acessToken)
    {
        var review = (await _reviewRepository
         .SelectAsync(id));

        var response = _mapper.Map<ReviewDto>(review);
        response.User = await _userIntegration.GetUserInfo(review.UserId, acessToken);

        return response;
    }

    public async Task<DefaultServiceResponseDto> Update(UpdateReviewDto updateReviewDto, int userId, string acessToken)
    {
        var validationResult = Validate(updateReviewDto, Activator.CreateInstance<UpdateReviewValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; }

        var reviewEntity = await _reviewRepository.SelectAsync(updateReviewDto.Id);
        if (reviewEntity == null || reviewEntity.UserId != userId)
            return DefaultResponse(StaticNotifications.ReviewNotFound.Message, false);

        if (!(await ValidateBusiness(updateReviewDto.BusinessId, acessToken))) return default;

        _mapper.Map(updateReviewDto, reviewEntity);
        reviewEntity.UpdatedAt = DateTime.Now;
        await _reviewRepository.UpdateAsync(reviewEntity);
        return DefaultResponse(StaticNotifications.ReviewUpdated.Message, true);
    }

    #region [Private]
    private async Task<bool> UploadPhotosAsync(List<IFormFile> photos, Domain.Entities.Review review)
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

            review.Photos.Add(
                new ReviewPhoto
                {
                    Url = _blobUrl + "/" + _containerName + "/" + fileName,
                });
            index++;
        }
        return true;
    }

    private async Task DeletePhotosAsync(Domain.Entities.Review review)
    {
        if (review.Photos == null || !review.Photos.Any())
            return;

        foreach (var photo in review.Photos)
            await _blobClient.Delete(photo.Url.Split("/").LastOrDefault(), _containerName);

        review.Photos = new List<ReviewPhoto>();
    }

    private async Task<bool> ValidateBusiness(int businessId, string acessToken)
    {
        var existsBusiness = await _businessIntegration.ExistsById(businessId, acessToken);
        if (existsBusiness) return true;

        _notificationContext.AddNotification(
        StaticNotifications.BusinessNotFound.Key, StaticNotifications.BusinessNotFound.Message); return false;
    }   
    #endregion
}
