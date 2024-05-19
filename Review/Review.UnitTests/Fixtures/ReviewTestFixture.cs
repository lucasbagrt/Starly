using Review.Domain.Dtos;

namespace Review.UnitTests.Fixtures;

public class ReviewTestFixture
{
    public ReviewTestFixture()
    {
    }

    public List<Domain.Entities.Review> GetReviewList() =>
        new()
        {
           GetReview()
        };

    public Domain.Entities.Review GetReview() =>
         new()
         {
             Id = 1,
             BusinessId = 1,
             Comment = "Comentario teste",
             CreatedAt = DateTime.Now,
             Rating = 5,
             UserId = 1,
         };

    public Domain.Entities.Review GetReviewWithoutId() =>
      new()
      {          
          BusinessId = 1,
          Comment = "Comentario teste",
          CreatedAt = DateTime.Now,
          Rating = 5,
          UserId = 1,
      };

    public ReviewDto GetReviewDto() =>
        new()
        {
            Id = 1,
            BusinessId = 1,
            Comment = "Comentario teste",
            CreatedAt = DateTime.Now,
            Rating = 5,
            UserId = 1,
        };

    public List<ReviewDto> GetReviewListDto() =>
        new()
        {
            GetReviewDto()
        };

    public CreateReviewDto GetCreateReviewDto() =>
        new()
        {
            BusinessId = 1,
            Comment = "Comentario teste",
            Rating = 5
        };
}
