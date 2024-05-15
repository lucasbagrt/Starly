namespace Businesses.Domain.Dtos;

public class BusinessCategoryDto
{
    public int BusinessId { get; set; }
    public int CategoryId { get; set; }
    public CategoryDto Category { get; set; }
}
