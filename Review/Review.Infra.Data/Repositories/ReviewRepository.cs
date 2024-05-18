using Review.Domain.Interfaces.Repositories;
using Starly.Infra.Data.Repositories;

namespace Review.Infra.Data.Repositories;

public class ReviewRepository(ApplicationDbContext context) : BaseRepository<Domain.Entities.Review, int, ApplicationDbContext>(context), IReviewRepository
{

}
