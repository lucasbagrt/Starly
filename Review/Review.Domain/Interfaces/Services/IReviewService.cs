using Review.Domain.Dtos;
using Review.Domain.Filters;
using Starly.Domain.Dtos.Default;

namespace Review.Domain.Interfaces.Services;

public interface IReviewService
{
    Task<Tuple<long, double>> GetCountAndRatingByBusiness(int businessId);
    Task<ICollection<ReviewDto>> GetAllAsync(ReviewFilter filter, string acessToken);
    Task<ReviewDto> GetById(int id, string acessToken);
    Task<DefaultServiceResponseDto> Create(CreateReviewDto createReviewDto, int userId, string acessToken);
    Task<DefaultServiceResponseDto> Delete(int id, int userId);
    Task<DefaultServiceResponseDto> Update(UpdateReviewDto updateReviewDto, int userId, string acessToken);
    Task<DefaultServiceResponseDto> UploadPhoto(UploadPhotoDto uploadPhotoDto, int userId);
}
