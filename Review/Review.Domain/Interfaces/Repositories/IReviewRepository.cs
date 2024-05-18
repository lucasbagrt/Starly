using Starly.Domain.Interfaces.Repositories;

namespace Review.Domain.Interfaces.Repositories;

public interface IReviewRepository : IBaseRepository<Entities.Review, int>
{
}
