using Starly.Domain.Dtos;

namespace Businesses.Domain.Dtos;

public class ReviewDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BusinessId { get; set; }
    public string Comment { get; set; }
    public short Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<ReviewPhotoDto> Photos { get; set; }
    public UserInfoDto User { get; set; }
}
