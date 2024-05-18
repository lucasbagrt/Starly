using Review.Domain.Entities;
using Review.Domain.Interfaces.Repositories;
using Starly.Infra.Data.Repositories;

namespace Review.Infra.Data.Repositories;

public class ReviewPhotoRepository(ApplicationDbContext context) : BaseRepository<ReviewPhoto, int, ApplicationDbContext>(context), IReviewPhotoRepository
{

}
