using Businesses.Domain.Dtos;

namespace Businesses.Domain.Interfaces.Integration;

public interface IReviewIntegration
{
    Task<List<ReviewDto>> GetAllAsync(int businessId, string accessToken);
    Task<Tuple<long, double>> GetReviewCountAndRatingAsync(int businessId, string accessToken);
}
