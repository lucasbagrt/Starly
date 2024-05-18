namespace Businesses.Domain.Dtos;

public class BusinessDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public BusinessLocationDto Location { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsOpenNow { get; set; }
    public string Photo { get; set; }
    public long ReviewCount { get; set; }
    public double AverageRating { get; set; }
    public List<BusinessCategoryDto> Categories { get; set; }    
}
