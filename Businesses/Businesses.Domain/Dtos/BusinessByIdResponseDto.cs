using Businesses.Domain.Entities;

namespace Businesses.Domain.Dtos;

public class BusinessByIdResponseDto
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Location { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsOpenNow { get; set; }
    public List<BusinessPhoto> Photos { get; set; }
    public List<CategoryDto> Categories { get; set; }
    public List<BusinessHour> Hours { get; set; }
}
