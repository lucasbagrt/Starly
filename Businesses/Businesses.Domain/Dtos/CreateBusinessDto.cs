namespace Businesses.Domain.Dtos;

public class CreateBusinessDto
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public BusinessLocationDto Location { get; set; }
    public List<CreateBusinessCategoryDto> Categories { get; set; }
    public List<CreateBusinessHourDto> Hours { get; set; }    
}
