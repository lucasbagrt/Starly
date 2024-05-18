using Microsoft.EntityFrameworkCore;
using Review.Domain.Interfaces.Repositories;
using Starly.Infra.Data.Repositories;

namespace Review.Infra.Data.Repositories;

public class ReviewRepository(ApplicationDbContext context) : BaseRepository<Domain.Entities.Review, int, ApplicationDbContext>(context), IReviewRepository
{
    public async Task<Tuple<long, double>> GetReviewCountAndRatingAsync(int businessId)
    {
        var reviewCount = await _dataContext.Review
            .Where(r => r.BusinessId == businessId)
            .LongCountAsync(); 

        var averageRating = await _dataContext.Review
            .Where(r => r.BusinessId == businessId)
            .AverageAsync(r => r.Rating); 

        return Tuple.Create(reviewCount, averageRating);
    }
}
