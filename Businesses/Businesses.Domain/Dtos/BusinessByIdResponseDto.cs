using Businesses.Domain.Entities;

namespace Businesses.Domain.Dtos;

public class BusinessByIdResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public BusinessLocationDto Location { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsOpenNow { get; set; }
    public long ReviewCount { get; set; }
    public double AverageRating { get; set; }
    public List<ReviewDto> Reviews { get; set; }
    public List<BusinessPhoto> Photos { get; set; }
    public List<BusinessCategoryDto> Categories { get; set; }
    public List<BusinessHour> Hours { get; set; }
}
